using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class RestrictionMap : EntityTypeConfiguration<Restriction>
    {
        public RestrictionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ObjectFieldId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ContactTenantId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("Restrictions");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.ObjectFieldId).HasColumnName("ObjectFieldId");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.ContactTenantId).HasColumnName("ContactTenantId");

            // Relationships
            this.HasRequired(t => t.ContactTenant)
                .WithMany()
                .HasForeignKey(d => d.ContactTenantId);
            //this.HasRequired(t => t.ObjectField)
            //    .WithMany(t => t.Restrictions)
            //    .HasForeignKey(d => d.ObjectFieldId);
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.Restrictions)
            //    .HasForeignKey(d => d.ObjectTableId);

        }
    }
}
