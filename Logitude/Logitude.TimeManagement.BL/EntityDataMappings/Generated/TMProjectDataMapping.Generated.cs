
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
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.EntityPMs; 
using Logitude.TimeManagement.Data;

namespace Logitude.TimeManagement.BL.EntityDataMappings
{
   
   public partial class TMProjectDataMapping: IMapping<TMProjectPM, TMProject>,IMappingEncodeBase64NVARCHARFields<TMProjectPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Name, 
	         Description, 
	         CustomerId, 
	         SearchFields, 
	         OwnerId, 
	         ProjectNumber, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         IsInnerProject, 
	         Inactive, 
	         BudgetId, 
	         CategoryId, 
	         IsProrated, 
	         ExternalProjectNumber, 
	         ExcludeFromProrating, 
	         DayOffTypeCode, 
	         BlockedForDataEntry,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Name, 
	         Description, 
	         CustomerId, 
	         SearchFields, 
	         OwnerId, 
	         ProjectNumber, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         OwnerName, 
	         CustomerName, 
	         IsInnerProject, 
	         Inactive, 
	         BudgetId, 
	         CategoryId, 
	         IsProrated, 
	         ExternalProjectNumber, 
	         CategoryName, 
	         ExcludeFromProrating, 
	         DayOffTypeCode, 
	         BlockedForDataEntry,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TMProjectPM entityPM, TMProject entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
				entityPOCO.Name = entityPM.Name;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
				entityPOCO.Description = entityPM.Description;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
				entityPOCO.CustomerId = entityPM.CustomerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnerId))
            {
				entityPOCO.OwnerId = entityPM.OwnerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProjectNumber))
            {
				entityPOCO.ProjectNumber = entityPM.ProjectNumber;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsInnerProject))
            {
				entityPOCO.IsInnerProject = entityPM.IsInnerProject;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
				entityPOCO.Inactive = entityPM.Inactive;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BudgetId))
            {
				entityPOCO.BudgetId = entityPM.BudgetId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CategoryId))
            {
				entityPOCO.CategoryId = entityPM.CategoryId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsProrated))
            {
				entityPOCO.IsProrated = entityPM.IsProrated;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalProjectNumber))
            {
				entityPOCO.ExternalProjectNumber = entityPM.ExternalProjectNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExcludeFromProrating))
            {
				entityPOCO.ExcludeFromProrating = entityPM.ExcludeFromProrating;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DayOffTypeCode))
            {
				entityPOCO.DayOffTypeCode = entityPM.DayOffTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BlockedForDataEntry))
            {
				entityPOCO.BlockedForDataEntry = entityPM.BlockedForDataEntry;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(TMProjectPM entityPM, TMProject entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Name))
            {
					entityPM.Name = entityPOCO.Name;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Description))
            {
					entityPM.Description = entityPOCO.Description;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerId))
            {
					entityPM.CustomerId = entityPOCO.CustomerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OwnerId))
            {
					entityPM.OwnerId = entityPOCO.OwnerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProjectNumber))
            {
					entityPM.ProjectNumber = entityPOCO.ProjectNumber;
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsInnerProject))
            {
					entityPM.IsInnerProject = entityPOCO.IsInnerProject;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Inactive))
            {
					entityPM.Inactive = entityPOCO.Inactive;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BudgetId))
            {
					entityPM.BudgetId = entityPOCO.BudgetId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CategoryId))
            {
					entityPM.CategoryId = entityPOCO.CategoryId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsProrated))
            {
					entityPM.IsProrated = entityPOCO.IsProrated;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExternalProjectNumber))
            {
					entityPM.ExternalProjectNumber = entityPOCO.ExternalProjectNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExcludeFromProrating))
            {
					entityPM.ExcludeFromProrating = entityPOCO.ExcludeFromProrating;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DayOffTypeCode))
            {
					entityPM.DayOffTypeCode = entityPOCO.DayOffTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BlockedForDataEntry))
            {
					entityPM.BlockedForDataEntry = entityPOCO.BlockedForDataEntry;
            }

		}

		public void PMToOldPM(TMProjectPM entityPM, TMProjectPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
                oldEntityPM.Name = entityPM.Name;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
                oldEntityPM.Description = entityPM.Description;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
                oldEntityPM.CustomerId = entityPM.CustomerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnerId))
            {
                oldEntityPM.OwnerId = entityPM.OwnerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProjectNumber))
            {
                oldEntityPM.ProjectNumber = entityPM.ProjectNumber;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsInnerProject))
            {
                oldEntityPM.IsInnerProject = entityPM.IsInnerProject;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
                oldEntityPM.Inactive = entityPM.Inactive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BudgetId))
            {
                oldEntityPM.BudgetId = entityPM.BudgetId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CategoryId))
            {
                oldEntityPM.CategoryId = entityPM.CategoryId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsProrated))
            {
                oldEntityPM.IsProrated = entityPM.IsProrated;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalProjectNumber))
            {
                oldEntityPM.ExternalProjectNumber = entityPM.ExternalProjectNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExcludeFromProrating))
            {
                oldEntityPM.ExcludeFromProrating = entityPM.ExcludeFromProrating;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DayOffTypeCode))
            {
                oldEntityPM.DayOffTypeCode = entityPM.DayOffTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BlockedForDataEntry))
            {
                oldEntityPM.BlockedForDataEntry = entityPM.BlockedForDataEntry;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TMProjectPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Name)) //T4 find type == nText 
            {
                entityPM.Name = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Name));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Description)) //T4 find type == nText 
            {
                entityPM.Description = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Description));
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
		
		private void BuildSearchFieldsGenerated(TMProjectPM entityPM, TMProject entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 