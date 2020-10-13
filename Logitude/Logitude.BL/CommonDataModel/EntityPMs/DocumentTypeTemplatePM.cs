using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.Helpers;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class DocumentTypeTemplatePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        //public string Name { get; set; }
        public byte[] TemplateBody { get; set; }
        public string TemplateType { get; set; }
        public string LastUpdatedByUserId { get; set; }
        public string DocumentTypeId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Description { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdateByUserName { get; set; }
        public bool IsDefault { get; set; }
        public bool InActive { get; set; }
        public string EditorTool { get; set; }
        public double? VerticalShift { get; set; }
        public double? HorizontalShift { get; set; }
        public string Subject { get; set; }
        public byte[] TemplateBodyHtml { get; set; }

        public byte[] TemplateBodyjson { get; set; }
        public string Language { get; set; }
        public string OriginalTemplateId { get; set; }

        public string ContactEmail { get; set; }
        public string InternalRemarks { get; set; }

        public byte[] TemplateFooterHtml { get; set; }
        public byte[] TemplateHeaderHtml { get; set; }

        public string CountryCode { get; set; }
        public bool IsCopiedAtSignup { get; set; }
        public bool IsEnabledForCustomers { get; set; }

        public string OriginalTemplateName { get; set; }
        public string DocumentTypeCode { get; set; }
        public string DocumentTypeName { get; set; }
        public bool IsHideDocumentName { get; set; }
        public string CountryName { get; set; }
        public string From { get; set; }
        public string ReplyTo { get; set; }
        public int TemplateHeaderHeight { get; set; }
        public int TemplateFooterHeight { get; set; }
        public string TemplateTechnologyCode { get; set; }

        public string ObjectTableId { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }

        [DataMember]
        public string DefultAttachmentsXML { get; set; }


        [DataMember]
        public bool IsDefultAttachmentsXMLChanged { get; set; }

        [DataMember]
        public List<DocumentDefultAttachment> DocumentDefultAttachments { get; set; }



        //         this.Property(t => t.CountryCode).IsFixedLength().IsUnicode(false).HasMaxLength(2);

        // this.Property(t => t.InActive)
        //.IsRequired();

        // this.Property(t => t.IsCopiedAtSignup)
        //     .IsRequired();

        // this.Property(t => t.IsEnabledForCustomers)
        //     .IsRequired();



        // this.Property(t => t.InternalRemarks)
        //    .HasMaxLength(500)
        //    .IsUnicode(true);

        // this.Property(t => t.Language)
        //    .HasMaxLength(100)
        //    .IsUnicode(true);

        // this.Property(t => t.OriginalTemplateId)
        //     .HasMaxLength(15)
        //     .IsUnicode(false);

        //public bool IsDuplex { get; set; }

    }
}
