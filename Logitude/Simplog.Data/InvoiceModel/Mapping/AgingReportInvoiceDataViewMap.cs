using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class AgingReportInvoiceDataViewMap : EntityTypeConfiguration<AgingReportInvoiceDataView>
    {
        public AgingReportInvoiceDataViewMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.Tenant, t.CurrentDate, t.DueDate, t.StatusCode, t.BillToId });

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.Tenant)
                .HasDatabaseGeneratedOption(null);

            this.Property(t => t.InvoiceNumber)
                .HasMaxLength(20);

            this.Property(t => t.DateRange)
                .HasMaxLength(40);

            this.Property(t => t.StatusCode)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.BillToId)
                .IsRequired()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("AgingReportInvoiceDataView");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.InvoiceNumber).HasColumnName("InvoiceNumber");
            this.Property(t => t.CurrentDate).HasColumnName("CurrentDate");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.DateRange).HasColumnName("DateRange");
            this.Property(t => t.AmountDueInLocalCurrency).HasColumnName("AmountDueInLocalCurrency");
            this.Property(t => t.AmountDueInProfitCurrency).HasColumnName("AmountDueInProfitCurrency");
            this.Property(t => t.IndexOrder).HasColumnName("IndexOrder");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.BillToId).HasColumnName("BillToId");
        }
    }
}
