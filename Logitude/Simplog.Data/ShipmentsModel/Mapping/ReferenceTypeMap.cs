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
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
 
    public class ReferenceTypeMap : EntityTypeConfiguration<ReferenceType>
    {
	    string dbms;
        public ReferenceTypeMap()
        { 
			  this.ToTable("ReferenceTypes");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").HasMaxLength(60).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.Inactive).HasColumnName("Inactive");
        }
    }
}
	 