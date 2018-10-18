using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class TriggerTypeRepository : IRepository<TriggerType>
    {

        IWebFreightContext webFreightContext;
        public TriggerTypeRepository()
        {
            webFreightContext = new WebFreightContext();

        }
        public TriggerTypeRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public TriggerTypeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<TriggerType> GetTriggerTypes()
        {
            return webFreightContext.TriggerTypes.OrderBy(d => d.Name);
        }

        public TriggerType GetTriggerTypeByCode(string code)
        {
            return webFreightContext.TriggerTypes.Where(r => r.Code == code).FirstOrDefault();
        }


        public void Add(TriggerType entity)
        {
            webFreightContext.TriggerTypes.Add(entity);
        }

        public void Remove(TriggerType entity)
        {
            webFreightContext.TriggerTypes.Remove(entity);
        }

        public void Update(TriggerType entity)
        {
            webFreightContext.TriggerTypes.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<TriggerType> All()
        {
            return webFreightContext.TriggerTypes.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }


        public List<TriggerType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TriggerType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}