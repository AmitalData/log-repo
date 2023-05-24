using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.BL
{
    public class PrivacyProtection
    {
        public void SendPRIVEventPrivacyProtectionMethod(int tenant, string declarationId,string CustomFileNo)
        {
            CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(tenant);
            var courierMasterPM = courierMasterQueryService.GetByDeclarationId(declarationId, tenant);
            var repository = new CardRepository(tenant);
            if (courierMasterPM != null)
            {
                var myCard = repository.GetSingleCard(courierMasterPM.IntegratorCode, tenant);
                if (myCard != null && !String.IsNullOrWhiteSpace(myCard.Code))
                {
                    DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(tenant);

                    string defValue = defaultValueQueryService.GetDefault("ISRAEL", "CGO_GDPR_PRIVAC", "NON", myCard.Code, tenant);
                    if (defValue == "Y")
                    {
                        this.SendPRIV(tenant,"", CustomFileNo);
                    }
                }
            }
        }
      
        public void SendPRIV(int Tenant, string remarks, string UnifreightLeadingFile)
        {
            string loggedContactId = null;
            ContactRepository contactRepository = new ContactRepository(Tenant);
            var loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(Tenant), Tenant);
            if (loggedContact != null)
            {
                loggedContactId = loggedContact.Id;
            }

            string unifrieghtEvent = "PRIV";
            string eventRemarks = remarks;
            var MyUnifreightEventParam = new UnifreightEventParam()
            {
                Code = unifrieghtEvent,
                Mode = UnifreightEventMode.@new,
                EventDateTime = DateTime.Now,
                Entname = "CFIFILEM",
                PrimaryNum = UnifreightLeadingFile,
                EventRemarks = eventRemarks,
            };
            LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
            var myOpenUnifreighTask = new UnifreightEventTaskService();
            myOpenUnifreighTask.UpsertEventLE2U(
                Tenant,
                loggedContactId,
                MyUnifreightEventParam);
        }
    }
}
