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
 
    public class TMOfficeHourMap : EntityTypeConfiguration<TMOfficeHour>
    {
	    string dbms;
        public TMOfficeHourMap()
        { 
				this.ToTable("TMOfficeHours");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.WorkDate).HasColumnName("WorkDate").IsRequired();

            this.Property(t => t.RecordedEntryTime).HasColumnName("RecordedEntryTime");

            this.Property(t => t.RecordedExitTime).HasColumnName("RecordedExitTime");

            this.Property(t => t.EntryTime).HasColumnName("EntryTime");

            this.Property(t => t.ExitTime).HasColumnName("ExitTime");

            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.Inactive).HasColumnName("Inactive");
        }
    }
}
	 