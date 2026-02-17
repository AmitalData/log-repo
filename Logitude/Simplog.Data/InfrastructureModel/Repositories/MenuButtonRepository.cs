using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class MenuButtonRepository:IRepository<MenuButton>
    {
        IWebFreightContext webFreightContext;
        public MenuButtonRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public MenuButtonRepository()
        {
            //Context = new WebFreightContext();
        }
        public MenuButtonRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public MenuButton GetSingleMenuButton(string id)
        {
            return (from a in context.MenuButtons
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<MenuButton> GetMenuButtonsByTenant(int tenant)
        {
            return from a in context.MenuButtons
                   where a.Tenant == tenant
                   select a;
        }

        

        public void Add(MenuButton entity)
        {
            context.MenuButtons.Add(entity);
        }

        public void Remove(MenuButton entity)
        {
            context.MenuButtons.Attach(entity);
            context.MenuButtons.Remove(entity);
        }

        public void Update(MenuButton entity)
        {
            context.MenuButtons.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MenuButton> All()
        {
            return context.MenuButtons.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<MenuButton> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public MenuButton GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}