using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Repositories;
using System.Reflection;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Logitude.Accounting.Data.Utilities;

namespace Logitude.Accounting.Data.EntityListQueryServices
{

    public partial class LedgerTransactionListQueryService
    {
        private IQueryable<LedgerTransactionList> GetIqueryableList(IQueryable<LedgerTransaction> iQueryable)
        {
            IQueryable<LedgerTransactionList> query = (from a in iQueryable.Include("JournalLine").Include("Currency").Include("Journal")
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

                                                           JournalId = a.JournalId,
                                                           JournalNumber = a.JournalLine.Journal.JournalNumber,
                                                           Source = a.JournalLine.Journal.AccountingEntityReference,
                                                           SourceType = a.JournalLine.Journal.AccountingEntity.EnglishName,
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
                                                           OpenAmountCurrencyId = a.OpenAmountCurrencyId,
                                                           OppositeAccountId = a.OppositeAccountId,
                                                           SearchFields = a.SearchFields,
                                                           OpenAmountCurrencyCode = a.OpenAmountCurrency.Code,
                                                           IsReconciled = a.IsReconciled,
                                                           InReconcileProgress = a.InReconcileProgress,

                                                           SourceId = a.JournalLine.Journal.AccountingEntityId, // hidden id to use in link
                                                           SourceNumber = a.JournalLine.Journal.AccountingEntityReference, // display number
                                                           SourceTypeCode = a.JournalLine.Journal.AccountingEntity.Code, // source type code from AccountingEntities

                                                           SelectCheckBox = false,
                                                           CurrencySign = a.Currency.Sign,
                                                           OpenAmountCurrencySign = a.OpenAmountCurrency.Sign,
                                                           IsExternalReconcile = a.IsExternalReconcile,
                                                           OppositeAccountEnglishName = a.OppositeAccount != null ? a.OppositeAccount.EnglishName : null,
                                                           OppositeAccountLocalName = a.OppositeAccount != null ? a.OppositeAccount.LocalName : null,
                                                           OppositeAccountDisplayNumber = a.OppositeAccount != null ? a.OppositeAccount.DisplayNumber : null,
                                                           
                                                       });


            return query;
        }

        private IQueryable<LedgerTransaction> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<LedgerTransaction> iQueryable, int tenant)
        {
            LedgerTransactionListCustomFilter customFilter = new LedgerTransactionListCustomFilter(tenant);
            QueryOperations customizedQueryOperation = new QueryOperations();
            customizedQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.IsCustom == true).ToList();
            // iQueryable = customFilter.GetFilteredQuery<LedgerTransaction>(customizedQueryOperation, iQueryable);
            iQueryable = customFilter.GetFilteredQuery(customizedQueryOperation, iQueryable);
            return iQueryable;
        }

        private IQueryable<LedgerTransaction> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<LedgerTransaction> iQueryable, int tenant)
        {
            return iQueryable;
        }

        public List<LedgerTransactionList> GetLedgerTransactionListForceOrderByDateTypeCodeAndId(
            IQueryable<LedgerTransaction> LedgerTransactionQuery, string DateTypeCode, int pageSize, int pageStartAtRecordIndex)
        {
            //var skip = pageSize * curPageZeroBase;
            var skip = pageStartAtRecordIndex;
            var q = LedgerTransactionQuery.Skip(skip).Take(pageSize);
            IQueryable<LedgerTransactionList> ledgerTransactionListQuery = null;
            if (this.context.ToString().StartsWith("Fake"))
            {
                ledgerTransactionListQuery = GetAFakeIqueryableList(q);
            }
            else
            {
                ledgerTransactionListQuery = GetIqueryableList(q);
            }

            switch (DateTypeCode)
            {
                case "2":// GLAccountTotalDateTypeValues.DueDate:
                    {
                        ledgerTransactionListQuery = ledgerTransactionListQuery.OrderBy(rec => rec.DueDate).ThenBy(rec => rec.Id);
                    }
                    break;
                case "3":// GLAccountTotalDateTypeValues.DocumentDate: 
                    {
                        ledgerTransactionListQuery = ledgerTransactionListQuery.OrderBy(rec => rec.DocumentDate).ThenBy(rec => rec.Id);
                    } break;
                case "1":// GLAccountTotalDateTypeValues.Accountingdate:
                default:
                    {
                        ledgerTransactionListQuery = ledgerTransactionListQuery.OrderBy(rec => rec.AccountingDate).ThenBy(rec => rec.Id);
                    }
                    break;
            }
            


            return ledgerTransactionListQuery.ToList();
        }

        private IQueryable<LedgerTransactionList> GetAFakeIqueryableList(IQueryable<LedgerTransaction> iQueryable)
        {
            IQueryable<LedgerTransactionList> query = (from a in iQueryable//.Include("JournalLine").Include("Currency").Include("Journal")
                                                       select new LedgerTransactionList()
                                                       {
                                                           Id = a.Id,
                                                           
                                                           AccountId = a.AccountId,
                                                           AccountingDate = a.AccountingDate,
                                                           
                                                           ControlAccountId = a.ControlAccountId,
                                                           
                                                           CurrencyId = a.CurrencyId,
                                                           DocumentDate = a.DocumentDate,
                                                           DueDate = a.DueDate,
                                                           ExchangeRate = a.ExchangeRate,
                                                           ForeignAmountCredit = a.ForeignAmountCredit,
                                                           ForeignAmountDebit = a.ForeignAmountDebit,
                                                           JournalId = a.JournalId,


                                                           //JournalNumber = a.JournalLine.Journal.JournalNumber,
                                                           //Source = a.JournalLine.Journal.AccountingEntityReference,
                                                           //SourceType = a.JournalLine.Journal.AccountingEntity.EnglishName,
                                                           //CurrencyCode = a.Currency.Code,
                                                           
                                                           //OpenAmountCurrencyCode = a.OpenAmountCurrency.Code,
                                                           
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
                                                           OpenAmountCurrencyId = a.OpenAmountCurrencyId,
                                                           OppositeAccountId = a.OppositeAccountId,
                                                           SearchFields = a.SearchFields,
                                                           
                                                           IsReconciled = a.IsReconciled,
                                                       });
            return query;
        }
        public List<LedgerTransactionList> GetByAccountId(string AccountId, DateTime date, int tenant)
        {
            IQueryable<LedgerTransaction> LedgerTransactionQuery = (from a in context.LedgerTransactions
                                                                    where a.Tenant == tenant && a.AccountId == AccountId && a.AccountingDate.Year >= date.Year
                                                                    select a).OrderByDescending(b => b.AccountingDate).ThenByDescending(b => b.JournalId);

            IQueryable<LedgerTransactionList> LedgerTransactionListQuery = GetIqueryableList(LedgerTransactionQuery);
            var myList = LedgerTransactionListQuery.ToList();
            return myList;
        }

        public List<LedgerTransactionList> GetByAccountId(string AccountId, DateTime fromdate, DateTime todate, int tenant, string currencyId)
        {

            var newToDate = todate.AddDays(1).AddSeconds(-1);
            IQueryable<LedgerTransaction> LedgerTransactionQuery;

            LedgerTransactionQuery = context.LedgerTransactions.Where(a => a.Tenant == tenant && a.AccountId == AccountId && a.AccountingDate >= fromdate && a.AccountingDate <= newToDate);
            if (!string.IsNullOrWhiteSpace(currencyId))
            {
                LedgerTransactionQuery = LedgerTransactionQuery.Where(a => a.CurrencyId == currencyId);
            }
            LedgerTransactionQuery = LedgerTransactionQuery.OrderByDescending(b => b.AccountingDate).ThenBy(b => b.JournalId);
            IQueryable<LedgerTransactionList> LedgerTransactionListQuery = GetIqueryableList(LedgerTransactionQuery);
            var myList = LedgerTransactionListQuery.ToList();
            return myList;
        }

        public List<LedgerTransactionList> GetByAccountId(string AccountId, int tenant)
        {
            IQueryable<LedgerTransaction> LedgerTransactionQuery = (from a in context.LedgerTransactions
                                                                    where a.Tenant == tenant && a.AccountId == AccountId
                                                                    select a);

            IQueryable<LedgerTransactionList> LedgerTransactionListQuery = GetIqueryableList(LedgerTransactionQuery);
            var myList = LedgerTransactionListQuery.ToList();
            return myList;
        }



        public List<LedgerTransactionList> GetOpenByAccountId(string accountId, int tenant)
        {
            IQueryable<LedgerTransaction> ledgerTransactionQuery = (from a in context.LedgerTransactions
                                                                    where a.Tenant == tenant && a.AccountId == accountId && (a.OpenAmount > 0 || a.OpenAmount < 0)
                                                                    select a).OrderBy(b => b.AccountingDate).ThenByDescending(b => b.JournalId);

            IQueryable<LedgerTransactionList> ledgerTransactionListQuery = this.GetIqueryableList(ledgerTransactionQuery);
            var myList = ledgerTransactionListQuery.ToList();
            return myList;
        }


        public bool DoesDraftByAccountIdExist(string accountId, int tenant)
        {
            IQueryable<LedgerTransaction> ledgerTransactionQuery = (from a in context.LedgerTransactions
                                                                    where a.Tenant == tenant && a.AccountId == accountId && a.Mark == true
                                                                    select a);

            IQueryable<LedgerTransactionList> ledgerTransactionListQuery = this.GetIqueryableList(ledgerTransactionQuery);
            int count = ledgerTransactionListQuery.ToList().Count;
            bool rv = (count > 0);
            return rv;
        }

        public List<LedgerTransactionList> GetDtoAsList(List<LedgerTransactionDto> joinWithDto)
        {
            var myIdList = joinWithDto.Select(d => d.Id).ToList();

            IQueryable<LedgerTransaction> ledgerTransactionQuery =
                (from a in context.LedgerTransactions
                 //join dto in joinWithDto
                 //on a.Id equals dto.Id
                 where myIdList.Contains(a.Id)
                 select a).OrderBy(b => b.AccountingDate).ThenByDescending(b => b.JournalId);

            IQueryable<LedgerTransactionList> ledgerTransactionListQuery = this.GetIqueryableList(ledgerTransactionQuery);

            var withGroup = new List<LedgerTransactionList>();
            var myList = ledgerTransactionListQuery.ToList();
            // update group match
            (from a in myList
             join dto in joinWithDto
             on a.Id equals dto.Id
             select new { a, dto })
             .ToList()
             .ForEach(j =>
             {
                 j.a.GroupHash = j.dto.GroupHash;
                 withGroup.Add(j.a);
             });
            ;

            return withGroup.OrderBy(r => r.GroupHash).ThenBy(b => b.AccountingDate).ThenByDescending(b => b.JournalId).ToList();
        }

        public class LedgerTransactionListCustomFilter
        {
            public int Tenant { get; set; }
            public LedgerTransactionListCustomFilter(int tenant)
            {
                this.Tenant = tenant;
            }

            public IQueryable<LedgerTransaction> GetFilteredQuery(QueryOperations operations, IQueryable<LedgerTransaction> queryableData)
            {
                List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

                foreach (QueryFilterItem item in queryFilters)
                {
                    if (item.FieldName == "IsOpen")
                    {
                        queryableData = queryableData.Where(d => d.OpenAmount > 0.00M || d.OpenAmount < 0.00M);
                    }

                    if (item.FieldName == "DecimalOpenAmount")
                    {
                        decimal d = decimal.Parse( item.FieldValue.ToString());
                        if (item.Operator == "GreaterThanOrEqual")
                        {

                            queryableData = queryableData.Where(r => r.OpenAmount >= d);
                        }

                        else if (item.Operator == "LessThanOrEqual")
                        {
                            queryableData = queryableData.Where(r => r.OpenAmount <= d);
                        }
                        else
                        {
                            throw new Exception("in DecimalOpenAmount  ,Only GreaterThanOrEqual Or LessThanOrEqual operators allowed !!  ");
                        }

                    }
                }
                return queryableData;
            }

        }


        public GenericCallBack GetReconciliationFilterCallBack(QueryOperations queryOperations, string AccountId,  int tenant, bool getOpenReconciliations = true)
        {

            IQueryable<LedgerTransactionList> query2 = BasicListFilter(queryOperations, tenant);

            const int MaxTotal = 99001;

            if (getOpenReconciliations == true)
                query2 = OpenReconciliationFilter(AccountId, query2, MaxTotal);
            else
                query2 = ReconciliationFilter(AccountId, query2);

            GenericCallBack myGenericCallBack = GetGenericCallback(query2, MaxTotal);
            return myGenericCallBack;
        }

        public GenericCallBack GetExternalReconciliationFilterCallBack(QueryOperations queryOperations, string AccountId, int tenant)
        {

            IQueryable<LedgerTransactionList> query2 = BasicListFilter(queryOperations, tenant);

            const int MaxTotal = 99001;
            
            query2 = AddFiltersForExternalReconciliations(AccountId, query2, tenant);

            GenericCallBack myGenericCallBack = GetGenericCallback(query2, MaxTotal);
            return myGenericCallBack;
        }

        private static GenericCallBack GetGenericCallback(IQueryable<LedgerTransactionList> query2, int MaxTotal)
        {
            var callback11 =
                            (from r in query2
                             group r by 1 into gb
                             select new
                             {
                                 TotalRecord = gb.Count(),
                                 MaxCreateDate = gb.Max(r => r.CreateDate)//.ToString("yyyy-MM-dd hh:mm:ss")
                             })
                         .FirstOrDefault() ?? new
                         {
                             TotalRecord = 0,
                             MaxCreateDate = DateTime.MinValue
                         }
                         ;

            var myGenericCallBack = new GenericCallBack();
            myGenericCallBack.MaxFieldName = "CreateDate";
            myGenericCallBack.TotalRecord = callback11.TotalRecord;
            myGenericCallBack.MaxValueAsString = callback11.MaxCreateDate
                //.ToString("yyyy-MM-dd HH:mm:ss");
                //.ToString("MM/dd/yyyy hh:mm:ss.fff tt");                        
                //.ToString("g");                        
                .ToString("o");   //                     

            if (callback11.TotalRecord > MaxTotal)
            {
                myGenericCallBack.TotalRecord = MaxTotal;
                myGenericCallBack.IsPartial = true;

            }

            return myGenericCallBack;
        }

        public GenericCallBack GetReconciliationFilterCallBack(QueryOperations queryOperations,
            string AccountId,
            int tenant)
        {
            IQueryable<LedgerTransactionList> query2 = BasicListFilter(queryOperations, tenant);
            const int MaxTotal = 99001;
            query2 = ReconciliationFilter(AccountId, query2);
            var callback11 =
                (from r in query2
                 group r by 1 into gb
                 select new
                 {
                     TotalRecord = gb.Count(),
                     MaxCreateDate = gb.Max(r => r.CreateDate)//.ToString("yyyy-MM-dd hh:mm:ss")
                 })
             .FirstOrDefault() ?? new
             {
                 TotalRecord = 0,
                 MaxCreateDate = DateTime.MinValue
             }
             ;
            var myGenericCallBack = new GenericCallBack();
            myGenericCallBack.MaxFieldName = "CreateDate";
            myGenericCallBack.TotalRecord = callback11.TotalRecord;
            myGenericCallBack.MaxValueAsString = callback11.MaxCreateDate
                //.ToString("yyyy-MM-dd HH:mm:ss");
                //.ToString("MM/dd/yyyy hh:mm:ss.fff tt");                        
                //.ToString("g");                        
                .ToString("o");   //                     

            if (callback11.TotalRecord > MaxTotal)
            {
                myGenericCallBack.TotalRecord = MaxTotal;
                myGenericCallBack.IsPartial = true;

            }
            return myGenericCallBack;
        }

        private static IQueryable<LedgerTransactionList> OpenReconciliationFilter(string AccountId, IQueryable<LedgerTransactionList> query2
            , int MaxTotal
            )
        {
            query2 = query2
                .Where(rec => rec.IsReconciled == false)
                .Where(rec => rec.InReconcileProgress == false)// Seee CreateJournalReconcileService!!!
                .Where(rec => rec.AccountId == AccountId)
                //.OrderBy(rec => rec.AccountingDate)
                //.Take(MaxTotal);
                ;
            return query2;
        }

        private static IQueryable<LedgerTransactionList> ReconciliationFilter(string AccountId, IQueryable<LedgerTransactionList> query2
            )
        {
            query2 = query2
                .Where(rec => rec.AccountId == AccountId)
                .OrderBy(rec => rec.AccountingDate)
                //.Take(MaxTotal);
                ;
            return query2;
        }

        public List<LedgerTransactionList> OpenReconciliationDraft(
            string AccountId,
            int tenant)
            {

                
                IQueryable<LedgerTransaction> LedgerTransactionQuery;

                LedgerTransactionQuery = context.LedgerTransactions.Where(a => a.Tenant == tenant && a.AccountId == AccountId && a.IsReconciled == false && a.Mark==true);
                
                LedgerTransactionQuery = LedgerTransactionQuery.OrderByDescending(b => b.AccountingDate).ThenBy(b => b.JournalId);
                IQueryable<LedgerTransactionList> LedgerTransactionListQuery = GetIqueryableList(LedgerTransactionQuery);
                var myList = LedgerTransactionListQuery.ToList();
                return myList;

            }
        public List<LedgerTransactionList> GetOpenReconciliationFilterList(QueryOperations queryOperations, GenericCallBack callback,
            string AccountId,
            int tenant)
        {
            IQueryable<LedgerTransactionList> query2 = BasicListFilter(queryOperations, tenant);
            const int MaxTotal = 99001;

            query2 = OpenReconciliationFilter(AccountId, query2, MaxTotal);

            LedgerTransactionSorterArgs args = new LedgerTransactionSorterArgs()
            {
                Tenant = tenant,
                AccountId = AccountId,
                QueryOperations = queryOperations,
                Transactions = query2,
            };
            LedgerTransactionsSorter transactionsSorter = new LedgerTransactionsSorter(args);
            query2 = transactionsSorter.SortQuery();

            DateTime maxCreateDate = DateTime.Parse(callback.MaxValueAsString);
            query2 = query2
                .Where(rec => rec.CreateDate <= maxCreateDate)
                .Take(callback.TotalRecord);
            var skipped = (queryOperations.PageIndex - 1);// * queryOperations.PageSize;
            query2 = query2
                .Skip(skipped)
                .Take(queryOperations.PageSize);
            var mylist = query2.ToList();
            return mylist;
        }
        public List<LedgerTransactionList> GetReconciliationFilterList(QueryOperations queryOperations, GenericCallBack callback,
            string AccountId,
            int tenant)
        {
            IQueryable<LedgerTransactionList> query2 = BasicListFilter(queryOperations, tenant);
            const int MaxTotal = 99001;

            query2 = ReconciliationFilter(AccountId, query2);

            DateTime maxCreateDate = DateTime.Parse(callback.MaxValueAsString);
            query2 = query2
                .Where(rec => rec.CreateDate <= maxCreateDate)
                .Take(callback.TotalRecord);
            var skipped = (queryOperations.PageIndex - 1);// * queryOperations.PageSize;
            query2 = query2
                .Skip(skipped)
                .Take(queryOperations.PageSize);

            var mylist = query2.ToList();

            return mylist;
        }

        public List<LedgerTransactionList> GetReconciliationFilterListForTransferGLAccount(QueryOperations queryOperations, GenericCallBack callback, string AccountId, int tenant)
        {
            IQueryable<LedgerTransactionList> query2 = BasicListFilter(queryOperations, tenant);

            query2 = AddFiltersForExternalReconciliations(AccountId, query2, tenant);

            DateTime maxCreateDate = DateTime.Parse(callback.MaxValueAsString);
            query2 = query2
                .Where(rec => rec.CreateDate <= maxCreateDate)
                .Take(callback.TotalRecord);

            var skipped = (queryOperations.PageIndex - 1);// * queryOperations.PageSize;

            query2 = query2
                .Skip(skipped)
                .Take(queryOperations.PageSize);

            var mylist = query2.ToList();

            return mylist;
        }

        private static IQueryable<LedgerTransactionList> AddFiltersForExternalReconciliations(string AccountId, IQueryable<LedgerTransactionList> query2, int tenant)
        {

            DateTime _today = TenantServerConfigration.GetCurrentDateTime(tenant);
            _today = new DateTime(_today.Year, _today.Month, _today.Day, 11, 59, 59);

            query2 = query2
                .Where(rec =>
                rec.AccountId == AccountId
            && (rec.SourceTypeCode == "5" || rec.SourceTypeCode == "9")
            && rec.DueDate <= _today
            && rec.IsExternalReconcile == false
            && Math.Abs(rec.OpenAmount) == Math.Abs(rec.LocalAmountCredit + rec.LocalAmountDebit)
            )
                .OrderBy(rec => rec.AccountingDate);
            return query2;
        }

        private IQueryable<LedgerTransactionList> BasicListFilter(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<LedgerTransaction> iQueryable = (from a in context.LedgerTransactions
                                                        where a.Tenant == tenant
                                                        select a);
            
            iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable, tenant);
            iQueryable = ApplyCustomFilters(queryOperations, iQueryable, tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<LedgerTransaction>(nonListQueryOperation, iQueryable);

            IQueryable<LedgerTransactionList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<LedgerTransactionList>(listQueryOperation, query2);

            query2 = ApplyOrderBy(queryOperations, query2, tenant);


            return query2;
        }

        private IQueryable<LedgerTransactionList> ApplyOrderBy(QueryOperations queryOperations, IQueryable<LedgerTransactionList> query2, int tenant)
        {
            GenericSort sortClass = new GenericSort();
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(LedgerTransactionList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> LedgerTransactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LedgerTransaction", tenant).ToList();

                ObjectField objectField = (from a in LedgerTransactionObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<LedgerTransactionList, string>(queryOperations, query2);
                    }
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                                {
                                    query2 = sortClass.GetSorterQuery<LedgerTransactionList, string>(queryOperations, query2);
                                    break;
                                }
                            case "sigdouble":
                            case "double":
                                {
                                    query2 = sortClass.GetSorterQuery<LedgerTransactionList, double>(queryOperations, query2);
                                    break;
                                }
                            case "date":
                            case "datetime":
                                {
                                    query2 = sortClass.GetSorterQuery<LedgerTransactionList, DateTime>(queryOperations, query2);
                                    break;
                                }
                            case "unsinteger":
                            case "integer":
                                {
                                    query2 = sortClass.GetSorterQuery<LedgerTransactionList, int>(queryOperations, query2);
                                    break;
                                }
                            case "boolean":
                                {
                                    query2 = sortClass.GetSorterQuery<LedgerTransactionList, bool>(queryOperations, query2);
                                    break;
                                }
                            case "unsdecimal":
                            case "decimal":
                                {
                                    query2 = sortClass.GetSorterQuery<LedgerTransactionList, decimal>(queryOperations, query2);
                                    break;
                                }
                            default:
                                {
                                    query2 = query2.OrderBy(d => d.JournalId);
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                query2 = query2.OrderBy(d => d.JournalId);
            }
            return query2;
        }

        public int GetRecoCount(string glAccountId, int tenant)
        {
            //
            //** USAGE: Get count of ledger transactions that is not reconciled **//
            //

            LedgerTransactionRepository repo = new LedgerTransactionRepository(tenant);
            int count = repo.getRecoCount(glAccountId);
            return count;
        }

        public IQueryable<LedgerTransactionList> GetIquerableOpenReconciliationFilterList(QueryOperations queryOperations, string AccountId, int tenant)
        {
            IQueryable<LedgerTransactionList> query2 = BasicListFilter(queryOperations, tenant);

            const int MaxTotal = 99001;
            query2 = OpenReconciliationFilter(AccountId, query2, MaxTotal);
            
            return query2;
        }

        public List<LedgerTransactionList> GetTransactionsByIds(List<string> ids)
        {
            IQueryable<LedgerTransaction> ledgerTransactionQuery = (from a in context.LedgerTransactions
                                                                    where ids.Contains(a.Id)
                                                                    select a).OrderBy(b => b.AccountingDate).ThenByDescending(b => b.JournalId);

            IQueryable<LedgerTransactionList> ledgerTransactionListQuery = GetIqueryableList(ledgerTransactionQuery);
            var myList = ledgerTransactionListQuery.ToList();
            return myList;
        }

        public decimal GetAccountOpenTransactionsTotal(string accountId, int tenant)
        {
            var query = (from a in context.LedgerTransactions
                              where a.AccountId == accountId && a.Tenant == tenant && a.OpenAmount != 0
                              select a);
            var list = query.ToList();
            return list.Count() == 0 ? 0 : query.Sum(d => d.OpenAmount);
        }
        public int GetAccountOpenTransactionsCount(string accountId, int tenant)
        {
            var query = (from a in context.LedgerTransactions
                         where a.AccountId == accountId && a.Tenant == tenant && a.OpenAmount != 0
                         select a);
            var list = query.ToList();
            return list.Count();
        }
        

        public List<LedgerTransactionList> GetARPaymentOpenTransactions(string billToGLAccountId, int tenant)
        {
            IQueryable<LedgerTransaction> ledgerTransactionQuery = (from a in context.LedgerTransactions
                                                                    where a.AccountId == billToGLAccountId && a.Tenant == tenant && a.IsReconciled == false
                                                                    select a).OrderBy(b => b.AccountingDate).ThenByDescending(b => b.JournalId);

            IQueryable<LedgerTransactionList> ledgerTransactionListQuery = GetIqueryableList(ledgerTransactionQuery);

            ledgerTransactionListQuery = (from t in ledgerTransactionListQuery
                                          where t.SourceTypeCode == "2" // 2- ARInvoice
                                          select t);
            return ledgerTransactionListQuery.ToList();
        }
        public List<LedgerTransactionList> GetARPaymentReconciledTransactions(string arpaymentId, string billToGLAccountId, int tenant)
        {
            IQueryable<LedgerTransaction> ledgerTransactionQuery = (from a in context.LedgerTransactions
                                                                    where a.AccountId == billToGLAccountId && a.Tenant == tenant && a.IsReconciled == true
                                                                    select a).OrderBy(b => b.AccountingDate).ThenByDescending(b => b.JournalId);

            IQueryable<LedgerTransactionList> ledgerTransactionListQuery = GetIqueryableList(ledgerTransactionQuery);

            //ledgerTransactionListQuery = ledgerTransactionListQuery.Where(d => d.SourceId == arpaymentId);
            ledgerTransactionListQuery = ledgerTransactionListQuery.Where(d => d.SourceTypeCode == "2");

            List<LedgerTransactionList> list = ledgerTransactionListQuery.ToList();
            list.ForEach(trans =>
            {
                trans.IsReconciled = true;
            });

            return list;
        }

    }

    public class LedgerTransactionDto
    {

        public string Id { get; set; }
        public decimal OpenAmount { get; set; }
        public string OpenAmountCurrencyId { get; set; }
        public decimal OpenAmountABS { get; set; }
        public DateTime DueDate { get; set; }
        public string Reference1 { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime AccountingDate { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public int GroupHash { get; set; }
    }

    public class LedgerTransactionBalanceFilter
    {
        public int Tenant { get; set; }
        public string GLAccountId { get; set; }
        public string CurrencyId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }


        public int PageSize { get; set; }
        public int PageStartAtRecordIndex { get; set; }

        public bool IncludeRelatedCurrenciesAccount { get; set; }

        public bool IncludeChildAccounts { get; set; }

        public string SearchFields { get; set; }

        public LedgerTransactionBalanceFilterCallBack CallBack { get; set; }
        public string DateTypeCode { get; set; }
        public bool CheckHaveAccountingQueued { get; set; }
    }
    public class LedgerTransactionBalanceResponse : LedgerTransactionBalanceFilterCallBack
    {
        public List<string> YearTransferLedgerTransactionIds;

        //[XmlIgnore]
        public List<LedgerTransactionList> MyLedgerTransactionList { get; set; }











        
    }

    public class LedgerTransactionBalanceFilterCallBack : LedgerTransactionBalanceFilterCallBackCanBeNull
    {
        
        public string SearchFields { get; set; }
        //must not null !!!
        public bool OmitAllBalance { get; set; }
        public int? TotalRowCount { get; set; }
        public List<string> AllIdAccounts { get; set; }
        public DateTime? MaxCreateAt { get; set; }
        public List<string> YearTransferLedgerTransactionIds { get; set; }
    }


    public class LedgerTransactionBalanceFilterCallBackCanBeNull
    {
        public bool? HaveAccountingQueued { get; set; }
        public bool? SuppressCumulativeDueMultiCurrencyInPeriod { get; set; }
        public string Have1CurrencyIdInPeriod { get; set; }
        public decimal? StartBalanceLocal { get; set; }
        public decimal? EndBalanceLocal { get; set; }

        //public decimal? StartBalanceForeign { get; set; }
        //public decimal? EndBalanceForeign { get; set; }


        public decimal? OpenBalanceForYearInLocalCurrency { get; set; }
        //public decimal BeginOfYearLocalAmountBalance { get; set; }
        public List<CallBackBalance> StartBalanceForeignList { get; set; }
        public List<CallBackBalance> EndBalanceForeignList { get; set; }


    }
    public class CallBackBalance
	{
	    public string CurrencyId { get; set; }
        public decimal? BalanceForeign { get; set; }
        public decimal? BalanceLocal { get; set; }
    }
    

       

    public class ReconciliationFilter
    {



        //Must

        public const bool DraftsOnFirstRecord = true;
        public const int MaxRecordToVirtualizationShow = 20000;

        //Starndart
        public int VirtualizationPageSize { get; set; }
        public int VirtualizationCurrentZeroPage { get; set; }  //1st Page ==0 


        //Optional
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SearchText { get; set; }


        public GenericCallBack MyReconciliationFilterCallBack { get; set; }







    }
    public class GenericCallBack
    {
        //if (VirtualizationCurrentPage>1) then CallBackMaxCreateDateLedgerTransaction is must !!!
        //public DateTime MaxCreateDateLedgerTransaction { get; set; }

        public string MaxFieldName { get; set; }
        public string MaxValueAsString { get; set; }

        public int TotalRecord { get; set; }

        // =HaveValue(FromDate || ToDate || SearchText ) Or DBCount HaveMore CallBackTotalRecord
        public bool IsPartial { get; set; }
    }

    public class LedgerTransactionCardIndexFilter
    {
        public int Tenant { get; set; }
        public string GLAccountId { get; set; }
        public string CurrencyId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }


        public int PageSize { get; set; }
        public int PageStartAtRecordIndex { get; set; }

        public bool? IsReconciled { get; set; }

        public bool IncludeChildAccounts { get; set; }
        public string Category1Id { get; set; }
        public string Category2Id { get; set; }
        public string Category3Id { get; set; }
        public string Category4Id { get; set; }
        public string Category5Id { get; set; }
        public string AccountTypeCode { get; set; }
        public string ChartOfAccountsId { get; set; }
        public string DateTypeCode { get; set; }

        public string SearchFields { get; set; }

        public LedgerTransactionCardIndexFilterCallBack CallBack { get; set; }



    }

    public class CallBackCardIndex
    {
        public List<LedgerTransactionList> CardIndex { get; set; }
    }

    public class LedgerTransactionCardIndexFilterCallBackCanBeNull
    {
        public bool? HaveAccountingQueued { get; set; }
        public string Have1CurrencyIdInPeriod { get; set; }
    }

    public class LedgerTransactionCardIndexResponse : LedgerTransactionCardIndexFilterCallBack
    {
        public List<string> YearTransferLedgerTransactionIds;

        //[XmlIgnore]
        public List<LedgerTransactionList> MyLedgerTransactionList { get; set; }


    }

    public class LedgerTransactionCardIndexFilterCallBack : LedgerTransactionCardIndexFilterCallBackCanBeNull
    {

        public string SearchFields { get; set; }
        //must not be null!
        public bool OmitAllCardIndex { get; set; }
        public int? TotalRowCount { get; set; }
        public List<string> AllIdAccounts { get; set; }
        public List<CallBackBalance> StartBalanceForeignList { get; set; }
        public List<CallBackBalance> EndBalanceForeignList { get; set; }
        public decimal? StartBalanceLocal { get; set; }
        public decimal? EndBalanceLocal { get; set; }
        public bool? SuppressCumulativeDueMultiCurrencyInPeriod { get; set; }
        public List<string> YearTransferLedgerTransactionIds { get; set; }
        public decimal? OpenBalanceForYearInLocalCurrency { get; set; }

    }

}
