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
 
    public class CB_CustomsBookAdditionMap : EntityTypeConfiguration<CB_CustomsBookAddition>
    {
	    string dbms;
        public CB_CustomsBookAdditionMap()
        { 
			  this.ToTable("CB_CustomsBookAdditions", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.TypeID).HasColumnName("TypeID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Title).HasColumnName("Title").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.CustomsBookTypeID).HasColumnName("CustomsBookTypeID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.AdditionCode).HasColumnName("AdditionCode");
        }
    }
}
	 