using System.Collections.Generic;
using System.Linq;
using WebFreight.Web.Security;
using Simplog.Data.InvoiceModel.Repositories;
using System.IO;
using System.Xml.Serialization;
using Simplog.Data.InvoiceModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.MessageModel;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InvoiceModel.CustomFilters;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        private AccountingTransferHeaderQuery accountingTransferHeaderQuery;
        private AccountingTransferHeaderRepository accountingTransferHeaderRepository;

        public void InsertAccountingTransferHeader(AccountingTransferHeaderPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("AccountingTransferHeader", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }

            AccountingTransferHeaderService service = new AccountingTransferHeaderService(objectContext, entityPM.Tenant);
            service.Create(entityPM);
        }

        public void UpdateAccountingTransferHeader(AccountingTransferHeaderPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("AccountingTransferHeader", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }


            List<AccountingTransferLinePM> transferLinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.TransferLines).Cast<AccountingTransferLinePM>().ToList();
            foreach (AccountingTransferLinePM itemPM in transferLinesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            AccountingTransferHeaderService service = new AccountingTransferHeaderService(objectContext, entityPM.Tenant);

            service.SetChangeSet(transferLinesChangeSet);
            service.Update(entityPM,true);
        }

        public AccountingTransferHeaderPM GetSingleAccountingTransferHeader(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AccountingTransferHeader", "READ", tenant);

            accountingTransferHeaderQuery = new AccountingTransferHeaderQuery(tenant);
            return accountingTransferHeaderQuery.GetSinglePM(id, tenant);
        }

        public void UpdateAccountingTransferHeaderList(AccountingTransferHeaderList currentEntity)
        {

        }

        public AccountingTransferHeaderList GetSingleAccountingTransferHeaderList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("AccountingTransferHeader", "READ", tenant);

            AccountingTransferHeaderList entityList = null;
            accountingTransferHeaderRepository = new AccountingTransferHeaderRepository(tenant);
            accountingTransferHeaderQuery = new AccountingTransferHeaderQuery(accountingTransferHeaderRepository);
            AccountingTransferHeader entity = accountingTransferHeaderRepository.GetSingleEntity(id, tenant);

            if (entity != null)
            {
                List<AccountingTransferHeader> SingleEntityList = new List<AccountingTransferHeader>();
                SingleEntityList.Add(entity);

                IQueryable<AccountingTransferHeader> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<AccountingTransferHeaderList> iQueryableEntityList = accountingTransferHeaderQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AccountingTransferHeaderList> GetAccountingTransferHeaderFilters(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AccountingTransferHeader", "READ", tenant);

            accountingTransferHeaderRepository = new AccountingTransferHeaderRepository(tenant);
            accountingTransferHeaderQuery = new AccountingTransferHeaderQuery(accountingTransferHeaderRepository);

            IQueryable<AccountingTransferHeader> iQueryable = accountingTransferHeaderRepository.GetAccountingTransferHeaders(tenant);
            TransferHeaderCustomFilter customFilters = new TransferHeaderCustomFilter(tenant);

            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(XmlFilters);

            iQueryable = EntityListFilter.ApplyEntityNonListFilters(queryOperations, iQueryable);
            iQueryable = customFilters.GetFilteredQuery(queryOperations, iQueryable);

            IQueryable<AccountingTransferHeaderList> query2 = accountingTransferHeaderQuery.GetIQueryableEntityList(iQueryable);

            query2 = EntityListFilter.ApplyEntityListFilters(queryOperations, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                query2 = QuerySortClass.GetSortedQuery(queryOperations, query2, "AccountingTransferHeader", tenant);
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.TransferDate);
            }

            int skippedInvoices = queryOperations.PageIndex;
            query2 = query2.Skip(skippedInvoices);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAccountingTransferHeaderFiltersCount(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AccountingTransferHeader", "READ", tenant);

            accountingTransferHeaderRepository = new AccountingTransferHeaderRepository(tenant);
            accountingTransferHeaderQuery = new AccountingTransferHeaderQuery(accountingTransferHeaderRepository);

            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AccountingTransferHeader> iQueryable = accountingTransferHeaderRepository.GetAccountingTransferHeaders(tenant);

            TransferHeaderCustomFilter customFilters = new TransferHeaderCustomFilter(tenant);
            iQueryable = customFilters.GetFilteredQuery(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AccountingTransferHeader>(nonListQueryOperation, iQueryable);

            IQueryable<AccountingTransferHeaderList> query2 = accountingTransferHeaderQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<AccountingTransferHeaderList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        [Invoke]
        public void RebuildTransferFile(string transferHeaderId, int tenant)
        {
            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(tenant);
            }

            AccountingTransferHeaderRepository entityRepository = new AccountingTransferHeaderRepository(objectContext);
            AccountingTransferLineRepository entityLineRepository = new AccountingTransferLineRepository(objectContext);

            AccountingTransferHeader entityPoco = entityRepository.GetSingleEntity(transferHeaderId, tenant);
            List<string> invoicesIdsList = entityLineRepository.GetInvoicesIdsList(transferHeaderId, tenant);

            MessageWebService webService = new MessageWebService();

            switch (entityPoco.AccountingTransferTypeCode)
            {
                case "ARIN":
                    {
                        ARInvoiceRepository myRepository = new ARInvoiceRepository(tenant);
                        List<ARInvoice> invoices = myRepository.GetInvoicesListFromIdList(invoicesIdsList, tenant);
                        webService.RebuildTransferFile(invoices, entityPoco.FileName, tenant);
                        break;
                    }

                case "APIN":
                    {
                        APInvoiceRepository myRepository = new APInvoiceRepository(tenant);
                        List<APInvoice> invoices = myRepository.GetInvoicesListFromIdList(invoicesIdsList, tenant);
                        webService.RebuildTransferFile(invoices, entityPoco.FileName, tenant);
                        break;
                    }
            }
        }
    }
}