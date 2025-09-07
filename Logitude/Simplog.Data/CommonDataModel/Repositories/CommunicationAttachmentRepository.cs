using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CommunicationAttachmentRepository:IRepository<CommunicationAttachment>
    {
        ICommonDataContext commonDataContext;



        public CommunicationAttachmentRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CommunicationAttachmentRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CommunicationAttachment> GetCommunicationAttachments(int tenant)
        {
            return (from record in context.CommunicationAttachments where record.Tenant == tenant select record);
        }

        public CommunicationAttachment GetSingleCommunicationAttachment(string id, int tenant)
        {
            return (from record in context.CommunicationAttachments where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public IQueryable<CommunicationAttachment> GetCommunicationAttachmentsForCommLog(string comLogId,int tenant)
        {
            return (from a in context.CommunicationAttachments.Include("Document")
                    where a.Tenant == tenant && a.CommunicationLogId == comLogId
                    select a);
        }

        public void Add(CommunicationAttachment entity)
        {
            context.CommunicationAttachments.Add(entity);
        }

        public void Remove(CommunicationAttachment entity)
        {
            context.CommunicationAttachments.Attach(entity);
            context.CommunicationAttachments.Remove(entity);
        }

        public void Update(CommunicationAttachment entity)
        {
            context.CommunicationAttachments.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CommunicationAttachment> All()
        {
            return context.CommunicationAttachments.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<CommunicationAttachment> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CommunicationAttachment GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}