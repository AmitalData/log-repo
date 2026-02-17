using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ObjectTableTypeRepository:IRepository<ObjectTableType>
    {
        IWebFreightContext webFreightContext;
        public ObjectTableTypeRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public ObjectTableTypeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public ObjectTableTypeRepository()
        {
               webFreightContext=new WebFreightContext(); 
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

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ObjectTableType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ObjectTableType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}