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
 
    public class GuaranteeConditionMap : EntityTypeConfiguration<GuaranteeCondition>
    {
	    string dbms;
        public GuaranteeConditionMap()
        { 
			  this.ToTable("GuaranteeConditions", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.GuaranteeId).HasColumnName("GuaranteeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ReturnConditionCode).HasColumnName("ReturnConditionCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.GuaranteeAmount).HasColumnName("GuaranteeAmount").HasPrecision(16, 2);
        }
    }
}
	 