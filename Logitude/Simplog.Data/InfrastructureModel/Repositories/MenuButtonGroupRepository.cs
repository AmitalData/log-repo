using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class MenuButtonGroupRepository:IRepository<MenuButtonGroup>
    {
        IWebFreightContext webFreightContext;
        public MenuButtonGroupRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public MenuButtonGroupRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public MenuButtonGroupRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

      

        public IQueryable<MenuButtonGroup> GetMenuButtonGroupsByTenant(int tenant)
        {
            return from a in context.MenuButtonGroups
                   where a.Tenant == tenant
                   select a;
        }

   

      

        public MenuButtonGroup GetSingleMenuButtonGroup(string id)
        {
            return (from a in context.MenuButtonGroups
                    where a.Id == id
                    select a).FirstOrDefault();
        }


        public void Add(MenuButtonGroup entity)
        {
            context.MenuButtonGroups.Add(entity);
        }

        public void Remove(MenuButtonGroup entity)
        {
            context.MenuButtonGroups.Attach(entity);
            context.MenuButtonGroups.Remove(entity);
        }

        public void Update(MenuButtonGroup entity)
        {
            context.MenuButtonGroups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MenuButtonGroup> All()
        {
            return context.MenuButtonGroups.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<MenuButtonGroup> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public MenuButtonGroup GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}