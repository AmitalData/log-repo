using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class PortMap : EntityTypeConfiguration<Port>
    {
        public PortMap()
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
                .IsFixedLength()
                .HasMaxLength(3)
                .IsUnicode(false);

            this.Property(t => t.EnglishName)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.LocalName)
                .HasMaxLength(40)
                .IsUnicode(true);

            this.Property(t => t.Notes)
                .HasMaxLength(250)
                .IsUnicode(true);

            this.Property(t => t.CountryId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Field1)
                .HasMaxLength(250)
                .IsUnicode(true);

            this.Property(t => t.Field2)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field3)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field4)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field5)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field6)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field7)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field8)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field9)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.Field10)
                .HasMaxLength(250)
                 .IsUnicode(true);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                 .IsUnicode(true);

            this.Property(t => t.StateId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CombinedCode)
              .HasMaxLength(30)
              .IsUnicode(false);

            this.Property(t => t.StateName).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.StateCode).HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.CountryCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.CountryName).HasMaxLength(120).IsUnicode(false);
            this.Property(t => t.PortTimeZoneCode).HasMaxLength(150).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("Ports");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.IsOcean).HasColumnName("IsOcean");
            this.Property(t => t.IsAir).HasColumnName("IsAir");
            this.Property(t => t.IsInland).HasColumnName("IsInland");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.Latitude).HasColumnName("Latitude");
            this.Property(t => t.Longtitude).HasColumnName("Longtitude");
            this.Property(t => t.Field1).HasColumnName("Field1");
            this.Property(t => t.Field2).HasColumnName("Field2");
            this.Property(t => t.Field3).HasColumnName("Field3");
            this.Property(t => t.Field4).HasColumnName("Field4");
            this.Property(t => t.Field5).HasColumnName("Field5");
            this.Property(t => t.Field6).HasColumnName("Field6");
            this.Property(t => t.Field7).HasColumnName("Field7");
            this.Property(t => t.Field8).HasColumnName("Field8");
            this.Property(t => t.Field9).HasColumnName("Field9");
            this.Property(t => t.Field10).HasColumnName("Field10");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.StateId).HasColumnName("StateId");
            this.Property(t => t.CombinedCode).HasColumnName("CombinedCode");
            this.Property(t => t.StateName).HasColumnName("StateName");
            this.Property(t => t.AutomaticLastUpdateDate).HasColumnName("AutomaticLastUpdateDate");
            this.Property(t => t.CountryCode).HasColumnName("CountryCode");
            this.Property(t => t.CountryName).HasColumnName("CountryName");
            this.Property(t => t.StateCode).HasColumnName("StateCode");
            this.Property(t => t.PortTimeZoneCode).HasColumnName("PortTimeZoneCode");

            // Relationships
            this.HasRequired(t => t.Country)
                .WithMany()
                .HasForeignKey(d => d.CountryId);

            this.HasOptional(t => t.State)
                .WithMany()
                .HasForeignKey(d => d.StateId);

            this.HasOptional(t => t.PortTimeZone)
               .WithMany()
               .HasForeignKey(d => d.PortTimeZoneCode);
        }
    }
}
