using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class TapagConnectionTableUpdateService
    {

        private void UpdateUnifreight(TapagConnectionTablePM dirtyEntityPM)
        {
            string loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);
            bool toSendStatusCGN = false;

            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.GRNT_MSG15_createGurateeRequestInfoResponseService:
                        toSendStatusCGN = true;
                        break;
                }
            }

            if (toSendStatusCGN == true)
            {
                SendDeclarationConstraintStatus("CGN", "CGN", dirtyEntityPM, loggingUserId);
            }
        }

        private void SendDeclarationConstraintStatus(string statusId, string unifrieghtStatus, TapagConnectionTablePM dirtyEntityPM, string loggingUserId)
        {
            try
            {
                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = statusId,
                    notes = eventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = dirtyEntityPM.DeclarationId.ToString(),
                    EntityId = dirtyEntityPM.DeclarationId,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyEntityPM.CustomFileNo, 
                        status = "new",
                        xml_status = "new",
                        status_id = unifrieghtStatus,
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                         
                        comments = eventContextTagModel.FUStatusRemarks,
                    }
                };
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);
            }
            catch (Exception)
            {
                 
                throw;
            }
        }
    }
}
