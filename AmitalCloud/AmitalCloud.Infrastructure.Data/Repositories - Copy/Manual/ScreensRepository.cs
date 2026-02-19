using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class ScreensRepository : IRepository<Screen, string>
    {
         IAmitalCloudContext amitalCloudContext;
        public ScreensRepository()
        {
            //Context = new AmitalCloudContext();
        }
        public ScreensRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;
        }

        public ScreensRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
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
            return (from a in context.Screens.Include("ObjectTable")
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public ScreenModification GetScreenModificationByScreen(string screenCode, int tenant)
        {
            return (from a in context.ScreenModifications
                    where a.ScreenCode == screenCode && a.Tenant == tenant
                    select a).FirstOrDefault();

        }

        public Screen GetByCode(string code, int tenant)
        {
            return (from a in context.Screens
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();

        }

        public void Add(Screen entity)
        {
            context.Screens.Add(entity);
        }

        public void Remove(Screen entity)
        {
            this.context.Screens.Remove(entity);
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

        public IAmitalCloudContext context
        {
            get { return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<Screen> GetMulti(IEntityKeyFields<Screen,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Screen GetSingle(IEntityKeyFields<Screen,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
