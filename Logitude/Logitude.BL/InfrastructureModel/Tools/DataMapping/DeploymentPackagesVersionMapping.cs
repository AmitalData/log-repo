using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class DeploymentPackagesVersionMapping
    {
        public static void MapEntity(DeploymentPackagesVersionPM entityPM, DeploymentPackagesVersion entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.DeploymentPackageID = entityPM.DeploymentPackageID;
            entityPOCO.IsExported = entityPM.IsExported;
            entityPOCO.DocumentId = entityPM.DocumentId;
            entityPOCO.VersionName = entityPM.VersionName;
            entityPOCO.VersionNumber = entityPM.VersionNumber;
        }
    }
}
