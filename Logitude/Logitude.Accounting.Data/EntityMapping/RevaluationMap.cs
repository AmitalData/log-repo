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
 
    public class RevaluationMap : EntityTypeConfiguration<Revaluation>
    {
	    string dbms;
        public RevaluationMap()
        { 
				this.ToTable("Revaluations");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.RevaluationNumber).HasColumnName("RevaluationNumber").IsRequired();

            this.Property(t => t.RevaluationDate).HasColumnName("RevaluationDate").IsRequired();

            this.Property(t => t.ChartOfAccountsId).HasColumnName("ChartOfAccountsId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.GLAccountId).HasColumnName("GLAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.RevaluationEnabled).HasColumnName("RevaluationEnabled");

            this.Property(t => t.Status).HasColumnName("Status").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Message).HasColumnName("Message").HasMaxLength(200).IsUnicode(false);

            this.Property(t => t.RevaluationsGLAccountId).HasColumnName("RevaluationsGLAccountId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 