using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.DataContracts
{
    public class ShipmentShareDocumentsData
    {
        public List<ShareDocument> ShareDocuments { get; set; }
        public string ShipmentNumber { get; set; }
        public string EntityId { get; set; }
        public string AgentSharedManifestRef { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string ShipmentLevelName { get; set; }
        public string AgentId { get; set; }
        public int TenantAgent { get; set; }

    }

    public class ShareDocument
    {
        public string DocumentTypeName { get; set; }
        public string DocumentTypeCode { get; set; }
        public bool IsReady { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public DateTime? LastShareDate { get; set; }
        public string SecurityId { get; set; }
        public string DirectionCode { get; set; }
        public string FileName { get; set; }
        public string DocumentId { get; set; }
        public string DocumentsFilingId { get; set; }
        public string Extension { get; set; }
        public double? FileSize { get; set; }
        public string EntityId { get; set; }

        public string DocumentTypeId { get; set; }
        public string DocumentTypeCopyId { get; set; }
        public DocumentsFilingPM DocumentsFilingPM { get; set; }
        public string DocumentOutId { get; set; }

        public string ActionButtonLabel { get; set; }

    }
}
