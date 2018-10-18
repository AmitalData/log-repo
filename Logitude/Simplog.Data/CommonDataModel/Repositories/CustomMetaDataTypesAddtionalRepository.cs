using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomMetaDataTypesAddtionalRepository : IRepository<CustomMetaDataTypesAddtional>
    {
        ICommonDataContext commonDataContext;

        public CustomMetaDataTypesAddtionalRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomMetaDataTypesAddtionalRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomMetaDataTypesAddtionalRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CustomMetaDataTypesAddtional> GetCustomMetaDataTypesAddtionals()
        {
            return context.CustomMetaDataTypesAddtionals;
        }

        public CustomMetaDataTypesAddtional GetSingleCustomMetaDataTypesAddtional(string id, int tenant)
        {
            return (from record in context.CustomMetaDataTypesAddtionals where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }




        public void Add(CustomMetaDataTypesAddtional entity)
        {
            context.CustomMetaDataTypesAddtionals.Add(entity);
        }

        public void Remove(CustomMetaDataTypesAddtional entity)
        {
            context.CustomMetaDataTypesAddtionals.Attach(entity);
            context.CustomMetaDataTypesAddtionals.Remove(entity);
        }

        public void Update(CustomMetaDataTypesAddtional entity)
        {
            context.CustomMetaDataTypesAddtionals.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomMetaDataTypesAddtional> All()
        {
            return context.CustomMetaDataTypesAddtionals.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<CustomMetaDataTypesAddtional> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomMetaDataTypesAddtional GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
