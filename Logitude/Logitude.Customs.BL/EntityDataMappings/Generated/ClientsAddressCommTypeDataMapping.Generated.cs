
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
   
   public partial class ClientsAddressCommTypeDataMapping: IMapping<ClientsAddressCommTypePM, ClientsAddressCommType>,IMappingEncodeBase64NVARCHARFields<ClientsAddressCommTypePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ClientId, 
	         AddressId, 
	         Line, 
	         CommunicationTypeCode, 
	         CommunicationAddress, 
	         Tenant,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ClientId, 
	         AddressId, 
	         Line, 
	         CommunicationTypeCode, 
	         CommunicationAddress, 
	         Tenant, 
	         CommunicationTypeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ClientsAddressCommTypePM entityPM, ClientsAddressCommType entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationTypeCode))
            {
				entityPOCO.CommunicationTypeCode = entityPM.CommunicationTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationAddress))
            {
				entityPOCO.CommunicationAddress = entityPM.CommunicationAddress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			}

		public void POCOToPM(ClientsAddressCommTypePM entityPM, ClientsAddressCommType entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClientId))
            {
					entityPM.ClientId = entityPOCO.ClientId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AddressId))
            {
					entityPM.AddressId = entityPOCO.AddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Line))
            {
					entityPM.Line = entityPOCO.Line;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CommunicationTypeCode))
            {
					entityPM.CommunicationTypeCode = entityPOCO.CommunicationTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CommunicationAddress))
            {
					entityPM.CommunicationAddress = entityPOCO.CommunicationAddress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

		}

		public void PMToOldPM(ClientsAddressCommTypePM entityPM, ClientsAddressCommTypePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationTypeCode))
            {
                oldEntityPM.CommunicationTypeCode = entityPM.CommunicationTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationAddress))
            {
                oldEntityPM.CommunicationAddress = entityPM.CommunicationAddress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ClientsAddressCommTypePM entityPM)
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
	 