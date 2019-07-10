
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CustomsWorkerRole.Queue;
using UnifreightIIG.Common.Utils;
using Logitude.Server.Tools;
using Logitude.CustomsMessaging.MessagingServices;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.Messaging.Customs;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using CustomsWorkerRole.Utils;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Data.CommonDataModel;
using Logitude.Customs.BL.EntityQueryServices;
using CustomsWorkerRole.BL;

namespace CustomsWorkerRole
{

    public class CustomsCommandAnalyzeResponseWR : CustomsCommandBase
    {
        private int _Tenant;
        protected override bool ProcessMessage_Db(Logitude.Server.Tools.QueueService.CustomDBQueueMessage message)
        {
         
            var dcaAnalyzeAggregateKey = "";
            bool success = false;
            try
            {
                dcaAnalyzeAggregateKey = message.GetProperty<string>(QueueExt.QueuePropertyNames.DcaAnalyzeAggregateKey, "");//, 
                if (String.IsNullOrWhiteSpace(dcaAnalyzeAggregateKey))
                {
                    return base.ProcessMessage_Db(message);
                }
                 
            }
            catch (Exception)
            {
                
                throw;
            }
            try
            {


                var SerialAnalyzeDcaResponseByAggregateKey = new SerialAnalyzeDcaResponseByAggregateKey(
                    message.GetProperty<int>(QueueExt.QueuePropertyNames.Tenant, -1), dcaAnalyzeAggregateKey);
                success = SerialAnalyzeDcaResponseByAggregateKey.DoSerialAnalyze();
                
                //message.SafeComplete();
                //_CustomDbQueueService.SafeComplete();
                return success;

            }
            catch (CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException)
            {

                //ExceptionHandler.HandleException(customsRequestsSheetServiceException, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method/CustomsRequestsSheetServiceException ", null);
                if (customsRequestsSheetServiceException.What2Do == CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue)
                {
                    //_CustomDbQueueService.SafeComplete();
                    return true;
                }
                else
                {
                    //message.SetProperty<DateTime>(QueueExt.QueuePropertyNames.LastExecAt, DateTime.UtcNow);
                    //message.SafeAbandon();
                    //_CustomDbQueueService.SafeAbandon();
                    return false;
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method", null);
                return false;
                
            }



        }

        protected override bool ProcessMessage(BrokeredMessage message, OverrideControllerModel controller = null)
        {

            var dcaAnalyzeAggregateKey = "";
            bool success = false;
            try
            {
                dcaAnalyzeAggregateKey = message.GetProperty<string>(QueueExt.QueuePropertyNames.DcaAnalyzeAggregateKey, "");//, 
                if (String.IsNullOrWhiteSpace(dcaAnalyzeAggregateKey))
                {
                    return base.ProcessMessage(message, controller);
                }
                 
            }
            catch (Exception)
            {
                
                throw;
            }
            try
            {
                

                //var SerialAnalyzeDcaResponseByAggregateKey = new SerialAnalyzeDcaResponseByAggregateKey(message);
                var SerialAnalyzeDcaResponseByAggregateKey = new SerialAnalyzeDcaResponseByAggregateKey(
                    message.GetProperty<int>(QueueExt.QueuePropertyNames.Tenant, -1), dcaAnalyzeAggregateKey);
                success = SerialAnalyzeDcaResponseByAggregateKey.DoSerialAnalyze();
                
                message.SafeComplete();
                return success;

            }
            catch (CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException)
            {

                //ExceptionHandler.HandleException(customsRequestsSheetServiceException, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method/CustomsRequestsSheetServiceException ", null);
                if (customsRequestsSheetServiceException.What2Do == CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue)
                {
                    message.SafeComplete();
                    return true;
                }
                else
                {
                    message.SetProperty<DateTime>(QueueExt.QueuePropertyNames.LastExecAt, DateTime.UtcNow);
                    message.SafeAbandon();
                    return false;
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method", null);
                return false;
                
            }



        }
    }
}
