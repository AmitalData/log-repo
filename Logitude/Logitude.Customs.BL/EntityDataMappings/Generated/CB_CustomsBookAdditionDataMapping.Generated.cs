
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
   
   public partial class CB_CustomsBookAdditionDataMapping: IMapping<CB_CustomsBookAdditionPM, CB_CustomsBookAddition>,IMappingEncodeBase64NVARCHARFields<CB_CustomsBookAdditionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         TypeID, 
	         Title, 
	         CustomsBookTypeID, 
	         AdditionCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         TypeID, 
	         Title, 
	         CustomsBookTypeID, 
	         AdditionCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_CustomsBookAdditionPM entityPM, CB_CustomsBookAddition entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypeID))
            {
				entityPOCO.TypeID = entityPM.TypeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Title))
            {
				entityPOCO.Title = entityPM.Title;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBookTypeID))
            {
				entityPOCO.CustomsBookTypeID = entityPM.CustomsBookTypeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AdditionCode))
            {
				entityPOCO.AdditionCode = entityPM.AdditionCode;
			}
			}

		public void POCOToPM(CB_CustomsBookAdditionPM entityPM, CB_CustomsBookAddition entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TypeID))
            {
					entityPM.TypeID = entityPOCO.TypeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Title))
            {
					entityPM.Title = entityPOCO.Title;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsBookTypeID))
            {
					entityPM.CustomsBookTypeID = entityPOCO.CustomsBookTypeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AdditionCode))
            {
					entityPM.AdditionCode = entityPOCO.AdditionCode;
            }

		}

		public void PMToOldPM(CB_CustomsBookAdditionPM entityPM, CB_CustomsBookAdditionPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypeID))
            {
                oldEntityPM.TypeID = entityPM.TypeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Title))
            {
                oldEntityPM.Title = entityPM.Title;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBookTypeID))
            {
                oldEntityPM.CustomsBookTypeID = entityPM.CustomsBookTypeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AdditionCode))
            {
                oldEntityPM.AdditionCode = entityPM.AdditionCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_CustomsBookAdditionPM entityPM)
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
	 