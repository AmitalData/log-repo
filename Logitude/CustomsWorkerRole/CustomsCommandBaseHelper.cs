
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
//using Microsoft.WindowsAzure.ServiceRuntime;
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
using Logitude.Server.Tools.QueueService;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Logitude.Server.Tools.Utils;
using System.Configuration;
using Logitude.Customs.BL.Messaging.Customs.PerformanceLogger;
using Logitude.CustomsMessaging.RabbitMQ;
using Logitude.Customs.BL.CloseTables;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Newtonsoft.Json;
using System.Globalization;

namespace CustomsWorkerRole
{
    public partial class CustomsCommandBaseHelper
    {
        public CustomsCommandBaseHelper()
        {

        }
       
        public void RunTask(CustomDBQueueMessage item, string myClass)
        {
            using (TransactionScope Queue_scope = TransactionFactory.GetTransaction())
            {
                LogMessagingUtilWR.Instance.AppendLine("ProcessMessage_Db");
                CustomDbQueueService _CustomDbQueueService = new CustomDbQueueService(myClass, 0, item);
                bool successProcessMessage = true;

                successProcessMessage = ProcessMessage_Db(item, myClass);

                LogMessagingUtilWR.Instance.AppendLine("successProcessMessage");
                if (successProcessMessage)
                {
                    _CustomDbQueueService.SafeComplete();
                    Queue_scope.Complete();
                    PerformanceM.LastInstance.QueueSuccessComplete = true;
                }
                else if (!successProcessMessage)/// IF FAILED USE NEW TRANS !!!!
                {
                    try
                    {
                        Queue_scope.Complete();
                    }
                    catch (Exception)
                    {
                        //throw;
                    }
                    try
                    {
                        Queue_scope.Dispose();//remove lock !!
                    }
                    catch (Exception)
                    {

                    }

                    using (var Abandon_Queue_scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        _CustomDbQueueService.SafeAbandon();//if (CurrentCustomQueueResponse.Retries > 10)
                        Abandon_Queue_scope.Complete();
                    }
                }
                PerformanceM.LastInstance.QueueEndDate = DateTime.Now;
                PerformanceM.EnqueueLastInstance();

                string logItMessagingUtilWR = ConfigurationManager.AppSettings.Get("LogMessagingUtilWR");

                if (!string.IsNullOrWhiteSpace(logItMessagingUtilWR))
                {
                    string morethan = "";
                    string str = LogMessagingUtilWR.Instance.GetString(out morethan);
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug(str + "_" + morethan);
                }
            }
        }

      

        protected virtual bool ProcessMessage_Db(CustomDBQueueMessage msgResponse, string myClass)
        {
            LogMessagingUtilWR.Instance.AppendLine("ProcessMessage_Db");
            try
            {
                //LogTime("start ProcessMessage_Db MessageId:" + msgResponse.MessageId + " at : ");

                int tenant = -1;
                string analyzeClass = msgResponse.Properties["InterfaceTypeCode"].ToString();




                if (String.IsNullOrWhiteSpace(analyzeClass))
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError("analyzeClass is null");
                    //_CustomDbQueueService.SafeAbandon();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :analyzeClass ==null", null);
                    //message.DeadLetter();
                    return false;//
                }

                //if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(analyzeClass))
                //{
                //    //_CustomDbQueueService.SafeAbandon();
                //    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + analyzeClass, null);
                //    //message.DeadLetter();
                //    return false;
                //}
                //var tenant = message.GetProperty<int>(QueueExt.QueuePropertyNames.Tenant, -1);
                int.TryParse(msgResponse.Properties["Tenant"].ToString(), out tenant);
                if (tenant == -1)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError("Tenant is null");
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :tenant==-1", null);
                    return false;
                }

                string correlationId = msgResponse.Properties["CorrelationId"].ToString();
                LogMessagingUtilWR.Instance.AppendLine($"correlationId = {correlationId};analyzeClass={analyzeClass}");
                // var correlationId = message.CorrelationId;
                //LogMessagingUtil.Instance.AppendLine("receivedMessage.DeliveryCount =" + message.DeliveryCount.ToString());

                var s = myClass;
                CustomsCommandEnum myCustomsCommandEnum;
                if (Enum.TryParse<CustomsCommandEnum>(s, out myCustomsCommandEnum))
                {
                    //myCustomsCommandEnum
                }
                else
                {
                    throw new Exception("Enum.TryParse<CustomsCommandEnum>(s, out myCustomsCommandEnum)");
                }
                PerformanceM.LastInstance.InterfaceTypeCode = analyzeClass;
                PerformanceM.LastInstance.RequestSheetID = correlationId;
                PerformanceM.LastInstance.QueueDefinitionCode = myCustomsCommandEnum.ToString();
                LogMessagingUtilWR.Instance.AppendLine("ResolveAndExecute");
                try
                {
                    QueueThreadStateService.Upsert(
        QueueThreadStateService.GetWRKey(s),
        $"Interface:{analyzeClass},RequestSheetID:{correlationId},QId:{msgResponse?.MessageId},QDefinition:{PerformanceM.LastInstance?.QueueDefinitionCode}"
        );

                }
                catch //(Exception)
                {


                }

                MessagingServiceFactoryHelper.ResolveAndExecute(analyzeClass, tenant, correlationId, myCustomsCommandEnum);

                //LogTime("end ProcessMessage_Db MessageId:" + msgResponse.MessageId + " at : ");


                //_CustomDbQueueService.SafeComplete();
                return true;

            }
            catch (CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException)
            {

                //ExceptionHandler.HandleException(customsRequestsSheetServiceException, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method/CustomsRequestsSheetServiceException ", null);

                if (customsRequestsSheetServiceException.What2Do == CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue)
                {
                    //message.SafeComplete();
                    //_CustomDbQueueService.SafeComplete();
                    return true;
                }
                else
                {
                    //_CustomDbQueueService.SafeAbandon();
                    return false;
                }


            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method", null);
                //message.SafeComplete();
                //_CustomDbQueueService.SafeComplete();

                //_CustomDbQueueService.SafeAbandon();// make try (in 5101 CRS was analyze *1000000)
                return false;
                //throw;
            }
        }


    }


}

        

       
 

