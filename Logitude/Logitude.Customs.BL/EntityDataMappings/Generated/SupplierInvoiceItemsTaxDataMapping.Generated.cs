
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
   
   public partial class SupplierInvoiceItemsTaxDataMapping: IMapping<SupplierInvoiceItemsTaxPM, SupplierInvoiceItemsTax>,IMappingEncodeBase64NVARCHARFields<SupplierInvoiceItemsTaxPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         LineNumber, 
	         TaxTypeCode, 
	         Tenant, 
	         TradeAgreementTypeCode, 
	         TaxRate, 
	         TaxBaseAmount, 
	         TaxAmount, 
	         DeferedTaxAmount, 
	         DefinedPerUnitMeasure, 
	         AlternateDefinedPerUnitMeasure, 
	         DefinedPerUnitQuantity, 
	         AlternateDefinedPerUnitQuant, 
	         MeasurementUnitCode, 
	         AlternateMeasurementUnitCode, 
	         TradeLevyNumber, 
	         TotalBtlCoverageNIS, 
	         AlternateRate,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         LineNumber, 
	         TaxTypeCode, 
	         Tenant, 
	         TradeAgreementTypeCode, 
	         TaxRate, 
	         TaxBaseAmount, 
	         TaxAmount, 
	         DeferedTaxAmount, 
	         DefinedPerUnitMeasure, 
	         AlternateDefinedPerUnitMeasure, 
	         DefinedPerUnitQuantity, 
	         AlternateDefinedPerUnitQuant, 
	         MeasurementUnitCode, 
	         AlternateMeasurementUnitCode, 
	         TradeLevyNumber, 
	         TotalBtlCoverageNIS, 
	         AlternateRate, 
	         TaxTypeName, 
	         TradeAgreementTypeName, 
	         MeasurementUnitName, 
	         AlternateMeasurementUnitName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SupplierInvoiceItemsTaxPM entityPM, SupplierInvoiceItemsTax entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementTypeCode))
            {
				entityPOCO.TradeAgreementTypeCode = entityPM.TradeAgreementTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxRate))
            {
				entityPOCO.TaxRate = entityPM.TaxRate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxBaseAmount))
            {
				entityPOCO.TaxBaseAmount = entityPM.TaxBaseAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxAmount))
            {
				entityPOCO.TaxAmount = entityPM.TaxAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeferedTaxAmount))
            {
				entityPOCO.DeferedTaxAmount = entityPM.DeferedTaxAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefinedPerUnitMeasure))
            {
				entityPOCO.DefinedPerUnitMeasure = entityPM.DefinedPerUnitMeasure;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AlternateDefinedPerUnitMeasure))
            {
				entityPOCO.AlternateDefinedPerUnitMeasure = entityPM.AlternateDefinedPerUnitMeasure;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefinedPerUnitQuantity))
            {
				entityPOCO.DefinedPerUnitQuantity = entityPM.DefinedPerUnitQuantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AlternateDefinedPerUnitQuant))
            {
				entityPOCO.AlternateDefinedPerUnitQuant = entityPM.AlternateDefinedPerUnitQuant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MeasurementUnitCode))
            {
				entityPOCO.MeasurementUnitCode = entityPM.MeasurementUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AlternateMeasurementUnitCode))
            {
				entityPOCO.AlternateMeasurementUnitCode = entityPM.AlternateMeasurementUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeLevyNumber))
            {
				entityPOCO.TradeLevyNumber = entityPM.TradeLevyNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalBtlCoverageNIS))
            {
				entityPOCO.TotalBtlCoverageNIS = entityPM.TotalBtlCoverageNIS;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AlternateRate))
            {
				entityPOCO.AlternateRate = entityPM.AlternateRate;
			}
			}

		public void POCOToPM(SupplierInvoiceItemsTaxPM entityPM, SupplierInvoiceItemsTax entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceCounterKey))
            {
					entityPM.InvoiceCounterKey = entityPOCO.InvoiceCounterKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxTypeCode))
            {
					entityPM.TaxTypeCode = entityPOCO.TaxTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TradeAgreementTypeCode))
            {
					entityPM.TradeAgreementTypeCode = entityPOCO.TradeAgreementTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxRate))
            {
					entityPM.TaxRate = entityPOCO.TaxRate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxBaseAmount))
            {
					entityPM.TaxBaseAmount = entityPOCO.TaxBaseAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxAmount))
            {
					entityPM.TaxAmount = entityPOCO.TaxAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeferedTaxAmount))
            {
					entityPM.DeferedTaxAmount = entityPOCO.DeferedTaxAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DefinedPerUnitMeasure))
            {
					entityPM.DefinedPerUnitMeasure = entityPOCO.DefinedPerUnitMeasure;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AlternateDefinedPerUnitMeasure))
            {
					entityPM.AlternateDefinedPerUnitMeasure = entityPOCO.AlternateDefinedPerUnitMeasure;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DefinedPerUnitQuantity))
            {
					entityPM.DefinedPerUnitQuantity = entityPOCO.DefinedPerUnitQuantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AlternateDefinedPerUnitQuant))
            {
					entityPM.AlternateDefinedPerUnitQuant = entityPOCO.AlternateDefinedPerUnitQuant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MeasurementUnitCode))
            {
					entityPM.MeasurementUnitCode = entityPOCO.MeasurementUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AlternateMeasurementUnitCode))
            {
					entityPM.AlternateMeasurementUnitCode = entityPOCO.AlternateMeasurementUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TradeLevyNumber))
            {
					entityPM.TradeLevyNumber = entityPOCO.TradeLevyNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalBtlCoverageNIS))
            {
					entityPM.TotalBtlCoverageNIS = entityPOCO.TotalBtlCoverageNIS;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AlternateRate))
            {
					entityPM.AlternateRate = entityPOCO.AlternateRate;
            }

		}

		public void PMToOldPM(SupplierInvoiceItemsTaxPM entityPM, SupplierInvoiceItemsTaxPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementTypeCode))
            {
                oldEntityPM.TradeAgreementTypeCode = entityPM.TradeAgreementTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxRate))
            {
                oldEntityPM.TaxRate = entityPM.TaxRate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxBaseAmount))
            {
                oldEntityPM.TaxBaseAmount = entityPM.TaxBaseAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxAmount))
            {
                oldEntityPM.TaxAmount = entityPM.TaxAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeferedTaxAmount))
            {
                oldEntityPM.DeferedTaxAmount = entityPM.DeferedTaxAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefinedPerUnitMeasure))
            {
                oldEntityPM.DefinedPerUnitMeasure = entityPM.DefinedPerUnitMeasure;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AlternateDefinedPerUnitMeasure))
            {
                oldEntityPM.AlternateDefinedPerUnitMeasure = entityPM.AlternateDefinedPerUnitMeasure;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefinedPerUnitQuantity))
            {
                oldEntityPM.DefinedPerUnitQuantity = entityPM.DefinedPerUnitQuantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AlternateDefinedPerUnitQuant))
            {
                oldEntityPM.AlternateDefinedPerUnitQuant = entityPM.AlternateDefinedPerUnitQuant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MeasurementUnitCode))
            {
                oldEntityPM.MeasurementUnitCode = entityPM.MeasurementUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AlternateMeasurementUnitCode))
            {
                oldEntityPM.AlternateMeasurementUnitCode = entityPM.AlternateMeasurementUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeLevyNumber))
            {
                oldEntityPM.TradeLevyNumber = entityPM.TradeLevyNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalBtlCoverageNIS))
            {
                oldEntityPM.TotalBtlCoverageNIS = entityPM.TotalBtlCoverageNIS;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AlternateRate))
            {
                oldEntityPM.AlternateRate = entityPM.AlternateRate;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(SupplierInvoiceItemsTaxPM entityPM)
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
	 