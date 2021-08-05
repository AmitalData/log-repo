
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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs; 
using Amital.QuoteOPM.Data;

namespace Amital.QuoteOPM.BL.EntityDataMappings
{
   
   public partial class QuoteOPComputedFieldDataMapping: IMapping<QuoteOPComputedFieldPM, QuoteOPComputedField>,IMappingEncodeBase64NVARCHARFields<QuoteOPComputedFieldPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         ConnectedToShipment, 
	         ConnectedToTicket, 
	         ToLocation, 
	         FromLocation, 
	         DeliveryTo, 
	         PickupFrom, 
	         Tenant, 
	         EstimatedPayablesInSales, 
	         EstimatedPayablesInLocal, 
	         EstimatedReceivablesInLocal, 
	         EstimatedReceivablesInSales, 
	         AutomaticLastUpdateDate,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         ConnectedToShipment, 
	         ConnectedToTicket, 
	         ToLocation, 
	         FromLocation, 
	         DeliveryTo, 
	         PickupFrom, 
	         Tenant, 
	         EstimatedPayablesInSales, 
	         EstimatedPayablesInLocal, 
	         EstimatedReceivablesInLocal, 
	         EstimatedReceivablesInSales,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPComputedFieldPM entityPM, QuoteOPComputedField entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConnectedToShipment))
            {
				entityPOCO.ConnectedToShipment = entityPM.ConnectedToShipment;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConnectedToTicket))
            {
				entityPOCO.ConnectedToTicket = entityPM.ConnectedToTicket;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToLocation))
            {
				entityPOCO.ToLocation = entityPM.ToLocation;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromLocation))
            {
				entityPOCO.FromLocation = entityPM.FromLocation;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryTo))
            {
				entityPOCO.DeliveryTo = entityPM.DeliveryTo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupFrom))
            {
				entityPOCO.PickupFrom = entityPM.PickupFrom;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedPayablesInSales))
            {
				entityPOCO.EstimatedPayablesInSales = entityPM.EstimatedPayablesInSales;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedPayablesInLocal))
            {
				entityPOCO.EstimatedPayablesInLocal = entityPM.EstimatedPayablesInLocal;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedReceivablesInLocal))
            {
				entityPOCO.EstimatedReceivablesInLocal = entityPM.EstimatedReceivablesInLocal;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedReceivablesInSales))
            {
				entityPOCO.EstimatedReceivablesInSales = entityPM.EstimatedReceivablesInSales;
			}
			}

		public void POCOToPM(QuoteOPComputedFieldPM entityPM, QuoteOPComputedField entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConnectedToShipment))
            {
					entityPM.ConnectedToShipment = entityPOCO.ConnectedToShipment;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConnectedToTicket))
            {
					entityPM.ConnectedToTicket = entityPOCO.ConnectedToTicket;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToLocation))
            {
					entityPM.ToLocation = entityPOCO.ToLocation;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromLocation))
            {
					entityPM.FromLocation = entityPOCO.FromLocation;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeliveryTo))
            {
					entityPM.DeliveryTo = entityPOCO.DeliveryTo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PickupFrom))
            {
					entityPM.PickupFrom = entityPOCO.PickupFrom;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EstimatedPayablesInSales))
            {
					entityPM.EstimatedPayablesInSales = entityPOCO.EstimatedPayablesInSales;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EstimatedPayablesInLocal))
            {
					entityPM.EstimatedPayablesInLocal = entityPOCO.EstimatedPayablesInLocal;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EstimatedReceivablesInLocal))
            {
					entityPM.EstimatedReceivablesInLocal = entityPOCO.EstimatedReceivablesInLocal;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EstimatedReceivablesInSales))
            {
					entityPM.EstimatedReceivablesInSales = entityPOCO.EstimatedReceivablesInSales;
            }

		}

		public void PMToOldPM(QuoteOPComputedFieldPM entityPM, QuoteOPComputedFieldPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConnectedToShipment))
            {
                oldEntityPM.ConnectedToShipment = entityPM.ConnectedToShipment;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConnectedToTicket))
            {
                oldEntityPM.ConnectedToTicket = entityPM.ConnectedToTicket;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToLocation))
            {
                oldEntityPM.ToLocation = entityPM.ToLocation;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromLocation))
            {
                oldEntityPM.FromLocation = entityPM.FromLocation;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryTo))
            {
                oldEntityPM.DeliveryTo = entityPM.DeliveryTo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupFrom))
            {
                oldEntityPM.PickupFrom = entityPM.PickupFrom;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedPayablesInSales))
            {
                oldEntityPM.EstimatedPayablesInSales = entityPM.EstimatedPayablesInSales;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedPayablesInLocal))
            {
                oldEntityPM.EstimatedPayablesInLocal = entityPM.EstimatedPayablesInLocal;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedReceivablesInLocal))
            {
                oldEntityPM.EstimatedReceivablesInLocal = entityPM.EstimatedReceivablesInLocal;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedReceivablesInSales))
            {
                oldEntityPM.EstimatedReceivablesInSales = entityPM.EstimatedReceivablesInSales;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPComputedFieldPM entityPM)
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
	 