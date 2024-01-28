
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
   
   public partial class CB_CustomsItemLinkageDataMapping: IMapping<CB_CustomsItemLinkagePM, CB_CustomsItemLinkage>,IMappingEncodeBase64NVARCHARFields<CB_CustomsItemLinkagePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         ChangeTypeID, 
	         CustomsItemDetailsHistoryID, 
	         Connect_CustItemDetailsHistID, 
	         CreateDate,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         ChangeTypeID, 
	         CustomsItemDetailsHistoryID, 
	         Connect_CustItemDetailsHistID, 
	         CreateDate,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_CustomsItemLinkagePM entityPM, CB_CustomsItemLinkage entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeTypeID))
            {
				entityPOCO.ChangeTypeID = entityPM.ChangeTypeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemDetailsHistoryID))
            {
				entityPOCO.CustomsItemDetailsHistoryID = entityPM.CustomsItemDetailsHistoryID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Connect_CustItemDetailsHistID))
            {
				entityPOCO.Connect_CustItemDetailsHistID = entityPM.Connect_CustItemDetailsHistID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			}

		public void POCOToPM(CB_CustomsItemLinkagePM entityPM, CB_CustomsItemLinkage entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ID))
            {
					entityPM.ID = entityPOCO.ID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChangeTypeID))
            {
					entityPM.ChangeTypeID = entityPOCO.ChangeTypeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsItemDetailsHistoryID))
            {
					entityPM.CustomsItemDetailsHistoryID = entityPOCO.CustomsItemDetailsHistoryID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Connect_CustItemDetailsHistID))
            {
					entityPM.Connect_CustItemDetailsHistID = entityPOCO.Connect_CustItemDetailsHistID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

		}

		public void PMToOldPM(CB_CustomsItemLinkagePM entityPM, CB_CustomsItemLinkagePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeTypeID))
            {
                oldEntityPM.ChangeTypeID = entityPM.ChangeTypeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemDetailsHistoryID))
            {
                oldEntityPM.CustomsItemDetailsHistoryID = entityPM.CustomsItemDetailsHistoryID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Connect_CustItemDetailsHistID))
            {
                oldEntityPM.Connect_CustItemDetailsHistID = entityPM.Connect_CustItemDetailsHistID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_CustomsItemLinkagePM entityPM)
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
	 