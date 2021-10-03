using System;
using System.Collections.Generic;


namespace Logitude.DocumentTests.Models
{
    public class DocumentOutPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Note { get; set; }
        public DateTime? IssuedDate { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public string IssuedByUserId { get; set; }
        public string DocumentTypeId { get; set; }
        public bool Issued { get; set; }
       // public string DocumentId { get; set; }
        public string SecurityId { get; set; }
        public string FollowUpId { get; set; }
        public int FollowUpCount { get; set; }
        public string IssuedByUserName { get; set; }
        public string Extension { get; set; }
        public string DocumentTypeName { get; set; }
        public byte[] ReportTemplate { get; set; }

        public bool HasFollowUp { get; set; }
        public string DocumentTypeCode { get; set; }
        public byte[] HTMLTemplate { get; set; }
        public string TemplateType { get; set; }
        public string DocumentTypeSubject { get; set; }

        public byte[] EditableFields { get; set; }
        public string DocumentTemplateId { get; set; }
        public string EmailTemplateId { get; set; }
        public string ChildEntityId { get; set; }
        public string ChildEntityReference { get; set; }
        public string XamlDocumentId { get; set; }
        public string DocumentTemplateEditorTool { get; set; }
        public string DocumentTypeObjectTableId { get; set; }
        public bool NeedsRebuild { get; set; }
        public bool IsBlobExist { get; set; }

        public int? FileSize { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public bool IsChangeIssuedDate { get; set; }

        public virtual List<DocumentOutCopyPM> DocumentOutCopies { get; set; }

        public bool IsAgentView { get; set; }
        public bool IsCustomerView { get; set; }
    }
}
