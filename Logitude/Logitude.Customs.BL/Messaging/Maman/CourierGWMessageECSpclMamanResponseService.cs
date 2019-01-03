#define  waitTillMiritWillCreateDBAndScreen
//https://maman.wsfreeze.co.il/WebAPIExt/Help/Api/POST-api-baldar-CreateECSpclMessgae
//https://docs.google.com/document/d/11_pcjrqx4f8pQBUnz2JMZDhxl4b23dRBRYdl4XKLd-I/edit



using Logitude.BL.Helpers;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Maman
{
    public class CourierGWMessageECSpclMamanResponseService : IWebAPIMessage2MamanAnalyzer///using  by SendWEBAPIMessage2MamanWRWR
    {



        public void AnalyzeResponse(Courier2MamanCommSettings settings, string webAPIResultString)
        {




            var responeECSpclMamanData = ProxyUtil.JsonConvertDeserializeTyped<ECSpclMamanMessage>(webAPIResultString);

            if (responeECSpclMamanData == null)
            {
                throw new Exception("(responeECSpclMamanData == null)");
            }



            LogMessagingUtil.Instance.AppendLine($"AnalyzeResponse(ResponseStatusCode={responeECSpclMamanData.ResponseStatusCode},{responeECSpclMamanData.ResponseStatusMsg})");
            var context = CustomContext.GetContext(settings.Tenant);
#if waitTillMiritWillCreateDBAndScreen

            //בעת שליחת המסר תבוצע שליפה של טבלת DeclarationMamanSpecialAction לפי מפתח הצהרה + קוד פעולה מיוחדת, והנתונים יישלחו לפי קוד פעולה שהמשתמש בחר + נתונים מ DB של הצהרה + DeclarationMamanSpecialAction
            var declarationMamanSpecialActionQueryService = new DeclarationMamanSpecialActionQueryService(settings.Tenant);
            var pmDeclarationMamanSpecialAction = declarationMamanSpecialActionQueryService.GetSingle(settings.DeclarationId, responeECSpclMamanData.SpSpclCode, false, false);

#endif


            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myCourierMasterQueryService = new CourierMasterQueryService(context);
            var declarationPM = myDeclarationQueryService.GetSingle(settings.DeclarationId, false, false);
            declarationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;


            bool mamanResponseSuccesed = false;
            switch (responeECSpclMamanData.ResponseStatusCode)
            {
                case 1:
                    {
                        //declarationPM.MamanStatusCode = "1";
                        mamanResponseSuccesed = true;
                    }
                    break;
                
                    
                default:
                    //declarationPM.MamanStatusCode = "2";
                    break;
            }
            string MamanSpecialActionsErrorXml = responeECSpclMamanData.ResponseStatusCode.ToString() + "," + responeECSpclMamanData.ResponseStatusMsg ?? "";


            string cfifilmFUStatus = "";
            switch (responeECSpclMamanData.SpSpclCode)
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
            UnifreightEventMode unifreightEventMode= UnifreightEventMode.@new;
            if (responeECSpclMamanData.ActionCode== "C")
            {
                toCancel = true;
                unifreightEventMode = UnifreightEventMode.del;
            }
            using (var scope = TransactionFactory.GetNewTransaction())
            {
#if waitTillMiritWillCreateDBAndScreen

                pmDeclarationMamanSpecialAction.MamanSpecialActionsErrorXml = MamanSpecialActionsErrorXml;

                var myDeclarationMamanSpecialAction = new DeclarationMamanSpecialActionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), settings.Tenant);
                pmDeclarationMamanSpecialAction.MamanSpecialActionsErrorXml = MamanSpecialActionsErrorXml;
#endif
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
