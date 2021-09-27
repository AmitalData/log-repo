using System.Collections.Generic;
namespace Logitude.DocumentTests.Models
{

    public class DocumentTypePM
    {
        
        
        public string Id { get; set; }
        
        public int Tenant { get; set; }

        
        
        public string Code { get; set; }

        
        
        public string Name { get; set; }

        
        
        public string Notes { get; set; }

        
        
        public bool IsAir { get; set; }

        
        
        public bool IsOcean { get; set; }

        
        
        public bool IsInland { get; set; }

        
        
        public bool IsDocIn { get; set; }

        
        
        public bool IsDocOut { get; set; }

        
        
        public bool InActive { get; set; }

        
        
        public string TemplateFormatCode { get; set; }

        
        
        public bool IsMaster { get; set; }

        
        
        public bool IsDirect { get; set; }

        
        
        public bool IsHouse { get; set; }

        
        
        public string Subject { get; set; }
        
        public string FollowUpTypeId { get; set; }
        
        public string FollowUpTypeName { get; set; }
        
        public string ObjectTableId { get; set; }
        
        public string SearchFields { get; set; }
        
        public string CustomControl { get; set; }
        
        public string CustomerRoleId { get; set; }
        
        public string AgentRoleId { get; set; }
        
        public string DocumentTypeDefaultReportTemplateId { get; set; }
        
        public string DocumentTypeDefaultHTMLTemplateId { get; set; }
        
        public string DocumentTypeDefaultEditorTool { get; set; }
        
        public bool IsAgentView { get; set; }
        
        public bool IsCustomerView { get; set; }
        
        public bool IsHybrid { get; set; }

        
        public string ObjectTableName { get; set; }

        
        public bool IsReadOnly { get; set; }

          
        public bool IsEnabledForCustomers { get; set; }
                  
        public bool IsCopiedAtSignup { get; set; }
          
        public string CountryCode { get; set; }


        
        public bool IsDocumentOneTimePrintLimited { get; set; }
        
        public string LimitedPrintCopyId { get; set; }

        
        public bool IsAgentSharedInMaster { get; set; }
        
        public bool IsAgentSharedInDirect { get; set; }
        
        public bool IsAgentSharedInHouse { get; set; }
        
        public string SharedDocumentTypeCopyId { get; set; }
        
        public bool IsAirDigitalSignRequired { get; set; }
        
        public bool IsOceanDigitalSignRequired { get; set; }
        
        public bool IsInlandDigitalSignRequired { get; set; }


        
        public string OnSendPopulateDateFieldName { get; set; }

        
        public string OnUploadPopulateDateFieldName { get; set; }

        
        public string OnPrintPopulateDateFieldName { get; set; }


        public string DocumentTypeCategoryCode { get; set; }

        
        public string DocumentTypeCategoryName { get; set; }
        
        public int OrderBy { get; set; }

        
        public string FileName { get; set; }


        
        public bool IsSystemAdditionalPrintingFields { get; set; }
        
        public string PrintingFieldsScreenCode { get; set; }


        
        public bool AddedManually { get; set; }
        
        public string CopyName { get; set; }




    }
}
