using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Simulators.CHAMPSimulators
{
    public partial class CHAMPSimulator
    {
        private void SimulateFVA()
        {
            string myFromPortCode = this.GetPortCode(args.FVA.FromPortId);
            string myToPortCode = this.GetPortCode(args.FVA.ToPortId);
            string myAirlineCode = this.GetCardCode(args.AirlineId);

            CHAMP17.ScheduleAndAvailabilityInformationAnswer xmlItem = new CHAMP17.ScheduleAndAvailabilityInformationAnswer()
            {
                StandardMessageIdentification = new CHAMP17.StandardMessageIdentification()
                {
                    MessageTypeVersionNumber = 1,
                    StandardMessageIdentifier = "FVA",
                },
            };

            #region RequestDetails

            xmlItem.ScheduleAndAvailabilityInformationRequestDetails = new CHAMP17.ScheduleAndAvailabilityInformationRequestDetails()
            {
                Flight = new CHAMP17.Flight(),
                RequestedRouting = new CHAMP17.RequestedRouting(),
                Date = new CHAMP17.Date(),
            };

            #region Flight
            if (!string.IsNullOrEmpty(myAirlineCode))
            {
                xmlItem.ScheduleAndAvailabilityInformationRequestDetails.Flight.CarrierCode = myAirlineCode;
            }
            #endregion

            #region RequestedRouting
            if (!string.IsNullOrEmpty(myFromPortCode))
            {
                xmlItem.ScheduleAndAvailabilityInformationRequestDetails.RequestedRouting.AirportCityCodeOfDeparture = myFromPortCode;
            }

            if (!string.IsNullOrEmpty(myToPortCode))
            {
                xmlItem.ScheduleAndAvailabilityInformationRequestDetails.RequestedRouting.AirportCityCodeOfArrival = myToPortCode;
            }
            #endregion

            #region Date
            if (args.FVA.ETD != null)
            {
                DateTime myDateTime = args.FVA.ETD.Value;

                string myMonth = String.Format("{0:MMM}", myDateTime, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")).ToUpper();
                string myDay = String.Format("{0:00}", myDateTime.Day);

                xmlItem.ScheduleAndAvailabilityInformationRequestDetails.Date.DayOfMonth = myDay;
                xmlItem.ScheduleAndAvailabilityInformationRequestDetails.Date.Month = myMonth;

                //if (myDateTime.Hour > 0 || myDateTime.Minute > 0)
                //{
                //    string myTime = String.Format("{0:00}", myDateTime.Hour) + String.Format("{0:00}", myDateTime.Minute);

                //    xmlItem.ScheduleAndAvailabilityInformationRequestDetails.EarliestDepartureTime = new CHAMP17.EarliestDepartureTime()
                //    {
                //        Time = myTime
                //    };
                //}
            }
            #endregion

            #region ETA
            //if (args.FVA.ETA != null)
            //{
            //    DateTime myDateTime = args.FVA.ETA.Value;

            //    string myMonth = String.Format("{0:MMM}", myDateTime, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")).ToUpper();
            //    string myDay = String.Format("{0:00}", myDateTime.Day);

            //    xmlItem.ScheduleAndAvailabilityInformationRequestDetails.LatestArrivalDateTime = new CHAMP17.LatestArrivalDateTime()
            //    {
            //        DayOfMonth = myDay,
            //        Month = myMonth,
            //    };

            //    if (myDateTime.Hour > 0 || myDateTime.Minute > 0)
            //    {
            //        string myTime = String.Format("{0:00}", myDateTime.Hour) + String.Format("{0:00}", myDateTime.Minute);

            //        xmlItem.ScheduleAndAvailabilityInformationRequestDetails.LatestArrivalDateTime.Time = myTime;
            //        xmlItem.ScheduleAndAvailabilityInformationRequestDetails.LatestArrivalDateTime.TimeSpecified = true;
            //    }
            //}
            #endregion

            #region Volume
            if (args.FVA.Volume != null && !string.IsNullOrEmpty(args.FVA.VolumeUnitCode))
            {
                xmlItem.ScheduleAndAvailabilityInformationRequestDetails.Volume = new CHAMP17.VolumeInfo()
                {
                    VolumeAmount = args.FVA.Volume.Value,
                    VolumeCode = ConvertVolumeUnitToChamp(args.FVA.VolumeUnitCode),
                };
            }
            #endregion

            #region Weight
            if (args.FVA.GrossWeight != null && !string.IsNullOrEmpty(args.FVA.GrossWeightUnitCode))
            {
                xmlItem.ScheduleAndAvailabilityInformationRequestDetails.Weight = new CHAMP17.WeightInfo()
                {
                    WeightAmount = args.FVA.GrossWeight == null ? 0 : MethodHelper.Normalize(args.FVA.GrossWeight.Value),
                    WeightCode = ConvertGrossWeightUnitToChamp(args.FVA.GrossWeightUnitCode),
                };
            }
            #endregion
            #endregion

            #region OtherServiceInformation
            if (!string.IsNullOrEmpty(args.FVA.AnswerOSI))
            {
                xmlItem.OtherServiceInformation = new string[] { FormatHelper.FormatString(args.FVA.AnswerOSI, FormatHelper.PatternType.Text, 65) };
            }
            #endregion

            #region ReasonForNoDisplay
            if (!string.IsNullOrEmpty(args.FVA.AnswerReasonForNoReply))
            {
                xmlItem.ReasonForNoDisplay = FormatHelper.FormatString(args.FVA.AnswerReasonForNoReply, FormatHelper.PatternType.Text, 65);
            }
            #endregion

            #region ScheduleInformation
            if (string.IsNullOrEmpty(args.FVA.AnswerReasonForNoReply))
            {
                List<CHAMP17.ScheduleInformation> myScheduleInformationsList = new List<CHAMP17.ScheduleInformation>();

                #region myScheduleInformation1

                CHAMP17.ScheduleInformation myScheduleInformation1 = new CHAMP17.ScheduleInformation()
                {
                    ScheduleAndAvailabilityInformationAnswerDetails = new CHAMP17.ScheduleAndAvailabilityInformationAnswerDetails()
                    {
                        //Date
                        //FlightDetails
                    },

                    //AvailabilitySupplementaryDetails
                };

                #region Date
                if (args.FVA.ETD != null)
                {
                    DateTime myETD = args.FVA.ETD.Value;
                    string myMonth = String.Format("{0:MMM}", myETD, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")).ToUpper();
                    string myDay = String.Format("{0:00}", myETD.Day);

                    myScheduleInformation1.ScheduleAndAvailabilityInformationAnswerDetails.Date = new CHAMP17.Date()
                    {
                        Month = myMonth,
                        DayOfMonth = myDay,
                    };
                }
                #endregion

                #region AvailabilitySupplementaryDetails
                if ((args.FVA.Volume != null && args.FVA.VolumeUnitCode != null) || (args.FVA.GrossWeight != null && args.FVA.GrossWeightUnitCode != null))
                {
                    List<CHAMP17.AvailabilitySupplementaryDetails> mySupplementaryDetailsList = new List<CHAMP17.AvailabilitySupplementaryDetails>();

                    CHAMP17.AvailabilitySupplementaryDetails mySupplementary1 = new CHAMP17.AvailabilitySupplementaryDetails();

                    if (args.FVA.Volume != null && args.FVA.VolumeUnitCode != null)
                    {
                        mySupplementary1.Volume = new CHAMP17.VolumeInfo()
                        {
                            VolumeAmount = args.FVA.Volume.Value,
                            VolumeCode = ConvertVolumeUnitToChamp(args.FVA.VolumeUnitCode),
                        };
                    }

                    if (args.FVA.GrossWeight != null && args.FVA.GrossWeightUnitCode != null)
                    {
                        mySupplementary1.Weight = new CHAMP17.WeightInfo()
                        {
                            WeightAmount = args.FVA.GrossWeight.Value,
                            WeightCode = ConvertGrossWeightUnitToChamp(args.FVA.GrossWeightUnitCode),
                        };
                    }

                    mySupplementaryDetailsList.Add(mySupplementary1);
                    myScheduleInformation1.AvailabilitySupplementaryDetails = mySupplementaryDetailsList.ToArray();
                }
                #endregion

                #region FlightDetails
                List<CHAMP17.FVAFlightDetails> myFlightDetailsList = new List<CHAMP17.FVAFlightDetails>();

                myFlightDetailsList.Add(new CHAMP17.FVAFlightDetails()
                {
                    NumberOfStops = 0,

                    RequestedRouting = new CHAMP17.RequestedRouting()
                    {
                        AirportCityCodeOfDeparture = xmlItem.ScheduleAndAvailabilityInformationRequestDetails.RequestedRouting.AirportCityCodeOfDeparture,
                        AirportCityCodeOfArrival = xmlItem.ScheduleAndAvailabilityInformationRequestDetails.RequestedRouting.AirportCityCodeOfArrival,
                    },

                    Flight = new CHAMP17.Flight()
                    {
                        CarrierCode = xmlItem.ScheduleAndAvailabilityInformationRequestDetails.Flight.CarrierCode,
                        FlightNumber = "659",
                    },

                    AircraftTypeCode = "343",

                    Schedule = new CHAMP17.Schedule()
                    {
                        TimeOfScheduledArrival = 1700,
                        TimeOfScheduledDeparture = 1500,
                        //ArrivalDayChangeIndicator = 
                        //DepartureDayChangeIndicator
                    },

                    //ScheduleSupplementaryInformation = "",
                });

                myScheduleInformation1.ScheduleAndAvailabilityInformationAnswerDetails.FlightDetails = myFlightDetailsList.ToArray();
                #endregion

                myScheduleInformationsList.Add(myScheduleInformation1);
                #endregion

                #region myScheduleInformation2

                CHAMP17.ScheduleInformation myScheduleInformation2 = new CHAMP17.ScheduleInformation()
                {
                    ScheduleAndAvailabilityInformationAnswerDetails = new CHAMP17.ScheduleAndAvailabilityInformationAnswerDetails()
                    {
                        //Date
                        //FlightDetails
                    },

                    //AvailabilitySupplementaryDetails
                };

                #region Date
                if (args.FVA.ETD != null)
                {
                    DateTime myETD = args.FVA.ETD.Value;
                    string myMonth = String.Format("{0:MMM}", myETD, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")).ToUpper();
                    string myDay = String.Format("{0:00}", myETD.Day);

                    myScheduleInformation2.ScheduleAndAvailabilityInformationAnswerDetails.Date = new CHAMP17.Date()
                    {
                        Month = myMonth,
                        DayOfMonth = myDay,
                    };
                }
                #endregion

                #region AvailabilitySupplementaryDetails
                if ((args.FVA.Volume != null && args.FVA.VolumeUnitCode != null) || (args.FVA.GrossWeight != null && args.FVA.GrossWeightUnitCode != null))
                {
                    List<CHAMP17.AvailabilitySupplementaryDetails> mySupplementaryDetailsList = new List<CHAMP17.AvailabilitySupplementaryDetails>();

                    CHAMP17.AvailabilitySupplementaryDetails mySupplementary1 = new CHAMP17.AvailabilitySupplementaryDetails();

                    if (args.FVA.Volume != null && args.FVA.VolumeUnitCode != null)
                    {
                        mySupplementary1.Volume = new CHAMP17.VolumeInfo()
                        {
                            VolumeAmount = args.FVA.Volume.Value,
                            VolumeCode = ConvertVolumeUnitToChamp(args.FVA.VolumeUnitCode),
                        };
                    }

                    if (args.FVA.GrossWeight != null && args.FVA.GrossWeightUnitCode != null)
                    {
                        mySupplementary1.Weight = new CHAMP17.WeightInfo()
                        {
                            WeightAmount = args.FVA.GrossWeight.Value,
                            WeightCode = ConvertGrossWeightUnitToChamp(args.FVA.GrossWeightUnitCode),
                        };
                    }

                    mySupplementaryDetailsList.Add(mySupplementary1);
                    myScheduleInformation1.AvailabilitySupplementaryDetails = mySupplementaryDetailsList.ToArray();
                }
                #endregion

                #region FlightDetails
                List<CHAMP17.FVAFlightDetails> myFlightDetailsList2 = new List<CHAMP17.FVAFlightDetails>();

                string viaPort = "ABC";

                myFlightDetailsList2.Add(new CHAMP17.FVAFlightDetails()
                {
                    NumberOfStops = 0,

                    RequestedRouting = new CHAMP17.RequestedRouting()
                    {
                        AirportCityCodeOfDeparture = xmlItem.ScheduleAndAvailabilityInformationRequestDetails.RequestedRouting.AirportCityCodeOfDeparture,
                        AirportCityCodeOfArrival = viaPort, //xmlItem.ScheduleAndAvailabilityInformationRequestDetails.RequestedRouting.AirportCityCodeOfArrival,
                    },

                    Flight = new CHAMP17.Flight()
                    {
                        CarrierCode = xmlItem.ScheduleAndAvailabilityInformationRequestDetails.Flight.CarrierCode,
                        FlightNumber = "111",
                    },

                    AircraftTypeCode = "343",

                    //Schedule = new CHAMP17.Schedule()
                    //{
                    //    TimeOfScheduledArrival = 1700,
                    //    TimeOfScheduledDeparture = 1500,
                    //    //ArrivalDayChangeIndicator = 
                    //    //DepartureDayChangeIndicator
                    //},

                    //ScheduleSupplementaryInformation = "",
                });

                myFlightDetailsList2.Add(new CHAMP17.FVAFlightDetails()
                {
                    NumberOfStops = 0,

                    RequestedRouting = new CHAMP17.RequestedRouting()
                    {
                        AirportCityCodeOfDeparture = viaPort,
                        AirportCityCodeOfArrival = xmlItem.ScheduleAndAvailabilityInformationRequestDetails.RequestedRouting.AirportCityCodeOfArrival,
                    },

                    Flight = new CHAMP17.Flight()
                    {
                        CarrierCode = xmlItem.ScheduleAndAvailabilityInformationRequestDetails.Flight.CarrierCode,
                        FlightNumber = "222",
                    },

                    AircraftTypeCode = "343",

                    Schedule = new CHAMP17.Schedule()
                    {
                        TimeOfScheduledArrival = 2100,
                        TimeOfScheduledDeparture = 1900,
                        //ArrivalDayChangeIndicator = 
                        //DepartureDayChangeIndicator
                    },

                    //ScheduleSupplementaryInformation = "",
                });

                myScheduleInformation2.ScheduleAndAvailabilityInformationAnswerDetails.FlightDetails = myFlightDetailsList2.ToArray();
                #endregion

                //myScheduleInformationsList.Add(myScheduleInformation2);
                #endregion

                xmlItem.ScheduleInformation = myScheduleInformationsList.ToArray();
            }
            #endregion

            this.Envelope.Item = xmlItem;
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
    }
}
