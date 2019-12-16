
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
   
   public partial class GLAccountInterestPeriodDataMapping: IMapping<GLAccountInterestPeriodPM, GLAccountInterestPeriod>,IMappingEncodeBase64NVARCHARFields<GLAccountInterestPeriodPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         LineNumber, 
	         GLAccountId, 
	         PeriodStartDate, 
	         StandardInterestRateBaseId, 
	         StandardAddInterestPercent, 
	         ExceptionalInterestRateBaseId, 
	         ExceptionalAddInterestPercent, 
	         CreditInterestRateBaseId, 
	         CreditAddInterestPercent, 
	         UpdatedByUserId, 
	         UpdateDateTime, 
	         CreatedByUserId, 
	         CreateDateTime,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         LineNumber, 
	         GLAccountId, 
	         PeriodStartDate, 
	         StandardInterestRateBaseId, 
	         StandardAddInterestPercent, 
	         ExceptionalInterestRateBaseId, 
	         ExceptionalAddInterestPercent, 
	         CreditInterestRateBaseId, 
	         CreditAddInterestPercent, 
	         UpdatedByUserId, 
	         UpdateDateTime, 
	         CreatedByUserId, 
	         CreateDateTime, 
	         UpdatedByUserName, 
	         CreatedByUserName, 
	         ExceptionalInterestRateName, 
	         CreditInterestRateBaseName, 
	         StandardInterestRateBaseName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(GLAccountInterestPeriodPM entityPM, GLAccountInterestPeriod entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PeriodStartDate))
            {
				entityPOCO.PeriodStartDate = entityPM.PeriodStartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StandardInterestRateBaseId))
            {
				entityPOCO.StandardInterestRateBaseId = entityPM.StandardInterestRateBaseId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StandardAddInterestPercent))
            {
				entityPOCO.StandardAddInterestPercent = entityPM.StandardAddInterestPercent;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExceptionalInterestRateBaseId))
            {
				entityPOCO.ExceptionalInterestRateBaseId = entityPM.ExceptionalInterestRateBaseId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExceptionalAddInterestPercent))
            {
				entityPOCO.ExceptionalAddInterestPercent = entityPM.ExceptionalAddInterestPercent;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreditInterestRateBaseId))
            {
				entityPOCO.CreditInterestRateBaseId = entityPM.CreditInterestRateBaseId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreditAddInterestPercent))
            {
				entityPOCO.CreditAddInterestPercent = entityPM.CreditAddInterestPercent;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDateTime))
            {
				entityPOCO.UpdateDateTime = entityPM.UpdateDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDateTime))
            {
				entityPOCO.CreateDateTime = entityPM.CreateDateTime;
			}
			}

		public void POCOToPM(GLAccountInterestPeriodPM entityPM, GLAccountInterestPeriod entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GLAccountId))
            {
					entityPM.GLAccountId = entityPOCO.GLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PeriodStartDate))
            {
					entityPM.PeriodStartDate = entityPOCO.PeriodStartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StandardInterestRateBaseId))
            {
					entityPM.StandardInterestRateBaseId = entityPOCO.StandardInterestRateBaseId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StandardAddInterestPercent))
            {
					entityPM.StandardAddInterestPercent = entityPOCO.StandardAddInterestPercent;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExceptionalInterestRateBaseId))
            {
					entityPM.ExceptionalInterestRateBaseId = entityPOCO.ExceptionalInterestRateBaseId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExceptionalAddInterestPercent))
            {
					entityPM.ExceptionalAddInterestPercent = entityPOCO.ExceptionalAddInterestPercent;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreditInterestRateBaseId))
            {
					entityPM.CreditInterestRateBaseId = entityPOCO.CreditInterestRateBaseId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreditAddInterestPercent))
            {
					entityPM.CreditAddInterestPercent = entityPOCO.CreditAddInterestPercent;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDateTime))
            {
					entityPM.UpdateDateTime = entityPOCO.UpdateDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDateTime))
            {
					entityPM.CreateDateTime = entityPOCO.CreateDateTime;
            }

		}

		public void PMToOldPM(GLAccountInterestPeriodPM entityPM, GLAccountInterestPeriodPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PeriodStartDate))
            {
                oldEntityPM.PeriodStartDate = entityPM.PeriodStartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StandardInterestRateBaseId))
            {
                oldEntityPM.StandardInterestRateBaseId = entityPM.StandardInterestRateBaseId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StandardAddInterestPercent))
            {
                oldEntityPM.StandardAddInterestPercent = entityPM.StandardAddInterestPercent;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExceptionalInterestRateBaseId))
            {
                oldEntityPM.ExceptionalInterestRateBaseId = entityPM.ExceptionalInterestRateBaseId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExceptionalAddInterestPercent))
            {
                oldEntityPM.ExceptionalAddInterestPercent = entityPM.ExceptionalAddInterestPercent;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreditInterestRateBaseId))
            {
                oldEntityPM.CreditInterestRateBaseId = entityPM.CreditInterestRateBaseId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreditAddInterestPercent))
            {
                oldEntityPM.CreditAddInterestPercent = entityPM.CreditAddInterestPercent;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDateTime))
            {
                oldEntityPM.UpdateDateTime = entityPM.UpdateDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDateTime))
            {
                oldEntityPM.CreateDateTime = entityPM.CreateDateTime;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(GLAccountInterestPeriodPM entityPM)
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
	 