
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
   
   public partial class QuoteOPTemplateSectionDataMapping: IMapping<QuoteOPTemplateSectionPM, QuoteOPTemplateSection>,IMappingEncodeBase64NVARCHARFields<QuoteOPTemplateSectionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Name, 
	         Description, 
	         IsCancel, 
	         QuoteTemplateId, 
	         SectionDocId, 
	         Order, 
	         QuoteOPTemplateSectionTypeCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Name, 
	         Description, 
	         IsCancel, 
	         QuoteTemplateId, 
	         SectionDocId, 
	         Order, 
	         QuoteOPTemplateSectionTypeCode, 
	         IschangeBodySection, 
	         IsSettingTypeCodeS, 
	         IsSettingTypeCodeP, 
	         Templatedata, 
	         QuoteId, 
	         IsQuoteEdited, 
	         IsExcluded,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPTemplateSectionPM entityPM, QuoteOPTemplateSection entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
				entityPOCO.Name = entityPM.Name;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
				entityPOCO.Description = entityPM.Description;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancel))
            {
				entityPOCO.IsCancel = entityPM.IsCancel;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteTemplateId))
            {
				entityPOCO.QuoteTemplateId = entityPM.QuoteTemplateId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SectionDocId))
            {
				entityPOCO.SectionDocId = entityPM.SectionDocId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Order))
            {
				entityPOCO.Order = entityPM.Order;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteOPTemplateSectionTypeCode))
            {
				entityPOCO.QuoteOPTemplateSectionTypeCode = entityPM.QuoteOPTemplateSectionTypeCode;
			}
			}

		public void POCOToPM(QuoteOPTemplateSectionPM entityPM, QuoteOPTemplateSection entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Name))
            {
					entityPM.Name = entityPOCO.Name;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Description))
            {
					entityPM.Description = entityPOCO.Description;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCancel))
            {
					entityPM.IsCancel = entityPOCO.IsCancel;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteTemplateId))
            {
					entityPM.QuoteTemplateId = entityPOCO.QuoteTemplateId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SectionDocId))
            {
					entityPM.SectionDocId = entityPOCO.SectionDocId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Order))
            {
					entityPM.Order = entityPOCO.Order;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteOPTemplateSectionTypeCode))
            {
					entityPM.QuoteOPTemplateSectionTypeCode = entityPOCO.QuoteOPTemplateSectionTypeCode;
            }

		}

		public void PMToOldPM(QuoteOPTemplateSectionPM entityPM, QuoteOPTemplateSectionPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
                oldEntityPM.Name = entityPM.Name;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
                oldEntityPM.Description = entityPM.Description;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancel))
            {
                oldEntityPM.IsCancel = entityPM.IsCancel;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteTemplateId))
            {
                oldEntityPM.QuoteTemplateId = entityPM.QuoteTemplateId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SectionDocId))
            {
                oldEntityPM.SectionDocId = entityPM.SectionDocId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Order))
            {
                oldEntityPM.Order = entityPM.Order;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteOPTemplateSectionTypeCode))
            {
                oldEntityPM.QuoteOPTemplateSectionTypeCode = entityPM.QuoteOPTemplateSectionTypeCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPTemplateSectionPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Name)) //T4 find type == nText 
            {
                entityPM.Name = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Name));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Description)) //T4 find type == nText 
            {
                entityPM.Description = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Description));
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
	 