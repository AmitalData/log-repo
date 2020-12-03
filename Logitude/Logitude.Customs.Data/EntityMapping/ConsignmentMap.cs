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
 
    public class ConsignmentMap : EntityTypeConfiguration<Consignment>
    {
	    string dbms;
        public ConsignmentMap()
        { 
			  this.ToTable("Consignments", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId, t.ConsignmentNumber });
	 
            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.ConsignmentNumber).HasColumnName("ConsignmentNumber").HasDatabaseGeneratedOption(null);

            this.Property(t => t.SequenceNumeric).HasColumnName("SequenceNumeric");

            this.Property(t => t.CargoTypeCode).HasColumnName("CargoTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ManifestDate).HasColumnName("ManifestDate");

            this.Property(t => t.ManifestNumber).HasColumnName("ManifestNumber").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.SecondCargoID).HasColumnName("SecondCargoID").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.ThirdCargoID).HasColumnName("ThirdCargoID").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.UnloadDate).HasColumnName("UnloadDate");

            this.Property(t => t.UnloadPortCode).HasColumnName("UnloadPortCode").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.CargoDescription).HasColumnName("CargoDescription").HasMaxLength(256).IsUnicode(true);

            this.Property(t => t.IsLastReleaseFromWarehous).HasColumnName("IsLastReleaseFromWarehous").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.LoadingPortCode).HasColumnName("LoadingPortCode").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.OriginCountryCode).HasColumnName("OriginCountryCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.StorageSiteCode).HasColumnName("StorageSiteCode").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.ReceiverWarehouseCode).HasColumnName("ReceiverWarehouseCode").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.DeliveryPlaceName).HasColumnName("DeliveryPlaceName").HasMaxLength(256).IsUnicode(false);

            this.Property(t => t.IsDangerousGoods).HasColumnName("IsDangerousGoods");

            this.Property(t => t.FinalDestinationPortCode).HasColumnName("FinalDestinationPortCode").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.ExportRecieverWareHouseCode).HasColumnName("ExportRecieverWareHouseCode").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.ExportUnloadingPortCode).HasColumnName("ExportUnloadingPortCode").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.ExportLoadingPortCode).HasColumnName("ExportLoadingPortCode").HasMaxLength(17).IsUnicode(false);
        }
    }
}
	 