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
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data;
 
namespace Logitude.TimeManagement.Data.EntityMapping
{
 
    public class TMReleaseMap : EntityTypeConfiguration<TMRelease>
    {
	    string dbms;
        public TMReleaseMap()
        { 
				this.ToTable("TMReleases");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.ReleaseName).HasColumnName("ReleaseName").IsRequired().HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.FromDate).HasColumnName("FromDate").IsRequired();

            this.Property(t => t.ToDate).HasColumnName("ToDate").IsRequired();
        }
    }
}
	 