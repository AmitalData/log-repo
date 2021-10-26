using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.Def.Messaging.Customs;

namespace Logitude.Customs.BL.Messaging.Customs
{
    public class SBQMessageService
    {


        public static string CreateSheetSBQMessage<TRequestParams>
            (TRequestParams requestParams, bool isInteractive, 
            DateTime? execTime = null)
            where TRequestParams : RequestParamsBase
        {

            if (requestParams is DeclarationStatusRequestParams)
            {

                LogitudeSettings.HandleLogMe(
                    "DeclarationId:" + requestParams.LoggingEntityId + Environment.NewLine + Environment.StackTrace.ToString()
                    , false, "8250", new DateTime(2021, 1, 1));
            }

            CustomsRequestsSheetDomainModelService<TRequestParams> customsRequestsSheetService = null;
            var reqSheetDetials = CustomsRequestsSheetDomainModelService<TRequestParams>.GetSheetDetailsFromRequestParam(requestParams);
            string customsRequestsSheetId = null;
            try
            {
                CustomsRequestsSheetDomainModelService<TRequestParams>.CreateNew(requestParams, reqSheetDetials, isInteractive, out customsRequestsSheetService);
                var tenant = customsRequestsSheetService.MyCustomsRequestsSheetPM.Tenant;
                var InterfaceTypeCode = customsRequestsSheetService.MyCustomsRequestsSheetPM.InterfaceTypeCode;
                var MyCustomsRequestsSheetPMId = customsRequestsSheetService.MyCustomsRequestsSheetPM.Id;
                LogMessagingUtilWR.Instance.AppendLine($"SetCustomsRequestsSheetId({MyCustomsRequestsSheetPMId})");
                if (LogitudeSettings.QueueServiceMode != "db" && Transaction.Current != null)
                {
                    Transaction.Current.TransactionCompleted += (sender, e) =>
                    {
                        SBQMessageService.CreateBasic<CustomsCommandEnum>(
                            CustomsCommandEnum.CustomsCommandGetCustomRequestWR,
                            tenant,
                            InterfaceTypeCode,
                            MyCustomsRequestsSheetPMId);
                    };
                }
                else
                {
                    SBQMessageService.CreateBasic<CustomsCommandEnum>(
                        CustomsCommandEnum.CustomsCommandGetCustomRequestWR,
                        tenant,
                        InterfaceTypeCode,
                        MyCustomsRequestsSheetPMId, execTime);
                }
                customsRequestsSheetId = customsRequestsSheetService.MyCustomsRequestsSheetPM.Id;
            }
            finally
            {
                if (customsRequestsSheetService != null)
                {
                    customsRequestsSheetService.Dispose();
                }
                RequestSheetContext.Current.Dispose();
                LogMessagingUtil.Instance.Clear();
            }

            return customsRequestsSheetId;

        }





        public static void CreateBasic<TEnum>(TEnum SBQueueName, int tenant, string interfaceTypeCode, string correlationId,DateTime? execTime =null)
            where TEnum : struct, IConvertible
        {

            TimeSpan? Delay=null;
            
            if (execTime.HasValue )
            {
                var srverTime = (new DualQueryService(AmitalContext.GetContext(tenant))).GetServerDateTime();
                //if (execTime.GetValueOrDefault()> srverTime.GetValueOrDefault())
               //{
                    Delay = execTime.GetValueOrDefault().Subtract(srverTime.GetValueOrDefault());
               // }
            }

            var queueSendModel = new QueueSendModel()
            {
                InterfaceTypeCode = interfaceTypeCode,
                //DebugMode = false,
                Tenant = tenant,
                Delay = Delay

            };
            CreateBasic<TEnum>(SBQueueName, correlationId, queueSendModel);

        }


        public static void CreateBasic<TEnum>(TEnum SBQueueName, string correlationId, QueueSendModel queueSendModel)
            where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            if (typeof(TEnum) != typeof(SBQueueNames))
            {
                if (typeof(TEnum) != typeof(CustomsCommandEnum))
                {
                    throw new ArgumentException("SBQMessageService.CreateBasic<TEnum> TEnum must be in (CustomsCommandEnum , SBQueueNames) ");
                }
            }
            if (LogitudeSettings.QueueServiceMode != "db")
            {
                if (Transaction.Current == null)
                {
                    using (var scope = TransactionFactory.GetNewSerializableTransaction())
                    {
                        SendImmdiatlly<TEnum>(SBQueueName, correlationId, queueSendModel);
                        scope.Complete();
                    }

                    //throw new Exception("Transaction.Current == null");
                }
                else
                {
                    Transaction.Current.TransactionCompleted += (sender, e) =>
                    {
                        SBQueueName = SendImmdiatlly<TEnum>(SBQueueName, correlationId, queueSendModel);
                    };
                }
                return;
            }

            using (TransactionScope scope =
                //(LogitudeSettings.QueueServiceMode != "db") ? TransactionFactory.GetNewSerializableTransaction() :TransactionFactory.GetTransaction())
                TransactionFactory.GetTransaction())
            {

                var queueSendService = new Logitude.Server.Tools.QueueService.QueueSendService(SBQueueName.ToString(), correlationId, queueSendModel);
                //queueSendService.InterfaceTypeCode = interfaceTypeCode;//this.GetType().FullName;
                //queueSendService.DebugMode = true;
                //queueSendService.Tenant = tenant;

                ///queueSendService.ProcessState = (int)CustomsRequestStepEnum.StartRequestParams;
                queueSendService.Send();
                scope.Complete();
            }
        }

        private static TEnum SendImmdiatlly<TEnum>(TEnum SBQueueName, string correlationId, QueueSendModel queueSendModel) where TEnum : struct, IConvertible
        {
            var queueSendService = new Logitude.Server.Tools.QueueService.QueueSendService(SBQueueName.ToString(), correlationId, queueSendModel);

            queueSendService.Send();
            return SBQueueName;
        }

#if false
        private void CreateNewOutRequestSheet1(CustomsRequestsSheetPM customsRequestsSheetPm, string selectedFile)
        {


            //Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.Clear();
            if (MyDocument == null)
            {
                throw new Exception("Please CreateNewComm Or Init It Before CreateNewRequestSheet");
            }
            if (customsRequestsSheetPm == null)
            {
                throw new Exception("Please Insert customsRequestsSheetPm with override data ");
            }


            //byte[] messageBytes = Encoding.ASCII.GetBytes(messageData);
            //string mmm = Encoding.ASCII.GetString(messageBytes);
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("selectedFile =" + selectedFile);
            string externalId = "";// GetExternalId(selectedFile);

            //string correlationId = "";
            //var customsRequestsSheetPm = new CustomsRequestsSheetPM();
            var currentContext = CustomContext.GetContext(MyCommunicationLog.Tenant);
            ///var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(currentContext);
            var customsRequestsSheetUpdateService = new CustomsRequestsSheetUpdateService(currentContext, new Dictionary<string, IContext>(), MyCommunicationLog.Tenant);

            //if (!String.IsNullOrWhiteSpace(externalId))
            //{
            //    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("externalId (our ref ) =" + externalId);
            //    customsRequestsSheetPm = customsRequestsSheetQueryService.GetSingle(externalId, true, false);
            //    if (customsRequestsSheetPm != null)
            //    {
            //        if (customsRequestsSheetPm.CorrelationId != correlationId)
            //        {
            //        }
            //    }
            //    else
            //    {
            //        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("externalId not find ---Customs Push ???" + externalId);
            //        /// if null arrived from custom Push !!!!
            //    }
            //    //customsRequestsSheetPm.ChangeSetOp = ChangeSetOperation.Update;
            //}
            //if (customsRequestsSheetPm == null)
            //{
            //    customsRequestsSheetPm = new CustomsRequestsSheetPM()
            //    {

            //        Tenant = MyCommunicationLog.Tenant,
            //        ChangeSetOp = ChangeSetOperation.Insert
            //    };



            //}
            if (customsRequestsSheetPm.Tenant != MyCommunicationLog.Tenant)
            {
            }
            //customsRequestsSheetPm.CorrelationId = correlationId;
            //customsRequestsSheetPm.IsDCA = true;//overide ??
            //customsRequestsSheetPm.RequestDescription = selectedFile.Substring(0, 119);
            customsRequestsSheetPm.RequestOwnerId = MyCommunicationLog.CreatedByUserId ;
            customsRequestsSheetPm.RequestComminicationId  = MyCommunicationLog.Id;

            //customsRequestsSheetPm.AnswerComminicationId 
            customsRequestsSheetUpdateService.Update(customsRequestsSheetPm, true);


            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("DcaService  analyzeQueue add ");


        }



        public Document MyDocument { get; set; }
        public CustomsRequestsSheetPM MyCustomsRequestsSheetPM { get; set; }
        public CommunicationLog MyCommunicationLog { get; set; }

        
#endif
    }

}
