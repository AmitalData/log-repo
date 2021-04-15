
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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class GLAccountCardsDataDataMapping: IMapping<GLAccountCardsDataPM, GLAccountCardsData>,IMappingEncodeBase64NVARCHARFields<GLAccountCardsDataPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SalesmanUserId, 
	         CreditLimit, 
	         PaymentTermId, 
	         CollectorUserId, 
	         Phone, 
	         VatNumber, 
	         TotalOpenShipments,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SalesmanUserId, 
	         CreditLimit, 
	         PaymentTermId, 
	         CollectorUserId, 
	         Phone, 
	         VatNumber, 
	         TotalOpenShipments,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(GLAccountCardsDataPM entityPM, GLAccountCardsData entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SalesmanUserId))
            {
				entityPOCO.SalesmanUserId = entityPM.SalesmanUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreditLimit))
            {
				entityPOCO.CreditLimit = entityPM.CreditLimit;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentTermId))
            {
				entityPOCO.PaymentTermId = entityPM.PaymentTermId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CollectorUserId))
            {
				entityPOCO.CollectorUserId = entityPM.CollectorUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Phone))
            {
				entityPOCO.Phone = entityPM.Phone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatNumber))
            {
				entityPOCO.VatNumber = entityPM.VatNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalOpenShipments))
            {
				entityPOCO.TotalOpenShipments = entityPM.TotalOpenShipments;
			}
			}

		public void POCOToPM(GLAccountCardsDataPM entityPM, GLAccountCardsData entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SalesmanUserId))
            {
					entityPM.SalesmanUserId = entityPOCO.SalesmanUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreditLimit))
            {
					entityPM.CreditLimit = entityPOCO.CreditLimit;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentTermId))
            {
					entityPM.PaymentTermId = entityPOCO.PaymentTermId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CollectorUserId))
            {
					entityPM.CollectorUserId = entityPOCO.CollectorUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Phone))
            {
					entityPM.Phone = entityPOCO.Phone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VatNumber))
            {
					entityPM.VatNumber = entityPOCO.VatNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalOpenShipments))
            {
					entityPM.TotalOpenShipments = entityPOCO.TotalOpenShipments;
            }

		}

		public void PMToOldPM(GLAccountCardsDataPM entityPM, GLAccountCardsDataPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SalesmanUserId))
            {
                oldEntityPM.SalesmanUserId = entityPM.SalesmanUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreditLimit))
            {
                oldEntityPM.CreditLimit = entityPM.CreditLimit;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentTermId))
            {
                oldEntityPM.PaymentTermId = entityPM.PaymentTermId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CollectorUserId))
            {
                oldEntityPM.CollectorUserId = entityPM.CollectorUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Phone))
            {
                oldEntityPM.Phone = entityPM.Phone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatNumber))
            {
                oldEntityPM.VatNumber = entityPM.VatNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalOpenShipments))
            {
                oldEntityPM.TotalOpenShipments = entityPM.TotalOpenShipments;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(GLAccountCardsDataPM entityPM)
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
	 