using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.XSD;
using Logitude.XSD.FVR;
using Logitude.XSD.Simulators;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for MessageSimulatingWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MessageSimulatingWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public SimulatorResult Simulate(byte[] byteData, int tenant)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SimulatorArgs));
            MemoryStream memoryStream = new MemoryStream(byteData);
            SimulatorArgs simulatorArgs = (SimulatorArgs)xmlSerializer.Deserialize(memoryStream);

            SimulatorResult myResult = null;

            if (simulatorArgs != null)
            {
                Simulator simulator = new Simulator(simulatorArgs);
                simulator.Run();

                myResult = simulator.Result;
            }

            return myResult;
        }

        [WebMethod]
        public FVASimulatorResult BuildFVAXML(string xmlString, string myShipmentId, string myBookingId, int tenant)
        {
            FVRManager myManager = new FVRManager(tenant);
            FVASimulatorResult myResult = myManager.SimulateXML(xmlString, myShipmentId, myBookingId);
            return myResult;

            //FVASimulatorResult myResult = new FVASimulatorResult()
            //{
            //    IsValid = false,
            //    IsValidXML = false,
            //    Errors = new List<string>()
            //};

            //try
            //{
            //    if (!string.IsNullOrEmpty(xmlString))
            //    {
            //        #region Serializ
            //        if (xmlString.Contains("<"))
            //        {
            //            int index = xmlString.IndexOf('<');
            //            if (index > 0)
            //            {
            //                xmlString = xmlString.Substring(index);
            //            }
            //        }

            //        byte[] messageBytes = Encoding.UTF8.GetBytes(xmlString);

            //        MemoryStream myMemoryStream = new MemoryStream(messageBytes);
            //        XmlDocument xmlDocument = new XmlDocument();
            //        xmlDocument.Load(myMemoryStream);
            //        myMemoryStream.Position = 0;

            //        XmlSerializer xmlSerializer = new XmlSerializer(typeof(CHAMP17.Envelope));
            //        CHAMP17.Envelope myEnvelope = (CHAMP17.Envelope)xmlSerializer.Deserialize(myMemoryStream);
            //        CHAMP17.ScheduleAndAvailabilityInformationAnswer myFVA = (CHAMP17.ScheduleAndAvailabilityInformationAnswer)myEnvelope.Item;
            //        #endregion

            //        if (myFVA == null)
            //        {
            //            myResult.IsValid = false;
            //            myResult.IsValidXML = false;
            //        }

            //        else
            //        {
            //            myResult.IsValid = true;
            //            myResult.IsValidXML = true;

            //            string myRecipientID = myEnvelope.Recipient;
            //            string myCarrierCode = myFVA.ScheduleAndAvailabilityInformationRequestDetails.Flight.CarrierCode;
            //            string myFlightNumber = myFVA.ScheduleAndAvailabilityInformationRequestDetails.Flight.FlightNumber;

            //            string myMonth = myFVA.ScheduleAndAvailabilityInformationRequestDetails.Date.Month;
            //            string myDayOfMonth = myFVA.ScheduleAndAvailabilityInformationRequestDetails.Date.DayOfMonth;

            //            string fromPortCode = myFVA.ScheduleAndAvailabilityInformationRequestDetails.RequestedRouting.AirportCityCodeOfDeparture;
            //            string toPortCode = myFVA.ScheduleAndAvailabilityInformationRequestDetails.RequestedRouting.AirportCityCodeOfArrival;

            //            IBookingContext myBookingContext = BookingContext.GetContext(tenant);
            //            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);

            //            PortRepository myPortRepository = new PortRepository(myCommonContext);
            //            CardRepository myCardRepository = new CardRepository(myCommonContext);

            //            #region Get Tenant Objects
            //            if (myCarrierCode != null)
            //            {
            //                Card myCard = myCardRepository.GetSingleCardByCode(myCarrierCode, tenant, true);
            //                if (myCard == null)
            //                {
            //                    myResult.Errors.Add("Airline: " + myCarrierCode + " doesnt exists in tenant " + tenant);
            //                    myResult.IsValid = false;
            //                }

            //                else
            //                {
            //                    myResult.AirlineId = myCard.Id;
            //                }
            //            }

            //            if (fromPortCode != null)
            //            {
            //                Port myPort = myPortRepository.GetAirlinePortByCode(tenant, fromPortCode, true);
            //                if (myPort == null)
            //                {
            //                    myResult.Errors.Add("Port: " + fromPortCode + " doesnt exists in tenant " + tenant);
            //                    myResult.IsValid = false;
            //                }

            //                else
            //                {
            //                    myResult.FromPortId = myPort.Id;
            //                }
            //            }

            //            if (toPortCode != null)
            //            {
            //                Port myPort = myPortRepository.GetAirlinePortByCode(tenant, toPortCode, true);
            //                if (myPort == null)
            //                {
            //                    myResult.Errors.Add("Port: " + toPortCode + " doesnt exists in tenant " + tenant);
            //                    myResult.IsValid = false;
            //                }

            //                else
            //                {
            //                    myResult.ToPortId = myPort.Id;
            //                }
            //            }
            //            #endregion

            //            if (myResult.IsValid)
            //            {
            //                #region Build Request
            //                string myLoggedContactId = null;
            //                ContactRepository myContactRepository = new ContactRepository(myCommonContext);
            //                Simplog.Data.CommonDataModel.EntityPOCOs.Contact myContact = myContactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);
            //                if (myContact != null)
            //                {
            //                    myLoggedContactId = myContact.Id;
            //                }

            //                DateTime? todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            //                string myRequestDetails = XSDHelper.GetFlightsSchedulesRequestDetails(myFVA.ScheduleAndAvailabilityInformationRequestDetails, myCommonContext, tenant);
            //                DateTime? myETD = XSDHelper.GenerateDate(myDayOfMonth, myMonth, todayDateTime);
            //                myResult.ETD = myETD;

            //                FlightsSchedulesRequest myRequest = new FlightsSchedulesRequest()
            //                {
            //                    Id = IdCounter.GetNumber("FlightsSchedulesRequest", tenant).ToString(),
            //                    Tenant = tenant,
            //                    CreateDate = todayDateTime,
            //                    ETD = myETD,
            //                    AirlineId = myResult.AirlineId,
            //                    FromPortId = myResult.FromPortId,
            //                    ToPortId = myResult.ToPortId,
            //                    RequestDetails = myRequestDetails,
            //                    StatusCode = "W",
            //                    CreatedByUserId = myLoggedContactId,
            //                    ShipmentId = myShipmentId,
            //                    BookingId = myBookingId,
            //                };

            //                FlightsSchedulesRequestRepository myRepository = new FlightsSchedulesRequestRepository(myBookingContext);
            //                myRepository.Add(myRequest);
            //                myRepository.SubmitChanges();

            //                myResult.RequestId = myRequest.Id;
            //                #endregion

            //                #region Build AnalyzeQueue
            //                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //                {
            //                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
            //                    TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
            //                    if (tenantManagement != null)
            //                    {
            //                        if (tenantManagement.TTY != myRecipientID)
            //                        {
            //                            tenantManagement.TTY = myRecipientID;
            //                            tenantManagementRepository.Update(tenantManagement);
            //                            tenantManagementRepository.SubmitChanges();
            //                        }
            //                    }

            //                    AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            //                    byte[] myXMLMessageBytes = Encoding.ASCII.GetBytes(xmlString);

            //                    AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            //                    {
            //                        CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
            //                        From = "Champ",
            //                        Id = IdCounter.GetNumber("AnalyzeQueue", tenant),
            //                        MessageBody = myXMLMessageBytes,
            //                        Status = "W",
            //                        Retries = 0,
            //                        ConnectedToTenant = false,
            //                        ConnectedToEntity = false,
            //                        FileSize = myXMLMessageBytes.Length,
            //                    };

            //                    analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            //                    analyzeQueueReposiory.Add(analyzeQueue);
            //                    analyzeQueueReposiory.SubmitChanges();

            //                    scope.Complete();
            //                }
            //                #endregion
            //            }
            //        }
            //    }
            //}

            //catch
            //{

            //}

            //return myResult;
        }

        [WebMethod]
        public FVASimulatorResult BuildFVRFNAXML(string xmlString, string myShipmentId, string myBookingId, int tenant)
        {
            FVRManager myManager = new FVRManager(tenant);
            FVASimulatorResult myResult = myManager.SimulateFNA(xmlString, myShipmentId, myBookingId);
            return myResult;

            //FVASimulatorResult myResult = new FVASimulatorResult()
            //{
            //    IsValid = false,
            //    IsValidXML = false,
            //    Errors = new List<string>()
            //};

            //try
            //{
            //    if (!string.IsNullOrEmpty(xmlString))
            //    {
            //        #region Serializ
            //        if (xmlString.Contains("<"))
            //        {
            //            int index = xmlString.IndexOf('<');
            //            if (index > 0)
            //            {
            //                xmlString = xmlString.Substring(index);
            //            }
            //        }

            //        byte[] messageBytes = Encoding.UTF8.GetBytes(xmlString);

            //        MemoryStream myMemoryStream = new MemoryStream(messageBytes);
            //        XmlDocument xmlDocument = new XmlDocument();
            //        xmlDocument.Load(myMemoryStream);
            //        myMemoryStream.Position = 0;

            //        XmlSerializer xmlSerializer = new XmlSerializer(typeof(CHAMP17.Envelope));
            //        CHAMP17.Envelope myEnvelope = (CHAMP17.Envelope)xmlSerializer.Deserialize(myMemoryStream);
            //        CHAMP17.ErrorMessage myFNA = (CHAMP17.ErrorMessage)myEnvelope.Item;
            //        #endregion

            //        if (myFNA == null)
            //        {
            //            myResult.IsValid = false;
            //            myResult.IsValidXML = false;
            //        }

            //        else
            //        {
            //            myResult.IsValid = true;
            //            myResult.IsValidXML = true;

            //            string myRecipientID = myEnvelope.Recipient;
            //            string myReceivedMessageDetail = myFNA.ReceivedMessageDetail;


            //            myReceivedMessageDetail = myReceivedMessageDetail.Trim();
            //            string[] myReceivedMessageDetails = myReceivedMessageDetail.Split('/');

            //            string portsString = myReceivedMessageDetails[1];
            //            string dateString = myReceivedMessageDetails[2];
            //            string myCarrierCode = myReceivedMessageDetails[3];

            //            string fromPortCode = portsString.Substring(0, 3);
            //            string toPortCode = portsString.Substring(3, 3);
            //            string myDayOfMonth = dateString.Substring(0, 2);
            //            string myMonth = dateString.Substring(2, 3);

            //            IBookingContext myBookingContext = BookingContext.GetContext(tenant);
            //            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
            //            PortRepository myPortRepository = new PortRepository(myCommonContext);
            //            CardRepository myCardRepository = new CardRepository(myCommonContext);

            //            #region Get Tenant Objects
            //            if (myCarrierCode != null)
            //            {
            //                Card myCard = myCardRepository.GetSingleCardByCode(myCarrierCode, tenant, true);
            //                if (myCard == null)
            //                {
            //                    myResult.Errors.Add("Airline: " + myCarrierCode + " doesnt exists in tenant " + tenant);
            //                    myResult.IsValid = false;
            //                }

            //                else
            //                {
            //                    myResult.AirlineId = myCard.Id;
            //                }
            //            }

            //            if (fromPortCode != null)
            //            {
            //                Port myPort = myPortRepository.GetAirlinePortByCode(tenant, fromPortCode, true);
            //                if (myPort == null)
            //                {
            //                    myResult.Errors.Add("Port: " + fromPortCode + " doesnt exists in tenant " + tenant);
            //                    myResult.IsValid = false;
            //                }

            //                else
            //                {
            //                    myResult.FromPortId = myPort.Id;
            //                }
            //            }

            //            if (toPortCode != null)
            //            {
            //                Port myPort = myPortRepository.GetAirlinePortByCode(tenant, toPortCode, true);
            //                if (myPort == null)
            //                {
            //                    myResult.Errors.Add("Port: " + toPortCode + " doesnt exists in tenant " + tenant);
            //                    myResult.IsValid = false;
            //                }

            //                else
            //                {
            //                    myResult.ToPortId = myPort.Id;
            //                }
            //            }
            //            #endregion

            //            if (myResult.IsValid)
            //            {
            //                #region Build Request
            //                string myLoggedContactId = null;
            //                ContactRepository myContactRepository = new ContactRepository(myCommonContext);
            //                Simplog.Data.CommonDataModel.EntityPOCOs.Contact myContact = myContactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);
            //                if (myContact != null)
            //                {
            //                    myLoggedContactId = myContact.Id;
            //                }

            //                DateTime? todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            //                string myRequestDetails = XSDHelper.GetFlightsSchedulesRequestDetails(myFNA, myCommonContext, tenant);
            //                DateTime? myETD = XSDHelper.GenerateDate(myDayOfMonth, myMonth, todayDateTime);
            //                myResult.ETD = myETD;

            //                FlightsSchedulesRequest myRequest = new FlightsSchedulesRequest()
            //                {
            //                    Id = IdCounter.GetNumber("FlightsSchedulesRequest", tenant).ToString(),
            //                    Tenant = tenant,
            //                    CreateDate = todayDateTime,
            //                    ETD = myETD,
            //                    AirlineId = myResult.AirlineId,
            //                    FromPortId = myResult.FromPortId,
            //                    ToPortId = myResult.ToPortId,
            //                    RequestDetails = myRequestDetails,
            //                    StatusCode = "W",
            //                    CreatedByUserId = myLoggedContactId,
            //                    ShipmentId = myShipmentId,
            //                    BookingId = myBookingId,
            //                };

            //                FlightsSchedulesRequestRepository myRepository = new FlightsSchedulesRequestRepository(myBookingContext);
            //                myRepository.Add(myRequest);
            //                myRepository.SubmitChanges();

            //                myResult.RequestId = myRequest.Id;
            //                #endregion

            //                #region Build AnalyzeQueue
            //                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //                {
            //                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
            //                    TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
            //                    if (tenantManagement != null)
            //                    {
            //                        if (tenantManagement.TTY != myRecipientID)
            //                        {
            //                            tenantManagement.TTY = myRecipientID;
            //                            tenantManagementRepository.Update(tenantManagement);
            //                            tenantManagementRepository.SubmitChanges();
            //                        }
            //                    }

            //                    AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            //                    byte[] myXMLMessageBytes = Encoding.ASCII.GetBytes(xmlString);

            //                    AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            //                    {
            //                        CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
            //                        From = "Champ",
            //                        Id = IdCounter.GetNumber("AnalyzeQueue", tenant),
            //                        MessageBody = myXMLMessageBytes,
            //                        Status = "W",
            //                        Retries = 0,
            //                        ConnectedToTenant = false,
            //                        ConnectedToEntity = false,
            //                        FileSize = myXMLMessageBytes.Length,
            //                    };

            //                    analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            //                    analyzeQueueReposiory.Add(analyzeQueue);
            //                    analyzeQueueReposiory.SubmitChanges();

            //                    scope.Complete();
            //                }
            //                #endregion
            //            }
            //        }
            //    }
            //}

            //catch
            //{

            //}

            //return myResult;
        }

        [WebMethod]
        public List<FlightSchedulePort> CopyFlightsSchedulesPorts(string[] myResponseIds, int tenant)
        {
            FVRManager myManager = new FVRManager(tenant);
            List<FlightSchedulePort> myResult = myManager.CopyFlightsSchedulesPorts(myResponseIds);                      
            return myResult;
        }
    }    
}
