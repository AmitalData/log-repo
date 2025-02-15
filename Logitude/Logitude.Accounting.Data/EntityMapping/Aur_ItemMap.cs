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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data;
 
namespace Logitude.Accounting.Data.EntityMapping
{
 
    public class Aur_ItemMap : EntityTypeConfiguration<Aur_Item>
    {
	    string dbms;
        public Aur_ItemMap()
        { 
				this.ToTable("Aur_Items");
		
		    this.HasKey(t => new { t.PaymentId, t.Line });
	 
            this.Property(t => t.PaymentId).HasColumnName("PaymentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Line).HasColumnName("Line").HasDatabaseGeneratedOption(null);

            this.Property(t => t.SalesOrderid).HasColumnName("SalesOrderid").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.RelatedContract).HasColumnName("RelatedContract").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.ProductNumber).HasColumnName("ProductNumber").HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.ProductName).HasColumnName("ProductName").HasMaxLength(256).IsUnicode(true);

            this.Property(t => t.PricePerUnit).HasColumnName("PricePerUnit").HasPrecision(10, 2);

            this.Property(t => t.Quantity).HasColumnName("Quantity").HasPrecision(10, 2);

            this.Property(t => t.Discount).HasColumnName("Discount").HasPrecision(10, 2);

            this.Property(t => t.BaseAmount).HasColumnName("BaseAmount").HasPrecision(10, 2);

            this.Property(t => t.Tax).HasColumnName("Tax").HasPrecision(5, 2);

            this.Property(t => t.ExtendedAmount).HasColumnName("ExtendedAmount").HasPrecision(10, 2);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();
        }
    }
}
	 