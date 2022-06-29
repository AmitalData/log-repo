using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class TabModificationMap : EntityTypeConfiguration<TabModification>
    {
        public TabModificationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TabId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TabCode)
               .HasMaxLength(100)
               .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("TabsModifications");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.TabId).HasColumnName("TabId");
            this.Property(t => t.TabCode).HasColumnName("TabCode");

        }
    }
}
