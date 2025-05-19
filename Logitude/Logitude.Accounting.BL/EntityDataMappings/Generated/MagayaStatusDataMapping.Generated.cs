
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
   
   public partial class MagayaStatusDataMapping: IMapping<MagayaStatusPM, MagayaStatus>,IMappingEncodeBase64NVARCHARFields<MagayaStatusPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         StatusCode, 
	         StatusName,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         StatusCode, 
	         StatusName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(MagayaStatusPM entityPM, MagayaStatus entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusName))
            {
				entityPOCO.StatusName = entityPM.StatusName;
			}
			}

		public void POCOToPM(MagayaStatusPM entityPM, MagayaStatus entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusCode))
            {
					entityPM.StatusCode = entityPOCO.StatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusName))
            {
					entityPM.StatusName = entityPOCO.StatusName;
            }

		}

		public void PMToOldPM(MagayaStatusPM entityPM, MagayaStatusPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusName))
            {
                oldEntityPM.StatusName = entityPM.StatusName;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(MagayaStatusPM entityPM)
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
	 