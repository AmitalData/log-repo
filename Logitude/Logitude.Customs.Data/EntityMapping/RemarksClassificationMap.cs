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
 
    public class RemarksClassificationMap : EntityTypeConfiguration<RemarksClassification>
    {
	    string dbms;
        public RemarksClassificationMap()
        { 
			  this.ToTable("RemarksClassifications", "Customs");
		
		    this.HasKey(t => new { t.CB_ID });
	 
            this.Property(t => t.Id).HasColumnName("Id");

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CB_ID).HasColumnName("CB_ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomsItemsID).HasColumnName("CustomsItemsID");

            this.Property(t => t.RemarkDescription).HasColumnName("RemarkDescription").IsMaxLength().IsUnicode(true);
        }
    }
}
	 