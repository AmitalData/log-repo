
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
   
   public partial class JournalMoreDataDataMapping: IMapping<JournalMoreDataPM, JournalMoreData>,IMappingEncodeBase64NVARCHARFields<JournalMoreDataPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         JournalId, 
	         Line, 
	         Tenant, 
	         GeneralData, 
	         IsLedgerCreated,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         JournalId, 
	         Line, 
	         Tenant, 
	         GeneralData, 
	         IsLedgerCreated,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(JournalMoreDataPM entityPM, JournalMoreData entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GeneralData))
            {
				entityPOCO.GeneralData = entityPM.GeneralData;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsLedgerCreated))
            {
				entityPOCO.IsLedgerCreated = entityPM.IsLedgerCreated;
			}
			}

		public void POCOToPM(JournalMoreDataPM entityPM, JournalMoreData entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.JournalId))
            {
					entityPM.JournalId = entityPOCO.JournalId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Line))
            {
					entityPM.Line = entityPOCO.Line;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GeneralData))
            {
					entityPM.GeneralData = entityPOCO.GeneralData;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsLedgerCreated))
            {
					entityPM.IsLedgerCreated = entityPOCO.IsLedgerCreated;
            }

		}

		public void PMToOldPM(JournalMoreDataPM entityPM, JournalMoreDataPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GeneralData))
            {
                oldEntityPM.GeneralData = entityPM.GeneralData;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsLedgerCreated))
            {
                oldEntityPM.IsLedgerCreated = entityPM.IsLedgerCreated;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(JournalMoreDataPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.GeneralData)) //T4 find type == nText 
            {
                entityPM.GeneralData = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.GeneralData));
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
	 