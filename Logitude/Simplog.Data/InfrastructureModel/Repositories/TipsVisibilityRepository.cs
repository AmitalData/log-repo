using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class TipsVisibilityRepository : IRepository<TipsVisibility>
    {
        IWebFreightContext webFreightContext;
        public TipsVisibilityRepository()
        {
            webFreightContext = new WebFreightContext();
        }


        public TipsVisibilityRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public TipsVisibilityRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

      
        public TipsVisibility GetSingleTipsVisibility(string id, int tenant)
        {
            TipsVisibility tipsVisibility = (from a in this.context.TipsVisibilities
                                               where a.Tenant == tenant && a.Id == id
                                               select a
                                                ).FirstOrDefault();

            return tipsVisibility;
        }



      
        public void Add(TipsVisibility entity)
        {
            this.context.TipsVisibilities.Add(entity);
        }

        public void Remove(TipsVisibility entity)
        {
            this.context.TipsVisibilities.Attach(entity);
            this.context.TipsVisibilities.Remove(entity);
        }

        public void Update(TipsVisibility entity)
        {
            this.context.TipsVisibilities.Attach(entity);
            this.context.SetAsModified(entity);
        }

        public List<TipsVisibility> All()
        {
            return this.context.TipsVisibilities.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<TipsVisibility> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TipsVisibility GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}