using System.Collections.Generic;
using System.Linq;
using AmitalCloud.Infrastructure.Data.Interfaces;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Context;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class TriggerTypeRepository : IRepository<TriggerType, string>
    {

        IAmitalCloudContext amitalCloudContext;
        public TriggerTypeRepository()
        {
            amitalCloudContext = new AmitalCloudContext();

        }
        public TriggerTypeRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;

        }
        public TriggerTypeRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
        }
        public IQueryable<TriggerType> GetTriggerTypes()
        {
            return amitalCloudContext.TriggerTypes.OrderBy(d => d.Name);
        }

        public TriggerType GetTriggerTypeByCode(string code)
        {
            return amitalCloudContext.TriggerTypes.Where(r => r.Code == code).FirstOrDefault();
        }


        public void Add(TriggerType entity)
        {
            amitalCloudContext.TriggerTypes.Add(entity);
        }

        public void Remove(TriggerType entity)
        {
            amitalCloudContext.TriggerTypes.Remove(entity);
        }

        public void Update(TriggerType entity)
        {
            amitalCloudContext.TriggerTypes.Attach(entity);
            amitalCloudContext.SetAsModified(entity);
        }

        public List<TriggerType> All()
        {
            return amitalCloudContext.TriggerTypes.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            amitalCloudContext.SaveChanges();
        }


        public List<TriggerType> GetMulti(IEntityKeyFields<TriggerType,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TriggerType GetSingle(IEntityKeyFields<TriggerType,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}