using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class BusinessHoursHolidayMap : EntityTypeConfiguration<BusinessHoursHoliday>
    {
        public BusinessHoursHolidayMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.BusinessHourId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.HolidayName).IsRequired().HasMaxLength(60).IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("BusinessHoursHolidays");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.HolidayName).HasColumnName("HolidayName");

            this.Property(t => t.IsRecurring).HasColumnName("IsRecurring");
            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");

            this.Property(t => t.BusinessHourId).HasColumnName("BusinessHourId");

            this.Property(t => t.Day).HasColumnName("Day");
            this.Property(t => t.Month).HasColumnName("Month");
            this.Property(t => t.Year).HasColumnName("Year");

            this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
            this.HasRequired(t => t.BusinessHour).WithMany().HasForeignKey(d => d.BusinessHourId);

        }
    }
}
