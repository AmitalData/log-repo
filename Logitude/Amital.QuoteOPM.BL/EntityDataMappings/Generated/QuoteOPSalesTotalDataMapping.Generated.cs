
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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs; 
using Amital.QuoteOPM.Data;

namespace Amital.QuoteOPM.BL.EntityDataMappings
{
   
   public partial class QuoteOPSalesTotalDataMapping: IMapping<QuoteOPSalesTotalPM, QuoteOPSalesTotal>,IMappingEncodeBase64NVARCHARFields<QuoteOPSalesTotalPM>
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
    
	    public void PMToPOCO(QuoteOPSalesTotalPM entityPM, QuoteOPSalesTotal entityPOCO)
        {
			 }

		public void POCOToPM(QuoteOPSalesTotalPM entityPM, QuoteOPSalesTotal entityPOCO)
        {
			 
		}

		public void PMToOldPM(QuoteOPSalesTotalPM entityPM, QuoteOPSalesTotalPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPSalesTotalPM entityPM)
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
	 