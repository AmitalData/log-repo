using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationConstraintUpdateService
    {
        private void UpdateUnifreight(DeclarationConstraintPM dirtyEntityPM)
        {
            bool toSendStatusVCD = false;
            bool toSendStatusVCC = false;
            bool toSendStatusRAM = false;
            bool toSendStatusRCA = false;

            DeclarationPM connectedDeclarationPM = GetDeclarationEntity(dirtyEntityPM);

            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.EV_NG_8215_ConstraintApprovalDecisionResponseServiceDeny:
                        toSendStatusVCD = true;
                        break;
                    case EventContextTagModel.ProccessEnum.EV_NG_8215_ConstraintApprovalDecisionResponseServiceConditionalApproval:
                        toSendStatusVCC = true;
                        break;
                    case EventContextTagModel.ProccessEnum.EV_NG_8215_ConstraintApprovalDecisionResponseServiceApproved:
                        toSendStatusRAM = true;
                        break;
                    case EventContextTagModel.ProccessEnum.EV_NG_8214_MSG23001_ConstraintApproval:
                        toSendStatusRCA = true;
                        break;
                }
            }

            if (toSendStatusVCD == true)
            {
                SendDeclarationConstraintStatus("CDC", "CDC", dirtyEntityPM, connectedDeclarationPM);
            }

            if (toSendStatusVCC)
            {
                SendDeclarationConstraintStatus("CDA", "CDA", dirtyEntityPM, connectedDeclarationPM);
            }
            if (toSendStatusRAM)
            {
                SendDeclarationConstraintStatus("RAM", "RAM", dirtyEntityPM, connectedDeclarationPM);
            }
            if (toSendStatusRCA)
            {
                SendDeclarationConstraintStatus("RCA", "RCA", dirtyEntityPM, connectedDeclarationPM);
            }
        }

        private DeclarationPM GetDeclarationEntity(DeclarationConstraintPM dirtyEntityPM)
        {
            var declarationQueryService = new DeclarationQueryService(dirtyEntityPM.Tenant);

            var myDBEntity = declarationQueryService.GetSingle(dirtyEntityPM.DeclarationID, false, false);
            return myDBEntity ?? new DeclarationPM();
        }

        private void SendDeclarationConstraintStatus(string statusId,string unifrieghtStatus, DeclarationConstraintPM dirtyEntityPM, DeclarationPM connectedDeclarationPM)
        {
            try
            {
                string loggingUserId = "";
                //var contactRep = new ContactRepository(dirtyEntityPM.Tenant);
                //var contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.ResolveUserIdentityName(dirtyEntityPM.Tenant), dirtyEntityPM.Tenant);
                //loggingUserId = contact.Id;
                loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);

                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = statusId,
                    notes = eventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = dirtyEntityPM.DeclarationID.ToString(),
                    EntityId = dirtyEntityPM.DeclarationID,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = connectedDeclarationPM.CustomFileNo, 
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

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as DeclarationConstraintRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}
