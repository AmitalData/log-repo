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
 
    public class CB_CustomsItemExclusionMap : EntityTypeConfiguration<CB_CustomsItemExclusion>
    {
	    string dbms;
        public CB_CustomsItemExclusionMap()
        { 
			  this.ToTable("CB_CustomsItemExclusion", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.RegularityRequirementID).HasColumnName("RegularityRequirementID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomsItemID).HasColumnName("CustomsItemID").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 