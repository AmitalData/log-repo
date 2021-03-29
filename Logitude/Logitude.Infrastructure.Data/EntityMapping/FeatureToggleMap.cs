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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data;
 
namespace Logitude.Infrastructure.Data.EntityMapping
{
 
    public class FeatureToggleMap : EntityTypeConfiguration<FeatureToggle>
    {
	    string dbms;
        public FeatureToggleMap()
        { 
				this.ToTable("FeatureToggles");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.TenantNumber).HasColumnName("TenantNumber");

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.ToggleCode).HasColumnName("ToggleCode").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.IsMultiTenant).HasColumnName("IsMultiTenant");

            this.Property(t => t.FromTenantNumber).HasColumnName("FromTenantNumber");

            this.Property(t => t.ToTenantNumber).HasColumnName("ToTenantNumber");
        }
    }
}
	 