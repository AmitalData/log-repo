
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.BL.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class SupplierInvoiceItemVehiclesAddtionalDataMapping: IMapping<SupplierInvoiceItemVehiclesAddtionalPM, SupplierInvoiceItemVehiclesAddtional>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         InvoiceItemLineNumber, 
	         LineNumber, 
	         VehicleModel, 
	         RichbitNumber, 
	         ChassisNumber, 
	         EngineNumber, 
	         VehicleValue, 
	         ChassisTax, 
	         ChassisPurchaseTax, 
	         ChassisVat, 
	         Exempt_type,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         InvoiceCounterKey, 
	         InvoiceItemLineNumber, 
	         LineNumber, 
	         VehicleModel, 
	         RichbitNumber, 
	         ChassisNumber, 
	         EngineNumber, 
	         VehicleValue, 
	         ChassisTax, 
	         ChassisPurchaseTax, 
	         ChassisVat, 
	         Exempt_type,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SupplierInvoiceItemVehiclesAddtionalPM entityPM, SupplierInvoiceItemVehiclesAddtional entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleModel))
            {
                entityPOCO.VehicleModel = entityPM.VehicleModel;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RichbitNumber))
            {
                entityPOCO.RichbitNumber = entityPM.RichbitNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChassisNumber))
            {
                entityPOCO.ChassisNumber = entityPM.ChassisNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EngineNumber))
            {
                entityPOCO.EngineNumber = entityPM.EngineNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleValue))
            {
                entityPOCO.VehicleValue = entityPM.VehicleValue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChassisTax))
            {
                entityPOCO.ChassisTax = entityPM.ChassisTax;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChassisPurchaseTax))
            {
                entityPOCO.ChassisPurchaseTax = entityPM.ChassisPurchaseTax;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChassisVat))
            {
                entityPOCO.ChassisVat = entityPM.ChassisVat;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Exempt_type))
            {
                entityPOCO.Exempt_type = entityPM.Exempt_type;
            }
					}

		public void POCOToPM(SupplierInvoiceItemVehiclesAddtionalPM entityPM, SupplierInvoiceItemVehiclesAddtional entityPOCO)
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
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VehicleModel))
            {
                entityPM.VehicleModel = entityPOCO.VehicleModel;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RichbitNumber))
            {
                entityPM.RichbitNumber = entityPOCO.RichbitNumber;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChassisNumber))
            {
                entityPM.ChassisNumber = entityPOCO.ChassisNumber;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EngineNumber))
            {
                entityPM.EngineNumber = entityPOCO.EngineNumber;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VehicleValue))
            {
                entityPM.VehicleValue = entityPOCO.VehicleValue;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChassisTax))
            {
                entityPM.ChassisTax = entityPOCO.ChassisTax;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChassisPurchaseTax))
            {
                entityPM.ChassisPurchaseTax = entityPOCO.ChassisPurchaseTax;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChassisVat))
            {
                entityPM.ChassisVat = entityPOCO.ChassisVat;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Exempt_type))
            {
                entityPM.Exempt_type = entityPOCO.Exempt_type;
            }
			
		}

		public void PMToOldPM(SupplierInvoiceItemVehiclesAddtionalPM entityPM, SupplierInvoiceItemVehiclesAddtionalPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleModel))
            {
                oldEntityPM.VehicleModel = entityPM.VehicleModel;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RichbitNumber))
            {
                oldEntityPM.RichbitNumber = entityPM.RichbitNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChassisNumber))
            {
                oldEntityPM.ChassisNumber = entityPM.ChassisNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EngineNumber))
            {
                oldEntityPM.EngineNumber = entityPM.EngineNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleValue))
            {
                oldEntityPM.VehicleValue = entityPM.VehicleValue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChassisTax))
            {
                oldEntityPM.ChassisTax = entityPM.ChassisTax;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChassisPurchaseTax))
            {
                oldEntityPM.ChassisPurchaseTax = entityPM.ChassisPurchaseTax;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChassisVat))
            {
                oldEntityPM.ChassisVat = entityPM.ChassisVat;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Exempt_type))
            {
                oldEntityPM.Exempt_type = entityPM.Exempt_type;
            }
			
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
	 