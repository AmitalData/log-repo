using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AirlineStatisticsMap: EntityTypeConfiguration<AirlineStatistics>
    {
        public AirlineStatisticsMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SourceTenantName).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.ShipmentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentLevelCode).HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.BookingId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EntityReference).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.AWBNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.HWBNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.AirlineCode).IsRequired().HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.EntityCreatedByUserName).IsRequired().HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.MessageType).IsRequired().HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.EntityStatus).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.ChargeableWeightUnitCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.GrossWeightUnitCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.VolumeUnitCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.OriginCode).IsRequired().HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.DestinationCode).IsRequired().HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.DescriptionOfGoods).HasMaxLength(512).IsUnicode(false);
            this.Property(t => t.ShipperName).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.ConsigneeName).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.Flight1).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Flight2).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Flight3).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.OnCarriageTo).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.PreCarriageFrom).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.Allotment).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.AirlinePrefix).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ProductName).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.Sender).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.MessagingStatus).HasMaxLength(60).IsUnicode(false);

            this.ToTable("AirlineStatistics");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.SourceTenant).HasColumnName("SourceTenant");
            this.Property(t => t.SourceTenantName).HasColumnName("SourceTenantName");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.ShipmentLevelCode).HasColumnName("ShipmentLevelCode");
            this.Property(t => t.BookingId).HasColumnName("BookingId");
            this.Property(t => t.EntityReference).HasColumnName("EntityReference");
            this.Property(t => t.AWBNumber).HasColumnName("AWBNumber");
            this.Property(t => t.HWBNumber).HasColumnName("HWBNumber");
            this.Property(t => t.AirlineCode).HasColumnName("AirlineCode");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.EntitiyCreateDate).HasColumnName("EntitiyCreateDate");
            this.Property(t => t.EntitiyUpdateDate).HasColumnName("EntitiyUpdateDate");
            this.Property(t => t.EntityCreatedByUserName).HasColumnName("EntityCreatedByUserName");
            this.Property(t => t.MessageType).HasColumnName("MessageType");
            this.Property(t => t.LastSentDate).HasColumnName("LastSentDate");
            this.Property(t => t.EntityStatus).HasColumnName("EntityStatus");
            this.Property(t => t.NumberOfPackages).HasColumnName("NumberOfPackages");
            this.Property(t => t.ChargeableWeight).HasColumnName("ChargeableWeight");
            this.Property(t => t.ChargeableWeightUnitCode).HasColumnName("ChargeableWeightUnitCode");
            this.Property(t => t.GrossWeight).HasColumnName("GrossWeight");
            this.Property(t => t.GrossWeightUnitCode).HasColumnName("GrossWeightUnitCode");
            this.Property(t => t.Volume).HasColumnName("Volume");
            this.Property(t => t.VolumeUnitCode).HasColumnName("VolumeUnitCode");
            this.Property(t => t.OriginCode).HasColumnName("OriginCode");
            this.Property(t => t.DestinationCode).HasColumnName("DestinationCode");
            this.Property(t => t.DescriptionOfGoods).HasColumnName("DescriptionOfGoods");
            this.Property(t => t.ShipperName).HasColumnName("ShipperName");
            this.Property(t => t.ConsigneeName).HasColumnName("ConsigneeName");
            this.Property(t => t.Flight1).HasColumnName("Flight1");
            this.Property(t => t.Flight1Date).HasColumnName("Flight1Date");
            this.Property(t => t.Flight2).HasColumnName("Flight2");
            this.Property(t => t.Flight2Date).HasColumnName("Flight2Date");
            this.Property(t => t.Flight3).HasColumnName("Flight3");
            this.Property(t => t.Flight3Date).HasColumnName("Flight3Date");
            this.Property(t => t.OnCarriageTo).HasColumnName("OnCarriageTo");
            this.Property(t => t.OnCarriageDate).HasColumnName("OnCarriageDate");
            this.Property(t => t.PreCarriageFrom).HasColumnName("PreCarriageFrom");
            this.Property(t => t.PreCarriageDate).HasColumnName("PreCarriageDate");
            this.Property(t => t.Allotment).HasColumnName("Allotment");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");
            this.Property(t => t.AirlinePrefix).HasColumnName("AirlinePrefix");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.Sender).HasColumnName("Sender");
            this.Property(t => t.Direct).HasColumnName("Direct");
            this.Property(t => t.MessagingStatus).HasColumnName("MessagingStatus");

            this.HasOptional(t => t.ShipmentLevel).WithMany().HasForeignKey(d => d.ShipmentLevelCode);
        }
    }
}
