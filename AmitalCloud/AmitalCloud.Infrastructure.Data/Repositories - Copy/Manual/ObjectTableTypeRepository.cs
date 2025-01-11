using System.Collections.Generic;
using System.Linq;
using AmitalCloud.Infrastructure.Data.Interfaces;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Context;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class ObjectTableTypeRepository:IRepository<ObjectTableType, string>
    {
        IAmitalCloudContext amitalCloudContext;
        public ObjectTableTypeRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;

        }
        public ObjectTableTypeRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
        }
        public ObjectTableTypeRepository()
        {
               amitalCloudContext = new AmitalCloudContext(); 
        }
        public void Add(ObjectTableType entity)
        {
            context.ObjectTableTypes.Add(entity);
        }

        public IQueryable<ObjectTableType> GetObjectTableTypes()
        {
            
            
            return context.ObjectTableTypes;
        }

        public ObjectTableType GetSingleObjectTableType(string code)
        {
            return (from a in context.ObjectTableTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Remove(ObjectTableType entity)
        {
            context.ObjectTableTypes.Attach(entity);
            context.ObjectTableTypes.Remove(entity);
        }

        public void Update(ObjectTableType entity)
        {
            context.ObjectTableTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ObjectTableType> All()
        {
            return context.ObjectTableTypes.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ObjectTableType> GetMulti(IEntityKeyFields<ObjectTableType,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ObjectTableType GetSingle(IEntityKeyFields<ObjectTableType,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}