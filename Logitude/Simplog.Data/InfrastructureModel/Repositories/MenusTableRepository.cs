using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class MenusTableRepository:IRepository<MenusTable>
    {
        IWebFreightContext webFreightContext;
        public MenusTableRepository( IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public MenusTableRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public MenusTableRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public MenusTable GetSingleMenusTable(string id)
        {
            return (from a in context.MenusTables
                    where a.Id == id
                    select a).FirstOrDefault();
        }

       

        public IQueryable<MenusTable> GetMenusTablesByTenant(int tenant)
        {
            IQueryable<MenusTable> menus = from a in context.MenusTables
                                           where a.Tenant == tenant
                                           select a;
            return menus.OrderBy(d => d.IndexOfOrder);
        }


      

        public int GetCount(int tenant,string menueTypeCode,string categoryType)
        {
            int count=0;
            if (!string.IsNullOrEmpty(categoryType))
            {
                 count = (from a in context.MenusTables
                             where a.MenuTypeCode == menueTypeCode && a.CategoryTypeCode == categoryType && a.Tenant == tenant
                             select a).Count();
            }
            else
            {
                 count = (from a in context.MenusTables
                             where a.MenuTypeCode == menueTypeCode && a.Tenant == tenant
                             select a).Count();
            }
            return count;
        }

       


        public void Add(MenusTable entity)
        {
            context.MenusTables.Add(entity);
        }

        public void Remove(MenusTable entity)
        {
            context.MenusTables.Attach(entity);
            context.MenusTables.Remove(entity);
        }

        public void Update(MenusTable entity)
        {
            try
            {
                context.MenusTables.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<MenusTable> All()
        {
            return context.MenusTables.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<MenusTable> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public MenusTable GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}