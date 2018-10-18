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
        private void SimulateFSA()
        {
            string myFromPortCode = this.GetPortCode(args.FSA.FromPortId);
            string myToPortCode = this.GetPortCode(args.FSA.ToPortId);

            CHAMP17.SplitConsignment mySplitConsignment = new CHAMP17.SplitConsignment()
            {
                MasterAWBConsignmentDetail = new CHAMP17.FSA_ConsignmentDetail()
                {
                    AWBIdentification = new CHAMP17.AWBIdentification()
                    {
                        AirlinePrefix = args.AirlinePrefix,
                        AWBSerialNumber = args.Master,
                    },

                    AWBOriginAndDestination = new CHAMP17.AWBOriginAndDestination()
                    {
                        AirportCityCodeOfOrigin = myFromPortCode,
                        AirportCityCodeOfDestination = myToPortCode,
                    },

                    QuantityDetail = new CHAMP17.QuantityDetail()
                    {
                        NumberOfPieces = args.FSA.NumberOfPieces,
                        Weight = args.FSA.Weight == null ? 0 : MethodHelper.Normalize(args.FSA.Weight.Value),
                        WeightCode = "K",
                    },
                },
            };

            if (!string.IsNullOrEmpty(args.FSA.OSIFirstLine) || !string.IsNullOrEmpty(args.FSA.OSISecondLine))
            {
                mySplitConsignment.OtherServiceInformation = new CHAMP17.FSAOtherServiceInformation();

                if (!string.IsNullOrEmpty(args.FSA.OSIFirstLine))
                {
                    mySplitConsignment.OtherServiceInformation.OSIDetailsFirstLine = args.FSA.OSIFirstLine.ToUpper();
                }

                if (!string.IsNullOrEmpty(args.FSA.OSISecondLine))
                {
                    mySplitConsignment.OtherServiceInformation.OSIDetailsSecondLine = args.FSA.OSISecondLine.ToUpper();
                }
            }

            if (args.FSA.AllStatusCodes.Count > 0)
            {
                string[] months = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };

                List<CHAMP17.StatusDetail> list = new List<CHAMP17.StatusDetail>();

                foreach (string statusCode in args.FSA.AllStatusCodes)
                {
                    CHAMP17.StatusDetail myStatusDetail = new CHAMP17.StatusDetail()
                    {
                        StatusCode = statusCode,

                        MovementDetail = new CHAMP17.MovementDetail()
                        {
                            Day = args.FSA.EventDate != null ? args.FSA.EventDate.Value.Day : 0,
                            DaySpecified = true,
                            Month = args.FSA.EventDate != null ? months[args.FSA.EventDate.Value.Month - 1] : null,
                            ActualTimeSpecified = true,
                            ActualTime = args.FSA.EventDate != null ? (args.FSA.EventDate.Value.Hour * 100) + args.FSA.EventDate.Value.Minute : 0,
                            AirportCityCodeOfDeparture = myFromPortCode,
                            AirportCityCodeOfArrival = myToPortCode,
                            FlightNumber = args.FSA.FlightNumber,
                            CarrierCode = args.FSA.CarrierCode,
                        },

                        QuantityDetail = new CHAMP17.QuantityDetail()
                        {
                            NumberOfPieces = args.FSA.NumberOfPieces,
                            WeightCode = "K",
                            Weight = args.FSA.Weight == null ? 0 : args.FSA.Weight.Value,
                            ShipmentDescriptionCode = args.FSA.EntityNumberOfPieces == args.FSA.NumberOfPieces ? "T" : "P",
                        },

                        TimeOfDepartureInfo = new CHAMP17.TimeOfDepartureInfo()
                        {
                            DayChangeIndicator = args.FSA.ArrivalDayChangeIndicator,
                            Time = args.FSA.DepartureTime,
                            TypeOfTimeIndicator = args.FSA.TimeOfDepartureInfo,
                        },

                        TimeOfArrivalInfo = new CHAMP17.TimeOfArrivalInfo()
                        {
                            DayChangeIndicator = args.FSA.ArrivalDayChangeIndicator,
                            Time = args.FSA.ArrivalTime,
                            TypeOfTimeIndicator = args.FSA.TimeOfArrivalInfo,
                        },
                    };

                    list.Add(myStatusDetail);
                }

                mySplitConsignment.StatusDetail = list.ToArray();
            }

            this.Envelope.Item = new CHAMP17.StatusAnswer()
            {
                StandardMessageIdentification = new CHAMP17.StandardMessageIdentification()
                {
                    MessageTypeVersionNumber = 1,
                    StandardMessageIdentifier = args.MessageIdentifier,
                },

                SplitConsignment = new CHAMP17.SplitConsignment[] { mySplitConsignment }
            };
        }

    }
}
