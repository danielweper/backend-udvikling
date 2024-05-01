using System.Runtime.InteropServices;

namespace ClientCLI;
using ClientLogic;

class CLI
{
    public void Run()
    {
        CliTransporter transporter = new CliTransporter();
        Client client = new Client(transporter);
        var currentState = client.currentState;
        while (true)
        {
            currentState = client.currentState;
            Console.WriteLine($"Current state: {currentState}");
            Console.Write("Enter command: ");
            var key = Console.ReadKey(true);
            HandleCommand((Command)key.KeyChar, client);
        }
    }

    private void HandleCommand(Command command, Client client)
    {
        switch (client.currentState)
        {
            case ClientStates.IsConnected:
                HandleConnectedState(command, client);
                break;
            case ClientStates.IsInLobby:
                HandleLobbyState(command, client);
                break;
            case ClientStates.IsFighter:
                HandleFightState(command, client);
                break;
            default:
                Console.WriteLine("Invalid state");
                break;
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
        if (client.isHost)
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
                Console.WriteLine("Sending message...");
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