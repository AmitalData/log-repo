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
 
    public class WarehouseEntryStatusMap : EntityTypeConfiguration<WarehouseEntryStatus>
    {
	    string dbms;
        public WarehouseEntryStatusMap()
        { 
				this.ToTable("WarehouseEntryStatuses");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(200).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsRequired().IsMaxLength().IsUnicode(true);
        }
    }
}
	 