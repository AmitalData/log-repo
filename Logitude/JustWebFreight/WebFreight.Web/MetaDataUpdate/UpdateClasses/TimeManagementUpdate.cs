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
    public class TimeManagementUpdate
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
            ObjectTable entityObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TMProject" && d.Tenant == 0).FirstOrDefault();
            List<ObjectField> entityObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).ToList();

            ObjectField OwnerField = objectContext.ObjectFields.Where(d => d.FieldName == "OwnerId" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField CustomerField = objectContext.ObjectFields.Where(d => d.FieldName == "CustomerId" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField NameField = objectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();
            ObjectField DescriptionField = objectContext.ObjectFields.Where(d => d.FieldName == "Description" && d.ObjectTable.Id == entityObjectTable.Id && d.Tenant == 0).FirstOrDefault();

            //General Screen
            Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TMProject.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = entityObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 4 }, screensRepository, tenantScreens);

            ScreenField OwnerIdScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = OwnerField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField CustomerIdScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = CustomerField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField NameScreenScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = NameField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
            ScreenField DescriptionScreenField = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = DescriptionField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);

            objectContext.SaveChanges();
        }

    }

}