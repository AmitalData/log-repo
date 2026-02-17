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
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data;
 
namespace Logitude.WarehouseLib.Data.EntityMapping
{
 
    public class WarehouseEntryPackagesReleaseMap : EntityTypeConfiguration<WarehouseEntryPackagesRelease>
    {
	    string dbms;
        public WarehouseEntryPackagesReleaseMap()
        { 
				this.ToTable("WarehouseEntryPackagesReleases");
		
		    this.HasKey(t => new { t.EntryPackageId, t.ReleasePackageId });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntryPackageId).HasColumnName("EntryPackageId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ReleasePackageId).HasColumnName("ReleasePackageId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Quantity).HasColumnName("Quantity").IsRequired();

            this.Property(t => t.IsCanceled).HasColumnName("IsCanceled").IsRequired();
        }
    }
}
	 