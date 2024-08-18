using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    class CustomerGroupMap : EntityTypeConfiguration<CustomerGroup>
    {
        public CustomerGroupMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("CustomerGroups");
            this.Property(t => t.Id).HasColumnName("Id");

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.Description).HasColumnName("Description").IsMaxLength().IsUnicode(true);

            this.Property(t => t.InActive).HasColumnName("InActive").IsRequired();
        }
    }
}
