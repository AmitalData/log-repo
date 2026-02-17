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
 
    public class CashBookMap : EntityTypeConfiguration<CashBook>
    {
	    string dbms;
        public CashBookMap()
        { 
				this.ToTable("CashBooks");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").IsRequired().HasMaxLength(60).IsUnicode(false);

            this.Property(t => t.LocalName).HasColumnName("LocalName").IsRequired().HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CashBookTypeCode).HasColumnName("CashBookTypeCode").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount").HasPrecision(16, 2);

            this.Property(t => t.AccountId).HasColumnName("AccountId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.BranchId).HasColumnName("BranchId").IsRequired().HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 