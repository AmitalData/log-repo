using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
   public class HorseMap : EntityTypeConfiguration<Horse>
    {
        public HorseMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(200).IsUnicode(false);
            this.Property(t => t.Color).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.Gender).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.Breed).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.Discipline).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.TravelBehavior).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.MicochipNumber).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.PassportNumber).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.CountryOfBirthId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CurrentStable).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Owner).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Remarks).HasMaxLength(500).IsUnicode(true);
            this.Property(t => t.CreatedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            this.ToTable("Horses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.YearOfBirth).HasColumnName("YearOfBirth");
            this.Property(t => t.Color).HasColumnName("Color");
            this.Property(t => t.Gender).HasColumnName("Gender");
            this.Property(t => t.Breed).HasColumnName("Breed");
            this.Property(t => t.Discipline).HasColumnName("Discipline");
            this.Property(t => t.TravelBehavior).HasColumnName("TravelBehavior");
            this.Property(t => t.MicochipNumber).HasColumnName("MicochipNumber");
            this.Property(t => t.PassportNumber).HasColumnName("PassportNumber");
            this.Property(t => t.CountryOfBirthId).HasColumnName("CountryOfBirthId");
            this.Property(t => t.CurrentStable).HasColumnName("CurrentStable");
            this.Property(t => t.Owner).HasColumnName("Owner");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.Inactive).HasColumnName("Inactive");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");            
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");

            this.HasOptional(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasOptional(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
            this.HasOptional(t => t.CountryOfBirth).WithMany().HasForeignKey(d => d.CountryOfBirthId);
        }
    }
}