
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
   
   public partial class GLAccountAgingDataDataMapping: IMapping<GLAccountAgingDataPM, GLAccountAgingData>,IMappingEncodeBase64NVARCHARFields<GLAccountAgingDataPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         AccountId, 
	         Tenant, 
	         PeriodPast, 
	         Period5, 
	         Period4, 
	         Period3, 
	         Period2, 
	         Period1, 
	         Period0, 
	         PeriodFuture, 
	         TotalOpenTransactions,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         AccountId, 
	         Tenant, 
	         PeriodPast, 
	         Period5, 
	         Period4, 
	         Period3, 
	         Period2, 
	         Period1, 
	         Period0, 
	         PeriodFuture, 
	         TotalOpenTransactions,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(GLAccountAgingDataPM entityPM, GLAccountAgingData entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PeriodPast))
            {
				entityPOCO.PeriodPast = entityPM.PeriodPast;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Period5))
            {
				entityPOCO.Period5 = entityPM.Period5;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Period4))
            {
				entityPOCO.Period4 = entityPM.Period4;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Period3))
            {
				entityPOCO.Period3 = entityPM.Period3;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Period2))
            {
				entityPOCO.Period2 = entityPM.Period2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Period1))
            {
				entityPOCO.Period1 = entityPM.Period1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Period0))
            {
				entityPOCO.Period0 = entityPM.Period0;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PeriodFuture))
            {
				entityPOCO.PeriodFuture = entityPM.PeriodFuture;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalOpenTransactions))
            {
				entityPOCO.TotalOpenTransactions = entityPM.TotalOpenTransactions;
			}
			}

		public void POCOToPM(GLAccountAgingDataPM entityPM, GLAccountAgingData entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountId))
            {
					entityPM.AccountId = entityPOCO.AccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PeriodPast))
            {
					entityPM.PeriodPast = entityPOCO.PeriodPast;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Period5))
            {
					entityPM.Period5 = entityPOCO.Period5;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Period4))
            {
					entityPM.Period4 = entityPOCO.Period4;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Period3))
            {
					entityPM.Period3 = entityPOCO.Period3;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Period2))
            {
					entityPM.Period2 = entityPOCO.Period2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Period1))
            {
					entityPM.Period1 = entityPOCO.Period1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Period0))
            {
					entityPM.Period0 = entityPOCO.Period0;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PeriodFuture))
            {
					entityPM.PeriodFuture = entityPOCO.PeriodFuture;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalOpenTransactions))
            {
					entityPM.TotalOpenTransactions = entityPOCO.TotalOpenTransactions;
            }

		}

		public void PMToOldPM(GLAccountAgingDataPM entityPM, GLAccountAgingDataPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PeriodPast))
            {
                oldEntityPM.PeriodPast = entityPM.PeriodPast;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Period5))
            {
                oldEntityPM.Period5 = entityPM.Period5;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Period4))
            {
                oldEntityPM.Period4 = entityPM.Period4;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Period3))
            {
                oldEntityPM.Period3 = entityPM.Period3;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Period2))
            {
                oldEntityPM.Period2 = entityPM.Period2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Period1))
            {
                oldEntityPM.Period1 = entityPM.Period1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Period0))
            {
                oldEntityPM.Period0 = entityPM.Period0;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PeriodFuture))
            {
                oldEntityPM.PeriodFuture = entityPM.PeriodFuture;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalOpenTransactions))
            {
                oldEntityPM.TotalOpenTransactions = entityPM.TotalOpenTransactions;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(GLAccountAgingDataPM entityPM)
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
	 