
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
using Logitude.ShipmentOrderLib.Data.EntityPOCOs;
using Logitude.ShipmentOrderLib.BL.EntityPMs; 
using Logitude.ShipmentOrderLib.Data;

namespace Logitude.ShipmentOrderLib.BL.EntityDataMappings
{
   
   public partial class ShipmentOrderDataMapping: IMapping<ShipmentOrderPM, ShipmentOrder>,IMappingEncodeBase64NVARCHARFields<ShipmentOrderPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         OrderNumber, 
	         TransportModeId, 
	         ConsigneeId, 
	         ShipperId, 
	         AgentId, 
	         IncotermId, 
	         AccountManagerId, 
	         PONumber, 
	         DescriptionofGoods, 
	         ShipmentTypeId, 
	         House, 
	         VesselId, 
	         CustomsAgentId, 
	         SpecialServicesTypeId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         OrderNumber, 
	         TransportModeId, 
	         ConsigneeId, 
	         ShipperId, 
	         AgentId, 
	         IncotermId, 
	         AccountManagerId, 
	         PONumber, 
	         DescriptionofGoods, 
	         ShipmentTypeId, 
	         Master, 
	         House, 
	         VesselId, 
	         ETD, 
	         ETA, 
	         ATD, 
	         ATA, 
	         CustomsAgentId, 
	         SpecialServicesTypeId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OrderNumber))
            {
				entityPOCO.OrderNumber = entityPM.OrderNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
				entityPOCO.TransportModeId = entityPM.TransportModeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeId))
            {
				entityPOCO.ConsigneeId = entityPM.ConsigneeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperId))
            {
				entityPOCO.ShipperId = entityPM.ShipperId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentId))
            {
				entityPOCO.AgentId = entityPM.AgentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IncotermId))
            {
				entityPOCO.IncotermId = entityPM.IncotermId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountManagerId))
            {
				entityPOCO.AccountManagerId = entityPM.AccountManagerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PONumber))
            {
				entityPOCO.PONumber = entityPM.PONumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionofGoods))
            {
				entityPOCO.DescriptionofGoods = entityPM.DescriptionofGoods;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentTypeId))
            {
				entityPOCO.ShipmentTypeId = entityPM.ShipmentTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.House))
            {
				entityPOCO.House = entityPM.House;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VesselId))
            {
				entityPOCO.VesselId = entityPM.VesselId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsAgentId))
            {
				entityPOCO.CustomsAgentId = entityPM.CustomsAgentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecialServicesTypeId))
            {
				entityPOCO.SpecialServicesTypeId = entityPM.SpecialServicesTypeId;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OrderNumber))
            {
					entityPM.OrderNumber = entityPOCO.OrderNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransportModeId))
            {
					entityPM.TransportModeId = entityPOCO.TransportModeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeId))
            {
					entityPM.ConsigneeId = entityPOCO.ConsigneeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperId))
            {
					entityPM.ShipperId = entityPOCO.ShipperId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AgentId))
            {
					entityPM.AgentId = entityPOCO.AgentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IncotermId))
            {
					entityPM.IncotermId = entityPOCO.IncotermId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountManagerId))
            {
					entityPM.AccountManagerId = entityPOCO.AccountManagerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PONumber))
            {
					entityPM.PONumber = entityPOCO.PONumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DescriptionofGoods))
            {
					entityPM.DescriptionofGoods = entityPOCO.DescriptionofGoods;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentTypeId))
            {
					entityPM.ShipmentTypeId = entityPOCO.ShipmentTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.House))
            {
					entityPM.House = entityPOCO.House;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VesselId))
            {
					entityPM.VesselId = entityPOCO.VesselId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsAgentId))
            {
					entityPM.CustomsAgentId = entityPOCO.CustomsAgentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SpecialServicesTypeId))
            {
					entityPM.SpecialServicesTypeId = entityPOCO.SpecialServicesTypeId;
            }

		}

		public void PMToOldPM(ShipmentOrderPM entityPM, ShipmentOrderPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OrderNumber))
            {
                oldEntityPM.OrderNumber = entityPM.OrderNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
                oldEntityPM.TransportModeId = entityPM.TransportModeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeId))
            {
                oldEntityPM.ConsigneeId = entityPM.ConsigneeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperId))
            {
                oldEntityPM.ShipperId = entityPM.ShipperId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentId))
            {
                oldEntityPM.AgentId = entityPM.AgentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IncotermId))
            {
                oldEntityPM.IncotermId = entityPM.IncotermId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountManagerId))
            {
                oldEntityPM.AccountManagerId = entityPM.AccountManagerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PONumber))
            {
                oldEntityPM.PONumber = entityPM.PONumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionofGoods))
            {
                oldEntityPM.DescriptionofGoods = entityPM.DescriptionofGoods;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentTypeId))
            {
                oldEntityPM.ShipmentTypeId = entityPM.ShipmentTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.House))
            {
                oldEntityPM.House = entityPM.House;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VesselId))
            {
                oldEntityPM.VesselId = entityPM.VesselId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsAgentId))
            {
                oldEntityPM.CustomsAgentId = entityPM.CustomsAgentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecialServicesTypeId))
            {
                oldEntityPM.SpecialServicesTypeId = entityPM.SpecialServicesTypeId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ShipmentOrderPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.DescriptionofGoods)) //T4 find type == nText 
            {
                entityPM.DescriptionofGoods = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DescriptionofGoods));
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
		
		private void BuildSearchFieldsGenerated(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 