
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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class QuestionnaireAnswerLineDataMapping: IMapping<QuestionnaireAnswerLinePM, QuestionnaireAnswerLine>,IMappingEncodeBase64NVARCHARFields<QuestionnaireAnswerLinePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         QuestionnaireAnswerId, 
	         Tenant, 
	         QuestionNumber, 
	         AnswerValue,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         QuestionnaireAnswerId, 
	         Tenant, 
	         QuestionNumber, 
	         AnswerValue,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuestionnaireAnswerLinePM entityPM, QuestionnaireAnswerLine entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnswerValue))
            {
				entityPOCO.AnswerValue = entityPM.AnswerValue;
			}
			}

		public void POCOToPM(QuestionnaireAnswerLinePM entityPM, QuestionnaireAnswerLine entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuestionnaireAnswerId))
            {
					entityPM.QuestionnaireAnswerId = entityPOCO.QuestionnaireAnswerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuestionNumber))
            {
					entityPM.QuestionNumber = entityPOCO.QuestionNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AnswerValue))
            {
					entityPM.AnswerValue = entityPOCO.AnswerValue;
            }

		}

		public void PMToOldPM(QuestionnaireAnswerLinePM entityPM, QuestionnaireAnswerLinePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnswerValue))
            {
                oldEntityPM.AnswerValue = entityPM.AnswerValue;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuestionnaireAnswerLinePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.AnswerValue)) //T4 find type == nText 
            {
                entityPM.AnswerValue = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.AnswerValue));
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
	 