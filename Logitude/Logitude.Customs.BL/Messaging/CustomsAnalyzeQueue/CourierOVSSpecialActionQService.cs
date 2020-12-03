
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.ILOVS;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public class CourierOVSSpecialActionQService : CustomAnalyzerQueueBase
    {
        public CourierOVSSpecialActionQService(InterfaceDetails MyInterfaceDetails)
            : base(MyInterfaceDetails)
        {

        }

        protected override AnalyzeResultModel AnalyzeData(string communicationsData)
        {
            var res = new AnalyzeResultModel();
            try
            {
                
                

                var commSetting = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertDeserializeTyped<CourierWEBAPICommSettings>(_CommunicationLog.LogSettings);
                AnalyzeQResponse(commSetting, communicationsData);
                res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D;

            }
            catch (Exception eee)
            {

                res.ErrorMessage = eee.ToString();
                res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
            }
            return res;
        }

        public void AnalyzeQResponse(CourierWEBAPICommSettings settings, string webAPIResultString)
        {


            if (string.IsNullOrWhiteSpace(settings.RqstCommLogID))
            {
                throw new Exception("settings.RqstCommLogID is must");
            }
            var rqstCommunicationLog = Communications.GetCommunicationLog(settings.Tenant, settings.RqstCommLogID);
            if (rqstCommunicationLog == null)
            {
                throw new Exception($"Cannnot GetCommunicationLog ({settings.RqstCommLogID})");
            }
            var communicationsData = Communications.GetData(rqstCommunicationLog); ;


            var myOVSECSpclRequest =ProxyUtil.JsonConvertDeserializeTyped<OVSECSpclRequest>(communicationsData);
            
            var responeECSpclMamanData = ProxyUtil.JsonConvertDeserializeTyped<CourierOVSHAWBResponse>(webAPIResultString);

            if (responeECSpclMamanData == null)
            {
                throw new Exception("(responeECSpclMamanData == null)");
            }



            LogMessagingUtil.Instance.AppendLine($"AnalyzeResponse(ResponseStatusCode={responeECSpclMamanData.StatusCode},{responeECSpclMamanData.ErrorDescription})");
            var context = CustomContext.GetContext(settings.Tenant);


            //בעת שליחת המסר תבוצע שליפה של טבלת DeclarationMamanSpecialAction לפי מפתח הצהרה + קוד פעולה מיוחדת, והנתונים יישלחו לפי קוד פעולה שהמשתמש בחר + נתונים מ DB של הצהרה + DeclarationMamanSpecialAction
            var declarationMamanSpecialActionQueryService = new DeclarationMamanSpecialActionQueryService(settings.Tenant);
            var pmDeclarationMamanSpecialAction = declarationMamanSpecialActionQueryService.GetSingle(settings.DeclarationId, myOVSECSpclRequest.SpecialActionCode, true, false);




            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myCourierMasterQueryService = new CourierMasterQueryService(context);
            var declarationPM = myDeclarationQueryService.GetSingle(settings.DeclarationId, false, false);
            declarationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;


            bool mamanResponseSuccesed = false;
            switch (responeECSpclMamanData.StatusCode)
            {
                case "1":
                    {
                        //declarationPM.MamanStatusCode = "1";
                        mamanResponseSuccesed = true;
                    }
                    break;


                default:
                    //declarationPM.MamanStatusCode = "2";
                    break;
            }
            string MamanSpecialActionsErrorXml = responeECSpclMamanData.StatusCode.ToString() + "," + responeECSpclMamanData.ErrorDescription?? "";


            string cfifilmFUStatus = "";
            switch (myOVSECSpclRequest.SpecialActionCode)
            {
                case "2"://MamanSpecialCode.ReceivingDelayCertificate_DelayIt
                    cfifilmFUStatus = "CDE";
                    break;
                case "4"://MamanSpecialCode.StickerPrinting 
                    cfifilmFUStatus = "CLB";
                    break;
                case "5"://MamanSpecialCode.PrintDocuments
                    cfifilmFUStatus = "CDO";
                    break;
            }
            var toCancel = false;
            UnifreightEventMode unifreightEventMode = UnifreightEventMode.@new;
            if (myOVSECSpclRequest.MessageType == "C")
            {
                toCancel = true;
                unifreightEventMode = UnifreightEventMode.del;
            }
            using (var scope = TransactionFactory.GetNewTransaction())
            {


                pmDeclarationMamanSpecialAction.MamanSpecialActionsErrorXml = MamanSpecialActionsErrorXml;

                var myDeclarationMamanSpecialAction = new DeclarationMamanSpecialActionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), settings.Tenant);
                pmDeclarationMamanSpecialAction.MamanSpecialActionsErrorXml = MamanSpecialActionsErrorXml;

                if (!mamanResponseSuccesed)
                {
                    //update Failed Status  + message !!!
                    pmDeclarationMamanSpecialAction.MamanSpecialActionStatusCode = "2";//2   Error   2,error


                    pmDeclarationMamanSpecialAction.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                }
                else
                {
                    if (toCancel)
                    {
                        // delete record myDeclarationMamanSpecialAction
                        pmDeclarationMamanSpecialAction.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                        // Create FUStatus Delete 
                    }
                    else
                    {
                        // update record myDeclarationMamanSpecialAction = for status
                        //update Failed Status  + message !!!
                        pmDeclarationMamanSpecialAction.MamanSpecialActionStatusCode = "1";//1   Valid   1,valid
                        pmDeclarationMamanSpecialAction.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        // Create FUStatus 
                    }
                    var unifreightFUStatusTaskService = new UnifreightFUStatusTaskService();
                    unifreightFUStatusTaskService.UpsertFUStatusLE2U(settings.Tenant, settings.LoggedContactId, new UnifreightFUStatusParam()
                    {
                        Entname = "CFIFILEM",
                        PrimaryNum = declarationPM.CustomFileNo,
                        Mode = unifreightEventMode,
                        StatusCode = cfifilmFUStatus,
                        StatusRemarks = MamanSpecialActionsErrorXml,

                    });

                }
                myDeclarationMamanSpecialAction.Update(pmDeclarationMamanSpecialAction, true);

                scope.Complete();
            }
        }
    }
}
