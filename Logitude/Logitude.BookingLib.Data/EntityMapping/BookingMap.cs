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
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data;
 
namespace Logitude.BookingLib.Data.EntityMapping
{
 
    public class BookingMap : EntityTypeConfiguration<Booking>
    {
	    string dbms;
        public BookingMap()
        { 
				this.ToTable("Bookings");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.BookingNumber).HasColumnName("BookingNumber").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DirectionCode).HasColumnName("DirectionCode").IsRequired().HasMaxLength(1).IsFixedLength();

            this.Property(t => t.TransportModeCode).HasColumnName("TransportModeCode").IsRequired().HasMaxLength(1).IsFixedLength();

            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.BookingStatusCode).HasColumnName("BookingStatusCode").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Master).HasColumnName("Master").HasMaxLength(16).IsUnicode(false);

            this.Property(t => t.SpaceAllocationCode).HasColumnName("SpaceAllocationCode").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.MainCarriageCarrierId).HasColumnName("MainCarriageCarrierId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.MainCarriageIsFromStack).HasColumnName("MainCarriageIsFromStack").IsRequired();

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.MainCarriageSpaceAllocationCode).HasColumnName("MainCarriageSpaceAllocationCod").HasMaxLength(2).IsUnicode(false);
			}
			else
			{
              this.Property(t => t.MainCarriageSpaceAllocationCode).HasColumnName("MainCarriageSpaceAllocationCode").HasMaxLength(2).IsUnicode(false);
			}


            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.MainCarriageAllotmentIdentification).HasColumnName("MainCarriageAllotmentIdentific").HasMaxLength(14).IsUnicode(false);
			}
			else
			{
              this.Property(t => t.MainCarriageAllotmentIdentification).HasColumnName("MainCarriageAllotmentIdentification").HasMaxLength(14).IsUnicode(false);
			}


            this.Property(t => t.MainCarriageFromPortId).HasColumnName("MainCarriageFromPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.MainCarriageToPortId).HasColumnName("MainCarriageToPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.MainCarriageCarrierPrefix).HasColumnName("MainCarriageCarrierPrefix").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.MainCarriageCarrierNumber).HasColumnName("MainCarriageCarrierNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.MainCarriageETD).HasColumnName("MainCarriageETD");

            this.Property(t => t.Transshipment1FromPortId).HasColumnName("Transshipment1FromPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Transshipment1ToPortId).HasColumnName("Transshipment1ToPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Transshipment1CarrierId).HasColumnName("Transshipment1CarrierId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Transshipment1ETD).HasColumnName("Transshipment1ETD");

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.Transshipment1AllotmentIdentification).HasColumnName("Transshipment1AllotmentIdentif").HasMaxLength(14).IsUnicode(false);
			}
			else
			{
              this.Property(t => t.Transshipment1AllotmentIdentification).HasColumnName("Transshipment1AllotmentIdentification").HasMaxLength(14).IsUnicode(false);
			}


            this.Property(t => t.Transshipment1CarrierPrefix).HasColumnName("Transshipment1CarrierPrefix").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.Transshipment1CarrierNumber).HasColumnName("Transshipment1CarrierNumber").HasMaxLength(15).IsUnicode(false);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.Transshipment1SpaceAllocationCode).HasColumnName("Transshipment1SpaceAllocationC").HasMaxLength(2).IsUnicode(false);
			}
			else
			{
              this.Property(t => t.Transshipment1SpaceAllocationCode).HasColumnName("Transshipment1SpaceAllocationCode").HasMaxLength(2).IsUnicode(false);
			}


            this.Property(t => t.Transshipment2FromPortId).HasColumnName("Transshipment2FromPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Transshipment2ToPortId).HasColumnName("Transshipment2ToPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Transshipment2CarrierId).HasColumnName("Transshipment2CarrierId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Transshipment2ETD).HasColumnName("Transshipment2ETD");

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.Transshipment2AllotmentIdentification).HasColumnName("Transshipment2AllotmentIdentif").HasMaxLength(14).IsUnicode(false);
			}
			else
			{
              this.Property(t => t.Transshipment2AllotmentIdentification).HasColumnName("Transshipment2AllotmentIdentification").HasMaxLength(14).IsUnicode(false);
			}


            this.Property(t => t.Transshipment2CarrierPrefix).HasColumnName("Transshipment2CarrierPrefix").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.Transshipment2CarrierNumber).HasColumnName("Transshipment2CarrierNumber").HasMaxLength(15).IsUnicode(false);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.Transshipment2SpaceAllocationCode).HasColumnName("Transshipment2SpaceAllocationC").HasMaxLength(2).IsUnicode(false);
			}
			else
			{
              this.Property(t => t.Transshipment2SpaceAllocationCode).HasColumnName("Transshipment2SpaceAllocationCode").HasMaxLength(2).IsUnicode(false);
			}


            this.Property(t => t.FFRStatusCode).HasColumnName("FFRStatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.FFRStatusDate).HasColumnName("FFRStatusDate");

            this.Property(t => t.FNAReason).HasColumnName("FNAReason").HasMaxLength(256).IsUnicode(false);

            this.Property(t => t.ShipperId).HasColumnName("ShipperId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipperAddressId).HasColumnName("ShipperAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipperReference).HasColumnName("ShipperReference").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.ConsigneeId).HasColumnName("ConsigneeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConsigneeAddressId).HasColumnName("ConsigneeAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConsigneeReference).HasColumnName("ConsigneeReference").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.IssuingCarrierAgentId).HasColumnName("IssuingCarrierAgentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IssuingCarrierAddressId).HasColumnName("IssuingCarrierAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IATACodeId).HasColumnName("IATACodeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CASSCode).HasColumnName("CASSCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.AWBSpecialHandlingCodeId1).HasColumnName("AWBSpecialHandlingCodeId1").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AWBSpecialHandlingCodeId2).HasColumnName("AWBSpecialHandlingCodeId2").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AWBSpecialHandlingCodeId3).HasColumnName("AWBSpecialHandlingCodeId3").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AWBSpecialHandlingCodeId4).HasColumnName("AWBSpecialHandlingCodeId4").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AWBSpecialHandlingCodeId5).HasColumnName("AWBSpecialHandlingCodeId5").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AWBSpecialHandlingCodeId6).HasColumnName("AWBSpecialHandlingCodeId6").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AWBSpecialHandlingCodeId7).HasColumnName("AWBSpecialHandlingCodeId7").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AWBSpecialHandlingCodeId8).HasColumnName("AWBSpecialHandlingCodeId8").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AWBSpecialHandlingCodeId9).HasColumnName("AWBSpecialHandlingCodeId9").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AWBCarrierTarrifReference).HasColumnName("AWBCarrierTarrifReference").HasMaxLength(25).IsUnicode(false);

            this.Property(t => t.DescriptionOfGoods).HasColumnName("DescriptionOfGoods").HasMaxLength(1024).IsUnicode(true);

            this.Property(t => t.Notes).HasColumnName("Notes").HasMaxLength(2000).IsUnicode(true);

            this.Property(t => t.SpecialServicesRequest).HasColumnName("SpecialServicesRequest").HasMaxLength(250).IsUnicode(false);

            this.Property(t => t.OtherServicesInformation).HasColumnName("OtherServicesInformation").HasMaxLength(250).IsUnicode(false);

            this.Property(t => t.Routing).HasColumnName("Routing").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.IssuingCarrierIATACode).HasColumnName("IssuingCarrierIATACode").HasMaxLength(7).IsUnicode(false);

            this.Property(t => t.GrossWeightEdited).HasColumnName("GrossWeightEdited").IsRequired();

            this.Property(t => t.ChargeableWeightEdited).HasColumnName("ChargeableWeightEdited").IsRequired();

            this.Property(t => t.NumberOfPackages).HasColumnName("NumberOfPackages");

            this.Property(t => t.GrossWeight).HasColumnName("GrossWeight").HasPrecision(18, 3);

            this.Property(t => t.Volume).HasColumnName("Volume").HasPrecision(18, 3);

            this.Property(t => t.VolumetricWeight).HasColumnName("VolumetricWeight").HasPrecision(18, 3);

            this.Property(t => t.ChargeableWeight).HasColumnName("ChargeableWeight").HasPrecision(18, 3);

            this.Property(t => t.AWBCommodityItemNumber).HasColumnName("AWBCommodityItemNumber").HasMaxLength(7).IsUnicode(false);

            this.Property(t => t.GrossWeightUnitCode).HasColumnName("GrossWeightUnitCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ChargeableWeightUnitCode).HasColumnName("ChargeableWeightUnitCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.DimensionsUnitCode).HasColumnName("DimensionsUnitCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.VolumeUnitCode).HasColumnName("VolumeUnitCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.GrossWeightInKG).HasColumnName("GrossWeightInKG").HasPrecision(18, 3);

            this.Property(t => t.ChargeableWeightInKG).HasColumnName("ChargeableWeightInKG").HasPrecision(18, 3);

            this.Property(t => t.Ratio).HasColumnName("Ratio").HasPrecision(18, 3);

            this.Property(t => t.DimFactor).HasColumnName("DimFactor").HasPrecision(18, 3);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.MainCarriageFinalDestinationPortId).HasColumnName("MainCarriageFinalDestinationPo").HasMaxLength(15).IsUnicode(false);
			}
			else
			{
              this.Property(t => t.MainCarriageFinalDestinationPortId).HasColumnName("MainCarriageFinalDestinationPortId").HasMaxLength(15).IsUnicode(false);
			}


            this.Property(t => t.IsDangerous).HasColumnName("IsDangerous").IsRequired();

            this.Property(t => t.DangerousClassNumber).HasColumnName("DangerousClassNumber").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.DangerousUnNumber).HasColumnName("DangerousUnNumber").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.DangerousPackagingGroup).HasColumnName("DangerousPackagingGroup").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.DangerousIMDGCode).HasColumnName("DangerousIMDGCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.DangerousFlashPoint).HasColumnName("DangerousFlashPoint").HasMaxLength(8).IsUnicode(false);

            this.Property(t => t.DangerousMaterialDescription).HasColumnName("DangerousMaterialDescription").HasMaxLength(200).IsUnicode(true);

            this.Property(t => t.MainHarmonize).HasColumnName("MainHarmonize").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.AWBHandlingInformation).HasColumnName("AWBHandlingInformation").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.AnswerOtherServicesInformation).HasColumnName("AnswerOtherServicesInformation").HasMaxLength(250).IsUnicode(false);

            this.Property(t => t.HasResponse).HasColumnName("HasResponse").IsRequired();

            this.Property(t => t.FMAAcknowledgementReason).HasColumnName("FMAAcknowledgementReason").HasMaxLength(256).IsUnicode(false);

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled").IsRequired();

            this.Property(t => t.HasErrors).HasColumnName("HasErrors").IsRequired();

            this.Property(t => t.WaitingForResponse).HasColumnName("WaitingForResponse").IsRequired();

            this.Property(t => t.InterlineId).HasColumnName("InterlineId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.BookingLevelCode).HasColumnName("BookingLevelCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.AirlinePrefix).HasColumnName("AirlinePrefix").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.BookingProductId).HasColumnName("BookingProductId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LastSentByUserId).HasColumnName("LastSentByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DescriptionOfGoodsId).HasColumnName("DescriptionOfGoodsId").HasMaxLength(15).IsUnicode(true);

            this.Property(t => t.ConcurrencyGUID).HasColumnName("ConcurrencyGUID").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.AccountNumber).HasColumnName("AccountNumber").HasMaxLength(14).IsUnicode(false);

            this.Property(t => t.IsTemperatureSensitive).HasColumnName("IsTemperatureSensitive").IsRequired();

            this.Property(t => t.LastFSRStatusRequestDate).HasColumnName("LastFSRStatusRequestDate");

            this.Property(t => t.UpdatedByPartner).HasColumnName("UpdatedByPartner").HasMaxLength(60).IsUnicode(false);
        }
    }
}
	 