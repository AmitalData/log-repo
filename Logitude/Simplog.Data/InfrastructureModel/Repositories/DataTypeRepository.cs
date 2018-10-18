using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DataTypeRepository:IRepository<FieldDataType>
    {
        IWebFreightContext webFreightContext;
        public DataTypeRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public DataTypeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public DataTypeRepository()
        {
               webFreightContext=new WebFreightContext(); 
        }
        public void Add(FieldDataType entity)
        {
            context.FieldDataTypes.Add(entity);
        }

        public IQueryable<FieldDataType> GetDataTypes()
        {
            
            
            return context.FieldDataTypes;
        }

        public void Remove(FieldDataType entity)
        {
            context.FieldDataTypes.Attach(entity);
            context.FieldDataTypes.Remove(entity);
        }

        public void Update(FieldDataType entity)
        {
            context.FieldDataTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FieldDataType> All()
        {
            return context.FieldDataTypes.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<FieldDataType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public FieldDataType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}