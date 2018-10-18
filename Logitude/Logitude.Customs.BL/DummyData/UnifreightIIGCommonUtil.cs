using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.DummyData
{
    private static class UnifreightIIGCommonUtil
    {
        private class TenantSetting
        {

            public int Tenant;
            public string ConsumerID;// = "038623617";
            public string UnifreightIIGServiceAddress;
            public int UnifreightIIGServiceTimeout = 361;


            public string UServerDNS;
            public string UServerPort;
            public int UserverTimeout = 61;

            public string DCAAddress;
            public int DCATimeout = 361;

        }
        private static readonly List<TenantSetting> MyDB;
        static UnifreightIIGCommonUtil()
        {
            MyDB = new List<TenantSetting>();
            if (Environment.UserDomainName.Equals("NTDOMAIN", StringComparison.OrdinalIgnoreCase) ||
                Environment.MachineName.Equals("IIGTest", StringComparison.OrdinalIgnoreCase)
                )
            {
                var customsDeploymentStage = SettingUtil.GetCustomsDeploymentStage();
                switch (customsDeploymentStage)
                {
                    
                    case SettingUtil.CustomsDeploymentStage.Pilot:
                        MyDB.Add(new TenantSetting()
                        {
                            Tenant = 1,
                            ConsumerID = "510120041",//"038623617",
                            UnifreightIIGServiceAddress = "http://10.10.10.149:5050/UnifreightIIG/GatewayServicePilot/Basic",
                            UServerDNS = "UNIV55", //"10.10.10.45", //"dev2008",
                            UServerPort = "8055", // "8053", ///http://10.10.10.45:5050/uniface/services/GWSFINSRVEXE
                            DCAAddress = ""
                        });
                        break;

                    default:
                        MyDB.Add(new TenantSetting()
                        {
                            Tenant = 1,
                            ConsumerID = "510120041",//"038623617",
                            UnifreightIIGServiceAddress = "http://10.10.10.149:5050/UnifreightIIG/GatewayService/Basic",
                            UServerDNS = "UNIV55", //"10.10.10.45", //"dev2008",
                            UServerPort = "8055", // "8053", ///http://10.10.10.45:5050/uniface/services/GWSFINSRVEXE
                            DCAAddress = ""
                        });
                        break;
                }
                
                
                return;
            }

            MyDB.Add(new TenantSetting()
            {
                Tenant = 1,
                ConsumerID = "510120041",
                UnifreightIIGServiceAddress = "http://iiggateway.cloudapp.net:5050/UnifreightIIG/GatewayService/Basic",
                UServerDNS = "UNIV55", //"UNIV54", //"dev2008",
                UServerPort = "8055" , // "8089",//"8053",
                DCAAddress = ""
            });

        }
        private static TenantSetting GetTenantSetting(int? tenant = null)
        {
            if (tenant == null)
            {
                return MyDB.FirstOrDefault();
            }
            return MyDB.FirstOrDefault(rec => rec.Tenant == tenant); ;
        }

     
    }
}
