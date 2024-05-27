using System.Data.SqlTypes;
using System.Drawing;
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
            //PrintWithColor($"[OUTGOING] {packet}", ConsoleColor.DarkYellow);
        };
        transporter.PacketReceived += (IPacket packet) =>
        {
            //PrintWithColor($"[INCOMING] {packet}", ConsoleColor.Yellow);
            if (packet.Type == PacketType.InvalidRequest)
            {
                //PrintWithColor($"{((InvalidRequestPacket)packet).ErrorMessage}", ConsoleColor.Red);
            }
        };

        client.ReceivedMessage += (string senderName, string content) =>
        {
            PrintWithColor($"[{senderName}] {content}", ConsoleColor.Cyan);
        };

        client.JoinedLobby += (string info) =>
        {
            PrintWithColor($"Joined Lobby! ({info})", ConsoleColor.Green);
        };
        client.ListingLobbies += (string info) =>
        {
            PrintWithColor($"Listing available lobbies:\n{info}", ConsoleColor.DarkYellow);
        };

        client.PlayerLeft += (string leavingPlayer) =>
        {
            PrintWithColor($"{leavingPlayer} has left the lobby", ConsoleColor.Magenta);
        };

        client.LeftLobby += (string leavingPlayer) =>
        {
            PrintWithColor("You have left the lobby ", ConsoleColor.DarkBlue);
        };

        client.GameStarting += (DateTime startingTime) =>
        {
            PrintWithColor("Game is starting now");
        };

        client.BadRequest += () =>
        {
            PrintWithColor("Request denied", ConsoleColor.DarkRed);
        };

        client.TurnIsOver += (string turn) =>
        {
            PrintWithColor("A turn has been executed, ready for next action", ConsoleColor.Green);
        };
        client.BattleIsOver += (bool b) =>
        {
            PrintWithColor("The battle is over", ConsoleColor.DarkGreen);
        };

        client.Name = ChooseName(null)!;
        Console.WriteLine($"Name: {client.Name}");

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

    private string ChooseName(string? currentName)
    {
        string? chosenName = null;
        while (string.IsNullOrWhiteSpace(chosenName) || chosenName.Contains(','))
        {
            Console.WriteLine("Enter your name:");
            chosenName = Console.ReadLine();
            Console.WriteLine();
            if (chosenName == null && currentName != null)
                break;
        }

        return (chosenName ?? currentName)!;
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
            Console.WriteLine(
                $"Command can not be used at this time (press '{(char)Command.ShowHelp}' to show usable commands)");
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
                client.JoinLobby(lobbyId);
                break;
            case Command.CreateLobby:
                client.CreateLobby();
                break;
            case Command.DisconnectLobby:
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
                string newName = ChooseName(client.Name);
                client.Name = newName;
                name = Console.ReadLine() ?? name;
                break;
            case Command.KickPlayer:
                Console.WriteLine("Which player do you wish to kick:");
                var kickPlayer = Console.ReadLine();
                if (kickPlayer == null)
                {
                    Console.WriteLine("Aborting...");
                    break;
                }

                Console.WriteLine("Why do you wish to kick:");
                var reason = Console.ReadLine();
                client.KickPlayer(kickPlayer, reason ?? "No reason");
                break;
            case Command.SubmitTurn:
                char turn = ' ';
                while (true)
                {
                    Console.WriteLine("Press 'A' to attack, 'D' to defend and 'H' to heal");
                    turn = char.ToUpper(Console.ReadKey().KeyChar);
                    switch (turn)
                    {
                        case 'A':
                        case 'D':
                        case 'H':
                        case 'F':
                            break;
                        default:
                            continue;
                    }
                    break;
                }
                client.SubmitTurn(turn);
                break;
            default:
                Console.WriteLine($"Command '{command}' is not yet implemented");
                break;
        }
    }

    public static void PrintWithColor(string output, ConsoleColor? color = null)
    {
        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = color ?? prevColor;
        Console.WriteLine(output);
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}