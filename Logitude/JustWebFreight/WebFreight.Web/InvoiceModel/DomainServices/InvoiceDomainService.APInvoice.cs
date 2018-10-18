using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel;
using Logitude.BL.Helpers;
using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InvoiceModel.CustomFilters;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        public APInvoicePM GetSingleAPInvoicePM(string id, int tenant)
        {           
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APInvoice", "READ", tenant);

            apInvoiceQuery = new APInvoiceQuery(tenant);
            return apInvoiceQuery.GetSinglePM(id, tenant);
        }

        public APInvoiceList GetSingleAPInvoiceList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APInvoice", "READ", tenant);

            APInvoiceList entityList = null;
            aPInvoiceRepository = new APInvoiceRepository(tenant);
            apInvoiceQuery = new APInvoiceQuery(aPInvoiceRepository);
            APInvoice entity = aPInvoiceRepository.GetSingleAPInvoice(id, tenant);

            if (entity != null)
            {
                List<APInvoice> SingleEntityList = new List<APInvoice>();
                SingleEntityList.Add(entity);

                IQueryable<APInvoice> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<APInvoiceList> iQueryableEntityList = apInvoiceQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public void UpdateAPInvoiceList(APInvoiceList currentEntity)
        {

        }

        [Query(HasSideEffects = true)]
        public IQueryable<APInvoiceList> GetAPInvoiceFilters(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APInvoice", "READ", tenant);

            aPInvoiceRepository = new APInvoiceRepository(tenant);
            apInvoiceQuery = new APInvoiceQuery(aPInvoiceRepository);
            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<APInvoice> iQueryable = aPInvoiceRepository.GetIQueryableInvoices(tenant);
            APInvoiceCustomFilter customFilters = new APInvoiceCustomFilter(tenant);
            iQueryable = customFilters.GetFilteredQuery(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<APInvoice>(nonListQueryOperation, iQueryable);

            IQueryable<APInvoiceList> query2 = apInvoiceQuery.GetIQueryableEntityList(iQueryable);

            query2 = EntityListFilter.ApplyEntityListFilters(queryOperations, query2);

            query2 = filter.GetFilteredQuery<APInvoiceList>(listQueryOperation, query2);


            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                query2 = QuerySortClass.GetSortedQuery(queryOperations, query2, "APInvoice", tenant);
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.DueDate);
            }

            int skippedInvoices = queryOperations.PageIndex;
            query2 = query2.Skip(skippedInvoices);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAPInvoiceFiltersCount(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APInvoice", "READ", tenant);

            aPInvoiceRepository = new APInvoiceRepository(tenant);
            apInvoiceQuery = new APInvoiceQuery(aPInvoiceRepository);
            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<APInvoice> iQueryable = aPInvoiceRepository.GetIQueryableInvoices(tenant);
            APInvoiceCustomFilter customFilters = new APInvoiceCustomFilter(tenant);
            iQueryable = customFilters.GetFilteredQuery(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<APInvoice>(nonListQueryOperation, iQueryable);

            IQueryable<APInvoiceList> query2 = apInvoiceQuery.GetIQueryableEntityList(iQueryable);

            query2 = EntityListFilter.ApplyEntityListFilters(queryOperations, query2);

            query2 = filter.GetFilteredQuery<APInvoiceList>(listQueryOperation, query2);
            query2 = EntityListFilter.ApplyEntityListFilters(queryOperations, query2);

            int count = query2.Count();
            return count;
        }

        public List<PayableInvoiceClass> GetPayableInvoices(string payableId,string parentPayableId, int tenant)
        {            
            apInvoiceLineQuery = new APInvoiceLineQuery(tenant);
            return apInvoiceLineQuery.GetPayableInvoices(payableId, parentPayableId, tenant);
        }

        public List<APInvoiceLinePM> GetAPInvoiceLinesByEntityId(string invoiceId, string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APInvoice", "READ", tenant);

            apInvoiceLineQuery = new APInvoiceLineQuery(tenant);
            return apInvoiceLineQuery.GetAPInvoiceLinesByEntityId(invoiceId, entityId, tenant);
        }

        public void InsertAPInvoice(APInvoicePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("APInvoice", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }

            APInvoiceService service = new APInvoiceService(objectContext, entityPM.Tenant);
            service.Create(entityPM);
        }

        public void UpdateAPInvoice(APInvoicePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("APInvoice", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }

            List<APInvoiceLinePM> invoiceLinesChangeSet = new List<APInvoiceLinePM>();
            List<APInvoiceMultipleShipmentPM> invoiceMultipleShipmentsChangeSet = new List<APInvoiceMultipleShipmentPM>();
            List<APInvoicePaymentPM> invoicePaymentsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.InvoicePayments).Cast<APInvoicePaymentPM>().ToList();
            foreach (APInvoicePaymentPM item in invoicePaymentsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(item))
                {
                    case ChangeOperation.Insert: { item.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { item.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { item.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { item.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            if (entityPM.IsMultipleEntities)
            {
                invoiceMultipleShipmentsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.InvoiceMultipleShipments).Cast<APInvoiceMultipleShipmentPM>().ToList();
                foreach (APInvoiceMultipleShipmentPM item in invoiceMultipleShipmentsChangeSet)
                {
                    switch (ChangeSet.GetChangeOperation(item))
                    {
                        case ChangeOperation.Insert: { item.ChangeSetOp = ChangeSetOperation.Insert; break; }
                        case ChangeOperation.Update: { item.ChangeSetOp = ChangeSetOperation.Update; break; }
                        case ChangeOperation.Delete: { item.ChangeSetOp = ChangeSetOperation.Delete; break; }
                        default: { item.ChangeSetOp = ChangeSetOperation.None; break; }
                    }
                }               
            }

            else
            {
                invoiceLinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.InvoiceLines).Cast<APInvoiceLinePM>().ToList();
                foreach (APInvoiceLinePM item in invoiceLinesChangeSet)
                {
                    switch (ChangeSet.GetChangeOperation(item))
                    {
                        case ChangeOperation.Insert: { item.ChangeSetOp = ChangeSetOperation.Insert; break; }
                        case ChangeOperation.Update: { item.ChangeSetOp = ChangeSetOperation.Update; break; }
                        case ChangeOperation.Delete: { item.ChangeSetOp = ChangeSetOperation.Delete; break; }
                        default: { item.ChangeSetOp = ChangeSetOperation.None; break; }
                    }
                }
            }

            APInvoiceService service = new APInvoiceService(objectContext, entityPM.Tenant);
            service.SetChangeSets(invoiceLinesChangeSet, invoicePaymentsChangeSet, invoiceMultipleShipmentsChangeSet);
            service.Update(entityPM);
        }

        public void DeleteAPInvoice(APInvoicePM entityPM)
        {

        }

        public APInvoiceMultipleShortPM GetSingleAPInvoiceShortPM(string id, string shipmentId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APInvoice", "READ", tenant);

            apInvoiceQuery = new APInvoiceQuery(tenant);
            return apInvoiceQuery.GetSingleAPInvoiceShortPM(id, shipmentId, tenant);
        }

        public void UpdateAPInvoiceMultipleShort(APInvoiceMultipleShortPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("APInvoice", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }

            List<APInvoiceLinePM> invoiceLinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.InvoiceLines).Cast<APInvoiceLinePM>().ToList();
            foreach (APInvoiceLinePM item in invoiceLinesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(item))
                {
                    case ChangeOperation.Insert: { item.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { item.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { item.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { item.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            APInvoiceMultipleShortService service = new APInvoiceMultipleShortService(objectContext, entityPM);
            service.Update(invoiceLinesChangeSet);            
        }

        public List<APInvoiceTransferHistoryPM> GetAPInvoiceTransferHistory(string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APInvoice", "READ", tenant);

            apInvoiceQuery = new APInvoiceQuery(tenant);
            return apInvoiceQuery.GetAPInvoiceTransferHistory(entityId, tenant);
        }

        [Invoke]
        public bool CheckVendor_NumberDuplication(string vendorId, string invoiceNumber, string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            bool isDuplicated = false;

            aPInvoiceRepository = new APInvoiceRepository(tenant);

            IQueryable<APInvoice> iQueryable = aPInvoiceRepository.GetIQueryableInvoices(tenant);

            if (string.IsNullOrEmpty(entityId))
            {
                isDuplicated = iQueryable.Where(d => d.VendorId == vendorId && d.InvoiceNumber == invoiceNumber).Any();
            }

            else
            {
                isDuplicated = iQueryable.Where(d => d.VendorId == vendorId && d.InvoiceNumber == invoiceNumber && d.Id != entityId).Any();
            }

            return isDuplicated;
        }

    }
}