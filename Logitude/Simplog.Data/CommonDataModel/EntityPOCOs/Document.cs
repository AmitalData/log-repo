using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Document
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string FileName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Extension { get; set; }
        public double? FileSize { get; set; }
        public bool HasFile { get; set; }
        public string Folder { get; set; }
        public string SmallDocumentId { get; set; }
        public string CalculatedFileName { get; set; }
        public bool IsEncrypted { get; set; }
        [ForeignKey("SmallDocumentId")]
        public SmallDocument SmallDocument { get; set; }
        //public List<DocumentIn> ExternalDocuments { get; set; }
        //public List<DocumentOut> InternalDocuments { get; set; }
        //public List<DocumentOut> DocumentOuts { get; set; }
        //public List<CommunicationLog> CommunicationLogs { get; set; }
        //public List<CommunicationAttachment> CommunicationAttachments { get; set; }
        //public List<DocumentOutCopy> DocumentOutCopies { get; set; }
        //public List<SharedLogisticsUpdate> SharedLogisticsUpdates { get; set; }

    }
}