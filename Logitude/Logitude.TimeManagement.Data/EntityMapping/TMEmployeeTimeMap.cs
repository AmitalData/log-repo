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
 
    public class TMEmployeeTimeMap : EntityTypeConfiguration<TMEmployeeTime>
    {
	    string dbms;
        public TMEmployeeTimeMap()
        { 
				this.ToTable("TMEmployeeTimes");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EmployeeUserId).HasColumnName("EmployeeUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DateOfWork).HasColumnName("DateOfWork").IsRequired();

            this.Property(t => t.Description).HasColumnName("Description").IsRequired().HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.TimeInMinutes).HasColumnName("TimeInMinutes");

            this.Property(t => t.WINumber).HasColumnName("WINumber").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.ProjectId).HasColumnName("ProjectId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LocationCode).HasColumnName("LocationCode").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.AnalyzeQueueId).HasColumnName("AnalyzeQueueId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SprintId).HasColumnName("SprintId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ProratedDuration).HasColumnName("ProratedDuration");

            this.Property(t => t.FullDuration).HasColumnName("FullDuration");

            this.Property(t => t.NeedsProrating).HasColumnName("NeedsProrating");
        }
    }
}
	 