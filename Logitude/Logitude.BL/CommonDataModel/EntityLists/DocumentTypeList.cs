using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    [DataContract]
    public class DocumentTypeList
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public int Tenant { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public bool IsAir { get; set; }
        [DataMember]
        public bool IsOcean { get; set; }
        [DataMember]
        public bool IsInland { get; set; }
        [DataMember]
        public bool IsDocIn { get; set; }
        [DataMember]
        public bool IsDocOut { get; set; }
        [DataMember]
        public string FollowUpTypeId { get; set; }
        [DataMember]
        public string FollowUpTypeName { get; set; }
        [DataMember]
        public string ObjectTableId { get; set; }
        [DataMember]
        public string ObjectTableName { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string DocumentTypeDefaulReportTempId { get; set; }
        [DataMember]
        public string DocumentTypeDefaultHTMLTemplateId { get; set; }
        [DataMember]
        public string DocumentTypeDefaultEditorTool { get; set; }
        [DataMember]
        public string TemplateFormatCode { get; set; }
        [DataMember]
        public bool InActive { get; set; }
        [DataMember]
        public string SearchFields { get; set; }
        [DataMember]
        public bool IsMaster { get; set; }
        [DataMember]
        public bool IsDirect { get; set; }
        [DataMember]
        public bool IsHouse { get; set; }
        [DataMember]
        public string CustomControl { get; set; }
        [DataMember]
        public bool IsCustomerView { get; set; }
        [DataMember]
        public bool IsCustomerUploadPermission { get; set; }
        [DataMember]
        public bool IsAgentView { get; set; }
        [DataMember]
        public bool IsReadOnly { get; set; }
        [DataMember]
        public string LimitedPrintCopyId { get; set; }
        [DataMember]
        public bool IsDocumentOneTimePrintLimited { get; set; }

        [DataMember]
        public string CountryCode { get; set; }

        public bool IsCopiedAtSignup { get; set; }
        public bool IsEnabledForCustomers { get; set; }
        [DataMember]
        public string DocumentTypeCategoryCode { get; set; }
        public string DocumentTypeCategoryName { get; set; }
        public string FileName { get; set; }

        public bool IsAgentSharedInMaster { get; set; }
        public bool IsAgentSharedInDirect { get; set; }
        public bool IsAgentSharedInHouse { get; set; }
        public string SharedDocumentTypeCopyId { get; set; }


        public string OnSendPopulateDateFieldName { get; set; }
        public string OnUploadPopulateDateFieldName { get; set; }
        public string OnPrintPopulateDateFieldName { get; set; }


       




        [DataMember]
        public int OrderBy { get; set; }

        [DataMember]
        public string OrderedDisplayName { get; set; }

        [DataMember]
        public bool IsSystemAdditionalPrintingFields { get; set; }
        [DataMember]
        public string PrintingFieldsScreenCode { get; set; }


        [DataMember]
        public bool AddedManually { get; set; }




    }
}
