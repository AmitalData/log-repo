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
    public partial class ProceduralFaultUpdateService
    {
        private void UpdateUnifreight(ProceduralFaultPM dirtyEntityPM,string direction)
        {
            string loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);

            bool toSendStatusLIK = false;
            bool toSendStatusLIC = false;

            if (string.IsNullOrWhiteSpace(dirtyEntityPM.DeclarationId))
            {
                return;
            }

            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.EV_NG_8218_MSG14100_ProceduralFaultMsgInsert:
                    case EventContextTagModel.ProccessEnum.EV_NG_8218_MSG14100_ProceduralFaultMsgUpdate:
                        toSendStatusLIK = true;
                        break;
                    case EventContextTagModel.ProccessEnum.EV_NG_8218_MSG14100_ProceduralFaultMsgCancel:
                        toSendStatusLIC = true;
                        break;
                }
            }

            if (direction != "E" && toSendStatusLIK == true)
            {
                SendProceduralFaultStatus("LIK", "LIK", dirtyEntityPM, loggingUserId);
            }
            if (toSendStatusLIC == true)
            {
                SendProceduralFaultStatus("LIC", "LIC", dirtyEntityPM, loggingUserId);
            }
        }

        private void SendProceduralFaultStatus(string statusId, string unifrieghtStatus, ProceduralFaultPM dirtyEntityPM, string loggingUserId)
        {
            try
            {
                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var objectTableName = "Customs.Deposit";
                var entityId = dirtyEntityPM.Id.ToString();
                var customFile = "";

                if (eventContextTagModel.StatusObjectTable != null && eventContextTagModel.StatusEntityId != null)
                {
                    objectTableName = eventContextTagModel.StatusObjectTable;
                    entityId = eventContextTagModel.StatusEntityId;
                    customFile = eventContextTagModel.StatusCustomFileNo;
                }
                else
                {
                    objectTableName = "Customs.Declaration";
                    entityId = dirtyEntityPM.DeclarationId;
                    customFile = dirtyEntityPM.CustomFileNo;
                }
           

                var comments = eventContextTagModel.FUStatusRemarks;
                if (statusId == "LIK")
                {
                    comments = dirtyEntityPM.Remarks;
                }


                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = objectTableName,
                    EntityId = entityId,
                    CommunicationLoggingEntityReference = entityId,
                    EventCode = statusId,
                    notes = eventContextTagModel.EventRemarks,                  
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = customFile, 
                        status = "new",
                        xml_status = "new",
                        status_id = unifrieghtStatus,
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = comments,
                    }
                };
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);
            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

    }
}
