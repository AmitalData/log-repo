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
 
    public class TaxWithholdingAssessOfficeMap : EntityTypeConfiguration<TaxWithholdingAssessOffice>
    {
	    string dbms;
        public TaxWithholdingAssessOfficeMap()
        { 
				this.ToTable("TaxWithholdingAssessOffices");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(120).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.LocalName).HasColumnName("LocalName").IsRequired().HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.Inactive).HasColumnName("Inactive");
        }
    }
}
	 