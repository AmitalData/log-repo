using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
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
                case "Vessel":
                    return GetVesselById(Tenant, EntityId);
                case "DocumentType":
                    return GetDocumentTypeById(Tenant, EntityId);
                case "Currency":
                    return GetCurrencyById(Tenant, EntityId);
                case "EntityStatus":
                    return GetEntityStatusById(Tenant, EntityId);
                case "SpecialServicesType":
                    return GetSpecialServicesTypeById(Tenant, EntityId);
                case "PackageType":
                    return GetPackageTypeById(Tenant, EntityId);
                default:
                    return null;
            }
        }

        private object GetPackageTypeById(int tenant, string id)
        {
            PackageTypeQuery packageTypeQuery = new PackageTypeQuery(tenant);
            PackageTypePM packageTypePM = packageTypeQuery.GetSinglePM(id, tenant);
            return packageTypePM;
        }

        private object GetDocumentTypeById(int tenant, string id)
        {
            DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
            DocumentTypePM documentTypePM = documentTypeQuery.GetSingelDocumentTypeById(id, tenant);
            return documentTypePM;
        }

        private object GetVesselById(int Tenant, string Id)
        {
            VesselQuery vesselQuery = new VesselQuery(Tenant);
            VesselPM vesselPM = vesselQuery.GetSinglePM(Id, Tenant);
            return vesselPM;
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

        private CurrencyPM GetCurrencyById(int tenant, string Id)
        {
            CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
            CurrencyPM currencyPM = currencyQuery.GetSinglePM(Id,tenant);
            return currencyPM;
        }
        private EntityStatusPM GetEntityStatusById(int tenant, string Id)
        {
            EntityStatusQuery entityStatusQuery = new EntityStatusQuery(tenant);
            EntityStatusPM entityStatusPM = entityStatusQuery.GetSinglePM(Id, tenant);
            return entityStatusPM;
        }
        private SpecialServicesTypePM GetSpecialServicesTypeById(int tenant, string Id)
        {
            SpecialServicesTypeQuery specialServicesTypeQuery = new SpecialServicesTypeQuery(tenant);
            SpecialServicesTypePM specialServicesTypePM = specialServicesTypeQuery.GetSinglePM(Id, tenant);
            return specialServicesTypePM;
        }

        private long GetMessageType(QueueResponse response)
        {
            string Entity = response.MessageValues["Entity"].ToString();
            switch (Entity)
            {
                case "Card":
                    return KakaMessageTypes.Card;
                case "Contact":
                    return KakaMessageTypes.Contact;
                case "Country":
                    return KakaMessageTypes.Country;
                case "Port":
                    return KakaMessageTypes.Port;
                case "Vessel":
                    return KakaMessageTypes.Vessel;
                case "DocumentType":
                    return KakaMessageTypes.DocumentType;
                case "Currency":
                    return KakaMessageTypes.Currency;
                case "EntityStatus":
                    return KakaMessageTypes.EntityStatus;
                case "SpecialServicesType":
                    return KakaMessageTypes.SpecialServicesType;
                case "PackageType":
                    return KakaMessageTypes.PackageType;
                default:
                    return 0;
            }
        }

        #endregion
    }
}
