using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data;
using UnifreightIIG.Common.VendorAddCommunicationDeviceServiceReference;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.EntityQueryServices;

namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for ClientWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ClientWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public byte[] SearchClientRequest(byte[] searchClientParams)
        {
            MemoryStream memorystream = new MemoryStream(searchClientParams);
            XmlSerializer serializer = new XmlSerializer(typeof(ClientSearchRequestParams));
            ClientSearchRequestParams newSearchClientParams = (ClientSearchRequestParams)serializer.Deserialize(memorystream);
            var messageService = new CL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService();


            ClientSearchResponseData responseData = null;
            if (newSearchClientParams.TestCase != null && newSearchClientParams.TestCase.Type == "webservice" && newSearchClientParams.TestCase.Code != "Real Logic")
            {
                responseData = new ClientSearchResponseData();
                switch (newSearchClientParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;

                            responseData.UserMessage = null;

                            AddClient(newSearchClientParams);

                            responseData.CanContinue = false;
                            responseData.Message = "Send Succeeded";
                          

                            break;
                        }

                    case "Send Succeeded With Can Continue":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;

                            responseData.UserMessage = null;

                            responseData.CanContinue = true;
                            responseData.Message = "Build Client Screen";


                            break;
                        }

                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for search client message!";
                            break;
                        }
                   

                }

            }
            else
            {
                messageService = new CL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService();
                responseData = messageService.Send(newSearchClientParams);
            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(ClientSearchResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        private void AddClient(ClientSearchRequestParams newSearchClientParams)
        {
            
            Random rand = new Random();
            string localName =rand.Next(1000000).ToString() + "زبون رقم";
            ICustomContext context = CustomContext.GetContext(newSearchClientParams.Tenant);
            ClientUpdateService service = new ClientUpdateService(context,new Dictionary<string,IContext>(),newSearchClientParams.Tenant);
            ClientPM newClient = new ClientPM()
            {
                Code = newSearchClientParams.ExternalId,
                FullName = localName,
                PassportCountryCode = newSearchClientParams.PassportCountryCode,
                PassportTypeCode = newSearchClientParams.PassportTypeCode,
                PassportNumber = newSearchClientParams.PassportNumber,
                ChangeSetOp=ChangeSetOperation.Insert,
                Tenant=newSearchClientParams.Tenant,
            };
            service.Update(newClient, true);
        }

        [WebMethod]
        public byte[] CreateClientRequest(byte[] searchClientParams)
        {
            MemoryStream memorystream = new MemoryStream(searchClientParams);
            XmlSerializer serializer = new XmlSerializer(typeof(CreateClientRequestParams));
            CreateClientRequestParams newSearchClientParams = (CreateClientRequestParams)serializer.Deserialize(memorystream);
            var messageService = new CL_MSG100_AddClientMassagingService();


            INF_MSG_GenericResponseData responseData = null;
            if (newSearchClientParams.TestCase != null && newSearchClientParams.TestCase.Type == "webservice" && newSearchClientParams.TestCase.Code != "Real Logic")
            {
                responseData = new INF_MSG_GenericResponseData();
                switch (newSearchClientParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;
                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for search client message!";
                            break;
                        }
                }
            }
            else
            {
                messageService = new CL_MSG100_AddClientMassagingService();
                responseData = messageService.Send(newSearchClientParams);
            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(INF_MSG_GenericResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] AddUpdateDeleteClientAddressContactRequest(byte[] addAddressContactForClientParams)
        {
            MemoryStream memorystream = new MemoryStream(addAddressContactForClientParams);
            XmlSerializer serializer = new XmlSerializer(typeof(AddAddressContactForClient));
            AddAddressContactForClient newAddAddressContactForClientParams = (AddAddressContactForClient)serializer.Deserialize(memorystream);
            var messageService = new CL_3630_AddUpdateDeleteAddressContactMassagingService();


            INF_MSG_GenericResponseData responseData = null;
            if (newAddAddressContactForClientParams.TestCase != null && newAddAddressContactForClientParams.TestCase.Type == "webservice" && newAddAddressContactForClientParams.TestCase.Code != "Real Logic")
            {
                responseData = new INF_MSG_GenericResponseData();
                switch (newAddAddressContactForClientParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;
                            break;
                        }

                    case "Send Succeeded With Can Continue":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;
                            break;
                        }

                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for search client message!";
                            break;
                        }
                }

            }
            else
            {
                messageService = new CL_3630_AddUpdateDeleteAddressContactMassagingService();
                responseData = messageService.Send(newAddAddressContactForClientParams);
            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(INF_MSG_GenericResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        //        public string RecallClientsForCutomsRequest(string guidId, int tenant, string clientsList)
        public string RecallClientsForCutomsRequest(string guidId, int tenant)
        {
            ClientQueryService clientQueryService = new ClientQueryService(tenant);
            var clientsList = clientQueryService.GetAllLocalClients(tenant);

            if (clientsList.Count == 0)
            {
                return "Client List Is Empty (Count==0)";
            }

            string clientCode = "";
            for (int i = 0; i < clientsList.Count; i++)
            {
                clientCode = clientsList[i].Code;
                string mess = string.Format(
                    "בניית תקשורת עדכון נתוני יבואנים  {2} ( {0}/{1} ) "
                    , (i + 1), (clientsList.Count), clientCode);
                ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(guidId, mess);

                var clientSearchByCustomsAgentMessagingService = new CL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService();
                var req = new ClientSearchRequestParams()
                {
                    Tenant = tenant,
                    ExternalId = clientCode,
                    RequestVIA = SendRequestVIA.WebServiceBatch
                };
                var test = false;
                if (test)
                {
                    req.RequestVIA = SendRequestVIA.WebServiceInteractive;
                    req.SuppressSplitWR = true;
                }
                clientSearchByCustomsAgentMessagingService.Send(req);
            }

            return "עידכון כל הלקוחות  ( " + clientsList.Count.ToString() + " ) ימשיך ברקע";
        }
    }
}
