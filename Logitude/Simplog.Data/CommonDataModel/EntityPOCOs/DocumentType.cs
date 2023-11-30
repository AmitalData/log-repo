using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{

    public class DocumentType
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public string FollowUpTypeId { get; set; }

        public string Notes { get; set; }
        public bool IsAir { get; set; }
        public bool IsOcean { get; set; }
        public bool IsInland { get; set; }

        public bool IsDocIn { get; set; }
        public bool IsDocOut { get; set; }

        public bool InActive { get; set; }

        public bool IsMaster { get; set; }
        public bool IsDirect { get; set; }
        public bool IsHouse { get; set; }
        public string SearchFields { get; set; }

    
        // public string XML { get; set; }
        public string ObjectTableId { get; set; }
        public string Subject { get; set; }
        public string DocumentTypeDefaultReportTemplateId { get; set; }
        public string DocumentTypeDefaultHTMLTemplateId { get; set; }
        public string DocumentTypeDefaultEditorTool { get; set; }

        public string TemplateFormatCode { get; set; }
        public string CustomControl { get; set; }
        public string CustomerRoleId { get; set; }
        public string AgentRoleId { get; set; }

        public bool IsDocumentOneTimePrintLimited { get; set; }

        public string LimitedPrintCopyId { get; set; }

        public bool IsEnabledForCustomers { get; set; }
        public bool IsCopiedAtSignup { get; set; }

        public string CountryCode { get; set; }
        public string DocumentsDataProviderCode { get; set; }



        public bool IsAgentSharedInMaster { get; set; }
        public bool IsAgentSharedInDirect { get; set; }
        public bool IsAgentSharedInHouse { get; set; }
        public string SharedDocumentTypeCopyId { get; set; }

        public bool IsAirDigitalSignRequired { get; set; }
        public bool IsOceanDigitalSignRequired { get; set; }
        public bool IsInlandDigitalSignRequired { get; set; }

        public bool IsSystemAdditionalPrintingFields { get; set; }
        public string PrintingFieldsScreenCode { get; set; }
        public bool AddedManually { get; set; }

        public string OnSendPopulateDateFieldName { get; set; }
        public string OnUploadPopulateDateFieldName { get; set; }
        public string OnPrintPopulateDateFieldName { get; set; }





        [ForeignKey("CustomerRoleId")]
        public Role CustomerRole { get; set; }
        [ForeignKey("AgentRoleId")]
        public Role AgentRole { get; set; }

        public bool IsCustomerView { get; set; }
        public bool IsAgentView { get; set; }

        public bool IsReadOnly { get; set; }

        public int OrderBy { get; set; }
        public string FileName { get; set; }


        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }
        [ForeignKey("TemplateFormatCode")]
        public virtual TemplateFormat TemplateFormat { get; set; }


        [ForeignKey("DocumentsDataProviderCode")]
        public virtual DocumentsDataProvider DocumentsDataProvider { get; set; }

        public string DocumentTypeCategoryCode { get; set; }
        [ForeignKey("DocumentTypeCategoryCode")]
        public virtual DocumentTypeCategory DocumentTypeCategory { get; set; }

        public bool IsCustomerUploadPermission { get; set; }
        ////[Include]
        ////[Association("DocumentTypeFollowUpType", "FollowUpTypeId", "Id", IsForeignKey = true)]
        //public virtual FollowUpType FollowUpType { get; set; }

        //public List<DocumentOut> InternalDocuments { get; set; }  
        //public List<DocumentIn> ExternalDocuments { get; set; }
        //public List<FormCustomField> FormCustomFields { get; set; }
        //public List<DocumentTypeCustomField> DocumentTypeCustomFields { get; set; }
        //public List<DocumentTypeTemplate> DocumentTypeTemplates { get; set; }
        //public List<DocumentTypeCopy> DocumentTypeCopies { get; set; }
    }
}