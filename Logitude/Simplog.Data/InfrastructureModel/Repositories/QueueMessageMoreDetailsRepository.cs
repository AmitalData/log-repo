using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class QueueMessageMoreDetailsRepository:IRepository<QueueMessageMoreDetails>
    {

        IWebFreightContext webFreightContext;
        public QueueMessageMoreDetailsRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public QueueMessageMoreDetailsRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public QueueMessageMoreDetailsRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<QueueMessageMoreDetails> GetQueueMessageMoreDetails()
        {
            return context.QueueMessageMoreDetails;
        }
        public IQueryable<QueueMessageMoreDetails> GetQueueMessageMoreDetails(DateTime BeforeDate)
        {
            return context.QueueMessageMoreDetails.Where(a => a.ProcessingDateTime < BeforeDate);
        }
        public QueueMessageMoreDetails GetSingleQueueMessage(long id)
        {
            return (from a in context.QueueMessageMoreDetails
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public void Add(QueueMessageMoreDetails entity)
        {
            context.QueueMessageMoreDetails.Add(entity);
        }


        public QueueMessageMoreDetails GetSingleQueueMessageMoreDetails(string id)
        {
            long? longId = null;
            if (id != null)
            {
                longId = long.Parse(id);
            }

            return (from a in context.QueueMessageMoreDetails
                    where a.Id == longId
                    select a).FirstOrDefault();


        }

        public void Remove(QueueMessageMoreDetails entity)
        {
            context.QueueMessageMoreDetails.Attach(entity);
            context.QueueMessageMoreDetails.Remove(entity);
        }

        public void Update(QueueMessageMoreDetails entity)
        {
            context.QueueMessageMoreDetails.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QueueMessageMoreDetails> All()
        {
            return context.QueueMessageMoreDetails.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<QueueMessageMoreDetails> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QueueMessageMoreDetails GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}