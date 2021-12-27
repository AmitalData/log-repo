using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System.Threading;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Transactions;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using WebFreight.Web.Helpers;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.WindowsAzure.Storage;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.ShipmentsModel;
using System.Security.Cryptography;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.SystemLogs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using CHAMP;
using Logitude.XSD.Analyzers.CHAMPAnalyzer;
using Logitude.BL.CommonDataModel.DataContracts;
using Logitude.Server.Tools;
using Logitude.BL.Security;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace CommunicationWorkerRole
{
    class EntityExternalUpdateWorkerRole : WorkerEntryPoint
    {
        AnalyzeQueueRepository analyzeQueueRepository;
        AnalyzeQueue analyzeQueue;
        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        analyzeQueueRepository = new AnalyzeQueueRepository();
                        analyzeQueue = analyzeQueueRepository.GetOpenAnalyzeQueue("EntityExternalUpdate");
                        LastActivity = DateTime.UtcNow;
                        if (analyzeQueue != null)
                        {
                            try
                            {
                                this.StartProcess();

                            }

                            catch (Exception ex)
                            {
                                this.OnCatchAnalyzingError(ex);
                                break;
                            }

                            
                        }

                        else
                        {
                            Thread.Sleep(3000);
                        }
                    }

                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "EntityExternalUpdateWorkerRole : Run() Method", null);
                        Thread.Sleep(5000);
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "MessageAnalyze";
            DoneItemsInRange = new Dictionary<DateTime, int>();
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


        private void OnCatchAnalyzingError(Exception ex )
        {
            ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "EntityExternalUpdateWorkerRole : Run() Method", null);

            analyzeQueue.ErrorMessage = ex.Message + (ex.InnerException != null ? Environment.NewLine + "InnerException: " + ex.InnerException.Message : "");
            analyzeQueue.StackTrace = (ex.StackTrace != null ? Environment.NewLine + "Stack Trace: " + ex.StackTrace : "");
            analyzeQueue.ErrorMessage = analyzeQueue.ErrorMessage.Length > 7950 ? analyzeQueue.ErrorMessage.Substring(0, 7950) : analyzeQueue.ErrorMessage;
            analyzeQueue.StackTrace = analyzeQueue.StackTrace.Length > 7950 ? analyzeQueue.StackTrace.Substring(0, 7950) : analyzeQueue.StackTrace;

            if (ex.Message.StartsWith("--"))
            {
                analyzeQueue.Status = "F";
            }

            else
            {
                analyzeQueue.Retries++;

                if (analyzeQueue.Retries >= 5)
                {
                    analyzeQueue.Status = "F";

                }
            }

            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
                analyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(analyzeQueue.Tenant);
                analyzeQueueRepository.Update(analyzeQueue);
                analyzeQueueRepository.SubmitChanges();

              //  scope.Complete();
            //}




        }

        private void StartProcess()
        {
            byte[] data = analyzeQueue.MessageBody;
            if (data != null)
            {
                EntityExternalUpdate entityExternalUpdate = LogitudeXmlSerializer.DeserializeObject<EntityExternalUpdate>(data);
                if (entityExternalUpdate != null && entityExternalUpdate.Entity =="Shipment")
                {
                    ShipmentQuery shipmentQuery = new ShipmentQuery(entityExternalUpdate.Tenant);
                    ShipmentPM entityPM = shipmentQuery.GetSingleShipmentPMByNumber(entityExternalUpdate.ShipmentNumber, entityExternalUpdate.Tenant);
                    bool isChange = false;
                    if (entityPM != null)
                    {
                        foreach (Status legStatus in entityExternalUpdate.Statuses)
                        {

                            if (legStatus.Code == "ARR")
                            {
                                //MainCarriage
                                if (entityPM.MainCarriageToPortCode == legStatus.PortCode && entityPM.MainCarriageToPortCountryCode == legStatus.PortCountry)
                                {
                                    entityPM.MainCarriageATA = legStatus.Date;
                                    isChange = true;
                                }
                                //Transshipment1
                                else if (entityPM.Transshipment1ToPortCode == legStatus.PortCode && entityPM.Transshipment1ToPortCountryCode == legStatus.PortCountry)
                                {
                                    entityPM.Transshipment1ATA = legStatus.Date;
                                    isChange = true;
                                }
                                //Transshipment2
                                else if (entityPM.Transshipment2ToPortCode == legStatus.PortCode && entityPM.Transshipment2ToPortCountryCode == legStatus.PortCountry)
                                {
                                    entityPM.Transshipment2ATA = legStatus.Date;
                                    isChange = true;
                                }
                                //Transshipment3
                                else if (entityPM.Transshipment3ToPortCode == legStatus.PortCode && entityPM.Transshipment3ToPortCountryCode == legStatus.PortCountry)
                                {
                                    entityPM.Transshipment3ATA = legStatus.Date;
                                    isChange = true;
                                }
                            }
                        }

                        if (isChange)
                        {
                            IShipmentsContext objectContext = ShipmentsContext.GetContext(entityPM.Tenant);
                            ShipmentService service = new ShipmentService(objectContext, entityPM, "system@tenant" + entityPM.Tenant + ".com");
                            service.Update();

                            using (TransactionScope scope = TransactionFactory.GetTransaction())
                            {
                                string email = "system@tenant" + entityPM.Tenant + ".com";
                                ContactQuery contactQuery = new ContactQuery(entityPM.Tenant);
                                ContactPM loggedContact = contactQuery.GetContactByNameAndTenant(email, entityPM.Tenant, true);
                                if (loggedContact == null)
                                {
                                    loggedContact = contactQuery.GetContactByEmailOnly(email, entityPM.Tenant);
                                }
                                ObjectTableQuery tablesQuery = new ObjectTableQuery(entityPM.Tenant);
                                ObjectTablePM table = tablesQuery.GetObjectTableByName("Shipment", 0);
                                CommunicationsParams logParams = new CommunicationsParams()
                                {
                                    Tenant = entityPM.Tenant,
                                    From = "Agent",
                                    To = "Agent",
                                    CommunicationLogTypeCode = "Q",
                                    Priority = 1,
                                    InOut = "I",
                                    Status = "D",
                                    LoggingUserId = loggedContact!=null ?loggedContact.Id:"",
                                    LoggingObjectTableId = table.Id,
                                    LoggingEntityId = entityPM.Id,
                                    Subject = "Status Update",
                                    FolderName = "AgentsSharedLogisticsQueue",
                                    ByteData = data,
                                };
                                Communications.AddCommunicationLog(logParams);
                                scope.Complete();
                            }
                        }
                    }
                }
         
            }

            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
                analyzeQueue.Status = "D";
                analyzeQueue.ErrorMessage = null;
                analyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(analyzeQueue.Tenant);
                analyzeQueueRepository.Update(analyzeQueue);
                analyzeQueueRepository.SubmitChanges();
            //    scope.Complete();
           // }
    

        }


    }
}
