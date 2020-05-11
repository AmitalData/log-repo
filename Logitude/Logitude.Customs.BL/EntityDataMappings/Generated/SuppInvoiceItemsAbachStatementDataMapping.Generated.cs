
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
   
   public partial class SuppInvoiceItemsAbachStatementDataMapping: IMapping<SuppInvoiceItemsAbachStatementPM, SuppInvoiceItemsAbachStatement>,IMappingEncodeBase64NVARCHARFields<SuppInvoiceItemsAbachStatementPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         InvoiceItemLineNumber, 
	         SequenceNumeric, 
	         Tenant, 
	         StatementType, 
	         StatementInd,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         InvoiceItemLineNumber, 
	         SequenceNumeric, 
	         Tenant, 
	         StatementType, 
	         StatementInd,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SuppInvoiceItemsAbachStatementPM entityPM, SuppInvoiceItemsAbachStatement entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatementType))
            {
				entityPOCO.StatementType = entityPM.StatementType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatementInd))
            {
				entityPOCO.StatementInd = entityPM.StatementInd;
			}
			}

		public void POCOToPM(SuppInvoiceItemsAbachStatementPM entityPM, SuppInvoiceItemsAbachStatement entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SequenceNumeric))
            {
					entityPM.SequenceNumeric = entityPOCO.SequenceNumeric;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatementType))
            {
					entityPM.StatementType = entityPOCO.StatementType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatementInd))
            {
					entityPM.StatementInd = entityPOCO.StatementInd;
            }

		}

		public void PMToOldPM(SuppInvoiceItemsAbachStatementPM entityPM, SuppInvoiceItemsAbachStatementPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatementType))
            {
                oldEntityPM.StatementType = entityPM.StatementType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatementInd))
            {
                oldEntityPM.StatementInd = entityPM.StatementInd;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(SuppInvoiceItemsAbachStatementPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.StatementType)) //T4 find type == nText 
            {
                entityPM.StatementType = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.StatementType));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.StatementInd)) //T4 find type == nText 
            {
                entityPM.StatementInd = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.StatementInd));
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
	 