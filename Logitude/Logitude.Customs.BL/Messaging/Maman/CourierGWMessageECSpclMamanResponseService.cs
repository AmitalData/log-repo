#define  waitTillMiritWillCreateDBAndScreen
//https://maman.wsfreeze.co.il/WebAPIExt/Help/Api/POST-api-baldar-CreateECSpclMessgae
//https://docs.google.com/document/d/11_pcjrqx4f8pQBUnz2JMZDhxl4b23dRBRYdl4XKLd-I/edit



using Logitude.BL.Helpers;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
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
    public class CourierGWMessageECSpclMamanResponseService //: IWebAPIMessage2MamanAnalyzer///using  by SendWEBAPIMessage2MamanWRWR
    {



        public void AnalyzeQResponse(CourierWEBAPICommSettings settings, string webAPIResultString)
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
                if (pmDeclarationMamanSpecialAction == null)
                {
                    throw new Exception("AnalyzeQResponse():pmDeclarationMamanSpecialAction == null");
                }
#endif      
            try
            {


                var myDeclarationQueryService = new DeclarationQueryService(context);
                var myCourierMasterQueryService = new CourierMasterQueryService(context);
                var declarationPM = myDeclarationQueryService.GetSingle(settings.DeclarationId, true, false);
                declarationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                LogMessagingUtil.Instance.AppendLine($"AnalyzeQResponse(declarationPM.id={declarationPM.Id})");
                bool mamanResponseSuccesed = false;
                switch (responeECSpclMamanData.ResponseStatusCode)
                {
                       
                    case 1:
                        {
                            //declarationPM.MamanStatusCode = "1";
                            mamanResponseSuccesed = true;
                            if (responeECSpclMamanData.SpSpclCode == "2")
                            {
                                var myDeclarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), settings.Tenant);
                                var declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(settings.Tenant);
                                DeclarationCourierStatusPM myDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(settings.DeclarationId, true, false);
                                myDeclarationCourierStatusPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                                myDeclarationCourierStatusPM.FastIndividualProcessCode = "I";
                                myDeclarationCourierStatusUpdateService.Update(myDeclarationCourierStatusPM, true);
                            }
                        }
                        break;

                    default:
                        //declarationPM.MamanStatusCode = "2";
                        break;
                }
                LogMessagingUtil.Instance.AppendLine($"mamanResponseSuccesed{mamanResponseSuccesed})");
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
                    case "6"://MamanSpecialCode.Sban
                        cfifilmFUStatus = "CDS";
                        break;
                }
                LogMessagingUtil.Instance.AppendLine($"cfifilmFUStatus{cfifilmFUStatus})");
                var toCancel = false;
                UnifreightEventMode unifreightEventMode = UnifreightEventMode.@new;
                if (responeECSpclMamanData.ActionCode == "C")
                {
                    toCancel = true;
                    unifreightEventMode = UnifreightEventMode.del;
                }
                ///using (var scope = TransactionFactory.GetNewTransaction())
                {
#if waitTillMiritWillCreateDBAndScreen

                    pmDeclarationMamanSpecialAction.MamanSpecialActionsErrorXml = MamanSpecialActionsErrorXml;

                    var myDeclarationMamanSpecialAction = new DeclarationMamanSpecialActionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), settings.Tenant);
                    pmDeclarationMamanSpecialAction.MamanSpecialActionsErrorXml = MamanSpecialActionsErrorXml;
#endif
                    LogMessagingUtil.Instance.AppendLine($"MamanSpecialActionsErrorXml{MamanSpecialActionsErrorXml})");
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
                        LogMessagingUtil.Instance.AppendLine($"UpsertFUStatusLE2U");
                        LogMessagingUtil.Instance.AppendLine($"declarationPM.CustomFileNo{declarationPM.CustomFileNo})");
                        LogMessagingUtil.Instance.AppendLine($"unifreightEventMode{unifreightEventMode})");
                        LogMessagingUtil.Instance.AppendLine($"cfifilmFUStatus{cfifilmFUStatus})");
                        LogMessagingUtil.Instance.AppendLine($"MamanSpecialActionsErrorXml{MamanSpecialActionsErrorXml})");


                        var unifreightFUStatusTaskService = new UnifreightFUStatusTaskService();
                        unifreightFUStatusTaskService.UpsertFUStatusLE2U(settings.Tenant, settings.LoggedContactId, new UnifreightFUStatusParam()
                        {
                            Entname = "CFIFILEM",
                            PrimaryNum = declarationPM?.CustomFileNo,
                            Mode = unifreightEventMode,
                            StatusCode = cfifilmFUStatus,
                            StatusRemarks = MamanSpecialActionsErrorXml,

                        });

                    }
                    myDeclarationMamanSpecialAction.Update(pmDeclarationMamanSpecialAction, true);
                    LogMessagingUtil.Instance.AppendLine($"UpsertFUStatusLE2U-> myDeclarationMamanSpecialAction.Update{pmDeclarationMamanSpecialAction})");


                    if (cfifilmFUStatus == "CDE" && mamanResponseSuccesed)
                    {
                        LogMessagingUtil.Instance.AppendLine($"Send2Masof");

                        var mySend2MasofIfNeededService = new Send2MasofIfNeededService();
                        mySend2MasofIfNeededService.Send2Masof(declarationPM, false, declarationPM, true);
                        LogMessagingUtil.Instance.AppendLine($"Send2Masof end");

                    }

                    //scope.Complete();
                }
            }
            catch (Exception ex)
			{

                LogMessagingUtil.Instance.AppendLine($"ex{ex.StackTrace}");

                var myDeclarationMamanSpecialAction = new DeclarationMamanSpecialActionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), settings.Tenant);

                pmDeclarationMamanSpecialAction.MamanSpecialActionStatusCode = "2";//2   Error   2,error
                pmDeclarationMamanSpecialAction.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                myDeclarationMamanSpecialAction.Update(pmDeclarationMamanSpecialAction, true);
            }
        }


#if true
        public void AnalyzeResponse(CourierWEBAPICommSettings settings, string webAPIResultString)
        {
            throw new Exception("use  SetInAnalyzeQResponseService by @intrface.ResponseCode");
            var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
            var def = customsPartnerFtpDetails.GetAllInterfaceDetails().First(r => r.Code == CustomsPartnerFtpDetails.InterfaceName_ECMMNSPCL_Response);
            var commSetting = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(settings);
            var analyzeQueueUtil = new AnalyzeQueueUtil();
            var new_analyze = analyzeQueueUtil
               .SaveMessageToAnalyzeQueue("", Encoding.UTF8.GetBytes(webAPIResultString), settings.Tenant,
               commSetting, def,
               new AnalyzeResultModel()
               {
                   EntityID = settings.DeclarationId,
                   ObjectTableID = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),

               });

            LogMessagingUtil.Instance.AppendLine($"new_analyze  CommunicationLogId = {new_analyze.CommunicationLogId}");

        }


#endif
    }


}
