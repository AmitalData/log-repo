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
 
    public class CB_CustomsBookAdditionsDetailsHistoryMap : EntityTypeConfiguration<CB_CustomsBookAdditionsDetailsHistory>
    {
	    string dbms;
        public CB_CustomsBookAdditionsDetailsHistoryMap()
        { 
		
     dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
    if (dbms == "oracle")
    {
	  this.ToTable("CB_CustomsBookAdditionsDetails", "Customs");
	}
    else
    {
	  this.ToTable("CB_CustomsBookAdditionsDetailsHistorys", "Customs");
	}

		
		    this.HasKey(t => new { t.CB_ID });
	 
            this.Property(t => t.ID).HasColumnName("ID");

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.TypeID).HasColumnName("TypeID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Title).HasColumnName("Title").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");

            this.Property(t => t.EntityStatusID).HasColumnName("EntityStatusID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.EnglishTitle).HasColumnName("EnglishTitle").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.CustomsBookAdditionID).HasColumnName("CustomsBookAdditionID");

            this.Property(t => t.ChangeRequestTypePriority).HasColumnName("ChangeRequestTypePriority");

            this.Property(t => t.CB_ID).HasColumnName("CB_ID").IsRequired().HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 