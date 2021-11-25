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
 
    public class SupplierInvoiceItemVehicleAddMap : EntityTypeConfiguration<SupplierInvoiceItemVehicleAdd>
    {
	    string dbms;
        public SupplierInvoiceItemVehicleAddMap()
        { 
			  this.ToTable("SupplierInvoiceItemVehicleAdds", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId, t.InvoiceCounterKey, t.InvoiceItemLineNumber, t.LineNumber });
	 
            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InvoiceCounterKey).HasColumnName("InvoiceCounterKey").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.InvoiceItemLineNumber).HasColumnName("InvoiceItemLineNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.VehicleModel).HasColumnName("VehicleModel").HasMaxLength(25).IsUnicode(false);

            this.Property(t => t.RichbitNumber).HasColumnName("RichbitNumber").HasMaxLength(12).IsUnicode(false);

            this.Property(t => t.ChassisNumber).HasColumnName("ChassisNumber").HasMaxLength(18).IsUnicode(false);

            this.Property(t => t.EngineNumber).HasColumnName("EngineNumber").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.WindowNumber).HasColumnName("WindowNumber").HasMaxLength(13).IsUnicode(false);

            this.Property(t => t.VehicleValue).HasColumnName("VehicleValue");

            this.Property(t => t.ChassisTax).HasColumnName("ChassisTax");

            this.Property(t => t.ChassisPurchaseTax).HasColumnName("ChassisPurchaseTax");

            this.Property(t => t.ChassisVat).HasColumnName("ChassisVat").HasPrecision(16, 2);

            this.Property(t => t.Exempt_type).HasColumnName("Exempt_type").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");
        }
    }
}
	 