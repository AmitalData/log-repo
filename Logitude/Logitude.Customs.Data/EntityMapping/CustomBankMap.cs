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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class CustomBankMap : EntityTypeConfiguration<CustomBank>
    {
	    string dbms;
        public CustomBankMap()
        { 
			  this.ToTable("CustomBanks", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.InternalCode).HasColumnName("InternalCode").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.BankCode).HasColumnName("BankCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.BranchCode).HasColumnName("BranchCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.AccountNumber).HasColumnName("AccountNumber").HasMaxLength(11).IsUnicode(false);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.InActive).HasColumnName("InActive");

            this.Property(t => t.PayerTypeCode).HasColumnName("PayerTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.BankAddress).HasColumnName("BankAddress").HasMaxLength(1024).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.CustomsBranchId).HasColumnName("CustomsBranchId").HasMaxLength(6).IsUnicode(false);
        }
    }
}
	 