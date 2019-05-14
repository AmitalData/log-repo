
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
   
   public partial class PendingByKeywordDataMapping: IMapping<PendingByKeywordPM, PendingByKeyword>,IMappingEncodeBase64NVARCHARFields<PendingByKeywordPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CourierPendingReasonCode, 
	         KeywordsList,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CourierPendingReasonCode, 
	         CourierPendingReasonName, 
	         KeywordsList,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(PendingByKeywordPM entityPM, PendingByKeyword entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierPendingReasonCode))
            {
				entityPOCO.CourierPendingReasonCode = entityPM.CourierPendingReasonCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.KeywordsList))
            {
				entityPOCO.KeywordsList = entityPM.KeywordsList;
			}
			}

		public void POCOToPM(PendingByKeywordPM entityPM, PendingByKeyword entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CourierPendingReasonCode))
            {
					entityPM.CourierPendingReasonCode = entityPOCO.CourierPendingReasonCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.KeywordsList))
            {
					entityPM.KeywordsList = entityPOCO.KeywordsList;
            }

		}

		public void PMToOldPM(PendingByKeywordPM entityPM, PendingByKeywordPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierPendingReasonCode))
            {
                oldEntityPM.CourierPendingReasonCode = entityPM.CourierPendingReasonCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.KeywordsList))
            {
                oldEntityPM.KeywordsList = entityPM.KeywordsList;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(PendingByKeywordPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.KeywordsList)) //T4 find type == nText 
            {
                entityPM.KeywordsList = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.KeywordsList));
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
	 