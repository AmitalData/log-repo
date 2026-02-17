
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
   
   public partial class ConsignmentPackageDataMapping: IMapping<ConsignmentPackagePM, ConsignmentPackage>,IMappingEncodeBase64NVARCHARFields<ConsignmentPackagePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Tenant, 
	         ConsignmentNumber, 
	         LineNumber, 
	         PackageMeasureQualifierCode, 
	         PackageQuantity, 
	         GrossMassMeasure, 
	         PackageTypeCode, 
	         MarksNumbers, 
	         SequenceNumeric, 
	         PackageQuantityTypeCode, 
	         GrossMassMeasureTypeCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Tenant, 
	         ConsignmentNumber, 
	         LineNumber, 
	         PackageMeasureQualifierCode, 
	         PackageMeasureQualifierName, 
	         PackageQuantity, 
	         GrossMassMeasure, 
	         PackageTypeCode, 
	         PackageTypeName, 
	         MarksNumbers, 
	         SequenceNumeric, 
	         PackageQuantityTypeCode, 
	         PackageQuantityTypeName, 
	         GrossMassMeasureTypeCode, 
	         GrossMassMeasureTypeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ConsignmentPackagePM entityPM, ConsignmentPackage entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageMeasureQualifierCode))
            {
				entityPOCO.PackageMeasureQualifierCode = entityPM.PackageMeasureQualifierCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageQuantity))
            {
				entityPOCO.PackageQuantity = entityPM.PackageQuantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossMassMeasure))
            {
				entityPOCO.GrossMassMeasure = entityPM.GrossMassMeasure;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageTypeCode))
            {
				entityPOCO.PackageTypeCode = entityPM.PackageTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MarksNumbers))
            {
				entityPOCO.MarksNumbers = entityPM.MarksNumbers;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
				entityPOCO.SequenceNumeric = entityPM.SequenceNumeric;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageQuantityTypeCode))
            {
				entityPOCO.PackageQuantityTypeCode = entityPM.PackageQuantityTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossMassMeasureTypeCode))
            {
				entityPOCO.GrossMassMeasureTypeCode = entityPM.GrossMassMeasureTypeCode;
			}
			}

		public void POCOToPM(ConsignmentPackagePM entityPM, ConsignmentPackage entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsignmentNumber))
            {
					entityPM.ConsignmentNumber = entityPOCO.ConsignmentNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageMeasureQualifierCode))
            {
					entityPM.PackageMeasureQualifierCode = entityPOCO.PackageMeasureQualifierCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageQuantity))
            {
					entityPM.PackageQuantity = entityPOCO.PackageQuantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossMassMeasure))
            {
					entityPM.GrossMassMeasure = entityPOCO.GrossMassMeasure;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageTypeCode))
            {
					entityPM.PackageTypeCode = entityPOCO.PackageTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MarksNumbers))
            {
					entityPM.MarksNumbers = entityPOCO.MarksNumbers;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SequenceNumeric))
            {
					entityPM.SequenceNumeric = entityPOCO.SequenceNumeric;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageQuantityTypeCode))
            {
					entityPM.PackageQuantityTypeCode = entityPOCO.PackageQuantityTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossMassMeasureTypeCode))
            {
					entityPM.GrossMassMeasureTypeCode = entityPOCO.GrossMassMeasureTypeCode;
            }

		}

		public void PMToOldPM(ConsignmentPackagePM entityPM, ConsignmentPackagePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageMeasureQualifierCode))
            {
                oldEntityPM.PackageMeasureQualifierCode = entityPM.PackageMeasureQualifierCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageQuantity))
            {
                oldEntityPM.PackageQuantity = entityPM.PackageQuantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossMassMeasure))
            {
                oldEntityPM.GrossMassMeasure = entityPM.GrossMassMeasure;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageTypeCode))
            {
                oldEntityPM.PackageTypeCode = entityPM.PackageTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MarksNumbers))
            {
                oldEntityPM.MarksNumbers = entityPM.MarksNumbers;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
                oldEntityPM.SequenceNumeric = entityPM.SequenceNumeric;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageQuantityTypeCode))
            {
                oldEntityPM.PackageQuantityTypeCode = entityPM.PackageQuantityTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossMassMeasureTypeCode))
            {
                oldEntityPM.GrossMassMeasureTypeCode = entityPM.GrossMassMeasureTypeCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ConsignmentPackagePM entityPM)
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
	 