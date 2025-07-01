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
    public partial class DepositUpdateService
    {
        private void UpdateUnifreight(DepositPM dirtyEntityPM)
        {
            string loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);;

            bool toSendStatusDRE = false;
            bool toSendStatusDFO = false;
            bool toSendStatusDPN = false;

            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.DEPO_2020_DepositRefundOrderInfoResponseServiceRefund:
                        toSendStatusDRE = true;
                        break;
                    case EventContextTagModel.ProccessEnum.DEPO_2000_DepositForfeitOrderInfoResponseServiceForfeit:
                        toSendStatusDFO = true;
                        break;
                    case EventContextTagModel.ProccessEnum.DEPO_NG_5110_DepositRequestFulfillednfoMsg:
                        toSendStatusDPN = true;
                        break;
                }
            }

            if (toSendStatusDRE == true)
            {
                SendDeclarationConstraintStatus("DRE", "DRE", dirtyEntityPM, loggingUserId);
            }
            if (toSendStatusDFO == true)
            {
                SendDeclarationConstraintStatus("DFO", "DFO", dirtyEntityPM, loggingUserId);
            }
            if (toSendStatusDPN == true)
            {
                SendDeclarationConstraintStatus("DPN", "DPN", dirtyEntityPM, loggingUserId);
            }
        }

        private void SendDeclarationConstraintStatus(string statusId, string unifrieghtStatus, DepositPM dirtyEntityPM, string loggingUserId)
        {
            try
            {
                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = "Customs.Deposit",
                    EventCode = statusId,
                    notes = eventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = dirtyEntityPM.Id.ToString(),
                    EntityId = dirtyEntityPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = "", //to check??
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
