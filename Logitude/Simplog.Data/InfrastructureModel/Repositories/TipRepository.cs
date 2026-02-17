using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class TipRepository : IRepository<Tip>
    {
        IWebFreightContext webFreightContext;
        public TipRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public TipRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public TipRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
      


        public List<Tip> GetTips(int tenant)
        {
            List<Tip> tips = (from t in context.Tips
                              where t.Tenant == tenant
                              select t).ToList();

            return tips;
        }

     

        public Tip GetSingleTip(string code, int tenant)
        {
            Tip tip = (from a in context.Tips
                         where a.Code == code && a.Tenant == tenant
                        select a
                           ).FirstOrDefault();

            return tip;
        }


        #region IRepository<Tip> Members

        public void Add(Tip entity)
        {
            this.context.Tips.Add(entity);
        }

        public void Remove(Tip entity)
        {
            context.Tips.Attach(entity);
            this.context.Tips.Remove(entity);
        }

        public void Update(Tip entity)
        {
            context.Tips.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Tip> All()
        {
            return context.Tips.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

       
        
        #endregion


        public List<Tip> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Tip GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}