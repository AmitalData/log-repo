
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
   
   public partial class SupplierInvoiceItemVehicleDataMapping: IMapping<SupplierInvoiceItemVehiclePM, SupplierInvoiceItemVehicle>,IMappingEncodeBase64NVARCHARFields<SupplierInvoiceItemVehiclePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         InvoiceItemLineNumber, 
	         LineNumber, 
	         SequenceNumeric, 
	         VehicleTypeCode, 
	         VehicleChassisNumber, 
	         RichbitFileNumber, 
	         VehicleId, 
	         Tenant, 
	         ExcludeFromInterface, 
	         IdentifierID,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         InvoiceItemLineNumber, 
	         LineNumber, 
	         SequenceNumeric, 
	         VehicleTypeCode, 
	         VehicleChassisNumber, 
	         RichbitFileNumber, 
	         VehicleId, 
	         Tenant, 
	         RichbitFileStatus, 
	         ExcludeFromInterface, 
	         IdentifierID, 
	         VehicleTypeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SupplierInvoiceItemVehiclePM entityPM, SupplierInvoiceItemVehicle entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
				entityPOCO.SequenceNumeric = entityPM.SequenceNumeric;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleTypeCode))
            {
				entityPOCO.VehicleTypeCode = entityPM.VehicleTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleChassisNumber))
            {
				entityPOCO.VehicleChassisNumber = entityPM.VehicleChassisNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RichbitFileNumber))
            {
				entityPOCO.RichbitFileNumber = entityPM.RichbitFileNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleId))
            {
				entityPOCO.VehicleId = entityPM.VehicleId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExcludeFromInterface))
            {
				entityPOCO.ExcludeFromInterface = entityPM.ExcludeFromInterface;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IdentifierID))
            {
				entityPOCO.IdentifierID = entityPM.IdentifierID;
			}
			}

		public void POCOToPM(SupplierInvoiceItemVehiclePM entityPM, SupplierInvoiceItemVehicle entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SequenceNumeric))
            {
					entityPM.SequenceNumeric = entityPOCO.SequenceNumeric;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VehicleTypeCode))
            {
					entityPM.VehicleTypeCode = entityPOCO.VehicleTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VehicleChassisNumber))
            {
					entityPM.VehicleChassisNumber = entityPOCO.VehicleChassisNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RichbitFileNumber))
            {
					entityPM.RichbitFileNumber = entityPOCO.RichbitFileNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VehicleId))
            {
					entityPM.VehicleId = entityPOCO.VehicleId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExcludeFromInterface))
            {
					entityPM.ExcludeFromInterface = entityPOCO.ExcludeFromInterface;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IdentifierID))
            {
					entityPM.IdentifierID = entityPOCO.IdentifierID;
            }

		}

		public void PMToOldPM(SupplierInvoiceItemVehiclePM entityPM, SupplierInvoiceItemVehiclePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
                oldEntityPM.SequenceNumeric = entityPM.SequenceNumeric;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleTypeCode))
            {
                oldEntityPM.VehicleTypeCode = entityPM.VehicleTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleChassisNumber))
            {
                oldEntityPM.VehicleChassisNumber = entityPM.VehicleChassisNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RichbitFileNumber))
            {
                oldEntityPM.RichbitFileNumber = entityPM.RichbitFileNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleId))
            {
                oldEntityPM.VehicleId = entityPM.VehicleId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExcludeFromInterface))
            {
                oldEntityPM.ExcludeFromInterface = entityPM.ExcludeFromInterface;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IdentifierID))
            {
                oldEntityPM.IdentifierID = entityPM.IdentifierID;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(SupplierInvoiceItemVehiclePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.IdentifierID)) //T4 find type == nText 
            {
                entityPM.IdentifierID = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.IdentifierID));
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
	 