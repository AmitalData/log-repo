using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class FieldDataTypesRepository:IRepository<FieldDataType>
    {

        IWebFreightContext webFreightContext;
        public FieldDataTypesRepository()
        {
            webFreightContext = new WebFreightContext();

        }
        public FieldDataTypesRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public FieldDataTypesRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<FieldDataType> GetFieldDataTypes()
        {
            return context.FieldDataTypes;
        }

        public FieldDataType GetSingleFieldDataType(string code)
        {
            return (from a in context.FieldDataTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(FieldDataType entity)
        {
            webFreightContext.FieldDataTypes.Add(entity);
        }

        public void Remove(FieldDataType entity)
        {
            webFreightContext.FieldDataTypes.Remove(entity);
        }

        public void Update(FieldDataType entity)
        {
            webFreightContext.FieldDataTypes.Attach(entity);
            webFreightContext.SetAsModified(entity);
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
            webFreightContext.SaveChanges();
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