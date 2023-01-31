using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class SATInterfaceSettingMap : EntityTypeConfiguration<SATInterfaceSetting>
    {
        public SATInterfaceSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Tenant);

            this.Property(t => t.Tenant).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SATInterfaceCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            this.Property(t => t.Token)
                .HasMaxLength(40)
                .IsUnicode(true);

            this.Property(t => t.MetodoPagoCode)
             .HasMaxLength(3)
             .IsUnicode(false);

            this.Property(t => t.SATCompanyName)
                .HasMaxLength(200)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("SATInterfaceSettings");

            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.SATInterfaceCode).HasColumnName("SATInterfaceCode");
            this.Property(t => t.Token).HasColumnName("Token");
            this.Property(t => t.ActivationDate).HasColumnName("ActivationDate");
            this.Property(t => t.MetodoPagoCode).HasColumnName("MetodoPagoCode");
            this.Property(t => t.IsARInvoiceTransferEnabled).HasColumnName("IsARInvoiceTransferEnabled");
            this.Property(t => t.IsCartaPorteTransferEnabled).HasColumnName("IsCartaPorteTransferEnabled");
            this.Property(t => t.SATCompanyName).HasColumnName("SATCompanyName");
            this.Property(t => t.TransferExpenseCharges).HasColumnName("TransferExpenseCharges");



            this.HasRequired(t => t.SATInterface).WithMany().HasForeignKey(d => d.SATInterfaceCode);
            this.HasOptional(t => t.MetodoPago).WithMany().HasForeignKey(d => d.MetodoPagoCode);


        }
    }
}

