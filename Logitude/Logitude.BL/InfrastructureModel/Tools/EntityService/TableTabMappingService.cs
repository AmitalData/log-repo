using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class TableTabMappingService
    {
        private ObjectTableTabRepository repository;
        public TableTabMappingService(ObjectTableTabRepository repository)
        {
            this.repository = repository;
        }
        public ObjectTableTab MapPoco(ObjectTableTabPM tabPM)
        {
            var tab = repository.GetSingleObjectTableTab(tabPM.Id);
            if (tab == null)
                tab = new ObjectTableTab();

            tab.Id = tabPM.Id;
            tab.Tenant = tabPM.Tenant;
            tab.ObjectTableId = tabPM.ObjectTableId;
            tab.ControlPath = tabPM.ControlPath;
            tab.TabNameTextCodeId = tabPM.TabNameTextCodeId;
            tab.Code = tabPM.Code;
            tab.FeatureId = tabPM.FeatureId;
            tab.TabNameTextCodeCode = tabPM.TabNameTextCodeCode;
            tab.FeatureUniqeCode = tabPM.FeatureUniqeCode;
            tab.HtmlComponentName = tabPM.HtmlComponentName;
            tab.HtmlComponentUrl = tabPM.HtmlComponentUrl;
            tab.ScreenCode = tabPM.ScreenCode;
            tab.Type = tabPM.Type;
            tab.OriginalTabCode = tabPM.OriginalTabCode;
            tab.IndexOrder = tabPM.IndexOrder;
            tab.IsLocked = tabPM.IsLocked;
            return tab;
        }
    }
}