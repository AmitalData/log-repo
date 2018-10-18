using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class AWBOCIMap : EntityTypeConfiguration<AWBOCI>
    {
        public AWBOCIMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CountryId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AWBCustomsInformationCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.AWBInformationCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.SupplementaryCustomsInfo).HasMaxLength(35).IsRequired().IsUnicode(false);

            this.ToTable("AWBOCIs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.AWBCustomsInformationCode).HasColumnName("AWBCustomsInformationCode");
            this.Property(t => t.AWBInformationCode).HasColumnName("AWBInformationCode");
            this.Property(t => t.SupplementaryCustomsInfo).HasColumnName("SupplementaryCustomsInfo");

            this.HasOptional(t => t.Shipment).WithMany().HasForeignKey(d => d.ShipmentId);
            this.HasOptional(t => t.Country).WithMany().HasForeignKey(d => d.CountryId);
            this.HasOptional(t => t.AWBCustomsInformation).WithMany().HasForeignKey(d => d.AWBCustomsInformationCode);
            this.HasOptional(t => t.AWBInformation).WithMany().HasForeignKey(d => d.AWBInformationCode);
        }
    }
}
