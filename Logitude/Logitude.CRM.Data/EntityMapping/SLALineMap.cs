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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data;
 
namespace Logitude.CRM.Data.EntityMapping
{
 
    public class SLALineMap : EntityTypeConfiguration<SLALine>
    {
	    string dbms;
        public SLALineMap()
        { 
				this.ToTable("SLALines");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SLAHeaderId).HasColumnName("SLAHeaderId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SeverityId).HasColumnName("SeverityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.BusinessHoursId).HasColumnName("BusinessHoursId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FirstResponseTime).HasColumnName("FirstResponseTime");

            this.Property(t => t.FirstResponseTimeUnit).HasColumnName("FirstResponseTimeUnit").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.FirstResponseTimeInMinute).HasColumnName("FirstResponseTimeInMinute");

            this.Property(t => t.ResolveWithinTime).HasColumnName("ResolveWithinTime");

            this.Property(t => t.ResolveWithinTimeUnit).HasColumnName("ResolveWithinTimeUnit").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.ResolveWithinTimeInMinute).HasColumnName("ResolveWithinTimeInMinute");

            this.Property(t => t.FirstResponseEscalate).HasColumnName("FirstResponseEscalate");

            this.Property(t => t.ResolveWithinEscalate).HasColumnName("ResolveWithinEscalate");
        }
    }
}
	 