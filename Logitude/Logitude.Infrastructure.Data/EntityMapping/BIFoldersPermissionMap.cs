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
 
    public class BIFoldersPermissionMap : EntityTypeConfiguration<BIFoldersPermission>
    {
	    string dbms;
        public BIFoldersPermissionMap()
        { 
				this.ToTable("BIFoldersPermissions");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.FolderId).HasColumnName("FolderId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UserId).HasColumnName("UserId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 