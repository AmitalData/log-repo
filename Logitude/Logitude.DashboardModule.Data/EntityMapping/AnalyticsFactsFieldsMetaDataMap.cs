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
 
    public class AnalyticsFactsFieldsMetaDataMap : EntityTypeConfiguration<AnalyticsFactsFieldsMetaData>
    {
	    string dbms;
        public AnalyticsFactsFieldsMetaDataMap()
        { 
				this.ToTable("AnalyticsFactsFieldsMetaDatas");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.AnalyticsFactsMetaDataId).HasColumnName("AnalyticsFactsMetaDataId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DataTypeCode).HasColumnName("DataTypeCode").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.CanMeasure).HasColumnName("CanMeasure");

            this.Property(t => t.CanGroup).HasColumnName("CanGroup");

            this.Property(t => t.FieldCode).HasColumnName("FieldCode").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.DisplayName).HasColumnName("DisplayName").HasMaxLength(150).IsUnicode(true);

            this.Property(t => t.DisplayNamePlural).HasColumnName("DisplayNamePlural").HasMaxLength(150).IsUnicode(true);

            this.Property(t => t.JoinedTableName).HasColumnName("JoinedTableName").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.JoinedTableKey).HasColumnName("JoinedTableKey").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.JoinedTableDisplayField).HasColumnName("JoinedTableDisplayField").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.JoinedTableDBName).HasColumnName("JoinedTableDBName").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.HasUnit).HasColumnName("HasUnit");

            this.Property(t => t.Unit).HasColumnName("Unit").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.CommonFilterCode).HasColumnName("CommonFilterCode").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.CanSecondaryGroup).HasColumnName("CanSecondaryGroup");

            this.Property(t => t.AllowTenantZeroFilter).HasColumnName("AllowTenantZeroFilter");
        }
    }
}
	 