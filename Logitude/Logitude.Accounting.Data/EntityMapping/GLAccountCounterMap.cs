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
 
    public class GLAccountCounterMap : EntityTypeConfiguration<GLAccountCounter>
    {
	    string dbms;
        public GLAccountCounterMap()
        { 
				this.ToTable("GLAccountCounters");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.Prefix).HasColumnName("Prefix").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StartNumber).HasColumnName("StartNumber");

            this.Property(t => t.CurrentNumber).HasColumnName("CurrentNumber");
        }
    }
}
	 