using CommunicationWorkerRole.LoginServiceReference;
using CommunicationWorkerRole.ExternalTasksQueueWcfService;
using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using WebFreight.Web.Helpers;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace CommunicationWorkerRole
{
    public class ContainerStatusesReceiverWR : WorkerEntryPoint
    {
        private int logitudeOceanInsightsTenant;
        private string amitalCloudEnvironmentURL;
        private string amitalCloudLogitudeTenantPrimaryKey;

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        //this.GetLogitudeOceanInsightsTenantConfigurations();
                        //this.ReadContainerStatusRequestToOceanInsightSevice();
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "ContainerStatusesReceiverWR : Run() Method", null);
                        Thread.Sleep(5000);
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private void GetLogitudeOceanInsightsTenantConfigurations()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                SettingRepository settingRepository = new SettingRepository();
                Setting setting = settingRepository.GetSingleSetting("1");
                if (setting != null)
                {
                    logitudeOceanInsightsTenant = setting.OITenantNumber;
                    amitalCloudEnvironmentURL = setting.AmitalCloudEnvironmentURL;
                    amitalCloudLogitudeTenantPrimaryKey = setting.AmitalCloudLogitudeTenantPrimaryKey;
                }
                scope.Complete();
            }
        }

        private void ReadContainerStatusRequestToOceanInsightSevice()
        {
            var token = LoginToCloud();
            if (!string.IsNullOrEmpty(token))
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                binding.MaxBufferSize = 2147483647;
                binding.MaxReceivedMessageSize = 2147483647;
                binding.ReaderQuotas.MaxStringContentLength = 2147483647;
                binding.ReaderQuotas.MaxArrayLength = 2147483647;
                var endpoint = new EndpointAddress(amitalCloudEnvironmentURL);
                ExternalTasksQueueWcfServiceClient externalTasksQueueWcfService = new ExternalTasksQueueWcfServiceClient(binding, endpoint);
                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)externalTasksQueueWcfService.InnerChannel))
                {
                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);
                    var oceanInsightResponseXML = externalTasksQueueWcfService.GetTaskFromQueue(1, 1);
                    if (!string.IsNullOrEmpty(oceanInsightResponseXML))
                    {
                        // Read the resopnse to our DB 
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                throw new Exception("Analyzing containetr status request faild, invalid token");
            }
        }

        private string LoginToCloud()
        {
            string token = "";
            LoginWcfServiceClient loginService = new LoginWcfServiceClient();
            var aPICredentialsParameters = new APICredentialsParameters()
            {
                PrimaryKey = this.amitalCloudLogitudeTenantPrimaryKey,
                Tenant = this.logitudeOceanInsightsTenant
            };
            Logitude.Server.Tools.Response loginResponse = loginService.LoginByCredential(null, aPICredentialsParameters);
            if (!loginResponse.HasError)
            {
                token = loginResponse.Result;
            }
            return token;
        }


        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ContainerStatusesReceiverWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                e.Cancel = true;
            }
        }
    }
}
