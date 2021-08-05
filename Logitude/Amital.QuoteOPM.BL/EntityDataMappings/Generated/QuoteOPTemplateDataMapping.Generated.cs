
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
   
   public partial class QuoteOPTemplateDataMapping: IMapping<QuoteOPTemplatePM, QuoteOPTemplate>,IMappingEncodeBase64NVARCHARFields<QuoteOPTemplatePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         HeaderDocId, 
	         FooterDocId, 
	         QuoteOPTemplateSettingId, 
	         Name, 
	         IsTemplate, 
	         OriginalQuoteOPTemplateId, 
	         CreateDate, 
	         UpdateDate, 
	         CreatedByUserId, 
	         UpdatedByUserId, 
	         SearchFields, 
	         TemplateTypeCode, 
	         IsDefault, 
	         InActive, 
	         IsCopiedAtSignup, 
	         IsEnabledForCustomers,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         HeaderDocId, 
	         FooterDocId, 
	         QuoteOPTemplateSettingId, 
	         Name, 
	         IsTemplate, 
	         OriginalQuoteOPTemplateId, 
	         CreateDate, 
	         UpdateDate, 
	         CreatedByUserId, 
	         UpdatedByUserId, 
	         SearchFields, 
	         TemplateTypeCode, 
	         IsDefault, 
	         InActive, 
	         IsLastQuoteOPTemplateDocumentVersion, 
	         IsCopiedAtSignup, 
	         IsEnabledForCustomers, 
	         TenantName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPTemplatePM entityPM, QuoteOPTemplate entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HeaderDocId))
            {
				entityPOCO.HeaderDocId = entityPM.HeaderDocId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FooterDocId))
            {
				entityPOCO.FooterDocId = entityPM.FooterDocId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteOPTemplateSettingId))
            {
				entityPOCO.QuoteOPTemplateSettingId = entityPM.QuoteOPTemplateSettingId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
				entityPOCO.Name = entityPM.Name;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsTemplate))
            {
				entityPOCO.IsTemplate = entityPM.IsTemplate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginalQuoteOPTemplateId))
            {
				entityPOCO.OriginalQuoteOPTemplateId = entityPM.OriginalQuoteOPTemplateId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TemplateTypeCode))
            {
				entityPOCO.TemplateTypeCode = entityPM.TemplateTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDefault))
            {
				entityPOCO.IsDefault = entityPM.IsDefault;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InActive))
            {
				entityPOCO.InActive = entityPM.InActive;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCopiedAtSignup))
            {
				entityPOCO.IsCopiedAtSignup = entityPM.IsCopiedAtSignup;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsEnabledForCustomers))
            {
				entityPOCO.IsEnabledForCustomers = entityPM.IsEnabledForCustomers;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(QuoteOPTemplatePM entityPM, QuoteOPTemplate entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HeaderDocId))
            {
					entityPM.HeaderDocId = entityPOCO.HeaderDocId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FooterDocId))
            {
					entityPM.FooterDocId = entityPOCO.FooterDocId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteOPTemplateSettingId))
            {
					entityPM.QuoteOPTemplateSettingId = entityPOCO.QuoteOPTemplateSettingId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Name))
            {
					entityPM.Name = entityPOCO.Name;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsTemplate))
            {
					entityPM.IsTemplate = entityPOCO.IsTemplate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginalQuoteOPTemplateId))
            {
					entityPM.OriginalQuoteOPTemplateId = entityPOCO.OriginalQuoteOPTemplateId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TemplateTypeCode))
            {
					entityPM.TemplateTypeCode = entityPOCO.TemplateTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDefault))
            {
					entityPM.IsDefault = entityPOCO.IsDefault;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InActive))
            {
					entityPM.InActive = entityPOCO.InActive;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCopiedAtSignup))
            {
					entityPM.IsCopiedAtSignup = entityPOCO.IsCopiedAtSignup;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsEnabledForCustomers))
            {
					entityPM.IsEnabledForCustomers = entityPOCO.IsEnabledForCustomers;
            }

		}

		public void PMToOldPM(QuoteOPTemplatePM entityPM, QuoteOPTemplatePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HeaderDocId))
            {
                oldEntityPM.HeaderDocId = entityPM.HeaderDocId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FooterDocId))
            {
                oldEntityPM.FooterDocId = entityPM.FooterDocId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteOPTemplateSettingId))
            {
                oldEntityPM.QuoteOPTemplateSettingId = entityPM.QuoteOPTemplateSettingId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
                oldEntityPM.Name = entityPM.Name;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsTemplate))
            {
                oldEntityPM.IsTemplate = entityPM.IsTemplate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginalQuoteOPTemplateId))
            {
                oldEntityPM.OriginalQuoteOPTemplateId = entityPM.OriginalQuoteOPTemplateId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TemplateTypeCode))
            {
                oldEntityPM.TemplateTypeCode = entityPM.TemplateTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDefault))
            {
                oldEntityPM.IsDefault = entityPM.IsDefault;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InActive))
            {
                oldEntityPM.InActive = entityPM.InActive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCopiedAtSignup))
            {
                oldEntityPM.IsCopiedAtSignup = entityPM.IsCopiedAtSignup;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsEnabledForCustomers))
            {
                oldEntityPM.IsEnabledForCustomers = entityPM.IsEnabledForCustomers;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPTemplatePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Name)) //T4 find type == nText 
            {
                entityPM.Name = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Name));
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
		
		private void BuildSearchFieldsGenerated(QuoteOPTemplatePM entityPM, QuoteOPTemplate entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 