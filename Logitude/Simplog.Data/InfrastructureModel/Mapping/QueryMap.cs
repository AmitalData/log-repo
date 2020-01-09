using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class QueryMap : EntityTypeConfiguration<Query>
    {
        public QueryMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Code).IsRequired().HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.UserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ObjectTableId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.OriginalQueryId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.QuerySection).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.QueryGroupCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.NameTextCodeId).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.DefaultSortColumn).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.DefaultSortDirection).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.SpotlightDataTemplate).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.FeatureId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EditWizardName).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Perspective).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.EditWizardComponentPath).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.NameTextCodeCode).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.FeatureUniqeCode)
                .HasMaxLength(120)
                .IsUnicode(false);
            // Table & Column Mappings
            this.ToTable("Queries");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.SystemLevel).HasColumnName("SystemLevel");
            this.Property(t => t.TenantLevel).HasColumnName("TenantLevel");
            this.Property(t => t.OriginalQueryId).HasColumnName("OriginalQueryId");
            this.Property(t => t.QuerySection).HasColumnName("QuerySection");
            this.Property(t => t.IndexOrder).HasColumnName("IndexOrder");
            this.Property(t => t.DisplayCount).HasColumnName("DisplayCount");
            this.Property(t => t.IsAddNewEntityEnabled).HasColumnName("IsAddNewEntityEnabled");
            this.Property(t => t.QueryGroupCode).HasColumnName("QueryGroupCode");
            this.Property(t => t.NameTextCodeId).HasColumnName("NameTextCodeId");
            this.Property(t => t.NameTextCodeCode).HasColumnName("NameTextCodeCode");
            this.Property(t => t.DefaultSortColumn).HasColumnName("DefaultSortColumn");
            this.Property(t => t.DefaultSortDirection).HasColumnName("DefaultSortDirection");
            this.Property(t => t.SpotlightDataTemplate).HasColumnName("SpotlightDataTemplate");
            this.Property(t => t.Internal).HasColumnName("Internal");
            this.Property(t => t.Customer).HasColumnName("Customer");
            this.Property(t => t.Agent).HasColumnName("Agent");
            this.Property(t => t.FeatureId).HasColumnName("FeatureId");
            this.Property(t => t.EditWizardName).HasColumnName("EditWizardName");
            this.Property(t => t.Perspective).HasColumnName("Perspective");
            this.Property(t => t.IsHiddenFromView).HasColumnName("IsHiddenFromView");
            this.Property(t => t.IsNewFromTenantZeroOnly).HasColumnName("IsNewFromTenantZeroOnly");
            this.Property(t => t.EditWizardComponentPath).HasColumnName("EditWizardComponentPath");
            this.Property(t => t.SharedWithAll).HasColumnName("SharedWithAll");
            this.Property(t => t.SharedWithSpecificUsers).HasColumnName("SharedWithSpecificUsers");
            this.Property(t => t.SharedByUserId).HasColumnName("SharedByUserId");
            this.Property(t => t.SpotlightModeActivated).HasColumnName("SpotlightModeActivated");
            this.Property(t => t.FeatureUniqeCode).HasColumnName("FeatureUniqeCode");

            // Relationships
            //this.HasOptional(t => t.Feature).WithMany().HasForeignKey(d => d.FeatureId);
            this.HasOptional(t => t.OriginalQuery).WithMany(t => t.CopiedQueries).HasForeignKey(d => d.OriginalQueryId);
            this.HasOptional(t => t.User).WithMany().HasForeignKey(d => d.UserId);
            this.HasOptional(t => t.SharedByUser).WithMany().HasForeignKey(d => d.SharedByUserId);
        }
    }
}
