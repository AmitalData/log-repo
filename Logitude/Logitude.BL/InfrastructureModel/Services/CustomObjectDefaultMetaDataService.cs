using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Services
{
    public class CustomObjectDefaultMetaDataService
    {
        private ObjectTablePM objectTablePM;
        private IWebFreightContext objectContext;
        public CustomObjectDefaultMetaDataService(ObjectTablePM objectTablePM, IWebFreightContext objectContext)
        {
            this.objectTablePM = objectTablePM;
            this.objectContext = objectContext;
        }
        public void Run()
        {
            if (!objectTablePM.IsCustom || !string.IsNullOrEmpty(objectTablePM.ParentObjectTableId)) return;
            AddGeneralScreen();
            AddGeneralTab();
        }

        private void AddGeneralScreen()
        {
            ScreenPM newScreen = new ScreenPM()
            {
                NumberOfRows = 10,
                NumberOfColumns = 2,
                IsReadOnly = false,
                Name = "General Tab Screen",
                Code = objectTablePM.Name + ".GeneralTabScreen",
                ObjectTableId = objectTablePM.Id,
                ObjectTableName = objectTablePM.Name,
                Tenant = objectTablePM.Tenant,
                Type = "LIGHTENING"
            };

            new ScreenService(objectContext, objectTablePM.Tenant).Create(newScreen);
        }

        private void AddGeneralTab()
        {
            ObjectTableTabPM newObjectTableTab = new ObjectTableTabPM()
            {
                ObjectTableId = objectTablePM.Id,
                IndexOrder = 0,
                Name = "General",
                TabNameTextCodeDefaultText = "General",
                Tenant = objectTablePM.Tenant,
                TabNameTextCodeCode = objectTablePM.Name + ".TH.General",
                CreateDefaultTextCode = true,
                ScreenCode = objectTablePM.Name + ".GeneralTabScreen",
                ScreenName = "General Tab Screen",
                Type = "Custom",
            };

            new ObjectTableTabService(objectContext, objectTablePM.Tenant).Create(newObjectTableTab);
        }
    }
}
