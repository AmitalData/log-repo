
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
   
   public partial class GatepassRequestDataMapping: IMapping<GatepassRequestPM, GatepassRequest>,IMappingEncodeBase64NVARCHARFields<GatepassRequestPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         MasterCourierId, 
	         Tenant, 
	         GatepassNumber, 
	         OriginSiteCode, 
	         UpdateCode, 
	         DesignateSiteCode, 
	         TransportationTypeCode, 
	         GatepassRequestStatus, 
	         CustomsUpdateDateTime,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         MasterCourierId, 
	         Tenant, 
	         GatepassNumber, 
	         OriginSiteCode, 
	         UpdateCode, 
	         DesignateSiteCode, 
	         TransportationTypeCode, 
	         GatepassRequestStatus, 
	         CustomsUpdateDateTime, 
	         OriginSiteName, 
	         UpdateCodeName, 
	         DesignateSiteName, 
	         TransportationTypeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(GatepassRequestPM entityPM, GatepassRequest entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GatepassNumber))
            {
				entityPOCO.GatepassNumber = entityPM.GatepassNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginSiteCode))
            {
				entityPOCO.OriginSiteCode = entityPM.OriginSiteCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateCode))
            {
				entityPOCO.UpdateCode = entityPM.UpdateCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DesignateSiteCode))
            {
				entityPOCO.DesignateSiteCode = entityPM.DesignateSiteCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportationTypeCode))
            {
				entityPOCO.TransportationTypeCode = entityPM.TransportationTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GatepassRequestStatus))
            {
				entityPOCO.GatepassRequestStatus = entityPM.GatepassRequestStatus;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsUpdateDateTime))
            {
				entityPOCO.CustomsUpdateDateTime = entityPM.CustomsUpdateDateTime;
			}
			}

		public void POCOToPM(GatepassRequestPM entityPM, GatepassRequest entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MasterCourierId))
            {
					entityPM.MasterCourierId = entityPOCO.MasterCourierId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GatepassNumber))
            {
					entityPM.GatepassNumber = entityPOCO.GatepassNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginSiteCode))
            {
					entityPM.OriginSiteCode = entityPOCO.OriginSiteCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateCode))
            {
					entityPM.UpdateCode = entityPOCO.UpdateCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DesignateSiteCode))
            {
					entityPM.DesignateSiteCode = entityPOCO.DesignateSiteCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransportationTypeCode))
            {
					entityPM.TransportationTypeCode = entityPOCO.TransportationTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GatepassRequestStatus))
            {
					entityPM.GatepassRequestStatus = entityPOCO.GatepassRequestStatus;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsUpdateDateTime))
            {
					entityPM.CustomsUpdateDateTime = entityPOCO.CustomsUpdateDateTime;
            }

		}

		public void PMToOldPM(GatepassRequestPM entityPM, GatepassRequestPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GatepassNumber))
            {
                oldEntityPM.GatepassNumber = entityPM.GatepassNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginSiteCode))
            {
                oldEntityPM.OriginSiteCode = entityPM.OriginSiteCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateCode))
            {
                oldEntityPM.UpdateCode = entityPM.UpdateCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DesignateSiteCode))
            {
                oldEntityPM.DesignateSiteCode = entityPM.DesignateSiteCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportationTypeCode))
            {
                oldEntityPM.TransportationTypeCode = entityPM.TransportationTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GatepassRequestStatus))
            {
                oldEntityPM.GatepassRequestStatus = entityPM.GatepassRequestStatus;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsUpdateDateTime))
            {
                oldEntityPM.CustomsUpdateDateTime = entityPM.CustomsUpdateDateTime;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(GatepassRequestPM entityPM)
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
	 