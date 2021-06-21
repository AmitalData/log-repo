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
 
    public class GovernmentProcedureTypeMap : EntityTypeConfiguration<GovernmentProcedureType>
    {
	    string dbms;
        public GovernmentProcedureTypeMap()
        { 
			  this.ToTable("GovernmentProcedureTypes", "Customs");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(7).IsUnicode(false);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(150).IsUnicode(true);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").HasMaxLength(150).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.IsImport).HasColumnName("IsImport");

            this.Property(t => t.IndexOrder).HasColumnName("IndexOrder");

            this.Property(t => t.IsExport).HasColumnName("IsExport");
        }
    }
}
	 