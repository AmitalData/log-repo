using System;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class ObjectTableDetails
    {
        public ObjectTableDetails()
        {
            IsSaveButtonVisible = true;
        }
        public int Tenant { get; set; }
        public string ObjectTableName { get; set; }
        public string ObjectTableSingular { get; set; }
        public string ObjectTablePlural { get; set; }
        public string DefaultText { get; set; }
        public string TranslatedText { get; set; }
        public bool IsNewWizard { get; set; }
        public string NewWizardControlName { get; set; }
        public string LookUp1 { get; set; }
        public string LookUp2 { get; set; }
        public string DependencyFilter1 { get; set; }
        public string DependencyFilter2 { get; set; }
        public string DependencyFilter3 { get; set; }
        public string KeyPropertyPath { get; set; }
        public bool AutoCompleteSearchWindow { get; set; }
        public bool IsClosed { get; set; }
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
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public string DescriptionDefaultText { get; set; }
        public string DescriptionLocalDefaultText { get; set; }
        public bool IsSaveButtonVisible { get; set; }
        public bool IsComposition { get; set; }
        public string MainTipCode { get; set; }
        public bool EnableSecurity { get; set; }
        public string ObjectTableTypeCode { get; set; }
        public int MaxNumberOfCustomFields { get; set; }
        public bool AllowCustomFields { get; set; }
        public bool HasDynamicHeader { get; set; }
        public string LocalDefaultText { get; set; }
        public bool IsEditable { get; set; }
        public string NewButtonLocalDefaultText { get; set; }
        public string NewButtonDefaultText { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CloseTableCode { get; set; }
        public string CloseTableName { get; set; }
        public bool GenerateDomainService { get; set; }
        public bool HasDocuments { get; set; }
        public bool HasCustomFilter { get; set; }
        public bool HasCustomValidator { get; set; }
        public bool NoViewsController { get; set; }
        public string ClientModuleName { get; set; }
        public string ServerModuleName { get; set; }
        public string NewWizardComponentPath { get; set; }
        public bool HasHelper { get; set; }
        public bool HasShortTitle { get; set; }
        public bool HasMenuButtons { get; set; }
        public bool HasFiltersMenu { get; set; }             
        public bool NoViewController { get; set; }
        public bool NoPMController { get; set; }
        public string ShortTitleControlPath { get; set; }
        public bool HasCustomFields { get; set; }
        public string SortingByDirection { get; internal set; }
        public bool NoTS { get; internal set; }
        public bool DisableSearchBox { get; set; }
        public int CustomFieldsCount { get; set; }
        public bool IsLookUp { get; set; }
        public bool AllowedForComputingPartners { get; set; }
        public bool IsNew { get; set; }
        public string OldDBTableName { get; set; }
        public string Name1 { get; set; }
        public string Code1 { get; set; }
        public string CodeField { get; set; }
        public string NameField { get; set; }
        public bool AllowedInQueues { get; set; }
        public string LovDisplayMemberPath { get; set; }
        public string LovDisplayMemberPathLocal { get; set; }
        public bool IsTabsHidden { get; set; }
        public bool NoDefaultFeatures { get; internal set; }
    }
}
