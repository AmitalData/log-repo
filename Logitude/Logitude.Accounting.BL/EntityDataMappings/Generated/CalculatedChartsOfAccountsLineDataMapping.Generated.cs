
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
   
   public partial class CalculatedChartsOfAccountsLineDataMapping: IMapping<CalculatedChartsOfAccountsLinePM, CalculatedChartsOfAccountsLine>,IMappingEncodeBase64NVARCHARFields<CalculatedChartsOfAccountsLinePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDateTime, 
	         CreatedByUserId, 
	         UpdatedDateTime, 
	         UpdatedByUserId, 
	         IsDetailedGLAccount, 
	         IsCancelled, 
	         GLAccountId, 
	         ChartOfAccountId, 
	         LineTypeCode, 
	         CalculatedChartsOfAccountsId, 
	         Line,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDateTime, 
	         CreatedByUserId, 
	         UpdatedDateTime, 
	         UpdatedByUserId, 
	         IsDetailedGLAccount, 
	         IsCancelled, 
	         GLAccountId, 
	         ChartOfAccountId, 
	         LineTypeCode, 
	         CalculatedChartsOfAccountsId, 
	         CreatedByLocalName, 
	         CreatedByEnglishName, 
	         UpdatedByEnglishName, 
	         UpdatedByLocalName, 
	         GLAccountEnglishName, 
	         GLAccountLocalName, 
	         ChartOfAccountEnglishName, 
	         ChartOfAccountLocalName, 
	         LineTypeEnglishName, 
	         LineTypeLocalName, 
	         Line, 
	         ErrorLog, 
	         ChartOfAccountIdForValidate,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CalculatedChartsOfAccountsLinePM entityPM, CalculatedChartsOfAccountsLine entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDateTime))
            {
				entityPOCO.CreateDateTime = entityPM.CreateDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedDateTime))
            {
				entityPOCO.UpdatedDateTime = entityPM.UpdatedDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDetailedGLAccount))
            {
				entityPOCO.IsDetailedGLAccount = entityPM.IsDetailedGLAccount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
				entityPOCO.IsCancelled = entityPM.IsCancelled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GLAccountId))
            {
				entityPOCO.GLAccountId = entityPM.GLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChartOfAccountId))
            {
				entityPOCO.ChartOfAccountId = entityPM.ChartOfAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineTypeCode))
            {
				entityPOCO.LineTypeCode = entityPM.LineTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CalculatedChartsOfAccountsId))
            {
				entityPOCO.CalculatedChartsOfAccountsId = entityPM.CalculatedChartsOfAccountsId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Line))
            {
				entityPOCO.Line = entityPM.Line;
			}
			}

		public void POCOToPM(CalculatedChartsOfAccountsLinePM entityPM, CalculatedChartsOfAccountsLine entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDateTime))
            {
					entityPM.CreateDateTime = entityPOCO.CreateDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedDateTime))
            {
					entityPM.UpdatedDateTime = entityPOCO.UpdatedDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDetailedGLAccount))
            {
					entityPM.IsDetailedGLAccount = entityPOCO.IsDetailedGLAccount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCancelled))
            {
					entityPM.IsCancelled = entityPOCO.IsCancelled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GLAccountId))
            {
					entityPM.GLAccountId = entityPOCO.GLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChartOfAccountId))
            {
					entityPM.ChartOfAccountId = entityPOCO.ChartOfAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineTypeCode))
            {
					entityPM.LineTypeCode = entityPOCO.LineTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CalculatedChartsOfAccountsId))
            {
					entityPM.CalculatedChartsOfAccountsId = entityPOCO.CalculatedChartsOfAccountsId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Line))
            {
					entityPM.Line = entityPOCO.Line;
            }

		}

		public void PMToOldPM(CalculatedChartsOfAccountsLinePM entityPM, CalculatedChartsOfAccountsLinePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDateTime))
            {
                oldEntityPM.CreateDateTime = entityPM.CreateDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedDateTime))
            {
                oldEntityPM.UpdatedDateTime = entityPM.UpdatedDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDetailedGLAccount))
            {
                oldEntityPM.IsDetailedGLAccount = entityPM.IsDetailedGLAccount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
                oldEntityPM.IsCancelled = entityPM.IsCancelled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GLAccountId))
            {
                oldEntityPM.GLAccountId = entityPM.GLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChartOfAccountId))
            {
                oldEntityPM.ChartOfAccountId = entityPM.ChartOfAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineTypeCode))
            {
                oldEntityPM.LineTypeCode = entityPM.LineTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CalculatedChartsOfAccountsId))
            {
                oldEntityPM.CalculatedChartsOfAccountsId = entityPM.CalculatedChartsOfAccountsId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Line))
            {
                oldEntityPM.Line = entityPM.Line;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CalculatedChartsOfAccountsLinePM entityPM)
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
	 