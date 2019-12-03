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
 
    public class DBMigrationLineMap : EntityTypeConfiguration<DBMigrationLine>
    {
	    string dbms;
        public DBMigrationLineMap()
        { 
			  this.ToTable("DBMigrationLines", "Customs");
		
		    this.HasKey(t => new { t.Id, t.CounterKey });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CounterKey).HasColumnName("CounterKey").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.SqlScript).HasColumnName("SqlScript").IsRequired().HasMaxLength(1024).IsUnicode(false);

            this.Property(t => t.ApprovedRemarks).HasColumnName("ApprovedRemarks").HasMaxLength(256).IsUnicode(true);
        }
    }
}
	 