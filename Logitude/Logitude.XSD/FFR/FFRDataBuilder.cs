using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Logitude.BL.Helpers;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Logitude.BookingLib.Data.Repositories;

namespace Logitude.XSD.FFR
{
    public class FFRDataBuilder
    {
        public FFRDataContext Context { get; set; }
        private List<BookingLastRequest> requestsList;
        public FFRDataBuilder(FFRDataContext myContext, BookingLastRequestRepository lastRequestRepository)
        {
            this.Context = myContext;

            requestsList = lastRequestRepository.GetBookingLastRequestsByBookingId(myContext.BookingId, myContext.Tenant).OrderBy(d => d.ETD).ToList();
        }

        public CHAMP17.AWBSpaceAllocationRequest GetBookingFFR(bool iscancelled)
        {
            bool isBookingUpdate = false;

            //if (Context.BookingStatusCode == "CNF" || (Context.WaitingForResponse && Context.FFRStatusCode == "BRQ"))
            if(Context.BookingStatusCode != "CRT")
            {
                isBookingUpdate = true;
            }

            CHAMP17.AWBSpaceAllocationRequest myXSDElement = new CHAMP17.AWBSpaceAllocationRequest();

            #region [1] Standard Message Identification
            myXSDElement.StandardMessageIdentification = new CHAMP17.StandardMessageIdentification()
            {
                MessageTypeVersionNumber = 7,
                StandardMessageIdentifier = "FFR"
            };
            #endregion

            #region[2] Consignment Details
            myXSDElement.ConsignmentDetail = new CHAMP17.ConsignmentDetail_FFR()
            {
                AWBIdentification = new CHAMP17.AWBIdentification()
                {
                    AirlinePrefix = Context.AirlinePrefix, // fixed type
                    AWBSerialNumber = Context.Master, // fixed type
                },

                AWBOriginAndDestination = new CHAMP17.AWBOriginAndDestination()
                {
                    AirportCityCodeOfDestination = Context.FinalDestinationPortCode,
                    AirportCityCodeOfOrigin = Context.MainCarriageFromPortCode,
                },

                QuantityDetail = new CHAMP17.QuantityDetail_FFR()
                {
                    NumberOfPieces = Context.NumberOfPackages,
                    ShipmentDescriptionCode = "T",
                    Weight = Context.GrossWeight,
                    WeightCode = Context.GrossWeightUnitCode,
                },

                Volume = new CHAMP17.Volume_FFR()
                {
                    VolumeCode = Context.VolumeUnitCode,
                    VolumeAmount = Context.Volume,
                },

                //Density = new Density_FFR()
                //{
                //    DensityGroup = 1,
                //    DensityIndicator = "",
                //},

                //TotalConsignmentPieces = new TotalConsignmentPieces()
                //{
                //    NumberOfPieces = 1,
                //    ShipmentDescriptionCode = "",
                //},
            };

            if (!string.IsNullOrEmpty(Context.NatureOfGoods))
            {
                myXSDElement.ConsignmentDetail.NatureOfGoods = Context.NatureOfGoods;
            }

            if (Context.HandlingCodesList.Count > 0)
            {
                myXSDElement.ConsignmentDetail.SpecialHandlingRequirements = Context.HandlingCodesList.ToArray<string>();
            }
            #endregion

            #region [3] Flight Details

            List<CHAMP17.FlightDetails_FFR> flightDetails = new List<CHAMP17.FlightDetails_FFR>();

            if (isBookingUpdate && !iscancelled)
            {
                foreach (BookingLastRequest item in requestsList)
                {
                    int day = 0;
                    Int32.TryParse(String.Format("{0:dd}", item.ETD.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), out day);

                    flightDetails.Add(new CHAMP17.FlightDetails_FFR()
                    {
                        AirportsOfDepartureAndArrival = new CHAMP17.AWBOriginAndDestination()
                        {
                            AirportCityCodeOfDestination = item.Destination,
                            AirportCityCodeOfOrigin = item.Origin,
                        },

                        FlightIdentification = new CHAMP17.FlightIdentification_FFR()
                        {
                            CarrierCode = item.Carrier.Code,
                            DayOfScheduledDeparture = FormatHelper.FormatInteger(2, day),
                            FlightNumber = item.FlightNumber.Substring(2),
                            MonthOfScheduledDeparture = String.Format("{0:MMM}", item.ETD.Value),
                        },

                        AllotmentIdentification = Context.MainCarriageAllotmentIdentification,
                        SpaceAllocationCode = "XX",
                    });
                }
            }

            flightDetails.Add(new CHAMP17.FlightDetails_FFR()
            {
                AirportsOfDepartureAndArrival = new CHAMP17.AWBOriginAndDestination()
                {
                    AirportCityCodeOfDestination = Context.MainCarriageToPortCode,
                    AirportCityCodeOfOrigin = Context.MainCarriageFromPortCode,
                },

                FlightIdentification = new CHAMP17.FlightIdentification_FFR()
                {
                    CarrierCode = Context.MainCarriageCarrierCode,
                    DayOfScheduledDeparture = Context.MainCarriageCarrierETDDay, // fixed type
                    FlightNumber = Context.MainCarriageCarrierNumber,
                    MonthOfScheduledDeparture = Context.MainCarriageCarrierETDMonth,
                },

                AllotmentIdentification = Context.MainCarriageAllotmentIdentification,
                SpaceAllocationCode = iscancelled ? "XX" : Context.MainCarriageSpaceAllocationCode,
            });

            if (!string.IsNullOrEmpty(Context.Transshipment1CarrierCode) && !string.IsNullOrEmpty(Context.Transshipment1CarrierNumber) && !string.IsNullOrEmpty(Context.Transshipment1CarrierETDDay))
            {
                flightDetails.Add(new CHAMP17.FlightDetails_FFR()
                {
                    AirportsOfDepartureAndArrival = new CHAMP17.AWBOriginAndDestination()
                    {
                        AirportCityCodeOfDestination = Context.Transshipment1ToPortCode,
                        AirportCityCodeOfOrigin = Context.Transshipment1FromPortCode,
                    },

                    FlightIdentification = new CHAMP17.FlightIdentification_FFR()
                    {
                        CarrierCode = Context.Transshipment1CarrierCode,
                        DayOfScheduledDeparture = Context.Transshipment1CarrierETDDay, // fixed type
                        FlightNumber = Context.Transshipment1CarrierNumber,
                        MonthOfScheduledDeparture = Context.Transshipment1CarrierETDMonth,
                    },

                    AllotmentIdentification = Context.Transshipment1AllotmentIdentification,
                    SpaceAllocationCode = iscancelled ? "XX" : Context.Transshipment1SpaceAllocationCode,
                });
            }

            if (!string.IsNullOrEmpty(Context.Transshipment2CarrierCode) && !string.IsNullOrEmpty(Context.Transshipment2CarrierNumber) && !string.IsNullOrEmpty(Context.Transshipment2CarrierETDDay))
            {
                flightDetails.Add(new CHAMP17.FlightDetails_FFR()
                {
                    AirportsOfDepartureAndArrival = new CHAMP17.AWBOriginAndDestination()
                    {
                        AirportCityCodeOfDestination = Context.Transshipment2ToPortCode,
                        AirportCityCodeOfOrigin = Context.Transshipment2FromPortCode,
                    },

                    FlightIdentification = new CHAMP17.FlightIdentification_FFR()
                    {
                        CarrierCode = Context.Transshipment2CarrierCode,
                        DayOfScheduledDeparture = Context.Transshipment2CarrierETDDay, // fixed type
                        FlightNumber = Context.Transshipment2CarrierNumber,
                        MonthOfScheduledDeparture = Context.Transshipment2CarrierETDMonth,
                    },

                    AllotmentIdentification = Context.Transshipment2AllotmentIdentification,
                    SpaceAllocationCode = iscancelled ? "XX" : Context.Transshipment2SpaceAllocationCode,
                });
            }

            myXSDElement.FlightDetails = flightDetails.ToArray<CHAMP17.FlightDetails_FFR>();
            #endregion

            #region [4] ULD Description
            //List<ULDDescriptionInfo> uldDescriptionInfo = new List<ULDDescriptionInfo>();

            //uldDescriptionInfo.Add(new ULDDescriptionInfo()
            //{
            //    ULDIdentification = new ULDIdentification_FFR()
            //    {
            //        ULDOwnerCode = "",
            //        ULDSerialNumber = "",
            //        ULDType = "",
            //    },

            //    ULDPositionInformation = new ULDPositionInformation()
            //    {
            //        ULDLoadingIndicator = "",
            //    },

            //    WeightOfULDContents = new WeightOfULDContents() 
            //    {
            //        Weight = 1,
            //        WeightCode = "",
            //    },
            //});

            //myXSDElement.ULDDescription = new ULDDescriptionGroup()
            //{
            //    NumberOfULDs = 1,
            //    ULDDescriptionInfo = uldDescriptionInfo.ToArray<ULDDescriptionInfo>(),
            //};
            #endregion

            #region [5] Special Service Request
            if (Context.SSRTextList.Count > 0)
            {
                myXSDElement.SpecialServiceRequest = Context.SSRTextList.ToArray<string>();
            }
            #endregion

            #region [6] Other Service Information
            if (!string.IsNullOrEmpty(Context.FirstLineOSI))
            {
                myXSDElement.OtherServiceInformation = new CHAMP17.OtherServiceInformation_FFR()
                {
                    OSIDetailsFirstLine = Context.FirstLineOSI,
                    OSIDetailsSecondLine = Context.SecondLineOSI,
                };
            }
            #endregion

            #region [7] Booking Reference
            string myAgentName = Context.IssuingCarrierAgentName.Replace(" ", "");
            if (!string.IsNullOrEmpty(myAgentName))
            {
                string alphaNumericFormat = @"[^A-Z0-9]*";

                myAgentName = myAgentName.Trim().ToUpper();
                myAgentName = Regex.Replace(myAgentName, alphaNumericFormat, string.Empty, RegexOptions.Compiled);
                myAgentName = (myAgentName.Length <= 17) ? myAgentName : myAgentName.Substring(0, 17);
            }

            myXSDElement.BookingReference = new CHAMP17.BookingReference_FFR()
            {
                RequestingOfficeFileReference = Context.BookingNumber,

                //RequestingOfficeMessageAddress = new OfficeMessageAddress()
                //{
                //    AirportCityCode = "",
                //    CompanyDesignator = "",
                //    OfficeFunctionDesignator = "",
                //},

                RequestingParticipantIdentification = new CHAMP17.AWBParticipantIdentification()
                {
                    AirportCityCode = Context.MainCarriageFromPortCode,
                    ParticipantCode = myAgentName,
                    ParticipantIdentifier = "AGT",
                },
            };
            #endregion

            #region [8] Dimensions Information
            if (Context.BookingPackages.Count > 0)
            {
                #region Dimensions

                List<CHAMP17.DimensionsInformation> dimensionsInformation = new List<CHAMP17.DimensionsInformation>();
                foreach (BookingPackage package in Context.BookingPackages)
                {
                    if (package.Length > 0 && package.Width > 0 && package.Height > 0)
                    {
                        CHAMP17.DimensionsInformation itemDimensions = new CHAMP17.DimensionsInformation()
                        {
                            NumberOfPieces = (package.Quantity == null) ? 0 : package.Quantity.Value,

                            DimensionsDetails = new CHAMP17.DimensionsDetails()
                            {
                                HeightDimension = (package.Height == null) ? 0 : (int)package.Height,
                                LengthDimension = (package.Length == null) ? 0 : (int)package.Length,
                                WidthDimension = (package.Width == null) ? 0 : (int)package.Width,
                                MeasurementUnitCode = Context.DimensionsUnitCode,
                            },
                        };

                        if (package.Weight > 0)
                        {

                            itemDimensions.TotalWeightDetails = new CHAMP17.TotalWeightDetails()
                            {
                                Weight = (package.Weight == null) ? 0 : (decimal)MethodHelper.Normalize(package.Weight),
                                WeightCode = Context.GrossWeightUnitCode,
                            };
                        };

                        dimensionsInformation.Add(itemDimensions);
                    }
                }
                #endregion

                myXSDElement.DimensionsInformation = dimensionsInformation.ToArray<CHAMP17.DimensionsInformation>();
            }

            #endregion

            #region [9] Product Information
            if (!string.IsNullOrEmpty(Context.CommodityItemNumber))
            {
                myXSDElement.ProductInformation = new CHAMP17.ProductInformation_FFR()
                {
                    RateInformation = new CHAMP17.RateInformation_FFR()
                    {
                        Item = new CHAMP17.CommodityItemNumberDetails()
                        {
                            Item = Context.CommodityItemNumber,
                        },

                        RateClassCode = "B",
                    },

                    ServiceCode = "D",
                };
            }
            #endregion

            #region [10] Shipper
            if (!string.IsNullOrEmpty(Context.ShipperId))
            {
                myXSDElement.Shipper = new CHAMP17.Account()
                {
                    Contact = new CHAMP17.Contact()
                    {
                        Name = new string[] { Context.ShipperName },
                        StreetAddress = new string[] { Context.ShipperAddress },

                        CodedLocation = new CHAMP17.CodedLocation()
                        {
                            ISOCountryCode = Context.ShipperCountryCode,
                        },

                        Location = new CHAMP17.Location()
                        {
                            Place = Context.ShipperCity,
                        },
                    },
                };

                //if (!string.IsNullOrEmpty(Context.AccountNumber))
                //{
                    //myXSDElement.Shipper.AccountDetail.AccountNumber = Context.AccountNumber;
                //}

                if (!string.IsNullOrEmpty(Context.ShipperStateCode))
                {
                    myXSDElement.Shipper.Contact.Location.StateOrProvince = Context.ShipperStateCode;
                }

                if (!string.IsNullOrEmpty(Context.ShipperZipCode))
                {
                    myXSDElement.Shipper.Contact.CodedLocation.PostCode = Context.ShipperZipCode;
                }

                if (!string.IsNullOrEmpty(Context.ShipperPhone))
                {
                    CHAMP17.ContactDetail contactDetail = new CHAMP17.ContactDetail()
                    {
                        ContactIdentifier = "TE",
                        ContactNumber = Context.ShipperPhone,
                    };

                    myXSDElement.Shipper.Contact.ContactDetail = new CHAMP17.ContactDetail[] { contactDetail };
                }
            }
            #endregion

            #region [11] Consignee
            if (!string.IsNullOrEmpty(Context.ConsigneeId))
            {
                myXSDElement.Consignee = new CHAMP17.Account()
                {
                    Contact = new CHAMP17.Contact()
                    {
                        Name = new string[] { Context.ConsigneeName },
                        StreetAddress = new string[] { Context.ConsigneeAddress },

                        CodedLocation = new CHAMP17.CodedLocation()
                        {
                            ISOCountryCode = Context.ConsigneeCountryCode,
                        },

                        Location = new CHAMP17.Location()
                        {
                            Place = Context.ConsigneeCity,
                        },
                    },
                };

                //if (!string.IsNullOrEmpty(Context.AccountNumber))
                //{
                //    myXSDElement.Consignee.AccountDetail.AccountNumber = Context.AccountNumber;
                //}

                if (!string.IsNullOrEmpty(Context.ConsigneeStateCode))
                {
                    myXSDElement.Consignee.Contact.Location.StateOrProvince = Context.ConsigneeStateCode;
                }

                if (!string.IsNullOrEmpty(Context.ConsigneeZipCode))
                {
                    myXSDElement.Consignee.Contact.CodedLocation.PostCode = Context.ConsigneeZipCode;
                }

                if (!string.IsNullOrEmpty(Context.ConsigneePhone))
                {
                    CHAMP17.ContactDetail contactDetail = new CHAMP17.ContactDetail()
                    {
                        ContactIdentifier = "TE",
                        ContactNumber = Context.ConsigneePhone,
                    };

                    myXSDElement.Consignee.Contact.ContactDetail = new CHAMP17.ContactDetail[] { contactDetail };
                }
            }
            #endregion

            #region [12] Customer Identification
            myXSDElement.CustomerIdentification = new CHAMP17.Agent()
            {
                Name = Context.IssuingCarrierAgentName,
                Place = Context.IssuingCarrierAgentCity,

                AgentAccountDetail = new CHAMP17.AgentAccountDetail()
                {
                    IATACargoAgentNumericCode = Context.IssuingCarrierIATACode, //fixed type
                    ParticipantIdentifier = "AIR",
                },
            };

            if (!string.IsNullOrEmpty(Context.AccountNumber))
            {
                myXSDElement.CustomerIdentification.AgentAccountDetail.AccountNumber = Context.AccountNumber;
            }

            if (Context.IsIssuingCarrierCASSCodeSet)
            {
                myXSDElement.CustomerIdentification.AgentAccountDetail.IATACargoAgentCASSAddress = Context.IssuingCarrierCASSCode; //fixed type
                myXSDElement.CustomerIdentification.AgentAccountDetail.IATACargoAgentCASSAddressSpecified = true;
            }
            #endregion

            #region [13] Shipment Reference Information
        
            if (Context.MainCarriageCarrierCode == "LH")
            {
                if (!string.IsNullOrEmpty(Context.DescriptionOfGoodsId))
                {
                    myXSDElement.ShipmentReferenceInformation = new CHAMP17.ShipmentReferenceInformation();

                    if (!string.IsNullOrEmpty(Context.DescriptionOfGoodsProductCode))
                    {
                        myXSDElement.ShipmentReferenceInformation.SupplementaryShipmentInformation = new string[] { Context.DescriptionOfGoodsProductCode };
                    }
                }
            }
            #endregion

            return myXSDElement;
        }
    }
}
