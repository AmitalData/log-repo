using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentAnalyticMap : EntityTypeConfiguration<ShipmentAnalytic>
    {
        public ShipmentAnalyticMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentNumber).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.BranchId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.IncotermId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SalesmanUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AccountManagerUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentTypeId).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.TransportModeId).IsRequired().IsFixedLength().HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.DirectionId).IsRequired().IsFixedLength().HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.ShipmentLevelCode).HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.ShipperId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ConsigneeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AgentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CustomerId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.StatusId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageFromPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageToPortId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentSubTypeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.MainCarriageCarrierId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FromCountryId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ToCountryId).HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");
            this.Property(t => t.ShipmentNumber).HasColumnName("ShipmentNumber");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.IncotermId).HasColumnName("IncotermId");
            this.Property(t => t.SalesmanUserId).HasColumnName("SalesmanUserId");
            this.Property(t => t.AccountManagerUserId).HasColumnName("AccountManagerUserId");
            this.Property(t => t.ShipmentTypeId).HasColumnName("ShipmentTypeId");
            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId");
            this.Property(t => t.DirectionId).HasColumnName("DirectionId");
            this.Property(t => t.ShipmentLevelCode).HasColumnName("ShipmentLevelCode");
            this.Property(t => t.ShipperId).HasColumnName("ShipperId");
            this.Property(t => t.ConsigneeId).HasColumnName("ConsigneeId");
            this.Property(t => t.AgentId).HasColumnName("AgentId");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.IsOperationalClosed).HasColumnName("IsOperationalClosed");
            this.Property(t => t.IsAccountingClosed).HasColumnName("IsAccountingClosed");
            this.Property(t => t.GrossWeightInKG).HasColumnName("GrossWeightInKG");
            this.Property(t => t.ChargeableWeightInKG).HasColumnName("ChargeableWeightInKG");
            this.Property(t => t.VolumeInCBM).HasColumnName("VolumeInCBM");
            this.Property(t => t.TEU).HasColumnName("TEU");
            this.Property(t => t.StatusId).HasColumnName("StatusId");
            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");
            this.Property(t => t.OpenReceivablesInLocalCurrency).HasColumnName("OpenReceivablesInLocalCurrency").IsRequired();
            this.Property(t => t.AccountedReceivablesInLocalCurrency).HasColumnName("AccountedReceivablesInLocalCurrency").IsRequired();
            this.Property(t => t.ProfitInLocalCurrency).HasColumnName("ProfitInLocalCurrency");
            this.Property(t => t.ProfitInProfitCurrency).HasColumnName("ProfitInProfitCurrency");
            this.Property(t => t.OpenReceivablesInProfitCurrency).HasColumnName("OpenReceivablesInProfitCurrency");
            this.Property(t => t.AccountedReceivablesInProfitCurrency).HasColumnName("AccountedReceivablesInProfitCurrency");
            this.Property(t => t.MainCarriageFromPortId).HasColumnName("MainCarriageFromPortId");
            this.Property(t => t.MainCarriageToPortId).HasColumnName("MainCarriageToPortId");
            this.Property(t => t.OpenPayablesInLocalCurrency).HasColumnName("OpenPayablesInLocalCurrency");
            this.Property(t => t.AccountedPayablesInLocalCurrency).HasColumnName("AccountedPayablesInLocalCurrency");
            this.Property(t => t.OpenPayablesInProfitCurrency).HasColumnName("OpenPayablesInProfitCurrency");
            this.Property(t => t.AccountedPayablesInProfitCurrency).HasColumnName("AccountedPayablesInProfitCurrency");
            this.Property(t => t.ARInvoiceIssued).HasColumnName("ARInvoiceIssued");
            this.Property(t => t.ShipmentSubTypeId).HasColumnName("ShipmentSubTypeId");
            this.Property(t => t.MainCarriageCarrierId).HasColumnName("MainCarriageCarrierId");
            this.Property(t => t.NumberOfPackages).HasColumnName("NumberOfPackages");
            this.Property(t => t.NumberOfContainers).HasColumnName("NumberOfContainers");
            this.Property(t => t.FromCountryId).HasColumnName("FromCountryId");
            this.Property(t => t.ToCountryId).HasColumnName("ToCountryId");
            this.Property(t => t.OperationalDate).HasColumnName("OperationalDate");
        }
    }
}
