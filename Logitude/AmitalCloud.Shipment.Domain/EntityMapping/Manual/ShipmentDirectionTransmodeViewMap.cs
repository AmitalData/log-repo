using AmitalCloud.Shipment.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Shipment.Domain.EntityMapping
{
    public class ShipmentDirectionTransmodeViewMap : EntityTypeConfiguration<ShipmentDirectionTransmodeView>
    {
        public ShipmentDirectionTransmodeViewMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.Tenant, t.ShipmentNumber, t.ProfitInLocalCurrency, t.AccountedReceivablesInLocalCurrency, t.OpenReceivablesInLocalCurrency, t.IsCancelled, t.DirectionId, t.TransportModeId, t.CreateDateTime, t.BranchId,});

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Tenant)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ShipmentNumber)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ShipmentLevelCode)
                .HasMaxLength(1)
                .IsUnicode(false);

       

            this.Property(t => t.CustomerId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DirectionId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1)
                .IsUnicode(false);

            this.Property(t => t.TransportModeId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1)
                .IsUnicode(false);

            this.Property(t => t.BranchId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DirectionName)
                .HasMaxLength(10)
                   .IsUnicode(false);

            this.Property(t => t.TransportModeName)
                .HasMaxLength(10)
                .IsUnicode(false);

           


            // Table & Column Mappings
            this.ToTable("ShipmentDirectionTransmodeView");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentNumber).HasColumnName("ShipmentNumber");
            this.Property(t => t.AccountedReceivablesInProfitCurrency).HasColumnName("AccountedReceivablesInProfitCurrency");
            this.Property(t => t.OpenReceivablesInProfitCurrency).HasColumnName("OpenReceivablesInProfitCurrency");
            this.Property(t => t.ProfitInProfitCurrency).HasColumnName("ProfitInProfitCurrency");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.ProfitInLocalCurrency).HasColumnName("ProfitInLocalCurrency");
            this.Property(t => t.AccountedReceivablesInLocalCurrency).HasColumnName("AccountedReceivablesInLocalCurrency");
            this.Property(t => t.OpenReceivablesInLocalCurrency).HasColumnName("OpenReceivablesInLocalCurrency");
            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");
            this.Property(t => t.DirectionId).HasColumnName("DirectionId");
            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId");
            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.DirectionName).HasColumnName("DirectionName");
            this.Property(t => t.TransportModeName).HasColumnName("TransportModeName");
            this.Property(t => t.ChargeableWeightInKG).HasColumnName("ChargeableWeightInKG");
            this.Property(t => t.GrossWeightInKG).HasColumnName("GrossWeightInKG");
           
        }
    }
}
