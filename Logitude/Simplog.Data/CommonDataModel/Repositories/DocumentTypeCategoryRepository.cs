using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentTypeCategoryRepository : IRepository<DocumentTypeCategory>
    {
         ICommonDataContext commonDataContext;

        public DocumentTypeCategoryRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DocumentTypeCategoryRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DocumentTypeCategoryRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<DocumentTypeCategory> GetDocumentTypeCategories()
        {
            return context.DocumentTypeCategories;
        }
        public IQueryable<DocumentTypeCategory> GetAll()
        {
            return context.DocumentTypeCategories;
        }

        public DocumentTypeCategory GetSingleDocumentTypeCategory(string code)
        {
            return (from a in context.DocumentTypeCategories where a.Code == code select a).FirstOrDefault();
        }

        public void Add(DocumentTypeCategory entity)
        {
            context.DocumentTypeCategories.Add(entity);
        }

        public void Remove(DocumentTypeCategory entity)
        {
            context.DocumentTypeCategories.Attach(entity);
            context.DocumentTypeCategories.Remove(entity);
        }

        public void Update(DocumentTypeCategory entity)
        {
            context.DocumentTypeCategories.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DocumentTypeCategory> All()
        {
            return context.DocumentTypeCategories.ToList();

        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DocumentTypeCategory> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentTypeCategory GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
