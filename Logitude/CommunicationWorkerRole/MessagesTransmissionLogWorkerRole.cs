using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Queue;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace CommunicationWorkerRole
{
    public class MessagesTransmissionLogWorkerRole : WorkerEntryPoint
    {
        string entitychangeQueueName;
        QueueDescription queueDescription;
        QueueClient client;
        public override bool OnStart()
        {
            try
            {
                entitychangeQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment("transmissionlogqueue");
                if (!StorageAcountDetails.NameSpaceManager.QueueExists(entitychangeQueueName))
                {
                    queueDescription = new QueueDescription(entitychangeQueueName);
                    queueDescription.MaxSizeInMegabytes = 5120;
                    queueDescription.EnableDeadLetteringOnMessageExpiration = false;
                    StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
                }

                client = StorageAcountDetails.CreateServiceBusQueueClient(entitychangeQueueName);
            }

            catch (Exception ex)
            {
                //  ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
            }

            ServicePointManager.DefaultConnectionLimit = 12;
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "TransmissionLog";
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

        public override void Run()
        {
            int forwarderTenant = 0;
            string transmissionLogId = string.Empty;

            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        var message = client.Receive(new TimeSpan(0, 0, 10));
                        LastActivity = DateTime.UtcNow;

                        if (message != null)
                        {
                            try
                            {
                                if (message.Properties.Keys.Contains("TransmissionLogId"))
                                {
                                    transmissionLogId = message.Properties["TransmissionLogId"].ToString();
                                }

                                if (message.Properties.Keys.Contains("Tenant"))
                                {
                                    forwarderTenant = (int)message.Properties["Tenant"];
                                }

                                if (transmissionLogId == null || forwarderTenant == null)
                                {
                                    message.Complete();
                                    continue;
                                }

                                LogitudeMessagesTransmissionLogRepository logRepository = new LogitudeMessagesTransmissionLogRepository(forwarderTenant);
                                LogitudeMessagesTransmissionLog myRecord = logRepository.GetSingleLogitudeMessagesTransmissionLog(transmissionLogId, forwarderTenant);

                                if (myRecord != null)
                                {
                                    TenantManagement airlineTenant = null;
                                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                                    {
                                        TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                                        airlineTenant = tenantManagementRepository.GetTenantManagementByConnectedArline(myRecord.AirlineCode);

                                        scope.Complete();
                                    }

                                    if (airlineTenant != null)
                                    {
                                        List<LogitudeMessagesTransmissionLog> allLogs = logRepository.GetLogitudeMessagesTransmissionLogs(airlineTenant.Id).ToList();

                                        bool exist = this.IsEntityExists(myRecord.AWBNumber, myRecord.SentDate, allLogs);

                                        if (!exist)
                                        {
                                            ParticipantRepository participantRepository = new ParticipantRepository(airlineTenant.Id);
                                            Participant myParticipant = participantRepository.GetSingleParticipantByForwarderandAirlineTenant(forwarderTenant, airlineTenant.Id);

                                            bool copySender = false;
                                            if (myParticipant != null && myParticipant.IsDirect)
                                            {
                                                copySender = true;
                                            }

                                            LogitudeMessagesTransmissionLog newLog = new LogitudeMessagesTransmissionLog()
                                            {
                                                Id = IdCounter.GetNumber("LogitudeMessagesTransmissionLog", airlineTenant.Id),
                                                Tenant = airlineTenant.Id,
                                                CCS = myRecord.CCS,
                                                AirlineCode = myRecord.AirlineCode,
                                                MessageTypeCode = myRecord.MessageTypeCode,
                                                Prefix = myRecord.Prefix,
                                                AWBNumber = myRecord.AWBNumber,
                                                HAWB = myRecord.HAWB,
                                                SentDate = myRecord.SentDate,
                                                ParticipantId = myParticipant == null ? null : myParticipant.Id,
                                                Participant = myParticipant == null ? null : myParticipant.Card.EnglishName,
                                                DirectParticipant = myRecord.DirectParticipant,
                                                IATACode = myRecord.IATACode,
                                                CASSCode = myRecord.CASSCode,
                                                UserEmail = copySender ? myRecord.UserEmail : null,
                                                UserName = copySender ? myRecord.UserName : null,
                                                Origin = myRecord.Origin,
                                                Destination = myRecord.Destination,
                                                Pieces = myRecord.Pieces,
                                                GrossWeight = myRecord.GrossWeight,
                                                GrossWeightUnitCode = myRecord.GrossWeightUnitCode,
                                                ChargeableWeight = myRecord.ChargeableWeight,
                                                ChargeableWeightUnitCode = myRecord.ChargeableWeightUnitCode,
                                                Volume = myRecord.Volume,
                                                VolumeUnitCode = myRecord.VolumeUnitCode,
                                                DescriptionOfGoods = myRecord.DescriptionOfGoods,
                                                IsUpdatedinAirlineTenant = true,
                                                SearchFields = myRecord.SearchFields,
                                            };

                                            if (copySender)
                                            {
                                                newLog.SourceTenant = forwarderTenant;
                                            }

                                            logRepository.Add(newLog);
                                            logRepository.SubmitChanges();

                                            this.AddEntityChange(newLog.Id, airlineTenant.Id);
                                        }
                                    }
                                }

                                else
                                {
                                    Thread.Sleep(60000);
                                }

                                message.Complete();
                                LogDoneItemInMemory();
                            }

                            catch (Exception ex)
                            {
                                ExceptionHandler.HandleException(ex, DateTime.Now, (int)forwarderTenant, "", "MessagesTransmissionLogWorkerRole", "", null);
                                message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.AddSeconds(5);
                                message.Abandon();
                            }
                        }                        
                    }

                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Messages Transmission Log worker role start", null, null);
                        Thread.Sleep(300000);
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private bool IsEntityExists(string master, DateTime? sentDate, List<LogitudeMessagesTransmissionLog> logs)
        {
            bool myResult = (from a in logs where a.AWBNumber == master && a.SentDate == sentDate select a).Any();
            return myResult;
        }

        private void AddEntityChange(string logId, int tenant)
        {
            LogitudeMessagesTransmissionLogQuery query = new LogitudeMessagesTransmissionLogQuery(tenant);
            LogitudeMessagesTransmissionLogPM myLog = query.GetSinglePM(logId, tenant);

            if (myLog != null)
            {
                EntityChangeHelper entityChangeHelper = new EntityChangeHelper();

                entityChangeHelper.AddEntityChange(myLog, null, "OnCreate", "", "LogitudeMessagesTransmissionLog");
            }
        }
    }
}
