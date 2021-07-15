
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
   
   public partial class QuoteOPTemplateTextDesignDataMapping: IMapping<QuoteOPTemplateTextDesignPM, QuoteOPTemplateTextDesign>,IMappingEncodeBase64NVARCHARFields<QuoteOPTemplateTextDesignPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         FontSize, 
	         TextColor, 
	         FontFamily, 
	         BackgroundColor, 
	         FontWeight, 
	         Italic, 
	         UnDerLine, 
	         Alignment,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         FontSize, 
	         TextColor, 
	         FontFamily, 
	         BackgroundColor, 
	         FontWeight, 
	         Italic, 
	         UnDerLine, 
	         Alignment, 
	         Title, 
	         TextValue, 
	         HideAlignment, 
	         SampleText,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPTemplateTextDesignPM entityPM, QuoteOPTemplateTextDesign entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FontSize))
            {
				entityPOCO.FontSize = entityPM.FontSize;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TextColor))
            {
				entityPOCO.TextColor = entityPM.TextColor;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FontFamily))
            {
				entityPOCO.FontFamily = entityPM.FontFamily;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BackgroundColor))
            {
				entityPOCO.BackgroundColor = entityPM.BackgroundColor;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FontWeight))
            {
				entityPOCO.FontWeight = entityPM.FontWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Italic))
            {
				entityPOCO.Italic = entityPM.Italic;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnDerLine))
            {
				entityPOCO.UnDerLine = entityPM.UnDerLine;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Alignment))
            {
				entityPOCO.Alignment = entityPM.Alignment;
			}
			}

		public void POCOToPM(QuoteOPTemplateTextDesignPM entityPM, QuoteOPTemplateTextDesign entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FontSize))
            {
					entityPM.FontSize = entityPOCO.FontSize;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TextColor))
            {
					entityPM.TextColor = entityPOCO.TextColor;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FontFamily))
            {
					entityPM.FontFamily = entityPOCO.FontFamily;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BackgroundColor))
            {
					entityPM.BackgroundColor = entityPOCO.BackgroundColor;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FontWeight))
            {
					entityPM.FontWeight = entityPOCO.FontWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Italic))
            {
					entityPM.Italic = entityPOCO.Italic;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UnDerLine))
            {
					entityPM.UnDerLine = entityPOCO.UnDerLine;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Alignment))
            {
					entityPM.Alignment = entityPOCO.Alignment;
            }

		}

		public void PMToOldPM(QuoteOPTemplateTextDesignPM entityPM, QuoteOPTemplateTextDesignPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FontSize))
            {
                oldEntityPM.FontSize = entityPM.FontSize;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TextColor))
            {
                oldEntityPM.TextColor = entityPM.TextColor;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FontFamily))
            {
                oldEntityPM.FontFamily = entityPM.FontFamily;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BackgroundColor))
            {
                oldEntityPM.BackgroundColor = entityPM.BackgroundColor;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FontWeight))
            {
                oldEntityPM.FontWeight = entityPM.FontWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Italic))
            {
                oldEntityPM.Italic = entityPM.Italic;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UnDerLine))
            {
                oldEntityPM.UnDerLine = entityPM.UnDerLine;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Alignment))
            {
                oldEntityPM.Alignment = entityPM.Alignment;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPTemplateTextDesignPM entityPM)
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
	 