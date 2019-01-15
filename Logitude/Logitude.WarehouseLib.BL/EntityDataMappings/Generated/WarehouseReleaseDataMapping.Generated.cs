
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
   
   public partial class WarehouseReleaseDataMapping: IMapping<WarehouseReleasePM, WarehouseRelease>,IMappingEncodeBase64NVARCHARFields<WarehouseReleasePM>
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
	         ReleaseNumber, 
	         CustomerId, 
	         ShipmentId, 
	         ShipmentNumber, 
	         WarehouseId, 
	         ExpectedReleaseDate, 
	         ActualReleaseDate, 
	         ReleaseBy, 
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
	         ShipmentTypeId, 
	         TransportModeId, 
	         ShipmentLevelCode, 
	         SearchFields, 
	         DirectionId, 
	         TotalQuantity, 
	         ChargeableWeightUnitCode,
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
	         ReleaseNumber, 
	         CustomerId, 
	         ShipmentId, 
	         ShipmentNumber, 
	         WarehouseId, 
	         ExpectedReleaseDate, 
	         ActualReleaseDate, 
	         ReleaseBy, 
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
	         ShipmentTypeId, 
	         ShipmentNumberWithType, 
	         TransportModeId, 
	         ShipmentLevelCode, 
	         SearchFields, 
	         DirectionId, 
	         ReleaseDate, 
	         TotalQuantity, 
	         ChargeableWeightUnitCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(WarehouseReleasePM entityPM, WarehouseRelease entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReleaseNumber))
            {
				entityPOCO.ReleaseNumber = entityPM.ReleaseNumber;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpectedReleaseDate))
            {
				entityPOCO.ExpectedReleaseDate = entityPM.ExpectedReleaseDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActualReleaseDate))
            {
				entityPOCO.ActualReleaseDate = entityPM.ActualReleaseDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReleaseBy))
            {
				entityPOCO.ReleaseBy = entityPM.ReleaseBy;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentTypeId))
            {
				entityPOCO.ShipmentTypeId = entityPM.ShipmentTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
				entityPOCO.TransportModeId = entityPM.TransportModeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentLevelCode))
            {
				entityPOCO.ShipmentLevelCode = entityPM.ShipmentLevelCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DirectionId))
            {
				entityPOCO.DirectionId = entityPM.DirectionId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalQuantity))
            {
				entityPOCO.TotalQuantity = entityPM.TotalQuantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightUnitCode))
            {
				entityPOCO.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(WarehouseReleasePM entityPM, WarehouseRelease entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReleaseNumber))
            {
					entityPM.ReleaseNumber = entityPOCO.ReleaseNumber;
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExpectedReleaseDate))
            {
					entityPM.ExpectedReleaseDate = entityPOCO.ExpectedReleaseDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActualReleaseDate))
            {
					entityPM.ActualReleaseDate = entityPOCO.ActualReleaseDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReleaseBy))
            {
					entityPM.ReleaseBy = entityPOCO.ReleaseBy;
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentTypeId))
            {
					entityPM.ShipmentTypeId = entityPOCO.ShipmentTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransportModeId))
            {
					entityPM.TransportModeId = entityPOCO.TransportModeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentLevelCode))
            {
					entityPM.ShipmentLevelCode = entityPOCO.ShipmentLevelCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DirectionId))
            {
					entityPM.DirectionId = entityPOCO.DirectionId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalQuantity))
            {
					entityPM.TotalQuantity = entityPOCO.TotalQuantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChargeableWeightUnitCode))
            {
					entityPM.ChargeableWeightUnitCode = entityPOCO.ChargeableWeightUnitCode;
            }

		}

		public void PMToOldPM(WarehouseReleasePM entityPM, WarehouseReleasePM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReleaseNumber))
            {
                oldEntityPM.ReleaseNumber = entityPM.ReleaseNumber;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpectedReleaseDate))
            {
                oldEntityPM.ExpectedReleaseDate = entityPM.ExpectedReleaseDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActualReleaseDate))
            {
                oldEntityPM.ActualReleaseDate = entityPM.ActualReleaseDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReleaseBy))
            {
                oldEntityPM.ReleaseBy = entityPM.ReleaseBy;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentTypeId))
            {
                oldEntityPM.ShipmentTypeId = entityPM.ShipmentTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
                oldEntityPM.TransportModeId = entityPM.TransportModeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentLevelCode))
            {
                oldEntityPM.ShipmentLevelCode = entityPM.ShipmentLevelCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DirectionId))
            {
                oldEntityPM.DirectionId = entityPM.DirectionId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalQuantity))
            {
                oldEntityPM.TotalQuantity = entityPM.TotalQuantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightUnitCode))
            {
                oldEntityPM.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(WarehouseReleasePM entityPM)
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
		
		private void BuildSearchFieldsGenerated(WarehouseReleasePM entityPM, WarehouseRelease entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 