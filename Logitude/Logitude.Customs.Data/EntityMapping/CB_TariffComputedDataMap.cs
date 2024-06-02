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
 
    public class CB_TariffComputedDataMap : EntityTypeConfiguration<CB_TariffComputedData>
    {
	    string dbms;
        public CB_TariffComputedDataMap()
        { 
			  this.ToTable("CB_TariffComputedDatas", "Customs");
		
		    this.HasKey(t => new { t.CB_ID });
	 
            this.Property(t => t.CB_ID).HasColumnName("CB_ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ID).HasColumnName("ID");

            this.Property(t => t.TariffID).HasColumnName("TariffID");

            this.Property(t => t.TDH_IDNum).HasColumnName("TDH_IDNum");

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");

            this.Property(t => t.WithoutQuota_ComputationID).HasColumnName("WithoutQuota_ComputationID");

            this.Property(t => t.WithinQuota_ComputationID).HasColumnName("WithinQuota_ComputationID");

            this.Property(t => t.CustomsItemIDNum).HasColumnName("CustomsItemIDNum");

            this.Property(t => t.TradeAgreementID).HasColumnName("TradeAgreementID");

            this.Property(t => t.QuotaID).HasColumnName("QuotaID");
        }
    }
}
	 