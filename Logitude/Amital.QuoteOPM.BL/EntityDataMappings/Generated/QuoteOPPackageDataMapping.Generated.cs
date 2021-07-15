
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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs; 
using Amital.QuoteOPM.Data;

namespace Amital.QuoteOPM.BL.EntityDataMappings
{
   
   public partial class QuoteopPackageDataMapping: IMapping<QuoteopPackagePM, QuoteopPackage>,IMappingEncodeBase64NVARCHARFields<QuoteopPackagePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         QuoteOPId, 
	         PackageTypeId, 
	         Quantity, 
	         GrossWeight, 
	         Volume, 
	         Height, 
	         Width, 
	         Length, 
	         VolumetricWeight,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         QuoteOPId, 
	         PackageTypeId, 
	         PackageTypeName, 
	         Quantity, 
	         GrossWeight, 
	         Volume, 
	         Height, 
	         Width, 
	         Length, 
	         VolumetricWeight, 
	         Quote, 
	         Dimensions,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteopPackagePM entityPM, QuoteopPackage entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteOPId))
            {
				entityPOCO.QuoteOPId = entityPM.QuoteOPId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageTypeId))
            {
				entityPOCO.PackageTypeId = entityPM.PackageTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
				entityPOCO.Quantity = entityPM.Quantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeight))
            {
				entityPOCO.GrossWeight = entityPM.GrossWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
				entityPOCO.Volume = entityPM.Volume;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Height))
            {
				entityPOCO.Height = entityPM.Height;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Width))
            {
				entityPOCO.Width = entityPM.Width;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Length))
            {
				entityPOCO.Length = entityPM.Length;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumetricWeight))
            {
				entityPOCO.VolumetricWeight = entityPM.VolumetricWeight;
			}
			}

		public void POCOToPM(QuoteopPackagePM entityPM, QuoteopPackage entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteOPId))
            {
					entityPM.QuoteOPId = entityPOCO.QuoteOPId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageTypeId))
            {
					entityPM.PackageTypeId = entityPOCO.PackageTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Quantity))
            {
					entityPM.Quantity = entityPOCO.Quantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeight))
            {
					entityPM.GrossWeight = entityPOCO.GrossWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Volume))
            {
					entityPM.Volume = entityPOCO.Volume;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Height))
            {
					entityPM.Height = entityPOCO.Height;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Width))
            {
					entityPM.Width = entityPOCO.Width;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Length))
            {
					entityPM.Length = entityPOCO.Length;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VolumetricWeight))
            {
					entityPM.VolumetricWeight = entityPOCO.VolumetricWeight;
            }

		}

		public void PMToOldPM(QuoteopPackagePM entityPM, QuoteopPackagePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteOPId))
            {
                oldEntityPM.QuoteOPId = entityPM.QuoteOPId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageTypeId))
            {
                oldEntityPM.PackageTypeId = entityPM.PackageTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
                oldEntityPM.Quantity = entityPM.Quantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeight))
            {
                oldEntityPM.GrossWeight = entityPM.GrossWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
                oldEntityPM.Volume = entityPM.Volume;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Height))
            {
                oldEntityPM.Height = entityPM.Height;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Width))
            {
                oldEntityPM.Width = entityPM.Width;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Length))
            {
                oldEntityPM.Length = entityPM.Length;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumetricWeight))
            {
                oldEntityPM.VolumetricWeight = entityPM.VolumetricWeight;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuoteopPackagePM entityPM)
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
	 