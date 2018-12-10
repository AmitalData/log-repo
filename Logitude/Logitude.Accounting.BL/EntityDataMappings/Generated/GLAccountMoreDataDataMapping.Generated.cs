
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
   
   public partial class GLAccountMoreDataDataMapping: IMapping<GLAccountMoreDataPM, GLAccountMoreData>,IMappingEncodeBase64NVARCHARFields<GLAccountMoreDataPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         AccountId, 
	         Tenant, 
	         BalanceInLocalCurrency, 
	         LocalBalanceInDue, 
	         NextDueDate, 
	         TotalOpenChequesInLocalCur, 
	         TotFutureOpenChequesInLocalCur,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         AccountId, 
	         Tenant, 
	         BalanceInLocalCurrency, 
	         LocalBalanceInDue, 
	         NextDueDate, 
	         TotalOpenChequesInLocalCur, 
	         TotFutureOpenChequesInLocalCur,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(GLAccountMoreDataPM entityPM, GLAccountMoreData entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BalanceInLocalCurrency))
            {
				entityPOCO.BalanceInLocalCurrency = entityPM.BalanceInLocalCurrency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalBalanceInDue))
            {
				entityPOCO.LocalBalanceInDue = entityPM.LocalBalanceInDue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextDueDate))
            {
				entityPOCO.NextDueDate = entityPM.NextDueDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalOpenChequesInLocalCur))
            {
				entityPOCO.TotalOpenChequesInLocalCur = entityPM.TotalOpenChequesInLocalCur;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotFutureOpenChequesInLocalCur))
            {
				entityPOCO.TotFutureOpenChequesInLocalCur = entityPM.TotFutureOpenChequesInLocalCur;
			}
			}

		public void POCOToPM(GLAccountMoreDataPM entityPM, GLAccountMoreData entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountId))
            {
					entityPM.AccountId = entityPOCO.AccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BalanceInLocalCurrency))
            {
					entityPM.BalanceInLocalCurrency = entityPOCO.BalanceInLocalCurrency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalBalanceInDue))
            {
					entityPM.LocalBalanceInDue = entityPOCO.LocalBalanceInDue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NextDueDate))
            {
					entityPM.NextDueDate = entityPOCO.NextDueDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalOpenChequesInLocalCur))
            {
					entityPM.TotalOpenChequesInLocalCur = entityPOCO.TotalOpenChequesInLocalCur;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotFutureOpenChequesInLocalCur))
            {
					entityPM.TotFutureOpenChequesInLocalCur = entityPOCO.TotFutureOpenChequesInLocalCur;
            }

		}

		public void PMToOldPM(GLAccountMoreDataPM entityPM, GLAccountMoreDataPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BalanceInLocalCurrency))
            {
                oldEntityPM.BalanceInLocalCurrency = entityPM.BalanceInLocalCurrency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalBalanceInDue))
            {
                oldEntityPM.LocalBalanceInDue = entityPM.LocalBalanceInDue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextDueDate))
            {
                oldEntityPM.NextDueDate = entityPM.NextDueDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalOpenChequesInLocalCur))
            {
                oldEntityPM.TotalOpenChequesInLocalCur = entityPM.TotalOpenChequesInLocalCur;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotFutureOpenChequesInLocalCur))
            {
                oldEntityPM.TotFutureOpenChequesInLocalCur = entityPM.TotFutureOpenChequesInLocalCur;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(GLAccountMoreDataPM entityPM)
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
	 