using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.KafkaConfigurations;
using Logitude.Server.Tools.Messages;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;

namespace CommunicationWorkerRole
{
    class TransfareOnGoingDataWR : WorkerEntryPoint
    {
        DbQueueService queueservice;
        string queueName = "CToolLookups";

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        queueservice = new DbQueueService(queueName, 0);
                        var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));

                        if (response.MessageId != null)
                        {
                            object entityPM = GetEntityById(response);
                            long messageType = GetMessageType(response);

                            var TransfareOnGoingMessageProducer = new Producer();
                            var serializedObjectUpdateMessage = JsonConvert.SerializeObject(entityPM, Formatting.Indented);
                            var result = TransfareOnGoingMessageProducer.Produce(KafkaTopics.LookupsTopic, messageType, serializedObjectUpdateMessage);
                            queueservice.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        queueservice.CompleteAsFailed();
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "CToolLookups worker role start", null, null);
                        Thread.Sleep(10000);
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
            ConnectClient();

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CToolLookups";

            return base.OnStart();
        }

        public void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService(queueName, 0);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "Connect client method", null, null);
            }
        }



        #region Private Methods

        private object GetEntityById(QueueResponse response)
        {
            int Tenant = int.Parse(response.MessageValues["Tenant"].ToString());
            string Entity = response.MessageValues["Entity"].ToString();
            string EntityId = response.MessageValues["EntityId"].ToString();

            switch (Entity)
            {
                case "Card":
                    return GetCardById(Tenant, EntityId);
                case "Contact":
                    return GetContactById(Tenant, EntityId);
                case "Country":
                    return GetCountryById(Tenant, EntityId);
                case "Port":
                    return GetPortById(Tenant, EntityId);
                default:
                    return null;
            }
        }

        private CardPM GetCardById(int Tenant, string Id)
        {
            CardQuery cardQuery = new CardQuery(Tenant);
            CardPM cardPM = cardQuery.GetSinglePM(Id, Tenant);
            return cardPM;
        }

        private ContactPM GetContactById(int Tenant, string Id)
        {
            ContactQuery contactQuery = new ContactQuery(Tenant);
            ContactPM contactPM = contactQuery.GetSinglePM(Id, Tenant);
            return contactPM;
        }

        private CountryPM GetCountryById(int Tenant, string Id)
        {
            CountryQuery countryQuery = new CountryQuery(Tenant);
            CountryPM countryPM = countryQuery.GetSinglePM(Id, Tenant);
            return countryPM;
        }

        private PortPM GetPortById(int Tenant, string Id)
        {
            PortQuery portQuery = new PortQuery(Tenant);
            PortPM portPM = portQuery.GetSinglePM(Id, Tenant);
            return portPM;
        }

        private long GetMessageType(QueueResponse response)
        {
            string Entity = response.MessageValues["Entity"].ToString();
            switch (Entity)
            {
                case "Card":
                    return 4;
                case "Contact":
                    return 5;
                case "Country":
                    return 7;
                case "Port":
                    return 6;
                default:
                    return 0;
            }
        }

        #endregion
    }
}
