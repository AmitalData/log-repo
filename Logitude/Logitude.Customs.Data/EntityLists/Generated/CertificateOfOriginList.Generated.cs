using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Customs.Data.EntityLists
{
   [DataContract]
   public partial class CertificateOfOriginList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string Counter  { get; set; }
       [DataMember]
       public string CooTypeCode  { get; set; }
       [DataMember]
       public string RequestReasonCode  { get; set; }
       [DataMember]
       public string COONumber  { get; set; }
       [DataMember]
       public string COONumberToCancel  { get; set; }
       [DataMember]
       public string ReplacementReason  { get; set; }
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public string ExporterVat  { get; set; }
       [DataMember]
       public string ExporterName  { get; set; }
       [DataMember]
       public string ExporterAddress  { get; set; }
       [DataMember]
       public string ExporterCountry  { get; set; }
       [DataMember]
       public string TradeAgreementCountry1  { get; set; }
       [DataMember]
       public string TradeAgreementCountry2  { get; set; }
       [DataMember]
       public string TradeAgreementGroupOfCountries  { get; set; }
       [DataMember]
       public string ConsigneeName  { get; set; }
       [DataMember]
       public string ConsigneeAddress  { get; set; }
       [DataMember]
       public string ConsigneeCountry  { get; set; }
       [DataMember]
       public string ConsigneeRemarks  { get; set; }
       [DataMember]
       public bool IsConsigneeForPrint  { get; set; }
       [DataMember]
       public string OriginCountry  { get; set; }
       [DataMember]
       public string OriginGroupOfCountry  { get; set; }
       [DataMember]
       public string DestinationCountry  { get; set; }
       [DataMember]
       public string DestinationGroupOfCountries  { get; set; }
       [DataMember]
       public string Transport  { get; set; }
       [DataMember]
       public string PortOfShipment  { get; set; }
       [DataMember]
       public bool IsCumulation  { get; set; }
       [DataMember]
       public string CumulationCountry  { get; set; }
       [DataMember]
       public string CumulationGroupOfCountries  { get; set; }
       [DataMember]
       public string PlaceOfManufacture  { get; set; }
       [DataMember]
       public string ZipCodeOfManufacture  { get; set; }
       [DataMember]
       public string Observations  { get; set; }
       [DataMember]
       public bool IsExportDecForPrint  { get; set; }
       [DataMember]
       public bool IsUnitedInvoices  { get; set; }
       [DataMember]
       public string CustomsHouse  { get; set; }
       [DataMember]
       public string IssuingCountry  { get; set; }
       [DataMember]
       public string CityOfDeclaration  { get; set; }
       [DataMember]
       public string CountryOfDeclaration  { get; set; }
       [DataMember]
       public DateTime? DateOfDeclaration  { get; set; }
       [DataMember]
       public bool IsDeclaredByManufacture  { get; set; }
       [DataMember]
       public bool IsDeclaredByExporter  { get; set; }
       [DataMember]
       public bool IsAttachedList  { get; set; }
       [DataMember]
       public bool InsufficentWorkingInd  { get; set; }
       [DataMember]
       public string InsufficentWorkingText  { get; set; }
       [DataMember]
       public DateTime? NonExportDate  { get; set; }
       [DataMember]
       public string NonExportCountry  { get; set; }
       [DataMember]
       public string NonImportBillOfLadingNum  { get; set; }
       [DataMember]
       public string NonExportPort  { get; set; }
       [DataMember]
       public DateTime? NonImportDate  { get; set; }
       [DataMember]
       public string NonExportBillOfLadingNum  { get; set; }
       [DataMember]
       public string NonTransirCountry  { get; set; }
       [DataMember]
       public string NonPortOfEntrance  { get; set; }
       [DataMember]
       public DateTime? NonExpectedExitDate  { get; set; }
       [DataMember]
       public string NonExitPort  { get; set; }
       [DataMember]
       public string NonGoodsDescription  { get; set; }
       [DataMember]
       public string NonDeclaringCompany  { get; set; }
       [DataMember]
       public string NonDeclaringPerson  { get; set; }
       [DataMember]
       public string NonDeclaringPosition  { get; set; }
       [DataMember]
       public string NonManifestNum  { get; set; }
       [DataMember]
       public string ErrXml  { get; set; }
       [DataMember]
       public string CooStatusCode  { get; set; }
       [DataMember]
       public string FeedbackRemark  { get; set; }
       [DataMember]
       public string RejectCancelReason  { get; set; }
       [DataMember]
       public DateTime? IssueDateIfReleased  { get; set; }
       [DataMember]
       public string QueryUrl  { get; set; }
       [DataMember]
       public string CooPdf  { get; set; }
       [DataMember]
       public string CoodPdf1  { get; set; }
       [DataMember]
       public string OpenByUser  { get; set; }
       [DataMember]
       public bool IsSubmitted  { get; set; }
       [DataMember]
       public string OpenByUserName  { get; set; }
   }

}
	 