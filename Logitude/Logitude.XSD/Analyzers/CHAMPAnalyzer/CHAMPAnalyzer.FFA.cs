using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.BL.EntityUpdateServices;
using Logitude.BookingLib.Data;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BookingLib.Data.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel;

namespace Logitude.XSD.Analyzers.CHAMPAnalyzer
{
    public partial class CHAMPAnalyzer
    {
        private CHAMP17.AWBSpaceAllocationAnswer myFFA;
        private void AnalyzeBaseData_FFA()
        {
            this.myFFA = (CHAMP17.AWBSpaceAllocationAnswer)myEnvelope.Item;
            this.myPrefix = myFFA.ConsignmentDetail.AWBIdentification.AirlinePrefix;
            this.myMaster = myFFA.ConsignmentDetail.AWBIdentification.AWBSerialNumber;

            if (myPrefix.Length < 3)
            {
                int rem = 3 - myPrefix.Length;
                for (int i = 0; i < rem; i++)
                {
                    myPrefix = "0" + myPrefix;
                }
            }

            if (myMaster.Length < 8)
            {
                int rem = 8 - myMaster.Length;
                for (int i = 0; i < rem; i++)
                {
                    myMaster = "0" + myMaster;
                }
            }

            if (!string.IsNullOrEmpty(myPrefix) && !string.IsNullOrEmpty(myMaster))
            {
                myLongMaster = myPrefix + "-" + myMaster;
            }
        }

        private void AnalyzeMessageQueue_FFA(BookingPM entityPM, IBookingContext myContext, AnalyzeQueue analyzeQueue)
        {
            entityPM.HasResponse = true;
            entityPM.FMAAcknowledgementReason = null;
            entityPM.FNAReason = null;
            entityPM.AnswerOtherServicesInformation = null;
            entityPM.FFRStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);

            BookingAnswerRepository bookingAnswerRepository = new BookingAnswerRepository(myContext);
            AirlineRepository airlineRepository = new AirlineRepository(myTenant);

            List<BookingAnswer> bookingAnswers = bookingAnswerRepository.GetBookingAnswersForBookingTenant(entityPM.Id, myTenant).ToList();
            foreach (BookingAnswer item in bookingAnswers)
            {
                bookingAnswerRepository.Remove(item);
            }

            if (myFFA.FlightDetails != null && myFFA.FlightDetails.Count() > 0)
            {
                foreach (CHAMP17.FlightDetails_FFA item in myFFA.FlightDetails)
                {
                    Airline airline = airlineRepository.GetSingleAirlineByCode(item.FlightIdentification.CarrierCode, myTenant);

                    BookingAnswer answer = new BookingAnswer()
                    {
                        Id = IdCounter.GetNumber("BookingAnswer", myTenant),
                        Tenant = myTenant,
                        BookingId = entityPM.Id,
                        ETD = this.ComputeETD(item, entityPM),
                        CreateDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                        StatusCode = "WAT",
                        Origin = item.AirportsOfDepartureAndArrival.AirportCityCodeOfOrigin,
                        Destination = item.AirportsOfDepartureAndArrival.AirportCityCodeOfDestination,
                        CommunicationLogId = analyzeQueue.CommunicationLogId,
                        FlightNumber = item.FlightIdentification.CarrierCode + item.FlightIdentification.FlightNumber,
                        BookingSpaceAllocationCode = item.SpaceAllocationCode,
                        CarrierId = airline == null ? null : airline.Id,
                    };

                    bookingAnswerRepository.Add(answer);
                }

                if (myFFA.FlightDetails.All(d => d.SpaceAllocationCode == "KK"))
                {
                    entityPM.BookingStatusCode = "CNF";
                    entityPM.FFRStatusCode = "BCN";
                    entityPM.WaitingForResponse = false;
                    entityPM.HasErrors = false;
                }

                else if (myFFA.FlightDetails.Where(d => d.SpaceAllocationCode == "UU").Any())
                {
                    if (entityPM.FFRStatusCode == "CRS")
                    {
                        entityPM.FFRStatusCode = "CRR";
                        entityPM.WaitingForResponse = false;
                        entityPM.HasErrors = true;
                    }
                    else
                    {
                        entityPM.BookingStatusCode = "CRT";
                        entityPM.FFRStatusCode = "BRR";
                        entityPM.WaitingForResponse = false;
                        entityPM.HasErrors = true;
                    }
                }

                else if (myFFA.FlightDetails.All(d => d.SpaceAllocationCode == "CN"))
                {
                    entityPM.BookingStatusCode = "CRT";
                    entityPM.FFRStatusCode = "CAA";
                    entityPM.WaitingForResponse = false;
                    entityPM.HasErrors = false;

                    IShipmentsContext shipmentContext = ShipmentsContext.GetContext(myTenant);
                    MessagingStockRepository stockRepository = new MessagingStockRepository(shipmentContext);
                    MessagingStockUsageHistoryRepository usageHistoryRepository = new MessagingStockUsageHistoryRepository(shipmentContext);
                    IQueryable<MessagingStockUsageHistory> myUsageHistoryData = usageHistoryRepository.GetTenantMessagingStockUsageHistory(myTenant);
                    MessagingStockUsageHistory usageHistory = myUsageHistoryData.Where(d => d.EntityId == entityPM.Id && d.MessageType == "FFR").FirstOrDefault();
                    if (usageHistory != null)
                    {
                        usageHistoryRepository.Remove(usageHistory);
                        usageHistoryRepository.SubmitChanges();

                        MessagingStock myStock = stockRepository.GetSingleMessagingStock(usageHistory.StockId);
                        if (myStock != null)
                        {
                            int myStockUsageCount = usageHistoryRepository.GetStockUsageCount(myStock.Id, myStock.TenantNumber);

                            myStock.Remaining = myStock.Amount - myStockUsageCount;
                            stockRepository.Update(myStock);
                            stockRepository.SubmitChanges();
                        }
                    }
                }
            }

            if (myFFA.OtherServiceInformation != null)
            {
                if (!string.IsNullOrEmpty(myFFA.OtherServiceInformation.OSIDetailsFirstLine))
                {
                    entityPM.AnswerOtherServicesInformation = myFFA.OtherServiceInformation.OSIDetailsFirstLine;
                }

                if (!string.IsNullOrEmpty(myFFA.OtherServiceInformation.OSIDetailsSecondLine))
                {
                    entityPM.AnswerOtherServicesInformation += " " + myFFA.OtherServiceInformation.OSIDetailsSecondLine;
                }
            }

            entityPM.IsUpdatedByChampAnalyzer = true;
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            BookingUpdateService service = new BookingUpdateService(myContext, new Dictionary<string, IContext>(), myTenant);
            service.Update(entityPM, true);
        }

        private DateTime? ComputeETD(CHAMP17.FlightDetails_FFA item, BookingPM entityPM)
        {
            DateTime? ETD = null;

            int day = item.FlightIdentification.DayOfScheduledDeparture;
            int month = !string.IsNullOrEmpty(item.FlightIdentification.MonthOfScheduledDeparture) ? GetMonthInNumbers(item.FlightIdentification.MonthOfScheduledDeparture.ToUpper()) : 0;

            DateTime? currentDate = entityPM.CreateDate;
            int currentMonth = currentDate.Value.Month;
            int currentYear = currentDate.Value.Year;
            int difference = Math.Abs(currentMonth - month);

            int year = 0;
            if (difference > 6)
            {
                if (month > currentMonth)
                {
                    year = currentYear - 1;
                }

                else
                {
                    year = currentYear + 1;
                }
            }

            else
            {
                year = currentYear;
            }

            if (day != 0 && month != 0)
            {
                ETD = new DateTime(year, month, day);
            }

            return ETD;
        }
    }
}
