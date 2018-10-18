using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ChargeTypeAccountingMap : EntityTypeConfiguration<ChargeTypeAccounting>
    {
        public ChargeTypeAccountingMap()
        {
            this.Property(t => t.Id).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant);
            this.Property(t => t.VatTypeId).HasMaxLength(15).IsRequired().IsUnicode(false);
            this.Property(t => t.ChargeTypeId).HasMaxLength(15).IsRequired().IsUnicode(false);
            this.Property(t => t.PayableDebitAccount).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ReceivableCreditAccount).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PayableDebitGLAcountId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ReceivableCreditGLAccountId).HasMaxLength(15).IsUnicode(false);

            this.ToTable("ChargeTypeAccountings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId");
            this.Property(t => t.ChargeTypeId).HasColumnName("ChargeTypeId");
            this.Property(t => t.PayableDebitAccount).HasColumnName("PayableDebitAccount");
            this.Property(t => t.ReceivableCreditAccount).HasColumnName("ReceivableCreditAccount");
            this.Property(t => t.PayableDebitGLAcountId).HasColumnName("PayableDebitGLAcountId");
            this.Property(t => t.ReceivableCreditGLAccountId).HasColumnName("ReceivableCreditGLAccountId");

            this.HasRequired(t => t.VatType).WithMany().HasForeignKey(t => t.VatTypeId);
            this.HasRequired(t => t.ChargeType).WithMany().HasForeignKey(t => t.ChargeTypeId);
        }                  
    }
}
