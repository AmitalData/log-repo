
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
   
   public partial class DecCargoSplitConsPackDetDataMapping: IMapping<DecCargoSplitConsPackDetPM, DecCargoSplitConsPackDet>,IMappingEncodeBase64NVARCHARFields<DecCargoSplitConsPackDetPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationCargoSplitId, 
	         Tenant, 
	         DecCargoSplitConsLineNo, 
	         DecCargoSplitConsItemLine, 
	         PackageLine, 
	         PackageQuantity, 
	         GrossMassMeasure, 
	         PackageTypeCode, 
	         MarksNumbers, 
	         ManifestNumber,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationCargoSplitId, 
	         Tenant, 
	         DecCargoSplitConsLineNo, 
	         DecCargoSplitConsItemLine, 
	         PackageLine, 
	         PackageQuantity, 
	         GrossMassMeasure, 
	         PackageTypeCode, 
	         PackageTypeName, 
	         MarksNumbers, 
	         ManifestNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DecCargoSplitConsPackDetPM entityPM, DecCargoSplitConsPackDet entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManifestNumber))
            {
				entityPOCO.ManifestNumber = entityPM.ManifestNumber;
			}
			}

		public void POCOToPM(DecCargoSplitConsPackDetPM entityPM, DecCargoSplitConsPackDet entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationCargoSplitId))
            {
					entityPM.DeclarationCargoSplitId = entityPOCO.DeclarationCargoSplitId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DecCargoSplitConsLineNo))
            {
					entityPM.DecCargoSplitConsLineNo = entityPOCO.DecCargoSplitConsLineNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DecCargoSplitConsItemLine))
            {
					entityPM.DecCargoSplitConsItemLine = entityPOCO.DecCargoSplitConsItemLine;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageLine))
            {
					entityPM.PackageLine = entityPOCO.PackageLine;
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ManifestNumber))
            {
					entityPM.ManifestNumber = entityPOCO.ManifestNumber;
            }

		}

		public void PMToOldPM(DecCargoSplitConsPackDetPM entityPM, DecCargoSplitConsPackDetPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManifestNumber))
            {
                oldEntityPM.ManifestNumber = entityPM.ManifestNumber;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DecCargoSplitConsPackDetPM entityPM)
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
	 