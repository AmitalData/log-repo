using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest
{
    public class TestEnvironmentGlobalParameters
    {
        public static string Token1 { get; set; }
        public static string Token2 { get; set; }
        public static int Tenant1
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Tenant1")); }
        }
        public static int Tenant2
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Tenant2")); }
        }
        public static string ServerURL
        {
            get { return System.Configuration.ConfigurationManager.AppSettings.Get("ServerURL"); }
        }
        public static string APICredential_PrimaryKey1
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("APICredential_PrimaryKey1");
            }
        }
        public static string APICredential_SecondaryKey1
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("APICredential_SecondaryKey1");
              
            }
        }
        public static string APICredential_PrimaryKey2
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("APICredential_PrimaryKey2");
            }
        }
        public static string APICredential_SecondaryKey2
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("APICredential_SecondaryKey2");

            }
        }
    }
}
