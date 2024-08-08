using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class CertificateOfOrigin
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("Counter")]
	    public string Counter { get; set; }
        [ForeignKey("CertificateOfOriginTypeCodeEnum")]
        [Column("CooTypeCode")]
	    public string CooTypeCode { get; set; }
	      
        public virtual CertificateOfOriginTypeCodeEnum CertificateOfOriginTypeCodeEnum { get; set; }
        [ForeignKey("RequestReasonCodeEnum")]
        [Column("RequestReasonCode")]
	    public string RequestReasonCode { get; set; }
	      
        public virtual RequestReasonCodeEnum RequestReasonCodeEnum { get; set; }
        [Column("COONumber")]
	    public string COONumber { get; set; }
        [Column("COONumberToCancel")]
	    public string COONumberToCancel { get; set; }
        [Column("ReplacementReason")]
	    public string ReplacementReason { get; set; }
        [ForeignKey("Declaration")]
        [Column("DeclarationId")]
	    public string DeclarationId { get; set; }
	      
        public virtual Declaration Declaration { get; set; }
        [Column("ExporterVat")]
	    public string ExporterVat { get; set; }
        [Column("ExporterName")]
	    public string ExporterName { get; set; }
        [Column("ExporterAddress")]
	    public string ExporterAddress { get; set; }
        [ForeignKey("Country1")]
        [Column("ExporterCountry")]
	    public string ExporterCountry { get; set; }
	      
        public virtual CustomsCountry Country1 { get; set; }
        [ForeignKey("Country2")]
        [Column("TradeAgreementCountry1")]
	    public string TradeAgreementCountry1 { get; set; }
	      
        public virtual CustomsCountry Country2 { get; set; }
        [ForeignKey("Country3")]
        [Column("TradeAgreementCountry2")]
	    public string TradeAgreementCountry2 { get; set; }
	      
        public virtual CustomsCountry Country3 { get; set; }
        [ForeignKey("CountryGroup1")]
        [Column("TradeAgreementGroupOfCountries")]
	    public string TradeAgreementGroupOfCountries { get; set; }
	      
        public virtual CountryGroup CountryGroup1 { get; set; }
        [Column("ConsigneeName")]
	    public string ConsigneeName { get; set; }
        [Column("ConsigneeAddress")]
	    public string ConsigneeAddress { get; set; }
        [ForeignKey("Country4")]
        [Column("ConsigneeCountry")]
	    public string ConsigneeCountry { get; set; }
	      
        public virtual CustomsCountry Country4 { get; set; }
        [Column("ConsigneeRemarks")]
	    public string ConsigneeRemarks { get; set; }
        [Column("IsConsigneeForPrint")]
	    public bool IsConsigneeForPrint { get; set; }
        [ForeignKey("Country5")]
        [Column("OriginCountry")]
	    public string OriginCountry { get; set; }
	      
        public virtual CustomsCountry Country5 { get; set; }
        [ForeignKey("CountryGroup2")]
        [Column("OriginGroupOfCountry")]
	    public string OriginGroupOfCountry { get; set; }
	      
        public virtual CountryGroup CountryGroup2 { get; set; }
        [ForeignKey("Country6")]
        [Column("DestinationCountry")]
	    public string DestinationCountry { get; set; }
	      
        public virtual CustomsCountry Country6 { get; set; }
        [ForeignKey("CountryGroup3")]
        [Column("DestinationGroupOfCountries")]
	    public string DestinationGroupOfCountries { get; set; }
	      
        public virtual CountryGroup CountryGroup3 { get; set; }
        [Column("Transport")]
	    public string Transport { get; set; }
        [ForeignKey("SiteLookup")]
        [Column("PortOfShipment")]
	    public string PortOfShipment { get; set; }
	      
        public virtual SiteLookup SiteLookup { get; set; }
        [Column("IsCumulation")]
	    public bool IsCumulation { get; set; }
        [ForeignKey("Country7")]
        [Column("CumulationCountry")]
	    public string CumulationCountry { get; set; }
	      
        public virtual CustomsCountry Country7 { get; set; }
        [ForeignKey("CountryGroup4")]
        [Column("CumulationGroupOfCountries")]
	    public string CumulationGroupOfCountries { get; set; }
	      
        public virtual CountryGroup CountryGroup4 { get; set; }
        [ForeignKey("City1")]
        [Column("PlaceOfManufacture")]
	    public string PlaceOfManufacture { get; set; }
	      
        public virtual City City1 { get; set; }
        [Column("ZipCodeOfManufacture")]
	    public string ZipCodeOfManufacture { get; set; }
        [Column("Observations")]
	    public string Observations { get; set; }
        [Column("IsExportDecForPrint")]
	    public bool IsExportDecForPrint { get; set; }
        [Column("IsUnitedInvoices")]
	    public bool IsUnitedInvoices { get; set; }
        [ForeignKey("CustomsHouseType")]
        [Column("CustomsHouse")]
	    public string CustomsHouse { get; set; }
	      
        public virtual CustomsHouseType CustomsHouseType { get; set; }
        [ForeignKey("Country8")]
        [Column("IssuingCountry")]
	    public string IssuingCountry { get; set; }
	      
        public virtual CustomsCountry Country8 { get; set; }
        [ForeignKey("City2")]
        [Column("CityOfDeclaration")]
	    public string CityOfDeclaration { get; set; }
	      
        public virtual City City2 { get; set; }
        [ForeignKey("Country9")]
        [Column("CountryOfDeclaration")]
	    public string CountryOfDeclaration { get; set; }
	      
        public virtual CustomsCountry Country9 { get; set; }
        [Column("DateOfDeclaration")]
	    public DateTime? DateOfDeclaration { get; set; }
        [Column("IsDeclaredByManufacture")]
	    public bool IsDeclaredByManufacture { get; set; }
        [Column("IsDeclaredByExporter")]
	    public bool IsDeclaredByExporter { get; set; }
        [Column("IsAttachedList")]
	    public bool IsAttachedList { get; set; }
        [Column("InsufficentWorkingInd")]
	    public bool InsufficentWorkingInd { get; set; }
        [Column("InsufficentWorkingText")]
	    public string InsufficentWorkingText { get; set; }
        [Column("NonExportDate")]
	    public DateTime? NonExportDate { get; set; }
        [ForeignKey("Country10")]
        [Column("NonExportCountry")]
	    public string NonExportCountry { get; set; }
	      
        public virtual CustomsCountry Country10 { get; set; }
        [Column("NonImportBillOfLadingNum")]
	    public string NonImportBillOfLadingNum { get; set; }
        [ForeignKey("InternationalSite1")]
        [Column("NonExportPort")]
	    public string NonExportPort { get; set; }
	      
        public virtual InternationalSite InternationalSite1 { get; set; }
        [Column("NonImportDate")]
	    public DateTime? NonImportDate { get; set; }
        [Column("NonExportBillOfLadingNum")]
	    public string NonExportBillOfLadingNum { get; set; }
        [ForeignKey("Country11")]
        [Column("NonTransirCountry")]
	    public string NonTransirCountry { get; set; }
	      
        public virtual CustomsCountry Country11 { get; set; }
        [ForeignKey("InternationalSite2")]
        [Column("NonPortOfEntrance")]
	    public string NonPortOfEntrance { get; set; }
	      
        public virtual InternationalSite InternationalSite2 { get; set; }
        [Column("NonExpectedExitDate")]
	    public DateTime? NonExpectedExitDate { get; set; }
        [ForeignKey("InternationalSite3")]
        [Column("NonExitPort")]
	    public string NonExitPort { get; set; }
	      
        public virtual InternationalSite InternationalSite3 { get; set; }
        [Column("NonGoodsDescription")]
	    public string NonGoodsDescription { get; set; }
        [Column("NonDeclaringCompany")]
	    public string NonDeclaringCompany { get; set; }
        [Column("NonDeclaringPerson")]
	    public string NonDeclaringPerson { get; set; }
        [Column("NonDeclaringPosition")]
	    public string NonDeclaringPosition { get; set; }
        [Column("NonManifestNum")]
	    public string NonManifestNum { get; set; }
        [Column("ErrXml")]
	    public string ErrXml { get; set; }
        [ForeignKey("CertificateOfOriginStatusCodeEnum")]
        [Column("CooStatusCode")]
	    public string CooStatusCode { get; set; }
	      
        public virtual CertificateOfOriginStatusCodeEnum CertificateOfOriginStatusCodeEnum { get; set; }
        [Column("FeedbackRemark")]
	    public string FeedbackRemark { get; set; }
        [Column("RejectCancelReason")]
	    public string RejectCancelReason { get; set; }
        [Column("IssueDateIfReleased")]
	    public DateTime? IssueDateIfReleased { get; set; }
        [Column("QueryUrl")]
	    public string QueryUrl { get; set; }
        [Column("CooPdf")]
	    public string CooPdf { get; set; }
        [Column("CoodPdf1")]
	    public string CoodPdf1 { get; set; }
        [ForeignKey("CreateByUser")]
        [Column("OpenByUser")]
	    public string OpenByUser { get; set; }
	      
        public virtual User CreateByUser { get; set; }
        [Column("IsSubmitted")]
	    public bool IsSubmitted { get; set; }
        [Column("UpdateDeclaration")]
	    public string UpdateDeclaration { get; set; }
        [Column("IsChange")]
	    public bool IsChange { get; set; }
        [Column("OpenDate")]
	    public DateTime? OpenDate { get; set; }
    }
}
	 