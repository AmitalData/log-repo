using Logitude.Server.Tools.Counters;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text; 
using System.Web;
using System.Xml;
using Logitude.BL.ShipmentsModel.EntityOtherServices;
using ContainerOISimulator.LoginWcfServiceReference;
using ContainerOISimulator.OceanInsightsTestWcfServiceReference;


namespace ContainerOISimulator
{
    public class ContainerSimulator
    {
        string type = "c_id";
        string scacCode;
        string containerNumber;
        string oceanInsightId;
        string token;
        public void Run(ShipmentContainerSimulator simulator, int tenant)
        {
            this.SetContainerFields(simulator);
            this.CreateOceanInsightsWcfServiceResponse(tenant);
            this.CreateLogitudeOceanInsightsRequest(oceanInsightId, simulator, tenant);
            var updatedOceanInsightsResponse = this.ReplaceOceanInsightTagInXML(simulator.XmlString, oceanInsightId, simulator.ContainerNumber);
            this.SendRequestToContainerPushService(updatedOceanInsightsResponse);
        }

        private void SetContainerFields(ShipmentContainerSimulator simulator)
        {
            containerNumber = simulator.ContainerNumber;
            scacCode = ReadOceanInsightsParametersXMLFields(simulator.XmlString, "carrier_scac", "shipment");
        }
        private void CreateOceanInsightsWcfServiceResponse(int tenant)
        {
            LoginToExternalService();
            //Calling Ocean Insight WCF Service
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;

            var endpoint = new EndpointAddress(LogitudeSettings.AmitalCloudEnvironmentURL + "WcfApi/OceanInsightsTestWcfService.svc");

            OceanInsightsWcfServiceClient oceanInsightsWcfService = new OceanInsightsWcfServiceClient(binding, endpoint);

            if (oceanInsightsWcfService.Endpoint.Address.Uri.Scheme == "https")
            {
                binding.Security.Mode = BasicHttpSecurityMode.Transport;
                binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
            }
            else
            {
                binding.Security.Mode = BasicHttpSecurityMode.None;
                binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            }
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)oceanInsightsWcfService.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);
                var result = oceanInsightsWcfService.Insert(LogitudeSettings.OITenantNumber, scacCode, containerNumber, type);
                this.oceanInsightId = result.Result;
            }
        }

        private void LoginToExternalService()
        {
            if (string.IsNullOrEmpty(token))
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                binding.MaxBufferSize = 2147483647;
                binding.MaxReceivedMessageSize = 2147483647;
                binding.ReaderQuotas.MaxStringContentLength = 2147483647;
                binding.ReaderQuotas.MaxArrayLength = 2147483647;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;

                var endpoint = new EndpointAddress(LogitudeSettings.AmitalCloudEnvironmentURL + "WcfApi/LoginWcfService.svc");

                LoginWcfServiceClient loginService = new LoginWcfServiceClient(binding, endpoint);

                if (loginService.Endpoint.Address.Uri.Scheme == "https")
                {
                    binding.Security.Mode = BasicHttpSecurityMode.Transport;
                    binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
                }
                else
                {
                    binding.Security.Mode = BasicHttpSecurityMode.None;
                    binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
                }

                var aPICredentialsParameters = new APICredentialsParameters()
                {
                    PrimaryKey = LogitudeSettings.AmitalCloudLogitudeTenantPrimaryKey,
                    Tenant = LogitudeSettings.OITenantNumber
                };
                var service = loginService.LoginByCredential(null, aPICredentialsParameters);
                this.token = service.Result;
            }
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
        private string ReplaceOceanInsightTagInXML(string xmlText, string oceanInsightId, string containerNumber)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlText);
                xmlDoc.DocumentElement.SelectSingleNode("//Root//container//shipment//shipmentsubscription_id").InnerText = oceanInsightId;
                xmlDoc.DocumentElement.SelectSingleNode("//Root//container//event//shipment_id").InnerText = oceanInsightId;
                xmlDoc.DocumentElement.SelectSingleNode("//Root//container//shipment//container_number").InnerText = containerNumber;
                return xmlDoc.OuterXml;
            }
            catch (Exception ex)
            {
                throw new Exception("Xml Text is not valid");
            }
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
            string url = LogitudeSettings.AmitalCloudEnvironmentURL + "/ContainerPush.aspx";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add("Authorization", "Token " + token);
            }
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
            reader.Close();
            dataStream.Close();
            webResponse.Close();
        }

        private byte[] GetXMLByteDataFromText(string xmlString)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xmlString);
                string jsonText = JsonConvert.SerializeXmlNode(doc);
                byte[] documentXML = Encoding.ASCII.GetBytes(jsonText);
                return documentXML;
            }
            catch (Exception ex)
            {
                throw new Exception("Xml Text is not valid");
            }
        }
    }
}
