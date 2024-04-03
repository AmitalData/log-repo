using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.Helpers.WorkerRole.Importer
{
    class CustomPickListsImporterService : IDeploymentPackageImporterService
    {
        DeploymentPackageImporterContext importerContext;
        private IWebFreightContext webFreightContext;
        private int tenant;
        public CustomPickListsImporterService()
        {

        }
        public void Deploy(DeploymentPackageImporterContext context)
        {
            if (context.DeploymentPackageDetails == null) return;
            if (context.DeploymentPackageDetails.CustomPickLists == null || context.DeploymentPackageDetails.CustomPickLists.Count == 0) return;

            importerContext = context;
            tenant = context.Tenant;

            webFreightContext = WebFreightContext.GetContext(importerContext.Tenant);
            CustomPickListService customPickListService = new CustomPickListService(webFreightContext, tenant);


            foreach (CustomPickListItem customPickListItem in context.DeploymentPackageDetails.CustomPickLists)
            {
                CreateCustomPickList(customPickListService, customPickListItem);
            }

        }

        private void CreateCustomPickList(CustomPickListService customPickListService, CustomPickListItem customPickListItem)
        {
            CustomPickListPM customPickListPM = GetInstanceOfCustomPickList(customPickListItem);
            customPickListService.Create(customPickListPM);
        }

        private CustomPickListPM GetInstanceOfCustomPickList(CustomPickListItem customPickListItem)
        {
            return new CustomPickListPM()
            {
                Tenant = tenant,
                Code = customPickListItem.Code,
                IsMultipleChoice = customPickListItem.IsMultipleChoice,
                Value = customPickListItem.Value
            };
        }
    }
}
