using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data;
 
namespace Logitude.Accounting.Data.EntityMapping
{
 
    public class PaymentChequeMap : EntityTypeConfiguration<PaymentCheque>
    {
	    string dbms;
        public PaymentChequeMap()
        { 
				this.ToTable("PaymentCheques");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.InternalNumber).HasColumnName("InternalNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ChequeNumber).HasColumnName("ChequeNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PayToGLAccountId).HasColumnName("PayToGLAccountId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PayToName).HasColumnName("PayToName").IsRequired().HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.BankAccountId).HasColumnName("BankAccountId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.BankAccountGLAccountId).HasColumnName("BankAccountGLAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LocalAmount).HasColumnName("LocalAmount").IsRequired().HasPrecision(16, 2);

            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ForeignAmount).HasColumnName("ForeignAmount").HasPrecision(16, 2);

            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate").HasPrecision(5, 3);

            this.Property(t => t.ValueDate).HasColumnName("ValueDate").IsRequired();

            this.Property(t => t.PrintDate).HasColumnName("PrintDate");

            this.Property(t => t.ApproveDate).HasColumnName("ApproveDate");

            this.Property(t => t.ApprovedByUserId).HasColumnName("ApprovedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");

            this.Property(t => t.CancelledByUserId).HasColumnName("CancelledByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CancelledDate).HasColumnName("CancelledDate");

            this.Property(t => t.CancellationRemarks).HasColumnName("CancellationRemarks").HasMaxLength(400).IsUnicode(true);

            this.Property(t => t.PaymentChequeStatusCode).HasColumnName("PaymentChequeStatusCode").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntityId).HasColumnName("EntityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Notes).HasColumnName("Notes").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.UniqueField).HasColumnName("UniqueField").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.APPaymentId).HasColumnName("APPaymentId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 