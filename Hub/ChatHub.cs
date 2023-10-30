using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendCoinPosition(float x, float y,float z,string sender)
        {
            await Clients.Others.SendAsync("ReceiveCoinPosition", x, y,z,sender);
        }
    }
}