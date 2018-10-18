
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
   
   public partial class DecConsAcceptanceDataMapping: IMapping<DecConsAcceptancePM, DecConsAcceptance>,IMappingEncodeBase64NVARCHARFields<DecConsAcceptancePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         DeclarationId, 
	         ConsignmentNumber, 
	         LineNumber, 
	         LoadDate, 
	         PackageTypeCode, 
	         PackageQuantity, 
	         GrossMassMeasure,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         DeclarationId, 
	         ConsignmentNumber, 
	         LineNumber, 
	         LoadDate, 
	         PackageTypeCode, 
	         PackageQuantity, 
	         GrossMassMeasure, 
	         LastLineNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DecConsAcceptancePM entityPM, DecConsAcceptance entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                entityPOCO.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LoadDate))
            {
                entityPOCO.LoadDate = entityPM.LoadDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageTypeCode))
            {
                entityPOCO.PackageTypeCode = entityPM.PackageTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageQuantity))
            {
                entityPOCO.PackageQuantity = entityPM.PackageQuantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossMassMeasure))
            {
                entityPOCO.GrossMassMeasure = entityPM.GrossMassMeasure;
            }
					}

		public void POCOToPM(DecConsAcceptancePM entityPM, DecConsAcceptance entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
                entityPM.Tenant = entityPOCO.Tenant;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
                entityPM.DeclarationId = entityPOCO.DeclarationId;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsignmentNumber))
            {
                entityPM.ConsignmentNumber = entityPOCO.ConsignmentNumber;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
                entityPM.LineNumber = entityPOCO.LineNumber;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LoadDate))
            {
                entityPM.LoadDate = entityPOCO.LoadDate;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageTypeCode))
            {
                entityPM.PackageTypeCode = entityPOCO.PackageTypeCode;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageQuantity))
            {
                entityPM.PackageQuantity = entityPOCO.PackageQuantity;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossMassMeasure))
            {
                entityPM.GrossMassMeasure = entityPOCO.GrossMassMeasure;
            }
			
		}

		public void PMToOldPM(DecConsAcceptancePM entityPM, DecConsAcceptancePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LoadDate))
            {
                oldEntityPM.LoadDate = entityPM.LoadDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageTypeCode))
            {
                oldEntityPM.PackageTypeCode = entityPM.PackageTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageQuantity))
            {
                oldEntityPM.PackageQuantity = entityPM.PackageQuantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossMassMeasure))
            {
                oldEntityPM.GrossMassMeasure = entityPM.GrossMassMeasure;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DecConsAcceptancePM entityPM)
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
	 