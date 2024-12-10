
//https://docs.google.com/document/d/1bFMdrDnByDpvLcvE9H5eOfCAzbVdeoUypbzhwxbr0Po/edit#heading=h.hjpcmz7krlpn

using Logitude.BL.Helpers;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;
using Logitude.Customs.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Logitude.Customs.BL.Messaging.ILSWS

{
    public class ILSWSQHAWBQService : CustomAnalyzerQueueBase
    {
        public ILSWSQHAWBQService(InterfaceDetails MyInterfaceDetails)
           : base(MyInterfaceDetails)
        {

        }

        protected override AnalyzeResultModel AnalyzeData(string communicationsData)
        {
            var res = new AnalyzeResultModel();
            try
            {
                res.ObjectTableID = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                LogMessagingUtil.Instance.AppendLine("CourierSWSStatusAvailabilityQService");

                CourierSWSHAWBResponse CourierSWSHAWBResponse = GetSWSHAWBResponse(communicationsData);
                if (string.IsNullOrWhiteSpace(CourierSWSHAWBResponse.CourierHawbNumber))
                {
                    res.ErrorMessage = $"bad communicationsData  mySTBMessage.CourierHawbNumber is null";
                    res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
                    return res;
                }
                var context = CustomContext.GetContext(_CommunicationLog.Tenant);
                var qs = new DeclarationQueryService(_CommunicationLog.Tenant);
                var idList = qs.GetListByCourierHAWB(CourierSWSHAWBResponse.CourierHawbNumber, _CommunicationLog.Tenant);
                var myCourierDeclarationsQueryService = new CourierDeclarationQueryService(context);
                string decID = null;
                if (idList.Count == 1)
                {
                    decID = idList.FirstOrDefault();
                    res.EntityReference = qs.GetCustomFileNoByDeclarationId(idList.FirstOrDefault(), _CommunicationLog.Tenant);
                    res.EntityID = decID;
                }
                var myDeclarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);

                var declarationCourierStatusQueryServicePM = myDeclarationCourierStatusQueryService.GetSingle(decID, true, false);
                declarationCourierStatusQueryServicePM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                switch (CourierSWSHAWBResponse.StatusCode)
                {
                    case "1"://45997
                        {
                            //declarationPM.MamanStatusCode = "1";
                            declarationCourierStatusQueryServicePM.StorageSiteStatusCode = "1";
                        }
                        break;
                    default:
                        //declarationPM.MamanStatusCode = "2";//45997
                        declarationCourierStatusQueryServicePM.StorageSiteStatusCode = "2";
                        break;
                }


                declarationCourierStatusQueryServicePM.StorageSiteErrorText = CourierSWSHAWBResponse.StatusCode + "," + CourierSWSHAWBResponse.ErrorDescription;

                using (var scope = TransactionFactory.GetNewTransaction())
                {
                    var myDeclarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), _CommunicationLog.Tenant);
                    myDeclarationCourierStatusUpdateService.Update(declarationCourierStatusQueryServicePM, true);
                    scope.Complete();
                }
                res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D;
            }
            catch (BusinessErrorException ee)
            {
                res.ErrorMessage = ee.ToString();
                res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D;
            }
            catch (Exception ee)
            {
                throw;
            }
            return res;
        }

        private static XElement GetXElement(XElement myXElementSTBMessage, string field)
        {



            XElement ele = myXElementSTBMessage.Element(field);
            if (ele == null)
            {
                throw new Exception($"XElement {field} not exist ");
            }

            return ele;
        }


        private static CourierSWSHAWBResponse GetSWSHAWBResponse(string communicationsData)
        {

            var myXElementSWSHAWBResponse = XElement.Parse(communicationsData);
            var mySWSHAWBResponse = new CourierSWSHAWBResponse();
            mySWSHAWBResponse.CourierCompanyVat = (string)GetXElement(myXElementSWSHAWBResponse, "CourierCompanyVat");
            mySWSHAWBResponse.CourierHawbNumber = (string)GetXElement(myXElementSWSHAWBResponse, "CourierHawbNumber");
            mySWSHAWBResponse.StatusCode = (string)GetXElement(myXElementSWSHAWBResponse, "StatusCode");
            mySWSHAWBResponse.ErrorCode = (string)GetXElement(myXElementSWSHAWBResponse, "ErrorCode");
            mySWSHAWBResponse.ErrorDescription = (string)GetXElement(myXElementSWSHAWBResponse, "ErrorDescription");

            return mySWSHAWBResponse;
        }
        //public void AnalyzeResponse(CourierWEBAPICommSettings settings, string webAPIResultString)
        //{
        //    throw new Exception("use  SetInAnalyzeQResponseService by @intrface.ResponseCode");
        //    var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
        //    var def = customsPartnerFtpDetails.GetAllInterfaceDetails().First(r => r.Code == CustomsPartnerFtpDetails.InterfaceName_ECSWSTHR_IN);
        //    var commSetting = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(settings);
        //    var analyzeQueueUtil = new AnalyzeQueueUtil();
        //    var new_analyze = analyzeQueueUtil
        //       .SaveMessageToAnalyzeQueue("", Encoding.UTF8.GetBytes(webAPIResultString), settings.Tenant,
        //       commSetting, def,
        //       new AnalyzeResultModel()
        //       {
        //           EntityID = settings.DeclarationId,
        //           ObjectTableID = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),

        //       });

        //    LogMessagingUtil.Instance.AppendLine($"new_analyze  CommunicationLogId = {new_analyze.CommunicationLogId}");

        //}
    }


}
