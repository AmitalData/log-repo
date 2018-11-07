using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.SystemLogs;

namespace WebFreight.Web
{
    public partial class ShipmentStatuses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HttpRequest iRequest = this.Request;

                if (iRequest != null)
                {
                    string iString = "";

                    using (var reader = new StreamReader(Request.InputStream))
                    {
                        iString = reader.ReadToEnd();
                    }

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(iString);

                    if (!string.IsNullOrEmpty(iString))
                    {
                        XmlNodeList nodeList = xmldoc.GetElementsByTagName("ship_no");
                        string ShipmentNumber = string.Empty; 
                        if (nodeList[0] != null)
                        {
                            ShipmentNumber = nodeList[0].InnerText;
                        }
                        this.AddExternalTaskQueue(iString, ShipmentNumber);
                    }
                }
            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "ShipmentStatuses Page", "ChampMessaging Method", null);
                throw ex;
            }
        }

        private void AddExternalTaskQueue(string messageData,string ShipmentNumber)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(0); 
            //var ForwarderShipment = shipmentQuery.GetSinglePM(ShipmentId, tenant);
            //ObjectTableQuery tablesQuery = new ObjectTableQuery(0);
            //ObjectTablePM table = tablesQuery.GetObjectTableByName("Shipment", 0);
            //CommunicationsParams logParams = new CommunicationsParams()
            //{
            //    Tenant = 0,
            //    CommunicationLogTypeCode = "Q",
            //    QueueName = "externaltasksqueue" + tenant + 1,
            //    Priority = 1,
            //    InOut = "O",
            //    Status = "W",
            //    LoggingUserId = loggedContact.Id,
            //    LoggingObjectTableId = table.Id,
            //    LoggingEntityId = entityPM.Id,
            //    Subject = "LogBox Customer Status",
            //    FolderName = "ExternalTasksQueue",
            //};

            //CustomerPM mappedpm = CustomerHybridMapping.MapEntityToHybrid(entityPM);
            //string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(mappedpm);
            //List<QueueTask> tasks = new List<QueueTask>();
            //if (entityPM.IsPrivateLabelCustomer == true || entityPOCO.IsPrivateLabelCustomer == true)
            //{
            //    tasks.Add(new QueueTask() { Action = "Customer.PrivateLabel", Parameters = new List<Parameter>() { new Parameter { Order = 1, Value = entityPM.Code }, new Parameter { Order = 3, Value = entityPM.IsPrivateLabelCustomer.ToString() }, new Parameter { Order = 4, Value = entityPM.CustomerTenant.ToString() } } });
            //}
            //else
            //{
            //    tasks.Add(new QueueTask() { Action = "Customer.LogBoxActivated", Parameters = new List<Parameter>() { new Parameter { Order = 1, Value = entityPM.Code }, new Parameter { Order = 2, Value = entityPM.LogBoxActivated.ToString() } } });
            //}


            //logParams.ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
            //Communications.AddCommunicationLog(logParams);

        }
    }
}