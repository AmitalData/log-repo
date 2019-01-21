
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
   
   public partial class CustomsPartnerFtpDataMapping: IMapping<CustomsPartnerFtpPM, CustomsPartnerFtp>,IMappingEncodeBase64NVARCHARFields<CustomsPartnerFtpPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         TypeCode, 
	         PartnerCode, 
	         InterfaceName, 
	         FtpDetailsId, 
	         FileName, 
	         FileExt, 
	         CommunicationDetails,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         TypeCode, 
	         PartnerCode, 
	         InterfaceName, 
	         FtpDetailsId, 
	         FileName, 
	         FileExt, 
	         CommunicationDetails,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomsPartnerFtpPM entityPM, CustomsPartnerFtp entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypeCode))
            {
				entityPOCO.TypeCode = entityPM.TypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PartnerCode))
            {
				entityPOCO.PartnerCode = entityPM.PartnerCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterfaceName))
            {
				entityPOCO.InterfaceName = entityPM.InterfaceName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FtpDetailsId))
            {
				entityPOCO.FtpDetailsId = entityPM.FtpDetailsId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FileName))
            {
				entityPOCO.FileName = entityPM.FileName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FileExt))
            {
				entityPOCO.FileExt = entityPM.FileExt;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationDetails))
            {
				entityPOCO.CommunicationDetails = entityPM.CommunicationDetails;
			}
			}

		public void POCOToPM(CustomsPartnerFtpPM entityPM, CustomsPartnerFtp entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TypeCode))
            {
					entityPM.TypeCode = entityPOCO.TypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PartnerCode))
            {
					entityPM.PartnerCode = entityPOCO.PartnerCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterfaceName))
            {
					entityPM.InterfaceName = entityPOCO.InterfaceName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FtpDetailsId))
            {
					entityPM.FtpDetailsId = entityPOCO.FtpDetailsId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FileName))
            {
					entityPM.FileName = entityPOCO.FileName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FileExt))
            {
					entityPM.FileExt = entityPOCO.FileExt;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CommunicationDetails))
            {
					entityPM.CommunicationDetails = entityPOCO.CommunicationDetails;
            }

		}

		public void PMToOldPM(CustomsPartnerFtpPM entityPM, CustomsPartnerFtpPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypeCode))
            {
                oldEntityPM.TypeCode = entityPM.TypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PartnerCode))
            {
                oldEntityPM.PartnerCode = entityPM.PartnerCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterfaceName))
            {
                oldEntityPM.InterfaceName = entityPM.InterfaceName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FtpDetailsId))
            {
                oldEntityPM.FtpDetailsId = entityPM.FtpDetailsId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FileName))
            {
                oldEntityPM.FileName = entityPM.FileName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FileExt))
            {
                oldEntityPM.FileExt = entityPM.FileExt;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CommunicationDetails))
            {
                oldEntityPM.CommunicationDetails = entityPM.CommunicationDetails;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomsPartnerFtpPM entityPM)
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
	 