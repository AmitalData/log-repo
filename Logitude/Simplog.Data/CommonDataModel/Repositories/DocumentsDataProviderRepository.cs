using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentsDataProviderRepository : IRepository<DocumentsDataProvider>
    {
        ICommonDataContext commonDataContext;

        public DocumentsDataProviderRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DocumentsDataProviderRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DocumentsDataProviderRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<DocumentsDataProvider> GetDocumentsDataProviders()
        {
            return context.DocumentsDataProviders;
        }

        public DocumentsDataProvider GetSingleDocumentsDataProvider(string code)
        {
            return (from record in context.DocumentsDataProviders where record.Code == code select record).FirstOrDefault();
        }

        public string GetSingleHasDocumentsDataProvider(string code)
        {
            return (from record in context.DocumentsDataProviders where record.Code == code select record.Code).FirstOrDefault();
        }


        public void Add(DocumentsDataProvider entity)
        {
            context.DocumentsDataProviders.Add(entity);
        }

        public void Remove(DocumentsDataProvider entity)
        {
            context.DocumentsDataProviders.Attach(entity);
            context.DocumentsDataProviders.Remove(entity);
        }

        public void Update(DocumentsDataProvider entity)
        {
            context.DocumentsDataProviders.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DocumentsDataProvider> All()
        {
            return context.DocumentsDataProviders.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DocumentsDataProvider> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentsDataProvider GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
