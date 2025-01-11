using AmitalCloud.Shipment.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Shipment.Domain.EntityMapping
{
    public class ShipmentContainerStatusMap : EntityTypeConfiguration<ShipmentContainerStatus>
    {
        public ShipmentContainerStatusMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.StatusCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.Details).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.FromPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ToPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VoyageNumber).HasMaxLength(35).IsUnicode(false);
            this.Property(t => t.VesselName).HasMaxLength(35).IsUnicode(false);
            this.Property(t => t.RecordHash).IsRequired().HasMaxLength(500).IsUnicode(true);
            this.Property(t => t.Location).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TimeOfDepartureInfo).HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.TimeOfArrivalInfo).HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.ShippingLineName).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.ContainerId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ContainerNumber).HasMaxLength(20).IsUnicode(false);

            this.ToTable("ShipmentContainerStatuses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.StatusSource).HasColumnName("StatusSource");
            this.Property(t => t.ContainerStatusCode).HasColumnName("ContainerStatusCode");
            this.Property(t => t.ReceivingDate).HasColumnName("ReceivingDate");
            this.Property(t => t.Details).HasColumnName("Details");
            this.Property(t => t.FromPortId).HasColumnName("FromPortId");
            this.Property(t => t.ToPortId).HasColumnName("ToPortId");
            this.Property(t => t.VoyageNumber).HasColumnName("VoyageNumber");
            this.Property(t => t.Pieces).HasColumnName("Pieces");
            this.Property(t => t.Partial).HasColumnName("Partial");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.RecordHash).HasColumnName("RecordHash");
            this.Property(t => t.EventDate).HasColumnName("EventDate");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.ShippingLineName).HasColumnName("ShippingLineName");
            this.Property(t => t.TimeOfArrivalInfo).HasColumnName("TimeOfArrivalInfo");
            this.Property(t => t.TimeOfDepartureInfo).HasColumnName("TimeOfDepartureInfo");
            this.Property(t => t.DepartureDate).HasColumnName("DepartureDate");
            this.Property(t => t.ArrivalDate).HasColumnName("ArrivalDate");
            this.Property(t => t.ContainerId).HasColumnName("ContainerId");
            this.Property(t => t.ContainerNumber).HasColumnName("ContainerNumber");
            this.Property(t => t.VesselName).HasColumnName("VesselName");

            this.HasRequired(t => t.INTTRAStatus).WithMany().HasForeignKey(d => d.StatusCode);
            this.HasRequired(t => t.ContainerStatusSource).WithMany().HasForeignKey(d => d.StatusSource);
            this.HasRequired(t => t.ContainerStatus).WithMany().HasForeignKey(d => d.ContainerStatusCode);
            this.HasOptional(t => t.FromPort).WithMany().HasForeignKey(d => d.FromPortId);
            this.HasOptional(t => t.LocationPort).WithMany().HasForeignKey(d => d.Location);
            this.HasOptional(t => t.ToPort).WithMany().HasForeignKey(d => d.ToPortId);
            this.HasOptional(t => t.Container).WithMany().HasForeignKey(d => d.ContainerId);

        }

    }
}
