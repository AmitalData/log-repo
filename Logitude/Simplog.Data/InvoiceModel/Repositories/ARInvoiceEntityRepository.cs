using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARInvoiceEntityRepository: IRepository<ARInvoiceEntity>
    {

        IInvoiceContext invoiceContext;
        public ARInvoiceEntityRepository()
        {
            invoiceContext = new InvoiceContext();

        }

        public ARInvoiceEntityRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }
        public ARInvoiceEntityRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public IQueryable<ARInvoiceEntity> GetInvoiceEntities(int tenant)
        {
            return from a in context.ARInvoiceEntities
                   where a.Tenant == tenant
                   select a;
        }

        public ARInvoiceEntity GetSingleInvoiceEntity(string id)
        {
            return (from a in context.ARInvoiceEntities
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public ARInvoiceEntity GetSingleInvoiceEntityByInvoiceId(string invoiceId)
        {
            return (from a in context.ARInvoiceEntities
                    where a.ARInvoiceId == invoiceId
                    select a).FirstOrDefault();
        }

        public ARInvoiceEntity GetSingleMasterInvoiceEntityByInvoiceId(string invoiceId,int tenant)
        {
            ARInvoiceEntity result = null;
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Master", tenant, true);
            if (objectTable != null)
            {
                result = (from a in context.ARInvoiceEntities
                          where a.ARInvoiceId == invoiceId
                          && a.ObjectTableId == objectTable.Id
                          select a).FirstOrDefault();
            }

            return result;
        }

        public IQueryable<ARInvoiceEntity> GetInvoiceEntitiesForInvoice(string invoiceId, int tenant)
        {
            return (from a in context.ARInvoiceEntities where a.ARInvoiceId == invoiceId && a.Tenant == tenant select a);
        }

        public IQueryable<ARInvoiceEntity> GetInvoiceEntitiesForEntity(string entityId, int tenant)
        {
            return (from a in context.ARInvoiceEntities
                    where a.EntityId == entityId && a.Tenant == tenant
                    select a);
        }

        
      

        public ARInvoiceEntity GetSingleInvoiceEntityByInvoiceAndEntity(string invoiceid, string entityid, int tenant)
        {
            return (from a in context.ARInvoiceEntities
                    where a.ARInvoiceId == invoiceid && a.EntityId == entityid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

     

       

        public void Add(ARInvoiceEntity entity)
        {
            context.ARInvoiceEntities.Add(entity);
        }

        public void Remove(ARInvoiceEntity entity)
        {
            context.ARInvoiceEntities.Attach(entity);
            context.ARInvoiceEntities.Remove(entity);
        }

        public void Update(ARInvoiceEntity entity)
        {
            context.ARInvoiceEntities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARInvoiceEntity> All()
        {
            return context.ARInvoiceEntities.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<ARInvoiceEntity> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ARInvoiceEntity GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}