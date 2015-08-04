using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Chat.Hubs.Chat
{
    public class Chat : Hub
    {
        public void Join(string name)
        {
            Clients.Caller.Id = new Guid();
            Clients.Caller.Name = name;
        }

        public void Send(string message)
        {
            Clients.All.addNewMessageToPage(Clients.Caller.Name, message);
        }
    }
}