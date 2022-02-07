using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class AnalyzeQueueRepository:IRepository<AnalyzeQueue>
    {
        IGlobalContext globalContext;

        public AnalyzeQueueRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public AnalyzeQueueRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public AnalyzeQueue GetOpenAnalyzeQueue(string fromSide)
        {
            return (from a in context.AnalyzeQueues.Include("AnalyzeQueueStatus").Include("TenantManagement")
                    where a.Status == "W" && a.From == fromSide
                    select a).OrderBy(d => d.Retries).FirstOrDefault();
        }

        public AnalyzeQueue GetSingleAnalyzeQueue(string id, int tenant)
        {
            return (from a in context.AnalyzeQueues.Include("AnalyzeQueueStatus").Include("TenantManagement")
                    where a.Id == id 
                    select a).FirstOrDefault();
        }

        public AnalyzeQueue GetSingleAnalyzeQueue(string id)
        {
            return (from a in context.AnalyzeQueues.Include("AnalyzeQueueStatus").Include("TenantManagement")
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<AnalyzeQueue> GetAllAnalyzeQueues()
        {
            return from a in context.AnalyzeQueues.Include("AnalyzeQueueStatus").Include("TenantManagement")
                   select a;
        }

        public IQueryable<AnalyzeQueue> GetAnalyzeQueues(int tenant)
        {
            return from a in context.AnalyzeQueues.Include("AnalyzeQueueStatus").Include("TenantManagement")
                   select a;
        }
        public void Add(AnalyzeQueue entity)
        {
            context.AnalyzeQueues.Add(entity);
        }

        public void Remove(AnalyzeQueue entity)
        {
            context.AnalyzeQueues.Attach(entity);
            context.AnalyzeQueues.Remove(entity);
        }

        public void Update(AnalyzeQueue entity)
        {
            entity.SearchFields = entity.EntityReference + ',' + entity.From + ',' + entity.ObjectTableName + ',' + entity.Status + ',' + entity.Subject + ',' + entity.Tenant;
            context.AnalyzeQueues.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AnalyzeQueue> All()
        {
            return context.AnalyzeQueues.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AnalyzeQueue> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AnalyzeQueue GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }


        public bool IsAnalyzeQueueExsit(byte[] analyzeQueueMessageBody)
        {
            if (analyzeQueueMessageBody == null)
            {
                return false;
            }
            return (from a in context.AnalyzeQueues
                    where a.MessageBody.SequenceEqual(analyzeQueueMessageBody)
                    select a).Any();
        }
    }
}
