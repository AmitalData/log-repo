using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentCarrierStatusMap : EntityTypeConfiguration<ShipmentCarrierStatus>
    {
        public ShipmentCarrierStatusMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ShipmentId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.Details)
                .HasMaxLength(250)
                .IsUnicode(true);

            this.Property(t => t.FromPortId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ToPortId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.FlightNumber)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.RecordHash)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(true);

            this.Property(t => t.Location)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TimeOfDepartureInfo)
                      .HasMaxLength(1)
                      .IsUnicode(false);

            this.Property(t => t.TimeOfArrivalInfo)
                     .HasMaxLength(1)
                     .IsUnicode(false);
               

            this.Property(t => t.AirlineName)
         
          .HasMaxLength(60)
          .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentCarrierStatuses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ReceivingDate).HasColumnName("ReceivingDate");
            this.Property(t => t.Details).HasColumnName("Details");
            this.Property(t => t.FromPortId).HasColumnName("FromPortId");
            this.Property(t => t.ToPortId).HasColumnName("ToPortId");
            this.Property(t => t.FlightNumber).HasColumnName("FlightNumber");
            this.Property(t => t.Pieces).HasColumnName("Pieces");
            this.Property(t => t.Partial).HasColumnName("Partial");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.RecordHash).HasColumnName("RecordHash");
            this.Property(t => t.EventDate).HasColumnName("EventDate");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.AirlineName).HasColumnName("AirlineName");
            this.Property(t => t.TimeOfArrivalInfo).HasColumnName("TimeOfArrivalInfo");
            this.Property(t => t.TimeOfDepartureInfo).HasColumnName("TimeOfDepartureInfo");
            this.Property(t => t.DepartureDate).HasColumnName("DepartureDate");
            this.Property(t => t.ArrivalDate).HasColumnName("ArrivalDate");

            // Relationships
            this.HasRequired(t => t.AWBStatus)
                .WithMany()
                .HasForeignKey(d => d.Status);
            this.HasOptional(t => t.FromPort)
                .WithMany()
                .HasForeignKey(d => d.FromPortId);
            this.HasOptional(t => t.LocationPort)
                .WithMany()
                .HasForeignKey(d => d.Location);
            this.HasOptional(t => t.ToPort)
                .WithMany()
                .HasForeignKey(d => d.ToPortId);

        }
    }
}
