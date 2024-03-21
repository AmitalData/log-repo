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
 
    public class CB_VendorMap : EntityTypeConfiguration<CB_Vendor>
    {
	    string dbms;
        public CB_VendorMap()
        { 
			  this.ToTable("CB_Vendors", "Customs");
		
		    this.HasKey(t => new { t.CB_ID });
	 
            this.Property(t => t.ID).HasColumnName("ID");

            this.Property(t => t.Title).HasColumnName("Title").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.State).HasColumnName("State");

            this.Property(t => t.EnglishCountryName).HasColumnName("EnglishCountryName").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.VendorSingleStringAddress).HasColumnName("VendorSingleStringAddress").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.CB_ID).HasColumnName("CB_ID").IsRequired().HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 