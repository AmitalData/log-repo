using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class BusinessHourMap : EntityTypeConfiguration<BusinessHour>
    {
       public BusinessHourMap()
       {
           this.HasKey(t => t.Id);
           this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
           this.Property(t => t.Name).IsRequired().HasMaxLength(60).IsUnicode(true);
           this.Property(t => t.Description).HasMaxLength(250).IsUnicode(true);
           this.Property(t => t.Code).HasMaxLength(3).IsUnicode(false);

           this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
           this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
           this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

           // Table & Column Mappings
           this.ToTable("BusinessHours");
           this.Property(t => t.Id).HasColumnName("Id");
           this.Property(t => t.Tenant).HasColumnName("Tenant");
           this.Property(t => t.Name).HasColumnName("Name");
           this.Property(t => t.Description).HasColumnName("Description");
           this.Property(t => t.Is247).HasColumnName("Is247");

           this.Property(t => t.Code).HasColumnName("Code");

           this.Property(t => t.CreateDate).HasColumnName("CreateDate");
           this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
           this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
           this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");

           this.Property(t => t.IsMondayEnabeled).HasColumnName("IsMondayEnabeled");
           this.Property(t => t.IsTuesdayEnabeled).HasColumnName("IsTuesdayEnabeled");
           this.Property(t => t.IsWednesdayEnabeled).HasColumnName("IsWednesdayEnabeled");
           this.Property(t => t.IsThursdayEnabeled).HasColumnName("IsThursdayEnabeled");
           this.Property(t => t.IsFridayEnabeled).HasColumnName("IsFridayEnabeled");
           this.Property(t => t.IsSaturdayEnabeled).HasColumnName("IsSaturdayEnabeled");
           this.Property(t => t.IsSundayEnabeled).HasColumnName("IsSundayEnabeled");

           this.Property(t => t.MondayFromHour).HasColumnName("MondayFromHour");
           this.Property(t => t.TuesdayFromHour).HasColumnName("TuesdayFromHour");
           this.Property(t => t.WednesdayFromHour).HasColumnName("WednesdayFromHour");
           this.Property(t => t.ThursdayFromHour).HasColumnName("ThursdayFromHour");
           this.Property(t => t.FridayFromHour).HasColumnName("FridayFromHour");
           this.Property(t => t.SaturdayFromHour).HasColumnName("SaturdayFromHour");
           this.Property(t => t.SundayFromHour).HasColumnName("SundayFromHour");

           this.Property(t => t.MondayToHour).HasColumnName("MondayToHour");
           this.Property(t => t.TuesdayToHour).HasColumnName("TuesdayToHour");
           this.Property(t => t.WednesdayToHour).HasColumnName("WednesdayToHour");
           this.Property(t => t.ThursdayToHour).HasColumnName("ThursdayToHour");
           this.Property(t => t.FridayToHour).HasColumnName("FridayToHour");
           this.Property(t => t.SaturdayToHour).HasColumnName("SaturdayToHour");
           this.Property(t => t.SundayToHour).HasColumnName("SundayToHour");
           this.Property(t => t.SearchFields).HasColumnName("SearchFields");

           this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
           this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
       }
    }
}
