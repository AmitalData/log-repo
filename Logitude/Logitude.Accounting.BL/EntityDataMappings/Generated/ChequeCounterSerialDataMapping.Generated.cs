
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
   
   public partial class ChequeCounterSerialDataMapping: IMapping<ChequeCounterSerialPM, ChequeCounterSerial>,IMappingEncodeBase64NVARCHARFields<ChequeCounterSerialPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         BankAccountId, 
	         SeriesId, 
	         ChequeCounterBegin, 
	         ChequeCounterEnd,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         BankAccountId, 
	         SeriesId, 
	         ChequeCounterBegin, 
	         ChequeCounterEnd,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ChequeCounterSerialPM entityPM, ChequeCounterSerial entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankAccountId))
            {
				entityPOCO.BankAccountId = entityPM.BankAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SeriesId))
            {
				entityPOCO.SeriesId = entityPM.SeriesId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChequeCounterBegin))
            {
				entityPOCO.ChequeCounterBegin = entityPM.ChequeCounterBegin;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChequeCounterEnd))
            {
				entityPOCO.ChequeCounterEnd = entityPM.ChequeCounterEnd;
			}
			}

		public void POCOToPM(ChequeCounterSerialPM entityPM, ChequeCounterSerial entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BankAccountId))
            {
					entityPM.BankAccountId = entityPOCO.BankAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SeriesId))
            {
					entityPM.SeriesId = entityPOCO.SeriesId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChequeCounterBegin))
            {
					entityPM.ChequeCounterBegin = entityPOCO.ChequeCounterBegin;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChequeCounterEnd))
            {
					entityPM.ChequeCounterEnd = entityPOCO.ChequeCounterEnd;
            }

		}

		public void PMToOldPM(ChequeCounterSerialPM entityPM, ChequeCounterSerialPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankAccountId))
            {
                oldEntityPM.BankAccountId = entityPM.BankAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SeriesId))
            {
                oldEntityPM.SeriesId = entityPM.SeriesId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChequeCounterBegin))
            {
                oldEntityPM.ChequeCounterBegin = entityPM.ChequeCounterBegin;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChequeCounterEnd))
            {
                oldEntityPM.ChequeCounterEnd = entityPM.ChequeCounterEnd;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ChequeCounterSerialPM entityPM)
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
	 