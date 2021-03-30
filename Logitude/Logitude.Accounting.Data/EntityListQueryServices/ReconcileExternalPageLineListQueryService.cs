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
using System.Linq.Expressions;
using Logitude.Accounting.Data.Repositories;

namespace Logitude.Accounting.Data.EntityListQueryServices
{

    public partial class ReconcileExternalPageLineListQueryService
    {
        public IQueryable<ReconcileExternalPageLineList> GetIqueryableList(IQueryable<ReconcileExternalPageLine> iQueryable)
        {
            IQueryable<ReconcileExternalPageLineList> query = (from a in iQueryable
                                                               select new ReconcileExternalPageLineList()
                                                               {

                                                                   LineNumber = a.LineNumber,

                                                                   Tenant = a.Tenant,

                                                                   ReferenceDate = a.ReferenceDate,

                                                                   Reference = a.Reference,

                                                                   IsReconciled = a.IsReconciled,

                                                                   SearchFields = a.SearchFields,

                                                                   CreditAmount = a.CreditAmount,
                                                                   DebitAmount = a.DebitAmount,
                                                                   Amount = a.CreditAmount != 0 ? a.CreditAmount : a.DebitAmount,

                                                                   Notes = a.Notes,

                                                                   ReconcileExternalPageId = a.ReconcileExternalPageId,

                                                                   Id = a.Id,


                                                                   InProgressExternalReconcile = a.InProgressExternalReconcile,
                                                                   ReconcileRemarks = a.ReconcileRemarks,
                                                                   InReconcileProgress = a.InReconcileProgress,
                                                                   
                                                               });
            return query;
        }

        public List<ReconcileExternalPageLineList> GetPageLinesByIds(List<string> ids)
        {
            IQueryable<ReconcileExternalPageLine> pageLineQuery = (from a in context.ReconcileExternalPageLines
                                                                    where ids.Contains(a.Id)
                                                                    select a);

            IQueryable<ReconcileExternalPageLineList> pageLineListQuery = GetIqueryableList(pageLineQuery);
            var myList = pageLineListQuery.ToList();
            return myList;
        }
        public List<ReconcileExternalPageLine> GetPageLines(string pageId, int tenant)
        {
            ReconcileExternalPageLineRepository pageLineRepository = new ReconcileExternalPageLineRepository(tenant);
            List<ReconcileExternalPageLine> pageLineQuery = pageLineRepository.GetPageLines(pageId, tenant);
            return pageLineQuery;
        }
        public IQueryable<ReconcileExternalPageLine> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ReconcileExternalPageLine> iQueryable, int tenant)
        {
            QueryFilterItem amountFilter = queryOperations.QueryFilterItems.Where(d=>d.FieldName == "Amount2Filter").FirstOrDefault();


            if (amountFilter != null)
            {
                decimal amount;
                decimal amount2;
                decimal.TryParse(amountFilter.FieldValue.ToString(), out amount);
                decimal.TryParse(amountFilter.FieldValue2?.ToString(), out amount2);

                switch (amountFilter.Operator)
                {
                    case "LargerThan":
                        {
                            iQueryable = iQueryable.Where(pageLine =>
                            amount < (pageLine.CreditAmount != 0 ? pageLine.CreditAmount : pageLine.DebitAmount)
                            ||
                            -1*amount > (pageLine.CreditAmount != 0 ? pageLine.CreditAmount : pageLine.DebitAmount));
                            break;
                        }

                    case "GreaterThanOrEqual":
                        {
                            iQueryable = iQueryable.Where(pageLine =>
                            amount <= (pageLine.CreditAmount != 0 ? pageLine.CreditAmount : pageLine.DebitAmount)
                            ||
                            -1*amount >= (pageLine.CreditAmount != 0 ? pageLine.CreditAmount : pageLine.DebitAmount));
                            break;
                        }

                    case "LessThan":
                        {
                            iQueryable = iQueryable.Where(pageLine => 
                            (amount > (pageLine.CreditAmount != 0 ? pageLine.CreditAmount : pageLine.DebitAmount)));
                            break;
                        }

                    case "LessThanOrEqual":
                        {
                            iQueryable = iQueryable.Where(pageLine => 
                            (amount >= (pageLine.CreditAmount != 0 ? pageLine.CreditAmount : pageLine.DebitAmount)));
                            break;
                        }

                    case "NotEqual":
                        {
                            iQueryable = iQueryable.Where(pageLine =>
                            (amount != (pageLine.CreditAmount != 0 ? pageLine.CreditAmount : pageLine.DebitAmount))
                            &&
                            (-1 * amount != (pageLine.CreditAmount != 0 ? pageLine.CreditAmount : pageLine.DebitAmount))
                            );
                            break;
                        }

                    case "Between":
                        {
                            iQueryable = iQueryable.Where(pageLine =>
                            (amount <= (pageLine.CreditAmount != 0 ? pageLine.CreditAmount : pageLine.DebitAmount))
                            &&
                            (amount2 >= (pageLine.CreditAmount != 0 ? pageLine.CreditAmount : pageLine.DebitAmount)));
                            break;
                        }
                    case "Equals":
                    default:
                        {
                            iQueryable = iQueryable.Where(pageLine =>
                            (amount == (pageLine.CreditAmount != 0 ? pageLine.CreditAmount : pageLine.DebitAmount))
                            ||
                            (amount2 == (pageLine.CreditAmount != 0 ? pageLine.CreditAmount : pageLine.DebitAmount))
                            );
                            break;
                        }
                }

            }

            return iQueryable;
        }
        

        public IQueryable<ReconcileExternalPageLine> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ReconcileExternalPageLine> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }
    
  


}
	