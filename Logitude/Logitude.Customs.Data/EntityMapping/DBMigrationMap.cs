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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class DBMigrationMap : EntityTypeConfiguration<DBMigration>
    {
	    string dbms;
        public DBMigrationMap()
        { 
			  this.ToTable("DBMigrations", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ExecuteDate).HasColumnName("ExecuteDate").IsRequired();

            this.Property(t => t.MajorVersion).HasColumnName("MajorVersion").IsRequired().HasPrecision(5, 2);

            this.Property(t => t.MinorVersion).HasColumnName("MinorVersion").IsRequired();

            this.Property(t => t.Remarks).HasColumnName("Remarks").HasMaxLength(256).IsUnicode(true);

            this.Property(t => t.IsClose).HasColumnName("IsClose");
        }
    }
}
	 