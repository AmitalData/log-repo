
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
   
   public partial class CB_TradeAgreementDataMapping: IMapping<CB_TradeAgreementPM, CB_TradeAgreement>,IMappingEncodeBase64NVARCHARFields<CB_TradeAgreementPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         Title, 
	         AdditionName, 
	         CountryGroupID, 
	         CustomsBookTypeID, 
	         EntityStatusID, 
	         TradeAgreementAbbreviation,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         Title, 
	         AdditionName, 
	         CountryGroupID, 
	         CustomsBookTypeID, 
	         EntityStatusID, 
	         TradeAgreementAbbreviation,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_TradeAgreementPM entityPM, CB_TradeAgreement entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Title))
            {
				entityPOCO.Title = entityPM.Title;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AdditionName))
            {
				entityPOCO.AdditionName = entityPM.AdditionName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryGroupID))
            {
				entityPOCO.CountryGroupID = entityPM.CountryGroupID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBookTypeID))
            {
				entityPOCO.CustomsBookTypeID = entityPM.CustomsBookTypeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityStatusID))
            {
				entityPOCO.EntityStatusID = entityPM.EntityStatusID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementAbbreviation))
            {
				entityPOCO.TradeAgreementAbbreviation = entityPM.TradeAgreementAbbreviation;
			}
			}

		public void POCOToPM(CB_TradeAgreementPM entityPM, CB_TradeAgreement entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ID))
            {
					entityPM.ID = entityPOCO.ID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Title))
            {
					entityPM.Title = entityPOCO.Title;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AdditionName))
            {
					entityPM.AdditionName = entityPOCO.AdditionName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CountryGroupID))
            {
					entityPM.CountryGroupID = entityPOCO.CountryGroupID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsBookTypeID))
            {
					entityPM.CustomsBookTypeID = entityPOCO.CustomsBookTypeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityStatusID))
            {
					entityPM.EntityStatusID = entityPOCO.EntityStatusID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TradeAgreementAbbreviation))
            {
					entityPM.TradeAgreementAbbreviation = entityPOCO.TradeAgreementAbbreviation;
            }

		}

		public void PMToOldPM(CB_TradeAgreementPM entityPM, CB_TradeAgreementPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Title))
            {
                oldEntityPM.Title = entityPM.Title;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AdditionName))
            {
                oldEntityPM.AdditionName = entityPM.AdditionName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryGroupID))
            {
                oldEntityPM.CountryGroupID = entityPM.CountryGroupID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBookTypeID))
            {
                oldEntityPM.CustomsBookTypeID = entityPM.CustomsBookTypeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityStatusID))
            {
                oldEntityPM.EntityStatusID = entityPM.EntityStatusID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementAbbreviation))
            {
                oldEntityPM.TradeAgreementAbbreviation = entityPM.TradeAgreementAbbreviation;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_TradeAgreementPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Title)) //T4 find type == nText 
            {
                entityPM.Title = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Title));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.AdditionName)) //T4 find type == nText 
            {
                entityPM.AdditionName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.AdditionName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.TradeAgreementAbbreviation)) //T4 find type == nText 
            {
                entityPM.TradeAgreementAbbreviation = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.TradeAgreementAbbreviation));
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
	 