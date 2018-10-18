using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ScreensRepository : IRepository<Screen>
    {
         IWebFreightContext webFreightContext;
        public ScreensRepository()
        {
            //Context = new WebFreightContext();
        }
        public ScreensRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public ScreensRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<Screen> GetScreens()
        {
            return this.context.Screens;
        }

        public IQueryable<Screen> GetScreensByTenant(int tenant)
        {
            IQueryable<Screen> screens = from a in context.Screens
                                         where a.Tenant == tenant
                                         select a;
            return screens;
        }

        public Screen GetSingleScreen(string id)
        {
            return (from a in context.Screens
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public ScreenModification GetScreenModificationByScreen(string screenid, int tenant)
        {
            return (from a in context.ScreenModifications
                    where a.ScreenId == screenid && a.Tenant == tenant
                    select a).FirstOrDefault();

        }

      

        public void Add(Screen entity)
        {
            webFreightContext.Screens.Add(entity);
        }

        public void Remove(Screen entity)
        {
            this.webFreightContext.Screens.Remove(entity);
        }

        public void Update(Screen entity)
        {
            try
            {
                //this.context.Screens.Detach(entity);
                this.context.Screens.Attach(entity);
                //this.context.SetAsModified(entity);
            }
            catch
            {
                //this.context.Screens.Attach(entity);
               
            } 
            this.context.SetAsModified(entity);
        }

        public List<Screen> All()
        {
            return this.context.Screens.ToList<Screen>();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<Screen> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Screen GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
