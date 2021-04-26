
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.EntityPMs; 
using Logitude.WarehouseLib.Data;

namespace Logitude.WarehouseLib.BL.EntityDataMappings
{
   
   public partial class WarehouseEntryDataMapping: IMapping<WarehouseEntryPM, WarehouseEntry>,IMappingEncodeBase64NVARCHARFields<WarehouseEntryPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         EntryNumber, 
	         CustomerId, 
	         ShipmentId, 
	         ShipmentNumber, 
	         WarehouseId, 
	         ExpectedEntryDate, 
	         ActualEntryDate, 
	         ReceivedBy, 
	         SpecialInstruction, 
	         StatusCode, 
	         TotalPieces, 
	         TotalGrossWeight, 
	         GrossWeightUnitCode, 
	         TotalVolume, 
	         VolumeUnitCode, 
	         Notes, 
	         CustomerRef1, 
	         CustomerRef2, 
	         HouseNumber, 
	         MasterNumber, 
	         DimensionsUnitCode, 
	         ShipmentLevelCode, 
	         TransportModeId, 
	         FromPortId, 
	         ToPortId, 
	         TruckerId, 
	         TruckerReference, 
	         ShipperId, 
	         DirectionId, 
	         ShipmentTypeId, 
	         EntryReference, 
	         SearchFields, 
	         ConnectedToShipment, 
	         FromAddressId, 
	         ToAddressId, 
	         ConsigneeId, 
	         ShipperReference1, 
	         ConsigneeReference1, 
	         ConsigneeReference2, 
	         ShipperReference2, 
	         ShipperName, 
	         ConsigneeName, 
	         Manufacturer, 
	         FromPartnerId, 
	         ToPartnerId, 
	         ChargeableWeightUnitCode, 
	         TotalVolumetricWeight, 
	         LastStatusUpdateDate, 
	         ConnectedTo, 
	         Ratio, 
	         ToTypeCode, 
	         FromTypeCode, 
	         FromCountryId, 
	         ToCountryId, 
	         MasterShipmentNumber,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         EntryNumber, 
	         CustomerId, 
	         ShipmentId, 
	         ShipmentNumber, 
	         WarehouseId, 
	         ExpectedEntryDate, 
	         ActualEntryDate, 
	         ReceivedBy, 
	         SpecialInstruction, 
	         StatusCode, 
	         TotalPieces, 
	         TotalGrossWeight, 
	         GrossWeightUnitCode, 
	         TotalVolume, 
	         VolumeUnitCode, 
	         Notes, 
	         CustomerRef1, 
	         CustomerRef2, 
	         HouseNumber, 
	         MasterNumber, 
	         WarehouseName, 
	         CustomerName, 
	         References, 
	         StatusName, 
	         DimensionsUnitCode, 
	         ShipmentNumberWithType, 
	         ShipmentLevelCode, 
	         TransportModeId, 
	         FromPortId, 
	         ToPortId, 
	         TruckerId, 
	         TruckerReference, 
	         ShipperId, 
	         DirectionId, 
	         ShipmentTypeId, 
	         EntryReference, 
	         SearchFields, 
	         Origin, 
	         Destination, 
	         MainCarriageCarrierName, 
	         Routing, 
	         ConnectedToShipment, 
	         FromAddressId, 
	         ToAddressId, 
	         ConsigneeId, 
	         ShipperReference1, 
	         ConsigneeReference1, 
	         ConsigneeReference2, 
	         ShipperReference2, 
	         ShipperName, 
	         ConsigneeName, 
	         Manufacturer, 
	         FromPartnerId, 
	         ToPartnerId, 
	         ChargeableWeightUnitCode, 
	         TotalVolumetricWeight, 
	         LastStatusUpdateDate, 
	         MasterHouse, 
	         EntryReferencesAndDate, 
	         ConnectedTo, 
	         Ratio, 
	         ToTypeCode, 
	         FromTypeCode, 
	         FromCountryId, 
	         ToCountryId, 
	         MasterShipmentNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(WarehouseEntryPM entityPM, WarehouseEntry entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntryNumber))
            {
				entityPOCO.EntryNumber = entityPM.EntryNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
				entityPOCO.CustomerId = entityPM.CustomerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
				entityPOCO.ShipmentId = entityPM.ShipmentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentNumber))
            {
				entityPOCO.ShipmentNumber = entityPM.ShipmentNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WarehouseId))
            {
				entityPOCO.WarehouseId = entityPM.WarehouseId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpectedEntryDate))
            {
				entityPOCO.ExpectedEntryDate = entityPM.ExpectedEntryDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActualEntryDate))
            {
				entityPOCO.ActualEntryDate = entityPM.ActualEntryDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReceivedBy))
            {
				entityPOCO.ReceivedBy = entityPM.ReceivedBy;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecialInstruction))
            {
				entityPOCO.SpecialInstruction = entityPM.SpecialInstruction;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
				entityPOCO.StatusCode = entityPM.StatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalPieces))
            {
				entityPOCO.TotalPieces = entityPM.TotalPieces;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalGrossWeight))
            {
				entityPOCO.TotalGrossWeight = entityPM.TotalGrossWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightUnitCode))
            {
				entityPOCO.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalVolume))
            {
				entityPOCO.TotalVolume = entityPM.TotalVolume;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumeUnitCode))
            {
				entityPOCO.VolumeUnitCode = entityPM.VolumeUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
				entityPOCO.Notes = entityPM.Notes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerRef1))
            {
				entityPOCO.CustomerRef1 = entityPM.CustomerRef1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerRef2))
            {
				entityPOCO.CustomerRef2 = entityPM.CustomerRef2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HouseNumber))
            {
				entityPOCO.HouseNumber = entityPM.HouseNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MasterNumber))
            {
				entityPOCO.MasterNumber = entityPM.MasterNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DimensionsUnitCode))
            {
				entityPOCO.DimensionsUnitCode = entityPM.DimensionsUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentLevelCode))
            {
				entityPOCO.ShipmentLevelCode = entityPM.ShipmentLevelCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
				entityPOCO.TransportModeId = entityPM.TransportModeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPortId))
            {
				entityPOCO.FromPortId = entityPM.FromPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPortId))
            {
				entityPOCO.ToPortId = entityPM.ToPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TruckerId))
            {
				entityPOCO.TruckerId = entityPM.TruckerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TruckerReference))
            {
				entityPOCO.TruckerReference = entityPM.TruckerReference;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperId))
            {
				entityPOCO.ShipperId = entityPM.ShipperId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DirectionId))
            {
				entityPOCO.DirectionId = entityPM.DirectionId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentTypeId))
            {
				entityPOCO.ShipmentTypeId = entityPM.ShipmentTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntryReference))
            {
				entityPOCO.EntryReference = entityPM.EntryReference;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConnectedToShipment))
            {
				entityPOCO.ConnectedToShipment = entityPM.ConnectedToShipment;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressId))
            {
				entityPOCO.FromAddressId = entityPM.FromAddressId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressId))
            {
				entityPOCO.ToAddressId = entityPM.ToAddressId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeId))
            {
				entityPOCO.ConsigneeId = entityPM.ConsigneeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperReference1))
            {
				entityPOCO.ShipperReference1 = entityPM.ShipperReference1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeReference1))
            {
				entityPOCO.ConsigneeReference1 = entityPM.ConsigneeReference1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeReference2))
            {
				entityPOCO.ConsigneeReference2 = entityPM.ConsigneeReference2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperReference2))
            {
				entityPOCO.ShipperReference2 = entityPM.ShipperReference2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperName))
            {
				entityPOCO.ShipperName = entityPM.ShipperName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeName))
            {
				entityPOCO.ConsigneeName = entityPM.ConsigneeName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Manufacturer))
            {
				entityPOCO.Manufacturer = entityPM.Manufacturer;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPartnerId))
            {
				entityPOCO.FromPartnerId = entityPM.FromPartnerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPartnerId))
            {
				entityPOCO.ToPartnerId = entityPM.ToPartnerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightUnitCode))
            {
				entityPOCO.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalVolumetricWeight))
            {
				entityPOCO.TotalVolumetricWeight = entityPM.TotalVolumetricWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastStatusUpdateDate))
            {
				entityPOCO.LastStatusUpdateDate = entityPM.LastStatusUpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConnectedTo))
            {
				entityPOCO.ConnectedTo = entityPM.ConnectedTo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Ratio))
            {
				entityPOCO.Ratio = entityPM.Ratio;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToTypeCode))
            {
				entityPOCO.ToTypeCode = entityPM.ToTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromTypeCode))
            {
				entityPOCO.FromTypeCode = entityPM.FromTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromCountryId))
            {
				entityPOCO.FromCountryId = entityPM.FromCountryId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToCountryId))
            {
				entityPOCO.ToCountryId = entityPM.ToCountryId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MasterShipmentNumber))
            {
				entityPOCO.MasterShipmentNumber = entityPM.MasterShipmentNumber;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(WarehouseEntryPM entityPM, WarehouseEntry entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntryNumber))
            {
					entityPM.EntryNumber = entityPOCO.EntryNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerId))
            {
					entityPM.CustomerId = entityPOCO.CustomerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentId))
            {
					entityPM.ShipmentId = entityPOCO.ShipmentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentNumber))
            {
					entityPM.ShipmentNumber = entityPOCO.ShipmentNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WarehouseId))
            {
					entityPM.WarehouseId = entityPOCO.WarehouseId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExpectedEntryDate))
            {
					entityPM.ExpectedEntryDate = entityPOCO.ExpectedEntryDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActualEntryDate))
            {
					entityPM.ActualEntryDate = entityPOCO.ActualEntryDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReceivedBy))
            {
					entityPM.ReceivedBy = entityPOCO.ReceivedBy;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SpecialInstruction))
            {
					entityPM.SpecialInstruction = entityPOCO.SpecialInstruction;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusCode))
            {
					entityPM.StatusCode = entityPOCO.StatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalPieces))
            {
					entityPM.TotalPieces = entityPOCO.TotalPieces;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalGrossWeight))
            {
					entityPM.TotalGrossWeight = entityPOCO.TotalGrossWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeightUnitCode))
            {
					entityPM.GrossWeightUnitCode = entityPOCO.GrossWeightUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalVolume))
            {
					entityPM.TotalVolume = entityPOCO.TotalVolume;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VolumeUnitCode))
            {
					entityPM.VolumeUnitCode = entityPOCO.VolumeUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Notes))
            {
					entityPM.Notes = entityPOCO.Notes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerRef1))
            {
					entityPM.CustomerRef1 = entityPOCO.CustomerRef1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerRef2))
            {
					entityPM.CustomerRef2 = entityPOCO.CustomerRef2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HouseNumber))
            {
					entityPM.HouseNumber = entityPOCO.HouseNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MasterNumber))
            {
					entityPM.MasterNumber = entityPOCO.MasterNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DimensionsUnitCode))
            {
					entityPM.DimensionsUnitCode = entityPOCO.DimensionsUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentLevelCode))
            {
					entityPM.ShipmentLevelCode = entityPOCO.ShipmentLevelCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransportModeId))
            {
					entityPM.TransportModeId = entityPOCO.TransportModeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromPortId))
            {
					entityPM.FromPortId = entityPOCO.FromPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToPortId))
            {
					entityPM.ToPortId = entityPOCO.ToPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TruckerId))
            {
					entityPM.TruckerId = entityPOCO.TruckerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TruckerReference))
            {
					entityPM.TruckerReference = entityPOCO.TruckerReference;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperId))
            {
					entityPM.ShipperId = entityPOCO.ShipperId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DirectionId))
            {
					entityPM.DirectionId = entityPOCO.DirectionId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentTypeId))
            {
					entityPM.ShipmentTypeId = entityPOCO.ShipmentTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntryReference))
            {
					entityPM.EntryReference = entityPOCO.EntryReference;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConnectedToShipment))
            {
					entityPM.ConnectedToShipment = entityPOCO.ConnectedToShipment;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromAddressId))
            {
					entityPM.FromAddressId = entityPOCO.FromAddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToAddressId))
            {
					entityPM.ToAddressId = entityPOCO.ToAddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeId))
            {
					entityPM.ConsigneeId = entityPOCO.ConsigneeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperReference1))
            {
					entityPM.ShipperReference1 = entityPOCO.ShipperReference1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeReference1))
            {
					entityPM.ConsigneeReference1 = entityPOCO.ConsigneeReference1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeReference2))
            {
					entityPM.ConsigneeReference2 = entityPOCO.ConsigneeReference2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperReference2))
            {
					entityPM.ShipperReference2 = entityPOCO.ShipperReference2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperName))
            {
					entityPM.ShipperName = entityPOCO.ShipperName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeName))
            {
					entityPM.ConsigneeName = entityPOCO.ConsigneeName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Manufacturer))
            {
					entityPM.Manufacturer = entityPOCO.Manufacturer;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromPartnerId))
            {
					entityPM.FromPartnerId = entityPOCO.FromPartnerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToPartnerId))
            {
					entityPM.ToPartnerId = entityPOCO.ToPartnerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChargeableWeightUnitCode))
            {
					entityPM.ChargeableWeightUnitCode = entityPOCO.ChargeableWeightUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalVolumetricWeight))
            {
					entityPM.TotalVolumetricWeight = entityPOCO.TotalVolumetricWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastStatusUpdateDate))
            {
					entityPM.LastStatusUpdateDate = entityPOCO.LastStatusUpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConnectedTo))
            {
					entityPM.ConnectedTo = entityPOCO.ConnectedTo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Ratio))
            {
					entityPM.Ratio = entityPOCO.Ratio;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToTypeCode))
            {
					entityPM.ToTypeCode = entityPOCO.ToTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromTypeCode))
            {
					entityPM.FromTypeCode = entityPOCO.FromTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromCountryId))
            {
					entityPM.FromCountryId = entityPOCO.FromCountryId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToCountryId))
            {
					entityPM.ToCountryId = entityPOCO.ToCountryId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MasterShipmentNumber))
            {
					entityPM.MasterShipmentNumber = entityPOCO.MasterShipmentNumber;
            }

		}

		public void PMToOldPM(WarehouseEntryPM entityPM, WarehouseEntryPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntryNumber))
            {
                oldEntityPM.EntryNumber = entityPM.EntryNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
                oldEntityPM.CustomerId = entityPM.CustomerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
                oldEntityPM.ShipmentId = entityPM.ShipmentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentNumber))
            {
                oldEntityPM.ShipmentNumber = entityPM.ShipmentNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WarehouseId))
            {
                oldEntityPM.WarehouseId = entityPM.WarehouseId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpectedEntryDate))
            {
                oldEntityPM.ExpectedEntryDate = entityPM.ExpectedEntryDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActualEntryDate))
            {
                oldEntityPM.ActualEntryDate = entityPM.ActualEntryDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReceivedBy))
            {
                oldEntityPM.ReceivedBy = entityPM.ReceivedBy;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecialInstruction))
            {
                oldEntityPM.SpecialInstruction = entityPM.SpecialInstruction;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
                oldEntityPM.StatusCode = entityPM.StatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalPieces))
            {
                oldEntityPM.TotalPieces = entityPM.TotalPieces;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalGrossWeight))
            {
                oldEntityPM.TotalGrossWeight = entityPM.TotalGrossWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightUnitCode))
            {
                oldEntityPM.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalVolume))
            {
                oldEntityPM.TotalVolume = entityPM.TotalVolume;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumeUnitCode))
            {
                oldEntityPM.VolumeUnitCode = entityPM.VolumeUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
                oldEntityPM.Notes = entityPM.Notes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerRef1))
            {
                oldEntityPM.CustomerRef1 = entityPM.CustomerRef1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerRef2))
            {
                oldEntityPM.CustomerRef2 = entityPM.CustomerRef2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HouseNumber))
            {
                oldEntityPM.HouseNumber = entityPM.HouseNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MasterNumber))
            {
                oldEntityPM.MasterNumber = entityPM.MasterNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DimensionsUnitCode))
            {
                oldEntityPM.DimensionsUnitCode = entityPM.DimensionsUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentLevelCode))
            {
                oldEntityPM.ShipmentLevelCode = entityPM.ShipmentLevelCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
                oldEntityPM.TransportModeId = entityPM.TransportModeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPortId))
            {
                oldEntityPM.FromPortId = entityPM.FromPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPortId))
            {
                oldEntityPM.ToPortId = entityPM.ToPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TruckerId))
            {
                oldEntityPM.TruckerId = entityPM.TruckerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TruckerReference))
            {
                oldEntityPM.TruckerReference = entityPM.TruckerReference;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperId))
            {
                oldEntityPM.ShipperId = entityPM.ShipperId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DirectionId))
            {
                oldEntityPM.DirectionId = entityPM.DirectionId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentTypeId))
            {
                oldEntityPM.ShipmentTypeId = entityPM.ShipmentTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntryReference))
            {
                oldEntityPM.EntryReference = entityPM.EntryReference;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConnectedToShipment))
            {
                oldEntityPM.ConnectedToShipment = entityPM.ConnectedToShipment;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressId))
            {
                oldEntityPM.FromAddressId = entityPM.FromAddressId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressId))
            {
                oldEntityPM.ToAddressId = entityPM.ToAddressId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeId))
            {
                oldEntityPM.ConsigneeId = entityPM.ConsigneeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperReference1))
            {
                oldEntityPM.ShipperReference1 = entityPM.ShipperReference1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeReference1))
            {
                oldEntityPM.ConsigneeReference1 = entityPM.ConsigneeReference1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeReference2))
            {
                oldEntityPM.ConsigneeReference2 = entityPM.ConsigneeReference2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperReference2))
            {
                oldEntityPM.ShipperReference2 = entityPM.ShipperReference2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperName))
            {
                oldEntityPM.ShipperName = entityPM.ShipperName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeName))
            {
                oldEntityPM.ConsigneeName = entityPM.ConsigneeName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Manufacturer))
            {
                oldEntityPM.Manufacturer = entityPM.Manufacturer;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPartnerId))
            {
                oldEntityPM.FromPartnerId = entityPM.FromPartnerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPartnerId))
            {
                oldEntityPM.ToPartnerId = entityPM.ToPartnerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightUnitCode))
            {
                oldEntityPM.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalVolumetricWeight))
            {
                oldEntityPM.TotalVolumetricWeight = entityPM.TotalVolumetricWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastStatusUpdateDate))
            {
                oldEntityPM.LastStatusUpdateDate = entityPM.LastStatusUpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConnectedTo))
            {
                oldEntityPM.ConnectedTo = entityPM.ConnectedTo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Ratio))
            {
                oldEntityPM.Ratio = entityPM.Ratio;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToTypeCode))
            {
                oldEntityPM.ToTypeCode = entityPM.ToTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromTypeCode))
            {
                oldEntityPM.FromTypeCode = entityPM.FromTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromCountryId))
            {
                oldEntityPM.FromCountryId = entityPM.FromCountryId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToCountryId))
            {
                oldEntityPM.ToCountryId = entityPM.ToCountryId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MasterShipmentNumber))
            {
                oldEntityPM.MasterShipmentNumber = entityPM.MasterShipmentNumber;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(WarehouseEntryPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Notes)) //T4 find type == nText 
            {
                entityPM.Notes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Notes));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ShipperName)) //T4 find type == nText 
            {
                entityPM.ShipperName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ShipperName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ConsigneeName)) //T4 find type == nText 
            {
                entityPM.ConsigneeName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ConsigneeName));
            }
            entityPM.EncodeBase64NVARCHARFieldsBy=null;
		}


	    public void AddPOCOPropertyName(POCOPropertyNames pocoPropertyName)
        {
            CustomMappedPOCOProperties.Add(pocoPropertyName);
        }

        public void AddPMPropertyName(PMPropertyNames pocoPropertyName)
        {
            CustomMappedPMProperties.Add(pocoPropertyName);
        }
		
		private void BuildSearchFieldsGenerated(WarehouseEntryPM entityPM, WarehouseEntry entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 