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
using Logitude.Infrastructure.Data.Models.AuditLog;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        public static void MapEntity(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, bool isNewEntity, 
                                     List<ShipmentPackagePM> myPackagesList, IShipmentsContext objectContext, List<FieldChange> fieldChanges)
        {
            IWebFreightContext webFrieghtcontext = WebFreightContext.GetContext(entityPM.Tenant);
            TraceEventRepository traceEventRepository = new TraceEventRepository(webFrieghtcontext);
            TraceEventQuery traceEventQuery = new TraceEventQuery(traceEventRepository);

            FixStringNullFields(entityPM);

            if (isNewEntity)
            {
                FieldChange.Add(entityPoco.CreateDateTime, entityPM.CreateDateTime, nameof(entityPM.CreateDateTime), fieldChanges);
                entityPoco.CreateDateTime = entityPM.CreateDateTime;

                FieldChange.Add(entityPoco.CreatedByUserId, entityPM.CreatedByUserId, nameof(entityPM.CreatedByUserId), fieldChanges);
                entityPoco.CreatedByUserId = entityPM.CreatedByUserId;

                FieldChange.Add(entityPoco.ShipmentTypeId, entityPM.ShipmentTypeId, nameof(entityPM.ShipmentTypeId), fieldChanges);
                entityPoco.ShipmentTypeId = entityPM.ShipmentTypeId;

                FieldChange.Add(entityPoco.Tenant, entityPM.Tenant, nameof(entityPM.Tenant), fieldChanges);
                entityPoco.Tenant = entityPM.Tenant;

                FieldChange.Add(entityPoco.TransportModeId, entityPM.TransportModeId, nameof(entityPM.TransportModeId), fieldChanges);
                entityPoco.TransportModeId = entityPM.TransportModeId;

                FieldChange.Add(entityPoco.DirectionId, entityPM.DirectionId, nameof(entityPM.DirectionId), fieldChanges);
                entityPoco.DirectionId = entityPM.DirectionId;

                FieldChange.Add(entityPoco.ShipmentNumber, entityPM.ShipmentNumber, nameof(entityPM.ShipmentNumber), fieldChanges);
                entityPoco.ShipmentNumber = entityPM.ShipmentNumber;

                if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C")
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

                FieldChange.Add(entityPoco.MasterShipmentDataId, entityPM.MasterShipmentDataId, nameof(entityPM.MasterShipmentDataId), fieldChanges);
                entityPoco.MasterShipmentDataId = entityPM.MasterShipmentDataId;

                FieldChange.Add(entityPoco.SecurityKey, entityPM.SecurityKey, nameof(entityPM.SecurityKey), fieldChanges);
                entityPoco.SecurityKey = entityPM.SecurityKey;

                FieldChange.Add(entityPoco.OriginShipmentId, entityPM.OriginShipmentId, nameof(entityPM.OriginShipmentId), fieldChanges);
                entityPoco.OriginShipmentId = entityPM.OriginShipmentId;
            }
            else
            {
                if (entityPM.ConvertToCustomFile)
                {
                    FieldChange.Add(entityPoco.DirectionId, entityPM.DirectionId, nameof(entityPM.DirectionId), fieldChanges);
                    entityPoco.DirectionId = entityPM.DirectionId;
                }

                if (entityPM.ShipmentDirectionConverted)
                {
                    FieldChange.Add(entityPoco.DirectionId, entityPM.DirectionId, nameof(entityPM.DirectionId), fieldChanges);
                    entityPoco.DirectionId = entityPM.DirectionId;
                    entityPM.ShipmentDirectionConverted = false;
                }

                if (entityPM.ShipmentConvertedNewNumber)
                {
                    FieldChange.Add(entityPoco.ShipmentNumber, entityPM.ShipmentNumber, nameof(entityPM.ShipmentNumber), fieldChanges);
                    entityPoco.ShipmentNumber = entityPM.ShipmentNumber;

                    if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C")
                    {
                        entityPM.ComputedShipmentNumber = entityPM.ShipmentNumber;
                    }                    
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
                FieldChange.Add(entityPoco.ShipmentTypeId, entityPM.ShipmentTypeId, nameof(entityPM.ShipmentTypeId), fieldChanges);
                entityPoco.ShipmentTypeId = entityPM.ShipmentTypeId;
            }

            if (entityPM.ConvertShipmentToLCL || entityPM.ConvertShipmentToFCL || entityPM.ConvertShipmentToLTL || entityPM.ConvertShipmentToFTL)
            {
                FieldChange.Add(entityPoco.ShipmentTypeId, entityPM.ShipmentTypeId, nameof(entityPM.ShipmentTypeId), fieldChanges);
                entityPoco.ShipmentTypeId = entityPM.ShipmentTypeId;

                entityPM.ConvertShipmentToLCL = false;
                entityPM.ConvertShipmentToFCL = false;
                entityPM.ConvertShipmentToLTL = false;
                entityPM.ConvertShipmentToFTL = false;
            }

            entityPoco.NoFreightFile = entityPM.NoFreightFile;

            if (entityPM.ShipmentLevelCode != "H")
            {
                CheckNextLeg(ref entityPM);
                CheckNextETAAndETD(ref entityPM);
                FieldChange.Add(entityPoco.NextETA, entityPM.NextETA, nameof(entityPM.NextETA), fieldChanges);
                entityPoco.NextETA = entityPM.NextETA;

                FieldChange.Add(entityPoco.NextETD, entityPM.NextETD, nameof(entityPM.NextETD), fieldChanges);
                entityPoco.NextETD = entityPM.NextETD;

                FieldChange.Add(entityPoco.NextLegCode, entityPM.NextLegCode, nameof(entityPM.NextLegCode), fieldChanges);
                entityPoco.NextLegCode = entityPM.NextLegCode;
            }

            FieldChange.Add(entityPoco.WarehouseStorageFreeDays, entityPM.WarehouseStorageFreeDays, nameof(entityPM.WarehouseStorageFreeDays), fieldChanges);
            entityPoco.WarehouseStorageFreeDays = entityPM.WarehouseStorageFreeDays;

            FieldChange.Add(entityPoco.FreelancerId, entityPM.FreelancerId, nameof(entityPM.FreelancerId), fieldChanges);
            entityPoco.FreelancerId = entityPM.FreelancerId;

            FieldChange.Add(entityPoco.FreelancerAddressId, entityPM.FreelancerAddressId, nameof(entityPM.FreelancerAddressId), fieldChanges);
            entityPoco.FreelancerAddressId = entityPM.FreelancerAddressId;

            FieldChange.Add(entityPoco.FreelancerContactId, entityPM.FreelancerContactId, nameof(entityPM.FreelancerContactId), fieldChanges);
            entityPoco.FreelancerContactId = entityPM.FreelancerContactId;

            FieldChange.Add(entityPoco.CustomFileId, entityPM.CustomFileId, nameof(entityPM.CustomFileId), fieldChanges);
            entityPoco.CustomFileId = entityPM.CustomFileId;

            FieldChange.Add(entityPoco.CustomFileNumber, entityPM.CustomFileNumber, nameof(entityPM.CustomFileNumber), fieldChanges);
            entityPoco.CustomFileNumber = entityPM.CustomFileNumber;

            FieldChange.Add(entityPoco.LastStatusLogDate, entityPM.LastStatusLogDate, nameof(entityPM.LastStatusLogDate), fieldChanges);
            entityPoco.LastStatusLogDate = entityPM.LastStatusLogDate;

            FieldChange.Add(entityPoco.ExceptionResolvedDescription, entityPM.ExceptionResolvedDescription, nameof(entityPM.ExceptionResolvedDescription), fieldChanges);
            entityPoco.ExceptionResolvedDescription = entityPM.ExceptionResolvedDescription;

            FieldChange.Add(entityPoco.LastExceptionDescription, entityPM.LastExceptionDescription, nameof(entityPM.LastExceptionDescription), fieldChanges);
            entityPoco.LastExceptionDescription = entityPM.LastExceptionDescription;

            FieldChange.Add(entityPoco.IsManifestSentToAgent, entityPM.IsManifestSentToAgent, nameof(entityPM.IsManifestSentToAgent), fieldChanges);
            entityPoco.IsManifestSentToAgent = entityPM.IsManifestSentToAgent;

            FieldChange.Add(entityPoco.AgentSharedManifestRef, entityPM.AgentSharedManifestRef, nameof(entityPM.AgentSharedManifestRef), fieldChanges);
            entityPoco.AgentSharedManifestRef = entityPM.AgentSharedManifestRef;

            FieldChange.Add(entityPoco.ManifestLastSharingDate, entityPM.ManifestLastSharingDate, nameof(entityPM.ManifestLastSharingDate), fieldChanges);
            entityPoco.ManifestLastSharingDate = entityPM.ManifestLastSharingDate;

            FieldChange.Add(entityPoco.CountryForStatisticsId, entityPM.CountryForStatisticsId, nameof(entityPM.CountryForStatisticsId), fieldChanges);
            entityPoco.CountryForStatisticsId = entityPM.CountryForStatisticsId;

            FieldChange.Add(entityPoco.CreatedByPartner, entityPM.CreatedByPartner, nameof(entityPM.CreatedByPartner), fieldChanges);
            entityPoco.CreatedByPartner = entityPM.CreatedByPartner;
            
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

            FieldChange.Add(entityPoco.CustomsDeclarationNumber, entityPM.CustomsDeclarationNumber, nameof(entityPM.CustomsDeclarationNumber), fieldChanges);
            entityPoco.CustomsDeclarationNumber = entityPM.CustomsDeclarationNumber;

            FieldChange.Add(entityPoco.CustomConnectToShipment, entityPM.CustomConnectToShipment, nameof(entityPM.CustomConnectToShipment), fieldChanges);
            entityPoco.CustomConnectToShipment = entityPM.CustomConnectToShipment;

            MapConcurrencyFields(entityPM, entityPoco, entityMasterData, myPackagesList.Count, isNewEntity, fieldChanges);
            MapMasterData(entityPM, entityPoco, entityMasterData, isNewEntity, fieldChanges);
            MapRoutings(entityPM, entityPoco, entityMasterData, isNewEntity, fieldChanges);
            MapPartners(entityPM, entityPoco, isNewEntity, fieldChanges);
            MapAWBFields(entityPM, entityPoco, isNewEntity, fieldChanges);
            MapTotalsFields(entityPM, entityPoco, isNewEntity, fieldChanges);
            MapTotalsOfPackages(entityPM, entityPoco);
            MapWeightsFields(entityPM, entityPoco, isNewEntity, fieldChanges);
            MapXSDMessagesFields(entityPM, entityPoco, entityMasterData, isNewEntity, fieldChanges);

            entityPM.ShipmentConvertedNewNumber = false;

            if (entityPM.ShipmentLevelCode == "H")
            {
                FieldChange.Add(entityPoco.MasterShipmentDataId, entityPM.MasterShipmentDataId, nameof(entityPM.MasterShipmentDataId), fieldChanges);
                entityPoco.MasterShipmentDataId = entityPM.MasterShipmentDataId;

                FieldChange.Add(entityPoco.FromPortId, entityPM.FromPortId, nameof(entityPM.FromPortId), fieldChanges);
                entityPoco.FromPortId = entityPM.FromPortId;

                FieldChange.Add(entityPoco.ToPortId, entityPM.ToPortId, nameof(entityPM.ToPortId), fieldChanges);
                entityPoco.ToPortId = entityPM.ToPortId;
            }

            else
            {
                entityPM.FromPortId = entityMasterData.MainCarriageFromPortId;
                entityPM.ToPortId = entityMasterData.MainCarriageFinalDestinationPortId;

                FieldChange.Add(entityPoco.FromPortId, entityPM.FromPortId, nameof(entityPM.FromPortId), fieldChanges);
                entityPoco.FromPortId = entityPM.FromPortId;

                FieldChange.Add(entityPoco.ToPortId, entityPM.ToPortId, nameof(entityPM.ToPortId), fieldChanges);
                entityPoco.ToPortId = entityPM.ToPortId;
            }

            if (entityPM.IsStatusChange)
            {
                MapShipmentStatus(entityPM, entityPoco, entityMasterData, fieldChanges);
            }

            if (entityPM.IsOperationalStatusChange)
            {
                MapShipmentOperationalStatus(entityPM, entityPoco, entityMasterData, fieldChanges);
            }

            TenantRepository tenantRepository = new TenantRepository(entityPM.Tenant);
            Tenant currentTenant = tenantRepository.GetSingleTenant(entityPM.Tenant);

            LogBoxTenantSettingRepository LBtenantRepository = new LogBoxTenantSettingRepository(entityPM.Tenant);
            LogBoxTenantSetting LBcurrentTenant = LBtenantRepository.GetSingleLBTenant(entityPM.Tenant);

            if (isNewEntity && entityPM.IsHybrid)
            {
                FieldChange.Add(entityPoco.StatusId, entityPM.StatusId, nameof(entityPM.StatusId), fieldChanges);
                entityPoco.StatusId = entityPM.StatusId;
                
                FieldChange.Add(entityPoco.StatusDate, entityPM.StatusDate, nameof(entityPM.StatusDate), fieldChanges);
                entityPoco.StatusDate = entityPM.StatusDate;
                
                FieldChange.Add(entityPoco.StatusLocation, entityPM.StatusLocation, nameof(entityPM.StatusLocation), fieldChanges);
                entityPoco.StatusLocation = entityPM.StatusLocation;
                
                FieldChange.Add(entityPoco.LastStatusLogDate, entityPM.LastStatusLogDate, nameof(entityPM.LastStatusLogDate), fieldChanges);
                entityPoco.LastStatusLogDate = entityPM.LastStatusLogDate;

                if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C")
                {
                    FieldChange.Add(entityMasterData.StatusId, entityPM.StatusId, nameof(entityPM.StatusId), fieldChanges);
                    entityMasterData.StatusId = entityPM.StatusId;
                    
                    FieldChange.Add(entityMasterData.StatusDate, entityPM.StatusDate, nameof(entityPM.StatusDate), fieldChanges);
                    entityMasterData.StatusDate = entityPM.StatusDate;
                    
                    FieldChange.Add(entityMasterData.StatusLocation, entityPM.StatusLocation, nameof(entityPM.StatusLocation), fieldChanges);
                    entityMasterData.StatusLocation = entityPM.StatusLocation;
                    
                    FieldChange.Add(entityMasterData.PartialStatusAmount, entityPM.PartialStatusAmount, nameof(entityPM.PartialStatusAmount), fieldChanges);
                    entityMasterData.PartialStatusAmount = entityPM.PartialStatusAmount;
                }
            }

            if (LBcurrentTenant.IsDocumentsArchive)
            {
                if (!isNewEntity)
                {
                    FieldChange.Add(entityPoco.TransportModeId, entityPM.TransportModeId, nameof(entityPM.TransportModeId), fieldChanges);
                    entityPoco.TransportModeId = entityPM.TransportModeId;
                }

                FieldChange.Add(entityPoco.ShipmentTypeId, entityPM.ShipmentTypeId, nameof(entityPM.ShipmentTypeId), fieldChanges);
                entityPoco.ShipmentTypeId = entityPM.ShipmentTypeId;
                
                FieldChange.Add(entityPoco.StatusId, entityPM.StatusId, nameof(entityPM.StatusId), fieldChanges);
                entityPoco.StatusId = entityPM.StatusId;
                
                FieldChange.Add(entityPoco.StatusDate, entityPM.StatusDate, nameof(entityPM.StatusDate), fieldChanges);
                entityPoco.StatusDate = entityPM.StatusDate;
                
                FieldChange.Add(entityPoco.HasException, entityPM.HasException, nameof(entityPM.HasException), fieldChanges);
                entityPoco.HasException = entityPM.HasException;
                
                FieldChange.Add(entityPoco.ExceptionDate, entityPM.ExceptionDate, nameof(entityPM.ExceptionDate), fieldChanges);
                entityPoco.ExceptionDate = entityPM.ExceptionDate;
                
                FieldChange.Add(entityPoco.ExceptionDescription, entityPM.ExceptionDescription, nameof(entityPM.ExceptionDescription), fieldChanges);
                entityPoco.ExceptionDescription = entityPM.ExceptionDescription;
            }
            else if (entityPM.IsUpdateEntityException)
            {
                FieldChange.Add(entityPoco.HasException, entityPM.HasException, nameof(entityPM.HasException), fieldChanges);
                entityPoco.HasException = entityPM.HasException;
                
                FieldChange.Add(entityPoco.ExceptionDate, entityPM.ExceptionDate, nameof(entityPM.ExceptionDate), fieldChanges);
                entityPoco.ExceptionDate = entityPM.ExceptionDate;
                
                FieldChange.Add(entityPoco.ExceptionDescription, entityPM.ExceptionDescription, nameof(entityPM.ExceptionDescription), fieldChanges);
                entityPoco.ExceptionDescription = entityPM.ExceptionDescription;
                
                entityPM.IsUpdateEntityException = false;
            }

            if (!entityPM.IsHybrid || entityPM.DontAddToImportersQueue)
            {
                FieldChange.Add(entityPoco.ForwarderShipmentNumber, entityPM.ForwarderShipmentNumber, nameof(entityPM.ForwarderShipmentNumber), fieldChanges);
                entityPoco.ForwarderShipmentNumber = entityPM.ForwarderShipmentNumber;
                
                FieldChange.Add(entityPoco.CustomerShipmentNumber, entityPM.CustomerShipmentNumber, nameof(entityPM.CustomerShipmentNumber), fieldChanges);
                entityPoco.CustomerShipmentNumber = entityPM.CustomerShipmentNumber;
                
                FieldChange.Add(entityPoco.CustomerTenantNumber, entityPM.CustomerTenantNumber, nameof(entityPM.CustomerTenantNumber), fieldChanges);
                entityPoco.CustomerTenantNumber = entityPM.CustomerTenantNumber;
            }

            FieldChange.Add(entityPoco.OrderGrossWeightEdited, entityPM.OrderGrossWeightEdited, nameof(entityPM.OrderGrossWeightEdited), fieldChanges);
            entityPoco.OrderGrossWeightEdited = entityPM.OrderGrossWeightEdited;
            
            FieldChange.Add(entityPoco.OrderChargeableWeightEdited, entityPM.OrderChargeableWeightEdited, nameof(entityPM.OrderChargeableWeightEdited), fieldChanges);
            entityPoco.OrderChargeableWeightEdited = entityPM.OrderChargeableWeightEdited;
            
            FieldChange.Add(entityPoco.CASSCode, entityPM.CASSCode, nameof(entityPM.CASSCode), fieldChanges);
            entityPoco.CASSCode = entityPM.CASSCode;
            
            FieldChange.Add(entityPoco.SLAC, entityPM.SLAC, nameof(entityPM.SLAC), fieldChanges);
            entityPoco.SLAC = entityPM.SLAC;
            
            FieldChange.Add(entityPoco.OrderGrossWeight, entityPM.OrderGrossWeight, nameof(entityPM.OrderGrossWeight), fieldChanges);
            entityPoco.OrderGrossWeight = entityPM.OrderGrossWeight;
            
            FieldChange.Add(entityPoco.BookingVolume, entityPM.BookingVolume, nameof(entityPM.BookingVolume), fieldChanges);
            entityPoco.BookingVolume = entityPM.BookingVolume;
            
            FieldChange.Add(entityPoco.OrderVolumetricWeight, entityPM.OrderVolumetricWeight, nameof(entityPM.OrderVolumetricWeight), fieldChanges);
            entityPoco.OrderVolumetricWeight = entityPM.OrderVolumetricWeight;
            
            FieldChange.Add(entityPoco.OrderChargeableWeight, entityPM.OrderChargeableWeight, nameof(entityPM.OrderChargeableWeight), fieldChanges);
            entityPoco.OrderChargeableWeight = entityPM.OrderChargeableWeight;
            
            FieldChange.Add(entityPoco.BookingNumberOfPackages, entityPM.BookingNumberOfPackages, nameof(entityPM.BookingNumberOfPackages), fieldChanges);
            entityPoco.BookingNumberOfPackages = entityPM.BookingNumberOfPackages;
            
            FieldChange.Add(entityPoco.OrderIsDangerouseGoods, entityPM.OrderIsDangerouseGoods, nameof(entityPM.OrderIsDangerouseGoods), fieldChanges);
            entityPoco.OrderIsDangerouseGoods = entityPM.OrderIsDangerouseGoods;
            
            FieldChange.Add(entityPoco.ShipmentLevelCode, entityPM.ShipmentLevelCode, nameof(entityPM.ShipmentLevelCode), fieldChanges);
            entityPoco.ShipmentLevelCode = entityPM.ShipmentLevelCode;
            
            FieldChange.Add(entityPoco.IncotermId, entityPM.IncotermId, nameof(entityPM.IncotermId), fieldChanges);
            entityPoco.IncotermId = entityPM.IncotermId;
            
            FieldChange.Add(entityPoco.FreightPrepaidCollectId, entityPM.FreightPrepaidCollectId, nameof(entityPM.FreightPrepaidCollectId), fieldChanges);
            entityPoco.FreightPrepaidCollectId = entityPM.FreightPrepaidCollectId;
            
            FieldChange.Add(entityPoco.OtherPrepaidCollectId, entityPM.OtherPrepaidCollectId, nameof(entityPM.OtherPrepaidCollectId), fieldChanges);
            entityPoco.OtherPrepaidCollectId = entityPM.OtherPrepaidCollectId;
            
            FieldChange.Add(entityPoco.DepartmentId, entityPM.DepartmentId, nameof(entityPM.DepartmentId), fieldChanges);
            entityPoco.DepartmentId = entityPM.DepartmentId;
            
            FieldChange.Add(entityPoco.BranchId, entityPM.BranchId, nameof(entityPM.BranchId), fieldChanges);
            entityPoco.BranchId = entityPM.BranchId;
            
            FieldChange.Add(entityPoco.SalesmanUserId, entityPM.SalesmanUserId, nameof(entityPM.SalesmanUserId), fieldChanges);
            entityPoco.SalesmanUserId = entityPM.SalesmanUserId;
            
            FieldChange.Add(entityPoco.AccountManagerUserId, entityPM.AccountManagerUserId, nameof(entityPM.AccountManagerUserId), fieldChanges);
            entityPoco.AccountManagerUserId = entityPM.AccountManagerUserId;
            
            FieldChange.Add(entityPoco.IsCancelled, entityPM.IsCancelled, nameof(entityPM.IsCancelled), fieldChanges);
            entityPoco.IsCancelled = entityPM.IsCancelled;

            FieldChange.Add(entityPoco.IsOperationalClosed, entityPM.IsOperationalClosed, nameof(entityPM.IsOperationalClosed), fieldChanges);
            entityPoco.IsOperationalClosed = entityPM.IsOperationalClosed;
            
            FieldChange.Add(entityPoco.IsAccountingClosed, entityPM.IsAccountingClosed, nameof(entityPM.IsAccountingClosed), fieldChanges);
            entityPoco.IsAccountingClosed = entityPM.IsAccountingClosed;
            
            FieldChange.Add(entityPoco.BasketId, entityPM.BasketId, nameof(entityPM.BasketId), fieldChanges);
            entityPoco.BasketId = entityPM.BasketId;
            
            FieldChange.Add(entityPoco.CurrentUserId, entityPM.CurrentUserId, nameof(entityPM.CurrentUserId), fieldChanges);
            entityPoco.CurrentUserId = entityPM.CurrentUserId;
            
            FieldChange.Add(entityPoco.DescriptionOfGoods, entityPM.DescriptionOfGoods, nameof(entityPM.DescriptionOfGoods), fieldChanges);
            entityPoco.DescriptionOfGoods = entityPM.DescriptionOfGoods;
            
            FieldChange.Add(entityPoco.Field1, entityPM.Field1?.Value, nameof(entityPM.Field1), fieldChanges);
            entityPoco.Field1 = entityPM.Field1 != null ? entityPM.Field1.Value : null;
            
            FieldChange.Add(entityPoco.Field2, entityPM.Field2?.Value, nameof(entityPM.Field2), fieldChanges);
            entityPoco.Field2 = entityPM.Field2 != null ? entityPM.Field2.Value : null;
            
            FieldChange.Add(entityPoco.Field3, entityPM.Field3?.Value, nameof(entityPM.Field3), fieldChanges);
            entityPoco.Field3 = entityPM.Field3 != null ? entityPM.Field3.Value : null;
            
            FieldChange.Add(entityPoco.Field4, entityPM.Field4?.Value, nameof(entityPM.Field4), fieldChanges);
            entityPoco.Field4 = entityPM.Field4 != null ? entityPM.Field4.Value : null;
            
            FieldChange.Add(entityPoco.Field5, entityPM.Field5?.Value, nameof(entityPM.Field5), fieldChanges);
            entityPoco.Field5 = entityPM.Field5 != null ? entityPM.Field5.Value : null;
            
            FieldChange.Add(entityPoco.Field6, entityPM.Field6?.Value, nameof(entityPM.Field6), fieldChanges);
            entityPoco.Field6 = entityPM.Field6 != null ? entityPM.Field6.Value : null;
            
            FieldChange.Add(entityPoco.Field7, entityPM.Field7?.Value, nameof(entityPM.Field7), fieldChanges);
            entityPoco.Field7 = entityPM.Field7 != null ? entityPM.Field7.Value : null;
            
            FieldChange.Add(entityPoco.Field8, entityPM.Field8?.Value, nameof(entityPM.Field8), fieldChanges);
            entityPoco.Field8 = entityPM.Field8 != null ? entityPM.Field8.Value : null;
            
            FieldChange.Add(entityPoco.Field9, entityPM.Field9?.Value, nameof(entityPM.Field9), fieldChanges);
            entityPoco.Field9 = entityPM.Field9 != null ? entityPM.Field9.Value : null;
            
            FieldChange.Add(entityPoco.Field10, entityPM.Field10?.Value, nameof(entityPM.Field10), fieldChanges);
            entityPoco.Field10 = entityPM.Field10 != null ? entityPM.Field10.Value : null;
            
            FieldChange.Add(entityPoco.Field11, entityPM.Field11?.Value, nameof(entityPM.Field11), fieldChanges);
            entityPoco.Field11 = entityPM.Field11 != null ? entityPM.Field11.Value : null;
            
            FieldChange.Add(entityPoco.Field12, entityPM.Field12?.Value, nameof(entityPM.Field12), fieldChanges);
            entityPoco.Field12 = entityPM.Field12 != null ? entityPM.Field12.Value : null;
            
            FieldChange.Add(entityPoco.Field13, entityPM.Field13?.Value, nameof(entityPM.Field13), fieldChanges);
            entityPoco.Field13 = entityPM.Field13 != null ? entityPM.Field13.Value : null;
            
            FieldChange.Add(entityPoco.Field14, entityPM.Field14?.Value, nameof(entityPM.Field14), fieldChanges);
            entityPoco.Field14 = entityPM.Field14 != null ? entityPM.Field14.Value : null;
            
            FieldChange.Add(entityPoco.Field15, entityPM.Field15?.Value, nameof(entityPM.Field15), fieldChanges);
            entityPoco.Field15 = entityPM.Field15 != null ? entityPM.Field15.Value : null;
            
            FieldChange.Add(entityPoco.Field16, entityPM.Field16?.Value, nameof(entityPM.Field16), fieldChanges);
            entityPoco.Field16 = entityPM.Field16 != null ? entityPM.Field16.Value : null;
            
            FieldChange.Add(entityPoco.Field17, entityPM.Field17?.Value, nameof(entityPM.Field17), fieldChanges);
            entityPoco.Field17 = entityPM.Field17 != null ? entityPM.Field17.Value : null;
            
            FieldChange.Add(entityPoco.Field18, entityPM.Field18?.Value, nameof(entityPM.Field18), fieldChanges);
            entityPoco.Field18 = entityPM.Field18 != null ? entityPM.Field18.Value : null;
            
            FieldChange.Add(entityPoco.Field19, entityPM.Field19?.Value, nameof(entityPM.Field19), fieldChanges);
            entityPoco.Field19 = entityPM.Field19 != null ? entityPM.Field19.Value : null;
            
            FieldChange.Add(entityPoco.Field20, entityPM.Field20?.Value, nameof(entityPM.Field20), fieldChanges);
            entityPoco.Field20 = entityPM.Field20 != null ? entityPM.Field20.Value : null;
            
            FieldChange.Add(entityPoco.Field21, entityPM.Field21?.Value, nameof(entityPM.Field21), fieldChanges);
            entityPoco.Field21 = entityPM.Field21 != null ? entityPM.Field21.Value : null;
            
            FieldChange.Add(entityPoco.Field22, entityPM.Field22?.Value, nameof(entityPM.Field22), fieldChanges);
            entityPoco.Field22 = entityPM.Field22 != null ? entityPM.Field22.Value : null;
            
            FieldChange.Add(entityPoco.Field23, entityPM.Field23?.Value, nameof(entityPM.Field23), fieldChanges);
            entityPoco.Field23 = entityPM.Field23 != null ? entityPM.Field23.Value : null;
            
            FieldChange.Add(entityPoco.Field24, entityPM.Field24?.Value, nameof(entityPM.Field24), fieldChanges);
            entityPoco.Field24 = entityPM.Field24 != null ? entityPM.Field24.Value : null;
            
            FieldChange.Add(entityPoco.Field25, entityPM.Field25?.Value, nameof(entityPM.Field25), fieldChanges);
            entityPoco.Field25 = entityPM.Field25 != null ? entityPM.Field25.Value : null;
            
            FieldChange.Add(entityPoco.Field26, entityPM.Field26?.Value, nameof(entityPM.Field26), fieldChanges);
            entityPoco.Field26 = entityPM.Field26 != null ? entityPM.Field26.Value : null;
            
            FieldChange.Add(entityPoco.Field27, entityPM.Field27?.Value, nameof(entityPM.Field27), fieldChanges);
            entityPoco.Field27 = entityPM.Field27 != null ? entityPM.Field27.Value : null;
            
            FieldChange.Add(entityPoco.Field28, entityPM.Field28?.Value, nameof(entityPM.Field28), fieldChanges);
            entityPoco.Field28 = entityPM.Field28 != null ? entityPM.Field28.Value : null;
            
            FieldChange.Add(entityPoco.Field29, entityPM.Field29?.Value, nameof(entityPM.Field29), fieldChanges);
            entityPoco.Field29 = entityPM.Field29 != null ? entityPM.Field29.Value : null;
            
            FieldChange.Add(entityPoco.Field30, entityPM.Field30?.Value, nameof(entityPM.Field30), fieldChanges);
            entityPoco.Field30 = entityPM.Field30 != null ? entityPM.Field30.Value : null;
            
            FieldChange.Add(entityPoco.Field31, entityPM.Field31?.Value, nameof(entityPM.Field31), fieldChanges);
            entityPoco.Field31 = entityPM.Field31 != null ? entityPM.Field31.Value : null;
            
            FieldChange.Add(entityPoco.Field32, entityPM.Field32?.Value, nameof(entityPM.Field32), fieldChanges);
            entityPoco.Field32 = entityPM.Field32 != null ? entityPM.Field32.Value : null;
            
            FieldChange.Add(entityPoco.Field33, entityPM.Field33?.Value, nameof(entityPM.Field33), fieldChanges);
            entityPoco.Field33 = entityPM.Field33 != null ? entityPM.Field33.Value : null;
            
            FieldChange.Add(entityPoco.Field34, entityPM.Field34?.Value, nameof(entityPM.Field34), fieldChanges);
            entityPoco.Field34 = entityPM.Field34 != null ? entityPM.Field34.Value : null;
            
            FieldChange.Add(entityPoco.Field35, entityPM.Field35?.Value, nameof(entityPM.Field35), fieldChanges);
            entityPoco.Field35 = entityPM.Field35 != null ? entityPM.Field35.Value : null;
            
            FieldChange.Add(entityPoco.Field36, entityPM.Field36?.Value, nameof(entityPM.Field36), fieldChanges);
            entityPoco.Field36 = entityPM.Field36 != null ? entityPM.Field36.Value : null;
            
            FieldChange.Add(entityPoco.Field37, entityPM.Field37?.Value, nameof(entityPM.Field37), fieldChanges);
            entityPoco.Field37 = entityPM.Field37 != null ? entityPM.Field37.Value : null;
            
            FieldChange.Add(entityPoco.Field38, entityPM.Field38?.Value, nameof(entityPM.Field38), fieldChanges);
            entityPoco.Field38 = entityPM.Field38 != null ? entityPM.Field38.Value : null;
            
            FieldChange.Add(entityPoco.Field39, entityPM.Field39?.Value, nameof(entityPM.Field39), fieldChanges);
            entityPoco.Field39 = entityPM.Field39 != null ? entityPM.Field39.Value : null;
            
            FieldChange.Add(entityPoco.Field40, entityPM.Field40?.Value, nameof(entityPM.Field40), fieldChanges);
            entityPoco.Field40 = entityPM.Field40 != null ? entityPM.Field40.Value : null;
            
            FieldChange.Add(entityPoco.Field41, entityPM.Field41?.Value, nameof(entityPM.Field41), fieldChanges);
            entityPoco.Field41 = entityPM.Field41 != null ? entityPM.Field41.Value : null;
            
            FieldChange.Add(entityPoco.Field42, entityPM.Field42?.Value, nameof(entityPM.Field42), fieldChanges);
            entityPoco.Field42 = entityPM.Field42 != null ? entityPM.Field42.Value : null;
            
            FieldChange.Add(entityPoco.Field43, entityPM.Field43?.Value, nameof(entityPM.Field43), fieldChanges);
            entityPoco.Field43 = entityPM.Field43 != null ? entityPM.Field43.Value : null;
            
            FieldChange.Add(entityPoco.Field44, entityPM.Field44?.Value, nameof(entityPM.Field44), fieldChanges);
            entityPoco.Field44 = entityPM.Field44 != null ? entityPM.Field44.Value : null;
            
            FieldChange.Add(entityPoco.Field45, entityPM.Field45?.Value, nameof(entityPM.Field45), fieldChanges);
            entityPoco.Field45 = entityPM.Field45 != null ? entityPM.Field45.Value : null;
            
            FieldChange.Add(entityPoco.Field46, entityPM.Field46?.Value, nameof(entityPM.Field46), fieldChanges);
            entityPoco.Field46 = entityPM.Field46 != null ? entityPM.Field46.Value : null;
            
            FieldChange.Add(entityPoco.Field47, entityPM.Field47?.Value, nameof(entityPM.Field47), fieldChanges);
            entityPoco.Field47 = entityPM.Field47 != null ? entityPM.Field47.Value : null;
            
            FieldChange.Add(entityPoco.Field48, entityPM.Field48?.Value, nameof(entityPM.Field48), fieldChanges);
            entityPoco.Field48 = entityPM.Field48 != null ? entityPM.Field48.Value : null;
            
            FieldChange.Add(entityPoco.Field49, entityPM.Field49?.Value, nameof(entityPM.Field49), fieldChanges);
            entityPoco.Field49 = entityPM.Field49 != null ? entityPM.Field49.Value : null;
            
            FieldChange.Add(entityPoco.Field50, entityPM.Field50?.Value, nameof(entityPM.Field50), fieldChanges);
            entityPoco.Field50 = entityPM.Field50 != null ? entityPM.Field50.Value : null;
            
            FieldChange.Add(entityPoco.Field51, entityPM.Field51?.Value, nameof(entityPM.Field51), fieldChanges);
            entityPoco.Field51 = entityPM.Field51 != null ? entityPM.Field51.Value : null;
            
            FieldChange.Add(entityPoco.Field52, entityPM.Field52?.Value, nameof(entityPM.Field52), fieldChanges);
            entityPoco.Field52 = entityPM.Field52 != null ? entityPM.Field52.Value : null;
            
            FieldChange.Add(entityPoco.Field53, entityPM.Field53?.Value, nameof(entityPM.Field53), fieldChanges);
            entityPoco.Field53 = entityPM.Field53 != null ? entityPM.Field53.Value : null;
            
            FieldChange.Add(entityPoco.Field54, entityPM.Field54?.Value, nameof(entityPM.Field54), fieldChanges);
            entityPoco.Field54 = entityPM.Field54 != null ? entityPM.Field54.Value : null;
            
            FieldChange.Add(entityPoco.Field55, entityPM.Field55?.Value, nameof(entityPM.Field55), fieldChanges);
            entityPoco.Field55 = entityPM.Field55 != null ? entityPM.Field55.Value : null;
            
            FieldChange.Add(entityPoco.Field56, entityPM.Field56?.Value, nameof(entityPM.Field56), fieldChanges);
            entityPoco.Field56 = entityPM.Field56 != null ? entityPM.Field56.Value : null;
            
            FieldChange.Add(entityPoco.Field57, entityPM.Field57?.Value, nameof(entityPM.Field57), fieldChanges);
            entityPoco.Field57 = entityPM.Field57 != null ? entityPM.Field57.Value : null;
            
            FieldChange.Add(entityPoco.Field58, entityPM.Field58?.Value, nameof(entityPM.Field58), fieldChanges);
            entityPoco.Field58 = entityPM.Field58 != null ? entityPM.Field58.Value : null;
            
            FieldChange.Add(entityPoco.Field59, entityPM.Field59?.Value, nameof(entityPM.Field59), fieldChanges);
            entityPoco.Field59 = entityPM.Field59 != null ? entityPM.Field59.Value : null;
            
            FieldChange.Add(entityPoco.Field60, entityPM.Field60?.Value, nameof(entityPM.Field60), fieldChanges);
            entityPoco.Field60 = entityPM.Field60 != null ? entityPM.Field60.Value : null;
            
            FieldChange.Add(entityPoco.Field61, entityPM.Field61?.Value, nameof(entityPM.Field61), fieldChanges);
            entityPoco.Field61 = entityPM.Field61 != null ? entityPM.Field61.Value : null;
            
            FieldChange.Add(entityPoco.Field62, entityPM.Field62?.Value, nameof(entityPM.Field62), fieldChanges);
            entityPoco.Field62 = entityPM.Field62 != null ? entityPM.Field62.Value : null;
            
            FieldChange.Add(entityPoco.Field63, entityPM.Field63?.Value, nameof(entityPM.Field63), fieldChanges);
            entityPoco.Field63 = entityPM.Field63 != null ? entityPM.Field63.Value : null;
            
            FieldChange.Add(entityPoco.Field64, entityPM.Field64?.Value, nameof(entityPM.Field64), fieldChanges);
            entityPoco.Field64 = entityPM.Field64 != null ? entityPM.Field64.Value : null;
            
            FieldChange.Add(entityPoco.Field65, entityPM.Field65?.Value, nameof(entityPM.Field65), fieldChanges);
            entityPoco.Field65 = entityPM.Field65 != null ? entityPM.Field65.Value : null;
            
            FieldChange.Add(entityPoco.Field66, entityPM.Field66?.Value, nameof(entityPM.Field66), fieldChanges);
            entityPoco.Field66 = entityPM.Field66 != null ? entityPM.Field66.Value : null;
            
            FieldChange.Add(entityPoco.Field67, entityPM.Field67?.Value, nameof(entityPM.Field67), fieldChanges);
            entityPoco.Field67 = entityPM.Field67 != null ? entityPM.Field67.Value : null;
            
            FieldChange.Add(entityPoco.Field68, entityPM.Field68?.Value, nameof(entityPM.Field68), fieldChanges);
            entityPoco.Field68 = entityPM.Field68 != null ? entityPM.Field68.Value : null;
            
            FieldChange.Add(entityPoco.Field69, entityPM.Field69?.Value, nameof(entityPM.Field69), fieldChanges);
            entityPoco.Field69 = entityPM.Field69 != null ? entityPM.Field69.Value : null;
            
            FieldChange.Add(entityPoco.Field70, entityPM.Field70?.Value, nameof(entityPM.Field70), fieldChanges);
            entityPoco.Field70 = entityPM.Field70 != null ? entityPM.Field70.Value : null;
            
            FieldChange.Add(entityPoco.SpecialServicesTypeId, entityPM.SpecialServicesTypeId, nameof(entityPM.SpecialServicesTypeId), fieldChanges);
            entityPoco.SpecialServicesTypeId = entityPM.SpecialServicesTypeId;

            FieldChange.Add(entityPoco.Notes, entityPM.Notes, nameof(entityPM.Notes), fieldChanges);
            entityPoco.Notes = entityPM.Notes;
            
            FieldChange.Add(entityPoco.NotesSharedWithCustomer, entityPM.NotesSharedWithCustomer, nameof(entityPM.NotesSharedWithCustomer), fieldChanges);
            entityPoco.NotesSharedWithCustomer = entityPM.NotesSharedWithCustomer;
            
            FieldChange.Add(entityPoco.House, entityPM.House, nameof(entityPM.House), fieldChanges);
            entityPoco.House = entityPM.House;
            
            FieldChange.Add(entityPoco.HAWBDate, entityPM.HAWBDate, nameof(entityPM.HAWBDate), fieldChanges);
            entityPoco.HAWBDate = entityPM.HAWBDate;
            
            FieldChange.Add(entityPoco.MainHarmonize, entityPM.MainHarmonize, nameof(entityPM.MainHarmonize), fieldChanges);
            entityPoco.MainHarmonize = entityPM.MainHarmonize;
            
            FieldChange.Add(entityPoco.IsDangerous, entityPM.IsDangerous, nameof(entityPM.IsDangerous), fieldChanges);
            entityPoco.IsDangerous = entityPM.IsDangerous;
            
            FieldChange.Add(entityPoco.DangerousPackagingGroup, entityPM.DangerousPackagingGroup, nameof(entityPM.DangerousPackagingGroup), fieldChanges);
            entityPoco.DangerousPackagingGroup = entityPM.DangerousPackagingGroup;
            
            FieldChange.Add(entityPoco.DangerousUnNumber, entityPM.DangerousUnNumber, nameof(entityPM.DangerousUnNumber), fieldChanges);
            entityPoco.DangerousUnNumber = entityPM.DangerousUnNumber;
            
            FieldChange.Add(entityPoco.DangerousMaterialDescription, entityPM.DangerousMaterialDescription, nameof(entityPM.DangerousMaterialDescription), fieldChanges);
            entityPoco.DangerousMaterialDescription = entityPM.DangerousMaterialDescription;
            
            FieldChange.Add(entityPoco.DangerousIMDGCode, entityPM.DangerousIMDGCode, nameof(entityPM.DangerousIMDGCode), fieldChanges);
            entityPoco.DangerousIMDGCode = entityPM.DangerousIMDGCode;
            
            FieldChange.Add(entityPoco.DangerousFlashPoint, entityPM.DangerousFlashPoint, nameof(entityPM.DangerousFlashPoint), fieldChanges);
            entityPoco.DangerousFlashPoint = entityPM.DangerousFlashPoint;
            
            FieldChange.Add(entityPoco.DangerousClassNumber, entityPM.DangerousClassNumber, nameof(entityPM.DangerousClassNumber), fieldChanges);
            entityPoco.DangerousClassNumber = entityPM.DangerousClassNumber;
            
            FieldChange.Add(entityPoco.LTCWEdited, entityPM.LTCWEdited, nameof(entityPM.LTCWEdited), fieldChanges);
            entityPoco.LTCWEdited = entityPM.LTCWEdited;
            
            FieldChange.Add(entityPoco.ShipmentPickUpIndex, entityPM.ShipmentPickUpIndex, nameof(entityPM.ShipmentPickUpIndex), fieldChanges);
            entityPoco.ShipmentPickUpIndex = entityPM.ShipmentPickUpIndex;
            
            FieldChange.Add(entityPoco.ShipmentDeliveryIndex, entityPM.ShipmentDeliveryIndex, nameof(entityPM.ShipmentDeliveryIndex), fieldChanges);
            entityPoco.ShipmentDeliveryIndex = entityPM.ShipmentDeliveryIndex;
            
            FieldChange.Add(entityPoco.ShipmentContainerReturnIndex, entityPM.ShipmentContainerReturnIndex, nameof(entityPM.ShipmentContainerReturnIndex), fieldChanges);
            entityPoco.ShipmentContainerReturnIndex = entityPM.ShipmentContainerReturnIndex;
            
            FieldChange.Add(entityPoco.QuoteId, entityPM.QuoteId, nameof(entityPM.QuoteId), fieldChanges);
            entityPoco.QuoteId = entityPM.QuoteId;
            
            FieldChange.Add(entityPoco.QuoteNumber, entityPM.QuoteNumber, nameof(entityPM.QuoteNumber), fieldChanges);
            entityPoco.QuoteNumber = entityPM.QuoteNumber;
            
            FieldChange.Add(entityPoco.BookingId, entityPM.BookingId, nameof(entityPM.BookingId), fieldChanges);
            entityPoco.BookingId = entityPM.BookingId;
            
            FieldChange.Add(entityPoco.CancelledDate, entityPM.CancelledDate, nameof(entityPM.CancelledDate), fieldChanges);
            entityPoco.CancelledDate = entityPM.CancelledDate;
            
            FieldChange.Add(entityPoco.LastUpdateDate, entityPM.LastUpdateDate, nameof(entityPM.LastUpdateDate), fieldChanges);
            entityPoco.LastUpdateDate = entityPM.LastUpdateDate;
            
            FieldChange.Add(entityPoco.UpdatedByUserId, entityPM.UpdatedByUserId, nameof(entityPM.UpdatedByUserId), fieldChanges);
            entityPoco.UpdatedByUserId = entityPM.UpdatedByUserId;
            
            FieldChange.Add(entityPoco.AsAgreedFreight, entityPM.AsAgreedFreight, nameof(entityPM.AsAgreedFreight), fieldChanges);
            entityPoco.AsAgreedFreight = entityPM.AsAgreedFreight;
            
            FieldChange.Add(entityPoco.AsAgreedOtherCharges, entityPM.AsAgreedOtherCharges, nameof(entityPM.AsAgreedOtherCharges), fieldChanges);
            entityPoco.AsAgreedOtherCharges = entityPM.AsAgreedOtherCharges;
            
            FieldChange.Add(entityPoco.AccountNumber, entityPM.AccountNumber, nameof(entityPM.AccountNumber), fieldChanges);
            entityPoco.AccountNumber = entityPM.AccountNumber;
            
            FieldChange.Add(entityPoco.DeliveryOrder, entityPM.DeliveryOrder, nameof(entityPM.DeliveryOrder), fieldChanges);
            entityPoco.DeliveryOrder = entityPM.DeliveryOrder;
            
            FieldChange.Add(entityPoco.FreightLocationId, entityPM.FreightLocationId, nameof(entityPM.FreightLocationId), fieldChanges);
            entityPoco.FreightLocationId = entityPM.FreightLocationId;
            
            FieldChange.Add(entityPoco.TransportDocumentNumber, entityPM.TransportDocumentNumber, nameof(entityPM.TransportDocumentNumber), fieldChanges);
            entityPoco.TransportDocumentNumber = entityPM.TransportDocumentNumber;
            
            FieldChange.Add(entityPoco.IsMultipleCommodities, entityPM.IsMultipleCommodities, nameof(entityPM.IsMultipleCommodities), fieldChanges);
            entityPoco.IsMultipleCommodities = entityPM.IsMultipleCommodities;
            
            FieldChange.Add(entityPoco.NominatedHandlingPartyId, entityPM.NominatedHandlingPartyId, nameof(entityPM.NominatedHandlingPartyId), fieldChanges);
            entityPoco.NominatedHandlingPartyId = entityPM.NominatedHandlingPartyId;
            
            FieldChange.Add(entityPoco.OtherParticipantIdCode1, entityPM.OtherParticipantIdCode1, nameof(entityPM.OtherParticipantIdCode1), fieldChanges);
            entityPoco.OtherParticipantIdCode1 = entityPM.OtherParticipantIdCode1;
            
            FieldChange.Add(entityPoco.OtherParticipantIdCode2, entityPM.OtherParticipantIdCode2, nameof(entityPM.OtherParticipantIdCode2), fieldChanges);
            entityPoco.OtherParticipantIdCode2 = entityPM.OtherParticipantIdCode2;
            
            FieldChange.Add(entityPoco.OtherParticipantIdCode3, entityPM.OtherParticipantIdCode3, nameof(entityPM.OtherParticipantIdCode3), fieldChanges);
            entityPoco.OtherParticipantIdCode3 = entityPM.OtherParticipantIdCode3;
            
            FieldChange.Add(entityPoco.OtherParticipantInformationCode1, entityPM.OtherParticipantInformationCode1, nameof(entityPM.OtherParticipantInformationCode1), fieldChanges);
            entityPoco.OtherParticipantInformationCode1 = entityPM.OtherParticipantInformationCode1;
            
            FieldChange.Add(entityPoco.OtherParticipantInformationCode2, entityPM.OtherParticipantInformationCode2, nameof(entityPM.OtherParticipantInformationCode2), fieldChanges);
            entityPoco.OtherParticipantInformationCode2 = entityPM.OtherParticipantInformationCode2;
            
            FieldChange.Add(entityPoco.OtherParticipantInformationCode3, entityPM.OtherParticipantInformationCode3, nameof(entityPM.OtherParticipantInformationCode3), fieldChanges);
            entityPoco.OtherParticipantInformationCode3 = entityPM.OtherParticipantInformationCode3;
            
            FieldChange.Add(entityPoco.OtherParticipantInformationPortCode1, entityPM.OtherParticipantInformationPortCode1, nameof(entityPM.OtherParticipantInformationPortCode1), fieldChanges);
            entityPoco.OtherParticipantInformationPortCode1 = entityPM.OtherParticipantInformationPortCode1;
            
            FieldChange.Add(entityPoco.OtherParticipantInformationPortCode2, entityPM.OtherParticipantInformationPortCode2, nameof(entityPM.OtherParticipantInformationPortCode2), fieldChanges);
            entityPoco.OtherParticipantInformationPortCode2 = entityPM.OtherParticipantInformationPortCode2;
            
            FieldChange.Add(entityPoco.OtherParticipantInformationPortCode3, entityPM.OtherParticipantInformationPortCode3, nameof(entityPM.OtherParticipantInformationPortCode3), fieldChanges);
            entityPoco.OtherParticipantInformationPortCode3 = entityPM.OtherParticipantInformationPortCode3;
            
            FieldChange.Add(entityPoco.OtherParticipantInformationName1, entityPM.OtherParticipantInformationName1, nameof(entityPM.OtherParticipantInformationName1), fieldChanges);
            entityPoco.OtherParticipantInformationName1 = entityPM.OtherParticipantInformationName1;
            
            FieldChange.Add(entityPoco.OtherParticipantInformationName2, entityPM.OtherParticipantInformationName2, nameof(entityPM.OtherParticipantInformationName2), fieldChanges);
            entityPoco.OtherParticipantInformationName2 = entityPM.OtherParticipantInformationName2;
            
            FieldChange.Add(entityPoco.OtherParticipantInformationName3, entityPM.OtherParticipantInformationName3, nameof(entityPM.OtherParticipantInformationName3), fieldChanges);
            entityPoco.OtherParticipantInformationName3 = entityPM.OtherParticipantInformationName3;
            
            FieldChange.Add(entityPoco.OtherParticipantInformationReference1, entityPM.OtherParticipantInformationReference1, nameof(entityPM.OtherParticipantInformationReference1), fieldChanges);
            entityPoco.OtherParticipantInformationReference1 = entityPM.OtherParticipantInformationReference1;
            
            FieldChange.Add(entityPoco.OtherParticipantInformationReference2, entityPM.OtherParticipantInformationReference2, nameof(entityPM.OtherParticipantInformationReference2), fieldChanges);
            entityPoco.OtherParticipantInformationReference2 = entityPM.OtherParticipantInformationReference2;
            
            FieldChange.Add(entityPoco.OtherParticipantInformationReference3, entityPM.OtherParticipantInformationReference3, nameof(entityPM.OtherParticipantInformationReference3), fieldChanges);
            entityPoco.OtherParticipantInformationReference3 = entityPM.OtherParticipantInformationReference3;
            
            FieldChange.Add(entityPoco.AccountingInformation1, entityPM.AccountingInformation1, nameof(entityPM.AccountingInformation1), fieldChanges);
            entityPoco.AccountingInformation1 = entityPM.AccountingInformation1;
            
            FieldChange.Add(entityPoco.AccountingInformation2, entityPM.AccountingInformation2, nameof(entityPM.AccountingInformation2), fieldChanges);
            entityPoco.AccountingInformation2 = entityPM.AccountingInformation2;
            
            FieldChange.Add(entityPoco.AccountingInformation3, entityPM.AccountingInformation3, nameof(entityPM.AccountingInformation3), fieldChanges);
            entityPoco.AccountingInformation3 = entityPM.AccountingInformation3;
            
            FieldChange.Add(entityPoco.AccountingInformation4, entityPM.AccountingInformation4, nameof(entityPM.AccountingInformation4), fieldChanges);
            entityPoco.AccountingInformation4 = entityPM.AccountingInformation4;
            
            FieldChange.Add(entityPoco.AccountingInformation5, entityPM.AccountingInformation5, nameof(entityPM.AccountingInformation5), fieldChanges);
            entityPoco.AccountingInformation5 = entityPM.AccountingInformation5;
            
            FieldChange.Add(entityPoco.AccountingInformation6, entityPM.AccountingInformation6, nameof(entityPM.AccountingInformation6), fieldChanges);
            entityPoco.AccountingInformation6 = entityPM.AccountingInformation6;
            
            FieldChange.Add(entityPoco.AccountingInformationIdentifierCode1, entityPM.AccountingInformationIdentifierCode1, nameof(entityPM.AccountingInformationIdentifierCode1), fieldChanges);
            entityPoco.AccountingInformationIdentifierCode1 = entityPM.AccountingInformationIdentifierCode1;
            
            FieldChange.Add(entityPoco.AccountingInformationIdentifierCode2, entityPM.AccountingInformationIdentifierCode2, nameof(entityPM.AccountingInformationIdentifierCode2), fieldChanges);
            entityPoco.AccountingInformationIdentifierCode2 = entityPM.AccountingInformationIdentifierCode2;
            
            FieldChange.Add(entityPoco.AccountingInformationIdentifierCode3, entityPM.AccountingInformationIdentifierCode3, nameof(entityPM.AccountingInformationIdentifierCode3), fieldChanges);
            entityPoco.AccountingInformationIdentifierCode3 = entityPM.AccountingInformationIdentifierCode3;
            
            FieldChange.Add(entityPoco.AccountingInformationIdentifierCode4, entityPM.AccountingInformationIdentifierCode4, nameof(entityPM.AccountingInformationIdentifierCode4), fieldChanges);
            entityPoco.AccountingInformationIdentifierCode4 = entityPM.AccountingInformationIdentifierCode4;
            
            FieldChange.Add(entityPoco.AccountingInformationIdentifierCode5, entityPM.AccountingInformationIdentifierCode5, nameof(entityPM.AccountingInformationIdentifierCode5), fieldChanges);
            entityPoco.AccountingInformationIdentifierCode5 = entityPM.AccountingInformationIdentifierCode5;
            
            FieldChange.Add(entityPoco.AccountingInformationIdentifierCode6, entityPM.AccountingInformationIdentifierCode6, nameof(entityPM.AccountingInformationIdentifierCode6), fieldChanges);
            entityPoco.AccountingInformationIdentifierCode6 = entityPM.AccountingInformationIdentifierCode6;
            
            FieldChange.Add(entityPoco.ReferenceNumber, entityPM.ReferenceNumber, nameof(entityPM.ReferenceNumber), fieldChanges);
            entityPoco.ReferenceNumber = entityPM.ReferenceNumber;
            
            FieldChange.Add(entityPoco.SupplementaryShipmentInformation1, entityPM.SupplementaryShipmentInformation1, nameof(entityPM.SupplementaryShipmentInformation1), fieldChanges);
            entityPoco.SupplementaryShipmentInformation1 = entityPM.SupplementaryShipmentInformation1;
            
            FieldChange.Add(entityPoco.SupplementaryShipmentInformation2, entityPM.SupplementaryShipmentInformation2, nameof(entityPM.SupplementaryShipmentInformation2), fieldChanges);
            entityPoco.SupplementaryShipmentInformation2 = entityPM.SupplementaryShipmentInformation2;
            
            FieldChange.Add(entityPoco.NumberOfInsidePackages, entityPM.NumberOfInsidePackages, nameof(entityPM.NumberOfInsidePackages), fieldChanges);
            entityPoco.NumberOfInsidePackages = entityPM.NumberOfInsidePackages;
            
            FieldChange.Add(entityPoco.NumberOfInsidePackagesDetails, entityPM.NumberOfInsidePackagesDetails, nameof(entityPM.NumberOfInsidePackagesDetails), fieldChanges);
            entityPoco.NumberOfInsidePackagesDetails = entityPM.NumberOfInsidePackagesDetails;
            
            FieldChange.Add(entityPoco.ViaColoader, entityPM.ViaColoader, nameof(entityPM.ViaColoader), fieldChanges);
            entityPoco.ViaColoader = entityPM.ViaColoader;
            
            FieldChange.Add(entityPoco.IssuingCarrierReference1, entityPM.IssuingCarrierReference1, nameof(entityPM.IssuingCarrierReference1), fieldChanges);
            entityPoco.IssuingCarrierReference1 = entityPM.IssuingCarrierReference1;

            if (entityPoco.OperationalCloseDate == null && entityPM.OperationalCloseDate != null)
            {
                
                FieldChange.Add(entityPoco.OperationalClosedByUserId, entityPM.OperationalClosedByUserId, nameof(entityPM.OperationalClosedByUserId), fieldChanges);
                entityPoco.OperationalClosedByUserId = entityPM.OperationalClosedByUserId;
            }
            else if (entityPoco.OperationalCloseDate != null && entityPM.OperationalCloseDate == null)
            {
                
                FieldChange.Add(entityPoco.OperationalClosedByUserId, entityPM.OperationalClosedByUserId, nameof(entityPM.OperationalClosedByUserId), fieldChanges);
                entityPoco.OperationalClosedByUserId = entityPM.OperationalClosedByUserId;
            }
              
            FieldChange.Add(entityPoco.OperationalCloseDate, entityPM.OperationalCloseDate, nameof(entityPM.OperationalCloseDate), fieldChanges);
            entityPoco.OperationalCloseDate = entityPM.OperationalCloseDate;
            
            FieldChange.Add(entityPoco.AccountingCloseDate, entityPM.AccountingCloseDate, nameof(entityPM.AccountingCloseDate), fieldChanges);
            entityPoco.AccountingCloseDate = entityPM.AccountingCloseDate;
            
            FieldChange.Add(entityPoco.ForwarderPartnerId, entityPM.ForwarderPartnerId, nameof(entityPM.ForwarderPartnerId), fieldChanges);
            entityPoco.ForwarderPartnerId = entityPM.ForwarderPartnerId;
            
            FieldChange.Add(entityPoco.ForwardingPartnerId, entityPM.ForwardingPartnerId, nameof(entityPM.ForwardingPartnerId), fieldChanges);
            entityPoco.ForwardingPartnerId = entityPM.ForwardingPartnerId;
            
            FieldChange.Add(entityPoco.NumberOfFollowUps, entityPM.NumberOfFollowUps, nameof(entityPM.NumberOfFollowUps), fieldChanges);
            entityPoco.NumberOfFollowUps = entityPM.NumberOfFollowUps;
            
            FieldChange.Add(entityPoco.ValueOfGoods, entityPM.ValueOfGoods == null ? null : MethodHelper.Round(entityPM.ValueOfGoods, 2), nameof(entityPM.ValueOfGoods), fieldChanges);
            entityPoco.ValueOfGoods = entityPM.ValueOfGoods == null ? null : MethodHelper.Round(entityPM.ValueOfGoods, 2);
            
            FieldChange.Add(entityPoco.ValueOfGoodsCurrencyId, entityPM.ValueOfGoodsCurrencyId, nameof(entityPM.ValueOfGoodsCurrencyId), fieldChanges);
            entityPoco.ValueOfGoodsCurrencyId = entityPM.ValueOfGoodsCurrencyId;
            
            FieldChange.Add(entityPoco.IsNewARInvoiceBlocked, entityPM.IsNewARInvoiceBlocked, nameof(entityPM.IsNewARInvoiceBlocked), fieldChanges);
            entityPoco.IsNewARInvoiceBlocked = entityPM.IsNewARInvoiceBlocked;
            
            FieldChange.Add(entityPoco.FBLIsFromStock, entityPM.FBLIsFromStock, nameof(entityPM.FBLIsFromStock), fieldChanges);
            entityPoco.FBLIsFromStock = entityPM.FBLIsFromStock;
            
            FieldChange.Add(entityPoco.AMSBL, entityPM.AMSBL, nameof(entityPM.AMSBL), fieldChanges);
            entityPoco.AMSBL = entityPM.AMSBL;
            
            FieldChange.Add(entityPoco.MoveTypeId, entityPM.MoveTypeId, nameof(entityPM.MoveTypeId), fieldChanges);
            entityPoco.MoveTypeId = entityPM.MoveTypeId;
            
            FieldChange.Add(entityPoco.TEU, entityPM.TEU, nameof(entityPM.TEU), fieldChanges);
            entityPoco.TEU = entityPM.TEU;
            
            FieldChange.Add(entityPoco.FreightRelease, entityPM.FreightRelease, nameof(entityPM.FreightRelease), fieldChanges);
            entityPoco.FreightRelease = entityPM.FreightRelease;
            
            FieldChange.Add(entityPoco.TerminalAvailable, entityPM.TerminalAvailable, nameof(entityPM.TerminalAvailable), fieldChanges);
            entityPoco.TerminalAvailable = entityPM.TerminalAvailable;
            
            FieldChange.Add(entityPoco.Terminal2Available, entityPM.Terminal2Available, nameof(entityPM.Terminal2Available), fieldChanges);
            entityPoco.Terminal2Available = entityPM.Terminal2Available;
            
            FieldChange.Add(entityPoco.ISFNumber, entityPM.ISFNumber, nameof(entityPM.ISFNumber), fieldChanges);
            entityPoco.ISFNumber = entityPM.ISFNumber;
            
            FieldChange.Add(entityPoco.ISFDate, entityPM.ISFDate, nameof(entityPM.ISFDate), fieldChanges);
            entityPoco.ISFDate = entityPM.ISFDate;
            
            FieldChange.Add(entityPoco.ITNumber, entityPM.ITNumber, nameof(entityPM.ITNumber), fieldChanges);
            entityPoco.ITNumber = entityPM.ITNumber;
            
            FieldChange.Add(entityPoco.ITDate, entityPM.ITDate, nameof(entityPM.ITDate), fieldChanges);
            entityPoco.ITDate = entityPM.ITDate;
            
            FieldChange.Add(entityPoco.ENSNumber, entityPM.ENSNumber, nameof(entityPM.ENSNumber), fieldChanges);
            entityPoco.ENSNumber = entityPM.ENSNumber;
            
            FieldChange.Add(entityPoco.ENSDate, entityPM.ENSDate, nameof(entityPM.ENSDate), fieldChanges);
            entityPoco.ENSDate = entityPM.ENSDate;
            
            FieldChange.Add(entityPoco.WarehouseLegWarehouseId, entityPM.WarehouseLegWarehouseId, nameof(entityPM.WarehouseLegWarehouseId), fieldChanges);
            entityPoco.WarehouseLegWarehouseId = entityPM.WarehouseLegWarehouseId;
            
            FieldChange.Add(entityPoco.WarehouseLegAddressId, entityPM.WarehouseLegAddressId, nameof(entityPM.WarehouseLegAddressId), fieldChanges);
            entityPoco.WarehouseLegAddressId = entityPM.WarehouseLegAddressId;
            
            FieldChange.Add(entityPoco.WarehouseLegTerminalCode, entityPM.WarehouseLegTerminalCode, nameof(entityPM.WarehouseLegTerminalCode), fieldChanges);
            entityPoco.WarehouseLegTerminalCode = entityPM.WarehouseLegTerminalCode;
            
            FieldChange.Add(entityPoco.WarehouseLegExpectedEntryDate, entityPM.WarehouseLegExpectedEntryDate, nameof(entityPM.WarehouseLegExpectedEntryDate), fieldChanges);
            entityPoco.WarehouseLegExpectedEntryDate = entityPM.WarehouseLegExpectedEntryDate;
            
            FieldChange.Add(entityPoco.WarehouseLegActualEntryDate, entityPM.WarehouseLegActualEntryDate, nameof(entityPM.WarehouseLegActualEntryDate), fieldChanges);
            entityPoco.WarehouseLegActualEntryDate = entityPM.WarehouseLegActualEntryDate;
            
            FieldChange.Add(entityPoco.WarehouseLegExpectedReleaseDate, entityPM.WarehouseLegExpectedReleaseDate, nameof(entityPM.WarehouseLegExpectedReleaseDate), fieldChanges);
            entityPoco.WarehouseLegExpectedReleaseDate = entityPM.WarehouseLegExpectedReleaseDate;
            
            FieldChange.Add(entityPoco.WarehouseLegActualReleaseDate, entityPM.WarehouseLegActualReleaseDate, nameof(entityPM.WarehouseLegActualReleaseDate), fieldChanges);
            entityPoco.WarehouseLegActualReleaseDate = entityPM.WarehouseLegActualReleaseDate;
            
            FieldChange.Add(entityPoco.WarehouseLegLastFreeDate, entityPM.WarehouseLegLastFreeDate, nameof(entityPM.WarehouseLegLastFreeDate), fieldChanges);
            entityPoco.WarehouseLegLastFreeDate = entityPM.WarehouseLegLastFreeDate;
            
            FieldChange.Add(entityPoco.WarehouseLegRemarks, entityPM.WarehouseLegRemarks, nameof(entityPM.WarehouseLegRemarks), fieldChanges);
            entityPoco.WarehouseLegRemarks = entityPM.WarehouseLegRemarks;
            
            FieldChange.Add(entityPoco.WarehouseLegReference, entityPM.WarehouseLegReference, nameof(entityPM.WarehouseLegReference), fieldChanges);
            entityPoco.WarehouseLegReference = entityPM.WarehouseLegReference;
            
            FieldChange.Add(entityPoco.WarehouseLegCutOffDate, entityPM.WarehouseLegCutOffDate, nameof(entityPM.WarehouseLegCutOffDate), fieldChanges);
            entityPoco.WarehouseLegCutOffDate = entityPM.WarehouseLegCutOffDate;
            
            FieldChange.Add(entityPoco.WarehouseLegVGMCutOffDate, entityPM.WarehouseLegVGMCutOffDate, nameof(entityPM.WarehouseLegVGMCutOffDate), fieldChanges);
            entityPoco.WarehouseLegVGMCutOffDate = entityPM.WarehouseLegVGMCutOffDate;
            
            FieldChange.Add(entityPoco.WarehouseLeg2WarehouseId, entityPM.WarehouseLeg2WarehouseId, nameof(entityPM.WarehouseLeg2WarehouseId), fieldChanges);
            entityPoco.WarehouseLeg2WarehouseId = entityPM.WarehouseLeg2WarehouseId;
            
            FieldChange.Add(entityPoco.WarehouseLeg2AddressId, entityPM.WarehouseLeg2AddressId, nameof(entityPM.WarehouseLeg2AddressId), fieldChanges);
            entityPoco.WarehouseLeg2AddressId = entityPM.WarehouseLeg2AddressId;
            
            FieldChange.Add(entityPoco.WarehouseLeg2TerminalCode, entityPM.WarehouseLeg2TerminalCode, nameof(entityPM.WarehouseLeg2TerminalCode), fieldChanges);
            entityPoco.WarehouseLeg2TerminalCode = entityPM.WarehouseLeg2TerminalCode;
            
            FieldChange.Add(entityPoco.WarehouseLeg2ExpectedEntryDate, entityPM.WarehouseLeg2ExpectedEntryDate, nameof(entityPM.WarehouseLeg2ExpectedEntryDate), fieldChanges);
            entityPoco.WarehouseLeg2ExpectedEntryDate = entityPM.WarehouseLeg2ExpectedEntryDate;
            
            FieldChange.Add(entityPoco.WarehouseLeg2ActualEntryDate, entityPM.WarehouseLeg2ActualEntryDate, nameof(entityPM.WarehouseLeg2ActualEntryDate), fieldChanges);
            entityPoco.WarehouseLeg2ActualEntryDate = entityPM.WarehouseLeg2ActualEntryDate;
            
            FieldChange.Add(entityPoco.WarehouseLeg2ExpectedReleaseDate, entityPM.WarehouseLeg2ExpectedReleaseDate, nameof(entityPM.WarehouseLeg2ExpectedReleaseDate), fieldChanges);
            entityPoco.WarehouseLeg2ExpectedReleaseDate = entityPM.WarehouseLeg2ExpectedReleaseDate;
            
            FieldChange.Add(entityPoco.WarehouseLeg2ActualReleaseDate, entityPM.WarehouseLeg2ActualReleaseDate, nameof(entityPM.WarehouseLeg2ActualReleaseDate), fieldChanges);
            entityPoco.WarehouseLeg2ActualReleaseDate = entityPM.WarehouseLeg2ActualReleaseDate;
            
            FieldChange.Add(entityPoco.WarehouseLeg2Remarks, entityPM.WarehouseLeg2Remarks, nameof(entityPM.WarehouseLeg2Remarks), fieldChanges);
            entityPoco.WarehouseLeg2Remarks = entityPM.WarehouseLeg2Remarks;
            
            FieldChange.Add(entityPoco.WarehouseLeg2Reference, entityPM.WarehouseLeg2Reference, nameof(entityPM.WarehouseLeg2Reference), fieldChanges);
            entityPoco.WarehouseLeg2Reference = entityPM.WarehouseLeg2Reference;
            
            FieldChange.Add(entityPoco.WarehouseLeg2CutOffDate, entityPM.WarehouseLeg2CutOffDate, nameof(entityPM.WarehouseLeg2CutOffDate), fieldChanges);
            entityPoco.WarehouseLeg2CutOffDate = entityPM.WarehouseLeg2CutOffDate;
            
            FieldChange.Add(entityPoco.WarehouseLeg2VGMCutOffDate, entityPM.WarehouseLeg2VGMCutOffDate, nameof(entityPM.WarehouseLeg2VGMCutOffDate), fieldChanges);
            entityPoco.WarehouseLeg2VGMCutOffDate = entityPM.WarehouseLeg2VGMCutOffDate;
            
            FieldChange.Add(entityPoco.IsAssembly, entityPM.IsAssembly, nameof(entityPM.IsAssembly), fieldChanges);
            entityPoco.IsAssembly = entityPM.IsAssembly;
            
            FieldChange.Add(entityPoco.LastSharedEventId, entityPM.LastSharedEventId, nameof(entityPM.LastSharedEventId), fieldChanges);
            entityPoco.LastSharedEventId = entityPM.LastSharedEventId;
            
            FieldChange.Add(entityPoco.LastSharedEventLocation, entityPM.LastSharedEventLocation, nameof(entityPM.LastSharedEventLocation), fieldChanges);
            entityPoco.LastSharedEventLocation = entityPM.LastSharedEventLocation;
            
            FieldChange.Add(entityPoco.LastSharedEventNotes, entityPM.LastSharedEventNotes, nameof(entityPM.LastSharedEventNotes), fieldChanges);
            entityPoco.LastSharedEventNotes = entityPM.LastSharedEventNotes;
            
            FieldChange.Add(entityPoco.LastSharedEventDate, entityPM.LastSharedEventDate, nameof(entityPM.LastSharedEventDate), fieldChanges);
            entityPoco.LastSharedEventDate = entityPM.LastSharedEventDate;

            if (entityPoco.FirstOperationalCloseDate == null)
            {
                FieldChange.Add(entityPoco.FirstOperationalCloseDate, entityPM.FirstOperationalCloseDate, nameof(entityPM.FirstOperationalCloseDate), fieldChanges);
                entityPoco.FirstOperationalCloseDate = entityPM.FirstOperationalCloseDate;
            }

            FieldChange.Add(entityPoco.FirstAccountingCloseDate, entityPM.FirstAccountingCloseDate, nameof(entityPM.FirstAccountingCloseDate), fieldChanges);
            entityPoco.FirstAccountingCloseDate = entityPM.FirstAccountingCloseDate;
            
            FieldChange.Add(entityPoco.AMSClosingDate, entityPM.AMSClosingDate, nameof(entityPM.AMSClosingDate), fieldChanges);
            entityPoco.AMSClosingDate = entityPM.AMSClosingDate;
            
            FieldChange.Add(entityPoco.UpdatedByPartner, entityPM.UpdatedByPartner, nameof(entityPM.UpdatedByPartner), fieldChanges);
            entityPoco.UpdatedByPartner = entityPM.UpdatedByPartner;
            
            FieldChange.Add(entityPoco.EmergencyContactId, entityPM.EmergencyContactId, nameof(entityPM.EmergencyContactId), fieldChanges);
            entityPoco.EmergencyContactId = entityPM.EmergencyContactId;            
            
            FieldChange.Add(entityPoco.LastFinalDestination, entityPM.LastFinalDestination, nameof(entityPM.LastFinalDestination), fieldChanges);
            entityPoco.LastFinalDestination = entityPM.LastFinalDestination;
            
            FieldChange.Add(entityPoco.FirstPickupETA, entityPM.FirstPickupETA, nameof(entityPM.FirstPickupETA), fieldChanges);
            entityPoco.FirstPickupETA = entityPM.FirstPickupETA;
            
            FieldChange.Add(entityPoco.FirstPickupETD, entityPM.FirstPickupETD, nameof(entityPM.FirstPickupETD), fieldChanges);
            entityPoco.FirstPickupETD = entityPM.FirstPickupETD;
            
            FieldChange.Add(entityPoco.OnForwardingAdditionalTransportModeCode, entityPM.OnForwardingAdditionalTransportModeCode, nameof(entityPM.OnForwardingAdditionalTransportModeCode), fieldChanges);
            entityPoco.OnForwardingAdditionalTransportModeCode = entityPM.OnForwardingAdditionalTransportModeCode;
            
            FieldChange.Add(entityPoco.SplitOnForwarding, entityPM.SplitOnForwarding, nameof(entityPM.SplitOnForwarding), fieldChanges);
            entityPoco.SplitOnForwarding = entityPM.SplitOnForwarding;
            
            FieldChange.Add(entityPoco.Notify1Reference, entityPM.Notify1Reference, nameof(entityPM.Notify1Reference), fieldChanges);
            entityPoco.Notify1Reference = entityPM.Notify1Reference;
            
            FieldChange.Add(entityPoco.Notify1Reference2, entityPM.Notify1Reference2, nameof(entityPM.Notify1Reference2), fieldChanges);
            entityPoco.Notify1Reference2 = entityPM.Notify1Reference2;
            
            FieldChange.Add(entityPoco.Notify2Reference, entityPM.Notify2Reference, nameof(entityPM.Notify2Reference), fieldChanges);
            entityPoco.Notify2Reference = entityPM.Notify2Reference;
            
            FieldChange.Add(entityPoco.ShipperNotExporterReference, entityPM.ShipperNotExporterReference, nameof(entityPM.ShipperNotExporterReference), fieldChanges);
            entityPoco.ShipperNotExporterReference = entityPM.ShipperNotExporterReference;
            
            FieldChange.Add(entityPoco.ShipperNotExporterReference1, entityPM.ShipperNotExporterReference1, nameof(entityPM.ShipperNotExporterReference1), fieldChanges);
            entityPoco.ShipperNotExporterReference1 = entityPM.ShipperNotExporterReference1;
            
            FieldChange.Add(entityPoco.ShipperNotExporterReference2, entityPM.ShipperNotExporterReference2, nameof(entityPM.ShipperNotExporterReference2), fieldChanges);
            entityPoco.ShipperNotExporterReference2 = entityPM.ShipperNotExporterReference2;
            
            FieldChange.Add(entityPoco.ConsigneeNotImporterReference, entityPM.ConsigneeNotImporterReference, nameof(entityPM.ConsigneeNotImporterReference), fieldChanges);
            entityPoco.ConsigneeNotImporterReference = entityPM.ConsigneeNotImporterReference;
            
            FieldChange.Add(entityPoco.ProjectNumber, entityPM.ProjectNumber, nameof(entityPM.ProjectNumber), fieldChanges);
            entityPoco.ProjectNumber = entityPM.ProjectNumber;
            
            FieldChange.Add(entityPoco.ContainerLastStatusDate, entityPM.ContainerLastStatusDate, nameof(entityPM.ContainerLastStatusDate), fieldChanges);
            entityPoco.ContainerLastStatusDate = entityPM.ContainerLastStatusDate;
            
            FieldChange.Add(entityPoco.BasicFreightId, entityPM.BasicFreightId, nameof(entityPM.BasicFreightId), fieldChanges);
            entityPoco.BasicFreightId = entityPM.BasicFreightId;
            
            FieldChange.Add(entityPoco.DestinationPortChargesId, entityPM.DestinationPortChargesId, nameof(entityPM.DestinationPortChargesId), fieldChanges);
            entityPoco.DestinationPortChargesId = entityPM.DestinationPortChargesId;
            
            FieldChange.Add(entityPoco.DestinationHaulageChargesId, entityPM.DestinationHaulageChargesId, nameof(entityPM.DestinationHaulageChargesId), fieldChanges);
            entityPoco.DestinationHaulageChargesId = entityPM.DestinationHaulageChargesId;
            
            FieldChange.Add(entityPoco.AdditionalChargesId, entityPM.AdditionalChargesId, nameof(entityPM.AdditionalChargesId), fieldChanges);
            entityPoco.AdditionalChargesId = entityPM.AdditionalChargesId;
            
            FieldChange.Add(entityPoco.FreightPayerId, entityPM.FreightPayerId, nameof(entityPM.FreightPayerId), fieldChanges);
            entityPoco.FreightPayerId = entityPM.FreightPayerId;
            
            FieldChange.Add(entityPoco.FreightPayerAddressId, entityPM.FreightPayerAddressId, nameof(entityPM.FreightPayerAddressId), fieldChanges);
            entityPoco.FreightPayerAddressId = entityPM.FreightPayerAddressId;
            
            FieldChange.Add(entityPoco.From, entityPM.From, nameof(entityPM.From), fieldChanges);
            entityPoco.From = entityPM.From;
            
            FieldChange.Add(entityPoco.To, entityPM.To, nameof(entityPM.To), fieldChanges);
            entityPoco.To = entityPM.To;
            
            FieldChange.Add(entityPoco.Origin, entityPM.Origin, nameof(entityPM.Origin), fieldChanges);
            entityPoco.Origin = entityPM.Origin;
            
            FieldChange.Add(entityPoco.ComputedShipmentNumber, entityPM.ComputedShipmentNumber, nameof(entityPM.ComputedShipmentNumber), fieldChanges);
            entityPoco.ComputedShipmentNumber = entityPM.ComputedShipmentNumber;
            
            FieldChange.Add(entityPoco.ShipmentSubTypeId, entityPM.ShipmentSubTypeId, nameof(entityPM.ShipmentSubTypeId), fieldChanges);
            entityPoco.ShipmentSubTypeId = entityPM.ShipmentSubTypeId;
            
            FieldChange.Add(entityPoco.ChargeStorage, entityPM.ChargeStorage, nameof(entityPM.ChargeStorage), fieldChanges);
            entityPoco.ChargeStorage = entityPM.ChargeStorage;
            
            FieldChange.Add(entityPoco.ChargeStorageCurrencyId, entityPM.ChargeStorageCurrencyId, nameof(entityPM.ChargeStorageCurrencyId), fieldChanges);
            entityPoco.ChargeStorageCurrencyId = entityPM.ChargeStorageCurrencyId;
            
            FieldChange.Add(entityPoco.WeightMeasurementCode, entityPM.WeightMeasurementCode, nameof(entityPM.WeightMeasurementCode), fieldChanges);
            entityPoco.WeightMeasurementCode = entityPM.WeightMeasurementCode;
            
            FieldChange.Add(entityPoco.WeightRoundingCode, entityPM.WeightRoundingCode, nameof(entityPM.WeightRoundingCode), fieldChanges);
            entityPoco.WeightRoundingCode = entityPM.WeightRoundingCode;
            
            FieldChange.Add(entityPoco.IsCFSWarehouse, entityPM.IsCFSWarehouse, nameof(entityPM.IsCFSWarehouse), fieldChanges);
            entityPoco.IsCFSWarehouse = entityPM.IsCFSWarehouse;
            
            FieldChange.Add(entityPoco.IsCFSWarehouseChanged, entityPM.IsCFSWarehouseChanged, nameof(entityPM.IsCFSWarehouseChanged), fieldChanges);
            entityPoco.IsCFSWarehouseChanged = entityPM.IsCFSWarehouseChanged;
            
            FieldChange.Add(entityPoco.FinalArrivalDate, entityPM.FinalArrivalDate, nameof(entityPM.FinalArrivalDate), fieldChanges);
            entityPoco.FinalArrivalDate = entityPM.FinalArrivalDate;
            
            FieldChange.Add(entityPoco.EstimatedFinalArrivalDate, entityPM.EstimatedFinalArrivalDate, nameof(entityPM.EstimatedFinalArrivalDate), fieldChanges);
            entityPoco.EstimatedFinalArrivalDate = entityPM.EstimatedFinalArrivalDate;
            
            FieldChange.Add(entityPoco.ActualFinalArrivalDate, entityPM.ActualFinalArrivalDate, nameof(entityPM.ActualFinalArrivalDate), fieldChanges);
            entityPoco.ActualFinalArrivalDate = entityPM.ActualFinalArrivalDate;
            
            FieldChange.Add(entityPoco.IsAccrualsApproved, entityPM.IsAccrualsApproved, nameof(entityPM.IsAccrualsApproved), fieldChanges);
            entityPoco.IsAccrualsApproved = entityPM.IsAccrualsApproved;
            
            FieldChange.Add(entityPoco.AccrualsApprovalDate, entityPM.AccrualsApprovalDate, nameof(entityPM.AccrualsApprovalDate), fieldChanges);
            entityPoco.AccrualsApprovalDate = entityPM.AccrualsApprovalDate;
            
            FieldChange.Add(entityPoco.AssignedToTruckerDate, entityPM.AssignedToTruckerDate, nameof(entityPM.AssignedToTruckerDate), fieldChanges);
            entityPoco.AssignedToTruckerDate = entityPM.AssignedToTruckerDate;
            
            FieldChange.Add(entityPoco.TruckerId, entityPM.TruckerId, nameof(entityPM.TruckerId), fieldChanges);
            entityPoco.TruckerId = entityPM.TruckerId;
            
            FieldChange.Add(entityPoco.TruckerAddressId, entityPM.TruckerAddressId, nameof(entityPM.TruckerAddressId), fieldChanges);
            entityPoco.TruckerAddressId = entityPM.TruckerAddressId;
            
            FieldChange.Add(entityPoco.TruckerContactId, entityPM.TruckerContactId, nameof(entityPM.TruckerContactId), fieldChanges);
            entityPoco.TruckerContactId = entityPM.TruckerContactId;
            
            FieldChange.Add(entityPoco.TruckerReference1, entityPM.TruckerReference1, nameof(entityPM.TruckerReference1), fieldChanges);
            entityPoco.TruckerReference1 = entityPM.TruckerReference1;
            
            FieldChange.Add(entityPoco.TruckerReference2, entityPM.TruckerReference2, nameof(entityPM.TruckerReference2), fieldChanges);
            entityPoco.TruckerReference2 = entityPM.TruckerReference2;
            
            FieldChange.Add(entityPoco.AssginedToCustomsAgentDate, entityPM.AssginedToCustomsAgentDate, nameof(entityPM.AssginedToCustomsAgentDate), fieldChanges);
            entityPoco.AssginedToCustomsAgentDate = entityPM.AssginedToCustomsAgentDate; 
            
            FieldChange.Add(entityPoco.IsStandalonePickupDelivery, entityPM.IsStandalonePickupDelivery, nameof(entityPM.IsStandalonePickupDelivery), fieldChanges);
            entityPoco.IsStandalonePickupDelivery = entityPM.IsStandalonePickupDelivery;
            
            FieldChange.Add(entityPoco.ForwarderStandaloneShipmentId, entityPM.ForwarderStandaloneShipmentId, nameof(entityPM.ForwarderStandaloneShipmentId), fieldChanges);
            entityPoco.ForwarderStandaloneShipmentId = entityPM.ForwarderStandaloneShipmentId;
            
            FieldChange.Add(entityPoco.ParentShipmentDirectionId, entityPM.ParentShipmentDirectionId, nameof(entityPM.ParentShipmentDirectionId), fieldChanges);
            entityPoco.ParentShipmentDirectionId = entityPM.ParentShipmentDirectionId;
            
            FieldChange.Add(entityPoco.ParentShipmentNumber, entityPM.ParentShipmentNumber, nameof(entityPM.ParentShipmentNumber), fieldChanges);
            entityPoco.ParentShipmentNumber = entityPM.ParentShipmentNumber;
            
            FieldChange.Add(entityPoco.ParentShipmentType, entityPM.ParentShipmentType, nameof(entityPM.ParentShipmentType), fieldChanges);
            entityPoco.ParentShipmentType = entityPM.ParentShipmentType;
            
            FieldChange.Add(entityPoco.StandalonePickupDeliveryId, entityPM.StandalonePickupDeliveryId, nameof(entityPM.StandalonePickupDeliveryId), fieldChanges);
            entityPoco.StandalonePickupDeliveryId = entityPM.StandalonePickupDeliveryId;
            
            FieldChange.Add(entityPoco.ForwarderPickUpDeliveryType, entityPM.ForwarderPickUpDeliveryType, nameof(entityPM.ForwarderPickUpDeliveryType), fieldChanges);
            entityPoco.ForwarderPickUpDeliveryType = entityPM.ForwarderPickUpDeliveryType;
            
            FieldChange.Add(entityPoco.IsHTSMissing, entityPM.IsHTSMissing, nameof(entityPM.IsHTSMissing), fieldChanges);
            entityPoco.IsHTSMissing = entityPM.IsHTSMissing;
            
            FieldChange.Add(entityPoco.HandlerUserId, entityPM.HandlerUserId, nameof(entityPM.HandlerUserId), fieldChanges);
            entityPoco.HandlerUserId = entityPM.HandlerUserId;
            
            FieldChange.Add(entityPoco.DestinationWarehouseId, entityPM.DestinationWarehouseId, nameof(entityPM.DestinationWarehouseId), fieldChanges);
            entityPoco.DestinationWarehouseId = entityPM.DestinationWarehouseId;
            
            FieldChange.Add(entityPoco.PlannedCargoReadyDate, entityPM.PlannedCargoReadyDate, nameof(entityPM.PlannedCargoReadyDate), fieldChanges);
            entityPoco.PlannedCargoReadyDate = entityPM.PlannedCargoReadyDate;
            
            FieldChange.Add(entityPoco.ApprovedCargoReadyDate, entityPM.ApprovedCargoReadyDate, nameof(entityPM.ApprovedCargoReadyDate), fieldChanges);
            entityPoco.ApprovedCargoReadyDate = entityPM.ApprovedCargoReadyDate; 
            
            FieldChange.Add(entityPoco.PrivateLabelInvoiceNumber, entityPM.PrivateLabelInvoiceNumber, nameof(entityPM.PrivateLabelInvoiceNumber), fieldChanges);
            entityPoco.PrivateLabelInvoiceNumber = entityPM.PrivateLabelInvoiceNumber; 
            
            FieldChange.Add(entityPoco.PrivateLabelIncludePickup, entityPM.PrivateLabelIncludePickup, nameof(entityPM.PrivateLabelIncludePickup), fieldChanges);
            entityPoco.PrivateLabelIncludePickup = entityPM.PrivateLabelIncludePickup;
            
            FieldChange.Add(entityPoco.PrivateLabelIncludeDelivery, entityPM.PrivateLabelIncludeDelivery, nameof(entityPM.PrivateLabelIncludeDelivery), fieldChanges);
            entityPoco.PrivateLabelIncludeDelivery = entityPM.PrivateLabelIncludeDelivery;
            
            FieldChange.Add(entityPoco.RequestedFlightDate, entityPM.RequestedFlightDate, nameof(entityPM.RequestedFlightDate), fieldChanges);
            entityPoco.RequestedFlightDate = entityPM.RequestedFlightDate;
            
            FieldChange.Add(entityPoco.HasUnassignedData, entityPM.HasUnassignedData, nameof(entityPM.HasUnassignedData), fieldChanges);
            entityPoco.HasUnassignedData = entityPM.HasUnassignedData;
            
            FieldChange.Add(entityPoco.IsShipmentOrder, entityPM.IsShipmentOrder, nameof(entityPM.IsShipmentOrder), fieldChanges);
            entityPoco.IsShipmentOrder = entityPM.IsShipmentOrder;
            
            FieldChange.Add(entityPoco.FirstPickupFullAddress, entityPM.FirstPickupFullAddress, nameof(entityPM.FirstPickupFullAddress), fieldChanges);
            entityPoco.FirstPickupFullAddress = entityPM.FirstPickupFullAddress;
            
            FieldChange.Add(entityPoco.LastDeliveryFullAddress, entityPM.LastDeliveryFullAddress, nameof(entityPM.LastDeliveryFullAddress), fieldChanges);
            entityPoco.LastDeliveryFullAddress = entityPM.LastDeliveryFullAddress;
            
            FieldChange.Add(entityPoco.QuoteFreightExpirationDate, entityPM.QuoteFreightExpirationDate, nameof(entityPM.QuoteFreightExpirationDate), fieldChanges);
            entityPoco.QuoteFreightExpirationDate = entityPM.QuoteFreightExpirationDate;
            
            FieldChange.Add(entityPoco.IsINTTRAFROB, entityPM.IsINTTRAFROB, nameof(entityPM.IsINTTRAFROB), fieldChanges);
            entityPoco.IsINTTRAFROB = entityPM.IsINTTRAFROB;
            entityPoco.ShippingLine = entityPM.ShippingLine;
            entityPoco.PlaceOfDelivery = entityPM.PlaceOfDelivery;
            entityPoco.PickupPlace = entityPM.PickupPlace;
            entityPoco.SealNo = entityPM.SealNo;
            entityPoco.HSCode = entityPM.HSCode;

            BuildSearchField(entityPM, entityPoco, entityMasterData, myPackagesList);
            if (!LBcurrentTenant.IsDocumentsArchive)
            {
                BuildRoutingField(entityPM, entityPoco, entityMasterData, objectContext);
            }

            entityPM.PackagesDeleted = false;
            entityPM.ConvertFromDirectToHouse = false;
            entityPM.ConvertFromHouseToDirect = false;

            ValidateMAWBStackField(entityPoco, entityMasterData);
        }

        private static void MapTotalsOfPackages(ShipmentPM entityPM, Shipment entityPoco)
        {
            if (IsLogboxEnvironment()) return;
            List<ShipmentPackagePM> shipmentPackages = entityPM.ShipmentPackages.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();
            if (shipmentPackages == null || (shipmentPackages != null && shipmentPackages.Count == 0))
            {
                entityPoco.NumberOfPackages = entityPM.NumberOfPackages = null;
                entityPoco.NumberOfContainers = entityPM.NumberOfContainers = null;
            }
            else
            {
                MapLCLPackages(entityPM, entityPoco, shipmentPackages);
                MapFCLPackages(entityPM, entityPoco, shipmentPackages);
            }
        }

        private static void MapLCLPackages(ShipmentPM entityPM, Shipment entityPoco, List<ShipmentPackagePM> shipmentPackages)
        {
            if (MethodHelper.IsLCLEntity(entityPM.TransportModeId, entityPM.ShipmentTypeId))
            {
                entityPoco.NumberOfPackages = entityPM.NumberOfPackages = shipmentPackages.Sum(s => s.Quantity);
            }
        }

        private static void MapFCLPackages(ShipmentPM entityPM, Shipment entityPoco, List<ShipmentPackagePM> shipmentPackages)
        {
            if (!MethodHelper.IsLCLEntity(entityPM.TransportModeId, entityPM.ShipmentTypeId))
            {
                entityPoco.NumberOfContainers = entityPM.NumberOfContainers = shipmentPackages.Sum(s => s.Quantity);
            }
        }

        private static void MapShipmentStatus(ShipmentPM entityPM, Shipment entityPoco, 
                                              ShipmentMasterData entityMasterData, List<FieldChange> fieldChanges)
        {
            FieldChange.Add(entityPoco.StatusId, entityPM.StatusId, nameof(entityPM.StatusId), fieldChanges);
            entityPoco.StatusId = entityPM.StatusId;
            
            FieldChange.Add(entityPoco.StatusDate, entityPM.StatusDate, nameof(entityPM.StatusDate), fieldChanges);
            entityPoco.StatusDate = entityPM.StatusDate;
            
            FieldChange.Add(entityPoco.StatusLocation, entityPM.StatusLocation, nameof(entityPM.StatusLocation), fieldChanges);
            entityPoco.StatusLocation = entityPM.StatusLocation;
            
            FieldChange.Add(entityPoco.LastStatusLogDate, entityPM.LastStatusLogDate, nameof(entityPM.LastStatusLogDate), fieldChanges);
            entityPoco.LastStatusLogDate = entityPM.LastStatusLogDate;
            
            FieldChange.Add(entityPoco.PartialStatusAmount, entityPM.PartialStatusAmount, nameof(entityPM.PartialStatusAmount), fieldChanges);
            entityPoco.PartialStatusAmount = entityPM.PartialStatusAmount;
            
            if (entityMasterData != null)
            {
                if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C")
                {
                    FieldChange.Add(entityMasterData.StatusId, entityPM.StatusId, nameof(entityPM.StatusId), fieldChanges);
                    entityMasterData.StatusId = entityPM.StatusId;
                    
                    FieldChange.Add(entityMasterData.StatusDate, entityPM.StatusDate, nameof(entityPM.StatusDate), fieldChanges);
                    entityMasterData.StatusDate = entityPM.StatusDate;
                    
                    FieldChange.Add(entityMasterData.StatusLocation, entityPM.StatusLocation, nameof(entityPM.StatusLocation), fieldChanges);
                    entityMasterData.StatusLocation = entityPM.StatusLocation;
                    
                    FieldChange.Add(entityMasterData.PartialStatusAmount, entityPM.PartialStatusAmount, nameof(entityPM.PartialStatusAmount), fieldChanges);
                    entityMasterData.PartialStatusAmount = entityPM.PartialStatusAmount;
                }
            }

            entityPM.IsStatusChange = false;
        }
        private static void MapShipmentOperationalStatus(ShipmentPM entityPM, Shipment entityPoco, 
                                                         ShipmentMasterData entityMasterData, List<FieldChange> fieldChanges)
        {
            FieldChange.Add(entityPoco.OperationalStatusId, entityPM.OperationalStatusId, nameof(entityPM.OperationalStatusId), fieldChanges);
            entityPoco.OperationalStatusId = entityPM.OperationalStatusId;           
            
            if (entityMasterData != null)
            {
                if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C")
                {
                    FieldChange.Add(entityMasterData.OperationalStatusId, entityPM.OperationalStatusId, nameof(entityPM.OperationalStatusId), fieldChanges);
                    entityMasterData.OperationalStatusId = entityPM.OperationalStatusId;
                }
            }

            entityPM.IsOperationalStatusChange = false;
        }
        private static void ValidateMAWBStackField(Shipment entityPoco, ShipmentMasterData entityMasterData)
        {
            if (entityMasterData != null)
            {
                if (entityPoco.TransportModeId == "A")
                {
                    if (entityMasterData.MainCarriageIsFromStack)
                    {
                        if (entityMasterData.MainCarriageCarrierId == null || entityMasterData.Master == null)
                        {
                            throw new ApplicationException("Master field from stock is required");
                        }
                    }
                }
            }
        }

        private static void MapXSDMessagesFields(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, 
                                                bool isNewEntity, List<FieldChange> fieldChanges)
        {
            // The Send Services uses the direct poco and update it
            // only the FSR Service is using the Shipment service to update shipment pm
            // CCSWebService, ShipmentToCustomsWebService, SimulatorResponsesWebService

            // the flag: IsUpdatedByAnalyzer is used only for mapping these fields
            // we need another flag for stopping the Concurrency test validation

            // but our problem is entityPM from client side using the update service
            // it wont see these flags (specialy the second flag)

            //Customs

            FieldChange.Add(entityPoco.LocalCustomsTransmissionsStatusCode, entityPM.LocalCustomsTransmissionsStatusCode, nameof(entityPM.LocalCustomsTransmissionsStatusCode), fieldChanges);
            entityPoco.LocalCustomsTransmissionsStatusCode = entityPM.LocalCustomsTransmissionsStatusCode;
            
            FieldChange.Add(entityPoco.LocalCustomsTransmissionsStatusError, entityPM.LocalCustomsTransmissionsStatusError, nameof(entityPM.LocalCustomsTransmissionsStatusError), fieldChanges);
            entityPoco.LocalCustomsTransmissionsStatusError = entityPM.LocalCustomsTransmissionsStatusError;
            
            FieldChange.Add(entityPoco.LocalCustomsTransmissionsStatusDate, entityPM.LocalCustomsTransmissionsStatusDate, nameof(entityPM.LocalCustomsTransmissionsStatusDate), fieldChanges);
            entityPoco.LocalCustomsTransmissionsStatusDate = entityPM.LocalCustomsTransmissionsStatusDate;
            
            FieldChange.Add(entityPoco.IncludesCustoms, GetIncludesCustomsValue(entityPM), nameof(entityPM.IncludesCustoms), fieldChanges);
            entityPoco.IncludesCustoms = GetIncludesCustomsValue(entityPM);
            
            FieldChange.Add(entityPoco.DeclarationNumber, entityPM.DeclarationNumber, nameof(entityPM.DeclarationNumber), fieldChanges);
            entityPoco.DeclarationNumber = entityPM.DeclarationNumber;
            
            FieldChange.Add(entityPoco.DeclarationDate, entityPM.DeclarationDate, nameof(entityPM.DeclarationDate), fieldChanges);
            entityPoco.DeclarationDate = entityPM.DeclarationDate;
            
            FieldChange.Add(entityPoco.CustomsClearanceDate, entityPM.CustomsClearanceDate, nameof(entityPM.CustomsClearanceDate), fieldChanges);
            entityPoco.CustomsClearanceDate = entityPM.CustomsClearanceDate;
        }

        private static bool GetIncludesCustomsValue(ShipmentPM entityPM)
        {
            return entityPM.IncludesCustoms || !string.IsNullOrEmpty(entityPM.DeclarationNumber) || entityPM.DeclarationDate != null || entityPM.CustomsClearanceDate != null;
        }

        private static void BuildRoutingField(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, IShipmentsContext objectContext)
        {
            string myRoutingField = null;
            AddressRepository addressRepository = new AddressRepository(entityPoco.Tenant);

            if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
            {
                switch (entityMasterData.InlandDomesticFromTypeCode)
                {
                    case "PART":
                        {
                            if (entityMasterData.MainCarriageFromAddressId != null)
                            {
                                Address fromAddress = addressRepository.GetSingleAddress(entityMasterData.MainCarriageFromAddressId, entityMasterData.Tenant);
                                myRoutingField = fromAddress.City;
                            }
                            break;
                        }

                    case "PORT":
                        {
                            if (entityMasterData.MainCarriageFromPortId != null)
                            {
                                PortPM fromPort = PortQuery.GetSinglePort(entityMasterData.Tenant, entityMasterData.MainCarriageFromPortId, true);
                                if (fromPort != null)
                                {
                                    myRoutingField = fromPort.Code;
                                }
                            }
                            break;
                        }

                    case "CASL":
                        {
                            myRoutingField = entityMasterData.InlandDomesticFromCity;
                            break;
                        }
                }

                switch (entityMasterData.InlandDomesticToTypeCode)
                {
                    case "PART":
                        {
                            if (entityMasterData.MainCarriageToAddressId != null)
                            {
                                Address toAddress = addressRepository.GetSingleAddress(entityMasterData.MainCarriageToAddressId, entityMasterData.Tenant);
                                if (toAddress != null)
                                {
                                    myRoutingField = myRoutingField + " , " + toAddress.City;
                                }
                            }
                            break;
                        }

                    case "PORT":
                        {
                            if (entityMasterData.MainCarriageToPortId != null)
                            {
                                PortPM toPort = PortQuery.GetSinglePort(entityMasterData.Tenant, entityMasterData.MainCarriageToPortId, true);
                                if(toPort != null)
                                {
                                    myRoutingField = myRoutingField + " , " + toPort.Code;
                                }
                            }
                            break;
                        }

                    case "CASL":
                        {
                            myRoutingField = myRoutingField + " , " + entityMasterData.InlandDomesticToCity;
                            break;
                        }
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

                string preCrriageFromPortCode = null;
                string onCarriageToPortCode = null;

                if (entityMasterData != null)
                {
                    if (entityMasterData.PreCarriageFromPortId != null)
                    {
                        PortPM precarriageFromPort = PortQuery.GetSinglePort(entityPoco.Tenant, entityMasterData.PreCarriageFromPortId, true);
                        preCrriageFromPortCode = precarriageFromPort.Code;
                    }

                    if (entityMasterData.OnCarriageToPortId != null)
                    {
                        PortPM oncarriageToPort = PortQuery.GetSinglePort(entityPoco.Tenant, entityMasterData.OnCarriageToPortId, true);
                        onCarriageToPortCode = oncarriageToPort.Code;
                    }
                }

                if (entityPM.ShipmentLevelCode == "H")
                {
                    string preForwardingFromPortCode = null;
                    string onForwardingToPortCode = null;

                    if (entityPoco.PreForwardingFromPortId != null)
                    {
                        PortPM preForwardingFromPort = PortQuery.GetSinglePort(entityPoco.Tenant, entityPoco.PreForwardingFromPortId, true);
                        preForwardingFromPortCode = preForwardingFromPort.Code;
                    }

                    if (entityPoco.OnForwardingToPortId != null)
                    {
                        PortPM onForwardingToPort = PortQuery.GetSinglePort(entityPoco.Tenant, entityPoco.OnForwardingToPortId, true);
                        onForwardingToPortCode = onForwardingToPort.Code;
                    }

                    if (!string.IsNullOrEmpty(preForwardingFromPortCode))
                    {
                        if (!string.IsNullOrEmpty(preCrriageFromPortCode))
                        {
                            myRoutingField = preForwardingFromPortCode + " , " + preCrriageFromPortCode + " , " + myRoutingField;
                        }

                        else
                        {
                            myRoutingField = preForwardingFromPortCode + " , " + myRoutingField;
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(preCrriageFromPortCode))
                        {
                            myRoutingField = preCrriageFromPortCode + " , " + myRoutingField;
                        }
                    }

                    if (!string.IsNullOrEmpty(onForwardingToPortCode))
                    {
                        if (!string.IsNullOrEmpty(onCarriageToPortCode))
                        {
                            myRoutingField = myRoutingField + " , " + onCarriageToPortCode + " , " + onForwardingToPortCode;
                        }

                        else
                        {
                            myRoutingField = myRoutingField + " , " + onForwardingToPortCode;
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(onCarriageToPortCode))
                        {
                            myRoutingField = myRoutingField + " , " + onCarriageToPortCode;
                        }
                    }
                }

                else
                {
                    if (preCrriageFromPortCode != null)
                    {
                        myRoutingField = preCrriageFromPortCode + " , " + myRoutingField;
                    }

                    if (onCarriageToPortCode != null)
                    {
                        myRoutingField = myRoutingField + " , " + onCarriageToPortCode;
                    }
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
        private static void MapMasterData(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, bool isNewEntity, List<FieldChange> fieldChanges)
        {
            if (entityPM.ShipmentLevelCode != "H")
            {
                if (entityMasterData != null)
                {
                    if (entityPM.ShipmentConvertedNewNumber)
                    {
                        FieldChange.Add(entityMasterData.MasterShipmentNumber, entityPM.MasterShipmentNumber, nameof(entityPM.MasterShipmentNumber), fieldChanges);
                        entityMasterData.MasterShipmentNumber = entityPM.MasterShipmentNumber;
                    }

                    FieldChange.Add(entityMasterData.DocumentsClosingDate, entityPM.DocumentsClosingDate, nameof(entityPM.DocumentsClosingDate), fieldChanges);
                    entityMasterData.DocumentsClosingDate = entityPM.DocumentsClosingDate;
                    
                    FieldChange.Add(entityMasterData.OBLTypeCode, entityPM.OBLTypeCode, nameof(entityPM.OBLTypeCode), fieldChanges);
                    entityMasterData.OBLTypeCode = entityPM.OBLTypeCode;
                    
                    FieldChange.Add(entityMasterData.ImportManifest, entityPM.ImportManifest, nameof(entityPM.ImportManifest), fieldChanges);
                    entityMasterData.ImportManifest = entityPM.ImportManifest;
                    
                    FieldChange.Add(entityMasterData.CarrierTransportDocumentNumber, entityPM.CarrierTransportDocumentNumber, nameof(entityPM.CarrierTransportDocumentNumber), fieldChanges);
                    entityMasterData.CarrierTransportDocumentNumber = entityPM.CarrierTransportDocumentNumber;
                    
                    FieldChange.Add(entityMasterData.Tenant, entityPM.Tenant, nameof(entityPM.Tenant), fieldChanges);
                    entityMasterData.Tenant = entityPM.Tenant;
                    
                    FieldChange.Add(entityMasterData.MainCarriageIsFromStack, entityPM.MainCarriageIsFromStack, nameof(entityPM.MainCarriageIsFromStack), fieldChanges);
                    entityMasterData.MainCarriageIsFromStack = entityPM.MainCarriageIsFromStack;
                    
                    FieldChange.Add(entityMasterData.Master, entityPM.Master, nameof(entityPM.Master), fieldChanges);
                    entityMasterData.Master = entityPM.Master;
                    
                    FieldChange.Add(entityMasterData.MAWBOBLDate, entityPM.MAWBOBLDate, nameof(entityPM.MAWBOBLDate), fieldChanges);
                    entityMasterData.MAWBOBLDate = entityPM.MAWBOBLDate;
                    
                    FieldChange.Add(entityMasterData.BookingConfirmationNotes, entityPM.BookingConfirmationNotes, nameof(entityPM.BookingConfirmationNotes), fieldChanges);
                    entityMasterData.BookingConfirmationNotes = entityPM.BookingConfirmationNotes;
                    
                    FieldChange.Add(entityMasterData.MainCarriageCarrierId, entityPM.MainCarriageCarrierId, nameof(entityPM.MainCarriageCarrierId), fieldChanges);
                    entityMasterData.MainCarriageCarrierId = entityPM.MainCarriageCarrierId;
                    
                    FieldChange.Add(entityMasterData.ManifestReason, entityPM.ManifestReason, nameof(entityPM.ManifestReason), fieldChanges);
                    entityMasterData.ManifestReason = entityPM.ManifestReason;
                    
                    FieldChange.Add(entityMasterData.ManifestStatusCode, entityPM.ManifestStatusCode, nameof(entityPM.ManifestStatusCode), fieldChanges);
                    entityMasterData.ManifestStatusCode = entityPM.ManifestStatusCode;
                    
                    FieldChange.Add(entityMasterData.AirlinePrefix, entityPM.AirlinePrefix, nameof(entityPM.AirlinePrefix), fieldChanges);
                    entityMasterData.AirlinePrefix = entityPM.AirlinePrefix;
                    
                    FieldChange.Add(entityMasterData.ProrateReceivables, entityPM.ProrateReceivables, nameof(entityPM.ProrateReceivables), fieldChanges);
                    entityMasterData.ProrateReceivables = entityPM.ProrateReceivables;

                    FieldChange.Add(entityMasterData.CutoffDate, entityPM.CutoffDate, nameof(entityPM.CutoffDate), fieldChanges);
                    entityMasterData.CutoffDate = entityPM.CutoffDate;

                    FieldChange.Add(entityMasterData.MainCarriageVesselId, entityPM.MainCarriageVesselId, nameof(entityPM.MainCarriageVesselId), fieldChanges);
                    entityMasterData.MainCarriageVesselId = entityPM.MainCarriageVesselId;
                    
                    FieldChange.Add(entityMasterData.MainCarriageVesselName, entityPM.MainCarriageVesselName, nameof(entityPM.MainCarriageVesselName), fieldChanges);
                    entityMasterData.MainCarriageVesselName = entityPM.MainCarriageVesselName;

                    FieldChange.Add(entityMasterData.MainCarriageIsFromStack, entityPM.MainCarriageIsFromStack, nameof(entityPM.MainCarriageIsFromStack), fieldChanges);
                    entityMasterData.MainCarriageIsFromStack = entityPM.MainCarriageIsFromStack;
                    
                    FieldChange.Add(entityMasterData.Transshipment1FromPortId, entityPM.Transshipment1FromPortId, nameof(entityPM.Transshipment1FromPortId), fieldChanges);
                    entityMasterData.Transshipment1FromPortId = entityPM.Transshipment1FromPortId;
                    
                    FieldChange.Add(entityMasterData.Transshipment1CarrierId, entityPM.Transshipment1CarrierId, nameof(entityPM.Transshipment1CarrierId), fieldChanges);
                    entityMasterData.Transshipment1CarrierId = entityPM.Transshipment1CarrierId;
                    
                    FieldChange.Add(entityMasterData.Transshipment1CarrierNumber, entityPM.Transshipment1CarrierNumber, nameof(entityPM.Transshipment1CarrierNumber), fieldChanges);
                    entityMasterData.Transshipment1CarrierNumber = entityPM.Transshipment1CarrierNumber;
                    
                    FieldChange.Add(entityMasterData.Transshipment1AdditionalMAWBOBLBL, entityPM.Transshipment1AdditionalMAWBOBLBL, nameof(entityPM.Transshipment1AdditionalMAWBOBLBL), fieldChanges);
                    entityMasterData.Transshipment1AdditionalMAWBOBLBL = entityPM.Transshipment1AdditionalMAWBOBLBL;
                    
                    FieldChange.Add(entityMasterData.Transshipment1VesselId, entityPM.Transshipment1VesselId, nameof(entityPM.Transshipment1VesselId), fieldChanges);
                    entityMasterData.Transshipment1VesselId = entityPM.Transshipment1VesselId;
                    
                    FieldChange.Add(entityMasterData.Transshipment1VesselName, entityPM.Transshipment1VesselName, nameof(entityPM.Transshipment1VesselName), fieldChanges);
                    entityMasterData.Transshipment1VesselName = entityPM.Transshipment1VesselName;
                    
                    FieldChange.Add(entityMasterData.Transshipment2FromPortId, entityPM.Transshipment2FromPortId, nameof(entityPM.Transshipment2FromPortId), fieldChanges);
                    entityMasterData.Transshipment2FromPortId = entityPM.Transshipment2FromPortId;
                    
                    FieldChange.Add(entityMasterData.Transshipment2CarrierId, entityPM.Transshipment2CarrierId, nameof(entityPM.Transshipment2CarrierId), fieldChanges);
                    entityMasterData.Transshipment2CarrierId = entityPM.Transshipment2CarrierId;
                    
                    FieldChange.Add(entityMasterData.Transshipment2CarrierNumber, entityPM.Transshipment2CarrierNumber, nameof(entityPM.Transshipment2CarrierNumber), fieldChanges);
                    entityMasterData.Transshipment2CarrierNumber = entityPM.Transshipment2CarrierNumber;
                    
                    FieldChange.Add(entityMasterData.Transshipment2AdditionalMAWBOBLBL, entityPM.Transshipment2AdditionalMAWBOBLBL, nameof(entityPM.Transshipment2AdditionalMAWBOBLBL), fieldChanges);
                    entityMasterData.Transshipment2AdditionalMAWBOBLBL = entityPM.Transshipment2AdditionalMAWBOBLBL;
                    
                    FieldChange.Add(entityMasterData.Transshipment2VesselId, entityPM.Transshipment2VesselId, nameof(entityPM.Transshipment2VesselId), fieldChanges);
                    entityMasterData.Transshipment2VesselId = entityPM.Transshipment2VesselId;
                    
                    FieldChange.Add(entityMasterData.Transshipment2VesselName, entityPM.Transshipment2VesselName, nameof(entityPM.Transshipment2VesselName), fieldChanges);
                    entityMasterData.Transshipment2VesselName = entityPM.Transshipment2VesselName;
                    
                    FieldChange.Add(entityMasterData.Transshipment3FromPortId, entityPM.Transshipment3FromPortId, nameof(entityPM.Transshipment3FromPortId), fieldChanges);
                    entityMasterData.Transshipment3FromPortId = entityPM.Transshipment3FromPortId;
                    
                    FieldChange.Add(entityMasterData.Transshipment3CarrierId, entityPM.Transshipment3CarrierId, nameof(entityPM.Transshipment3CarrierId), fieldChanges);
                    entityMasterData.Transshipment3CarrierId = entityPM.Transshipment3CarrierId;
                    
                    FieldChange.Add(entityMasterData.Transshipment3CarrierNumber, entityPM.Transshipment3CarrierNumber, nameof(entityPM.Transshipment3CarrierNumber), fieldChanges);
                    entityMasterData.Transshipment3CarrierNumber = entityPM.Transshipment3CarrierNumber;
                    
                    FieldChange.Add(entityMasterData.Transshipment3AdditionalMAWBOBLBL, entityPM.Transshipment3AdditionalMAWBOBLBL, nameof(entityPM.Transshipment3AdditionalMAWBOBLBL), fieldChanges);
                    entityMasterData.Transshipment3AdditionalMAWBOBLBL = entityPM.Transshipment3AdditionalMAWBOBLBL;
                    
                    FieldChange.Add(entityMasterData.Transshipment3VesselId, entityPM.Transshipment3VesselId, nameof(entityPM.Transshipment3VesselId), fieldChanges);
                    entityMasterData.Transshipment3VesselId = entityPM.Transshipment3VesselId;
                    
                    FieldChange.Add(entityMasterData.Transshipment3VesselName, entityPM.Transshipment3VesselName, nameof(entityPM.Transshipment3VesselName), fieldChanges);
                    entityMasterData.Transshipment3VesselName = entityPM.Transshipment3VesselName;
                    
                    FieldChange.Add(entityMasterData.OnCarriageAdditionalTransportModeCode, entityPM.OnCarriageAdditionalTransportModeCode, nameof(entityPM.OnCarriageAdditionalTransportModeCode), fieldChanges);
                    entityMasterData.OnCarriageAdditionalTransportModeCode = entityPM.OnCarriageAdditionalTransportModeCode;
                    
                    FieldChange.Add(entityMasterData.SplitOnCarriage, entityPM.SplitOnCarriage, nameof(entityPM.SplitOnCarriage), fieldChanges);
                    entityMasterData.SplitOnCarriage = entityPM.SplitOnCarriage;
                    
                    FieldChange.Add(entityMasterData.CarrierServiceLineId, entityPM.CarrierServiceLineId, nameof(entityPM.CarrierServiceLineId), fieldChanges);
                    entityMasterData.CarrierServiceLineId = entityPM.CarrierServiceLineId;
                    
                    FieldChange.Add(entityMasterData.NumberOfTransshipments, entityPM.NumberOfTransshipments, nameof(entityPM.NumberOfTransshipments), fieldChanges);
                    entityMasterData.NumberOfTransshipments = entityPM.NumberOfTransshipments;

                    if (entityPM.TransportModeId == "I")
                    {
                        FieldChange.Add(entityMasterData.TrailerNumber, entityPM.TrailerNumber, nameof(entityPM.TrailerNumber), fieldChanges);
                        entityMasterData.TrailerNumber = entityPM.TrailerNumber;
                        
                        FieldChange.Add(entityMasterData.Transshipment1TrailerNumber, entityPM.Transshipment1TrailerNumber, nameof(entityPM.Transshipment1TrailerNumber), fieldChanges);
                        entityMasterData.Transshipment1TrailerNumber = entityPM.Transshipment1TrailerNumber;
                        
                        FieldChange.Add(entityMasterData.Transshipment2TrailerNumber, entityPM.Transshipment2TrailerNumber, nameof(entityPM.Transshipment2TrailerNumber), fieldChanges);
                        entityMasterData.Transshipment2TrailerNumber = entityPM.Transshipment2TrailerNumber;
                        
                        FieldChange.Add(entityMasterData.Transshipment3TrailerNumber, entityPM.Transshipment3TrailerNumber, nameof(entityPM.Transshipment3TrailerNumber), fieldChanges);
                        entityMasterData.Transshipment3TrailerNumber = entityPM.Transshipment3TrailerNumber;
                    }

                    if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
                    {
                        FieldChange.Add(entityMasterData.MainCarriageFromPartnerId, entityPM.MainCarriageFromPartnerId, nameof(entityPM.MainCarriageFromPartnerId), fieldChanges);
                        entityMasterData.MainCarriageFromPartnerId = entityPM.MainCarriageFromPartnerId;
                        
                        FieldChange.Add(entityMasterData.MainCarriageFromAddressId, entityPM.MainCarriageFromAddressId, nameof(entityPM.MainCarriageFromAddressId), fieldChanges);
                        entityMasterData.MainCarriageFromAddressId = entityPM.MainCarriageFromAddressId;
                        
                        FieldChange.Add(entityMasterData.MainCarriageToPartnerId, entityPM.MainCarriageToPartnerId, nameof(entityPM.MainCarriageToPartnerId), fieldChanges);
                        entityMasterData.MainCarriageToPartnerId = entityPM.MainCarriageToPartnerId;
                        
                        FieldChange.Add(entityMasterData.MainCarriageToAddressId, entityPM.MainCarriageToAddressId, nameof(entityPM.MainCarriageToAddressId), fieldChanges);
                        entityMasterData.MainCarriageToAddressId = entityPM.MainCarriageToAddressId;
                        
                        FieldChange.Add(entityMasterData.Driver, entityPM.Driver, nameof(entityPM.Driver), fieldChanges);
                        entityMasterData.Driver = entityPM.Driver;
                        
                        FieldChange.Add(entityMasterData.TruckNumber, entityPM.TruckNumber, nameof(entityPM.TruckNumber), fieldChanges);
                        entityMasterData.TruckNumber = entityPM.TruckNumber;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticFromZipCode, entityPM.InlandDomesticFromZipCode, nameof(entityPM.InlandDomesticFromZipCode), fieldChanges);
                        entityMasterData.InlandDomesticFromZipCode = entityPM.InlandDomesticFromZipCode;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticToZipCode, entityPM.InlandDomesticToZipCode, nameof(entityPM.InlandDomesticToZipCode), fieldChanges);
                        entityMasterData.InlandDomesticToZipCode = entityPM.InlandDomesticToZipCode;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticFromCity, entityPM.InlandDomesticFromCity, nameof(entityPM.InlandDomesticFromCity), fieldChanges);
                        entityMasterData.InlandDomesticFromCity = entityPM.InlandDomesticFromCity;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticToCity, entityPM.InlandDomesticToCity, nameof(entityPM.InlandDomesticToCity), fieldChanges);
                        entityMasterData.InlandDomesticToCity = entityPM.InlandDomesticToCity;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticFromCountryId, entityPM.InlandDomesticFromCountryId, nameof(entityPM.InlandDomesticFromCountryId), fieldChanges);
                        entityMasterData.InlandDomesticFromCountryId = entityPM.InlandDomesticFromCountryId;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticToCountryId, entityPM.InlandDomesticToCountryId, nameof(entityPM.InlandDomesticToCountryId), fieldChanges);
                        entityMasterData.InlandDomesticToCountryId = entityPM.InlandDomesticToCountryId;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticFromTypeCode, entityPM.InlandDomesticFromTypeCode, nameof(entityPM.InlandDomesticFromTypeCode), fieldChanges);
                        entityMasterData.InlandDomesticFromTypeCode = entityPM.InlandDomesticFromTypeCode;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticToTypeCode, entityPM.InlandDomesticToTypeCode, nameof(entityPM.InlandDomesticToTypeCode), fieldChanges);
                        entityMasterData.InlandDomesticToTypeCode = entityPM.InlandDomesticToTypeCode;
                        
                        FieldChange.Add(entityMasterData.MainCarriageFromPortAddress, entityPM.MainCarriageFromPortAddress, nameof(entityPM.MainCarriageFromPortAddress), fieldChanges);
                        entityMasterData.MainCarriageFromPortAddress = entityPM.MainCarriageFromPortAddress;
                        
                        FieldChange.Add(entityMasterData.MainCarriageToPortAddress, entityPM.MainCarriageToPortAddress, nameof(entityPM.MainCarriageToPortAddress), fieldChanges);
                        entityMasterData.MainCarriageToPortAddress = entityPM.MainCarriageToPortAddress;
                        
                        FieldChange.Add(entityMasterData.MainCarriageFromPortId, entityPM.MainCarriageFromPortId, nameof(entityPM.MainCarriageFromPortId), fieldChanges);
                        entityMasterData.MainCarriageFromPortId = entityPM.MainCarriageFromPortId;
                        
                        FieldChange.Add(entityMasterData.MainCarriageToPortId, entityPM.MainCarriageToPortId, nameof(entityPM.MainCarriageToPortId), fieldChanges);
                        entityMasterData.MainCarriageToPortId = entityPM.MainCarriageToPortId;
                        
                        FieldChange.Add(entityMasterData.MainCarriageToPortId, entityPM.MainCarriageToPortId, nameof(entityPM.MainCarriageToPortId), fieldChanges);
                        entityMasterData.MainCarriageFinalDestinationPortId = entityPM.MainCarriageToPortId;

                        FieldChange.Add(entityMasterData.InlandDomesticToAddress1, entityPM.InlandDomesticToAddress1, nameof(entityPM.InlandDomesticToAddress1), fieldChanges);
                        entityMasterData.InlandDomesticToAddress1 = entityPM.InlandDomesticToAddress1;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticToAddress2, entityPM.InlandDomesticToAddress2, nameof(entityPM.InlandDomesticToAddress2), fieldChanges);
                        entityMasterData.InlandDomesticToAddress2 = entityPM.InlandDomesticToAddress2;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticToPhone, entityPM.InlandDomesticToPhone, nameof(entityPM.InlandDomesticToPhone), fieldChanges);
                        entityMasterData.InlandDomesticToPhone = entityPM.InlandDomesticToPhone;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticToFax, entityPM.InlandDomesticToFax, nameof(entityPM.InlandDomesticToFax), fieldChanges);
                        entityMasterData.InlandDomesticToFax = entityPM.InlandDomesticToFax;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticToStateId, entityPM.InlandDomesticToStateId, nameof(entityPM.InlandDomesticToStateId), fieldChanges);
                        entityMasterData.InlandDomesticToStateId = entityPM.InlandDomesticToStateId;

                        FieldChange.Add(entityMasterData.InlandDomesticFromAddress1, entityPM.InlandDomesticFromAddress1, nameof(entityPM.InlandDomesticFromAddress1), fieldChanges);
                        entityMasterData.InlandDomesticFromAddress1 = entityPM.InlandDomesticFromAddress1;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticFromAddress2, entityPM.InlandDomesticFromAddress2, nameof(entityPM.InlandDomesticFromAddress2), fieldChanges);
                        entityMasterData.InlandDomesticFromAddress2 = entityPM.InlandDomesticFromAddress2;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticFromPhone, entityPM.InlandDomesticFromPhone, nameof(entityPM.InlandDomesticFromPhone), fieldChanges);
                        entityMasterData.InlandDomesticFromPhone = entityPM.InlandDomesticFromPhone;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticFromFax, entityPM.InlandDomesticFromFax, nameof(entityPM.InlandDomesticFromFax), fieldChanges);
                        entityMasterData.InlandDomesticFromFax = entityPM.InlandDomesticFromFax;
                        
                        FieldChange.Add(entityMasterData.InlandDomesticFromStateId, entityPM.InlandDomesticFromStateId, nameof(entityPM.InlandDomesticFromStateId), fieldChanges);
                        entityMasterData.InlandDomesticFromStateId = entityPM.InlandDomesticFromStateId;
                    }

                    if (entityPM.TransportModeId == "A")
                    {
                        FieldChange.Add(entityMasterData.MainCarriageCarrierPrefix, entityPM.MainCarriageCarrierPrefix, nameof(entityPM.MainCarriageCarrierPrefix), fieldChanges);
                        entityMasterData.MainCarriageCarrierPrefix = entityPM.MainCarriageCarrierPrefix;
                        
                        FieldChange.Add(entityMasterData.Transshipment1CarrierPrefix, entityPM.Transshipment1CarrierPrefix, nameof(entityPM.Transshipment1CarrierPrefix), fieldChanges);
                        entityMasterData.Transshipment1CarrierPrefix = entityPM.Transshipment1CarrierPrefix;
                        
                        FieldChange.Add(entityMasterData.Transshipment2CarrierPrefix, entityPM.Transshipment2CarrierPrefix, nameof(entityPM.Transshipment2CarrierPrefix), fieldChanges);
                        entityMasterData.Transshipment2CarrierPrefix = entityPM.Transshipment2CarrierPrefix;
                        
                        FieldChange.Add(entityMasterData.Transshipment3CarrierPrefix, entityPM.Transshipment3CarrierPrefix, nameof(entityPM.Transshipment3CarrierPrefix), fieldChanges);
                        entityMasterData.Transshipment3CarrierPrefix = entityPM.Transshipment3CarrierPrefix;
                    }

                    FieldChange.Add(entityMasterData.IsKnownCargo, entityPM.IsKnownCargo, nameof(entityPM.IsKnownCargo), fieldChanges);
                    entityMasterData.IsKnownCargo = entityPM.IsKnownCargo;
                    
                    FieldChange.Add(entityMasterData.RegulatedAgentRANumber, entityPM.RegulatedAgentRANumber, nameof(entityPM.RegulatedAgentRANumber), fieldChanges);
                    entityMasterData.RegulatedAgentRANumber = entityPM.RegulatedAgentRANumber;
                    
                    FieldChange.Add(entityMasterData.KnownConsignorNumber, entityPM.KnownConsignorNumber, nameof(entityPM.KnownConsignorNumber), fieldChanges);
                    entityMasterData.KnownConsignorNumber = entityPM.KnownConsignorNumber;
                    
                    FieldChange.Add(entityMasterData.KCExpirationDate, entityPM.KCExpirationDate, nameof(entityPM.KCExpirationDate), fieldChanges);
                    entityMasterData.KCExpirationDate = entityPM.KCExpirationDate;
                    
                    FieldChange.Add(entityMasterData.ColoaderRANumber, entityPM.ColoaderRANumber, nameof(entityPM.ColoaderRANumber), fieldChanges);
                    entityMasterData.ColoaderRANumber = entityPM.ColoaderRANumber;
                    
                    FieldChange.Add(entityMasterData.AWBPrintingSecurityStatusId, entityPM.AWBPrintingSecurityStatusId, nameof(entityPM.AWBPrintingSecurityStatusId), fieldChanges);
                    entityMasterData.AWBPrintingSecurityStatusId = entityPM.AWBPrintingSecurityStatusId;
                    
                    FieldChange.Add(entityMasterData.AWBPrintingRANumber, entityPM.AWBPrintingRANumber, nameof(entityPM.AWBPrintingRANumber), fieldChanges);
                    entityMasterData.AWBPrintingRANumber = entityPM.AWBPrintingRANumber;
                    
                    FieldChange.Add(entityMasterData.AdditionalHandlingInfo, entityPM.AdditionalHandlingInfo, nameof(entityPM.AdditionalHandlingInfo), fieldChanges);
                    entityMasterData.AdditionalHandlingInfo = entityPM.AdditionalHandlingInfo;
                    
                    FieldChange.Add(entityMasterData.AWBPrintingSecurityStatusEdited, entityPM.AWBPrintingSecurityStatusEdited, nameof(entityPM.AWBPrintingSecurityStatusEdited), fieldChanges);
                    entityMasterData.AWBPrintingSecurityStatusEdited = entityPM.AWBPrintingSecurityStatusEdited;
                    
                    FieldChange.Add(entityMasterData.AWBPrintingRANumberEdited, entityPM.AWBPrintingRANumberEdited, nameof(entityPM.AWBPrintingRANumberEdited), fieldChanges);
                    entityMasterData.AWBPrintingRANumberEdited = entityPM.AWBPrintingRANumberEdited;
                    
                    FieldChange.Add(entityMasterData.AdditionalHandlingInfoEdited, entityPM.AdditionalHandlingInfoEdited, nameof(entityPM.AdditionalHandlingInfoEdited), fieldChanges);
                    entityMasterData.AdditionalHandlingInfoEdited = entityPM.AdditionalHandlingInfoEdited;
                    
                    FieldChange.Add(entityMasterData.InterlineId, entityPM.InterlineId, nameof(entityPM.InterlineId), fieldChanges);
                    entityMasterData.InterlineId = entityPM.InterlineId;
                    
                    ComputeDepartureArrivalDates(entityMasterData, entityPM);
                    ComputeMainCarriageFinalDestinationDates(entityMasterData, entityPM);

                    FieldChange.Add(entityMasterData.MainCarriageFromPortId, entityPM.MainCarriageFromPortId, nameof(entityPM.MainCarriageFromPortId), fieldChanges);
                    entityPM.OriginMainCarriageFromPortId = entityMasterData.MainCarriageFromPortId;
                    
                    FieldChange.Add(entityMasterData.MainCarriageFinalDestinationPortId, entityPM.MainCarriageFinalDestinationPortId, nameof(entityPM.MainCarriageFinalDestinationPortId), fieldChanges);
                    entityPM.OriginFinalDestinationPortId = entityMasterData.MainCarriageFinalDestinationPortId;
                    
                    FieldChange.Add(entityMasterData.PreCarriageFromPortId, entityPM.PreCarriageFromPortId, nameof(entityPM.PreCarriageFromPortId), fieldChanges);
                    entityPM.OriginPreCarriageFromPortId = entityMasterData.PreCarriageFromPortId;
                    
                    FieldChange.Add(entityMasterData.OnCarriageToPortId, entityPM.OnCarriageToPortId, nameof(entityPM.OnCarriageToPortId), fieldChanges);
                    entityPM.OriginOnCarriageToPortId = entityMasterData.OnCarriageToPortId;                    
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
        }
        public static void ComputeMainCarriageFinalDestinationDates(ShipmentMasterData entityMasterData, ShipmentPM entityPM, bool isChangeShipmetPMOnly = false)
        {
            DateTime? to_ETA = null;
            DateTime? to_ATA = null;

            if (entityPM.Transshipment3ToPortId != null)
            {
                to_ETA = entityPM.Transshipment3ETA;
                to_ATA = entityPM.Transshipment3ATA;
            }

            else if (entityPM.Transshipment2ToPortId != null)
            {
                to_ETA = entityPM.Transshipment2ETA;
                to_ATA = entityPM.Transshipment2ATA;
            }

            else if (entityPM.Transshipment1ToPortId != null)
            {
                to_ETA = entityPM.Transshipment1ETA;
                to_ATA = entityPM.Transshipment1ATA;
            }

            else
            {
                to_ETA = entityPM.MainCarriageETA;
                to_ATA = entityPM.MainCarriageATA;
            }

            entityPM.MainCarriageFinalDestinationETA = to_ETA;
            entityPM.MainCarriageFinalDestinationATA = to_ATA;

            if (isChangeShipmetPMOnly) return;
            entityMasterData.MainCarriageFinalDestinationETA = entityPM.MainCarriageFinalDestinationETA;
            entityMasterData.MainCarriageFinalDestinationATA = entityPM.MainCarriageFinalDestinationATA;
        }

        private static void MapRoutings(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, 
                                        bool isNewEntity, List<FieldChange> fieldChanges)
        {
            if (entityPM.ShipmentLevelCode != "H")
            {
                if (entityMasterData != null)
                {
                    MapPreOnCarriage(entityPM, entityMasterData, fieldChanges);

                    if(entityPM.ConvertFromHouseToDirect)
                    {
                        MapPreOnForwarding(entityPM, entityPoco, fieldChanges);
                    }                    
                }
            }

            else
            {
                MapPreOnForwarding(entityPM, entityPoco, fieldChanges);

                if (entityPM.ConvertFromDirectToHouse)
                {
                    MapPreOnCarriage(entityPM, entityMasterData, fieldChanges);
                }                
            }
        }
        private static void MapPreOnCarriage(ShipmentPM entityPM, ShipmentMasterData entityMasterData, List<FieldChange> fieldChanges)
        {
            FieldChange.Add(entityMasterData.PreCarriageFromPortId, entityPM.PreCarriageFromPortId, nameof(entityPM.PreCarriageFromPortId), fieldChanges);
            entityMasterData.PreCarriageFromPortId = entityPM.PreCarriageFromPortId;
            
            FieldChange.Add(entityMasterData.PreCarriageToPortId, entityPM.PreCarriageToPortId, nameof(entityPM.PreCarriageToPortId), fieldChanges);
            entityMasterData.PreCarriageToPortId = entityPM.PreCarriageToPortId;
            
            FieldChange.Add(entityMasterData.PreCarriageCarrierId, entityPM.PreCarriageCarrierId, nameof(entityPM.PreCarriageCarrierId), fieldChanges);
            entityMasterData.PreCarriageCarrierId = entityPM.PreCarriageCarrierId;
            
            FieldChange.Add(entityMasterData.PreCarriageCarrierNumber, entityPM.PreCarriageCarrierNumber, nameof(entityPM.PreCarriageCarrierNumber), fieldChanges);
            entityMasterData.PreCarriageCarrierNumber = entityPM.PreCarriageCarrierNumber;
            
            FieldChange.Add(entityMasterData.PreCarriageVesselId, entityPM.PreCarriageVesselId, nameof(entityPM.PreCarriageVesselId), fieldChanges);
            entityMasterData.PreCarriageVesselId = entityPM.PreCarriageVesselId;
            
            FieldChange.Add(entityMasterData.PreCarriageVesselName, entityPM.PreCarriageVesselName, nameof(entityPM.PreCarriageVesselName), fieldChanges);
            entityMasterData.PreCarriageVesselName = entityPM.PreCarriageVesselName;
            
            FieldChange.Add(entityMasterData.PreCarriageTransportModeId, entityPM.PreCarriageTransportModeId, nameof(entityPM.PreCarriageTransportModeId), fieldChanges);
            entityMasterData.PreCarriageTransportModeId = entityPM.PreCarriageTransportModeId;
            
            FieldChange.Add(entityMasterData.OnCarriageFromPortId, entityPM.OnCarriageFromPortId, nameof(entityPM.OnCarriageFromPortId), fieldChanges);
            entityMasterData.OnCarriageFromPortId = entityPM.OnCarriageFromPortId;
            
            FieldChange.Add(entityMasterData.OnCarriageToPortId, entityPM.OnCarriageToPortId, nameof(entityPM.OnCarriageToPortId), fieldChanges);
            entityMasterData.OnCarriageToPortId = entityPM.OnCarriageToPortId;
            
            FieldChange.Add(entityMasterData.OnCarriageCarrierId, entityPM.OnCarriageCarrierId, nameof(entityPM.OnCarriageCarrierId), fieldChanges);
            entityMasterData.OnCarriageCarrierId = entityPM.OnCarriageCarrierId;
            
            FieldChange.Add(entityMasterData.OnCarriageCarrierNumber, entityPM.OnCarriageCarrierNumber, nameof(entityPM.OnCarriageCarrierNumber), fieldChanges);
            entityMasterData.OnCarriageCarrierNumber = entityPM.OnCarriageCarrierNumber;
            
            FieldChange.Add(entityMasterData.OnCarriageVesselId, entityPM.OnCarriageVesselId, nameof(entityPM.OnCarriageVesselId), fieldChanges);
            entityMasterData.OnCarriageVesselId = entityPM.OnCarriageVesselId;
            
            FieldChange.Add(entityMasterData.OnCarriageVesselName, entityPM.OnCarriageVesselName, nameof(entityPM.OnCarriageVesselName), fieldChanges);
            entityMasterData.OnCarriageVesselName = entityPM.OnCarriageVesselName;
            
            FieldChange.Add(entityMasterData.OnCarriageTransportModeId, entityPM.OnCarriageTransportModeId, nameof(entityPM.OnCarriageTransportModeId), fieldChanges);
            entityMasterData.OnCarriageTransportModeId = entityPM.OnCarriageTransportModeId;
        }

        private static void MapPreOnForwarding(ShipmentPM entityPM, Shipment entityPoco, List<FieldChange> fieldChanges)
        {
            FieldChange.Add(entityPoco.PreForwardingFromPortId, entityPM.PreForwardingFromPortId, nameof(entityPM.PreForwardingFromPortId), fieldChanges);
            entityPoco.PreForwardingFromPortId = entityPM.PreForwardingFromPortId;
            
            FieldChange.Add(entityPoco.PreForwardingToPortId, entityPM.PreForwardingToPortId, nameof(entityPM.PreForwardingToPortId), fieldChanges);
            entityPoco.PreForwardingToPortId = entityPM.PreForwardingToPortId;
            
            FieldChange.Add(entityPoco.PreForwardingCarrierId, entityPM.PreForwardingCarrierId, nameof(entityPM.PreForwardingCarrierId), fieldChanges);
            entityPoco.PreForwardingCarrierId = entityPM.PreForwardingCarrierId;
            
            FieldChange.Add(entityPoco.PreForwardingCarrierNumber, entityPM.PreForwardingCarrierNumber, nameof(entityPM.PreForwardingCarrierNumber), fieldChanges);
            entityPoco.PreForwardingCarrierNumber = entityPM.PreForwardingCarrierNumber;
            
            FieldChange.Add(entityPoco.PreForwardingVesselId, entityPM.PreForwardingVesselId, nameof(entityPM.PreForwardingVesselId), fieldChanges);
            entityPoco.PreForwardingVesselId = entityPM.PreForwardingVesselId;
            
            FieldChange.Add(entityPoco.PreForwardingVesselName, entityPM.PreForwardingVesselName, nameof(entityPM.PreForwardingVesselName), fieldChanges);
            entityPoco.PreForwardingVesselName = entityPM.PreForwardingVesselName;
            
            FieldChange.Add(entityPoco.PreForwardingTransportModeId, entityPM.PreForwardingTransportModeId, nameof(entityPM.PreForwardingTransportModeId), fieldChanges);
            entityPoco.PreForwardingTransportModeId = entityPM.PreForwardingTransportModeId;
            
            FieldChange.Add(entityPoco.OnForwardingFromPortId, entityPM.OnForwardingFromPortId, nameof(entityPM.OnForwardingFromPortId), fieldChanges);
            entityPoco.OnForwardingFromPortId = entityPM.OnForwardingFromPortId;
            
            FieldChange.Add(entityPoco.OnForwardingToPortId, entityPM.OnForwardingToPortId, nameof(entityPM.OnForwardingToPortId), fieldChanges);
            entityPoco.OnForwardingToPortId = entityPM.OnForwardingToPortId;
            
            FieldChange.Add(entityPoco.OnForwardingCarrierId, entityPM.OnForwardingCarrierId, nameof(entityPM.OnForwardingCarrierId), fieldChanges);
            entityPoco.OnForwardingCarrierId = entityPM.OnForwardingCarrierId;
            
            FieldChange.Add(entityPoco.OnForwardingCarrierNumber, entityPM.OnForwardingCarrierNumber, nameof(entityPM.OnForwardingCarrierNumber), fieldChanges);
            entityPoco.OnForwardingCarrierNumber = entityPM.OnForwardingCarrierNumber;
            
            FieldChange.Add(entityPoco.OnForwardingVesselId, entityPM.OnForwardingVesselId, nameof(entityPM.OnForwardingVesselId), fieldChanges);
            entityPoco.OnForwardingVesselId = entityPM.OnForwardingVesselId;
            
            FieldChange.Add(entityPoco.OnForwardingVesselName, entityPM.OnForwardingVesselName, nameof(entityPM.OnForwardingVesselName), fieldChanges);
            entityPoco.OnForwardingVesselName = entityPM.OnForwardingVesselName;
            
            FieldChange.Add(entityPoco.OnForwardingTransportModeId, entityPM.OnForwardingTransportModeId, nameof(entityPM.OnForwardingTransportModeId), fieldChanges);
            entityPoco.OnForwardingTransportModeId = entityPM.OnForwardingTransportModeId;
        }
        private static void MapPartners(ShipmentPM entityPM, Shipment entityPoco, bool isNewEntity, List<FieldChange> fieldChanges)
        {
            FieldChange.Add(entityPoco.ShipmentCustomerTypeCode, entityPM.ShipmentCustomerTypeCode, nameof(entityPM.ShipmentCustomerTypeCode), fieldChanges);
            entityPoco.ShipmentCustomerTypeCode = entityPM.ShipmentCustomerTypeCode;
            
            FieldChange.Add(entityPoco.ConsigneeAddressOneTime, entityPM.ConsigneeAddressOneTime, nameof(entityPM.ConsigneeAddressOneTime), fieldChanges);
            entityPoco.ConsigneeAddressOneTime = entityPM.ConsigneeAddressOneTime;
            
            FieldChange.Add(entityPoco.ShipperAddressOneTime, entityPM.ShipperAddressOneTime, nameof(entityPM.ShipperAddressOneTime), fieldChanges);
            entityPoco.ShipperAddressOneTime = entityPM.ShipperAddressOneTime;

            FieldChange.Add(entityPoco.CustomerId, entityPM.CustomerId, nameof(entityPM.CustomerId), fieldChanges);
            entityPoco.CustomerId = entityPM.CustomerId;
            
            FieldChange.Add(entityPoco.CustomerAddressId, entityPM.CustomerAddressId, nameof(entityPM.CustomerAddressId), fieldChanges);
            entityPoco.CustomerAddressId = entityPM.CustomerAddressId;
            
            FieldChange.Add(entityPoco.CustomerContactId, entityPM.CustomerContactId, nameof(entityPM.CustomerContactId), fieldChanges);
            entityPoco.CustomerContactId = entityPM.CustomerContactId;
            
            FieldChange.Add(entityPoco.CustomerReference1, entityPM.CustomerReference1, nameof(entityPM.CustomerReference1), fieldChanges);
            entityPoco.CustomerReference1 = entityPM.CustomerReference1;
            
            FieldChange.Add(entityPoco.CustomerReference2, entityPM.CustomerReference2, nameof(entityPM.CustomerReference2), fieldChanges);
            entityPoco.CustomerReference2 = entityPM.CustomerReference2;
            
            FieldChange.Add(entityPoco.CustomerReference3, entityPM.CustomerReference3, nameof(entityPM.CustomerReference3), fieldChanges);
            entityPoco.CustomerReference3 = entityPM.CustomerReference3;

            FieldChange.Add(entityPoco.FreightForwarderId, entityPM.FreightForwarderId, nameof(entityPM.FreightForwarderId), fieldChanges);
            entityPoco.FreightForwarderId = entityPM.FreightForwarderId;
            
            FieldChange.Add(entityPoco.FreightForwarderAddressId, entityPM.FreightForwarderAddressId, nameof(entityPM.FreightForwarderAddressId), fieldChanges);
            entityPoco.FreightForwarderAddressId = entityPM.FreightForwarderAddressId;
            
            FieldChange.Add(entityPoco.FreightForwarderContactId, entityPM.FreightForwarderContactId, nameof(entityPM.FreightForwarderContactId), fieldChanges);
            entityPoco.FreightForwarderContactId = entityPM.FreightForwarderContactId;
            
            FieldChange.Add(entityPoco.FreightForwarderReference, entityPM.FreightForwarderReference, nameof(entityPM.FreightForwarderReference), fieldChanges);
            entityPoco.FreightForwarderReference = entityPM.FreightForwarderReference;

            FieldChange.Add(entityPoco.ShipperId, entityPM.ShipperId, nameof(entityPM.ShipperId), fieldChanges);
            entityPoco.ShipperId = entityPM.ShipperId;
            
            FieldChange.Add(entityPoco.ShipperAddressId, entityPM.ShipperAddressId, nameof(entityPM.ShipperAddressId), fieldChanges);
            entityPoco.ShipperAddressId = entityPM.ShipperAddressId;
            
            FieldChange.Add(entityPoco.ShipperContactId, entityPM.ShipperContactId, nameof(entityPM.ShipperContactId), fieldChanges);
            entityPoco.ShipperContactId = entityPM.ShipperContactId;
            
            FieldChange.Add(entityPoco.ShipperReference1, entityPM.ShipperReference1, nameof(entityPM.ShipperReference1), fieldChanges);
            entityPoco.ShipperReference1 = entityPM.ShipperReference1;
            
            FieldChange.Add(entityPoco.ShipperReference2, entityPM.ShipperReference2, nameof(entityPM.ShipperReference2), fieldChanges);
            entityPoco.ShipperReference2 = entityPM.ShipperReference2;
            
            FieldChange.Add(entityPoco.ShipperReference3, entityPM.ShipperReference3, nameof(entityPM.ShipperReference3), fieldChanges);
            entityPoco.ShipperReference3 = entityPM.ShipperReference3;

            FieldChange.Add(entityPoco.ConsigneeId, entityPM.ConsigneeId, nameof(entityPM.ConsigneeId), fieldChanges);
            entityPoco.ConsigneeId = entityPM.ConsigneeId;
            
            FieldChange.Add(entityPoco.ConsigneeAddressId, entityPM.ConsigneeAddressId, nameof(entityPM.ConsigneeAddressId), fieldChanges);
            entityPoco.ConsigneeAddressId = entityPM.ConsigneeAddressId;
            
            FieldChange.Add(entityPoco.ConsigneeContactId, entityPM.ConsigneeContactId, nameof(entityPM.ConsigneeContactId), fieldChanges);
            entityPoco.ConsigneeContactId = entityPM.ConsigneeContactId;
            
            FieldChange.Add(entityPoco.ConsigneeReference1, entityPM.ConsigneeReference1, nameof(entityPM.ConsigneeReference1), fieldChanges);
            entityPoco.ConsigneeReference1 = entityPM.ConsigneeReference1;
            
            FieldChange.Add(entityPoco.ConsigneeReference2, entityPM.ConsigneeReference2, nameof(entityPM.ConsigneeReference2), fieldChanges);
            entityPoco.ConsigneeReference2 = entityPM.ConsigneeReference2;
            
            FieldChange.Add(entityPoco.ConsigneeReference3, entityPM.ConsigneeReference3, nameof(entityPM.ConsigneeReference3), fieldChanges);
            entityPoco.ConsigneeReference3 = entityPM.ConsigneeReference3;

            FieldChange.Add(entityPoco.AgentId, entityPM.AgentId, nameof(entityPM.AgentId), fieldChanges);
            entityPoco.AgentId = entityPM.AgentId;


            FieldChange.Add(entityPoco.AgentAddressId, entityPM.AgentAddressId, nameof(entityPM.AgentAddressId), fieldChanges);
            entityPoco.AgentAddressId = entityPM.AgentAddressId;
            
            FieldChange.Add(entityPoco.AgentContactId, entityPM.AgentContactId, nameof(entityPM.AgentContactId), fieldChanges);
            entityPoco.AgentContactId = entityPM.AgentContactId;
            
            FieldChange.Add(entityPoco.AgentReference1, entityPM.AgentReference1, nameof(entityPM.AgentReference1), fieldChanges);
            entityPoco.AgentReference1 = entityPM.AgentReference1;
            
            FieldChange.Add(entityPoco.AgentReference2, entityPM.AgentReference2, nameof(entityPM.AgentReference2), fieldChanges);
            entityPoco.AgentReference2 = entityPM.AgentReference2;

            FieldChange.Add(entityPoco.IssuingCarrierAgentId, entityPM.IssuingCarrierAgentId, nameof(entityPM.IssuingCarrierAgentId), fieldChanges);
            entityPoco.IssuingCarrierAgentId = entityPM.IssuingCarrierAgentId;
            
            FieldChange.Add(entityPoco.IssuingCarrierAddressId, entityPM.IssuingCarrierAddressId, nameof(entityPM.IssuingCarrierAddressId), fieldChanges);
            entityPoco.IssuingCarrierAddressId = entityPM.IssuingCarrierAddressId;
            
            FieldChange.Add(entityPoco.IssuingCarrierIATACode, entityPM.IssuingCarrierIATACode, nameof(entityPM.IssuingCarrierIATACode), fieldChanges);
            entityPoco.IssuingCarrierIATACode = entityPM.IssuingCarrierIATACode;

            FieldChange.Add(entityPoco.CustomAgentExportId, entityPM.CustomAgentExportId, nameof(entityPM.CustomAgentExportId), fieldChanges);
            entityPoco.CustomAgentExportId = entityPM.CustomAgentExportId;
            
            FieldChange.Add(entityPoco.CustomAgentExportAddressId, entityPM.CustomAgentExportAddressId, nameof(entityPM.CustomAgentExportAddressId), fieldChanges);
            entityPoco.CustomAgentExportAddressId = entityPM.CustomAgentExportAddressId;
            
            FieldChange.Add(entityPoco.CustomAgentExportContactId, entityPM.CustomAgentExportContactId, nameof(entityPM.CustomAgentExportContactId), fieldChanges);
            entityPoco.CustomAgentExportContactId = entityPM.CustomAgentExportContactId;
            
            FieldChange.Add(entityPoco.CustomAgentExportReference, entityPM.CustomAgentExportReference, nameof(entityPM.CustomAgentExportReference), fieldChanges);
            entityPoco.CustomAgentExportReference = entityPM.CustomAgentExportReference;

            FieldChange.Add(entityPoco.CustomAgentImportId, entityPM.CustomAgentImportId, nameof(entityPM.CustomAgentImportId), fieldChanges);
            entityPoco.CustomAgentImportId = entityPM.CustomAgentImportId;
            
            FieldChange.Add(entityPoco.CustomAgentImportAddressId, entityPM.CustomAgentImportAddressId, nameof(entityPM.CustomAgentImportAddressId), fieldChanges);
            entityPoco.CustomAgentImportAddressId = entityPM.CustomAgentImportAddressId;
            
            FieldChange.Add(entityPoco.CustomAgentImportContactId, entityPM.CustomAgentImportContactId, nameof(entityPM.CustomAgentImportContactId), fieldChanges);
            entityPoco.CustomAgentImportContactId = entityPM.CustomAgentImportContactId;
            
            FieldChange.Add(entityPoco.CustomAgentImportReference, entityPM.CustomAgentImportReference, nameof(entityPM.CustomAgentImportReference), fieldChanges);
            entityPoco.CustomAgentImportReference = entityPM.CustomAgentImportReference;

            FieldChange.Add(entityPoco.Notify1Id, entityPM.Notify1Id, nameof(entityPM.Notify1Id), fieldChanges);
            entityPoco.Notify1Id = entityPM.Notify1Id;
            
            FieldChange.Add(entityPoco.Notify1AddressId, entityPM.Notify1AddressId, nameof(entityPM.Notify1AddressId), fieldChanges);
            entityPoco.Notify1AddressId = entityPM.Notify1AddressId;
            
            FieldChange.Add(entityPoco.Notify1ContactId, entityPM.Notify1ContactId, nameof(entityPM.Notify1ContactId), fieldChanges);
            entityPoco.Notify1ContactId = entityPM.Notify1ContactId;

            FieldChange.Add(entityPoco.Notify2Id, entityPM.Notify2Id, nameof(entityPM.Notify2Id), fieldChanges);
            entityPoco.Notify2Id = entityPM.Notify2Id;
            
            FieldChange.Add(entityPoco.Notify2AddressId, entityPM.Notify2AddressId, nameof(entityPM.Notify2AddressId), fieldChanges);
            entityPoco.Notify2AddressId = entityPM.Notify2AddressId;
            
            FieldChange.Add(entityPoco.Notify2ContactId, entityPM.Notify2ContactId, nameof(entityPM.Notify2ContactId), fieldChanges);
            entityPoco.Notify2ContactId = entityPM.Notify2ContactId;

            FieldChange.Add(entityPoco.ShipperNotExporterId, entityPM.ShipperNotExporterId, nameof(entityPM.ShipperNotExporterId), fieldChanges);
            entityPoco.ShipperNotExporterId = entityPM.ShipperNotExporterId;
            
            FieldChange.Add(entityPoco.ShipperNotExporterAddressId, entityPM.ShipperNotExporterAddressId, nameof(entityPM.ShipperNotExporterAddressId), fieldChanges);
            entityPoco.ShipperNotExporterAddressId = entityPM.ShipperNotExporterAddressId;
            
            FieldChange.Add(entityPoco.ShipperNotExporterContactId, entityPM.ShipperNotExporterContactId, nameof(entityPM.ShipperNotExporterContactId), fieldChanges);
            entityPoco.ShipperNotExporterContactId = entityPM.ShipperNotExporterContactId;

            FieldChange.Add(entityPoco.ConsigneeNotImporterId, entityPM.ConsigneeNotImporterId, nameof(entityPM.ConsigneeNotImporterId), fieldChanges);
            entityPoco.ConsigneeNotImporterId = entityPM.ConsigneeNotImporterId;
            
            FieldChange.Add(entityPoco.ConsigneeNotImporterAddressId, entityPM.ConsigneeNotImporterAddressId, nameof(entityPM.ConsigneeNotImporterAddressId), fieldChanges);
            entityPoco.ConsigneeNotImporterAddressId = entityPM.ConsigneeNotImporterAddressId;
            
            FieldChange.Add(entityPoco.ConsigneeNotImporterContactId, entityPM.ConsigneeNotImporterContactId, nameof(entityPM.ConsigneeNotImporterContactId), fieldChanges);
            entityPoco.ConsigneeNotImporterContactId = entityPM.ConsigneeNotImporterContactId;

            FieldChange.Add(entityPoco.ColoaderId, entityPM.ColoaderId, nameof(entityPM.ColoaderId), fieldChanges);
            entityPoco.ColoaderId = entityPM.ColoaderId;
            
            FieldChange.Add(entityPoco.ColoaderAddressId, entityPM.ColoaderAddressId, nameof(entityPM.ColoaderAddressId), fieldChanges);
            entityPoco.ColoaderAddressId = entityPM.ColoaderAddressId;
            
            FieldChange.Add(entityPoco.ColoaderContactId, entityPM.ColoaderContactId, nameof(entityPM.ColoaderContactId), fieldChanges);
            entityPoco.ColoaderContactId = entityPM.ColoaderContactId;
            
            FieldChange.Add(entityPoco.ColoaderReference1, entityPM.ColoaderReference1, nameof(entityPM.ColoaderReference1), fieldChanges);
            entityPoco.ColoaderReference1 = entityPM.ColoaderReference1;

            FieldChange.Add(entityPoco.CustomClearancePointId, entityPM.CustomClearancePointId, nameof(entityPM.CustomClearancePointId), fieldChanges);
            entityPoco.CustomClearancePointId = entityPM.CustomClearancePointId;
            
            FieldChange.Add(entityPoco.CustomClearancePointAddressId, entityPM.CustomClearancePointAddressId, nameof(entityPM.CustomClearancePointAddressId), fieldChanges);
            entityPoco.CustomClearancePointAddressId = entityPM.CustomClearancePointAddressId;
            
            FieldChange.Add(entityPoco.CustomClearancePointContactId, entityPM.CustomClearancePointContactId, nameof(entityPM.CustomClearancePointContactId), fieldChanges);
            entityPoco.CustomClearancePointContactId = entityPM.CustomClearancePointContactId;
            
            FieldChange.Add(entityPoco.CustomClearancePointReference1, entityPM.CustomClearancePointReference1, nameof(entityPM.CustomClearancePointReference1), fieldChanges);
            entityPoco.CustomClearancePointReference1 = entityPM.CustomClearancePointReference1;


            FieldChange.Add(entityPoco.ConsolidatorId, entityPM.ConsolidatorId, nameof(entityPM.ConsolidatorId), fieldChanges);
            entityPoco.ConsolidatorId = entityPM.ConsolidatorId;
            
            FieldChange.Add(entityPoco.ConsolidatorAddressId, entityPM.ConsolidatorAddressId, nameof(entityPM.ConsolidatorAddressId), fieldChanges);
            entityPoco.ConsolidatorAddressId = entityPM.ConsolidatorAddressId;
            
            FieldChange.Add(entityPoco.ConsolidatorContactId, entityPM.ConsolidatorContactId, nameof(entityPM.ConsolidatorContactId), fieldChanges);
            entityPoco.ConsolidatorContactId = entityPM.ConsolidatorContactId;
            
            FieldChange.Add(entityPoco.ConsolidatorReference, entityPM.ConsolidatorReference, nameof(entityPM.ConsolidatorReference), fieldChanges);
            entityPoco.ConsolidatorReference = entityPM.ConsolidatorReference;

            FieldChange.Add(entityPoco.ReleasingAgentId, entityPM.ReleasingAgentId, nameof(entityPM.ReleasingAgentId), fieldChanges);
            entityPoco.ReleasingAgentId = entityPM.ReleasingAgentId;
            
            FieldChange.Add(entityPoco.ReleasingAgentAddressId, entityPM.ReleasingAgentAddressId, nameof(entityPM.ReleasingAgentAddressId), fieldChanges);
            entityPoco.ReleasingAgentAddressId = entityPM.ReleasingAgentAddressId;
            
            FieldChange.Add(entityPoco.ReleasingAgentContactId, entityPM.ReleasingAgentContactId, nameof(entityPM.ReleasingAgentContactId), fieldChanges);
            entityPoco.ReleasingAgentContactId = entityPM.ReleasingAgentContactId;
            
            FieldChange.Add(entityPoco.ReleasingAgentReference1, entityPM.ReleasingAgentReference1, nameof(entityPM.ReleasingAgentReference1), fieldChanges);
            entityPoco.ReleasingAgentReference1 = entityPM.ReleasingAgentReference1;
            
            FieldChange.Add(entityPoco.ReleasingAgentReference2, entityPM.ReleasingAgentReference2, nameof(entityPM.ReleasingAgentReference2), fieldChanges);
            entityPoco.ReleasingAgentReference2 = entityPM.ReleasingAgentReference2;
        }

        private static void MapAWBFields(ShipmentPM entityPM, Shipment entityPoco, bool isNewEntity, List<FieldChange> fieldChanges)
        {
            FieldChange.Add(entityPoco.AWBFreightAmountCollect, entityPM.AWBFreightAmountCollect, nameof(entityPM.AWBFreightAmountCollect), fieldChanges);
            entityPoco.AWBFreightAmountCollect = entityPM.AWBFreightAmountCollect;
            
            FieldChange.Add(entityPoco.AWBFreightAmountPrepaid, entityPM.AWBFreightAmountPrepaid, nameof(entityPM.AWBFreightAmountPrepaid), fieldChanges);
            entityPoco.AWBFreightAmountPrepaid = entityPM.AWBFreightAmountPrepaid;
            
            FieldChange.Add(entityPoco.AWBCurrencyId, entityPM.AWBCurrencyId, nameof(entityPM.AWBCurrencyId), fieldChanges);
            entityPoco.AWBCurrencyId = entityPM.AWBCurrencyId;
            
            FieldChange.Add(entityPoco.AWBAccountingInformation, entityPM.AWBAccountingInformation, nameof(entityPM.AWBAccountingInformation), fieldChanges);
            entityPoco.AWBAccountingInformation = entityPM.AWBAccountingInformation;
            
            FieldChange.Add(entityPoco.AWBCarrierTarrifReference, entityPM.AWBCarrierTarrifReference, nameof(entityPM.AWBCarrierTarrifReference), fieldChanges);
            entityPoco.AWBCarrierTarrifReference = entityPM.AWBCarrierTarrifReference;
            
            FieldChange.Add(entityPoco.AWBDeclaredValueForCarriage, entityPM.AWBDeclaredValueForCarriage, nameof(entityPM.AWBDeclaredValueForCarriage), fieldChanges);
            entityPoco.AWBDeclaredValueForCarriage = entityPM.AWBDeclaredValueForCarriage;
            
            FieldChange.Add(entityPoco.AWBDeclaredValueForCustoms, entityPM.AWBDeclaredValueForCustoms, nameof(entityPM.AWBDeclaredValueForCustoms), fieldChanges);
            entityPoco.AWBDeclaredValueForCustoms = entityPM.AWBDeclaredValueForCustoms;
            
            FieldChange.Add(entityPoco.AWBInsurrenceValue, entityPM.AWBInsurrenceValue, nameof(entityPM.AWBInsurrenceValue), fieldChanges);
            entityPoco.AWBInsurrenceValue = entityPM.AWBInsurrenceValue;
            
            FieldChange.Add(entityPoco.AWBHandlingInformation, entityPM.AWBHandlingInformation, nameof(entityPM.AWBHandlingInformation), fieldChanges);
            entityPoco.AWBHandlingInformation = entityPM.AWBHandlingInformation;
            
            FieldChange.Add(entityPoco.SCI, entityPM.SCI, nameof(entityPM.SCI), fieldChanges);
            entityPoco.SCI = entityPM.SCI;
            
            FieldChange.Add(entityPoco.AWBComments, entityPM.AWBComments, nameof(entityPM.AWBComments), fieldChanges);
            entityPoco.AWBComments = entityPM.AWBComments;
            
            FieldChange.Add(entityPoco.AWBPrintingComments, entityPM.AWBPrintingComments, nameof(entityPM.AWBPrintingComments), fieldChanges);
            entityPoco.AWBPrintingComments = entityPM.AWBPrintingComments;
            
            FieldChange.Add(entityPoco.AWBSignature, entityPM.AWBSignature, nameof(entityPM.AWBSignature), fieldChanges);
            entityPoco.AWBSignature = entityPM.AWBSignature;
            
            FieldChange.Add(entityPoco.AWBPlace, entityPM.AWBPlace, nameof(entityPM.AWBPlace), fieldChanges);
            entityPoco.AWBPlace = entityPM.AWBPlace;
            
            FieldChange.Add(entityPoco.AWBChargesCodeCode, entityPM.AWBChargesCodeCode, nameof(entityPM.AWBChargesCodeCode), fieldChanges);
            entityPoco.AWBChargesCodeCode = entityPM.AWBChargesCodeCode;
            
            FieldChange.Add(entityPoco.AWBSpecialHandlingCodeId1, entityPM.AWBSpecialHandlingCodeId1, nameof(entityPM.AWBSpecialHandlingCodeId1), fieldChanges);
            entityPoco.AWBSpecialHandlingCodeId1 = entityPM.AWBSpecialHandlingCodeId1;
            
            FieldChange.Add(entityPoco.AWBSpecialHandlingCodeId2, entityPM.AWBSpecialHandlingCodeId2, nameof(entityPM.AWBSpecialHandlingCodeId2), fieldChanges);
            entityPoco.AWBSpecialHandlingCodeId2 = entityPM.AWBSpecialHandlingCodeId2;
            
            FieldChange.Add(entityPoco.AWBSpecialHandlingCodeId3, entityPM.AWBSpecialHandlingCodeId3, nameof(entityPM.AWBSpecialHandlingCodeId3), fieldChanges);
            entityPoco.AWBSpecialHandlingCodeId3 = entityPM.AWBSpecialHandlingCodeId3;
            
            FieldChange.Add(entityPoco.AWBSpecialHandlingCodeId4, entityPM.AWBSpecialHandlingCodeId4, nameof(entityPM.AWBSpecialHandlingCodeId4), fieldChanges);
            entityPoco.AWBSpecialHandlingCodeId4 = entityPM.AWBSpecialHandlingCodeId4;
            
            FieldChange.Add(entityPoco.AWBSpecialHandlingCodeId5, entityPM.AWBSpecialHandlingCodeId5, nameof(entityPM.AWBSpecialHandlingCodeId5), fieldChanges);
            entityPoco.AWBSpecialHandlingCodeId5 = entityPM.AWBSpecialHandlingCodeId5;
            
            FieldChange.Add(entityPoco.AWBSpecialHandlingCodeId6, entityPM.AWBSpecialHandlingCodeId6, nameof(entityPM.AWBSpecialHandlingCodeId6), fieldChanges);
            entityPoco.AWBSpecialHandlingCodeId6 = entityPM.AWBSpecialHandlingCodeId6;
            
            FieldChange.Add(entityPoco.AWBSpecialHandlingCodeId7, entityPM.AWBSpecialHandlingCodeId7, nameof(entityPM.AWBSpecialHandlingCodeId7), fieldChanges);
            entityPoco.AWBSpecialHandlingCodeId7 = entityPM.AWBSpecialHandlingCodeId7;
            
            FieldChange.Add(entityPoco.AWBSpecialHandlingCodeId8, entityPM.AWBSpecialHandlingCodeId8, nameof(entityPM.AWBSpecialHandlingCodeId8), fieldChanges);
            entityPoco.AWBSpecialHandlingCodeId8 = entityPM.AWBSpecialHandlingCodeId8;
            
            FieldChange.Add(entityPoco.AWBSpecialHandlingCodeId9, entityPM.AWBSpecialHandlingCodeId9, nameof(entityPM.AWBSpecialHandlingCodeId9), fieldChanges);
            entityPoco.AWBSpecialHandlingCodeId9 = entityPM.AWBSpecialHandlingCodeId9;
        }
        private static void MapTotalsFields(ShipmentPM entityPM, Shipment entityPoco, bool isNewEntity, List<FieldChange> fieldChanges)
        {
            FieldChange.Add(entityPoco.ProfitCurrencyId, entityPM.ProfitCurrencyId, nameof(entityPM.ProfitCurrencyId), fieldChanges);
            entityPoco.ProfitCurrencyId = entityPM.ProfitCurrencyId;
            
            FieldChange.Add(entityPoco.ProfitExchangeRate, entityPM.ProfitExchangeRate, nameof(entityPM.ProfitExchangeRate), fieldChanges);
            entityPoco.ProfitExchangeRate = entityPM.ProfitExchangeRate;
            
            FieldChange.Add(entityPoco.EstimateProfitInLocalCurrency, entityPM.EstimateProfitInLocalCurrency, nameof(entityPM.EstimateProfitInLocalCurrency), fieldChanges);
            entityPoco.EstimateProfitInLocalCurrency = entityPM.EstimateProfitInLocalCurrency;
            
            FieldChange.Add(entityPoco.EstimateProfitInProfitCurrency, entityPM.EstimateProfitInProfitCurrency, nameof(entityPM.EstimateProfitInProfitCurrency), fieldChanges);
            entityPoco.EstimateProfitInProfitCurrency = entityPM.EstimateProfitInProfitCurrency;

            // Ayman: these fields should not be mapped on update            
            if (isNewEntity || entityPM.IsHybrid) // Islam: we need to update these fields in hybrid case
            {
                FieldChange.Add(entityPoco.ShipmentReceivableStatusCode, entityPM.ShipmentReceivableStatusCode, nameof(entityPM.ShipmentReceivableStatusCode), fieldChanges);
                entityPoco.ShipmentReceivableStatusCode = entityPM.ShipmentReceivableStatusCode;
                
                FieldChange.Add(entityPoco.ShipmentPayableStatusCode, entityPM.ShipmentPayableStatusCode, nameof(entityPM.ShipmentPayableStatusCode), fieldChanges);
                entityPoco.ShipmentPayableStatusCode = entityPM.ShipmentPayableStatusCode;
                
                FieldChange.Add(entityPoco.OpenPayablesInLocalCurrency, entityPM.OpenPayablesInLocalCurrency == null ? 0 : entityPM.OpenPayablesInLocalCurrency, nameof(entityPM.OpenPayablesInLocalCurrency), fieldChanges);
                entityPoco.OpenPayablesInLocalCurrency = entityPM.OpenPayablesInLocalCurrency == null ? 0 : entityPM.OpenPayablesInLocalCurrency;
                
                FieldChange.Add(entityPoco.AccountedPayablesInLocalCurrency, entityPM.AccountedPayablesInLocalCurrency == null ? 0 : entityPM.AccountedPayablesInLocalCurrency, nameof(entityPM.AccountedPayablesInLocalCurrency), fieldChanges);
                entityPoco.AccountedPayablesInLocalCurrency = entityPM.AccountedPayablesInLocalCurrency == null ? 0 : entityPM.AccountedPayablesInLocalCurrency;
                
                FieldChange.Add(entityPoco.OpenReceivablesInLocalCurrency, entityPM.OpenReceivablesInLocalCurrency == null ? 0 : entityPM.OpenReceivablesInLocalCurrency, nameof(entityPM.OpenReceivablesInLocalCurrency), fieldChanges);
                entityPoco.OpenReceivablesInLocalCurrency = entityPM.OpenReceivablesInLocalCurrency == null ? 0 : entityPM.OpenReceivablesInLocalCurrency;
                
                FieldChange.Add(entityPoco.AccountedReceivablesInLocalCurrency, entityPM.AccountedReceivablesInLocalCurrency == null ? 0 : entityPM.AccountedReceivablesInLocalCurrency, nameof(entityPM.AccountedReceivablesInLocalCurrency), fieldChanges);
                entityPoco.AccountedReceivablesInLocalCurrency = entityPM.AccountedReceivablesInLocalCurrency == null ? 0 : entityPM.AccountedReceivablesInLocalCurrency;
                
                FieldChange.Add(entityPoco.ProfitInLocalCurrency, entityPM.ProfitInLocalCurrency, nameof(entityPM.ProfitInLocalCurrency), fieldChanges);
                entityPoco.ProfitInLocalCurrency = entityPM.ProfitInLocalCurrency;
                
                FieldChange.Add(entityPoco.OpenPayablesInProfitCurrency, entityPM.OpenPayablesInProfitCurrency == null ? 0 : entityPM.OpenPayablesInProfitCurrency, nameof(entityPM.OpenPayablesInProfitCurrency), fieldChanges);
                entityPoco.OpenPayablesInProfitCurrency = entityPM.OpenPayablesInProfitCurrency == null ? 0 : entityPM.OpenPayablesInProfitCurrency;
                
                FieldChange.Add(entityPoco.AccountedPayablesInProfitCurrency, entityPM.AccountedPayablesInProfitCurrency == null ? 0 : entityPM.AccountedPayablesInProfitCurrency, nameof(entityPM.AccountedPayablesInProfitCurrency), fieldChanges);
                entityPoco.AccountedPayablesInProfitCurrency = entityPM.AccountedPayablesInProfitCurrency == null ? 0 : entityPM.AccountedPayablesInProfitCurrency;
                
                FieldChange.Add(entityPoco.OpenReceivablesInProfitCurrency, entityPM.OpenReceivablesInProfitCurrency == null ? 0 : entityPM.OpenReceivablesInProfitCurrency, nameof(entityPM.OpenReceivablesInProfitCurrency), fieldChanges);
                entityPoco.OpenReceivablesInProfitCurrency = entityPM.OpenReceivablesInProfitCurrency == null ? 0 : entityPM.OpenReceivablesInProfitCurrency;
                
                FieldChange.Add(entityPoco.AccountedReceivablesInProfitCurrency, entityPM.AccountedReceivablesInProfitCurrency == null ? 0 : entityPM.AccountedReceivablesInProfitCurrency, nameof(entityPM.AccountedReceivablesInProfitCurrency), fieldChanges);
                entityPoco.AccountedReceivablesInProfitCurrency = entityPM.AccountedReceivablesInProfitCurrency == null ? 0 : entityPM.AccountedReceivablesInProfitCurrency;
                
                FieldChange.Add(entityPoco.ProfitInProfitCurrency, entityPM.ProfitInProfitCurrency == null ? 0 : entityPM.ProfitInProfitCurrency, nameof(entityPM.ProfitInProfitCurrency), fieldChanges);
                entityPoco.ProfitInProfitCurrency = entityPM.ProfitInProfitCurrency == null ? 0 : entityPM.ProfitInProfitCurrency;
            }
        }

        private static void MapWeightsFields(ShipmentPM entityPM, Shipment entityPoco, bool isNewEntity, List<FieldChange> fieldChanges)
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

            FieldChange.Add(entityPoco.Ratio, entityPM.Ratio, nameof(entityPM.Ratio), fieldChanges);
            entityPoco.Ratio = entityPM.Ratio;
            
            FieldChange.Add(entityPoco.DimFactor, entityPM.DimFactor, nameof(entityPM.DimFactor), fieldChanges);
            entityPoco.DimFactor = entityPM.DimFactor;
            
            FieldChange.Add(entityPoco.NumberOfContainers, entityPM.NumberOfContainers, nameof(entityPM.NumberOfContainers), fieldChanges);
            entityPoco.NumberOfContainers = entityPM.NumberOfContainers;

            FieldChange.Add(entityPoco.DimensionsUnitCode, entityPM.DimensionsUnitCode, nameof(entityPM.DimensionsUnitCode), fieldChanges);
            entityPoco.DimensionsUnitCode = entityPM.DimensionsUnitCode;

            FieldChange.Add(entityPoco.VolumeUnitCode, entityPM.VolumeUnitCode, nameof(entityPM.VolumeUnitCode), fieldChanges);
            entityPoco.VolumeUnitCode = entityPM.VolumeUnitCode;

            FieldChange.Add(entityPoco.ChargeableWeightUnitCode, entityPM.ChargeableWeightUnitCode, nameof(entityPM.ChargeableWeightUnitCode), fieldChanges);
            entityPoco.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;

            FieldChange.Add(entityPoco.GrossWeightPerStorageDays, entityPM.GrossWeightPerStorageDays, nameof(entityPM.GrossWeightPerStorageDays), fieldChanges);
            entityPoco.GrossWeightPerStorageDays = entityPM.GrossWeightPerStorageDays;

            FieldChange.Add(entityPoco.GrossWeightEdited, entityPM.GrossWeightEdited, nameof(entityPM.GrossWeightEdited), fieldChanges);
            entityPoco.GrossWeightEdited = entityPM.GrossWeightEdited;

            FieldChange.Add(entityPoco.ChargeableWeightEdited, entityPM.ChargeableWeightEdited, nameof(entityPM.ChargeableWeightEdited), fieldChanges);
            entityPoco.ChargeableWeightEdited = entityPM.ChargeableWeightEdited;

            FieldChange.Add(entityPoco.Volume, entityPM.Volume, nameof(entityPM.Volume), fieldChanges);
            entityPoco.Volume = entityPM.Volume;

            FieldChange.Add(entityPoco.VolumetricWeight, entityPM.VolumetricWeight, nameof(entityPM.VolumetricWeight), fieldChanges);
            entityPoco.VolumetricWeight = entityPM.VolumetricWeight;

            FieldChange.Add(entityPoco.VolumeInCBM, GetVolumeInCBM(entityPM.VolumeUnitCode, entityPM.Volume), nameof(entityPM.VolumeInCBM), fieldChanges);
            entityPoco.VolumeInCBM = GetVolumeInCBM(entityPM.VolumeUnitCode, entityPM.Volume);

            FieldChange.Add(entityPoco.CustomsDeclarationNumber, entityPM.CustomsDeclarationNumber, nameof(entityPM.CustomsDeclarationNumber), fieldChanges);
            entityPoco.CustomsDeclarationNumber = entityPM.CustomsDeclarationNumber;

            FieldChange.Add(entityPoco.ShipperName, entityPM.ShipperName, nameof(entityPM.ShipperName), fieldChanges);
            entityPoco.ShipperName = entityPM.ShipperName;

            FieldChange.Add(entityPoco.ConsigneeName, entityPM.ConsigneeName, nameof(entityPM.ConsigneeName), fieldChanges);
            entityPoco.ConsigneeName = entityPM.ConsigneeName;

            FieldChange.Add(entityPoco.ShippingAgent, entityPM.ShippingAgent, nameof(entityPM.ShippingAgent), fieldChanges);
            entityPoco.ShippingAgent = entityPM.ShippingAgent;

            FieldChange.Add(entityPoco.PrivateLabelAgentName, entityPM.AgentName ?? entityPM.PrivateLabelAgentName, nameof(entityPM.PrivateLabelAgentName), fieldChanges);
            entityPoco.PrivateLabelAgentName = entityPM.AgentName ?? entityPM.PrivateLabelAgentName;

            if (!entityPM.IsHybrid || entityPM.DontAddToImportersQueue)
            {
                FieldChange.Add(entityPoco.ForwarderShipmentNumber, entityPM.ForwarderShipmentNumber, nameof(entityPM.ForwarderShipmentNumber), fieldChanges);
                entityPoco.ForwarderShipmentNumber = entityPM.ForwarderShipmentNumber;

                FieldChange.Add(entityPoco.CustomerShipmentNumber, entityPM.CustomerShipmentNumber, nameof(entityPM.CustomerShipmentNumber), fieldChanges);
                entityPoco.CustomerShipmentNumber = entityPM.CustomerShipmentNumber;

                FieldChange.Add(entityPoco.CustomerTenantNumber, entityPM.CustomerTenantNumber, nameof(entityPM.CustomerTenantNumber), fieldChanges);
                entityPoco.CustomerTenantNumber = entityPM.CustomerTenantNumber;
            }

            if (!string.IsNullOrEmpty(entityPM.ForwarderShipmentNumber))
            {
                FieldChange.Add(entityPoco.ComputedForwarderShipmentNumber, entityPM.ComputedForwarderShipmentNumber, nameof(entityPM.ComputedForwarderShipmentNumber), fieldChanges);
                entityPoco.ComputedForwarderShipmentNumber = entityPM.ForwarderShipmentNumber;
            }
            else
            {
                entityPoco.ComputedForwarderShipmentNumber = entityPM.Id;
            }

            if (entityPM.IsHybrid && !string.IsNullOrEmpty(entityPM.CustomerShipmentNumber))
            {
                FieldChange.Add(entityPoco.CustomerShipmentNumber, entityPM.CustomerShipmentNumber, nameof(entityPM.CustomerShipmentNumber), fieldChanges);
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

            FieldChange.Add(entityPoco.PackagesQuantity, entityPM.PackagesQuantity, nameof(entityPM.PackagesQuantity), fieldChanges);
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

        public static double? GetVolumeInCBF(double? volumeInCBM)
        {
            double? myResult = null;
            myResult = volumeInCBM * 35.315;
            
            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }

        public static double? GetWeightInLB(double? weight)
        {
            double? myResult = null;
            myResult = weight / 0.45359237;

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
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShippingAgent);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Transshipment1AdditionalMAWBOBLBL);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Transshipment2AdditionalMAWBOBLBL);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Transshipment3AdditionalMAWBOBLBL);

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ProjectNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.AMSBL);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.WarehouseLegReference);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.WarehouseLeg2Reference);

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShippingLine);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PlaceOfDelivery);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PickupPlace);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.SealNo);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.HSCode);

            if(entityPM.ShipmentAdditionalData != null && !String.IsNullOrEmpty(entityPM.ShipmentAdditionalData.ShipmentOrderNumber))
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipmentAdditionalData.ShipmentOrderNumber);
            }
            if (entityPM.TransportModeId == "I")
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.TrailerNumber);
            }

            if (entityPM.ShipmentLevelCode == "H")
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, entityMasterData?.MasterShipmentNumber);
            }

            if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
            {
                AddressRepository addressRepository = new AddressRepository(entityPoco.Tenant);

                if (entityMasterData.MainCarriageFromAddressId != null)
                {
                    Address fromAddress = addressRepository.GetSingleAddress(entityMasterData.MainCarriageFromAddressId, entityMasterData.Tenant);
                    if (fromAddress != null)
                    {
                        MethodHelper.AddToSearchFields(ref mySearchFields, fromAddress.City);
                    }
                }

                if (entityMasterData.MainCarriageToAddressId != null)
                {
                    Address toAddress = addressRepository.GetSingleAddress(entityMasterData.MainCarriageToAddressId, entityMasterData.Tenant);
                    if (toAddress != null)
                    {
                        MethodHelper.AddToSearchFields(ref mySearchFields, toAddress.City);
                    }
                }
            }

            #region Quote
            if (!string.IsNullOrEmpty(entityPM.QuoteId))
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.QuoteNumber);
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
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.PreForwardingFromPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.OnForwardingFromPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.Transshipment1FromPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.Transshipment2FromPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.Transshipment3FromPortId);

            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.ToPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.MainCarriageToPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.PreCarriageToPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.OnCarriageToPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.PreForwardingToPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, tenant, entityPM.OnForwardingToPortId);
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
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerReference3);
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

                if (!string.IsNullOrEmpty(item.HorseName))
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, item.HorseName);
                }
            }
            #endregion

            #region Custom Fields
            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Shipment", tenant).Where(o => o.DataTypeCode == "Text" || o.DataTypeCode == "nText").ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
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
            AddFieldChangedProperties(changeTrackingPM, "ShipmentTypeId", changeTrackingPM.ShipmentTypeId, pm.ShipmentTypeId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "IncotermId", changeTrackingPM.IncotermId, pm.IncotermId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "IsOperationalClosed", changeTrackingPM.IsOperationalClosed, pm.IsOperationalClosed, "bool", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "IsAccountingClosed", changeTrackingPM.IsAccountingClosed, pm.IsAccountingClosed, "bool", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "MainCarriageETD", changeTrackingPM.MainCarriageETD, pm.MainCarriageETD, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "MainCarriageATD", changeTrackingPM.MainCarriageATD, pm.MainCarriageATD, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "MainCarriageETA", changeTrackingPM.MainCarriageETA, pm.MainCarriageETA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "MainCarriageATA", changeTrackingPM.MainCarriageATA, pm.MainCarriageATA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "MainCarriageFinalDestinationETA", changeTrackingPM.MainCarriageFinalDestinationETA, pm.MainCarriageFinalDestinationETA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "MainCarriageFinalDestinationATA", changeTrackingPM.MainCarriageFinalDestinationATA, pm.MainCarriageFinalDestinationATA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "FinalDeliveryATA", changeTrackingPM.FinalDeliveryATA, pm.FinalDeliveryATA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "FinalDeliveryATD", changeTrackingPM.FinalDeliveryATD, pm.FinalDeliveryATD, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "FinalDeliveryETA", changeTrackingPM.FinalDeliveryETA, pm.FinalDeliveryETA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "FinalDeliveryETD", changeTrackingPM.FinalDeliveryETD, pm.FinalDeliveryETD, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "CutoffDate", changeTrackingPM.CutoffDate, pm.CutoffDate, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "IsCancelled", changeTrackingPM.IsCancelled, pm.IsCancelled, "bool", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "IsDocumentsNeedApprove", changeTrackingPM.IsDocumentsNeedApprove, pm.IsDocumentsNeedApprove, "bool", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "FirstARInvoiceApprovalDate", changeTrackingPM.FirstARInvoiceApprovalDate, pm.FirstARInvoiceApprovalDate, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "ActualFinalArrivalDate", changeTrackingPM.ActualFinalArrivalDate, pm.ActualFinalArrivalDate, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "EstimatedFinalArrivalDate", changeTrackingPM.EstimatedFinalArrivalDate, pm.EstimatedFinalArrivalDate, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "CreateDateTime", changeTrackingPM.CreateDateTime, pm.CreateDateTime, "CreateDateTime", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "WarehouseStorageFreeDays", changeTrackingPM.WarehouseStorageFreeDays, pm.WarehouseStorageFreeDays, "WarehouseStorageFreeDays", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "OrderIsDangerouseGoods", changeTrackingPM.OrderIsDangerouseGoods, pm.OrderIsDangerouseGoods, "OrderIsDangerouseGoods", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "FirstPickupETA", changeTrackingPM.FirstPickupETA, pm.FirstPickupETA, "FirstPickupETA", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "FirstPickupETD", changeTrackingPM.FirstPickupETD, pm.FirstPickupETD, "FirstPickupETD", notifyPropertyChangeValuesList);            
            AddFieldChangedProperties(changeTrackingPM, "DocumentsClosingDate", changeTrackingPM.DocumentsClosingDate, pm.DocumentsClosingDate, "DocumentsClosingDate", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "WarehouseLegLastFreeDate", changeTrackingPM.WarehouseLegLastFreeDate, pm.WarehouseLegLastFreeDate, "WarehouseLegLastFreeDate", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "LastSharedEventId", changeTrackingPM.LastSharedEventId, pm.LastSharedEventId, "LastSharedEventId", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "LastSharedEventDate", changeTrackingPM.LastSharedEventDate, pm.LastSharedEventDate, "LastSharedEventDate", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "BookingConfirmationSentDate", changeTrackingPM.BookingConfirmationSentDate, pm.BookingConfirmationSentDate, "BookingConfirmationSentDate", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "ContainersNumbers", changeTrackingPM.ContainersNumbers, pm.ContainersNumbers, "ContainersNumbers", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "PreAlertSentDate", changeTrackingPM.PreAlertSentDate, pm.PreAlertSentDate, "PreAlertSentDate", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "DeliveryNoticeSentDate", changeTrackingPM.DeliveryNoticeSentDate, pm.DeliveryNoticeSentDate, "DeliveryNoticeSentDate", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "ExpectedArrivalNoticeSentDate", changeTrackingPM.ExpectedArrivalNoticeSentDate, pm.ExpectedArrivalNoticeSentDate, "ExpectedArrivalNoticeSentDate", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "ArrivalNoticeSentDate", changeTrackingPM.ArrivalNoticeSentDate, pm.ArrivalNoticeSentDate, "ArrivalNoticeSentDate", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "T1ReceivedDate", changeTrackingPM.T1ReceivedDate, pm.T1ReceivedDate, "T1ReceivedDate", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "LastUpdateDate", changeTrackingPM.LastUpdateDate, pm.LastUpdateDate, "LastUpdateDate", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "IsAccrualsApproved", changeTrackingPM.IsAccrualsApproved, pm.IsAccrualsApproved, "bool", notifyPropertyChangeValuesList);                      
            AddFieldChangedProperties(changeTrackingPM, "FirstPickupATD", changeTrackingPM.FirstPickupATD, pm.FirstPickupATD, "FirstPickupATD", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "IncludesCustoms", changeTrackingPM.IncludesCustoms, pm.IncludesCustoms, "IncludesCustoms", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "CustomsClearanceDate", changeTrackingPM.CustomsClearanceDate, pm.CustomsClearanceDate, "CustomsClearanceDate", notifyPropertyChangeValuesList);

            AddFieldChangedProperties(changeTrackingPM, "MainCarriageFromPortId", changeTrackingPM.MainCarriageFromPortId, pm.MainCarriageFromPortId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "MainCarriageToPortId", changeTrackingPM.MainCarriageToPortId, pm.MainCarriageToPortId, "string", notifyPropertyChangeValuesList);

            AddFieldChangedProperties(changeTrackingPM, "Transshipment1ATA", changeTrackingPM.Transshipment1ATA, pm.Transshipment1ATA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment1ETA", changeTrackingPM.Transshipment1ETA, pm.Transshipment1ETA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment1ATD", changeTrackingPM.Transshipment1ATD, pm.Transshipment1ATD, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment1ETD", changeTrackingPM.Transshipment1ETD, pm.Transshipment1ETD, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment1FromPortId", changeTrackingPM.Transshipment1FromPortId, pm.Transshipment1FromPortId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment1ToPortId", changeTrackingPM.Transshipment1ToPortId, pm.Transshipment1ToPortId, "string", notifyPropertyChangeValuesList);

            AddFieldChangedProperties(changeTrackingPM, "Transshipment2ATA", changeTrackingPM.Transshipment2ATA, pm.Transshipment2ATA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment2ETA", changeTrackingPM.Transshipment2ETA, pm.Transshipment2ETA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment2ATD", changeTrackingPM.Transshipment2ATD, pm.Transshipment2ATD, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment2ETD", changeTrackingPM.Transshipment2ETD, pm.Transshipment2ETD, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment2FromPortId", changeTrackingPM.Transshipment2FromPortId, pm.Transshipment2FromPortId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment2ToPortId", changeTrackingPM.Transshipment2ToPortId, pm.Transshipment2ToPortId, "string", notifyPropertyChangeValuesList);

            AddFieldChangedProperties(changeTrackingPM, "Transshipment3ATA", changeTrackingPM.Transshipment3ATA, pm.Transshipment3ATA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment3ETA", changeTrackingPM.Transshipment3ETA, pm.Transshipment3ETA, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment3ATD", changeTrackingPM.Transshipment3ATD, pm.Transshipment3ATD, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment3ETD", changeTrackingPM.Transshipment3ETD, pm.Transshipment3ETD, "DateTime?", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment3FromPortId", changeTrackingPM.Transshipment3FromPortId, pm.Transshipment3FromPortId, "string", notifyPropertyChangeValuesList);
            AddFieldChangedProperties(changeTrackingPM, "Transshipment3ToPortId", changeTrackingPM.Transshipment3ToPortId, pm.Transshipment3ToPortId, "string", notifyPropertyChangeValuesList);


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
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field41, pm.Field41, "Field41", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field42, pm.Field42, "Field42", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field43, pm.Field43, "Field43", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field44, pm.Field44, "Field44", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field45, pm.Field45, "Field45", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field46, pm.Field46, "Field46", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field47, pm.Field47, "Field47", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field48, pm.Field48, "Field48", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field49, pm.Field49, "Field49", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field50, pm.Field50, "Field50", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field51, pm.Field51, "Field51", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field52, pm.Field52, "Field52", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field53, pm.Field53, "Field53", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field54, pm.Field54, "Field54", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field55, pm.Field55, "Field55", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field56, pm.Field56, "Field56", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field57, pm.Field57, "Field57", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field58, pm.Field58, "Field58", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field59, pm.Field59, "Field59", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field60, pm.Field60, "Field60", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field61, pm.Field61, "Field61", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field62, pm.Field62, "Field62", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field63, pm.Field63, "Field63", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field64, pm.Field64, "Field64", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field65, pm.Field65, "Field65", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field66, pm.Field66, "Field66", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field67, pm.Field67, "Field67", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field68, pm.Field68, "Field68", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field69, pm.Field69, "Field69", notifyPropertyChangeValuesList);
            AddCustomFieldChangedProperties(changeTrackingPM, changeTrackingPM.Field70, pm.Field70, "Field70", notifyPropertyChangeValuesList);

            if (changeTrackingPM.ShipmentLevelCode == "H")
            {
                AddFieldChangedProperties(changeTrackingPM, "PreForwardingETA", changeTrackingPM.PreForwardingETA, pm.PreForwardingETA, "PreForwardingETA", notifyPropertyChangeValuesList);
                AddFieldChangedProperties(changeTrackingPM, "PreForwardingETD", changeTrackingPM.PreForwardingETD, pm.PreForwardingETD, "PreForwardingETD", notifyPropertyChangeValuesList);
                AddFieldChangedProperties(changeTrackingPM, "OnForwardingETA", changeTrackingPM.OnForwardingETA, pm.OnForwardingETA, "OnForwardingETA", notifyPropertyChangeValuesList);
                AddFieldChangedProperties(changeTrackingPM, "OnForwardingETD", changeTrackingPM.OnForwardingETD, pm.OnForwardingETD, "OnForwardingETD", notifyPropertyChangeValuesList);
            }

            else
            {
                AddFieldChangedProperties(changeTrackingPM, "PreCarriageETA", changeTrackingPM.PreCarriageETA, pm.PreCarriageETA, "PreCarriageETA", notifyPropertyChangeValuesList);
                AddFieldChangedProperties(changeTrackingPM, "PreCarriageETD", changeTrackingPM.PreCarriageETD, pm.PreCarriageETD, "PreCarriageETD", notifyPropertyChangeValuesList);
                AddFieldChangedProperties(changeTrackingPM, "OnCarriageETA", changeTrackingPM.OnCarriageETA, pm.OnCarriageETA, "OnCarriageETA", notifyPropertyChangeValuesList);
                AddFieldChangedProperties(changeTrackingPM, "OnCarriageETD", changeTrackingPM.OnCarriageETD, pm.OnCarriageETD, "OnCarriageETD", notifyPropertyChangeValuesList);
            }

            if (EntityChangeHelper.IsShowLogBoxAutomationFields())
            {
                AddFieldChangedProperties(changeTrackingPM, "IsDigitalSignRequired", changeTrackingPM.IsDigitalSignRequired, pm.IsDigitalSignRequired, "bool", notifyPropertyChangeValuesList);
                AddFieldChangedProperties(changeTrackingPM, "IsRequestedDocuments", changeTrackingPM.IsRequestedDocuments, pm.IsRequestedDocuments, "bool", notifyPropertyChangeValuesList);
                AddFieldChangedProperties(changeTrackingPM, "IsDepositionRequired", changeTrackingPM.IsDepositionRequired, pm.IsDepositionRequired, "bool", notifyPropertyChangeValuesList);
                AddFieldChangedProperties(changeTrackingPM, "IsImporterApprovalRequired", changeTrackingPM.IsImporterApprovalRequired, pm.IsImporterApprovalRequired, "bool", notifyPropertyChangeValuesList);
            }


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

        public static ShipmentPM MapShipmentPMToShipmentPMForAutomation(ShipmentPM masterShipment, ShipmentPM houseShipment, ShipmentPM shipmentPM)
        {
            shipmentPM.Id = houseShipment.Id;
            shipmentPM.ShipmentNumber = houseShipment.ShipmentNumber;
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
            shipmentPM.IsCancelled = houseShipment.IsCancelled;
            shipmentPM.IsDocumentsNeedApprove = houseShipment.IsDocumentsNeedApprove;
            shipmentPM.FirstARInvoiceApprovalDate = houseShipment.FirstARInvoiceApprovalDate;
            shipmentPM.ActualFinalArrivalDate = houseShipment.ActualFinalArrivalDate;
            shipmentPM.EstimatedFinalArrivalDate = houseShipment.EstimatedFinalArrivalDate;
            shipmentPM.CreateDateTime = houseShipment.CreateDateTime;
            shipmentPM.WarehouseStorageFreeDays = houseShipment.WarehouseStorageFreeDays;
            shipmentPM.OrderIsDangerouseGoods = houseShipment.OrderIsDangerouseGoods;
            shipmentPM.FirstPickupETA = houseShipment.FirstPickupETA;
            shipmentPM.FirstPickupETD = houseShipment.FirstPickupETD;
            shipmentPM.PreCarriageETA = houseShipment.PreCarriageETA;
            shipmentPM.PreCarriageETD = houseShipment.PreCarriageETD;
            shipmentPM.OnCarriageETA = houseShipment.OnCarriageETA;
            shipmentPM.OnCarriageETD = houseShipment.OnCarriageETD;
            shipmentPM.DocumentsClosingDate = houseShipment.DocumentsClosingDate;
            shipmentPM.WarehouseLegLastFreeDate = houseShipment.WarehouseLegLastFreeDate;
            shipmentPM.LastSharedEventDate = houseShipment.LastSharedEventDate;
            shipmentPM.LastSharedEventId = houseShipment.LastSharedEventId;
            shipmentPM.IsAccrualsApproved = houseShipment.IsAccrualsApproved;
            shipmentPM.IncludesCustoms = houseShipment.IncludesCustoms;
            shipmentPM.CustomsClearanceDate = houseShipment.CustomsClearanceDate;


            if (EntityChangeHelper.IsShowLogBoxAutomationFields())
            {
                shipmentPM.IsDepositionRequired = houseShipment.IsDepositionRequired;
                shipmentPM.IsDigitalSignRequired = houseShipment.IsDigitalSignRequired;
                shipmentPM.IsRequestedDocuments = houseShipment.IsRequestedDocuments;
                shipmentPM.IsImporterApprovalRequired = houseShipment.IsImporterApprovalRequired;
            }


            shipmentPM.BookingConfirmationSentDate = houseShipment.BookingConfirmationSentDate;
            shipmentPM.ContainersNumbers = houseShipment.ContainersNumbers;
            shipmentPM.PreAlertSentDate = houseShipment.PreAlertSentDate;
            shipmentPM.DeliveryNoticeSentDate = houseShipment.DeliveryNoticeSentDate;
            shipmentPM.ExpectedArrivalNoticeSentDate = houseShipment.ExpectedArrivalNoticeSentDate;
            shipmentPM.ArrivalNoticeSentDate = houseShipment.ArrivalNoticeSentDate;
            shipmentPM.T1ReceivedDate = houseShipment.T1ReceivedDate;


            shipmentPM.ShipmentTypeId = houseShipment.ShipmentTypeId;

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
            shipmentPM.Field31 = houseShipment.Field31;
            shipmentPM.Field32 = houseShipment.Field32;
            shipmentPM.Field33 = houseShipment.Field33;
            shipmentPM.Field34 = houseShipment.Field34;
            shipmentPM.Field35 = houseShipment.Field35;
            shipmentPM.Field36 = houseShipment.Field36;
            shipmentPM.Field37 = houseShipment.Field37;
            shipmentPM.Field38 = houseShipment.Field38;
            shipmentPM.Field39 = houseShipment.Field39;
            shipmentPM.Field40 = houseShipment.Field40;
            shipmentPM.Field41 = houseShipment.Field41;
            shipmentPM.Field42 = houseShipment.Field42;
            shipmentPM.Field43 = houseShipment.Field43;
            shipmentPM.Field44 = houseShipment.Field44;
            shipmentPM.Field45 = houseShipment.Field45;
            shipmentPM.Field46 = houseShipment.Field46;
            shipmentPM.Field47 = houseShipment.Field47;
            shipmentPM.Field48 = houseShipment.Field48;
            shipmentPM.Field49 = houseShipment.Field49;
            shipmentPM.Field50 = houseShipment.Field50;
            shipmentPM.Field51 = houseShipment.Field51;
            shipmentPM.Field52 = houseShipment.Field52;
            shipmentPM.Field53 = houseShipment.Field53;
            shipmentPM.Field54 = houseShipment.Field54;
            shipmentPM.Field55 = houseShipment.Field55;
            shipmentPM.Field56 = houseShipment.Field56;
            shipmentPM.Field57 = houseShipment.Field57;
            shipmentPM.Field58 = houseShipment.Field58;
            shipmentPM.Field59 = houseShipment.Field59;
            shipmentPM.Field60 = houseShipment.Field60;
            shipmentPM.Field61 = houseShipment.Field61;
            shipmentPM.Field62 = houseShipment.Field62;
            shipmentPM.Field63 = houseShipment.Field63;
            shipmentPM.Field64 = houseShipment.Field64;
            shipmentPM.Field65 = houseShipment.Field65;
            shipmentPM.Field66 = houseShipment.Field66;
            shipmentPM.Field67 = houseShipment.Field67;
            shipmentPM.Field68 = houseShipment.Field68;
            shipmentPM.Field69 = houseShipment.Field69;
            shipmentPM.Field70 = houseShipment.Field70;
            shipmentPM.HandlerUserId = houseShipment.HandlerUserId;
            shipmentPM.PlannedCargoReadyDate = houseShipment.PlannedCargoReadyDate;
            shipmentPM.ApprovedCargoReadyDate = houseShipment.ApprovedCargoReadyDate;

            if (masterShipment != null)
            {
                shipmentPM.MainCarriageCarrierId = masterShipment.MainCarriageCarrierId;
                shipmentPM.MainCarriageETA = masterShipment.MainCarriageETA;
                shipmentPM.MainCarriageETD = masterShipment.MainCarriageETD;
                shipmentPM.MainCarriageATD = masterShipment.MainCarriageATD;
                shipmentPM.MainCarriageATA = masterShipment.MainCarriageATA;
                shipmentPM.FinalDistenationPortId = masterShipment.FinalDistenationPortId;
                shipmentPM.StatusId = masterShipment.StatusId;
                shipmentPM.CutoffDate = masterShipment.CutoffDate;
                shipmentPM.MasterShipmentDataId = masterShipment.Id;

                shipmentPM.MainCarriageFromPortId = masterShipment.MainCarriageFromPortId;
                shipmentPM.MainCarriageToPortId = masterShipment.MainCarriageToPortId;

                shipmentPM.Transshipment1ATA = masterShipment.Transshipment1ATA;
                shipmentPM.Transshipment1ETA = masterShipment.Transshipment1ETA;
                shipmentPM.Transshipment1ATD = masterShipment.Transshipment1ATD;
                shipmentPM.Transshipment1ETD = masterShipment.Transshipment1ETD;
                shipmentPM.Transshipment1FromPortId = masterShipment.Transshipment1FromPortId;
                shipmentPM.Transshipment1ToPortId = masterShipment.Transshipment1ToPortId;

                shipmentPM.Transshipment2ATA = masterShipment.Transshipment2ATA;
                shipmentPM.Transshipment2ETA = masterShipment.Transshipment2ETA;
                shipmentPM.Transshipment2ATD = masterShipment.Transshipment2ATD;
                shipmentPM.Transshipment2ETD = masterShipment.Transshipment2ETD;
                shipmentPM.Transshipment2FromPortId = masterShipment.Transshipment2FromPortId;
                shipmentPM.Transshipment2ToPortId = masterShipment.Transshipment2ToPortId;

                shipmentPM.Transshipment3ATA = masterShipment.Transshipment3ATA;
                shipmentPM.Transshipment3ETA = masterShipment.Transshipment3ETA;
                shipmentPM.Transshipment3ATD = masterShipment.Transshipment3ATD;
                shipmentPM.Transshipment3ETD = masterShipment.Transshipment3ETD;
                shipmentPM.Transshipment3FromPortId = masterShipment.Transshipment3FromPortId;
                shipmentPM.Transshipment3ToPortId = masterShipment.Transshipment3ToPortId;
            }


            return shipmentPM;

        }

        public static ShipmentPM MapMasterDetailsForShipment(ShipmentPM masterShipment, ShipmentPM shipmentPM)
        {
            if (masterShipment == null) return shipmentPM;

            shipmentPM.MainCarriageCarrierId = masterShipment.MainCarriageCarrierId;
            shipmentPM.MainCarriageETA = masterShipment.MainCarriageETA;
            shipmentPM.MainCarriageETD = masterShipment.MainCarriageETD;
            shipmentPM.MainCarriageATD = masterShipment.MainCarriageATD;
            shipmentPM.MainCarriageATA = masterShipment.MainCarriageATA;
            shipmentPM.FinalDistenationPortId = masterShipment.FinalDistenationPortId;
            shipmentPM.StatusId = masterShipment.StatusId;
            shipmentPM.CutoffDate = masterShipment.CutoffDate;
            shipmentPM.MasterShipmentDataId = masterShipment.Id;

            return shipmentPM;
        }
        
        private static bool IsLogboxEnvironment()
        {
            return !string.IsNullOrEmpty(LogitudeSettings.DeploymentStage) && (LogitudeSettings.DeploymentStage.ToLower() == "logboxpre" || LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1");
        }
    }
}
