using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.XSD.Simulators.CHAMPSimulators
{
    public partial class CHAMPSimulator
    {        
        private void SimulateFFA()
        {
            string myMainCarriageFromPortCode = this.GetPortCode(args.FFA.MainCarriageFromPortId);
            string myMainCarriageToPortCode = this.GetPortCode(args.FFA.MainCarriageToPortId);

            CHAMP17.AWBSpaceAllocationAnswer envelopeItem = new CHAMP17.AWBSpaceAllocationAnswer()
            {
                StandardMessageIdentification = new CHAMP17.StandardMessageIdentification()
                {
                    StandardMessageIdentifier = "FFA",
                    MessageTypeVersionNumber = 7,
                },

                ConsignmentDetail = new CHAMP17.ConsignmentDetail_FFA()
                {
                    AWBIdentification = new CHAMP17.AWBIdentification()
                    {
                        AirlinePrefix = args.AirlinePrefix,
                        AWBSerialNumber = args.Master,
                    },

                    AWBOriginAndDestination = new CHAMP17.AWBOriginAndDestination()
                    {
                        AirportCityCodeOfOrigin = myMainCarriageFromPortCode,
                        AirportCityCodeOfDestination = myMainCarriageToPortCode,
                    },

                    QuantityDetail = new CHAMP17.QuantityDetail_FFA()
                    {
                        NumberOfPieces = args.FFA.NumberOfPieces,
                        Weight = args.FFA.Weight == null ? 0 : MethodHelper.Normalize(args.FFA.Weight.Value),
                        WeightCode = "K",
                        ShipmentDescriptionCode = "T",
                    },                    
                },
            };

            if (!string.IsNullOrEmpty(args.FFA.DescriptionOfGoods))
            {
                envelopeItem.ConsignmentDetail.NatureOfGoods = args.FFA.DescriptionOfGoods;
            }

            #region FlightDetails
            List<CHAMP17.FlightDetails_FFA> flightDetails = new List<CHAMP17.FlightDetails_FFA>();

            #region MainCarriage
            flightDetails.Add(new CHAMP17.FlightDetails_FFA()
            {
                AirportsOfDepartureAndArrival = new CHAMP17.AWBOriginAndDestination()
                {
                    AirportCityCodeOfOrigin = myMainCarriageFromPortCode,
                    AirportCityCodeOfDestination = myMainCarriageToPortCode,                  
                },

                FlightIdentification = new CHAMP17.FlightIdentification_FFA()
                {
                    CarrierCode = args.FFA.MainCarriageCarrierCode,
                    DayOfScheduledDeparture = args.FFA.MainCarriageETD.Value.Day,
                    FlightNumber = args.FFA.MainCarriageCarrierNumber,
                    MonthOfScheduledDeparture = String.Format("{0:MMM}", args.FFA.MainCarriageETD.Value),
                },

                SpaceAllocationCode = args.FFA.MainCarriageSpaceAllocationCode,
            });
            #endregion

            #region Transshipment1
            if (!string.IsNullOrEmpty(args.FFA.Transshipment1FromPortId) && !string.IsNullOrEmpty(args.FFA.Transshipment1ToPortId))
            {
                string myFromPortCode = this.GetPortCode(args.FFA.Transshipment1FromPortId);
                string myToPortCode = this.GetPortCode(args.FFA.Transshipment1ToPortId);

                flightDetails.Add(new CHAMP17.FlightDetails_FFA()
                {
                    AirportsOfDepartureAndArrival = new CHAMP17.AWBOriginAndDestination()
                    {
                        AirportCityCodeOfDestination = myFromPortCode,
                        AirportCityCodeOfOrigin = myToPortCode,
                    },

                    FlightIdentification = new CHAMP17.FlightIdentification_FFA()
                    {
                        CarrierCode = args.FFA.Transshipment1CarrierCode,
                        DayOfScheduledDeparture = args.FFA.Transshipment1ETD.Value.Day,
                        FlightNumber = args.FFA.Transshipment1CarrierNumber,
                        MonthOfScheduledDeparture = String.Format("{0:MMM}", args.FFA.Transshipment1ETD.Value),
                    },

                    SpaceAllocationCode = args.FFA.Transshipment1SpaceAllocationCode,
                });
            }
            #endregion

            #region Transshipment2
            if (!string.IsNullOrEmpty(args.FFA.Transshipment2FromPortId) && !string.IsNullOrEmpty(args.FFA.Transshipment2ToPortId))
            {
                string myFromPortCode = this.GetPortCode(args.FFA.Transshipment2FromPortId);
                string myToPortCode = this.GetPortCode(args.FFA.Transshipment2ToPortId);

                flightDetails.Add(new CHAMP17.FlightDetails_FFA()
                {
                    AirportsOfDepartureAndArrival = new CHAMP17.AWBOriginAndDestination()
                    {
                        AirportCityCodeOfDestination = myFromPortCode,
                        AirportCityCodeOfOrigin = myToPortCode,
                    },

                    FlightIdentification = new CHAMP17.FlightIdentification_FFA()
                    {
                        CarrierCode = args.FFA.Transshipment2CarrierCode,
                        DayOfScheduledDeparture = args.FFA.Transshipment2ETD.Value.Day,
                        FlightNumber = args.FFA.Transshipment2CarrierNumber,
                        MonthOfScheduledDeparture = String.Format("{0:MMM}", args.FFA.Transshipment2ETD.Value),
                    },

                    SpaceAllocationCode = args.FFA.Transshipment2SpaceAllocationCode,
                });
            }
            #endregion

            if (flightDetails.Count > 0)
            {
                envelopeItem.FlightDetails = flightDetails.ToArray();
            }
            #endregion

            #region OSIs
            if (!string.IsNullOrEmpty(args.FFA.OSIFirstLine) || !string.IsNullOrEmpty(args.FFA.OSISecondLine))
            {
                envelopeItem.OtherServiceInformation = new CHAMP17.OtherServiceInformation_FFA();

                if (!string.IsNullOrEmpty(args.FFA.OSIFirstLine))
                {
                    envelopeItem.OtherServiceInformation.OSIDetailsFirstLine = args.FFA.OSIFirstLine.ToUpper();
                }

                if (!string.IsNullOrEmpty(args.FFA.OSISecondLine))
                {
                    envelopeItem.OtherServiceInformation.OSIDetailsSecondLine = args.FFA.OSISecondLine.ToUpper();
                }
            }
            #endregion

            #region SSR
            if (!string.IsNullOrEmpty(args.FFA.SpecialServicesRequest))
            {
                List<string> mySSRTextList = this.BuildSSRTextList(args.FFA.SpecialServicesRequest);
                if (mySSRTextList.Count > 0)
                {
                    envelopeItem.SpecialServiceRequest = mySSRTextList.ToArray();
                }
            }
            #endregion

            this.Envelope.Item = envelopeItem;
        }

        private List<string> BuildSSRTextList(string mySSRText)
        {
            List<string> mySSRTextList = new List<string>();

            if (!string.IsNullOrEmpty(mySSRText))
            {
                int itemLength = 65;
                string xmlField = "";
                string systemField = mySSRText;

                if (systemField != null)
                {
                    systemField = Regex.Replace(systemField, @"(\r)(\1)+", "$1");
                    systemField = systemField.Replace("\r", " ");
                    systemField = FormatHelper.FormatString(systemField, FormatHelper.PatternType.Text);
                }

                for (int i = 1; i <= 3; i++)
                {
                    systemField = systemField.Trim();

                    if (string.IsNullOrEmpty(systemField))
                    {
                        break;
                    }

                    else
                    {
                        if (systemField.Length <= itemLength)
                        {
                            xmlField = systemField.ToUpper();
                            systemField = "";
                        }

                        else
                        {
                            xmlField = systemField.Substring(0, itemLength).ToUpper();
                            systemField = systemField.Remove(0, itemLength);
                        }

                        xmlField = xmlField.Trim();
                        mySSRTextList.Add(xmlField);
                    }
                }
            }

            return mySSRTextList;
        }
    }
}
