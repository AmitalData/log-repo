
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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class ContainerSettingDataMapping: IMapping<ContainerSettingPM, ContainerSetting>,IMappingEncodeBase64NVARCHARFields<ContainerSettingPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         EmptyReturnClosingDays, 
	         ShipmentATAClosingDays, 
	         ShipmentATADateIndicator, 
	         IsExport, 
	         IsDomestic, 
	         IsImport, 
	         IsDrop, 
	         AddedManually, 
	         ActivationDate,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         EmptyReturnClosingDays, 
	         ShipmentATAClosingDays, 
	         ShipmentATADateIndicator, 
	         IsExport, 
	         IsDomestic, 
	         IsImport, 
	         IsDrop, 
	         AddedManually, 
	         ActivationDate,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ContainerSettingPM entityPM, ContainerSetting entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EmptyReturnClosingDays))
            {
				entityPOCO.EmptyReturnClosingDays = entityPM.EmptyReturnClosingDays;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentATAClosingDays))
            {
				entityPOCO.ShipmentATAClosingDays = entityPM.ShipmentATAClosingDays;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentATADateIndicator))
            {
				entityPOCO.ShipmentATADateIndicator = entityPM.ShipmentATADateIndicator;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExport))
            {
				entityPOCO.IsExport = entityPM.IsExport;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDomestic))
            {
				entityPOCO.IsDomestic = entityPM.IsDomestic;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsImport))
            {
				entityPOCO.IsImport = entityPM.IsImport;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDrop))
            {
				entityPOCO.IsDrop = entityPM.IsDrop;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddedManually))
            {
				entityPOCO.AddedManually = entityPM.AddedManually;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivationDate))
            {
				entityPOCO.ActivationDate = entityPM.ActivationDate;
			}
			}

		public void POCOToPM(ContainerSettingPM entityPM, ContainerSetting entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EmptyReturnClosingDays))
            {
					entityPM.EmptyReturnClosingDays = entityPOCO.EmptyReturnClosingDays;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentATAClosingDays))
            {
					entityPM.ShipmentATAClosingDays = entityPOCO.ShipmentATAClosingDays;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentATADateIndicator))
            {
					entityPM.ShipmentATADateIndicator = entityPOCO.ShipmentATADateIndicator;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsExport))
            {
					entityPM.IsExport = entityPOCO.IsExport;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDomestic))
            {
					entityPM.IsDomestic = entityPOCO.IsDomestic;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsImport))
            {
					entityPM.IsImport = entityPOCO.IsImport;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDrop))
            {
					entityPM.IsDrop = entityPOCO.IsDrop;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AddedManually))
            {
					entityPM.AddedManually = entityPOCO.AddedManually;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActivationDate))
            {
					entityPM.ActivationDate = entityPOCO.ActivationDate;
            }

		}

		public void PMToOldPM(ContainerSettingPM entityPM, ContainerSettingPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EmptyReturnClosingDays))
            {
                oldEntityPM.EmptyReturnClosingDays = entityPM.EmptyReturnClosingDays;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentATAClosingDays))
            {
                oldEntityPM.ShipmentATAClosingDays = entityPM.ShipmentATAClosingDays;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentATADateIndicator))
            {
                oldEntityPM.ShipmentATADateIndicator = entityPM.ShipmentATADateIndicator;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExport))
            {
                oldEntityPM.IsExport = entityPM.IsExport;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDomestic))
            {
                oldEntityPM.IsDomestic = entityPM.IsDomestic;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsImport))
            {
                oldEntityPM.IsImport = entityPM.IsImport;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDrop))
            {
                oldEntityPM.IsDrop = entityPM.IsDrop;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddedManually))
            {
                oldEntityPM.AddedManually = entityPM.AddedManually;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivationDate))
            {
                oldEntityPM.ActivationDate = entityPM.ActivationDate;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ContainerSettingPM entityPM)
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
	 