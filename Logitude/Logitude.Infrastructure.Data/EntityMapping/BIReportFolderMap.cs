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
 
    public class BIReportFolderMap : EntityTypeConfiguration<BIReportFolder>
    {
	    string dbms;
        public BIReportFolderMap()
        { 
				this.ToTable("BIReportFolders");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(200).IsUnicode(true);

            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.Index).HasColumnName("Index");

            this.Property(t => t.PermissionForAll).HasColumnName("PermissionForAll");

            this.Property(t => t.PermittedByUserId).HasColumnName("PermittedByUserId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 