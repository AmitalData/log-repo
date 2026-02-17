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
 
    public class TicketClassificationMap : EntityTypeConfiguration<TicketClassification>
    {
	    string dbms;
        public TicketClassificationMap()
        { 
				this.ToTable("TicketClassifications");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.Inactive).HasColumnName("Inactive").IsRequired();

            this.Property(t => t.ParentId).HasColumnName("ParentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DefaultSeverityId).HasColumnName("DefaultSeverityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EmployeeGroupId).HasColumnName("EmployeeGroupId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ManagerUserId).HasColumnName("ManagerUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EscalationNotify).HasColumnName("EscalationNotify").HasMaxLength(4000).IsUnicode(false);
        }
    }
}
	 