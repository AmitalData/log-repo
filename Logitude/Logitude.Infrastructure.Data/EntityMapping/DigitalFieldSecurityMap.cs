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
 
    public class DigitalFieldSecurityMap : EntityTypeConfiguration<DigitalFieldSecurity>
    {
	    string dbms;
        public DigitalFieldSecurityMap()
        { 
				this.ToTable("DigitalFieldSecurities");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DefaultSettings).HasColumnName("DefaultSettings").IsRequired().IsMaxLength().IsUnicode(true);

            this.Property(t => t.ProfileId).HasColumnName("ProfileId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ParentObjectTableId).HasColumnName("ParentObjectTableId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 