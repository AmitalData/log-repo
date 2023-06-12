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
 
    public class SupplierInvioceExportDefaultMap : EntityTypeConfiguration<SupplierInvioceExportDefault>
    {
	    string dbms;
        public SupplierInvioceExportDefaultMap()
        { 
			  this.ToTable("SupplierInvioceExportDefaults", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.AccountTypeCode).HasColumnName("AccountTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.PartyRelationshipCode).HasColumnName("PartyRelationshipCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.BuyerRoleCode).HasColumnName("BuyerRoleCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.ProcessTypeCode).HasColumnName("ProcessTypeCode").HasMaxLength(7).IsUnicode(false);

            this.Property(t => t.TransactionNatureCode).HasColumnName("TransactionNatureCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ClaimReasonCode).HasColumnName("ClaimReasonCode").HasMaxLength(4).IsUnicode(false);
        }
    }
}
	 