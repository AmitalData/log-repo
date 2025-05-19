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
 
    public class MagayaStatusMap : EntityTypeConfiguration<MagayaStatus>
    {
	    string dbms;
        public MagayaStatusMap()
        { 
				this.ToTable("MagayaStatuses");
		
		    this.HasKey(t => new { t.StatusCode });
	 
            this.Property(t => t.StatusCode).HasColumnName("StatusCode").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.StatusName).HasColumnName("StatusName").HasMaxLength(60).IsUnicode(false);
        }
    }
}
	 