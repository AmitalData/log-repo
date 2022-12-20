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

        public const string HSMActiveCertificates_URL = @"https://customs.amital.co.il/api/SignHSMGetActiveCertificates";
        public const string HSMAzureToken = @"9edYig7zg_b2mBV-72DaOKVMlqtJp-xovFY0k5uBNSRtAzFuY2xcGA==";
        public const string HSMsignprocess = "MehesExport";

        public const string TENATcompanyid = "101";
        public const string TENATtoken = "c6f85591-6e4e-4203-95ef-628b826577b8";

        public bool IsHSMSign_IsOn(int tenant)
        {
            bool fromEnvSetting = !string.IsNullOrEmpty(HSMActiveCertificates_URL) &&
                !string.IsNullOrEmpty(HSMAzureToken) &&
                !string.IsNullOrEmpty(HSMsignprocess)

                ;
            bool fromTenantSetting =
                !string.IsNullOrEmpty(TENATcompanyid) &&
                !string.IsNullOrEmpty(TENATtoken);
            return fromEnvSetting && fromTenantSetting;
        }

        string GetAvailableSignServer(int tenant, SignQueueByType SignatureBy, string personId, string customsAgentId)
        {
            int LastAccessedInMin = -1000000000;//HSM aleays on !!

            SignStation availableSignServer = null;// GetSignStation();
            const string TSTPersonId = "308623615";
            var TST_SignStation = new SignStation()
            {
                SignCertificate = $"C=IL, T=Manager, OU=Peltransport Ltd, O=05-{customsAgentId}, SERIALNUMBER=01-{TSTPersonId}, G=David, SN=Michaeli, CN=David Michaeli ID_{TSTPersonId},MachineName=DAVID-M-PC,UserName=davidm,VER=1.23,STS=OK",

                CustomsAgentId = customsAgentId,
                PersonId = TSTPersonId,
                LastAccessedAt = DateTime.Now
            };

            switch (SignatureBy)
            {

                case SignQueueByType.SignQueueByCustomsAgentId:


                    availableSignServer = TST_SignStation;//repo.GetAvailableSignServerByCustomsAgentId(customsAgentId, LastAccessedInMin);


                    break;
                case SignQueueByType.SignQueueByPersonId:
                    if (String.IsNullOrWhiteSpace(personId))
                    {
                        return null;
                    }

                    availableSignServer = //repo.GetSingle(customsAgentId, personId);
                        TST_SignStation;
                    if (availableSignServer == null)
                    {
                        return null;
                    }
                    if (!availableSignServer.IsPersonalSignOn)
                    {
                        return null;
                    }
                    if (DateTime.Now.Subtract(availableSignServer.LastAccessedAt) > TimeSpan.FromMinutes(LastAccessedInMin))
                    {
                        return null;
                    }
                    break;
                default:
                    throw new Exception("GetAvailableSignServer() while SignatureBy Not P/C");
                    break;
            }

            if (availableSignServer == null)
            {
                return null;
            }
            return availableSignServer.SignCertificate;
        }

        public List<SignStation> GetHSMAllCertificates(int tenant)
        {

            var customsEnvironmentSettingQueryService = new CustomsEnvironmentSettingQueryService(tenant);
            var environmentSettingPM =customsEnvironmentSettingQueryService.GetEnvironmentSettingPM();
            CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
            var setting = settingService.GetSettingByTenantN(tenant);


            var hSMActiveSignCardService = new HSMActiveSignCardService();
            var res = hSMActiveSignCardService.GetActiveCertificates(
                tenant,
                HSMActiveCertificates_URL,
                HSMAzureToken,
      new HSMActiveSignCardParams()
      {
          companyid = "101",
          token = "c6f85591-6e4e-4203-95ef-628b826577b8",
          signprocess = HSMsignprocess
      },
      true
    );
            if (res == null)
            {
                return new List<SignStation>();
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
            if (DateTime.Now> new DateTime(2022,12,30))
            {
                var badCert= res.FirstOrDefault(r => r.companyBN != setting.CustomsAgentId);
                if (badCert!=null)
                {
                    throw new Exception($"GetHSMAllCertificates- found  r.companyBN ({badCert.companyBN}) != setting.CustomsAgentId {setting.CustomsAgentId}");
                }
            }
            

            var list = res.Select(r =>
            new SignStation()
            {
                CustomsAgentId = r.companyBN,
                LastAccessedAt = DateTime.Now,
                MachineName = "HSM",
                PersonId = r.id,
                IsCompanySignOn = r?.companyPersonal.Contains("C") == true,
                IsPersonalSignOn = r?.companyPersonal.Contains("P") == true,
                SignCertificate = $"C=IL, T=Manager, OU=XXXXX Ltd, O=05-{r.companyBN}, SERIALNUMBER=01-{r.id}, G={r.userName}, SN=NAME{r.id}, CN=FN{r.userName} ID_{r.id},MachineName=HSM,UserName={r.userName},VER=1.HSM,STS=OK",
                IsPersonalDefault = r?.companyPersonal.Contains("D") == true,
                SignMethodByQueue = SignMethodByQueueEnum.HSMSignQueue.ToString(),


            }).ToList(); ;
            return list;
        }




    }
}
