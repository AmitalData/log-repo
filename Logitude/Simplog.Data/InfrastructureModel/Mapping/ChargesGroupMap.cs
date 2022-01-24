using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.Infrastructure.Annotations;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class ChargesGroupMap : EntityTypeConfiguration<ChargesGroup>
    {
        public ChargesGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);


            this.Property(t => t.Tenant);

            // Properties

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(5)
                .IsUnicode(false);
               


            this.Property(t => t.LocalName)
            .HasMaxLength(40)
            .IsUnicode(true);

   
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("ChargesGroups");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.ViewOrder).HasColumnName("ViewOrder");
            this.Property(t => t.QuoteGroupSectionID).HasColumnName("QuoteGroupSectionID").HasMaxLength(1);


        }
    }
}
