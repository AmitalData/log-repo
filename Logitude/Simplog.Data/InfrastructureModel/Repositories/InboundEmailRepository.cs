using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class InboundEmailRepository : IRepository<InboundEmail>
    {
        public IWebFreightContext webFreightContext;

        public InboundEmailRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public InboundEmailRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public InboundEmailRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public InboundEmail GetSingleInboundEmail(string id, int tenant)
        {

            InboundEmail entity = this.webFreightContext.InboundEmails.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();


            return entity;

        }

        public IQueryable<InboundEmail> GetInboundEmailsByTenant(int tenant)
        {
            IQueryable<InboundEmail> InboundEmails = from a in webFreightContext.InboundEmails
                                                       where a.Tenant == tenant
                                                       select a;
            return InboundEmails;

        }
        public IQueryable<InboundEmail> GetInboundEmailsByObjectTable(string objectTableName, int tenant)
        {
            IQueryable<InboundEmail> InboundEmails = from a in webFreightContext.InboundEmails.Include("ObjectTable")
                                                       where a.Tenant == tenant && a.ObjectTable.Name == objectTableName
                                                       select a;
            return InboundEmails;

        }
        public IQueryable<InboundEmail> GetInboundEmails(int tenant)
        {
            return (from record in webFreightContext.InboundEmails where record.Tenant == tenant select record);
        }

        public IQueryable<InboundEmail> GetAllInboundEmails()
        {
            return (from record in webFreightContext.InboundEmails select record);
        }

        public InboundEmail GetInboundEmail(string entityId, string objectTableId, int tenant)
        {
            return (from d in webFreightContext.InboundEmails
                    where d.Tenant == tenant
                    && d.ObjectTableId == objectTableId
                    && d.EntityId == entityId
                    select d).FirstOrDefault();
        }
        
        public void Add(InboundEmail entity)
        {
            webFreightContext.InboundEmails.Add(entity);
        }

        public void Remove(InboundEmail entity)
        {
            webFreightContext.InboundEmails.Attach(entity);
            webFreightContext.InboundEmails.Remove(entity);
        }

        public void Update(InboundEmail entity)
        {
            webFreightContext.InboundEmails.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<InboundEmail> All()
        {
            return webFreightContext.InboundEmails.ToList();
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }

        public List<InboundEmail> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public InboundEmail GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public bool GetInboundEmailByAnalyzeQueueId(string analyzeQueueId, int tenant)
        {
            return (from d in webFreightContext.InboundEmails
                    where d.Tenant == tenant
                    && d.AnalyzeQueueId == analyzeQueueId
                    select d).Any();
        }
    }
}
