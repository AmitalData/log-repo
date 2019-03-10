using System;
using System.Collections.Generic;
namespace WebFreight.Web.DataProviders
{
    public class AWBDataProvider
    {
        public string BranchSignature { get; set; }
        public string MainCarriageCarrierPrefix { get; set; }        
        public string MainCarriageCarrierAddress { get; set; }
        public string MAWBShort { get; set; }
        public string MAWBFull { get; set; }
        public string ShipperNameAddress { get; set; }
        public string MainCarriageCarrierName { get; set; }
        public string MainCarriageCarrierCode { get; set; }
        public string ConsigneeNameAddress { get; set; }
        public string TenantCompanyNameAddress { get; set; }
        public string AccountingInformation { get; set; }//custom
        public string CarrierTarrifRef { get; set; }//custom
        public string CompanyIATACode { get; set; }
        public string AWBAccount { get; set; }
        public string Place { get; set; }
        public string MainCarriageFromPortCode { get; set; }
        public string MainCarriageFromPortName { get; set; }
        public string MainCarriageToPortCode { get; set; }
        public string MainCarriageDepartedCarrierName { get; set; }
        public string Transshipment1ToPortCode { get; set; }
        public string Transshipment1CarrierCode { get; set; }
        public string Transshipment2ToPortCode { get; set; }
        public string Transshipment2CarrierCode { get; set; }
        public string LeadingCurrency { get; set; }
        public string ChargesCode { get; set; }//custom
        public string FreightPrepaid { get; set; }//custom
        public string FreightCollect { get; set; }//custom
        public string OtherPrepaid { get; set; }//custom
        public string OtherCollect { get; set; }//custom
        public string DeclaredValueForCarriage { get; set; }//custom N V D
        public string DeclaredValueForCustoms { get; set; }//custom N C V
        public string LastToInMainCarriage { get; set; }
        public string MainCarriageFlightNumberAndDate { get; set; }
        public string Transshipment1FlightNumberAndDate { get; set; }
        public string Transshipment2FlightNumberAndDate { get; set; }
        public string InsurrenceValue { get; set; }//custom default X X X        
        public string SCI { get; set; }
        public string TotalQuantity { get; set; }
        public string GrossWeight { get; set; }
        public string GrossWeightInKG { get; set; }
        public string WeightUnit { get; set; }
        public string RateTypeCode { get; set; }//custom
        public string ChargeableWeight { get; set; }
        public string MAWBRate { get; set; }
        public string TotalFreight { get; set; }//39*40
        public string DescriptionOfGoods { get; set; }
        public string Signature { get; set; }
        public string ShipmentNumber { get; set; }
        public string MAWBOBLDate { get; set; }
        public string AWBFreightPrepaid { get; set; }
        public string AWBFreightCollect { get; set; }
        public string OtherCharges { get; set; }
        public string OtherCharges_ChargeEnglishName { get; set; }
        public string TotalOtherChargesDueAgentPrepaid { get; set; }
        public string TotalOtherChargesDueAgentCollect { get; set; }
        public string TotalOtherChargesDueCarrierPrepaid { get; set; }
        public string TotalOtherChargesDueCarrierCollect { get; set; }
        public string OtherCharges_AsAgreed { get; set; }
        public string TotalCollect { get; set; }
        public string TotalPrepaid { get; set; }
        public string ValuationTotalPrepaid { get; set; }
        public string ValuationTotalCollect { get; set; }
        public string TaxTotalPrepaid { get; set; }
        public string TaxTotalCollect { get; set; }
        public string CopyName { get; set; }
        public bool HasBackPaper { get; set; }
        public string HAWB { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string AWBComments { get; set; }
        public string AWBCommodityItemNumber { get; set; }
        public string MainCarriageFullCarrierNumber { get; set; }
        public string AccountNumber { get; set; }
        public string PlaceOfDelivery { get; set; }
        public string Notes { get; set; }
        public string ShipmentField1 { get; set; }
        public string ShipmentField2 { get; set; }
        public string ShipmentField3 { get; set; }
        public string ShipmentField4 { get; set; }
        public string ShipmentField5 { get; set; }
        public string ShipmentField6 { get; set; }
        public string ShipmentField7 { get; set; }
        public string ShipmentField8 { get; set; }
        public string ShipmentField9 { get; set; }
        public string ShipmentField10 { get; set; }
        public string ShipmentField11 { get; set; }
        public string ShipmentField12 { get; set; }
        public string ShipmentField13 { get; set; }
        public string ShipmentField14 { get; set; }
        public string ShipmentField15 { get; set; }
        public string ShipmentField16 { get; set; }
        public string ShipmentField17 { get; set; }
        public string ShipmentField18 { get; set; }
        public string ShipmentField19 { get; set; }
        public string ShipmentField20 { get; set; }
        public string ShipmentField21 { get; set; }
        public string ShipmentField22 { get; set; }
        public string ShipmentField23 { get; set; }
        public string ShipmentField24 { get; set; }
        public string ShipmentField25 { get; set; }
        public string ShipmentField26 { get; set; }
        public string ShipmentField27 { get; set; }
        public string ShipmentField28 { get; set; }
        public string ShipmentField29 { get; set; }
        public string ShipmentField30 { get; set; }
        public string ShipmentField31 { get; set; }
        public string ShipmentField32 { get; set; }
        public string ShipmentField33 { get; set; }
        public string ShipmentField34 { get; set; }
        public string ShipmentField35 { get; set; }
        public string ShipmentField36 { get; set; }
        public string ShipmentField37 { get; set; }
        public string ShipmentField38 { get; set; }
        public string ShipmentField39 { get; set; }
        public string ShipmentField40 { get; set; }

        public string AgentReference1 { get; set; }
        public string AgentReference2 { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public DateTime? Transshipment1ETD { get; set; }
        public DateTime? Transshipment2ETD { get; set; }
        public DateTime? Transshipment3ETD { get; set; }

        public string JustDescriptionofGoods { get; set; }
        public string Dimension { get; set; }
        public double? VolumetricWeight { get; set; }
        public double? Volume { get; set; }
        public DateTime? FlightDate { get; set; }
        public string FlightNumber { get; set; }
        public string ShipperName { get; set; }
        public string ShipperAddress1 { get; set; }
        public string ShipperAddress2 { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperCountry { get; set; }
        public string ShipperTel { get; set; }
        public string ShipperFax { get; set; }
        public string ShipperZipCode { get; set; }       
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress1 { get; set; }
        public string ConsigneeAddress2 { get; set; }
        public string ConsigneeCity { get; set; }
        public string ConsigneeCountry { get; set; }
        public string ConsigneeTel { get; set; }
        public string ConsigneeFax { get; set; }
        public string ConsigneeZipCode { get; set; }        

        public List<CommodityLine> CommoditiesLinesList { get; set; }

        public string HandlingInformation { get; set; }
        public string IssuingShipperSignature { get; set; }
        public string IssuingCarrierSignature { get; set; }
        public string IssuingShipperSecurityCode { get; set; }
        public string IssuingCarrierSecurityCode { get; set; }
        public string RA { get; set; }

        public string ReferenceNumber { get; set; }
        public string SupplementaryInformation1 { get; set; }
        public string SupplementaryInformation2 { get; set; }

        public string NotifyPartyName { get; set; }
        public string NotifyPartyLocalName { get; set; }
        public string NotifyPartyAddress1 { get; set; }
        public string NotifyPartyAddress2 { get; set; }
        public string NotifyPartyTel { get; set; }
        public string NotifyPartyFax { get; set; }
        public string NotifyPartyCity { get; set; }
        public string NotifyPartyCountry { get; set; }
        public string NotifyPartyZipCode { get; set; }

        public string InterlineCarrierName { get; set; }
        public string InterlineCarrierAddress { get; set; }
        public string IncotermCode { get; set; }

        public string FMCNumber { get; set; }
        public string ENSNumber { get; set; }
        public DateTime? ENSDate { get; set; }

        public string ShipperVATNo { get; set; }
        public string ConsigneeVATNo { get; set; }
        public string ITNumber { get; set; }

        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }
        public string VolumeInCBM { get; set; }
        public string VolumeUnitCode { get; set; }
        public bool ChargeableWeightEdited { get; set; }

        public string ChargeDetails { get; set; }

        public string ActualShipperNameAddress { get; set; }
        public string ActualConsigneeNameAddress { get; set; }

        public string Branch { get; set; }

        public string ShipperAddress_WithName { get; set; }
        public string ShipperNotExporterAddress_WithName { get; set; }        
        public string ConsigneeAddress_WithName { get; set; }
        public string NotifyAddress_WithName { get; set; }
        public string NotifyAddress2_WithName { get; set; }
        public string HandlingInformationOnly { get; set; }

        public string ShipperATTN { get; set; }
        public string ConsigneeATTN { get; set; }
        public string ShipperNotExporterATTN { get; set; }
        public string ConsigneeNotImporterATTN { get; set; }
        public string Notify1ATTN { get; set; }
        public string Notify2ATTN { get; set; }
        public string AgentATTN { get; set; }

        public string ShipperContactDetails { get; set; }
        public string ConsigneeContactDetails { get; set; }
        public string Notify1ContactDetails { get; set; }
        public string Notify2ContactDetails { get; set; }
        public string ShipperNotExporterContactDetails { get; set; }

        public string ShipperPrimaryContactName { get; set; }
        public string ShipperPrimaryContactPhone { get; set; }
        public string ConsigneePrimaryContactName { get; set; }
        public string ConsigneePrimaryContactPhone { get; set; }

        public string ConsolidatorName { get; set; }
    }

    public class CommodityLine
    {
        public string RateClassCode { get; set; }
        public string CommodityNumber { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string TotalQuantity { get; set; }
        public string GrossWeight { get; set; }
        public string GrossWeightInKG { get; set; }
        public string ChargeableWeight { get; set; }
        public string MAWBRate { get; set; }
        public string TotalFreight { get; set; }
    }
}