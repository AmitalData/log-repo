
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
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs; 
using Logitude.TariffModule.Data;

namespace Logitude.TariffModule.BL.EntityDataMappings
{
   
   public partial class TariffVersionUploadedExcelDataMapping: IMapping<TariffVersionUploadedExcelPM, TariffVersionUploadedExcel>,IMappingEncodeBase64NVARCHARFields<TariffVersionUploadedExcelPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         UploadDate, 
	         UploadedByUserId, 
	         TariffId, 
	         Version, 
	         DocumentId, 
	         NumberOfLines, 
	         Index,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         UploadDate, 
	         UploadedByUserId, 
	         TariffId, 
	         Version, 
	         DocumentId, 
	         NumberOfLines, 
	         Index, 
	         UploadedByUserName, 
	         FileName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TariffVersionUploadedExcelPM entityPM, TariffVersionUploadedExcel entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UploadDate))
            {
				entityPOCO.UploadDate = entityPM.UploadDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UploadedByUserId))
            {
				entityPOCO.UploadedByUserId = entityPM.UploadedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffId))
            {
				entityPOCO.TariffId = entityPM.TariffId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Version))
            {
				entityPOCO.Version = entityPM.Version;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentId))
            {
				entityPOCO.DocumentId = entityPM.DocumentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfLines))
            {
				entityPOCO.NumberOfLines = entityPM.NumberOfLines;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Index))
            {
				entityPOCO.Index = entityPM.Index;
			}
			}

		public void POCOToPM(TariffVersionUploadedExcelPM entityPM, TariffVersionUploadedExcel entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UploadDate))
            {
					entityPM.UploadDate = entityPOCO.UploadDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UploadedByUserId))
            {
					entityPM.UploadedByUserId = entityPOCO.UploadedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TariffId))
            {
					entityPM.TariffId = entityPOCO.TariffId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Version))
            {
					entityPM.Version = entityPOCO.Version;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DocumentId))
            {
					entityPM.DocumentId = entityPOCO.DocumentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NumberOfLines))
            {
					entityPM.NumberOfLines = entityPOCO.NumberOfLines;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Index))
            {
					entityPM.Index = entityPOCO.Index;
            }

		}

		public void PMToOldPM(TariffVersionUploadedExcelPM entityPM, TariffVersionUploadedExcelPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UploadDate))
            {
                oldEntityPM.UploadDate = entityPM.UploadDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UploadedByUserId))
            {
                oldEntityPM.UploadedByUserId = entityPM.UploadedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffId))
            {
                oldEntityPM.TariffId = entityPM.TariffId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Version))
            {
                oldEntityPM.Version = entityPM.Version;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentId))
            {
                oldEntityPM.DocumentId = entityPM.DocumentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfLines))
            {
                oldEntityPM.NumberOfLines = entityPM.NumberOfLines;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Index))
            {
                oldEntityPM.Index = entityPM.Index;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TariffVersionUploadedExcelPM entityPM)
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
	 