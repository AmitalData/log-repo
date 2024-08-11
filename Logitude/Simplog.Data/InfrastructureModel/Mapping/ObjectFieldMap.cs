using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class ObjectFieldMap : EntityTypeConfiguration<ObjectField>
    {
        public ObjectFieldMap()
        {
            this.HasEntitySetName("ObjectFields");
            this.HasKey(t => t.Id);
           
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FieldName).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ShortName).IsRequired().HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.ObjectTableId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DataTypeCode).HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.LookUpTableId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.HelpTextCodeId).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.FullNameTextCodeId).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.ListTextCodeId).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.ConverterName).HasMaxLength(200).IsUnicode(true);
            this.Property(t => t.DataTemplateName).HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.LookUpControlName).HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.Operator).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.PMPropertyPath).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ListPropertyPath).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ShortNameTextCodeId).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.MultiTableId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DependencyFilter1Type).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.DependencyFilter2Type).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.DependencyFilter3Type).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.DependencyFilter1Value).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.DependencyFilter2Value).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.DependencyFilter3Value).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.ValidForQuerySection1).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ValidForQuerySection2).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.TextCase).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.DisplayInLookupColumnSize).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ColumnHeaderTemplateName).HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.CustomerPermissionTypeCode).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AgentPermissionTypeCode).HasMaxLength(15).IsUnicode(false);            
            this.Property(t => t.CustomPickListCode).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.ControlField1).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ControlField2).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ControlField3).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.AllowedinAutomationConditions);
            this.Property(t => t.AutomationEmailRecipient);
            this.Property(t => t.CanAutomateSetValue);
            this.Property(t => t.HtmlHeaderComponentUrl).HasMaxLength(256).IsUnicode(false);
            this.Property(t => t.HtmlListComponentUrl).HasMaxLength(256).IsUnicode(false);
            this.Property(t => t.HtmlHeaderComponentName).HasMaxLength(200).IsUnicode(true);
            this.Property(t => t.HtmlListComponentName).HasMaxLength(200).IsUnicode(true);
            this.Property(t => t.GeneratedComponentPath).HasMaxLength(250).IsUnicode(false);
            this.Property(t => t.DisplayInDocumentReferences);
            this.Property(t => t.Code).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.EnableFullscreenTextBox);
            this.Property(t => t.RecordType).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.FieldCode).HasMaxLength(200).IsUnicode(false);
            //this.Property(t => t.FieldCode).IsRequired().HasMaxLength(200).IsUnicode(false);
            this.Property(t => t.FullNameTextCodeCode).HasMaxLength(200).IsUnicode(true);
            this.Property(t => t.HelpTextCodeCode).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ListTextCodeCode).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ShortNameTextCodeCode).HasMaxLength(200).IsUnicode(true);
            this.Property(t => t.AdditionalQuerySections).HasMaxLength(200).IsUnicode(false);

            this.Property(t => t.LeftKey).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.RightKey).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.IsForeignKey);
            this.Property(t => t.ForeignEntity).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.NavigationPropertyName).HasMaxLength(200).IsUnicode(true);
            this.Property(t => t.DefaultAdditionalFilters).IsMaxLength().IsUnicode(true);
            this.Property(t => t.ForMetaDataOnly);
            this.Property(t => t.IsListFilter);

            // Table & Column Mappings
            this.ToTable("ObjectFields");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.FieldName).HasColumnName("FieldName");
            this.Property(t => t.ShortName).HasColumnName("ShortName");
            this.Property(t => t.MaxLength).HasColumnName("MaxLength");
            this.Property(t => t.IsRequiered).HasColumnName("IsRequiered");
            this.Property(t => t.IsCustom).HasColumnName("IsCustom");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.DataTypeCode).HasColumnName("DataTypeCode");
            this.Property(t => t.MinLength).HasColumnName("MinLength");
            this.Property(t => t.DisplayOnLookUp).HasColumnName("DisplayOnLookUp");
            this.Property(t => t.LookUpTableId).HasColumnName("LookUpTableId");
            this.Property(t => t.HelpTextCodeId).HasColumnName("HelpTextCodeId");
            this.Property(t => t.FullNameTextCodeId).HasColumnName("FullNameTextCodeId");
            this.Property(t => t.CanFilter).HasColumnName("CanFilter");
            this.Property(t => t.DisplayOnly).HasColumnName("DisplayOnly");
            this.Property(t => t.SystemRequired).HasColumnName("SystemRequired");
            this.Property(t => t.SystemMaxLength).HasColumnName("SystemMaxLength");
            this.Property(t => t.DisplayInList).HasColumnName("DisplayInList");
            this.Property(t => t.ListTextCodeId).HasColumnName("ListTextCodeId");
            this.Property(t => t.ConverterName).HasColumnName("ConverterName");
            this.Property(t => t.DataTemplateName).HasColumnName("DataTemplateName");
            this.Property(t => t.MultiLine).HasColumnName("MultiLine");
            this.Property(t => t.LookUpControlName).HasColumnName("LookUpControlName");
            this.Property(t => t.IsCustomFilter).HasColumnName("IsCustomFilter");
            this.Property(t => t.Operator).HasColumnName("Operator");
            this.Property(t => t.IsTimeFrameFilter).HasColumnName("IsTimeFrameFilter");
            this.Property(t => t.DisplayInSearchWindowFilters).HasColumnName("DisplayInSearchWindowFilters");
            this.Property(t => t.DisplayInSearchWindowList).HasColumnName("DisplayInSearchWindowList");
            this.Property(t => t.PMPropertyPath).HasColumnName("PMPropertyPath");
            this.Property(t => t.ListPropertyPath).HasColumnName("ListPropertyPath");
            this.Property(t => t.DisplayInLookUpIndex).HasColumnName("DisplayInLookUpIndex");
            this.Property(t => t.AutomaticField).HasColumnName("AutomaticField");
            this.Property(t => t.UniqueField).HasColumnName("UniqueField");
            this.Property(t => t.ShortNameTextCodeId).HasColumnName("ShortNameTextCodeId");
            this.Property(t => t.DisplayInSearchWindowListIndex).HasColumnName("DisplayInSearchWindowListIndex");
            this.Property(t => t.IsMulti).HasColumnName("IsMulti");
            this.Property(t => t.MultiTableId).HasColumnName("MultiTableId");
            this.Property(t => t.DependencyFilter1Type).HasColumnName("DependencyFilter1Type");
            this.Property(t => t.DependencyFilter2Type).HasColumnName("DependencyFilter2Type");
            this.Property(t => t.DependencyFilter3Type).HasColumnName("DependencyFilter3Type");
            this.Property(t => t.DependencyFilter1Value).HasColumnName("DependencyFilter1Value");
            this.Property(t => t.DependencyFilter2Value).HasColumnName("DependencyFilter2Value");
            this.Property(t => t.DependencyFilter3Value).HasColumnName("DependencyFilter3Value");
            this.Property(t => t.ValidForQuerySection1).HasColumnName("ValidForQuerySection1");
            this.Property(t => t.ValidForQuerySection2).HasColumnName("ValidForQuerySection2");
            this.Property(t => t.IsRestrictable).HasColumnName("IsRestrictable");
            this.Property(t => t.DisplayInEntityVariables).HasColumnName("DisplayInEntityVariables");
            this.Property(t => t.TextCase).HasColumnName("TextCase");
            this.Property(t => t.DigitsAfterPoint).HasColumnName("DigitsAfterPoint");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.DisplayInLookupColumnSize).HasColumnName("DisplayInLookupColumnSize");
            this.Property(t => t.ColumnHeaderTemplateName).HasColumnName("ColumnHeaderTemplateName");
            this.Property(t => t.DisplayLongName).HasColumnName("DisplayLongName");
            this.Property(t => t.CustomerPermissionTypeCode).HasColumnName("CustomerPermissionTypeCode");
            this.Property(t => t.AgentPermissionTypeCode).HasColumnName("AgentPermissionTypeCode");
            this.Property(t => t.CustomPickListCode).HasColumnName("CustomPickListCode");
            this.Property(t => t.ControlField1).HasColumnName("ControlField1");
            this.Property(t => t.ControlField2).HasColumnName("ControlField2");
            this.Property(t => t.ControlField3).HasColumnName("ControlField3");
            this.Property(t => t.AutomationEmailRecipient).HasColumnName("AutomationEmailRecipient");
            this.Property(t => t.AllowedinAutomationConditions).HasColumnName("AllowedinAutomationConditions");
            this.Property(t => t.CanAutomateSetValue).HasColumnName("CanAutomateSetValue");
            this.Property(t => t.AllowedInAirlineMessaging).HasColumnName("AllowedInAirlineMessaging");
            this.Property(t => t.HasTemplate).HasColumnName("HasTemplate");
            this.Property(t => t.AllowedInCustomerFieldsSettings).HasColumnName("AllowedInCustomerFieldsSettings");
            this.Property(t => t.GeneratedComponentPath).HasColumnName("GeneratedComponentPath");
            this.Property(t => t.DisplayInDocumentReferences).HasColumnName("DisplayInDocumentReferences");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CopyToDW).HasColumnName("CopyToDW");
            this.Property(t => t.DisplayOnLookUpLocal).HasColumnName("DisplayOnLookUpLocal");
            this.Property(t => t.EnableFullscreenTextBox).HasColumnName("EnableFullscreenTextBox");
            this.Property(t => t.DisplayInAutomationAsEnitity).HasColumnName("DisplayInAutomationAsEnitity");
            this.Property(t => t.RecordType).HasColumnName("RecordType");
            this.Property(t => t.FieldCode).HasColumnName("FieldCode");
            this.Property(t => t.FullNameTextCodeCode).HasColumnName("FullNameTextCodeCode");
            this.Property(t => t.HelpTextCodeCode).HasColumnName("HelpTextCodeCode");
            this.Property(t => t.ListTextCodeCode).HasColumnName("ListTextCodeCode");
            this.Property(t => t.ShortNameTextCodeCode).HasColumnName("ShortNameTextCodeCode");
            this.Property(t => t.AdditionalQuerySections).HasColumnName("AdditionalQuerySections");
             this.Property(t => t.DisplayInRequiredFields).HasColumnName("DisplayInRequiredFields");

            this.Property(t => t.LeftKey).HasColumnName("LeftKey");
            this.Property(t => t.RightKey).HasColumnName("RightKey");
            this.Property(t => t.IsForeignKey).HasColumnName("IsForeignKey");
            this.Property(t => t.ForeignEntity).HasColumnName("ForeignEntity");
            this.Property(t => t.NavigationPropertyName).HasColumnName("NavigationPropertyName"); 
            this.Property(t => t.DefaultAdditionalFilters).HasColumnName("DefaultAdditionalFilters");
            this.Property(t => t.ForMetaDataOnly).HasColumnName("ForMetaDataOnly");
            this.Property(t => t.IsListFilter).HasColumnName("IsListFilter");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.DisplayInSearchWindowFiltersIndex).HasColumnName("SearchWindowFiltersIndex");
                this.Property(t => t.AllowedInCustomerFieldsSettings).HasColumnName("AllowedInCustFieldsSettings");
            }

            //#else
            else
            {
                this.Property(t => t.DisplayInSearchWindowFiltersIndex).HasColumnName("DisplayInSearchWindowFiltersIndex");
                this.Property(t => t.AllowedInCustomerFieldsSettings).HasColumnName("AllowedInCustomerFieldsSettings");
            }

//#endif

        }
    }
}
