using CWXSD;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class ExportDeclarationDataProvider : BaseDataProvider
    {
		public DateTime? CreateDateFrom { get; set; }
		public DateTime? CreateDateTo { get; set; }

		public string TransportModeId { get; set; }
		public string DeclarationStatusTypeCode { get; set; }
		public string DeclarationStatusTypeName { get; set; }
		public string DeclarationTypeCode { get; set; }
        public string DeclarationTypeName { get; set; }
		public string ReferentUserId { get; set; }
        public string ReferentUserName { get; set; }
		public string DestinationCountryCode { get; set; }
        public string DestinationCountryName { get; set; }
		public string Customer { get; set; }
        public string CustomerName { get; set; }
		public bool IsShowInvoices { get; set; }
		public bool IsShowConsignments { get; set; }
		public List<ExportDeclaration> ExportDeclaration { get; set; }//הצהרות


    }
    public class ExportDeclaration
    {
        public string DeclarationId { get; set; }
        public DateTime? TaxationDateTime { get; set; }//תאריך חישוב מיסים
        public string ExportFile { get; set; }// תיק תפעולי
        public string TransportModeName { get; set; }
        public string CustomFileNo { get; set; }
        public string DeclarationNumber { get; set; }
        public string DeclarationTypeName { get; set; }
        public string ProcedureCurrentName { get; set; }
        public string ExporterImporterCode { get; set; }
		public string ExporterImporterName { get; set; }
		public string RecipientName { get; set; }// שם המקבל טבלת Customs.DeclarationExportRecipient
        public string DestinationCountryName { get; set; }//ארץ יעד
        public string DeclarationStatusTypeName { get; set; }
        #region נתונים מסגירת הצהרה
        public string FinalCargoTypeName { get; set; }
        public string FinalManifestNumber { get; set; }
        public string FinalSecondCargoId { get; set; }
        public string FinalThirdCargoId { get; set; }
        #endregion
        public List<SupplierInvoices> SupplierInvoices { get; set; }//חשבונות
        public List<Consignment> Consignment { get; set; }//נתוני סגירה

    }


    public class SupplierInvoices
    {
        public string InvoiceNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public string IncotermCode { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public int InvoiceCounterKey { get; set; }
		public string InvoiceCurrencyTypeName { get; set; }
		public List<InvoiceItems> InvoiceItems { get; set; }



    }

    public class InvoiceItems
    {
        public string ItemCode { get; set; }
        public string ClassificationCode { get; set; }
        public int? PackageQuantity { get; set; }
        public string InvoiceQuantityType { get; set; }
        public decimal? ItemPrice { get; set; }
        public string OriginCountryName { get; set; }
        public string TransactionNatureName { get; set; }//אופי עסקה
        public int LineNumber { get; set; }//אופי עסקה
        
    }

    
    public class Consignment
    {
        public int? ConsignmentNumber { get; set; }
        public string ConsignmentType { get; set; }
        public string CargoTypeName { get; set; }
        public string ManifestNumber { get; set; }
        public string SecondCargoID { get; set; }
        public string ThirdCargoID { get; set; }
        public string CargoDescription { get; set; }
        public string FinalDestinationPortName { get; set; }
        public int? PackageQuantity { get; set; }//כמות מטבלת Customs.ConsignmentPackage
        public decimal? GrossMassMeasure { get; set; }//משקל מטבלת Customs.ConsignmentPackage

    }
}