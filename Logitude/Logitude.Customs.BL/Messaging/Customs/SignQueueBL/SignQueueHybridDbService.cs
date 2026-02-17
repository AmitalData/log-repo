using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.Server.Tools.ExternalServices;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.EntityQueries;

namespace Logitude.Customs.BL.Messaging.Customs.SignQueueBL
{
    public class SignQueueHybridDbService
    {

        const int LastAccessedInMin = 5;
        public void UpsertSignStation(string currentSignCertificate, bool isPersonalSignOn, bool isCompanySignOn, int tenant)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(currentSignCertificate)) { return; }

                //string companyTenant = SignQueue.GetCompanyTenant(currentSignCertificate) ?? "0";
                //var tenantListOfPersonID = SignQueue.GetTenantListOfPersonID(SignCertificateClass.Get(currentSignCertificate).PersonId, 0);
                //int tenant = 0;
                //int.TryParse(companyTenant, out tenant);

                var context = CustomContext.GetContext(tenant);
                var signStationQueryService = new SignStationQueryService(context);
                var signStationUpdateService = new SignStationUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                using (var trans = TransactionFactory.GetTransaction())
                {
                    var pm = signStationQueryService.GetSingle(
                    SignCertificateClass.Get(currentSignCertificate).CustomsAgentId,
                    SignCertificateClass.Get(currentSignCertificate).PersonId,
                    getComposition: false, getFromCache: false);
                    if (pm == null)
                    {
                        pm = new Def.EntityPMs.SignStationPM()
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            CustomsAgentId = SignCertificateClass.Get(currentSignCertificate).CustomsAgentId,
                            PersonId = SignCertificateClass.Get(currentSignCertificate).PersonId,
                        };
                    }
                    else
                    {
                        pm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    }

                    pm.SignCertificate = currentSignCertificate;
                    //pm.CompanyTenant = companyTenant;
                    //pm.TenantListFromPersonID = tenantListOfPersonID;

                    pm.IsPersonalSignOn = isPersonalSignOn;
                    pm.IsCompanySignOn = isCompanySignOn;
                    pm.MachineName = SignCertificateClass.Get(currentSignCertificate).MachineName;
                    pm.UserName = SignCertificateClass.Get(currentSignCertificate).UserName;
                    pm.VersionByFeatures = SignCertificateClass.Get(currentSignCertificate).SignServerVersionByFeature;
                    pm.Status = SignCertificateClass.Get(currentSignCertificate).SignServerStatus;
                    pm.LastAccessedAt = DateTime.Now;
                    pm.Tenant = tenant;
                    signStationUpdateService.Update(pm, true);
                    trans.Complete();
                }

            }
            catch (Exception ex)
            {

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "DbSignQueueService", "SignStation", null);
                ///throw;
            }
        }

        public List<MySignStationList> GetAllStation(string searchfields, int tenant)
        {
            var repo = new SignStationRepository(tenant);
            string customsAgentId = SignQueue.GetCustomsAgentIdFromTenant(tenant);
            var res = repo.GetAllAvailable(tenant,customsAgentId, LastAccessedInMin);

            var entityLists = new List<MySignStationList>();
            res.ForEach(r =>
            {
                //var MySignCertificateClass = SignCertificateClass.Get(r.SignCertificate);
                //var my = new MySignStationList()
                //{
                //    PersonId = MySignCertificateClass.PersonId,
                //    SignerName = MySignCertificateClass.SignerName,

                //    MachineName = MySignCertificateClass.MachineName,
                //    MachineUser = MySignCertificateClass.UserName,

                //    CustomsAgentId = MySignCertificateClass.CustomsAgentId,


                //    IsCompanySignOn = r.IsCompanySignOn,
                //    IsPersonalSignOn = r.IsPersonalSignOn,
                //    VersionByFeatures = r.VersionByFeatures,
                //    Status = r.Status,
                //    LastSignAt = r.LastAccessedAt,
                //    IsOk = r.Status.Equals("ok"),
                    


                //};
                var my = r.ToMySignStationList();
                entityLists.Add(my);

            }
            );
            return entityLists;
        }
        public (string signCertificate, SignMethodByQueueEnum dSignMethodByQueue) GetAvailableSignServer(int tenant, SignQueueByType SignatureBy, string personId,bool isCloud = false)
        {
			IGlobalContext myContextCommon = GlobalContext.GetContext();
			FeatureRepository myFeatureRepository = new FeatureRepository(myContextCommon);
			FeatureQuery featureQuery = new FeatureQuery(myFeatureRepository);

			MySignStationList availableSignServer = null;
            var repo = new SignStationRepository(tenant);
            string customsAgentId = SignQueue.GetCustomsAgentIdFromTenant(tenant);

            var hSMAllCertificates = new List<MySignStationList>();
            var hSMSignService = new SignQueueHSMService();
            if (hSMSignService.IsHSMSign_IsOn(tenant))
            {
                hSMAllCertificates = hSMSignService.GetHSMAllCertificates(tenant);
            }
                switch (SignatureBy)
            {

                case SignQueueByType.SignQueueByCustomsAgentId:

                    availableSignServer = hSMAllCertificates.FirstOrDefault(r => r.IsCompanySignOn);
                    if (availableSignServer != null)
                    {
                        return (availableSignServer.SignCertificate, SignMethodByQueueEnum.HSMSignQueue);
                    }
                     
                    availableSignServer = repo.GetAvailableSignServerByCustomsAgentId(tenant,customsAgentId, LastAccessedInMin).ToMySignStationList();


                    break;
                case SignQueueByType.SignQueueByPersonId:

                    var defaultSignServer = hSMAllCertificates.FirstOrDefault(r => r.IsPersonalDefault);
                    if (String.IsNullOrWhiteSpace(personId))
                    {
                        
                        if (defaultSignServer != null)
                        {
                            return (defaultSignServer.SignCertificate, SignMethodByQueueEnum.HSMSignQueue);
                        }
                        return (null, SignMethodByQueueEnum.None);
                    }
                    var hsmSignServer = hSMAllCertificates.FirstOrDefault(r => r.PersonId == personId);
                    if (hsmSignServer != null)
                    {
                        return (hsmSignServer.SignCertificate, SignMethodByQueueEnum.HSMSignQueue);
                    }

                    availableSignServer = repo.GetAllAvailable(tenant, customsAgentId, LastAccessedInMin).Where(a => a.PersonId == personId).FirstOrDefault().ToMySignStationList();

                    if (availableSignServer == null)
                    {
                        if (defaultSignServer != null)
                        {
                            return (defaultSignServer.SignCertificate, SignMethodByQueueEnum.HSMSignQueue);
                        }
                        return (null, SignMethodByQueueEnum.None); ;
                    }
                    if (!availableSignServer.IsPersonalSignOn)
                    {
                        if (defaultSignServer != null)
                        {
                            return (defaultSignServer.SignCertificate, SignMethodByQueueEnum.HSMSignQueue);
                        }
                        return (null, SignMethodByQueueEnum.None); ;
                    }

					
					var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(tenant), tenant);
					var featureIsExportSign = features.Features.FirstOrDefault(x => x.Code == "IsExportSign");
					if (featureIsExportSign != null && isCloud)
					{
						return (availableSignServer.SignCertificate, SignMethodByQueueEnum.HybridDbSignQueue);
					}
					if (DateTime.Now.Subtract(availableSignServer.LastAccessedAt) > TimeSpan.FromMinutes(LastAccessedInMin))
                    {
                        if (defaultSignServer != null)
                        {
                            return (defaultSignServer.SignCertificate, SignMethodByQueueEnum.HSMSignQueue);
                        }
                        return (null, SignMethodByQueueEnum.None); ;
                    }
                    else if(isCloud)
                    {
                        return (availableSignServer.SignCertificate, SignMethodByQueueEnum.HybridDbSignQueue);
                    }
                    else
                    {
                        return (availableSignServer.SignCertificate, SignMethodByQueueEnum.None);

                    }

                    break;
                default:
                    throw new Exception("GetAvailableSignServer() while SignatureBy Not P/C");
                    break;
            }

            if (availableSignServer == null)
            {
                return (null, SignMethodByQueueEnum.None); 
            }
            return (availableSignServer.SignCertificate, SignMethodByQueueEnum.HybridDbSignQueue);
        }

        public static bool IsCloudExport(int tenant, string Direction = null)
        {
            return (!CustomsSettingQueryService.GetSettingByTenant(tenant).IsConnectedToUniFreight && Direction != "I");
        }
        public static bool IsCloud(int tenant)
        {
            return (!CustomsSettingQueryService.GetSettingByTenant(tenant).IsConnectedToUniFreight);
        }
    }
    public class MySignStationList
    {
        public string PersonId { get; set; }
        public string SignerName { get; set; }
        public string CustomsAgentId { get; set; }
        public string MachineName { get; set; }
        public string MachineUser { get; set; }

        public bool IsPersonalSignOn { get; set; }
        public bool IsCompanySignOn { get; set; }
        public string Status { get; set; }
        public DateTime? LastSignAt { get; set; }
        public bool? IsOk { get; set; }
        public string VersionByFeatures { get; set; }
        public bool IsPersonalDefault { get; set; }
        public string SignMethodByQueue { get; set; } //= SignMethodByQueueEnum.None.ToString(),
        public string SignCertificate { get; set; }

        public DateTime LastAccessedAt { get; set; }




    }


    public static class SignStation_Ext
    {
        public static MySignStationList ToMySignStationList(this SignStation signStation
            )
        {
            if (signStation == null) return null; ;

            var MySignCertificateClass = SignCertificateClass.Get(signStation.SignCertificate);

            
            var my = new MySignStationList()
            {
                PersonId = signStation.PersonId,
                SignerName = MySignCertificateClass.SignerName,

                MachineName = signStation.MachineName,
                MachineUser = signStation.UserName,

                CustomsAgentId = signStation.CustomsAgentId,


                IsCompanySignOn = signStation.IsCompanySignOn,
                IsPersonalSignOn = signStation.IsPersonalSignOn,
                VersionByFeatures = signStation.VersionByFeatures,
                Status = signStation.Status,
                LastSignAt = signStation.LastAccessedAt,
                IsOk = signStation.Status?.Equals("ok"),
                SignCertificate = signStation.SignCertificate,
                LastAccessedAt = signStation.LastAccessedAt,
                 



            };
            return my;
            
        }
    }

}