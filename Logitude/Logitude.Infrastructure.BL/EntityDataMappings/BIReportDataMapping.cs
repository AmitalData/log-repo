
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data.ExtendedServices;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class BIReportDataMapping: IMapping<BIReportPM, BIReport>
   {

        public void CustomPMToPOCO(BIReportPM entityPM, BIReport entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                Logitude.BL.CommonDataModel.EntityQueries.UserQuery userQuery = new Logitude.BL.CommonDataModel.EntityQueries.UserQuery(entityPM.Tenant);
                string systemUserId = userQuery.GetSystemUserIdIfItIsCustomerCare(entityPM.Tenant);
                if (!string.IsNullOrEmpty(systemUserId)) entityPM.UpdatedByUserId = systemUserId;
            }

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO);

            if (entityPM.BIReportFolderId != entityPOCO.BIReportFolderId)
            {
                CheckIfUserHasPermissionOnBIFolder(entityPM.BIReportFolderId, entityPOCO.Tenant);
            }
        }

        private void CheckIfUserHasPermissionOnBIFolder(string biReportFolderId, int tenant)
        {
            BIReportFolderQueryService bIReportFolderQuery = new BIReportFolderQueryService(tenant);
            bool isPermittedUser = bIReportFolderQuery.CheckIfUserHasFolderPermission(tenant, biReportFolderId);
            if (!isPermittedUser) throw new Exception("Sorry! you have no permission to do this operation.");
        }

        public void CustomPOCOToPM(BIReportPM entityPM, BIReport entityPOCO)
        {
            MapAvailableForScheduling(entityPM);
        }

        private void MapAvailableForScheduling(BIReportPM entityPM)
        {
            AvailableForSchedulingBIReportService availableForSchedulingBIReportService = new AvailableForSchedulingBIReportService();
            entityPM.AvailableForScheduling = availableForSchedulingBIReportService.IsAvailableForScheduling(entityPM.FactTableName, entityPM.Tenant);
        }

        private void BuildSearchFields(BIReportPM entityPM, BIReport entityPOCO)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }


}
   