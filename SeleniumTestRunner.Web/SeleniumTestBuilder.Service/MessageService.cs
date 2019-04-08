using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.AspNet.SignalR;
using SeleniumTestBuilder.Service.Hubs;

namespace SeleniumTestBuilder.Service
{
    public class MessageService
    {
        public void SubmitMessage(string m)
        {
            TestMessageHub hub = new TestMessageHub();
            
                var context = GlobalHost.ConnectionManager.GetHubContext<TestMessageHub>();
                context.Clients.All.broadcastMessage("Admin",m);
               
          
           
        }
    }
}
