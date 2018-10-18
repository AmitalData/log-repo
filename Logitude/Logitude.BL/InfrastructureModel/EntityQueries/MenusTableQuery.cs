using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class MenusTableQuery
    {
        MenusTableRepository repository;
        public MenusTableQuery()
        {
            repository = new MenusTableRepository(); 
        }

        public MenusTableQuery(int tenant)
        {
            repository = new MenusTableRepository(tenant);
        }

        public MenusTableQuery(MenusTableRepository menusTableRepository)
        {
            repository = menusTableRepository;
        }

        public IQueryable<MenusTablePM> GetMenusTablePMsByTenant(int tenant)
        {
            IQueryable<MenusTablePM> menus = from a in repository.context.MenusTables.Include("ObjectTable").Include("Feature")
                                             where a.Tenant == tenant || a.Tenant == 0
                                             select new MenusTablePM()
                                             {
                                                 CategoryTypeCode = a.CategoryTypeCode,
                                                 Icon = a.Icon,
                                                 Id = a.Id,
                                                 IndexOfOrder = a.IndexOfOrder,
                                                 MenuTypeCode = a.MenuTypeCode,
                                                 ObjectTableId = a.ObjectTableId,
                                                 Tenant = a.Tenant,
                                                 TextCode = a.TextCode,
                                                 UserControlName = a.UserControlName,
                                                 ObjectTableName = a.ObjectTable.Name,
                                                 FeatureId = a.FeatureId,
                                                 FeatureCode = a.Feature.Code,
                                                 Code = a.Code,
                                                 HtmlView = a.HtmlView
                                             };

            return menus.OrderBy(d => d.IndexOfOrder);
        }

        public IQueryable<MenusTablePM> GetMainMenuList(int tenant)
        {
            IQueryable<MenusTablePM> mainMenu
                = from a in repository.context.MenusTables.Include("ObjectTable").Include("Feature")
                  where a.Tenant == tenant
                  && a.MenuTypeCode == "Main"
                  select new MenusTablePM()
                  {
                      CategoryTypeCode = a.CategoryTypeCode,
                      Icon = a.Icon,
                      Id = a.Id,
                      IndexOfOrder = a.IndexOfOrder,
                      MenuTypeCode = a.MenuTypeCode,
                      ObjectTableId = a.ObjectTableId,
                      Tenant = a.Tenant,
                      TextCode = a.TextCode,
                      UserControlName = a.UserControlName,
                      ObjectTableName = a.ObjectTable.Name,
                      FeatureCode = a.Feature.Code,
                      FeatureId = a.FeatureId,
                      Code = a.Code,
                      HtmlView = a.HtmlView
                  };

            return mainMenu.OrderBy(d => d.IndexOfOrder);
        }

        public IQueryable<MenusTablePM> GetMaintenanceMenusList(int tenant)
        {
            IQueryable<MenusTablePM> mainMenu
                = from a in repository.context.MenusTables.Include("ObjectTable").Include("Feature")
                  where a.Tenant == tenant
                  && a.MenuTypeCode == "MTC"
                  select new MenusTablePM()
                  {
                      CategoryTypeCode = a.CategoryTypeCode,
                      Icon = a.Icon,
                      Id = a.Id,
                      IndexOfOrder = a.IndexOfOrder,
                      MenuTypeCode = a.MenuTypeCode,
                      ObjectTableId = a.ObjectTableId,
                      Tenant = a.Tenant,
                      TextCode = a.TextCode,
                      UserControlName = a.UserControlName,
                      ObjectTableName = a.ObjectTable.Name,
                      FeatureId = a.FeatureId,
                      FeatureCode = a.Feature.Code,
                      Code = a.Code,
                      HtmlView = a.HtmlView
                  };
            return mainMenu.OrderBy(d => d.IndexOfOrder);
        }


    }
}