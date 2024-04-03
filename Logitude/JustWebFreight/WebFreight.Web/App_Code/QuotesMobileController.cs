using Logitude.CRM.Data;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.QuoteModel;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityQueries;
using WebFreight.Web.Security;
using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel;

namespace WebFreight.Web.App_Code
{
    public class QuotesMobileController : ApiController
    {
        public List<QuoteList> PostFilteredQuotes(int tenant, QuoteFilters filters)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            QueryOperations queryOperations = new QueryOperations();
            queryOperations.SetFilter("CustomerId", filters.CustomerId, false, "Equals", null, false);
            queryOperations.SetFilter("IsCancelled", filters.IsCancelled, false, "Equals", null, false);


            QuoteRepository quoteRepository = new QuoteRepository(tenant);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            QuoteCustomFilter customfilters = new QuoteCustomFilter(tenant);
            IQueryable<Quote> quotes = quoteRepository.GetQuoteByTenant(tenant, null);
            quotes = customfilters.GetFilteredQuery(queryOperations, quotes);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            quotes = filter.GetFilteredQuery<Quote>(nonListQueryOperation, quotes);
            int numOfQuotes = quotes.Count();
            int skippedQuotes = queryOperations.PageIndex;

            QuoteQuery quoteQuery = new QuoteQuery(tenant);
            IQueryable<QuoteList> query2 = quoteQuery.GetIQueryableEntityList(quotes);
            query2 = filter.GetFilteredQuery<QuoteList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> quoteObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Quote", tenant).ToList();

                ObjectField objectField = (from a in quoteObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.OpenDate);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.OpenDate);
            }

            query2 = query2.Skip(skippedQuotes);
            query2 = query2.Take(queryOperations.PageSize);


            List<QuoteList> listQuery = query2.ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Quote", tenant, listQuery.Cast<object>().ToList());

            return listQuery;


        }
    }
}