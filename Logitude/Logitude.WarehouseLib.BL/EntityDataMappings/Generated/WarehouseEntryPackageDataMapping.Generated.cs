
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
	         Make, 
	         Model, 
	         Year, 
	         Color, 
	         ChassisNumber, 
	         RegistrationNumber, 
	         CountryId, 
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
	         Make, 
	         Model, 
	         Year, 
	         Color, 
	         ChassisNumber, 
	         RegistrationNumber, 
	         CountryId, 
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Make))
            {
				entityPOCO.Make = entityPM.Make;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Model))
            {
				entityPOCO.Model = entityPM.Model;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Year))
            {
				entityPOCO.Year = entityPM.Year;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Color))
            {
				entityPOCO.Color = entityPM.Color;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChassisNumber))
            {
				entityPOCO.ChassisNumber = entityPM.ChassisNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RegistrationNumber))
            {
				entityPOCO.RegistrationNumber = entityPM.RegistrationNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryId))
            {
				entityPOCO.CountryId = entityPM.CountryId;
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Make))
            {
					entityPM.Make = entityPOCO.Make;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Model))
            {
					entityPM.Model = entityPOCO.Model;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Year))
            {
					entityPM.Year = entityPOCO.Year;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Color))
            {
					entityPM.Color = entityPOCO.Color;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChassisNumber))
            {
					entityPM.ChassisNumber = entityPOCO.ChassisNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RegistrationNumber))
            {
					entityPM.RegistrationNumber = entityPOCO.RegistrationNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CountryId))
            {
					entityPM.CountryId = entityPOCO.CountryId;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Make))
            {
                oldEntityPM.Make = entityPM.Make;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Model))
            {
                oldEntityPM.Model = entityPM.Model;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Year))
            {
                oldEntityPM.Year = entityPM.Year;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Color))
            {
                oldEntityPM.Color = entityPM.Color;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChassisNumber))
            {
                oldEntityPM.ChassisNumber = entityPM.ChassisNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RegistrationNumber))
            {
                oldEntityPM.RegistrationNumber = entityPM.RegistrationNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryId))
            {
                oldEntityPM.CountryId = entityPM.CountryId;
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
            if (!String.IsNullOrWhiteSpace(entityPM.Make)) //T4 find type == nText 
            {
                entityPM.Make = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Make));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Model)) //T4 find type == nText 
            {
                entityPM.Model = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Model));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Year)) //T4 find type == nText 
            {
                entityPM.Year = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Year));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Color)) //T4 find type == nText 
            {
                entityPM.Color = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Color));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ChassisNumber)) //T4 find type == nText 
            {
                entityPM.ChassisNumber = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ChassisNumber));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.RegistrationNumber)) //T4 find type == nText 
            {
                entityPM.RegistrationNumber = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.RegistrationNumber));
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
	 