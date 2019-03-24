
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
   
   public partial class WarehouseEntryPackageDataMapping: IMapping<WarehouseEntryPackagePM, WarehouseEntryPackage>,IMappingEncodeBase64NVARCHARFields<WarehouseEntryPackagePM>
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
	         WarehouseEntryId, 
	         ContainerNumber, 
	         Quantity, 
	         Weight, 
	         Volume, 
	         Description, 
	         PackageTypeId, 
	         Seal, 
	         Harmonize, 
	         Width, 
	         Length, 
	         Height, 
	         IsContainer, 
	         Instock, 
	         Location, 
	         IsConnectedToShipment, 
	         VolumetricWeight, 
	         CommodityNumber,
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
	         WarehouseEntryId, 
	         ContainerNumber, 
	         Quantity, 
	         Weight, 
	         Volume, 
	         Description, 
	         PackageTypeId, 
	         Seal, 
	         Harmonize, 
	         Width, 
	         Length, 
	         Height, 
	         PackageTypeName, 
	         Dimensions, 
	         IsContainer, 
	         Instock, 
	         IsSelected, 
	         ReleaseQTY, 
	         CustomerId, 
	         WarehouseId, 
	         InstockTemp, 
	         ContainerNumberWarning, 
	         ActualEntryDate, 
	         Location, 
	         DimensionUnitCode, 
	         VolumeUnitCode, 
	         GrossWeightUnitCode, 
	         IsConnectedToShipment, 
	         DirectionId, 
	         TransportModeId, 
	         FromPortId, 
	         ToPortId, 
	         VolumetricWeight, 
	         ChargeableWeightUnitCode, 
	         CommodityNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(WarehouseEntryPackagePM entityPM, WarehouseEntryPackage entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WarehouseEntryId))
            {
				entityPOCO.WarehouseEntryId = entityPM.WarehouseEntryId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerNumber))
            {
				entityPOCO.ContainerNumber = entityPM.ContainerNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
				entityPOCO.Quantity = entityPM.Quantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Weight))
            {
				entityPOCO.Weight = entityPM.Weight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
				entityPOCO.Volume = entityPM.Volume;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
				entityPOCO.Description = entityPM.Description;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageTypeId))
            {
				entityPOCO.PackageTypeId = entityPM.PackageTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Seal))
            {
				entityPOCO.Seal = entityPM.Seal;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Harmonize))
            {
				entityPOCO.Harmonize = entityPM.Harmonize;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Width))
            {
				entityPOCO.Width = entityPM.Width;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Length))
            {
				entityPOCO.Length = entityPM.Length;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Height))
            {
				entityPOCO.Height = entityPM.Height;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsContainer))
            {
				entityPOCO.IsContainer = entityPM.IsContainer;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Instock))
            {
				entityPOCO.Instock = entityPM.Instock;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Location))
            {
				entityPOCO.Location = entityPM.Location;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConnectedToShipment))
            {
				entityPOCO.IsConnectedToShipment = entityPM.IsConnectedToShipment;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumetricWeight))
            {
				entityPOCO.VolumetricWeight = entityPM.VolumetricWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommodityNumber))
            {
				entityPOCO.CommodityNumber = entityPM.CommodityNumber;
			}
			}

		public void POCOToPM(WarehouseEntryPackagePM entityPM, WarehouseEntryPackage entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WarehouseEntryId))
            {
					entityPM.WarehouseEntryId = entityPOCO.WarehouseEntryId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerNumber))
            {
					entityPM.ContainerNumber = entityPOCO.ContainerNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Quantity))
            {
					entityPM.Quantity = entityPOCO.Quantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Weight))
            {
					entityPM.Weight = entityPOCO.Weight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Volume))
            {
					entityPM.Volume = entityPOCO.Volume;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Description))
            {
					entityPM.Description = entityPOCO.Description;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageTypeId))
            {
					entityPM.PackageTypeId = entityPOCO.PackageTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Seal))
            {
					entityPM.Seal = entityPOCO.Seal;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Harmonize))
            {
					entityPM.Harmonize = entityPOCO.Harmonize;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Width))
            {
					entityPM.Width = entityPOCO.Width;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Length))
            {
					entityPM.Length = entityPOCO.Length;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Height))
            {
					entityPM.Height = entityPOCO.Height;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsContainer))
            {
					entityPM.IsContainer = entityPOCO.IsContainer;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Instock))
            {
					entityPM.Instock = entityPOCO.Instock;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Location))
            {
					entityPM.Location = entityPOCO.Location;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsConnectedToShipment))
            {
					entityPM.IsConnectedToShipment = entityPOCO.IsConnectedToShipment;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VolumetricWeight))
            {
					entityPM.VolumetricWeight = entityPOCO.VolumetricWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CommodityNumber))
            {
					entityPM.CommodityNumber = entityPOCO.CommodityNumber;
            }

		}

		public void PMToOldPM(WarehouseEntryPackagePM entityPM, WarehouseEntryPackagePM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WarehouseEntryId))
            {
                oldEntityPM.WarehouseEntryId = entityPM.WarehouseEntryId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerNumber))
            {
                oldEntityPM.ContainerNumber = entityPM.ContainerNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
                oldEntityPM.Quantity = entityPM.Quantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Weight))
            {
                oldEntityPM.Weight = entityPM.Weight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
                oldEntityPM.Volume = entityPM.Volume;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
                oldEntityPM.Description = entityPM.Description;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageTypeId))
            {
                oldEntityPM.PackageTypeId = entityPM.PackageTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Seal))
            {
                oldEntityPM.Seal = entityPM.Seal;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Harmonize))
            {
                oldEntityPM.Harmonize = entityPM.Harmonize;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Width))
            {
                oldEntityPM.Width = entityPM.Width;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Length))
            {
                oldEntityPM.Length = entityPM.Length;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Height))
            {
                oldEntityPM.Height = entityPM.Height;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsContainer))
            {
                oldEntityPM.IsContainer = entityPM.IsContainer;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Instock))
            {
                oldEntityPM.Instock = entityPM.Instock;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Location))
            {
                oldEntityPM.Location = entityPM.Location;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConnectedToShipment))
            {
                oldEntityPM.IsConnectedToShipment = entityPM.IsConnectedToShipment;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumetricWeight))
            {
                oldEntityPM.VolumetricWeight = entityPM.VolumetricWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommodityNumber))
            {
                oldEntityPM.CommodityNumber = entityPM.CommodityNumber;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(WarehouseEntryPackagePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

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
			  
   }
}
	 