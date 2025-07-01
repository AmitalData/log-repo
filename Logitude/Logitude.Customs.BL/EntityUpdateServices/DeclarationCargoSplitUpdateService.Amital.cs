using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
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
    public partial class DeclarationCargoSplitUpdateService
    {
        private void UpdateUnifreight(DeclarationCargoSplitPM dirtyEntityPM)
        {
            bool toSendStatusOPN = false;
            string FUStatusRemarks = "";
            string eventRemarks = "";

            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    //case EventContextTagModel.ProccessEnum.CreateNewDeclarationCargoSplit:
                    //    toSendStatusOPN = true;
                    //    break;
                }
                FUStatusRemarks = eventContextTagModel.FUStatusRemarks;
                eventRemarks = eventContextTagModel.EventRemarks;
            }

            if (toSendStatusOPN == true)
            {

                RaiseDeclarationCargoSplitEventAndStatus("OPN", "OPN", dirtyEntityPM, null, true, eventRemarks, FUStatusRemarks);
            }

        }

        public static void RaiseDeclarationCargoSplitEventAndStatus(string statusId, string unifrieghtStatus, DeclarationCargoSplitPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, bool suppressSendToUniFreight, string eventRemarks, string FUStatusRemarks)
        {
            try
            {
                string loggingUserId = "";
                loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);

                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = "Customs.DeclarationCargoSplit",
                    EventCode = statusId,
                    notes = eventRemarks,
                    CommunicationLoggingEntityReference = dirtyEntityPM.Id.ToString(),
                    EntityId = dirtyEntityPM.Id,
                    UserId = loggingUserId,
                    CommunicationSubject = "FU Status from logitude ",
                };

                if (suppressSendToUniFreight != true && connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomFileNo))
                {
                    myAmitalEventTracerModel.MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = connectedDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = unifrieghtStatus,
                        status_DateTime = DateTime.Now,
                         
                        comments = FUStatusRemarks,
                    };
                }
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, suppressSendToUniFreight);
            }
            catch (Exception)
            {
                 
                throw;
            }
        }
    }

}
