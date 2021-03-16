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
 
    public class GLAccountRecocileDataMap : EntityTypeConfiguration<GLAccountRecocileData>
    {
	    string dbms;
        public GLAccountRecocileDataMap()
        { 
				this.ToTable("GLAccountRecocileDatas");
		
		    this.HasKey(t => new { t.AccountId });
	 
            this.Property(t => t.AccountId).HasColumnName("AccountId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.LastReconciledByUserId).HasColumnName("LastReconciledByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LastReconcileDateTime).HasColumnName("LastReconcileDateTime");
        }
    }
}
	 