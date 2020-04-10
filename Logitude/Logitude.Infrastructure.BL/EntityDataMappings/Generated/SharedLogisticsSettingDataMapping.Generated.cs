
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
	         IsIssuingCarrierAgentShared, 
	         IsCustomsAgentExportShared, 
	         IsCustomsAgentImportShared, 
	         IsCustomClearancePoinShared, 
	         IsConsolidatorShared, 
	         IsReleasingAgentShared, 
	         IsShipperShared, 
	         IsConsigneeShared, 
	         IsShowAmountLocalCurrency,
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
	         IsIssuingCarrierAgentShared, 
	         IsCustomsAgentExportShared, 
	         IsCustomsAgentImportShared, 
	         IsCustomClearancePoinShared, 
	         IsConsolidatorShared, 
	         IsReleasingAgentShared, 
	         IsShipperShared, 
	         IsConsigneeShared, 
	         IsShowAmountLocalCurrency,
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsIssuingCarrierAgentShared))
            {
				entityPOCO.IsIssuingCarrierAgentShared = entityPM.IsIssuingCarrierAgentShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCustomsAgentExportShared))
            {
				entityPOCO.IsCustomsAgentExportShared = entityPM.IsCustomsAgentExportShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCustomsAgentImportShared))
            {
				entityPOCO.IsCustomsAgentImportShared = entityPM.IsCustomsAgentImportShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCustomClearancePoinShared))
            {
				entityPOCO.IsCustomClearancePoinShared = entityPM.IsCustomClearancePoinShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConsolidatorShared))
            {
				entityPOCO.IsConsolidatorShared = entityPM.IsConsolidatorShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsReleasingAgentShared))
            {
				entityPOCO.IsReleasingAgentShared = entityPM.IsReleasingAgentShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsShipperShared))
            {
				entityPOCO.IsShipperShared = entityPM.IsShipperShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConsigneeShared))
            {
				entityPOCO.IsConsigneeShared = entityPM.IsConsigneeShared;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsShowAmountLocalCurrency))
            {
				entityPOCO.IsShowAmountLocalCurrency = entityPM.IsShowAmountLocalCurrency;
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsIssuingCarrierAgentShared))
            {
					entityPM.IsIssuingCarrierAgentShared = entityPOCO.IsIssuingCarrierAgentShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCustomsAgentExportShared))
            {
					entityPM.IsCustomsAgentExportShared = entityPOCO.IsCustomsAgentExportShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCustomsAgentImportShared))
            {
					entityPM.IsCustomsAgentImportShared = entityPOCO.IsCustomsAgentImportShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCustomClearancePoinShared))
            {
					entityPM.IsCustomClearancePoinShared = entityPOCO.IsCustomClearancePoinShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsConsolidatorShared))
            {
					entityPM.IsConsolidatorShared = entityPOCO.IsConsolidatorShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsReleasingAgentShared))
            {
					entityPM.IsReleasingAgentShared = entityPOCO.IsReleasingAgentShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsShipperShared))
            {
					entityPM.IsShipperShared = entityPOCO.IsShipperShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsConsigneeShared))
            {
					entityPM.IsConsigneeShared = entityPOCO.IsConsigneeShared;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsShowAmountLocalCurrency))
            {
					entityPM.IsShowAmountLocalCurrency = entityPOCO.IsShowAmountLocalCurrency;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsIssuingCarrierAgentShared))
            {
                oldEntityPM.IsIssuingCarrierAgentShared = entityPM.IsIssuingCarrierAgentShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCustomsAgentExportShared))
            {
                oldEntityPM.IsCustomsAgentExportShared = entityPM.IsCustomsAgentExportShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCustomsAgentImportShared))
            {
                oldEntityPM.IsCustomsAgentImportShared = entityPM.IsCustomsAgentImportShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCustomClearancePoinShared))
            {
                oldEntityPM.IsCustomClearancePoinShared = entityPM.IsCustomClearancePoinShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConsolidatorShared))
            {
                oldEntityPM.IsConsolidatorShared = entityPM.IsConsolidatorShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsReleasingAgentShared))
            {
                oldEntityPM.IsReleasingAgentShared = entityPM.IsReleasingAgentShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsShipperShared))
            {
                oldEntityPM.IsShipperShared = entityPM.IsShipperShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConsigneeShared))
            {
                oldEntityPM.IsConsigneeShared = entityPM.IsConsigneeShared;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsShowAmountLocalCurrency))
            {
                oldEntityPM.IsShowAmountLocalCurrency = entityPM.IsShowAmountLocalCurrency;
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
	 