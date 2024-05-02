// See https://aka.ms/new-console-template for more information
using ClientCLI;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Core.Packets.Transport;

class Program
{
    static void Main(string[] args)
    {
        CLI cli = new CLI();
        cli.Run();
    }
}
