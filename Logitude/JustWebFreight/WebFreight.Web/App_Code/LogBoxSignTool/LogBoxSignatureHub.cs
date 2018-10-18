using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebFreight.Web.App_Code.LogBoxSignTool
{
    public class LogBoxSignatureHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
        //public void Send(string name, string message)
        //{
        //    // Call the broadcastMessage method to update clients.
        //    Clients.All.broadcastMessage(name, message);
        //}


        public void Send(string name)
        {
            //Clients.All.addMessage(name, message);
            Clients.All.NewSignRequestReceived(name);
        }
        public void SignRequestReceived(string clientId)
        {
            Clients.User(clientId).NewSignRequestReceived(clientId);
            //Clients.All.addMessage(name, message);
        }
        public override Task OnConnected()
        {
            // Program.MainForm.WriteToConsole("Client connected: " + Context.ConnectionId);
            //Connections.Add(Context.User.Identity.Name, Context.ConnectionId);
            Clients.All.Connected();
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            //Program.MainForm.WriteToConsole("Client disconnected: " + Context.ConnectionId);
            Clients.All.Disconnected();
            return base.OnDisconnected(stopCalled);
        }

    }
}