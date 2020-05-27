using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    
    public class ObjectField
    {
        [Key]
        public string Id { get; set; }
        [Index(IsUnique = true)]
        public string FieldCode { get; set; }
        public int Tenant { get; set; }
        public string FullNameTextCodeId { get; set; }
        public string ObjectTableId { get; set; }
        public string FieldName { get; set; }
        public string Code { get; set; }
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
        public int  DisplayInSearchWindowListIndex { get; set; }
        public int SearchWindowFiltersIndex { get; set; }
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

        public string ControlField1 { get; set; }
        public string ControlField2 { get; set; }
        public string ControlField3 { get; set; }

        public string ValidForQuerySection1 { get; set; }
        public string ValidForQuerySection2 { get; set; }
        
        public bool IsRestrictable { get; set; }
        public bool DisplayInEntityVariables { get; set; }

        public string TextCase { get; set; }       
        public int DigitsAfterPoint { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }

        public string DisplayInLookupColumnSize { get; set; }
        public bool DisplayLongName { get; set; }

        public string CustomerPermissionTypeCode { get; set; }
        public string AgentPermissionTypeCode { get; set; }
        public int NumberOfDigits { get; set; }
        public string CustomPickListCode { get; set; }
        public bool IsMaxLength { get; set; }

        public bool AllowedinAutomationConditions { get; set; }
        public bool AutomationEmailRecipient { get; set; }
        public bool CanAutomateSetValue { get; set; }

        public bool AllowedInCustFieldsSettings { get; set; }

        public bool AllowedInAirlineMessaging { get; set; }

        public string GeneratedComponentPath { get; set; }
        
        public bool DisplayInDocumentReferences { get; set; }

        public bool CopyToDW { get; set; }

        public bool DisplayOnLookUpLocal { get; set; }

        public string FullNameTextCodeCode { get; set; }

        public string HelpTextCodeCode { get; set; }

        public string ListTextCodeCode { get; set; }

        public string ShortNameTextCodeCode { get; set; }

        public bool DisplayInAutomationAsEnitity { get; set; }
        public string RecordType { get; set; }

        [ForeignKey("CustomerPermissionTypeCode")]
        public PermissionType CustomerPermissionType { get; set; }

        [ForeignKey("AgentPermissionTypeCode")]
        public PermissionType AgentPermissionType { get; set; }

        //[Include]
        //[Association("ObjectFieldTextCode", "ShortNameTextCodeId", "Id", IsForeignKey = true)]
        [ForeignKey("ShortNameTextCodeId")]
        public virtual TextCode ShortNameTextCode { get; set; }

        //[Include]
        //[Association("ListTextCodeObjectFieldTextCode", "ListTextCodeId", "Id", IsForeignKey = true)]
        [ForeignKey("ListTextCodeId")]
        public virtual TextCode ListTextCode { get; set; }
        //[Include]
        //[Association("ObjectTableObjectField1", "LookUpTableId", "Id", IsForeignKey = true)]
        [ForeignKey("LookUpTableId")]
        public virtual ObjectTable ObjectTable_LookUpTable { get; set; }

        //[Include]
        //[Association("MultiTableObjectField", "MultiTableId", "Id", IsForeignKey = true)]
        [ForeignKey("MultiTableId")]
        public virtual ObjectTable ObjectTable_MultiTable { get; set; }

        //[Include]
        //[Association("ObjectTableFieldObject", "ObjectTableId", "Id", IsForeignKey = true)]
        [ForeignKey("ObjectTableId")]
        public virtual  ObjectTable ObjectTable { get; set; }
        //[Include]
        //[Association("ObjectFieldDataType", "DataTypeCode", "Code", IsForeignKey = true)]
        [ForeignKey("DataTypeCode")]
        public virtual FieldDataType DataType { get; set; }

        //[Include]
        //[Association("TextCodeObjectField1", "FullNameTextCodeId", "Id", IsForeignKey = true)]
        [ForeignKey("FullNameTextCodeId")]
        public virtual TextCode FullNameTextCode { get; set; }
        //[Include]
        //[Association("TextCodeObjectField", "HelpTextCodeId", "Id", IsForeignKey = true)]
        [ForeignKey("HelpTextCodeId")]
        public virtual TextCode HelpTextCode { get; set; }

        // Silverlight
        public string ConverterName { get; set; }
        public string DataTemplateName { get; set; }
        public string ColumnHeaderTemplateName { get; set; }

        // HTML5
        public bool HasTemplate { get; set; }

        public string HtmlHeaderComponentUrl { get; set; }
        public string HtmlListComponentUrl { get; set; }

        public string HtmlHeaderComponentName { get; set; }
        public string HtmlListComponentName { get; set; }
        public bool EnableFullscreenTextBox { get; set; }
    }
}