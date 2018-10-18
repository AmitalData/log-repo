using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.XSD.DataContracts;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD
{
    public class XSDHelper
    {
        public static string GetFlightsSchedulesRequestDetails(CHAMP17.ScheduleAndAvailabilityInformationRequestDetails entityDetails, ICommonDataContext myCommonContext, int myTenant)
        {
            string myResult = "";

            if (entityDetails != null)
            {
                string airlineCode = "";
                string fromPortCode = "";
                string toPortCode = "";
                string fromDate = "";

                bool isNoAvailabilityInFVAMessages = false;
                if (entityDetails.Flight != null)
                {
                    airlineCode = entityDetails.Flight.CarrierCode;
                    if (!string.IsNullOrEmpty(airlineCode))
                    {
                        if (myCommonContext == null)
                        {
                            myCommonContext = CommonDataContext.GetContext(myTenant);
                        }

                        CardRepository myCardRepository = new CardRepository(myCommonContext);
                        Card myCard = myCardRepository.GetSingleCardByCode(airlineCode, myTenant, true);
                        if (myCard != null)
                        {
                            AirlineRepository airlineRepository = new AirlineRepository(myCommonContext);
                            Airline myAirline = airlineRepository.GetSingleAirline(myCard.Id, myTenant);
                            if (myAirline != null)
                            {
                                isNoAvailabilityInFVAMessages = myAirline.NoAvailabilityInFVAMessages;
                            }
                        }
                    }
                }

                if (entityDetails.RequestedRouting != null)
                {
                    fromPortCode = entityDetails.RequestedRouting.AirportCityCodeOfDeparture;
                    toPortCode = entityDetails.RequestedRouting.AirportCityCodeOfArrival;
                }

                if (entityDetails.Date != null)
                {
                    string myMonth = "";
                    string myDay = "";

                    if (entityDetails.Date.Month != null)
                    {
                        myMonth = entityDetails.Date.Month;
                    }

                    if (entityDetails.Date.DayOfMonth != null)
                    {
                        myDay = entityDetails.Date.DayOfMonth;
                    }

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

                    if (entityDetails.Volume != null)
                    {
                        volume = String.Format("{0:N3}", entityDetails.Volume.VolumeAmount) + entityDetails.Volume.VolumeCode;
                    }

                    if (entityDetails.Weight != null)
                    {
                        weight = String.Format("{0:N3}", entityDetails.Weight.WeightAmount) + entityDetails.Weight.WeightCode;
                    }

                    myResult += "Volume:" + volume + ",";
                    myResult += "Weight:" + weight;
                }
            }

            return myResult;
        }
        public static string GetFlightsSchedulesRequestDetails(CHAMP17.ErrorMessage myErrorMessage, ICommonDataContext myCommonContext, int myTenant)
        {
            string myResult = "";

            if (myErrorMessage != null)
            {
                string myReceivedMessageDetail = myErrorMessage.ReceivedMessageDetail;

                if (myReceivedMessageDetail != null)
                {
                    myReceivedMessageDetail = myReceivedMessageDetail.Trim();
                    string[] myReceivedMessageDetails = myReceivedMessageDetail.Split('/');

                    string airlineCode = "";
                    string fromPortCode = "";
                    string toPortCode = "";
                    string fromDate = "";

                    string portsString = myReceivedMessageDetails[1];
                    string dateString = myReceivedMessageDetails[2];
                    string myCarrierCode = myReceivedMessageDetails[3];
                    string myDay = dateString.Substring(0, 2);
                    string myMonth = dateString.Substring(2, 3);

                    airlineCode = myCarrierCode;
                    fromPortCode = portsString.Substring(0, 3);
                    toPortCode = portsString.Substring(3, 3);
                    fromDate = myMonth + "-" + myDay;

                    bool isNoAvailabilityInFVAMessages = false;
                    if (!string.IsNullOrEmpty(airlineCode))
                    {
                        if (myCommonContext == null)
                        {
                            myCommonContext = CommonDataContext.GetContext(myTenant);
                        }

                        CardRepository myCardRepository = new CardRepository(myCommonContext);
                        Card myCard = myCardRepository.GetSingleCardByCode(airlineCode, myTenant, true);
                        if (myCard != null)
                        {
                            AirlineRepository airlineRepository = new AirlineRepository(myCommonContext);
                            Airline myAirline = airlineRepository.GetSingleAirline(myCard.Id, myTenant);
                            if (myAirline != null)
                            {
                                isNoAvailabilityInFVAMessages = myAirline.NoAvailabilityInFVAMessages;
                            }
                        }
                    }

                    myResult += "Airline:" + airlineCode + ",";
                    myResult += "FromPort:" + fromPortCode + ",";
                    myResult += "ToPort:" + toPortCode + ",";
                    myResult += "FromDate:" + fromDate;

                    //if (!isNoAvailabilityInFVAMessages)
                    //{
                    //    myResult += ",";

                    //    string volume = "";
                    //    string weight = "";

                    //    if (entityDetails.Volume != null)
                    //    {
                    //        volume = String.Format("{0:N3}", entityDetails.Volume.VolumeAmount) + entityDetails.Volume.VolumeCode;
                    //    }

                    //    if (entityDetails.Weight != null)
                    //    {
                    //        weight = String.Format("{0:N3}", entityDetails.Weight.WeightAmount) + entityDetails.Weight.WeightCode;
                    //    }

                    //    myResult += "Volume:" + volume + ",";
                    //    myResult += "Weight:" + weight;
                    //}
                }
            }

            return myResult;
        }

        public static DateTime? GenerateDate(string dayString, string monthString, DateTime? myComparativeDate)
        {
            DateTime? myResult = null;

            int myDay = 0;
            int myMonth = 0;

            if (!string.IsNullOrEmpty(dayString))
            {
                Int32.TryParse(dayString, out myDay);
            }

            if (!string.IsNullOrEmpty(monthString))
            {
                myMonth = GetMonthInNumbers(monthString);
            }

            if (myDay != 0 && myMonth != 0)
            {
                if (myComparativeDate != null)
                {
                    int myYear = 0;

                    int myCurrentMonth = myComparativeDate.Value.Month;
                    int myCurrentYear = myComparativeDate.Value.Year;
                    int difference = Math.Abs(myCurrentMonth - myMonth);

                    if (difference > 6)
                    {
                        if (myMonth > myCurrentMonth)
                        {
                            myYear = myCurrentYear - 1;
                        }

                        else
                        {
                            myYear = myCurrentYear + 1;
                        }
                    }

                    else
                    {
                        myYear = myCurrentYear;
                    }

                    int hour = 0;
                    int minute = 0;

                    myResult = new DateTime(myYear, myMonth, myDay, hour, minute, 0);
                }
            }

            return myResult;
        }
        public static int GetMonthInNumbers(string month)
        {
            int monthInNumber = 1;
            switch (month)
            {
                case "JAN":
                    {
                        monthInNumber = 1;
                        break;
                    }
                case "FEB":
                    {
                        monthInNumber = 2;
                        break;
                    }
                case "MAR":
                    {
                        monthInNumber = 3;
                        break;
                    }
                case "APR":
                    {
                        monthInNumber = 4;
                        break;
                    }
                case "MAY":
                    {
                        monthInNumber = 5;
                        break;
                    }
                case "JUN":
                    {
                        monthInNumber = 6;
                        break;
                    }
                case "JUL":
                    {
                        monthInNumber = 7;
                        break;
                    }
                case "AUG":
                    {
                        monthInNumber = 8;
                        break;
                    }
                case "SEP":
                    {
                        monthInNumber = 9;
                        break;
                    }
                case "OCT":
                    {
                        monthInNumber = 10;
                        break;
                    }
                case "NOV":
                    {
                        monthInNumber = 11;
                        break;
                    }
                case "DEC":
                    {
                        monthInNumber = 12;
                        break;
                    }
            }
            return monthInNumber;
        }
        private static string ConvertVolumeUnitToChamp(string myVolumeUnitCode)
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
        private static string ConvertGrossWeightUnitToChamp(string myGrossWeightUnitCode)
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
    }
}
