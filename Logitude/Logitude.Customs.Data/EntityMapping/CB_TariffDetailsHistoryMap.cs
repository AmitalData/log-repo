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
 
    public class CB_TariffDetailsHistoryMap : EntityTypeConfiguration<CB_TariffDetailsHistory>
    {
	    string dbms;
        public CB_TariffDetailsHistoryMap()
        { 
			  this.ToTable("CB_TariffDetailsHistorys", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.TariffID).HasColumnName("TariffID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.QuotaID).HasColumnName("QuotaID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");

            this.Property(t => t.EntityStatusID).HasColumnName("EntityStatusID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.WithinQuota_ComputMethDataID).HasColumnName("WithinQuota_ComputMethDataID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.WithoutQuota_ComputMethDataID).HasColumnName("WithoutQuota_ComputMethDataID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ChangeRequestTypePriority).HasColumnName("ChangeRequestTypePriority");
        }
    }
}
	 