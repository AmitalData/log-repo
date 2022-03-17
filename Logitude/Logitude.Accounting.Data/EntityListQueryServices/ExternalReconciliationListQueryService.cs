	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;
using System.Data.Entity;

namespace Logitude.Accounting.Data.EntityListQueryServices
{

    public partial class ExternalReconciliationListQueryService
    {
        private IQueryable<ExternalReconciliationList> GetIqueryableList(IQueryable<ExternalReconciliation> iQueryable)
        {
            IQueryable<ExternalReconciliationList> query = (from a in iQueryable
                                                            select new ExternalReconciliationList()
                                                            {
                                                                Id = a.Id,
                                                                GLAccountId = a.GLAccountId,
                                                                CreatedByUserId = a.CreatedByUserId,
                                                                ReconciliationNumber = a.ReconciliationNumber,
                                                                CreateDate = a.CreateDate,
                                                                Tenant = a.Tenant,
                                                                SearchFields = a.SearchFields,
                                                                CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.LocalName : null,
                                                                AccountNumber = a.Account != null ? a.Account.DisplayNumber : null,
                                                                AccountName = a.Account != null ? a.Account.EnglishName : null,
                                                                AccountLocalName = a.Account != null ? a.Account.LocalName : null,
                                                                IsCancelled = a.IsCancelled,
                                                                CrossYearReconcile = a.CrossYearReconcile,

                                                            });
            return query;
        }

        private IQueryable<ExternalReconciliation> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ExternalReconciliation> iQueryable, int tenant)
        {
            var reconciliationLinesQueryOperations = GetReconciliationAmountFieldOperations(queryOperations);
            if (reconciliationLinesQueryOperations == null)
                return iQueryable;
            var reconcileExternalPageLineQuery = CreateReconcileExternalPageLineQuery(reconciliationLinesQueryOperations, tenant);
            var ledgerTransactionQuery = CreateLedgerTransactionQuery(reconciliationLinesQueryOperations, tenant);
            var externalReconciliationLines = context.ExternalReconciliationLines.AsQueryable();
            iQueryable = iQueryable
                .Where(l =>
                    externalReconciliationLines
                        .Where(p =>
                            l.Id == p.ReconciliationId
                            && 
                            (
                            reconcileExternalPageLineQuery.Where(pl => pl.Id == p.ExternalPageLineId).Any()
                            ||
                            ledgerTransactionQuery.Where(lt => lt.Id == p.LedgerTransactionId).Any()
                            )
                        ).Any()
                );

            return iQueryable;
        }

        private IQueryable<ReconcileExternalPageLineList> CreateReconcileExternalPageLineQuery(QueryFilterItem reconciliationLinesQueryOperations, int tenant)
        {
            var fieldName = "Amount";
            GenericFilter filter = new GenericFilter();

            var externalReconciliationQuery = GetIQueryableReconcileExternalPageLineList(tenant);
            var queryOperations = CreateReconciliationLinesQueryOperations(reconciliationLinesQueryOperations, fieldName);
            externalReconciliationQuery = filter.GetFilteredQuery(queryOperations, externalReconciliationQuery);
            return externalReconciliationQuery;
        }

        private IQueryable<ReconcileExternalPageLineList> GetIQueryableReconcileExternalPageLineList(int tenant)
        {
            return context.ReconcileExternalPageLines.Where(e => e.Tenant == tenant).Select(a => new ReconcileExternalPageLineList()
            {
                LineNumber = a.LineNumber,
                Tenant = a.Tenant,

                ReferenceDate = a.ReferenceDate,
                Reference = a.Reference,
                IsReconciled = a.IsReconciled,
                SearchFields = a.SearchFields,
                CreditAmount = a.CreditAmount,
                DebitAmount = a.DebitAmount,
                Amount = a.CreditAmount != 0 ? Math.Abs( a.CreditAmount ):Math.Abs( a.DebitAmount),
                Notes = a.Notes,
                ReconcileExternalPageId = a.ReconcileExternalPageId,
                Id = a.Id,
                InProgressExternalReconcile = a.InProgressExternalReconcile,
                ReconcileRemarks = a.ReconcileRemarks,
                InReconcileProgress = a.InReconcileProgress,

            });
        }

        private IQueryable<LedgerTransactionList> CreateLedgerTransactionQuery(QueryFilterItem reconciliationLinesQueryOperations, int tenant)
        {
            var fieldName = "CalculatedLocalAmount";
            GenericFilter filter = new GenericFilter();
            var externalReconciliationQuery = GetIQueryableLedgerTransactionList(tenant);
            var queryOperations = CreateReconciliationLinesQueryOperations(reconciliationLinesQueryOperations, fieldName);
            externalReconciliationQuery = filter.GetFilteredQuery(queryOperations, externalReconciliationQuery);
            return externalReconciliationQuery;
        }

        private IQueryable<LedgerTransactionList> GetIQueryableLedgerTransactionList(int tenant)
        {
            return context.LedgerTransactions.Where(e => e.Tenant == tenant).Select(a=>
            new LedgerTransactionList()
            {
                Id = a.Id,
               
                ForeignAmountCredit = a.ForeignAmountCredit,
                ForeignAmountDebit = a.ForeignAmountDebit,
                ForeignAmount = a.ForeignAmountDebit == 0 ? Math.Abs( a.ForeignAmountCredit) : Math.Abs(a.ForeignAmountDebit),

                JournalLineNumber = a.JournalLineNumber,
                LocalAmountCredit = a.LocalAmountCredit,
                LocalAmountDebit = a.LocalAmountDebit,
                CalculatedForeignAmount = a.ForeignAmountCredit != 0 ? a.ForeignAmountCredit : a.ForeignAmountDebit,
                CalculatedLocalAmount = a.LocalAmountCredit != 0 ? a.LocalAmountCredit : a.LocalAmountDebit,

            }
            );
        }

        private QueryFilterItem GetReconciliationAmountFieldOperations(QueryOperations queryOperations)
        {
            var filterFieldName = "ReconciliationAmount";
            var reconciliationAmountFieldOperations = queryOperations.QueryFilterItems.Where(e => e.FieldName == filterFieldName).FirstOrDefault();
            if (reconciliationAmountFieldOperations == null)
                return null;
            return reconciliationAmountFieldOperations;
        }

        private QueryOperations CreateReconciliationLinesQueryOperations(QueryFilterItem reconciliationAmountFieldOperations, string fieldName)
        {
            return new QueryOperations()
            {
                GetAll = true,
                QueryFilterItems = new List<QueryFilterItem>()
                {
                    new QueryFilterItem()
                    {
                        FieldName = fieldName,
                        FieldValue = decimal.Parse(reconciliationAmountFieldOperations.FieldValue.ToString()),
                        FieldValue2 = reconciliationAmountFieldOperations.FieldValue2 != null ?
                        decimal.Parse(reconciliationAmountFieldOperations.FieldValue2.ToString()) : reconciliationAmountFieldOperations.FieldValue2,
                        Operator = reconciliationAmountFieldOperations.Operator
                    }
                }
            };
        }
        private IQueryable<ExternalReconciliation> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ExternalReconciliation> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
	