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
 
    public class SLAEscalationRecepientMap : EntityTypeConfiguration<SLAEscalationRecepient>
    {
	    string dbms;
        public SLAEscalationRecepientMap()
        { 
				this.ToTable("SLAEscalationRecepients");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SLAEscalationId).HasColumnName("SLAEscalationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PreDefinitionId).HasColumnName("PreDefinitionId").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.UserId).HasColumnName("UserId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 