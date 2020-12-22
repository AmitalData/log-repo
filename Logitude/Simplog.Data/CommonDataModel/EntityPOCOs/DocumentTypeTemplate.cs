using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class DocumentTypeTemplate
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        //public string Name { get; set; }
        public byte[] TemplateBody { get; set; }
        public string TemplateType { get; set; }
        public string LastUpdatedByUserId { get; set; }
        public string DocumentTypeId { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        //public bool IsDefault { get; set; }
        public bool InActive { get; set; }
        public string EditorTool { get; set; }
        public double? VerticalShift { get; set; }
        public double? HorizontalShift { get; set; }

        public byte[] TemplateFooterHtml { get; set; }
        public byte[] TemplateHeaderHtml { get; set; }

        public byte[] TemplateBodyHtml { get; set; }
        public byte[] TemplateBodyjson { get; set; }
        public bool IsEnabledForCustomers { get; set; }
        public bool IsCopiedAtSignup { get; set; }

        public string CountryCode { get; set; }

        public string InternalRemarks { get; set; }
        public string Language { get; set; }
        public string OriginalTemplateId { get; set; }

        public int TemplateHeaderHeight { get; set; }
        public int TemplateFooterHeight { get; set; }
        public string TemplateTechnologyCode { get; set; }


        public string From { get; set; }
        public string ReplyTo { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string To { get; set; }

        public string AutomationId { get; set; }



        public string DefultAttachmentsXML { get; set; }
        //public bool IsDuplex { get; set; }
        [ForeignKey("LastUpdatedByUserId")]
        public virtual User LastUpdatedByUser { get; set; }
        [ForeignKey("DocumentTypeId")]
        public virtual DocumentType DocumentType { get; set; }

        [ForeignKey("OriginalTemplateId")]
        public virtual DocumentTypeTemplate OriginalTemplate { get; set; }



        [ForeignKey("AutomationId")]
        public virtual Automation Automation { get; set; }





    }
}