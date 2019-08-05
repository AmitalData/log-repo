using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.XSD
{
    public class FWBDataContext
    {
        #region Properties

        public string SCI { get; set; }
        public string Master { get; set; }
        public string ShipmentNumber { get; set; }
        public string AccountNumber { get; set; }        
        public string ShipmentLevelCode { get; set; }
        public bool IsViaColoader { get; set; }
        public string ColoaderKey { get; set; }
        public bool IsRACodeActivated { get; set; }
        public bool IsKnownCargo { get; set; }
        public string KnownConsignorNumber { get; set; }
        public string MainHarmonize { get; set; }
        public string FNANotifyDetails { get; set; }

        #region Carriers
        public string AirlinePrefix { get; set; }
        public string MainCarriageCarrierCode { get; set; }
        public string Transshipment1CarrierCode { get; set; }
        public string Transshipment2CarrierCode { get; set; }
        public string Transshipment3CarrierCode { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public string Transshipment1CarrierNumber { get; set; }
        public string Transshipment2CarrierNumber { get; set; }
        public string Transshipment3CarrierNumber { get; set; }
        public string MainCarriageCarrierETDDay { get; set; }
        public string Transshipment1CarrierETDDay { get; set; }
        public string Transshipment2CarrierETDDay { get; set; }
        public string Transshipment3CarrierETDDay { get; set; }
        public DateTime MainCarriageCarrierETD { get; set; }
        public DateTime Transshipment1CarrierETD { get; set; }
        public DateTime Transshipment2CarrierETD { get; set; }
        public DateTime Transshipment3CarrierETD { get; set; }

        #endregion

        #region Ports
        public string MainCarriageFromPortCode { get; set; }
        public string MainCarriageFromPortName { get; set; }
        public string MainCarriageToPortCode { get; set; }
        public string FinalDestinationPortCode { get; set; }
        public string Transshipment1ToPortCode { get; set; }
        public string Transshipment2ToPortCode { get; set; }
        public string Transshipment3ToPortCode { get; set; }

        public string MainCarriageFromPortId { get; set; }
        public string Transshipment1FromPortId { get; set; }
        public string Transshipment2FromPortId { get; set; }
        public string Transshipment3FromPortId { get; set; }
        public string MainCarriageToPortId { get; set; }
        public string Transshipment1ToPortId { get; set; }
        public string Transshipment2ToPortId { get; set; }
        public string Transshipment3ToPortId { get; set; }
        #endregion

        #region Amounts
        public decimal Volume { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal ChargeableWeight { get; set; }
        public int NumberOfPackages { get; set; }
        public string VolumeUnitCode { get; set; }
        public string DimensionsUnitCode { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
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

        #region Notify1
        public string Notify1Name { get; set; }
        public string Notify1Name2 { get; set; }
        public string Notify1City { get; set; }
        public string Notify1Phone { get; set; }
        public string Notify1Fax { get; set; }
        public string Notify1ZipCode { get; set; }
        public string Notify1Address { get; set; }
        public string Notify1Address1 { get; set; }
        public string Notify1Address2 { get; set; }
        public string Notify1StateCode { get; set; }
        public string Notify1CountryCode { get; set; }
        #endregion

        #region IssuingCarrier
        public string IssuingCarrierAgentName { get; set; }
        public string IssuingCarrierAgentCity { get; set; }
        public string IssuingCarrierIATACode { get; set; }
        public string IssuingCarrierCASSCode { get; set; }
        public bool IsIssuingCarrierIATACodeSet { get; set; }
        public bool IsIssuingCarrierCASSCodeSet { get; set; }
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

        #region RateDescription
        public bool IsMultipleCommodities { get; set; }
        public string AWBCommodityItemNumber { get; set; }
        public List<ShipmentPackage> ShipmentPackages { get; set; }
        public List<ShipmentCommodityPM> ShipmentCommodities { get; set; }
        public string RateClassCode { get; set; }
        public decimal AWBChargeRate { get; set; }
        public decimal AWBChargeAmount { get; set; }
        public bool AsAgreedFreight { get; set; }
        public bool AsAgreedOtherCharges { get; set; }
        public string NatureOfGoods { get; set; }
        public List<string> NatureOfGoodsList { get; set; }
        #endregion
        
        public List<string> SSRTextList { get; set; }
        public List<string> CommentsTextList { get; set; }
        public List<string> HandlingCodesList { get; set; }
        public List<AccountingInfoItem> AccountingInfoList { get; set; }
        public List<AccountingInfoItem> AccountingInfoList17 { get; set; }
        public List<OtherParticipantItem> OtherParticipantList { get; set; }
        public List<ShipmentOCIItem> ShipmentOCIList { get; set; }

        #region Prepaid Collect ChargeSummary
        public List<ShipmentAWBPrintOnly> AWBPrintOnlies { get; set; }
        public List<ShipmentPayable> ShipmentPayables { get; set; }
        public List<ShipmentReceivable> ShipmentReceivables { get; set; }
        public decimal AWBFreightAmountPrepaid { get; set; }
        public decimal AWBFreightAmountCollect { get; set; }

        public decimal PrepaidWeight { get; set; }
        public decimal PrepaidTaxes { get; set; }
        public decimal PrepaidValuation { get; set; }
        public decimal PrepaidDueAgent { get; set; }
        public decimal PrepaidDueCarrier { get; set; }
        public decimal PrepaidTotal { get; set; }
        public decimal PrepaidTotalWithoutWeight { get; set; }

        public decimal CollectWeight { get; set; }
        public decimal CollectTaxes { get; set; }
        public decimal CollectValuation { get; set; }
        public decimal CollectDueAgent { get; set; }
        public decimal CollectDueCarrier { get; set; }
        public decimal CollectTotal { get; set; }
        public decimal CollectTotalWithoutWeight { get; set; }
        #endregion

        #region CarrierExecution
        public DateTime? AWBMasterDate { get; set; }
        public string AWBMasterDay { get; set; }
        public string AWBMasterYear { get; set; }
        public string AWBMasterMonth { get; set; }
        public string AWBPlace { get; set; }
        public string AWBSignature { get; set; }
        #endregion

        #region NominatedHandlingParty
        public string NominatedHandlingPartyId { get; set; }
        public string NominatedHandlingPartyName { get; set; }
        public string NominatedHandlingPartyCity { get; set; }
        #endregion

        #region ShipmentReferenceInfo
        public bool IsReferenceInfoSpecified { get; set; }
        public string ReferenceNumber { get; set; }
        public string SupplementaryShipmentInformation1 { get; set; }
        public string SupplementaryShipmentInformation2 { get; set; }
        public List<string> SupplementaryTextList { get; set; }
        #endregion

        #endregion

        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public Shipment Shipment { get; set; }
        public ShipmentMasterData MasterData { get; set; }
        private StateRepository stateRepository;
        private AddressRepository addressRepository;
        private string myCCSTypeCode;
        public FWBDataContext(Shipment myShipment, ShipmentMasterData myMasterData, string myCCSTypeCode)
        {
            this.Tenant = myShipment.Tenant;
            this.ShipmentId = myShipment.Id;
            this.Shipment = myShipment;
            this.MasterData = myMasterData;
            this.myCCSTypeCode = myCCSTypeCode;
            this.stateRepository = new StateRepository(Tenant);
            this.addressRepository = new AddressRepository(Tenant);
           
            this.BuidBaseData();
            this.BuildAmountsData();
            this.BuildRoutingData();
            this.BuildPartnersData();
            this.BuildSSRTextList();
            this.BuildCommentsTextList();
            this.BuildHandlingCodesList();
            this.BuildAccountingInfoList();
            this.BuildChargeDeclarationsData();
            this.BuildRateDescriptionData();
            this.BuildOtherListsData();
            this.BuildChargeSummaryPrepaidData();
            this.BuildChargeSummaryCollectData();
            this.BuildCarrierExecutionData();
            this.BuildNominatedHandlingPartyData();
            this.BuildShipmentReferenceInfoData();
            this.BuildOtherParticipantsData();
            this.BuildShipmentAWBOCIList();
        }

        private void BuidBaseData()
        {
            this.SCI = string.IsNullOrEmpty(Shipment.SCI) ? null : FormatHelper.FormatString(Shipment.SCI, FormatHelper.PatternType.AlphaNumeric, 2);
            this.Master = string.IsNullOrEmpty(MasterData.Master) ? null : FormatHelper.FormatInteger(8, MasterData.Master);
            this.AccountNumber = string.IsNullOrEmpty(Shipment.AccountNumber) ? null : Shipment.AccountNumber;
            this.ShipmentNumber = string.IsNullOrEmpty(Shipment.ShipmentNumber) ? null : Shipment.ShipmentNumber;            
            this.ShipmentLevelCode = Shipment.ShipmentLevelCode;
            this.AWBFreightAmountPrepaid = Shipment.AWBFreightAmountPrepaid == null ? 0 : (decimal)Shipment.AWBFreightAmountPrepaid;
            this.AWBFreightAmountCollect = Shipment.AWBFreightAmountCollect == null ? 0 : (decimal)Shipment.AWBFreightAmountCollect;
            this.MainHarmonize = string.IsNullOrEmpty(Shipment.MainHarmonize) ? null : FormatHelper.FormatString(Shipment.MainHarmonize, FormatHelper.PatternType.AlphaNumeric, 18);

            this.IsKnownCargo = MasterData.IsKnownCargo;
            this.KnownConsignorNumber = MasterData.KnownConsignorNumber;

            if (myCCSTypeCode == "GLSHK")
            {
                TenantRepository tenantRepository = new TenantRepository(Tenant);
                Tenant myTenant = tenantRepository.GetSingleTenant(Tenant);
                if (myTenant != null)
                {
                    if (myTenant.RegulatedAgentRegimeActivated)
                    {
                        this.IsRACodeActivated = true;
                    }
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
        }
        private void BuildAmountsData()
        {
            string myVolumeUnitCode = "";
            string myDimensionsUnitCode = string.IsNullOrEmpty(Shipment.DimensionsUnitCode) ? "" : Shipment.DimensionsUnitCode.ToUpper();
            string myGrossWeightUnitCode = "K";
            string myChargeableWeightUnitCode = "K";

            if (Shipment.GrossWeightUnitCode == "LB")
            {
                myGrossWeightUnitCode = "L";
            }

            if (Shipment.ChargeableWeightUnitCode == "LB")
            {
                myChargeableWeightUnitCode = "L";
            }

            switch (myDimensionsUnitCode)
            {
                case "CM":
                    {
                        myDimensionsUnitCode = "CMT";
                        break;
                    }

                case "FT":
                    {
                        myDimensionsUnitCode = "FOT";
                        break;
                    }

                case "INC":
                    {
                        myDimensionsUnitCode = "INH";
                        break;
                    }
            }

            if (!string.IsNullOrEmpty(Shipment.VolumeUnitCode))
            {
                switch (Shipment.VolumeUnitCode.ToUpper())
                {
                    case "CBF": { myVolumeUnitCode = "CF"; break; }
                    case "CBI": { myVolumeUnitCode = "CI"; break; }
                    case "CBM": { myVolumeUnitCode = "MC"; break; }
                    default: { break; }
                }
            }

            int myPieces = Shipment.NumberOfPackages == null ? 0 : Shipment.NumberOfPackages.Value;

            decimal myVolume = 0;
            if (Shipment.Volume != null)
            {
                myVolume = (decimal)MethodHelper.Round(Shipment.Volume.Value, 2);
            }

            decimal myGrossWeight = 0;
            if (myGrossWeightUnitCode == "K")
            {
                myGrossWeight = (Shipment.GrossWeightInKG == null) ? 0 : (decimal)Shipment.GrossWeightInKG.Value;
            }

            else
            {
                myGrossWeight = (Shipment.GrossWeight == null) ? 0 : (decimal)Shipment.GrossWeight.Value;
            }

            decimal myChargeableWeight = 0;
            if (myChargeableWeightUnitCode == "K")
            {
                myChargeableWeight = (Shipment.ChargeableWeightInKG == null) ? 0 : (decimal)Shipment.ChargeableWeightInKG.Value;
            }

            else
            {
                myChargeableWeight = (Shipment.ChargeableWeight == null) ? 0 : (decimal)Shipment.ChargeableWeight.Value;
            }

            this.Volume = MethodHelper.Normalize(myVolume);
            this.NumberOfPackages = myPieces;
            this.GrossWeight = MethodHelper.Normalize(myGrossWeight);
            this.ChargeableWeight = MethodHelper.Normalize(myChargeableWeight);
            this.VolumeUnitCode = myVolumeUnitCode;
            this.DimensionsUnitCode = myDimensionsUnitCode;
            this.GrossWeightUnitCode = myGrossWeightUnitCode;
            this.ChargeableWeightUnitCode = myChargeableWeightUnitCode;
        }
        private void BuildRoutingData()
        {
            this.MainCarriageFromPortId = MasterData.MainCarriageFromPortId;
            this.Transshipment1FromPortId = MasterData.Transshipment1FromPortId;
            this.Transshipment2FromPortId = MasterData.Transshipment2FromPortId;
            this.Transshipment3FromPortId = MasterData.Transshipment3FromPortId;
            this.MainCarriageToPortId = MasterData.MainCarriageToPortId;
            this.Transshipment1ToPortId = MasterData.Transshipment1ToPortId;
            this.Transshipment2ToPortId = MasterData.Transshipment2ToPortId;
            this.Transshipment3ToPortId = MasterData.Transshipment3ToPortId;
            
            #region MainCarriage
            if (!string.IsNullOrEmpty(MasterData.MainCarriageFromPortId) && !string.IsNullOrEmpty(MasterData.MainCarriageToPortId))
            {
                PortPM myFromPort = PortQuery.GetSinglePort(Tenant, MasterData.MainCarriageFromPortId, false);
                PortPM myToPort = PortQuery.GetSinglePort(Tenant, MasterData.MainCarriageToPortId, false);
                PortPM myFinalPort = PortQuery.GetSinglePort(Tenant, MasterData.MainCarriageFinalDestinationPortId, false);

                this.MainCarriageFromPortCode = myFromPort.Code.ToUpper();
                this.MainCarriageFromPortName = myFromPort.EnglishName.ToUpper();
                this.MainCarriageToPortCode = myToPort.Code.ToUpper();
                this.FinalDestinationPortCode = myFinalPort.Code.ToUpper();

                this.AirlinePrefix = FormatHelper.FormatInteger(3, MasterData.AirlinePrefix);

                if (!string.IsNullOrEmpty(MasterData.MainCarriageCarrierId))
                {
                    Card myCard = CardRepository.GetSingleCard(MasterData.MainCarriageCarrierId, Tenant, false);
                    if (myCard != null)
                    {
                        this.MainCarriageCarrierCode = myCard.Code.ToUpper();
                    }

                    if (!string.IsNullOrEmpty(MasterData.MainCarriageCarrierPrefix))
                    {
                        this.MainCarriageCarrierCode = MasterData.MainCarriageCarrierPrefix.Trim().ToUpper();
                    }
                }

                if (!string.IsNullOrEmpty(MasterData.MainCarriageCarrierNumber))
                {
                    this.MainCarriageCarrierNumber = FormatHelper.FormatString(MasterData.MainCarriageCarrierNumber, FormatHelper.PatternType.FlightNumber);
                }

                if (MasterData.MainCarriageETD != null)
                {
                    int day = 0;
                    Int32.TryParse(String.Format("{0:dd}", MasterData.MainCarriageETD.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), out day);
                    this.MainCarriageCarrierETDDay = FormatHelper.FormatInteger(2, day);
                    this.MainCarriageCarrierETD = MasterData.MainCarriageETD.Value;
                }
            }
            #endregion

            #region Transshipment1
            if (!string.IsNullOrEmpty(MasterData.Transshipment1FromPortId) && !string.IsNullOrEmpty(MasterData.Transshipment1ToPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(Tenant, MasterData.Transshipment1ToPortId, false);
                if (myPort != null)
                {
                    this.Transshipment1ToPortCode = myPort.Code.ToUpper();
                }

                if (!string.IsNullOrEmpty(MasterData.Transshipment1CarrierId))
                {
                    Card myCard = CardRepository.GetSingleCard(MasterData.Transshipment1CarrierId, Tenant, false);
                    if (myCard != null)
                    {
                        this.Transshipment1CarrierCode = myCard.Code.ToUpper();
                    }

                    if (!string.IsNullOrEmpty(MasterData.Transshipment1CarrierPrefix))
                    {
                        this.Transshipment1CarrierCode = MasterData.Transshipment1CarrierPrefix.Trim().ToUpper();
                    }
                }

                if (!string.IsNullOrEmpty(MasterData.Transshipment1CarrierNumber))
                {
                    this.Transshipment1CarrierNumber = FormatHelper.FormatString(MasterData.Transshipment1CarrierNumber, FormatHelper.PatternType.FlightNumber);
                }

                if (MasterData.Transshipment1ETD != null)
                {
                    int day = 0;
                    Int32.TryParse(String.Format("{0:dd}", MasterData.Transshipment1ETD.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), out day);
                    this.Transshipment1CarrierETDDay = FormatHelper.FormatInteger(2, day);
                    this.Transshipment1CarrierETD = MasterData.Transshipment1ETD.Value;
                }
            }
            #endregion

            #region Transshipment2
            if (!string.IsNullOrEmpty(MasterData.Transshipment2FromPortId) && !string.IsNullOrEmpty(MasterData.Transshipment2ToPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(Tenant, MasterData.Transshipment2ToPortId, false);
                if (myPort != null)
                {
                    this.Transshipment2ToPortCode = myPort.Code.ToUpper();
                }

                if (!string.IsNullOrEmpty(MasterData.Transshipment2CarrierId))
                {
                    Card myCard = CardRepository.GetSingleCard(MasterData.Transshipment2CarrierId, Tenant, false);
                    if (myCard != null)
                    {
                        this.Transshipment2CarrierCode = myCard.Code.ToUpper();
                    }

                    if (!string.IsNullOrEmpty(MasterData.Transshipment2CarrierPrefix))
                    {
                        this.Transshipment2CarrierCode = MasterData.Transshipment2CarrierPrefix.Trim().ToUpper();
                    }
                }

                if (!string.IsNullOrEmpty(MasterData.Transshipment2CarrierNumber))
                {
                    this.Transshipment2CarrierNumber = FormatHelper.FormatString(MasterData.Transshipment2CarrierNumber, FormatHelper.PatternType.FlightNumber);
                }

                if (MasterData.Transshipment2ETD != null)
                {
                    int day = 0;
                    Int32.TryParse(String.Format("{0:dd}", MasterData.Transshipment2ETD.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), out day);
                    this.Transshipment2CarrierETDDay = FormatHelper.FormatInteger(2, day);
                    this.Transshipment2CarrierETD = MasterData.Transshipment2ETD.Value;
                }
            }
            #endregion

            #region Transshipment3
            if (!string.IsNullOrEmpty(MasterData.Transshipment3FromPortId) && !string.IsNullOrEmpty(MasterData.Transshipment3ToPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(Tenant, MasterData.Transshipment3ToPortId, false);
                if (myPort != null)
                {
                    this.Transshipment3ToPortCode = myPort.Code.ToUpper();
                }

                if (!string.IsNullOrEmpty(MasterData.Transshipment3CarrierId))
                {
                    Card myCard = CardRepository.GetSingleCard(MasterData.Transshipment3CarrierId, Tenant, false);
                    if (myCard != null)
                    {
                        this.Transshipment3CarrierCode = myCard.Code.ToUpper();
                    }

                    if (!string.IsNullOrEmpty(MasterData.Transshipment3CarrierPrefix))
                    {
                        this.Transshipment3CarrierCode = MasterData.Transshipment3CarrierPrefix.Trim().ToUpper();
                    }
                }

                if (!string.IsNullOrEmpty(MasterData.Transshipment3CarrierNumber))
                {
                    this.Transshipment3CarrierNumber = FormatHelper.FormatString(MasterData.Transshipment3CarrierNumber, FormatHelper.PatternType.FlightNumber);
                }

                if (MasterData.Transshipment3ETD != null)
                {
                    int day = 0;
                    Int32.TryParse(String.Format("{0:dd}", MasterData.Transshipment3ETD.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), out day);
                    this.Transshipment3CarrierETDDay = FormatHelper.FormatInteger(2, day);
                    this.Transshipment3CarrierETD = MasterData.Transshipment3ETD.Value;
                }
            }
            #endregion
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

            #region Notify1
            if (!string.IsNullOrEmpty(Shipment.Notify1Id))
            {
                Card myCard = CardRepository.GetSingleCard(Shipment.Notify1Id, Tenant, false);
                if (myCard != null)
                {
                    string myField = FormatHelper.FormatString(myCard.EnglishName, FormatHelper.PatternType.Text);
                    if (!string.IsNullOrEmpty(myField))
                    {
                        this.Notify1Name = FormatHelper.FormatString(myField, FormatHelper.PatternType.Name);

                        if (myField.Length > 35)
                        {
                            this.Notify1Name2 = FormatHelper.FormatString(myField.Substring(34), FormatHelper.PatternType.Name);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Shipment.Notify1AddressId))
                {
                    Address myAddress = addressRepository.GetSingleAddress(Shipment.Notify1AddressId, Tenant);
                    if (myAddress != null)
                    {
                        this.Notify1Address = FormatHelper.FormatString(myAddress.Address1 + " " + myAddress.Address2, FormatHelper.PatternType.Address);
                        this.Notify1Address1 = FormatHelper.FormatString(myAddress.Address1, FormatHelper.PatternType.Address);
                        this.Notify1Address2 = FormatHelper.FormatString(myAddress.Address2, FormatHelper.PatternType.Address);
                        this.Notify1City = FormatHelper.FormatString(myAddress.City, FormatHelper.PatternType.City);
                        this.Notify1ZipCode = FormatHelper.FormatString(myAddress.ZipCode, FormatHelper.PatternType.ZipCode);
                        this.Notify1Phone = FormatHelper.FormatString(myAddress.PhoneNumber, FormatHelper.PatternType.Phone);
                        this.Notify1Fax = FormatHelper.FormatString(myAddress.FaxNumber, FormatHelper.PatternType.Phone);

                        Country myCountry = CountryRepository.GetSingleCountry(myAddress.CountryId, Tenant, false);
                        if (myCountry != null)
                        {
                            this.Notify1CountryCode = string.IsNullOrEmpty(myCountry.Code) ? "" : myCountry.Code.ToUpper();
                        }

                        if (!string.IsNullOrEmpty(myAddress.StateId))
                        {
                            State myState = stateRepository.GetSingleState(myAddress.StateId, Tenant);
                            if (myState != null)
                            {
                                this.Notify1StateCode = myState.Code.ToUpper();
                            }
                        }
                    }
                }
            }
            #endregion

            #region IssuingCarrierAgent
            if (!string.IsNullOrEmpty(Shipment.IssuingCarrierAgentId))
            {
                Card myCard = CardRepository.GetSingleCard(Shipment.IssuingCarrierAgentId, Tenant, false);
                if (myCard != null)
                {
                    string myField = FormatHelper.FormatString(myCard.EnglishName, FormatHelper.PatternType.Text);
                    if (!string.IsNullOrEmpty(myField))
                    {
                        this.IssuingCarrierAgentName = FormatHelper.FormatString(myField, FormatHelper.PatternType.Name);
                    }
                }

                if (!string.IsNullOrEmpty(Shipment.IssuingCarrierAddressId))
                {
                    Address myAddress = addressRepository.GetSingleAddress(Shipment.IssuingCarrierAddressId, Tenant);
                    if (myAddress != null)
                    {
                        this.IssuingCarrierAgentCity = FormatHelper.FormatString(myAddress.City, FormatHelper.PatternType.City);
                    }
                }

                if (!string.IsNullOrEmpty(Shipment.IssuingCarrierIATACode))
                {
                    this.IsIssuingCarrierIATACodeSet = true;
                    this.IssuingCarrierIATACode = FormatHelper.FormatInteger(7, Shipment.IssuingCarrierIATACode);
                }

                if (!string.IsNullOrEmpty(Shipment.CASSCode))
                {
                    this.IsIssuingCarrierCASSCodeSet = true;
                    this.IssuingCarrierCASSCode = FormatHelper.FormatInteger(4, Shipment.CASSCode);
                }
            }
            #endregion
        }
        private void BuildSSRTextList()
        {
            // SSR
            // SpecialServiceRequest
            // Shipment.AWBHandlingInformation

            // System DB Field Length: 250
            // Champ XML Field Length: [3] of 65
            // 3 * 65 = 195

            this.SSRTextList = new List<string>();
            List<string> mySSRTextList = new List<string>();

            if (!string.IsNullOrEmpty(Shipment.AWBHandlingInformation))
            {
                int itemLength = 65;
                string xmlField = "";
                string systemField = Shipment.AWBHandlingInformation;

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

            if (IsRACodeActivated)
            {
                //if (!string.IsNullOrEmpty(MasterData.AdditionalHandlingInfo))
                //{
                //    string myField = MasterData.AdditionalHandlingInfo;
                //    myField = FormatHelper.FormatString(myField, FormatHelper.PatternType.Text, 65);
                //    myField = myField.Trim();
                //    myField = myField.ToUpper();
                //    this.SSRTextList.Add(myField);
                //}

                if (SSRTextList.Count < 3)
                {
                    if (!string.IsNullOrEmpty(MasterData.AWBPrintingRANumber))
                    {
                        string myField = "RAR-RA" + MasterData.AWBPrintingRANumber;
                        myField = FormatHelper.FormatString(myField, FormatHelper.PatternType.Text, 65);
                        myField = myField.Trim();
                        myField = myField.ToUpper();
                        this.SSRTextList.Add(myField);
                    }
                }

                foreach (string item in mySSRTextList)
                {
                    if (SSRTextList.Count < 3)
                    {
                        this.SSRTextList.Add(item);
                    }
                }
            }

            else
            {
                foreach (string item in mySSRTextList)
                {
                    this.SSRTextList.Add(item);
                }
            }
        }
        private void BuildCommentsTextList()
        {
            this.CommentsTextList = new List<string>();

            if (!string.IsNullOrEmpty(Shipment.AWBComments))
            {
                int itemLength = 65;
                string xmlField = "";
                string systemField = Shipment.AWBComments;

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
                        this.CommentsTextList.Add(xmlField);
                    }
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
        private void BuildAccountingInfoList()
        {
            // System DB Field Length: 250
            // Champ XML Field Length: [6] of 34 "GEN"
            // 6 * 34 = 204

            bool isAccountingInfoSplitted = false;
            this.AccountingInfoList = new List<AccountingInfoItem>();
            this.AccountingInfoList17 = new List<AccountingInfoItem>();

            if (!string.IsNullOrEmpty(Shipment.AccountingInformationIdentifierCode1) && !string.IsNullOrEmpty(Shipment.AccountingInformation1))
            {
                isAccountingInfoSplitted = true;
                string xmlField = FormatHelper.FormatString(Shipment.AccountingInformation1.Trim(), FormatHelper.PatternType.Text);

                AccountingInfoList17.Add(new AccountingInfoItem()
                {
                    Code = Shipment.AccountingInformationIdentifierCode1,
                    Element = xmlField
                });
            }

            if (!string.IsNullOrEmpty(Shipment.AccountingInformationIdentifierCode2) && !string.IsNullOrEmpty(Shipment.AccountingInformation2))
            {
                isAccountingInfoSplitted = true;
                string xmlField = FormatHelper.FormatString(Shipment.AccountingInformation2.Trim(), FormatHelper.PatternType.Text);

                AccountingInfoList17.Add(new AccountingInfoItem()
                {
                    Code = Shipment.AccountingInformationIdentifierCode2,
                    Element = xmlField
                });
            }

            if (!string.IsNullOrEmpty(Shipment.AccountingInformationIdentifierCode3) && !string.IsNullOrEmpty(Shipment.AccountingInformation3))
            {
                isAccountingInfoSplitted = true;
                string xmlField = FormatHelper.FormatString(Shipment.AccountingInformation3.Trim(), FormatHelper.PatternType.Text);

                AccountingInfoList17.Add(new AccountingInfoItem()
                {
                    Code = Shipment.AccountingInformationIdentifierCode3,
                    Element = xmlField
                });
            }

            if (!string.IsNullOrEmpty(Shipment.AccountingInformationIdentifierCode4) && !string.IsNullOrEmpty(Shipment.AccountingInformation4))
            {
                isAccountingInfoSplitted = true;
                string xmlField = FormatHelper.FormatString(Shipment.AccountingInformation4.Trim(), FormatHelper.PatternType.Text);

                AccountingInfoList17.Add(new AccountingInfoItem()
                {
                    Code = Shipment.AccountingInformationIdentifierCode4,
                    Element = xmlField
                });
            }

            if (!string.IsNullOrEmpty(Shipment.AccountingInformationIdentifierCode5) && !string.IsNullOrEmpty(Shipment.AccountingInformation5))
            {
                isAccountingInfoSplitted = true;
                string xmlField = FormatHelper.FormatString(Shipment.AccountingInformation5.Trim(), FormatHelper.PatternType.Text);

                AccountingInfoList17.Add(new AccountingInfoItem()
                {
                    Code = Shipment.AccountingInformationIdentifierCode5,
                    Element = xmlField
                });
            }

            if (!string.IsNullOrEmpty(Shipment.AccountingInformationIdentifierCode6) && !string.IsNullOrEmpty(Shipment.AccountingInformation6))
            {
                isAccountingInfoSplitted = true;
                string xmlField = FormatHelper.FormatString(Shipment.AccountingInformation6.Trim(), FormatHelper.PatternType.Text);

                AccountingInfoList17.Add(new AccountingInfoItem()
                {
                    Code = Shipment.AccountingInformationIdentifierCode6,
                    Element = xmlField
                });
            }

            if (!string.IsNullOrEmpty(Shipment.AWBAccountingInformation))
            {
                int itemLength = 34;
                string xmlField = "";
                string systemField = Shipment.AWBAccountingInformation;

                if (systemField != null)
                {
                    systemField = Regex.Replace(systemField, @"(\r)(\1)+", "$1");
                    systemField = systemField.Replace("\r", " ");
                    systemField = FormatHelper.FormatString(systemField, FormatHelper.PatternType.Text);
                }

                for (int i = 1; i <= 6; i++)
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

                        this.AccountingInfoList.Add(new AccountingInfoItem()
                        {
                            Code = "GEN",
                            Element = xmlField
                        });

                        if (!isAccountingInfoSplitted)
                        {
                            this.AccountingInfoList17.Add(new AccountingInfoItem()
                            {
                                Code = "GEN",
                                Element = xmlField
                            });
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
        private void BuildRateDescriptionData()
        {
            this.IsMultipleCommodities = Shipment.IsMultipleCommodities;

            ShipmentCommodityQuery shipmentCommodityQuery = new ShipmentCommodityQuery(Tenant);
            ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(Tenant);

            this.ShipmentCommodities = shipmentCommodityQuery.GetCommoditiesByShipmentId(ShipmentId, Tenant).ToList();
            this.ShipmentPackages = shipmentPackageRepository.GetShipmentPackagesForShipmentTenant(ShipmentId, Tenant).ToList();

            if (IsMultipleCommodities)
            {
                this.NatureOfGoodsList = new List<string>();
            }

            else
            {
                ShipmentCommodityPM shipmentCommodity = ShipmentCommodities.FirstOrDefault();
                if (shipmentCommodity != null)
                {
                    this.AWBChargeRate = shipmentCommodity.ChargeRate == null ? 0 : (decimal)shipmentCommodity.ChargeRate.Value;
                    this.AWBChargeAmount = shipmentCommodity.ChargeAmount == null ? 0 : (decimal)shipmentCommodity.ChargeAmount.Value;
                    this.RateClassCode = shipmentCommodity.RateClassCode == null ? null : shipmentCommodity.RateClassCode.ToUpper();
                    this.AWBCommodityItemNumber = shipmentCommodity.CommodityNumber;
                    this.AsAgreedFreight = Shipment.AsAgreedFreight;
                    this.AsAgreedOtherCharges = Shipment.AsAgreedOtherCharges;

                    this.NatureOfGoods = shipmentCommodity.DescriptionOfGoods;
                    if (NatureOfGoods != null)
                    {
                        this.NatureOfGoods = Regex.Replace(NatureOfGoods, @"(\r)(\1)+", "$1");
                        this.NatureOfGoods = NatureOfGoods.Replace("\r", " ");
                        this.NatureOfGoods = FormatHelper.FormatString(NatureOfGoods, FormatHelper.PatternType.NatureAndQuantityOfGoods);
                    }
                    
                    string varDescriptionOfGoods = shipmentCommodity.DescriptionOfGoods;
                    if (varDescriptionOfGoods != null)
                    {
                        varDescriptionOfGoods = Regex.Replace(varDescriptionOfGoods, @"(\r)(\1)+", "$1");
                        varDescriptionOfGoods = varDescriptionOfGoods.Replace("\r", " ");
                    }

                    this.NatureOfGoodsList = new List<string>();
                    if (!string.IsNullOrEmpty(varDescriptionOfGoods))
                    {
                        int len = 20;
                        string textFormat = @"[^A-Z0-9\-\. ]*";

                        string input = Regex.Replace(varDescriptionOfGoods.Trim().ToUpper(), textFormat, string.Empty, RegexOptions.Compiled);
                        string taken = "";

                        while (input.Length > 0)
                        {
                            if (input.Length <= len)
                            {
                                taken = input;
                                input = "";
                            }

                            else
                            {
                                taken = input.Substring(0, len);
                                input = input.Remove(0, len);
                            }

                            this.NatureOfGoodsList.Add(taken);
                        }
                    }
                }
            }

        }
        private void BuildOtherListsData()
        {
            ShipmentPayableRepository shipmentPayableRepository = new ShipmentPayableRepository(Tenant);
            ShipmentReceivableRepository shipmentReceivableRepository = new ShipmentReceivableRepository(Tenant);
            ShipmentAWBPrintOnlyRepository shipmentAWBPrintOnlyRepository = new ShipmentAWBPrintOnlyRepository(Tenant);

            List<ShipmentAWBPrintOnly> list1 = shipmentAWBPrintOnlyRepository.GetShipmentAWBPrintOnliesByShipment(ShipmentId, Tenant);
            List<ShipmentPayable> list2 = shipmentPayableRepository.GetShipemntPayablesByShipmentId(ShipmentId, Tenant).ToList();
            List<ShipmentReceivable> list3 = shipmentReceivableRepository.GetShipmentReceivablesByShipmentId(ShipmentId, Tenant).ToList();

            this.AWBPrintOnlies = list1.Where(d => d.CurrencyId == Shipment.AWBCurrencyId).ToList();
            this.ShipmentPayables = list2.Where(d => d.ChargesType.ChargesGroupCode != "FRT" && d.CurrencyId == Shipment.AWBCurrencyId && d.AWBPrint).ToList();
            this.ShipmentReceivables = list3.Where(d => d.ChargesType.ChargesGroupCode != "FRT" && d.CurrencyId == Shipment.AWBCurrencyId && d.AWBPrint).ToList();
        }
        private void BuildChargeSummaryPrepaidData()
        {            
            List<ShipmentPayable> myPayable = this.ShipmentPayables.Where(d => d.PrepaidCollectId == "P").ToList();
            List<ShipmentReceivable> myReceivables = this.ShipmentReceivables.Where(d => d.PrepaidCollectId == "P").ToList();
            List<ShipmentAWBPrintOnly> myPrintOnlies = this.AWBPrintOnlies.Where(d => d.PrepaidCollectId == "P").ToList();

            double? myTaxes = myPayable.Where(d => d.DueTypeCode == "TX").Sum(s => s.ExpectedAmount) + myReceivables.Where(d => d.DueTypeCode == "TX").Sum(s => s.TotalAmount) + myPrintOnlies.Where(d => d.DueTypeCode == "TX").Sum(s => s.Amount);
            double? myValuation = myPayable.Where(d => d.DueTypeCode == "VL").Sum(s => s.ExpectedAmount) + myReceivables.Where(d => d.DueTypeCode == "VL").Sum(s => s.TotalAmount) + myPrintOnlies.Where(d => d.DueTypeCode == "VL").Sum(s => s.Amount);
            double? myDueAgent = myPayable.Where(d => d.DueTypeCode == "AG").Sum(s => s.ExpectedAmount) + myReceivables.Where(d => d.DueTypeCode == "AG").Sum(s => s.TotalAmount) + myPrintOnlies.Where(d => d.DueTypeCode == "AG").Sum(s => s.Amount);
            double? myDueCarrier = myPayable.Where(d => d.DueTypeCode == "CA").Sum(s => s.ExpectedAmount) + myReceivables.Where(d => d.DueTypeCode == "CA").Sum(s => s.TotalAmount) + myPrintOnlies.Where(d => d.DueTypeCode == "CA").Sum(s => s.Amount);
            double? myTotalWithoutWeight = myPayable.Sum(s => s.ExpectedAmount) + myReceivables.Sum(s => s.TotalAmount) + myPrintOnlies.Sum(s => s.Amount);

            this.PrepaidWeight = AWBFreightAmountPrepaid;
            this.PrepaidTaxes = myTaxes == null ? 0 : (decimal)myTaxes.Value;
            this.PrepaidValuation = myValuation == null ? 0 : (decimal)myValuation.Value;
            this.PrepaidDueAgent = myDueAgent == null ? 0 : (decimal)myDueAgent.Value;
            this.PrepaidDueCarrier = myDueCarrier == null ? 0 : (decimal)myDueCarrier.Value;
            this.PrepaidTotalWithoutWeight = myTotalWithoutWeight == null ? 0 : (decimal)myTotalWithoutWeight.Value;
            this.PrepaidTotal = PrepaidWeight + PrepaidTotalWithoutWeight;
        }
        private void BuildChargeSummaryCollectData()
        {
            List<ShipmentPayable> myPayable = this.ShipmentPayables.Where(d => d.PrepaidCollectId == "C").ToList();
            List<ShipmentReceivable> myReceivables = this.ShipmentReceivables.Where(d => d.PrepaidCollectId == "C").ToList();
            List<ShipmentAWBPrintOnly> myPrintOnlies = this.AWBPrintOnlies.Where(d => d.PrepaidCollectId == "C").ToList();

            double? myTaxes = myPayable.Where(d => d.DueTypeCode == "TX").Sum(s => s.ExpectedAmount) + myReceivables.Where(d => d.DueTypeCode == "TX").Sum(s => s.TotalAmount) + myPrintOnlies.Where(d => d.DueTypeCode == "TX").Sum(s => s.Amount);
            double? myValuation = myPayable.Where(d => d.DueTypeCode == "VL").Sum(s => s.ExpectedAmount) + myReceivables.Where(d => d.DueTypeCode == "VL").Sum(s => s.TotalAmount) + myPrintOnlies.Where(d => d.DueTypeCode == "VL").Sum(s => s.Amount);
            double? myDueAgent = myPayable.Where(d => d.DueTypeCode == "AG").Sum(s => s.ExpectedAmount) + myReceivables.Where(d => d.DueTypeCode == "AG").Sum(s => s.TotalAmount) + myPrintOnlies.Where(d => d.DueTypeCode == "AG").Sum(s => s.Amount);
            double? myDueCarrier = myPayable.Where(d => d.DueTypeCode == "CA").Sum(s => s.ExpectedAmount) + myReceivables.Where(d => d.DueTypeCode == "CA").Sum(s => s.TotalAmount) + myPrintOnlies.Where(d => d.DueTypeCode == "CA").Sum(s => s.Amount);
            double? myTotalWithoutWeight = myPayable.Sum(s => s.ExpectedAmount) + myReceivables.Sum(s => s.TotalAmount) + myPrintOnlies.Sum(s => s.Amount);
            
            this.CollectWeight = AWBFreightAmountCollect;
            this.CollectTaxes = myTaxes == null ? 0 : (decimal)myTaxes.Value;
            this.CollectValuation = myValuation == null ? 0 : (decimal)myValuation.Value;
            this.CollectDueAgent = myDueAgent == null ? 0 : (decimal)myDueAgent.Value;
            this.CollectDueCarrier = myDueCarrier == null ? 0 : (decimal)myDueCarrier.Value;
            this.CollectTotalWithoutWeight = myTotalWithoutWeight == null ? 0 : (decimal)myTotalWithoutWeight.Value;
            this.CollectTotal = CollectWeight + CollectTotalWithoutWeight;
        }
        private void BuildCarrierExecutionData()
        {
            this.AWBMasterDate = MasterData.MAWBOBLDate;

            if (this.AWBMasterDate != null)
            {
                int varAWBMasterDay = 0;
                int varAWBMasterYear = 0;
                string varAWBMasterMonth = "";
                Int32.TryParse(String.Format("{0:dd}", MasterData.MAWBOBLDate.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), out varAWBMasterDay);
                Int32.TryParse(String.Format("{0:yy}", MasterData.MAWBOBLDate.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), out varAWBMasterYear);
                varAWBMasterMonth = String.Format("{0:MMM}", MasterData.MAWBOBLDate.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

                this.AWBMasterDay = FormatHelper.FormatInteger(2, varAWBMasterDay);
                this.AWBMasterYear = FormatHelper.FormatInteger(2,varAWBMasterYear);
                this.AWBMasterMonth = varAWBMasterMonth.ToUpper();
            }

            if (!string.IsNullOrEmpty(Shipment.AWBPlace))
            {
                this.AWBPlace = FormatHelper.FormatString(Shipment.AWBPlace, FormatHelper.PatternType.Place);
            }

            if (!string.IsNullOrEmpty(Shipment.AWBSignature))
            {
                this.AWBSignature = FormatHelper.FormatString(Shipment.AWBSignature, FormatHelper.PatternType.Signature);
            }
        }
        private void BuildNominatedHandlingPartyData()
        {
            this.NominatedHandlingPartyId = Shipment.NominatedHandlingPartyId;

            if (!string.IsNullOrEmpty(Shipment.NominatedHandlingPartyId))
            {
                Card myCard = CardRepository.GetSingleCard(Shipment.NominatedHandlingPartyId, Tenant, true);
                if (myCard != null)
                {
                    this.NominatedHandlingPartyName = FormatHelper.FormatString(myCard.EnglishName, FormatHelper.PatternType.Name);

                    Address myAddress = addressRepository.GetSingleAddressByCardIdAndTypeId(Shipment.NominatedHandlingPartyId, "M", Tenant);
                    if (myAddress != null)
                    {
                        this.NominatedHandlingPartyCity = FormatHelper.FormatString(myAddress.City, FormatHelper.PatternType.City);
                    }

                    else
                    {
                        if (myCard.PartnerTypeId == "AL" || myCard.PartnerTypeId == "SL" || myCard.PartnerTypeId == "TR")
                        {
                            this.NominatedHandlingPartyCity = this.FinalDestinationPortCode;
                        }
                    }
                }
            }
        }
        private void BuildShipmentReferenceInfoData()
        {            
            this.ReferenceNumber = FormatHelper.FormatString(Shipment.ReferenceNumber, FormatHelper.PatternType.Text);
            this.SupplementaryShipmentInformation1 = FormatHelper.FormatString(Shipment.SupplementaryShipmentInformation1, FormatHelper.PatternType.Text);
            this.SupplementaryShipmentInformation2 = FormatHelper.FormatString(Shipment.SupplementaryShipmentInformation2, FormatHelper.PatternType.Text);            

            this.IsReferenceInfoSpecified = false;
            this.SupplementaryTextList = new List<string>();

            if (!string.IsNullOrEmpty(ReferenceNumber))
            {
                this.IsReferenceInfoSpecified = true;                
            }

            if (!string.IsNullOrEmpty(SupplementaryShipmentInformation1))
            {
                this.IsReferenceInfoSpecified = true;
                this.SupplementaryTextList.Add(SupplementaryShipmentInformation1);
            }

            if (!string.IsNullOrEmpty(SupplementaryShipmentInformation2))
            {
                this.IsReferenceInfoSpecified = true;
                this.SupplementaryTextList.Add(SupplementaryShipmentInformation2);
            }
        }
        private void BuildOtherParticipantsData()
        {
            this.OtherParticipantList = new List<OtherParticipantItem>();

            if (!string.IsNullOrEmpty(Shipment.OtherParticipantIdCode1) && !string.IsNullOrEmpty(Shipment.OtherParticipantInformationCode1) && !string.IsNullOrEmpty(Shipment.OtherParticipantInformationPortCode1) && !string.IsNullOrEmpty(Shipment.OtherParticipantInformationName1) && !string.IsNullOrEmpty(Shipment.OtherParticipantInformationReference1))
            {
                OtherParticipantList.Add(new OtherParticipantItem()
                {
                    Id = FormatHelper.FormatString(Shipment.OtherParticipantIdCode1, FormatHelper.PatternType.AlphaNumeric, 3),
                    Code = FormatHelper.FormatString(Shipment.OtherParticipantInformationCode1, FormatHelper.PatternType.AlphaNumeric, 17),
                    Port = FormatHelper.FormatString(Shipment.OtherParticipantInformationPortCode1, FormatHelper.PatternType.Alpha, 3),
                    Name = FormatHelper.FormatString(Shipment.OtherParticipantInformationName1, FormatHelper.PatternType.Text, 35),
                    Reference = FormatHelper.FormatString(Shipment.OtherParticipantInformationReference1, FormatHelper.PatternType.Text, 15),
                });
            }

            if (!string.IsNullOrEmpty(Shipment.OtherParticipantIdCode2) && !string.IsNullOrEmpty(Shipment.OtherParticipantInformationCode2) && !string.IsNullOrEmpty(Shipment.OtherParticipantInformationPortCode2) && !string.IsNullOrEmpty(Shipment.OtherParticipantInformationName2) && !string.IsNullOrEmpty(Shipment.OtherParticipantInformationReference2))
            {
                OtherParticipantList.Add(new OtherParticipantItem()
                {
                    Id = FormatHelper.FormatString(Shipment.OtherParticipantIdCode2, FormatHelper.PatternType.AlphaNumeric, 3),
                    Code = FormatHelper.FormatString(Shipment.OtherParticipantInformationCode2, FormatHelper.PatternType.AlphaNumeric, 17),
                    Port = FormatHelper.FormatString(Shipment.OtherParticipantInformationPortCode2, FormatHelper.PatternType.Alpha, 3),
                    Name = FormatHelper.FormatString(Shipment.OtherParticipantInformationName2, FormatHelper.PatternType.Text, 35),
                    Reference = FormatHelper.FormatString(Shipment.OtherParticipantInformationReference2, FormatHelper.PatternType.Text, 15),
                });
            }

            if (!string.IsNullOrEmpty(Shipment.OtherParticipantIdCode3) && !string.IsNullOrEmpty(Shipment.OtherParticipantInformationCode3) && !string.IsNullOrEmpty(Shipment.OtherParticipantInformationPortCode3) && !string.IsNullOrEmpty(Shipment.OtherParticipantInformationName3) && !string.IsNullOrEmpty(Shipment.OtherParticipantInformationReference3))
            {
                OtherParticipantList.Add(new OtherParticipantItem()
                {
                    Id = FormatHelper.FormatString(Shipment.OtherParticipantIdCode3, FormatHelper.PatternType.AlphaNumeric, 3),
                    Code = FormatHelper.FormatString(Shipment.OtherParticipantInformationCode3, FormatHelper.PatternType.AlphaNumeric, 17),
                    Port = FormatHelper.FormatString(Shipment.OtherParticipantInformationPortCode3, FormatHelper.PatternType.Alpha, 3),
                    Name = FormatHelper.FormatString(Shipment.OtherParticipantInformationName3, FormatHelper.PatternType.Text, 35),
                    Reference = FormatHelper.FormatString(Shipment.OtherParticipantInformationReference3, FormatHelper.PatternType.Text, 15),
                });
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

    public class AccountingInfoItem
    {
        public string Code { get; set; }
        public string Element { get; set; }
    }

    public class OtherParticipantItem
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Port { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }                       
    }

    public class ShipmentOCIItem
    {
        public string CountryCode { get; set; }
        public string InformationCode { get; set; }
        public string CustomsInformationCode { get; set; }
        public string SupplementaryCustomsInfo { get; set; }
    }
}
