
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
   
   public partial class VendorCommunicationDataMapping: IMapping<VendorCommunicationPM, VendorCommunication>,IMappingEncodeBase64NVARCHARFields<VendorCommunicationPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         SearchFields, 
	         VendorId, 
	         LineNumber, 
	         CommunicationTypeCode, 
	         CommunicationAddress,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         SearchFields, 
	         VendorId, 
	         LineNumber, 
	         CommunicationTypeCode, 
	         CommunicationAddress, 
	         CommunicationTypeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(VendorCommunicationPM entityPM, VendorCommunication entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationTypeCode))
            {
				entityPOCO.CommunicationTypeCode = entityPM.CommunicationTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationAddress))
            {
				entityPOCO.CommunicationAddress = entityPM.CommunicationAddress;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(VendorCommunicationPM entityPM, VendorCommunication entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VendorId))
            {
					entityPM.VendorId = entityPOCO.VendorId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CommunicationTypeCode))
            {
					entityPM.CommunicationTypeCode = entityPOCO.CommunicationTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CommunicationAddress))
            {
					entityPM.CommunicationAddress = entityPOCO.CommunicationAddress;
            }

		}

		public void PMToOldPM(VendorCommunicationPM entityPM, VendorCommunicationPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationTypeCode))
            {
                oldEntityPM.CommunicationTypeCode = entityPM.CommunicationTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationAddress))
            {
                oldEntityPM.CommunicationAddress = entityPM.CommunicationAddress;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(VendorCommunicationPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

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
		
		private void BuildSearchFieldsGenerated(VendorCommunicationPM entityPM, VendorCommunication entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 