
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
   
   public partial class GLAccountCounterDataMapping: IMapping<GLAccountCounterPM, GLAccountCounter>,IMappingEncodeBase64NVARCHARFields<GLAccountCounterPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Prefix, 
	         StartNumber, 
	         CurrentNumber,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Prefix, 
	         StartNumber, 
	         CurrentNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(GLAccountCounterPM entityPM, GLAccountCounter entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Prefix))
            {
				entityPOCO.Prefix = entityPM.Prefix;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartNumber))
            {
				entityPOCO.StartNumber = entityPM.StartNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrentNumber))
            {
				entityPOCO.CurrentNumber = entityPM.CurrentNumber;
			}
			}

		public void POCOToPM(GLAccountCounterPM entityPM, GLAccountCounter entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Prefix))
            {
					entityPM.Prefix = entityPOCO.Prefix;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartNumber))
            {
					entityPM.StartNumber = entityPOCO.StartNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CurrentNumber))
            {
					entityPM.CurrentNumber = entityPOCO.CurrentNumber;
            }

		}

		public void PMToOldPM(GLAccountCounterPM entityPM, GLAccountCounterPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Prefix))
            {
                oldEntityPM.Prefix = entityPM.Prefix;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartNumber))
            {
                oldEntityPM.StartNumber = entityPM.StartNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrentNumber))
            {
                oldEntityPM.CurrentNumber = entityPM.CurrentNumber;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(GLAccountCounterPM entityPM)
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
	 