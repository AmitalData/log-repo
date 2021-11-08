using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddEntityStatus
    {
        public static void AddEntityStatu(EntityStatusDetails entityStatusDetails, EntityStatusRepository entityStatusRepository, Dictionary<string, EntityStatus> tenantEntityStatus)
        {
            if (tenantEntityStatus.Keys.Contains(entityStatusDetails.Code))
            {
                EntityStatus entityStatus = tenantEntityStatus[entityStatusDetails.Code];
                entityStatus.Name = entityStatusDetails.Name;
                entityStatus.DisplayName = !string.IsNullOrEmpty(entityStatusDetails.DisplayName) ? entityStatusDetails.DisplayName : entityStatusDetails.Name;
                entityStatus.ObjectTableId = entityStatusDetails.ObjectTableId;
                entityStatus.StatusWeight = entityStatusDetails.StatusWeight;
                entityStatus.Tenant = entityStatusDetails.Tenant;
                entityStatus.InActive = entityStatusDetails.InActive;
                entityStatus.EntityStatusTypeCode = entityStatusDetails.EntityStatusTypeCode;
                entityStatus.SearchFields = entityStatusDetails.Code + "," + entityStatusDetails.Name;               
                entityStatusRepository.Update(entityStatus);

            }
            else 
            {
                EntityStatus newEntityStatus = new EntityStatus()
                {
                    Tenant = entityStatusDetails.Tenant,
                    StatusWeight = entityStatusDetails.StatusWeight,
                    ObjectTableId = entityStatusDetails.ObjectTableId,
                    Name = entityStatusDetails.Name,
                    DisplayName = !string.IsNullOrEmpty(entityStatusDetails.DisplayName)? entityStatusDetails.DisplayName: entityStatusDetails.Name,
                    Code = entityStatusDetails.Code,
                    InActive = entityStatusDetails.InActive,
                    EntityStatusTypeCode = entityStatusDetails.EntityStatusTypeCode,
                    Id = IdCounter.GetNumber("EntityStatus",entityStatusDetails.Tenant).ToString(),
                    SearchFields = entityStatusDetails.Code + "," + entityStatusDetails.Name,
                };

                entityStatusRepository.Add(newEntityStatus);
            }
        }
    }
}