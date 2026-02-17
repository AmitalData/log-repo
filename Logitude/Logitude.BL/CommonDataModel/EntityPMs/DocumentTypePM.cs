using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.Validators;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(DocumentTypeClassLevelValidator),"ValidateClass")]
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [CustomValidation(typeof(DocumentTypeTransModeValidator), "IsDocumentTypeValid")]
    [DataContract]
    public class DocumentTypePM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Code { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Name { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool IsAir { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool IsOcean { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool IsInland { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool IsDocIn { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool IsDocOut { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool InActive { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string TemplateFormatCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool IsMaster { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool IsDirect { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool IsHouse { get; set; }

        [StringLength(500, MinimumLength = 0, ErrorMessage = "The length of the subject must be less than 500 character!")]
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string FollowUpTypeId { get; set; }
        [DataMember]
        public string FollowUpTypeName { get; set; }
        [DataMember]
        public string ObjectTableId { get; set; }
        [DataMember]
        public string SearchFields { get; set; }
        [DataMember]
        public string CustomControl { get; set; }
        [DataMember]
        public string CustomerRoleId { get; set; }
        [DataMember]
        public string AgentRoleId { get; set; }
        [DataMember]
        public string DocumentTypeDefaultReportTemplateId { get; set; }
        [DataMember]
        public string DocumentTypeDefaultHTMLTemplateId { get; set; }
        [DataMember]
        public string DocumentTypeDefaultEditorTool { get; set; }
        [DataMember]
        public bool IsAgentView { get; set; }
        [DataMember]
        public bool IsCustomerView { get; set; }
        [DataMember]
        public bool IsHybrid { get; set; }

        [DataMember]
        public string ObjectTableName { get; set; }

        [DataMember]
        public bool IsReadOnly { get; set; }

          [DataMember]
        public bool IsEnabledForCustomers { get; set; }
           [DataMember]       
        public bool IsCopiedAtSignup { get; set; }
          [DataMember]
        public string CountryCode { get; set; }


        [DataMember]
        public bool IsDocumentOneTimePrintLimited { get; set; }
        [DataMember]
        public string LimitedPrintCopyId { get; set; }

        [DataMember]
        public bool IsAgentSharedInMaster { get; set; }
        [DataMember]
        public bool IsAgentSharedInDirect { get; set; }
        [DataMember]
        public bool IsAgentSharedInHouse { get; set; }
        [DataMember]
        public string SharedDocumentTypeCopyId { get; set; }
        [DataMember]
        public bool IsAirDigitalSignRequired { get; set; }
        [DataMember]
        public bool IsOceanDigitalSignRequired { get; set; }
        [DataMember]
        public bool IsInlandDigitalSignRequired { get; set; }


        private List<DocumentTypeCustomFieldPM> documentTypeCustomFields;
        [Include]
        [Association("DocumentTypePMDocumentTypeCustomFieldPM", "Id", "DocumentTypeId")]
        [DataMember]
        public List<DocumentTypeCustomFieldPM> DocumentTypeCustomFields
        {
            get 
            {
                if (documentTypeCustomFields == null)
                {
                    documentTypeCustomFields = new List<DocumentTypeCustomFieldPM>();
                }
                return documentTypeCustomFields; 
            }
            set { documentTypeCustomFields = value; }
        }

        private List<DocumentTypeCopyPM> documentTypeCopies;
        [Composition]
        [Include]
        [Association("DocumentTypePMDocumentTypeCopyPM", "Id", "DocumentTypeId")]
        [DataMember]
        public virtual List<DocumentTypeCopyPM> DocumentTypeCopies
        {
            get
            {
                if (documentTypeCopies == null)
                {
                    documentTypeCopies = new List<DocumentTypeCopyPM>();
                }
                return documentTypeCopies;
            }
            set { documentTypeCopies = value; }
        }
        
        public List<DocumentTypeTemplatePM> DocumentTypeTemplates
        {
            get
            ;
            set;
        }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DocumentTypeCategoryCode { get; set; }

        [DataMember]
        public string DocumentTypeCategoryName { get; set; }
        [DataMember]
        public int OrderBy { get; set; }

        [DataMember]
        public string FileName { get; set; }


        [DataMember]
        public bool IsSystemAdditionalPrintingFields { get; set; }
        [DataMember]
        public string PrintingFieldsScreenCode { get; set; }
    }
}
