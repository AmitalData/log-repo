
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class PriorityDataMapping: IMapping<PriorityPM, Priority>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Code, 
	         Name, 
	         SearchFields,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Code, 
	         Name, 
	         SearchFields,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(PriorityPM entityPM, Priority entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
                entityPOCO.Name = entityPM.Name;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                entityPOCO.SearchFields = entityPM.SearchFields;
            }
			
		}

		public void POCOToPM(PriorityPM entityPM, Priority entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Code))
            {
                entityPM.Code = entityPOCO.Code;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Name))
            {
                entityPM.Name = entityPOCO.Name;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
                entityPM.SearchFields = entityPOCO.SearchFields;
            }
			
		}

		public void PMToOldPM(PriorityPM entityPM, PriorityPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
                oldEntityPM.Name = entityPM.Name;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
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
	 