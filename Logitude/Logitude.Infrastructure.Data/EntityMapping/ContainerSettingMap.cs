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
 
    public class ContainerSettingMap : EntityTypeConfiguration<ContainerSetting>
    {
	    string dbms;
        public ContainerSettingMap()
        { 
				this.ToTable("ContainerSettings");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.EmptyReturnClosingDays).HasColumnName("EmptyReturnClosingDays");

            this.Property(t => t.ShipmentATAClosingDays).HasColumnName("ShipmentATAClosingDays");

            this.Property(t => t.ShipmentATADateIndicator).HasColumnName("ShipmentATADateIndicator").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.IsExport).HasColumnName("IsExport");

            this.Property(t => t.IsDomestic).HasColumnName("IsDomestic");

            this.Property(t => t.IsImport).HasColumnName("IsImport");

            this.Property(t => t.IsDrop).HasColumnName("IsDrop");

            this.Property(t => t.AddedManually).HasColumnName("AddedManually");

            this.Property(t => t.ActivationDate).HasColumnName("ActivationDate");
        }
    }
}
	 