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
 
    public class CorrespondenceMap : EntityTypeConfiguration<Correspondence>
    {
	    string dbms;
        public CorrespondenceMap()
        { 
				this.ToTable("Correspondences");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreatedByContactId).HasColumnName("CreatedByContactId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.Description).HasMaxLength(2000);
			}
            else
            {
              this.Property(t => t.Description).HasMaxLength(4000);
			}


            this.Property(t => t.Description).HasColumnName("Description").IsRequired().IsUnicode(true);

            this.Property(t => t.IsInternal).HasColumnName("IsInternal").IsRequired();

            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntityId).HasColumnName("EntityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ActivityTypeCode).HasColumnName("ActivityTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.ActivityId).HasColumnName("ActivityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ActivitySubject).HasColumnName("ActivitySubject").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.CCs).HasColumnName("CCs").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.Bcc).HasColumnName("Bcc").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.NotifyMe).HasColumnName("NotifyMe");

            this.Property(t => t.NotifyOwner).HasColumnName("NotifyOwner");

            this.Property(t => t.InternalUsers).HasColumnName("InternalUsers").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.Direction).HasColumnName("Direction").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.HTMLFullBody).HasColumnName("HTMLFullBody").IsMaxLength().IsUnicode(true);

            this.Property(t => t.RightToLeft).HasColumnName("RightToLeft");
        }
    }
}
	 