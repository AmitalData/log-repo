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
 
    public class TicketEscalationMap : EntityTypeConfiguration<TicketEscalation>
    {
	    string dbms;
        public TicketEscalationMap()
        { 
				this.ToTable("TicketEscalations");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.TicketId).HasColumnName("TicketId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired();

            this.Property(t => t.EscalationFor).HasColumnName("EscalationFor").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.Recepients).HasColumnName("Recepients").HasMaxLength(4000).IsUnicode(false);

            this.Property(t => t.IsClose).HasColumnName("IsClose").IsRequired();

            this.Property(t => t.IsSLAViolated).HasColumnName("IsSLAViolated").IsRequired();

            this.Property(t => t.DueDate).HasColumnName("DueDate");

            this.Property(t => t.CloseDate).HasColumnName("CloseDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();
        }
    }
}
	 