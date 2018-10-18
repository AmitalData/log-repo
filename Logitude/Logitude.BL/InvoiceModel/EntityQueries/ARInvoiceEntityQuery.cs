using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ARInvoiceEntityQuery
    {
        ARInvoiceEntityRepository repository;
        public ARInvoiceEntityQuery()
        {
            repository = new ARInvoiceEntityRepository(); 
        }


        public ARInvoiceEntityQuery(int tenant)
        {
            repository = new ARInvoiceEntityRepository(tenant);
        }

        public ARInvoiceEntityQuery(ARInvoiceEntityRepository arInvoiceEntityRepository)
        {
            repository = arInvoiceEntityRepository;
        }

        public IQueryable<ARInvoiceEntityPM> GetInvoiceEntityPMs(int tenant)
        {
            return from a in repository.context.ARInvoiceEntities
                   where a.Tenant == tenant
                   select new ARInvoiceEntityPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       ARInvoiceId = a.ARInvoiceId,
                       EntityId = a.EntityId,
                       ObjectTableId = a.ObjectTableId,
                   };
        }

        public ARInvoiceEntityPM GetSingleInvoiceEntityPM(string id)
        {
            return (from a in repository.context.ARInvoiceEntities
                    where a.Id == id
                    select new ARInvoiceEntityPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ARInvoiceId = a.ARInvoiceId,
                        EntityId = a.EntityId,
                        ObjectTableId = a.ObjectTableId,
                    }).FirstOrDefault();
        }

        public List<ARInvoiceEntityPM> GetInvoiceEntityPMsForInvoice(string invoiceId, int tenant)
        {
            return (from a in repository.context.ARInvoiceEntities
                    where a.ARInvoiceId == invoiceId && a.Tenant == tenant
                    select new ARInvoiceEntityPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ARInvoiceId = a.ARInvoiceId,
                        EntityId = a.EntityId,
                        ObjectTableId = a.ObjectTableId,
                        EntityReference = a.EntityReference,
                    }).ToList();
        }

        public ARInvoiceEntityPM GetSingleInvoiceEntityPMByInvoiceAndEntity(string invoiceid, string entityid, int tenant)
        {
            return (from a in repository.context.ARInvoiceEntities
                    where a.ARInvoiceId == invoiceid && a.EntityId == entityid && a.Tenant == tenant
                    select new ARInvoiceEntityPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ARInvoiceId = a.ARInvoiceId,
                        EntityId = a.EntityId,
                        ObjectTableId = a.ObjectTableId,
                        EntityReference = a.EntityReference,
                    }).FirstOrDefault();
        }
        
        public IQueryable<ARInvoiceEntityPM> GetInvoiceEntityPMsForEntity(string entityId, int tenant)
        {
            return (from a in repository.context.ARInvoiceEntities
                    where a.EntityId == entityId && a.Tenant == tenant
                    select new ARInvoiceEntityPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ARInvoiceId = a.ARInvoiceId,
                        EntityId = a.EntityId,
                        EntityReference = a.EntityReference,
                        ObjectTableId = a.ObjectTableId,
                    });
        }

        public List<string> GetEntityIds(int tenant, string objectTableName)
        {
            List<string> data = new List<string>();

            data = (from a in repository.context.ARInvoiceEntities.Include("ObjectTable")
                    where a.ObjectTable.Name == objectTableName && a.Tenant == tenant
                    select a.EntityId).ToList();

            return data;
        }

        public List<ARInvoiceEntity> GetShipmentInvoiceEntities(int tenant)
        {
            List<ARInvoiceEntity> data = new List<ARInvoiceEntity>();

            data = (from a in repository.context.ARInvoiceEntities.Include("ObjectTable").Include("ARInvoice")
                    where a.ObjectTable.Name == "Shipment" && a.Tenant == tenant
                    select a).ToList();

            return data;
        }
    }
}