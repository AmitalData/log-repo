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
 
    public class AvailableStatusFieldMap : EntityTypeConfiguration<AvailableStatusField>
    {
	    string dbms;
        public AvailableStatusFieldMap()
        { 
			  this.ToTable("AvailableStatusFields", "Customs");
		
		    this.HasKey(t => new { t.FieldCode });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.FieldCode).HasColumnName("FieldCode").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsAvailable).HasColumnName("IsAvailable");
        }
    }
}
	 