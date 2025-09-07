using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.ExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs.SignQueueBL
{
    public class SignQueueHSMService
    {

        //public const string HSMActiveCertificates_URL = @"https://customs.amital.co.il/api/SignHSMGetActiveCertificates";
        //public const string HSMAzureToken = @"9edYig7zg_b2mBV-72DaOKVMlqtJp-xovFY0k5uBNSRtAzFuY2xcGA==";
        //public const string HSMsignprocess = "MehesExport";

        //public const string TENATcompanyid = "101";
        //public const string TENATtoken = "c6f85591-6e4e-4203-95ef-628b826577b8";

        public bool IsHSMSign_IsOn(int tenant)
        {
            CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
            return settingService.IsHSMSign_IsOn(tenant);
        }

        

        public List<MySignStationList> GetHSMAllCertificates(int tenant,bool fromCache=true)
        {

            var customsEnvironmentSettingQueryService = new CustomsEnvironmentSettingQueryService(tenant);
            var environmentSettingPM =customsEnvironmentSettingQueryService.GetEnvironmentSettingPM(tenant);
            var settingService = new CustomsSettingQueryService(tenant);
            var tenantSetting = settingService.GetSettingByTenantN(tenant);


            var hSMActiveSignCardService = new HSMActiveSignCardService();
            var res = hSMActiveSignCardService.GetActiveCertificates(
                tenant,
                environmentSettingPM.HSMActiveCertUrl,//@"https://customs.amital.co.il/api/SignHSMGetActiveCertificates";
                environmentSettingPM.HSMToken,//@@"9edYig7zg_b2mBV-72DaOKVMlqtJp-xovFY0k5uBNSRtAzFuY2xcGA==";
      new HSMActiveSignCardParams()
      {
          companyid = tenantSetting.HSMCompanyId,// "101",
          token = tenantSetting.HSMToken,// "c6f85591-6e4e-4203-95ef-628b826577b8",
          signprocess = environmentSettingPM.HSMSignProcess,
          companyBN = tenantSetting.CustomsAgentId, //"550221105"

      },
       fromCache
    );
            if (res == null)
            {
                return new List<MySignStationList>();
            }
            /*
             *  {
            "companyID": "101",
            "id": "038611216",
            "userName": null,
            "companyBN": "550221105",
            "companyPersonal": "PCD"
        }
             */
            if (tenantSetting.HSMCompanyId != "9999" && DateTime.Now> new DateTime(2023,01,14))
            {
                var badCert= res.FirstOrDefault(r => r.companyBN != tenantSetting.CustomsAgentId);
                if (badCert!=null)
                {
                    throw new Exception($"GetHSMAllCertificates- found  r.companyBN ({badCert.companyBN}) != setting.CustomsAgentId {tenantSetting.CustomsAgentId}");
                }
            }


            var list = res.Select(r =>
            new MySignStationList()
            {
                CustomsAgentId = r.companyBN,
                LastAccessedAt = DateTime.Now,
                MachineName = "HSM",
                SignerName = r.userName,
                LastSignAt = DateTime.Now,
                IsOk = true,
                VersionByFeatures = r?.companyPersonal.Contains("D")==true ? "PersonalDefault" /*"Joker"*/ : "HSM" ,
                MachineUser = "HSM",
                Status = "OK",
                PersonId = r.id,

                IsCompanySignOn = r?.companyPersonal.Contains("C") == true,
                IsPersonalSignOn = r?.companyPersonal.Contains("P") == true,
                SignCertificate = $"C=IL, T=Manager, OU=XXXXX Ltd, O=05-{r.companyBN}, SERIALNUMBER=01-{r.id}, G={r.userName}, SN=NAME{r.id}, CN=FN{r.userName} ID_{r.id},MachineName=HSM,UserName={r.userName},VER=1.HSM,STS=OK",
                IsPersonalDefault = r?.companyPersonal.Contains("D") == true,
                SignMethodByQueue = SignMethodByQueueEnum.HSMSignQueue.ToString(),


            }).ToList();
            return list;
        }




    }
}
