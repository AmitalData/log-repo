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
 
    public class ExportStorgeMap : EntityTypeConfiguration<ExportStorge>
    {
	    string dbms;
        public ExportStorgeMap()
        { 
			  this.ToTable("ExportStorges", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ExportFileNo).HasColumnName("ExportFileNo").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OrderNo).HasColumnName("OrderNo").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.CustomFileNo).HasColumnName("CustomFileNo").HasMaxLength(12).IsUnicode(false);

            this.Property(t => t.FclLcl).HasColumnName("FclLcl").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.Direction).HasColumnName("Direction").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.StorageNo).HasColumnName("StorageNo");

            this.Property(t => t.VoyageNo).HasColumnName("VoyageNo");

            this.Property(t => t.StorageDate).HasColumnName("StorageDate");

            this.Property(t => t.StorageStatus).HasColumnName("StorageStatus").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.IsOpenStoarge).HasColumnName("IsOpenStoarge");

            this.Property(t => t.IsConnectedToDeclaration).HasColumnName("IsConnectedToDeclaration");

            this.Property(t => t.OperationCode).HasColumnName("OperationCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.SenderCodeID).HasColumnName("SenderCodeID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.MessageFromForm).HasColumnName("MessageFromForm");

            this.Property(t => t.ReplyPhoneNumeric).HasColumnName("ReplyPhoneNumeric");

            this.Property(t => t.OperatorID).HasColumnName("OperatorID");

            this.Property(t => t.InformedParty).HasColumnName("InformedParty").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.DeclarationNumber).HasColumnName("DeclarationNumber").HasMaxLength(14).IsUnicode(false);

            this.Property(t => t.DeclarationsInContainer).HasColumnName("DeclarationsInContainer");

            this.Property(t => t.ExportManifestNumber).HasColumnName("ExportManifestNumber");

            this.Property(t => t.ReceivingSite).HasColumnName("ReceivingSite").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.StuffingSiteType).HasColumnName("StuffingSiteType").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.LoadingSite).HasColumnName("LoadingSite").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.ForwarderReference).HasColumnName("ForwarderReference").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.TransactionQuantity).HasColumnName("TransactionQuantity").HasPrecision(8, 0);

            this.Property(t => t.ExportDocument).HasColumnName("ExportDocument").HasMaxLength(8).IsUnicode(false);

            this.Property(t => t.MessageContent).HasColumnName("MessageContent").HasMaxLength(2).IsUnicode(true);

            this.Property(t => t.BookingNumber).HasColumnName("BookingNumber").HasMaxLength(16).IsUnicode(false);

            this.Property(t => t.LogisticDeliveryTypeID).HasColumnName("LogisticDeliveryTypeID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.CargoRows).HasColumnName("CargoRows").HasPrecision(4, 0);

            this.Property(t => t.ExporterIdentificationType).HasColumnName("ExporterIdentificationType").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.FinalDestinationInternatID).HasColumnName("FinalDestinationInternatID").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.FirstDestinationInternatID).HasColumnName("FirstDestinationInternatID").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.OriginAbroadSite).HasColumnName("OriginAbroadSite").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.ExpectedPortArrivalDate).HasColumnName("ExpectedPortArrivalDate").HasMaxLength(14).IsUnicode(true);

            this.Property(t => t.DraggedOrSupportedNumber).HasColumnName("DraggedOrSupportedNumber").HasMaxLength(10).IsUnicode(true);

            this.Property(t => t.TruckOrTrainNumber).HasColumnName("TruckOrTrainNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DriverId).HasColumnName("DriverId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipCode).HasColumnName("ShipCode").HasMaxLength(25).IsUnicode(false);

            this.Property(t => t.ShipName).HasColumnName("ShipName").HasMaxLength(24).IsUnicode(true);

            this.Property(t => t.TransportCompany).HasColumnName("TransportCompany").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.ShipAgent).HasColumnName("ShipAgent").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShippingCompanyCode).HasColumnName("ShippingCompanyCode").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SecurityClearence).HasColumnName("SecurityClearence").HasMaxLength(5).IsUnicode(false);

            this.Property(t => t.TransferCargoMethodType).HasColumnName("TransferCargoMethodType").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.StorageOrDockID).HasColumnName("StorageOrDockID").HasMaxLength(3).IsUnicode(true);

            this.Property(t => t.ExporterName).HasColumnName("ExporterName").HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.ExporterFileNumber).HasColumnName("ExporterFileNumber").HasMaxLength(16).IsUnicode(false);

            this.Property(t => t.PassportCountry).HasColumnName("PassportCountry").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.ExporterNumber).HasColumnName("ExporterNumber").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 