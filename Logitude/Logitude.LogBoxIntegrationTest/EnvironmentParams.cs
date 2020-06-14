using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.LogBoxIntegrationTest
{
    public class EnvironmentParams
    {
        public static string LogBoxTenantToken { get; set; }
        public static int LogBoxTenant
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("LogBoxTenant")); }
        }
        public static string LogBoxServerURL
        {
            get { return System.Configuration.ConfigurationManager.AppSettings.Get("LogBoxServerURL"); }
        }
        public static string LogBoxTenant_APICredential_PrimaryKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("LogBoxTenant_APICredential_PrimaryKey");
            }
        }
        public static string LogBoxTenant_APICredential_SecondaryKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("LogBoxTenant_APICredential_SecondaryKey");
              
            }
        }
    }
}
