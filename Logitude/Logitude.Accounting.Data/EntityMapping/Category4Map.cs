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
 
    public class Category4Map : EntityTypeConfiguration<Category4>
    {
	    string dbms;
        public Category4Map()
        { 
				this.ToTable("Category4");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").IsRequired().IsMaxLength().IsUnicode(true);

            this.Property(t => t.LocalName).HasColumnName("LocalName").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);
        }
    }
}
	 