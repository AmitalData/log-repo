using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationReferantDataUpdateService
    {

        protected override void OnCreating(DeclarationReferantDataPM entityPM, EntityPM entityParentPM)
        {
            if(!string.IsNullOrEmpty(entityPM.DeclarationId)) entityPM.DeclarationIdToDisplay = entityPM.DeclarationId;
            base.OnCreating(entityPM, entityParentPM);
        }


        protected override void OnUpdating(DeclarationReferantDataPM entityPM)
        {
            if( entityPM != null && entityPM.IsCloseOrOpenFromUser)
            {
                if (entityPM.IsClosedForFollowUp == "1")
                    SendUnifreightEventParam(entityPM, "REFFC");
                if (entityPM.IsClosedForFollowUp == "0")
                    SendUnifreightEventParam(entityPM, "REFFO");
            }
            base.OnUpdating(entityPM);
        }


        private void SendUnifreightEventParam(DeclarationReferantDataPM declarationReferantDataPM,string code)
        {
            try
            {
                var declarationQueryService = new DeclarationQueryService(declarationReferantDataPM.Tenant);
                DeclarationPM connectedDeclarationPM = declarationQueryService.GetSingle(declarationReferantDataPM.DeclarationId, false, false);


                string loggedContactId = null;
                ContactRepository contactRepository = new ContactRepository(Tenant);
                var loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(Tenant), Tenant);
                if (loggedContact != null)
                {
                    loggedContactId = loggedContact.Id;
                }

                var MyUnifreightEventParam = new UnifreightEventParam()
                {
                    Code = code,
                    Mode = UnifreightEventMode.@new,
                    EventDateTime = DateTime.Now,
                    Entname = "CFIFILEM",
                    PrimaryNum = connectedDeclarationPM.CustomFileNo,
                    EventRemarks = "",
                };
                LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
                var myOpenUnifreighTask = new UnifreightEventTaskService();
                myOpenUnifreighTask.UpsertEventLE2U(
                    declarationReferantDataPM.Tenant,
                   loggedContactId,
                    MyUnifreightEventParam);

            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }
    }
}
