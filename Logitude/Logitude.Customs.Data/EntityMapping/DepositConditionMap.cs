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
 
    public class DepositConditionMap : EntityTypeConfiguration<DepositCondition>
    {
	    string dbms;
        public DepositConditionMap()
        { 
			  this.ToTable("DepositConditions", "Customs");
		
		    this.HasKey(t => new { t.DepositId, t.DepositConditionCode });
	 
            this.Property(t => t.DepositId).HasColumnName("DepositId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.DepositConditionCode).HasColumnName("DepositConditionCode").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.DepositAmount).HasColumnName("DepositAmount").HasPrecision(16, 2);
        }
    }
}
	 