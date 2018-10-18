using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InvoiceModel.Repositories;

using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class APInvoiceEntityQuery
    {
        APInvoiceEntityRepository repository;
        public APInvoiceEntityQuery()
        {
            repository = new APInvoiceEntityRepository(); 
        }
        public APInvoiceEntityQuery(int tenant)
        {
            repository = new APInvoiceEntityRepository(tenant);
        }
        public APInvoiceEntityQuery(APInvoiceEntityRepository aPInvoiceEntityRepository)
        {
            repository = aPInvoiceEntityRepository;
        }

        public List<APInvoiceEntityPM> GetInvoiceEntitiesPMForInvoice(string invoiceId, int tenant)
        {
            return (from a in repository.context.APInvoiceEntities
                    where a.APInvoiceId == invoiceId && a.Tenant == tenant
                    select new APInvoiceEntityPM()
                    {
                        APInvoiceId = a.APInvoiceId,
                        Id = a.Id,
                        EntityId = a.EntityId,
                        Tenant = a.Tenant,
                        EntityReference = a.EntityReference,
                        ObjectTableId = a.ObjectTableId,
                        IndexOrder = a.IndexOrder,
                    }).ToList();
        }

        public APInvoiceEntityPM GetSingleAPInvoiceEntityPM(string id, int tenant)
        {
            return (from a in repository.context.APInvoiceEntities
                    where a.Id == id && a.Tenant == tenant
                    select new APInvoiceEntityPM()
                    {
                        APInvoiceId = a.APInvoiceId,
                        Id = a.Id,
                        EntityId = a.EntityId,
                        Tenant = a.Tenant,
                        EntityReference = a.EntityReference,
                        ObjectTableId = a.ObjectTableId,
                        IndexOrder = a.IndexOrder,
                    }).FirstOrDefault();
        }

        public IQueryable<APInvoiceEntityPM> GetAPInvoiceEntityPMsByTenant(int tenant)
        {
            return (from a in repository.context.APInvoiceEntities
                    where a.Tenant == tenant
                    select new APInvoiceEntityPM()
                    {
                        APInvoiceId = a.APInvoiceId,
                        Id = a.Id,
                        EntityId = a.EntityId,
                        Tenant = a.Tenant,
                        EntityReference = a.EntityReference,
                        ObjectTableId = a.ObjectTableId,
                        IndexOrder = a.IndexOrder,
                    });
        }
    }
}