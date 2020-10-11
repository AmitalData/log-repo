using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.CustomsMessaging.Utils;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniDecReAnalysisBatchSend9999_MsgResponseService : ResponseServiceBase
        //<TResponseData, TCustomResponse, TRequestParams>
        <INF_MSG_GenericResponseData, DCAInUCB9999WithResponseContentHeader, GenericRequestParams>
    {
         public override INF_MSG_GenericResponseData GetResponse(DCAInUCB9999WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
                       return this.MyResponseData;
 
        }

        public override void Update(DCAInUCB9999WithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new CustomsCollateralsAnswerQueryService(context);
            var myDeclarationUpdateService = new CustomsCollateralsAnswerUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CustomsCollateralsAnswer");
            //var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
           var qs = new CustomsRequestsSheetQueryService(context);

            //List<DeclarationCourierStatusPM> listPM = new List<DeclarationCourierStatusPM>();
            var repo = new CustomsCollateralsAnswerRepository(context);
            List<CustomsCollateralsAnswer> listPoco = new List<CustomsCollateralsAnswer>();

            foreach (var item in customResponse.RequestsList)
            {
                var request = qs.GetSingle(item,false,false);

                try
                {
                    using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                    {
                        
                        try
                        {

                            MessagingServiceFactoryHelper.ResolveAndReQueue(request.InterfaceTypeCode, request.Tenant, request.Id);
                            mess = null;
                        }
                        catch (System.Exception e)
                        {
                            mess.AppendLine(e.Message);
                            scopeNewCRS.Complete();
                            continue;
                        }


                        scopeNewCRS.Complete();
                    }

          
                }
                catch (System.Exception ee1)
                {

                    LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({request.Id}) : {ee1.Message}");
                    mess.AppendLine($"Exception!!!CreateSheetSBQMessage({request.Id}) : {ee1.Message}");
                    
                    continue;
                }

           
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();

            //this.MyRequestSheetParam.RequestDescription = "Build Custom Zip File";
            ///LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject(false, true);

            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;

        }


        private static void CreateCRS9999_Update2InProgress(DCAInUCB9999WithResponseContentHeader customResponse, GenericRequestParams requestParams, StringBuilder mess, string objectTableId, string objectTableIdCourierMaster, List<CustomsCollateralsAnswer> listPoco, List<MyDTO> listCustomsCollateralsAnswer)
        {
 

            var realUpdatedList = new List<string>();
            //foreach (var itemPoco in customResponse)
            //{


            //    try
            //    {
            //        using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
            //        {
            //            var requestParams8212 = new GenericRequestParams()
            //            {
            //                Tenant = requestParams.Tenant,
            //                 LoggingEnabled = true,
            //                LoggingObjectTableId = objectTableId,
            //                LoggingEntityId = itemPoco.CustomsCollateralId,
            //                LoggingObjectTableId2 = requestParams.LoggingObjectTableId,
            //                LoggingEntityId2 = objectTableIdCourierMaster,
            //                AppicationId = itemPoco.CustomsCollateralId,
            //                InterfaceTypeCode = "8212",
            //                //LoggingEntityReference = declarationNumber,
            //                LoggingUserId = requestParams.LoggingUserId,
            //                RequestVIA = SendRequestVIA.WebServiceBatch,
 
            //            };

            //            SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams8212, false);
            //            LogMessagingUtil.Instance.AppendLine($" CreateSheetSBQMessage({itemPoco.CustomsCollateralId})");
            //            mess.AppendLine($" CreateSheetSBQMessage({itemPoco.CustomsCollateralId})");

            //            scopeNewCRS.Complete();
            //        }


            //        realUpdatedList.Add(itemPoco.CustomsCollateralId);

            //    }
            //    catch (System.Exception ee1)
            //    {

            //        LogMessagingUtil.Instance.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPoco.CustomsCollateralId}) : {ee1.Message}");
            //        mess.AppendLine($"Exception!!!CreateSheetSBQMessage({itemPoco.CustomsCollateralId}) : {ee1.Message}");
            //    }
            //}

            realUpdatedList.ChunkBy(100)
    .ForEach(list100 =>
    {
        string inList = String.Join(",", list100.Select(declarationId => $"'{declarationId}'").ToArray());
        string updateSql = $"Update DeclarationCourierStatuses set COURIERPAYMENTSTATUSCODE='I' where DECLARATIONID in ({inList}) ";

        CustomContext.CommandExecuteNonQuery(requestParams.Tenant, updateSql);
    });
        }

    }
}
