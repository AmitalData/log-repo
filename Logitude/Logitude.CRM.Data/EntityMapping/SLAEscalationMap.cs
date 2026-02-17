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
 
    public class SLAEscalationMap : EntityTypeConfiguration<SLAEscalation>
    {
	    string dbms;
        public SLAEscalationMap()
        { 
				this.ToTable("SLAEscalations");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SLAHeaderId).HasColumnName("SLAHeaderId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired();

            this.Property(t => t.EscalationFor).HasColumnName("EscalationFor").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.EscalationActionTimeIndicator).HasColumnName("EscalationActionTimeIndicator").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.EscalationTime).HasColumnName("EscalationTime");

            this.Property(t => t.EscalationTimeUnit).HasColumnName("EscalationTimeUnit").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.EscalaitonTimeInMinutes).HasColumnName("EscalaitonTimeInMinutes");
        }
    }
}
	 