using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.Simulators.GLSHKSimulators
{
    public partial class GLSHKSimulator
    {
        string myAirlineCode = null;
        string myFromPortCode = null;
        string myToPortCode = null;

        private void SimulateFSA()
        {
            this.myAirlineCode = this.GetCardCode(args.AirlineId);
            this.myFromPortCode = this.GetPortCode(args.FSA.FromPortId);
            this.myToPortCode = this.GetPortCode(args.FSA.ToPortId);

            GLSHK.FSUAWB myFSUAWB = new GLSHK.FSUAWB()
            {
                AWB = new GLSHK.AWBID()
                {
                    Prefix = args.AirlinePrefix,
                    SerialNum = args.Master,
                },

                AWBOD = new GLSHK.OriginDestination()
                {
                    Origin = myFromPortCode,
                    Destination = myToPortCode,
                },

                Quantity = new GLSHK.QTYDetail()
                {
                    Pieces = args.FSA.NumberOfPieces.ToString(),
                    Weight = args.FSA.Weight == null ? 0 : args.FSA.Weight.Value,
                    WeightCode = GLSHK.QTYDetailWeightCode.K,
                    WeightSpecified = true,
                    WeightCodeSpecified = true,
                },

                FSUStatusDetail = new GLSHK.FSUStatusDetail[1],
            };

            if (args.FSA.AllStatusCodes.Count > 0)
            {                                             
                List<GLSHK.FSUStatusDetail> list = new List<GLSHK.FSUStatusDetail>();

                foreach (string statusCode in args.FSA.AllStatusCodes)
                {
                    GLSHK.FSUStatusDetail myStatusDetail = new GLSHK.FSUStatusDetail()
                    {
                         
                    };

                    list.Add(myStatusDetail);

                    switch (statusCode)
                    {
                        case "RCS":
                            {
                                myStatusDetail.RCSDetail = new GLSHK.RCSDetail()
                                {
                                    Quantity = this.GetStatusQuantity(),
                                    Movement = this.GetStatusMovement(),
                                };

                                break;
                            }

                        case "RCT":
                            {
                                myStatusDetail.RCTDetail = new GLSHK.RCTDetail()
                                {
                                    Quantity = this.GetStatusQuantity(),
                                    Movement = this.GetStatusMovement(),
                                };

                                break;
                            }

                        case "RCF":
                            {
                                myStatusDetail.RCFDetail = new GLSHK.RCFDetail()
                                {
                                    Quantity = this.GetStatusQuantity(),
                                    Movement = this.GetStatusMovement(),
                                    ARRTime = new GLSHK.FlightTime() 
                                    { 
                                        Time = args.FSA.ArrivalTime.ToString(),
                                        TimeType = args.FSA.TimeOfArrivalInfo,
                                    } ,
                                    DEPTime = new GLSHK.FlightTime() 
                                    { 
                                        Time = args.FSA.DepartureTime.ToString(),
                                        TimeType = args.FSA.TimeOfDepartureInfo,
                                    },
                                };

                                break;
                            }

                        case "BKD":
                            {
                                myStatusDetail.BKDDetail = new GLSHK.BKDDetail()
                                {
                                    Quantity = this.GetStatusQuantity(),
                                    Movement = this.GetStatusMovement_DEPARR(),
                                    ARRTime = new GLSHK.FlightTime()
                                    {
                                        Time = args.FSA.ArrivalTime.ToString(),
                                        TimeType = args.FSA.TimeOfArrivalInfo,
                                    },
                                    DEPTime = new GLSHK.FlightTime()
                                    {
                                        Time = args.FSA.DepartureTime.ToString(),
                                        TimeType = args.FSA.TimeOfDepartureInfo,
                                    },
                                };

                                break;
                            }

                        case "ARR":
                            {
                                myStatusDetail.ARRDetail = new GLSHK.ARRDetail()
                                {
                                    Quantity = this.GetStatusQuantity(),
                                    Movement = this.GetStatusMovement(),
                                    ARRTime = new GLSHK.FlightTime()
                                    {
                                        Time = args.FSA.ArrivalTime.ToString(),
                                        TimeType = args.FSA.TimeOfArrivalInfo,
                                    },
                                    DEPTime = new GLSHK.FlightTime()
                                    {
                                        Time = args.FSA.DepartureTime.ToString(),
                                        TimeType = args.FSA.TimeOfDepartureInfo,
                                    },
                                };

                                break;
                            }
                    }
                }

                myFSUAWB.FSUStatusDetail = list.ToArray();
            }

            GLSHK.FSU myFSU = new GLSHK.FSU()
            {
                FSUAWB = new GLSHK.FSUAWB[1] { myFSUAWB }
            };

            if (!string.IsNullOrEmpty(args.FSA.OSIFirstLine) || !string.IsNullOrEmpty(args.FSA.OSISecondLine))
            {
                List<GLSHK.OtherServiceInfo> list = new List<GLSHK.OtherServiceInfo>();

                if (!string.IsNullOrEmpty(args.FSA.OSIFirstLine))
                {
                    list.Add(new GLSHK.OtherServiceInfo()
                    {
                        OtherServiceInformation = args.FSA.OSIFirstLine.ToUpper()
                    });
                }

                if (!string.IsNullOrEmpty(args.FSA.OSISecondLine))
                {
                    list.Add(new GLSHK.OtherServiceInfo()
                    {
                        OtherServiceInformation = args.FSA.OSISecondLine.ToUpper()
                    });
                }

                myFSU.OtherServiceInfo = list.ToArray();
            }

            this.Message.Item = myFSU;
        }

        private GLSHK.MovementDetail GetStatusMovement()
        {
            string[] months = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };

            GLSHK.MovementDetail myResult = new GLSHK.MovementDetail()
            {
                Day = args.FSA.EventDate != null ? args.FSA.EventDate.Value.Day.ToString() : "0",
                Month = args.FSA.EventDate != null ? months[args.FSA.EventDate.Value.Month - 1] : null,
                ActualTime = args.FSA.EventDate != null ? (args.FSA.EventDate.Value.Hour * 100 + args.FSA.EventDate.Value.Minute).ToString() : "0",
                AirportCode = this.myFromPortCode,
                FlightNum = args.FSA.FlightNumber,
                DayChangeIndicator = args.FSA.DepartureDayChangeIndicator,
                CarrierCode = this.myAirlineCode,
            };

            return myResult;
        }

        private GLSHK.MovementDetail_DEPARR GetStatusMovement_DEPARR()
        {
            string[] months = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };

            GLSHK.MovementDetail_DEPARR myResult = new GLSHK.MovementDetail_DEPARR()
            {
                Day = args.FSA.EventDate != null ? args.FSA.EventDate.Value.Day.ToString() : "0",
                Month = args.FSA.EventDate != null ? months[args.FSA.EventDate.Value.Month - 1] : null,
                ActualTime = args.FSA.EventDate != null ? (args.FSA.EventDate.Value.Hour * 100 + args.FSA.EventDate.Value.Minute).ToString() : "0",
                DEPAirport = this.myFromPortCode,
                ARRAirport = this.myToPortCode,
                FlightNum = args.FSA.FlightNumber,
                DayChangeIndicator = args.FSA.DepartureDayChangeIndicator,
                CarrierCode = this.myAirlineCode,
            };

            return myResult;
        }

        private GLSHK.QTYDetail GetStatusQuantity()
        {
            GLSHK.QTYDetail myResult = new GLSHK.QTYDetail()
            {
                Pieces = args.FSA.NumberOfPieces.ToString(),
                Weight = args.FSA.Weight == null ? 0 : args.FSA.Weight.Value,
                WeightCode = GLSHK.QTYDetailWeightCode.K,
                WeightSpecified = true,
                WeightCodeSpecified = true,
            };

            return myResult;
        }
    }
}
