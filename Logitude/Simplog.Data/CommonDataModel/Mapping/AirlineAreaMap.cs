using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AirlineAreaMap : EntityTypeConfiguration<AirlineArea>
    {
        public AirlineAreaMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Name).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.CreatedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AirlineId).HasMaxLength(15).IsUnicode(false);


            this.ToTable("AirlineAreas");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.AirlineId).HasColumnName("AirlineId");




            this.HasOptional(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasOptional(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
            this.HasRequired(t => t.Airline).WithMany().HasForeignKey(d => d.AirlineId).WillCascadeOnDelete(false);

        }
    }
}
