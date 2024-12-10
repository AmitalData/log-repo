using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DocumentFolderRepository:IRepository<DocumentFolder>
    {
        ICommonDataContext commonDataContext;

        public DocumentFolderRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DocumentFolderRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DocumentFolderRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<DocumentFolder> GetDocumentFolders(int tenant)
        {
            return context.DocumentFolders.Where(a=>a.Tenant == tenant);
        }

        public DocumentFolder GetSingleDocumentFolder(string id,int tenant)
        {
            return (from record in context.DocumentFolders where record.Id == id select record).FirstOrDefault();
        }

        public DocumentFolder GetSingleDocumentFolder(string id)
        {
            return (from record in context.DocumentFolders where record.Id == id select record).FirstOrDefault();
        }

        public DocumentFolder GetSingleDocumentFolderByCode(string code)
        {
            return (from record in context.DocumentFolders where record.Code == code select record).FirstOrDefault();
        }

        public void Add(DocumentFolder entity)
        {
            context.DocumentFolders.Add(entity);
        }

        public void Remove(DocumentFolder entity)
        {
            context.DocumentFolders.Attach(entity);
            context.DocumentFolders.Remove(entity);
        }

        public void Update(DocumentFolder entity)
        {
            context.DocumentFolders.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DocumentFolder> All()
        {
            return context.DocumentFolders.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DocumentFolder> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DocumentFolder GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
