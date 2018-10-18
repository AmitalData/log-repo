using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole
{
#if false
    

    public class UpdateClosedTablesWR : CustomsWorkerEntryPoint
    {
        QueueDescription queueDescription;
        QueueClient client;

        public override void Run()
        {

            while (true)
            {
                
                try
                {
                    WorkOnce();

                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "update closed tables worker role after receiving a message", null,null);
                }
            }
        }


        bool _OnStartDone = false;
        private Logitude.Customs.BL.EntityPMs.CustomsSettingPM _CustomsSetting;
        
        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true ;
                _OnStartDone = true;
                var qs = new CustomsSettingQueryService(0); ;
                _CustomsSetting =qs.GetAll().FirstOrDefault();
                string emailQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment(SBQueueNames.updateclosedtables.ToString());


                if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailQueueName))
                {
                    queueDescription = new QueueDescription(emailQueueName);
                    queueDescription.MaxSizeInMegabytes = 5120;
                    
                    StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
                }

                client = StorageAcountDetails.CreateServiceBusQueueClient(emailQueueName);

                
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "update closed tables worker role start", null, null);
            }

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {

            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {

                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }



        public override void WorkOnce()
        {

            OnStart();
            if (!General.IsUpdating())
            {
                try
                {
                   // int tenant = 0;
                    var message = client.Receive();
                    if (message != null)
                    {
                        message.Complete();
                        int tenant = (int) message.Properties["tenant"];
                        if (message.Properties["type"].ToString() == "all")
                        {
                            LoadCustomClosedTables.UpdateAllClosedTables(tenant);

                        }
                        else
                        {
                            string tableId = message.Properties["closedtableid"].ToString();
                            //LoadCustomClosedTables.UpdateSingleClosedTable(tableId);
                            var messageService = new SYSTBL_NG_9000_MSG_SystemTableRequestMessageService();
                            messageService.Send(new Logitude.CustomsMessaging.Common.RequestParams.SystemTableRequestParams()
                            {
                                TableId = tableId,
                                Tenant = tenant,//_CustomsSetting.Tenant ,
                                 RequestVIA= Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive 
                            }); 
                        }

                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "update closed tables worker role run", null, null);
                }
            }
   
        }
    }
#endif
}
