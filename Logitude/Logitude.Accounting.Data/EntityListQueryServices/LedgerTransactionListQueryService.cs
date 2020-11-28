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
            IQueryable<LedgerTransactionList> query = (from a in iQueryable.Include("JournalLine").Include("Account").Include("Currency").Include("Journal")
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
                                                           InProgressExternalReconcile = a.InProgressExternalReconcile,
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
            IQueryable<LedgerTransaction> LedgerTransactionQuery, LedgerTransactionBalanceFilter _Param , bool IsFromExcelGenerator=false)
        {
            //var skip = pageSize * curPageZeroBase;
            var skip = _Param.PageStartAtRecordIndex;
            var q = IsFromExcelGenerator? LedgerTransactionQuery :LedgerTransactionQuery.Skip(skip).Take(_Param.PageSize);
            IQueryable<LedgerTransactionList> ledgerTransactionListQuery = null;
            if (this.context.ToString().StartsWith("Fake"))
            {
                ledgerTransactionListQuery = GetAFakeIqueryableList(q);
            }
            else
            {
                ledgerTransactionListQuery = GetIqueryableList(q);
            }

            switch (_Param.DateTypeCode)
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

            IQueryable<LedgerTransactionList> query2 = GetFilteredList(queryOperations, tenant);

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

            IQueryable<LedgerTransactionList> query2 = GetFilteredList(queryOperations, tenant);

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
            IQueryable<LedgerTransactionList> query2 = GetFilteredList(queryOperations, tenant);
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
                .Where(rec => rec.IsExternalReconcile == false)
                .Where(rec => rec.InReconcileProgress == false)// Seee CreateJournalReconcileService!!!
                .Where(rec => rec.AccountId == AccountId)
                //.OrderBy(rec => rec.AccountingDate)
                //.Take(MaxTotal);
                ;
            return query2;
        }
        private static IQueryable<LedgerTransactionList> FilterOpenTransactionsForExternalReconcile(string AccountId, IQueryable<LedgerTransactionList> query2)
        {
            query2 = query2
                .Where(rec => rec.IsExternalReconcile == false)
                .Where(rec => rec.InProgressExternalReconcile == false)
                .Where(rec => rec.AccountId == AccountId)
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
            IQueryable<LedgerTransactionList> query2 = GetFilteredList(queryOperations, tenant);
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
            query2 = callback.IsFromExcelGenerator? query2: query2
                .Skip(skipped)
                .Take(queryOperations.PageSize);
            var mylist = query2.ToList();
            MapLedgerTransactionnList(mylist, callback.IsFromExcelGenerator);
            return mylist;
        }
 
        private void MapLedgerTransactionnList (List<LedgerTransactionList> LedgerTransactions, bool  IsFromExcelGenerator)
        {
            LedgerTransactionHelper ledgerTransactionHelper = new LedgerTransactionHelper();
            LedgerTransactions.ForEach(rec =>
            {
                rec.OriginalAmount = ledgerTransactionHelper.CalculateOriginalAmount(rec);
                rec.IconCode = ledgerTransactionHelper.getEntityIcon(rec.SourceTypeCode);
                rec.Source =  rec.IconCode+" "+ rec.SourceNumber;
                rec.IsLocalAmountCreditPos = rec.LocalAmountCredit != 0;
                rec.LocalAmountCredit = rec.LocalAmountCredit != 0 ? rec.LocalAmountCredit : rec.LocalAmountDebit;
                rec.IsCumulativeLocalAmountPos = rec.CumulativeLocalAmount < 0;
                rec.IsForeignAmountCreditPos = rec.ForeignAmountCredit != 0;
                rec.ForeignAmountCredit = rec.ForeignAmountCredit != 0 ? rec.ForeignAmountCredit : rec.ForeignAmountDebit;
                rec.IsCumulativeForeignAmountPos = rec.CumulativeForeignAmount < 0;
                rec.IsOriginalAmountPos = rec.OpenAmount < 0;
                rec.IsForeignAmountPos = rec.ForeignAmountCredit != 0;
                if (IsFromExcelGenerator)
                {
                    ledgerTransactionHelper.MapAmountWithNegativeValue(rec);
                }
            });
        }
       
      
        public List<LedgerTransactionList> GetReconciliationFilterList(QueryOperations queryOperations, GenericCallBack callback,
            string AccountId,
            int tenant)
        {
            IQueryable<LedgerTransactionList> query2 = GetFilteredList(queryOperations, tenant);
            const int MaxTotal = 99001;

            query2 = ReconciliationFilter(AccountId, query2);

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

        public List<LedgerTransactionList> GetReconciliationFilterListForTransferGLAccount(QueryOperations queryOperations, GenericCallBack callback, string AccountId, int tenant)
        {
            IQueryable<LedgerTransactionList> query2 = GetFilteredList(queryOperations, tenant);

            query2 = AddFiltersForExternalReconciliations(AccountId, query2, tenant);

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

        private static IQueryable<LedgerTransactionList> AddFiltersForExternalReconciliations(string AccountId, IQueryable<LedgerTransactionList> query2, int tenant)
        {

            DateTime _today = TenantServerConfigration.GetCurrentDateTime(tenant);
            _today = new DateTime(_today.Year, _today.Month, _today.Day, 11, 59, 59);

            query2 = query2
                .Where(rec =>
                rec.AccountId == AccountId
            && (rec.SourceTypeCode == "5" || rec.SourceTypeCode == "9")
            && rec.DueDate < _today
            && rec.IsExternalReconcile == false
            && Math.Abs(rec.OpenAmount) == Math.Abs(rec.LocalAmountCredit + rec.LocalAmountDebit)
            )
                .OrderBy(rec => rec.AccountingDate);
            return query2;
        }

        private IQueryable<LedgerTransactionList> GetFilteredList(QueryOperations queryOperations, int tenant)
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
            IQueryable<LedgerTransactionList> query2 = GetFilteredList(queryOperations, tenant);

            query2 = FilterOpenTransactionsForExternalReconcile(AccountId, query2);
            
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
        public int getRecoCount(string glAccountId, int tenant)
        {
            return (from a in context.LedgerTransactions
                    where
                    a.AccountId == glAccountId
                    && a.InReconcileProgress == false
                    && a.IsReconciled == false
                    && a.Tenant == tenant
                    select a).Count();
        }


        public List<string> GetGLAccountIdList__NotReconciled(GetAllAccountArgs getAllAccountArgs)
        {
            List<string> result = null;
            LedgerTransactionRepository repo = new LedgerTransactionRepository(this.context);

            IQueryable<LedgerTransaction> q = (from ltline in repo.GetAll(getAllAccountArgs.Tenant).Where(rec => rec.Tenant == getAllAccountArgs.Tenant
                        && (rec.OpenAmount > 0.00m || rec.OpenAmount < 0.00m || (rec.OpenAmount == 0m && rec.IsReconciled == false)) && rec.DueDate >= getAllAccountArgs.FromDate
                        && rec.DueDate < getAllAccountArgs.UpToDueDate)
                                               join gLAccounts in (context as AccountingContext).GLAccounts.Where(
                                                   r => r.Tenant == getAllAccountArgs.Tenant && r.AccountTypeCode == getAllAccountArgs.AccountTypeCode)
                                               on ltline.AccountId equals gLAccounts.Id
                                               select ltline); 

            if (q != null)
            {
                result = q.Select(rec => rec.AccountId).Distinct().ToList();
            }

            return result;
        }

        public List<LedgerTransaction> GetLedgerTransactionsByAcc_RunOnPairs(ref GetNextGroupArgs getNextGroupArgs)
        {
            getNextGroupArgs.ActualDifference = 0m;
            string old_id_saved = getNextGroupArgs.OldId;
            IQueryable<LedgerTransaction> query;
            int myMAX = getNextGroupArgs.LT_LinesMaximum; //getNextGroupArgs.LT_LinesMaximum;
            GetNextGroupArgs args = getNextGroupArgs;
            LedgerTransactionRepository repo = new LedgerTransactionRepository(this.context);
            List<LedgerTransaction> q;
            if (getNextGroupArgs.MoveOn)
            {
                query = repo.GetAll(getNextGroupArgs.Tenant).Where(rec => rec.Tenant == args.Tenant && rec.AccountId == args.GLAccountId
                    && (rec.OpenAmount > 0.00m || rec.OpenAmount < 0.00m) && (Math.Abs(rec.OpenAmount) < args.OldAmount || (Math.Abs(rec.OpenAmount) == args.OldAmount
                    && (rec.DueDate > args.OldDate || (rec.DueDate == args.OldDate && String.Compare(rec.Id, args.OldId) > 0))))
                    && rec.DueDate < args.UpToDueDate).OrderByDescending(r0 => Math.Abs(r0.OpenAmount)).ThenBy(r => r.DueDate).ThenBy(r1 => r1.Id);//.Take(myMAX);
            }
            else
            {
                query = repo.GetAll(getNextGroupArgs.Tenant).Where(rec => rec.Tenant == args.Tenant && rec.AccountId == args.GLAccountId
                    && (rec.OpenAmount > 0.00m || rec.OpenAmount < 0.00m)
                    && (rec.DueDate > args.OldDate || (rec.DueDate == args.OldDate && String.Compare(rec.Id, args.OldId) > 0))
                    && rec.DueDate < args.UpToDueDate).OrderByDescending(r0 => Math.Abs(r0.OpenAmount)).ThenBy(r => r.DueDate).ThenBy(r1 => r1.Id);//.Take(myMAX);

            }
            q = query.ToList<LedgerTransaction>();
            string long_text = "";
            q.ForEach(item => long_text += item.OpenAmount.ToString() + "   ");
            List<LedgerTransaction> result = new List<LedgerTransaction>();

            if (q == null || q.Count <= 1)
            {
                if (!getNextGroupArgs.MoveOn)
                //Nothing retrieved. Stop here!
                    getNextGroupArgs.Stop = true;
                else
                {
                    //Return to regular run
                    getNextGroupArgs.RunOnPairs = false;
                    getNextGroupArgs.MoveOn = false;
                    getNextGroupArgs.OldId = "";
                    getNextGroupArgs.OldDate = DateTime.MinValue;
                    getNextGroupArgs.OldAmount = Decimal.MaxValue;
                }
            }
            else
            {
                //getNextGroupArgs.OldDate = q.FirstOrDefault().DueDate;
                //getNextGroupArgs.OldId = q.FirstOrDefault().Id;
                LedgerTransaction[] arr = q.ToArray();
                int count = q.Count;
                decimal sum = 0.00m;
                int j = 0;

                while (j < count - 1)
                {
                    sum = arr[j].OpenAmount + arr[j + 1].OpenAmount;
                    if (((arr[j].OpenAmount > 0 && arr[j + 1].OpenAmount < 0) || (arr[j].OpenAmount < 0 && arr[j + 1].OpenAmount > 0))
                        && (sum == 0.00m || Math.Abs(sum) <= getNextGroupArgs.MaximalDifference))
                    {
                        result.Add(arr[j]);
                        result.Add(arr[j + 1]);
                        getNextGroupArgs.OldAmount = Math.Abs(arr[j + 1].OpenAmount);
                        getNextGroupArgs.OldDate = arr[j + 1].DueDate;
                        getNextGroupArgs.OldId = arr[j + 1].Id;
                        getNextGroupArgs.ActualDifference = sum;
                        break;
                    }
                    else 
                    {
                        j++;
                    }
                }

                if (result.Count == 0)
                {
                    getNextGroupArgs.RunOnPairs = false;
                    getNextGroupArgs.MoveOn = false;
                    getNextGroupArgs.OldId = "";
                    getNextGroupArgs.OldDate = DateTime.MinValue;
                    getNextGroupArgs.OldAmount = Decimal.MaxValue;
                }
                
            }

            return result;
        }



        public List<LedgerTransaction> GetLedgerTransactionsByAcc_NotReconciled(ref GetNextGroupArgs getNextGroupArgs)
        {
            getNextGroupArgs.ActualDifference = 0m;
            string old_id_saved = getNextGroupArgs.OldId;
            IQueryable<LedgerTransaction> query;
            int myMAX = getNextGroupArgs.MaxPageSize; //getNextGroupArgs.LT_LinesMaximum;
            GetNextGroupArgs args = getNextGroupArgs;
            LedgerTransactionRepository repo = new LedgerTransactionRepository(this.context);
            List<LedgerTransaction> q;
            if (getNextGroupArgs.MoveOn)
            {
                query = repo.GetAll(getNextGroupArgs.Tenant).Where(rec => rec.Tenant == args.Tenant && rec.AccountId == args.GLAccountId
                    && (rec.OpenAmount > 0.00m || rec.OpenAmount < 0.00m || (rec.OpenAmount == 0m && rec.IsReconciled == false))
                    && (rec.DueDate > args.OldDate || (rec.DueDate == args.OldDate && String.Compare(rec.Id, args.OldId) > 0))
                    && rec.DueDate < args.UpToDueDate).OrderBy(r => r.DueDate).ThenBy(r1 => r1.Id).Take(myMAX);//.ToList();
            }
            else if (getNextGroupArgs.RunAgain)
            {
               // myMAX = getNextGroupArgs.LT_LinesMaximum;// was * 2;
                query = repo.GetAll(getNextGroupArgs.Tenant).Where(rec => rec.Tenant == args.Tenant && rec.AccountId == args.GLAccountId
                    && (rec.OpenAmount > 0.00m || rec.OpenAmount < 0.00m || (rec.OpenAmount == 0m && rec.IsReconciled == false))
                    && (rec.DueDate > args.OldDate || (rec.DueDate == args.OldDate && String.Compare(rec.Id, args.OldId) >= 0))
                    && rec.DueDate < args.UpToDueDate).OrderBy(r => r.DueDate).ThenBy(r1 => r1.Id).Take(myMAX);//.ToList();

            }
            else
            {
                query = repo.GetAll(getNextGroupArgs.Tenant).Where(rec => rec.Tenant == args.Tenant && rec.AccountId == args.GLAccountId
                    && (rec.OpenAmount > 0.00m || rec.OpenAmount < 0.00m || (rec.OpenAmount == 0m && rec.IsReconciled == false))
                    && (rec.DueDate > args.OldDate || (rec.DueDate == args.OldDate && String.Compare(rec.Id, args.OldId) > 0))
                    && rec.DueDate < args.UpToDueDate).OrderBy(r => r.DueDate).ThenBy(r1 => r1.Id).Take(myMAX);//.ToList();

            }
            q = query.ToList<LedgerTransaction>();
            string long_text = "";
            int ctr = 1;
            q.ForEach(item => long_text += "#" + ctr++ +  ","+ item.DueDate.ToString("dd.MM.yyyy") + ","  + item.Id + "," + item.OpenAmount.ToString() + "\n");
            List<LedgerTransaction> result = new List<LedgerTransaction>();
            bool next_set = false;

            if (q == null || q.Count <= 1)
            {
                //Nothing retrieved. Stop here!
                getNextGroupArgs.Stop = true;
            }
            else
            {
                getNextGroupArgs.OldDate = q.FirstOrDefault().DueDate;
                getNextGroupArgs.OldId = q.FirstOrDefault().Id;
                List<LedgerTransaction> q1 = q;//.OrderByDescending(rec => Math.Abs(rec.OpenAmount)).ToList();
               // LedgerTransaction[] arr = q1.ToArray();
                int count = q1.Count;
                int lineCount = 0;
                int positiveCount = 0;
                int negativeCount = 0;
                decimal maxPositive = 0m;
                decimal maxNegative = 0m;
                int maxPositiveIndex = -1;
                int maxNegativeIndex = -1;
                decimal sum = 0.00m;
                int firstOpposite = 0;
                int lastOpposite = 0;

                int j = 0;
                List<int> goodList = new List<int>();
                // init = sum getNexrGroupArgs.MIN first elements
                bool cont = true;
                while (cont)
                {
                    cont = false;
                    firstOpposite = 0;
                    lastOpposite = 0;
                    maxPositiveIndex = -1;
                    maxNegativeIndex = -1;

                    while (j < getNextGroupArgs.MaxPageSize && j < count) //getNextGroupArgs.LT_LinesMaximum && j < count)
                    {
                        sum += q1.ElementAt(j).OpenAmount; //arr[j].OpenAmount;
                        lineCount++;
                        if (q1.ElementAt(j).OpenAmount > 0m)
                        {
                            positiveCount++;
                            if (q1.ElementAt(j).OpenAmount > maxPositive)
                            {
                                maxPositive = q1.ElementAt(j).OpenAmount;
                                maxPositiveIndex = j;
                            }
                        }
                        if (q1.ElementAt(j).OpenAmount < 0m)
                        {
                            negativeCount++;
                            if (q1.ElementAt(j).OpenAmount < maxNegative)
                            { 
                                maxNegative = q1.ElementAt(j).OpenAmount;
                                maxNegativeIndex = j;
                            }

                        }
                        if (q1.ElementAt(0).OpenAmount > 0m)
                        {
                            if (j > 0 && q1.ElementAt(j).OpenAmount < 0m && firstOpposite == 0)
                                firstOpposite = j;
                            if (j > 0 && q1.ElementAt(j).OpenAmount < 0m)
                                lastOpposite = j;
                        }
                        if (q1.ElementAt(0).OpenAmount < 0m)
                        {
                            if (j > 0 && q1.ElementAt(j).OpenAmount > 0m && firstOpposite == 0)
                                firstOpposite = j;
                            if (j > 0 && q1.ElementAt(j).OpenAmount > 0m)
                                lastOpposite = j;
                        }
                        if (lineCount > 1 && ((positiveCount > 0 && negativeCount > 0) || (positiveCount == 0 && negativeCount == 0)))
                        {
                            if ((sum > 0m && sum < maxPositive && !(maxPositiveIndex >= 0 && (firstOpposite - maxPositiveIndex > 1)))
                                || (sum < 0m && sum > maxNegative && !(maxNegativeIndex >= 0 && (firstOpposite - maxNegativeIndex > 1))) 
                                || (sum == 0m))
                            {
                                if (j + 1 >= getNextGroupArgs.LT_LinesMaximum)
                                    break; // while j
                                goodList.Add(j);
                            }
                        }

                        j++;
                    }
                    //   if (goodList.Count == 0 && fistOpposite > 1) // because fistOpposite>0 would be too tight 
                    if (goodList.Count == 0 && lastOpposite > 1) // because fistOpposite>0 would be too tight 
                    {
                        //   for (int i = fistOpposite - 1; i > 0; i--)
                        for (int i = lastOpposite - 1; i > 0; i--)
                        {
                            if (q1.ElementAt(i).OpenAmount != 0m && ((q1.ElementAt(0).OpenAmount > 0m && q1.ElementAt(i).OpenAmount > 0m) || (q1.ElementAt(0).OpenAmount < 0m && q1.ElementAt(i).OpenAmount < 0m)))
                            {
                                q1.RemoveAt(i);
                                count = q1.Count;
                                lineCount = 0;
                                positiveCount = 0;
                                negativeCount = 0;
                                maxPositive = 0m;
                                maxNegative = 0m;
                                sum = 0.00m;
                                j = 0;

                                cont = true;
                                break; // for i
                            }
                        }
                    }
                }

                // New Portion 1.
                if (goodList.Count > 0)
                {
                    sum = 0m;
                    j = goodList.Max();
                    // Exit
                    for (int k = 0; k <= j; k++)
                    {
                        if (k < count)
                        {
                            result.Add(q1.ElementAt(k));
                            sum += q1.ElementAt(k).OpenAmount;
                        }

                    }
                    // now, we took some records from the very beginning.
                    // next time we will try from the same point
                    //getNextGroupArgs.OldDate = q.FirstOrDefault().DueDate;
                    //getNextGroupArgs.OldId = q.FirstOrDefault().Id;
                    getNextGroupArgs.ActualDifference = sum;
                    next_set = true;
                }




                if (!next_set)
                {
                    if (result.Count == 0)
                    {
                        //if (getNextGroupArgs.RunAgain)
                        //{
                        //    int half_way = (q.Count / 2) - 1; // Attention! integer division's there
                        //    getNextGroupArgs.OldDate = q.ElementAtOrDefault(half_way).DueDate;
                        //    getNextGroupArgs.OldId = q.ElementAtOrDefault(half_way).Id;
                        //}
                        //else

                        //{
                            if (q.Count > 1 && q.ElementAt(0).Id == old_id_saved)
                            {
                                getNextGroupArgs.OldDate = q.ElementAt(1).DueDate;
                                getNextGroupArgs.OldId = q.ElementAt(1).Id;
                            }
                            else if (getNextGroupArgs.MoveOn && q.Count > 0)
                            {
                                getNextGroupArgs.OldDate = q.ElementAt(0).DueDate;
                                getNextGroupArgs.OldId = q.ElementAt(0).Id;
                            }

                        //}
                    }

                    else
                    {
                        getNextGroupArgs.OldDate = q.FirstOrDefault().DueDate;
                        getNextGroupArgs.OldId = q.FirstOrDefault().Id;
                    }
                }
            }
            string long_text_res = "";
            result.ForEach(item => long_text_res += item.DueDate.ToString("dd.MM.yyyy") + " : " + item.OpenAmount.ToString() + "   ");

            return result;
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

        public List<LedgerTransactionList> GetOpenLedgerTransactions(QueryOperations queryOperations, string accountId, string transferAccountId, int tenant)
        {

            IQueryable<LedgerTransactionList> resultedList = GetLedgerTransactions(queryOperations, tenant, accountId, transferAccountId);

            LedgerTransactionSorterArgs args = new LedgerTransactionSorterArgs()
            {
                Tenant = tenant,
                AccountId = accountId,
                QueryOperations = queryOperations,
                Transactions = resultedList,
            };
            LedgerTransactionsSorter transactionsSorter = new LedgerTransactionsSorter(args);
            resultedList = transactionsSorter.SortQuery();


            //resultedList = FilterMaxDate(callback, resultedList);

            var skippedPages = (queryOperations.PageIndex - 1);// * queryOperations.PageSize;
            resultedList = resultedList
                .Skip(skippedPages)
                .Take(queryOperations.PageSize);

            var mylist = resultedList.ToList();


            return mylist;
        }

        public int GetOpenLedgerTransactionsCount(QueryOperations queryOperations, string accountId, string transferAccountId, int tenant)
        {

            IQueryable<LedgerTransactionList> resultedList = GetLedgerTransactions(queryOperations, tenant, accountId, transferAccountId);


            return resultedList.Count();
        }

        private IQueryable<LedgerTransactionList> GetLedgerTransactions(QueryOperations queryOperations, int tenant, string accountId, string transferAccountId)
        {
            IQueryable<LedgerTransactionList> ledgerTransactions = GetFilteredList(queryOperations, tenant);

            IQueryable<LedgerTransactionList> accountOpenTransaction = GetTransactionsForNormalAccount(tenant, accountId, ledgerTransactions);
            IQueryable<LedgerTransactionList> transferAccountOpenTransaction = GetTransactionsForTransferAccount(tenant, transferAccountId, ledgerTransactions);

            IQueryable<LedgerTransactionList> resultedList = accountOpenTransaction.Union(transferAccountOpenTransaction).OrderByDescending(d=>d.DocumentDate);
            return resultedList;
        }

        private IQueryable<LedgerTransactionList> FilterMaxDate(GenericCallBack callback, IQueryable<LedgerTransactionList> resultedList)
        {
            DateTime maxCreateDate = DateTime.Parse(callback.MaxValueAsString);

            resultedList = resultedList
                .Where(rec => rec.CreateDate <= maxCreateDate)
                .Take(callback.TotalRecord);
            return resultedList;
        }

        private IQueryable<LedgerTransactionList> GetTransactionsForTransferAccount(int tenant, string transferAccountId, IQueryable<LedgerTransactionList> transactions)
        {
            DateTime today = GetCurrentDate(tenant);
            return from a in transactions
                   where a.Tenant == tenant
                   && a.AccountId == transferAccountId
                    && (a.SourceTypeCode == "5" || a.SourceTypeCode == "9")
                  && a.DueDate < today
                  && a.IsExternalReconcile == false
                  && Math.Abs(a.OpenAmount) == Math.Abs(a.LocalAmountCredit + a.LocalAmountDebit)
                   select a;
        }

        private static IQueryable<LedgerTransactionList> GetTransactionsForNormalAccount(int tenant, string accountId, IQueryable<LedgerTransactionList> transactions)
        {
            return from a in transactions
                   where a.Tenant == tenant
                   && a.AccountId == accountId
                   select a;
        }

        private static DateTime GetCurrentDate(int tenant)
        {
            DateTime _today = TenantServerConfigration.GetCurrentDateTime(tenant);
            _today = new DateTime(_today.Year, _today.Month, _today.Day, 11, 59, 59);
            return _today;
        }


        public IQueryable<LedgerTransactionList> GetExternalTransactionsForAccount(string accountId, int tenant)
        {
            DateTime today = GetCurrentDate(tenant);

            IQueryable<LedgerTransaction> ledgerTransactionQuery = (from trans in context.LedgerTransactions
                                                                    join jrn in context.Journals on trans.JournalId equals jrn.Id
                                                                    where 
                                                                        jrn.ExternalSystem != null
                                                                    &&  trans.AccountId == accountId 
                                                                    &&  trans.Tenant == tenant 
                                                                    && trans.DueDate > today
                                                                    && trans.LocalAmountCredit != 0
                                                                    select trans);

            IQueryable<LedgerTransactionList> ledgerTransactionListQuery = GetIqueryableList(ledgerTransactionQuery);

            return ledgerTransactionListQuery;
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
        public string GLAccountId { get; set; }

        public long TookMS { get; set; }
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

        public bool IsFromExcelGenerator { get; set; }
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

    public class GetNextGroupArgs
    {
        public int Tenant { get; set; }
        public string GLAccountId { get; set; }
        public int MIN { get; set; }
        public decimal OldAmount { get; set; }
        public DateTime OldDate { get; set; }
        public string OldId { get; set; }
        public DateTime FromDate { get; set; }
        public string FromId { get; set; }
        public DateTime UpToDueDate { get; set; }
        public string ToId { get; set; }
        public bool RunOnPairs { get; set; }
        public bool RunAgain { get; set; }
        public bool MoveOn { get; set; }
        public bool Stop { get; set; }
        public int LT_LinesMaximum { get; set; }
        public int MaxPageSize { get; set; }
        public decimal MaximalDifference { get; set; }
        public decimal ActualDifference { get; set; }
    }

    public class GetAllAccountArgs
    {
        public int Tenant { get; set; }
        public string AccountTypeCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime UpToDueDate { get; set; }
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
