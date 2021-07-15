
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
   
   public partial class QuoteOPTemplateTableDesignDataMapping: IMapping<QuoteOPTemplateTableDesignPM, QuoteOPTemplateTableDesign>,IMappingEncodeBase64NVARCHARFields<QuoteOPTemplateTableDesignPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         BorderTypeCode, 
	         BorderColor, 
	         BorderThickness, 
	         HeaderDesignId, 
	         LinesDesignId, 
	         GroupByDesignId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         BorderTypeCode, 
	         BorderColor, 
	         BorderThickness, 
	         HeaderDesignId, 
	         LinesDesignId, 
	         GroupByDesignId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPTemplateTableDesignPM entityPM, QuoteOPTemplateTableDesign entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BorderTypeCode))
            {
				entityPOCO.BorderTypeCode = entityPM.BorderTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BorderColor))
            {
				entityPOCO.BorderColor = entityPM.BorderColor;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BorderThickness))
            {
				entityPOCO.BorderThickness = entityPM.BorderThickness;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HeaderDesignId))
            {
				entityPOCO.HeaderDesignId = entityPM.HeaderDesignId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LinesDesignId))
            {
				entityPOCO.LinesDesignId = entityPM.LinesDesignId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GroupByDesignId))
            {
				entityPOCO.GroupByDesignId = entityPM.GroupByDesignId;
			}
			}

		public void POCOToPM(QuoteOPTemplateTableDesignPM entityPM, QuoteOPTemplateTableDesign entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BorderTypeCode))
            {
					entityPM.BorderTypeCode = entityPOCO.BorderTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BorderColor))
            {
					entityPM.BorderColor = entityPOCO.BorderColor;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BorderThickness))
            {
					entityPM.BorderThickness = entityPOCO.BorderThickness;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HeaderDesignId))
            {
					entityPM.HeaderDesignId = entityPOCO.HeaderDesignId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LinesDesignId))
            {
					entityPM.LinesDesignId = entityPOCO.LinesDesignId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GroupByDesignId))
            {
					entityPM.GroupByDesignId = entityPOCO.GroupByDesignId;
            }

		}

		public void PMToOldPM(QuoteOPTemplateTableDesignPM entityPM, QuoteOPTemplateTableDesignPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BorderTypeCode))
            {
                oldEntityPM.BorderTypeCode = entityPM.BorderTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BorderColor))
            {
                oldEntityPM.BorderColor = entityPM.BorderColor;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BorderThickness))
            {
                oldEntityPM.BorderThickness = entityPM.BorderThickness;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HeaderDesignId))
            {
                oldEntityPM.HeaderDesignId = entityPM.HeaderDesignId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LinesDesignId))
            {
                oldEntityPM.LinesDesignId = entityPM.LinesDesignId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GroupByDesignId))
            {
                oldEntityPM.GroupByDesignId = entityPM.GroupByDesignId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPTemplateTableDesignPM entityPM)
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
	 