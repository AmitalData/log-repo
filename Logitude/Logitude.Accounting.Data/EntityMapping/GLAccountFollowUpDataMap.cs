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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data;
 
namespace Logitude.Accounting.Data.EntityMapping
{
 
    public class GLAccountFollowUpDataMap : EntityTypeConfiguration<GLAccountFollowUpData>
    {
	    string dbms;
        public GLAccountFollowUpDataMap()
        { 
				this.ToTable("GLAccountFollowUpDatas");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FollowUpDate).HasColumnName("FollowUpDate");

            this.Property(t => t.FollowUpRemarks).HasColumnName("FollowUpRemarks").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.GlAccountId).HasColumnName("GlAccountId").IsRequired().HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 