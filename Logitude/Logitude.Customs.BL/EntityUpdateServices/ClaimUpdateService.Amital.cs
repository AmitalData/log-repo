using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClaimUpdateService
    {
        private void UpdateUnifreight(ClaimPM dirtyEntityPM)
        {
            bool toSendStatusOPN = false;

            //DeclarationPM connectedDeclarationPM = GetDeclarationEntity(dirtyEntityPM);

            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.CreateNewClaim:
                        toSendStatusOPN = true;
                        break;
                }
            }

            if (toSendStatusOPN == true)
            {
                RaiseClaimEventAndStatus("OPNC", "OPNC", dirtyEntityPM, null, true);
            }

        }

        public static void RaiseClaimEventAndStatus(string statusId, string unifrieghtStatus, ClaimPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, bool isRaiseUnifreightStatus)
        {
            try
            {
                string loggingUserId = "";
                loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);

                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = "Customs.Claim",
                    EventCode = statusId,
                    notes = eventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = dirtyEntityPM.Id.ToString(),
                    EntityId = dirtyEntityPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status from logitude ",
                };

                if (isRaiseUnifreightStatus != true && connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomFileNo))
                {
                    myAmitalEventTracerModel.MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = connectedDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = unifrieghtStatus,
                        status_DateTime = DateTime.Now,
                        //status_save = "no_fail",
                        comments = eventContextTagModel.FUStatusRemarks,
                    };
                }
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, isRaiseUnifreightStatus);
            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }
    }

}
