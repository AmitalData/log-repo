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
 
    public class TMProjectMap : EntityTypeConfiguration<TMProject>
    {
	    string dbms;
        public TMProjectMap()
        { 
				this.ToTable("TMProjects");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.Description).HasColumnName("Description").IsMaxLength().IsUnicode(true);

            this.Property(t => t.CustomerId).HasColumnName("CustomerId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.OwnerId).HasColumnName("OwnerId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ProjectNumber).HasColumnName("ProjectNumber").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsInnerProject).HasColumnName("IsInnerProject");

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.BudgetId).HasColumnName("BudgetId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CategoryId).HasColumnName("CategoryId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsProrated).HasColumnName("IsProrated");

            this.Property(t => t.ExternalProjectNumber).HasColumnName("ExternalProjectNumber").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.ExcludeFromProrating).HasColumnName("ExcludeFromProrating");

            this.Property(t => t.DayOffTypeCode).HasColumnName("DayOffTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.BlockedForDataEntry).HasColumnName("BlockedForDataEntry");
        }
    }
}
	 