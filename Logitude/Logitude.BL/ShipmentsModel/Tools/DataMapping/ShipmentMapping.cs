using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.QuoteModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Reflection;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        public static void MapEntity(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, bool isNewEntity, List<ShipmentPackagePM> myPackagesList, IShipmentsContext objectContext)
        {
            IWebFreightContext webFrieghtcontext = WebFreightContext.GetContext(entityPM.Tenant);
            TraceEventRepository traceEventRepository = new TraceEventRepository(webFrieghtcontext);
            TraceEventQuery traceEventQuery = new TraceEventQuery(traceEventRepository);

            FixStringNullFields(entityPM);

            if (isNewEntity)
            {
                entityPoco.CreateDateTime = entityPM.CreateDateTime;
                entityPoco.CreatedByUserId = entityPM.CreatedByUserId;
                entityPoco.ShipmentTypeId = entityPM.ShipmentTypeId;
                entityPoco.Tenant = entityPM.Tenant;
                entityPoco.TransportModeId = entityPM.TransportModeId;
                entityPoco.DirectionId = entityPM.DirectionId;
                entityPoco.ShipmentNumber = entityPM.ShipmentNumber;

                if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "H")
                {
                    entityPM.ComputedShipmentNumber = entityPM.ShipmentNumber;
                }


                if (entityPoco.DirectionId == "C")
                {
                    entityPM.ProductCode = "CI";
                    entityPoco.ProductCode = "CI";
                }

                else
                {
                    entityPM.ProductCode = entityPoco.TransportModeId + entityPoco.DirectionId;
                    entityPoco.ProductCode = entityPoco.TransportModeId + entityPoco.DirectionId;
                }

                entityPoco.MasterShipmentDataId = entityPM.MasterShipmentDataId;

                entityPoco.SecurityKey = entityPM.SecurityKey;
                entityPoco.OriginShipmentId = entityPM.OriginShipmentId;
            }

            else
            {
                if (entityPM.ConvertToCustomFile)
                {
                    entityPoco.DirectionId = entityPM.DirectionId;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ForwarderShipmentNumber))
            {
                entityPoco.ComputedForwarderShipmentNumber = entityPM.ForwarderShipmentNumber;
            }

            else
            {
                entityPoco.ComputedForwarderShipmentNumber = entityPM.Id;
            }

            if (entityPM.IsHybrid)
            {
                entityPoco.ShipmentTypeId = entityPM.ShipmentTypeId;
            }

            if (entityPM.ConvertShipmentToLCL || entityPM.ConvertShipmentToFCL)
            {
                entityPoco.ShipmentTypeId = entityPM.ShipmentTypeId;

                entityPM.ConvertShipmentToLCL = false;
                entityPM.ConvertShipmentToFCL = false;
            }

            entityPoco.NoFreightFile = entityPM.NoFreightFile;

            CheckNextLeg(ref entityPM);
            CheckNextETAAndETD(ref entityPM);
            entityPoco.NextETA = entityPM.NextETA;
            entityPoco.NextETD = entityPM.NextETD;
            entityPoco.NextLegCode = entityPM.NextLegCode;
            entityPoco.FreelancerId = entityPM.FreelancerId;
            entityPoco.FreelancerAddressId = entityPM.FreelancerAddressId;
            entityPoco.FreelancerContactId = entityPM.FreelancerContactId;
            entityPoco.CustomFileId = entityPM.CustomFileId;
            entityPoco.CustomFileNumber = entityPM.CustomFileNumber;
            entityPoco.LastStatusLogDate = entityPM.LastStatusLogDate;
            entityPoco.ExceptionResolvedDescription = entityPM.ExceptionResolvedDescription;
            entityPoco.LastExceptionDescription = entityPM.LastExceptionDescription;
            entityPoco.IsManifestSentToAgent = entityPM.IsManifestSentToAgent;
            entityPoco.AgentSharedManifestRef = entityPM.AgentSharedManifestRef;
            entityPoco.ManifestLastSharingDate = entityPM.ManifestLastSharingDate;
            entityPoco.CountryForStatisticsId = entityPM.CountryForStatisticsId;

            if (entityPM.IsExceptionResolved)
            {
                entityPoco.ExceptionDescription = null;
                entityPoco.ExceptionDate = null;
                entityPoco.HasException = false;
            }

            if (entityPM.HasException)
            {
                entityPoco.ExceptionResolvedDescription = null;
            }

            entityPoco.CustomsDeclarationNumber = entityPM.CustomsDeclarationNumber;
            entityPoco.CustomConnectToShipment = entityPM.CustomConnectToShipment;

            MapMasterData(entityPM, entityPoco, entityMasterData, isNewEntity);
            MapRoutings(entityPM, entityPoco, isNewEntity);
            MapPartners(entityPM, entityPoco, isNewEntity);
            MapAWBFields(entityPM, entityPoco, isNewEntity);
            MapTotalsFields(entityPM, entityPoco, isNewEntity);
            MapWeightsFields(entityPM, entityPoco, isNewEntity);
            MapXSDMessagesFields(entityPM, entityPoco, entityMasterData, isNewEntity);

            if (entityPM.ShipmentLevelCode == "H")
            {
                entityPoco.MasterShipmentDataId = entityPM.MasterShipmentDataId;
                entityPoco.FromPortId = entityPM.FromPortId;
                entityPoco.ToPortId = entityPM.ToPortId;
            }

            else
            {
                entityPM.FromPortId = entityMasterData.MainCarriageFromPortId;
                entityPM.ToPortId = entityMasterData.MainCarriageFinalDestinationPortId;

                entityPoco.FromPortId = entityPM.FromPortId;
                entityPoco.ToPortId = entityPM.ToPortId;
            }

            TenantRepository tenantRepository = new TenantRepository(entityPM.Tenant);
            Tenant currentTenant = tenantRepository.GetSingleTenant(entityPM.Tenant);

            if (isNewEntity && entityPM.IsHybrid)
            {
                entityPoco.StatusId = entityPM.StatusId;
                entityPoco.StatusDate = entityPM.StatusDate;
                entityPoco.StatusLocation = entityPM.StatusLocation;
                entityPoco.LastStatusLogDate = entityPM.LastStatusLogDate;

                if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C")
                {
                    entityMasterData.StatusId = entityPM.StatusId;
                    entityMasterData.StatusDate = entityPM.StatusDate;
                    entityMasterData.StatusLocation = entityPM.StatusLocation;
                }
            }

            if (currentTenant.IsDocumentsArchive)
            {
                if (!isNewEntity)
                {
                    entityPoco.TransportModeId = entityPM.TransportModeId;
                }
                entityPoco.ShipmentTypeId = entityPM.ShipmentTypeId;
                entityPoco.StatusId = entityPM.StatusId;
                entityPoco.StatusDate = entityPM.StatusDate;
                entityPoco.HasException = entityPM.HasException;
                entityPoco.ExceptionDate = entityPM.ExceptionDate;
                entityPoco.ExceptionDescription = entityPM.ExceptionDescription;
            }
            if (!entityPM.IsHybrid || entityPM.DontAddToImportersQueue)
            {
                entityPoco.ForwarderShipmentNumber = entityPM.ForwarderShipmentNumber;
                entityPoco.CustomerShipmentNumber = entityPM.CustomerShipmentNumber;
                entityPoco.CustomerTenantNumber = entityPM.CustomerTenantNumber;
            }


            entityPoco.OrderGrossWeightEdited = entityPM.OrderGrossWeightEdited;
            entityPoco.OrderChargeableWeightEdited = entityPM.OrderChargeableWeightEdited;
            entityPoco.CASSCode = entityPM.CASSCode;
            entityPoco.OrderGrossWeight = entityPM.OrderGrossWeight;
            entityPoco.BookingVolume = entityPM.BookingVolume;
            entityPoco.OrderVolumetricWeight = entityPM.OrderVolumetricWeight;
            entityPoco.OrderChargeableWeight = entityPM.OrderChargeableWeight;
            entityPoco.BookingNumberOfPackages = entityPM.BookingNumberOfPackages;
            entityPoco.OrderIsDangerouseGoods = entityPM.OrderIsDangerouseGoods;
            entityPoco.CutoffDate = entityPM.CutoffDate;
            entityPoco.ShipmentLevelCode = entityPM.ShipmentLevelCode;
            entityPoco.IncotermId = entityPM.IncotermId;
            entityPoco.FreightPrepaidCollectId = entityPM.FreightPrepaidCollectId;
            entityPoco.OtherPrepaidCollectId = entityPM.OtherPrepaidCollectId;
            entityPoco.DepartmentId = entityPM.DepartmentId;
            entityPoco.BranchId = entityPM.BranchId;
            entityPoco.SalesmanUserId = entityPM.SalesmanUserId;
            entityPoco.AccountManagerUserId = entityPM.AccountManagerUserId;
            entityPoco.IsCancelled = entityPM.IsCancelled;
            entityPoco.IsOperationalClosed = entityPM.IsOperationalClosed;
            entityPoco.IsAccountingClosed = entityPM.IsAccountingClosed;
            entityPoco.BasketId = entityPM.BasketId;
            entityPoco.CurrentUserId = entityPM.CurrentUserId;
            entityPoco.DescriptionOfGoods = entityPM.DescriptionOfGoods;
            entityPoco.Field1 = entityPM.Field1 != null ? entityPM.Field1.Value : null;
            entityPoco.Field2 = entityPM.Field2 != null ? entityPM.Field2.Value : null;
            entityPoco.Field3 = entityPM.Field3 != null ? entityPM.Field3.Value : null;
            entityPoco.Field4 = entityPM.Field4 != null ? entityPM.Field4.Value : null;
            entityPoco.Field5 = entityPM.Field5 != null ? entityPM.Field5.Value : null;
            entityPoco.Field6 = entityPM.Field6 != null ? entityPM.Field6.Value : null;
            entityPoco.Field7 = entityPM.Field7 != null ? entityPM.Field7.Value : null;
            entityPoco.Field8 = entityPM.Field8 != null ? entityPM.Field8.Value : null;
            entityPoco.Field9 = entityPM.Field9 != null ? entityPM.Field9.Value : null;
            entityPoco.Field10 = entityPM.Field10 != null ? entityPM.Field10.Value : null;
            entityPoco.Field11 = entityPM.Field11 != null ? entityPM.Field11.Value : null;
            entityPoco.Field12 = entityPM.Field12 != null ? entityPM.Field12.Value : null;
            entityPoco.Field13 = entityPM.Field13 != null ? entityPM.Field13.Value : null;
            entityPoco.Field14 = entityPM.Field14 != null ? entityPM.Field14.Value : null;
            entityPoco.Field15 = entityPM.Field15 != null ? entityPM.Field15.Value : null;
            entityPoco.Field16 = entityPM.Field16 != null ? entityPM.Field16.Value : null;
            entityPoco.Field17 = entityPM.Field17 != null ? entityPM.Field17.Value : null;
            entityPoco.Field18 = entityPM.Field18 != null ? entityPM.Field18.Value : null;
            entityPoco.Field19 = entityPM.Field19 != null ? entityPM.Field19.Value : null;
            entityPoco.Field20 = entityPM.Field20 != null ? entityPM.Field20.Value : null;
            entityPoco.Field21 = entityPM.Field21 != null ? entityPM.Field21.Value : null;
            entityPoco.Field22 = entityPM.Field22 != null ? entityPM.Field22.Value : null;
            entityPoco.Field23 = entityPM.Field23 != null ? entityPM.Field23.Value : null;
            entityPoco.Field24 = entityPM.Field24 != null ? entityPM.Field24.Value : null;
            entityPoco.Field25 = entityPM.Field25 != null ? entityPM.Field25.Value : null;
            entityPoco.Field26 = entityPM.Field26 != null ? entityPM.Field26.Value : null;
            entityPoco.Field27 = entityPM.Field27 != null ? entityPM.Field27.Value : null;
            entityPoco.Field28 = entityPM.Field28 != null ? entityPM.Field28.Value : null;
            entityPoco.Field29 = entityPM.Field29 != null ? entityPM.Field29.Value : null;
            entityPoco.Field30 = entityPM.Field30 != null ? entityPM.Field30.Value : null;
            entityPoco.Field31 = entityPM.Field31 != null ? entityPM.Field31.Value : null;
            entityPoco.Field32 = entityPM.Field32 != null ? entityPM.Field32.Value : null;
            entityPoco.Field33 = entityPM.Field33 != null ? entityPM.Field33.Value : null;
            entityPoco.Field34 = entityPM.Field34 != null ? entityPM.Field34.Value : null;
            entityPoco.Field35 = entityPM.Field35 != null ? entityPM.Field35.Value : null;
            entityPoco.Field36 = entityPM.Field36 != null ? entityPM.Field36.Value : null;
            entityPoco.Field37 = entityPM.Field37 != null ? entityPM.Field37.Value : null;
            entityPoco.Field38 = entityPM.Field38 != null ? entityPM.Field38.Value : null;
            entityPoco.Field39 = entityPM.Field39 != null ? entityPM.Field39.Value : null;
            entityPoco.Field40 = entityPM.Field40 != null ? entityPM.Field40.Value : null;
            entityPoco.SpecialServicesTypeId = entityPM.SpecialServicesTypeId;
            entityPoco.Notes = entityPM.Notes;
            entityPoco.House = entityPM.House;
            entityPoco.HAWBDate = entityPM.HAWBDate;
            entityPoco.MainHarmonize = entityPM.MainHarmonize;
            entityPoco.IsDangerous = entityPM.IsDangerous;
            entityPoco.DangerousPackagingGroup = entityPM.DangerousPackagingGroup;
            entityPoco.DangerousUnNumber = entityPM.DangerousUnNumber;
            entityPoco.DangerousMaterialDescription = entityPM.DangerousMaterialDescription;
            entityPoco.DangerousIMDGCode = entityPM.DangerousIMDGCode;
            entityPoco.DangerousFlashPoint = entityPM.DangerousFlashPoint;
            entityPoco.DangerousClassNumber = entityPM.DangerousClassNumber;
            entityPoco.LTCWEdited = entityPM.LTCWEdited;
            entityPoco.ShipmentPickUpIndex = entityPM.ShipmentPickUpIndex;
            entityPoco.ShipmentDeliveryIndex = entityPM.ShipmentDeliveryIndex;
            entityPoco.ShipmentContainerReturnIndex = entityPM.ShipmentContainerReturnIndex;
            entityPoco.QuoteId = entityPM.QuoteId;
            entityPoco.BookingId = entityPM.BookingId;
            entityPoco.CancelledDate = entityPM.CancelledDate;
            entityPoco.LastUpdateDate = entityPM.LastUpdateDate;
            entityPoco.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPoco.AsAgreedFreight = entityPM.AsAgreedFreight;
            entityPoco.AsAgreedOtherCharges = entityPM.AsAgreedOtherCharges;
            entityPoco.AccountNumber = entityPM.AccountNumber;
            entityPoco.DeliveryOrder = entityPM.DeliveryOrder;
            entityPoco.FreightLocationId = entityPM.FreightLocationId;
            entityPoco.TransportDocumentNumber = entityPM.TransportDocumentNumber;
            entityPoco.IsMultipleCommodities = entityPM.IsMultipleCommodities;
            entityPoco.NominatedHandlingPartyId = entityPM.NominatedHandlingPartyId;
            entityPoco.OtherParticipantIdCode1 = entityPM.OtherParticipantIdCode1;
            entityPoco.OtherParticipantIdCode2 = entityPM.OtherParticipantIdCode2;
            entityPoco.OtherParticipantIdCode3 = entityPM.OtherParticipantIdCode3;
            entityPoco.OtherParticipantInformationCode1 = entityPM.OtherParticipantInformationCode1;
            entityPoco.OtherParticipantInformationCode2 = entityPM.OtherParticipantInformationCode2;
            entityPoco.OtherParticipantInformationCode3 = entityPM.OtherParticipantInformationCode3;
            entityPoco.OtherParticipantInformationPortCode1 = entityPM.OtherParticipantInformationPortCode1;
            entityPoco.OtherParticipantInformationPortCode2 = entityPM.OtherParticipantInformationPortCode2;
            entityPoco.OtherParticipantInformationPortCode3 = entityPM.OtherParticipantInformationPortCode3;
            entityPoco.OtherParticipantInformationName1 = entityPM.OtherParticipantInformationName1;
            entityPoco.OtherParticipantInformationName2 = entityPM.OtherParticipantInformationName2;
            entityPoco.OtherParticipantInformationName3 = entityPM.OtherParticipantInformationName3;
            entityPoco.OtherParticipantInformationReference1 = entityPM.OtherParticipantInformationReference1;
            entityPoco.OtherParticipantInformationReference2 = entityPM.OtherParticipantInformationReference2;
            entityPoco.OtherParticipantInformationReference3 = entityPM.OtherParticipantInformationReference3;
            entityPoco.AccountingInformation1 = entityPM.AccountingInformation1;
            entityPoco.AccountingInformation2 = entityPM.AccountingInformation2;
            entityPoco.AccountingInformation3 = entityPM.AccountingInformation3;
            entityPoco.AccountingInformation4 = entityPM.AccountingInformation4;
            entityPoco.AccountingInformation5 = entityPM.AccountingInformation5;
            entityPoco.AccountingInformation6 = entityPM.AccountingInformation6;
            entityPoco.AccountingInformationIdentifierCode1 = entityPM.AccountingInformationIdentifierCode1;
            entityPoco.AccountingInformationIdentifierCode2 = entityPM.AccountingInformationIdentifierCode2;
            entityPoco.AccountingInformationIdentifierCode3 = entityPM.AccountingInformationIdentifierCode3;
            entityPoco.AccountingInformationIdentifierCode4 = entityPM.AccountingInformationIdentifierCode4;
            entityPoco.AccountingInformationIdentifierCode5 = entityPM.AccountingInformationIdentifierCode5;
            entityPoco.AccountingInformationIdentifierCode6 = entityPM.AccountingInformationIdentifierCode6;
            entityPoco.ReferenceNumber = entityPM.ReferenceNumber;
            entityPoco.SupplementaryShipmentInformation1 = entityPM.SupplementaryShipmentInformation1;
            entityPoco.SupplementaryShipmentInformation2 = entityPM.SupplementaryShipmentInformation2;
            entityPoco.NumberOfInsidePackages = entityPM.NumberOfInsidePackages;
            entityPoco.NumberOfInsidePackagesDetails = entityPM.NumberOfInsidePackagesDetails;
            entityPoco.ViaColoader = entityPM.ViaColoader;
            entityPoco.IssuingCarrierReference1 = entityPM.IssuingCarrierReference1;
            if (entityPoco.OperationalCloseDate == null && entityPM.OperationalCloseDate != null)
            {
                entityPoco.OperationalClosedByUserId = entityPM.OperationalClosedByUserId;
            }
            else if (entityPoco.OperationalCloseDate != null && entityPM.OperationalCloseDate == null)
            {
                entityPoco.OperationalClosedByUserId = entityPM.OperationalClosedByUserId;
            }

            entityPoco.OperationalCloseDate = entityPM.OperationalCloseDate;
            entityPoco.AccountingCloseDate = entityPM.AccountingCloseDate;
            entityPoco.ForwarderPartnerId = entityPM.ForwarderPartnerId;
            entityPoco.ForwardingPartnerId = entityPM.ForwardingPartnerId;
            entityPoco.NumberOfFollowUps = entityPM.NumberOfFollowUps;
            entityPoco.ValueOfGoods = entityPM.ValueOfGoods == null ? null : MethodHelper.Round(entityPM.ValueOfGoods, 2);
            entityPoco.ValueOfGoodsCurrencyId = entityPM.ValueOfGoodsCurrencyId;
            entityPoco.IsNewARInvoiceBlocked = entityPM.IsNewARInvoiceBlocked;
            entityPoco.FBLIsFromStock = entityPM.FBLIsFromStock;
            entityPoco.AMSBL = entityPM.AMSBL;
            entityPoco.MoveTypeId = entityPM.MoveTypeId;
            entityPoco.TEU = entityPM.TEU;
            entityPoco.FreightRelease = entityPM.FreightRelease;
            entityPoco.TerminalAvailable = entityPM.TerminalAvailable;
            entityPoco.ISFNumber = entityPM.ISFNumber;
            entityPoco.ISFDate = entityPM.ISFDate;
            entityPoco.ITNumber = entityPM.ITNumber;
            entityPoco.ITDate = entityPM.ITDate;
            entityPoco.ENSNumber = entityPM.ENSNumber;
            entityPoco.ENSDate = entityPM.ENSDate;
            entityPoco.WarehouseLegWarehouseId = entityPM.WarehouseLegWarehouseId;
            entityPoco.WarehouseLegAddressId = entityPM.WarehouseLegAddressId;
            entityPoco.WarehouseLegTerminalCode = entityPM.WarehouseLegTerminalCode;
            entityPoco.WarehouseLegExpectedEntryDate = entityPM.WarehouseLegExpectedEntryDate;
            entityPoco.WarehouseLegActualEntryDate = entityPM.WarehouseLegActualEntryDate;
            entityPoco.WarehouseLegExpectedReleaseDate = entityPM.WarehouseLegExpectedReleaseDate;
            entityPoco.WarehouseLegActualReleaseDate = entityPM.WarehouseLegActualReleaseDate;
            entityPoco.WarehouseLegLastFreeDate = entityPM.WarehouseLegLastFreeDate;
            entityPoco.WarehouseLegRemarks = entityPM.WarehouseLegRemarks;
            entityPoco.WarehouseLegReference = entityPM.WarehouseLegReference;
            entityPoco.WarehouseLegCutOffDate = entityPM.WarehouseLegCutOffDate;
            entityPoco.WarehouseLegVGMCutOffDate = entityPM.WarehouseLegVGMCutOffDate;
            entityPoco.IsAssembly = entityPM.IsAssembly;
            entityPoco.LastSharedEventId = entityPM.LastSharedEventId;
            entityPoco.LastSharedEventLocation = entityPM.LastSharedEventLocation;
            entityPoco.LastSharedEventNotes = entityPM.LastSharedEventNotes;
            entityPoco.LastSharedEventDate = entityPM.LastSharedEventDate;
            entityPoco.FirstOperationalCloseDate = entityPM.FirstOperationalCloseDate;
            entityPoco.FirstAccountingCloseDate = entityPM.FirstAccountingCloseDate;
            entityPoco.AMSClosingDate = entityPM.AMSClosingDate;
            entityPoco.UpdatedByPartner = entityPM.UpdatedByPartner;
            entityPoco.EmergencyContactId = entityPM.EmergencyContactId;
            entityPoco.OnCarriageAdditionalTransportModeCode = entityPM.OnCarriageAdditionalTransportModeCode;
            entityPoco.LastFinalDestination = entityPM.LastFinalDestination;
            entityPoco.FirstPickupETA = entityPM.FirstPickupETA;
            entityPoco.FirstPickupETD = entityPM.FirstPickupETD;
            entityPoco.SplitOnCarriage = entityPM.SplitOnCarriage;
            entityPoco.Notify1Reference = entityPM.Notify1Reference;
            entityPoco.Notify2Reference = entityPM.Notify2Reference;
            entityPoco.ShipperNotExporterReference = entityPM.ShipperNotExporterReference;
            entityPoco.ConsigneeNotImporterReference = entityPM.ConsigneeNotImporterReference;
            entityPoco.ProjectNumber = entityPM.ProjectNumber;
            entityPoco.ContainerLastStatusDate = entityPM.ContainerLastStatusDate;
            entityPoco.BasicFreightId = entityPM.BasicFreightId;
            entityPoco.DestinationPortChargesId = entityPM.DestinationPortChargesId;
            entityPoco.DestinationHaulageChargesId = entityPM.DestinationHaulageChargesId;
            entityPoco.AdditionalChargesId = entityPM.AdditionalChargesId;
            entityPoco.FreightPayerId = entityPM.FreightPayerId;
            entityPoco.FreightPayerAddressId = entityPM.FreightPayerAddressId;
            entityPoco.HasContainerException = entityPM.HasContainerException;
            entityPoco.From = entityPM.From;
            entityPoco.To = entityPM.To;
            entityPoco.Origin = entityPM.Origin;
            entityPoco.ComputedShipmentNumber = entityPM.ComputedShipmentNumber;

            // No need to map these fields
            // they are computed via PROCEDURE
            //entityPoco.OperationalDate = entityPM.OperationalDate;
            //entityPoco.FinalArrivalDate = entityPM.FinalArrivalDate;
            //entityPoco.EstimatedFinalArrivalDate = entityPM.EstimatedFinalArrivalDate;
            //entityPoco.ActualFinalArrivalDate = entityPM.ActualFinalArrivalDate;

            BuildSearchField(entityPM, entityPoco, entityMasterData, myPackagesList);
            if (!currentTenant.IsDocumentsArchive)
            {
                BuildRoutingField(entityPM, entityPoco, entityMasterData, objectContext);
            }

            entityPoco.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            entityPM.ConcurrencyGUID = entityPoco.ConcurrencyGUID;

            if (entityPM.ShipmentDirectionConverted)
            {
                entityPoco.DirectionId = entityPM.DirectionId;
                entityPM.ShipmentDirectionConverted = false;
            }
        }

        private static void MapXSDMessagesFields(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, bool isNewEntity)
        {
            // The Send Services uses the direct poco and update it
            // only the FSR Service is using the Shipment service to update shipment pm
            // CCSWebService, ShipmentToCustomsWebService, SimulatorResponsesWebService

            // the flag: IsUpdatedByAnalyzer is used only for mapping these fields
            // we need another flag for stopping the Concurrency test validation

            // but our problem is entityPM from client side using the update service
            // it wont see these flags (specialy the second flag)

            //Customs              
            entityPoco.LocalCustomsTransmissionsStatusCode = entityPM.LocalCustomsTransmissionsStatusCode;
            entityPoco.LocalCustomsTransmissionsStatusError = entityPM.LocalCustomsTransmissionsStatusError;
            entityPoco.LocalCustomsTransmissionsStatusDate = entityPM.LocalCustomsTransmissionsStatusDate;
            entityPoco.IncludesCustoms = entityPM.IncludesCustoms;
            entityPoco.DeclarationNumber = entityPM.DeclarationNumber;
            entityPoco.DeclarationDate = entityPM.DeclarationDate;
            entityPoco.CustomsClearanceDate = entityPM.CustomsClearanceDate;

            // FNA:FMA:FSA
            entityPoco.CarrierLastStatusCode = entityPM.CarrierLastStatusCode;
            entityPoco.CarrierLastStatusDate = entityPM.CarrierLastStatusDate;

            // FSR:FSA
            entityPoco.IsFSRSent = entityPM.IsFSRSent;
            entityPoco.LastFSRStatusRequestDate = entityPM.LastFSRStatusRequestDate;

            entityPoco.FNAReason = entityPM.FNAReason;
            entityPoco.FHLStatusCode = entityPM.FHLStatusCode;
            entityPoco.FHLStatusDate = entityPM.FHLStatusDate;
            entityPoco.CargonautFHLStatusCode = entityPM.CargonautFHLStatusCode;
            entityPoco.CargonautFHLStatusDate = entityPM.CargonautFHLStatusDate;


            if (entityPM.ShipmentLevelCode != "H")
            {
                if (entityMasterData != null)
                {
                    entityMasterData.FWBStatusCode = entityPM.FWBStatusCode;
                    entityMasterData.FWBStatusDate = entityPM.FWBStatusDate;
                    entityMasterData.CargonautFWBStatusCode = entityPM.CargonautFWBStatusCode;
                    entityMasterData.CargonautFWBStatusDate = entityPM.CargonautFWBStatusDate;
                }
            }
            //}

            entityPoco.INTTRASIStatusCode = entityPM.INTTRASIStatusCode;
            entityPoco.INTTRASIStatusDate = entityPM.INTTRASIStatusDate;
            entityPoco.INTTRASIError = entityPM.INTTRASIError;
            entityPoco.INTTRAContractNumber = entityPM.INTTRAContractNumber;
            entityPoco.INTTRAInstructions = entityPM.INTTRAInstructions;
            entityPoco.INTTRAComments = entityPM.INTTRAComments;
            entityPoco.INTTRADocumentQTY = entityPM.INTTRADocumentQTY;
            entityPoco.SIHasAttachList = entityPM.SIHasAttachList;
            entityPoco.INTTRAIsFreighted = entityPM.INTTRAIsFreighted;
            entityPoco.INTTRADocumentTypeCode = entityPM.INTTRADocumentTypeCode;
            entityPoco.INTTRALastStatusDate = entityPM.INTTRALastStatusDate;
        }
        private static void BuildRoutingField(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, IShipmentsContext objectContext)
        {
            string myRoutingField = null;
            AddressRepository addressRepository = new AddressRepository(entityPoco.Tenant);

            if (entityPoco.DirectionId == "D" && entityPoco.TransportModeId == "I")
            {
                if (entityMasterData.MainCarriageFromAddressId != null)
                {
                    Address fromAddress = addressRepository.GetSingleAddress(entityMasterData.MainCarriageFromAddressId, entityMasterData.Tenant);
                    myRoutingField = fromAddress.City;
                }

                if (entityMasterData.MainCarriageToAddressId != null)
                {
                    Address toAddress = addressRepository.GetSingleAddress(entityMasterData.MainCarriageToAddressId, entityMasterData.Tenant);
                    myRoutingField = myRoutingField + " , " + toAddress.City;
                }
            }

            else
            {
                PortPM fromPort = null;
                PortPM toPort = null;

                if (entityPM.ShipmentLevelCode == "H" && entityPM.MasterShipmentDataId != null)
                {
                    if (entityMasterData == null)
                    {
                        ShipmentMasterDataRepository shipmentMasterDataRepository = new ShipmentMasterDataRepository(objectContext);
                        entityMasterData = shipmentMasterDataRepository.GetSingleMasterData(entityPM.MasterShipmentDataId);
                    }
                }

                if (entityPM.MasterShipmentDataId != null && entityMasterData != null)
                {
                    fromPort = PortQuery.GetSinglePort(entityMasterData.Tenant, entityMasterData.MainCarriageFromPortId, true);
                    if (entityMasterData.Transshipment3ToPortId != null)
                    {
                        toPort = PortQuery.GetSinglePort(entityMasterData.Tenant, entityMasterData.Transshipment3ToPortId, true);
                    }
                    else if (entityMasterData.Transshipment2ToPortId != null)
                    {
                        toPort = PortQuery.GetSinglePort(entityMasterData.Tenant, entityMasterData.Transshipment2ToPortId, true);
                    }
                    else if (entityMasterData.Transshipment1ToPortId != null)
                    {
                        toPort = PortQuery.GetSinglePort(entityMasterData.Tenant, entityMasterData.Transshipment1ToPortId, true);
                    }
                    else
                    {
                        toPort = PortQuery.GetSinglePort(entityMasterData.Tenant, entityMasterData.MainCarriageToPortId, true);
                    }
                }

                else
                {
                    fromPort = PortQuery.GetSinglePort(entityPoco.Tenant, entityPoco.FromPortId, true);
                    toPort = PortQuery.GetSinglePort(entityPoco.Tenant, entityPoco.ToPortId, true);
                }

                myRoutingField = fromPort.Code + " , " + toPort.Code;

                if (entityPoco.PreCarriageFromPortId != null)
                {
                    PortPM precarriageFromPort = PortQuery.GetSinglePort(entityPoco.Tenant, entityPoco.PreCarriageFromPortId, true);
                    myRoutingField = precarriageFromPort.Code + " , " + myRoutingField;
                }

                if (entityPoco.OnCarriageToPortId != null)
                {
                    PortPM oncarriageToPort = PortQuery.GetSinglePort(entityPoco.Tenant, entityPoco.OnCarriageToPortId, true);
                    myRoutingField = myRoutingField + " , " + oncarriageToPort.Code;
                }
            }

            entityPM.Routing = myRoutingField;
            entityPoco.Routing = myRoutingField;
        }
        private static void FixStringNullFields(ShipmentPM entityPM)
        {
            if (entityPM != null)
            {
                if (string.IsNullOrEmpty(entityPM.TruckNumber))
                {
                    entityPM.TruckNumber = null;
                }

                if (string.IsNullOrEmpty(entityPM.MainCarriageCarrierNumber))
                {
                    entityPM.MainCarriageCarrierNumber = null;
                }
            }
        }
        private static void CheckNextLeg(ref ShipmentPM shipmentPM)
        {
            List<ShipmentDeliveryPM> shipmentDeliveries = null;
            bool deliveryExists = false;
            if (shipmentPM.ShipmentDeliveries != null)
            {
                if (shipmentPM.ShipmentDeliveries.Count != 0)
                {
                    shipmentDeliveries = (from a in shipmentPM.ShipmentDeliveries
                                          select a).ToList();
                }
                if (shipmentDeliveries != null)
                {
                    if (shipmentDeliveries.Count != 0)
                    {
                        deliveryExists = true;
                    }
                }
            }

            #region precarriage
            if (shipmentPM.PreCarriageFromPortId != null)//precarriage exists.
            {
                if (shipmentPM.PreCarriageATD != null || shipmentPM.PreCarriageATA != null)//the precarriage is departed or arrived.
                {
                    shipmentPM.NextLegCode = "MAL1";
                }
                else
                {
                    shipmentPM.NextLegCode = "PRCR";
                }
            }
            else //there is no precarriage.
            {
                shipmentPM.NextLegCode = "MAL1";
            }
            #endregion

            #region Main Carriage legs

            if (shipmentPM.MainCarriageATA != null || shipmentPM.MainCarriageATD != null)//main carriage departed or arrived.
            {
                #region leg2
                if (shipmentPM.Transshipment1FromPortId != null)//leg 2 exists
                {

                    if (shipmentPM.Transshipment1ATA != null || shipmentPM.Transshipment1ATD != null) // leg 2 is departed or arrived.
                    {
                        #region leg3
                        if (shipmentPM.Transshipment2FromPortId != null)//leg 3 exists.
                        {
                            if (shipmentPM.Transshipment2ATA != null || shipmentPM.Transshipment2ATD != null) // leg 3 is departed or arrived.
                            {
                                #region leg4
                                if (shipmentPM.Transshipment3FromPortId != null)//leg 4 exists.
                                {
                                    if (shipmentPM.Transshipment3ATA != null || shipmentPM.Transshipment3ATD != null) // leg 4 is departed or arrived.
                                    {
                                        if (shipmentPM.OnCarriageFromPortId != null)//oncarriage exists.
                                        {
                                            if (shipmentPM.OnCarriageATA != null || shipmentPM.OnCarriageATD != null)//oncarriage departed or arrived.
                                            {
                                                if (deliveryExists)
                                                {
                                                    shipmentPM.NextLegCode = "DELV";
                                                }
                                                else
                                                {
                                                    shipmentPM.NextLegCode = null;
                                                }
                                            }
                                            else
                                            {
                                                shipmentPM.NextLegCode = "ONCR";
                                            }
                                        }
                                        else
                                        {

                                            if (deliveryExists)
                                            {
                                                shipmentPM.NextLegCode = "DELV";
                                            }
                                            else
                                            {
                                                shipmentPM.NextLegCode = null;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        shipmentPM.NextLegCode = "MAL4";
                                    }
                                }
                                else// leg 4 doesn't exist.
                                {
                                    if (shipmentPM.OnCarriageFromPortId != null)//oncarriage exists.
                                    {
                                        if (shipmentPM.OnCarriageATA != null || shipmentPM.OnCarriageATD != null)//oncarriage departed or arrived.
                                        {
                                            if (deliveryExists)
                                            {
                                                shipmentPM.NextLegCode = "DELV";
                                            }
                                            else
                                            {
                                                shipmentPM.NextLegCode = null;
                                            }
                                        }
                                        else
                                        {
                                            shipmentPM.NextLegCode = "ONCR";
                                        }
                                    }
                                    else
                                    {

                                        if (deliveryExists)
                                        {
                                            shipmentPM.NextLegCode = "DELV";
                                        }
                                        else
                                        {
                                            shipmentPM.NextLegCode = null;
                                        }

                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                shipmentPM.NextLegCode = "MAL3";
                            }
                        }
                        else// leg 3 doesn't exist.
                        {
                            if (shipmentPM.OnCarriageFromPortId != null)//oncarriage exists.
                            {
                                if (shipmentPM.OnCarriageATA != null || shipmentPM.OnCarriageATD != null)//oncarriage departed or arrived.
                                {
                                    if (deliveryExists)
                                    {
                                        shipmentPM.NextLegCode = "DELV";
                                    }
                                    else
                                    {
                                        shipmentPM.NextLegCode = null;
                                    }
                                }
                                else
                                {
                                    shipmentPM.NextLegCode = "ONCR";
                                }
                            }
                            else
                            {

                                if (deliveryExists)
                                {
                                    shipmentPM.NextLegCode = "DELV";
                                }
                                else
                                {
                                    shipmentPM.NextLegCode = null;
                                }

                            }
                        }
                        #endregion
                    }
                    else
                    {
                        shipmentPM.NextLegCode = "MAL2";
                    }


                }
                else
                {
                    if (shipmentPM.OnCarriageFromPortId != null)//oncarriage exists.
                    {
                        if (shipmentPM.OnCarriageATA != null || shipmentPM.OnCarriageATD != null)//oncarriage departed or arrived.
                        {
                            if (deliveryExists)
                            {
                                shipmentPM.NextLegCode = "DELV";
                            }
                            else
                            {
                                shipmentPM.NextLegCode = null;
                            }
                        }
                        else
                        {
                            shipmentPM.NextLegCode = "ONCR";
                        }
                    }
                    else
                    {

                        if (deliveryExists)
                        {
                            shipmentPM.NextLegCode = "DELV";
                        }
                        else
                        {
                            shipmentPM.NextLegCode = null;
                        }

                    }
                }
                #endregion
            }

            #endregion

            #region oncarriage
            if (shipmentPM.OnCarriageFromPortId != null)//on carriage exists.
            {
                if (shipmentPM.OnCarriageATD != null || shipmentPM.OnCarriageATA != null)//oncarriage departed or arrived.
                {

                    if (deliveryExists)
                    {
                        shipmentPM.NextLegCode = "DELV";
                    }
                    else
                    {
                        shipmentPM.NextLegCode = null;
                    }

                }
            }

            #endregion

            #region delivery
            if (deliveryExists)
            {
                bool deliveryActualTimeAllExists = true;
                foreach (ShipmentDeliveryPM delivery in shipmentDeliveries)
                {
                    if (delivery.ATD == null)
                    {
                        deliveryActualTimeAllExists = false;
                    }
                }

                if (deliveryActualTimeAllExists)
                {
                    shipmentPM.NextLegCode = null;
                }

            }
            #endregion
        }
        private static void CheckNextETAAndETD(ref ShipmentPM entityPM)
        {
            List<ShipmentPickUpPM> shipmentPickUps = null;
            List<ShipmentDeliveryPM> shipmentDeliveries = null;
            bool deliveryExists = false;
            bool pickUpExists = false;
            bool deliveryAtaAllExists = true;
            bool deliveryAtdAllExists = true;
            bool continueEta = true;
            bool continueEtd = true;
            bool continueActualCheck = true;
            DateTime? nextDeliveryEta = null;
            DateTime? nextDeliveryEtd = null;
            DateTime? nextPickUpEta = null;
            DateTime? nextPickUpEtd = null;
            DateTime? nextPreCarriageEtd = null;
            DateTime? nextOnCarriageEtd = null;
            DateTime? nextMainCarriageLeg1Etd = null;
            DateTime? nextMainCarriageLeg2Etd = null;
            DateTime? nextMainCarriageLeg3Etd = null;
            DateTime? nextMainCarriageLeg4Etd = null;
            DateTime? nextPreCarriageEta = null;
            DateTime? nextOnCarriageEta = null;
            DateTime? nextMainCarriageLeg1Eta = null;
            DateTime? nextMainCarriageLeg2Eta = null;
            DateTime? nextMainCarriageLeg3Eta = null;
            DateTime? nextMainCarriageLeg4Eta = null;
            bool expectedEtaExists = false;
            bool expectedEtdExists = false;

            if (entityPM.ShipmentPickUps != null)
            {
                if (entityPM.ShipmentPickUps.Count != 0)
                {
                    shipmentPickUps = (from a in entityPM.ShipmentPickUps
                                       select a).ToList();
                }

                if (shipmentPickUps != null)
                {
                    if (shipmentPickUps.Count != 0)
                    {
                        pickUpExists = true;
                    }
                }
            }

            if (entityPM.ShipmentDeliveries != null)
            {
                if (entityPM.ShipmentDeliveries.Count != 0)
                {
                    shipmentDeliveries = (from a in entityPM.ShipmentDeliveries
                                          select a).ToList();
                }

                if (shipmentDeliveries != null)
                {
                    if (shipmentDeliveries.Count != 0)
                    {
                        deliveryExists = true;
                    }
                }
            }

            if (deliveryExists)//if there is deliveries in entityPoco.
            {
                foreach (ShipmentDeliveryPM delivery in shipmentDeliveries)
                {
                    if (delivery.ATA == null)
                    {
                        deliveryAtaAllExists = false;
                        if (nextDeliveryEta == null && delivery.ETA != null)
                        {
                            nextDeliveryEta = delivery.ETA;
                            expectedEtaExists = true;
                        }
                        else
                        {
                            if (delivery.ETA != null)
                            {
                                if (DateTime.Compare(delivery.ETA.Value, nextDeliveryEta.Value) < 0)
                                {
                                    nextDeliveryEta = delivery.ETA;
                                }
                            }
                        }
                    }
                    else
                    {
                        continueEta = false;
                        continueEtd = false;
                        continueActualCheck = false;
                    }

                    if (delivery.ATD == null)
                    {
                        deliveryAtdAllExists = false;
                        if (nextDeliveryEtd == null)
                        {
                            nextDeliveryEtd = delivery.ETD;
                            expectedEtdExists = true;
                        }
                        else
                        {
                            if (delivery.ETD != null)
                            {
                                if (DateTime.Compare(delivery.ETD.Value, nextDeliveryEtd.Value) < 0)
                                {
                                    nextDeliveryEtd = delivery.ETD;
                                }
                            }
                        }
                    }
                    else
                    {
                        continueEta = false;
                        continueEtd = false;
                        continueActualCheck = false;
                    }
                }
                //if (DeliveryATAAllExists)
                //{
                //    ContinueETA = false;
                //}
                //if (DeliveryATDAllExists)
                //{
                //    ContinueETD = false;
                //}
                entityPM.NextETA = nextDeliveryEta;
                entityPM.NextETD = nextDeliveryEtd;
            }

            #region NextETD

            if (entityPM.OnCarriageATD == null && entityPM.OnCarriageETD != null && continueEtd)
            {
                nextOnCarriageEtd = entityPM.OnCarriageETD;
                entityPM.NextETD = nextOnCarriageEtd;
                expectedEtdExists = true;
            }
            else if (entityPM.OnCarriageATD != null && continueEtd)
            {
                continueEtd = false;
                continueEta = false;
                nextOnCarriageEtd = null;
                expectedEtaExists = true;
                //NextOnCarriageETA =entityPM.OnCarriageATA==null? entityPM.OnCarriageETA:NextDeliveryETA;
                entityPM.NextETA = entityPM.OnCarriageATA == null ? entityPM.OnCarriageETA : nextDeliveryEta;
                #region fill nextETA

                if (entityPM.OnCarriageATA == null && entityPM.OnCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.OnCarriageETA;
                }
                else
                {
                    entityPM.NextETA = nextDeliveryEta;
                }



                #endregion
            }
            if (entityPM.Transshipment3ATD == null && entityPM.Transshipment3ETD != null && continueEtd)
            {
                nextMainCarriageLeg4Etd = entityPM.Transshipment3ETD;
                entityPM.NextETD = nextMainCarriageLeg4Etd;
                expectedEtdExists = true;
            }
            else if (entityPM.Transshipment3ATD != null && continueEtd)
            {
                continueEtd = false;
                continueEta = false;
                nextMainCarriageLeg4Etd = null;
                #region fill nextETA

                if (entityPM.Transshipment3ATA == null && entityPM.Transshipment3ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment3ETA;
                }
                else if (entityPM.OnCarriageATA == null && entityPM.OnCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.OnCarriageETA;
                }
                else
                {
                    entityPM.NextETA = nextDeliveryEta;
                }



                #endregion
                // NextMainCarriageLeg4ETA = entityPM.Transshipment3ETA;
                //entityPM.NextETA =entityPM.Transshipment3ATA==null?entityPM.Transshipment3ETA:entityPM.OnCarriageATA == null ? entityPM.OnCarriageETA : NextDeliveryETA;
                expectedEtaExists = true;
            }
            if (entityPM.Transshipment2ATD == null && entityPM.Transshipment2ETD != null && continueEtd)
            {
                nextMainCarriageLeg3Etd = entityPM.Transshipment2ETD;
                entityPM.NextETD = nextMainCarriageLeg3Etd;
                expectedEtdExists = true;
            }
            else if (entityPM.Transshipment2ATD != null && continueEtd)
            {
                continueEtd = false;
                continueEta = false;
                nextMainCarriageLeg3Etd = null;
                #region fill nextETA

                if (entityPM.Transshipment2ATA == null && entityPM.Transshipment2ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment2ETA;
                }
                else if (entityPM.Transshipment3ATA == null && entityPM.Transshipment3ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment3ETA;
                }
                else if (entityPM.OnCarriageATA == null && entityPM.OnCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.OnCarriageETA;
                }
                else
                {
                    entityPM.NextETA = nextDeliveryEta;
                }



                #endregion
                expectedEtaExists = true;
                //NextMainCarriageLeg3ETA = entityPM.Transshipment2ETA;
                //  entityPM.NextETA = entityPM.Transshipment2ATA == null ? entityPM.Transshipment2ETA : entityPM.Transshipment3ATA == null ? entityPM.Transshipment3ETA : entityPM.OnCarriageATA == null ? entityPM.OnCarriageETA : NextDeliveryETA;
            }
            if (entityPM.Transshipment1ATD == null && entityPM.Transshipment1ETD != null && continueEtd)
            {
                nextMainCarriageLeg2Etd = entityPM.Transshipment1ETD;
                entityPM.NextETD = nextMainCarriageLeg2Etd;
                expectedEtdExists = true;
            }
            else if (entityPM.Transshipment1ATD != null && continueEtd)
            {
                continueEtd = false;
                continueEta = false;
                nextMainCarriageLeg2Etd = null;
                expectedEtaExists = true;
                #region fill nextETA

                if (entityPM.Transshipment1ATA == null && entityPM.Transshipment1ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment1ETA;
                }
                else if (entityPM.Transshipment2ATA == null && entityPM.Transshipment2ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment2ETA;
                }
                else if (entityPM.Transshipment3ATA == null && entityPM.Transshipment3ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment3ETA;
                }
                else if (entityPM.OnCarriageATA == null && entityPM.OnCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.OnCarriageETA;
                }
                else
                {
                    entityPM.NextETA = nextDeliveryEta;
                }


                expectedEtaExists = true;
                #endregion
                //NextMainCarriageLeg2ETA = entityPM.Transshipment1ETA;
                //entityPM.NextETA = entityPM.Transshipment1ATA == null ? entityPM.Transshipment1ETA : entityPM.Transshipment2ATA == null ? entityPM.Transshipment2ETA : entityPM.Transshipment3ATA == null ? entityPM.Transshipment3ETA : entityPM.OnCarriageATA == null ? entityPM.OnCarriageETA : NextDeliveryETA;
            }
            if (entityPM.MainCarriageATD == null && entityPM.MainCarriageETD != null && continueEtd)
            {
                nextMainCarriageLeg1Etd = entityPM.MainCarriageETD;
                entityPM.NextETD = nextMainCarriageLeg1Etd;
                expectedEtdExists = true;
            }
            else if (entityPM.MainCarriageATD != null && continueEtd)
            {
                continueEtd = false;
                continueEta = false;
                nextMainCarriageLeg1Etd = null;
                #region fill nextETA

                if (entityPM.MainCarriageATA == null && entityPM.MainCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.MainCarriageETA;
                }
                else if (entityPM.Transshipment1ATA == null && entityPM.Transshipment1ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment1ETA;
                }
                else if (entityPM.Transshipment2ATA == null && entityPM.Transshipment2ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment2ETA;
                }
                else if (entityPM.Transshipment3ATA == null && entityPM.Transshipment3ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment3ETA;
                }
                else if (entityPM.OnCarriageATA == null && entityPM.OnCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.OnCarriageETA;
                }
                else
                {
                    entityPM.NextETA = nextDeliveryEta;
                }



                #endregion
                expectedEtaExists = true;
                //NextMainCarriageLeg1ETA = entityPM.MainCarriageETA;
                //entityPM.NextETA =entityPM.MainCarriageATA==null ?entityPM.MainCarriageETA : entityPM.Transshipment1ATA == null ? entityPM.Transshipment1ETA : entityPM.Transshipment2ATA == null ? entityPM.Transshipment2ETA : entityPM.Transshipment3ATA == null ? entityPM.Transshipment3ETA : entityPM.OnCarriageATA == null ? entityPM.OnCarriageETA: NextDeliveryETA;
            }
            if (entityPM.PreCarriageATD == null && entityPM.PreCarriageETD != null && continueEtd)
            {
                nextPreCarriageEtd = entityPM.PreCarriageETD;
                entityPM.NextETD = nextPreCarriageEtd;
                expectedEtdExists = true;
            }
            else if (entityPM.PreCarriageATD != null && continueEtd)
            {
                continueEtd = false;
                continueEta = false;
                nextPreCarriageEtd = null;
                #region fill nextETA
                if (entityPM.PreCarriageATA == null && entityPM.PreCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.PreCarriageETA;
                }
                else if (entityPM.MainCarriageATA == null && entityPM.MainCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.MainCarriageETA;
                }
                else if (entityPM.Transshipment1ATA == null && entityPM.Transshipment1ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment1ETA;
                }
                else if (entityPM.Transshipment2ATA == null && entityPM.Transshipment2ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment2ETA;
                }
                else if (entityPM.Transshipment3ATA == null && entityPM.Transshipment3ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment3ETA;
                }
                else if (entityPM.OnCarriageATA == null && entityPM.OnCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.OnCarriageETA;
                }
                else
                {
                    entityPM.NextETA = nextDeliveryEta;
                }



                #endregion
                expectedEtaExists = true;
                // NextPreCarriageETA = entityPM.PreCarriageETA;
                // entityPM.NextETA =entityPM.PreCarriageATA==null?entityPM.PreCarriageETA:entityPM.MainCarriageATA == null ? entityPM.MainCarriageETA : entityPM.Transshipment1ATA == null ? entityPM.Transshipment1ETA : entityPM.Transshipment2ATA == null ? entityPM.Transshipment2ETA : entityPM.Transshipment3ATA == null ? entityPM.Transshipment3ETA : entityPM.OnCarriageATA == null ? entityPM.OnCarriageETA : NextDeliveryETA;
            }
            if (pickUpExists && continueEtd)
            {
                foreach (ShipmentPickUpPM pickUp in shipmentPickUps)
                {
                    if (pickUp.ATD == null)
                    {

                        if (nextPickUpEtd == null && pickUp.ETD != null)
                        {
                            nextPickUpEtd = pickUp.ETD;
                            expectedEtdExists = true;
                        }
                        else
                        {
                            if (pickUp.ETD != null)
                            {
                                if (DateTime.Compare(pickUp.ETD.Value, nextPickUpEtd.Value) < 0)
                                {
                                    nextPickUpEtd = pickUp.ETD;
                                }
                            }
                        }
                    }


                }
                if (nextPickUpEtd != null)
                {
                    entityPM.NextETD = nextPickUpEtd;
                    expectedEtdExists = true;
                }

            }

            #endregion

            #region NextETA

            if (entityPM.OnCarriageATA == null && entityPM.OnCarriageETA != null && continueEta)
            {
                entityPM.NextETA = entityPM.OnCarriageETA;
                expectedEtaExists = true;
            }
            else if (entityPM.OnCarriageATA != null && continueActualCheck)
            {
                continueEta = false;
                entityPM.NextETD = nextDeliveryEtd;
                entityPM.NextETA = nextDeliveryEta;
                continueActualCheck = false;
                expectedEtaExists = true;
            }
            if (entityPM.Transshipment3ATA == null && entityPM.Transshipment3ETA != null && continueEta)
            {
                entityPM.NextETA = entityPM.Transshipment3ETA;
                expectedEtaExists = true;
            }
            else if (entityPM.Transshipment3ATA != null && continueActualCheck)
            {
                continueEta = false;
                entityPM.NextETD = nextOnCarriageEtd != null ? nextOnCarriageEtd : nextDeliveryEtd;

                #region fill nextETA

                if (entityPM.OnCarriageATA == null && entityPM.OnCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.OnCarriageETA;
                }
                else
                {
                    entityPM.NextETA = nextDeliveryEta;
                }



                #endregion
                expectedEtaExists = true;
                // entityPM.NextETA =  entityPM.OnCarriageATA == null ? entityPM.OnCarriageETA : NextDeliveryETA;
                continueActualCheck = false;
            }
            if (entityPM.Transshipment2ATA == null && entityPM.Transshipment2ETA != null && continueEta)
            {
                entityPM.NextETA = entityPM.Transshipment2ETA;
                expectedEtaExists = true;
            }
            else if (entityPM.Transshipment2ATA != null && continueActualCheck)
            {
                continueEta = false;
                continueActualCheck = false;
                entityPM.NextETD = nextMainCarriageLeg4Etd != null ? nextMainCarriageLeg4Etd : nextOnCarriageEtd != null ? nextOnCarriageEtd : nextDeliveryEtd;
                #region fill nextETA

                if (entityPM.Transshipment3ATA == null && entityPM.Transshipment3ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment3ETA;
                }
                else if (entityPM.OnCarriageATA == null && entityPM.OnCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.OnCarriageETA;
                }
                else
                {
                    entityPM.NextETA = nextDeliveryEta;
                }
                #endregion
                expectedEtaExists = true;
                //entityPM.NextETA = entityPM.Transshipment3ATA == null ? entityPM.Transshipment3ETA : entityPM.OnCarriageATA == null ? entityPM.OnCarriageETA : NextDeliveryETA;
            }
            if (entityPM.Transshipment1ATA == null && entityPM.Transshipment1ETA != null && continueEta)
            {
                entityPM.NextETA = entityPM.Transshipment1ETA;
                expectedEtaExists = true;
            }
            else if (entityPM.Transshipment1ATA != null && continueActualCheck)
            {
                continueEta = false;
                continueActualCheck = false;
                entityPM.NextETD = nextMainCarriageLeg3Etd != null ? nextMainCarriageLeg3Etd : nextMainCarriageLeg4Etd != null ? nextMainCarriageLeg4Etd : nextOnCarriageEtd != null ? nextOnCarriageEtd : nextDeliveryEtd;
                #region fill nextETA

                if (entityPM.Transshipment2ATA == null && entityPM.Transshipment2ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment2ETA;
                }
                else if (entityPM.Transshipment3ATA == null && entityPM.Transshipment3ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment3ETA;
                }
                else if (entityPM.OnCarriageATA == null && entityPM.OnCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.OnCarriageETA;
                }
                else
                {
                    entityPM.NextETA = nextDeliveryEta;
                }



                #endregion
                expectedEtaExists = true;
                //entityPM.NextETA = entityPM.Transshipment2ATA == null ? entityPM.Transshipment2ETA : entityPM.Transshipment3ATA == null ? entityPM.Transshipment3ETA : entityPM.OnCarriageATA == null ? entityPM.OnCarriageETA : NextDeliveryETA;
            }
            if (entityPM.MainCarriageATA == null && entityPM.MainCarriageETA != null && continueEta)
            {
                entityPM.NextETA = entityPM.MainCarriageETA;
                expectedEtaExists = true;
            }
            else if (entityPM.MainCarriageATA != null && continueActualCheck)
            {
                continueEta = false;
                continueActualCheck = false;
                entityPM.NextETD = nextMainCarriageLeg2Etd != null ? nextMainCarriageLeg2Etd : nextMainCarriageLeg3Etd != null ? nextMainCarriageLeg3Etd : nextMainCarriageLeg4Etd != null ? nextMainCarriageLeg4Etd : nextOnCarriageEtd != null ? nextOnCarriageEtd : nextDeliveryEtd;
                #region fill nextETA

                if (entityPM.Transshipment1ATA == null && entityPM.Transshipment1ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment1ETA;
                }
                else if (entityPM.Transshipment2ATA == null && entityPM.Transshipment2ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment2ETA;
                }
                else if (entityPM.Transshipment3ATA == null && entityPM.Transshipment3ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment3ETA;
                }
                else if (entityPM.OnCarriageATA == null && entityPM.OnCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.OnCarriageETA;
                }
                else
                {
                    entityPM.NextETA = nextDeliveryEta;
                }



                #endregion
                expectedEtaExists = true;
                //entityPM.NextETA = entityPM.Transshipment1ATA == null ? entityPM.Transshipment1ETA : entityPM.Transshipment2ATA == null ? entityPM.Transshipment2ETA : entityPM.Transshipment3ATA == null ? entityPM.Transshipment3ETA : entityPM.OnCarriageATA == null ? entityPM.OnCarriageETA : NextDeliveryETA;
            }
            if (entityPM.PreCarriageATA == null && entityPM.PreCarriageETA != null && continueEta)
            {
                entityPM.NextETA = entityPM.PreCarriageETA;
                expectedEtaExists = true;
            }
            else if (entityPM.PreCarriageATA != null && continueActualCheck)
            {
                continueEta = false;
                continueActualCheck = false;
                entityPM.NextETD = nextMainCarriageLeg1Etd != null ? nextMainCarriageLeg1Etd : nextMainCarriageLeg3Etd != null ? nextMainCarriageLeg3Etd : nextMainCarriageLeg4Etd != null ? nextMainCarriageLeg4Etd : nextOnCarriageEtd != null ? nextOnCarriageEtd : nextDeliveryEtd;
                #region fill nextETA

                if (entityPM.MainCarriageATA == null && entityPM.MainCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.MainCarriageETA;
                }
                else if (entityPM.Transshipment1ATA == null && entityPM.Transshipment1ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment1ETA;
                }
                else if (entityPM.Transshipment2ATA == null && entityPM.Transshipment2ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment2ETA;
                }
                else if (entityPM.Transshipment3ATA == null && entityPM.Transshipment3ETA != null)
                {
                    entityPM.NextETA = entityPM.Transshipment3ETA;
                }
                else if (entityPM.OnCarriageATA == null && entityPM.OnCarriageETA != null)
                {
                    entityPM.NextETA = entityPM.OnCarriageETA;
                }
                else
                {
                    entityPM.NextETA = nextDeliveryEta;
                }



                #endregion
                expectedEtaExists = true;
                //entityPM.NextETA = entityPM.MainCarriageATA == null ? entityPM.MainCarriageETA : entityPM.Transshipment1ATA == null ? entityPM.Transshipment1ETA : entityPM.Transshipment2ATA == null ? entityPM.Transshipment2ETA : entityPM.Transshipment3ATA == null ? entityPM.Transshipment3ETA : entityPM.OnCarriageATA == null ? entityPM.OnCarriageETA : NextDeliveryETA;
            }
            if (pickUpExists && continueEta)
            {
                foreach (ShipmentPickUpPM pickUp in shipmentPickUps)
                {
                    if (pickUp.ATA == null)
                    {

                        if (nextPickUpEta == null && pickUp.ETA != null)
                        {
                            nextPickUpEta = pickUp.ETA;
                            expectedEtaExists = true;
                        }
                        else
                        {
                            if (pickUp.ETA != null)
                            {
                                if (DateTime.Compare(pickUp.ETA.Value, nextPickUpEta.Value) < 0)
                                {
                                    nextPickUpEta = pickUp.ETA;
                                }
                            }
                        }
                    }
                }
                if (nextPickUpEta != null)
                {
                    entityPM.NextETA = nextPickUpEta;
                }

            }


            if (!expectedEtaExists)
            {
                entityPM.NextETA = null;
            }
            if (!expectedEtdExists)
            {
                entityPM.NextETD = null;
            }
            #endregion
        }
        private static void MapMasterData(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, bool isNewEntity)
        {
            if (entityPM.ShipmentLevelCode != "H")
            {
                if (entityMasterData != null)
                {
                    entityMasterData.DocumentsClosingDate = entityPM.DocumentsClosingDate;
                    entityMasterData.OBLTypeCode = entityPM.OBLTypeCode;

                    entityMasterData.ImportManifest = entityPM.ImportManifest;
                    entityMasterData.CarrierTransportDocumentNumber = entityPM.CarrierTransportDocumentNumber;
                    entityMasterData.Tenant = entityPM.Tenant;
                    entityMasterData.MainCarriageIsFromStack = entityPM.MainCarriageIsFromStack;
                    entityMasterData.Master = entityPM.Master;
                    entityMasterData.MAWBOBLDate = entityPM.MAWBOBLDate;
                    entityMasterData.BookingConfirmationNumber = entityPM.BookingConfirmationNumber;
                    entityMasterData.BookingConfirmationNotes = entityPM.BookingConfirmationNotes;
                    entityMasterData.BookingConfirmedBy = entityPM.BookingConfirmedBy;
                    entityMasterData.MainCarriageFromPortId = entityPM.MainCarriageFromPortId;
                    entityMasterData.MainCarriageToPortId = entityPM.MainCarriageToPortId;
                    entityMasterData.MainCarriageCarrierId = entityPM.MainCarriageCarrierId;
                    entityMasterData.MainCarriageETD = entityPM.MainCarriageETD;
                    entityMasterData.MainCarriageETA = entityPM.MainCarriageETA;
                    entityMasterData.ManifestReason = entityPM.ManifestReason;
                    entityMasterData.ManifestStatusCode = entityPM.ManifestStatusCode;
                    entityMasterData.AirlinePrefix = entityPM.AirlinePrefix;
                    entityMasterData.MainCarriageCarrierNumber = entityPM.MainCarriageCarrierNumber;
                    entityMasterData.ProrateReceivables = entityPM.ProrateReceivables;

                    if (entityMasterData.MainCarriageSTD == null)
                    {
                        entityMasterData.MainCarriageSTD = entityPM.MainCarriageETD;
                    }

                    if (entityMasterData.MainCarriageSTA == null)
                    {
                        entityMasterData.MainCarriageSTA = entityPM.MainCarriageETA;
                    }

                    entityMasterData.MainCarriageVesselId = entityPM.MainCarriageVesselId;
                    entityMasterData.MainCarriageIsFromStack = entityPM.MainCarriageIsFromStack;
                    entityMasterData.MainCarriageATD = entityPM.MainCarriageATD;
                    entityMasterData.MainCarriageATA = entityPM.MainCarriageATA;
                    entityMasterData.Transshipment1FromPortId = entityPM.Transshipment1FromPortId;
                    entityMasterData.Transshipment1ToPortId = entityPM.Transshipment1ToPortId;
                    entityMasterData.Transshipment1CarrierId = entityPM.Transshipment1CarrierId;
                    entityMasterData.Transshipment1CarrierNumber = entityPM.Transshipment1CarrierNumber;
                    entityMasterData.Transshipment1AdditionalMAWBOBLBL = entityPM.Transshipment1AdditionalMAWBOBLBL;
                    entityMasterData.Transshipment1VesselId = entityPM.Transshipment1VesselId;
                    entityMasterData.Transshipment1ATA = entityPM.Transshipment1ATA;
                    entityMasterData.Transshipment1ATD = entityPM.Transshipment1ATD;
                    entityMasterData.Transshipment1ETA = entityPM.Transshipment1ETA;
                    entityMasterData.Transshipment1ETD = entityPM.Transshipment1ETD;

                    if (entityMasterData.Transshipment1STD == null)
                    {
                        entityMasterData.Transshipment1STD = entityPM.Transshipment1ETD;
                    }
                    if (entityMasterData.Transshipment1STA == null)
                    {
                        entityMasterData.Transshipment1STA = entityPM.Transshipment1ETA;
                    }

                    entityMasterData.Transshipment2FromPortId = entityPM.Transshipment2FromPortId;
                    entityMasterData.Transshipment2ToPortId = entityPM.Transshipment2ToPortId;
                    entityMasterData.Transshipment2CarrierId = entityPM.Transshipment2CarrierId;
                    entityMasterData.Transshipment2CarrierNumber = entityPM.Transshipment2CarrierNumber;
                    entityMasterData.Transshipment2AdditionalMAWBOBLBL = entityPM.Transshipment2AdditionalMAWBOBLBL;
                    entityMasterData.Transshipment2VesselId = entityPM.Transshipment2VesselId;
                    entityMasterData.Transshipment2ATA = entityPM.Transshipment2ATA;
                    entityMasterData.Transshipment2ATD = entityPM.Transshipment2ATD;
                    entityMasterData.Transshipment2ETA = entityPM.Transshipment2ETA;
                    entityMasterData.Transshipment2ETD = entityPM.Transshipment2ETD;

                    if (entityMasterData.Transshipment2STD == null)
                    {
                        entityMasterData.Transshipment2STD = entityPM.Transshipment2ETD;
                    }
                    if (entityMasterData.Transshipment2STA == null)
                    {
                        entityMasterData.Transshipment2STA = entityPM.Transshipment2ETA;
                    }

                    entityMasterData.Transshipment3FromPortId = entityPM.Transshipment3FromPortId;
                    entityMasterData.Transshipment3ToPortId = entityPM.Transshipment3ToPortId;
                    entityMasterData.Transshipment3CarrierId = entityPM.Transshipment3CarrierId;
                    entityMasterData.Transshipment3CarrierNumber = entityPM.Transshipment3CarrierNumber;
                    entityMasterData.Transshipment3AdditionalMAWBOBLBL = entityPM.Transshipment3AdditionalMAWBOBLBL;
                    entityMasterData.Transshipment3VesselId = entityPM.Transshipment3VesselId;
                    entityMasterData.Transshipment3ATA = entityPM.Transshipment3ATA;
                    entityMasterData.Transshipment3ATD = entityPM.Transshipment3ATD;
                    entityMasterData.Transshipment3ETA = entityPM.Transshipment3ETA;
                    entityMasterData.Transshipment3ETD = entityPM.Transshipment3ETD;

                    if (entityMasterData.Transshipment3STD == null)
                    {
                        entityMasterData.Transshipment3STD = entityPM.Transshipment3ETD;
                    }
                    if (entityMasterData.Transshipment3STA == null)
                    {
                        entityMasterData.Transshipment3STA = entityPM.Transshipment3ETA;
                    }

                    if (entityPM.TransportModeId == "I")
                    {
                        entityMasterData.TrailerNumber = entityPM.TrailerNumber;
                    }

                    if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
                    {
                        entityMasterData.MainCarriageFromPartnerId = entityPM.MainCarriageFromPartnerId;
                        entityMasterData.MainCarriageFromAddressId = entityPM.MainCarriageFromAddressId;
                        entityMasterData.MainCarriageToPartnerId = entityPM.MainCarriageToPartnerId;
                        entityMasterData.MainCarriageToAddressId = entityPM.MainCarriageToAddressId;
                        entityMasterData.Driver = entityPM.Driver;
                        entityMasterData.TruckNumber = entityPM.TruckNumber;
                    }

                    string myFinalDestinationPortId = null;

                    if (entityMasterData.Transshipment3ToPortId != null)
                    {
                        myFinalDestinationPortId = entityMasterData.Transshipment3ToPortId;
                    }

                    else if (entityMasterData.Transshipment2ToPortId != null)
                    {
                        myFinalDestinationPortId = entityMasterData.Transshipment2ToPortId;
                    }

                    else if (entityMasterData.Transshipment1ToPortId != null)
                    {
                        myFinalDestinationPortId = entityMasterData.Transshipment1ToPortId;
                    }

                    else
                    {
                        myFinalDestinationPortId = entityMasterData.MainCarriageToPortId;
                    }


                    entityMasterData.MainCarriageFinalDestinationPortId = myFinalDestinationPortId;
                    entityMasterData.MainCarriageCarrierPrefix = entityPM.MainCarriageCarrierPrefix;
                    entityMasterData.Transshipment1CarrierPrefix = entityPM.Transshipment1CarrierPrefix;
                    entityMasterData.Transshipment2CarrierPrefix = entityPM.Transshipment2CarrierPrefix;
                    entityMasterData.Transshipment3CarrierPrefix = entityPM.Transshipment3CarrierPrefix;
                    entityMasterData.IsKnownCargo = entityPM.IsKnownCargo;
                    entityMasterData.RegulatedAgentRANumber = entityPM.RegulatedAgentRANumber;
                    entityMasterData.KnownConsignorNumber = entityPM.KnownConsignorNumber;
                    entityMasterData.KCExpirationDate = entityPM.KCExpirationDate;
                    entityMasterData.ColoaderRANumber = entityPM.ColoaderRANumber;
                    entityMasterData.AWBPrintingSecurityStatusId = entityPM.AWBPrintingSecurityStatusId;
                    entityMasterData.AWBPrintingRANumber = entityPM.AWBPrintingRANumber;
                    entityMasterData.AdditionalHandlingInfo = entityPM.AdditionalHandlingInfo;
                    entityMasterData.AWBPrintingSecurityStatusEdited = entityPM.AWBPrintingSecurityStatusEdited;
                    entityMasterData.AWBPrintingRANumberEdited = entityPM.AWBPrintingRANumberEdited;
                    entityMasterData.AdditionalHandlingInfoEdited = entityPM.AdditionalHandlingInfoEdited;
                    entityMasterData.InterlineId = entityPM.InterlineId;
                    ComputeDepartureArrivalDates(entityMasterData, entityPM);

                    entityPM.OriginMainCarriageFromPortId = entityMasterData.MainCarriageFromPortId;
                    entityPM.OriginFinalDestinationPortId = entityMasterData.MainCarriageFinalDestinationPortId;
                }
            }
        }
        private static void ComputeDepartureArrivalDates(ShipmentMasterData entityMasterData, ShipmentPM entityPM)
        {
            entityPM.DepartureArrivalFromDate = entityPM.MainCarriageETD;
            if (entityPM.MainCarriageATD != null)
            {
                entityPM.DepartureArrivalFromDate = entityPM.MainCarriageATD;
            }

            DateTime? to_ETA = entityPM.MainCarriageETA;
            DateTime? to_ATA = entityPM.MainCarriageATA;

            if (entityPM.Transshipment1ToPortId != null)
            {
                if (entityPM.Transshipment1ETA != null)
                {
                    to_ETA = entityPM.Transshipment1ETA;
                }

                if (entityPM.Transshipment1ATA != null)
                {
                    to_ATA = entityPM.Transshipment1ATA;
                }
            }

            if (entityPM.Transshipment2ToPortId != null)
            {
                if (entityPM.Transshipment2ETA != null)
                {
                    to_ETA = entityPM.Transshipment2ETA;
                }

                if (entityPM.Transshipment2ATA != null)
                {
                    to_ATA = entityPM.Transshipment2ATA;
                }
            }

            if (entityPM.Transshipment3ToPortId != null)
            {
                if (entityPM.Transshipment3ETA != null)
                {
                    to_ETA = entityPM.Transshipment3ETA;
                }

                if (entityPM.Transshipment3ATA != null)
                {
                    to_ATA = entityPM.Transshipment3ATA;
                }
            }

            entityPM.MainCarriageFinalDestinationETA = to_ETA;
            entityPM.MainCarriageFinalDestinationATA = to_ATA;
            entityPM.DepartureArrivalToDate = to_ETA;
            if (to_ATA != null)
            {
                if (entityPM.DepartureArrivalToDate == null)
                {
                    entityPM.DepartureArrivalToDate = to_ATA;
                }

                else if (to_ATA > entityPM.DepartureArrivalToDate)
                {
                    entityPM.DepartureArrivalToDate = to_ATA;
                }
            }

            entityMasterData.DepartureArrivalFromDate = entityPM.DepartureArrivalFromDate;
            entityMasterData.DepartureArrivalToDate = entityPM.DepartureArrivalToDate;
            entityMasterData.MainCarriageFinalDestinationETA = entityPM.MainCarriageFinalDestinationETA;
            entityMasterData.MainCarriageFinalDestinationATA = entityPM.MainCarriageFinalDestinationATA;
        }
        private static void MapRoutings(ShipmentPM entityPM, Shipment entityPoco, bool isNewEntity)
        {
            entityPoco.PreCarriageFromPortId = entityPM.PreCarriageFromPortId;
            entityPoco.PreCarriageToPortId = entityPM.PreCarriageToPortId;
            entityPoco.PreCarriageCarrierId = entityPM.PreCarriageCarrierId;
            entityPoco.PreCarriageCarrierNumber = entityPM.PreCarriageCarrierNumber;
            entityPoco.PreCarriageVesselId = entityPM.PreCarriageVesselId;
            entityPoco.PreCarriageATA = entityPM.PreCarriageATA;
            entityPoco.PreCarriageATD = entityPM.PreCarriageATD;
            entityPoco.PreCarriageETA = entityPM.PreCarriageETA;
            entityPoco.PreCarriageETD = entityPM.PreCarriageETD;
            entityPoco.PreCarriageTransportModeId = entityPM.PreCarriageTransportModeId;
            entityPoco.OnCarriageFromPortId = entityPM.OnCarriageFromPortId;
            entityPoco.OnCarriageToPortId = entityPM.OnCarriageToPortId;
            entityPoco.OnCarriageCarrierId = entityPM.OnCarriageCarrierId;
            entityPoco.OnCarriageCarrierNumber = entityPM.OnCarriageCarrierNumber;
            entityPoco.OnCarriageVesselId = entityPM.OnCarriageVesselId;
            entityPoco.OnCarriageATA = entityPM.OnCarriageATA;
            entityPoco.OnCarriageATD = entityPM.OnCarriageATD;
            entityPoco.OnCarriageETA = entityPM.OnCarriageETA;
            entityPoco.OnCarriageETD = entityPM.OnCarriageETD;
            entityPoco.OnCarriageTransportModeId = entityPM.OnCarriageTransportModeId;
        }
        private static void MapPartners(ShipmentPM entityPM, Shipment entityPoco, bool isNewEntity)
        {
            entityPoco.ShipmentCustomerTypeCode = entityPM.ShipmentCustomerTypeCode;
            entityPoco.ConsigneeAddressOneTime = entityPM.ConsigneeAddressOneTime;
            entityPoco.ShipperAddressOneTime = entityPM.ShipperAddressOneTime;

            entityPoco.CustomerId = entityPM.CustomerId;
            entityPoco.CustomerAddressId = entityPM.CustomerAddressId;
            entityPoco.CustomerContactId = entityPM.CustomerContactId;
            entityPoco.CustomerReference1 = entityPM.CustomerReference1;
            entityPoco.CustomerReference2 = entityPM.CustomerReference2;

            entityPoco.FreightForwarderId = entityPM.FreightForwarderId;
            entityPoco.FreightForwarderAddressId = entityPM.FreightForwarderAddressId;
            entityPoco.FreightForwarderContactId = entityPM.FreightForwarderContactId;
            entityPoco.FreightForwarderReference = entityPM.FreightForwarderReference;

            entityPoco.ShipperId = entityPM.ShipperId;
            entityPoco.ShipperAddressId = entityPM.ShipperAddressId;
            entityPoco.ShipperContactId = entityPM.ShipperContactId;
            entityPoco.ShipperReference1 = entityPM.ShipperReference1;
            entityPoco.ShipperReference2 = entityPM.ShipperReference2;

            entityPoco.ConsigneeId = entityPM.ConsigneeId;
            entityPoco.ConsigneeAddressId = entityPM.ConsigneeAddressId;
            entityPoco.ConsigneeContactId = entityPM.ConsigneeContactId;
            entityPoco.ConsigneeReference1 = entityPM.ConsigneeReference1;
            entityPoco.ConsigneeReference2 = entityPM.ConsigneeReference2;

            entityPoco.AgentId = entityPM.AgentId;


            entityPoco.AgentAddressId = entityPM.AgentAddressId;
            entityPoco.AgentContactId = entityPM.AgentContactId;
            entityPoco.AgentReference1 = entityPM.AgentReference1;
            entityPoco.AgentReference2 = entityPM.AgentReference2;

            entityPoco.IssuingCarrierAgentId = entityPM.IssuingCarrierAgentId;
            entityPoco.IssuingCarrierAddressId = entityPM.IssuingCarrierAddressId;
            entityPoco.IssuingCarrierIATACode = entityPM.IssuingCarrierIATACode;

            entityPoco.CustomAgentExportId = entityPM.CustomAgentExportId;
            entityPoco.CustomAgentExportAddressId = entityPM.CustomAgentExportAddressId;
            entityPoco.CustomAgentExportContactId = entityPM.CustomAgentExportContactId;
            entityPoco.CustomAgentExportReference = entityPM.CustomAgentExportReference;

            entityPoco.CustomAgentImportId = entityPM.CustomAgentImportId;
            entityPoco.CustomAgentImportAddressId = entityPM.CustomAgentImportAddressId;
            entityPoco.CustomAgentImportContactId = entityPM.CustomAgentImportContactId;
            entityPoco.CustomAgentImportReference = entityPM.CustomAgentImportReference;

            entityPoco.Notify1Id = entityPM.Notify1Id;
            entityPoco.Notify1AddressId = entityPM.Notify1AddressId;
            entityPoco.Notify1ContactId = entityPM.Notify1ContactId;

            entityPoco.Notify2Id = entityPM.Notify2Id;
            entityPoco.Notify2AddressId = entityPM.Notify2AddressId;
            entityPoco.Notify2ContactId = entityPM.Notify2ContactId;

            entityPoco.ShipperNotExporterId = entityPM.ShipperNotExporterId;
            entityPoco.ShipperNotExporterAddressId = entityPM.ShipperNotExporterAddressId;
            entityPoco.ShipperNotExporterContactId = entityPM.ShipperNotExporterContactId;

            entityPoco.ConsigneeNotImporterId = entityPM.ConsigneeNotImporterId;
            entityPoco.ConsigneeNotImporterAddressId = entityPM.ConsigneeNotImporterAddressId;
            entityPoco.ConsigneeNotImporterContactId = entityPM.ConsigneeNotImporterContactId;

            entityPoco.ColoaderId = entityPM.ColoaderId;
            entityPoco.ColoaderAddressId = entityPM.ColoaderAddressId;
            entityPoco.ColoaderContactId = entityPM.ColoaderContactId;
            entityPoco.ColoaderReference1 = entityPM.ColoaderReference1;

            entityPoco.CustomClearancePointId = entityPM.CustomClearancePointId;
            entityPoco.CustomClearancePointAddressId = entityPM.CustomClearancePointAddressId;
            entityPoco.CustomClearancePointContactId = entityPM.CustomClearancePointContactId;
            entityPoco.CustomClearancePointReference1 = entityPM.CustomClearancePointReference1;


            entityPoco.ConsolidatorId = entityPM.ConsolidatorId;
            entityPoco.ConsolidatorAddressId = entityPM.ConsolidatorAddressId;
            entityPoco.ConsolidatorContactId = entityPM.ConsolidatorContactId;
            entityPoco.ConsolidatorReference = entityPM.ConsolidatorReference;

            entityPoco.ReleasingAgentId = entityPM.ReleasingAgentId;
            entityPoco.ReleasingAgentAddressId = entityPM.ReleasingAgentAddressId;
            entityPoco.ReleasingAgentContactId = entityPM.ReleasingAgentContactId;
            entityPoco.ReleasingAgentReference1 = entityPM.ReleasingAgentReference1;
            entityPoco.ReleasingAgentReference2 = entityPM.ReleasingAgentReference2;
        }
        private static void MapAWBFields(ShipmentPM entityPM, Shipment entityPoco, bool isNewEntity)
        {
            //entityPoco.AWBChargeRate = entityPM.AWBChargeRate;
            //entityPoco.AWBChargeAmount = entityPM.AWBChargeAmount;
            //entityPoco.AWBCommodityItemNumber = entityPM.AWBCommodityItemNumber;
            //entityPoco.RateClassCode = entityPM.RateClassCode;

            entityPoco.AWBFreightAmountCollect = entityPM.AWBFreightAmountCollect;
            entityPoco.AWBFreightAmountPrepaid = entityPM.AWBFreightAmountPrepaid;
            entityPoco.AWBCurrencyId = entityPM.AWBCurrencyId;
            entityPoco.AWBAccountingInformation = entityPM.AWBAccountingInformation;
            entityPoco.AWBCarrierTarrifReference = entityPM.AWBCarrierTarrifReference;
            entityPoco.AWBDeclaredValueForCarriage = entityPM.AWBDeclaredValueForCarriage;
            entityPoco.AWBDeclaredValueForCustoms = entityPM.AWBDeclaredValueForCustoms;
            entityPoco.AWBInsurrenceValue = entityPM.AWBInsurrenceValue;
            entityPoco.AWBHandlingInformation = entityPM.AWBHandlingInformation;
            entityPoco.SCI = entityPM.SCI;
            entityPoco.AWBComments = entityPM.AWBComments;
            entityPoco.AWBPrintingComments = entityPM.AWBPrintingComments;
            entityPoco.AWBSignature = entityPM.AWBSignature;
            entityPoco.AWBPlace = entityPM.AWBPlace;
            entityPoco.AWBChargesCodeCode = entityPM.AWBChargesCodeCode;
            entityPoco.AWBSpecialHandlingCodeId1 = entityPM.AWBSpecialHandlingCodeId1;
            entityPoco.AWBSpecialHandlingCodeId2 = entityPM.AWBSpecialHandlingCodeId2;
            entityPoco.AWBSpecialHandlingCodeId3 = entityPM.AWBSpecialHandlingCodeId3;
            entityPoco.AWBSpecialHandlingCodeId4 = entityPM.AWBSpecialHandlingCodeId4;
            entityPoco.AWBSpecialHandlingCodeId5 = entityPM.AWBSpecialHandlingCodeId5;
            entityPoco.AWBSpecialHandlingCodeId6 = entityPM.AWBSpecialHandlingCodeId6;
            entityPoco.AWBSpecialHandlingCodeId7 = entityPM.AWBSpecialHandlingCodeId7;
            entityPoco.AWBSpecialHandlingCodeId8 = entityPM.AWBSpecialHandlingCodeId8;
            entityPoco.AWBSpecialHandlingCodeId9 = entityPM.AWBSpecialHandlingCodeId9;
        }
        private static void MapTotalsFields(ShipmentPM entityPM, Shipment entityPoco, bool isNewEntity)
        {
            entityPoco.ProfitCurrencyId = entityPM.ProfitCurrencyId;
            entityPoco.ProfitExchangeRate = entityPM.ProfitExchangeRate;
            entityPoco.EstimateProfitInLocalCurrency = entityPM.EstimateProfitInLocalCurrency;
            entityPoco.EstimateProfitInProfitCurrency = entityPM.EstimateProfitInProfitCurrency;

            // Ayman: these fields should not be mapped on update            
            if (isNewEntity || entityPM.IsHybrid) // Islam: we need to update these fields in hybrid case
            {
                entityPoco.ShipmentReceivableStatusCode = entityPM.ShipmentReceivableStatusCode;
                entityPoco.ShipmentPayableStatusCode = entityPM.ShipmentPayableStatusCode;
                entityPoco.OpenPayablesInLocalCurrency = entityPM.OpenPayablesInLocalCurrency == null ? 0 : entityPM.OpenPayablesInLocalCurrency;
                entityPoco.AccountedPayablesInLocalCurrency = entityPM.AccountedPayablesInLocalCurrency == null ? 0 : entityPM.AccountedPayablesInLocalCurrency;
                entityPoco.OpenReceivablesInLocalCurrency = entityPM.OpenReceivablesInLocalCurrency == null ? 0 : entityPM.OpenReceivablesInLocalCurrency;
                entityPoco.AccountedReceivablesInLocalCurrency = entityPM.AccountedReceivablesInLocalCurrency == null ? 0 : entityPM.AccountedReceivablesInLocalCurrency;
                entityPoco.ProfitInLocalCurrency = entityPM.ProfitInLocalCurrency;
                entityPoco.OpenPayablesInProfitCurrency = entityPM.OpenPayablesInProfitCurrency == null ? 0 : entityPM.OpenPayablesInProfitCurrency;
                entityPoco.AccountedPayablesInProfitCurrency = entityPM.AccountedPayablesInProfitCurrency == null ? 0 : entityPM.AccountedPayablesInProfitCurrency;
                entityPoco.OpenReceivablesInProfitCurrency = entityPM.OpenReceivablesInProfitCurrency == null ? 0 : entityPM.OpenReceivablesInProfitCurrency;
                entityPoco.AccountedReceivablesInProfitCurrency = entityPM.AccountedReceivablesInProfitCurrency == null ? 0 : entityPM.AccountedReceivablesInProfitCurrency;
                entityPoco.ProfitInProfitCurrency = entityPM.ProfitInProfitCurrency == null ? 0 : entityPM.ProfitInProfitCurrency;
            }
        }
        private static void MapWeightsFields(ShipmentPM entityPM, Shipment entityPoco, bool isNewEntity)
        {
            if (entityPM.Ratio == null)
            {
                string countryCode = null;

                TenantRepository tenantRepository = new TenantRepository(entityPM.Tenant);
                Tenant tenant = tenantRepository.GetSingleTenant(entityPM.Tenant);
                if (tenant != null)
                {
                    if (string.IsNullOrEmpty(tenant.AddressId))
                    {
                        AddressRepository addressRepository = new AddressRepository(entityPM.Tenant);
                        Address address = addressRepository.GetSingleAddress(tenant.AddressId, entityPM.Tenant);
                        if (address != null)
                        {
                            CountryRepository countryRepository = new CountryRepository(entityPM.Tenant);
                            Country country = countryRepository.GetSingleCountry(address.CountryId, entityPM.Tenant);
                            if (country != null)
                            {
                                countryCode = country.Code;
                            }
                        }
                    }
                }

                entityPM.Ratio = GetRatio(entityPM.DirectionId, entityPM.TransportModeId, entityPM.ShipmentTypeId, countryCode);
            }

            if (entityPM.DimFactor == null)
            {
                entityPM.DimFactor = GetDimFactorFromRatio(entityPM.Ratio, entityPM.DimensionsUnitCode, entityPM.ChargeableWeightUnitCode);
            }

            entityPoco.Ratio = entityPM.Ratio;
            entityPoco.DimFactor = entityPM.DimFactor;
            entityPoco.NumberOfPackages = entityPM.NumberOfPackages;
            entityPoco.NumberOfContainers = entityPM.NumberOfContainers;
            entityPoco.DimensionsUnitCode = entityPM.DimensionsUnitCode;
            entityPoco.VolumeUnitCode = entityPM.VolumeUnitCode;
            entityPoco.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
            entityPoco.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;
            entityPoco.GrossWeightEdited = entityPM.GrossWeightEdited;
            entityPoco.ChargeableWeightEdited = entityPM.ChargeableWeightEdited;
            entityPoco.Volume = entityPM.Volume;
            entityPoco.GrossWeight = entityPM.GrossWeight;
            entityPoco.VolumetricWeight = entityPM.VolumetricWeight;
            entityPoco.ChargeableWeight = entityPM.ChargeableWeight;
            entityPoco.VolumeInCBM = GetVolumeInCBM(entityPM.VolumeUnitCode, entityPM.Volume);
            entityPoco.GrossWeightInKG = entityPM.GrossWeightInKG = GetWeightInKG(entityPM.GrossWeightUnitCode, entityPM.GrossWeight);
            entityPoco.GrossWeightPerTon = entityPM.GrossWeightPerTon = GetWeightInTon(entityPM.GrossWeightInKG);
            entityPoco.ChargeableWeightInKG = GetWeightInKG(entityPM.ChargeableWeightUnitCode, entityPM.ChargeableWeight);

            entityPoco.CustomsDeclarationNumber = entityPM.CustomsDeclarationNumber;
            entityPoco.ShipperName = entityPM.ShipperName;
            entityPoco.ConsigneeName = entityPM.ConsigneeName;

            if (!entityPM.IsHybrid || entityPM.DontAddToImportersQueue)
            {
                entityPoco.ForwarderShipmentNumber = entityPM.ForwarderShipmentNumber;
                entityPoco.CustomerShipmentNumber = entityPM.CustomerShipmentNumber;
                entityPoco.CustomerTenantNumber = entityPM.CustomerTenantNumber;
            }
            if (!string.IsNullOrEmpty(entityPM.ForwarderShipmentNumber))
            {
                entityPoco.ComputedForwarderShipmentNumber = entityPM.ForwarderShipmentNumber;
            }
            else
            {
                entityPoco.ComputedForwarderShipmentNumber = entityPM.Id;
            }
            if (entityPM.IsHybrid && !string.IsNullOrEmpty(entityPM.CustomerShipmentNumber))
            {
                entityPoco.CustomerShipmentNumber = entityPM.CustomerShipmentNumber;
            }

            if (MethodHelper.IsLCLEntity(entityPM.TransportModeId, entityPM.ShipmentTypeId))
            {
                entityPM.PackagesQuantity = entityPM.NumberOfPackages;
            }

            else
            {
                entityPM.PackagesQuantity = entityPM.NumberOfContainers;
            }

            entityPoco.PackagesQuantity = entityPM.PackagesQuantity;
        }

        public static double? GetRatio(string directionId, string transportModeId, string shipmentTypeId, string countryCode)
        {
            double? myResult = null;

            if (!string.IsNullOrEmpty(countryCode))
            {
                if (countryCode.ToUpper() == "US")
                {
                    if (directionId == "D")
                    {
                        if (transportModeId == "A")
                        {
                            myResult = 7;
                        }

                        else if (transportModeId == "I")
                        {
                            if (shipmentTypeId == "LTL")
                            {
                                myResult = 9;
                            }
                        }
                    }
                }
            }

            if (myResult == null)
            {
                {
                    switch (transportModeId)
                    {
                        case "A": { myResult = 6; break; }
                        case "O": { myResult = 1; break; }
                        case "I":
                            {
                                if (shipmentTypeId == "LTL")
                                {
                                    myResult = 3.3;
                                }

                                else
                                {
                                    myResult = 1;
                                }

                                break;
                            }
                    }
                }
            }

            return myResult;
        }
        public static double? GetDimFactorFromRatio(double? myRatio, string dimentionCode, string weightCode)
        {
            double? myResult = null;

            if (myRatio != null)
            {
                double WeightFactorOfConvert = 1;
                double DimensiosFactorOfConvert = 1;

                if (!string.IsNullOrEmpty(weightCode))
                {
                    switch (weightCode.ToUpper())
                    {
                        case "KG": { WeightFactorOfConvert = 1; break; }
                        case "LB": { WeightFactorOfConvert = 0.45359237; break; }
                        case "MT": { WeightFactorOfConvert = 1000; break; }
                    }
                }

                if (!string.IsNullOrEmpty(dimentionCode))
                {
                    switch (dimentionCode.ToUpper())
                    {
                        case "CM": { DimensiosFactorOfConvert = 1; break; }
                        case "INC": { DimensiosFactorOfConvert = 2.54; break; }
                        case "FT": { DimensiosFactorOfConvert = 30.48; break; }
                    }
                }

                myResult = WeightFactorOfConvert * 1000 * myRatio / Math.Pow(DimensiosFactorOfConvert, 3);
            }

            if (myResult != null)
            {
                string toString = myResult.ToString();
                string[] myArray = toString.Split('.');

                if (myArray.Length > 1)
                {
                    string strDigits = "0." + myArray[1];
                    double? digits = Convert.ToDouble(strDigits);

                    if (digits < 0.5)
                    {
                        myResult = Math.Floor(myResult.Value);
                    }

                    else
                    {
                        myResult = Math.Ceiling(myResult.Value);
                    }
                }
            }

            return myResult;
        }
        public static double? GetVolumeInCBM(string volumeCode, double? volume)
        {
            double? myResult = null;

            if (volume != null)
            {
                double? factorOfConvert = 1;

                if (!string.IsNullOrEmpty(volumeCode))
                {
                    switch (volumeCode.ToUpper())
                    {
                        case "CBM": { factorOfConvert = 1; break; }
                        case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                        case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
                    }
                }

                myResult = volume / factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static double? GetWeightInKG(string weightCode, double? weight)
        {
            double? myResult = null;

            if (weight != null)
            {
                double? factorOfConvert = 1;

                if (!string.IsNullOrEmpty(weightCode))
                {
                    switch (weightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                        case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                    }
                }

                myResult = weight * factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static double? GetWeightInTon(double? weightInKG)
        {
            double? myResult = null;

            if (weightInKG != null)
            {
                myResult = weightInKG / 1000;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static void BuildSearchField(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, List<ShipmentPackagePM> myPackagesList)
        {
            string mySearchFields = "";

            int tenant = entityPM.Tenant;
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.House);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Master);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipmentNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomFileNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.AWBCarrierTarrifReference);

            // New Fields
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomsDeclarationNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ForwarderShipmentNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.TransportDocumentNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ImportManifest);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.BookingConfirmationNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CarrierTransportDocumentNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipperName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ConsigneeName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Transshipment1AdditionalMAWBOBLBL);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Transshipment2AdditionalMAWBOBLBL);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Transshipment3AdditionalMAWBOBLBL);

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ProjectNumber);

            #region Quote
            if (!string.IsNullOrEmpty(entityPM.QuoteId))
            {
                QuoteRepository myQuoteRepository = new QuoteRepository(tenant);
                string myQuoteNumber = myQuoteRepository.GetQuoteNumber(entityPM.QuoteId);
                MethodHelper.AddToSearchFields(ref mySearchFields, myQuoteNumber);
            }
            #endregion

            #region Status
            if (!string.IsNullOrEmpty(entityPM.StatusId))
            {
                EntityStatusRepository entityStatusRepository = new EntityStatusRepository(tenant);
                EntityStatus entityStatus = entityStatusRepository.GetSingleEntityStatus(entityPM.StatusId, tenant);
                if (entityStatus != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityStatus.Code);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityStatus.Name);
                }
            }
            #endregion

            #region Vessel
            if (!string.IsNullOrEmpty(entityPM.MainCarriageVesselId))
            {
                VesselRepository vesselRepository = new VesselRepository(tenant);
                Vessel vessel = vesselRepository.GetSingleVessel(entityPM.MainCarriageVesselId, tenant);
                if (vessel != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, vessel.Code);
                    MethodHelper.AddToSearchFields(ref mySearchFields, vessel.EnglishName);
                }
            }
            #endregion

            #region Long Master
            if (entityPoco != null && entityMasterData != null)
            {
                string myLongMaster = EntityFieldsHelper.GetLongMasterField(entityPoco, entityMasterData);
                MethodHelper.AddToSearchFields(ref mySearchFields, myLongMaster);
            }
            #endregion

            #region MainCarriageCarrier
            if (!string.IsNullOrEmpty(entityPM.MainCarriageCarrierId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.MainCarriageCarrierId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.Code);
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.MainCarriageCarrierNumber);
                }
            }
            #endregion

            #region Salesman
            if (!string.IsNullOrEmpty(entityPM.SalesmanUserId))
            {
                UserRepository myUserRepository = new UserRepository(tenant);
                User myUser = myUserRepository.GetSingleUser(entityPM.SalesmanUserId, tenant, true);
                if (myUser != null)
                {
                    if (myUser.Contact != null)
                    {
                        MethodHelper.AddToSearchFields(ref mySearchFields, myUser.Contact.EnglishName);
                    }
                }
            }
            #endregion

            #region Ports

            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.FromPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.MainCarriageFromPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.PreCarriageFromPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.OnCarriageFromPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.Transshipment1FromPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.Transshipment2FromPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.Transshipment3FromPortId);

            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.ToPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.MainCarriageToPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.PreCarriageToPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.OnCarriageToPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.Transshipment1ToPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.Transshipment2ToPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.Transshipment3ToPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.MainCarriageFinalDestinationPortId);

            #endregion

            #region Partners

            if (!string.IsNullOrEmpty(entityPM.AgentId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.AgentId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.AgentReference1);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.AgentReference2);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ShipperId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.ShipperId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipperReference1);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipperReference2);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ConsigneeId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.ConsigneeId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ConsigneeReference1);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ConsigneeReference2);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.CustomerId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerReference1);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerReference2);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.Notify1Id))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.Notify1Id, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.Notify2Id))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.Notify2Id, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.IssuingCarrierAgentId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.IssuingCarrierAgentId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.CustomAgentImportId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.CustomAgentImportId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomAgentImportReference);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.CustomAgentExportId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.CustomAgentExportId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomAgentExportReference);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ShipperNotExporterId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.ShipperNotExporterId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ConsigneeNotImporterId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.ConsigneeNotImporterId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.FreightForwarderId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.FreightForwarderId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.FreightForwarderReference);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ConsolidatorId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.ConsolidatorId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ConsolidatorReference);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ReleasingAgentId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.ReleasingAgentId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ReleasingAgentReference1);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ReleasingAgentReference2);
                }
            }
            #endregion

            #region Invoices
            foreach (ShipmentARInvoicePM item in entityPM.ShipmentARInvoices)
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, item.InvoiceNumber);
            }
            #endregion

            #region Packages
            foreach (ShipmentPackagePM item in myPackagesList)
            {
                if (!string.IsNullOrEmpty(item.ContainerNumber))
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, item.ContainerNumber);
                }
            }
            #endregion

            #region Custom Fields
            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Shipment", tenant).Where(o => o.DataTypeCode == "Text" || o.DataTypeCode == "nText").ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            foreach (ObjectField field in customFields)
            {
                object value = customFieldResolver.GetFieldValue(entityPM, field, tenant);
                if (value != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, value.ToString());
                }
            }
            #endregion

            var FormattedSearchFields = GetFormattedSearchFields(mySearchFields);
            var NewSearchFields = mySearchFields + "*" + FormattedSearchFields;

            if (NewSearchFields.Length > 4000)
            {
                NewSearchFields = NewSearchFields.Substring(0, 3999);
            }
            if (entityPM.Tenant == 1 || entityPoco.Tenant == 1)
            {
                //var FormattedSearchFields = GetFormattedSearchFields(mySearchFields);
                //var NewSearchFields = mySearchFields + "*" + FormattedSearchFields;
                //if (NewSearchFields.Length > 4000)
                //{
                //    NewSearchFields = NewSearchFields.Substring(0, 3999);
                //}
                entityPM.SearchFields = NewSearchFields;
                entityPoco.SearchFields = NewSearchFields;
            }
            else
            {
                entityPM.SearchFields = mySearchFields;
                entityPoco.SearchFields = mySearchFields;
            }

        }

        public static string GetFormattedSearchFields(string OrigionalString)
        {
            List<string> Strings = OrigionalString.Split(',').ToList();
            Strings = Strings.Where(a => !string.IsNullOrEmpty(a)).ToList();
            List<string> SubStrings = new List<string>();
            foreach (var item in Strings)
            {
                for (int i = item.Length - 2; i > -1; i--)
                {
                    SubStrings.Add(item.Substring(i, item.Length - i));
                }
            }

            string FinalString = string.Join(",", SubStrings);
            return FinalString;
        }

        //public static List<NotifyPropertyChangeValues> GetEntityPMChanges(ShipmentPM pm, ShipmentPM changeTrackingPM)
        //{
        //    List<NotifyPropertyChangeValues> ChangedProperties = new List<NotifyPropertyChangeValues>();
        //    if (changeTrackingPM.IncotermId != pm.IncotermId)
        //    {
        //        NotifyPropertyChangeValues values = new NotifyPropertyChangeValues() { PropertyName = "IncotermId", OldValue = changeTrackingPM.IncotermId, NewValue = pm.IncotermId, PropertyType = "string" };
        //        values.Id = Guid.NewGuid().ToString();
        //        ChangedProperties.Add(values);
        //    }

        //    return ChangedProperties;
        //}

        public static List<NotifyPropertyChangeValues> BuildChangedProperties(ShipmentPM pm, ShipmentPM changeTrackingPM)
        {
            List<NotifyPropertyChangeValues> notifyPropertyChangeValuesList = new List<NotifyPropertyChangeValues>();

            AddFieldChangedProperties(changeTrackingPM, "CreatedByUserId", changeTrackingPM.CreatedByUserId, pm.CreatedByUserId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "DirectionId", changeTrackingPM.DirectionId, pm.DirectionId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "MainCarriageTransportModeId", changeTrackingPM.TransportModeId, pm.TransportModeId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "ShipmentLevelCode", changeTrackingPM.ShipmentLevelCode, pm.ShipmentLevelCode, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "CustomerId", changeTrackingPM.CustomerId, pm.CustomerId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "UpdatedByUserId", changeTrackingPM.UpdatedByUserId, pm.UpdatedByUserId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "AccountManagerUserId", changeTrackingPM.AccountManagerUserId, pm.AccountManagerUserId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "BranchId", changeTrackingPM.BranchId, pm.BranchId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "DepartmentId", changeTrackingPM.DepartmentId, pm.DepartmentId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "MainCarriageCarrierId", changeTrackingPM.MainCarriageCarrierId, pm.MainCarriageCarrierId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "AgentId", changeTrackingPM.AgentId, pm.AgentId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "OriginShipmentId", changeTrackingPM.OriginShipmentId, pm.OriginShipmentId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "FinalDistenationPortId", changeTrackingPM.FinalDistenationPortId, pm.FinalDistenationPortId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "SalesmanUserId", changeTrackingPM.SalesmanUserId, pm.SalesmanUserId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "StatusId", changeTrackingPM.StatusId, pm.StatusId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "IncotermId", changeTrackingPM.IncotermId, pm.IncotermId, "string", notifyPropertyChangeValuesList);

            AddFieldChangedProperties(changeTrackingPM, "IsOperationalClosed", changeTrackingPM.IsOperationalClosed, pm.IsOperationalClosed, "bool", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "IsAccountingClosed", changeTrackingPM.IsAccountingClosed, pm.IsAccountingClosed, "bool", notifyPropertyChangeValuesList);

            AddFieldChangedProperties(changeTrackingPM, "MainCarriageETD", changeTrackingPM.MainCarriageETD, pm.MainCarriageETD, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "MainCarriageATD", changeTrackingPM.MainCarriageATD, pm.MainCarriageATD, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "MainCarriageETA", changeTrackingPM.MainCarriageETA, pm.MainCarriageETA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "MainCarriageATA", changeTrackingPM.MainCarriageATA, pm.MainCarriageATA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "MainCarriageFinalDestinationETA", changeTrackingPM.MainCarriageFinalDestinationETA, pm.MainCarriageFinalDestinationETA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "MainCarriageFinalDestinationATA", changeTrackingPM.MainCarriageFinalDestinationATA, pm.MainCarriageFinalDestinationATA, "DateTime?", notifyPropertyChangeValuesList);








            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field1, pm.Field1, "Field1", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field2, pm.Field2, "Field2", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field3, pm.Field3, "Field3", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field4, pm.Field4, "Field4", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field5, pm.Field5, "Field5", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field6, pm.Field6, "Field6", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field7, pm.Field7, "Field7", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field8, pm.Field8, "Field8", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field9, pm.Field9, "Field9", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field10, pm.Field10, "Field10", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field11, pm.Field11, "Field11", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field12, pm.Field12, "Field12", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field13, pm.Field13, "Field13", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field14, pm.Field14, "Field14", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field15, pm.Field15, "Field15", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field16, pm.Field16, "Field16", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field17, pm.Field17, "Field17", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field18, pm.Field18, "Field18", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field19, pm.Field19, "Field19", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field20, pm.Field20, "Field20", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field21, pm.Field21, "Field21", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field22, pm.Field22, "Field22", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field23, pm.Field23, "Field23", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field24, pm.Field24, "Field24", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field25, pm.Field25, "Field25", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field26, pm.Field26, "Field26", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field27, pm.Field27, "Field27", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field28, pm.Field28, "Field28", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field29, pm.Field29, "Field29", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field30, pm.Field30, "Field30", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field31, pm.Field31, "Field31", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field32, pm.Field32, "Field32", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field33, pm.Field33, "Field33", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field34, pm.Field34, "Field34", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field35, pm.Field35, "Field35", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field36, pm.Field36, "Field36", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field37, pm.Field37, "Field37", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field38, pm.Field38, "Field38", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field39, pm.Field39, "Field39", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field40, pm.Field40, "Field40", notifyPropertyChangeValuesList);

            //if (!string.IsNullOrEmpty(LogitudeSettings.DeploymentStage) && LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1")
            // {
            AddFieldChangedProperties(changeTrackingPM, "IsDigitalSignRequired", changeTrackingPM.IsDigitalSignRequired, pm.IsDigitalSignRequired, "bool", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "IsRequestedDocuments", changeTrackingPM.IsRequestedDocuments, pm.IsRequestedDocuments, "bool", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "IsDepositionRequired", changeTrackingPM.IsDepositionRequired, pm.IsDepositionRequired, "bool", notifyPropertyChangeValuesList);
            // }


            return notifyPropertyChangeValuesList;
        }

        private static void AddFieldChangedProperties(ShipmentPM changeTrackingPM, string fieldName, object objectOldValue, object objectNewValue, string PropertyType, List<NotifyPropertyChangeValues> notifyPropertyChangeValuesList)
        {
            string oldValue = objectOldValue != null ? objectOldValue.ToString() : "";
            string newValue = objectNewValue != null ? objectNewValue.ToString() : "";

            if (oldValue != newValue)
            {
                NotifyPropertyChangeValues values = new NotifyPropertyChangeValues() { PropertyName = fieldName, OldValue = objectOldValue, NewValue = objectNewValue, PropertyType = PropertyType };
                notifyPropertyChangeValuesList.Add(values);
            }
        }



        private static void AddCustomFieldChangedProperties(ShipmentPM changeTrackingPM, CustomFieldClass oldFieldValue, CustomFieldClass newFieldValue, string fieldName, List<NotifyPropertyChangeValues> notifyPropertyChangeValuesList)
        {
            string oldValue = oldFieldValue != null ? !string.IsNullOrEmpty(oldFieldValue.Value) ? oldFieldValue.Value : "" : "";
            string newValue = newFieldValue != null ? !string.IsNullOrEmpty(newFieldValue.Value) ? newFieldValue.Value : "" : "";

            if (oldValue != newValue)
            {
                NotifyPropertyChangeValues values = new NotifyPropertyChangeValues() { PropertyName = fieldName, OldValue = oldValue, NewValue = newValue, PropertyType = "CustomFieldClass" };
                notifyPropertyChangeValuesList.Add(values);
            }
        }

        public static ShipmentPM MapShipmentPMToShipmentPMForAutomation(ShipmentPM masterShipment, ShipmentPM houseShipment)
        {
            ShipmentPM shipmentPM = new ShipmentPM();

            shipmentPM.Id = houseShipment.Id;
            shipmentPM.Tenant = houseShipment.Tenant;
            shipmentPM.CreatedByUserId = houseShipment.CreatedByUserId;
            shipmentPM.UpdatedByUserId = houseShipment.UpdatedByUserId;
            shipmentPM.DirectionId = houseShipment.DirectionId;
            shipmentPM.ShipmentLevelCode = houseShipment.ShipmentLevelCode;
            shipmentPM.CustomerId = houseShipment.CustomerId;
            shipmentPM.AccountManagerUserId = houseShipment.AccountManagerUserId;
            shipmentPM.BranchId = houseShipment.BranchId;
            shipmentPM.DepartmentId = houseShipment.DepartmentId;
            shipmentPM.AgentId = houseShipment.AgentId;
            shipmentPM.IsAccountingClosed = houseShipment.IsAccountingClosed;
            shipmentPM.IsOperationalClosed = houseShipment.IsOperationalClosed;
            shipmentPM.OriginShipmentId = houseShipment.OriginShipmentId;
            shipmentPM.SalesmanUserId = houseShipment.SalesmanUserId;
            shipmentPM.MainCarriageTransportModeId = houseShipment.TransportModeId;
            shipmentPM.TransportModeId = houseShipment.TransportModeId;
            shipmentPM.IncotermId = houseShipment.IncotermId;
            shipmentPM.CustomerContactId = houseShipment.CustomerContactId;
            shipmentPM.AgentContactId = houseShipment.AgentContactId;

            shipmentPM.IsDepositionRequired = houseShipment.IsDepositionRequired;
            shipmentPM.IsDigitalSignRequired = houseShipment.IsDigitalSignRequired;
            shipmentPM.IsRequestedDocuments = houseShipment.IsRequestedDocuments;


            shipmentPM.Field1 = houseShipment.Field1;
            shipmentPM.Field2 = houseShipment.Field2;
            shipmentPM.Field3 = houseShipment.Field3;
            shipmentPM.Field4 = houseShipment.Field4;
            shipmentPM.Field5 = houseShipment.Field5;
            shipmentPM.Field6 = houseShipment.Field6;
            shipmentPM.Field7 = houseShipment.Field7;
            shipmentPM.Field8 = houseShipment.Field8;
            shipmentPM.Field9 = houseShipment.Field9;
            shipmentPM.Field10 = houseShipment.Field10;
            shipmentPM.Field11 = houseShipment.Field11;
            shipmentPM.Field12 = houseShipment.Field12;
            shipmentPM.Field13 = houseShipment.Field13;
            shipmentPM.Field14 = houseShipment.Field14;
            shipmentPM.Field15 = houseShipment.Field15;
            shipmentPM.Field16 = houseShipment.Field16;
            shipmentPM.Field17 = houseShipment.Field17;
            shipmentPM.Field18 = houseShipment.Field18;
            shipmentPM.Field19 = houseShipment.Field19;
            shipmentPM.Field20 = houseShipment.Field20;
            shipmentPM.Field21 = houseShipment.Field21;
            shipmentPM.Field22 = houseShipment.Field22;
            shipmentPM.Field23 = houseShipment.Field23;
            shipmentPM.Field24 = houseShipment.Field24;
            shipmentPM.Field25 = houseShipment.Field25;
            shipmentPM.Field26 = houseShipment.Field26;
            shipmentPM.Field27 = houseShipment.Field27;
            shipmentPM.Field28 = houseShipment.Field28;
            shipmentPM.Field29 = houseShipment.Field29;
            shipmentPM.Field30 = houseShipment.Field30;
            shipmentPM.Field31 = houseShipment.Field31;
            shipmentPM.Field32 = houseShipment.Field32;
            shipmentPM.Field33 = houseShipment.Field33;
            shipmentPM.Field34 = houseShipment.Field34;
            shipmentPM.Field35 = houseShipment.Field35;
            shipmentPM.Field36 = houseShipment.Field36;
            shipmentPM.Field37 = houseShipment.Field37;
            shipmentPM.Field38 = houseShipment.Field38;
            shipmentPM.Field39 = houseShipment.Field39;
            shipmentPM.Field30 = houseShipment.Field30;

            shipmentPM.MainCarriageCarrierId = masterShipment.MainCarriageCarrierId;
            shipmentPM.MainCarriageETA = masterShipment.MainCarriageETA;
            shipmentPM.MainCarriageETD = masterShipment.MainCarriageETD;
            shipmentPM.MainCarriageATD = masterShipment.MainCarriageATD;
            shipmentPM.MainCarriageATA = masterShipment.MainCarriageATA;
            shipmentPM.FinalDistenationPortId = masterShipment.FinalDistenationPortId;
            shipmentPM.StatusId = masterShipment.StatusId;


            return shipmentPM;

        }

    }


    }
