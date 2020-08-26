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
            this.Property(t => t.Name).IsRequired().HasMaxLength(100).IsUnicode(false);






            this.Property(t => t.CreatedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CarrierId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TransportModeCode).IsRequired().HasMaxLength(1).IsUnicode(false);

            this.ToTable("Horses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.CarrierId).HasColumnName("CarrierId");
            this.Property(t => t.TransportModeCode).HasColumnName("TransportModeCode");

            this.HasOptional(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasOptional(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
            this.HasRequired(t => t.CountryOfBirth).WithMany().HasForeignKey(d => d.CountryOfBirthId);
        }
    }
}