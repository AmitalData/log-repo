using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD
{
    public class FHLDataBuilder
    {
        public FHLDataContext Context { get; set; }
        public FHLDataBuilder(FHLDataContext myContext)
        {
            this.Context = myContext;
        }

        public CHAMP.ConsolidationList GetChampFHL()
        {
            CHAMP.ConsolidationList myXSDElement = new CHAMP.ConsolidationList();

            #region [1] StandardMessageIdentification
            myXSDElement.StandardMessageIdentification = new CHAMP.StandardMessageIdentification()
            {
                MessageTypeVersionNumber = 2,
                StandardMessageIdentifier = "FHL"
            };
            #endregion

            #region[2] MasterAWBConsignmentDetail
            myXSDElement.MasterAWBConsignmentDetail = new CHAMP.AWBConsignmentDetails()
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
                    NumberOfPieces = Context.MasterNumberOfPackages,
                    Weight = Context.MasterGrossWeight,
                    WeightCode = Context.MasterGrossWeightUnitCode,
                },
            };
            #endregion

            #region [3] HouseWaybillSummaryDetails

            List<CHAMP.HouseWaybillSummaryDetails> summaryDetailsList = new List<CHAMP.HouseWaybillSummaryDetails>();

            CHAMP.HouseWaybillSummaryDetails summaryDetailsItem = new CHAMP.HouseWaybillSummaryDetails()
            {
                HWBSerialNumber = Context.House,

                HouseWaybillOriginAndDestination = new CHAMP.AWBOriginAndDestination()
                {
                    AirportCityCodeOfOrigin = Context.FromPortCode,
                    AirportCityCodeOfDestination = Context.ToPortCode,
                },

                HouseWaybillTotals = new CHAMP.HouseWaybillTotals()
                {
                    NumberOfPieces = Context.NumberOfPackages,
                    NumberOfPiecesSpecified = true,
                    Weight = Context.GrossWeight,
                    WeightCode = Context.GrossWeightUnitCode,
                    WeightSpecified = true,
                },
            };

            if (!string.IsNullOrEmpty(Context.DescriptionOfGoods))
            {
                summaryDetailsItem.NatureOfGoods = Context.DescriptionOfGoods;

                if(!string.IsNullOrEmpty(Context.SLAC))
                {
                    summaryDetailsItem.NatureOfGoods += Environment.NewLine + "SLAC: " + Context.SLAC;
                }

                if (Context.DescriptionOfGoodsTextList.Count > 0)
                {
                    summaryDetailsItem.FreeTextDescriptionOfGoods = new string[1] { Context.DescriptionOfGoodsTextList.FirstOrDefault() };
                }
            }

            summaryDetailsList.Add(summaryDetailsItem);
            myXSDElement.HouseWaybillSummaryDetails = summaryDetailsList.ToArray<CHAMP.HouseWaybillSummaryDetails>();
            #endregion

            #region [4] Shipper
            myXSDElement.Shipper = new CHAMP.Contact()
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
            };

            if (!string.IsNullOrEmpty(Context.ShipperZipCode))
            {
                myXSDElement.Shipper.CodedLocation.PostCode = Context.ShipperZipCode;
            }

            if (!string.IsNullOrEmpty(Context.ShipperStateCode))
            {
                myXSDElement.Shipper.Location.StateOrProvince = Context.ShipperStateCode;
            }

            if (!string.IsNullOrEmpty(Context.ShipperPhone))
            {
                CHAMP.ContactDetail contactDetail = new CHAMP.ContactDetail()
                {
                    ContactIdentifier = "TE",
                    ContactNumber = Context.ShipperPhone,
                };

                myXSDElement.Shipper.ContactDetail = new CHAMP.ContactDetail[] { contactDetail };
            }
            #endregion

            #region [5] Consignee
            myXSDElement.Consignee = new CHAMP.Contact()
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
            };

            if (!string.IsNullOrEmpty(Context.ConsigneeZipCode))
            {
                myXSDElement.Consignee.CodedLocation.PostCode = Context.ConsigneeZipCode;
            }

            if (!string.IsNullOrEmpty(Context.ConsigneeStateCode))
            {
                myXSDElement.Consignee.Location.StateOrProvince = Context.ConsigneeStateCode;
            }

            if (!string.IsNullOrEmpty(Context.ConsigneePhone))
            {
                CHAMP.ContactDetail contactDetail = new CHAMP.ContactDetail()
                {
                    ContactIdentifier = "TE",
                    ContactNumber = Context.ConsigneePhone,
                };

                myXSDElement.Consignee.ContactDetail = new CHAMP.ContactDetail[] { contactDetail };
            }
            #endregion

            return myXSDElement;
        }

        public CHAMP17.ConsolidationList GetChampFHL5(bool isMultiHS = false)
        {
            CHAMP17.ConsolidationList myXSDElement = new CHAMP17.ConsolidationList();

            #region [1] StandardMessageIdentification
            myXSDElement.StandardMessageIdentification = new CHAMP17.StandardMessageIdentification()
            {
                MessageTypeVersionNumber = 5,
                StandardMessageIdentifier = "FHL",
            };
            #endregion

            #region[2] MasterAWBConsignmentDetail
            myXSDElement.MasterAWBConsignmentDetail = new CHAMP17.AWBConsignmentDetails()
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
                    NumberOfPieces = Context.MasterNumberOfPackages,
                    Weight = Context.MasterGrossWeight,
                    WeightCode = Context.MasterGrossWeightUnitCode,
                },
            };
            #endregion

            #region [3] HouseWaybillSummaryDetails

            List<CHAMP17.HouseWaybillSummaryDetails> summaryDetailsList = new List<CHAMP17.HouseWaybillSummaryDetails>();

            CHAMP17.HouseWaybillSummaryDetails summaryDetailsItem = new CHAMP17.HouseWaybillSummaryDetails()
            {
                HWBSerialNumber = Context.House,

                HouseWaybillOriginAndDestination = new CHAMP17.AWBOriginAndDestination()
                {
                    AirportCityCodeOfOrigin = Context.FromPortCode,
                    AirportCityCodeOfDestination = Context.ToPortCode,
                },

                HouseWaybillTotals = new CHAMP17.HouseWaybillTotals()
                {
                    NumberOfPieces = Context.NumberOfPackages,                    
                    Weight = Context.GrossWeight,
                    WeightCode = Context.GrossWeightUnitCode,
                },
            };

            if (!string.IsNullOrEmpty(Context.DescriptionOfGoods))
            {
                summaryDetailsItem.NatureOfGoods = Context.DescriptionOfGoods;

                if (!string.IsNullOrEmpty(Context.SLAC))
                {
                    summaryDetailsItem.NatureOfGoods += Environment.NewLine + "SLAC: " + Context.SLAC;
                }

                if (Context.DescriptionOfGoodsTextList.Count > 0)
                {
                    summaryDetailsItem.FreeTextDescriptionOfGoods = new string[1] { Context.DescriptionOfGoodsTextList.FirstOrDefault() };
                }
            }

            #region OCI OtherCustomsInformation
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

                summaryDetailsItem.OtherCustomsInformation = shipmentOCIList.ToArray<CHAMP17.OtherCustomsInformation>();
            }
            #endregion

            if (!string.IsNullOrEmpty(Context.MainHarmonize))
            {
                if (isMultiHS)
                    summaryDetailsItem.HarmonisedTariffScheduleInformation = new string[3] { Context.MainHarmonize, "5555555", "7845100" };

                else
                    summaryDetailsItem.HarmonisedTariffScheduleInformation = new string[1] { Context.MainHarmonize };
            }

            summaryDetailsList.Add(summaryDetailsItem);
            myXSDElement.HouseWaybillSummaryDetails = summaryDetailsList.ToArray<CHAMP17.HouseWaybillSummaryDetails>();
            #endregion

            #region [4] Shipper
            myXSDElement.Shipper = new CHAMP17.Contact()
            {
                Name = new string[] { Context.ShipperName },
                StreetAddress = new string[] { Context.ShipperAddress },

                Location = new CHAMP17.Location()
                {
                    Place = Context.ShipperCity,
                },

                CodedLocation = new CHAMP17.CodedLocation()
                {
                    ISOCountryCode = Context.ShipperCountryCode,
                },
            };

            if (!string.IsNullOrEmpty(Context.ShipperZipCode))
            {
                myXSDElement.Shipper.CodedLocation.PostCode = Context.ShipperZipCode;
            }

            if (!string.IsNullOrEmpty(Context.ShipperStateCode))
            {
                myXSDElement.Shipper.Location.StateOrProvince = Context.ShipperStateCode;
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
                myXSDElement.Shipper.ContactDetail = ContactDetails_Shipper.ToArray<CHAMP17.ContactDetail>();
            }
            #endregion

            #region [5] Consignee
            myXSDElement.Consignee = new CHAMP17.Contact()
            {
                Name = new string[] { Context.ConsigneeName },
                StreetAddress = new string[] { Context.ConsigneeAddress },

                Location = new CHAMP17.Location()
                {
                    Place = Context.ConsigneeCity
                },

                CodedLocation = new CHAMP17.CodedLocation()
                {
                    ISOCountryCode = Context.ConsigneeCountryCode,
                },
            };

            if (!string.IsNullOrEmpty(Context.ConsigneeZipCode))
            {
                myXSDElement.Consignee.CodedLocation.PostCode = Context.ConsigneeZipCode;
            }

            if (!string.IsNullOrEmpty(Context.ConsigneeStateCode))
            {
                myXSDElement.Consignee.Location.StateOrProvince = Context.ConsigneeStateCode;
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
                myXSDElement.Consignee.ContactDetail = ContactDetails_Consignee.ToArray<CHAMP17.ContactDetail>();
            }
            #endregion

            return myXSDElement;
        }

        public GLSHK.FHL GetGLSHKFHL()
        {
            GLSHK.FHL myXSDElement = new GLSHK.FHL();

            #region AWB
            myXSDElement.AWB = new GLSHK.FHLAWB()
            {
                Prefix = Context.AirlinePrefix,
                SerialNum = Context.Master,
            };
            #endregion

            #region AWBOD
            myXSDElement.AWBOD = new GLSHK.OriginDestination()
            {
                Origin = Context.MainCarriageFromPortCode,
                Destination = Context.FinalDestinationPortCode,
            };
            #endregion

            #region Quantity
            myXSDElement.Quantity = new GLSHK.QuantityDetail()
            {
                 DescCode = "T",
                 Pieces = Context.MasterNumberOfPackages.ToString(),
                 Weight = Context.MasterGrossWeight,
                 WeightCode = Context.MasterGrossWeightUnitCode,
            };
            #endregion

            #region Item

            GLSHK.HWBSegDetail itemFHL = new GLSHK.HWBSegDetail();
            
            #region Shipper
            itemFHL.Shipper = new GLSHK.ContactAddress()
            {
                Name = Context.ShipperName,
                Place = Context.ShipperCity,
                ISOCountryCode = Context.ShipperCountryCode,                
            };

            if (!string.IsNullOrEmpty(Context.ShipperName2))
            {
                itemFHL.Shipper.Name2 = Context.ShipperName2;
            }

            if (!string.IsNullOrEmpty(Context.ShipperAddress1) && !string.IsNullOrEmpty(Context.ShipperAddress2))
            {
                itemFHL.Shipper.Address = Context.ShipperAddress1;
                itemFHL.Shipper.Address2 = Context.ShipperAddress2;
            }

            else
            {
                if (!string.IsNullOrEmpty(Context.ShipperAddress1))
                {
                    itemFHL.Shipper.Address = Context.ShipperAddress1;
                }

                else
                {
                    itemFHL.Shipper.Address = Context.ShipperAddress2;
                }
            }

            if (!string.IsNullOrEmpty(Context.ShipperZipCode))
            {
                itemFHL.Shipper.PostCode = Context.ShipperZipCode;
            }

            if (!string.IsNullOrEmpty(Context.ShipperStateCode))
            {
                itemFHL.Shipper.StateProvince = Context.ShipperStateCode;
            }

            if (!string.IsNullOrEmpty(Context.ShipperPhone))
            {
                GLSHK.Contact contactDetail = new GLSHK.Contact()
                {
                    ContactID = "TE",
                    ContactNum = Context.ShipperPhone,
                };

                itemFHL.Shipper.ContactDetail = new GLSHK.Contact[1] { contactDetail };
            }
            #endregion

            #region Consignee
            itemFHL.Consignee = new GLSHK.ContactAddress()
            {
                Name = Context.ConsigneeName,
                Place = Context.ConsigneeCity,
                ISOCountryCode = Context.ConsigneeCountryCode,                
            };

            if (!string.IsNullOrEmpty(Context.ConsigneeName2))
            {
                itemFHL.Consignee.Name2 = Context.ConsigneeName2;
            }

            if (!string.IsNullOrEmpty(Context.ConsigneeAddress1) && !string.IsNullOrEmpty(Context.ConsigneeAddress2))
            {
                itemFHL.Consignee.Address = Context.ConsigneeAddress1;
                itemFHL.Consignee.Address2 = Context.ConsigneeAddress2;
            }

            else
            {
                if (!string.IsNullOrEmpty(Context.ConsigneeAddress1))
                {
                    itemFHL.Consignee.Address = Context.ConsigneeAddress1;
                }

                else
                {
                    itemFHL.Consignee.Address = Context.ConsigneeAddress2;
                }
            }

            if (!string.IsNullOrEmpty(Context.ConsigneeZipCode))
            {
                itemFHL.Consignee.PostCode = Context.ConsigneeZipCode;
            }

            if (!string.IsNullOrEmpty(Context.ConsigneeStateCode))
            {
                itemFHL.Consignee.StateProvince = Context.ConsigneeStateCode;
            }

            if (!string.IsNullOrEmpty(Context.ConsigneePhone))
            {
                GLSHK.Contact contactDetail = new GLSHK.Contact()
                {
                    ContactID = "TE",
                    ContactNum = Context.ConsigneePhone,
                };

                itemFHL.Consignee.ContactDetail = new GLSHK.Contact[1] { contactDetail };
            }
            #endregion

            #region ChargeDeclaration

            itemFHL.ChargeDeclarations = new GLSHK.ChargeDeclaration()
            {
                CurrencyCode = Context.AWBCurrencyCode,
                ChargeCode = Context.AWBChargeCode,

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
                itemFHL.ChargeDeclarations.DeclaredValueCarriage = new GLSHK.DeclaredValue() { Items = new object[1] { Context.ChargeCarriageValue } };
            }
            else
            {
                itemFHL.ChargeDeclarations.DeclaredValueCarriage = new GLSHK.DeclaredValue() { Items = new object[1] { "" } };
            }

            if (Context.IsChargeCustomsDeclared)
            {
                itemFHL.ChargeDeclarations.DeclaredValueCustoms = new GLSHK.DeclaredValue() { Items = new object[1] { Context.ChargeCustomsValue } };
            }
            else
            {
                itemFHL.ChargeDeclarations.DeclaredValueCustoms = new GLSHK.DeclaredValue() { Items = new object[1] { "" } };
            }

            if (Context.IsChargeInsurrenceDeclared)
            {
                itemFHL.ChargeDeclarations.DeclaredValueInsurance = new GLSHK.DeclaredValue() { Items = new object[1] { Context.ChargeInsurrenceValue } };
            }
            else
            {
                itemFHL.ChargeDeclarations.DeclaredValueInsurance = new GLSHK.DeclaredValue() { Items = new object[1] { "" } };
            }
            #endregion

            #region HouseWaybillSummaryDetail
            itemFHL.HouseWaybillSummaryDetail = new GLSHK.HWBGrp()
            {
                HWBSerialNum = Context.House,

                AWBOD = new GLSHK.OriginDestination()
                {
                    Origin = Context.FromPortCode,
                    Destination = Context.ToPortCode,
                }, 

                Pieces = Context.NumberOfPackages.ToString(),
                Weight = Context.GrossWeight,
                WeightCode = Context.GrossWeightUnitCode == "K" ? GLSHK.HWBGrpWeightCode.K : GLSHK.HWBGrpWeightCode.L,
            };

            if (!string.IsNullOrEmpty(Context.DescriptionOfGoods))
            {
                itemFHL.HouseWaybillSummaryDetail.NatureOfGoods = Context.DescriptionOfGoods;
            }

            if (Context.HandlingCodesList.Count > 0)
            {
                List<GLSHK.SpecialHandling> handlingCodesList = new List<GLSHK.SpecialHandling>();

                foreach (string item in Context.HandlingCodesList)
                {
                    handlingCodesList.Add(new GLSHK.SpecialHandling()
                    {
                         SpecialHandlingCode = item
                    });
                }

                itemFHL.HouseWaybillSummaryDetail.SpecialHandlingDetail = handlingCodesList.ToArray<GLSHK.SpecialHandling>();
            }
            #endregion

            #region FreeTextDescriptionOfGoods
            if (Context.DescriptionOfGoodsTextList.Count > 0)
            {
                List<GLSHK.FreeTextDOGDetail> freeTextList = new List<GLSHK.FreeTextDOGDetail>();

                foreach (string item in Context.DescriptionOfGoodsTextList)
                {
                    if (freeTextList.Count < 9)
                    {
                        freeTextList.Add(new GLSHK.FreeTextDOGDetail()
                        {
                            DescriptionOfGoods = item
                        });
                    }
                }

                itemFHL.FreeTextDescriptionOfGoods = freeTextList.ToArray<GLSHK.FreeTextDOGDetail>();
            }
            #endregion

            #region HarmonisedTariffScheduleInfo
            //List<GLSHK.HTSDetail> harmonisedCommoditiesList = new List<GLSHK.HTSDetail>();

            //GLSHK.HTSDetail d2 = new GLSHK.HTSDetail()
            //{
            //    HarmonisedCommodityCode = "",
            //};

            //itemFHL.HarmonisedTariffScheduleInfo = harmonisedCommoditiesList.ToArray<GLSHK.HTSDetail>();
            #endregion

            #region OCI OtherCustomsInfo
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

                itemFHL.OtherCustomsInfo = shipmentOCIList.ToArray<GLSHK.OtherCustoms>();
            }
            #endregion

            myXSDElement.Item = itemFHL;

            #endregion

            return myXSDElement;
        }
    }
}
