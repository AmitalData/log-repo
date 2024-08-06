using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class IncotermMap : EntityTypeConfiguration<Incoterm>
    {
        public IncotermMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(80)
                .IsUnicode(true);

            this.Property(t => t.Notes)
                .HasMaxLength(250)
                .IsUnicode(true);

            this.Property(t => t.LocalName)
                .HasMaxLength(40)
                .IsUnicode(true);

            this.Property(t => t.Freight)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false);

            this.Property(t => t.OtherCharges)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("Incoterms");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.Freight).HasColumnName("Freight");
            this.Property(t => t.OtherCharges).HasColumnName("OtherCharges");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.AutomaticLastUpdateDate).HasColumnName("AutomaticLastUpdateDate"); 

            // Relationships
            //this.HasRequired(t => t.FreightPrepaidCollect)
            //    .WithMany(t => t.FreightIncoterms)
            //    .HasForeignKey(d => d.Freight);
            //this.HasRequired(t => t.OtherChargesPrepaidCollect)
            //    .WithMany(t => t.OtherChargesIncoterms)
            //    .HasForeignKey(d => d.OtherCharges)
            //    .WillCascadeOnDelete(false);

        }
    }
}
