
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
   
   public partial class ClientsTapagDataMapping: IMapping<ClientsTapagPM, ClientsTapag>,IMappingEncodeBase64NVARCHARFields<ClientsTapagPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ClientId, 
	         TapagNumber,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ClientId, 
	         TapagNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ClientsTapagPM entityPM, ClientsTapag entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClientId))
            {
				entityPOCO.ClientId = entityPM.ClientId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TapagNumber))
            {
				entityPOCO.TapagNumber = entityPM.TapagNumber;
			}
			}

		public void POCOToPM(ClientsTapagPM entityPM, ClientsTapag entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClientId))
            {
					entityPM.ClientId = entityPOCO.ClientId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TapagNumber))
            {
					entityPM.TapagNumber = entityPOCO.TapagNumber;
            }

		}

		public void PMToOldPM(ClientsTapagPM entityPM, ClientsTapagPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClientId))
            {
                oldEntityPM.ClientId = entityPM.ClientId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TapagNumber))
            {
                oldEntityPM.TapagNumber = entityPM.TapagNumber;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ClientsTapagPM entityPM)
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
	 