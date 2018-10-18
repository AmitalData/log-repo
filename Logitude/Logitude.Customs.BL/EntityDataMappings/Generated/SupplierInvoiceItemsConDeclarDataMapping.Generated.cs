
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
   
   public partial class SupplierInvoiceItemsConDeclarDataMapping: IMapping<SupplierInvoiceItemsConDeclarPM, SupplierInvoiceItemsConDeclar>,IMappingEncodeBase64NVARCHARFields<SupplierInvoiceItemsConDeclarPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         InvoiceItemLineNumber, 
	         LineNumber, 
	         Tenant, 
	         DeclarationNumber, 
	         ItemSequence, 
	         DeclarationTypeCode, 
	         InvoiceNumber, 
	         Quantity, 
	         QuantityTypeCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         InvoiceItemLineNumber, 
	         LineNumber, 
	         Tenant, 
	         DeclarationNumber, 
	         ItemSequence, 
	         DeclarationTypeCode, 
	         InvoiceNumber, 
	         Quantity, 
	         DeclarationTypeName, 
	         QuantityTypeCode, 
	         QuantityTypeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SupplierInvoiceItemsConDeclarPM entityPM, SupplierInvoiceItemsConDeclar entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationNumber))
            {
				entityPOCO.DeclarationNumber = entityPM.DeclarationNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemSequence))
            {
				entityPOCO.ItemSequence = entityPM.ItemSequence;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationTypeCode))
            {
				entityPOCO.DeclarationTypeCode = entityPM.DeclarationTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceNumber))
            {
				entityPOCO.InvoiceNumber = entityPM.InvoiceNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
				entityPOCO.Quantity = entityPM.Quantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuantityTypeCode))
            {
				entityPOCO.QuantityTypeCode = entityPM.QuantityTypeCode;
			}
			}

		public void POCOToPM(SupplierInvoiceItemsConDeclarPM entityPM, SupplierInvoiceItemsConDeclar entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceCounterKey))
            {
					entityPM.InvoiceCounterKey = entityPOCO.InvoiceCounterKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceItemLineNumber))
            {
					entityPM.InvoiceItemLineNumber = entityPOCO.InvoiceItemLineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationNumber))
            {
					entityPM.DeclarationNumber = entityPOCO.DeclarationNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ItemSequence))
            {
					entityPM.ItemSequence = entityPOCO.ItemSequence;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationTypeCode))
            {
					entityPM.DeclarationTypeCode = entityPOCO.DeclarationTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceNumber))
            {
					entityPM.InvoiceNumber = entityPOCO.InvoiceNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Quantity))
            {
					entityPM.Quantity = entityPOCO.Quantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuantityTypeCode))
            {
					entityPM.QuantityTypeCode = entityPOCO.QuantityTypeCode;
            }

		}

		public void PMToOldPM(SupplierInvoiceItemsConDeclarPM entityPM, SupplierInvoiceItemsConDeclarPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationNumber))
            {
                oldEntityPM.DeclarationNumber = entityPM.DeclarationNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemSequence))
            {
                oldEntityPM.ItemSequence = entityPM.ItemSequence;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationTypeCode))
            {
                oldEntityPM.DeclarationTypeCode = entityPM.DeclarationTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceNumber))
            {
                oldEntityPM.InvoiceNumber = entityPM.InvoiceNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
                oldEntityPM.Quantity = entityPM.Quantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuantityTypeCode))
            {
                oldEntityPM.QuantityTypeCode = entityPM.QuantityTypeCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(SupplierInvoiceItemsConDeclarPM entityPM)
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
	 