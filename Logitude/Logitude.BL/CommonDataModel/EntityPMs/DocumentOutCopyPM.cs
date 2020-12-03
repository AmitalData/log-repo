using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class DocumentOutCopyPM
    {
        [Key]
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
        public ChangeSetOperation changeOp { get; set; }
        public string FileExtension { get; set; }


    }
}