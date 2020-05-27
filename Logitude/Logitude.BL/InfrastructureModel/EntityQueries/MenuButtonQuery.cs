using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class MenuButtonQuery
    {
        MenuButtonRepository repository;
        public MenuButtonQuery()
        {
            repository = new MenuButtonRepository(); 
        }

        public MenuButtonQuery(int tenant)
        {
            repository = new MenuButtonRepository(tenant);
        }

        public MenuButtonQuery(MenuButtonRepository menuButtonRepository)
        {
            repository = menuButtonRepository;
        }

        public IQueryable<MenuButtonPM> GetMenuButtonPMsByTenant(int tenant)
        {
            return from a in repository.context.MenuButtons.Include("TextCode")
                   where a.Tenant == tenant
                   select new MenuButtonPM()
                   {
                       Id = a.Id,
                       EventCode = a.EventCode,
                       Index = a.Index,
                       IsActive = a.IsActive,
                       LabelTextCodeId = a.LabelTextCodeId,
                       MenuButtonGroupId = a.MenuButtonGroupId,
                       ParentMenuButtonId = a.ParentMenuButtonId,
                       Tenant = a.Tenant,
                       LabelTextCodeCode = a.LabelTextCodeCode,
                       FeatureId = a.FeatureId,
                       MenuButtonType = a.MenuButtonType,
                       DropDownControl = a.DropDownControl,
                       Style = a.Style,
                       ControlPath=a.ControlPath,
                       HtmlComponentPath =a.HtmlComponentPath,
                       Width =a.Width,
                       FeatureUniqeCode = a.FeatureUniqeCode,
                   };
        }

        public IQueryable<MenuButtonPM> GetMenuButtonsByMenuButtonGroup(string menuButtonGroupId, int tenant)
        {

            return from a in repository.context.MenuButtons.Include("TextCode")
                   where a.Tenant == tenant && a.MenuButtonGroupId == menuButtonGroupId
                   select new MenuButtonPM()
                   {
                       Id = a.Id,
                       EventCode = a.EventCode,
                       Index = a.Index,
                       IsActive = a.IsActive,
                       LabelTextCodeId = a.LabelTextCodeId,
                       MenuButtonGroupId = a.MenuButtonGroupId,
                       ParentMenuButtonId = a.ParentMenuButtonId,
                       Tenant = a.Tenant,
                       LabelTextCodeCode = a.LabelTextCodeCode,
                       FeatureId = a.FeatureId,
                       MenuButtonType = a.MenuButtonType,
                       DropDownControl = a.DropDownControl,
                       Style = a.Style,
                       ControlPath = a.ControlPath,
                       Width = a.Width,
                       HtmlComponentPath = a.HtmlComponentPath,
                       FeatureUniqeCode = a.FeatureUniqeCode,
                   };
        }
    }
}