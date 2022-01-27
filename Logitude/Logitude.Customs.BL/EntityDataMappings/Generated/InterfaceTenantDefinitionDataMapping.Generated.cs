
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
   
   public partial class InterfaceTenantDefinitionDataMapping: IMapping<InterfaceTenantDefinitionPM, InterfaceTenantDefinition>,IMappingEncodeBase64NVARCHARFields<InterfaceTenantDefinitionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Code, 
	         TenantSendOptionsCode, 
	         TenantPriority, 
	         Active, 
	         DcaRenameFileEnable, 
	         DcaRenameFilePrefix, 
	         QueueType, 
	         UseRabbitMQ, 
	         QueueGroupCode, 
	         SendTime,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Code, 
	         TenantSendOptionsCode, 
	         TenantPriority, 
	         Active, 
	         DcaRenameFileEnable, 
	         DcaRenameFilePrefix, 
	         QueueType, 
	         UseRabbitMQ, 
	         QueueGroupCode, 
	         SendTime,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(InterfaceTenantDefinitionPM entityPM, InterfaceTenantDefinition entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Code))
            {
				entityPOCO.Code = entityPM.Code;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TenantSendOptionsCode))
            {
				entityPOCO.TenantSendOptionsCode = entityPM.TenantSendOptionsCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TenantPriority))
            {
				entityPOCO.TenantPriority = entityPM.TenantPriority;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Active))
            {
				entityPOCO.Active = entityPM.Active;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DcaRenameFileEnable))
            {
				entityPOCO.DcaRenameFileEnable = entityPM.DcaRenameFileEnable;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DcaRenameFilePrefix))
            {
				entityPOCO.DcaRenameFilePrefix = entityPM.DcaRenameFilePrefix;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QueueType))
            {
				entityPOCO.QueueType = entityPM.QueueType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UseRabbitMQ))
            {
				entityPOCO.UseRabbitMQ = entityPM.UseRabbitMQ;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QueueGroupCode))
            {
				entityPOCO.QueueGroupCode = entityPM.QueueGroupCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SendTime))
            {
				entityPOCO.SendTime = entityPM.SendTime;
			}
			}

		public void POCOToPM(InterfaceTenantDefinitionPM entityPM, InterfaceTenantDefinition entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Code))
            {
					entityPM.Code = entityPOCO.Code;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TenantSendOptionsCode))
            {
					entityPM.TenantSendOptionsCode = entityPOCO.TenantSendOptionsCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TenantPriority))
            {
					entityPM.TenantPriority = entityPOCO.TenantPriority;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Active))
            {
					entityPM.Active = entityPOCO.Active;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DcaRenameFileEnable))
            {
					entityPM.DcaRenameFileEnable = entityPOCO.DcaRenameFileEnable;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DcaRenameFilePrefix))
            {
					entityPM.DcaRenameFilePrefix = entityPOCO.DcaRenameFilePrefix;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QueueType))
            {
					entityPM.QueueType = entityPOCO.QueueType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UseRabbitMQ))
            {
					entityPM.UseRabbitMQ = entityPOCO.UseRabbitMQ;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QueueGroupCode))
            {
					entityPM.QueueGroupCode = entityPOCO.QueueGroupCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SendTime))
            {
					entityPM.SendTime = entityPOCO.SendTime;
            }

		}

		public void PMToOldPM(InterfaceTenantDefinitionPM entityPM, InterfaceTenantDefinitionPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Code))
            {
                oldEntityPM.Code = entityPM.Code;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TenantSendOptionsCode))
            {
                oldEntityPM.TenantSendOptionsCode = entityPM.TenantSendOptionsCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TenantPriority))
            {
                oldEntityPM.TenantPriority = entityPM.TenantPriority;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Active))
            {
                oldEntityPM.Active = entityPM.Active;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DcaRenameFileEnable))
            {
                oldEntityPM.DcaRenameFileEnable = entityPM.DcaRenameFileEnable;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DcaRenameFilePrefix))
            {
                oldEntityPM.DcaRenameFilePrefix = entityPM.DcaRenameFilePrefix;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QueueType))
            {
                oldEntityPM.QueueType = entityPM.QueueType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UseRabbitMQ))
            {
                oldEntityPM.UseRabbitMQ = entityPM.UseRabbitMQ;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QueueGroupCode))
            {
                oldEntityPM.QueueGroupCode = entityPM.QueueGroupCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SendTime))
            {
                oldEntityPM.SendTime = entityPM.SendTime;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(InterfaceTenantDefinitionPM entityPM)
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
	 