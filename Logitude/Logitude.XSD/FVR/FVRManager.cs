using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.XSD.FVR
{
    public class FVRManager
    {
        private int tenant;
        public FVRResultClass FVRResult { get; set; }        
        public FVRManager(int tenant)
        {
            this.tenant = tenant;

            this.FVRResult = new FVRResultClass()
            {
                Id = tenant,
                Tenant = tenant,
                IsValid = true,
                IsUpgradingChamp = false,
                Errors = new List<string>(),
            };

            this.GetGlobalVariables();
        }

        private void GetGlobalVariables()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                if (this.tenant != 290)
                {
                    SettingRepository settingRepository = new SettingRepository();
                    Setting setting = settingRepository.GetSingleSetting("1");
                    if (setting != null)
                    {
                        if (setting.IsUpgradingChamp)
                        {
                            this.FVRResult.IsValid = false;
                            this.FVRResult.IsUpgradingChamp = setting.IsUpgradingChamp;
                        }
                    }
                }

                scope.Complete();
            }
        }

        public FVRResultClass SendFVR(string myAirlineId, string myFromPortId, string myToPortId, DateTime? myETD, DateTime? myETA, decimal? myVolume, decimal? myGrossWeight, string myVolumeUnitCode, string myGrossWeightUnitCode, string myShipmentId, string myBookingId, string myRecipient)
        {
            if (this.FVRResult.IsValid)
            {
                try
                {
                    #region
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        myVolume = MethodHelper.Normalize(myVolume);
                        myGrossWeight = MethodHelper.Normalize(myGrossWeight);

                        XmlSender xmlSender = new XmlSender(tenant, "FVR");

                        this.ValidateFVR(xmlSender.TTY, myAirlineId, myFromPortId, myToPortId, myETD, myETA, myVolume, myGrossWeight, myVolumeUnitCode, myGrossWeightUnitCode, myRecipient);

                        if (FVRResult.IsValid)
                        {
                            if (myETD != null)
                            {
                                myETD = myETD.Value.Date;
                            }

                            FlightsSchedulesRequest entityPOCO = new FlightsSchedulesRequest()
                            {
                                Id = IdCounter.GetNumber("FlightsSchedulesRequest", tenant).ToString(),
                                Tenant = tenant,
                                StatusCode = "W",
                                AirlineId = myAirlineId,
                                FromPortId = myFromPortId,
                                ToPortId = myToPortId,
                                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                CreatedByUserId = xmlSender.LoggedContactId,
                                ETD = myETD,
                                ETA = myETA,
                                Volume = myVolume,
                                VolumeUnitCode = myVolumeUnitCode,
                                GrossWeight = myGrossWeight,
                                GrossWeightUnitCode = myGrossWeightUnitCode,
                                ShipmentId = myShipmentId,
                                BookingId = myBookingId,
                            };

                            entityPOCO.RequestDetails = this.GetFlightsSchedulesRequestDetails(entityPOCO);

                            FlightsSchedulesRequestRepository myRepository = new FlightsSchedulesRequestRepository(tenant);
                            myRepository.Add(entityPOCO);
                            myRepository.SubmitChanges();

                            FVRResult.RequestId = entityPOCO.Id;

                            CHAMP17.ScheduleAndAvailabilityInformationRequest fVR = this.BuildFVR(myAirlineId, myFromPortId, myToPortId, myETD, myETA, myVolume, myGrossWeight, myVolumeUnitCode, myGrossWeightUnitCode);

                            CHAMP17.Envelope envelop = new CHAMP17.Envelope()
                            {
                                Sender = xmlSender.TTY,
                                Recipient = myRecipient,
                                Item = fVR,
                            };

                            xmlSender.SetSettings(FVRResult.RequestId, FVRResult.RequestId, "FlightsSchedulesRequest", true);
                            xmlSender.Send(envelop, "champmessageoutqueue");

                            this.BuildTransmissionLog(entityPOCO);
                        }

                        scope.Complete();
                    }
                    #endregion
                }

                catch (Exception ex)
                {
                    string ip = "";

                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        ip = currentIP;
                    }

                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "FVR web service (aka: simulator response web service)", null, ip);
                }
            }

            return this.FVRResult;
        }
        private void ValidateFVR(string myTTY, string myAirlineId, string myFromPortId, string myToPortId, DateTime? myETD, DateTime? myETA, decimal? myVolume, decimal? myGrossWeight, string myVolumeUnitCode, string myGrossWeightUnitCode, string myRecipient)
        {
            if (string.IsNullOrEmpty(myTTY))
            {
                FVRResult.IsValid = false;
                FVRResult.Errors.Add("TTY field is missing");
            }

            if (string.IsNullOrEmpty(myAirlineId))
            {
                FVRResult.IsValid = false;
                FVRResult.Errors.Add("Airline field is missing");
            }

            if (string.IsNullOrEmpty(myFromPortId))
            {
                FVRResult.IsValid = false;
                FVRResult.Errors.Add("From port field is missing");
            }

            if (string.IsNullOrEmpty(myToPortId))
            {
                FVRResult.IsValid = false;
                FVRResult.Errors.Add("To port field is missing");
            }

            if (string.IsNullOrEmpty(myRecipient))
            {
                FVRResult.IsValid = false;
                FVRResult.Errors.Add("This Airline doesn't support transmitting messages");
            }

            if (myETD == null)
            {
                FVRResult.IsValid = false;
                FVRResult.Errors.Add("ETD field is missing");
            }

            if (myVolume != null && string.IsNullOrEmpty(myVolumeUnitCode))
            {
                FVRResult.IsValid = false;
                FVRResult.Errors.Add("Volume unit field is missing");
            }

            if (myGrossWeight != null && string.IsNullOrEmpty(myGrossWeightUnitCode))
            {
                FVRResult.IsValid = false;
                FVRResult.Errors.Add("Weight unit field is missing");
            }

            ValidateAirlineRestriction(myAirlineId, FVRResult.Tenant);
        }
        private void ValidateAirlineRestriction(string myAirlineId, int tenant)
        {
            bool isRestrictedByAirline = false;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
                if (tenantManagement != null)
                {
                    isRestrictedByAirline = tenantManagement.IsRestrictedByAirline;
                    //CCSMessageType = tenantManagement.AWBMessagesCCSTypeCode;
                }
            }

            if (isRestrictedByAirline)
            {
                AirlineRepository airlineRepository = new AirlineRepository(tenant);

                if (MethodHelper.IsAirlineRestricted(myAirlineId, airlineRepository, tenant))
                {
                    FVRResult.IsValid = false;
                    FVRResult.Errors.Add("Airline is not allowed");
                }
            }
        }
        private string GetFlightsSchedulesRequestDetails(FlightsSchedulesRequest entity)
        {
            string myResult = "";

            if (entity != null)
            {
                int tenant = entity.Tenant;

                string airlineCode = "";
                string fromPortCode = "";
                string toPortCode = "";
                string fromDate = "";

                bool isNoAvailabilityInFVAMessages = false;
                if (!string.IsNullOrEmpty(entity.AirlineId))
                {
                    Card myCard = CardRepository.GetSingleCard(entity.AirlineId, tenant, true);
                    if (myCard != null)
                    {
                        airlineCode = myCard.Code;
                    }

                    AirlineRepository airlineRepository = new AirlineRepository(tenant);
                    Airline myAirline = airlineRepository.GetSingleAirline(entity.AirlineId, tenant);
                    if (myAirline != null)
                    {
                        isNoAvailabilityInFVAMessages = myAirline.NoAvailabilityInFVAMessages;
                    }
                }

                if (!string.IsNullOrEmpty(entity.FromPortId))
                {
                    PortPM myPort = PortQuery.GetSinglePort(tenant, entity.FromPortId, true);
                    if (myPort != null)
                    {
                        fromPortCode = myPort.Code;
                    }
                }

                if (!string.IsNullOrEmpty(entity.ToPortId))
                {
                    PortPM myPort = PortQuery.GetSinglePort(tenant, entity.ToPortId, true);
                    if (myPort != null)
                    {
                        toPortCode = myPort.Code;
                    }
                }

                if (entity.ETD != null)
                {
                    DateTime myDateTime = entity.ETD.Value;

                    string myMonth = String.Format("{0:MMM}", myDateTime, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")).ToUpper();
                    string myDay = String.Format("{0:00}", myDateTime.Day);

                    fromDate = myMonth + "-" + myDay;
                }

                myResult += "Airline:" + airlineCode + ",";
                myResult += "FromPort:" + fromPortCode + ",";
                myResult += "ToPort:" + toPortCode + ",";
                myResult += "FromDate:" + fromDate;

                if (!isNoAvailabilityInFVAMessages)
                {
                    myResult += ",";

                    string volume = "";
                    string weight = "";

                    if (entity.Volume != null && entity.VolumeUnitCode != null)
                    {
                        volume = String.Format("{0:N3}", entity.Volume) + ConvertVolumeUnitToChamp(entity.VolumeUnitCode);
                    }

                    if (entity.GrossWeight != null && entity.GrossWeightUnitCode != null)
                    {
                        weight = String.Format("{0:N3}", entity.GrossWeight) + ConvertGrossWeightUnitToChamp(entity.GrossWeightUnitCode);
                    }

                    myResult += "Volume:" + volume + ",";
                    myResult += "Weight:" + weight;
                }
            }

            return myResult;
        }
        private string ConvertVolumeUnitToChamp(string myVolumeUnitCode)
        {
            string myResult = null;

            if (myVolumeUnitCode != null)
            {
                switch (myVolumeUnitCode.ToUpper())
                {
                    case "CBF": { myResult = "CF"; break; }
                    case "CBI": { myResult = "CI"; break; }
                    case "CBM": { myResult = "MC"; break; }
                }
            }

            return myResult;
        }
        private string ConvertGrossWeightUnitToChamp(string myGrossWeightUnitCode)
        {
            string myResult = null;

            if (myGrossWeightUnitCode != null)
            {
                if (myGrossWeightUnitCode == "LB")
                {
                    myResult = "L";
                }

                else
                {
                    myResult = "K";
                }
            }

            return myResult;
        }
        private CHAMP17.ScheduleAndAvailabilityInformationRequest BuildFVR(string myAirlineId, string myFromPortId, string myToPortId, DateTime? fromDate, DateTime? toDate, decimal? myVolume, decimal? myGrossWeight, string myVolumeUnitCode, string myGrossWeightUnitCode)
        {
            string myCarrierCode = CardRepository.GetSingleCard(myAirlineId, FVRResult.Tenant, true).Code.ToUpper();
            string myFromPortCode = PortQuery.GetSinglePort(FVRResult.Tenant, myFromPortId, true).Code.ToUpper();
            string myToPortCode = PortQuery.GetSinglePort(FVRResult.Tenant, myToPortId, true).Code.ToUpper();

            CHAMP17.ScheduleAndAvailabilityInformationRequest myXSDElement = new CHAMP17.ScheduleAndAvailabilityInformationRequest()
            {
                StandardMessageIdentification = new CHAMP17.StandardMessageIdentification()
                {
                    MessageTypeVersionNumber = 1,
                    StandardMessageIdentifier = "FVR",
                },

                ScheduleAndAvailabilityInformationRequestDetails = new CHAMP17.ScheduleAndAvailabilityInformationRequestDetails()
                {
                    Flight = new CHAMP17.Flight()
                    {
                        CarrierCode = myCarrierCode,
                        //FlightNumber = "",
                    },

                    RequestedRouting = new CHAMP17.RequestedRouting()
                    {
                        AirportCityCodeOfDeparture = myFromPortCode,
                        AirportCityCodeOfArrival = myToPortCode,
                    },
                },
            };

            #region Earliest Departure DateTime
            if (fromDate != null)
            {
                string month = String.Format("{0:MMM}", fromDate.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")).ToUpper();
                string dayOfMonth = String.Format("{0:00}", fromDate.Value.Day);

                myXSDElement.ScheduleAndAvailabilityInformationRequestDetails.Date = new CHAMP17.Date()
                {
                    Month = month,
                    DayOfMonth = dayOfMonth,
                };

                //if (fromDate.Value.Hour > 0 || fromDate.Value.Minute > 0)
                //{
                //    string time = String.Format("{0:00}", fromDate.Value.Hour) + String.Format("{0:00}", fromDate.Value.Minute);

                //    myXSDElement.ScheduleAndAvailabilityInformationRequestDetails.EarliestDepartureTime = new CHAMP17.EarliestDepartureTime()
                //    {
                //        Time = time,
                //    };
                //}
            }
            #endregion

            #region LatestArrival DateTime
            //if (toDate != null)
            //{
            //    string month = String.Format("{0:MMM}", toDate.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")).ToUpper();
            //    string dayOfMonth = String.Format("{0:00}", toDate.Value.Day);

            //    myXSDElement.ScheduleAndAvailabilityInformationRequestDetails.LatestArrivalDateTime = new CHAMP17.LatestArrivalDateTime()
            //    {
            //        Month = month,
            //        DayOfMonth = dayOfMonth
            //    };

            //    if (toDate.Value.Hour > 0 || toDate.Value.Minute > 0)
            //    {
            //        string time = String.Format("{0:00}", toDate.Value.Hour) + String.Format("{0:00}", toDate.Value.Minute);

            //        myXSDElement.ScheduleAndAvailabilityInformationRequestDetails.LatestArrivalDateTime.Time = time;
            //        myXSDElement.ScheduleAndAvailabilityInformationRequestDetails.LatestArrivalDateTime.TimeSpecified = true;
            //    }
            //}
            #endregion

            #region Volume
            if (myVolume != null && !string.IsNullOrEmpty(myVolumeUnitCode))
            {
                myXSDElement.ScheduleAndAvailabilityInformationRequestDetails.Volume = new CHAMP17.VolumeInfo()
                {
                    VolumeCode = ConvertVolumeUnitToChamp(myVolumeUnitCode),
                    VolumeAmount = myVolume.Value
                };
            }
            #endregion

            #region Weight
            if (myGrossWeight != null && !string.IsNullOrEmpty(myGrossWeightUnitCode))
            {
                myXSDElement.ScheduleAndAvailabilityInformationRequestDetails.Weight = new CHAMP17.WeightInfo()
                {
                    WeightCode = ConvertGrossWeightUnitToChamp(myGrossWeightUnitCode),
                    WeightAmount = myGrossWeight.Value
                };
            }
            #endregion

            return myXSDElement;
        }

        public FVASimulatorResult SimulateXML(string xmlString, string myShipmentId, string myBookingId)
        {
            FVASimulatorResult myResult = new FVASimulatorResult()
            {
                IsValid = false,
                IsValidXML = false,
                Errors = new List<string>()
            };

            try
            {
                if (!string.IsNullOrEmpty(xmlString))
                {
                    #region Serializ
                    if (xmlString.Contains("<"))
                    {
                        int index = xmlString.IndexOf('<');
                        if (index > 0)
                        {
                            xmlString = xmlString.Substring(index);
                        }
                    }

                    byte[] messageBytes = Encoding.UTF8.GetBytes(xmlString);

                    MemoryStream myMemoryStream = new MemoryStream(messageBytes);
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(myMemoryStream);
                    myMemoryStream.Position = 0;

                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(CHAMP17.Envelope));
                    CHAMP17.Envelope myEnvelope = (CHAMP17.Envelope)xmlSerializer.Deserialize(myMemoryStream);
                    CHAMP17.ScheduleAndAvailabilityInformationAnswer myFVA = (CHAMP17.ScheduleAndAvailabilityInformationAnswer)myEnvelope.Item;
                    #endregion

                    if (myFVA == null)
                    {
                        myResult.IsValid = false;
                        myResult.IsValidXML = false;
                    }

                    else
                    {
                        myResult.IsValid = true;
                        myResult.IsValidXML = true;

                        string myRecipientID = myEnvelope.Recipient;
                        string myCarrierCode = myFVA.ScheduleAndAvailabilityInformationRequestDetails.Flight.CarrierCode;
                        string myFlightNumber = myFVA.ScheduleAndAvailabilityInformationRequestDetails.Flight.FlightNumber;

                        string myMonth = myFVA.ScheduleAndAvailabilityInformationRequestDetails.Date.Month;
                        string myDayOfMonth = myFVA.ScheduleAndAvailabilityInformationRequestDetails.Date.DayOfMonth;

                        string fromPortCode = myFVA.ScheduleAndAvailabilityInformationRequestDetails.RequestedRouting.AirportCityCodeOfDeparture;
                        string toPortCode = myFVA.ScheduleAndAvailabilityInformationRequestDetails.RequestedRouting.AirportCityCodeOfArrival;

                        IBookingContext myBookingContext = BookingContext.GetContext(tenant);
                        ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);

                        PortRepository myPortRepository = new PortRepository(myCommonContext);
                        CardRepository myCardRepository = new CardRepository(myCommonContext);

                        #region Get Tenant Objects
                        if (myCarrierCode != null)
                        {
                            Card myCard = myCardRepository.GetSingleCardByCode(myCarrierCode, tenant, true);
                            if (myCard == null)
                            {
                                myResult.Errors.Add("Airline: " + myCarrierCode + " doesnt exists in tenant " + tenant);
                                myResult.IsValid = false;
                            }

                            else
                            {
                                myResult.AirlineId = myCard.Id;
                            }
                        }

                        if (fromPortCode != null)
                        {
                            Port myPort = myPortRepository.GetAirlinePortByCode(tenant, fromPortCode, true);
                            if (myPort == null)
                            {
                                myResult.Errors.Add("Port: " + fromPortCode + " doesnt exists in tenant " + tenant);
                                myResult.IsValid = false;
                            }

                            else
                            {
                                myResult.FromPortId = myPort.Id;
                            }
                        }

                        if (toPortCode != null)
                        {
                            Port myPort = myPortRepository.GetAirlinePortByCode(tenant, toPortCode, true);
                            if (myPort == null)
                            {
                                myResult.Errors.Add("Port: " + toPortCode + " doesnt exists in tenant " + tenant);
                                myResult.IsValid = false;
                            }

                            else
                            {
                                myResult.ToPortId = myPort.Id;
                            }
                        }
                        #endregion

                        if (myResult.IsValid)
                        {
                            #region Build Request
                            string myLoggedContactId = null;
                            ContactRepository myContactRepository = new ContactRepository(myCommonContext);
                            Simplog.Data.CommonDataModel.EntityPOCOs.Contact myContact = myContactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);
                            if (myContact != null)
                            {
                                myLoggedContactId = myContact.Id;
                            }

                            DateTime? todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                            string myRequestDetails = XSDHelper.GetFlightsSchedulesRequestDetails(myFVA.ScheduleAndAvailabilityInformationRequestDetails, myCommonContext, tenant);
                            DateTime? myETD = XSDHelper.GenerateDate(myDayOfMonth, myMonth, todayDateTime);
                            myResult.ETD = myETD;

                            FlightsSchedulesRequest myRequest = new FlightsSchedulesRequest()
                            {
                                Id = IdCounter.GetNumber("FlightsSchedulesRequest", tenant).ToString(),
                                Tenant = tenant,
                                CreateDate = todayDateTime,
                                ETD = myETD,
                                AirlineId = myResult.AirlineId,
                                FromPortId = myResult.FromPortId,
                                ToPortId = myResult.ToPortId,
                                RequestDetails = myRequestDetails,
                                StatusCode = "W",
                                CreatedByUserId = myLoggedContactId,
                                ShipmentId = myShipmentId,
                                BookingId = myBookingId,
                            };

                            FlightsSchedulesRequestRepository myRepository = new FlightsSchedulesRequestRepository(myBookingContext);
                            myRepository.Add(myRequest);
                            myRepository.SubmitChanges();

                            myResult.RequestId = myRequest.Id;
                            #endregion

                            #region Build AnalyzeQueue
                            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                            {
                                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                                TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
                                if (tenantManagement != null)
                                {
                                    if (tenantManagement.TTY != myRecipientID)
                                    {
                                        tenantManagement.TTY = myRecipientID;
                                        tenantManagementRepository.Update(tenantManagement);
                                        tenantManagementRepository.SubmitChanges();
                                    }
                                }

                                AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
                                byte[] myXMLMessageBytes = Encoding.ASCII.GetBytes(xmlString);

                                AnalyzeQueue analyzeQueue = new AnalyzeQueue()
                                {
                                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                    From = "Champ",
                                    Id = IdCounter.GetNumber("AnalyzeQueue", tenant),
                                    MessageBody = myXMLMessageBytes,
                                    Status = "W",
                                    Retries = 0,
                                    ConnectedToTenant = false,
                                    ConnectedToEntity = false,
                                    FileSize = myXMLMessageBytes.Length,
                                };

                                analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
                                analyzeQueueReposiory.Add(analyzeQueue);
                                analyzeQueueReposiory.SubmitChanges();

                                scope.Complete();
                            }
                            #endregion
                        }
                    }
                }
            }

            catch
            {

            }

            return myResult;
        }
        public FVASimulatorResult SimulateFNA(string xmlString, string myShipmentId, string myBookingId)
        {
            FVASimulatorResult myResult = new FVASimulatorResult()
            {
                IsValid = false,
                IsValidXML = false,
                Errors = new List<string>()
            };

            try
            {
                if (!string.IsNullOrEmpty(xmlString))
                {
                    #region Serializ
                    if (xmlString.Contains("<"))
                    {
                        int index = xmlString.IndexOf('<');
                        if (index > 0)
                        {
                            xmlString = xmlString.Substring(index);
                        }
                    }

                    byte[] messageBytes = Encoding.UTF8.GetBytes(xmlString);

                    MemoryStream myMemoryStream = new MemoryStream(messageBytes);
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(myMemoryStream);
                    myMemoryStream.Position = 0;

                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(CHAMP17.Envelope));
                    CHAMP17.Envelope myEnvelope = (CHAMP17.Envelope)xmlSerializer.Deserialize(myMemoryStream);
                    CHAMP17.ErrorMessage myFNA = (CHAMP17.ErrorMessage)myEnvelope.Item;
                    #endregion

                    if (myFNA == null)
                    {
                        myResult.IsValid = false;
                        myResult.IsValidXML = false;
                    }

                    else
                    {
                        myResult.IsValid = true;
                        myResult.IsValidXML = true;

                        string myRecipientID = myEnvelope.Recipient;
                        string myReceivedMessageDetail = myFNA.ReceivedMessageDetail;


                        myReceivedMessageDetail = myReceivedMessageDetail.Trim();
                        string[] myReceivedMessageDetails = myReceivedMessageDetail.Split('/');

                        string portsString = myReceivedMessageDetails[1];
                        string dateString = myReceivedMessageDetails[2];
                        string myCarrierCode = myReceivedMessageDetails[3];

                        string fromPortCode = portsString.Substring(0, 3);
                        string toPortCode = portsString.Substring(3, 3);
                        string myDayOfMonth = dateString.Substring(0, 2);
                        string myMonth = dateString.Substring(2, 3);

                        IBookingContext myBookingContext = BookingContext.GetContext(tenant);
                        ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
                        PortRepository myPortRepository = new PortRepository(myCommonContext);
                        CardRepository myCardRepository = new CardRepository(myCommonContext);

                        #region Get Tenant Objects
                        if (myCarrierCode != null)
                        {
                            Card myCard = myCardRepository.GetSingleCardByCode(myCarrierCode, tenant, true);
                            if (myCard == null)
                            {
                                myResult.Errors.Add("Airline: " + myCarrierCode + " doesnt exists in tenant " + tenant);
                                myResult.IsValid = false;
                            }

                            else
                            {
                                myResult.AirlineId = myCard.Id;
                            }
                        }

                        if (fromPortCode != null)
                        {
                            Port myPort = myPortRepository.GetAirlinePortByCode(tenant, fromPortCode, true);
                            if (myPort == null)
                            {
                                myResult.Errors.Add("Port: " + fromPortCode + " doesnt exists in tenant " + tenant);
                                myResult.IsValid = false;
                            }

                            else
                            {
                                myResult.FromPortId = myPort.Id;
                            }
                        }

                        if (toPortCode != null)
                        {
                            Port myPort = myPortRepository.GetAirlinePortByCode(tenant, toPortCode, true);
                            if (myPort == null)
                            {
                                myResult.Errors.Add("Port: " + toPortCode + " doesnt exists in tenant " + tenant);
                                myResult.IsValid = false;
                            }

                            else
                            {
                                myResult.ToPortId = myPort.Id;
                            }
                        }
                        #endregion

                        if (myResult.IsValid)
                        {
                            #region Build Request
                            string myLoggedContactId = null;
                            ContactRepository myContactRepository = new ContactRepository(myCommonContext);
                            Simplog.Data.CommonDataModel.EntityPOCOs.Contact myContact = myContactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);
                            if (myContact != null)
                            {
                                myLoggedContactId = myContact.Id;
                            }

                            DateTime? todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                            string myRequestDetails = XSDHelper.GetFlightsSchedulesRequestDetails(myFNA, myCommonContext, tenant);
                            DateTime? myETD = XSDHelper.GenerateDate(myDayOfMonth, myMonth, todayDateTime);
                            myResult.ETD = myETD;

                            FlightsSchedulesRequest myRequest = new FlightsSchedulesRequest()
                            {
                                Id = IdCounter.GetNumber("FlightsSchedulesRequest", tenant).ToString(),
                                Tenant = tenant,
                                CreateDate = todayDateTime,
                                ETD = myETD,
                                AirlineId = myResult.AirlineId,
                                FromPortId = myResult.FromPortId,
                                ToPortId = myResult.ToPortId,
                                RequestDetails = myRequestDetails,
                                StatusCode = "W",
                                CreatedByUserId = myLoggedContactId,
                                ShipmentId = myShipmentId,
                                BookingId = myBookingId,
                            };

                            FlightsSchedulesRequestRepository myRepository = new FlightsSchedulesRequestRepository(myBookingContext);
                            myRepository.Add(myRequest);
                            myRepository.SubmitChanges();

                            myResult.RequestId = myRequest.Id;
                            #endregion

                            #region Build AnalyzeQueue
                            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                            {
                                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                                TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
                                if (tenantManagement != null)
                                {
                                    if (tenantManagement.TTY != myRecipientID)
                                    {
                                        tenantManagement.TTY = myRecipientID;
                                        tenantManagementRepository.Update(tenantManagement);
                                        tenantManagementRepository.SubmitChanges();
                                    }
                                }

                                AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
                                byte[] myXMLMessageBytes = Encoding.ASCII.GetBytes(xmlString);

                                AnalyzeQueue analyzeQueue = new AnalyzeQueue()
                                {
                                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                    From = "Champ",
                                    Id = IdCounter.GetNumber("AnalyzeQueue", tenant),
                                    MessageBody = myXMLMessageBytes,
                                    Status = "W",
                                    Retries = 0,
                                    ConnectedToTenant = false,
                                    ConnectedToEntity = false,
                                    FileSize = myXMLMessageBytes.Length,
                                };

                                analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
                                analyzeQueueReposiory.Add(analyzeQueue);
                                analyzeQueueReposiory.SubmitChanges();

                                scope.Complete();
                            }
                            #endregion
                        }
                    }
                }
            }

            catch
            {

            }

            return myResult;
        }

        public List<FlightSchedulePort> CopyFlightsSchedulesPorts(string[] myResponseIds)
        {
            List<FlightSchedulePort> myResult = new List<FlightSchedulePort>();

            if (myResponseIds != null)
            {
                if (myResponseIds.Count() > 0)
                {
                    List<string> myResponseIdsList = myResponseIds.ToList();

                    ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
                    PortRepository myPortRepository = new PortRepository(myCommonContext);
                    PortQuery myPortQuery = new PortQuery(myPortRepository);

                    FlightsSchedulesResponseRepository myRepository = new FlightsSchedulesResponseRepository(tenant);
                    List<FlightsSchedulesResponse> myResponses = myRepository.GetResponsesByIds(myResponseIdsList, tenant);

                    foreach (FlightsSchedulesResponse item in myResponses)
                    {
                        if (item.FromPortId == null)
                        {
                            #region
                            string myPortCode = item.FromPortCode;

                            Port myPort = myPortRepository.GetAirlinePortByCode(tenant, myPortCode, true);
                            if (myPort != null)
                            {
                                item.FromPortId = myPort.Id;
                                item.FromPortName = myPort.EnglishName;
                                this.FillFlightSchedulePort(myResult, myPort);
                            }

                            else
                            {
                                myPort = myPortRepository.GetAirlinePortByCode(0, myPortCode, true);
                                if (myPort != null)
                                {
                                    PortList list = myPortQuery.GetPortCopyToCurrentTenant(myPort.Id, tenant);
                                    if (list != null)
                                    {
                                        item.FromPortId = list.Id;
                                        item.FromPortName = list.EnglishName;
                                        this.FillFlightSchedulePort(myResult, list);
                                    }
                                }
                            }
                            #endregion
                        }

                        if (item.ToPortId == null)
                        {
                            #region
                            string myPortCode = item.ToPortCode;

                            Port myPort = myPortRepository.GetAirlinePortByCode(tenant, myPortCode, true);
                            if (myPort != null)
                            {
                                item.ToPortId = myPort.Id;
                                item.ToPortName = myPort.EnglishName;
                                this.FillFlightSchedulePort(myResult, myPort);
                            }

                            else
                            {
                                myPort = myPortRepository.GetAirlinePortByCode(0, myPortCode, true);
                                if (myPort != null)
                                {
                                    PortList list = myPortQuery.GetPortCopyToCurrentTenant(myPort.Id, tenant);
                                    if (list != null)
                                    {
                                        item.ToPortId = list.Id;
                                        item.ToPortName = list.EnglishName;
                                        this.FillFlightSchedulePort(myResult, list);
                                    }
                                }
                            }
                            #endregion
                        }

                        if (item.FromPortId == null || item.ToPortId == null)
                        {
                            item.MissingPort = true;
                        }

                        else
                        {
                            item.MissingPort = false;
                        }

                        myRepository.Update(item);
                    }

                    myRepository.SubmitChanges();
                }
            }

            return myResult;
        }
        private void FillFlightSchedulePort(List<FlightSchedulePort> myResult, Port myPort)
        {
            if (myPort != null)
            {
                if (!myResult.Where(d => d.PortId == myPort.Id).Any())
                {
                    FlightSchedulePort myResultItem = new FlightSchedulePort()
                    {
                        PortId = myPort.Id,
                        PortCode = myPort.Code,
                        PortName = myPort.EnglishName,
                        PortCountryCode = myPort.Country == null ? null : myPort.Country.Code,
                        PortCountryName = myPort.Country == null ? null : myPort.Country.EnglishName,
                    };

                    myResult.Add(myResultItem);
                }
            }
        }
        private void FillFlightSchedulePort(List<FlightSchedulePort> myResult, PortList myPort)
        {
            if (myPort != null)
            {
                if (!myResult.Where(d => d.PortId == myPort.Id).Any())
                {
                    FlightSchedulePort myResultItem = new FlightSchedulePort()
                    {
                        PortId = myPort.Id,
                        PortCode = myPort.Code,
                        PortName = myPort.EnglishName,
                        PortCountryCode = myPort.CountryCode,
                        PortCountryName = myPort.CountryName,
                    };

                    myResult.Add(myResultItem);
                }
            }
        }

        private void BuildTransmissionLog(FlightsSchedulesRequest entityPOCO)
        {
            MessagesTransmissionHelper TransmissionHelper = new MessagesTransmissionHelper(tenant, "FVR");
            TransmissionHelper.Build(entityPOCO);
        }
    }

    public class FVRResultClass
    {
        [Key]
        public int Id { get; set; }
        public int Tenant { get; set; }
        public bool IsValid { get; set; }
        public string RequestId { get; set; }
        public bool IsUpgradingChamp { get; set; }
        public List<string> Errors { get; set; }
    }
    public class FVASimulatorResult
    {
        [Key]
        public string RequestId { get; set; }
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public string AirlineId { get; set; }
        public DateTime? ETD { get; set; }
        public bool IsValid { get; set; }
        public bool IsValidXML { get; set; }
        public List<string> Errors { get; set; }
    }
    public class FlightSchedulePort
    {
        [Key]
        public string PortId { get; set; }
        public string PortCode { get; set; }
        public string PortName { get; set; }
        public string PortCountryCode { get; set; }
        public string PortCountryName { get; set; }
    }
}
