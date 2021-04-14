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

            this.Property(t => t.OpenDate).HasColumnName("OpenDate");

            this.Property(t => t.CargoTypeCode).HasColumnName("CargoTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Manifest).HasColumnName("Manifest").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.SecondCargoID).HasColumnName("SecondCargoID").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.ThirdCargoID).HasColumnName("ThirdCargoID").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.CargoDescription).HasColumnName("CargoDescription").HasMaxLength(512).IsUnicode(true);

            this.Property(t => t.CargoType).HasColumnName("CargoType").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.HandlingCode).HasColumnName("HandlingCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.DangerousGoodsIndication).HasColumnName("DangerousGoodsIndication").HasPrecision(1, 0);

            this.Property(t => t.CodeBreaksIndication).HasColumnName("CodeBreaksIndication").HasPrecision(1, 0);

            this.Property(t => t.DamageCode).HasColumnName("DamageCode").HasPrecision(1, 0);

            this.Property(t => t.ForeignCurrencyType).HasColumnName("ForeignCurrencyType").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ForeignCurrencyAmoun).HasColumnName("ForeignCurrencyAmoun").HasPrecision(14, 2);

            this.Property(t => t.GoodsValueNIS).HasColumnName("GoodsValueNIS").HasPrecision(14, 2);

            this.Property(t => t.PackageType).HasColumnName("PackageType").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Quantity).HasColumnName("Quantity").HasPrecision(8, 0);

            this.Property(t => t.MarksNumbers).HasColumnName("MarksNumbers").HasMaxLength(12).IsUnicode(true);

            this.Property(t => t.WeightInPortMandatory).HasColumnName("WeightInPortMandatory").HasPrecision(1, 0);

            this.Property(t => t.Weight).HasColumnName("Weight").HasPrecision(11, 3);

            this.Property(t => t.VolumeSize).HasColumnName("VolumeSize").HasPrecision(8, 0);

            this.Property(t => t.LicensePlateNumber).HasColumnName("LicensePlateNumber").HasMaxLength(11).IsUnicode(false);

            this.Property(t => t.CustomsItem).HasColumnName("CustomsItem").HasMaxLength(18).IsUnicode(true);

            this.Property(t => t.RiskLevel).HasColumnName("RiskLevel").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.DangerousSubstancename).HasColumnName("DangerousSubstancename").HasMaxLength(10).IsUnicode(true);

            this.Property(t => t.WeightVerificationNumber).HasColumnName("WeightVerificationNumber").HasMaxLength(10).IsUnicode(true);

            this.Property(t => t.ExporterReportedWeightID).HasColumnName("ExporterReportedWeightID").HasPrecision(9, 0);

            this.Property(t => t.ExporterReportedWeightName).HasColumnName("ExporterReportedWeightName").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.ContainerNumber).HasColumnName("ContainerNumber").HasMaxLength(12).IsUnicode(true);

            this.Property(t => t.CoolingActivated).HasColumnName("CoolingActivated").HasPrecision(1, 0);

            this.Property(t => t.RequiredTemperature).HasColumnName("RequiredTemperature").HasPrecision(3, 1);

            this.Property(t => t.PharmaGroceryIndication).HasColumnName("PharmaGroceryIndication").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.LeftException).HasColumnName("LeftException").HasPrecision(4, 0);

            this.Property(t => t.RightException).HasColumnName("RightException").HasPrecision(4, 0);

            this.Property(t => t.FrontException).HasColumnName("FrontException").HasPrecision(4, 0);

            this.Property(t => t.BackException).HasColumnName("BackException").HasPrecision(4, 0);

            this.Property(t => t.HeightException).HasColumnName("HeightException").HasPrecision(4, 0);

            this.Property(t => t.ContainerLineCode).HasColumnName("ContainerLineCode").HasMaxLength(2).IsUnicode(true);

            this.Property(t => t.VentValue).HasColumnName("VentValue").HasPrecision(3, 0);

            this.Property(t => t.HumidityPercentage).HasColumnName("HumidityPercentage").HasPrecision(3, 0);

            this.Property(t => t.Co2Percentage).HasColumnName("Co2Percentage").HasPrecision(2, 0);

            this.Property(t => t.O2Percentage).HasColumnName("O2Percentage").HasPrecision(2, 0);

            this.Property(t => t.SealNumber).HasColumnName("SealNumber").HasMaxLength(35).IsUnicode(true);

            this.Property(t => t.SealType).HasColumnName("SealType").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.CoolingReportingMethod).HasColumnName("CoolingReportingMethod").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.FullnessCode).HasColumnName("FullnessCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.OwnershipCode).HasColumnName("OwnershipCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.ContainerTypeWCO).HasColumnName("ContainerTypeWCO").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.UNNumber).HasColumnName("UNNumber").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.RiskGroup).HasColumnName("RiskGroup").HasMaxLength(2).IsUnicode(true);

            this.Property(t => t.ExporterRef).HasColumnName("ExporterRef").HasMaxLength(1000).IsUnicode(true);
        }
    }
}
	 