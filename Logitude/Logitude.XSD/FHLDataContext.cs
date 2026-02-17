using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
    public class FHLDataContext
    {
        #region Properties

        public string House { get; set; }
        public string Master { get; set; }
        public string ShipmentNumber { get; set; }
        public string MasterShipmentNumber { get; set; }
        public string FromPortCode { get; set; }
        public string ToPortCode { get; set; }
        public string MainCarriageFromPortCode { get; set; }
        public string AirlinePrefix { get; set; }
        public string FinalDestinationPortCode { get; set; }
        public string DescriptionOfGoods { get; set; }
        public List<string> DescriptionOfGoodsTextList { get; set; }
        public List<string> HandlingCodesList { get; set; }
        public List<ShipmentOCIItem> ShipmentOCIList { get; set; }
        public bool IsViaColoader { get; set; }
        public string ColoaderKey { get; set; }
        public string MainHarmonize { get; set; }

        #region Amounts
        public int NumberOfPackages { get; set; }
        public decimal GrossWeight { get; set; }
        public string GrossWeightUnitCode { get; set; }

        public int MasterNumberOfPackages { get; set; }
        public decimal MasterGrossWeight { get; set; }
        public string MasterGrossWeightUnitCode { get; set; }
        #endregion

        #region Shipper
        public string ShipperName { get; set; }
        public string ShipperName2 { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperPhone { get; set; }
        public string ShipperFax { get; set; }
        public string ShipperZipCode { get; set; }
        public string ShipperAddress { get; set; }
        public string ShipperAddress1 { get; set; }
        public string ShipperAddress2 { get; set; }
        public string ShipperStateCode { get; set; }
        public string ShipperCountryCode { get; set; }
        #endregion

        #region Consignee
        public string ConsigneeName { get; set; }
        public string ConsigneeName2 { get; set; }
        public string ConsigneeCity { get; set; }
        public string ConsigneePhone { get; set; }
        public string ConsigneeFax { get; set; }
        public string ConsigneeZipCode { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ConsigneeAddress1 { get; set; }
        public string ConsigneeAddress2 { get; set; }
        public string ConsigneeStateCode { get; set; }
        public string ConsigneeCountryCode { get; set; }
        #endregion

        #region ChargeDeclarations
        public string AWBChargeCode { get; set; }
        public string AWBCurrencyCode { get; set; }
        public string FreightPrepaidCollectId { get; set; }
        public string OtherPrepaidCollectId { get; set; }
        public decimal ChargeCustomsValue { get; set; }
        public decimal ChargeCarriageValue { get; set; }
        public decimal ChargeInsurrenceValue { get; set; }
        public bool IsChargeCustomsDeclared { get; set; }
        public bool IsChargeCarriageDeclared { get; set; }
        public bool IsChargeInsurrenceDeclared { get; set; }
        #endregion

        #endregion

        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public Shipment Shipment { get; set; }
        public Shipment MasterShipment { get; set; }
        public ShipmentMasterData MasterData { get; set; }
        private StateRepository stateRepository;
        private AddressRepository addressRepository;
        public FHLDataContext(Shipment myShipment, ShipmentMasterData myMasterData,Shipment myMasterShipment)
        {
            this.Tenant = myShipment.Tenant;
            this.ShipmentId = myShipment.Id;
            this.Shipment = myShipment;            
            this.MasterData = myMasterData;
            this.MasterShipment = myMasterShipment;
            this.stateRepository = new StateRepository(Tenant);
            this.addressRepository = new AddressRepository(Tenant);

            this.BuidBaseData();
            this.BuildAmountsData();
            this.BuildMasterAmountsData();
            this.BuildRoutingData();
            this.BuildPartnersData();
            this.BuildDescriptionOfGoods();
            this.BuildChargeDeclarationsData();
            this.BuildHandlingCodesList();
            this.BuildShipmentAWBOCIList();
        }

        private void BuidBaseData()
        {
            this.House = FormatHelper.FormatString(Shipment.House, FormatHelper.PatternType.HWBSerialNumber);
            this.Master = string.IsNullOrEmpty(MasterData.Master) ? null : FormatHelper.FormatInteger(8, MasterData.Master);
            this.ShipmentNumber = string.IsNullOrEmpty(Shipment.ShipmentNumber) ? null : Shipment.ShipmentNumber;
            this.MasterShipmentNumber = string.IsNullOrEmpty(MasterData.MasterShipmentNumber) ? null : MasterData.MasterShipmentNumber;
            this.MainHarmonize = string.IsNullOrEmpty(Shipment.MainHarmonize) ? null : FormatHelper.FormatString(Shipment.MainHarmonize, FormatHelper.PatternType.AlphaNumeric, 18);

            if (Shipment.ViaColoader)
            {
                this.IsViaColoader = true;

                if (!string.IsNullOrEmpty(Shipment.IssuingCarrierReference1))
                {
                    this.ColoaderKey = FormatHelper.FormatString(Shipment.IssuingCarrierReference1, FormatHelper.PatternType.Text, 50);
                }
            }
        }
        private void BuildAmountsData()
        {
            int myPieces = Shipment.NumberOfPackages == null ? 0 : Shipment.NumberOfPackages.Value;

            decimal myGrossWeight = 0;
            string myGrossWeightUnitCode = "K";
            if (Shipment.GrossWeightUnitCode == "LB")
            {
                myGrossWeightUnitCode = "L";
            }
            
            if (myGrossWeightUnitCode == "K")
            {
                myGrossWeight = (Shipment.GrossWeightInKG == null) ? 0 : (decimal)Shipment.GrossWeightInKG.Value;
            }

            else
            {
                myGrossWeight = (Shipment.GrossWeight == null) ? 0 : (decimal)Shipment.GrossWeight.Value;
            }

            this.NumberOfPackages = myPieces;
            this.GrossWeight = MethodHelper.Normalize(myGrossWeight);
            this.GrossWeightUnitCode = myGrossWeightUnitCode;
        }
        private void BuildMasterAmountsData()
        {
            this.MasterNumberOfPackages = MasterShipment.NumberOfPackages == null ? 0 : MasterShipment.NumberOfPackages.Value;

            decimal myGrossWeight = 0;
            string myGrossWeightUnitCode = "K";
            if (MasterShipment.GrossWeightUnitCode == "LB")
            {
                myGrossWeightUnitCode = "L";
            }

            if (myGrossWeightUnitCode == "K")
            {
                myGrossWeight = (MasterShipment.GrossWeightInKG == null) ? 0 : (decimal)MasterShipment.GrossWeightInKG;
            }
            else
            {
                myGrossWeight = (MasterShipment.GrossWeight == null) ? 0 : (decimal)MasterShipment.GrossWeight;
            }

            this.MasterGrossWeight = MethodHelper.Normalize(myGrossWeight);
            this.MasterGrossWeightUnitCode = myGrossWeightUnitCode;          
        }
        private void BuildRoutingData()
        {
            this.FromPortCode = Shipment.FromPort.Code.ToUpper();
            this.ToPortCode = Shipment.ToPort.Code.ToUpper();

            if (!string.IsNullOrEmpty(MasterData.MainCarriageFromPortId) && !string.IsNullOrEmpty(MasterData.MainCarriageToPortId))
            {
                PortPM myFromPort = PortQuery.GetSinglePort(Tenant, MasterData.MainCarriageFromPortId, false);
                PortPM myFinalPort = PortQuery.GetSinglePort(Tenant, MasterData.MainCarriageFinalDestinationPortId, false);

                this.MainCarriageFromPortCode = myFromPort.Code.ToUpper();
                this.FinalDestinationPortCode = myFinalPort.Code.ToUpper();

                this.AirlinePrefix = FormatHelper.FormatInteger(3, MasterData.AirlinePrefix);
            }
        }
        private void BuildPartnersData()
        {
            #region Shipper
            if (!string.IsNullOrEmpty(Shipment.ShipperId))
            {
                Card myCard = CardRepository.GetSingleCard(Shipment.ShipperId, Tenant, false);
                if (myCard != null)
                {
                    string myField = FormatHelper.FormatString(myCard.EnglishName, FormatHelper.PatternType.Text);
                    if (!string.IsNullOrEmpty(myField))
                    {
                        this.ShipperName = FormatHelper.FormatString(myField, FormatHelper.PatternType.Name);

                        if (myField.Length > 35)
                        {
                            this.ShipperName2 = FormatHelper.FormatString(myField.Substring(34), FormatHelper.PatternType.Name);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Shipment.ShipperAddressId))
                {
                    Address myAddress = addressRepository.GetSingleAddress(Shipment.ShipperAddressId, Tenant);
                    if (myAddress != null)
                    {
                        this.ShipperAddress = FormatHelper.FormatString(myAddress.Address1 + " " + myAddress.Address2, FormatHelper.PatternType.Address);
                        this.ShipperAddress1 = FormatHelper.FormatString(myAddress.Address1, FormatHelper.PatternType.Address);
                        this.ShipperAddress2 = FormatHelper.FormatString(myAddress.Address2, FormatHelper.PatternType.Address);
                        this.ShipperCity = FormatHelper.FormatString(myAddress.City, FormatHelper.PatternType.City);
                        this.ShipperZipCode = FormatHelper.FormatString(myAddress.ZipCode, FormatHelper.PatternType.ZipCode);
                        this.ShipperPhone = FormatHelper.FormatString(myAddress.PhoneNumber, FormatHelper.PatternType.Phone);
                        this.ShipperFax = FormatHelper.FormatString(myAddress.FaxNumber, FormatHelper.PatternType.Phone);

                        Country myCountry = CountryRepository.GetSingleCountry(myAddress.CountryId, Tenant, false);
                        if (myCountry != null)
                        {
                            this.ShipperCountryCode = string.IsNullOrEmpty(myCountry.Code) ? "" : myCountry.Code.ToUpper();
                        }

                        if (!string.IsNullOrEmpty(myAddress.StateId))
                        {
                            State myState = stateRepository.GetSingleState(myAddress.StateId, Tenant);
                            if (myState != null)
                            {
                                this.ShipperStateCode = myState.Code.ToUpper();
                            }
                        }
                    }
                }
            }
            #endregion

            #region Consignee
            if (!string.IsNullOrEmpty(Shipment.ConsigneeId))
            {
                Card myCard = CardRepository.GetSingleCard(Shipment.ConsigneeId, Tenant, false);
                if (myCard != null)
                {
                    string myField = FormatHelper.FormatString(myCard.EnglishName, FormatHelper.PatternType.Text);
                    if (!string.IsNullOrEmpty(myField))
                    {
                        this.ConsigneeName = FormatHelper.FormatString(myField, FormatHelper.PatternType.Name);

                        if (myField.Length > 35)
                        {
                            this.ConsigneeName2 = FormatHelper.FormatString(myField.Substring(34), FormatHelper.PatternType.Name);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Shipment.ConsigneeAddressId))
                {
                    Address myAddress = addressRepository.GetSingleAddress(Shipment.ConsigneeAddressId, Tenant);
                    if (myAddress != null)
                    {
                        this.ConsigneeAddress = FormatHelper.FormatString(myAddress.Address1 + " " + myAddress.Address2, FormatHelper.PatternType.Address);
                        this.ConsigneeAddress1 = FormatHelper.FormatString(myAddress.Address1, FormatHelper.PatternType.Address);
                        this.ConsigneeAddress2 = FormatHelper.FormatString(myAddress.Address2, FormatHelper.PatternType.Address);
                        this.ConsigneeCity = FormatHelper.FormatString(myAddress.City, FormatHelper.PatternType.City);
                        this.ConsigneeZipCode = FormatHelper.FormatString(myAddress.ZipCode, FormatHelper.PatternType.ZipCode);
                        this.ConsigneePhone = FormatHelper.FormatString(myAddress.PhoneNumber, FormatHelper.PatternType.Phone);
                        this.ConsigneeFax = FormatHelper.FormatString(myAddress.FaxNumber, FormatHelper.PatternType.Phone);

                        Country myCountry = CountryRepository.GetSingleCountry(myAddress.CountryId, Tenant, false);
                        if (myCountry != null)
                        {
                            this.ConsigneeCountryCode = string.IsNullOrEmpty(myCountry.Code) ? "" : myCountry.Code.ToUpper();
                        }

                        if (!string.IsNullOrEmpty(myAddress.StateId))
                        {
                            State myState = stateRepository.GetSingleState(myAddress.StateId, Tenant);
                            if (myState != null)
                            {
                                this.ConsigneeStateCode = myState.Code.ToUpper();
                            }
                        }
                    }
                }
            }
            #endregion
        }
        private void BuildDescriptionOfGoods()
        {
            this.DescriptionOfGoodsTextList = new List<string>();

            if (!string.IsNullOrEmpty(Shipment.DescriptionOfGoods))
            {
                this.DescriptionOfGoods = Shipment.DescriptionOfGoods;

                if (DescriptionOfGoods != null)
                {
                    this.DescriptionOfGoods = Regex.Replace(DescriptionOfGoods, @"(\r)(\1)+", "$1");
                    this.DescriptionOfGoods = DescriptionOfGoods.Replace("\r", " ");
                    this.DescriptionOfGoods = FormatHelper.FormatString(DescriptionOfGoods, FormatHelper.PatternType.Text, 15);
                }

                int itemLength = 65;
                string xmlField = "";
                string systemField = Shipment.DescriptionOfGoods;

                if (systemField != null)
                {
                    systemField = Regex.Replace(systemField, @"(\r)(\1)+", "$1");
                    systemField = systemField.Replace("\r", " ");
                    systemField = FormatHelper.FormatString(systemField, FormatHelper.PatternType.Text);
                }

                while (systemField.Length > 0)
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
                        DescriptionOfGoodsTextList.Add(xmlField);
                    }
                }
            }
        }
        private void BuildChargeDeclarationsData()
        {
            string myField = null;
            this.AWBChargeCode = string.IsNullOrEmpty(Shipment.AWBChargesCodeCode) ? null : Shipment.AWBChargesCodeCode.ToUpper();
            this.FreightPrepaidCollectId = string.IsNullOrEmpty(Shipment.FreightPrepaidCollectId) ? null : Shipment.FreightPrepaidCollectId.ToUpper();
            this.OtherPrepaidCollectId = string.IsNullOrEmpty(Shipment.OtherPrepaidCollectId) ? null : Shipment.OtherPrepaidCollectId.ToUpper();

            if (!string.IsNullOrEmpty(Shipment.AWBCurrencyId))
            {
                Currency myCurrency = CurrencyRepository.GetSingleCurrency(Shipment.AWBCurrencyId, Tenant, false);
                if (myCurrency != null)
                {
                    this.AWBCurrencyCode = myCurrency.Code.ToUpper();
                }
            }

            myField = Shipment.AWBDeclaredValueForCarriage;
            if (!string.IsNullOrEmpty(myField))
            {
                myField = myField.Replace(" ", "").ToUpper();
                if (myField != "NVD")
                {
                    this.ChargeCarriageValue = FormatHelper.FormatDecimal(myField, 12);
                    this.IsChargeCarriageDeclared = true;
                }
            }

            myField = Shipment.AWBDeclaredValueForCustoms;
            if (!string.IsNullOrEmpty(myField))
            {
                myField = myField.Replace(" ", "").ToUpper();
                if (myField != "NCV")
                {
                    this.ChargeCustomsValue = FormatHelper.FormatDecimal(myField, 12);
                    this.IsChargeCustomsDeclared = true;
                }
            }

            // 12 : 11 ?
            myField = Shipment.AWBInsurrenceValue;
            if (!string.IsNullOrEmpty(myField))
            {
                myField = myField.Replace(" ", "").ToUpper();
                if (myField != "XXX")
                {
                    this.ChargeInsurrenceValue = FormatHelper.FormatDecimal(myField, 11);
                    this.IsChargeInsurrenceDeclared = true;
                }
            }
        }
        private void BuildHandlingCodesList()
        {
            // System DB Code Length: 4
            // Champ XML Code Length: 3

            int itemLength = 3;
            string xmlField = "";
            string systemField = "";

            this.HandlingCodesList = new List<string>();

            string myFieldId = null;
            AWBSpecialHandlingCodeRepository myRepository = new AWBSpecialHandlingCodeRepository(Tenant);
            List<AWBSpecialHandlingCode> allCodes = myRepository.GetAWBHandlingCodes().ToList();

            myFieldId = Shipment.AWBSpecialHandlingCodeId1;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Shipment.AWBSpecialHandlingCodeId2;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Shipment.AWBSpecialHandlingCodeId3;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Shipment.AWBSpecialHandlingCodeId4;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Shipment.AWBSpecialHandlingCodeId5;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Shipment.AWBSpecialHandlingCodeId6;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Shipment.AWBSpecialHandlingCodeId7;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Shipment.AWBSpecialHandlingCodeId8;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Shipment.AWBSpecialHandlingCodeId9;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }
        }
        private void BuildShipmentAWBOCIList()
        {
            AWBOCIRepository aWBOCIRepository = new AWBOCIRepository(Tenant);
            List<AWBOCI> list = aWBOCIRepository.GetAWBOCIsbyShipmentId(ShipmentId, Tenant).ToList();

            this.ShipmentOCIList = new List<ShipmentOCIItem>();

            if (list.Count > 0)
            {
                foreach (AWBOCI item in list)
                {
                    ShipmentOCIItem oCIRecord = new ShipmentOCIItem();

                    Country myCountry = CountryRepository.GetSingleCountry(item.CountryId, Tenant, false);
                    if (myCountry != null)
                    {
                        oCIRecord.CountryCode = myCountry.Code.ToUpper();
                    }

                    if (!string.IsNullOrEmpty(item.AWBInformationCode))
                    {
                        oCIRecord.InformationCode = item.AWBInformationCode.ToUpper();
                    }

                    if (!string.IsNullOrEmpty(item.AWBCustomsInformationCode))
                    {
                        oCIRecord.CustomsInformationCode = item.AWBCustomsInformationCode.ToUpper();
                    }

                    if (!string.IsNullOrEmpty(item.SupplementaryCustomsInfo))
                    {
                        oCIRecord.SupplementaryCustomsInfo = FormatHelper.FormatString(item.SupplementaryCustomsInfo, FormatHelper.PatternType.Text);
                    }

                    this.ShipmentOCIList.Add(oCIRecord);
                }
            }
        }
    }
}
