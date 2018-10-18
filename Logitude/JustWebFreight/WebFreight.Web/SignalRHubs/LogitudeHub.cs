using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;

namespace WebFreight.Web.SignalRHubs
{
    public class LogitudeHub : Hub
    {


        public async Task Subscribe(string channel, string token)
        {

            // string token = HttpContext.Current.Request.Headers["Token"];
            //string token = HttpContext.Current.Request.QueryString["token"];
            //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(0);
            AuthenticationToken authToken = authenticationTokenRepository.GetSingleToken(token);
            if (authToken == null)
            {
                throw new AutenticationException("Sorry! this user is not authorized!");
            }

            //SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            await Groups.Add(Context.ConnectionId, channel);
        }

        public async Task Unsubscribe(string channel)
        {
            await Groups.Remove(Context.ConnectionId, channel);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}