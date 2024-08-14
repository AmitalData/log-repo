using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AddressTypeMap : EntityTypeConfiguration<AddressType>
    {
        public AddressTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("AddressTypes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}
