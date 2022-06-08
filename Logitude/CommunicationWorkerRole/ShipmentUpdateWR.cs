using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.KafkaConfigurations;
using Logitude.Server.Tools.Messages;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace CommunicationWorkerRole
{
    class ShipmentUpdateWR : WorkerEntryPoint
    {
        DbQueueService queueservice;
        string queueName = "CToolShipmentsUpdate";

        public override void Run()
        {
            var ShipmentUpdateMessageProducer = new Producer();

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
                            var entityPMString = GetSerializedExtendedShipmentById(response);
                            var result = ShipmentUpdateMessageProducer.Produce(KafkaTopics.ShipmentsUpdateTopic, KakaMessageTypes.ShipmentUpdate, entityPMString);

                            queueservice.Complete();
                        }
                        else
                        {
                            Thread.Sleep(5000);
                        }
                    }
                    catch (Exception ex)
                    {
                        queueservice.CompleteAsFailed();
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "CToolShipmentsUpdate worker role start", null, null);
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
            BatchServiceCode = "CToolShipmentsUpdate";

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
        private string GetSerializedExtendedShipmentById(QueueResponse response)
        {
            int Tenant = int.Parse(response.MessageValues["Tenant"].ToString());
            string ShipmentId = response.MessageValues["ShipmentId"].ToString();

            ShipmentQuery shipmentQuery = new ShipmentQuery(Tenant);
            ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(ShipmentId, Tenant);
            var shipmentPMString = JsonConvert.SerializeObject(shipmentPM, Formatting.Indented);
            Dictionary<string, object> shipmentPMDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(shipmentPMString);

            shipmentPMDictionary.Add("DocumentsFilingPM", GetShipmentDocumentsFilingPM(shipmentPM.ShipmentNumber, Tenant));
            return JsonConvert.SerializeObject(shipmentPMDictionary, Formatting.Indented);
        }

        private List<DocumentsFilingPM> GetShipmentDocumentsFilingPM(string shipmentNumber, int tenant)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            string entityId = shipmentQuery.GetEntitiyIdByShipmentNumber(shipmentNumber, tenant);
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            List<DocumentsFilingPM> documentsFilingPM = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(entityId, "", ObjectTableRepository.GetObjectTableByName("Shipment"), "I", tenant);
            documentsFilingPM = documentsFilingPM.Where(d => d.DocumentId != null && d.HasFile == true).ToList();

            return documentsFilingPM;
        }
        #endregion
    }

    public class ExtendedShipmentPM : ShipmentPM
    {
        public List<DocumentsFilingPM> DocumentsFilingPM { get; set; }
    }
}
