using Logitude.BL.CommonDataModel.EntityPMs;
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

        public static string Email
        {
            get { return System.Configuration.ConfigurationManager.AppSettings.Get("Email"); }
        }

        public static string Password
        {
            get { return System.Configuration.ConfigurationManager.AppSettings.Get("Password"); }
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
        public static string LoginUserId { get; set; }

        public static string LoginUserName { get; set; }

        public static TenantPM TenantPM { get; set; }
    }
}
