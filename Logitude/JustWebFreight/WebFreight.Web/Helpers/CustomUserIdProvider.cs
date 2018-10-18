using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Server.Tools
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            string userName = request.QueryString["UserName"];
            //var userId = HttpContext.Current.User.Identity.Name;
            return userName;
        }
    }
}
