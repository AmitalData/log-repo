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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.XSD
{
    public class GLSHKCustomsContext
    {
        public int Tenant { get; set; }
        public string PIMA { get; set; }
        public string Recipient { get; set; }
        public string ShipmentId { get; set; }
        public Shipment Shipment { get; set; }
        public List<Shipment> AllHouses { get; set; }
        public ShipmentMasterData MasterData { get; set; }
        public List<HouseContext> AllHousesContext { get; set; }
        public GLSHKCustomsContext(Shipment myShipment, ShipmentMasterData myMasterData, List<Shipment> myAllHouses, string myPIMA, string myRecipient)
        {
            this.PIMA = myPIMA;
            this.Recipient = myRecipient;
            this.Tenant = myShipment.Tenant;
            this.ShipmentId = myShipment.Id;
            this.Shipment = myShipment;
            this.AllHouses = myAllHouses;
            this.MasterData = myMasterData;
            
            this.BuidBaseData();
            this.BuildRoutingData();
            this.BuildAmountsData();
            this.BuildHousesArray();
        }

        #region Members
        public string Master { get; set; }
        public string ManifestStatusCode { get; set; }
        public string ShipmentNumber { get; set; }
        public string ShipmentNumberTrimmed { get; set; }
        public string AgentCode { get; set; }
        public bool IsRACodeActivated { get; set; }
        public bool IsViaColoader { get; set; }
        public string ColoaderKey { get; set; }
        public string RefID { get; set; }
        public string RefFullID { get; set; }
        public string AirlinePrefix { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public string MainCarriageCarrierETDFormatted { get; set; }
        public string MainCarriageFromPortCode { get; set; }
        public string MainCarriageFromPortCountryCode { get; set; }
        public string FinalDestinationPortCode { get; set; }
        public string FinalDestinationPortCountryCode { get; set; }
        public decimal GrossWeight { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public int NumberOfPackages { get; set; }
        #endregion

        private void BuidBaseData()
        {           
            this.ShipmentNumber = string.IsNullOrEmpty(Shipment.ShipmentNumber) ? null : Shipment.ShipmentNumber;
            this.Master = string.IsNullOrEmpty(MasterData.Master) ? null : FormatHelper.FormatInteger(8, MasterData.Master);
            this.ManifestStatusCode = MasterData.ManifestStatusCode;

            this.ShipmentNumberTrimmed = this.ShipmentNumber;
            if (ShipmentNumberTrimmed != null)
            {
                if (ShipmentNumberTrimmed.Length > 14)
                {
                    ShipmentNumberTrimmed = ShipmentNumberTrimmed.Substring(0, 14);
                }
            }

            if (Shipment.ViaColoader)
            {
                this.IsViaColoader = true;

                if (!string.IsNullOrEmpty(Shipment.IssuingCarrierReference1))
                {
                    this.ColoaderKey = FormatHelper.FormatString(Shipment.IssuingCarrierReference1, FormatHelper.PatternType.Text, 50);
                }
            }

            TenantRepository tenantRepository = new TenantRepository(Tenant);
            Tenant myTenant = tenantRepository.GetSingleTenant(Tenant);
            if (myTenant != null)
            {
                this.AgentCode = FormatHelper.FormatString(myTenant.LocalCustomsCode, FormatHelper.PatternType.Text, 7);
                if (myTenant.RegulatedAgentRegimeActivated)
                {
                    this.IsRACodeActivated = true;
                }
            }
        }
        private void BuildRoutingData()
        {
            if (!string.IsNullOrEmpty(MasterData.MainCarriageFromPortId) && !string.IsNullOrEmpty(MasterData.MainCarriageToPortId))
            {
                PortPM myFromPort = PortQuery.GetSinglePort(Tenant, MasterData.MainCarriageFromPortId, false);
                PortPM myFinalPort = PortQuery.GetSinglePort(Tenant, MasterData.MainCarriageFinalDestinationPortId, false);
                if (myFromPort != null)
                {
                    this.MainCarriageFromPortCode = myFromPort.Code.ToUpper();

                    if (myFromPort.CountryCode != null)
                    {
                        this.MainCarriageFromPortCountryCode = myFromPort.CountryCode;
                    }
                }

                if (myFinalPort != null)
                {
                    this.FinalDestinationPortCode = myFinalPort.Code.ToUpper();

                    if (myFinalPort.CountryCode != null)
                    {
                        this.FinalDestinationPortCountryCode = myFinalPort.CountryCode;
                    }
                }

                this.AirlinePrefix = FormatHelper.FormatInteger(3, MasterData.AirlinePrefix);

                if (!string.IsNullOrEmpty(MasterData.MainCarriageCarrierId))
                {
                    if (!string.IsNullOrEmpty(MasterData.MainCarriageCarrierNumber))
                    {
                        this.MainCarriageCarrierNumber = FormatHelper.FormatString(MasterData.MainCarriageCarrierNumber, FormatHelper.PatternType.FlightNumber);
                    }
                }

                if (MasterData.MainCarriageETD != null)
                {
                    this.MainCarriageCarrierETDFormatted = String.Format("{0:yyMMdd}", MasterData.MainCarriageETD.Value);
                }
            }

            this.RefID = "HMF" + Master.Substring(0, 7) + "X" + AirlinePrefix;
            this.RefFullID = "HMF" + Master + "X" + AirlinePrefix;
        }
        private void BuildAmountsData()
        {
            int myPieces = 0;
            decimal myGrossWeight = 0;
            string myGrossWeightUnitCode = "KGM";

            if (Shipment.GrossWeightUnitCode == "LB")
            {
                myGrossWeightUnitCode = "LBR";
            }

            if (Shipment.NumberOfPackages != null)
            {
                myPieces = Shipment.NumberOfPackages.Value;
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
        private void BuildHousesArray()
        {
            this.AllHousesContext = new List<HouseContext>();

            if (Shipment.ShipmentLevelCode == "D")
            {
                this.AllHousesContext.Add(new HouseContext(Shipment, 1));
            }

            else if (Shipment.ShipmentLevelCode == "C")
            {
                if (AllHouses.Count > 0)
                {
                    int index = 1;

                    foreach (Shipment item in AllHouses)
                    {
                        this.AllHousesContext.Add(new HouseContext(item, index));
                        index++;
                    }
                }
            }
        }
    }

    public class HouseContext
    {
        [Key]
        public string SerialNumber { get; set; }
        public string House { get; set; }
        public int NumberOfPackages { get; set; }
        public decimal GrossWeight { get; set; }
        public string GrossWeightUnitCode { get; set; }

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

        #region Others
        public string ManifestDescription { get; set; }
        public List<string> HandlingCodesList { get; set; }
        public List<string> DescriptionOfGoodsList { get; set; }
        #endregion

        public int Tenant { get; set; }
        public Shipment Shipment { get; set; }
        public HouseContext(Shipment myShipment, int index)
        {
            this.Shipment = myShipment;
            this.Tenant = myShipment.Tenant;
            this.SerialNumber = index.ToString();
            
            if (!string.IsNullOrEmpty(myShipment.House))
            {
                House = FormatHelper.FormatString(myShipment.House, FormatHelper.PatternType.HWBSerialNumber);
            }

            this.BuildAmountsData();
            this.BuildPartnersData();
            this.BuildHandlingCodesList();
            this.BuildDescriptionOfGoods();
            this.BuildChargeDeclarationsData();
        }

        private void BuildAmountsData()
        {
            int myPieces = 0;
            decimal myGrossWeight = 0;
            string myGrossWeightUnitCode = "KGM";

            if (Shipment.GrossWeightUnitCode == "LB")
            {
                myGrossWeightUnitCode = "LBR";
            }

            if (Shipment.NumberOfPackages != null)
            {
                myPieces = Shipment.NumberOfPackages.Value;
            }

            if (myGrossWeightUnitCode == "KGM")
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
        private void BuildPartnersData()
        {
            StateRepository stateRepository = new StateRepository(Tenant);
            AddressRepository addressRepository = new AddressRepository(Tenant);

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

                if (!string.IsNullOrEmpty(Shipment.ShipperContactId))
                {
                    Contact myContact = ContactRepository.GetSingleContact(Shipment.ShipperContactId, Tenant, false);
                    if (myContact != null)
                    {
                        this.ShipperPhone = FormatHelper.FormatString(myContact.BusinessPhone, FormatHelper.PatternType.Phone);
                        this.ShipperFax = FormatHelper.FormatString(myContact.Fax, FormatHelper.PatternType.Phone);
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

                if (!string.IsNullOrEmpty(Shipment.ConsigneeContactId))
                {
                    Contact myContact = ContactRepository.GetSingleContact(Shipment.ConsigneeContactId, Tenant, false);
                    if (myContact != null)
                    {
                        this.ConsigneePhone = FormatHelper.FormatString(myContact.BusinessPhone, FormatHelper.PatternType.Phone);
                        this.ConsigneeFax = FormatHelper.FormatString(myContact.Fax, FormatHelper.PatternType.Phone);
                    }
                }
            }
            #endregion
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
        private void BuildDescriptionOfGoods()
        {
            string myDescriptionOfGoods = null;

            if (Shipment.IsMultipleCommodities)
            {
                ShipmentCommodityRepository myRepository = new ShipmentCommodityRepository(Tenant);
                IQueryable<ShipmentCommodity> shipmentCommodities = myRepository.GetCommoditiesbyShipmentId(Shipment.Id, Tenant);
                if (shipmentCommodities != null)
                {
                    ShipmentCommodity myShipmentCommodity = shipmentCommodities.FirstOrDefault();
                    //ShipmentCommodity myShipmentCommodity = shipmentCommodities.Where(d => d.IsFirstLine).FirstOrDefault();
                    if (myShipmentCommodity != null)
                    {
                        myDescriptionOfGoods = myShipmentCommodity.DescriptionOfGoods;
                    }
                }
            }

            else
            {
                myDescriptionOfGoods = Shipment.DescriptionOfGoods;
            }

            if (!string.IsNullOrEmpty(myDescriptionOfGoods))
            {
                this.DescriptionOfGoodsList = new List<string>();
                this.ManifestDescription = FormatHelper.FormatString(myDescriptionOfGoods, FormatHelper.PatternType.Text, 70);

                int itemLength = 65;
                string xmlField = "";
                string systemField = myDescriptionOfGoods;

                if (systemField != null)
                {
                    systemField = Regex.Replace(systemField, @"(\r)(\1)+", "$1");
                    systemField = systemField.Replace("\r", " ");
                    systemField = FormatHelper.FormatString(systemField, FormatHelper.PatternType.Text);
                }

                for (int i = 1; i <= 3; i++)
                {
                    if (DescriptionOfGoodsList.Count < 9)
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
                            this.DescriptionOfGoodsList.Add(xmlField);
                        }
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
    }
}
