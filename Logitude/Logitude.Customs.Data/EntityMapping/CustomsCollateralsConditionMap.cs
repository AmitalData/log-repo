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
 
    public class CustomsCollateralsConditionMap : EntityTypeConfiguration<CustomsCollateralsCondition>
    {
	    string dbms;
        public CustomsCollateralsConditionMap()
        { 
			  this.ToTable("CustomsCollateralsConditions", "Customs");
		
		    this.HasKey(t => new { t.CustomsCollateralId, t.ConditionCode });
	 
            this.Property(t => t.CustomsCollateralId).HasColumnName("CustomsCollateralId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.ConditionCode).HasColumnName("ConditionCode").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.RequestedAmount).HasColumnName("RequestedAmount").HasPrecision(18, 2);
        }
    }
}
	 