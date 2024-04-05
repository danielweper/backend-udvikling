using System.Runtime.InteropServices;
using Microsoft.AspNetCore.SignalR;

namespace Turnbased_Game.Hubs
{
    public class ChatHub : Hub
    {
        public Task SendMessage(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public Task ClickButton(string user, string color)
        {
            return Clients.All.SendAsync("ReceiveButton", user, color);
        }
        public Task SendClass(Person person)
        {
            return Clients.All.SendAsync("ReceiveClass", person);
        }
    }
}