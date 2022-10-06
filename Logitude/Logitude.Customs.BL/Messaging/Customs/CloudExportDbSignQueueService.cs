using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.Server.Tools.ExternalServices;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs
{
    public class CloudExportDbSignQueueService
    {

        const int LastAccessedInMin = 5;
        public void UpsertSignStation(string currentSignCertificate, bool isPersonalSignOn, bool isCompanySignOn)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(currentSignCertificate)) { return; }

                string companyTenant = SignQueue.GetCompanyTenant(currentSignCertificate) ?? "0";
                //var tenantListOfPersonID = SignQueue.GetTenantListOfPersonID(SignCertificateClass.Get(currentSignCertificate).PersonId, 0);
                int tenant = 0;
                int.TryParse(companyTenant, out tenant);

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

        public List<SignStationList> GetAllStation(string searchfields, int tenant)
        {
            var repo = new SignStationRepository(tenant);
            string customsAgentId = SignQueue.GetCustomsAgentIdFromTenant(tenant);
            var res = repo.GetAllAvailable(customsAgentId, LastAccessedInMin);

            var entityLists = new List<SignStationList>();
            res.ForEach(r =>
            {
                var MySignCertificateClass = SignCertificateClass.Get(r.SignCertificate);
                var my = new SignStationList()
                {
                    PersonId = MySignCertificateClass.PersonId,
                    SignerName = MySignCertificateClass.SignerName,

                    MachineName = MySignCertificateClass.MachineName,
                    MachineUser = MySignCertificateClass.UserName,

                    CustomsAgentId = MySignCertificateClass.CustomsAgentId,


                    IsCompanySignOn = r.IsCompanySignOn,
                    IsPersonalSignOn = r.IsPersonalSignOn,
                    VersionByFeatures = r.VersionByFeatures,
                    Status = r.Status,
                    LastSignAt = r.LastAccessedAt,
                    IsOk = r.Status.Equals("ok")


                };

                entityLists.Add(my);

            }
            );
            return entityLists;
        }
        public string GetAvailableSignServer(int tenant, SignQueueByType SignatureBy, string personId)
        {

            SignStation availableSignServer = null;
            var repo = new SignStationRepository(tenant);
            string customsAgentId = SignQueue.GetCustomsAgentIdFromTenant(tenant);

            switch (SignatureBy)
            {

                case SignQueueByType.SignQueueByCustomsAgentId:


                    availableSignServer = repo.GetAvailableSignServerByCustomsAgentId(customsAgentId, LastAccessedInMin);


                    break;
                case SignQueueByType.SignQueueByPersonId:
                    if (String.IsNullOrWhiteSpace(personId))
                    {
                        return null;
                    }

                    availableSignServer = repo.GetSingle(customsAgentId, personId);
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

        public static bool IsCloudExport(int tenant)
        {
            return (!CustomsSettingQueryService.GetSettingByTenant(tenant).IsConnectedToUniFreight);
        }
    }
    public class SignStationList
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
    }
}