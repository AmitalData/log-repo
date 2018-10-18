using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure.Azure;
//using CommunicationWorkerRole.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
//using CustomsWorkerRole.Queue;
using UnifreightIIG.Common.Utils;
using Logitude.Server.Tools;
//using Logitude.CustomsMessaging.MessagingServices;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.Messaging.Customs;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
//using CustomsWorkerRole.Utils;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.BL.Utils;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.AccountingWebServices.Testers
{
    public partial class RevaluationWorkerRoleTester : System.Web.UI.Page
    {
        private QueueDescription _QueueDescription;
        private QueueClient _QueueClient;
        protected void Page_Load(object sender, EventArgs e)
        {

            var myClass = this.GetType().Name;
            myClass = "RevaluationWorkerRole";
            string emailQueueName = ///GetQueueByEnviroment(myClass); //Amitalqueue
            WebFreightEntryPoint.GetQueueByEnviroment(myClass);
            //_QueueDescription = new QueueDescription(emailQueueName);
            //_QueueDescription.MaxSizeInMegabytes = 5120;
            //_QueueDescription.LockDuration = TimeSpan.FromMinutes(5);//due debug + raise after 5 min !!
            ////_QueueDescription.MaxDeliveryCount = 100;
            //_QueueDescription.MaxDeliveryCount = 20;

            //StorageAcountDetails.NameSpaceManager.CreateQueue(_QueueDescription);

            _QueueClient = StorageAcountDetails.CreateServiceBusQueueClient(emailQueueName);
            BrokeredMessage receivedMessage = null;
            try
            {
                receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(5));
            }
            catch (Exception)
            {

                throw;
            }
            if (receivedMessage == null)
            {
                return;
            }
            ProcessMessage(receivedMessage);
            //var RevaluationWorkerRole = new RevaluationWorkerRole();
            //RevaluationWorkerRole.WorkOnce();
        }
        public static string GetQueueByEnviroment(string queueName)
        {

            return Simplog.Server.Infrastructure.WebFreightEntryPoint.GetQueueByEnviroment(queueName);

        }
        private void ProcessMessage(BrokeredMessage message)
        {

            try
            {



                var tenant = message.GetProperty<int>(QueueExt.QueuePropertyNames.Tenant, -1);
                if (tenant == -1)
                {
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "RevaluationWorkerRole: ProcessMessage() Method :tenant==-1", null);
                    return;
                }
                var correlationId = message.CorrelationId;
                LogMessagingUtil.Instance.AppendLine("receivedMessage.DeliveryCount =" + message.DeliveryCount.ToString());


                RevaluationBatch revaluationBatch = new RevaluationBatch();
                revaluationBatch.RunOneRevaluation(correlationId, tenant);
                //MessagingServiceFactoryHelper.ResolveAndExecute(analyzeClass, tenant, correlationId, myCustomsCommandEnum);

                message.SafeComplete();

            }
            catch (CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException)
            {

                //ExceptionHandler.HandleException(customsRequestsSheetServiceException, DateTime.Now, 0, "", "WorkerRole", "RevaluationWorkerRole: ProcessMessage() Method/CustomsRequestsSheetServiceException ", null);
                if (customsRequestsSheetServiceException.What2Do == CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue)
                {
                    message.SafeComplete();
                }
                else
                {
                    message.SetProperty<DateTime>(QueueExt.QueuePropertyNames.LastExecAt, DateTime.UtcNow);
                    message.SafeAbandon();
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "RevaluationWorkerRole: ProcessMessage() Method", null);
                message.SafeComplete();
                //throw;
            }
        }

    }
}