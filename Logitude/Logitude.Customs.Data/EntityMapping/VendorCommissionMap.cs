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
 
    public class VendorCommissionMap : EntityTypeConfiguration<VendorCommission>
    {
	    string dbms;
        public VendorCommissionMap()
        { 
			  this.ToTable("VendorCommissions", "Customs");
		
		    this.HasKey(t => new { t.VendorId, t.CustomerId });
	 
            this.Property(t => t.VendorId).HasColumnName("VendorId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomerId).HasColumnName("CustomerId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CommisionPercentage).HasColumnName("CommisionPercentage").HasPrecision(7, 4);

            this.Property(t => t.Tenant).HasColumnName("Tenant");
        }
    }
}
	 