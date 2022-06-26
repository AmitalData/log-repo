
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
   
   public partial class GTBFUSTATUDataMapping: IMapping<GTBFUSTATUPM, GTBFUSTATU>,IMappingEncodeBase64NVARCHARFields<GTBFUSTATUPM>
   {
          public enum POCOPropertyNames
          { 
		     None, 
	      }


	      public enum PMPropertyNames
          { 
		     None, 
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(GTBFUSTATUPM entityPM, GTBFUSTATU entityPOCO)
        {
			 }

		public void POCOToPM(GTBFUSTATUPM entityPM, GTBFUSTATU entityPOCO)
        {
			 
		}

		public void PMToOldPM(GTBFUSTATUPM entityPM, GTBFUSTATUPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
		}

	    public void EncodeBase64NVARCHARFields(GTBFUSTATUPM entityPM)
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
	 