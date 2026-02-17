using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AirlineAreasPortMap : EntityTypeConfiguration<AirlineAreasPort>
    {
        public AirlineAreasPortMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Name).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.AddedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AirlineAreaId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PortId).HasMaxLength(15).IsUnicode(false);


            this.ToTable("AirlineAreasPorts");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.AddedByUserId).HasColumnName("AddedByUserId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.AddedDate).HasColumnName("AddedDate");
            this.Property(t => t.AirlineAreaId).HasColumnName("AirlineAreaId");
            this.Property(t => t.PortId).HasColumnName("PortId");




            this.HasOptional(t => t.AddedByUser).WithMany().HasForeignKey(d => d.AddedByUserId);
            this.HasRequired(t => t.AirlineArea).WithMany().HasForeignKey(d => d.AirlineAreaId).WillCascadeOnDelete(false);
            this.HasOptional(t => t.Port).WithMany().HasForeignKey(d => d.PortId);

        }
    }
}
