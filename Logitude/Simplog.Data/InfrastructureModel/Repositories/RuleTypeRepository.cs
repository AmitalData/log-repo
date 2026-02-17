using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class RuleTypeRepository : IRepository<RuleType>
    {

        IWebFreightContext webFreightContext;
        public RuleTypeRepository()
        {
            webFreightContext = new WebFreightContext();

        }
        public RuleTypeRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public RuleTypeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<RuleType> GetRuleTypes()
        {
            return webFreightContext.RuleTypes.OrderBy(d => d.Name);
        }

        public RuleType GetRuleTypeByCode(string code)
        {
            return webFreightContext.RuleTypes.Where(r=>r.Code == code).FirstOrDefault();
        }


        public void Add(RuleType entity)
        {
            webFreightContext.RuleTypes.Add(entity);
        }

        public void Remove(RuleType entity)
        {
            webFreightContext.RuleTypes.Remove(entity);
        }

        public void Update(RuleType entity)
        {
            webFreightContext.RuleTypes.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<RuleType> All()
        {
            return webFreightContext.RuleTypes.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }


        public List<RuleType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public RuleType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}