using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Core.Login
{
    public class IntegrationTestLoginParameters
    {
        public static string Token { get; set; }
        public static int Tenant
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Tenant")); }
        }

        public static string ServerURL
        {
            get { return System.Configuration.ConfigurationManager.AppSettings.Get("ServerURL"); }
        }

        public static string APICredential_PrimaryKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("APICredential_PrimaryKey");
            }
        }

        public static string APICredential_SecondaryKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("APICredential_SecondaryKey");

            }
        }
    }
}
