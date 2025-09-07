using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class BlobFileRepository:IRepository<BlobFile>
    {
        ICommonDataContext commonDataContext;



        public BlobFileRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public BlobFileRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }


        public BlobFile GetSingleBlobFile(string id)
        {
            return (from record in context.BlobFiles where record.Id == id  select record).FirstOrDefault(); 
        }
        
        public void Add(BlobFile entity)
        {
            context.BlobFiles.Add(entity);
        }

        public void Remove(BlobFile entity)
        {
            context.BlobFiles.Attach(entity);
            context.BlobFiles.Remove(entity);
        }

        public void Update(BlobFile entity)
        {
            context.BlobFiles.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BlobFile> All()
        {
            return context.BlobFiles.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<BlobFile> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public BlobFile GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
