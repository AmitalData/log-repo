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
 
    public class DigitalPortalScreenMap : EntityTypeConfiguration<DigitalPortalScreen>
    {
	    string dbms;
        public DigitalPortalScreenMap()
        { 
				this.ToTable("DigitalPortalScreens");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ScreenCode).HasColumnName("ScreenCode").IsRequired().HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.Content).HasColumnName("Content").IsRequired().IsMaxLength().IsUnicode(true);

            this.Property(t => t.DraftContent).HasColumnName("DraftContent").IsMaxLength().IsUnicode(true);

            this.Property(t => t.ProfileId).HasColumnName("ProfileId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsList).HasColumnName("IsList");
        }
    }
}
	 