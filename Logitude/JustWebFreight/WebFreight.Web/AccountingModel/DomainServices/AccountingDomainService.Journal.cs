using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Windows.Documents;
using WebFreight.Web.Security;

namespace WebFreight.Web.AccountingModel.DomainServices
{
    public partial class AccountingDomainService
    {
        public JournalPM GetSingleJournalPM(string Id, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            journalQuery = new JournalQueryService(accountingContext);
            JournalPM Journal = journalQuery.GetSingle(Id, true, false);
            return Journal;

        }

        public JournalList GetSingleJournalList(string Id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);


            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            JournalListQueryService listService = new JournalListQueryService(accountingContext);
            return listService.GetSingle(Id);
        }

        public List<JournalList> GetJournalLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.SupplierInvoiceItemsTax", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            JournalListQueryService listService = new JournalListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<JournalList> GetJournalFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.SupplierInvoiceItemsTax", "READ", tenant);

            accountingContext = AccountingContext.GetContext(tenant);
            JournalListQueryService listService = new JournalListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public List<CashBookList> GetCashBookFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountingContext = AccountingContext.GetContext(tenant);
            CashBookListQueryService listService = new CashBookListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }
        public int GetCashBookFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            CashBookListQueryService queryService = new CashBookListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public List<BankDepositList> GetBankDepositFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountingContext = AccountingContext.GetContext(tenant);
            BankDepositListQueryService listService = new BankDepositListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }
        public int GetBankDepositFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            BankDepositListQueryService queryService = new BankDepositListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public int GetJournalFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.SupplierInvoiceItemsTax", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            JournalListQueryService queryService = new JournalListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public List<RevaluationList> GetRevaluationFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountingContext = AccountingContext.GetContext(tenant);
            RevaluationListQueryService listService = new RevaluationListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetRevaluationFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            RevaluationListQueryService queryService = new RevaluationListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertJournal(JournalPM entityPm)
        {
            SecurityUtility.CheckContactFeature("Journal", "NEW", entityPm.Tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(entityPm.Tenant);
            }

            //   JournalListQueryService service = new JournalListQueryService(accountingContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            JournalUpdateService service = new JournalUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
          
            service.Update(entityPm, true);

        }

        public void UpdateJournal(JournalPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("Journal", "UPDATE", currententityPm.Tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(currententityPm.Tenant);
            }

            JournalUpdateService service = new JournalUpdateService(accountingContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetJournalLinesChangeSet(currententityPm);
            service.Update(currententityPm, true);

        }

        [Invoke]
        public string GetJournalMaxNumber(int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            journalQuery = new JournalQueryService(accountingContext);
            return this.journalQuery.GetJournalMaxNumber(tenant);

           
        }


        


        [Invoke]
        public JournalPM GetJouranlByJournalNumber(string journalNumber, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            JournalQueryService qs = new JournalQueryService(accountingContext);
            List<JournalPM> list = qs.GetJournalByJournalNumber(journalNumber, tenant);
            return list.FirstOrDefault();
        }

        private void SetJournalLinesChangeSet(JournalPM currententityPm)
        {
            List<JournalLinePM> journalLineChangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.JournalLines).Cast<JournalLinePM>().ToList();
            foreach (JournalLinePM itemPM in journalLineChangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            JournalLinePM currentItemPM = currententityPm.JournalLines.Where(d => d.JournalId == itemPM.JournalId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            //SetSupplierInvoiceItemChangeSet(currentItemPM);
                            //SetSupplierInvoiceModificationsChangeSet(currentItemPM);
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            JournalLinePM currentItemPM = currententityPm.JournalLines.Where(d => d.JournalId == itemPM.JournalId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            //SetSupplierInvoiceItemChangeSet(currentItemPM);
                            //SetSupplierInvoiceModificationsChangeSet(currentItemPM);
                            //SetSuppli/erInvoiceFreightAmounts(currentItemPM);
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            JournalLinePM currentItemPM = new JournalLinePM() { ChangeSetOp = ChangeSetOperation.Delete, JournalId = itemPM.JournalId, Line = itemPM.Line, Tenant = currententityPm.Tenant };

                            currententityPm.DeletedJournalLines.Add(currentItemPM);

                            //invoice items

                            break;
                        }
                    default:
                        {
                            JournalLinePM currentItemPM = currententityPm.JournalLines.Where(d => d.JournalId == itemPM.JournalId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }


        [Invoke]
        public JournalPM GetJournalPM(string id, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            JournalQueryService qs = new JournalQueryService(accountingContext);
            JournalPM entityPM = qs.GetSingle(id, true, false);
            return entityPM;
        }

        public void UpdateJournalList(JournalList entity)
        {

        }


    }
}