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
 
    public class ActivityOwnerHistoryMap : EntityTypeConfiguration<ActivityOwnerHistory>
    {
	    string dbms;
        public ActivityOwnerHistoryMap()
        { 
				this.ToTable("ActivityOwnerHistories");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.ActivityId).HasColumnName("ActivityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OwnerId).HasColumnName("OwnerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.NeedSynchronization).HasColumnName("NeedSynchronization");
        }
    }
}
	 