
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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class DigitalPortalScreenDataMapping: IMapping<DigitalPortalScreenPM, DigitalPortalScreen>,IMappingEncodeBase64NVARCHARFields<DigitalPortalScreenPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         UpdateDate, 
	         ObjectTableId, 
	         ScreenCode, 
	         Name, 
	         Content, 
	         DraftContent, 
	         ProfileId, 
	         IsList,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         UpdateDate, 
	         ObjectTableId, 
	         ScreenCode, 
	         Name, 
	         Content, 
	         DraftContent, 
	         ProfileId, 
	         ProfileCode, 
	         IsList,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DigitalPortalScreenPM entityPM, DigitalPortalScreen entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
				entityPOCO.ObjectTableId = entityPM.ObjectTableId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ScreenCode))
            {
				entityPOCO.ScreenCode = entityPM.ScreenCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
				entityPOCO.Name = entityPM.Name;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Content))
            {
				entityPOCO.Content = entityPM.Content;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DraftContent))
            {
				entityPOCO.DraftContent = entityPM.DraftContent;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfileId))
            {
				entityPOCO.ProfileId = entityPM.ProfileId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsList))
            {
				entityPOCO.IsList = entityPM.IsList;
			}
			}

		public void POCOToPM(DigitalPortalScreenPM entityPM, DigitalPortalScreen entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ObjectTableId))
            {
					entityPM.ObjectTableId = entityPOCO.ObjectTableId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ScreenCode))
            {
					entityPM.ScreenCode = entityPOCO.ScreenCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Name))
            {
					entityPM.Name = entityPOCO.Name;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Content))
            {
					entityPM.Content = entityPOCO.Content;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DraftContent))
            {
					entityPM.DraftContent = entityPOCO.DraftContent;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProfileId))
            {
					entityPM.ProfileId = entityPOCO.ProfileId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsList))
            {
					entityPM.IsList = entityPOCO.IsList;
            }

		}

		public void PMToOldPM(DigitalPortalScreenPM entityPM, DigitalPortalScreenPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
                oldEntityPM.ObjectTableId = entityPM.ObjectTableId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ScreenCode))
            {
                oldEntityPM.ScreenCode = entityPM.ScreenCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
                oldEntityPM.Name = entityPM.Name;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Content))
            {
                oldEntityPM.Content = entityPM.Content;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DraftContent))
            {
                oldEntityPM.DraftContent = entityPM.DraftContent;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfileId))
            {
                oldEntityPM.ProfileId = entityPM.ProfileId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsList))
            {
                oldEntityPM.IsList = entityPM.IsList;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DigitalPortalScreenPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.ScreenCode)) //T4 find type == nText 
            {
                entityPM.ScreenCode = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ScreenCode));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Name)) //T4 find type == nText 
            {
                entityPM.Name = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Name));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Content)) //T4 find type == nText 
            {
                entityPM.Content = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Content));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.DraftContent)) //T4 find type == nText 
            {
                entityPM.DraftContent = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DraftContent));
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
	 