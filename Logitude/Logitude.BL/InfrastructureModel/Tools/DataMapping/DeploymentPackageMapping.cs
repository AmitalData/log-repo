using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class DeploymentPackageMapping
    {
        public static void MapEntity(DeploymentPackagePM entityPM, DeploymentPackage entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPM.DeploymentPackageDetails = new DeploymentPackageDetails();
            }
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.CreatedBy = entityPM.CreatedBy;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.UpdatedBy = entityPM.UpdatedBy;
            entityPOCO.SearchFields = entityPM.SearchFields;
            entityPOCO.Name = entityPM.Name;
            entityPOCO.Code = entityPM.Code;
            entityPOCO.InActive = entityPM.InActive;
            entityPOCO.Description = entityPM.Description;
            entityPOCO.DirectionId = entityPM.DirectionId;
            entityPOCO.VersionId = entityPM.VersionId;
            BuildSearchFields(entityPM, entityPOCO);
        }

        private static void BuildSearchFields(DeploymentPackagePM entityPM, DeploymentPackage entityPOCO)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }

    }
}
