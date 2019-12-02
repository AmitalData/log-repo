using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest
{
    public class EnvironmentGlobalParams
    {
        public static string MainToken { get; set; }
        public static string SecondaryToken { get; set; }
        public static int MainTenant
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("MainTenant")); }
        }
        public static int SecondaryTenant
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("SecondaryTenant")); }
        }
        public static string ServerURL
        {
            get { return System.Configuration.ConfigurationManager.AppSettings.Get("ServerURL"); }
        }
        public static string MainTenant_APICredential_PrimaryKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("MainTenant_APICredential_PrimaryKey");
            }
        }
        public static string MainTenant_APICredential_SecondaryKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("MainTenant_APICredential_SecondaryKey");
              
            }
        }
        public static string SecondaryTenant_APICredential_PrimaryKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("SecondaryTenant_APICredential_PrimaryKey");
            }
        }
        public static string SecondaryTenant_APICredential_SecondaryKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("SecondaryTenant_APICredential_SecondaryKey");

            }
        }
    }
}
