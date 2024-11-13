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


        // supplier invoice
        public string SupplierInvoiceNumber { get; set; }
        public DateTime? SupplierInvoiceIssueDate { get; set; }
        public string SupplierInvoiceIncotermCode { get; set; }
        public decimal? SupplierInvoiceAmount { get; set; }
        public int SupplierInvoiceCounterKey { get; set; }
        public string SupplierInvoiceCurrencyTypeName { get; set; }

        // supplier invoice item
        public string SupplierInvoiceItemCode { get; set; }
        public string SupplierInvoiceItemClassificationCode { get; set; }
        public int? SupplierInvoiceItemPackageQuantity { get; set; }
        public string SupplierInvoiceItemInvoiceQuantityType { get; set; }
        public decimal? SupplierInvoiceItemPrice { get; set; }
        public string SupplierInvoiceItemOriginCountryName { get; set; }
        public string SupplierInvoiceItemTransactionNatureName { get; set; }//אופי עסקה
        public int SupplierInvoiceItemLineNumber { get; set; }//אופי עסקה

        // consignment

        public int? ConsignmentNumber { get; set; }
        public string ConsignmentType { get; set; }
        public string ConsignmentCargoTypeName { get; set; }
        public string ConsignmentManifestNumber { get; set; }
        public string ConsignmentSecondCargoID { get; set; }
        public string ConsignmentThirdCargoID { get; set; }
        public string ConsignmentCargoDescription { get; set; }
        public string ConsignmentFinalDestinationPortName { get; set; }
        public int? ConsignmentPackageQuantity { get; set; }//כמות מטבלת Customs.ConsignmentPackage
        public decimal? ConsignmentGrossMassMeasure { get; set; }//משקל מטבלת Customs.ConsignmentPackage
    }
}