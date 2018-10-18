using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebFreight.Web.CustomWebServices
{
    public class SignatureHub : Hub
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


        public void Send(string name, string message)
        {
            //Clients.All.addMessage(name, message);
            Clients.All.Message4U(name, message);
        }
        public void Uploded(string clientId, int tenant, string FileName)
        {
            Clients.User(clientId).Uploded(tenant, FileName);
            //Clients.All.addMessage(name, message);
        }
        public override Task OnConnected()
        {
            // Program.MainForm.WriteToConsole("Client connected: " + Context.ConnectionId);
            return base.OnConnected();
        }
        //public override Task OnDisconnected()
        //{
        //    //Program.MainForm.WriteToConsole("Client disconnected: " + Context.ConnectionId);
        //    return base.OnDisconnected();
        //}

    }
}