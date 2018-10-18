
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
   
   public partial class QuestionnaireQuestionDataMapping: IMapping<QuestionnaireQuestionPM, QuestionnaireQuestion>,IMappingEncodeBase64NVARCHARFields<QuestionnaireQuestionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         QuestioneerId, 
	         VersionNumber, 
	         QuestionNumber, 
	         Tenant, 
	         Question, 
	         QuestionTypeCode, 
	         IsMandatory, 
	         CreatedByUserId, 
	         CreateDate, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         PickListCode, 
	         IsAddOther,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         QuestioneerId, 
	         VersionNumber, 
	         QuestionNumber, 
	         Tenant, 
	         Question, 
	         QuestionTypeCode, 
	         IsMandatory, 
	         CreatedByUserId, 
	         CreateDate, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         PickListCode, 
	         IsAddOther,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuestionnaireQuestionPM entityPM, QuestionnaireQuestion entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Question))
            {
				entityPOCO.Question = entityPM.Question;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuestionTypeCode))
            {
				entityPOCO.QuestionTypeCode = entityPM.QuestionTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMandatory))
            {
				entityPOCO.IsMandatory = entityPM.IsMandatory;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickListCode))
            {
				entityPOCO.PickListCode = entityPM.PickListCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAddOther))
            {
				entityPOCO.IsAddOther = entityPM.IsAddOther;
			}
			}

		public void POCOToPM(QuestionnaireQuestionPM entityPM, QuestionnaireQuestion entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuestioneerId))
            {
					entityPM.QuestioneerId = entityPOCO.QuestioneerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VersionNumber))
            {
					entityPM.VersionNumber = entityPOCO.VersionNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuestionNumber))
            {
					entityPM.QuestionNumber = entityPOCO.QuestionNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Question))
            {
					entityPM.Question = entityPOCO.Question;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuestionTypeCode))
            {
					entityPM.QuestionTypeCode = entityPOCO.QuestionTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsMandatory))
            {
					entityPM.IsMandatory = entityPOCO.IsMandatory;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PickListCode))
            {
					entityPM.PickListCode = entityPOCO.PickListCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsAddOther))
            {
					entityPM.IsAddOther = entityPOCO.IsAddOther;
            }

		}

		public void PMToOldPM(QuestionnaireQuestionPM entityPM, QuestionnaireQuestionPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Question))
            {
                oldEntityPM.Question = entityPM.Question;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuestionTypeCode))
            {
                oldEntityPM.QuestionTypeCode = entityPM.QuestionTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMandatory))
            {
                oldEntityPM.IsMandatory = entityPM.IsMandatory;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickListCode))
            {
                oldEntityPM.PickListCode = entityPM.PickListCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAddOther))
            {
                oldEntityPM.IsAddOther = entityPM.IsAddOther;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuestionnaireQuestionPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Question)) //T4 find type == nText 
            {
                entityPM.Question = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Question));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.PickListCode)) //T4 find type == nText 
            {
                entityPM.PickListCode = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.PickListCode));
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
	 