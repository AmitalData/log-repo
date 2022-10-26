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
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data;
 
namespace Logitude.DashboardModule.Data.EntityMapping
{
 
    public class AnalyticsFactsMetaDataMap : EntityTypeConfiguration<AnalyticsFactsMetaData>
    {
	    string dbms;
        public AnalyticsFactsMetaDataMap()
        { 
				this.ToTable("AnalyticsFactsMetaDatas");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.TableName).HasColumnName("TableName").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.HashString).HasColumnName("HashString").HasMaxLength(32).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.ObjectTableName).HasColumnName("ObjectTableName").HasMaxLength(100).IsUnicode(false);
        }
    }
}
	 