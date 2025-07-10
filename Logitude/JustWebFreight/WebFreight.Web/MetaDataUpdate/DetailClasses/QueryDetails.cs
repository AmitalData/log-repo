namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class QueryDetails
    {
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string UserId { get; set; }
        public string ObjectTableId { get; set; }
        public bool SystemLevel { get; set; }
        public bool TenantLevel { get; set; }
        public string OriginalQueryId { get; set; }
        public string OriginalQueryCode { get; set; }
        public string QuerySection { get; set; }
        public int IndexOrder { get; set; }
        public bool DisplayCount { get; set; }
        public bool IsAddNewEntityEnabled { get; set; }
        public string NameTextCodeId { get; set; }
        public string QueryGroupCode { get; set; }
        public string DefaultSortName { get; set; }
        public string DefaultSortDirection { get; set; }
        public string SpotlightDataTemplate { get; set; }
        public bool Agent { get; set; }
        public bool Customer { get; set; }
        public bool Internal { get; set; }
        public string FeatureId { get; set; }
        public string EditWizardName { get; set; }
        public string FullLocalDefaultText { get; set; }
        public string FullLocalDefaultTextBack_up { get; set; }

        public string Perspective { get; set; }
        public bool IsHiddenFromView { get; set; }
        public bool IsNewFromTenantZeroOnly { get; set; }
        public string EditWizardComponentPath { get; set; }
        public string NameTextCodeCode { get; set; }
        public string ObjectTableName { get; set; }
        public string FeatureUniqeCode { get; set; }
    }
}