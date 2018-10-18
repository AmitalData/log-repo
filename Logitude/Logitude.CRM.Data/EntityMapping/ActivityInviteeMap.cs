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
 
    public class ActivityInviteeMap : EntityTypeConfiguration<ActivityInvitee>
    {
	    string dbms;
        public ActivityInviteeMap()
        { 
				this.ToTable("ActivityInvitees");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ActivityId).HasColumnName("ActivityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ContactId).HasColumnName("ContactId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.Email).HasColumnName("Email").HasMaxLength(70).IsUnicode(false);

            this.Property(t => t.IsRequired).HasColumnName("IsRequired").IsRequired();
        }
    }
}
	 