using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class ObjectTableMap : EntityTypeConfiguration<ObjectTable>
    {
        public ObjectTableMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.LookUp1).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.LookUp2).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.DependencyFilter1).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.DependencyFilter2).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.DependencyFilter3).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.KeyPropertyPath).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.HeaderScreenId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.HeaderScreenCode).HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.SortingByObjectField).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.DBTableName).HasMaxLength(200).IsUnicode(true);
            this.Property(t => t.DescriptionTextCodeId).HasMaxLength(60).IsUnicode(true);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.MainTipCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ObjectTableTypeCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.NewWizardControlName).HasMaxLength(500).IsUnicode(true);
            this.Property(t => t.HasDocuments);
            this.Property(t => t.NewButtonTextCodeId).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.HasCustomValidator);
            this.Property(t => t.UpdateKey).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.AllowedForComputingPartners);
            this.Property(t => t.CodeField).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.NameField).HasMaxLength(120).IsUnicode(true);
            this.Property(t => t.ClientModuleName).HasMaxLength(120).IsUnicode(true);
            this.Property(t => t.ServerModuleName).HasMaxLength(120).IsUnicode(true);
            this.Property(t => t.NewWizardComponentPath).HasMaxLength(250).IsUnicode(false);
            this.Property(t => t.DownloadToExcelFeatureCode).HasMaxLength(120).IsUnicode(false);
            this.Property(t => t.DescriptionTextCodeCode).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.NewButtonTextCodeCode).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.SplitComponentPath).HasMaxLength(500).IsUnicode(true);
            this.Property(t => t.ParentObjectTableName).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ParentObjectTableId).HasMaxLength(15).IsUnicode(true);
            this.Property(t => t.IsCustom);
            this.Property(t => t.FullNameTextCodeId).HasMaxLength(60).IsUnicode(true);
            this.Property(t => t.FullNameTextCodeCode).HasMaxLength(200).IsUnicode(true);

            //this.Property(t => t.FilterMenuComponentPath).HasMaxLength(250).IsUnicode(false);
            //this.Property(t => t.ShortTitleComponentPath).HasMaxLength(250).IsUnicode(false);
            //this.Property(t => t.HelperComponentPath).HasMaxLength(250).IsUnicode(false);
            //this.Property(t => t.MenuButtonsComponentPath).HasMaxLength(250).IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("ObjectTables");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.IsNewWizard).HasColumnName("IsNewWizard");
            this.Property(t => t.NewWizardControlName).HasColumnName("NewWizardControlName");
            this.Property(t => t.LookUp1).HasColumnName("LookUp1");
            this.Property(t => t.LookUp2).HasColumnName("LookUp2");
            this.Property(t => t.DependencyFilter1).HasColumnName("DependencyFilter1");
            this.Property(t => t.DependencyFilter2).HasColumnName("DependencyFilter2");
            this.Property(t => t.DependencyFilter3).HasColumnName("DependencyFilter3");
            this.Property(t => t.KeyPropertyPath).HasColumnName("KeyPropertyPath");
            this.Property(t => t.AutoCompleteSearchWindow).HasColumnName("AutoCompleteSearchWindow");
            this.Property(t => t.IsClosed).HasColumnName("IsClosed");
            this.Property(t => t.HeaderScreenId).HasColumnName("HeaderScreenId");
            this.Property(t => t.HeaderScreenCode).HasColumnName("HeaderScreenCode");

            this.Property(t => t.CacheOnClient).HasColumnName("CacheOnClient");
            this.Property(t => t.EditableFromAutoCompleteWindow).HasColumnName("EditableFromAutoCompleteWindow");
            this.Property(t => t.LastUpdateDate).HasColumnName("LastUpdateDate");
            this.Property(t => t.HasCounter).HasColumnName("HasCounter");
            this.Property(t => t.EnableEditFromLOV).HasColumnName("EnableEditFromLOV");
            this.Property(t => t.EnableAddFromLOV).HasColumnName("EnableAddFromLOV");
            this.Property(t => t.IsRestrictable).HasColumnName("IsRestrictable");
            this.Property(t => t.IsMain).HasColumnName("IsMain");
            this.Property(t => t.IsAutoComplete).HasColumnName("IsAutoComplete");
            this.Property(t => t.SortingByObjectField).HasColumnName("SortingByObjectField");
            this.Property(t => t.DBTableName).HasColumnName("DBTableName");
            this.Property(t => t.HasCustomFields).HasColumnName("HasCustomFields");
            this.Property(t => t.CustomFieldsCount).HasColumnName("CustomFieldsCount");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.DescriptionTextCodeId).HasColumnName("DescriptionTextCodeId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.IsSaveButtonVisible).HasColumnName("IsSaveButtonVisible");
            this.Property(t => t.MainTipCode).HasColumnName("MainTipCode");
            this.Property(t => t.EnableSecurity).HasColumnName("EnableSecurity");
            this.Property(t => t.ObjectTableTypeCode).HasColumnName("ObjectTableTypeCode");
            this.Property(t => t.IsComposition).HasColumnName("IsComposition");
            this.Property(t => t.MaxNumberOfCustomFields).HasColumnName("MaxNumberOfCustomFields");
            this.Property(t => t.AllowCustomFields).HasColumnName("AllowCustomFields");
            this.Property(t => t.HasDynamicHeader).HasColumnName("HasDynamicHeader");
            this.Property(t => t.IsLookUp).HasColumnName("IsLookUp");
            this.Property(t => t.HasDocuments).HasColumnName("HasDocuments");
            this.Property(t => t.NewButtonTextCodeId).HasColumnName("NewButtonTextCodeId");
            this.Property(t => t.HasCustomValidator).HasColumnName("HasCustomValidator");
            this.Property(t => t.EntityResource).HasColumnName("EntityResource");
            this.Property(t => t.HasCustomFilter).HasColumnName("HasCustomFilter");
            this.Property(t => t.ClientModuleName).HasColumnName("ClientModuleName");
            this.Property(t => t.ServerModuleName).HasColumnName("ServerModuleName");
            this.Property(t => t.NewWizardComponentPath).HasColumnName("NewWizardComponentPath");
            this.Property(t => t.HasHelper).HasColumnName("HasHelper");
            this.Property(t => t.HasShortTitle).HasColumnName("HasShortTitle");
            this.Property(t => t.HasMenuButtons).HasColumnName("HasMenuButtons");
            this.Property(t => t.HasFiltersMenu).HasColumnName("HasFiltersMenu");
            this.Property(t => t.EntityResourceLastUpdate).HasColumnName("EntityResourceLastUpdate");
            this.Property(t => t.DownloadToExcelFeatureCode).HasColumnName("DownloadToExcelFeatureCode");
            this.Property(t => t.AllowedForComputingPartners).HasColumnName("AllowedForComputingPartners");
            this.Property(t => t.CodeField).HasColumnName("CodeField");            
            this.Property(t => t.SplitComponentPath).HasColumnName("SplitComponentPath");
            this.Property(t => t.DisableSearchBox).HasColumnName("DisableSearchBox");
            this.Property(t => t.AllowedInTicket).HasColumnName("AllowedInTicket");
            this.Property(t => t.IsTabsHidden).HasColumnName("IsTabsHidden");
            this.Property(t => t.DescriptionTextCodeCode).HasColumnName("DescriptionTextCodeCode");
            this.Property(t => t.NewButtonTextCodeCode).HasColumnName("NewButtonTextCodeCode");
            this.Property(t => t.ParentObjectTableName).HasColumnName("ParentObjectTableName");
            this.Property(t => t.AvailableInCustomization).HasColumnName("AvailableInCustomization");
            this.Property(t => t.ParentObjectTableId).HasColumnName("ParentObjectTableId");
            this.Property(t => t.IsCustom).HasColumnName("IsCustom");
            this.Property(t => t.SupportSubEntity).HasColumnName("SupportSubEntity");
            this.Property(t => t.ApplyGenericCustomFields).HasColumnName("ApplyGenericCustomFields");
            this.Property(t => t.FullNameTextCodeId).HasColumnName("FullNameTextCodeId");
            this.Property(t => t.FullNameTextCodeCode).HasColumnName("FullNameTextCodeCode");
            this.Property(t => t.AvailableInDocumentTypes).HasColumnName("AvailableInDocumentTypes");

            // Relationships
            this.HasOptional(t => t.DescriptionTextCode).WithMany().HasForeignKey(d => d.DescriptionTextCodeId);
            this.HasOptional(t => t.MainTip).WithMany().HasForeignKey(d => d.MainTipCode);
            this.HasOptional(t => t.HeaderScreen).WithMany().HasForeignKey(d => d.HeaderScreenId);

        }
    }
}
