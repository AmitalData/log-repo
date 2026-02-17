
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
   
   public partial class SupplierInvoiceItemProcesTypeDataMapping: IMapping<SupplierInvoiceItemProcesTypePM, SupplierInvoiceItemProcesType>,IMappingEncodeBase64NVARCHARFields<SupplierInvoiceItemProcesTypePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         InvoiceItemLineNumber, 
	         LineNumber, 
	         Tenant, 
	         ProcessTypeCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         InvoiceItemLineNumber, 
	         LineNumber, 
	         Tenant, 
	         ProcessTypeCode, 
	         ProcessTypeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SupplierInvoiceItemProcesTypePM entityPM, SupplierInvoiceItemProcesType entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProcessTypeCode))
            {
				entityPOCO.ProcessTypeCode = entityPM.ProcessTypeCode;
			}
			}

		public void POCOToPM(SupplierInvoiceItemProcesTypePM entityPM, SupplierInvoiceItemProcesType entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProcessTypeCode))
            {
					entityPM.ProcessTypeCode = entityPOCO.ProcessTypeCode;
            }

		}

		public void PMToOldPM(SupplierInvoiceItemProcesTypePM entityPM, SupplierInvoiceItemProcesTypePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProcessTypeCode))
            {
                oldEntityPM.ProcessTypeCode = entityPM.ProcessTypeCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(SupplierInvoiceItemProcesTypePM entityPM)
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
	 