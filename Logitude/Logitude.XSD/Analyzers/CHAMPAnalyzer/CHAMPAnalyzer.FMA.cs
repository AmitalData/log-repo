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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Analyzers.CHAMPAnalyzer
{
    public partial class CHAMPAnalyzer
    {
        private CHAMP17.MessageAcknowledgement myFMA;
        private void AnalyzeBaseData_FMA()
        {
            this.myFMA = (CHAMP17.MessageAcknowledgement)myEnvelope.Item;
            string myReceivedMessageDetail = myFMA.ReceivedMessageDetail;

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
            }

            if (!string.IsNullOrEmpty(myPrefix) && !string.IsNullOrEmpty(myMaster))
            {
                myLongMaster = myPrefix + "-" + myMaster;
            }
        }

        private void AnalyzeMessageQueue_FMA(ShipmentPM entityPM, IShipmentsContext myContext)
        {
            entityPM.IsUpdatedByChampAnalyzer = true;

            entityPM.FNAReason = null;
            entityPM.CarrierLastStatusCode = "FMA";
            entityPM.CarrierLastStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
            string systemEmail = "system@tenant" + myTenant + ".com";

            switch (myMessageDetailCode)
            {
                case "FWB":
                    {
                        entityPM.FWBStatusCode = "ACPT";
                        entityPM.FWBStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);

                        ShipmentService service = new ShipmentService(myContext, entityPM, systemEmail);
                        service.Update();

                        break;
                    }

                case "FHL":
                    {
                        entityPM.FHLStatusCode = "ACPT";
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
            }
        }

        private void AnalyzeMessageQueue_FMA(BookingPM entityPM, IBookingContext myContext)
        {
            entityPM.HasResponse = true;
            entityPM.WaitingForResponse = true;
            entityPM.HasErrors = false;
            entityPM.FNAReason = null;
            entityPM.FFRStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
            entityPM.BookingStatusCode = "WCF";  

            if (entityPM.FFRStatusCode == "BRQ")
            {
                entityPM.FFRStatusCode = "RBA";
            }

            else if (entityPM.FFRStatusCode == "CRS")
            {
                entityPM.FFRStatusCode = "RBC";
            }

            BookingAnswerRepository bookingAnswerRepository = new BookingAnswerRepository(myContext);
            List<BookingAnswer> bookingAnswers = bookingAnswerRepository.GetBookingAnswersForBookingTenant(entityPM.Id, myTenant).ToList();
            foreach (BookingAnswer item in bookingAnswers)
            {
                bookingAnswerRepository.Remove(item);
            }

            string systemEmail = "system@tenant" + myTenant + ".com";

            string myReasonField = null;
            string[] myReasonForAcknowledgement = myFMA.ReasonForAcknowledgement;

            foreach (string reason in myReasonForAcknowledgement)
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

            string queuedReason = "queued for manual action";
            if (myReasonField.ToLower().Contains(queuedReason))
            {
                FFRStatusRepository ffrRep = new FFRStatusRepository(myTenant);
                FFRStatus ffrStatus = ffrRep.GetSingle(entityPM.FFRStatusCode);

                myReasonField = ffrStatus == null ? myReasonField : ffrStatus.Name;
            }

            entityPM.FMAAcknowledgementReason = myReasonField;
            entityPM.IsUpdatedByChampAnalyzer = true;

            myAnalyzeQueue.AckReason = myReasonField;

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            BookingUpdateService service = new BookingUpdateService(myContext, new Dictionary<string, IContext>(), myTenant);
            service.Update(entityPM, true);
        }
    }
}
