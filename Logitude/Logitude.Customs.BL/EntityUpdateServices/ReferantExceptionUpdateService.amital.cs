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
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.TraceEvents;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ReferantExceptionUpdateService
    {
        private void UpdateUnifreight(ReferantExceptionPM dirtyReferantExceptionPM, ExceptionReasonPM exceptionReasonPM)
        {
            if (dirtyReferantExceptionPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Update &&
                dirtyReferantExceptionPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                return;
            }

            var context = CustomContext.GetContext(dirtyReferantExceptionPM.Tenant);
            DeclarationQueryService myDeclarationQueryService = new DeclarationQueryService(context);
            DeclarationPM myDeclarationPM = myDeclarationQueryService.GetSingle(dirtyReferantExceptionPM.DeclarationId, false, false);
            if (myDeclarationPM == null)
            {
                return;
            }

            if (!myDeclarationPM.IsConnectedToUnifreight)
            {
                return;
            }

            ReferantExceptionPM dbOccReferantExceptionPM = GetDBEntity(dirtyReferantExceptionPM);
            if (dbOccReferantExceptionPM == null || dbOccReferantExceptionPM.ExceptionRemarks != dirtyReferantExceptionPM.ExceptionRemarks)
            {
                RaiseEventAndStatus(null, exceptionReasonPM.UnifreightStatusCode, myDeclarationPM, dirtyReferantExceptionPM.ExceptionRemarks, true);

            }

        }
        private ReferantExceptionPM GetDBEntity(ReferantExceptionPM dirtyReferantExceptionPM)
        {

            ReferantExceptionQueryService ReferantExceptionQueryService = new ReferantExceptionQueryService(dirtyReferantExceptionPM.Tenant);
            var myDBEntity = ReferantExceptionQueryService.GetSingle(dirtyReferantExceptionPM.DeclarationId, dirtyReferantExceptionPM.ExceptionReasonsCode, true, false);
            return myDBEntity ?? new ReferantExceptionPM();

        }
        public static void RaiseEventAndStatus(string statusId, string unifrieghtStatus, DeclarationPM declarationPM, string remarks, bool isRaiseUnifreightStatus)
        {
            try
            {
                string loggingUserId = "";
                loggingUserId = AuthenticationUtil.ResolveUserId(declarationPM.Tenant);

                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = declarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = statusId,
                    notes = "DO_NOT_RAISE_EVENT",
                    CommunicationLoggingEntityReference = declarationPM.Id.ToString(),
                    EntityId = declarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status from logitude ",
                };
                myAmitalEventTracerModel.MyFUStatus = new AmitalEventTracerModel.FUStatus()
                {
                    entname = "CFIFILEM",
                    primary_number = declarationPM.CustomFileNo,
                    status = "new",
                    xml_status = "new",
                    status_id = unifrieghtStatus,
                    status_DateTime = DateTime.Now,
                    comments = remarks,
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
