using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.BL.EntityUpdateServices;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.XSD.Analyzers.CHAMPAnalyzer
{
    public partial class CHAMPAnalyzer
    {
        private CHAMP17.ErrorMessage myFNA;
        private void AnalyzeBaseData_FNA()
        {
            this.myFNA = (CHAMP17.ErrorMessage)myEnvelope.Item;
            string myReceivedMessageDetail = myFNA.ReceivedMessageDetail;

            if (isTechnicalFNA)
            {
                this.AnalyzeTechnicalFNA(myReceivedMessageDetail);
            }

            else
            {
                this.myMessageDetailCode = myReceivedMessageDetail.Substring(0, 3);
                switch (myMessageDetailCode)
                {
                    case "FWB":
                    case "AWB":
                    case "FFR":
                        {
                            int indexOfSeparator = myReceivedMessageDetail.IndexOf("-");
                            this.myPrefix = myReceivedMessageDetail.Substring(indexOfSeparator - 3, 3);
                            this.myMaster = myReceivedMessageDetail.Substring(indexOfSeparator + 1, 8);

                            break;
                        }

                    case "FHL":
                        {
                            this.isFHLType = true;

                            int indexOfSeparator = myReceivedMessageDetail.IndexOf("-");
                            this.myPrefix = myReceivedMessageDetail.Substring(indexOfSeparator - 3, 3);
                            this.myMaster = myReceivedMessageDetail.Substring(indexOfSeparator + 1, 8);

                            if (myReceivedMessageDetail.Contains("HBS"))
                            {
                                string myFixedDetails = myReceivedMessageDetail.Substring(myReceivedMessageDetail.IndexOf("HBS"));
                                string[] dataArray = myFixedDetails.Split('/');
                                this.myHouse = dataArray[1];
                            }

                            break;
                        }

                    case "FSR":
                        {
                            string[] messageDetails = myReceivedMessageDetail.Split(new string[] { }, StringSplitOptions.RemoveEmptyEntries);
                            if (messageDetails.Count() > 0)
                            {
                                string master = messageDetails[1];

                                string[] PrefixAndAWB = master.Split('-');
                                myPrefix = PrefixAndAWB[0];
                                myMaster = PrefixAndAWB[1];
                            }

                            else
                            {
                                throw new Exception("Message details not recognized for analyzing fna for fsa ");
                            }

                            break;
                        }

                    case "FVR":
                        {
                            this.myRequestDetails = XSDHelper.GetFlightsSchedulesRequestDetails(myFNA, myCommonContext, myTenant);
                            break;
                        }

                    //default:
                    //    {
                    //        myReceivedMessageDetail = myReceivedMessageDetail.Replace("\"", "");

                    //        if (myReceivedMessageDetail.Contains("AirlinePrefix="))
                    //        {
                    //            int indexOfSeparator = myReceivedMessageDetail.IndexOf("AirlinePrefix=");
                    //            this.myPrefix = myReceivedMessageDetail.Substring(indexOfSeparator + "AirlinePrefix=".Length, 3);
                    //        }

                    //        if (myReceivedMessageDetail.Contains("AWBSerialNumber="))
                    //        {
                    //            int indexOfSeparator = myReceivedMessageDetail.IndexOf("AWBSerialNumber=");
                    //            this.myMaster = myReceivedMessageDetail.Substring(indexOfSeparator + "AWBSerialNumber=".Length, 8);
                    //        }

                    //        break;
                    //    }
                }
            }

            if (!string.IsNullOrEmpty(myPrefix) && !string.IsNullOrEmpty(myMaster))
            {
                myLongMaster = myPrefix + "-" + myMaster;
            }
        }

        private void AnalyzeTechnicalFNA(string myReceivedMessageDetail)
        {
            string myElementBody = null;
            string theOpenTag = null;
            string theCloseTag = null;

            myReceivedMessageDetail.Replace("<![CDATA[", "");
            myReceivedMessageDetail.Replace("]]>", "");


            if (myReceivedMessageDetail.Contains("<AWBConsignmentDetails>"))
            {
                myTechnicalIdentifier = "FWB";
                theOpenTag = "<AWBConsignmentDetails>";
                theCloseTag = "</AWBConsignmentDetails>";

                int indexOfStart = myReceivedMessageDetail.IndexOf(theOpenTag);
                int indexOfLast = myReceivedMessageDetail.IndexOf(theCloseTag);

                myElementBody = myReceivedMessageDetail.Substring(indexOfStart);
                myElementBody = myElementBody.Substring(0, indexOfLast - indexOfStart + theCloseTag.Length);

                myElementBody = myElementBody.Insert(0, "<StandardMessageIdentification StandardMessageIdentifier='FWB' MessageTypeVersionNumber='17'/>");
                myElementBody = myElementBody.Insert(0, "<AirWaybillData>");
                myElementBody = myElementBody.Insert(0, "<Envelope xmlns='http://www.champ.aero/GCCS/CargoXML'>");
                myElementBody = myElementBody.Insert(0, "<?xml version='1.0' encoding='UTF-8' standalone='yes'?>");
                myElementBody += "</AirWaybillData>";
                myElementBody += "</Envelope>";
            }

            // AWBSpaceAllocationRequest : Booking
            if (myReceivedMessageDetail.Contains("<ConsignmentDetail>"))
            {
                myTechnicalIdentifier = "FFR";
                theOpenTag = "<ConsignmentDetail>";
                theCloseTag = "</ConsignmentDetail>";

                int indexOfStart = myReceivedMessageDetail.IndexOf(theOpenTag);
                int indexOfLast = myReceivedMessageDetail.IndexOf(theCloseTag);

                myElementBody = myReceivedMessageDetail.Substring(indexOfStart);
                myElementBody = myElementBody.Substring(0, indexOfLast - indexOfStart + theCloseTag.Length);

                myElementBody = myElementBody.Insert(0, "<StandardMessageIdentification StandardMessageIdentifier='FFR' MessageTypeVersionNumber='7'/>");
                myElementBody = myElementBody.Insert(0, "<AWBSpaceAllocationRequest>");
                myElementBody = myElementBody.Insert(0, "<Envelope xmlns='http://www.champ.aero/GCCS/CargoXML'>");
                myElementBody = myElementBody.Insert(0, "<?xml version='1.0' encoding='UTF-8' standalone='yes'?>");
                myElementBody += "</AWBSpaceAllocationRequest>";
                myElementBody += "</Envelope>";
            }

            else if (myReceivedMessageDetail.Contains("<ScheduleAndAvailabilityInformationRequestDetails>"))
            {
                myTechnicalIdentifier = "FVR";
                theOpenTag = "<ScheduleAndAvailabilityInformationRequestDetails>";
                theCloseTag = "</ScheduleAndAvailabilityInformationRequestDetails>";

                int indexOfStart = myReceivedMessageDetail.IndexOf(theOpenTag);
                int indexOfLast = myReceivedMessageDetail.IndexOf(theCloseTag);

                myElementBody = myReceivedMessageDetail.Substring(indexOfStart);
                myElementBody = myElementBody.Substring(0, indexOfLast - indexOfStart + theCloseTag.Length);

                myElementBody = myElementBody.Insert(0, "<StandardMessageIdentification StandardMessageIdentifier='FVA' MessageTypeVersionNumber='1'/>");
                myElementBody = myElementBody.Insert(0, "<ScheduleAndAvailabilityInformationAnswer>");
                myElementBody = myElementBody.Insert(0, "<Envelope xmlns='http://www.champ.aero/GCCS/CargoXML'>");
                myElementBody = myElementBody.Insert(0, "<?xml version='1.0' encoding='UTF-8' standalone='yes'?>");

                myElementBody += "</ScheduleAndAvailabilityInformationAnswer>";
                myElementBody += "</Envelope>";
            }

            if (!string.IsNullOrEmpty(myElementBody))
            {
                byte[] bytearray = Encoding.ASCII.GetBytes(myElementBody);

                int index = 0;
                while (index < bytearray.Length)
                {
                    int d = bytearray[index];
                    if (d == 60)
                    {
                        bytearray = bytearray.Skip(index).ToArray();
                        break;
                    }

                    index++;
                }

                MemoryStream myMemoryStream = new MemoryStream(bytearray);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(myMemoryStream);
                myMemoryStream.Position = 0;

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(CHAMP17.Envelope));
                CHAMP17.Envelope entityEnvelope = (CHAMP17.Envelope)xmlSerializer.Deserialize(myMemoryStream);

                if (entityEnvelope != null)
                {
                    switch (myTechnicalIdentifier)
                    {
                        case "FWB":
                            {
                                CHAMP17.AirWaybillData entityItem = (CHAMP17.AirWaybillData)entityEnvelope.Item;

                                if (entityItem != null)
                                {
                                    if (entityItem.AWBConsignmentDetails != null)
                                    {
                                        if (entityItem.AWBConsignmentDetails.AWBIdentification != null)
                                        {
                                            this.myPrefix = entityItem.AWBConsignmentDetails.AWBIdentification.AirlinePrefix;
                                            this.myMaster = entityItem.AWBConsignmentDetails.AWBIdentification.AWBSerialNumber;
                                        }
                                    }
                                }

                                break;
                            }

                        case "FFR":
                            {
                                CHAMP17.AWBSpaceAllocationRequest entityItem = (CHAMP17.AWBSpaceAllocationRequest)entityEnvelope.Item;

                                if (entityItem != null)
                                {
                                    if (entityItem.ConsignmentDetail != null)
                                    {
                                        if (entityItem.ConsignmentDetail.AWBIdentification != null)
                                        {
                                            this.myPrefix = entityItem.ConsignmentDetail.AWBIdentification.AirlinePrefix;
                                            this.myMaster = entityItem.ConsignmentDetail.AWBIdentification.AWBSerialNumber;
                                        }
                                    }
                                }

                                break;
                            }

                        case "FVR":
                            {
                                CHAMP17.ScheduleAndAvailabilityInformationAnswer entityItem = (CHAMP17.ScheduleAndAvailabilityInformationAnswer)entityEnvelope.Item;

                                if (entityItem != null)
                                {
                                    if (entityItem.ScheduleAndAvailabilityInformationRequestDetails != null)
                                    {
                                        this.myRequestDetails = XSDHelper.GetFlightsSchedulesRequestDetails(entityItem.ScheduleAndAvailabilityInformationRequestDetails, myCommonContext, myTenant);
                                    }
                                }

                                break;
                            }
                    }
                }
            }
        }

        private void AnalyzeMessageQueue_FNA(ShipmentPM entityPM, IShipmentsContext myContext)
        {
            entityPM.IsUpdatedByChampAnalyzer = true;           

            entityPM.FNAReason = null;
            entityPM.CarrierLastStatusCode = "FNA";
            entityPM.CarrierLastStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
            string systemEmail = "system@tenant" + myTenant + ".com";

            string myReasonField = null;
            string[] myReasonforRejectionError = myFNA.ReasonForRejection;

            foreach (string reason in myReasonforRejectionError)
            {
                if(string.IsNullOrEmpty(myReasonField))
                {
                    myReasonField = reason;
                }

                else
                {
                    myReasonField += ',' + reason;
                }
            }

            if (myReasonField.Length > 249)
            {
                myReasonField = myReasonField.Substring(0, 249);
            }

            entityPM.FNAReason = myReasonField;
            myAnalyzeQueue.AckReason = myReasonField;

            switch (myMessageDetailCode)
            {
                case "FWB":
                    {
                        entityPM.FWBStatusCode = "EROR";
                        entityPM.FWBStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);

                        ShipmentService service = new ShipmentService(myContext, entityPM, systemEmail);
                        service.Update();

                        break;
                    }

                case "FHL":
                    {
                        entityPM.FHLStatusCode = "EROR";
                        entityPM.FHLStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);

                        ShipmentService service = new ShipmentService(myContext, entityPM, systemEmail);
                        service.Update();

                        if (entityPM.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(entityPM.MasterShipmentDataId))
                        {
                            ShipmentRepository shipmentRepository = new ShipmentRepository(myContext);
                            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                            ShipmentPM masterShipmentPM = shipmentQuery.GetSinglePM(entityPM.MasterShipmentDataId, myTenant);

                            masterShipmentPM.FHLStatusCode = shipmentRepository.GetMasterFHLStatus(masterShipmentPM.Id);
                            masterShipmentPM.IsUpdatedByChampAnalyzer = true;

                            ShipmentService masterService = new ShipmentService(myContext, masterShipmentPM, systemEmail);
                            masterService.Update();
                        }

                        break;
                    }

                case "FSR":
                    {

                        break;
                    }
            }
        }

        private void AnalyzeMessageQueue_FNA(BookingPM entityPM, IBookingContext myContext)
        {
            entityPM.HasResponse = true;
            entityPM.WaitingForResponse = false;
            entityPM.HasErrors = true;
            entityPM.FNAReason = null;
            entityPM.FFRStatusCode = "BRR";
            entityPM.BookingStatusCode = "CRT";
            entityPM.FFRStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
            entityPM.FMAAcknowledgementReason = null;
            entityPM.AnswerOtherServicesInformation = null;

            BookingAnswerRepository bookingAnswerRepository = new BookingAnswerRepository(myContext);
            List<BookingAnswer> bookingAnswers = bookingAnswerRepository.GetBookingAnswersForBookingTenant(entityPM.Id, myTenant).ToList();
            foreach (BookingAnswer item in bookingAnswers)
            {
                bookingAnswerRepository.Remove(item);
            }

            string systemEmail = "system@tenant" + myTenant + ".com";

            string myReasonField = null;
            string[] myReasonforRejectionError = myFNA.ReasonForRejection;

            foreach (string reason in myReasonforRejectionError)
            {
                if (string.IsNullOrEmpty(myReasonField))
                {
                    myReasonField = reason;
                }

                else
                {
                    myReasonField += ',' + reason;
                }
            }

            if (myReasonField.Length > 249)
            {
                myReasonField = myReasonField.Substring(0, 249);
            }

            entityPM.FNAReason = myReasonField;
            myAnalyzeQueue.AckReason = myReasonField;

            entityPM.IsUpdatedByChampAnalyzer = true;
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            BookingUpdateService service = new BookingUpdateService(myContext, new Dictionary<string, IContext>(), myTenant);
            service.Update(entityPM, true);
        }

        private void AnalyzeMessageQueue_FNA(FlightsSchedulesRequest myRequest, FlightsSchedulesRequestRepository myRepository)
        {
            if (myFNA.ReasonForRejection != null)
            {
                string myReasonField = null;
                string[] myReasonforRejectionError = myFNA.ReasonForRejection;

                foreach (string reason in myReasonforRejectionError)
                {
                    if (string.IsNullOrEmpty(myReasonField))
                    {
                        myReasonField = reason;
                    }

                    else
                    {
                        myReasonField += ',' + reason;
                    }
                }

                if (myReasonField.Length > 150)
                {
                    myReasonField = myReasonField.Substring(0, 150);
                }

                myAnalyzeQueue.AckReason = myReasonField;

                myRequest.ResponseDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
                myRequest.AnswerReasonForNoReply = myReasonField;
                myRepository.Update(myRequest);
                myRepository.SubmitChanges();
            }
        }
    }
}
