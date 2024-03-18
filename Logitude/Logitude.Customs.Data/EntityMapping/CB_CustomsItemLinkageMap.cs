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
 
    public class CB_CustomsItemLinkageMap : EntityTypeConfiguration<CB_CustomsItemLinkage>
    {
	    string dbms;
        public CB_CustomsItemLinkageMap()
        { 
			  this.ToTable("CB_CustomsItemLinkages", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ChangeTypeID).HasColumnName("ChangeTypeID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CustomsItemDetailsHistoryID).HasColumnName("CustomsItemDetailsHistoryID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Connect_CustItemDetailsHistID).HasColumnName("Connect_CustItemDetailsHistID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
        }
    }
}
	 