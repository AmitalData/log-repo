
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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class SharedLogisticsSettingDataMapping: IMapping<SharedLogisticsSettingPM, SharedLogisticsSetting>,IMappingEncodeBase64NVARCHARFields<SharedLogisticsSettingPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         IsAgentShared, 
	         IsShipperNotExporterShared, 
	         IsNotify1Shared, 
	         IsNotify2Shared, 
	         IsFreightForwarderShared, 
	         IsColoaderShared, 
	         IsConsigneeNotImporterShared, 
	         IsMainCarrierShared, 
	         IsPickDelivCarriesShared, 
	         IsInvoicesMenuEnabled, 
	         IsMoneyTabEnabled, 
	         Id,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         IsAgentShared, 
	         IsShipperNotExporterShared, 
	         IsNotify1Shared, 
	         IsNotify2Shared, 
	         IsFreightForwarderShared, 
	         IsColoaderShared, 
	         IsConsigneeNotImporterShared, 
	         IsMainCarrierShared, 
	         IsPickDelivCarriesShared, 
	         IsInvoicesMenuEnabled, 
	         IsMoneyTabEnabled, 
	         Id,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SharedLogisticsSettingPM entityPM, SharedLogisticsSetting entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAgentShared))
            {
				entityPOCO.IsAgentShared = entityPM.IsAgentShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsShipperNotExporterShared))
            {
				entityPOCO.IsShipperNotExporterShared = entityPM.IsShipperNotExporterShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsNotify1Shared))
            {
				entityPOCO.IsNotify1Shared = entityPM.IsNotify1Shared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsNotify2Shared))
            {
				entityPOCO.IsNotify2Shared = entityPM.IsNotify2Shared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsFreightForwarderShared))
            {
				entityPOCO.IsFreightForwarderShared = entityPM.IsFreightForwarderShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsColoaderShared))
            {
				entityPOCO.IsColoaderShared = entityPM.IsColoaderShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConsigneeNotImporterShared))
            {
				entityPOCO.IsConsigneeNotImporterShared = entityPM.IsConsigneeNotImporterShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMainCarrierShared))
            {
				entityPOCO.IsMainCarrierShared = entityPM.IsMainCarrierShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPickDelivCarriesShared))
            {
				entityPOCO.IsPickDelivCarriesShared = entityPM.IsPickDelivCarriesShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsInvoicesMenuEnabled))
            {
				entityPOCO.IsInvoicesMenuEnabled = entityPM.IsInvoicesMenuEnabled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMoneyTabEnabled))
            {
				entityPOCO.IsMoneyTabEnabled = entityPM.IsMoneyTabEnabled;
			}
			}

		public void POCOToPM(SharedLogisticsSettingPM entityPM, SharedLogisticsSetting entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsAgentShared))
            {
					entityPM.IsAgentShared = entityPOCO.IsAgentShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsShipperNotExporterShared))
            {
					entityPM.IsShipperNotExporterShared = entityPOCO.IsShipperNotExporterShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsNotify1Shared))
            {
					entityPM.IsNotify1Shared = entityPOCO.IsNotify1Shared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsNotify2Shared))
            {
					entityPM.IsNotify2Shared = entityPOCO.IsNotify2Shared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsFreightForwarderShared))
            {
					entityPM.IsFreightForwarderShared = entityPOCO.IsFreightForwarderShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsColoaderShared))
            {
					entityPM.IsColoaderShared = entityPOCO.IsColoaderShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsConsigneeNotImporterShared))
            {
					entityPM.IsConsigneeNotImporterShared = entityPOCO.IsConsigneeNotImporterShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsMainCarrierShared))
            {
					entityPM.IsMainCarrierShared = entityPOCO.IsMainCarrierShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsPickDelivCarriesShared))
            {
					entityPM.IsPickDelivCarriesShared = entityPOCO.IsPickDelivCarriesShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsInvoicesMenuEnabled))
            {
					entityPM.IsInvoicesMenuEnabled = entityPOCO.IsInvoicesMenuEnabled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsMoneyTabEnabled))
            {
					entityPM.IsMoneyTabEnabled = entityPOCO.IsMoneyTabEnabled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

		}

		public void PMToOldPM(SharedLogisticsSettingPM entityPM, SharedLogisticsSettingPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAgentShared))
            {
                oldEntityPM.IsAgentShared = entityPM.IsAgentShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsShipperNotExporterShared))
            {
                oldEntityPM.IsShipperNotExporterShared = entityPM.IsShipperNotExporterShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsNotify1Shared))
            {
                oldEntityPM.IsNotify1Shared = entityPM.IsNotify1Shared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsNotify2Shared))
            {
                oldEntityPM.IsNotify2Shared = entityPM.IsNotify2Shared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsFreightForwarderShared))
            {
                oldEntityPM.IsFreightForwarderShared = entityPM.IsFreightForwarderShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsColoaderShared))
            {
                oldEntityPM.IsColoaderShared = entityPM.IsColoaderShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConsigneeNotImporterShared))
            {
                oldEntityPM.IsConsigneeNotImporterShared = entityPM.IsConsigneeNotImporterShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMainCarrierShared))
            {
                oldEntityPM.IsMainCarrierShared = entityPM.IsMainCarrierShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPickDelivCarriesShared))
            {
                oldEntityPM.IsPickDelivCarriesShared = entityPM.IsPickDelivCarriesShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsInvoicesMenuEnabled))
            {
                oldEntityPM.IsInvoicesMenuEnabled = entityPM.IsInvoicesMenuEnabled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMoneyTabEnabled))
            {
                oldEntityPM.IsMoneyTabEnabled = entityPM.IsMoneyTabEnabled;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(SharedLogisticsSettingPM entityPM)
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
	 