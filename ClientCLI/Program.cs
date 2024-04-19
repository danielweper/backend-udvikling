// See https://aka.ms/new-console-template for more information
using ClientCLI;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using ClientLogic;
using Turnbased_Game.Models.Packets.Transport;
class Program
{
    static void Main(string[] args)
    {

        CliTransporter transporter = new CliTransporter();
        Client client = new Client(transporter);
        while (true)
        {
            Console.WriteLine("Welcome to TurnBased-Game CLI \n\n");

            ShowOption(Command.DisplayLobbies);
            ShowOption(Command.IsReady);

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Escape)
            {
                break;
            }

            switch((Command)key.KeyChar)
            {
                case Command.DisplayLobbies:
                    client.ListAvailableLobbies();
                    Console.WriteLine(transporter.hasSent);
                    break;
                default:
                    Console.WriteLine("Available commands are: 'd', ");
                    break;
            }
        }
        Console.WriteLine("Exiting");
    }
  
    static void ShowOption(Command option)
    {
        Console.WriteLine($"Press '{(char)option}' to {option}");
    }
}
