using System.Collections.Generic;
using System.Linq;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class RuleTypeRepository : IRepository<RuleType, string>
    {

        IAmitalCloudContext amitalCloudContext;
        public RuleTypeRepository()
        {
            amitalCloudContext = new AmitalCloudContext();

        }
        public RuleTypeRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;

        }
        public RuleTypeRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
        }
        public IQueryable<RuleType> GetRuleTypes()
        {
            return amitalCloudContext.RuleTypes.OrderBy(d => d.Name);
        }

        public RuleType GetRuleTypeByCode(string code)
        {
            return amitalCloudContext.RuleTypes.Where(r=>r.Code == code).FirstOrDefault();
        }


        public void Add(RuleType entity)
        {
            amitalCloudContext.RuleTypes.Add(entity);
        }

        public void Remove(RuleType entity)
        {
            amitalCloudContext.RuleTypes.Remove(entity);
        }

        public void Update(RuleType entity)
        {
            amitalCloudContext.RuleTypes.Attach(entity);
            amitalCloudContext.SetAsModified(entity);
        }

        public List<RuleType> All()
        {
            return amitalCloudContext.RuleTypes.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            amitalCloudContext.SaveChanges();
        }


        public List<RuleType> GetMulti(IEntityKeyFields<RuleType,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public RuleType GetSingle(IEntityKeyFields<RuleType,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}