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
 
    public class ToggleMap : EntityTypeConfiguration<Toggle>
    {
	    string dbms;
        public ToggleMap()
        { 
				this.ToTable("Toggles");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(400).IsUnicode(true);
        }
    }
}
	 