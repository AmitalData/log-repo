using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class AWBSpecialHandlingCodeMap : EntityTypeConfiguration<AWBSpecialHandlingCode>
    {
        public AWBSpecialHandlingCodeMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Code).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(400).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.IsIATA).IsRequired();
            this.Property(t => t.InActive).IsRequired();
            this.Property(t => t.AirlineId).HasMaxLength(15).IsUnicode(false);


            this.ToTable("AWBSpecialHandlingCodes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.IsIATA).HasColumnName("IsIATA");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.AirlineId).HasColumnName("AirlineId");

            this.HasOptional(t => t.Airline).WithMany().HasForeignKey(d => d.AirlineId);
        }
    }
}
