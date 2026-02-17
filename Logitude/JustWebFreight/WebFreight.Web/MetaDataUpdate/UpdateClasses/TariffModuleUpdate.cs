using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate.UpdateClasses
{
    public class TariffModuleUpdate
    {
        private IWebFreightContext objectContext;

        #region Repositories
        private ScreensRepository screensRepository;
        private ScreenFieldsRepository screenFieldsRepository;
        #endregion

        public void loadScreens()
        {
            objectContext = WebFreightContext.GetContext(0);
            screenFieldsRepository = new ScreenFieldsRepository(objectContext);
            screensRepository = new ScreensRepository(objectContext);
            Dictionary<string, Screen> tenantScreens = screensRepository.GetScreensByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, ScreenField> tenantScreenField = screenFieldsRepository.GetScreenFieldsByTenant(0).ToDictionary(d => d.ScreenId + d.ObjectFieldId);

            BuildTMProjectScreens(tenantScreens, tenantScreenField);
        }

        public void BuildTMProjectScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        {
          
        }

    }

}