using ClientLogic;
using Core.Packets;
using Core.Packets.Shared;

namespace ClientCLI;

class CLI
{
    private string? name = null;
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

        client.ReceivedUserMessage += (string senderName, string content) =>
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[{senderName}] {content}");
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

        while (name != null)
        {
            name = Console.ReadLine();
        }
        
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
                acceptableCommands.Add(Command.ListAvailableLobbies);
                acceptableCommands.Add(Command.JoinLobby);
                acceptableCommands.Add(Command.CreateLobby);
                acceptableCommands.Add(Command.ChangeName);
            }
        }

        if (!acceptableCommands.Contains(command))
        {
            Console.WriteLine($"Command can not be used at this time (press '{(char)Command.ShowHelp}' to show usable commands)");
            return;
        }
        
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
                client.JoinLobby(lobbyId, name);
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
            case Command.ListAvailableLobbies:
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
            case Command.ChangeName:
                name = Console.ReadLine() ?? name;
                break;
            default:
                Console.WriteLine($"Command '{command}' is not yet implemented");
                break;
        }
    }
}