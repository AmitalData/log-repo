using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationUpdateService
    {
       public  void CheckFileCredit(DeclarationPM dirtyDeclarationPM, DeclarationPM dbOccDeclarationPM, string loggingUserId)
        {
            var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
               Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess,
               "CWSFCREDITFILE", "DeclarationCheckCredit")
           {
               Tenant = dirtyDeclarationPM.Tenant,
               objectTableName = "Customs.Declaration",
               CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
               EntityId = dirtyDeclarationPM.Id,
               UserId = loggingUserId,
               CommunicationSubject = "Logitude Declaration check File Credit",

               //LogitudeFile = myFile,
           };

            var myCreditFile = new CustomFileCreditRequest();
            myCreditFile.CustomsFile = new CustomsFile[] { new CustomsFile() };
            myCreditFile.CustomsFile[0].FileNo = dirtyDeclarationPM.CustomFileNo;
            myCreditFile.CustomsFile[0].Mode = "Check";
            myCreditFile.CustomsFile[0].TotalTax = dirtyDeclarationPM.TotalTax.ToString();

            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService
                <Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase, CustomFileCreditRequest>(
                amitalCustomFileCommunicationModel, myCreditFile);
            var info = myUServerCommunicationService.Send();
        }

       public void TransferFileCredit(DeclarationPM dirtyDeclarationPM, DeclarationPM dbOccDeclarationPM, string loggingUserId)
        {
            var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
               Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess,
               "CWSFCREDITFILE", "DeclarationCheckCredit")
            {
                Tenant = dirtyDeclarationPM.Tenant,
                objectTableName = "Customs.Declaration",
                CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                EntityId = dirtyDeclarationPM.Id,
                UserId = loggingUserId,
                CommunicationSubject = "Logitude Declaration Check File Credit",

                //LogitudeFile = myFile,
            };

            var myCreditFile = new CustomFileCreditRequest();
            myCreditFile.CustomsFile = new CustomsFile[] { new CustomsFile() }; // to check if only one occ?
            myCreditFile.CustomsFile[0].FileNo = dirtyDeclarationPM.CustomFileNo;
            myCreditFile.CustomsFile[0].Mode = "Transfer";
            myCreditFile.CustomsFile[0].TotalTax = dirtyDeclarationPM.TotalTax.ToString();

            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService
                <Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase, CustomFileCreditRequest>(
                amitalCustomFileCommunicationModel, myCreditFile);
            var info = myUServerCommunicationService.Send();
        }

        public static string ResetDeclarationNumber(string declarationId, int tenant)
        {
            try
            {
                var customContext = CustomContext.GetContext(tenant);
                var declarationQuery = new DeclarationQueryService(customContext);
                DeclarationPM myDeclarationPM = declarationQuery.GetSingle(declarationId, false, false);

                //If the Declaration exists
                if (myDeclarationPM == null) return null;

                //Update ExternalDeclarationNumber
                string externalDeclarationNumber = null;
                int pos = myDeclarationPM.ExternalDeclarationNumber.IndexOf("-");
                if (pos == -1)
                {
                    externalDeclarationNumber = myDeclarationPM.ExternalDeclarationNumber + "-1";
                }
                else
                {
                    if (pos + 1 >= myDeclarationPM.ExternalDeclarationNumber.Length)
                    {
                        externalDeclarationNumber = myDeclarationPM.ExternalDeclarationNumber.Substring(0, myDeclarationPM.ExternalDeclarationNumber.Length - 1) + "-1";
                    }
                    else
                    {
                        int after;
                        if (int.TryParse(myDeclarationPM.ExternalDeclarationNumber.Substring(pos + 1), out after))
                        {
                            after++;
                            externalDeclarationNumber = myDeclarationPM.ExternalDeclarationNumber.Substring(0, pos) + "-" + after.ToString();
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(externalDeclarationNumber))
                {
                    myDeclarationPM.ExternalDeclarationNumber = externalDeclarationNumber;
                }

                var traceEventParams = new EventTracerArgs()
                {
                    EntityId = myDeclarationPM.Id,
                    ObjectTableName = "Customs.Declaration",
                    Tenant = myDeclarationPM.Tenant,
                    UserId = AuthenticationUtil.ResolveUserId(myDeclarationPM.Tenant),
                    EventTypeCode = "DNR",
                    Notes = "Reset Declaration Number." + Environment.NewLine + "Old DeclarationNumber: " + myDeclarationPM.DeclarationNumber + Environment.NewLine + "Old VersionId: " + myDeclarationPM.VersionId,
                };
                EventTracer.CreateTraceEvent(traceEventParams);

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent: EventCode= " + "DNR" + "CustomFileNo= " + myDeclarationPM.CustomFileNo + "  ");

                myDeclarationPM.DeclarationNumber = null;
                myDeclarationPM.VersionId = null;
                myDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;

                myDeclarationPM.MarkAsChanged = true;
                myDeclarationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                DeclarationUpdateService service = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), myDeclarationPM.Tenant);
                service.Update(myDeclarationPM, true);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }

        public static string DeclarationClosure(string declarationId, int tenant)
        {
            try
            {
                var customContext = CustomContext.GetContext(tenant);
                var declarationQuery = new DeclarationQueryService(customContext);
                DeclarationPM myDeclarationPM = declarationQuery.GetSingle(declarationId, false, false);

                //If the Declaration exists
                if (myDeclarationPM == null) return null;

                //Update IsClose
                var traceEventParams = new EventTracerArgs()
                {
                    EntityId = myDeclarationPM.Id,
                    ObjectTableName = "Customs.Declaration",
                    Tenant = myDeclarationPM.Tenant,
                    UserId = AuthenticationUtil.ResolveUserId(myDeclarationPM.Tenant),
                    EventTypeCode = "DCS",
                    Notes = "Close Declaration " + myDeclarationPM.DeclarationNumber,
                };
                EventTracer.CreateTraceEvent(traceEventParams);

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent: EventCode= " + "DCS" + "CustomFileNo= " + myDeclarationPM.CustomFileNo + "  ");
                myDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                myDeclarationPM.IsClose = true;
                DeclarationUpdateService service = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), myDeclarationPM.Tenant);
                service.Update(myDeclarationPM, true);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }

        public static string CancelDeclarationClosure(string declarationId, int tenant)
        {
            try
            {
                var customContext = CustomContext.GetContext(tenant);
                var declarationQuery = new DeclarationQueryService(customContext);
                DeclarationPM myDeclarationPM = declarationQuery.GetSingle(declarationId, false, false);

                //If the Declaration exists
                if (myDeclarationPM == null) return null;

                //Update IsClose
                var traceEventParams = new EventTracerArgs()
                {
                    EntityId = myDeclarationPM.Id,
                    ObjectTableName = "Customs.Declaration",
                    Tenant = myDeclarationPM.Tenant,
                    UserId = AuthenticationUtil.ResolveUserId(myDeclarationPM.Tenant),
                    EventTypeCode = "CDCS",
                    Notes = "Cancel Declaration Close " + myDeclarationPM.DeclarationNumber,
                };
                EventTracer.CreateTraceEvent(traceEventParams);

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent: EventCode= " + "CDCS" + "CustomFileNo= " + myDeclarationPM.CustomFileNo + "  ");
                myDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                myDeclarationPM.IsClose = false;
                DeclarationUpdateService service = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), myDeclarationPM.Tenant);
                service.Update(myDeclarationPM, true);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }

    }
}
