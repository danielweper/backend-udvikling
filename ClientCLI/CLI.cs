using ClientLogic;
using Core.Packets;
using Core.Packets.Shared;

namespace ClientCLI;

class CLI
{
    public void Run()
    {
        CliTransporter transporter = new CliTransporter();
        Client client = new Client(transporter);

        transporter.PacketSent += (IPacket packet) =>
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"[OUTGOING] {packet}");
            Console.ForegroundColor = prevColor;
        };
        transporter.PacketReceived += (IPacket packet) =>
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[INCOMING] {packet}");
            if (packet.Type == PacketType.InvalidRequest)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{((InvalidRequestPacket)packet).ErrorMessage}");
            }
            Console.ForegroundColor = prevColor;
        };

        client.ReceivedUserMessage += (byte senderId, string content) =>
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[{senderId}] {content}");
            Console.ForegroundColor = prevColor;
        };

        client.JoinedLobby += (string info) =>
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Joined Lobby! ({info})");
            Console.ForegroundColor = prevColor;
        };
        client.ListingLobbies += (string info) =>
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Listing available lobbies:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(info);
            Console.ForegroundColor = prevColor;
        };

        while (true)
        {
            Console.WriteLine($"Current state: {client.CurrentState}");
            Console.Write("Enter command: ");
            var key = Console.ReadKey(false);
            Console.WriteLine();
            if (key.Key == ConsoleKey.Escape)
            {
                break;
            }
            HandleCommand((Command)key.KeyChar, client);
        }
    }

    private void HandleCommand(Command command, Client client)
    {
        List<Command> acceptableCommands = [Command.ShowHelp];

        ClientStates state = client.CurrentState;
        if (state.HasFlag(ClientStates.IsConnected))
        {
            if (state.HasFlag(ClientStates.IsInLobby))
            {
                acceptableCommands.Add(Command.DisconnectLobby);
                acceptableCommands.Add(Command.IsReady);
                acceptableCommands.Add(Command.IsNotReady);

                if (state.HasFlag(ClientStates.IsHost))
                {
                    acceptableCommands.Add(Command.KickPlayer);
                    acceptableCommands.Add(Command.StartGame);
                    acceptableCommands.Add(Command.ChangeGameSettings);
                }

                acceptableCommands.Add(Command.RequestPlayerUpdate);
                acceptableCommands.Add(Command.RequestRoleChange);
                acceptableCommands.Add(Command.SendMessage);

                if (state.HasFlag(ClientStates.IsFighter) && state.HasFlag(ClientStates.IsInGame))
                {
                    acceptableCommands.Add(Command.SubmitTurn);
                }
            }
            else
            {
                acceptableCommands.Add(Command.DisplayLobbies);
                acceptableCommands.Add(Command.JoinLobby);
                acceptableCommands.Add(Command.CreateLobby);
            }
        }

        if (acceptableCommands.Contains(command)) {
            switch (command)
            {
                case Command.ShowHelp:
                    Console.WriteLine("Usable commands:");
                    foreach (Command acceptable in acceptableCommands)
                    {
                        Console.WriteLine($"Press '{(char)acceptable}' to {acceptable}");
                    }
                    break;
                case Command.JoinLobby:
                    Console.WriteLine("Enter Lobby id");
                    // TODO: take user input
                    byte lobbyId = Convert.ToByte(Console.ReadLine());
                    client.JoinLobby(lobbyId);
                    break;
                case Command.CreateLobby:
                    Console.WriteLine("Creating Lobby");
                    client.CreateLobby();
                    break;
                case Command.DisconnectLobby:
                    Console.WriteLine("Leaving Lobby");
                    client.DisconnectLobby();
                    break;
                case Command.IsReady:
                    //Console.WriteLine("READY");
                    client.IsReady();
                    break;
                case Command.IsNotReady:
                    //Console.WriteLine("Not Ready");
                    client.IsNotReady();
                    break;
                case Command.DisplayLobbies:
                    Console.WriteLine("Listing available lobbies:");
                    client.ListAvailableLobbies();
                    break;
                case Command.SendMessage:
                    Console.WriteLine("Enter A Message");
                
                    var message = Console.ReadLine();
                    if (message != null) client.SendMessage(message);
                    break;
                case Command.StartGame:
                    client.StartGame();
                    break;
                default:
                    Console.WriteLine($"Command '{command}' is not yet implemented");
                    break;
            }
        }
        else
        {
            Console.WriteLine($"Command can not be used at this time (press '{(char)Command.ShowHelp}' to show usable commands)");
        }
    }

    private void HandleConnectedState(Command command, Client client)
    {
        switch (command)
        {
            case Command.DisplayLobbies:
                Console.WriteLine("Displaying Lobbies...");
                client.ListAvailableLobbies();
                break;
            case Command.JoinLobby:
                Console.WriteLine("Joining lobby...");
                client.JoinLobby(1);
                
                break;
            case Command.CreateLobby:
                Console.WriteLine("Creating lobby...");
                break;
            default:
                Console.WriteLine("Invalid command");
                break;
        }
    }

    private void HandleLobbyState(Command command, Client client)
    {
        if (client.IsHost)
        {
            HandleHostLobbyState(command, client);
        }
        else
        {
            HandleFighterLobbyState(command, client);
        }
    }
    private void HandleHostLobbyState(Command command, Client client)
    {
        switch (command)
        {
            case Command.KickPlayer:
                Console.WriteLine("Kicking player...");
                break;
            case Command.StartGame:
                Console.WriteLine("Starting game...");
                break;
            case Command.ChangeGameSettings:
                Console.WriteLine("Changing game settings...");
                break;
            default:
                Console.WriteLine("Invalid command");
                break;
        }
    }

    private void HandleFighterLobbyState(Command command, Client client)
    {
        switch (command)
        {
            case Command.DisconnectLobby:
                Console.WriteLine("Disconnecting lobby...");
                break;
            case Command.SendMessage:
                Console.WriteLine("Enter A Message");
                
                var message = Console.ReadLine();
                if (message != null) client.SendMessage(message);
                
                break; 
            case Command.RequestRoleChange:
                Console.WriteLine("Requesting role change...");
                break;
            case Command.RequestPlayerUpdate:
                Console.WriteLine("Requesting player update...");
                break;
            case Command.IsReady:
                Console.WriteLine("Setting ready...");
                break;
            case Command.IsNotReady:
                Console.WriteLine("Setting not ready...");
                break;
        } 
    }

    private void HandleFightState(Command command, Client client)
    {
        switch (command)
        {
            case Command.SubmitTurn:
                Console.WriteLine("Submitting turn...");
                break;
            case Command.SendMessage:
                Console.WriteLine("Sending message...");
                break;
            default:
                Console.WriteLine("Invalid command");
                break;
        }
    }
}