
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ConsignmentDataMapping: IMapping<ConsignmentPM, Consignment>,IMappingEncodeBase64NVARCHARFields<ConsignmentPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Tenant, 
	         ConsignmentNumber, 
	         SequenceNumeric, 
	         CargoTypeCode, 
	         ManifestDate, 
	         ManifestNumber, 
	         SecondCargoID, 
	         ThirdCargoID, 
	         UnloadDate, 
	         UnloadPortCode, 
	         CargoDescription, 
	         IsLastReleaseFromWarehous, 
	         LoadingPortCode, 
	         OriginCountryCode, 
	         StorageSiteCode, 
	         ReceiverWarehouseCode, 
	         DeliveryPlaceName, 
	         IsDangerousGoods, 
	         FinalDestinationPortCode, 
	         ExportRecieverWareHouseCode, 
	         ExportUnloadingPortCode, 
	         ExportLoadingPortCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Tenant, 
	         ConsignmentNumber, 
	         SequenceNumeric, 
	         CargoTypeCode, 
	         CargoTypeName, 
	         ManifestDate, 
	         ManifestNumber, 
	         SecondCargoID, 
	         ThirdCargoID, 
	         UnloadDate, 
	         UnloadPortCode, 
	         UnloadPortName, 
	         CargoDescription, 
	         IsLastReleaseFromWarehous, 
	         LoadingPortCode, 
	         OriginCountryCode, 
	         OriginCountryName, 
	         StorageSiteCode, 
	         StorageSiteName, 
	         ReceiverWarehouseCode, 
	         ReceiverWarehouseName, 
	         CargoDate, 
	         DeliveryPlaceName, 
	         IsDangerousGoods, 
	         FinalDestinationPortCode, 
	         FinalDestinationPortName, 
	         ExportRecieverWareHouseCode, 
	         ExportRecieverWareHouseName, 
	         ExportUnloadingPortCode, 
	         ExportLoadingPortCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ConsignmentPM entityPM, Consignment entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
				entityPOCO.SequenceNumeric = entityPM.SequenceNumeric;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoTypeCode))
            {
				entityPOCO.CargoTypeCode = entityPM.CargoTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManifestDate))
            {
				entityPOCO.ManifestDate = entityPM.ManifestDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManifestNumber))
            {
				entityPOCO.ManifestNumber = entityPM.ManifestNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecondCargoID))
            {
				entityPOCO.SecondCargoID = entityPM.SecondCargoID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ThirdCargoID))
            {
				entityPOCO.ThirdCargoID = entityPM.ThirdCargoID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnloadDate))
            {
				entityPOCO.UnloadDate = entityPM.UnloadDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnloadPortCode))
            {
				entityPOCO.UnloadPortCode = entityPM.UnloadPortCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoDescription))
            {
				entityPOCO.CargoDescription = entityPM.CargoDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsLastReleaseFromWarehous))
            {
				entityPOCO.IsLastReleaseFromWarehous = entityPM.IsLastReleaseFromWarehous;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LoadingPortCode))
            {
				entityPOCO.LoadingPortCode = entityPM.LoadingPortCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginCountryCode))
            {
				entityPOCO.OriginCountryCode = entityPM.OriginCountryCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageSiteCode))
            {
				entityPOCO.StorageSiteCode = entityPM.StorageSiteCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReceiverWarehouseCode))
            {
				entityPOCO.ReceiverWarehouseCode = entityPM.ReceiverWarehouseCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryPlaceName))
            {
				entityPOCO.DeliveryPlaceName = entityPM.DeliveryPlaceName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDangerousGoods))
            {
				entityPOCO.IsDangerousGoods = entityPM.IsDangerousGoods;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalDestinationPortCode))
            {
				entityPOCO.FinalDestinationPortCode = entityPM.FinalDestinationPortCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportRecieverWareHouseCode))
            {
				entityPOCO.ExportRecieverWareHouseCode = entityPM.ExportRecieverWareHouseCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportUnloadingPortCode))
            {
				entityPOCO.ExportUnloadingPortCode = entityPM.ExportUnloadingPortCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportLoadingPortCode))
            {
				entityPOCO.ExportLoadingPortCode = entityPM.ExportLoadingPortCode;
			}
			}

		public void POCOToPM(ConsignmentPM entityPM, Consignment entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsignmentNumber))
            {
					entityPM.ConsignmentNumber = entityPOCO.ConsignmentNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SequenceNumeric))
            {
					entityPM.SequenceNumeric = entityPOCO.SequenceNumeric;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoTypeCode))
            {
					entityPM.CargoTypeCode = entityPOCO.CargoTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ManifestDate))
            {
					entityPM.ManifestDate = entityPOCO.ManifestDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ManifestNumber))
            {
					entityPM.ManifestNumber = entityPOCO.ManifestNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SecondCargoID))
            {
					entityPM.SecondCargoID = entityPOCO.SecondCargoID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ThirdCargoID))
            {
					entityPM.ThirdCargoID = entityPOCO.ThirdCargoID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UnloadDate))
            {
					entityPM.UnloadDate = entityPOCO.UnloadDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UnloadPortCode))
            {
					entityPM.UnloadPortCode = entityPOCO.UnloadPortCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoDescription))
            {
					entityPM.CargoDescription = entityPOCO.CargoDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsLastReleaseFromWarehous))
            {
					entityPM.IsLastReleaseFromWarehous = entityPOCO.IsLastReleaseFromWarehous;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LoadingPortCode))
            {
					entityPM.LoadingPortCode = entityPOCO.LoadingPortCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginCountryCode))
            {
					entityPM.OriginCountryCode = entityPOCO.OriginCountryCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StorageSiteCode))
            {
					entityPM.StorageSiteCode = entityPOCO.StorageSiteCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReceiverWarehouseCode))
            {
					entityPM.ReceiverWarehouseCode = entityPOCO.ReceiverWarehouseCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeliveryPlaceName))
            {
					entityPM.DeliveryPlaceName = entityPOCO.DeliveryPlaceName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDangerousGoods))
            {
					entityPM.IsDangerousGoods = entityPOCO.IsDangerousGoods;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FinalDestinationPortCode))
            {
					entityPM.FinalDestinationPortCode = entityPOCO.FinalDestinationPortCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExportRecieverWareHouseCode))
            {
					entityPM.ExportRecieverWareHouseCode = entityPOCO.ExportRecieverWareHouseCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExportUnloadingPortCode))
            {
					entityPM.ExportUnloadingPortCode = entityPOCO.ExportUnloadingPortCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExportLoadingPortCode))
            {
					entityPM.ExportLoadingPortCode = entityPOCO.ExportLoadingPortCode;
            }

		}

		public void PMToOldPM(ConsignmentPM entityPM, ConsignmentPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
                oldEntityPM.SequenceNumeric = entityPM.SequenceNumeric;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoTypeCode))
            {
                oldEntityPM.CargoTypeCode = entityPM.CargoTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManifestDate))
            {
                oldEntityPM.ManifestDate = entityPM.ManifestDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManifestNumber))
            {
                oldEntityPM.ManifestNumber = entityPM.ManifestNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecondCargoID))
            {
                oldEntityPM.SecondCargoID = entityPM.SecondCargoID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ThirdCargoID))
            {
                oldEntityPM.ThirdCargoID = entityPM.ThirdCargoID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnloadDate))
            {
                oldEntityPM.UnloadDate = entityPM.UnloadDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnloadPortCode))
            {
                oldEntityPM.UnloadPortCode = entityPM.UnloadPortCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoDescription))
            {
                oldEntityPM.CargoDescription = entityPM.CargoDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsLastReleaseFromWarehous))
            {
                oldEntityPM.IsLastReleaseFromWarehous = entityPM.IsLastReleaseFromWarehous;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LoadingPortCode))
            {
                oldEntityPM.LoadingPortCode = entityPM.LoadingPortCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginCountryCode))
            {
                oldEntityPM.OriginCountryCode = entityPM.OriginCountryCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageSiteCode))
            {
                oldEntityPM.StorageSiteCode = entityPM.StorageSiteCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReceiverWarehouseCode))
            {
                oldEntityPM.ReceiverWarehouseCode = entityPM.ReceiverWarehouseCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryPlaceName))
            {
                oldEntityPM.DeliveryPlaceName = entityPM.DeliveryPlaceName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDangerousGoods))
            {
                oldEntityPM.IsDangerousGoods = entityPM.IsDangerousGoods;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalDestinationPortCode))
            {
                oldEntityPM.FinalDestinationPortCode = entityPM.FinalDestinationPortCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportRecieverWareHouseCode))
            {
                oldEntityPM.ExportRecieverWareHouseCode = entityPM.ExportRecieverWareHouseCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportUnloadingPortCode))
            {
                oldEntityPM.ExportUnloadingPortCode = entityPM.ExportUnloadingPortCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportLoadingPortCode))
            {
                oldEntityPM.ExportLoadingPortCode = entityPM.ExportLoadingPortCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ConsignmentPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.CargoDescription)) //T4 find type == nText 
            {
                entityPM.CargoDescription = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CargoDescription));
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
	 