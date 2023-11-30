using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class TabModificationRepository : IRepository<TabModification>
    {
         IWebFreightContext webFreightContext;
        public TabModificationRepository()
        {
        }
        public TabModificationRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public TabModificationRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<TabModification> GetTabModifications()
        {
            return this.context.TabsModifications;
        }

        public IQueryable<TabModification> GetTabModificationsByTenant(int tenant)
        {
            IQueryable<TabModification> tabModifications = from a in context.TabsModifications
                                                           where a.Tenant == tenant
                                         select a;
            return tabModifications;
        }

        public TabModification GetSingleTabModification(string id)
        {
            return (from a in context.TabsModifications
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public TabModification GetByTabCode(string tabCode, int tenant)
        {
            return (from a in context.TabsModifications
                    where a.TabCode == tabCode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(TabModification entity)
        {
            webFreightContext.TabsModifications.Add(entity);
        }

        public void Remove(TabModification entity)
        {
            this.webFreightContext.TabsModifications.Remove(entity);
        }

        public void Update(TabModification entity)
        {
            try
            {
                this.context.TabsModifications.Attach(entity);
            }
            catch
            {
            } 
            this.context.SetAsModified(entity);
        }

        public List<TabModification> All()
        {
            return this.context.TabsModifications.ToList<TabModification>();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<TabModification> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TabModification GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
