
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
   
   public partial class ExportStorageDataMapping: IMapping<ExportStoragePM, ExportStorage>,IMappingEncodeBase64NVARCHARFields<ExportStoragePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         DeclarationId, 
	         ExportFileNo, 
	         StorageStatus, 
	         CargoTypeCode, 
	         OpenDate, 
	         CargoType, 
	         CustomsStatus, 
	         ExporterID, 
	         ShipCode, 
	         FirstCargoID, 
	         SecondCargoID, 
	         ThirdCargoID, 
	         StorErrorXML,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         DeclarationId, 
	         ExportFileNo, 
	         StorageStatus, 
	         CargoTypeCode, 
	         OpenDate, 
	         CargoType, 
	         CustomsStatus, 
	         ExporterID, 
	         ShipCode, 
	         FirstCargoID, 
	         SecondCargoID, 
	         ThirdCargoID, 
	         DeclarationStatusTypeName, 
	         CargoTypeName, 
	         StorageStatusName, 
	         ExporterName, 
	         ShipName, 
	         StorErrorXML,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ExportStoragePM entityPM, ExportStorage entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationId))
            {
				entityPOCO.DeclarationId = entityPM.DeclarationId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportFileNo))
            {
				entityPOCO.ExportFileNo = entityPM.ExportFileNo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageStatus))
            {
				entityPOCO.StorageStatus = entityPM.StorageStatus;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoTypeCode))
            {
				entityPOCO.CargoTypeCode = entityPM.CargoTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenDate))
            {
				entityPOCO.OpenDate = entityPM.OpenDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoType))
            {
				entityPOCO.CargoType = entityPM.CargoType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsStatus))
            {
				entityPOCO.CustomsStatus = entityPM.CustomsStatus;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterID))
            {
				entityPOCO.ExporterID = entityPM.ExporterID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipCode))
            {
				entityPOCO.ShipCode = entityPM.ShipCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstCargoID))
            {
				entityPOCO.FirstCargoID = entityPM.FirstCargoID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecondCargoID))
            {
				entityPOCO.SecondCargoID = entityPM.SecondCargoID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ThirdCargoID))
            {
				entityPOCO.ThirdCargoID = entityPM.ThirdCargoID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorErrorXML))
            {
				entityPOCO.StorErrorXML = entityPM.StorErrorXML;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(ExportStoragePM entityPM, ExportStorage entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExportFileNo))
            {
					entityPM.ExportFileNo = entityPOCO.ExportFileNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StorageStatus))
            {
					entityPM.StorageStatus = entityPOCO.StorageStatus;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoTypeCode))
            {
					entityPM.CargoTypeCode = entityPOCO.CargoTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpenDate))
            {
					entityPM.OpenDate = entityPOCO.OpenDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoType))
            {
					entityPM.CargoType = entityPOCO.CargoType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsStatus))
            {
					entityPM.CustomsStatus = entityPOCO.CustomsStatus;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExporterID))
            {
					entityPM.ExporterID = entityPOCO.ExporterID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipCode))
            {
					entityPM.ShipCode = entityPOCO.ShipCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FirstCargoID))
            {
					entityPM.FirstCargoID = entityPOCO.FirstCargoID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SecondCargoID))
            {
					entityPM.SecondCargoID = entityPOCO.SecondCargoID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ThirdCargoID))
            {
					entityPM.ThirdCargoID = entityPOCO.ThirdCargoID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StorErrorXML))
            {
					entityPM.StorErrorXML = entityPOCO.StorErrorXML;
            }

		}

		public void PMToOldPM(ExportStoragePM entityPM, ExportStoragePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationId))
            {
                oldEntityPM.DeclarationId = entityPM.DeclarationId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportFileNo))
            {
                oldEntityPM.ExportFileNo = entityPM.ExportFileNo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageStatus))
            {
                oldEntityPM.StorageStatus = entityPM.StorageStatus;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoTypeCode))
            {
                oldEntityPM.CargoTypeCode = entityPM.CargoTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenDate))
            {
                oldEntityPM.OpenDate = entityPM.OpenDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoType))
            {
                oldEntityPM.CargoType = entityPM.CargoType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsStatus))
            {
                oldEntityPM.CustomsStatus = entityPM.CustomsStatus;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterID))
            {
                oldEntityPM.ExporterID = entityPM.ExporterID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipCode))
            {
                oldEntityPM.ShipCode = entityPM.ShipCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstCargoID))
            {
                oldEntityPM.FirstCargoID = entityPM.FirstCargoID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecondCargoID))
            {
                oldEntityPM.SecondCargoID = entityPM.SecondCargoID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ThirdCargoID))
            {
                oldEntityPM.ThirdCargoID = entityPM.ThirdCargoID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorErrorXML))
            {
                oldEntityPM.StorErrorXML = entityPM.StorErrorXML;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ExportStoragePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ExportFileNo)) //T4 find type == nText 
            {
                entityPM.ExportFileNo = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ExportFileNo));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CustomsStatus)) //T4 find type == nText 
            {
                entityPM.CustomsStatus = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CustomsStatus));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.StorErrorXML)) //T4 find type == nText 
            {
                entityPM.StorErrorXML = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.StorErrorXML));
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
		
		private void BuildSearchFieldsGenerated(ExportStoragePM entityPM, ExportStorage entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 