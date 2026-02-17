
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
   
   public partial class ClientDrivingLicenseDataMapping: IMapping<ClientDrivingLicensePM, ClientDrivingLicense>,IMappingEncodeBase64NVARCHARFields<ClientDrivingLicensePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ClientId, 
	         Tenant, 
	         Line, 
	         DrivingLicenseNumber, 
	         DriverLicenseValidityDate, 
	         DrivingLicenseCountryID,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ClientId, 
	         Tenant, 
	         Line, 
	         DrivingLicenseNumber, 
	         DriverLicenseValidityDate, 
	         DrivingLicenseCountryID, 
	         DrivingLicenseCountryName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ClientDrivingLicensePM entityPM, ClientDrivingLicense entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DrivingLicenseNumber))
            {
				entityPOCO.DrivingLicenseNumber = entityPM.DrivingLicenseNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DriverLicenseValidityDate))
            {
				entityPOCO.DriverLicenseValidityDate = entityPM.DriverLicenseValidityDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DrivingLicenseCountryID))
            {
				entityPOCO.DrivingLicenseCountryID = entityPM.DrivingLicenseCountryID;
			}
			}

		public void POCOToPM(ClientDrivingLicensePM entityPM, ClientDrivingLicense entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClientId))
            {
					entityPM.ClientId = entityPOCO.ClientId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Line))
            {
					entityPM.Line = entityPOCO.Line;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DrivingLicenseNumber))
            {
					entityPM.DrivingLicenseNumber = entityPOCO.DrivingLicenseNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DriverLicenseValidityDate))
            {
					entityPM.DriverLicenseValidityDate = entityPOCO.DriverLicenseValidityDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DrivingLicenseCountryID))
            {
					entityPM.DrivingLicenseCountryID = entityPOCO.DrivingLicenseCountryID;
            }

		}

		public void PMToOldPM(ClientDrivingLicensePM entityPM, ClientDrivingLicensePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DrivingLicenseNumber))
            {
                oldEntityPM.DrivingLicenseNumber = entityPM.DrivingLicenseNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DriverLicenseValidityDate))
            {
                oldEntityPM.DriverLicenseValidityDate = entityPM.DriverLicenseValidityDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DrivingLicenseCountryID))
            {
                oldEntityPM.DrivingLicenseCountryID = entityPM.DrivingLicenseCountryID;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ClientDrivingLicensePM entityPM)
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
	 