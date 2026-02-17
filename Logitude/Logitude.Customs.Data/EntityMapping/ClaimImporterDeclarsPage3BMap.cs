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
 
    public class ClaimImporterDeclarsPage3BMap : EntityTypeConfiguration<ClaimImporterDeclarsPage3B>
    {
	    string dbms;
        public ClaimImporterDeclarsPage3BMap()
        { 
			  this.ToTable("ClaimImporterDeclarsPage3Bs", "Customs");
		
		    this.HasKey(t => new { t.ClaimId, t.LineNo });
	 
            this.Property(t => t.ClaimId).HasColumnName("ClaimId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.LineNo).HasColumnName("LineNo").HasDatabaseGeneratedOption(null);

            this.Property(t => t.SaleAmountAfter).HasColumnName("SaleAmountAfter").HasPrecision(16, 2);

            this.Property(t => t.SaleAmountClaim).HasColumnName("SaleAmountClaim").HasPrecision(16, 2);

            this.Property(t => t.SaleAmountBefore).HasColumnName("SaleAmountBefore").HasPrecision(16, 2);

            this.Property(t => t.DescriptionOfGoods).HasColumnName("DescriptionOfGoods").HasMaxLength(256).IsUnicode(true);

            this.Property(t => t.InventoryAmount).HasColumnName("InventoryAmount").HasPrecision(12, 5);

            this.Property(t => t.SoldGoodsAmount).HasColumnName("SoldGoodsAmount").HasPrecision(12, 5);
        }
    }
}
	 