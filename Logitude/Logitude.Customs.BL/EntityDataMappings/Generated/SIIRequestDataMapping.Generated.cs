
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
   
   public partial class SIIRequestDataMapping: IMapping<SIIRequestPM, SIIRequest>,IMappingEncodeBase64NVARCHARFields<SIIRequestPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         RequestNo, 
	         DeclarationId, 
	         Status, 
	         WareHouseAddress, 
	         WareHouseCity, 
	         IsClosed, 
	         Remarks, 
	         ContactId, 
	         FromApplicationId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         RequestNo, 
	         DeclarationId, 
	         Status, 
	         WareHouseAddress, 
	         WareHouseCity, 
	         IsClosed, 
	         WareHouseCityName, 
	         Remarks, 
	         ListCounter, 
	         ImporterId, 
	         ContactName, 
	         UnloadDate, 
	         ManifestNumber, 
	         VesselName, 
	         ContactEmail, 
	         ContactTel, 
	         ContactCellPhone, 
	         ContactFax, 
	         ContactId, 
	         FromApplicationId, 
	         OriginCountryCode, 
	         UnloadPortCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SIIRequestPM entityPM, SIIRequest entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestNo))
            {
				entityPOCO.RequestNo = entityPM.RequestNo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationId))
            {
				entityPOCO.DeclarationId = entityPM.DeclarationId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Status))
            {
				entityPOCO.Status = entityPM.Status;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WareHouseAddress))
            {
				entityPOCO.WareHouseAddress = entityPM.WareHouseAddress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WareHouseCity))
            {
				entityPOCO.WareHouseCity = entityPM.WareHouseCity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
				entityPOCO.IsClosed = entityPM.IsClosed;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Remarks))
            {
				entityPOCO.Remarks = entityPM.Remarks;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactId))
            {
				entityPOCO.ContactId = entityPM.ContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromApplicationId))
            {
				entityPOCO.FromApplicationId = entityPM.FromApplicationId;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(SIIRequestPM entityPM, SIIRequest entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestNo))
            {
					entityPM.RequestNo = entityPOCO.RequestNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Status))
            {
					entityPM.Status = entityPOCO.Status;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WareHouseAddress))
            {
					entityPM.WareHouseAddress = entityPOCO.WareHouseAddress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WareHouseCity))
            {
					entityPM.WareHouseCity = entityPOCO.WareHouseCity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClosed))
            {
					entityPM.IsClosed = entityPOCO.IsClosed;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Remarks))
            {
					entityPM.Remarks = entityPOCO.Remarks;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContactId))
            {
					entityPM.ContactId = entityPOCO.ContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromApplicationId))
            {
					entityPM.FromApplicationId = entityPOCO.FromApplicationId;
            }

		}

		public void PMToOldPM(SIIRequestPM entityPM, SIIRequestPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestNo))
            {
                oldEntityPM.RequestNo = entityPM.RequestNo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationId))
            {
                oldEntityPM.DeclarationId = entityPM.DeclarationId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Status))
            {
                oldEntityPM.Status = entityPM.Status;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WareHouseAddress))
            {
                oldEntityPM.WareHouseAddress = entityPM.WareHouseAddress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WareHouseCity))
            {
                oldEntityPM.WareHouseCity = entityPM.WareHouseCity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
                oldEntityPM.IsClosed = entityPM.IsClosed;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Remarks))
            {
                oldEntityPM.Remarks = entityPM.Remarks;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactId))
            {
                oldEntityPM.ContactId = entityPM.ContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromApplicationId))
            {
                oldEntityPM.FromApplicationId = entityPM.FromApplicationId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(SIIRequestPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.WareHouseAddress)) //T4 find type == nText 
            {
                entityPM.WareHouseAddress = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.WareHouseAddress));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Remarks)) //T4 find type == nText 
            {
                entityPM.Remarks = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Remarks));
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
		
		private void BuildSearchFieldsGenerated(SIIRequestPM entityPM, SIIRequest entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 