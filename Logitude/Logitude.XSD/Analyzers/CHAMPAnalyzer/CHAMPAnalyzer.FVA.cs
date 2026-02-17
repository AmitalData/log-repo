using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Analyzers.CHAMPAnalyzer
{
    public partial class CHAMPAnalyzer
    {
        private string myRequestDetails;
        private CHAMP17.ScheduleAndAvailabilityInformationAnswer myFVA;
        private void AnalyzeBaseData_FVA()
        {
            this.myFVA = (CHAMP17.ScheduleAndAvailabilityInformationAnswer)myEnvelope.Item;

            this.myRequestDetails = XSDHelper.GetFlightsSchedulesRequestDetails(myFVA.ScheduleAndAvailabilityInformationRequestDetails, myCommonContext, myTenant);
        }

        private void AnalyzeMessageQueue_FVA(FlightsSchedulesRequest myRequest, FlightsSchedulesRequestRepository myRequestRepository)
        {
            bool hasResponse = false;
            FlightsSchedulesResponseRepository myResponseRepository = new FlightsSchedulesResponseRepository(myTenant);

            #region ReasonForNoDisplay
            if (myFVA.ReasonForNoDisplay != null)
            {
                hasResponse = true;

                if (myFVA.ReasonForNoDisplay.Length > 150)
                {
                    myRequest.AnswerReasonForNoReply = myFVA.ReasonForNoDisplay.Substring(0, 150);
                }

                else
                {
                    myRequest.AnswerReasonForNoReply = myFVA.ReasonForNoDisplay;
                }

                myAnalyzeQueue.AckReason = myFVA.ReasonForNoDisplay;
            }
            #endregion

            #region AnswerOSI
            if (myFVA.OtherServiceInformation != null)
            {
                hasResponse = true;

                string myOsiField = null;
                string[] myOtherServiceInformation = myFVA.OtherServiceInformation;

                foreach (string item in myOtherServiceInformation)
                {
                    if (string.IsNullOrEmpty(myOsiField))
                    {
                        myOsiField = item;
                    }

                    else
                    {
                        myOsiField += ',' + item;
                    }
                }

                if (myOsiField.Length > 150)
                {
                    myOsiField = myOsiField.Substring(0, 150);
                }

                myRequest.AnswerOSI = myOsiField;
            }
            #endregion

            if (myFVA.ScheduleInformation != null)
            {
                hasResponse = true;

                if (myCommonContext == null)
                {
                    myCommonContext = CommonDataContext.GetContext(myTenant);
                }

                PortRepository myPortRepository = new PortRepository(myCommonContext);
                CardRepository myCardRepository = new CardRepository(myCommonContext);

                int myResultNumber = 1;                
                CHAMP17.ScheduleInformation[] myScheduleInformation = myFVA.ScheduleInformation;

                foreach (CHAMP17.ScheduleInformation scheduleInformation in myScheduleInformation)
                {                    
                    #region AnswerDetails
                    CHAMP17.ScheduleAndAvailabilityInformationAnswerDetails myAnswerDetails = scheduleInformation.ScheduleAndAvailabilityInformationAnswerDetails;
                    if (myAnswerDetails != null)
                    {
                        AnswerItemClass itemClass = new AnswerItemClass()
                        {
                            Id = myResultNumber,
                            MissingPort = false,
                        };

                        #region Date
                        if (myAnswerDetails.Date != null)
                        {
                            string myMonthString = myAnswerDetails.Date.Month;
                            string myDayString = myAnswerDetails.Date.DayOfMonth;
                            DateTime? myCurrentDate = myRequest.ETD != null ? myRequest.ETD : myRequest.CreateDate;
                            itemClass.AnswerDate = XSDHelper.GenerateDate(myDayString, myMonthString, myCurrentDate);
                        }
                        #endregion

                        if (myAnswerDetails.FlightDetails != null)
                        {
                            int myLineNumber = 0;
                            CHAMP17.FVAFlightDetails[] flightDetails = myAnswerDetails.FlightDetails;

                            foreach (CHAMP17.FVAFlightDetails item in flightDetails)
                            {
                                itemClass.AirplaneType = item.AircraftTypeCode;
                                itemClass.NumberOfStops = item.NumberOfStops;
                                
                                #region Flight
                                if (item.Flight != null)
                                {
                                    itemClass.FlightNumber = item.Flight.FlightNumber;

                                    if (!string.IsNullOrEmpty(item.Flight.CarrierCode))
                                    {
                                        Card myCard = myCardRepository.GetSingleCardByCode(item.Flight.CarrierCode, myTenant, true);
                                        if (myCard != null)
                                        {
                                            itemClass.AirlineId = myCard.Id;
                                        }
                                    }
                                }
                                #endregion

                                #region RequestedRouting | Ports
                                itemClass.FromPortId = null;
                                itemClass.FromPortCode = null;
                                itemClass.FromPortName = null;
                                itemClass.ToPortId = null;
                                itemClass.ToPortCode = null;
                                itemClass.ToPortName = null;

                                if (item.RequestedRouting != null)
                                {
                                    if (item.RequestedRouting.AirportCityCodeOfDeparture != null)
                                    {
                                        itemClass.FromPortCode = item.RequestedRouting.AirportCityCodeOfDeparture;

                                        Port myPort = myPortRepository.GetAirlinePortByCode(myTenant, itemClass.FromPortCode, true);
                                        if (myPort != null)
                                        {
                                            itemClass.FromPortId = myPort.Id;
                                            itemClass.FromPortName = myPort.EnglishName;
                                        }

                                        else
                                        {
                                            myPort = myPortRepository.GetAirlinePortByCode(0, itemClass.FromPortCode, true);
                                            if (myPort != null)
                                            {
                                                itemClass.FromPortName = myPort.EnglishName;
                                            }
                                        }
                                    }

                                    if (item.RequestedRouting.AirportCityCodeOfArrival != null)
                                    {
                                        itemClass.ToPortCode = item.RequestedRouting.AirportCityCodeOfArrival;

                                        Port myPort = myPortRepository.GetAirlinePortByCode(myTenant, itemClass.ToPortCode, true);
                                        if (myPort != null)
                                        {
                                            itemClass.ToPortId = myPort.Id;
                                            itemClass.ToPortName = myPort.EnglishName;
                                        }

                                        else
                                        {
                                            myPort = myPortRepository.GetAirlinePortByCode(0, itemClass.ToPortCode, true);
                                            if (myPort != null)
                                            {
                                                itemClass.ToPortName = myPort.EnglishName;
                                            }
                                        }
                                    }

                                    if (itemClass.FromPortId == null || itemClass.ToPortId == null)
                                    {
                                        itemClass.MissingPort = true;
                                    }
                                }
                                #endregion

                                #region Schedule
                                if (item.Schedule != null)
                                {
                                    int myDepartureHour = item.Schedule.TimeOfScheduledDeparture / 100;
                                    int myDepartureMinute = item.Schedule.TimeOfScheduledDeparture % 100;
                                    int myDepartureDayIndicator = GetDayChangeIndicator(item.Schedule.DepartureDayChangeIndicator);
                                    itemClass.ETD = new DateTime(itemClass.AnswerDate.Value.Year, itemClass.AnswerDate.Value.Month, itemClass.AnswerDate.Value.Day, myDepartureHour, myDepartureMinute, 0);
                                    itemClass.ETD = itemClass.ETD.Value.AddDays(myDepartureDayIndicator);

                                    int myArrivalHour = item.Schedule.TimeOfScheduledArrival / 100;
                                    int myArrivalMinute = item.Schedule.TimeOfScheduledArrival % 100;
                                    int myArrivalDayIndicator = GetDayChangeIndicator(item.Schedule.ArrivalDayChangeIndicator);
                                    itemClass.ETA = new DateTime(itemClass.AnswerDate.Value.Year, itemClass.AnswerDate.Value.Month, itemClass.AnswerDate.Value.Day, myArrivalHour, myArrivalMinute, 0);
                                    itemClass.ETA = itemClass.ETA.Value.AddDays(myArrivalDayIndicator);
                                }

                                else
                                {

                                }
                                #endregion

                                if (item.ScheduleSupplementaryInformation != null)
                                {
                                    //item.ScheduleSupplementaryInformation.
                                }

                                FlightsSchedulesResponse myResponse = new FlightsSchedulesResponse()
                                {
                                    Id = IdCounter.GetNumber("FlightsSchedulesResponse", myTenant).ToString(),
                                    Tenant = myTenant,
                                    RequestId = myRequest.Id,
                                    ResultNumber = myResultNumber,
                                    LineNumber = myLineNumber,
                                    AirlineId = itemClass.AirlineId,
                                    AirplaneType = itemClass.AirplaneType,
                                    FlightNumber = itemClass.FlightNumber,
                                    FromPortId = itemClass.FromPortId,
                                    FromPortCode = itemClass.FromPortCode,
                                    FromPortName = itemClass.FromPortName,
                                    ToPortId = itemClass.ToPortId,
                                    ToPortCode = itemClass.ToPortCode,
                                    ToPortName = itemClass.ToPortName,
                                    MissingPort = itemClass.MissingPort,
                                    NumberOfStops = itemClass.NumberOfStops,
                                    ETD = itemClass.ETD,
                                    ETA = itemClass.ETA,
                                };

                                myResponseRepository.Add(myResponse);
                                myLineNumber++;
                            }
                        }
                    }
                    #endregion
                    
                    #region SupplementaryDetails
                    CHAMP17.AvailabilitySupplementaryDetails[] supplementaryDetails = scheduleInformation.AvailabilitySupplementaryDetails;
                    if (supplementaryDetails != null)
                    {
                        //foreach (CHAMP17.AvailabilitySupplementaryDetails item in supplementaryDetails)
                        //{
                        //    int var1 = item.LatestCheckInTime;
                        //    bool var2 = item.LatestCheckInTimeSpecified;
                        //    string var3 = item.ServiceCode;
                        //    CHAMP17.VolumeInfo var4 = item.Volume;
                        //    CHAMP17.WeightInfo var5 = item.Weight;
                        //}
                    }
                    #endregion

                    myResultNumber++;
                }
            }

            if (hasResponse)
            {
                myRequest.ResponseDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
                myRequestRepository.Update(myRequest);
                myRequestRepository.SubmitChanges();
                myResponseRepository.SubmitChanges();
            }
        }
    }

    public class AnswerItemClass
    {
        public int Id { get; set; }
        public DateTime? AnswerDate { get; set; }
        public string AirlineId { get; set; }
        public string FlightNumber { get; set; }
        public string FromPortId { get; set; }
        public string FromPortCode { get; set; }
        public string FromPortName { get; set; }
        public string ToPortId { get; set; }
        public string ToPortCode { get; set; }
        public string ToPortName { get; set; }
        public bool MissingPort { get; set; }
        public string AirplaneType { get; set; }
        public int NumberOfStops { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
    }
}
