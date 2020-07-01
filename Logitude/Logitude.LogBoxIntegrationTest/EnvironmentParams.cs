using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.LogboxIntegrationTest
{
    public class EnvironmentParams
    {
        public static string LogboxTenantToken { get; set; }
        public static string CloudTenantToken { get; set; }
        public static int LogboxTenant
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("LogboxTenant")); }
        }
        public static string LogboxServerURL
        {
            get { return System.Configuration.ConfigurationManager.AppSettings.Get("LogboxServerURL"); }
        }
        public static string LogboxTenant_APICredential_PrimaryKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("LogboxTenant_APICredential_PrimaryKey");
            }
        }
        public static string LogboxTenant_APICredential_SecondaryKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("LogboxTenant_APICredential_SecondaryKey");
              
            }
        }

        public static int CloudTenant
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("CloudTenant")); }
        }
        public static string CloudServerURL
        {
            get { return System.Configuration.ConfigurationManager.AppSettings.Get("CloudServerURL"); }
        }
        public static string CloudTenant_APICredential_PrimaryKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("CloudTenant_APICredential_PrimaryKey");
            }
        }
        public static string CloudTenant_APICredential_SecondaryKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("CloudTenant_APICredential_SecondaryKey");

            }
        }
    }
}
