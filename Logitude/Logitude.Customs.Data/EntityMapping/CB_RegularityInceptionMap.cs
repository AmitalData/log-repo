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
 
    public class CB_RegularityInceptionMap : EntityTypeConfiguration<CB_RegularityInception>
    {
	    string dbms;
        public CB_RegularityInceptionMap()
        { 
			  this.ToTable("CB_RegularityInceptions", "Customs");
		
		    this.HasKey(t => new { t.CB_ID });
	 
            this.Property(t => t.ID).HasColumnName("ID");

            this.Property(t => t.RegularityRequirementID).HasColumnName("RegularityRequirementID");

            this.Property(t => t.InterConditionsRelationshipID).HasColumnName("InterConditionsRelationshipID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.IsPersonalImportIncluded).HasColumnName("IsPersonalImportIncluded");

            this.Property(t => t.RequirementGoodsDescription).HasColumnName("RequirementGoodsDescription").IsMaxLength().IsUnicode(false);

            this.Property(t => t.RegularityRequirementWarnID).HasColumnName("RegularityRequirementWarnID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.IsCarnetIncluded).HasColumnName("IsCarnetIncluded");

            this.Property(t => t.CB_ID).HasColumnName("CB_ID").IsRequired().HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 