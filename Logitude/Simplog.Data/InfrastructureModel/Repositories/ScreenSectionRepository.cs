using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ScreenSectionRepository : IRepository<ScreenSection>
    {
        IWebFreightContext webFreightContext;
        public ScreenSectionRepository()
        {
           webFreightContext = new WebFreightContext();
        }
        public ScreenSectionRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public ScreenSectionRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<ScreenSection> GetScreenSectionSection()
        {
            return this.context.ScreenSections;
        }

        public IQueryable<ScreenSection> GetScreenSectionsByScreenCode(string screenCode,int tenant)
        {
            IQueryable<ScreenSection> ScreenSection = from a in context.ScreenSections
                                               where a.Tenant == tenant && a.ScreenCode == screenCode
                                                      select a;
            return ScreenSection;
        }

        public ScreenSection GetSingleScreenSection(string screenCode, int sectionNumber)
        {
            return (from a in context.ScreenSections
                    where a.Number == sectionNumber && a.ScreenCode == screenCode
                    select a).FirstOrDefault();
        }





        public void Add(ScreenSection entity)
        {
            webFreightContext.ScreenSections.Add(entity);
        }

        public void Remove(ScreenSection entity)
        {
            this.webFreightContext.ScreenSections.Remove(entity);
        }


        public void Update(ScreenSection entity)
        {
            webFreightContext.ScreenSections.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }



        public List<ScreenSection> All()
        {
            return this.context.ScreenSections.ToList<ScreenSection>();
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

        List<ScreenSection> IRepository<ScreenSection>.GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        ScreenSection IRepository<ScreenSection>.GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
