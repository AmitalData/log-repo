
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
   
   public partial class ClientDrivingLicenseTypeDataMapping: IMapping<ClientDrivingLicenseTypePM, ClientDrivingLicenseType>,IMappingEncodeBase64NVARCHARFields<ClientDrivingLicenseTypePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ClientId, 
	         Tenant, 
	         ClientDrivingLicenseLine, 
	         DriversLicenseTypeCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ClientId, 
	         Tenant, 
	         ClientDrivingLicenseLine, 
	         DriversLicenseTypeCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ClientDrivingLicenseTypePM entityPM, ClientDrivingLicenseType entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			}

		public void POCOToPM(ClientDrivingLicenseTypePM entityPM, ClientDrivingLicenseType entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClientId))
            {
					entityPM.ClientId = entityPOCO.ClientId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClientDrivingLicenseLine))
            {
					entityPM.ClientDrivingLicenseLine = entityPOCO.ClientDrivingLicenseLine;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DriversLicenseTypeCode))
            {
					entityPM.DriversLicenseTypeCode = entityPOCO.DriversLicenseTypeCode;
            }

		}

		public void PMToOldPM(ClientDrivingLicenseTypePM entityPM, ClientDrivingLicenseTypePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ClientDrivingLicenseTypePM entityPM)
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
	 