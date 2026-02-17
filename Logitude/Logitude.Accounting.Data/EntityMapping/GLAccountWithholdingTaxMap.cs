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
 
    public class GLAccountWithholdingTaxMap : EntityTypeConfiguration<GLAccountWithholdingTax>
    {
	    string dbms;
        public GLAccountWithholdingTaxMap()
        { 
				this.ToTable("GLAccountWithholdingTax");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.GLAccountId).HasColumnName("GLAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FromDate).HasColumnName("FromDate").IsRequired();

            this.Property(t => t.ToDate).HasColumnName("ToDate").IsRequired();

            this.Property(t => t.Percentage).HasColumnName("Percentage").IsRequired();

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.LineNumber).HasColumnName("LineNumber");
        }
    }
}
	 