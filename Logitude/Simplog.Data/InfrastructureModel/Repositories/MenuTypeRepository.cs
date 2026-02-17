using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class MenuTypeRepository:IRepository<MenuType>
    {
        IWebFreightContext webFreightContext;
        public MenuTypeRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }

        public MenuTypeRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public MenuTypeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<MenuType> GetMenuTypes()
        {
            return context.MenusTypes;
        }

        public MenuType GetSingleMenuType(string code)
        {
            return (from a in context.MenusTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }
            

        public void Add(MenuType entity)
        {
            context.MenusTypes.Add(entity);
        }

        public void Remove(MenuType entity)
        {
            context.MenusTypes.Attach(entity);
            context.MenusTypes.Remove(entity);
        }

        public void Update(MenuType entity)
        {
            context.MenusTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MenuType> All()
        {
            return context.MenusTypes.ToList();
        }

        public IWebFreightContext context
        {
            get {return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<MenuType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public MenuType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}