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
 
    public class ChartOfAccountMap : EntityTypeConfiguration<ChartOfAccount>
    {
	    string dbms;
        public ChartOfAccountMap()
        { 
				this.ToTable("ChartOfAccounts");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(5).IsUnicode(false);

            this.Property(t => t.LocalName).HasColumnName("LocalName").IsRequired().HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").IsRequired().HasMaxLength(60).IsUnicode(false);

            this.Property(t => t.ParentId).HasColumnName("ParentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TypeCode).HasColumnName("TypeCode").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(200).IsUnicode(true);

            this.Property(t => t.ChartOfAccountSecurityLevel).HasColumnName("ChartOfAccountSecurityLevel");
        }
    }
}
	 