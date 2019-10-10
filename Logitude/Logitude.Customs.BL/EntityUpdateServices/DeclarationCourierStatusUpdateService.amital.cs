using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationCourierStatusUpdateService
    {

        private void UpdateUnifreight(DeclarationCourierStatusPM dirtyDeclarationCourierStatusPM)
        {
            if (dirtyDeclarationCourierStatusPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Update &&
                dirtyDeclarationCourierStatusPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                return;
            }

            var context = CustomContext.GetContext(dirtyDeclarationCourierStatusPM.Tenant);
            DeclarationQueryService myDeclarationQueryService = new DeclarationQueryService(context);
            DeclarationPM myDeclarationPM = myDeclarationQueryService.GetSingle(dirtyDeclarationCourierStatusPM.DeclarationId, false, false);
            if (myDeclarationPM == null)
            {
                return;
            }

            if (!myDeclarationPM.IsConnectedToUnifreight)
            {
                return;
            }

            DeclarationCourierStatusPM dbOccDeclarationCourierStatusPM = GetDBEntity(dirtyDeclarationCourierStatusPM);

            //if ((!string.IsNullOrEmpty(dirtyDeclarationCourierStatusPM.CourierPendingReasonCode) && dirtyDeclarationCourierStatusPM.CourierPendingReasonCode != dbOccDeclarationCourierStatusPM.CourierPendingReasonCode)
            //   || (!string.IsNullOrEmpty(dirtyDeclarationCourierStatusPM.PendingRemarks) && dirtyDeclarationCourierStatusPM.PendingRemarks != dbOccDeclarationCourierStatusPM.PendingRemarks))
            if ((!string.IsNullOrEmpty(dirtyDeclarationCourierStatusPM.CourierPendingReasonList) && dirtyDeclarationCourierStatusPM.CourierPendingReasonList != dbOccDeclarationCourierStatusPM.CourierPendingReasonList))
            {
                CourierPendingReasonQueryService myCourierPendingReasonQueryService = new CourierPendingReasonQueryService(context);
                string[] courierPendingReasonList = dirtyDeclarationCourierStatusPM.CourierPendingReasonList.Split(',').Select(sValue => sValue.Trim()).ToArray();
                string[] prevCourierPendingReasonList = !string.IsNullOrEmpty(dbOccDeclarationCourierStatusPM.CourierPendingReasonList) ? dbOccDeclarationCourierStatusPM.CourierPendingReasonList.Split(',').Select(sValue => sValue.Trim()).ToArray() : new string[] { };
                //CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingle(dirtyDeclarationCourierStatusPM.CourierPendingReasonCode, false, false);
                foreach (var courierPendingReason in courierPendingReasonList)
                {
                    if (!string.IsNullOrWhiteSpace(courierPendingReason) && !prevCourierPendingReasonList.Contains(courierPendingReason))
                    {
                        CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingle(courierPendingReason, false, false);

                        if (courierPendingReasonPM != null && !string.IsNullOrEmpty(courierPendingReasonPM.UnifreightStatusCode))
                        {
                            DeclarationPendingPM declarationPendingPM = new DeclarationPendingPM();
                            declarationPendingPM = dirtyDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == myDeclarationPM.Id && r.CourierPendingReasonCode == courierPendingReason).FirstOrDefault();
                            if (declarationPendingPM != null)
                            {
                                RaiseEventAndStatus(null, courierPendingReasonPM.UnifreightStatusCode, myDeclarationPM, declarationPendingPM.PendingRemarks, true);
                            }
                        }
                    }
                }
            }
        }

        private DeclarationCourierStatusPM GetDBEntity(DeclarationCourierStatusPM dirtyDeclarationCourierStatusPM)
        {

            DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(dirtyDeclarationCourierStatusPM.Tenant);
            var myDBEntity = declarationCourierStatusQueryService.GetSingle(dirtyDeclarationCourierStatusPM.DeclarationId, true, false);
            return myDBEntity ?? new DeclarationCourierStatusPM();

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
                    objectTableName = "Customs.Claim",
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
