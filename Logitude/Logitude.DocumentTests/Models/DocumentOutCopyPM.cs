using System;

namespace Logitude.DocumentTests.Models
{
    public class DocumentOutCopyPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DocumentId { get; set; }
        public string DocumentOutId { get; set; }
        public string DocumentTypeCopyId { get; set; }
        public string DocoumentTypeCopyName { get; set; }
        public string DocumentTypeCopyNameWithDocumentTypeName { get; set; }
        public string LastPrintedByUserId { get; set; }
        public DateTime? LastPrintDate { get; set; }

        public double? FileSize { get; set; }
        public string FileName { get; set; }
        public string LastPrintedByUserName { get; set; }
        public string CalculatedFileName { get; set; }
        public string FileExtension { get; set; }


    }
}