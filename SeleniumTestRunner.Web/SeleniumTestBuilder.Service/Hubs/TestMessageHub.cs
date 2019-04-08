using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNet.SignalR;

namespace SeleniumTestBuilder.Service.Hubs
{
    public class TestMessageHub:Hub
    {
        public void BroadCastMessage(string message)
        {
            Clients.All.addMessage(message);
        }
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }
    }
}
