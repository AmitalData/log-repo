using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Logitude.Accounting.BL.CoreBL.Reconcile
{
    public class LedgerTransactionsSorter
    {
        private LedgerTransactionSorterArgs serviceArgs;
        public LedgerTransactionsSorter(LedgerTransactionSorterArgs _serviceArgs)
        {
            serviceArgs = _serviceArgs;
        }

        public List<LedgerTransactionList> SortQuery()
        {
            if (serviceArgs.SortingColumn != null)
            {
                if (serviceArgs.SortingColumn.Name == "OriginalAmount")
                    return GetSortedByOriginalAmountQuery();
                else
                    return GetSortedQuery();
            }
            else return GetDefaultSortedQuery();
        }

        private List<LedgerTransactionList> GetDefaultSortedQuery() => serviceArgs.Transactions.OrderByDescending(d => d.DocumentDate).ToList();

        private List<LedgerTransactionList> GetSortedQuery()
        {
            IQueryable<LedgerTransactionList> reconciliationsIQuerable = serviceArgs.Transactions.AsQueryable();

            GenericSort sortClass = new GenericSort();

            ObjectField objectField = GetSortByObjectField();



            if (objectField != null)
            {
                switch (objectField.DataTypeCode.ToLower())
                {
                    case "ntext":
                    case "text":
                        {
                            reconciliationsIQuerable = sortClass.GetSorterQuery<LedgerTransactionList, string>(serviceArgs.QueryOperations, reconciliationsIQuerable);
                            break;
                        }
                    case "decimal":
                    case "double":
                        {
                            reconciliationsIQuerable = sortClass.GetSorterQuery<LedgerTransactionList, double>(serviceArgs.QueryOperations, reconciliationsIQuerable);
                            break;
                        }
                    case "date":
                    case "datetime":
                        {
                            reconciliationsIQuerable = sortClass.GetSorterQuery<LedgerTransactionList, DateTime>(serviceArgs.QueryOperations, reconciliationsIQuerable);
                            break;
                        }
                    case "integer":
                        {
                            reconciliationsIQuerable = sortClass.GetSorterQuery<LedgerTransactionList, int>(serviceArgs.QueryOperations, reconciliationsIQuerable);
                            break;
                        }
                    case "boolean":
                        {
                            reconciliationsIQuerable = sortClass.GetSorterQuery<LedgerTransactionList, bool>(serviceArgs.QueryOperations, reconciliationsIQuerable);
                            break;
                        }
                    default:
                        {
                            reconciliationsIQuerable = reconciliationsIQuerable.OrderByDescending(d => d.DocumentDate);
                            break;
                        }
                }
            }

            return reconciliationsIQuerable.ToList();
        }

        private List<LedgerTransactionList> GetSortedByOriginalAmountQuery()
        {
            IQueryable<LedgerTransactionList> reconciliationsIQuerable = serviceArgs.Transactions.AsQueryable();

            GLAccountPM glAccount = GetGLAccount();

            if (glAccount.ReconcileMethodCode == ReconcileMethodValues.LocalCurrency)
                reconciliationsIQuerable = SortByLocalOriginalAmount(reconciliationsIQuerable);
            else if (glAccount.ReconcileMethodCode == ReconcileMethodValues.ForeignCurrency)
                reconciliationsIQuerable = SortByForeignOriginalAmount(reconciliationsIQuerable);

            return reconciliationsIQuerable.ToList();
        }

        private IQueryable<LedgerTransactionList> SortByForeignOriginalAmount(IQueryable<LedgerTransactionList> openReconciliationsIquerable)
        {
            if (serviceArgs.QueryOperations.SortDirectin.ToLower() == "ascending")
                openReconciliationsIquerable = openReconciliationsIquerable.OrderBy(d => d.ForeignAmountCredit == 0 ? d.ForeignAmountDebit : d.ForeignAmountCredit);
            else
                openReconciliationsIquerable = openReconciliationsIquerable.OrderByDescending(d => d.ForeignAmountCredit == 0 ? d.ForeignAmountDebit : d.ForeignAmountCredit);
            return openReconciliationsIquerable;
        }

        private IQueryable<LedgerTransactionList> SortByLocalOriginalAmount(IQueryable<LedgerTransactionList> openReconciliationsIquerable)
        {
            if (serviceArgs.QueryOperations.SortDirectin.ToLower() == "ascending")
                openReconciliationsIquerable = openReconciliationsIquerable.OrderBy(d => d.LocalAmountCredit == 0 ? d.LocalAmountDebit : d.LocalAmountCredit);
            else
                openReconciliationsIquerable = openReconciliationsIquerable.OrderByDescending(d => d.LocalAmountCredit == 0 ? d.LocalAmountDebit : d.LocalAmountCredit);
            return openReconciliationsIquerable;
        }

        private GLAccountPM GetGLAccount()
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(serviceArgs.Tenant);
            GLAccountPM glaccount = gLAccountQueryService.GetSingle(serviceArgs.AccountId, false, false);
            return glaccount;
        }
        private ObjectField GetSortByObjectField()
        {
            PropertyInfo propInfo = typeof(LedgerTransactionList).GetProperty(serviceArgs.QueryOperations.SortByColumnName);
            List<ObjectField> transactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LedgerTransaction", serviceArgs.Tenant).ToList();

            ObjectField objectField = (from a in transactionObjectFields
                                       where a.FieldName == serviceArgs.QueryOperations.SortByColumnName
                                       select a).FirstOrDefault();
            return objectField;
        }

    }

    public class LedgerTransactionSorterArgs
    {
        public QueryOperations QueryOperations { get; set; }
        public List<LedgerTransactionList> Transactions { get; set; }
        public string AccountId { get; set; }
        public int Tenant { get; set; }
        public SortingColumnArgs SortingColumn
        {
            get
            {
                if (!string.IsNullOrEmpty(QueryOperations.SortByColumnName) && !string.IsNullOrEmpty(QueryOperations.SortDirectin))
                    return new SortingColumnArgs() { Name = QueryOperations.SortByColumnName, Direction = QueryOperations.SortDirectin };
                else
                    return null;
            }
        }
        
    }
    public class SortingColumnArgs
    {
        public string Name { get; set; }
        public string Direction { get; set; }
    }
}
