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
 
    public class AutomaticReconcileMethodMap : EntityTypeConfiguration<AutomaticReconcileMethod>
    {
	    string dbms;
        public AutomaticReconcileMethodMap()
        { 
				this.ToTable("AutomaticReconcileMethods");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AutomaticReconcile1).HasColumnName("AutomaticReconcile1").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AutomaticReconcile2).HasColumnName("AutomaticReconcile2").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AutomaticReconcile3).HasColumnName("AutomaticReconcile3").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);
        }
    }
}
	 