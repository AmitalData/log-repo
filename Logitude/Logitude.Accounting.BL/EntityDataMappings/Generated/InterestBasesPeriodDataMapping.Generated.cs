
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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class InterestBasesPeriodDataMapping: IMapping<InterestBasesPeriodPM, InterestBasesPeriod>,IMappingEncodeBase64NVARCHARFields<InterestBasesPeriodPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         InterestBaseTypeId, 
	         LineNumber, 
	         InterestBaseStartDate, 
	         InterestRate,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         InterestBaseTypeId, 
	         LineNumber, 
	         InterestBaseStartDate, 
	         InterestRate,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(InterestBasesPeriodPM entityPM, InterestBasesPeriod entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestBaseStartDate))
            {
				entityPOCO.InterestBaseStartDate = entityPM.InterestBaseStartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestRate))
            {
				entityPOCO.InterestRate = entityPM.InterestRate;
			}
			}

		public void POCOToPM(InterestBasesPeriodPM entityPM, InterestBasesPeriod entityPOCO)
        {
			 
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterestBaseTypeId))
            {
					entityPM.InterestBaseTypeId = entityPOCO.InterestBaseTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterestBaseStartDate))
            {
					entityPM.InterestBaseStartDate = entityPOCO.InterestBaseStartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterestRate))
            {
					entityPM.InterestRate = entityPOCO.InterestRate;
            }

		}

		public void PMToOldPM(InterestBasesPeriodPM entityPM, InterestBasesPeriodPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestBaseStartDate))
            {
                oldEntityPM.InterestBaseStartDate = entityPM.InterestBaseStartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestRate))
            {
                oldEntityPM.InterestRate = entityPM.InterestRate;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(InterestBasesPeriodPM entityPM)
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
	 