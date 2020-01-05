using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class ObjectTable
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public bool IsNewWizard { get; set; }
        public string NewWizardControlName { get; set; }
        public string LookUp1 { get; set; }
        public string LookUp2 { get; set; }
        public string DependencyFilter1 { get; set; }
        public string DependencyFilter2 { get; set; }
        public string DependencyFilter3 { get; set; }
        public bool HasCustomFilter { get; set; }
        public bool HasCustomValidator { get; set; }
        public string KeyPropertyPath { get; set; }
        public bool AutoCompleteSearchWindow { get; set; }
        public bool IsClosed { get; set; }
        public string HeaderScreenId { get; set; }
        public bool CacheOnClient { get; set; }
        public bool EditableFromAutoCompleteWindow { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public bool HasCounter { get; set; }
        public bool EnableEditFromLOV { get; set; }
        public bool EnableAddFromLOV { get; set; }
        public bool IsRestrictable { get; set; }
        public bool IsMain { get; set; }
        public bool IsAutoComplete { get; set; }
        public string SortingByObjectField { get; set; }
        public string DBTableName { get; set; }
        public int CustomFieldsCount { get; set; }
        public bool HasCustomFields { get; set; }
        public bool InActive { get; set; }
        public string DescriptionTextCodeId { get; set; }
        public string SearchFields { get; set; }
        public bool IsSaveButtonVisible { get; set; }
        public string MainTipCode { get; set; }
        public bool EnableSecurity { get; set; }
        public string ObjectTableTypeCode { get; set; }
        public bool IsComposition { get; set; }
        public int MaxNumberOfCustomFields { get; set; }
        public bool AllowCustomFields { get; set; }
        public bool HasDynamicHeader { get; set; }
        public bool HasDocuments { get; set; }
        public byte[] EntityResource { get; set; }
        public bool IsLookUp { get; set; }
        public string NewButtonTextCodeId { get; set; }
        public bool IsEditable { get; set; }
        public bool AllowedForComputingPartners { get; set; }
        public string CodeField { get; set; }
        public string NameField { get; set; }

        public string LovDisplayMemberPath { get; set; }
        public string LovDisplayMemberPathLocal { get; set; }
        public string DescriptionTextCodeCode { get; set; }
        public string NewButtonTextCodeCode { get; set; }

        public DateTime? EntityResourceLastUpdate { get; set; }
        public bool DisableSearchBox { get; set; }

        [ForeignKey("ObjectTableTypeCode")]
        public ObjectTableType ObjectTableType { get; set; }

        [ForeignKey("MainTipCode")]
        public Tip MainTip { get; set; }

        [ForeignKey("DescriptionTextCodeId")]
        public virtual TextCode DescriptionTextCode { get; set; }

        [ForeignKey("HeaderScreenId")]
        public virtual Screen HeaderScreen { get; set; }

        [ForeignKey("NewButtonTextCodeId")]
        public virtual TextCode NewButtonTextCode { get; set; }

        // HTML5
        public string ClientModuleName { get; set; }
        public string ServerModuleName { get; set; }
        public string NewWizardComponentPath { get; set; }
        public bool HasHelper { get; set; }
        public bool HasShortTitle { get; set; }
        public bool HasMenuButtons { get; set; }
        public bool HasFiltersMenu { get; set; }
        public string UpdateKey { get; set; }

        public string DownloadToExcelFeatureCode { get; set; }
        public string SplitComponentPath { get; set; }
        public bool AllowedInQueues { get; set; }
        public bool IsTabsHidden { get; set; }
    }
}
