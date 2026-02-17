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
    public partial class CustomsDocumentPointerUpdateService
    {

        private void UpdateUnifreight(CustomsDocumentPointerPM dirtyEntityPM)
        {
            string loggingUserId = "";
            //var contactRep = new ContactRepository(dirtyEntityPM.Tenant);
            //var contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.ResolveUserIdentityName(dirtyEntityPM.Tenant), dirtyEntityPM.Tenant);
            //loggingUserId = contact.Id;
            loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);

            bool toSendStatusCRD = false;
            bool toSendStatusCRC = false;

            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.VAL_NG_8227_MSG_520_RequiredDocumentMessageInsert:
                        toSendStatusCRD = true;
                        break;
                    case EventContextTagModel.ProccessEnum.VAL_NG_8227_MSG_520_RequiredDocumentMessageDelete:
                        toSendStatusCRC = true;
                        break;
                }
            }

            if (toSendStatusCRD == true)
            {
                SendDocumentPointerStatus("CRD", "CRD", dirtyEntityPM, loggingUserId);
            }
            if (toSendStatusCRC == true)
            {
                SendDocumentPointerStatus("CRC", "CRC", dirtyEntityPM, loggingUserId);
            }
        }

        private void SendDocumentPointerStatus(string statusId, string unifrieghtStatus, CustomsDocumentPointerPM dirtyEntityPM, string loggingUserId)
        {
            try
            {
                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = eventContextTagModel.StatusObjectTable,
                    EventCode = statusId,
                    notes = eventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = eventContextTagModel.StatusEntityId,
                    EntityId = dirtyEntityPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = eventContextTagModel.StatusCustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = unifrieghtStatus,
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = eventContextTagModel.FUStatusRemarks,
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
