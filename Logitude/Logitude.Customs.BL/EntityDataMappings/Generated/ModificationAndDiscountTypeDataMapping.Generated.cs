
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
   
   public partial class ModificationAndDiscountTypeDataMapping: IMapping<ModificationAndDiscountTypePM, ModificationAndDiscountType>,IMappingEncodeBase64NVARCHARFields<ModificationAndDiscountTypePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Code, 
	         EnglishName, 
	         LocalName, 
	         SearchFields, 
	         Inactive, 
	         IsRelevantGoodsItem, 
	         IsRelevantInvoice, 
	         IsRelevantInvoiceExport, 
	         IsRelevantGoodsItemExport,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Code, 
	         EnglishName, 
	         LocalName, 
	         SearchFields, 
	         Inactive, 
	         IsRelevantGoodsItem, 
	         IsRelevantInvoice, 
	         IsRelevantInvoiceExport, 
	         IsRelevantGoodsItemExport,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ModificationAndDiscountTypePM entityPM, ModificationAndDiscountType entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
				entityPOCO.EnglishName = entityPM.EnglishName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
				entityPOCO.LocalName = entityPM.LocalName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
				entityPOCO.Inactive = entityPM.Inactive;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRelevantGoodsItem))
            {
				entityPOCO.IsRelevantGoodsItem = entityPM.IsRelevantGoodsItem;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRelevantInvoice))
            {
				entityPOCO.IsRelevantInvoice = entityPM.IsRelevantInvoice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRelevantInvoiceExport))
            {
				entityPOCO.IsRelevantInvoiceExport = entityPM.IsRelevantInvoiceExport;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRelevantGoodsItemExport))
            {
				entityPOCO.IsRelevantGoodsItemExport = entityPM.IsRelevantGoodsItemExport;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(ModificationAndDiscountTypePM entityPM, ModificationAndDiscountType entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Code))
            {
					entityPM.Code = entityPOCO.Code;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishName))
            {
					entityPM.EnglishName = entityPOCO.EnglishName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalName))
            {
					entityPM.LocalName = entityPOCO.LocalName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Inactive))
            {
					entityPM.Inactive = entityPOCO.Inactive;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsRelevantGoodsItem))
            {
					entityPM.IsRelevantGoodsItem = entityPOCO.IsRelevantGoodsItem;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsRelevantInvoice))
            {
					entityPM.IsRelevantInvoice = entityPOCO.IsRelevantInvoice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsRelevantInvoiceExport))
            {
					entityPM.IsRelevantInvoiceExport = entityPOCO.IsRelevantInvoiceExport;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsRelevantGoodsItemExport))
            {
					entityPM.IsRelevantGoodsItemExport = entityPOCO.IsRelevantGoodsItemExport;
            }

		}

		public void PMToOldPM(ModificationAndDiscountTypePM entityPM, ModificationAndDiscountTypePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
                oldEntityPM.EnglishName = entityPM.EnglishName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
                oldEntityPM.LocalName = entityPM.LocalName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
                oldEntityPM.Inactive = entityPM.Inactive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRelevantGoodsItem))
            {
                oldEntityPM.IsRelevantGoodsItem = entityPM.IsRelevantGoodsItem;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRelevantInvoice))
            {
                oldEntityPM.IsRelevantInvoice = entityPM.IsRelevantInvoice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRelevantInvoiceExport))
            {
                oldEntityPM.IsRelevantInvoiceExport = entityPM.IsRelevantInvoiceExport;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRelevantGoodsItemExport))
            {
                oldEntityPM.IsRelevantGoodsItemExport = entityPM.IsRelevantGoodsItemExport;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ModificationAndDiscountTypePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.LocalName)) //T4 find type == nText 
            {
                entityPM.LocalName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LocalName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
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
		
		private void BuildSearchFieldsGenerated(ModificationAndDiscountTypePM entityPM, ModificationAndDiscountType entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 