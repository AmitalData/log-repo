
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
   
   public partial class PaymentOrderConnectionTableDataMapping: IMapping<PaymentOrderConnectionTablePM, PaymentOrderConnectionTable>,IMappingEncodeBase64NVARCHARFields<PaymentOrderConnectionTablePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         PaymentOrderId, 
	         ConnectedEntityCode, 
	         ConnectedEntityId, 
	         Tenant,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         PaymentOrderId, 
	         ConnectedEntityCode, 
	         ConnectedEntityId, 
	         Tenant,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(PaymentOrderConnectionTablePM entityPM, PaymentOrderConnectionTable entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConnectedEntityCode))
            {
				entityPOCO.ConnectedEntityCode = entityPM.ConnectedEntityCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			}

		public void POCOToPM(PaymentOrderConnectionTablePM entityPM, PaymentOrderConnectionTable entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentOrderId))
            {
					entityPM.PaymentOrderId = entityPOCO.PaymentOrderId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConnectedEntityCode))
            {
					entityPM.ConnectedEntityCode = entityPOCO.ConnectedEntityCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConnectedEntityId))
            {
					entityPM.ConnectedEntityId = entityPOCO.ConnectedEntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

		}

		public void PMToOldPM(PaymentOrderConnectionTablePM entityPM, PaymentOrderConnectionTablePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConnectedEntityCode))
            {
                oldEntityPM.ConnectedEntityCode = entityPM.ConnectedEntityCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(PaymentOrderConnectionTablePM entityPM)
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
	 