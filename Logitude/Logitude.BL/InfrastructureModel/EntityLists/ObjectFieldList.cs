using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class ObjectFieldList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string FullNameTextCodeId { get; set; }
        public string ObjectTableId { get; set; }
        public string FieldName { get; set; }
        public string DataTypeCode { get; set; }
        public int MaxLength { get; set; }
        public bool IsRequiered { get; set; }
        public bool IsCustom { get; set; }
        public string HelpTextCodeId { get; set; }
        public int MinLength { get; set; }
        public string LookUpTableId { get; set; }
        public bool DisplayOnLookUp { get; set; }
        public bool CanFilter { get; set; }
        public bool DisplayOnly { get; set; }
        public bool SystemRequired { get; set; }
        public int SystemMaxLength { get; set; }
        public string ListTextCodeId { get; set; }
        public bool DisplayInList { get; set; }
        public string ConverterName { get; set; }
        public string DataTemplateName { get; set; }
        public bool IsCustomFilter { get; set; }
        public string Operator { get; set; }
        public bool MultiLine { get; set; }
        public bool IsTimeFrameFilter { get; set; }
        public bool DisplayInSearchWindowList { get; set; }
        public bool DisplayInSearchWindowFilters { get; set; }
        public string PMPropertyPath { get; set; }
        public string ListPropertyPath { get; set; }
        public string LookUpControlName { get; set; }
        public int DisplayInLookUpIndex { get; set; }
        public bool AutomaticField { get; set; }
        public bool UniqueField { get; set; }
        public string ShortNameTextCodeId { get; set; }
        public int DisplayInSearchWindowListIndex { get; set; }
        public int DisplayInSearchWindowFilterInx { get; set; }
        public bool IsMulti { get; set; }
        public string MultiTableId { get; set; }
        public string DependencyFilter1Value { get; set; }
        public string DependencyFilter2Value { get; set; }
        public string DependencyFilter3Value { get; set; }
        public string DependencyFilter1Type { get; set; }
        public string DependencyFilter2Type { get; set; }
        public string DependencyFilter3Type { get; set; }
        public bool DependencyFilter1IsList { get; set; }
        public bool DependencyFilter2IsList { get; set; }
        public bool DependencyFilter3IsList { get; set; }
        public string ValidForQuerySection1 { get; set; }
        public string ValidForQuerySection2 { get; set; }
        public bool IsRestrictable { get; set; }
        public bool DisplayInEntityVariables { get; set; }
        public string SearchFields { get; set; }
        public string DisplayInLookupColumnSize { get; set; }
        public string ColumnHeaderTemplateName { get; set; }
        public bool IsMaxLength { get; set; }
        public bool AllowedinAutomationConditions { get; set; }
        public bool AutomationEmailRecipient { get; set; }
        public bool CanAutomateSetValue { get; set; }
        public bool HasTemplate { get; set; }        
        public string FullNameTextCodeDefaultText { get; set; }
        public bool AllowedInCustomerFieldsSetting { get; set; }
        public bool DisplayInDocumentReferences { get; set; }
        public string ListTextCodeCode { get; set; }
        public string Code { get; set; }
        public bool CopyToDW { get; set; }
        public bool DisplayOnLookUpLocal { get; set; }
        public bool EnableFullscreenTextBox { get; set; }
        public bool DisplayInAutomationAsEnitity { get; set; }
        public string RecordType { get; set; }
        public string ObjectTable_LookUpTableName { get; set; }
        public string FieldCode { get;  set; }
        public string FullNameTextCodeCode { get; set; }
        public string ShortNameTextCodeCode { get; set; }
        public string HelpTextCodeCode { get; set; }
        public string AdditionalQuerySections { get; set; }
        public bool DisplayInRequiredFields { get; set; }

        public string LeftKey { get; set; }
        public string RightKey { get; set; }
        public bool IsForeignKey { get; set; }
        public string ForeignEntity { get; set; }
        public string NavigationPropertyName { get; set; }
        public bool ForMetaDataOnly { get; set; }
        public bool IsListFilter { get; set; }
        public int NumberOfDigits { get; set; }
        public int DigitsAfterPoint { get; set; }
        public string CustomPickListCode { get; set; }

        public bool InUse { get; set; }

    }
}