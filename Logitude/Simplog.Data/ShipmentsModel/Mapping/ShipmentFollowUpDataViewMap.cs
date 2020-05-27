using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentModel.Mapping
{
    public class ShipmentFollowUpDataViewMap : EntityTypeConfiguration<ShipmentFollowUpDataView>
    {
        public ShipmentFollowUpDataViewMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.Tenant, t.ShipmentNumber, t.ConsigneeAddressOneTime, t.ShipperAddressOneTime, t.ProfitInLocalCurrency, t.AccountedReceivablesInLocalCurrency, t.OpenReceivablesInLocalCurrency, t.LastUpdateDate, t.UpdatedByUserId, t.IsCancelled, t.IsAccountingClosed, t.LTCWEdited, t.IsDangerous, t.ShipmentStatusId, t.ChargeableWeightEdited, t.GrossWeightEdited, t.OtherPrepaidCollectId, t.FreightPrepaidCollectId, t.OrderIsDangerouseGoods,  t.IsOperationalClosed, t.DirectionId, t.TransportModeId, t.DepartmentId, t.CreateDateTime, t.BranchId, t.CreatedByUserId, t.AWBPrint, t.FollowUpId, t.FollowUpTypeId, t.FollowUpOwnerId });

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.Tenant)
                .HasDatabaseGeneratedOption(null);

            this.Property(t => t.ShipmentNumber)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.ShipperReference1)
                .HasMaxLength(50);

            this.Property(t => t.ShipmentMasterDataId)
                .HasMaxLength(15);

            this.Property(t => t.MainCarriageFromPortId)
                .HasMaxLength(15);

            this.Property(t => t.MainCarriageToPortId)
                .HasMaxLength(15);

            this.Property(t => t.MainCarriageFinalDestinationPortId)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment3CarrierId)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment2CarrierId)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment1CarrierId)
                .HasMaxLength(15);

            this.Property(t => t.MainCarriageCarrierId)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment3AdditionalMAWBOBLBL)
                .HasMaxLength(20);

            this.Property(t => t.Transshipment2AdditionalMAWBOBLBL)
                .HasMaxLength(20);

            this.Property(t => t.Transshipment1AdditionalMAWBOBLBL)
                .HasMaxLength(20);

            this.Property(t => t.Transshipment3VesselId)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment2VesselId)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment1VesselId)
                .HasMaxLength(15);

            this.Property(t => t.MainCarriageVesselId)
                .HasMaxLength(15);

            this.Property(t => t.BookingConfirmedBy)
                .HasMaxLength(40);

            this.Property(t => t.BookingConfirmationNotes)
                .HasMaxLength(250);

            this.Property(t => t.BookingConfirmationNumber)
                .HasMaxLength(25);

            this.Property(t => t.Transshipment3CarrierNumber)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment3ToPortId)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment3FromPortId)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment2CarrierNumber)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment2ToPortId)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment2FromPortId)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment1CarrierNumber)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment1ToPortId)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment1FromPortId)
                .HasMaxLength(15);

            this.Property(t => t.Master)
                .HasMaxLength(20);

            this.Property(t => t.MainCarriageCarrierNumber)
                .HasMaxLength(15);

            this.Property(t => t.ShipperReference2)
                .HasMaxLength(50);

            this.Property(t => t.ToPortId)
                .HasMaxLength(15);

            this.Property(t => t.FromPortId)
                .HasMaxLength(15);

            this.Property(t => t.MasterShipmentDataId)
                .HasMaxLength(15);

            this.Property(t => t.ShipmentLevelCode)
                .HasMaxLength(1);

            this.Property(t => t.NextLegCode)
                .HasMaxLength(4);

            this.Property(t => t.ProfitCurrencyId)
                .HasMaxLength(15);

            this.Property(t => t.SCI)
                .HasMaxLength(1);

            this.Property(t => t.AWBHandlingInformation)
                .HasMaxLength(250);

            this.Property(t => t.AWBInsurrenceValue)
                .HasMaxLength(25);

            this.Property(t => t.AWBAccountingInformation)
                .HasMaxLength(250);

            this.Property(t => t.AWBDeclaredValueForCustoms)
                .HasMaxLength(25);

            this.Property(t => t.AWBDeclaredValueForCarriage)
                .HasMaxLength(25);

            this.Property(t => t.AWBCarrierTarrifReference)
                .HasMaxLength(25);

            this.Property(t => t.FreightForwarderContactId)
                .HasMaxLength(15);

            this.Property(t => t.FreightForwarderAddressId)
                .HasMaxLength(15);

            this.Property(t => t.CustomAgentExportContactId)
                .HasMaxLength(15);

            this.Property(t => t.CustomAgentExportAddressId)
                .HasMaxLength(15);

            this.Property(t => t.ShipmentCustomerTypeCode)
                .HasMaxLength(4);

            this.Property(t => t.CustomerReference1)
                .HasMaxLength(50);
            
            this.Property(t => t.CustomerReference2)
                .HasMaxLength(50);

            this.Property(t => t.CustomerContactId)
                .HasMaxLength(15);

            this.Property(t => t.CustomerAddressId)
                .HasMaxLength(15);

            this.Property(t => t.CustomerId)
                .HasMaxLength(15);

            this.Property(t => t.FreightForwarderReference)
                .HasMaxLength(50);

            this.Property(t => t.FreightForwarderId)
                .HasMaxLength(15);

            this.Property(t => t.CustomAgentExportReference)
                .HasMaxLength(50);

            this.Property(t => t.CustomAgentExportId)
                .HasMaxLength(15);

            this.Property(t => t.CustomAgentImportReference)
                .HasMaxLength(50);

            this.Property(t => t.AWBCurrencyId)
                .HasMaxLength(15);

            this.Property(t => t.OnCarriageCarrierId)
                .HasMaxLength(15);

            this.Property(t => t.PreCarriageCarrierId)
                .HasMaxLength(15);

            this.Property(t => t.UpdatedByUserId)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.ShipmentPayableStatusCode)
                .HasMaxLength(4);

            this.Property(t => t.ShipmentReceivableStatusCode)
                .HasMaxLength(4);

            this.Property(t => t.QuoteId)
                .HasMaxLength(15);

            this.Property(t => t.OnCarriageVesselId)
                .HasMaxLength(15);

            this.Property(t => t.PreCarriageVesselId)
                .HasMaxLength(15);

            this.Property(t => t.DangerousMaterialDescription)
                .HasMaxLength(30);

            this.Property(t => t.DangerousPackagingGroup)
                .HasMaxLength(10);

            this.Property(t => t.DangerousClassNumber)
                .HasMaxLength(10);

            this.Property(t => t.DangerousUnNumber)
                .HasMaxLength(4);

            this.Property(t => t.DangerousIMDGCode)
                .HasMaxLength(4);

            this.Property(t => t.DangerousFlashPoint)
                .HasMaxLength(8);

            this.Property(t => t.MainHarmonize)
                .HasMaxLength(9);



            this.Property(t => t.VolumeUnitCode)
                .HasMaxLength(3);

            this.Property(t => t.DimensionsUnitCode)
                .HasMaxLength(3);

            this.Property(t => t.AgentReference2)
                .HasMaxLength(50);

            this.Property(t => t.AgentReference1)
                .HasMaxLength(50);

            this.Property(t => t.ShipperNotExporterContactId)
                .HasMaxLength(15);

            this.Property(t => t.ConsigneeNotImporterContactId)
                .HasMaxLength(15);

            this.Property(t => t.ConsigneeNotImporterAddressId)
                .HasMaxLength(15);

            this.Property(t => t.ShipperNotExporterAddressId)
                .HasMaxLength(15);

            this.Property(t => t.ConsigneeNotImporterId)
                .HasMaxLength(15);

            this.Property(t => t.ShipperNotExporterId)
                .HasMaxLength(15);

            this.Property(t => t.OtherPrepaidCollectId)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.FreightPrepaidCollectId)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Field10)
                .HasMaxLength(250);

            this.Property(t => t.Field9)
                .HasMaxLength(250);

            this.Property(t => t.Field8)
                .HasMaxLength(250);

            this.Property(t => t.Field7)
                .HasMaxLength(250);

            this.Property(t => t.Field6)
                .HasMaxLength(250);

            this.Property(t => t.Field5)
                .HasMaxLength(250);

            this.Property(t => t.Field4)
                .HasMaxLength(250);

            this.Property(t => t.Field3)
                .HasMaxLength(250);

            this.Property(t => t.Field2)
                .HasMaxLength(250);

            this.Property(t => t.Field1)
                .HasMaxLength(250);

            this.Property(t => t.Field11).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field12).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field13).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field14).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field15).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field16).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field17).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field18).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field19).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field20).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field21).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field22).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field23).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field24).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field25).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field26).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field27).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field28).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field29).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field30).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field31).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field32).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field33).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field34).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field35).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field36).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field37).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field38).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field39).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.Field40).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.From).HasMaxLength(150).IsUnicode(false);
            this.Property(t => t.To).HasMaxLength(150).IsUnicode(false);
            this.Property(t => t.Origin).HasMaxLength(150).IsUnicode(false);

            //this.Property(t => t.LastModified)
            //    .IsRequired()
            //    .IsFixedLength()
            //    .HasMaxLength(8)
            //    .IsRowVersion();

            this.Property(t => t.ConsigneeContactId)
                .HasMaxLength(15);

            this.Property(t => t.AgentContactId)
                .HasMaxLength(15);

            this.Property(t => t.AgentAddressId)
                .HasMaxLength(15);

            this.Property(t => t.CustomAgentImportAddressId)
                .HasMaxLength(15);

            this.Property(t => t.CustomAgentImportContactId)
                .HasMaxLength(15);

            this.Property(t => t.ShipperContactId)
                .HasMaxLength(15);

            this.Property(t => t.Notify2ContactId)
                .HasMaxLength(15);

            this.Property(t => t.Notify1ContactId)
                .HasMaxLength(15);

            this.Property(t => t.Notify2AddressId)
                .HasMaxLength(15);

            this.Property(t => t.Notify1AddressId)
                .HasMaxLength(15);

            this.Property(t => t.AgentId)
                .HasMaxLength(15);

            this.Property(t => t.OnCarriageCarrierNumber)
                .HasMaxLength(15);

            this.Property(t => t.OnCarriageToPortId)
                .HasMaxLength(15);

            this.Property(t => t.OnCarriageFromPortId)
                .HasMaxLength(15);

            this.Property(t => t.OnCarriageTransportModeId)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.PreCarriageCarrierNumber)
                .HasMaxLength(15);

            this.Property(t => t.PreCarriageToPortId)
                .HasMaxLength(15);

            this.Property(t => t.PreCarriageFromPortId)
                .HasMaxLength(15);

            this.Property(t => t.PreCarriageTransportModeId)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.DescriptionOfGoods)
                .HasMaxLength(512);

            this.Property(t => t.Notes)
                .HasMaxLength(250);

            this.Property(t => t.DirectionId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.TransportModeId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.ConsigneeAddressId)
                .HasMaxLength(15);

            this.Property(t => t.ShipperAddressId)
                .HasMaxLength(15);

            this.Property(t => t.Notify2Id)
                .HasMaxLength(15);

            this.Property(t => t.Notify1Id)
                .HasMaxLength(15);

            this.Property(t => t.ConsigneeId)
                .HasMaxLength(15);

            this.Property(t => t.CustomAgentImportId)
                .HasMaxLength(15);

            this.Property(t => t.ShipperId)
                .HasMaxLength(15);

            this.Property(t => t.ShipmentTypeId)
                .HasMaxLength(4);

            this.Property(t => t.DepartmentId)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.SalesmanUserId)
                .HasMaxLength(15);

            this.Property(t => t.IncotermId)
                .HasMaxLength(15);

            this.Property(t => t.BranchId)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.House)
                .HasMaxLength(20);

            this.Property(t => t.ConsigneeReference2)
                .HasMaxLength(50);

            this.Property(t => t.ConsigneeReference1)
                .HasMaxLength(50);

            this.Property(t => t.MainCarriageFromPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.MainCarriageToPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.Transshipment1FromPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.Transshipment1ToPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.Transshipment2FromPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.Transshipment2ToPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.Transshipment3FromPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.Transshipment3ToPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.PreCarriageFromPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.PreCarriageToPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.OnCarriageFromPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.OnCarriageToPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.MainCarriageToPortName)
                .HasMaxLength(40);

            this.Property(t => t.MainCarriageFromPortName)
                .HasMaxLength(40);

            this.Property(t => t.Transshipment1FromPortName)
                .HasMaxLength(40);

            this.Property(t => t.Transshipment1ToPortName)
                .HasMaxLength(40);

            this.Property(t => t.Transshipment2ToPortName)
                .HasMaxLength(40);

            this.Property(t => t.Transshipment2FromPortName)
                .HasMaxLength(40);

            this.Property(t => t.PreCarriageFromPortName)
                .HasMaxLength(40);

            this.Property(t => t.OnCarriageFromPortName)
                .HasMaxLength(40);

            this.Property(t => t.PreCarriageToPortName)
                .HasMaxLength(40);

            this.Property(t => t.Transshipment3FromPortName)
                .HasMaxLength(40);

            this.Property(t => t.Transshipment3ToPortName)
                .HasMaxLength(40);

            this.Property(t => t.OnCarriageToPortName)
                .HasMaxLength(40);

            this.Property(t => t.CustomerName)
                .HasMaxLength(60);

            this.Property(t => t.CustomerNote)
                .HasMaxLength(250);

            this.Property(t => t.FreightForwarderName)
                .HasMaxLength(60);

            this.Property(t => t.FreightForwarderNote)
                .HasMaxLength(250);

            this.Property(t => t.ShipperName)
                .HasMaxLength(60);

            this.Property(t => t.ShipperNote)
                .HasMaxLength(250);

            this.Property(t => t.ConsigneeName)
                .HasMaxLength(60);

            this.Property(t => t.ConsigneeNote)
                .HasMaxLength(250);

            this.Property(t => t.AgentName)
                .HasMaxLength(60);

            this.Property(t => t.AgentNote)
                .HasMaxLength(250);

            this.Property(t => t.CustomAgentExportName)
                .HasMaxLength(60);

            this.Property(t => t.CustomAgentExportNote)
                .HasMaxLength(250);

            this.Property(t => t.CustomAgentImportName)
                .HasMaxLength(60);

            this.Property(t => t.CustomAgentImportNote)
                .HasMaxLength(250);

            this.Property(t => t.Notify1Name)
                .HasMaxLength(60);

            this.Property(t => t.Notify1Note)
                .HasMaxLength(250);

            this.Property(t => t.Notify2Name)
                .HasMaxLength(60);

            this.Property(t => t.Notify2Note)
                .HasMaxLength(250);

            this.Property(t => t.ShipperNotExporterName)
                .HasMaxLength(60);

            this.Property(t => t.ShipperNotExporterNote)
                .HasMaxLength(250);

            this.Property(t => t.ConsigneeNotImporterName)
                .HasMaxLength(60);

            this.Property(t => t.ConsigneeNotImporterNote)
                .HasMaxLength(250);

            this.Property(t => t.ToPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.ToPortName)
                .HasMaxLength(40);

            this.Property(t => t.FromPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.FromPortName)
                .HasMaxLength(40);

            this.Property(t => t.MainCarriageFromPortCountryCode)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.MainCarriageFromPortCountryName)
                .HasMaxLength(120);

            this.Property(t => t.MainCarriageToPortCountryCode)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.MainCarriageToPortCountryName)
                .HasMaxLength(120);

            this.Property(t => t.Transshipment1FromPortCountryCode)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.Transshipment1FromPortCountryName)
                .HasMaxLength(120);

            this.Property(t => t.Transshipment2FromPortCountryCode)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.Transshipment2FromPortCountryName)
                .HasMaxLength(120);

            this.Property(t => t.Transshipment3FromPortCountryCode)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.Transshipment3FromPortCountryName)
                .HasMaxLength(120);

            this.Property(t => t.PreCarriageFromPortCountryCode)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.PreCarriageFromPortCountryName)
                .HasMaxLength(120);

            this.Property(t => t.OnCarriageFromPortCountryCode)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.OnCarriageFromPortCountryName)
                .HasMaxLength(120);

            this.Property(t => t.Transshipment1ToPortCountryCode)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.Transshipment1ToPortCountryName)
                .HasMaxLength(120);

            this.Property(t => t.OnCarriageToPortCountryCode)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.OnCarriageToPortCountryName)
                .HasMaxLength(120);

            this.Property(t => t.Transshipment2ToPortCountryCode)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.Transshipment2ToPortCountryName)
                .HasMaxLength(120);

            this.Property(t => t.PreCarriageToPortCountryCode)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.PreCarriageToPortCountryName)
                .HasMaxLength(120);

            this.Property(t => t.Transshipment3ToPortCountryCode)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.Transshipment3ToPortCountryName)
                .HasMaxLength(120);

            this.Property(t => t.MainCarriageCarrierName)
                .HasMaxLength(60);

            this.Property(t => t.MainCarriageCarrierCode)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment1CarrierName)
                .HasMaxLength(60);

            this.Property(t => t.Transshipment1CarrierCode)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment2CarrierName)
                .HasMaxLength(60);

            this.Property(t => t.Transshipment2CarrierCode)
                .HasMaxLength(15);

            this.Property(t => t.Transshipment3CarrierName)
                .HasMaxLength(60);

            this.Property(t => t.Transshipment3CarrierCode)
                .HasMaxLength(15);

            this.Property(t => t.PreCarriageCarrierName)
                .HasMaxLength(60);

            this.Property(t => t.PreCarriageCarrierCode)
                .HasMaxLength(15);

            this.Property(t => t.OnCarriageCarrierName)
                .HasMaxLength(60);

            this.Property(t => t.OnCarriageCarrierCode)
                .HasMaxLength(15);

            this.Property(t => t.NextLegName)
                .HasMaxLength(40);

            this.Property(t => t.DirectionName)
                .HasMaxLength(10);

            this.Property(t => t.TransportModeName)
                .HasMaxLength(10);

            this.Property(t => t.ShipmentTypeName)
                .HasMaxLength(40);

            this.Property(t => t.ShipmentReceivableStatusName)
                .HasMaxLength(40);

            this.Property(t => t.ShipmentPayableStatusName)
                .HasMaxLength(40);

            this.Property(t => t.AWBCurrencyCode)
                .HasMaxLength(3);

            this.Property(t => t.BranchName)
                .HasMaxLength(40);

            this.Property(t => t.ShipmentLevelName)
                .HasMaxLength(40);

            this.Property(t => t.MainCarriageFinalDestinationPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.MainCarriageFinalDestinationPortName)
                .HasMaxLength(40);

            this.Property(t => t.MasterShipmentNumber)
                .HasMaxLength(15);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000);

            this.Property(t => t.MainCarriageAirlinePrefix)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.CreatedByUserId)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.GrossWeightUnitCode)
                .HasMaxLength(3);

            this.Property(t => t.ChargeableWeightUnitCode)
                .HasMaxLength(3);

            this.Property(t => t.IssuingCarrierAgentId)
                .HasMaxLength(15);

            this.Property(t => t.IncotermCode)
                .HasMaxLength(3);

            this.Property(t => t.FHLStatusCode)
                .HasMaxLength(4);

            this.Property(t => t.FWBStatusCode)
                .HasMaxLength(4);

            this.Property(t => t.FHLStatusName)
                .HasMaxLength(40);

            this.Property(t => t.FWBStatusName)
                .HasMaxLength(40);

            this.Property(t => t.CarrierLastStatusName)
                .HasMaxLength(40);

            this.Property(t => t.FNAReason)
                .HasMaxLength(40);

            this.Property(t => t.Routing)
                .HasMaxLength(100);

            this.Property(t => t.TruckNumber)
                .HasMaxLength(15);

            this.Property(t => t.FollowUpId)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.FollowUpNotes)
                .HasMaxLength(250);

            this.Property(t => t.FollowUpTypeId)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.FollowUpOwnerId)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.FollowUpOwner)
                .HasMaxLength(40);

            this.Property(t => t.FollowUpType)
                .HasMaxLength(40);

            this.Property(t => t.CarrierLastStatusCode)
                .HasMaxLength(4);

            this.Property(t => t.FromPortCountryCode)
                .HasMaxLength(2);

            this.Property(t => t.ToPortCountryCode)
                .HasMaxLength(2);

            this.Property(t => t.AccountNumber)
                .HasMaxLength(14)
                .IsUnicode(false);

            this.Property(t => t.ProductCode)
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Ignore(d => d.ShipmentFollowUpId);
            this.Ignore(d => d.CurrentUserId);
            this.Ignore(d => d.BasketId);
            this.Ignore(d => d.MasterId);
            this.Ignore(d => d.MAWBTakenFromStack);
            this.Ignore(d => d.MAWBReturnedToStack);
            this.Ignore(d => d.MAWBStackNumber);
            this.Ignore(d => d.LongMaster);
            this.Ignore(d => d.TotalContainers);
            this.Ignore(d => d.ShipmentType);
            this.Ignore(d => d.ShipmentPMId);
            this.Ignore(d => d.LastUpdate);
            this.Ignore(d => d.NewMessage);
            this.Ignore(d => d.IsAnyConversation);
            this.Ignore(d => d.NumberOfShipments);
            this.Ignore(d => d.AccessDate);
            this.Ignore(d => d.UpdatedByUserName);
            this.Ignore(d => d.EventNote);
            this.Ignore(d => d.PreCarriageCarrierWebSite);
            this.Ignore(d => d.OnCarriageCarrierWebSite);
            this.Ignore(d => d.MainCarriageTransportModeId);
            this.Ignore(d => d.MainCarriageCarrierWebSite);
            this.Ignore(d => d.Transshipment1CarrierWebSite);
            this.Ignore(d => d.Transshipment2CarrierWebSite);
            this.Ignore(d => d.Transshipment3CarrierWebSite);
            this.Ignore(d => d.FinalDistenationPortId);
            this.Ignore(d => d.FromPort);
            this.Ignore(d => d.ToPort);
            this.Property(t => t.CargonautFHLStatusCode).HasMaxLength(4);
            this.Property(t => t.CargonautFWBStatusCode).HasMaxLength(4);
            this.Property(t => t.CargonautFHLStatusName).HasMaxLength(40);
            this.Property(t => t.CargonautFWBStatusName).HasMaxLength(40);
            this.Property(t => t.NumberOfInsidePackages).IsRequired();
            this.Property(t => t.NumberOfInsidePackagesDetails).HasMaxLength(500).IsUnicode(false);
            this.Property(t => t.ConsolidatorId).HasMaxLength(15);
            this.Property(t => t.ConsolidatorReference).HasMaxLength(50);
            this.Property(t => t.ConsolidatorContactId).HasMaxLength(15);
            this.Property(t => t.ConsolidatorAddressId).HasMaxLength(15);
            this.Property(t => t.ConsolidatorName).HasMaxLength(60);
            this.Property(t => t.ConsolidatorNote).HasMaxLength(250);
            this.Property(t => t.ManifestReason).HasMaxLength(500).IsUnicode(false);
            this.Property(t => t.ManifestStatusCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.AirlinePrefix).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.StatusId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentStatusId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentStatusName).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.ShipmentStatusLocation).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.ShipmentMasterDataStatusId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentMasterDataStatusName).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.ShipmentMasterDataStatusLocation).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.CustomsDeclarationNumber).HasMaxLength(35).IsUnicode(false);
            this.Property(t => t.ISFNumber).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.ITNumber).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.OBLTypeCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.ENSNumber).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.LastSharedEventId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.LastSharedEventName).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.LastSharedEventLocation).HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.LastSharedEventNotes).HasMaxLength(4000).IsUnicode(true);
            this.Property(t => t.INTTRASIStatusCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.INTTRASIStatusName).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.INTTRASIError).HasMaxLength(256).IsUnicode(false);
            this.Property(t => t.MainCarriageFromCity).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.MainCarriageToCity).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.LastFinalDestination).HasMaxLength(150).IsUnicode(false);
            this.Property(t => t.Notify1Reference).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.Notify2Reference).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.ShipperNotExporterReference).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.ConsigneeNotImporterReference).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.ARInvoices).HasMaxLength(1000).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentFollowUpDataView");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentNumber).HasColumnName("ShipmentNumber");
            this.Property(t => t.ShipperReference1).HasColumnName("ShipperReference1");
            this.Property(t => t.ShipmentMasterDataTenant).HasColumnName("ShipmentMasterDataTenant");
            this.Property(t => t.ShipmentMasterDataId).HasColumnName("ShipmentMasterDataId");
            this.Property(t => t.MainCarriageFromPortId).HasColumnName("MainCarriageFromPortId");
            this.Property(t => t.MainCarriageToPortId).HasColumnName("MainCarriageToPortId");
            this.Property(t => t.MainCarriageFinalDestinationPortId).HasColumnName("MainCarriageFinalDestinationPortId");
            this.Property(t => t.Transshipment3CarrierId).HasColumnName("Transshipment3CarrierId");
            this.Property(t => t.Transshipment2CarrierId).HasColumnName("Transshipment2CarrierId");
            this.Property(t => t.Transshipment1CarrierId).HasColumnName("Transshipment1CarrierId");
            this.Property(t => t.MainCarriageCarrierId).HasColumnName("MainCarriageCarrierId");
            this.Property(t => t.MainCarriageIsFromStack).HasColumnName("MainCarriageIsFromStack");
            this.Property(t => t.Transshipment3AdditionalMAWBOBLBL).HasColumnName("Transshipment3AdditionalMAWBOBLBL");
            this.Property(t => t.Transshipment2AdditionalMAWBOBLBL).HasColumnName("Transshipment2AdditionalMAWBOBLBL");
            this.Property(t => t.Transshipment1AdditionalMAWBOBLBL).HasColumnName("Transshipment1AdditionalMAWBOBLBL");
            this.Property(t => t.Transshipment3VesselId).HasColumnName("Transshipment3VesselId");
            this.Property(t => t.Transshipment2VesselId).HasColumnName("Transshipment2VesselId");
            this.Property(t => t.Transshipment1VesselId).HasColumnName("Transshipment1VesselId");
            this.Property(t => t.MainCarriageVesselId).HasColumnName("MainCarriageVesselId");
            this.Property(t => t.BookingConfirmedBy).HasColumnName("BookingConfirmedBy");
            this.Property(t => t.BookingConfirmationNotes).HasColumnName("BookingConfirmationNotes");
            this.Property(t => t.BookingConfirmationNumber).HasColumnName("BookingConfirmationNumber");
            this.Property(t => t.MAWBOBLDate).HasColumnName("MAWBOBLDate");
            this.Property(t => t.Transshipment3CarrierNumber).HasColumnName("Transshipment3CarrierNumber");
            this.Property(t => t.Transshipment3ETA).HasColumnName("Transshipment3ETA");
            this.Property(t => t.Transshipment3ETD).HasColumnName("Transshipment3ETD");
            this.Property(t => t.Transshipment3ATA).HasColumnName("Transshipment3ATA");
            this.Property(t => t.Transshipment3ATD).HasColumnName("Transshipment3ATD");
            this.Property(t => t.Transshipment3ToPortId).HasColumnName("Transshipment3ToPortId");
            this.Property(t => t.Transshipment3FromPortId).HasColumnName("Transshipment3FromPortId");
            this.Property(t => t.Transshipment2CarrierNumber).HasColumnName("Transshipment2CarrierNumber");
            this.Property(t => t.Transshipment2ETA).HasColumnName("Transshipment2ETA");
            this.Property(t => t.Transshipment2ETD).HasColumnName("Transshipment2ETD");
            this.Property(t => t.Transshipment2ATA).HasColumnName("Transshipment2ATA");
            this.Property(t => t.Transshipment2ATD).HasColumnName("Transshipment2ATD");
            this.Property(t => t.Transshipment2ToPortId).HasColumnName("Transshipment2ToPortId");
            this.Property(t => t.Transshipment2FromPortId).HasColumnName("Transshipment2FromPortId");
            this.Property(t => t.Transshipment1CarrierNumber).HasColumnName("Transshipment1CarrierNumber");
            this.Property(t => t.Transshipment1ETA).HasColumnName("Transshipment1ETA");
            this.Property(t => t.Transshipment1ETD).HasColumnName("Transshipment1ETD");
            this.Property(t => t.Transshipment1ATA).HasColumnName("Transshipment1ATA");
            this.Property(t => t.Transshipment1ATD).HasColumnName("Transshipment1ATD");
            this.Property(t => t.Transshipment1ToPortId).HasColumnName("Transshipment1ToPortId");
            this.Property(t => t.Transshipment1FromPortId).HasColumnName("Transshipment1FromPortId");
            this.Property(t => t.Master).HasColumnName("Master");
            this.Property(t => t.MainCarriageCarrierNumber).HasColumnName("MainCarriageCarrierNumber");
            this.Property(t => t.MainCarriageETD).HasColumnName("MainCarriageETD");
            this.Property(t => t.MainCarriageETA).HasColumnName("MainCarriageETA");
            this.Property(t => t.MainCarriageATA).HasColumnName("MainCarriageATA");
            this.Property(t => t.MainCarriageATD).HasColumnName("MainCarriageATD");
            this.Property(t => t.ShipperReference2).HasColumnName("ShipperReference2");
            this.Property(t => t.ToPortId).HasColumnName("ToPortId");
            this.Property(t => t.FromPortId).HasColumnName("FromPortId");
            this.Property(t => t.MasterShipmentDataId).HasColumnName("MasterShipmentDataId");
            this.Property(t => t.ShipmentLevelCode).HasColumnName("ShipmentLevelCode");
            this.Property(t => t.NextETA).HasColumnName("NextETA");
            this.Property(t => t.NextETD).HasColumnName("NextETD");
            this.Property(t => t.NextLegCode).HasColumnName("NextLegCode");
            this.Property(t => t.AccountedReceivablesInProfitCurrency).HasColumnName("AccountedReceivablesInProfitCurrency");
            this.Property(t => t.OpenReceivablesInProfitCurrency).HasColumnName("OpenReceivablesInProfitCurrency");
            this.Property(t => t.ProfitInProfitCurrency).HasColumnName("ProfitInProfitCurrency");
            this.Property(t => t.EstimateProfitInProfitCurrency).HasColumnName("EstimateProfitInProfitCurrency");
            this.Property(t => t.ProfitCurrencyId).HasColumnName("ProfitCurrencyId");
            this.Property(t => t.SCI).HasColumnName("SCI");
            this.Property(t => t.AWBHandlingInformation).HasColumnName("AWBHandlingInformation");
            this.Property(t => t.AWBInsurrenceValue).HasColumnName("AWBInsurrenceValue");
            this.Property(t => t.AWBAccountingInformation).HasColumnName("AWBAccountingInformation");
            this.Property(t => t.AWBDeclaredValueForCustoms).HasColumnName("AWBDeclaredValueForCustoms");
            this.Property(t => t.AWBDeclaredValueForCarriage).HasColumnName("AWBDeclaredValueForCarriage");
            this.Property(t => t.AWBCarrierTarrifReference).HasColumnName("AWBCarrierTarrifReference");
            this.Property(t => t.FreightForwarderContactId).HasColumnName("FreightForwarderContactId");
            this.Property(t => t.FreightForwarderAddressId).HasColumnName("FreightForwarderAddressId");
            this.Property(t => t.CustomAgentExportContactId).HasColumnName("CustomAgentExportContactId");
            this.Property(t => t.CustomAgentExportAddressId).HasColumnName("CustomAgentExportAddressId");
            this.Property(t => t.ShipmentCustomerTypeCode).HasColumnName("ShipmentCustomerTypeCode");
            this.Property(t => t.CustomerReference1).HasColumnName("CustomerReference1");
            this.Property(t => t.CustomerReference2).HasColumnName("CustomerReference2");
            this.Property(t => t.CustomerContactId).HasColumnName("CustomerContactId");
            this.Property(t => t.CustomerAddressId).HasColumnName("CustomerAddressId");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.FreightForwarderReference).HasColumnName("FreightForwarderReference");
            this.Property(t => t.FreightForwarderId).HasColumnName("FreightForwarderId");
            this.Property(t => t.CustomAgentExportReference).HasColumnName("CustomAgentExportReference");
            this.Property(t => t.CustomAgentExportId).HasColumnName("CustomAgentExportId");
            this.Property(t => t.CustomAgentImportReference).HasColumnName("CustomAgentImportReference");
            this.Property(t => t.ConsigneeAddressOneTime).HasColumnName("ConsigneeAddressOneTime");
            this.Property(t => t.ShipperAddressOneTime).HasColumnName("ShipperAddressOneTime");
            this.Property(t => t.EstimateProfitInLocalCurrency).HasColumnName("EstimateProfitInLocalCurrency");
            this.Property(t => t.AWBCurrencyId).HasColumnName("AWBCurrencyId");
            this.Property(t => t.OrderChargeableWeight).HasColumnName("OrderChargeableWeight");
            this.Property(t => t.OnCarriageCarrierId).HasColumnName("OnCarriageCarrierId");
            this.Property(t => t.PreCarriageCarrierId).HasColumnName("PreCarriageCarrierId");
            this.Property(t => t.ProfitInLocalCurrency).HasColumnName("ProfitInLocalCurrency");
            this.Property(t => t.AccountedReceivablesInLocalCurrency).HasColumnName("AccountedReceivablesInLocalCurrency");
            this.Property(t => t.OpenReceivablesInLocalCurrency).HasColumnName("OpenReceivablesInLocalCurrency");
            this.Property(t => t.LastUpdateDate).HasColumnName("LastUpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");
            this.Property(t => t.IsAccountingClosed).HasColumnName("IsAccountingClosed");
            this.Property(t => t.ShipmentPayableStatusCode).HasColumnName("ShipmentPayableStatusCode");
            this.Property(t => t.ShipmentReceivableStatusCode).HasColumnName("ShipmentReceivableStatusCode");
            this.Property(t => t.QuoteId).HasColumnName("QuoteId");
            this.Property(t => t.ShipmentDeliveryIndex).HasColumnName("ShipmentDeliveryIndex");
            this.Property(t => t.ShipmentPickUpIndex).HasColumnName("ShipmentPickUpIndex");
            this.Property(t => t.LTCWEdited).HasColumnName("LTCWEdited");
            this.Property(t => t.OnCarriageVesselId).HasColumnName("OnCarriageVesselId");
            this.Property(t => t.PreCarriageVesselId).HasColumnName("PreCarriageVesselId");
            this.Property(t => t.DangerousMaterialDescription).HasColumnName("DangerousMaterialDescription");
            this.Property(t => t.DangerousPackagingGroup).HasColumnName("DangerousPackagingGroup");
            this.Property(t => t.DangerousClassNumber).HasColumnName("DangerousClassNumber");
            this.Property(t => t.DangerousUnNumber).HasColumnName("DangerousUnNumber");
            this.Property(t => t.DangerousIMDGCode).HasColumnName("DangerousIMDGCode");
            this.Property(t => t.DangerousFlashPoint).HasColumnName("DangerousFlashPoint");
            this.Property(t => t.AWBFreightAmountPrepaid).HasColumnName("AWBFreightAmountPrepaid");
            this.Property(t => t.AWBFreightAmountCollect).HasColumnName("AWBFreightAmountCollect");
            this.Property(t => t.IsDangerous).HasColumnName("IsDangerous");
            this.Property(t => t.MainHarmonize).HasColumnName("MainHarmonize");
            this.Property(t => t.VolumeUnitCode).HasColumnName("VolumeUnitCode");
            this.Property(t => t.ChargeableWeightEdited).HasColumnName("ChargeableWeightEdited");
            this.Property(t => t.GrossWeightEdited).HasColumnName("GrossWeightEdited");
            this.Property(t => t.Ratio).HasColumnName("Ratio");
            this.Property(t => t.PackagesQuantity).HasColumnName("PackagesQuantity");
            this.Property(t => t.NumberOfPackages).HasColumnName("NumberOfPackages");
            this.Property(t => t.NumberOfContainers).HasColumnName("NumberOfContainers");
            this.Property(t => t.NumberOfFollowUps).HasColumnName("NumberOfFollowUps");
            this.Property(t => t.VolumeInCBM).HasColumnName("VolumeInCBM");
            this.Property(t => t.DimensionsUnitCode).HasColumnName("DimensionsUnitCode");
            this.Property(t => t.AgentReference2).HasColumnName("AgentReference2");
            this.Property(t => t.AgentReference1).HasColumnName("AgentReference1");
            this.Property(t => t.ShipperNotExporterContactId).HasColumnName("ShipperNotExporterContactId");
            this.Property(t => t.ConsigneeNotImporterContactId).HasColumnName("ConsigneeNotImporterContactId");
            this.Property(t => t.ConsigneeNotImporterAddressId).HasColumnName("ConsigneeNotImporterAddressId");
            this.Property(t => t.ShipperNotExporterAddressId).HasColumnName("ShipperNotExporterAddressId");
            this.Property(t => t.ConsigneeNotImporterId).HasColumnName("ConsigneeNotImporterId");
            this.Property(t => t.ShipperNotExporterId).HasColumnName("ShipperNotExporterId");
            this.Property(t => t.OtherPrepaidCollectId).HasColumnName("OtherPrepaidCollectId");
            this.Property(t => t.FreightPrepaidCollectId).HasColumnName("FreightPrepaidCollectId");
            this.Property(t => t.OrderIsDangerouseGoods).HasColumnName("OrderIsDangerouseGoods");
            this.Property(t => t.BookingNumberOfPackages).HasColumnName("BookingNumberOfPackages");
            this.Property(t => t.BookingVolume).HasColumnName("BookingVolume");
            this.Property(t => t.OrderGrossWeight).HasColumnName("OrderGrossWeight");
            this.Property(t => t.Field10).HasColumnName("Field10");
            this.Property(t => t.Field9).HasColumnName("Field9");
            this.Property(t => t.Field8).HasColumnName("Field8");
            this.Property(t => t.Field7).HasColumnName("Field7");
            this.Property(t => t.Field6).HasColumnName("Field6");
            this.Property(t => t.Field5).HasColumnName("Field5");
            this.Property(t => t.Field4).HasColumnName("Field4");
            this.Property(t => t.Field3).HasColumnName("Field3");
            this.Property(t => t.Field2).HasColumnName("Field2");
            this.Property(t => t.Field1).HasColumnName("Field1");
            this.Property(t => t.Field11).HasColumnName("Field11");
            this.Property(t => t.Field12).HasColumnName("Field12");
            this.Property(t => t.Field13).HasColumnName("Field13");
            this.Property(t => t.Field14).HasColumnName("Field14");
            this.Property(t => t.Field15).HasColumnName("Field15");
            this.Property(t => t.Field16).HasColumnName("Field16");
            this.Property(t => t.Field17).HasColumnName("Field17");
            this.Property(t => t.Field18).HasColumnName("Field18");
            this.Property(t => t.Field19).HasColumnName("Field19");
            this.Property(t => t.Field20).HasColumnName("Field20");
            this.Property(t => t.Field21).HasColumnName("Field21");
            this.Property(t => t.Field22).HasColumnName("Field22");
            this.Property(t => t.Field23).HasColumnName("Field23");
            this.Property(t => t.Field24).HasColumnName("Field24");
            this.Property(t => t.Field25).HasColumnName("Field25");
            this.Property(t => t.Field26).HasColumnName("Field26");
            this.Property(t => t.Field27).HasColumnName("Field27");
            this.Property(t => t.Field28).HasColumnName("Field28");
            this.Property(t => t.Field29).HasColumnName("Field29");
            this.Property(t => t.Field30).HasColumnName("Field30");
            this.Property(t => t.Field31).HasColumnName("Field31");
            this.Property(t => t.Field32).HasColumnName("Field32");
            this.Property(t => t.Field33).HasColumnName("Field33");
            this.Property(t => t.Field34).HasColumnName("Field34");
            this.Property(t => t.Field35).HasColumnName("Field35");
            this.Property(t => t.Field36).HasColumnName("Field36");
            this.Property(t => t.Field37).HasColumnName("Field37");
            this.Property(t => t.Field38).HasColumnName("Field38");
            this.Property(t => t.Field39).HasColumnName("Field39");
            this.Property(t => t.Field40).HasColumnName("Field40");
            this.Property(t => t.GrossWeight).HasColumnName("GrossWeight");
            this.Property(t => t.ChargeableWeight).HasColumnName("ChargeableWeight");
            this.Property(t => t.IsOperationalClosed).HasColumnName("IsOperationalClosed");
            this.Property(t => t.ConsigneeContactId).HasColumnName("ConsigneeContactId");
            this.Property(t => t.AgentContactId).HasColumnName("AgentContactId");
            this.Property(t => t.AgentAddressId).HasColumnName("AgentAddressId");
            this.Property(t => t.CustomAgentImportAddressId).HasColumnName("CustomAgentImportAddressId");
            this.Property(t => t.CustomAgentImportContactId).HasColumnName("CustomAgentImportContactId");
            this.Property(t => t.ShipperContactId).HasColumnName("ShipperContactId");
            this.Property(t => t.Notify2ContactId).HasColumnName("Notify2ContactId");
            this.Property(t => t.Notify1ContactId).HasColumnName("Notify1ContactId");
            this.Property(t => t.Notify2AddressId).HasColumnName("Notify2AddressId");
            this.Property(t => t.Notify1AddressId).HasColumnName("Notify1AddressId");
            this.Property(t => t.PreCarriageETD).HasColumnName("PreCarriageETD");
            this.Property(t => t.PreCarriageETA).HasColumnName("PreCarriageETA");
            this.Property(t => t.OnCarriageETA).HasColumnName("OnCarriageETA");
            this.Property(t => t.OnCarriageETD).HasColumnName("OnCarriageETD");
            this.Property(t => t.AgentId).HasColumnName("AgentId");
            this.Property(t => t.OnCarriageCarrierNumber).HasColumnName("OnCarriageCarrierNumber");
            this.Property(t => t.OnCarriageATA).HasColumnName("OnCarriageATA");
            this.Property(t => t.OnCarriageATD).HasColumnName("OnCarriageATD");
            this.Property(t => t.OnCarriageToPortId).HasColumnName("OnCarriageToPortId");
            this.Property(t => t.OnCarriageFromPortId).HasColumnName("OnCarriageFromPortId");
            this.Property(t => t.OnCarriageTransportModeId).HasColumnName("OnCarriageTransportModeId");
            this.Property(t => t.PreCarriageCarrierNumber).HasColumnName("PreCarriageCarrierNumber");
            this.Property(t => t.PreCarriageATA).HasColumnName("PreCarriageATA");
            this.Property(t => t.PreCarriageATD).HasColumnName("PreCarriageATD");
            this.Property(t => t.PreCarriageToPortId).HasColumnName("PreCarriageToPortId");
            this.Property(t => t.PreCarriageFromPortId).HasColumnName("PreCarriageFromPortId");
            this.Property(t => t.PreCarriageTransportModeId).HasColumnName("PreCarriageTransportModeId");
            this.Property(t => t.HAWBDate).HasColumnName("HAWBDate");
            this.Property(t => t.DescriptionOfGoods).HasColumnName("DescriptionOfGoods");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.DirectionId).HasColumnName("DirectionId");
            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId");
            this.Property(t => t.ConsigneeAddressId).HasColumnName("ConsigneeAddressId");
            this.Property(t => t.ShipperAddressId).HasColumnName("ShipperAddressId");
            this.Property(t => t.Notify2Id).HasColumnName("Notify2Id");
            this.Property(t => t.Notify1Id).HasColumnName("Notify1Id");
            this.Property(t => t.ConsigneeId).HasColumnName("ConsigneeId");
            this.Property(t => t.CustomAgentImportId).HasColumnName("CustomAgentImportId");
            this.Property(t => t.ShipperId).HasColumnName("ShipperId");
            this.Property(t => t.ShipmentTypeId).HasColumnName("ShipmentTypeId");
            this.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");
            this.Property(t => t.SalesmanUserId).HasColumnName("SalesmanUserId");
            this.Property(t => t.IncotermId).HasColumnName("IncotermId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.House).HasColumnName("House");
            this.Property(t => t.ConsigneeReference2).HasColumnName("ConsigneeReference2");
            this.Property(t => t.ConsigneeReference1).HasColumnName("ConsigneeReference1");
            this.Property(t => t.MainCarriageFromPortCode).HasColumnName("MainCarriageFromPortCode");
            this.Property(t => t.MainCarriageToPortCode).HasColumnName("MainCarriageToPortCode");
            this.Property(t => t.Transshipment1FromPortCode).HasColumnName("Transshipment1FromPortCode");
            this.Property(t => t.Transshipment1ToPortCode).HasColumnName("Transshipment1ToPortCode");
            this.Property(t => t.Transshipment2FromPortCode).HasColumnName("Transshipment2FromPortCode");
            this.Property(t => t.Transshipment2ToPortCode).HasColumnName("Transshipment2ToPortCode");
            this.Property(t => t.Transshipment3FromPortCode).HasColumnName("Transshipment3FromPortCode");
            this.Property(t => t.Transshipment3ToPortCode).HasColumnName("Transshipment3ToPortCode");
            this.Property(t => t.PreCarriageFromPortCode).HasColumnName("PreCarriageFromPortCode");
            this.Property(t => t.PreCarriageToPortCode).HasColumnName("PreCarriageToPortCode");
            this.Property(t => t.OnCarriageFromPortCode).HasColumnName("OnCarriageFromPortCode");
            this.Property(t => t.OnCarriageToPortCode).HasColumnName("OnCarriageToPortCode");
            this.Property(t => t.MainCarriageToPortName).HasColumnName("MainCarriageToPortName");
            this.Property(t => t.MainCarriageFromPortName).HasColumnName("MainCarriageFromPortName");
            this.Property(t => t.Transshipment1FromPortName).HasColumnName("Transshipment1FromPortName");
            this.Property(t => t.Transshipment1ToPortName).HasColumnName("Transshipment1ToPortName");
            this.Property(t => t.Transshipment2ToPortName).HasColumnName("Transshipment2ToPortName");
            this.Property(t => t.Transshipment2FromPortName).HasColumnName("Transshipment2FromPortName");
            this.Property(t => t.PreCarriageFromPortName).HasColumnName("PreCarriageFromPortName");
            this.Property(t => t.OnCarriageFromPortName).HasColumnName("OnCarriageFromPortName");
            this.Property(t => t.PreCarriageToPortName).HasColumnName("PreCarriageToPortName");
            this.Property(t => t.Transshipment3FromPortName).HasColumnName("Transshipment3FromPortName");
            this.Property(t => t.Transshipment3ToPortName).HasColumnName("Transshipment3ToPortName");
            this.Property(t => t.OnCarriageToPortName).HasColumnName("OnCarriageToPortName");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.CustomerNote).HasColumnName("CustomerNote");
            this.Property(t => t.FreightForwarderName).HasColumnName("FreightForwarderName");
            this.Property(t => t.FreightForwarderNote).HasColumnName("FreightForwarderNote");
            this.Property(t => t.ShipperName).HasColumnName("ShipperName");
            this.Property(t => t.ShipperNote).HasColumnName("ShipperNote");
            this.Property(t => t.ConsigneeName).HasColumnName("ConsigneeName");
            this.Property(t => t.ConsigneeNote).HasColumnName("ConsigneeNote");
            this.Property(t => t.AgentName).HasColumnName("AgentName");
            this.Property(t => t.AgentNote).HasColumnName("AgentNote");
            this.Property(t => t.CustomAgentExportName).HasColumnName("CustomAgentExportName");
            this.Property(t => t.CustomAgentExportNote).HasColumnName("CustomAgentExportNote");
            this.Property(t => t.CustomAgentImportName).HasColumnName("CustomAgentImportName");
            this.Property(t => t.CustomAgentImportNote).HasColumnName("CustomAgentImportNote");
            this.Property(t => t.Notify1Name).HasColumnName("Notify1Name");
            this.Property(t => t.Notify1Note).HasColumnName("Notify1Note");
            this.Property(t => t.Notify2Name).HasColumnName("Notify2Name");
            this.Property(t => t.Notify2Note).HasColumnName("Notify2Note");
            this.Property(t => t.ShipperNotExporterName).HasColumnName("ShipperNotExporterName");
            this.Property(t => t.ShipperNotExporterNote).HasColumnName("ShipperNotExporterNote");
            this.Property(t => t.ConsigneeNotImporterName).HasColumnName("ConsigneeNotImporterName");
            this.Property(t => t.ConsigneeNotImporterNote).HasColumnName("ConsigneeNotImporterNote");
            this.Property(t => t.ToPortCode).HasColumnName("ToPortCode");
            this.Property(t => t.ToPortName).HasColumnName("ToPortName");
            this.Property(t => t.FromPortCode).HasColumnName("FromPortCode");
            this.Property(t => t.FromPortName).HasColumnName("FromPortName");
            this.Property(t => t.MainCarriageFromPortCountryCode).HasColumnName("MainCarriageFromPortCountryCode");
            this.Property(t => t.MainCarriageFromPortCountryName).HasColumnName("MainCarriageFromPortCountryName");
            this.Property(t => t.MainCarriageToPortCountryCode).HasColumnName("MainCarriageToPortCountryCode");
            this.Property(t => t.MainCarriageToPortCountryName).HasColumnName("MainCarriageToPortCountryName");
            this.Property(t => t.Transshipment1FromPortCountryCode).HasColumnName("Transshipment1FromPortCountryCode");
            this.Property(t => t.Transshipment1FromPortCountryName).HasColumnName("Transshipment1FromPortCountryName");
            this.Property(t => t.Transshipment2FromPortCountryCode).HasColumnName("Transshipment2FromPortCountryCode");
            this.Property(t => t.Transshipment2FromPortCountryName).HasColumnName("Transshipment2FromPortCountryName");
            this.Property(t => t.Transshipment3FromPortCountryCode).HasColumnName("Transshipment3FromPortCountryCode");
            this.Property(t => t.Transshipment3FromPortCountryName).HasColumnName("Transshipment3FromPortCountryName");
            this.Property(t => t.PreCarriageFromPortCountryCode).HasColumnName("PreCarriageFromPortCountryCode");
            this.Property(t => t.PreCarriageFromPortCountryName).HasColumnName("PreCarriageFromPortCountryName");
            this.Property(t => t.OnCarriageFromPortCountryCode).HasColumnName("OnCarriageFromPortCountryCode");
            this.Property(t => t.OnCarriageFromPortCountryName).HasColumnName("OnCarriageFromPortCountryName");
            this.Property(t => t.Transshipment1ToPortCountryCode).HasColumnName("Transshipment1ToPortCountryCode");
            this.Property(t => t.Transshipment1ToPortCountryName).HasColumnName("Transshipment1ToPortCountryName");
            this.Property(t => t.OnCarriageToPortCountryCode).HasColumnName("OnCarriageToPortCountryCode");
            this.Property(t => t.OnCarriageToPortCountryName).HasColumnName("OnCarriageToPortCountryName");
            this.Property(t => t.Transshipment2ToPortCountryCode).HasColumnName("Transshipment2ToPortCountryCode");
            this.Property(t => t.Transshipment2ToPortCountryName).HasColumnName("Transshipment2ToPortCountryName");
            this.Property(t => t.PreCarriageToPortCountryCode).HasColumnName("PreCarriageToPortCountryCode");
            this.Property(t => t.PreCarriageToPortCountryName).HasColumnName("PreCarriageToPortCountryName");
            this.Property(t => t.Transshipment3ToPortCountryCode).HasColumnName("Transshipment3ToPortCountryCode");
            this.Property(t => t.Transshipment3ToPortCountryName).HasColumnName("Transshipment3ToPortCountryName");
            this.Property(t => t.MainCarriageCarrierName).HasColumnName("MainCarriageCarrierName");
            this.Property(t => t.MainCarriageCarrierCode).HasColumnName("MainCarriageCarrierCode");
            this.Property(t => t.Transshipment1CarrierName).HasColumnName("Transshipment1CarrierName");
            this.Property(t => t.Transshipment1CarrierCode).HasColumnName("Transshipment1CarrierCode");
            this.Property(t => t.Transshipment2CarrierName).HasColumnName("Transshipment2CarrierName");
            this.Property(t => t.Transshipment2CarrierCode).HasColumnName("Transshipment2CarrierCode");
            this.Property(t => t.Transshipment3CarrierName).HasColumnName("Transshipment3CarrierName");
            this.Property(t => t.Transshipment3CarrierCode).HasColumnName("Transshipment3CarrierCode");
            this.Property(t => t.PreCarriageCarrierName).HasColumnName("PreCarriageCarrierName");
            this.Property(t => t.PreCarriageCarrierCode).HasColumnName("PreCarriageCarrierCode");
            this.Property(t => t.OnCarriageCarrierName).HasColumnName("OnCarriageCarrierName");
            this.Property(t => t.OnCarriageCarrierCode).HasColumnName("OnCarriageCarrierCode");
            this.Property(t => t.NextLegName).HasColumnName("NextLegName");
            this.Property(t => t.DirectionName).HasColumnName("DirectionName");
            this.Property(t => t.TransportModeName).HasColumnName("TransportModeName");
            this.Property(t => t.ShipmentTypeName).HasColumnName("ShipmentTypeName");
            this.Property(t => t.ShipmentReceivableStatusName).HasColumnName("ShipmentReceivableStatusName");
            this.Property(t => t.ShipmentPayableStatusName).HasColumnName("ShipmentPayableStatusName");
            this.Property(t => t.AWBCurrencyCode).HasColumnName("AWBCurrencyCode");
            this.Property(t => t.BranchName).HasColumnName("BranchName");
            this.Property(t => t.ShipmentLevelName).HasColumnName("ShipmentLevelName");
            this.Property(t => t.ShipmentStatusWeight).HasColumnName("ShipmentStatusWeight");
            this.Property(t => t.MainCarriageFinalDestinationPortCode).HasColumnName("MainCarriageFinalDestinationPortCode");
            this.Property(t => t.MainCarriageFinalDestinationPortName).HasColumnName("MainCarriageFinalDestinationPortName");
            this.Property(t => t.MasterShipmentNumber).HasColumnName("MasterShipmentNumber");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.MainCarriageAirlinePrefix).HasColumnName("MainCarriageAirlinePrefix");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.OpenPayablesInLocalCurrency).HasColumnName("OpenPayablesInLocalCurrency");
            this.Property(t => t.AccountedPayablesInLocalCurrency).HasColumnName("AccountedPayablesInLocalCurrency");
            this.Property(t => t.OpenPayablesInProfitCurrency).HasColumnName("OpenPayablesInProfitCurrency");
            this.Property(t => t.AccountedPayablesInProfitCurrency).HasColumnName("AccountedPayablesInProfitCurrency");
            this.Property(t => t.ChargeableWeightInKG).HasColumnName("ChargeableWeightInKG");
            this.Property(t => t.GrossWeightInKG).HasColumnName("GrossWeightInKG");
            this.Property(t => t.GrossWeightUnitCode).HasColumnName("GrossWeightUnitCode");
            this.Property(t => t.ChargeableWeightUnitCode).HasColumnName("ChargeableWeightUnitCode");
            this.Property(t => t.OrderVolumetricWeight).HasColumnName("OrderVolumetricWeight");
            this.Property(t => t.VolumetricWeight).HasColumnName("VolumetricWeight");
            this.Property(t => t.Volume).HasColumnName("Volume");
            this.Property(t => t.IssuingCarrierAgentId).HasColumnName("IssuingCarrierAgentId");
            this.Property(t => t.IncotermCode).HasColumnName("IncotermCode");
            this.Property(t => t.FHLStatusCode).HasColumnName("FHLStatusCode");
            this.Property(t => t.FWBStatusCode).HasColumnName("FWBStatusCode");
            this.Property(t => t.FHLStatusName).HasColumnName("FHLStatusName");
            this.Property(t => t.FWBStatusName).HasColumnName("FWBStatusName");
            this.Property(t => t.AWBPrint).HasColumnName("AWBPrint");
            this.Property(t => t.CarrierLastStatusName).HasColumnName("CarrierLastStatusName");
            this.Property(t => t.FNAReason).HasColumnName("FNAReason");
            this.Property(t => t.Routing).HasColumnName("Routing");
            this.Property(t => t.TruckNumber).HasColumnName("TruckNumber");
            this.Property(t => t.AsAgreedFreight).HasColumnName("AsAgreedFreight");
            this.Property(t => t.AsAgreedOtherCharges).HasColumnName("AsAgreedOtherCharges");
            this.Property(t => t.AccountNumber).HasColumnName("AccountNumber");
            this.Property(t => t.FollowUpId).HasColumnName("FollowUpId");
            this.Property(t => t.FollowUpDate).HasColumnName("FollowUpDate");
            this.Property(t => t.FollowUpNotes).HasColumnName("FollowUpNotes");
            this.Property(t => t.FollowUpTypeId).HasColumnName("FollowUpTypeId");
            this.Property(t => t.FollowUpOwnerId).HasColumnName("FollowUpOwnerId");
            this.Property(t => t.FollowUpOwner).HasColumnName("FollowUpOwner");
            this.Property(t => t.FollowUpType).HasColumnName("FollowUpType");
            this.Property(t => t.CarrierLastStatusCode).HasColumnName("CarrierLastStatusCode");
            this.Property(t => t.CarrierLastStatusDate).HasColumnName("CarrierLastStatusDate");
            this.Property(t => t.FWBStatusDate).HasColumnName("FWBStatusDate");
            this.Property(t => t.FHLStatusDate).HasColumnName("FHLStatusDate");
            this.Property(t => t.FromPortCountryCode).HasColumnName("FromPortCountryCode");
            this.Property(t => t.ToPortCountryCode).HasColumnName("ToPortCountryCode");
            this.Property(t => t.ARInvoiceIssued).HasColumnName("ARInvoiceIssued");
            this.Property(t => t.CreditNoteIssued).HasColumnName("CreditNoteIssued");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.CargonautFHLStatusCode).HasColumnName("CargonautFHLStatusCode");
            this.Property(t => t.CargonautFWBStatusCode).HasColumnName("CargonautFWBStatusCode");
            this.Property(t => t.CargonautFHLStatusName).HasColumnName("CargonautFHLStatusName");
            this.Property(t => t.CargonautFWBStatusName).HasColumnName("CargonautFWBStatusName");
            this.Property(t => t.CargonautFWBStatusDate).HasColumnName("CargonautFWBStatusDate");
            this.Property(t => t.CargonautFHLStatusDate).HasColumnName("CargonautFHLStatusDate");
            this.Property(t => t.NumberOfInsidePackages).HasColumnName("NumberOfInsidePackages");
            this.Property(t => t.NumberOfInsidePackagesDetails).HasColumnName("NumberOfInsidePackagesDetails");
            this.Property(t => t.ConsolidatorId).HasColumnName("ConsolidatorId");
            this.Property(t => t.ConsolidatorAddressId).HasColumnName("ConsolidatorAddressId");
            this.Property(t => t.ConsolidatorContactId).HasColumnName("ConsolidatorContactId");
            this.Property(t => t.ConsolidatorReference).HasColumnName("ConsolidatorReference");
            this.Property(t => t.ConsolidatorName).HasColumnName("ConsolidatorName");
            this.Property(t => t.ConsolidatorNote).HasColumnName("ConsolidatorNote");
            this.Property(t => t.ManifestReason).HasColumnName("ManifestReason");
            this.Property(t => t.ManifestStatusCode).HasColumnName("ManifestStatusCode");
            this.Property(t => t.AirlinePrefix).HasColumnName("AirlinePrefix");
            this.Property(t => t.CustomsDeclarationNumber).HasColumnName("CustomsDeclarationNumber");
            this.Property(t => t.StatusId).HasColumnName("StatusId");
            this.Property(t => t.ShipmentStatusId).HasColumnName("ShipmentStatusId");
            this.Property(t => t.ShipmentStatusName).HasColumnName("ShipmentStatusName");
            this.Property(t => t.ShipmentStatusDate).HasColumnName("ShipmentStatusDate");
            this.Property(t => t.ShipmentStatusWeight).HasColumnName("ShipmentStatusWeight");
            this.Property(t => t.ShipmentStatusLocation).HasColumnName("ShipmentStatusLocation");
            this.Property(t => t.ShipmentMasterDataStatusId).HasColumnName("ShipmentMasterDataStatusId");
            this.Property(t => t.ShipmentMasterDataStatusName).HasColumnName("ShipmentMasterDataStatusName");
            this.Property(t => t.ShipmentMasterDataStatusDate).HasColumnName("ShipmentMasterDataStatusDate");
            this.Property(t => t.ShipmentMasterDataStatusWeight).HasColumnName("ShipmentMasterDataStatusWeight");
            this.Property(t => t.ShipmentMasterDataStatusLocation).HasColumnName("ShipmentMasterDataStatusLocation");
            this.Property(t => t.OperationalDate).HasColumnName("OperationalDate");
            this.Property(t => t.CutoffDate).HasColumnName("CutoffDate");
            this.Property(t => t.NumberOfHouses).HasColumnName("NumberOfHouses");
            this.Property(t => t.ValueOfGoods).HasColumnName("ValueOfGoods");
            this.Property(t => t.FreightRelease).HasColumnName("FreightRelease");
            this.Property(t => t.TerminalAvailable).HasColumnName("TerminalAvailable");
            this.Property(t => t.ISFNumber).HasColumnName("ISFNumber");
            this.Property(t => t.ISFDate).HasColumnName("ISFDate");
            this.Property(t => t.ITNumber).HasColumnName("ITNumber");
            this.Property(t => t.ITDate).HasColumnName("ITDate");
            this.Property(t => t.DocumentsClosingDate).HasColumnName("DocumentsClosingDate");
            this.Property(t => t.OBLTypeCode).HasColumnName("OBLTypeCode");
            this.Property(t => t.ENSNumber).HasColumnName("ENSNumber");
            this.Property(t => t.ENSDate).HasColumnName("ENSDate");
            this.Property(t => t.RegistryDate).HasColumnName("RegistryDate");
            this.Property(t => t.IsAssembly).HasColumnName("IsAssembly");
            this.Property(t => t.LastSharedEventId).HasColumnName("LastSharedEventId");
            this.Property(t => t.LastSharedEventName).HasColumnName("LastSharedEventName");
            this.Property(t => t.LastSharedEventLocation).HasColumnName("LastSharedEventLocation");
            this.Property(t => t.LastSharedEventNotes).HasColumnName("LastSharedEventNotes");
            this.Property(t => t.LastSharedEventDate).HasColumnName("LastSharedEventDate");
            this.Property(t => t.GrossWeightPerTon).HasColumnName("GrossWeightPerTon");
            this.Property(t => t.FirstOperationalCloseDate).HasColumnName("FirstOperationalCloseDate");
            this.Property(t => t.FirstAccountingCloseDate).HasColumnName("FirstAccountingCloseDate");
            this.Property(t => t.INTTRASIStatusCode).HasColumnName("INTTRASIStatusCode");
            this.Property(t => t.INTTRASIStatusName).HasColumnName("INTTRASIStatusName");
            this.Property(t => t.INTTRASIStatusDate).HasColumnName("INTTRASIStatusDate");
            this.Property(t => t.INTTRASIError).HasColumnName("INTTRASIError");
            this.Property(t => t.MainCarriageFromCity).HasColumnName("MainCarriageFromCity");
            this.Property(t => t.MainCarriageToCity).HasColumnName("MainCarriageToCity");
            this.Property(t => t.LastFinalDestination).HasColumnName("LastFinalDestination");
            this.Property(t => t.FirstPickupETA).HasColumnName("FirstPickupETA");
            this.Property(t => t.FirstPickupETD).HasColumnName("FirstPickupETD");
            this.Property(t => t.EstimatedFinalArrivalDate).HasColumnName("EstimatedFinalArrivalDate");
            this.Property(t => t.ActualFinalArrivalDate).HasColumnName("ActualFinalArrivalDate");
            this.Property(t => t.INTTRALastStatusDate).HasColumnName("INTTRALastStatusDate");
            this.Property(t => t.Notify1Reference).HasColumnName("Notify1Reference");
            this.Property(t => t.Notify2Reference).HasColumnName("Notify2Reference");
            this.Property(t => t.ShipperNotExporterReference).HasColumnName("ShipperNotExporterReference");
            this.Property(t => t.ConsigneeNotImporterReference).HasColumnName("ConsigneeNotImporterReference");
            this.Property(t => t.ContainerLastStatusDate).HasColumnName("ContainerLastStatusDate");
            this.Property(t => t.From).HasColumnName("From");
            this.Property(t => t.To).HasColumnName("To");
            this.Property(t => t.Origin).HasColumnName("Origin");
            this.Property(t => t.DeclarationDate).HasColumnName("DeclarationDate");
            this.Property(t => t.DeclarationNumber).HasColumnName("DeclarationNumber");
            this.Property(t => t.ARInvoices).HasColumnName("ARInvoices");
            this.Property(t => t.NotInvoicedReceivablesAmount).HasColumnName("NotInvoicedReceivablesAmount");

        }
    }
}
