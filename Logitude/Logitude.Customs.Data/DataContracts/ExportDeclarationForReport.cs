using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.DataContracts
{
    public class ExportDeclarationForReport
    {
        public string DeclarationId { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public DateTime? TaxationDateTime { get; set; }//תאריך חישוב מיסים
        public string ExportFile { get; set; }// תיק תפעולי
        public string TransportModeName { get; set; }
        public string CustomFileNo { get; set; }
        public string DeclarationNumber { get; set; }
        public string DeclarationTypeName { get; set; }
        public string ProcedureCurrentName { get; set; }
        public string ExporterImporterCode { get; set; }
        public string RecipientName { get; set; }// שם המקבל טבלת Customs.DeclarationExportRecipient
        public string DestinationCountryName { get; set; }//ארץ יעד
        public string DeclarationStatusTypeName { get; set; }

        public string FinalCargoTypeName { get; set; }
        public string FinalManifestNumber { get; set; }
        public string FinalSecondCargoId { get; set; }
        public string FinalThirdCargoId { get; set; }
        public List<Invoice> Invoices { get; set; }//חשבונות
        public List<Consignments> Consignments { get; set; }//נתוני סגירה


    }

    public class Invoice
    {
        public string DeclarationId { get; set; }
        public int InvoiceCounterKey { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public string IncotermCode { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }



    }

    public class InvoiceItem
    {
        public string DeclarationId { get; set; }
        public int CounterKey { get; set; }
        public int LineNumber { get; set; }
        public string ItemCode { get; set; }
        public string ClassificationCode { get; set; }
        public int? PackageQuantity { get; set; }
        public string InvoiceQuantityType { get; set; }
        public decimal? ItemPrice { get; set; }
        public string OriginCountryName { get; set; }
        public string TransactionNatureName { get; set; }//אופי עסקה

    }

    public class Consignments
    {
        public string DeclarationId { get; set; }
        public int ConsignmentNumber { get; set; }
        public string ConsignmentType { get; set; }
        public string CargoTypeName { get; set; }
        public string ManifestNumber { get; set; }
        public string SecondCargoID { get; set; }
        public string ThirdCargoID { get; set; }
        public string CargoDescription { get; set; }
        public string FinalDestinationPortName { get; set; }
        public int PackageQuantity { get; set; }//כמות מטבלת Customs.ConsignmentPackage
        public decimal? GrossMassMeasure { get; set; }//משקל מטבלת Customs.ConsignmentPackage

    }
}