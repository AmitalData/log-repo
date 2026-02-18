using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class BlobFileRepository:IRepository<BlobFile,string>
    {
        IAmitalCloudContext currentContext;

        public BlobFileRepository()
        {
            currentContext = new AmitalCloudContext();
        }

        public BlobFileRepository(IAmitalCloudContext context)
        {
            currentContext = context;
        }

        public BlobFileRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
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

        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<BlobFile> GetMulti(IEntityKeyFields<BlobFile,string> entityKeys)
        {
            throw new NotImplementedException();
        }

        public BlobFile GetSingle(IEntityKeyFields<BlobFile,string> entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
