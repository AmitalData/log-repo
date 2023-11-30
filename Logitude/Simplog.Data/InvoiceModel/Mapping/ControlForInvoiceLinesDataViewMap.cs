using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    class ControlForInvoiceLinesDataViewMap : EntityTypeConfiguration<ControlForInvoiceLinesDataView>
    {
        public ControlForInvoiceLinesDataViewMap()
        {
			// Primary Key
			this.HasKey(t => new { t.Tenant, t.InvoiceNumber});

			this.Property(t => t.InvoiceNumber)
				.HasMaxLength(20)
				.IsUnicode(false);

			// Table & Column Mappings
			this.ToTable("ControlForInvoiceLinesDataView");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.InvoiceDate).HasColumnName("InvoiceDate");
            this.Property(t => t.InvoiceNumber).HasColumnName("InvoiceNumber");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.LineActionCode).HasColumnName("LineActionCode");
            this.Property(t => t.VatPercentage).HasColumnName("VatPercentage");
            this.Property(t => t.LocalCurrencyAmount).HasColumnName("LocalCurrencyAmount");

            this.Property(t => t.AccountTypeCode).HasColumnName("AccountTypeCode");
            this.Property(t => t.Displaynumber).HasColumnName("Displaynumber");
            this.Property(t => t.LocalName1).HasColumnName("LocalName1");
            this.Property(t => t.LocalName2).HasColumnName("LocalName2");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.TypeCode).HasColumnName("TypeCode");
            this.Property(t => t.LocalName3).HasColumnName("LocalName3");
            this.Property(t => t.TaxReportMonth).HasColumnName("TaxReportMonth");


                 
        }
    }
}
