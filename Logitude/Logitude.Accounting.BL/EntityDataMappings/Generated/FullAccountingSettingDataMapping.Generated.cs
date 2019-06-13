
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
   
   public partial class FullAccountingSettingDataMapping: IMapping<FullAccountingSettingPM, FullAccountingSetting>,IMappingEncodeBase64NVARCHARFields<FullAccountingSettingPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         DeductionFileNumber, 
	         ConsolidationVAT, 
	         DefaultVATTypeId, 
	         VATInputsGLAccountId, 
	         AutomaticReconcileMethodId, 
	         ExchangeRateDiffGLAccountId, 
	         RevenueExpenseGLAccountId, 
	         Id, 
	         VATOutputGLAccountId, 
	         CustomerControlAccountId, 
	         VendorControlAccountId, 
	         FileControlAccountId, 
	         OceanExportJobControlAccountId, 
	         AirExportJobControlAccountId, 
	         OceanImportJobControlAccountId, 
	         AirImportJobControlAccountId, 
	         ExternalReconciliationDefault, 
	         TaxWithholdingGLAccountId, 
	         DefaultTaxWithholdPercentage, 
	         CustomsGLAccountId, 
	         DefaultDifferencesGLAccountId, 
	         DefaultExternalDiffGLAccountId, 
	         SoftwareVersion, 
	         IsPaymentChequesActivated,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         DeductionFileNumber, 
	         ConsolidationVAT, 
	         DefaultVATTypeId, 
	         VATInputsGLAccountId, 
	         AutomaticReconcileMethodId, 
	         ExchangeRateDiffGLAccountId, 
	         RevenueExpenseGLAccountId, 
	         Id, 
	         VATOutputGLAccountId, 
	         CustomerControlAccountId, 
	         CustomerControlAccountName, 
	         CustomerControlAccountNumber, 
	         VendorControlAccountId, 
	         VendorControlAccountName, 
	         VendorControlAccountNumber, 
	         FileControlAccountId, 
	         FileControlAccountName, 
	         FileControlAccountNumber, 
	         OceanExportJobControlAccountId, 
	         OceanExportJobControlAccountName, 
	         OceanExportJobControlAccountNumber, 
	         AirExportJobControlAccountId, 
	         AirExportJobControlAccountName, 
	         AirExportJobControlAccountNumber, 
	         OceanImportJobControlAccountId, 
	         OceanImportJobControlAccountName, 
	         OceanImportJobControlAccountNumber, 
	         AirImportJobControlAccountId, 
	         AirImportJobControlAccountName, 
	         AirImportJobControlAccountNumber, 
	         TenantPaymentTermId, 
	         AccountingActivationDate, 
	         AccountingActivated, 
	         ExternalReconciliationDefault, 
	         TaxWithholdingGLAccountId, 
	         DefaultTaxWithholdPercentage, 
	         CustomsGLAccountId, 
	         DefaultDifferencesGLAccountId, 
	         DefaultExternalDiffGLAccountId, 
	         SoftwareVersion, 
	         IsPaymentChequesActivated,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(FullAccountingSettingPM entityPM, FullAccountingSetting entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeductionFileNumber))
            {
				entityPOCO.DeductionFileNumber = entityPM.DeductionFileNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsolidationVAT))
            {
				entityPOCO.ConsolidationVAT = entityPM.ConsolidationVAT;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultVATTypeId))
            {
				entityPOCO.DefaultVATTypeId = entityPM.DefaultVATTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VATInputsGLAccountId))
            {
				entityPOCO.VATInputsGLAccountId = entityPM.VATInputsGLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AutomaticReconcileMethodId))
            {
				entityPOCO.AutomaticReconcileMethodId = entityPM.AutomaticReconcileMethodId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExchangeRateDiffGLAccountId))
            {
				entityPOCO.ExchangeRateDiffGLAccountId = entityPM.ExchangeRateDiffGLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RevenueExpenseGLAccountId))
            {
				entityPOCO.RevenueExpenseGLAccountId = entityPM.RevenueExpenseGLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VATOutputGLAccountId))
            {
				entityPOCO.VATOutputGLAccountId = entityPM.VATOutputGLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerControlAccountId))
            {
				entityPOCO.CustomerControlAccountId = entityPM.CustomerControlAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorControlAccountId))
            {
				entityPOCO.VendorControlAccountId = entityPM.VendorControlAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FileControlAccountId))
            {
				entityPOCO.FileControlAccountId = entityPM.FileControlAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OceanExportJobControlAccountId))
            {
				entityPOCO.OceanExportJobControlAccountId = entityPM.OceanExportJobControlAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirExportJobControlAccountId))
            {
				entityPOCO.AirExportJobControlAccountId = entityPM.AirExportJobControlAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OceanImportJobControlAccountId))
            {
				entityPOCO.OceanImportJobControlAccountId = entityPM.OceanImportJobControlAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirImportJobControlAccountId))
            {
				entityPOCO.AirImportJobControlAccountId = entityPM.AirImportJobControlAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalReconciliationDefault))
            {
				entityPOCO.ExternalReconciliationDefault = entityPM.ExternalReconciliationDefault;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxWithholdingGLAccountId))
            {
				entityPOCO.TaxWithholdingGLAccountId = entityPM.TaxWithholdingGLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultTaxWithholdPercentage))
            {
				entityPOCO.DefaultTaxWithholdPercentage = entityPM.DefaultTaxWithholdPercentage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsGLAccountId))
            {
				entityPOCO.CustomsGLAccountId = entityPM.CustomsGLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultDifferencesGLAccountId))
            {
				entityPOCO.DefaultDifferencesGLAccountId = entityPM.DefaultDifferencesGLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultExternalDiffGLAccountId))
            {
				entityPOCO.DefaultExternalDiffGLAccountId = entityPM.DefaultExternalDiffGLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SoftwareVersion))
            {
				entityPOCO.SoftwareVersion = entityPM.SoftwareVersion;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPaymentChequesActivated))
            {
				entityPOCO.IsPaymentChequesActivated = entityPM.IsPaymentChequesActivated;
			}
			}

		public void POCOToPM(FullAccountingSettingPM entityPM, FullAccountingSetting entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeductionFileNumber))
            {
					entityPM.DeductionFileNumber = entityPOCO.DeductionFileNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsolidationVAT))
            {
					entityPM.ConsolidationVAT = entityPOCO.ConsolidationVAT;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DefaultVATTypeId))
            {
					entityPM.DefaultVATTypeId = entityPOCO.DefaultVATTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VATInputsGLAccountId))
            {
					entityPM.VATInputsGLAccountId = entityPOCO.VATInputsGLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AutomaticReconcileMethodId))
            {
					entityPM.AutomaticReconcileMethodId = entityPOCO.AutomaticReconcileMethodId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExchangeRateDiffGLAccountId))
            {
					entityPM.ExchangeRateDiffGLAccountId = entityPOCO.ExchangeRateDiffGLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RevenueExpenseGLAccountId))
            {
					entityPM.RevenueExpenseGLAccountId = entityPOCO.RevenueExpenseGLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VATOutputGLAccountId))
            {
					entityPM.VATOutputGLAccountId = entityPOCO.VATOutputGLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerControlAccountId))
            {
					entityPM.CustomerControlAccountId = entityPOCO.CustomerControlAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VendorControlAccountId))
            {
					entityPM.VendorControlAccountId = entityPOCO.VendorControlAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FileControlAccountId))
            {
					entityPM.FileControlAccountId = entityPOCO.FileControlAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OceanExportJobControlAccountId))
            {
					entityPM.OceanExportJobControlAccountId = entityPOCO.OceanExportJobControlAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AirExportJobControlAccountId))
            {
					entityPM.AirExportJobControlAccountId = entityPOCO.AirExportJobControlAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OceanImportJobControlAccountId))
            {
					entityPM.OceanImportJobControlAccountId = entityPOCO.OceanImportJobControlAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AirImportJobControlAccountId))
            {
					entityPM.AirImportJobControlAccountId = entityPOCO.AirImportJobControlAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExternalReconciliationDefault))
            {
					entityPM.ExternalReconciliationDefault = entityPOCO.ExternalReconciliationDefault;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxWithholdingGLAccountId))
            {
					entityPM.TaxWithholdingGLAccountId = entityPOCO.TaxWithholdingGLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DefaultTaxWithholdPercentage))
            {
					entityPM.DefaultTaxWithholdPercentage = entityPOCO.DefaultTaxWithholdPercentage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsGLAccountId))
            {
					entityPM.CustomsGLAccountId = entityPOCO.CustomsGLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DefaultDifferencesGLAccountId))
            {
					entityPM.DefaultDifferencesGLAccountId = entityPOCO.DefaultDifferencesGLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DefaultExternalDiffGLAccountId))
            {
					entityPM.DefaultExternalDiffGLAccountId = entityPOCO.DefaultExternalDiffGLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SoftwareVersion))
            {
					entityPM.SoftwareVersion = entityPOCO.SoftwareVersion;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsPaymentChequesActivated))
            {
					entityPM.IsPaymentChequesActivated = entityPOCO.IsPaymentChequesActivated;
            }

		}

		public void PMToOldPM(FullAccountingSettingPM entityPM, FullAccountingSettingPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeductionFileNumber))
            {
                oldEntityPM.DeductionFileNumber = entityPM.DeductionFileNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsolidationVAT))
            {
                oldEntityPM.ConsolidationVAT = entityPM.ConsolidationVAT;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultVATTypeId))
            {
                oldEntityPM.DefaultVATTypeId = entityPM.DefaultVATTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VATInputsGLAccountId))
            {
                oldEntityPM.VATInputsGLAccountId = entityPM.VATInputsGLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AutomaticReconcileMethodId))
            {
                oldEntityPM.AutomaticReconcileMethodId = entityPM.AutomaticReconcileMethodId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExchangeRateDiffGLAccountId))
            {
                oldEntityPM.ExchangeRateDiffGLAccountId = entityPM.ExchangeRateDiffGLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RevenueExpenseGLAccountId))
            {
                oldEntityPM.RevenueExpenseGLAccountId = entityPM.RevenueExpenseGLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VATOutputGLAccountId))
            {
                oldEntityPM.VATOutputGLAccountId = entityPM.VATOutputGLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerControlAccountId))
            {
                oldEntityPM.CustomerControlAccountId = entityPM.CustomerControlAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorControlAccountId))
            {
                oldEntityPM.VendorControlAccountId = entityPM.VendorControlAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FileControlAccountId))
            {
                oldEntityPM.FileControlAccountId = entityPM.FileControlAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OceanExportJobControlAccountId))
            {
                oldEntityPM.OceanExportJobControlAccountId = entityPM.OceanExportJobControlAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirExportJobControlAccountId))
            {
                oldEntityPM.AirExportJobControlAccountId = entityPM.AirExportJobControlAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OceanImportJobControlAccountId))
            {
                oldEntityPM.OceanImportJobControlAccountId = entityPM.OceanImportJobControlAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirImportJobControlAccountId))
            {
                oldEntityPM.AirImportJobControlAccountId = entityPM.AirImportJobControlAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalReconciliationDefault))
            {
                oldEntityPM.ExternalReconciliationDefault = entityPM.ExternalReconciliationDefault;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxWithholdingGLAccountId))
            {
                oldEntityPM.TaxWithholdingGLAccountId = entityPM.TaxWithholdingGLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultTaxWithholdPercentage))
            {
                oldEntityPM.DefaultTaxWithholdPercentage = entityPM.DefaultTaxWithholdPercentage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsGLAccountId))
            {
                oldEntityPM.CustomsGLAccountId = entityPM.CustomsGLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultDifferencesGLAccountId))
            {
                oldEntityPM.DefaultDifferencesGLAccountId = entityPM.DefaultDifferencesGLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultExternalDiffGLAccountId))
            {
                oldEntityPM.DefaultExternalDiffGLAccountId = entityPM.DefaultExternalDiffGLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SoftwareVersion))
            {
                oldEntityPM.SoftwareVersion = entityPM.SoftwareVersion;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPaymentChequesActivated))
            {
                oldEntityPM.IsPaymentChequesActivated = entityPM.IsPaymentChequesActivated;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(FullAccountingSettingPM entityPM)
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
	 