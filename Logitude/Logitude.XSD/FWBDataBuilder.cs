using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.XSD
{
    public class FWBDataBuilder
    {
        public FWBDataContext Context { get; set; }
        public FWBDataBuilder(FWBDataContext myContext)
        {
            this.Context = myContext;            
        }

        public CHAMP.AirWaybillData GetChampFWB()
        {
            CHAMP.AirWaybillData myXSDElement = new CHAMP.AirWaybillData();

            #region [1] StandardMessageIdentification
            myXSDElement.StandardMessageIdentification = new CHAMP.StandardMessageIdentification()
            {
                MessageTypeVersionNumber = 9,
                StandardMessageIdentifier = "FWB"
            };
            #endregion

            #region[2] AWBConsignmentDetails
            myXSDElement.AWBConsignmentDetails = new CHAMP.AWBConsignmentDetails()
            {
                AWBIdentification = new CHAMP.AWBIdentification()
                {
                    AirlinePrefix = Context.AirlinePrefix,
                    AWBSerialNumber = Context.Master,
                },

                AWBOriginAndDestination = new CHAMP.AWBOriginAndDestination()
                {
                    AirportCityCodeOfOrigin = Context.MainCarriageFromPortCode,
                    AirportCityCodeOfDestination = Context.FinalDestinationPortCode,
                },

                QuantityDetail = new CHAMP.QuantityDetail()
                {
                    ShipmentDescriptionCode = "T",
                    NumberOfPieces = Context.NumberOfPackages,
                    Weight = Context.GrossWeight,
                    WeightCode = Context.GrossWeightUnitCode,
                },
            };
            #endregion

            #region [3] FlightBookings

            List<CHAMP.AWBFlightIdentification> flightBookingLegs = new List<CHAMP.AWBFlightIdentification>();

            flightBookingLegs.Add(new CHAMP.AWBFlightIdentification()
            {
                CarrierCode = Context.MainCarriageCarrierCode,
                FlightNumber = Context.MainCarriageCarrierNumber,
                DayOfScheduledDeparture = Context.MainCarriageCarrierETDDay,
            });

            if (flightBookingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment1CarrierCode) && !string.IsNullOrEmpty(Context.Transshipment1CarrierNumber) && !string.IsNullOrEmpty(Context.Transshipment1CarrierETDDay))
                {
                    flightBookingLegs.Add(new CHAMP.AWBFlightIdentification()
                    {
                        CarrierCode = Context.Transshipment1CarrierCode,
                        FlightNumber = Context.Transshipment1CarrierNumber,
                        DayOfScheduledDeparture = Context.Transshipment1CarrierETDDay,
                    });
                }
            }

            if (flightBookingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment2CarrierCode) && !string.IsNullOrEmpty(Context.Transshipment2CarrierNumber) && !string.IsNullOrEmpty(Context.Transshipment2CarrierETDDay))
                {
                    flightBookingLegs.Add(new CHAMP.AWBFlightIdentification()
                    {
                        CarrierCode = Context.Transshipment2CarrierCode,
                        FlightNumber = Context.Transshipment2CarrierNumber,
                        DayOfScheduledDeparture = Context.Transshipment2CarrierETDDay,
                    });
                }
            }

            if (flightBookingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment3CarrierCode) && !string.IsNullOrEmpty(Context.Transshipment3CarrierNumber) && !string.IsNullOrEmpty(Context.Transshipment3CarrierETDDay))
                {
                    flightBookingLegs.Add(new CHAMP.AWBFlightIdentification()
                    {
                        CarrierCode = Context.Transshipment3CarrierCode,
                        FlightNumber = Context.Transshipment3CarrierNumber,
                        DayOfScheduledDeparture = Context.Transshipment3CarrierETDDay,
                    });
                }
            }

            myXSDElement.FlightBookings = flightBookingLegs.ToArray<CHAMP.AWBFlightIdentification>();
            #endregion

            #region [4] Routing

            myXSDElement.Routing = new CHAMP.Routing()
            {
                FirstDestinationAndCarrier = new CHAMP.FirstDestinationAndCarrier()
                {
                    CarrierCode = Context.MainCarriageCarrierCode,
                    AirportCityCode = Context.MainCarriageToPortCode
                },
            };

            List<CHAMP.OnwardDestinationAndCarrier> flightRoutingLegs = new List<CHAMP.OnwardDestinationAndCarrier>();

            if (flightRoutingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment1FromPortId) && !string.IsNullOrEmpty(Context.Transshipment1ToPortId))
                {
                    flightRoutingLegs.Add(new CHAMP.OnwardDestinationAndCarrier()
                    {
                        CarrierCode = Context.Transshipment1CarrierCode,
                        AirportCityCode = Context.Transshipment1ToPortCode,
                    });
                }
            }

            if (flightRoutingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment2FromPortId) && !string.IsNullOrEmpty(Context.Transshipment2ToPortId))
                {
                    flightRoutingLegs.Add(new CHAMP.OnwardDestinationAndCarrier()
                    {
                        CarrierCode = Context.Transshipment2CarrierCode,
                        AirportCityCode = Context.Transshipment2ToPortCode,
                    });
                }
            }

            if (flightRoutingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment3FromPortId) && !string.IsNullOrEmpty(Context.Transshipment3ToPortId))
                {
                    flightRoutingLegs.Add(new CHAMP.OnwardDestinationAndCarrier()
                    {
                        CarrierCode = Context.Transshipment3CarrierCode,
                        AirportCityCode = Context.Transshipment3ToPortCode,
                    });
                }
            }

            if (flightRoutingLegs.Count > 0)
            {
                myXSDElement.Routing.OnwardDestinationAndCarrier = flightRoutingLegs.ToArray<CHAMP.OnwardDestinationAndCarrier>();
            }

            #endregion

            #region [5] Shipper
            myXSDElement.Shipper = new CHAMP.Account()
            {
                Contact = new CHAMP.Contact()
                {
                    Name = Context.ShipperName,
                    StreetAddress = Context.ShipperAddress,

                    Location = new CHAMP.Location()
                    {
                        Place = Context.ShipperCity
                    },

                    CodedLocation = new CHAMP.CodedLocation()
                    {
                        ISOCountryCode = Context.ShipperCountryCode,
                    },
                },
            };

            if (!string.IsNullOrEmpty(Context.ShipperZipCode))
            {
                myXSDElement.Shipper.Contact.CodedLocation.PostCode = Context.ShipperZipCode;
            }

            if (!string.IsNullOrEmpty(Context.ShipperStateCode))
            {
                myXSDElement.Shipper.Contact.Location.StateOrProvince = Context.ShipperStateCode;
            }

            if (!string.IsNullOrEmpty(Context.ShipperPhone))
            {
                CHAMP.ContactDetail contactDetail = new CHAMP.ContactDetail()
                {
                    ContactIdentifier = "TE",
                    ContactNumber = Context.ShipperPhone,
                };

                myXSDElement.Shipper.Contact.ContactDetail = new CHAMP.ContactDetail[] { contactDetail };
            }
            #endregion

            #region [6] Consignee
            myXSDElement.Consignee = new CHAMP.Account()
            {
                Contact = new CHAMP.Contact()
                {
                    Name = Context.ConsigneeName,
                    StreetAddress = Context.ConsigneeAddress,

                    Location = new CHAMP.Location()
                    {
                        Place = Context.ConsigneeCity
                    },

                    CodedLocation = new CHAMP.CodedLocation()
                    {
                        ISOCountryCode = Context.ConsigneeCountryCode,
                    },
                }
            };

            if (!string.IsNullOrEmpty(Context.ConsigneeZipCode))
            {
                myXSDElement.Consignee.Contact.CodedLocation.PostCode = Context.ConsigneeZipCode;
            }

            if (!string.IsNullOrEmpty(Context.ConsigneeStateCode))
            {
                myXSDElement.Consignee.Contact.Location.StateOrProvince = Context.ConsigneeStateCode;
            }

            if (!string.IsNullOrEmpty(Context.ConsigneePhone))
            {
                CHAMP.ContactDetail contactDetail = new CHAMP.ContactDetail()
                {
                    ContactIdentifier = "TE",
                    ContactNumber = Context.ConsigneePhone,
                };

                myXSDElement.Consignee.Contact.ContactDetail = new CHAMP.ContactDetail[] { contactDetail };
            }
            #endregion

            #region [7] Agent

            if (Context.IsIssuingCarrierIATACodeSet)
            {
                myXSDElement.Agent = new CHAMP.Agent()
                {
                    Name = Context.IssuingCarrierAgentName,
                    Place = Context.IssuingCarrierAgentCity,

                    AgentAccountDetail = new CHAMP.AgentAccountDetail()
                    {
                        IATACargoAgentNumericCode = Context.IssuingCarrierIATACode,
                    }
                };

                if (!string.IsNullOrEmpty(Context.AccountNumber))
                {
                    myXSDElement.Agent.AgentAccountDetail.AccountNumber = Context.AccountNumber;
                }

                if (Context.IsIssuingCarrierCASSCodeSet)
                {
                    myXSDElement.Agent.AgentAccountDetail.IATACargoAgentCASSAddress = Context.IssuingCarrierCASSCode;
                    myXSDElement.Agent.AgentAccountDetail.IATACargoAgentCASSAddressSpecified = true;
                }
            }
            #endregion

            #region [8] SSR: Special Service Request
            if (Context.SSRTextList.Count > 0)
            {
                myXSDElement.SpecialServiceRequest = Context.SSRTextList.ToArray<string>();
            }
            #endregion

            #region [9] Notify1
            if (!string.IsNullOrEmpty(Context.Notify1Name))
            {
                myXSDElement.AlsoNotify = new CHAMP.Contact()
                {
                    Name = Context.Notify1Name,
                    StreetAddress = Context.Notify1Address,

                    Location = new CHAMP.Location()
                    {
                        Place = Context.Notify1City
                    },

                    CodedLocation = new CHAMP.CodedLocation()
                    {
                        ISOCountryCode = Context.Notify1CountryCode,
                    },
                };

                if (!string.IsNullOrEmpty(Context.Notify1ZipCode))
                {
                    myXSDElement.AlsoNotify.CodedLocation.PostCode = Context.Notify1ZipCode;
                }

                if (!string.IsNullOrEmpty(Context.Notify1StateCode))
                {
                    myXSDElement.AlsoNotify.Location.StateOrProvince = Context.Notify1StateCode;
                }

                if (!string.IsNullOrEmpty(Context.Notify1Phone))
                {
                    CHAMP.ContactDetail contactDetail = new CHAMP.ContactDetail()
                    {
                        ContactIdentifier = "TE",
                        ContactNumber = Context.Notify1Phone,
                    };

                    myXSDElement.AlsoNotify.ContactDetail = new CHAMP.ContactDetail[] { contactDetail };
                }
            }
            #endregion

            #region [10] AccountingInformation
            if (Context.AccountingInfoList.Count > 0)
            {
                List<CHAMP.AccountingInformationDetail> list = new List<CHAMP.AccountingInformationDetail>();

                foreach (AccountingInfoItem item in Context.AccountingInfoList)
                {
                    list.Add(new CHAMP.AccountingInformationDetail()
                    {
                        AccountingInformationIdentifier = item.Code,
                        AccountingInformationEntry = item.Element,
                    });
                }

                myXSDElement.AccountingInformation = list.ToArray<CHAMP.AccountingInformationDetail>();
            }
            #endregion

            #region [11] ChargeDeclarations
            myXSDElement.ChargeDeclarations = new CHAMP.FWBChargeDeclarations()
            {
                ISOCurrencyCode = Context.AWBCurrencyCode,
                ChargeCode = Context.AWBChargeCode,

                PrepaidOrCollectChargeDeclarations = new CHAMP.PrepaidOrCollectChargeDeclarations()
                {
                    PrepaidCollectIndicatorOfOtherCharges = Context.OtherPrepaidCollectId,
                    PrepaidCollectIndicatorOfWeightOrValuation = Context.FreightPrepaidCollectId,
                },
            };

            if (Context.IsChargeCarriageDeclared)
            {
                myXSDElement.ChargeDeclarations.ValueForCarriageDeclaration = new CHAMP.ValueForCarriageDeclaration() { Item = Context.ChargeCarriageValue };
            }
            else
            {
                myXSDElement.ChargeDeclarations.ValueForCarriageDeclaration = new CHAMP.ValueForCarriageDeclaration() { Item = new CHAMP.NoValueDeclared() };
            }

            if (Context.IsChargeCustomsDeclared)
            {
                myXSDElement.ChargeDeclarations.ValueForCustomsDeclaration = new CHAMP.ValueForCustomsDeclaration() { Item = Context.ChargeCustomsValue };
            }
            else
            {
                myXSDElement.ChargeDeclarations.ValueForCustomsDeclaration = new CHAMP.ValueForCustomsDeclaration() { Item = new CHAMP.NoCustomsValue() };
            }

            if (Context.IsChargeInsurrenceDeclared)
            {
                myXSDElement.ChargeDeclarations.ValueForInsuranceDeclaration = new CHAMP.ValueForInsuranceDeclaration() { Item = Context.ChargeInsurrenceValue };
            }
            else
            {
                myXSDElement.ChargeDeclarations.ValueForInsuranceDeclaration = new CHAMP.ValueForInsuranceDeclaration() { Item = new CHAMP.NoValue() };
            }
            #endregion
           
            #region [12] RateDescription

            bool isHarmonizeExists = !string.IsNullOrEmpty(Context.MainHarmonize) ? true : false;
            bool isSLACExists = !string.IsNullOrEmpty(Context.SLAC) ? true : false;

            if (Context.IsMultipleCommodities)
            {
                if (Context.ShipmentCommodities.Count > 0)
                {
                    List<CHAMP.RateDescriptionFullBody> listRateDescription = new List<CHAMP.RateDescriptionFullBody>();

                    #region
                    int i = 0;
                    int totalLines = 12;
                    if (isHarmonizeExists)
                    {
                        totalLines = 11;
                    }

                    foreach (ShipmentCommodityPM item in Context.ShipmentCommodities)
                    {
                        if (listRateDescription.Count < totalLines)
                        {
                            i += 1;

                            #region Line Totals

                            var itemGrossWeight = item.GrossWeight;
                            var itemChargeableWeight = item.ChargeableWeight;
                            var itemNumberOfPackages = item.NumberOfPackages == null ? 0 : item.NumberOfPackages;

                            if (Context.GrossWeightUnitCode == "K")
                            {
                                itemGrossWeight = ShipmentMapping.GetWeightInKG("KG", itemGrossWeight);
                            }

                            if (Context.ChargeableWeightUnitCode == "K")
                            {
                                itemChargeableWeight = ShipmentMapping.GetWeightInKG("KG", itemChargeableWeight);
                            }

                            CHAMP.RateDescriptionFullBody listRateTotalsItem = new CHAMP.RateDescriptionFullBody()
                            {
                                ChargeLineCount = new CHAMP.ChargeLineCount()
                                {
                                    AWBRateLineNumber = i,
                                },

                                RateDescriptionFullChoices = new CHAMP.RateDescriptionFullChoices()
                                {
                                    ChargeableWeightDetails = new CHAMP.ChargeableWeightDetails()
                                    {
                                        Weight = itemChargeableWeight == null ? 0 : (decimal)itemChargeableWeight,
                                    },

                                    GrossWeightDetails = new CHAMP.GrossWeightDetails()
                                    {
                                        Weight = itemGrossWeight == null ? 0 : (decimal)itemGrossWeight,
                                        WeightCode = Context.GrossWeightUnitCode,
                                    },

                                    NumberOfPiecesRCPDetails = new CHAMP.NumberOfPiecesRCPDetails()
                                    {
                                        Item = itemNumberOfPackages,
                                    },
                                },
                            };

                            if (!string.IsNullOrEmpty(item.CommodityNumber))
                            {
                                listRateTotalsItem.RateDescriptionFullChoices.CommodityItemNumberDetails = new CHAMP.CommodityItemNumberDetails()
                                {
                                    Item = item.CommodityNumber
                                };
                            }

                            if (!string.IsNullOrEmpty(item.RateClassCode))
                            {
                                listRateTotalsItem.RateDescriptionFullChoices.RateClassDetails = new CHAMP.RateClassDetails()
                                {
                                    RateClassCode = item.RateClassCode,
                                };
                            }

                            if (!Context.AsAgreedFreight)
                            {
                                listRateTotalsItem.RateDescriptionFullChoices.RateChargeDetails = new CHAMP.RateChargeDetails()
                                {
                                    Item = (item.ChargeRate == null) ? 0 : (decimal)item.ChargeRate,
                                    ItemElementName = CHAMP.ItemChoiceType.RateOrCharge,
                                };

                                listRateTotalsItem.RateDescriptionFullChoices.TotalDetails = new CHAMP.TotalDetails()
                                {
                                    Item = (item.ChargeAmount == null) ? 0 : (decimal)item.ChargeAmount,
                                    ItemElementName = CHAMP.ItemChoiceType1.ChargeAmount,
                                };
                            }

                            if (!string.IsNullOrEmpty(item.DescriptionOfGoods))
                            {
                                string natureOfGoods = FormatHelper.FormatString(item.DescriptionOfGoods, FormatHelper.PatternType.NatureAndQuantityOfGoods);

                                if (!string.IsNullOrEmpty(natureOfGoods))
                                {
                                    if (Context.ShipmentLevelCode == "C")
                                    {
                                        listRateTotalsItem.RateDescriptionFullChoices.RateDescriptionMainBody = new CHAMP.RateDescriptionMainBody()
                                        {
                                            Item = new CHAMP.Consolidation()
                                            {
                                                NatureAndQuantityOfGoods = natureOfGoods,
                                            }
                                        };
                                    }

                                    else
                                    {
                                        listRateTotalsItem.RateDescriptionFullChoices.RateDescriptionMainBody = new CHAMP.RateDescriptionMainBody()
                                        {
                                            Item = new CHAMP.GoodsDescription()
                                            {
                                                NatureAndQuantityOfGoods = natureOfGoods,
                                            }
                                        };
                                    }
                                }
                            }

                            listRateDescription.Add(listRateTotalsItem);
                            #endregion

                            #region Dimensions
                            foreach (CommodityPackagePM package in item.CommodityPackages)
                            {
                                if (package.Length > 0 && package.Width > 0 && package.Height > 0)
                                {
                                    if (listRateDescription.Count < totalLines)
                                    {
                                        i += 1;

                                        CHAMP.Dimensions itemDimensions = new CHAMP.Dimensions()
                                        {
                                            HeightDimension = (package.Height == null) ? 0 : (int)package.Height,
                                            LengthDimension = (package.Length == null) ? 0 : (int)package.Length,
                                            WidthDimension = (package.Width == null) ? 0 : (int)package.Width,

                                            MeasurementUnitCode = Context.DimensionsUnitCode,
                                            NumberOfPieces = (package.Quantity == null) ? 0 : package.Quantity.Value,
                                        };

                                        if (package.Weight > 0)
                                        {
                                            itemDimensions.WeightInformation = new CHAMP.WeightInformation()
                                            {
                                                Weight = (package.Weight == null) ? 0 : (decimal)package.Weight,
                                                WeightCode = Context.GrossWeightUnitCode,
                                            };
                                        }

                                        CHAMP.RateDescriptionFullBody listRateDimensionsItem = new CHAMP.RateDescriptionFullBody()
                                        {
                                            ChargeLineCount = new CHAMP.ChargeLineCount()
                                            {
                                                AWBRateLineNumber = i,
                                            },

                                            RateDescriptionFullChoices = new CHAMP.RateDescriptionFullChoices()
                                            {
                                                RateDescriptionMainBody = new CHAMP.RateDescriptionMainBody()
                                                {
                                                    Item = itemDimensions
                                                }
                                            }
                                        };

                                        listRateDescription.Add(listRateDimensionsItem);
                                    }
                                }
                            }
                            #endregion
                        }
                    }

                    if (isHarmonizeExists)
                    {
                        i += 1;

                        CHAMP.RateDescriptionFullBody listRateHarmonizeItem = new CHAMP.RateDescriptionFullBody()
                        {
                            ChargeLineCount = new CHAMP.ChargeLineCount()
                            {
                                AWBRateLineNumber = i,
                            },

                            RateDescriptionFullChoices = new CHAMP.RateDescriptionFullChoices()
                            {
                                RateDescriptionMainBody = new CHAMP.RateDescriptionMainBody()
                                {
                                    Item = new CHAMP.HarmonisedCommodityCode()
                                    {
                                        HarmonisedCommodityCodeEntry = Context.MainHarmonize,
                                    },
                                }
                            }
                        };

                        listRateDescription.Add(listRateHarmonizeItem);
                    }

                    if (isSLACExists)
                    {
                        CHAMP.RateDescriptionFullBody listRateSLACItem = new CHAMP.RateDescriptionFullBody()
                        {
                            RateDescriptionFullChoices = new CHAMP.RateDescriptionFullChoices()
                            {
                                RateDescriptionMainBody = new CHAMP.RateDescriptionMainBody()
                                {
                                    Item = new CHAMP.ShippersLoadAndCount()
                                    {
                                        SLAC = Context.SLAC,
                                    },
                                }
                            }
                        };

                        listRateDescription.Add(listRateSLACItem);
                    }

                    #endregion

                    myXSDElement.RateDescription = new CHAMP.RateDescription()
                    {
                        RateDescriptionFullBody = listRateDescription.ToArray<CHAMP.RateDescriptionFullBody>(),                        
                    };
                }
            }

            else
            {
                if (Context.ShipmentPackages.Count > 0)
                {
                    List<CHAMP.RateDescriptionFullBody> listRateDescription = new List<CHAMP.RateDescriptionFullBody>();

                    #region

                    int i = 0;
                    int totalLines = 11;
                    if (isHarmonizeExists)
                    {
                        totalLines = 10;
                    }

                    #region Line Totals

                    i += 1;

                    CHAMP.RateDescriptionFullBody listRateTotalsItem = new CHAMP.RateDescriptionFullBody()
                    {
                        ChargeLineCount = new CHAMP.ChargeLineCount()
                        {
                            AWBRateLineNumber = i,
                        },

                        RateDescriptionFullChoices = new CHAMP.RateDescriptionFullChoices()
                        {
                            ChargeableWeightDetails = new CHAMP.ChargeableWeightDetails()
                            {
                                Weight = Context.ChargeableWeight,
                            },

                            GrossWeightDetails = new CHAMP.GrossWeightDetails()
                            {
                                Weight = Context.GrossWeight,
                                WeightCode = Context.GrossWeightUnitCode,
                            },

                            NumberOfPiecesRCPDetails = new CHAMP.NumberOfPiecesRCPDetails()
                            {
                                Item = Context.NumberOfPackages,
                            },
                        },
                    };

                    if (!string.IsNullOrEmpty(Context.AWBCommodityItemNumber))
                    {
                        listRateTotalsItem.RateDescriptionFullChoices.CommodityItemNumberDetails = new CHAMP.CommodityItemNumberDetails()
                        {
                            Item = Context.AWBCommodityItemNumber,
                        };
                    }

                    if (!string.IsNullOrEmpty(Context.RateClassCode))
                    {
                        listRateTotalsItem.RateDescriptionFullChoices.RateClassDetails = new CHAMP.RateClassDetails()
                        {
                            RateClassCode = Context.RateClassCode,
                        };
                    }

                    if (!Context.AsAgreedFreight)
                    {
                        listRateTotalsItem.RateDescriptionFullChoices.RateChargeDetails = new CHAMP.RateChargeDetails()
                        {
                            Item = Context.AWBChargeRate,
                            ItemElementName = CHAMP.ItemChoiceType.RateOrCharge,
                        };

                        listRateTotalsItem.RateDescriptionFullChoices.TotalDetails = new CHAMP.TotalDetails()
                        {
                            Item = Context.AWBChargeAmount,
                            ItemElementName = CHAMP.ItemChoiceType1.ChargeAmount,
                        };
                    }

                    if (Context.NatureOfGoodsList.Count > 0)
                    {
                        if (Context.ShipmentLevelCode == "C")
                        {
                            listRateTotalsItem.RateDescriptionFullChoices.RateDescriptionMainBody = new CHAMP.RateDescriptionMainBody()
                            {
                                Item = new CHAMP.Consolidation()
                                {
                                    NatureAndQuantityOfGoods = Context.NatureOfGoodsList[0],
                                }
                            };
                        }

                        else
                        {
                            listRateTotalsItem.RateDescriptionFullChoices.RateDescriptionMainBody = new CHAMP.RateDescriptionMainBody()
                            {
                                Item = new CHAMP.GoodsDescription()
                                {
                                    NatureAndQuantityOfGoods = Context.NatureOfGoodsList[0],
                                }
                            };
                        }

                        Context.NatureOfGoodsList.RemoveAt(0);
                    }

                    listRateDescription.Add(listRateTotalsItem);
                    #endregion

                    #region Dimensions

                    foreach (ShipmentPackage package in Context.ShipmentPackages)
                    {
                        if (package.Length > 0 && package.Width > 0 && package.Height > 0)
                        {
                            if (listRateDescription.Count < totalLines)
                            {
                                i += 1;

                                CHAMP.Dimensions itemDimensions = new CHAMP.Dimensions()
                                {
                                    HeightDimension = (package.Height == null) ? 0 : (int)package.Height,
                                    LengthDimension = (package.Length == null) ? 0 : (int)package.Length,
                                    WidthDimension = (package.Width == null) ? 0 : (int)package.Width,

                                    MeasurementUnitCode = Context.DimensionsUnitCode,
                                    NumberOfPieces = (package.Quantity == null) ? 0 : package.Quantity.Value,
                                };

                                if (package.Weight > 0)
                                {
                                    itemDimensions.WeightInformation = new CHAMP.WeightInformation()
                                    {
                                        Weight = (package.Weight == null) ? 0 : (decimal)package.Weight,
                                        WeightCode = Context.GrossWeightUnitCode,
                                    };
                                }

                                CHAMP.RateDescriptionFullBody listRateDimensionsItem = new CHAMP.RateDescriptionFullBody()
                                {
                                    ChargeLineCount = new CHAMP.ChargeLineCount()
                                    {
                                        AWBRateLineNumber = i,
                                    },

                                    RateDescriptionFullChoices = new CHAMP.RateDescriptionFullChoices()
                                    {
                                        RateDescriptionMainBody = new CHAMP.RateDescriptionMainBody()
                                        {
                                            Item = itemDimensions,
                                        }
                                    }
                                };

                                listRateDescription.Add(listRateDimensionsItem);
                            }
                        }
                    }
                    #endregion

                    #region NatureOfGoodsList

                    foreach (string str in Context.NatureOfGoodsList)
                    {
                        if (listRateDescription.Count < totalLines)
                        {
                            i += 1;

                            CHAMP.RateDescriptionFullBody listRateGoodsItem = new CHAMP.RateDescriptionFullBody()
                            {
                                ChargeLineCount = new CHAMP.ChargeLineCount()
                                {
                                    AWBRateLineNumber = i,
                                },
                            };

                            if (Context.ShipmentLevelCode == "C")
                            {
                                listRateGoodsItem.RateDescriptionFullChoices = new CHAMP.RateDescriptionFullChoices()
                                {
                                    RateDescriptionMainBody = new CHAMP.RateDescriptionMainBody()
                                    {
                                        Item = new CHAMP.Consolidation()
                                        {
                                            NatureAndQuantityOfGoods = str,
                                        }
                                    }
                                };
                            }

                            else
                            {
                                listRateGoodsItem.RateDescriptionFullChoices = new CHAMP.RateDescriptionFullChoices()
                                {
                                    RateDescriptionMainBody = new CHAMP.RateDescriptionMainBody()
                                    {
                                        Item = new CHAMP.GoodsDescription()
                                        {
                                            NatureAndQuantityOfGoods = str,
                                        }
                                    }
                                };
                            }

                            listRateDescription.Add(listRateGoodsItem);
                        }
                    }
                    #endregion

                    if (isHarmonizeExists)
                    {
                        i += 1;

                        CHAMP.RateDescriptionFullBody listRateHarmonizeItem = new CHAMP.RateDescriptionFullBody()
                        {
                            ChargeLineCount = new CHAMP.ChargeLineCount()
                            {
                                AWBRateLineNumber = i,
                            },

                            RateDescriptionFullChoices = new CHAMP.RateDescriptionFullChoices()
                            {
                                RateDescriptionMainBody = new CHAMP.RateDescriptionMainBody()
                                {
                                    Item = new CHAMP.HarmonisedCommodityCode()
                                    {
                                        HarmonisedCommodityCodeEntry = Context.MainHarmonize,
                                    },
                                }
                            }
                        };

                        listRateDescription.Add(listRateHarmonizeItem);
                    }

                    if (isSLACExists)
                    {
                        CHAMP.RateDescriptionFullBody listRateSLACItem = new CHAMP.RateDescriptionFullBody()
                        {
                            RateDescriptionFullChoices = new CHAMP.RateDescriptionFullChoices()
                            {
                                RateDescriptionMainBody = new CHAMP.RateDescriptionMainBody()
                                {
                                    Item = new CHAMP.ShippersLoadAndCount()
                                    {
                                        SLAC = Context.SLAC,
                                    },
                                }
                            }
                        };

                        listRateDescription.Add(listRateSLACItem);
                    }
                    #endregion

                    myXSDElement.RateDescription = new CHAMP.RateDescription()
                    {
                        RateDescriptionFullBody = listRateDescription.ToArray<CHAMP.RateDescriptionFullBody>(),
                    };
                }
            }            
            #endregion

            #region [13] Other Charges
            if (!Context.AsAgreedOtherCharges)
            {
                if (Context.AWBPrintOnlies != null && Context.AWBPrintOnlies.Count > 0)
                {
                    List<ShipmentAWBPrintOnly> otherChargesList = new List<ShipmentAWBPrintOnly>();

                    foreach (ShipmentAWBPrintOnly item in Context.AWBPrintOnlies)
                    {
                        if (!string.IsNullOrEmpty(item.IATACodeId) && !string.IsNullOrEmpty(item.PrepaidCollectId) && item.Amount != null && (item.DueTypeCode == "AG" || item.DueTypeCode == "CA"))
                        {
                            otherChargesList.Add(item);
                        }
                    }

                    if (otherChargesList.Count > 0)
                    {
                        IATACodeRepository iATACodeRepository = new IATACodeRepository(Context.Tenant);

                        List<CHAMP.OtherChargesBody> bodyList = new List<CHAMP.OtherChargesBody>();

                        foreach (ShipmentAWBPrintOnly item in otherChargesList)
                        {
                            string itemIATACode = "";
                            IATACode iATACode = iATACodeRepository.GetSingleIATACode(item.IATACodeId);
                            if (iATACode != null)
                            {
                                itemIATACode = iATACode.Code;
                            }

                            decimal itemAmount = (decimal)item.Amount;
                            string itemDueCode = (item.DueTypeCode.Length <= 1) ? item.DueTypeCode : item.DueTypeCode.Substring(0, 1);
                            itemIATACode = (itemIATACode.Length <= 2) ? itemIATACode : itemIATACode.Substring(0, 2);

                            CHAMP.OtherChargeItems items = new CHAMP.OtherChargeItems()
                            {
                                ChargeAmount = itemAmount,
                                EntitlementCode = itemDueCode,
                                OtherChargeCode = itemIATACode,
                            };

                            CHAMP.OtherChargesBody body = new CHAMP.OtherChargesBody()
                            {
                                ChargeLine = new CHAMP.ChargeLine()
                                {
                                    PrepaidCollectIndicatorOfOtherCharges = item.PrepaidCollectId,
                                },

                                OtherChargeItems = new CHAMP.OtherChargeItems[] { items }
                            };

                            bodyList.Add(body);
                        }

                        myXSDElement.OtherCharges = bodyList.ToArray<CHAMP.OtherChargesBody>();
                    }
                }
            }
            #endregion

            #region [14|15] Prepaid|Collect ChargeSummary

            if (!Context.AsAgreedFreight && !Context.AsAgreedOtherCharges)
            {
                if (Context.PrepaidTotal > 0)
                {
                    #region

                    myXSDElement.PrepaidChargeSummary = new CHAMP.ChargeSummary()
                    {
                        ChargeSummaryTotal = Context.PrepaidTotal,
                    };

                    if (Context.PrepaidWeight > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalWeightCharge = Context.PrepaidWeight;
                        myXSDElement.PrepaidChargeSummary.TotalWeightChargeSpecified = true;
                    }

                    if (Context.PrepaidTaxes > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.Taxes = Context.PrepaidTaxes;
                        myXSDElement.PrepaidChargeSummary.TaxesSpecified = true;
                    }

                    if (Context.PrepaidValuation > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.ValuationCharge = Context.PrepaidValuation;
                        myXSDElement.PrepaidChargeSummary.ValuationChargeSpecified = true;
                    }

                    if (Context.PrepaidDueAgent > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueAgent = Context.PrepaidDueAgent;
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueAgentSpecified = true;
                    }

                    if (Context.PrepaidDueCarrier > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueCarrier = Context.PrepaidDueCarrier;
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueCarrierSpecified = true;
                    }
                    #endregion
                }

                if (Context.CollectTotal > 0)
                {
                    #region

                    myXSDElement.CollectChargeSummary = new CHAMP.ChargeSummary()
                    {
                        ChargeSummaryTotal = Context.CollectTotal,
                    };

                    if (Context.CollectWeight > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalWeightCharge = Context.CollectWeight;
                        myXSDElement.CollectChargeSummary.TotalWeightChargeSpecified = true;
                    }

                    if (Context.CollectTaxes > 0)
                    {
                        myXSDElement.CollectChargeSummary.Taxes = Context.CollectTaxes;
                        myXSDElement.CollectChargeSummary.TaxesSpecified = true;
                    }

                    if (Context.CollectValuation > 0)
                    {
                        myXSDElement.CollectChargeSummary.ValuationCharge = Context.CollectValuation;
                        myXSDElement.CollectChargeSummary.ValuationChargeSpecified = true;
                    }

                    if (Context.CollectDueAgent > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueAgent = Context.CollectDueAgent;
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueAgentSpecified = true;
                    }

                    if (Context.CollectDueCarrier > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueCarrier = Context.CollectDueCarrier;
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueCarrierSpecified = true;
                    }
                    #endregion
                }
            }

            else if (Context.AsAgreedFreight && !Context.AsAgreedOtherCharges)
            {
                if (Context.PrepaidTotal > 0)
                {
                    #region

                    myXSDElement.PrepaidChargeSummary = new CHAMP.ChargeSummary()
                    {
                        ChargeSummaryTotal = Context.PrepaidTotalWithoutWeight,
                    };

                    if (Context.PrepaidTaxes > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.Taxes = Context.PrepaidTaxes;
                        myXSDElement.PrepaidChargeSummary.TaxesSpecified = true;
                    }

                    if (Context.PrepaidValuation > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.ValuationCharge = Context.PrepaidValuation;
                        myXSDElement.PrepaidChargeSummary.ValuationChargeSpecified = true;
                    }

                    if (Context.PrepaidDueAgent > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueAgent = Context.PrepaidDueAgent;
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueAgentSpecified = true;
                    }

                    if (Context.PrepaidDueCarrier > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueCarrier = Context.PrepaidDueCarrier;
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueCarrierSpecified = true;
                    }
                    #endregion
                }

                if (Context.CollectTotal > 0)
                {
                    #region

                    myXSDElement.CollectChargeSummary = new CHAMP.ChargeSummary()
                    {
                        ChargeSummaryTotal = Context.CollectTotalWithoutWeight,
                    };

                    if (Context.CollectTaxes > 0)
                    {
                        myXSDElement.CollectChargeSummary.Taxes = Context.CollectTaxes;
                        myXSDElement.CollectChargeSummary.TaxesSpecified = true;
                    }

                    if (Context.CollectValuation > 0)
                    {
                        myXSDElement.CollectChargeSummary.ValuationCharge = Context.CollectValuation;
                        myXSDElement.CollectChargeSummary.ValuationChargeSpecified = true;
                    }

                    if (Context.CollectDueAgent > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueAgent = Context.CollectDueAgent;
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueAgentSpecified = true;
                    }

                    if (Context.CollectDueCarrier > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueCarrier = Context.CollectDueCarrier;
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueCarrierSpecified = true;
                    }
                    #endregion
                }                
            }

            else if (!Context.AsAgreedFreight && Context.AsAgreedOtherCharges)
            {
                if (Context.PrepaidWeight > 0)
                {
                    myXSDElement.PrepaidChargeSummary = new CHAMP.ChargeSummary()
                    {
                        ChargeSummaryTotal = Context.PrepaidWeight,
                        TotalWeightCharge = Context.PrepaidWeight,
                        TotalWeightChargeSpecified = true,
                    };
                }

                if (Context.CollectWeight > 0)
                {
                    myXSDElement.CollectChargeSummary = new CHAMP.ChargeSummary()
                    {
                        ChargeSummaryTotal = Context.CollectWeight,
                        TotalWeightCharge = Context.CollectWeight,
                        TotalWeightChargeSpecified = true,
                    };
                }
            }
            
            #endregion

            #region [16] ShippersCertification
            if (!string.IsNullOrEmpty(Context.AWBSignature))
            {
                myXSDElement.ShippersCertification = new CHAMP.ShippersCertification()
                {
                    Signature = Context.AWBSignature
                };
            }
            #endregion

            #region [17] CarriersExecution
            myXSDElement.CarriersExecution = new CHAMP.CarriersExecution()
            {
                AWBIssueDetails = new CHAMP.AWBIssueDetails()
                {
                    DayOfMonth = Context.AWBMasterDay,
                    Month = Context.AWBMasterMonth,
                    Year = Context.AWBMasterYear,
                    Item = Context.AWBPlace,
                    ItemElementName = CHAMP.ItemChoiceType2.Place
                },

                Authorisation = new CHAMP.Authorisation()
                {
                    Signature = Context.AWBSignature
                }
            };
            #endregion

            #region [18] AWB Comments
            if (Context.CommentsTextList.Count > 0)
            {
                myXSDElement.OtherServiceInformation = Context.CommentsTextList.ToArray<string>();
            }
            #endregion

            #region [20] SenderReference
            string myAgentName = Context.IssuingCarrierAgentName.Replace(" ", "");
            if (!string.IsNullOrEmpty(myAgentName))
            {
                string alphaNumericFormat = @"[^A-Z0-9]*";

                myAgentName = myAgentName.Trim().ToUpper();
                myAgentName = Regex.Replace(myAgentName, alphaNumericFormat, string.Empty, RegexOptions.Compiled);
                myAgentName = (myAgentName.Length <= 17) ? myAgentName : myAgentName.Substring(0, 17);
            }

            myXSDElement.SenderReference = new CHAMP.SenderReference()
            {
                SenderParticipantIdentification = new CHAMP.AWBParticipantIdentification()
                {
                    AirportCityCode = Context.MainCarriageFromPortCode,
                    ParticipantCode = myAgentName,
                    ParticipantIdentifier = "AGT"
                }
            };
            #endregion

            #region [25] Special Handling Codes
            if (Context.HandlingCodesList.Count > 0)
            {
                myXSDElement.SpecialHandlingDetails = Context.HandlingCodesList.ToArray<string>();
            }
            #endregion

            if (!string.IsNullOrEmpty(Context.SCI))
            {
                myXSDElement.CustomsOrigin = new CHAMP.CustomsOrigin()
                {
                    CustomsOriginCode = Context.SCI,
                };
            }

            return myXSDElement;
        }

        public CHAMP17.AirWaybillData GetChampFWB17()
        {
            CHAMP17.AirWaybillData myXSDElement = new CHAMP17.AirWaybillData();

            #region [1] StandardMessageIdentification
            myXSDElement.StandardMessageIdentification = new CHAMP17.StandardMessageIdentification()
            {
                MessageTypeVersionNumber = 17,
                StandardMessageIdentifier = "FWB"
            };
            #endregion

            #region[2] AWBConsignmentDetails
            myXSDElement.AWBConsignmentDetails = new CHAMP17.AWBConsignmentDetails()
            {
                AWBIdentification = new CHAMP17.AWBIdentification()
                {
                    AirlinePrefix = Context.AirlinePrefix,
                    AWBSerialNumber = Context.Master,
                },

                AWBOriginAndDestination = new CHAMP17.AWBOriginAndDestination()
                {
                    AirportCityCodeOfOrigin = Context.MainCarriageFromPortCode,
                    AirportCityCodeOfDestination = Context.FinalDestinationPortCode,
                },

                QuantityDetail = new CHAMP17.QuantityDetail()
                {
                    ShipmentDescriptionCode = "T",
                    NumberOfPieces = Context.NumberOfPackages,
                    Weight = Context.GrossWeight,
                    WeightCode = Context.GrossWeightUnitCode,
                },

                VolumeDetail = new CHAMP17.VolumeDetail()
                {
                    VolumeCode = Context.VolumeUnitCode,
                    VolumeAmount = Context.Volume,
                },
            };
            #endregion

            #region [3] FlightBookings

            List<CHAMP17.AWBFlightIdentification> flightBookingLegs = new List<CHAMP17.AWBFlightIdentification>();

            flightBookingLegs.Add(new CHAMP17.AWBFlightIdentification()
            {
                CarrierCode = Context.MainCarriageCarrierCode,
                FlightNumber = Context.MainCarriageCarrierNumber,
                DayOfScheduledDeparture = Context.MainCarriageCarrierETDDay,
            });

            if (flightBookingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment1CarrierCode) && !string.IsNullOrEmpty(Context.Transshipment1CarrierNumber) && !string.IsNullOrEmpty(Context.Transshipment1CarrierETDDay))
                {
                    flightBookingLegs.Add(new CHAMP17.AWBFlightIdentification()
                    {
                        CarrierCode = Context.Transshipment1CarrierCode,
                        FlightNumber = Context.Transshipment1CarrierNumber,
                        DayOfScheduledDeparture = Context.Transshipment1CarrierETDDay,
                    });
                }
            }

            if (flightBookingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment2CarrierCode) && !string.IsNullOrEmpty(Context.Transshipment2CarrierNumber) && !string.IsNullOrEmpty(Context.Transshipment2CarrierETDDay))
                {
                    flightBookingLegs.Add(new CHAMP17.AWBFlightIdentification()
                    {
                        CarrierCode = Context.Transshipment2CarrierCode,
                        FlightNumber = Context.Transshipment2CarrierNumber,
                        DayOfScheduledDeparture = Context.Transshipment2CarrierETDDay,
                    });
                }
            }

            if (flightBookingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment3CarrierCode) && !string.IsNullOrEmpty(Context.Transshipment3CarrierNumber) && !string.IsNullOrEmpty(Context.Transshipment3CarrierETDDay))
                {
                    flightBookingLegs.Add(new CHAMP17.AWBFlightIdentification()
                    {
                        CarrierCode = Context.Transshipment3CarrierCode,
                        FlightNumber = Context.Transshipment3CarrierNumber,
                        DayOfScheduledDeparture = Context.Transshipment3CarrierETDDay,
                    });
                }
            }

            myXSDElement.FlightBookings = flightBookingLegs.ToArray<CHAMP17.AWBFlightIdentification>();
            #endregion

            #region [4] Routing

            myXSDElement.Routing = new CHAMP17.Routing()
            {
                FirstDestinationAndCarrier = new CHAMP17.FirstDestinationAndCarrier()
                {
                    CarrierCode = Context.MainCarriageCarrierCode,
                    AirportCityCode = Context.MainCarriageToPortCode
                },
            };

            List<CHAMP17.OnwardDestinationAndCarrier> flightRoutingLegs = new List<CHAMP17.OnwardDestinationAndCarrier>();

            if (flightRoutingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment1FromPortId) && !string.IsNullOrEmpty(Context.Transshipment1ToPortId))
                {
                    flightRoutingLegs.Add(new CHAMP17.OnwardDestinationAndCarrier()
                    {
                        CarrierCode = Context.Transshipment1CarrierCode,
                        AirportCityCode = Context.Transshipment1ToPortCode,
                    });
                }
            }

            if (flightRoutingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment2FromPortId) && !string.IsNullOrEmpty(Context.Transshipment2ToPortId))
                {
                    flightRoutingLegs.Add(new CHAMP17.OnwardDestinationAndCarrier()
                    {
                        CarrierCode = Context.Transshipment2CarrierCode,
                        AirportCityCode = Context.Transshipment2ToPortCode,
                    });
                }
            }

            if (flightRoutingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment3FromPortId) && !string.IsNullOrEmpty(Context.Transshipment3ToPortId))
                {
                    flightRoutingLegs.Add(new CHAMP17.OnwardDestinationAndCarrier()
                    {
                        CarrierCode = Context.Transshipment3CarrierCode,
                        AirportCityCode = Context.Transshipment3ToPortCode,
                    });
                }
            }

            if (flightRoutingLegs.Count > 0)
            {
                myXSDElement.Routing.OnwardDestinationAndCarrier = flightRoutingLegs.ToArray<CHAMP17.OnwardDestinationAndCarrier>();
            }

            #endregion

            #region [5] Shipper
            List<string> myShipperNames = new List<string>();
            myShipperNames.Add(Context.ShipperName);
            if (!string.IsNullOrEmpty(Context.ShipperName2))
            {
                myShipperNames.Add(Context.ShipperName2);
            }

            myXSDElement.Shipper = new CHAMP17.Account()
            {
                Contact = new CHAMP17.Contact()
                {
                    //Name = new string[] { Context.ShipperName },
                    Name = myShipperNames.ToArray(),

                    StreetAddress = new string[] { Context.ShipperAddress },

                    Location = new CHAMP17.Location()
                    {
                        Place = Context.ShipperCity
                    },

                    CodedLocation = new CHAMP17.CodedLocation()
                    {
                        ISOCountryCode = Context.ShipperCountryCode,
                    },
                },
            };

            if (!string.IsNullOrEmpty(Context.ShipperZipCode))
            {
                myXSDElement.Shipper.Contact.CodedLocation.PostCode = Context.ShipperZipCode;
            }

            if (!string.IsNullOrEmpty(Context.ShipperStateCode))
            {
                myXSDElement.Shipper.Contact.Location.StateOrProvince = Context.ShipperStateCode;
            }

            List<CHAMP17.ContactDetail> ContactDetails_Shipper = new List<CHAMP17.ContactDetail>();

            if (!string.IsNullOrEmpty(Context.ShipperPhone))
            {
                CHAMP17.ContactDetail itemDetail = new CHAMP17.ContactDetail()
                {
                    ContactIdentifier = "TE",
                    ContactNumber = Context.ShipperPhone,
                };

                ContactDetails_Shipper.Add(itemDetail);
            }

            if (!string.IsNullOrEmpty(Context.ShipperFax))
            {
                CHAMP17.ContactDetail itemDetail = new CHAMP17.ContactDetail()
                {
                    ContactIdentifier = "FX",
                    ContactNumber = Context.ShipperFax,
                };

                ContactDetails_Shipper.Add(itemDetail);
            }

            if (ContactDetails_Shipper.Count > 0)
            {
                myXSDElement.Shipper.Contact.ContactDetail = ContactDetails_Shipper.ToArray<CHAMP17.ContactDetail>();
            }
            #endregion

            #region [6] Consignee

            List<string> myConsigneeNames = new List<string>();
            myConsigneeNames.Add(Context.ConsigneeName);
            if (!string.IsNullOrEmpty(Context.ConsigneeName2))
            {
                myConsigneeNames.Add(Context.ConsigneeName2);
            }

            myXSDElement.Consignee = new CHAMP17.Account()
            {
                Contact = new CHAMP17.Contact()
                {
                    //Name = new string[] { Context.ConsigneeName },
                    Name = myConsigneeNames.ToArray(),

                    StreetAddress = new string[] { Context.ConsigneeAddress },

                    Location = new CHAMP17.Location()
                    {
                        Place = Context.ConsigneeCity
                    },

                    CodedLocation = new CHAMP17.CodedLocation()
                    {
                        ISOCountryCode = Context.ConsigneeCountryCode,
                    },
                }
            };

            if (!string.IsNullOrEmpty(Context.ConsigneeZipCode))
            {
                myXSDElement.Consignee.Contact.CodedLocation.PostCode = Context.ConsigneeZipCode;
            }

            if (!string.IsNullOrEmpty(Context.ConsigneeStateCode))
            {
                myXSDElement.Consignee.Contact.Location.StateOrProvince = Context.ConsigneeStateCode;
            }

            List<CHAMP17.ContactDetail> ContactDetails_Consignee = new List<CHAMP17.ContactDetail>();

            if (!string.IsNullOrEmpty(Context.ConsigneePhone))
            {
                CHAMP17.ContactDetail itemDetail = new CHAMP17.ContactDetail()
                {
                    ContactIdentifier = "TE",
                    ContactNumber = Context.ConsigneePhone,
                };

                ContactDetails_Consignee.Add(itemDetail);
            }

            if (!string.IsNullOrEmpty(Context.ConsigneeFax))
            {
                CHAMP17.ContactDetail itemDetail = new CHAMP17.ContactDetail()
                {
                    ContactIdentifier = "FX",
                    ContactNumber = Context.ConsigneeFax,
                };

                ContactDetails_Consignee.Add(itemDetail);
            }

            if (ContactDetails_Consignee.Count > 0)
            {
                myXSDElement.Consignee.Contact.ContactDetail = ContactDetails_Consignee.ToArray<CHAMP17.ContactDetail>();
            }            
            #endregion

            #region [7] Agent

            if (Context.IsIssuingCarrierIATACodeSet)
            {
                myXSDElement.Agent = new CHAMP17.Agent()
                {
                    Name = Context.IssuingCarrierAgentName,
                    Place = Context.IssuingCarrierAgentCity,

                    AgentAccountDetail = new CHAMP17.AgentAccountDetail()
                    {
                        IATACargoAgentNumericCode = Context.IssuingCarrierIATACode,
                    }
                };

                if (!string.IsNullOrEmpty(Context.AccountNumber))
                {
                    myXSDElement.Agent.AgentAccountDetail.AccountNumber = Context.AccountNumber;
                }

                if (Context.IsIssuingCarrierCASSCodeSet)
                {
                    myXSDElement.Agent.AgentAccountDetail.IATACargoAgentCASSAddress = Context.IssuingCarrierCASSCode;
                    myXSDElement.Agent.AgentAccountDetail.IATACargoAgentCASSAddressSpecified = true;
                }
            }
            #endregion

            #region [8] SSR: Special Service Request
            if (Context.SSRTextList.Count > 0)
            {
                myXSDElement.SpecialServiceRequest = Context.SSRTextList.ToArray<string>();
            }
            #endregion

            #region [9] Notify1
            if (!string.IsNullOrEmpty(Context.Notify1Name))
            {
                Context.FNANotifyDetails = Context.Notify1Name;

                myXSDElement.AlsoNotify = new CHAMP17.Contact()
                {
                    Name = new string[] { Context.Notify1Name },
                    StreetAddress = new string[] { Context.Notify1Address },

                    Location = new CHAMP17.Location()
                    {
                        Place = Context.Notify1City
                    },

                    CodedLocation = new CHAMP17.CodedLocation()
                    {
                        ISOCountryCode = Context.Notify1CountryCode,
                    },
                };

                Context.FNANotifyDetails = String.Concat(Context.FNANotifyDetails, Context.Notify1Address, Context.Notify1City, Context.Notify1CountryCode);

                if (!string.IsNullOrEmpty(Context.Notify1ZipCode))
                {
                    myXSDElement.AlsoNotify.CodedLocation.PostCode = Context.Notify1ZipCode;
                    Context.FNANotifyDetails = String.Concat(Context.FNANotifyDetails, Context.Notify1ZipCode);
                }

                if (!string.IsNullOrEmpty(Context.Notify1StateCode))
                {
                    myXSDElement.AlsoNotify.Location.StateOrProvince = Context.Notify1StateCode;
                    Context.FNANotifyDetails = String.Concat(Context.FNANotifyDetails, Context.Notify1StateCode);
                }

                List<CHAMP17.ContactDetail> ContactDetails_Notify1 = new List<CHAMP17.ContactDetail>();

                if (!string.IsNullOrEmpty(Context.Notify1Phone))
                {
                    CHAMP17.ContactDetail itemDetail = new CHAMP17.ContactDetail()
                    {
                        ContactIdentifier = "TE",
                        ContactNumber = Context.Notify1Phone,
                    };

                    ContactDetails_Notify1.Add(itemDetail);
                    Context.FNANotifyDetails = String.Concat(Context.FNANotifyDetails, Context.Notify1Phone);
                }

                if (!string.IsNullOrEmpty(Context.Notify1Fax))
                {
                    CHAMP17.ContactDetail itemDetail = new CHAMP17.ContactDetail()
                    {
                        ContactIdentifier = "FX",
                        ContactNumber = Context.Notify1Fax,
                    };

                    ContactDetails_Notify1.Add(itemDetail);
                    Context.FNANotifyDetails = String.Concat(Context.FNANotifyDetails, Context.Notify1Fax);
                }

                if (ContactDetails_Notify1.Count > 0)
                {
                    myXSDElement.AlsoNotify.ContactDetail = ContactDetails_Notify1.ToArray<CHAMP17.ContactDetail>();
                }               
            }
            #endregion

            #region [10] AccountingInformation
            if (Context.AccountingInfoList17.Count > 0)
            {
                List<CHAMP17.AccountingInformationDetail> list = new List<CHAMP17.AccountingInformationDetail>();

                foreach (AccountingInfoItem item in Context.AccountingInfoList17)
                {
                    list.Add(new CHAMP17.AccountingInformationDetail()
                    {
                        AccountingInformationIdentifier = item.Code,
                        AccountingInformationEntry = item.Element,
                    });
                }

                myXSDElement.AccountingInformation = list.ToArray<CHAMP17.AccountingInformationDetail>();
            }
            #endregion

            #region [11] ChargeDeclarations

            myXSDElement.ChargeDeclarations = new CHAMP17.FWBChargeDeclarations()
            {
                ISOCurrencyCode = Context.AWBCurrencyCode,
                ChargeCode = Context.AWBChargeCode,

                PrepaidOrCollectChargeDeclarations = new CHAMP17.PrepaidOrCollectChargeDeclarations()
                {
                    PrepaidCollectIndicatorOfOtherCharges = Context.OtherPrepaidCollectId,
                    PrepaidCollectIndicatorOfWeightOrValuation = Context.FreightPrepaidCollectId,
                },
            };

            if (Context.IsChargeCarriageDeclared)
            {
                myXSDElement.ChargeDeclarations.ValueForCarriageDeclaration = new CHAMP17.ValueForCarriageDeclaration() { Item = Context.ChargeCarriageValue };
            }
            else
            {
                myXSDElement.ChargeDeclarations.ValueForCarriageDeclaration = new CHAMP17.ValueForCarriageDeclaration() { Item = new CHAMP17.NoValueDeclared() };
            }

            if (Context.IsChargeCustomsDeclared)
            {
                myXSDElement.ChargeDeclarations.ValueForCustomsDeclaration = new CHAMP17.ValueForCustomsDeclaration() { Item = Context.ChargeCustomsValue };
            }
            else
            {
                myXSDElement.ChargeDeclarations.ValueForCustomsDeclaration = new CHAMP17.ValueForCustomsDeclaration() { Item = new CHAMP17.NoCustomsValue() };
            }

            if (Context.IsChargeInsurrenceDeclared)
            {
                myXSDElement.ChargeDeclarations.ValueForInsuranceDeclaration = new CHAMP17.ValueForInsuranceDeclaration() { Item = Context.ChargeInsurrenceValue };
            }
            else
            {
                myXSDElement.ChargeDeclarations.ValueForInsuranceDeclaration = new CHAMP17.ValueForInsuranceDeclaration() { Item = new CHAMP17.NoValue() };
            }

            #endregion

            #region [12] RateDescription

            bool isSLACExists = !string.IsNullOrEmpty(Context.SLAC) ? true : false;
            bool isHarmonizeExists = !string.IsNullOrEmpty(Context.MainHarmonize) ? true : false;

            if (Context.IsMultipleCommodities)
            {
                if (Context.ShipmentCommodities.Count > 0)
                {
                    List<CHAMP17.RateDescriptionFullBody> listRateDescription = new List<CHAMP17.RateDescriptionFullBody>();

                    #region

                    int i = 0;
                    int totalLines = 12;
                    if (isHarmonizeExists)
                    {
                        totalLines = 11;
                    }

                    foreach (ShipmentCommodityPM item in Context.ShipmentCommodities)
                    {
                        if (listRateDescription.Count < totalLines)
                        {
                            i += 1;

                            #region Line Totals

                            var itemGrossWeight = item.GrossWeight;
                            var itemChargeableWeight = item.ChargeableWeight;
                            var itemNumberOfPackages = item.NumberOfPackages == null ? 0 : item.NumberOfPackages;

                            if (Context.GrossWeightUnitCode == "K")
                            {
                                itemGrossWeight = ShipmentMapping.GetWeightInKG("KG", itemGrossWeight);
                            }

                            if (Context.ChargeableWeightUnitCode == "K")
                            {
                                itemChargeableWeight = ShipmentMapping.GetWeightInKG("KG", itemChargeableWeight);
                            }

                            CHAMP17.RateDescriptionFullBody listRateTotalsItem = new CHAMP17.RateDescriptionFullBody()
                            {
                                ChargeLineCount = new CHAMP17.ChargeLineCount()
                                {
                                    AWBRateLineNumber = i,
                                },

                                RateDescriptionFullChoices = new CHAMP17.RateDescriptionFullChoices()
                                {
                                    ChargeableWeightDetails = new CHAMP17.ChargeableWeightDetails()
                                    {
                                        Weight = itemChargeableWeight == null ? 0 : (decimal)itemChargeableWeight,
                                    },

                                    GrossWeightDetails = new CHAMP17.GrossWeightDetails()
                                    {
                                        Weight = itemGrossWeight == null ? 0 : (decimal)itemGrossWeight,
                                        WeightCode = Context.GrossWeightUnitCode,
                                    },

                                    NumberOfPiecesRCPDetails = new CHAMP17.NumberOfPiecesRCPDetails()
                                    {
                                        Item = itemNumberOfPackages,
                                    },
                                },
                            };

                            if (!string.IsNullOrEmpty(item.CommodityNumber))
                            {
                                listRateTotalsItem.RateDescriptionFullChoices.CommodityItemNumberDetails = new CHAMP17.CommodityItemNumberDetails()
                                {
                                    Item = item.CommodityNumber,
                                };
                            }

                            if (!string.IsNullOrEmpty(item.RateClassCode))
                            {
                                listRateTotalsItem.RateDescriptionFullChoices.RateClassDetails = new CHAMP17.RateClassDetails()
                                {
                                    RateClassCode = item.RateClassCode,
                                };
                            }

                            if (!Context.AsAgreedFreight)
                            {
                                listRateTotalsItem.RateDescriptionFullChoices.RateChargeDetails = new CHAMP17.RateChargeDetails()
                                {
                                    Item = (item.ChargeRate == null) ? 0 : (decimal)item.ChargeRate,
                                    ItemElementName = CHAMP17.ItemChoiceType.RateOrCharge,
                                };

                                listRateTotalsItem.RateDescriptionFullChoices.TotalDetails = new CHAMP17.TotalDetails()
                                {
                                    Item = (item.ChargeAmount == null) ? 0 : (decimal)item.ChargeAmount,
                                    ItemElementName = CHAMP17.ItemChoiceType1.ChargeAmount,
                                };
                            }

                            //if (!string.IsNullOrEmpty(Context.MainHarmonize))
                            //{
                            //    listRateTotalsItem.RateDescriptionFullChoices.RateDescriptionMainBody = new CHAMP17.RateDescriptionMainBody()
                            //    {
                            //        Item = new CHAMP17.HarmonisedCommodityCode()
                            //        {
                            //            HarmonisedCommodityCodeEntry = Context.MainHarmonize,
                            //        }
                            //    };
                            //}

                            if (!string.IsNullOrEmpty(item.DescriptionOfGoods))
                            {
                                string natureOfGoods = FormatHelper.FormatString(item.DescriptionOfGoods, FormatHelper.PatternType.NatureAndQuantityOfGoods);

                                if (Context.ShipmentLevelCode == "C")
                                {
                                    listRateTotalsItem.RateDescriptionFullChoices.RateDescriptionMainBody = new CHAMP17.RateDescriptionMainBody()
                                    {
                                        Item = new CHAMP17.Consolidation()
                                        {
                                            NatureAndQuantityOfGoods = natureOfGoods,
                                        }
                                    };
                                }

                                else
                                {
                                    listRateTotalsItem.RateDescriptionFullChoices.RateDescriptionMainBody = new CHAMP17.RateDescriptionMainBody()
                                    {
                                        Item = new CHAMP17.GoodsDescription()
                                        {
                                            NatureAndQuantityOfGoods = natureOfGoods,
                                        }
                                    };
                                }
                            }

                            listRateDescription.Add(listRateTotalsItem);
                            #endregion

                            #region Dimensions
                            foreach (CommodityPackagePM package in item.CommodityPackages)
                            {
                                if (package.Length > 0 && package.Width > 0 && package.Height > 0)
                                {
                                    if (listRateDescription.Count < totalLines)
                                    {
                                        i += 1;

                                        CHAMP17.Dimensions itemDimensions = new CHAMP17.Dimensions()
                                        {
                                            HeightDimension = (package.Height == null) ? 0 : (int)package.Height,
                                            LengthDimension = (package.Length == null) ? 0 : (int)package.Length,
                                            WidthDimension = (package.Width == null) ? 0 : (int)package.Width,

                                            MeasurementUnitCode = Context.DimensionsUnitCode,
                                            NumberOfPieces = (package.Quantity == null) ? 0 : package.Quantity.Value,
                                        };

                                        if (package.Weight > 0)
                                        {
                                            itemDimensions.WeightInformation = new CHAMP17.WeightInformation()
                                            {
                                                Weight = (package.Weight == null) ? 0 : (decimal)package.Weight,
                                                WeightCode = Context.GrossWeightUnitCode,
                                            };
                                        }

                                        CHAMP17.RateDescriptionFullBody listRateDimensionsItem = new CHAMP17.RateDescriptionFullBody()
                                        {
                                            ChargeLineCount = new CHAMP17.ChargeLineCount()
                                            {
                                                AWBRateLineNumber = i,
                                            },

                                            RateDescriptionFullChoices = new CHAMP17.RateDescriptionFullChoices()
                                            {
                                                RateDescriptionMainBody = new CHAMP17.RateDescriptionMainBody()
                                                {
                                                    Item = itemDimensions,
                                                },
                                            },
                                        };

                                        listRateDescription.Add(listRateDimensionsItem);
                                    }
                                }
                            }
                            #endregion
                        }
                    }

                    if (isHarmonizeExists)
                    {
                        i += 1;

                        CHAMP17.RateDescriptionFullBody listRateHarmonizeItem = new CHAMP17.RateDescriptionFullBody()
                        {
                            ChargeLineCount = new CHAMP17.ChargeLineCount()
                            {
                                AWBRateLineNumber = i,
                            },

                            RateDescriptionFullChoices = new CHAMP17.RateDescriptionFullChoices()
                            {
                                RateDescriptionMainBody = new CHAMP17.RateDescriptionMainBody()
                                {
                                    Item = new CHAMP17.HarmonisedCommodityCode()
                                    {
                                        HarmonisedCommodityCodeEntry = Context.MainHarmonize,
                                    },
                                }
                            }
                        };

                        listRateDescription.Add(listRateHarmonizeItem);
                    }

                    if (isSLACExists)
                    {
                        CHAMP17.RateDescriptionFullBody listRateSLACItem = new CHAMP17.RateDescriptionFullBody()
                        {
                            RateDescriptionFullChoices = new CHAMP17.RateDescriptionFullChoices()
                            {
                                RateDescriptionMainBody = new CHAMP17.RateDescriptionMainBody()
                                {
                                    Item = new CHAMP17.ShippersLoadAndCount()
                                    {
                                        SLAC = Context.SLAC,
                                    },
                                }
                            }
                        };

                        listRateDescription.Add(listRateSLACItem);
                    }
                    #endregion

                    myXSDElement.RateDescription = listRateDescription.ToArray<CHAMP17.RateDescriptionFullBody>();
                }
            }

            else
            {
                if (Context.ShipmentPackages.Count > 0)
                {
                    List<CHAMP17.RateDescriptionFullBody> listRateDescription = new List<CHAMP17.RateDescriptionFullBody>();

                    int i = 0;
                    int RateDescriptionMaxOccurs = 11;
                    int DimensionsLinesMaxOccurs = RateDescriptionMaxOccurs - 1;
                    int allValidPackagesCounts = Context.ShipmentPackages.Where(d => d.Length > 0 && d.Width > 0 && d.Height > 0).Count();
                    
                    int lines = 1;

                    if (isSLACExists)
                    {
                        lines++;
                        DimensionsLinesMaxOccurs -= 1;
                    }

                    if (isHarmonizeExists)
                    {
                        lines++;
                        DimensionsLinesMaxOccurs -= 1;
                    }

                    if (allValidPackagesCounts > DimensionsLinesMaxOccurs)
                    {
                        lines = 11;
                        DimensionsLinesMaxOccurs -= 1;
                    }

                    else
                    {
                        lines += allValidPackagesCounts;
                    }
                    #region

                    #region First Totals Line

                    i += 1;

                    CHAMP17.RateDescriptionFullBody listRateTotalsItem = new CHAMP17.RateDescriptionFullBody()
                    {
                        ChargeLineCount = new CHAMP17.ChargeLineCount()
                        {
                            AWBRateLineNumber = i,
                        },

                        RateDescriptionFullChoices = new CHAMP17.RateDescriptionFullChoices()
                        {
                            ChargeableWeightDetails = new CHAMP17.ChargeableWeightDetails()
                            {
                                Weight = Context.ChargeableWeight,
                            },

                            GrossWeightDetails = new CHAMP17.GrossWeightDetails()
                            {
                                Weight = Context.GrossWeight,
                                WeightCode = Context.GrossWeightUnitCode,
                            },

                            NumberOfPiecesRCPDetails = new CHAMP17.NumberOfPiecesRCPDetails()
                            {
                                Item = Context.NumberOfPackages,
                            },
                        },
                    };

                    if (!string.IsNullOrEmpty(Context.AWBCommodityItemNumber))
                    {
                        listRateTotalsItem.RateDescriptionFullChoices.CommodityItemNumberDetails = new CHAMP17.CommodityItemNumberDetails()
                        {
                            Item = Context.AWBCommodityItemNumber,
                        };
                    }

                    if (!string.IsNullOrEmpty(Context.RateClassCode))
                    {
                        listRateTotalsItem.RateDescriptionFullChoices.RateClassDetails = new CHAMP17.RateClassDetails()
                        {
                            RateClassCode = Context.RateClassCode,
                        };
                    }

                    if (!Context.AsAgreedFreight)
                    {
                        listRateTotalsItem.RateDescriptionFullChoices.RateChargeDetails = new CHAMP17.RateChargeDetails()
                        {
                            Item = Context.AWBChargeRate,
                            ItemElementName = CHAMP17.ItemChoiceType.RateOrCharge,
                        };

                        listRateTotalsItem.RateDescriptionFullChoices.TotalDetails = new CHAMP17.TotalDetails()
                        {
                            Item = Context.AWBChargeAmount,
                            ItemElementName = CHAMP17.ItemChoiceType1.ChargeAmount,
                        };
                    }

                    if (Context.NatureOfGoodsList.Count > 0)
                    {
                        if (Context.ShipmentLevelCode == "C")
                        {
                            listRateTotalsItem.RateDescriptionFullChoices.RateDescriptionMainBody = new CHAMP17.RateDescriptionMainBody()
                            {
                                Item = new CHAMP17.Consolidation()
                                {
                                    NatureAndQuantityOfGoods = Context.NatureOfGoodsList[0],
                                }
                            };
                        }

                        else
                        {
                            listRateTotalsItem.RateDescriptionFullChoices.RateDescriptionMainBody = new CHAMP17.RateDescriptionMainBody()
                            {
                                Item = new CHAMP17.GoodsDescription()
                                {
                                    NatureAndQuantityOfGoods = Context.NatureOfGoodsList[0],
                                }
                            };
                        }

                        Context.NatureOfGoodsList.RemoveAt(0);
                    }

                    listRateDescription.Add(listRateTotalsItem);
                    #endregion

                    #region NatureOfGoodsList
                    int NatureOfGoodsMaxOccurs = RateDescriptionMaxOccurs - lines;

                    foreach (string str in Context.NatureOfGoodsList)
                    {
                        if (NatureOfGoodsMaxOccurs > 0)
                        {
                            i += 1;

                            CHAMP17.RateDescriptionFullBody listRateGoodsItem = new CHAMP17.RateDescriptionFullBody()
                            {
                                ChargeLineCount = new CHAMP17.ChargeLineCount()
                                {
                                    AWBRateLineNumber = i,
                                },
                            };

                            if (Context.ShipmentLevelCode == "C")
                            {
                                listRateGoodsItem.RateDescriptionFullChoices = new CHAMP17.RateDescriptionFullChoices()
                                {
                                    RateDescriptionMainBody = new CHAMP17.RateDescriptionMainBody()
                                    {
                                        Item = new CHAMP17.Consolidation()
                                        {
                                            NatureAndQuantityOfGoods = str,
                                        }
                                    }
                                };
                            }

                            else
                            {
                                listRateGoodsItem.RateDescriptionFullChoices = new CHAMP17.RateDescriptionFullChoices()
                                {
                                    RateDescriptionMainBody = new CHAMP17.RateDescriptionMainBody()
                                    {
                                        Item = new CHAMP17.GoodsDescription()
                                        {
                                            NatureAndQuantityOfGoods = str,
                                        }
                                    }
                                };
                            }

                            listRateDescription.Add(listRateGoodsItem);
                            NatureOfGoodsMaxOccurs--;
                        }
                    }
                    #endregion

                    #region Dimensions
                    List<ShipmentPackage> allRemainingPackages = new List<ShipmentPackage>();

                    foreach (ShipmentPackage package in Context.ShipmentPackages)
                    {
                        if (package.Length > 0 && package.Width > 0 && package.Height > 0)
                        {
                            if (listRateDescription.Count <= DimensionsLinesMaxOccurs)
                            {
                                i += 1;

                                CHAMP17.Dimensions itemDimensions = new CHAMP17.Dimensions()
                                {
                                    HeightDimension = (package.Height == null) ? 0 : (int)package.Height,
                                    LengthDimension = (package.Length == null) ? 0 : (int)package.Length,
                                    WidthDimension = (package.Width == null) ? 0 : (int)package.Width,
                                    MeasurementUnitCode = Context.DimensionsUnitCode,
                                    NumberOfPieces = (package.Quantity == null) ? 0 : package.Quantity.Value,
                                };

                                if (package.Weight > 0)
                                {
                                    itemDimensions.WeightInformation = new CHAMP17.WeightInformation()
                                    {
                                        Weight = (package.Weight == null) ? 0 : (decimal)package.Weight,
                                        WeightCode = Context.GrossWeightUnitCode,
                                    };
                                }

                                CHAMP17.RateDescriptionFullBody listRateDimensionsItem = new CHAMP17.RateDescriptionFullBody()
                                {
                                    ChargeLineCount = new CHAMP17.ChargeLineCount()
                                    {
                                        AWBRateLineNumber = i,
                                    },

                                    RateDescriptionFullChoices = new CHAMP17.RateDescriptionFullChoices()
                                    {
                                        RateDescriptionMainBody = new CHAMP17.RateDescriptionMainBody()
                                        {
                                            Item = itemDimensions,
                                        }
                                    },
                                };

                                listRateDescription.Add(listRateDimensionsItem);
                            }

                            else
                            {
                                allRemainingPackages.Add(package);
                            }
                        }
                    }

                    if (allRemainingPackages.Count > 0)
                    {
                        i += 1;

                        int? allNumberOfPieces = allRemainingPackages.Sum(s => s.Quantity);
                        double? allWeight = allRemainingPackages.Sum(s => s.Weight);

                        CHAMP17.Dimensions itemDimensions = new CHAMP17.Dimensions()
                        {
                            NumberOfPieces = allNumberOfPieces == null ? 0 : allNumberOfPieces.Value,
                        };

                        if (allWeight > 0)
                        {
                            itemDimensions.WeightInformation = new CHAMP17.WeightInformation()
                            {
                                Weight = allWeight == null ? 0 : (decimal)allWeight,
                                WeightCode = Context.GrossWeightUnitCode,
                            };
                        }

                        CHAMP17.RateDescriptionFullBody listRateDimensionsItem = new CHAMP17.RateDescriptionFullBody()
                        {
                            ChargeLineCount = new CHAMP17.ChargeLineCount()
                            {
                                AWBRateLineNumber = i,
                            },

                            RateDescriptionFullChoices = new CHAMP17.RateDescriptionFullChoices()
                            {
                                RateDescriptionMainBody = new CHAMP17.RateDescriptionMainBody()
                                {
                                    Item = itemDimensions,
                                }
                            },
                        };

                        listRateDescription.Add(listRateDimensionsItem);
                    }
                    #endregion

                    #region Harmonize
                    if (isHarmonizeExists)
                    {
                        i += 1;

                        CHAMP17.RateDescriptionFullBody listRateHarmonizeItem = new CHAMP17.RateDescriptionFullBody()
                        {
                            ChargeLineCount = new CHAMP17.ChargeLineCount()
                            {
                                AWBRateLineNumber = i,
                            },

                            RateDescriptionFullChoices = new CHAMP17.RateDescriptionFullChoices()
                            {
                                RateDescriptionMainBody = new CHAMP17.RateDescriptionMainBody()
                                {
                                    Item = new CHAMP17.HarmonisedCommodityCode()
                                    {
                                        HarmonisedCommodityCodeEntry = Context.MainHarmonize,
                                    },
                                }
                            }
                        };

                        listRateDescription.Add(listRateHarmonizeItem);
                    }
                    #endregion

                    if (isSLACExists)
                    {
                        i += 1;

                        CHAMP17.RateDescriptionFullBody listRateSLACItem = new CHAMP17.RateDescriptionFullBody()
                        {
                            ChargeLineCount = new CHAMP17.ChargeLineCount()
                            {
                                AWBRateLineNumber = i,
                            },

                            RateDescriptionFullChoices = new CHAMP17.RateDescriptionFullChoices()
                            {
                                RateDescriptionMainBody = new CHAMP17.RateDescriptionMainBody()
                                {
                                    Item = new CHAMP17.ShippersLoadAndCount()
                                    {
                                        SLAC = Context.SLAC,
                                    },
                                }
                            }
                        };

                        listRateDescription.Add(listRateSLACItem);
                    }
                    #endregion

                    myXSDElement.RateDescription = listRateDescription.ToArray<CHAMP17.RateDescriptionFullBody>();
                }
            }
            #endregion
            
            #region [13] Other Charges
            if (!Context.AsAgreedOtherCharges)
            {
                List<FWBOtherChargesItem> dataItems = new List<FWBOtherChargesItem>();

                if (Context.AWBPrintOnlies != null)
                {
                    foreach (ShipmentAWBPrintOnly item in Context.AWBPrintOnlies)
                    {
                        if (!string.IsNullOrEmpty(item.IATACodeId) && !string.IsNullOrEmpty(item.PrepaidCollectId) && item.Amount != null && (item.DueTypeCode == "AG" || item.DueTypeCode == "CA"))
                        {
                            dataItems.Add(new FWBOtherChargesItem()
                            {
                                IATACodeId = item.IATACodeId,
                                DueTypeCode = item.DueTypeCode,
                                PrepaidCollectId = item.PrepaidCollectId,
                                Amount = item.Amount,
                            });
                        }
                    }
                }

                if (Context.ShipmentPayables != null)
                {
                    foreach (ShipmentPayable item in Context.ShipmentPayables)
                    {
                        if (!string.IsNullOrEmpty(item.IATACodeId) && !string.IsNullOrEmpty(item.PrepaidCollectId) && item.ExpectedAmount != null && (item.DueTypeCode == "AG" || item.DueTypeCode == "CA"))
                        {
                            dataItems.Add(new FWBOtherChargesItem()
                            {
                                IATACodeId = item.IATACodeId,
                                DueTypeCode = item.DueTypeCode,
                                PrepaidCollectId = item.PrepaidCollectId,
                                Amount = item.ExpectedAmount,
                            });
                        }
                    }
                }

                if (Context.ShipmentReceivables != null)
                {
                    foreach (ShipmentReceivable item in Context.ShipmentReceivables)
                    {
                        if (!string.IsNullOrEmpty(item.IATACodeId) && !string.IsNullOrEmpty(item.PrepaidCollectId) && item.TotalAmount != null && (item.DueTypeCode == "AG" || item.DueTypeCode == "CA"))
                        {
                            dataItems.Add(new FWBOtherChargesItem()
                            {
                                IATACodeId = item.IATACodeId,
                                DueTypeCode = item.DueTypeCode,
                                PrepaidCollectId = item.PrepaidCollectId,
                                Amount = item.TotalAmount,
                            });
                        }
                    }
                }

                if (dataItems.Count > 0)
                {
                    IATACodeRepository iATACodeRepository = new IATACodeRepository(Context.Tenant);

                    List<CHAMP17.OtherChargesBody> bodyList = new List<CHAMP17.OtherChargesBody>();

                    foreach (FWBOtherChargesItem item in dataItems)
                    {
                        string itemIATACode = "";
                        IATACode iATACode = iATACodeRepository.GetSingleIATACode(item.IATACodeId);
                        if (iATACode != null)
                        {
                            itemIATACode = iATACode.Code;
                        }

                        decimal itemAmount = (decimal)item.Amount;
                        string itemDueCode = (item.DueTypeCode.Length <= 1) ? item.DueTypeCode : item.DueTypeCode.Substring(0, 1);
                        itemIATACode = (itemIATACode.Length <= 2) ? itemIATACode : itemIATACode.Substring(0, 2);

                        CHAMP17.OtherChargeItems items = new CHAMP17.OtherChargeItems()
                        {
                            ChargeAmount = itemAmount,
                            EntitlementCode = itemDueCode,
                            OtherChargeCode = itemIATACode,                             
                        };

                        CHAMP17.OtherChargesBody body = new CHAMP17.OtherChargesBody()
                        {
                            ChargeLine = new CHAMP17.ChargeLine()
                            {
                                PrepaidCollectIndicatorOfOtherCharges = item.PrepaidCollectId,
                            },

                            OtherChargeItems = new CHAMP17.OtherChargeItems[] { items }
                        };

                        bodyList.Add(body);
                    }

                    myXSDElement.OtherCharges = bodyList.ToArray<CHAMP17.OtherChargesBody>();
                }
            }
            #endregion

            #region [14|15] Prepaid|Collect ChargeSummary

            if (!Context.AsAgreedFreight && !Context.AsAgreedOtherCharges)
            {
                if (Context.PrepaidTotal > 0)
                {
                    #region

                    myXSDElement.PrepaidChargeSummary = new CHAMP17.ChargeSummary()
                    {
                        ChargeSummaryTotal = Context.PrepaidTotal,
                    };

                    if (Context.PrepaidWeight > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalWeightCharge = Context.PrepaidWeight;
                        myXSDElement.PrepaidChargeSummary.TotalWeightChargeSpecified = true;
                    }

                    if (Context.PrepaidTaxes > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.Taxes = Context.PrepaidTaxes;
                        myXSDElement.PrepaidChargeSummary.TaxesSpecified = true;
                    }

                    if (Context.PrepaidValuation > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.ValuationCharge = Context.PrepaidValuation;
                        myXSDElement.PrepaidChargeSummary.ValuationChargeSpecified = true;
                    }

                    if (Context.PrepaidDueAgent > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueAgent = Context.PrepaidDueAgent;
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueAgentSpecified = true;
                    }

                    if (Context.PrepaidDueCarrier > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueCarrier = Context.PrepaidDueCarrier;
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueCarrierSpecified = true;
                    }
                    #endregion
                }

                if (Context.CollectTotal > 0)
                {
                    #region

                    myXSDElement.CollectChargeSummary = new CHAMP17.ChargeSummary()
                    {
                        ChargeSummaryTotal = Context.CollectTotal,
                    };

                    if (Context.CollectWeight > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalWeightCharge = Context.CollectWeight;
                        myXSDElement.CollectChargeSummary.TotalWeightChargeSpecified = true;
                    }

                    if (Context.CollectTaxes > 0)
                    {
                        myXSDElement.CollectChargeSummary.Taxes = Context.CollectTaxes;
                        myXSDElement.CollectChargeSummary.TaxesSpecified = true;
                    }

                    if (Context.CollectValuation > 0)
                    {
                        myXSDElement.CollectChargeSummary.ValuationCharge = Context.CollectValuation;
                        myXSDElement.CollectChargeSummary.ValuationChargeSpecified = true;
                    }

                    if (Context.CollectDueAgent > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueAgent = Context.CollectDueAgent;
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueAgentSpecified = true;
                    }

                    if (Context.CollectDueCarrier > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueCarrier = Context.CollectDueCarrier;
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueCarrierSpecified = true;
                    }
                    #endregion
                }
            }

            else if (Context.AsAgreedFreight && !Context.AsAgreedOtherCharges)
            {
                if (Context.PrepaidTotal > 0)
                {
                    #region

                    myXSDElement.PrepaidChargeSummary = new CHAMP17.ChargeSummary()
                    {
                        ChargeSummaryTotal = Context.PrepaidTotalWithoutWeight,
                    };

                    if (Context.PrepaidTaxes > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.Taxes = Context.PrepaidTaxes;
                        myXSDElement.PrepaidChargeSummary.TaxesSpecified = true;
                    }

                    if (Context.PrepaidValuation > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.ValuationCharge = Context.PrepaidValuation;
                        myXSDElement.PrepaidChargeSummary.ValuationChargeSpecified = true;
                    }

                    if (Context.PrepaidDueAgent > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueAgent = Context.PrepaidDueAgent;
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueAgentSpecified = true;
                    }

                    if (Context.PrepaidDueCarrier > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueCarrier = Context.PrepaidDueCarrier;
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueCarrierSpecified = true;
                    }
                    #endregion
                }

                if (Context.CollectTotal > 0)
                {
                    #region

                    myXSDElement.CollectChargeSummary = new CHAMP17.ChargeSummary()
                    {
                        ChargeSummaryTotal = Context.CollectTotalWithoutWeight,
                    };

                    if (Context.CollectTaxes > 0)
                    {
                        myXSDElement.CollectChargeSummary.Taxes = Context.CollectTaxes;
                        myXSDElement.CollectChargeSummary.TaxesSpecified = true;
                    }

                    if (Context.CollectValuation > 0)
                    {
                        myXSDElement.CollectChargeSummary.ValuationCharge = Context.CollectValuation;
                        myXSDElement.CollectChargeSummary.ValuationChargeSpecified = true;
                    }

                    if (Context.CollectDueAgent > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueAgent = Context.CollectDueAgent;
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueAgentSpecified = true;
                    }

                    if (Context.CollectDueCarrier > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueCarrier = Context.CollectDueCarrier;
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueCarrierSpecified = true;
                    }
                    #endregion
                }

                if (Context.CollectTotal == 0 && Context.PrepaidTotal == 0)
                {
                    myXSDElement.PrepaidChargeSummary = new CHAMP17.ChargeSummary()
                    {
                        ChargeSummaryTotal = 0,
                    };
                }
            }

            else if (!Context.AsAgreedFreight && Context.AsAgreedOtherCharges)
            {
                if (Context.PrepaidWeight > 0)
                {
                    myXSDElement.PrepaidChargeSummary = new CHAMP17.ChargeSummary()
                    {
                        ChargeSummaryTotal = Context.PrepaidWeight,
                        TotalWeightCharge = Context.PrepaidWeight,
                        TotalWeightChargeSpecified = true,
                    };
                }

                if (Context.CollectWeight > 0)
                {
                    myXSDElement.CollectChargeSummary = new CHAMP17.ChargeSummary()
                    {
                        ChargeSummaryTotal = Context.CollectWeight,
                        TotalWeightCharge = Context.CollectWeight,
                        TotalWeightChargeSpecified = true,
                    };
                }
            }

            else
            {
                myXSDElement.PrepaidChargeSummary = new CHAMP17.ChargeSummary()
                {
                    ChargeSummaryTotal = 0,
                };
            }
            #endregion

            #region [16] ShippersCertification
            if (!string.IsNullOrEmpty(Context.AWBSignature))
            {
                myXSDElement.ShippersCertification = new CHAMP17.ShippersCertification()
                {
                    Signature = Context.AWBSignature
                };
            }
            #endregion

            #region [17] CarriersExecution
            myXSDElement.CarriersExecution = new CHAMP17.CarriersExecution()
            {
                AWBIssueDetails = new CHAMP17.AWBIssueDetails()
                {
                    DayOfMonth = Context.AWBMasterDay,
                    Month = Context.AWBMasterMonth,
                    Year = Context.AWBMasterYear,
                    Item = Context.AWBPlace,
                    ItemElementName = CHAMP17.ItemChoiceType2.Place
                },

                Authorisation = new CHAMP17.Authorisation()
                {
                    Signature = Context.AWBSignature
                }
            };
            #endregion

            #region [18] AWB Comments
            if (Context.CommentsTextList.Count > 0)
            {
                myXSDElement.OtherServiceInformation = Context.CommentsTextList.ToArray<string>();
            }
            #endregion

            #region [20] SenderReference
            string myAgentName = Context.IssuingCarrierAgentName.Replace(" ", "");
            if (!string.IsNullOrEmpty(myAgentName))
            {
                string alphaNumericFormat = @"[^A-Z0-9]*";

                myAgentName = myAgentName.Trim().ToUpper();
                myAgentName = Regex.Replace(myAgentName, alphaNumericFormat, string.Empty, RegexOptions.Compiled);
                myAgentName = (myAgentName.Length <= 17) ? myAgentName : myAgentName.Substring(0, 17);
            }

            myXSDElement.SenderReference = new CHAMP17.SenderReference()
            {
                SenderParticipantIdentification = new CHAMP17.AWBParticipantIdentification()
                {
                    AirportCityCode = Context.MainCarriageFromPortCode,
                    ParticipantCode = myAgentName,
                    ParticipantIdentifier = "AGT"
                }
            };
            #endregion

            #region [25] Special Handling Codes
            if (Context.HandlingCodesList.Count > 0)
            {
                myXSDElement.SpecialHandlingDetails = Context.HandlingCodesList.ToArray<string>();
            }
            #endregion

            #region [26] NominatedHandlingParty
            if (!string.IsNullOrEmpty(Context.NominatedHandlingPartyId))
            {
                myXSDElement.NominatedHandlingParty = new CHAMP17.NominatedHandlingParty()
                {
                    Name = Context.NominatedHandlingPartyName,
                    Place = Context.NominatedHandlingPartyCity,
                };
            }
            #endregion

            #region [27] ShipmentReferenceInformation
            if (Context.IsReferenceInfoSpecified)
            {
                myXSDElement.ShipmentReferenceInformation = new CHAMP17.ShipmentReferenceInformation();

                if (!string.IsNullOrEmpty(Context.ReferenceNumber))
                {
                    myXSDElement.ShipmentReferenceInformation.ReferenceNumber = Context.ReferenceNumber;
                }

                if (Context.SupplementaryTextList.Count > 0)
                {
                    myXSDElement.ShipmentReferenceInformation.SupplementaryShipmentInformation = Context.SupplementaryTextList.ToArray<string>();
                }
            }
            #endregion

            #region [28] OtherParticipantInformation
            if (Context.OtherParticipantList.Count > 0)
            {
                List<CHAMP17.OtherParticipantInformationBody> OtherParticipantList = new List<CHAMP17.OtherParticipantInformationBody>();

                foreach (OtherParticipantItem item in Context.OtherParticipantList)
                {
                    OtherParticipantList.Add(new CHAMP17.OtherParticipantInformationBody()
                    {
                        Name = item.Name,
                        OtherParticipantOfficeFileReference = item.Reference,

                        OtherParticipantIdentification = new CHAMP17.AWBParticipantIdentification()
                        {
                            AirportCityCode = item.Port,
                            ParticipantCode = item.Code,
                            ParticipantIdentifier = item.Id,
                        },
                    });
                }

                myXSDElement.OtherParticipantInformation = OtherParticipantList.ToArray<CHAMP17.OtherParticipantInformationBody>();
            }           
            #endregion

            #region [29] OCI
            if (Context.ShipmentOCIList.Count > 0)
            {
                List<CHAMP17.OtherCustomsInformation> shipmentOCIList = new List<CHAMP17.OtherCustomsInformation>();

                foreach (ShipmentOCIItem item in Context.ShipmentOCIList)
                {
                    CHAMP17.OtherCustomsInformation shipmentOCIItem = new CHAMP17.OtherCustomsInformation();

                    if (!string.IsNullOrEmpty(item.CountryCode))
                    {
                        shipmentOCIItem.ISOCountryCode = item.CountryCode;
                    }

                    if (!string.IsNullOrEmpty(item.InformationCode))
                    {
                        shipmentOCIItem.InformationIdentifier = item.InformationCode;
                    }

                    if (!string.IsNullOrEmpty(item.CustomsInformationCode))
                    {
                        shipmentOCIItem.CustomsInformationIdentifier = item.CustomsInformationCode;
                    }

                    if (!string.IsNullOrEmpty(item.SupplementaryCustomsInfo))
                    {
                        shipmentOCIItem.SupplementaryCustomsInformation = item.SupplementaryCustomsInfo;
                    }

                    shipmentOCIList.Add(shipmentOCIItem);
                }

                myXSDElement.OtherCustomsInformation = shipmentOCIList.ToArray<CHAMP17.OtherCustomsInformation>();
            }
            #endregion

            if (!string.IsNullOrEmpty(Context.SCI))
            {
                myXSDElement.CustomsOrigin = new CHAMP17.CustomsOrigin()
                {
                    CustomsOriginCode = Context.SCI,
                };
            }

            return myXSDElement;
        }

        public GLSHK.FWB GetGLSHKFWB()
        {
            GLSHK.FWB myXSDElement = new GLSHK.FWB();

            #region [R] AWB
            myXSDElement.AWB = new GLSHK.FWBAWB()
            {
                Prefix = Context.AirlinePrefix,
                SerialNum = Context.Master,
            };
            #endregion

            #region [R] AWBOD
            myXSDElement.AWBOD = new GLSHK.OriginDestination()
            {
                Origin = Context.MainCarriageFromPortCode,
                Destination = Context.FinalDestinationPortCode,
            };
            #endregion

            #region [R] Quantity
            myXSDElement.Quantity = new GLSHK.QuantityDetail()
            {
                DescCode = "T",
                Pieces = Context.NumberOfPackages.ToString(),
                Weight = Context.GrossWeight,
                WeightCode = Context.GrossWeightUnitCode,
            };
            #endregion

            #region VolumeDetail
            myXSDElement.Item = new GLSHK.VolumeDetail()
            {
                Code = Context.VolumeUnitCode,
                Value = Context.Volume,
            };          
            #endregion

            #region FlightBookings

            List<GLSHK.Booking> flightBookingLegs = new List<GLSHK.Booking>();

            flightBookingLegs.Add(new GLSHK.Booking()
            {
                CarrierCode = Context.MainCarriageCarrierCode,
                FlightNum = Context.MainCarriageCarrierNumber,
                Day = Context.MainCarriageCarrierETD,
            });

            if (flightBookingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment1CarrierCode) && !string.IsNullOrEmpty(Context.Transshipment1CarrierNumber) && !string.IsNullOrEmpty(Context.Transshipment1CarrierETDDay))
                {
                    flightBookingLegs.Add(new GLSHK.Booking()
                    {
                        CarrierCode = Context.Transshipment1CarrierCode,
                        FlightNum = Context.Transshipment1CarrierNumber,
                        Day = Context.Transshipment1CarrierETD,
                    });
                }
            }

            if (flightBookingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment2CarrierCode) && !string.IsNullOrEmpty(Context.Transshipment2CarrierNumber) && !string.IsNullOrEmpty(Context.Transshipment2CarrierETDDay))
                {
                    flightBookingLegs.Add(new GLSHK.Booking()
                    {
                        CarrierCode = Context.Transshipment2CarrierCode,
                        FlightNum = Context.Transshipment2CarrierNumber,
                        Day = Context.Transshipment2CarrierETD,
                    });
                }
            }

            if (flightBookingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment3CarrierCode) && !string.IsNullOrEmpty(Context.Transshipment3CarrierNumber) && !string.IsNullOrEmpty(Context.Transshipment3CarrierETDDay))
                {
                    flightBookingLegs.Add(new GLSHK.Booking()
                    {
                        CarrierCode = Context.Transshipment3CarrierCode,
                        FlightNum = Context.Transshipment3CarrierNumber,
                        Day = Context.Transshipment3CarrierETD,
                    });
                }
            }

            myXSDElement.FlightBookings = flightBookingLegs.ToArray<GLSHK.Booking>();
            #endregion

            #region [R] Routing

            myXSDElement.Routing = new GLSHK.RoutingGrp()
            {
                DestinationCarrier = new GLSHK.RouteFirst()
                {
                    CarrierCode = Context.MainCarriageCarrierCode,
                    AirportCode = Context.MainCarriageToPortCode,
                },
            };

            List<GLSHK.RouteOnward> flightRoutingLegs = new List<GLSHK.RouteOnward>();

            if (flightRoutingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment1FromPortId) && !string.IsNullOrEmpty(Context.Transshipment1ToPortId))
                {
                    flightRoutingLegs.Add(new GLSHK.RouteOnward()
                    {
                        CarrierCode = Context.Transshipment1CarrierCode,
                        AirportCode = Context.Transshipment1ToPortCode,
                    });
                }
            }

            if (flightRoutingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment2FromPortId) && !string.IsNullOrEmpty(Context.Transshipment2ToPortId))
                {
                    flightRoutingLegs.Add(new GLSHK.RouteOnward()
                    {
                        CarrierCode = Context.Transshipment2CarrierCode,
                        AirportCode = Context.Transshipment2ToPortCode,
                    });
                }
            }

            if (flightRoutingLegs.Count < 2)
            {
                if (!string.IsNullOrEmpty(Context.Transshipment3FromPortId) && !string.IsNullOrEmpty(Context.Transshipment3ToPortId))
                {
                    flightRoutingLegs.Add(new GLSHK.RouteOnward()
                    {
                        CarrierCode = Context.Transshipment3CarrierCode,
                        AirportCode = Context.Transshipment3ToPortCode,
                    });
                }
            }

            if (flightRoutingLegs.Count > 0)
            {
                myXSDElement.Routing.DestinationCarrierOnward = flightRoutingLegs.ToArray<GLSHK.RouteOnward>();
            }
            #endregion

            #region [R] Shipper
            myXSDElement.Shipper = new GLSHK.ContactAddress()
            {
                Name = Context.ShipperName,
                Place = Context.ShipperCity,
                ISOCountryCode = Context.ShipperCountryCode,                
            };

            if (!string.IsNullOrEmpty(Context.ShipperName2))
            {
                myXSDElement.Shipper.Name2 = Context.ShipperName2;
            }

            if (!string.IsNullOrEmpty(Context.ShipperAddress1) && !string.IsNullOrEmpty(Context.ShipperAddress2))
            {
                myXSDElement.Shipper.Address = Context.ShipperAddress1;
                myXSDElement.Shipper.Address2 = Context.ShipperAddress2;
            }

            else
            {
                if (!string.IsNullOrEmpty(Context.ShipperAddress1))
                {
                    myXSDElement.Shipper.Address = Context.ShipperAddress1;
                }

                else
                {
                    myXSDElement.Shipper.Address = Context.ShipperAddress2;
                }
            }

            if (!string.IsNullOrEmpty(Context.ShipperZipCode))
            {
                myXSDElement.Shipper.PostCode = Context.ShipperZipCode;
            }

            if (!string.IsNullOrEmpty(Context.ShipperStateCode))
            {
                myXSDElement.Shipper.StateProvince = Context.ShipperStateCode;
            }

            if (!string.IsNullOrEmpty(Context.ShipperPhone))
            {
                GLSHK.Contact contactDetail = new GLSHK.Contact()
                {
                    ContactID = "TE",
                    ContactNum = Context.ShipperPhone,
                };

                myXSDElement.Shipper.ContactDetail = new GLSHK.Contact[1] { contactDetail };
            }
            #endregion

            #region [R] Consignee
            myXSDElement.Consignee = new GLSHK.ContactAddress()
            {
                Name = Context.ConsigneeName,
                Place = Context.ConsigneeCity,
                ISOCountryCode = Context.ConsigneeCountryCode,                
            };

            if (!string.IsNullOrEmpty(Context.ConsigneeName2))
            {
                myXSDElement.Consignee.Name2 = Context.ConsigneeName2;
            }

            if (!string.IsNullOrEmpty(Context.ConsigneeAddress1) && !string.IsNullOrEmpty(Context.ConsigneeAddress2))
            {
                myXSDElement.Consignee.Address = Context.ConsigneeAddress1;
                myXSDElement.Consignee.Address2 = Context.ConsigneeAddress2;
            }

            else
            {
                if (!string.IsNullOrEmpty(Context.ConsigneeAddress1))
                {
                    myXSDElement.Consignee.Address = Context.ConsigneeAddress1;
                }

                else
                {
                    myXSDElement.Consignee.Address = Context.ConsigneeAddress2;
                }
            }

            if (!string.IsNullOrEmpty(Context.ConsigneeZipCode))
            {
                myXSDElement.Consignee.PostCode = Context.ConsigneeZipCode;
            }

            if (!string.IsNullOrEmpty(Context.ConsigneeStateCode))
            {
                myXSDElement.Consignee.StateProvince = Context.ConsigneeStateCode;
            }

            if (!string.IsNullOrEmpty(Context.ConsigneePhone))
            {
                GLSHK.Contact contactDetail = new GLSHK.Contact()
                {
                    ContactID = "TE",
                    ContactNum = Context.ConsigneePhone,
                };

                myXSDElement.Consignee.ContactDetail = new GLSHK.Contact[1] { contactDetail };
            }
            #endregion

            #region Agent
            if (Context.IsIssuingCarrierIATACodeSet)
            {
                myXSDElement.Agent = new GLSHK.Agent()
                {
                    Name = Context.IssuingCarrierAgentName, 
                    Place = Context.IssuingCarrierAgentCity,

                    AccountDetail = new GLSHK.Account()
                    {
                        IATAAgentCode = Context.IssuingCarrierIATACode,
                    }
                };

                if (!string.IsNullOrEmpty(Context.AccountNumber))
                {
                    myXSDElement.Agent.AccountDetail.AccountNum = Context.AccountNumber;
                }

                if (Context.IsIssuingCarrierCASSCodeSet)
                {
                    myXSDElement.Agent.AccountDetail.IATAAgentCASSAddress = Context.IssuingCarrierCASSCode;
                }
            }
            #endregion

            #region [8] SSR: Special Service Request
            if (Context.SSRTextList.Count > 0)
            {
                List<GLSHK.SSR> list = new List<GLSHK.SSR>();

                foreach (string item in Context.SSRTextList)
                {
                    list.Add(new GLSHK.SSR()
                    {
                        SSRElement = item
                    });
                }

                myXSDElement.SpecialServiceRequest = list.ToArray<GLSHK.SSR>();
            }
            #endregion

            #region Notify1
            if (!string.IsNullOrEmpty(Context.Notify1Name))
            {
                myXSDElement.AlsoNotify = new GLSHK.ContactAddress()
                {
                    Name = Context.Notify1Name,
                    Place = Context.Notify1City,
                    ISOCountryCode = Context.Notify1CountryCode,                    
                };

                if (!string.IsNullOrEmpty(Context.Notify1Name2))
                {
                    myXSDElement.AlsoNotify.Name2 = Context.Notify1Name2;
                }

                if (!string.IsNullOrEmpty(Context.Notify1Address1) && !string.IsNullOrEmpty(Context.Notify1Address2))
                {
                    myXSDElement.AlsoNotify.Address = Context.Notify1Address1;
                    myXSDElement.AlsoNotify.Address2 = Context.Notify1Address2;
                }

                else
                {
                    if (!string.IsNullOrEmpty(Context.Notify1Address1))
                    {
                        myXSDElement.AlsoNotify.Address = Context.Notify1Address1;
                    }

                    else
                    {
                        myXSDElement.AlsoNotify.Address = Context.Notify1Address2;
                    }
                }

                if (!string.IsNullOrEmpty(Context.Notify1ZipCode))
                {
                    myXSDElement.AlsoNotify.PostCode = Context.Notify1ZipCode;
                }

                if (!string.IsNullOrEmpty(Context.Notify1StateCode))
                {
                    myXSDElement.AlsoNotify.StateProvince = Context.Notify1StateCode;
                }

                if (!string.IsNullOrEmpty(Context.Notify1Phone))
                {
                    GLSHK.Contact contactDetail = new GLSHK.Contact()
                    {
                        ContactID = "TE",
                        ContactNum = Context.Notify1Phone,
                    };

                    myXSDElement.AlsoNotify.ContactDetail = new GLSHK.Contact[1] { contactDetail };
                }
            }
            #endregion

            #region AccountingInfo
            if (Context.AccountingInfoList17.Count > 0)
            {
                List<GLSHK.AccountingInfo> list = new List<GLSHK.AccountingInfo>();

                foreach (AccountingInfoItem item in Context.AccountingInfoList17)
                {
                    list.Add(new GLSHK.AccountingInfo()
                    {
                        AccountingID = item.Code,
                        AccountingInfoElement = item.Element,
                    });
                }

                myXSDElement.AccountingInfo = list.ToArray<GLSHK.AccountingInfo>();
            }
            #endregion

            #region [R] ChargeDeclaration

            myXSDElement.ChargeDeclarations = new GLSHK.ChargeDeclaration()
            {
                ChargeCode = Context.AWBChargeCode,
                CurrencyCode = Context.AWBCurrencyCode,
                
                PrepaidCollectDeclarations = new GLSHK.DeclaredPrepaidCollect()
                {
                    Weight = Context.FreightPrepaidCollectId == "P" ? GLSHK.DeclaredPrepaidCollectWeight.P : GLSHK.DeclaredPrepaidCollectWeight.C,
                    OtherCharge = Context.OtherPrepaidCollectId == "P" ? GLSHK.DeclaredPrepaidCollectOtherCharge.P : GLSHK.DeclaredPrepaidCollectOtherCharge.C,

                    WeightOrValuation = GLSHK.DeclaredPrepaidCollectWeightOrValuation.W,
                    WeightOrValuationSpecified = false,
                },
            };

            if (Context.IsChargeCarriageDeclared)
            {
                myXSDElement.ChargeDeclarations.DeclaredValueCarriage = new GLSHK.DeclaredValue() { Items = new object[1] { Context.ChargeCarriageValue } };
            }
            else
            {
                myXSDElement.ChargeDeclarations.DeclaredValueCarriage = new GLSHK.DeclaredValue() { Items = new object[1] { "" } };
            }

            if (Context.IsChargeCustomsDeclared)
            {
                myXSDElement.ChargeDeclarations.DeclaredValueCustoms = new GLSHK.DeclaredValue() { Items = new object[1] { Context.ChargeCustomsValue } };
            }
            else
            {
                myXSDElement.ChargeDeclarations.DeclaredValueCustoms = new GLSHK.DeclaredValue() { Items = new object[1] { "" } };
            }

            if (Context.IsChargeInsurrenceDeclared)
            {
                myXSDElement.ChargeDeclarations.DeclaredValueInsurance = new GLSHK.DeclaredValue() { Items = new object[1] { Context.ChargeInsurrenceValue } };
            }
            else
            {
                myXSDElement.ChargeDeclarations.DeclaredValueInsurance = new GLSHK.DeclaredValue() { Items = new object[1] { "" } };
            }
            #endregion

            #region [R] RateDescription

            if (Context.IsMultipleCommodities)
            {
                if (Context.ShipmentCommodities.Count > 0)
                {
                    List<GLSHK.RateDescDetail> listRateDescription = new List<GLSHK.RateDescDetail>();

                    #region
                    int i = 0;
                    
                    foreach (ShipmentCommodityPM item in Context.ShipmentCommodities)
                    {
                        if (listRateDescription.Count < 12)
                        {                            
                            i += 1;

                            #region Line Totals

                            var itemGrossWeight = item.GrossWeight;
                            var itemChargeableWeight = item.ChargeableWeight;
                            var itemVolume = item.Volume == null ? 0 : item.Volume;
                            var itemNumberOfPackages = item.NumberOfPackages == null ? 0 : item.NumberOfPackages;

                            if (Context.GrossWeightUnitCode == "K")
                            {
                                itemGrossWeight = ShipmentMapping.GetWeightInKG("KG", itemGrossWeight);
                            }

                            if (Context.ChargeableWeightUnitCode == "K")
                            {
                                itemChargeableWeight = ShipmentMapping.GetWeightInKG("KG", itemChargeableWeight);
                            }

                            GLSHK.RateDescDetail listRateTotalsItem = new GLSHK.RateDescDetail()
                            {
                                AWBRateLineNum = i.ToString(),

                                GrossWeight = new GLSHK.Weight()
                                {
                                    Code = Context.GrossWeightUnitCode == "K" ? GLSHK.WeightCode.K : GLSHK.WeightCode.L,
                                    Value = itemGrossWeight == null ? 0 : (decimal)itemGrossWeight,
                                    CodeSpecified = true,
                                },

                                ChargeableWeight = new GLSHK.Weight()
                                {
                                    Code = Context.ChargeableWeightUnitCode == "K" ? GLSHK.WeightCode.K : GLSHK.WeightCode.L,
                                    Value = itemChargeableWeight == null ? 0 : (decimal)itemChargeableWeight,
                                    CodeSpecified = true,
                                },

                                Item = itemNumberOfPackages.ToString(),
                                ItemElementName = GLSHK.ItemChoiceType.Pieces,
                            };

                            if (itemVolume != null)
                            {
                                listRateTotalsItem.Item1 = new GLSHK.VolumeDetail()
                                {
                                    Value = itemVolume == null ? 0 : (decimal)itemVolume,
                                    Code = Context.VolumeUnitCode,
                                };

                                listRateTotalsItem.Item1ElementName = GLSHK.Item1ChoiceType.Volume;
                            }

                            if (!string.IsNullOrEmpty(item.CommodityNumber))
                            {
                                listRateTotalsItem.CommodityDetail = new GLSHK.CommodityItem()
                                {
                                    Items = new string[1] { item.CommodityNumber },
                                    ItemsElementName = new GLSHK.ItemsChoiceType[1] { GLSHK.ItemsChoiceType.CommodityItemNum },
                                };
                            }

                            if (!string.IsNullOrEmpty(item.RateClassCode))
                            {
                                listRateTotalsItem.RateClassCode = item.RateClassCode;
                            }

                            if (!Context.AsAgreedFreight)
                            {
                                listRateTotalsItem.RateChargeDetail = new GLSHK.RateCharge()
                                {
                                    Item = (item.ChargeRate == null) ? 0 : (decimal)item.ChargeRate,
                                    ItemElementName = GLSHK.ItemChoiceType1.RateOrCharge,
                                };

                                listRateTotalsItem.TotalDetail = new GLSHK.TotalDetail()
                                {
                                    Item = (item.ChargeAmount == null) ? 0 : (decimal)item.ChargeAmount,
                                    ItemElementName = GLSHK.ItemChoiceType2.Charge,
                                };
                            }

                            if (!string.IsNullOrEmpty(item.DescriptionOfGoods))
                            {
                                string natureOfGoods = FormatHelper.FormatString(item.DescriptionOfGoods, FormatHelper.PatternType.NatureAndQuantityOfGoods);

                                if (!string.IsNullOrEmpty(natureOfGoods))
                                {
                                    if (Context.ShipmentLevelCode == "C")
                                    {
                                        listRateTotalsItem.Item1 = natureOfGoods;
                                        listRateTotalsItem.Item1ElementName = GLSHK.Item1ChoiceType.Consolidation;
                                    }

                                    else
                                    {
                                        listRateTotalsItem.Item1 = natureOfGoods;
                                        listRateTotalsItem.Item1ElementName = GLSHK.Item1ChoiceType.GoodsDesc;
                                    }
                                }
                            }

                            listRateDescription.Add(listRateTotalsItem);
                            #endregion

                            #region Dimensions
                            foreach (CommodityPackagePM package in item.CommodityPackages)
                            {
                                if (listRateDescription.Count < 12)
                                {
                                    if (package.Length > 0 && package.Width > 0 && package.Height > 0)
                                    {
                                        i += 1;

                                        GLSHK.Dimensions itemDimensions = new GLSHK.Dimensions()
                                        {
                                            Height = (package.Height == null) ? "0" : package.Height.Value.ToString(),
                                            Length = (package.Length == null) ? "0" : package.Length.Value.ToString(),
                                            Width = (package.Width == null) ? "0" : package.Width.Value.ToString(),
                                            Pieces = (package.Quantity == null) ? "0" : package.Quantity.Value.ToString(),
                                            MeasurementUnitCode = Context.DimensionsUnitCode,
                                        };

                                        if (package.Weight > 0)
                                        {
                                            itemDimensions.Weight = new GLSHK.Weight()
                                            {
                                                Value = (package.Weight == null) ? 0 : (decimal)package.Weight,
                                                Code = Context.GrossWeightUnitCode == "K" ? GLSHK.WeightCode.K : GLSHK.WeightCode.L,
                                                CodeSpecified = true,
                                            };
                                        }

                                        GLSHK.RateDescDetail listRateDimensionsItem = new GLSHK.RateDescDetail()
                                        {
                                            AWBRateLineNum = i.ToString(),
                                            Item1 = itemDimensions,
                                            Item1ElementName = GLSHK.Item1ChoiceType.Dimensions,
                                        };

                                        listRateDescription.Add(listRateDimensionsItem);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                    myXSDElement.RateDescription = listRateDescription.ToArray<GLSHK.RateDescDetail>();
                }
            }

            else
            {
                if (Context.ShipmentPackages.Count > 0)
                {
                    List<GLSHK.RateDescDetail> listRateDescription = new List<GLSHK.RateDescDetail>();

                    #region

                    int i = 0;

                    #region Line Totals

                    i += 1;

                    GLSHK.RateDescDetail listRateTotalsItem = new GLSHK.RateDescDetail()
                    {
                        AWBRateLineNum = i.ToString(),

                        GrossWeight = new GLSHK.Weight()
                        {
                            Code = Context.GrossWeightUnitCode == "K" ? GLSHK.WeightCode.K : GLSHK.WeightCode.L,
                            Value = Context.GrossWeight,
                            CodeSpecified = true,
                        },

                        ChargeableWeight = new GLSHK.Weight()
                        {
                            Code = Context.ChargeableWeightUnitCode == "K" ? GLSHK.WeightCode.K : GLSHK.WeightCode.L,
                            Value = Context.ChargeableWeight,
                            CodeSpecified = true,
                        },

                        Item = Context.NumberOfPackages.ToString(),
                        ItemElementName = GLSHK.ItemChoiceType.Pieces,
                    };

                    var itemVolume = Context.Volume;
                    if (itemVolume != null)
                    {
                        listRateTotalsItem.Item1 = new GLSHK.VolumeDetail()
                        {
                            Value = itemVolume == null ? 0 : (decimal)itemVolume,
                            Code = Context.VolumeUnitCode,
                        };

                        listRateTotalsItem.Item1ElementName = GLSHK.Item1ChoiceType.Volume;
                    }

                    if (!string.IsNullOrEmpty(Context.AWBCommodityItemNumber))
                    {
                        listRateTotalsItem.CommodityDetail = new GLSHK.CommodityItem()
                        {
                            Items = new string[1] { Context.AWBCommodityItemNumber },
                            ItemsElementName = new GLSHK.ItemsChoiceType[1] { GLSHK.ItemsChoiceType.CommodityItemNum },
                        };
                    }

                    if (!string.IsNullOrEmpty(Context.RateClassCode))
                    {
                        listRateTotalsItem.RateClassCode = Context.RateClassCode;
                    }

                    if (!Context.AsAgreedFreight)
                    {
                        listRateTotalsItem.RateChargeDetail = new GLSHK.RateCharge()
                        {
                            Item = Context.AWBChargeRate,
                            ItemElementName = GLSHK.ItemChoiceType1.RateOrCharge,
                        };

                        listRateTotalsItem.TotalDetail = new GLSHK.TotalDetail()
                        {
                            Item = Context.AWBChargeAmount,
                            ItemElementName = GLSHK.ItemChoiceType2.Charge,
                        };
                    }

                    if (Context.NatureOfGoodsList.Count > 0)
                    {
                        string myFirstItemString = Context.NatureOfGoodsList[0];

                        if (!string.IsNullOrEmpty(myFirstItemString))
                        {
                            if (Context.ShipmentLevelCode == "C")
                            {
                                listRateTotalsItem.Item1 = myFirstItemString;
                                listRateTotalsItem.Item1ElementName = GLSHK.Item1ChoiceType.Consolidation;
                            }

                            else
                            {
                                listRateTotalsItem.Item1 = myFirstItemString;
                                listRateTotalsItem.Item1ElementName = GLSHK.Item1ChoiceType.GoodsDesc;
                            }
                        }

                        Context.NatureOfGoodsList.RemoveAt(0);
                    }

                    listRateDescription.Add(listRateTotalsItem);
                    #endregion

                    #region Dimensions
                    foreach (ShipmentPackage package in Context.ShipmentPackages)
                    {
                        if (package.Length > 0 && package.Width > 0 && package.Height > 0)
                        {
                            if (listRateDescription.Count < 11)
                            {
                                i += 1;

                                GLSHK.Dimensions itemDimensions = new GLSHK.Dimensions()
                                {
                                    Height = (package.Height == null) ? "0" : package.Height.Value.ToString(),
                                    Length = (package.Length == null) ? "0" : package.Length.Value.ToString(),
                                    Width = (package.Width == null) ? "0" : package.Width.Value.ToString(),
                                    Pieces = (package.Quantity == null) ? "0" : package.Quantity.Value.ToString(),
                                    MeasurementUnitCode = Context.DimensionsUnitCode,
                                };

                                if (package.Weight > 0)
                                {
                                    itemDimensions.Weight = new GLSHK.Weight()
                                    {
                                        Value = (package.Weight == null) ? 0 : (decimal)package.Weight,
                                        Code = Context.GrossWeightUnitCode == "K" ? GLSHK.WeightCode.K : GLSHK.WeightCode.L,
                                        CodeSpecified = true,
                                    };
                                }


                                GLSHK.RateDescDetail listRateDimensionsItem = new GLSHK.RateDescDetail()
                                {
                                    AWBRateLineNum = i.ToString(),
                                    Item1 = itemDimensions,
                                    Item1ElementName = GLSHK.Item1ChoiceType.Dimensions,                                     
                                };

                                listRateDescription.Add(listRateDimensionsItem);
                            }
                        }
                    }
                    #endregion

                    #region NatureOfGoodsList

                    foreach (string str in Context.NatureOfGoodsList)
                    {
                        if (listRateDescription.Count < 11)
                        {
                            i += 1;

                            GLSHK.RateDescDetail listRateGoodsItem = new GLSHK.RateDescDetail()
                            {
                                AWBRateLineNum = i.ToString(),                
                            };

                            if (Context.ShipmentLevelCode == "C")
                            {
                                listRateGoodsItem.Item1 = str;
                                listRateGoodsItem.Item1ElementName = GLSHK.Item1ChoiceType.Consolidation;
                            }

                            else
                            {
                                listRateGoodsItem.Item1 = str;
                                listRateGoodsItem.Item1ElementName = GLSHK.Item1ChoiceType.GoodsDesc;
                            }

                            listRateDescription.Add(listRateGoodsItem);
                        }
                    }
                    #endregion

                    #endregion

                    myXSDElement.RateDescription = listRateDescription.ToArray<GLSHK.RateDescDetail>();
                }
            }
            #endregion
            
            #region Other Charges
            if (!Context.AsAgreedOtherCharges)
            {
                if (Context.AWBPrintOnlies != null && Context.AWBPrintOnlies.Count > 0)
                {
                    List<ShipmentAWBPrintOnly> otherChargesList = new List<ShipmentAWBPrintOnly>();

                    foreach (ShipmentAWBPrintOnly item in Context.AWBPrintOnlies)
                    {
                        if (!string.IsNullOrEmpty(item.IATACodeId) && !string.IsNullOrEmpty(item.PrepaidCollectId) && item.Amount != null && (item.DueTypeCode == "AG" || item.DueTypeCode == "CA"))
                        {
                            otherChargesList.Add(item);
                        }
                    }

                    if (otherChargesList.Count > 0)
                    {
                        IATACodeRepository iATACodeRepository = new IATACodeRepository(Context.Tenant);

                        List<GLSHK.OtherCharges> bodyList = new List<GLSHK.OtherCharges>();

                        foreach (ShipmentAWBPrintOnly item in otherChargesList)
                        {
                            string itemIATACode = "";
                            IATACode iATACode = iATACodeRepository.GetSingleIATACode(item.IATACodeId);
                            if (iATACode != null)
                            {
                                itemIATACode = iATACode.Code;
                            }

                            decimal itemAmount = (decimal)item.Amount;
                            string itemDueCode = (item.DueTypeCode.Length <= 1) ? item.DueTypeCode : item.DueTypeCode.Substring(0, 1);
                            itemIATACode = (itemIATACode.Length <= 2) ? itemIATACode : itemIATACode.Substring(0, 2);

                            GLSHK.OtherChargesOtherChargeItems items = new GLSHK.OtherChargesOtherChargeItems()
                            {
                                ChargeAmount = itemAmount,
                                EntitlementCode = itemDueCode,
                                OtherChargeCode = itemIATACode,                                
                            };

                            GLSHK.OtherCharges body = new GLSHK.OtherCharges()
                            {
                                PCIndicator = item.PrepaidCollectId,
                                OtherChargeItems = new GLSHK.OtherChargesOtherChargeItems[] { items }
                            };

                            bodyList.Add(body);
                        }

                        myXSDElement.OtherCharges = bodyList.ToArray<GLSHK.OtherCharges>();
                    }
                }
            }
            #endregion

            #region Prepaid|Collect ChargeSummary

            if (!Context.AsAgreedFreight && !Context.AsAgreedOtherCharges)
            {
                if (Context.PrepaidTotal > 0)
                {
                    #region

                    myXSDElement.PrepaidChargeSummary = new GLSHK.ChargeSummary()
                    {
                        ChargeSummaryTotal = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.PrepaidTotal,
                        }
                    };

                    if (Context.PrepaidWeight > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalWeightCharge = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.PrepaidWeight,
                        };
                    }

                    if (Context.PrepaidTaxes > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.Taxes = new GLSHK.Charge()
                        {
                             ChargeAmount = Context.PrepaidTaxes,
                        };
                    }

                    if (Context.PrepaidValuation > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.ValuationCharge = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.PrepaidValuation
                        };
                    }

                    if (Context.PrepaidDueAgent > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueAgent = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.PrepaidDueAgent
                        };
                    }

                    if (Context.PrepaidDueCarrier > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueCarrier = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.PrepaidDueCarrier
                        };
                    }
                    #endregion
                }

                if (Context.CollectTotal > 0)
                {
                    #region

                    myXSDElement.CollectChargeSummary = new GLSHK.ChargeSummary()
                    {
                        ChargeSummaryTotal = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.CollectTotal,
                        }
                    };

                    if (Context.CollectWeight > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalWeightCharge = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.CollectWeight
                        };
                    }

                    if (Context.CollectTaxes > 0)
                    {
                        myXSDElement.CollectChargeSummary.Taxes = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.CollectTaxes
                        };
                    }

                    if (Context.CollectValuation > 0)
                    {
                        myXSDElement.CollectChargeSummary.ValuationCharge = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.CollectValuation

                        };
                    }

                    if (Context.CollectDueAgent > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueAgent = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.CollectDueAgent
                        };
                    }

                    if (Context.CollectDueCarrier > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueCarrier = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.CollectDueCarrier
                        };
                    }
                    #endregion
                }
            }

            else if (Context.AsAgreedFreight && !Context.AsAgreedOtherCharges)
            {
                if (Context.PrepaidTotal > 0)
                {
                    #region

                    myXSDElement.PrepaidChargeSummary = new GLSHK.ChargeSummary()
                    {
                        ChargeSummaryTotal = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.PrepaidTotalWithoutWeight
                        }
                    };

                    if (Context.PrepaidTaxes > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.Taxes = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.PrepaidTaxes
                        };
                    }

                    if (Context.PrepaidValuation > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.ValuationCharge = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.PrepaidValuation
                        };
                    }

                    if (Context.PrepaidDueAgent > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueAgent = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.PrepaidDueAgent
                        };
                    }

                    if (Context.PrepaidDueCarrier > 0)
                    {
                        myXSDElement.PrepaidChargeSummary.TotalOtherChargesDueCarrier = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.PrepaidDueCarrier
                        };
                    }
                    #endregion
                }

                if (Context.CollectTotal > 0)
                {
                    #region

                    myXSDElement.CollectChargeSummary = new GLSHK.ChargeSummary()
                    {
                        ChargeSummaryTotal = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.CollectTotalWithoutWeight
                        }
                    };

                    if (Context.CollectTaxes > 0)
                    {
                        myXSDElement.CollectChargeSummary.Taxes = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.CollectTaxes
                        };
                    }

                    if (Context.CollectValuation > 0)
                    {
                        myXSDElement.CollectChargeSummary.ValuationCharge = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.CollectValuation
                        };
                    }

                    if (Context.CollectDueAgent > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueAgent = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.CollectDueAgent
                        };
                    }

                    if (Context.CollectDueCarrier > 0)
                    {
                        myXSDElement.CollectChargeSummary.TotalOtherChargesDueCarrier = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.CollectDueCarrier
                        };
                    }
                    #endregion
                }
            }

            else if (!Context.AsAgreedFreight && Context.AsAgreedOtherCharges)
            {
                if (Context.PrepaidWeight > 0)
                {
                    myXSDElement.PrepaidChargeSummary = new GLSHK.ChargeSummary()
                    {
                        ChargeSummaryTotal = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.PrepaidWeight
                        },

                        TotalWeightCharge = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.PrepaidWeight
                        },
                    };
                }

                if (Context.CollectWeight > 0)
                {
                    myXSDElement.CollectChargeSummary = new GLSHK.ChargeSummary()
                    {
                        ChargeSummaryTotal = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.CollectWeight,
                        },

                        TotalWeightCharge = new GLSHK.Charge()
                        {
                            ChargeAmount = Context.CollectWeight,
                        }
                    };
                }
            }
            #endregion

            #region [R] CarrierExecution
            myXSDElement.CarrierExecution = new GLSHK.Execution()
            {
                IssueDate = Context.AWBMasterDate.Value,
                IssuePlace = Context.AWBPlace,
                //Authorisation = Context.AWBSignature,
            };
            #endregion

            #region [18] AWB Comments
            if (Context.CommentsTextList.Count > 0)
            {
                List<GLSHK.OtherServiceInfo> list = new List<GLSHK.OtherServiceInfo>();

                foreach (string item in Context.CommentsTextList)
                {
                    list.Add(new GLSHK.OtherServiceInfo()
                    {
                         OtherServiceInformation = item
                    });
                }

                myXSDElement.OtherServiceInfo = list.ToArray<GLSHK.OtherServiceInfo>();
            }
            #endregion

            #region [R] SenderReference
            myXSDElement.SenderReference = new GLSHK.SenderRef()
            {
                Item = new GLSHK.SenderRefParticipantID()
                {
                    Airport = Context.MainCarriageFromPortCode,
                    ParticipantCode = "NA",
                    ParticipantIDElement = "FFW",
                },

                OfficeFileRef = Context.ShipmentNumber,
            };
            #endregion            

            #region [25] Special Handling Codes
            if (Context.HandlingCodesList.Count > 0)
            {
                List<GLSHK.SpecialHandling> list = new List<GLSHK.SpecialHandling>();

                foreach (string item in Context.HandlingCodesList)
                {
                    list.Add(new GLSHK.SpecialHandling()
                    {
                        SpecialHandlingCode = item,
                    });
                }

                myXSDElement.SpecialHandlingDetail = list.ToArray<GLSHK.SpecialHandling>();
            }
            #endregion

            #region [26] NominatedHandlingParty
            if (!string.IsNullOrEmpty(Context.NominatedHandlingPartyId))
            {
                myXSDElement.NominatedHandlingParty = new GLSHK.NominatedHandlingParty()
                {
                    Name = Context.NominatedHandlingPartyName,
                    Place = Context.NominatedHandlingPartyCity,
                };
            }
            #endregion

            #region [27] ShipmentReferenceInformation
            if (Context.IsReferenceInfoSpecified)
            {
                myXSDElement.ShipmentReference = new GLSHK.ShipmentRef()
                {
                    ShipmentDetail = new GLSHK.ShipmentDetail()
                    {
                         
                    }
                };

                if (!string.IsNullOrEmpty(Context.ReferenceNumber))
                {
                    myXSDElement.ShipmentReference.ShipmentDetail.ReferenceNum = Context.ReferenceNumber;
                }

                if (Context.SupplementaryTextList.Count > 0)
                {
                    myXSDElement.ShipmentReference.ShipmentDetail.SupplementaryShipmentInfo = Context.SupplementaryTextList.ToArray<string>();
                }
            }
            #endregion

            #region [28] OtherParticipantInformation
            if (Context.OtherParticipantList.Count > 0)
            {
                List<GLSHK.OtherParticipant> OtherParticipantList = new List<GLSHK.OtherParticipant>();

                foreach (OtherParticipantItem item in Context.OtherParticipantList)
                {
                    OtherParticipantList.Add(new GLSHK.OtherParticipant()
                    {
                        Name = item.Name,
                        OfficeFileReference = item.Reference,

                        Item = new GLSHK.ParticipantID()
                        {
                            Airport = item.Port,
                            Code = item.Code,
                            ParticipantID1 = item.Id,
                        }
                    });
                }

                myXSDElement.OtherParticipantInfo = OtherParticipantList.ToArray<GLSHK.OtherParticipant>();
            }
            #endregion

            #region [29] OCI
            if (Context.ShipmentOCIList.Count > 0)
            {
                List<GLSHK.OtherCustoms> shipmentOCIList = new List<GLSHK.OtherCustoms>();

                foreach (ShipmentOCIItem item in Context.ShipmentOCIList)
                {
                    GLSHK.OtherCustoms shipmentOCIItem = new GLSHK.OtherCustoms();

                    if (!string.IsNullOrEmpty(item.CountryCode))
                    {
                        shipmentOCIItem.ISOCountryCode = item.CountryCode;
                    }

                    if (!string.IsNullOrEmpty(item.InformationCode))
                    {
                        shipmentOCIItem.InformationID = item.InformationCode;
                    }

                    if (!string.IsNullOrEmpty(item.CustomsInformationCode))
                    {
                        shipmentOCIItem.CustomsInformationID = item.CustomsInformationCode;
                    }

                    if (!string.IsNullOrEmpty(item.SupplementaryCustomsInfo))
                    {
                        shipmentOCIItem.SupplementaryCustomsInfo = item.SupplementaryCustomsInfo;
                    }

                    shipmentOCIList.Add(shipmentOCIItem);
                }

                myXSDElement.OtherCustomsInfo = shipmentOCIList.ToArray<GLSHK.OtherCustoms>();
            }
            #endregion

            if (!string.IsNullOrEmpty(Context.SCI))
            {
                myXSDElement.CustomsOrigin = Context.SCI;
            }

            return myXSDElement;
        }        

        class FWBOtherChargesItem
        {
            public string IATACodeId { get; set; }
            public string DueTypeCode { get; set; }
            public string PrepaidCollectId { get; set; }
            public double? Amount { get; set; }
        }
    }
}
