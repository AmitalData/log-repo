using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.XSD.Analyzers.INTTRAAnalyzer;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.Analyzers;
using WebFreight.Web.Security;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.WebServices
{
    public class ShipmentContainersWebServiceController : ApiController
    {
        string type = "c_id";
        string scacCode;
        string containerNumber;
        string oceanInsightId;

        public HttpResponseMessage Post(ShipmentContainerSimulator simulator)
        {
            try
            {
                ShipmentContainerSimulator shipmentContainerSimulator = new ShipmentContainerSimulator();
                if (simulator.IsFromContainer)
                {
                    shipmentContainerSimulator = this.RunFullContainerStatusSimulator(simulator);
                }
                else
                {
                    shipmentContainerSimulator = RunContainerStatusResponseSimulator(simulator);
                }
                return Request.CreateResponse(HttpStatusCode.OK, shipmentContainerSimulator);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private ShipmentContainerSimulator RunFullContainerStatusSimulator(ShipmentContainerSimulator simulator)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;
            SecurityUtility.AuthenticationOnTenant(tenant);
            this.SetContainerFields(simulator);
            this.CreateOceanInsightsWcfServiceResponse(tenant);
            this.CreateLogitudeOceanInsightsRequest(oceanInsightId, simulator, tenant);
            var updatedOceanInsightsResponse = this.ReplaceOceanInsightTagInXML(simulator.XmlString, oceanInsightId, simulator.ContainerNumber);
            this.SendRequestToContainerPushService(updatedOceanInsightsResponse);
            return simulator;
        }
        private void SetContainerFields(ShipmentContainerSimulator simulator)
        {
            containerNumber = simulator.ContainerNumber;
            scacCode = ReadOceanInsightsParametersXMLFields(simulator.XmlString, "carrier_scac", "shipment");
        }
        private void CreateOceanInsightsWcfServiceResponse(int tenant)
        {
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            OceanInsightsRequestQuery oceanInsightsRequestQuery = new OceanInsightsRequestQuery(tenant);
            OceanInsightsRequestService service = new OceanInsightsRequestService(shipmentsContext, tenant);
            OceanInsightsRequestPM oceanInsightsRequest;
            oceanInsightsRequest = oceanInsightsRequestQuery.GetSinglePMByOceanInsightsByScacCodeContainerNoTenant(scacCode, containerNumber, tenant);
            if (oceanInsightsRequest == null)
            {
                oceanInsightsRequest = new OceanInsightsRequestPM();
                oceanInsightsRequest.ContainerNumber = containerNumber;
                oceanInsightsRequest.SCACCode = scacCode;
                oceanInsightsRequest.Tenant = tenant;
                oceanInsightsRequest.OceanInsigntId = GetEightDigitssRandomNumber();
                oceanInsightsRequest.Type = type;
                service.Create(oceanInsightsRequest);
            }
            this.oceanInsightId = oceanInsightsRequest.OceanInsigntId;
        }
        private string GetEightDigitssRandomNumber()
        {
            Random random = new Random();
            int randomNo = random.Next(10000000, 99999999);
            return randomNo.ToString();
        }

        private string ReadOceanInsightsParametersXMLFields(string xmlText, string tag, string root)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlText);
            XmlNodeList xnList = xmlDoc.SelectNodes("//Root//container");
            var tagValue = "";
            foreach (XmlNode xn in xnList)
            {
                foreach (XmlNode item in xn.ChildNodes)
                {
                    if (item.ChildNodes != null && item.Name == root)
                    {
                        tagValue = item.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == tag).FirstOrDefault()?.InnerText;
                    }
                }
            }
            return tagValue;
        }
        private string ReplaceOceanInsightTagInXML(string xmlText,string oceanInsightId, string containerNumber)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlText);
            xmlDoc.DocumentElement.SelectSingleNode("//Root//container//shipment//shipmentsubscription_id").InnerText = oceanInsightId;
            xmlDoc.DocumentElement.SelectSingleNode("//Root//container//event//shipment_id").InnerText = oceanInsightId;
            xmlDoc.DocumentElement.SelectSingleNode("//Root//container//shipment//container_number").InnerText = containerNumber;
            return xmlDoc.OuterXml;
        }
        private void CreateLogitudeOceanInsightsRequest(string oceanInsightId, ShipmentContainerSimulator simulator, int tenant)
        {
            LogitudeOceanInsightsRequestRepository logitudeOceanInsightsRequestRepository = new LogitudeOceanInsightsRequestRepository(tenant);
            LogitudeOceanInsightsRequest logitudeOceanInsightsRequest = logitudeOceanInsightsRequestRepository.GetSingleLogitudeOceanInsightsByOceanInsigntId(oceanInsightId, tenant);
            if (logitudeOceanInsightsRequest == null)
            {
                logitudeOceanInsightsRequest = new LogitudeOceanInsightsRequest()
                {
                    Id = IdCounter.GetNumber("LogitudeOceanInsightsRequest", tenant),
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    OceanInsigntId = oceanInsightId,
                    ContainerNumber = containerNumber,
                    SCACCode = scacCode,
                    Tenant = tenant,
                    Type = type,
                    ShipmentId = simulator.ShipmentId
                };
                logitudeOceanInsightsRequestRepository.Add(logitudeOceanInsightsRequest);
            }
            else
            {
                logitudeOceanInsightsRequest.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                logitudeOceanInsightsRequest.ContainerNumber = containerNumber;
                logitudeOceanInsightsRequest.SCACCode = scacCode;
                logitudeOceanInsightsRequest.Type = type;
                logitudeOceanInsightsRequest.ShipmentId = simulator.ShipmentId;
                logitudeOceanInsightsRequestRepository.Update(logitudeOceanInsightsRequest);
            }
            logitudeOceanInsightsRequestRepository.SubmitChanges();
        }
        private void SendRequestToContainerPushService(string xmlString)
        {
            byte[] byteArray = GetXMLByteDataFromText(xmlString);
            string url = LogitudeSettings.LogitudeURL + "/ContainerPush.aspx";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // Get the response.
            WebResponse webResponse = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)webResponse).StatusDescription);
            dataStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            webResponse.Close();
        }
        private byte[] GetXMLByteDataFromText(string xmlString)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlString);
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            byte[] documentXML = Encoding.ASCII.GetBytes(jsonText);
            return documentXML;
        }
        private ShipmentContainerSimulator RunContainerStatusResponseSimulator(ShipmentContainerSimulator simulator)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                ShipmentContainerSimulator myResult = new ShipmentContainerSimulator();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                using (TransactionScope scope2 = TransactionFactory.GetTransaction())
                {
                    AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();

                    if (simulator.AnalyzeQueueId != null)
                    {
                        AnalyzeQueue analyzeQueue = analyzeQueueReposiory.GetSingleAnalyzeQueue(simulator.AnalyzeQueueId);
                        if (analyzeQueue == null)
                        {
                            myResult.Success = false;
                            myResult.Errors.Add("The Analyze Queue does not exists");
                        }

                        else
                        {
                            try
                            {
                                ContainerStatusesConnecterAnalyzer analyzer = new ContainerStatusesConnecterAnalyzer(analyzeQueue, analyzeQueueReposiory);
                                analyzer.Run();
                            }
                            catch (Exception ex)
                            {
                                myResult.Success = false;
                                myResult.Errors.Add(ex.Message);
                            }
                        }
                    }

                    else if (simulator.XmlString != null)
                    {
                        byte[] fileBytes = null;
                        try
                        {
                            fileBytes = Encoding.ASCII.GetBytes(simulator.XmlString);
                        }
                        catch (Exception ex)
                        {
                            myResult.Success = false;
                            myResult.Errors.Add("Xml Text is not valid");
                        }

                        try
                        {
                            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
                            {
                                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                                From = "ContainerStatusesReceiver",
                                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                                MessageBody = fileBytes,
                                Status = "W",
                                Retries = 0,
                                ConnectedToEntity = false,
                                ConnectedToTenant = false,
                                FileSize = fileBytes.Length,
                                Tenant = tenant,
                                FileName = "XmlString Simulator",
                            };

                            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
                            analyzeQueueReposiory.Add(analyzeQueue);
                            analyzeQueueReposiory.SubmitChanges();
                            ContainerStatusesConnecterAnalyzer analyzer = new ContainerStatusesConnecterAnalyzer(analyzeQueue, analyzeQueueReposiory);
                            analyzer.Run();
                        }
                        catch (Exception ex)
                        {
                            myResult.Success = false;
                            myResult.Errors.Add(ex.Message);
                        }
                    }

                    scope2.Complete();
                }

                scope.Complete();
                return myResult;
            }
        }
        public HttpResponseMessage GetContainerStatusRequest(string shipmentId, string containerId, bool isContainer)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    ContainerStatusesHelper myHelper = new ContainerStatusesHelper(shipmentId, containerId, isContainer, tenant);
                    if (myHelper.Validate())
                    {
                        myHelper.SendContainerStatusRequest();
                    }
                   else
                    {
                        throw new ApplicationException("The ScacCode code or Container number are empty, please fill them first");
                    }
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

    public class ShipmentContainerSimulator
    {
        public string XmlString { get; set; }
        public string AnalyzeQueueId { get; set; }
        public int FilesCount { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public bool IsFromContainer { get; set; }
        public string ShipmentId { get; set; }
        public string ContainerNumber { get; set; }
        public ShipmentContainerSimulator()
        {
            this.Success = true;
            this.Errors = new List<string>();
        }
    }
}