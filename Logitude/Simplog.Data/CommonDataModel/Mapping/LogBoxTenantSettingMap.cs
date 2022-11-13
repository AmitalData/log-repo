using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class LogBoxTenantSettingMap : EntityTypeConfiguration<LogBoxTenantSetting>
    {
        public LogBoxTenantSettingMap()
        {
            this.HasKey(t => t.Id);
           // this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.IsDocumentsArchive).IsRequired();
            this.Property(t => t.LogBoxAdminUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AutoArchiveOnInvoice).IsRequired();
            this.Property(t => t.StockTypeCode).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AutoArchiveOnInvoice).IsRequired();
            this.Property(t => t.CustomerTenantShareImportFile).IsRequired();
            this.Property(t => t.ShowTaxAmountWarning).IsRequired();



            this.ToTable("LogBoxTenantSettings");
            this.Property(t => t.IsDocumentsArchive).HasColumnName("IsDocumentsArchive");
            this.Property(t => t.CustomerTenantShareImportFile).HasColumnName("CustomerTenantShareImportFile");
            this.Property(t => t.DocumentShareAsDefault).HasColumnName("DocumentShareAsDefault");
            this.Property(t => t.StockTypeCode).HasColumnName("StockTypeCode");
            
            this.Property(t => t.LogBoxAdminUserId).HasColumnName("LogBoxAdminUserId");
            this.Property(t => t.AutoArchiveOnInvoice).HasColumnName("AutoArchiveOnInvoice");
            this.Property(t => t.AutoArchiveOnPODExport).HasColumnName("AutoArchiveOnPODExport");

            this.HasRequired(t => t.Tenant);
        }
    }
}