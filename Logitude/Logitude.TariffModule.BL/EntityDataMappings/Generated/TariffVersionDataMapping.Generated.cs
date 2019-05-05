
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
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs; 
using Logitude.TariffModule.Data;

namespace Logitude.TariffModule.BL.EntityDataMappings
{
   
   public partial class TariffVersionDataMapping: IMapping<TariffVersionPM, TariffVersion>,IMappingEncodeBase64NVARCHARFields<TariffVersionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         TariffId, 
	         Tenant, 
	         CreatedByUserId, 
	         SearchFields, 
	         StartDate, 
	         ExpirationDate, 
	         CreateDate, 
	         Version, 
	         IsDraft, 
	         ApproveDate, 
	         ApprovedByUserId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         TariffId, 
	         Tenant, 
	         CreatedByUserId, 
	         SearchFields, 
	         StartDate, 
	         ExpirationDate, 
	         CreateDate, 
	         Version, 
	         IsDraft, 
	         ApproveDate, 
	         ApprovedByUserId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TariffVersionPM entityPM, TariffVersion entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
				entityPOCO.StartDate = entityPM.StartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpirationDate))
            {
				entityPOCO.ExpirationDate = entityPM.ExpirationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDraft))
            {
				entityPOCO.IsDraft = entityPM.IsDraft;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApproveDate))
            {
				entityPOCO.ApproveDate = entityPM.ApproveDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApprovedByUserId))
            {
				entityPOCO.ApprovedByUserId = entityPM.ApprovedByUserId;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(TariffVersionPM entityPM, TariffVersion entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TariffId))
            {
					entityPM.TariffId = entityPOCO.TariffId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDate))
            {
					entityPM.StartDate = entityPOCO.StartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExpirationDate))
            {
					entityPM.ExpirationDate = entityPOCO.ExpirationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Version))
            {
					entityPM.Version = entityPOCO.Version;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDraft))
            {
					entityPM.IsDraft = entityPOCO.IsDraft;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ApproveDate))
            {
					entityPM.ApproveDate = entityPOCO.ApproveDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ApprovedByUserId))
            {
					entityPM.ApprovedByUserId = entityPOCO.ApprovedByUserId;
            }

		}

		public void PMToOldPM(TariffVersionPM entityPM, TariffVersionPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
                oldEntityPM.StartDate = entityPM.StartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpirationDate))
            {
                oldEntityPM.ExpirationDate = entityPM.ExpirationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDraft))
            {
                oldEntityPM.IsDraft = entityPM.IsDraft;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApproveDate))
            {
                oldEntityPM.ApproveDate = entityPM.ApproveDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApprovedByUserId))
            {
                oldEntityPM.ApprovedByUserId = entityPM.ApprovedByUserId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TariffVersionPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

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
		
		private void BuildSearchFieldsGenerated(TariffVersionPM entityPM, TariffVersion entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 