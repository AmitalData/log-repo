using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.UnifreightGateway
{
    public class GatewayService
    {

       

        public static string GetExecutablePath()
        {
            var dir = "";// Path.GetDirectoryName(System.ServiceModel.Diagnostics.Application.ExecutablePath);
            Debug.WriteLine("Path.GetDirectoryName(Application.ExecutablePath)=" + dir, false);
            return dir;
        }
    }
}
