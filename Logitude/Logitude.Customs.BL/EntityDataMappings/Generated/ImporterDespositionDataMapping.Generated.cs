
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ImporterDespositionDataMapping: IMapping<ImporterDespositionPM, ImporterDesposition>,IMappingEncodeBase64NVARCHARFields<ImporterDespositionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DepositionNumber, 
	         ImporterDepositionStatusCode, 
	         ImporterlId, 
	         VendorID, 
	         StartDate, 
	         EndDate, 
	         NotesToAgent, 
	         ErrorMessage, 
	         Tenant, 
	         Id,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DepositionNumber, 
	         ImporterDepositionStatusCode, 
	         ImporterlId, 
	         VendorID, 
	         StartDate, 
	         EndDate, 
	         NotesToAgent, 
	         ErrorMessage, 
	         ImporterDepositionStatusName, 
	         Tenant, 
	         Id,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ImporterDespositionPM entityPM, ImporterDesposition entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepositionNumber))
            {
				entityPOCO.DepositionNumber = entityPM.DepositionNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterDepositionStatusCode))
            {
				entityPOCO.ImporterDepositionStatusCode = entityPM.ImporterDepositionStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterlId))
            {
				entityPOCO.ImporterlId = entityPM.ImporterlId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorID))
            {
				entityPOCO.VendorID = entityPM.VendorID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
				entityPOCO.StartDate = entityPM.StartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
				entityPOCO.EndDate = entityPM.EndDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotesToAgent))
            {
				entityPOCO.NotesToAgent = entityPM.NotesToAgent;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrorMessage))
            {
				entityPOCO.ErrorMessage = entityPM.ErrorMessage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			}

		public void POCOToPM(ImporterDespositionPM entityPM, ImporterDesposition entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DepositionNumber))
            {
					entityPM.DepositionNumber = entityPOCO.DepositionNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImporterDepositionStatusCode))
            {
					entityPM.ImporterDepositionStatusCode = entityPOCO.ImporterDepositionStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImporterlId))
            {
					entityPM.ImporterlId = entityPOCO.ImporterlId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VendorID))
            {
					entityPM.VendorID = entityPOCO.VendorID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDate))
            {
					entityPM.StartDate = entityPOCO.StartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EndDate))
            {
					entityPM.EndDate = entityPOCO.EndDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NotesToAgent))
            {
					entityPM.NotesToAgent = entityPOCO.NotesToAgent;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ErrorMessage))
            {
					entityPM.ErrorMessage = entityPOCO.ErrorMessage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

		}

		public void PMToOldPM(ImporterDespositionPM entityPM, ImporterDespositionPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepositionNumber))
            {
                oldEntityPM.DepositionNumber = entityPM.DepositionNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterDepositionStatusCode))
            {
                oldEntityPM.ImporterDepositionStatusCode = entityPM.ImporterDepositionStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterlId))
            {
                oldEntityPM.ImporterlId = entityPM.ImporterlId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorID))
            {
                oldEntityPM.VendorID = entityPM.VendorID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
                oldEntityPM.StartDate = entityPM.StartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
                oldEntityPM.EndDate = entityPM.EndDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotesToAgent))
            {
                oldEntityPM.NotesToAgent = entityPM.NotesToAgent;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrorMessage))
            {
                oldEntityPM.ErrorMessage = entityPM.ErrorMessage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ImporterDespositionPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.NotesToAgent)) //T4 find type == nText 
            {
                entityPM.NotesToAgent = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.NotesToAgent));
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
	 