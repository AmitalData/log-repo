using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class ObjectTableTabQuery
    {
        ObjectTableTabRepository repository;
        private int tenantZero = 0;
        public ObjectTableTabQuery()
        {
            repository = new ObjectTableTabRepository(); 
        }

        public ObjectTableTabQuery(int tenant)
        {
            repository = new ObjectTableTabRepository(tenant);
        }

        public ObjectTableTabQuery(ObjectTableTabRepository objectTableTabRepository )
        {
            repository = objectTableTabRepository;
        }

        public List<ObjectTableTabPM> GetObjectTableTabPMsByTenant(int tenant)
        {
            List<ObjectTableTabPM> tenantZeroTabs = GetTenantZeroTabs();
            if (tenant == tenantZero) return tenantZeroTabs;

            GetEntityChangesFromModification(tenant, tenantZeroTabs);
            var tenantTabs = GetTenantTabs(tenant);
            

            return tenantZeroTabs.Concat(tenantTabs).ToList();
        }

        private List<ObjectTableTabPM> GetTenantZeroTabs()
        {
            return (from a in repository.context.ObjectTableTabs.Include("TabNameTextCode").Include("ObjectTable")
                    where a.Tenant == 0
                    select new ObjectTableTabPM()
                    {
                        ControlPath = a.ControlPath,
                        Id = a.Id,
                        IndexOrder = a.IndexOrder,
                        ObjectTableId = a.ObjectTableId,
                        TabNameTextCodeDefaultText = a.TabNameTextCode.DefaultText,
                        TabNameTextCodeId = a.TabNameTextCodeId,
                        Tenant = a.Tenant,
                        ObjectTableName = a.ObjectTable.Name,
                        TabNameTextCodeCode = a.TabNameTextCodeCode,
                        Code = a.Code,
                        FeatureId = a.FeatureId,
                        HtmlComponentName = a.HtmlComponentName,
                        HtmlComponentUrl = a.HtmlComponentUrl,
                        Type = "Predefined",
                        FeatureUniqeCode = a.FeatureUniqeCode,
                        HideTabNameInScreen = a.HideTabNameInScreen,
                        IsLocked = a.IsLocked
                    }).ToList();
        }

        private void GetEntityChangesFromModification(int tenant, List<ObjectTableTabPM> tabs)
        {
         
            Dictionary<string, TabModification> tabsModsDictionary
                            = repository.context.TabsModifications
                            .Where(te => te.Tenant == tenant).Distinct()
                            .ToDictionary(dic => dic.TabCode, dic => dic);

            foreach (ObjectTableTabPM tab in tabs)
                MapTabFieldsFromModification(tabsModsDictionary, tab);
        }

        private static void MapTabFieldsFromModification(Dictionary<string, TabModification> tabsModsDictionary, ObjectTableTabPM tab)
        {
            if (!tabsModsDictionary.Keys.Contains(tab.Code))
                return;

            TabModification mod = tabsModsDictionary[tab.Code];

            if (mod == null)
                return;

            tab.Name = mod.Name;
            tab.IndexOrder = mod.IndexOrder;
        }

        public IQueryable<ObjectTableTabPM> GetObjectTableTabsByTenantAndObjectTable(string objectTableId, int tenant)
        {
            IQueryable<ObjectTableTabPM> tabs = from a in repository.context.ObjectTableTabs.Include("TabNameTextCode").Include("ObjectTable")
                                                where a.Tenant == tenant && a.ObjectTableId == objectTableId
                                                select new ObjectTableTabPM()
                                                {
                                                    ControlPath = a.ControlPath,
                                                    Id = a.Id,
                                                    IndexOrder = a.IndexOrder,
                                                    ObjectTableId = a.ObjectTableId,
                                                    TabNameTextCodeDefaultText = a.TabNameTextCode.DefaultText,
                                                    TabNameTextCodeId = a.TabNameTextCodeId,
                                                    Tenant = a.Tenant,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    TabNameTextCodeCode = a.TabNameTextCodeCode,
                                                    Code = a.Code,
                                                    FeatureId = a.FeatureId,
                                                    FeatureUniqeCode = a.FeatureUniqeCode,
                                                    Name = a.TabNameTextCode.DefaultText,
                                                    Type = a.Type,
                                                    OriginalTabCode = a.OriginalTabCode,
                                                    ScreenCode = a.ScreenCode,
                                                    HtmlComponentName = a.HtmlComponentName,
                                                    HtmlComponentUrl = a.HtmlComponentUrl,
                                                    HideTabNameInScreen = a.HideTabNameInScreen,
													IsLocked = a.IsLocked
												};
            return tabs;
        }

        public List<ObjectTableTabPM> GetTenantTabs(int tenant)
        {
            List<ObjectTableTabPM> tabs = (from tab in repository.context.ObjectTableTabs.Include("TabNameTextCode").Include("ObjectTable")
                                           join screen in repository.context.Screens on tab.ScreenCode equals screen.Code into screenJoin
                                           from screen in screenJoin.DefaultIfEmpty()
                                            where tab.Tenant == tenant
                                            select new ObjectTableTabPM()
                                            {
                                                ControlPath = tab.ControlPath,
                                                Id = tab.Id,
                                                IndexOrder = tab.IndexOrder,
                                                ObjectTableId = tab.ObjectTableId,
                                                TabNameTextCodeDefaultText = tab.TabNameTextCode.DefaultText,
                                                TabNameTextCodeId = tab.TabNameTextCodeId,
                                                Tenant = tab.Tenant,
                                                ObjectTableName = tab.ObjectTable.Name,
                                                TabNameTextCodeCode = tab.TabNameTextCodeCode,
                                                Code = tab.Code,
                                                FeatureId = tab.FeatureId,
                                                FeatureUniqeCode = tab.FeatureUniqeCode,
                                                Name = tab.TabNameTextCode.DefaultText,
                                                Type = tab.Type,
                                                OriginalTabCode = tab.OriginalTabCode,
                                                ScreenCode = tab.ScreenCode,
                                                HtmlComponentName = tab.HtmlComponentName,
                                                HtmlComponentUrl = tab.HtmlComponentUrl,
                                                ScreenName = screen == null ? null : screen.Name,
                                                HideTabNameInScreen = tab.HideTabNameInScreen,
												IsLocked = tab.IsLocked
											}).ToList();
            return tabs;
        }

    }
}