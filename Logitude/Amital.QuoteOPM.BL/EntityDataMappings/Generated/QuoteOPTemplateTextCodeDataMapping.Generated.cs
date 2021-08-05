
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
   
   public partial class QuoteOPTemplateTextCodeDataMapping: IMapping<QuoteOPTemplateTextCodePM, QuoteOPTemplateTextCode>,IMappingEncodeBase64NVARCHARFields<QuoteOPTemplateTextCodePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         TextCode, 
	         EnglishName, 
	         LocalName, 
	         QuoteTemplateId, 
	         Area, 
	         OriginalEnglishName, 
	         OriginalLocalName,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         TextCode, 
	         EnglishName, 
	         LocalName, 
	         QuoteTemplateId, 
	         Area, 
	         OriginalEnglishName, 
	         OriginalLocalName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPTemplateTextCodePM entityPM, QuoteOPTemplateTextCode entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TextCode))
            {
				entityPOCO.TextCode = entityPM.TextCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
				entityPOCO.EnglishName = entityPM.EnglishName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
				entityPOCO.LocalName = entityPM.LocalName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteTemplateId))
            {
				entityPOCO.QuoteTemplateId = entityPM.QuoteTemplateId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Area))
            {
				entityPOCO.Area = entityPM.Area;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginalEnglishName))
            {
				entityPOCO.OriginalEnglishName = entityPM.OriginalEnglishName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginalLocalName))
            {
				entityPOCO.OriginalLocalName = entityPM.OriginalLocalName;
			}
			}

		public void POCOToPM(QuoteOPTemplateTextCodePM entityPM, QuoteOPTemplateTextCode entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TextCode))
            {
					entityPM.TextCode = entityPOCO.TextCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishName))
            {
					entityPM.EnglishName = entityPOCO.EnglishName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalName))
            {
					entityPM.LocalName = entityPOCO.LocalName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteTemplateId))
            {
					entityPM.QuoteTemplateId = entityPOCO.QuoteTemplateId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Area))
            {
					entityPM.Area = entityPOCO.Area;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginalEnglishName))
            {
					entityPM.OriginalEnglishName = entityPOCO.OriginalEnglishName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginalLocalName))
            {
					entityPM.OriginalLocalName = entityPOCO.OriginalLocalName;
            }

		}

		public void PMToOldPM(QuoteOPTemplateTextCodePM entityPM, QuoteOPTemplateTextCodePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TextCode))
            {
                oldEntityPM.TextCode = entityPM.TextCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
                oldEntityPM.EnglishName = entityPM.EnglishName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
                oldEntityPM.LocalName = entityPM.LocalName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteTemplateId))
            {
                oldEntityPM.QuoteTemplateId = entityPM.QuoteTemplateId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Area))
            {
                oldEntityPM.Area = entityPM.Area;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginalEnglishName))
            {
                oldEntityPM.OriginalEnglishName = entityPM.OriginalEnglishName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginalLocalName))
            {
                oldEntityPM.OriginalLocalName = entityPM.OriginalLocalName;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPTemplateTextCodePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.LocalName)) //T4 find type == nText 
            {
                entityPM.LocalName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LocalName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.OriginalLocalName)) //T4 find type == nText 
            {
                entityPM.OriginalLocalName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.OriginalLocalName));
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
	 