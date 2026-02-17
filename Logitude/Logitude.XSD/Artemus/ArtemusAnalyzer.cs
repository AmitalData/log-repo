using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.XSD.Artemus
{
    public partial class ArtemusAnalyzer
    {
        private AnalyzeQueue myAnalyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        CommunicationLogRepository myCommunicationLogRepository;
        private MemoryStream myMemoryStream;
        ArtemusEDIVoyageRoot VoyageRoot;
        ArtemusEDIBillRoot BillRoot;
        private IShipmentsContext myShipmentContext;
        private ShipmentCustomsTransmissionRepository shipmentCustomsRepository;
        int Tenant;

        public ArtemusAnalyzer(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
        {
            if (analyzeQueue != null)
            {
                this.Tenant = analyzeQueue.Tenant;
                this.myAnalyzeQueue = analyzeQueue;
                this.analyzeQueueRepository = analyzeQueueRepository;
                this.myCommunicationLogRepository = new CommunicationLogRepository(this.Tenant);
                this.myShipmentContext = ShipmentsContext.GetContext(this.Tenant);
            }
        }
        public void Run()
        {
            if (myAnalyzeQueue != null)
            {
                this.Deserialize();
            }
        }
        private void Deserialize()
        {
            try
            {
                this.AnalyzeData();
            }

            catch (Exception ex)
            {
                myAnalyzeQueue.Status = "F";
                myAnalyzeQueue.ErrorMessage = "Artemus Analyzer failed: " + ex.Message;
                myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
                return;
            } 
        }

        private void AnalyzeData()
        {
            try
            {
                this.ConnectAnalyzeQueue();
            }
            catch (Exception ex)
            {
                this.OnCatchAnalyzingError(ex);
            }
        }
        private void ConnectAnalyzeQueue()
        {
            try
            {
                if (!myAnalyzeQueue.ConnectedToTenant)
                {
                    myAnalyzeQueue.ConnectedToTenant = true;
                    analyzeQueueRepository.Update(myAnalyzeQueue);
                    analyzeQueueRepository.SubmitChanges();
                }

                if (!myAnalyzeQueue.ConnectedToEntity)
                {
                    myAnalyzeQueue.ConnectedToEntity = true;
                    analyzeQueueRepository.Update(myAnalyzeQueue);
                    analyzeQueueRepository.SubmitChanges();
                }

                if (!string.IsNullOrEmpty(myAnalyzeQueue.FileName))
                {
                    var shipmentNumber = myAnalyzeQueue.FileName.Split('.')[0].Split('_')[1];
                    var type = myAnalyzeQueue.FileName.Split('.')[0].Split('_')[0];
                    this.Simulate(this.Tenant, shipmentNumber, myAnalyzeQueue.MessageBody, type);
                }
            }

            catch (Exception ex)
            {
                this.OnCatchAnalyzingError(ex);
            }
        }
        public void Simulate(int tenant, string shipmentNumber, byte[] xmlString, string type )
        {
            this.Tenant = tenant;
            this.myShipmentContext = ShipmentsContext.GetContext(tenant);
            this.shipmentCustomsRepository = new ShipmentCustomsTransmissionRepository(myShipmentContext);
            string shipmentId = this.myShipmentContext.Shipments.Where(a => a.ShipmentNumber == shipmentNumber && a.Tenant == tenant).Select(a => a.Id).FirstOrDefault();

            if (!string.IsNullOrEmpty(shipmentId))
            {
                List<ShipmentCustomsTransmission> shipmentCustomslist = this.myShipmentContext.ShipmentCustomsTransmissions.Where(a => a.ShipmentId == shipmentId).Select(a => a).ToList();

                byte[] messageBytes = xmlString;
                int length = messageBytes.Length;
                XmlDocument xmlDocument = new XmlDocument();
                MemoryStream myMemoryStream = new MemoryStream(messageBytes);
                xmlDocument.Load(myMemoryStream);
                myMemoryStream.Position = 0;

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(AMSResponse));
                AMSResponse myResponse = (AMSResponse)xmlSerializer.Deserialize(myMemoryStream);
                if (myResponse != null)
                {
                    ShipmentCustomsTransmission customShipment = null;
                    foreach (response item in myResponse.response)
                    {
                        if (item.code == "INFO")
                        {
                            if (type.ToLower() == "voyage")
                            {
                                customShipment = shipmentCustomslist.Where(a => a.MessageCode == "ASVO").FirstOrDefault();
                            }
                            else if (type.ToLower() == "bl")
                            {
                                customShipment = shipmentCustomslist.Where(a => a.MessageCode == "ARBL").FirstOrDefault();
                            }

                            if (customShipment != null)
                            {
                                customShipment.Status = "ACPT";
                                customShipment.Error = "";
                                this.shipmentCustomsRepository.Update(customShipment);
                            }
                        }
                        else if (item.code == "ERROR")
                        {
                            if (type.ToLower() == "voyage")
                            {
                                customShipment = shipmentCustomslist.Where(a => a.MessageCode == "ASVO").FirstOrDefault();
                            }
                            else if (type.ToLower() == "bl")
                            {
                                customShipment = shipmentCustomslist.Where(a => a.MessageCode == "ARBL").FirstOrDefault();
                            }

                            if (customShipment != null)
                            {
                                customShipment.Status = "EROR";
                                customShipment.Error += item.description;
                                this.shipmentCustomsRepository.Update(customShipment);
                            }
                        }
                    }
                    this.shipmentCustomsRepository.SubmitChanges();
                    myAnalyzeQueue.Status = "D";
                    myAnalyzeQueue.ErrorMessage = null;
                    analyzeQueueRepository.Update(myAnalyzeQueue);
                    analyzeQueueRepository.SubmitChanges();
                }
            }
            else
            {
                //Update analyze queue
                myAnalyzeQueue.Status = "F";
                myAnalyzeQueue.ErrorMessage = "Shipment does not  exist";
                myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
            }
        }
        private void OnCatchAnalyzingError(Exception ex)
        {
            myAnalyzeQueue.ErrorMessage = ex.Message + (ex.InnerException != null ? Environment.NewLine + "InnerException: " + ex.InnerException.Message : "");
            myAnalyzeQueue.StackTrace = (ex.StackTrace != null ? Environment.NewLine + "Stack Trace: " + ex.StackTrace : "");
            myAnalyzeQueue.ErrorMessage = myAnalyzeQueue.ErrorMessage.Length > 7950 ? myAnalyzeQueue.ErrorMessage.Substring(0, 7950) : myAnalyzeQueue.ErrorMessage;
            myAnalyzeQueue.StackTrace = myAnalyzeQueue.StackTrace.Length > 7950 ? myAnalyzeQueue.StackTrace.Substring(0, 7950) : myAnalyzeQueue.StackTrace;

            if (ex.Message.StartsWith("--"))
            {
                myAnalyzeQueue.Status = "F";
            }

            else
            {
                myAnalyzeQueue.Retries++;

                if (myAnalyzeQueue.Retries >= 5)
                {
                    myAnalyzeQueue.Status = "F";

                    if (myAnalyzeQueue.ConnectedToTenant)
                    {
                        CommunicationLog commLog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, Tenant);
                        if (commLog != null)
                        {
                            commLog.CommunicationStatusTypeCode = "F";
                            commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                            commLog.LastStatusDateUTC = DateTime.UtcNow;
                            commLog.ExceptionMessage = myAnalyzeQueue.ErrorMessage;

                            if (myAnalyzeQueue.StackTrace != null)
                            {
                                commLog.ExceptionMessage = commLog.ExceptionMessage + Environment.NewLine + "Stack Trace: " + myAnalyzeQueue.StackTrace;
                            }

                            myCommunicationLogRepository.Update(commLog);
                            myCommunicationLogRepository.SubmitChanges();
                        }
                    }
                }
            }
            myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
            analyzeQueueRepository.Update(myAnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }
    }

    [System.SerializableAttribute()]
    public class AMSResponse
    {
        private response[] _response;
        [System.Xml.Serialization.XmlElementAttribute("response")]
        public response[] response
        {
            get
            {
                return this._response;
            }
            set
            {
                this._response = value;
            }
        }
    }

    [System.SerializableAttribute()]
    public class response
    {
        public string code { get; set; }
        public string description { get; set; }
        public string element { get; set; }
        public string source { get; set; }
    }
}
