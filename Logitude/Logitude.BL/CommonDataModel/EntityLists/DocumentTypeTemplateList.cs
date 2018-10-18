using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class DocumentTypeTemplateList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        //public byte[] TemplateBody { get; set; }
        public string TemplateType { get; set; }
        public string LastUpdatedByUserId { get; set; }
        public string DocumentTypeId { get; set; }
        public string Description { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string EditorTool { get; set; }
        public double? VerticalShift { get; set; }
        public double? HorizontalShift { get; set; }
        public bool  InActive { get; set; }
        public bool  IsDefault { get; set; }
        public string LastUpdateByUserName { get; set; }
        public string Subject { get; set; }
        public string ContactEmail { get; set; }
        
        public string Language { get; set; }
        public string OriginalTemplateId { get; set; }

        public bool IsHaveJsonString { get; set; }

        public string OriginalTemplateName { get; set; }
        public string InternalRemarks { get; set; }

        public string CountryCode { get; set; }
        public bool IsCopiedAtSignup { get; set; }
        public bool IsEnabledForCustomers { get; set; }

        public string DocumentTypeCode { get; set; }
        public string From { get; set; }
        public string ReplyTo { get; set; }

        public string DocumentTypeName { get; set; }
        public bool IsHideDocumentName { get; set; }
        public byte[] TemplateFooterHtml { get; set; }
        public byte[] TemplateHeaderHtml { get; set; }
        public string ObjectTableId { get; set; }
        public int TemplateHeaderHeight { get; set; }
        public int TemplateFooterHeight { get; set; }
        public string TemplateTechnologyCode { get; set; }
        public string CC { get; set; }

        public string CountryName { get; set; }
        
    }
}
