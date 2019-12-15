
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
   
   public partial class InterestTransactionDataMapping: IMapping<InterestTransactionPM, InterestTransaction>,IMappingEncodeBase64NVARCHARFields<InterestTransactionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDateTime, 
	         UpdateDateTime, 
	         SearchFields, 
	         GLAccountId, 
	         InterestEntityTypeCode, 
	         EntityId, 
	         OriginalEntityLineNumber, 
	         LocalAmount, 
	         ForeignAmount, 
	         CurrencyId, 
	         InterestValueDate, 
	         InterestReportId, 
	         IsClosed,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDateTime, 
	         UpdateDateTime, 
	         SearchFields, 
	         GLAccountId, 
	         InterestEntityTypeCode, 
	         EntityId, 
	         OriginalEntityLineNumber, 
	         LocalAmount, 
	         ForeignAmount, 
	         CurrencyId, 
	         InterestValueDate, 
	         InterestReportId, 
	         IsClosed,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(InterestTransactionPM entityPM, InterestTransaction entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDateTime))
            {
				entityPOCO.CreateDateTime = entityPM.CreateDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDateTime))
            {
				entityPOCO.UpdateDateTime = entityPM.UpdateDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GLAccountId))
            {
				entityPOCO.GLAccountId = entityPM.GLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestEntityTypeCode))
            {
				entityPOCO.InterestEntityTypeCode = entityPM.InterestEntityTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
				entityPOCO.EntityId = entityPM.EntityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginalEntityLineNumber))
            {
				entityPOCO.OriginalEntityLineNumber = entityPM.OriginalEntityLineNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalAmount))
            {
				entityPOCO.LocalAmount = entityPM.LocalAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForeignAmount))
            {
				entityPOCO.ForeignAmount = entityPM.ForeignAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyId))
            {
				entityPOCO.CurrencyId = entityPM.CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestValueDate))
            {
				entityPOCO.InterestValueDate = entityPM.InterestValueDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestReportId))
            {
				entityPOCO.InterestReportId = entityPM.InterestReportId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
				entityPOCO.IsClosed = entityPM.IsClosed;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(InterestTransactionPM entityPM, InterestTransaction entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDateTime))
            {
					entityPM.UpdateDateTime = entityPOCO.UpdateDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GLAccountId))
            {
					entityPM.GLAccountId = entityPOCO.GLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterestEntityTypeCode))
            {
					entityPM.InterestEntityTypeCode = entityPOCO.InterestEntityTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityId))
            {
					entityPM.EntityId = entityPOCO.EntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginalEntityLineNumber))
            {
					entityPM.OriginalEntityLineNumber = entityPOCO.OriginalEntityLineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalAmount))
            {
					entityPM.LocalAmount = entityPOCO.LocalAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ForeignAmount))
            {
					entityPM.ForeignAmount = entityPOCO.ForeignAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CurrencyId))
            {
					entityPM.CurrencyId = entityPOCO.CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterestValueDate))
            {
					entityPM.InterestValueDate = entityPOCO.InterestValueDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterestReportId))
            {
					entityPM.InterestReportId = entityPOCO.InterestReportId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClosed))
            {
					entityPM.IsClosed = entityPOCO.IsClosed;
            }

		}

		public void PMToOldPM(InterestTransactionPM entityPM, InterestTransactionPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDateTime))
            {
                oldEntityPM.UpdateDateTime = entityPM.UpdateDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GLAccountId))
            {
                oldEntityPM.GLAccountId = entityPM.GLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestEntityTypeCode))
            {
                oldEntityPM.InterestEntityTypeCode = entityPM.InterestEntityTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
                oldEntityPM.EntityId = entityPM.EntityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginalEntityLineNumber))
            {
                oldEntityPM.OriginalEntityLineNumber = entityPM.OriginalEntityLineNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalAmount))
            {
                oldEntityPM.LocalAmount = entityPM.LocalAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForeignAmount))
            {
                oldEntityPM.ForeignAmount = entityPM.ForeignAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyId))
            {
                oldEntityPM.CurrencyId = entityPM.CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestValueDate))
            {
                oldEntityPM.InterestValueDate = entityPM.InterestValueDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestReportId))
            {
                oldEntityPM.InterestReportId = entityPM.InterestReportId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
                oldEntityPM.IsClosed = entityPM.IsClosed;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(InterestTransactionPM entityPM)
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
		
		private void BuildSearchFieldsGenerated(InterestTransactionPM entityPM, InterestTransaction entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 