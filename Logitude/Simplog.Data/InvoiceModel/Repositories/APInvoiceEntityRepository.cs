using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InvoiceModel.Repositories
{
    public class APInvoiceEntityRepository: IRepository<APInvoiceEntity>
    {

        IInvoiceContext invoiceContext;
        public APInvoiceEntityRepository()
        {
            invoiceContext = new InvoiceContext();

        }
        public APInvoiceEntityRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public APInvoiceEntityRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<APInvoiceEntity> GetInvoiceEntitiesForInvoice(string invoiceId, int tenant)
        {
            return (from a in context.APInvoiceEntities
                    where a.APInvoiceId == invoiceId && a.Tenant == tenant
                    select a);
        }

        public IQueryable<APInvoiceEntity> GetInvoiceEntitiesbyEntityId(string entityId, int tenant)
        {
            return (from a in context.APInvoiceEntities where a.EntityId == entityId && a.Tenant == tenant select a);
        }
       
        public APInvoiceEntity GetSingleAPInvoiceEntity(string id, int tenant)
        {
            return (from a in context.APInvoiceEntities
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<APInvoiceEntity> GetAPInvoiceEntitiesByTenant(int tenant)
        {
            return (from a in context.APInvoiceEntities
                    where a.Tenant == tenant
                    select a);
        }
       
        public void Add(APInvoiceEntity entity)
        {
            context.APInvoiceEntities.Add(entity);
        }

        public void Remove(APInvoiceEntity entity)
        {
            context.APInvoiceEntities.Attach(entity);
            context.APInvoiceEntities.Remove(entity);
        }

        public void Update(APInvoiceEntity entity)
        {
            context.APInvoiceEntities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<APInvoiceEntity> All()
        {
            return  context.APInvoiceEntities.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public APInvoiceEntity GetMainAPInvoiceEntity(string invoiceid, string mainEntityId)
        {
            return (from a in context.APInvoiceEntities
                    where a.APInvoiceId == invoiceid && a.EntityId == mainEntityId
                    select a).FirstOrDefault();
        }


        public List<APInvoiceEntity> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public APInvoiceEntity GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}