using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;

namespace Logitude.CustomsMessaging.Helpers
{
    public static class UnifreightIIGCommonUtil
    {
        static UnifreightIIGCommonUtil()
        {
            InitializeSettings();//TRY ONLY ONCE TO InitializeSettings ...
        }
        static void InitializeSettings() //can throw exception!!! due developer !!
        {

            int unifreightIIGServiceTimeout = 361;
            int.TryParse(ConfigurationManager.AppSettings["UnifreightIIGServiceTimeout"], out unifreightIIGServiceTimeout);
            string consumerID = ConfigurationManager.AppSettings["ConsumerID"];
            string unifreightIIGServiceAddress = ConfigurationManager.AppSettings["UnifreightIIGServiceAddress"];
            if (Environment.UserDomainName == "NTDOMAIN")
            {
                unifreightIIGServiceAddress = @"http://itzik7:5050/UnifreightIIG/GatewayService/Basic";
            }

            UnifreightIIGCommonSetting.New()
                     .SetConsumerID("038623617")
                     .SetUnifreightIIGServiceAddress(consumerID)

                     .SetUnifreightIIGServiceAddress(@"http://localhost:5050/UnifreightIIG/GatewayService/Basic")
                     .SetUnifreightIIGServiceAddress(@"http://iiggateway.cloudapp.net:5050/UnifreightIIG/GatewayService/Basic")
                     
                     .SetUnifreightIIGServiceAddress(unifreightIIGServiceAddress)
                     
                     .SetUnifreightIIGServiceTimeout(unifreightIIGServiceTimeout)
                     
                     .CreateSetting();

        }
        public static bool ForceInitializeSettings()
        {
            return true;
        }
    }
}
