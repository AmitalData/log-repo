using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Transactions;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class MenuButtonGroupQuery
    {
        MenuButtonGroupRepository repository;
        public MenuButtonGroupQuery()
        {
            repository = new MenuButtonGroupRepository(); 
        }

        public MenuButtonGroupQuery(int tenant)
        {
            repository = new MenuButtonGroupRepository(tenant);
        }

        public MenuButtonGroupQuery(MenuButtonGroupRepository menuButtonGroupRepository)
        {
            repository = menuButtonGroupRepository;
        }

        public List<MenuButtonGroupPM> GetMenuButtonGroupsByObjectTable(string objectTableId, int tenant)
        {
            List<MenuButtonGroupPM> menubuttongruops;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                MenuButtonRepository menuButtonRep = new MenuButtonRepository(0);
                MenuButtonQuery menuButtonQuery = new MenuButtonQuery(menuButtonRep);
                WebFreightContext webFreightContext = (WebFreightContext)WebFreightContext.GetContext(0);
                menubuttongruops = (from a in webFreightContext.MenuButtonGroups//repository.context.MenuButtonGroups
                                    where a.Tenant == 0&&a.ObjectTableId==objectTableId
                                    select new MenuButtonGroupPM()
                                    {
                                        Id = a.Id,
                                        MenuButtonGroupType = a.MenuButtonGroupType,
                                        Name = a.Name,
                                        ObjectTableId = a.ObjectTableId,
                                        Tenant = a.Tenant,
                                    }).ToList();

                foreach (MenuButtonGroupPM gruop in menubuttongruops)
                {
                    gruop.MenuButtons = menuButtonQuery.GetMenuButtonsByMenuButtonGroup(gruop.Id, gruop.Tenant).ToList();
                }
            }


            return menubuttongruops;
        }

        public List<MenuButtonGroupPM> GetMenuButtonGroupPMsByTenant(int tenant)
        {
            List<MenuButtonGroupPM> menubuttongruops;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                MenuButtonRepository menuButtonRep = new MenuButtonRepository(0);
                MenuButtonQuery menuButtonQuery = new MenuButtonQuery(menuButtonRep);
                WebFreightContext webFreightContext = (WebFreightContext)WebFreightContext.GetContext(0);
                menubuttongruops = (from a in webFreightContext.MenuButtonGroups//repository.context.MenuButtonGroups
                                    where a.Tenant == 0//a.Tenant == tenant
                                    select new MenuButtonGroupPM()
                                    {
                                        Id = a.Id,
                                        MenuButtonGroupType = a.MenuButtonGroupType,
                                        Name = a.Name,
                                        ObjectTableId = a.ObjectTableId,
                                        Tenant = a.Tenant,
                                    }).ToList();

                foreach (MenuButtonGroupPM gruop in menubuttongruops)
                {
                    gruop.MenuButtons = menuButtonQuery.GetMenuButtonsByMenuButtonGroup(gruop.Id, gruop.Tenant).ToList();
                }
            }


            return menubuttongruops;
        }


        public MenuButtonGroupPM GetSingleMenuButtonGroupPM(string id, int tenant)
        {
            MenuButtonRepository menuButtonRep = new MenuButtonRepository(tenant);
            MenuButtonQuery menuButtonQuery = new MenuButtonQuery(menuButtonRep);
            MenuButtonGroupPM menuButtonGroup = (from a in repository.context.MenuButtonGroups
                                                 where a.Id == id
                                                 select new MenuButtonGroupPM()
                                                 {
                                                     Id = a.Id,
                                                     MenuButtonGroupType = a.MenuButtonGroupType,
                                                     Name = a.Name,
                                                     ObjectTableId = a.ObjectTableId,
                                                     Tenant = a.Tenant,
                                                 }).FirstOrDefault();

            menuButtonGroup.MenuButtons = menuButtonQuery.GetMenuButtonsByMenuButtonGroup(menuButtonGroup.Id, menuButtonGroup.Tenant).ToList();

            return menuButtonGroup;
        }

        public MenuButtonGroupPM GetSingleMenuButtonGroupPMByObjectTable(string objectTableId, int tenant)
        {
            MenuButtonRepository menuButtonRep = new MenuButtonRepository(tenant);
            MenuButtonQuery menuButtonQuery = new MenuButtonQuery(menuButtonRep);
            MenuButtonGroupPM menuButtonGroup = (from a in repository.context.MenuButtonGroups
                                                 where a.ObjectTableId == objectTableId && a.Tenant == tenant
                                                 select new MenuButtonGroupPM()
                                                 {
                                                     Id = a.Id,
                                                     MenuButtonGroupType = a.MenuButtonGroupType,
                                                     Name = a.Name,
                                                     ObjectTableId = a.ObjectTableId,
                                                     Tenant = a.Tenant,
                                                 }).FirstOrDefault();

            menuButtonGroup.MenuButtons = menuButtonQuery.GetMenuButtonsByMenuButtonGroup(menuButtonGroup.Id, menuButtonGroup.Tenant).ToList();

            return menuButtonGroup;
        }


    }
}