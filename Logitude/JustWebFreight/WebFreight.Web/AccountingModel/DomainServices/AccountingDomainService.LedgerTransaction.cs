using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Server.Tools.Helpers;
//using Logitude.BL.AccountingModel.CustomFilters;
using System.Transactions;
using Logitude.BL.Helpers;
using Logitude.Accounting.BL.Extensions;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.AccountingModel.DomainServices
{
    public partial class AccountingDomainService
    {
        public LedgerTransactionPM GetSingleLedgerTransactionPM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            ledgerTransactionQuery = new LedgerTransactionQueryService(accountingContext);
            LedgerTransactionPM ledgerTransactionPM = ledgerTransactionQuery.GetSingle(code, true, false);
            return ledgerTransactionPM;
        }


        [Invoke]
        public bool CheckIfLedgerTransactionOtherCurrencyExist(string gLAccointId, string currencyId, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            ledgerTransactionQuery = new LedgerTransactionQueryService(accountingContext);
            return this.ledgerTransactionQuery.CheckIfLedgerTransactionOtherCurrencyExist(gLAccointId, currencyId, tenant);
         
        }

        [Invoke]
        public List<LedgerTransactionList> LedgerTransactionByYearAndAccount(string gLAccointId, DateTime year, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService listService = new LedgerTransactionListQueryService(accountingContext);
            var list = listService.GetByAccountId(gLAccointId, year, tenant);
            return list;
        }

        [Invoke]
        public List<LedgerTransactionList> LedgerTransactionFromTo(string gLAccointId, DateTime fromDate, DateTime toDate, int tenant, string currencyId)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService listService = new LedgerTransactionListQueryService(accountingContext);
            var list = listService.GetByAccountId(gLAccointId, fromDate, toDate, tenant, currencyId);
            return list;

        }


        [Invoke]
        public List<LedgerTransactionPM> LedgerTransactionsByIdList(List<string> idList, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionQueryService listService = new LedgerTransactionQueryService(accountingContext);
            List<LedgerTransactionPM> list = listService.GetLedgerTransactionPMsByIdList(idList, tenant);
            return list;
        }


           //[Invoke]
           //public List<LedgerTransactionReconciliationLineList> OpenLedgerTransactionByAccount(string gLAccointId, int tenant)
           //{
           //    accountingContext = AccountingContext.GetContext(tenant);
           //    LedgerTransactionListQueryService listService = new LedgerTransactionListQueryService(accountingContext);
           //    List<LedgerTransactionReconciliationLineList> list = listService.GetOpenWithRecoLineByAccountId(gLAccointId, tenant);
           //    return list;
           //}

        [Invoke]
        public bool AutomaticReconciliationByAccount(string gLAccointId, string automaticReconcileMethodId, string reconcileMethodCode, int tenant)
        {
            bool rv = false;
            accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService listService = new LedgerTransactionListQueryService(accountingContext);

            // 1. Get open Ledger Transactions
            List<LedgerTransactionList> openLedgerTransactions = listService.GetOpenByAccountId(gLAccointId, tenant);

            // 2. Get automatic reconcile method
            AutomaticReconcileMethodList automaticReconcileMethod = this.GetAutomaticReconcileMethod(automaticReconcileMethodId, tenant);

            // 3. Get accounting currency
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tPM = tenantQuery.GetSinglePM(tenant);
            string accountingCurrencyId = tPM.CurrencyId;
            CurrencyQuery currencyQueryService = new CurrencyQuery(tenant);
            CurrencyPM currency = currencyQueryService.GetSinglePM(accountingCurrencyId, tenant);
            string accountingCurrencyCode = "";
            if (currency != null) accountingCurrencyCode = currency.Code;

            List<LedgerTransactionExtension> ledgerTransactionExtensions = new List<LedgerTransactionExtension>();
            foreach (var item in openLedgerTransactions)//(LedgerTransactionReconcileViewModel item in array)
            {
                LedgerTransactionExtension extension = new LedgerTransactionExtension(item, reconcileMethodCode, accountingCurrencyId, accountingCurrencyCode);
                extension.SetXYZ(automaticReconcileMethod.AutomaticReconcile1, automaticReconcileMethod.AutomaticReconcile2, automaticReconcileMethod.AutomaticReconcile3);
                ledgerTransactionExtensions.Add(extension);
            }

            IComparer<LedgerTransactionExtension> ic = LedgerTransactionExtension.sortLedgerTransactionExtension(automaticReconcileMethod.AutomaticReconcile1, automaticReconcileMethod.AutomaticReconcile2, automaticReconcileMethod.AutomaticReconcile3);
            ledgerTransactionExtensions.Sort(ic);
            List<string> idList = new List<string>(); ;
            int i = 0;
            int startLine = 0;
            decimal total = 0;
            foreach (LedgerTransactionExtension item in ledgerTransactionExtensions)
            {
                LedgerTransactionExtension current; 
                LedgerTransactionExtension previous = null; 
                LedgerTransactionExtension next = null; 

                current = ledgerTransactionExtensions.ElementAt(i);
                if (i > 0) previous = ledgerTransactionExtensions.ElementAt(i - 1);

                if (i + 1 < ledgerTransactionExtensions.Count) next = ledgerTransactionExtensions.ElementAt(i + 1);

                if (i == 0) // the very first line 
                {
                    total = current.OpenAmount;
                    startLine = i;
                }
                else if (previous != null && ic.Compare(current, previous) == 0) // equal, i. e. the current sequence is contiguous 
                {
                    total += current.OpenAmount;
                }
                else // start of a new sequence 
                {
                    total = current.OpenAmount;
                    startLine = i;
                }

                if (total == 0 && (next == null || ic.Compare(current, next) != 0)) // end of a sequence
                {
                    for (int k = startLine; k < i + 1; k++)
                    {
                        LedgerTransactionExtension ltExt = ledgerTransactionExtensions.ElementAt(k);
                        ltExt.AmountToReconcile = ltExt.OpenAmount;
                        ltExt.Mark = true;
                        idList.Add(ltExt.ledgerTransactionList.Id);
                    }
                }


                i++;
            }

            LedgerTransactionQueryService queryService = new LedgerTransactionQueryService(accountingContext);
            List<LedgerTransactionPM> ltPMs = queryService.GetLedgerTransactionPMsByIdList(idList, tenant);
            foreach (LedgerTransactionPM item in ltPMs)
            {
                LedgerTransactionExtension ltExtSource = ledgerTransactionExtensions.Where(d => d.ledgerTransactionList.Id == item.Id).FirstOrDefault();
                if (item != null)
                {
                    item.AmountToReconcile = ltExtSource.AmountToReconcile.GetValueOrDefault();
                    item.Mark = ltExtSource.Mark;
                    item.ChangeSetOp = ChangeSetOperation.Update;
                    rv = true;
                }
            }
            LedgerTransactionUpdateService service = new LedgerTransactionUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);

            foreach (LedgerTransactionPM item in ltPMs)
            {
                service.Update(item, true);
            }
            return rv;// ledgerTransactionExtensions;
        } //AutomaticReconciliationByAccount

        [Invoke]
        public string GetAnyLedgerTransactionCurrency(string gLAccointId, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            ledgerTransactionQuery = new LedgerTransactionQueryService(accountingContext);
            return this.ledgerTransactionQuery.GetAnyLedgerTransactionCurrency(gLAccointId, tenant);
        }


        [Invoke]
        public string GetCurrencyWhenMultiOff(string gLAccointId, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            ledgerTransactionQuery = new LedgerTransactionQueryService(accountingContext);
            return this.ledgerTransactionQuery.GetCurrencyWhenMultiOff(gLAccointId, tenant);
        }

        [Invoke]
        public decimal? GetLedgerTransactionSumFromTo(string gLAccointId,  DateTime fromDate, DateTime toDate, int tenant)
        {

            accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionQueryService ServiceQuery = new LedgerTransactionQueryService(accountingContext);
            var myBalance = ServiceQuery.GetLedgerTransactionSumFromTo(gLAccointId, fromDate, toDate, tenant);
            return myBalance;

        }


        //[Invoke]
        //public decimal? GetLedgerTransactionSumFromTo(string gLAccointId, DateTime fromDate, DateTime toDate, int tenant, string currencyId)
        //{

        //    accountingContext = AccountingContext.GetContext(tenant);
        //    LedgerTransactionQueryService ServiceQuery = new LedgerTransactionQueryService(accountingContext);
        //    var myBalance = ServiceQuery.GetLedgerTransactionSumFromTo(gLAccointId, fromDate, toDate, tenant);
        //    return myBalance;

        //}

        

        [Invoke]
        public decimal? GetLedgerTransactionSumFromToByCurrnecy(string gLAccointId, DateTime fromDate, DateTime toDate, int tenant, string currencyId)
        {

            accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionQueryService ServiceQuery = new LedgerTransactionQueryService(accountingContext);
            var myBalance = ServiceQuery.GetLedgerTransactionSumFromTo(gLAccointId, fromDate, toDate, tenant, currencyId);
            return myBalance;

        }


        
        [Invoke]
        public List<CurrencySum> GetLedgerTransactionTotalLocalAmountFromTo(string gLAccointId, DateTime fromDate, DateTime toDate, int tenant)
        {

            accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionQueryService ServiceQuery = new LedgerTransactionQueryService(accountingContext);
            var myBalance = ServiceQuery.GetLedgerTransactionTotalLocalAmountFromTo(gLAccointId, fromDate, toDate, tenant);
            return myBalance;

        }



        [Invoke]
        public bool DoesDraftReconciliationByAccountExist(string gLAccountId, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService listService = new LedgerTransactionListQueryService(accountingContext);
            var rv = listService.DoesDraftByAccountIdExist(gLAccountId, tenant);
            return rv;
        }


        public List<LedgerTransactionPM> GetDraftLedgerTransactionPMsByAccountId(string gLAccountId, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            ledgerTransactionQuery = new LedgerTransactionQueryService(accountingContext);
            List<LedgerTransactionPM> draftPMs = ledgerTransactionQuery.DraftLedgerTransactionPMsByAccountId(gLAccountId, tenant);
            return draftPMs;
        }



        public LedgerTransactionList GetSingleLedgerTransactionList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService listService = new LedgerTransactionListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<LedgerTransactionList> GetLedgerTransactionLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService listService = new LedgerTransactionListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        //public List<LedgerTransactionList> GetLedgerTransactionFilters(byte[] xmlFilters, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
        //    accountingContext = AccountingContext.GetContext(tenant);
        //    LedgerTransactionListQueryService listService = new LedgerTransactionListQueryService(accountingContext);
        //    QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
        //    return listService.GetList(queryOperations, tenant);

        //}

        public int GetLedgerTransactionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService queryService = new LedgerTransactionListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }


        public void UpdateLedgerTransactionList(LedgerTransactionList entity)
        {

        }


        [Query(HasSideEffects = true)]
        public IQueryable<LedgerTransactionList> GetLedgerTransactionFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("LedgerTransaction", "READ", tenant);

            ledgerTransactionRepository = new LedgerTransactionRepository(tenant);
            ledgerTransactionQuery = new LedgerTransactionQueryService(ledgerTransactionRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<LedgerTransaction> iQueryable = ledgerTransactionRepository.GetAll(tenant);

            //LedgerTransactionCustomFilter customFilter = new LedgerTransactionCustomFilter(tenant);
            //iQueryable = customFilter.GetFilteredQuery(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<LedgerTransaction>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<LedgerTransactionList> query2 = from a in iQueryable.Include("JournalLine").Include("Account").Include("Currency").Include("Journal")
                                                        select new LedgerTransactionList()
                                                        {
                                                            Id = a.Id,
                                                            // Account = a.Account,
                                                            AccountId = a.AccountId,
                                                            AccountingDate = a.AccountingDate,
                                                            // ControlAccount = a.ControlAccount,
                                                            ControlAccountId = a.ControlAccountId,
                                                            // Currency = a.Currency,
                                                            CurrencyId = a.CurrencyId,
                                                            DocumentDate = a.DocumentDate,
                                                            DueDate = a.DueDate,
                                                            ExchangeRate = a.ExchangeRate,
                                                            ForeignAmountCredit = a.ForeignAmountCredit,
                                                            ForeignAmountDebit = a.ForeignAmountDebit,
                                                            ForeignAmount = a.ForeignAmountDebit == 0 ? a.ForeignAmountCredit : a.ForeignAmountDebit,
                                                            ReconcileMethodCode = a.Account.ReconcileMethodCode,
                                                            JournalId = a.JournalId,
                                                            JournalNumber = a.JournalLine.Journal.JournalNumber,
                                                            Source = a.JournalLine.Journal.AccountingEntityReference,
                                                            SourceType = a.JournalLine.Journal.AccountingEntity.EnglishName,
                                                            SourceTypeCode = a.JournalLine.Journal.AccountingEntity.Code,
                                                            CurrencyCode = a.Currency.Code,
                                                            // JournalLine = a.JournalLine,
                                                            JournalLineNumber = a.JournalLineNumber,
                                                            LocalAmountCredit = a.LocalAmountCredit,
                                                            LocalAmountDebit = a.LocalAmountDebit,
                                                            OpenAmount = a.OpenAmount,
                                                            Reference1 = a.Reference1,
                                                            Reference2 = a.Reference2,
                                                            Reference3 = a.Reference3,
                                                            CreateDate = a.CreateDate,
                                                            Tenant = a.Tenant,
                                                            AmountToReconcile = a.AmountToReconcile,
                                                            Mark = a.Mark,
                                                            Notes = a.Notes,
                                                            InternalNote=a.InternalNote,
                                                            OpenAmountCurrencyId = a.OpenAmountCurrencyId,
                                                            OppositeAccountId = a.OppositeAccountId,
                                                            SearchFields = a.SearchFields,
                                                            OpenAmountCurrencyCode = a.OpenAmountCurrency.Code,
                                                            IsReconciled = a.IsReconciled,
                                                            IsExternalReconcile = a.IsExternalReconcile,
                                                            InReconcileProgress = a.InReconcileProgress,
                                                            AccountDisplayNumber = a.Account != null ? a.Account.DisplayNumber : null,
                                                            AccountLocalName = a.Account != null ? a.Account.LocalName : null,
                                                            OppositeAccountEnglishName = a.OppositeAccount != null ? a.OppositeAccount.EnglishName : null,
                                                            OppositeAccountLocalName = a.OppositeAccount != null ? a.OppositeAccount.LocalName : null,
                                                            OppositeAccountDisplayNumber = a.OppositeAccount != null ? a.OppositeAccount.DisplayNumber : null,
															SecurityLevelFiltering = 1,
														};

            query2 = filter.GetFilteredQuery<LedgerTransactionList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(LedgerTransactionList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> ledgerTransactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LedgerTransaction", tenant).ToList();

                ObjectField objectField = (from a in ledgerTransactionObjectFields
                                            where a.FieldName == queryOperations.SortByColumnName
                                            select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<LedgerTransactionList, string>(queryOperations, query2);
                                break;
                            }
                        case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<LedgerTransactionList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<LedgerTransactionList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<LedgerTransactionList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<LedgerTransactionList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<LedgerTransactionList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Mark).ThenBy(d => d.AccountingDate).ThenBy(d => d.JournalNumber);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Mark).ThenBy(d => d.AccountingDate).ThenBy(d => d.JournalNumber);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);


            List<LedgerTransactionList> listQuery = query2.ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("LedgerTransaction", tenant, listQuery.Cast<object>().ToList());

            return query2;

        }

        public void InsertLedgerTransaction(LedgerTransactionPM entityPm)
        {
            SecurityUtility.CheckContactFeature("LedgerTransaction", "NEW", entityPm.Tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(entityPm.Tenant);
            }


            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            LedgerTransactionUpdateService service = new LedgerTransactionUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPm.Tenant);

            service.Update(entityPm, true);
        }

    

        public void UpdateLedgerTransaction(LedgerTransactionPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("LedgerTransaction", "UPDATE", currententityPm.Tenant);
            var sssss = this.ChangeSet.ChangeSetEntries;
            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(currententityPm.Tenant);
            }
            //currententityPm.MarkAsChanged = true;

            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            LedgerTransactionUpdateService service = new LedgerTransactionUpdateService(accountingContext, new Dictionary<string, IContext>(), currententityPm.Tenant);

            service.Update(currententityPm, true);

        }


    }

    

    public class SerializableResult
    {
        public string Result { get; set; }
    }
}