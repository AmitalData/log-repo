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
 
    public class CB_RuleClassificationMap : EntityTypeConfiguration<CB_RuleClassification>
    {
	    string dbms;
        public CB_RuleClassificationMap()
        { 
			  this.ToTable("CB_RuleClassifications", "Customs");
		
		    this.HasKey(t => new { t.CB_ID });
	 
            this.Property(t => t.CB_ID).HasColumnName("CB_ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomsItemID).HasColumnName("CustomsItemID");

            this.Property(t => t.ID).HasColumnName("ID");

            this.Property(t => t.CustomsBookType).HasColumnName("CustomsBookType").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Rules).HasColumnName("Rules").IsMaxLength().IsUnicode(true);

            this.Property(t => t.ParentID).HasColumnName("ParentID");

            this.Property(t => t.Index).HasColumnName("Index").HasMaxLength(10).IsUnicode(true);
        }
    }
}
	 