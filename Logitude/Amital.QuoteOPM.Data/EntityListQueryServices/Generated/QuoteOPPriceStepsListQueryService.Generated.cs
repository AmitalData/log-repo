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

using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.EntityLists;

namespace Amital.QuoteOPM.Data.EntityListQueryServices
{ 

    public partial class QuoteOPPriceStepsListQueryService
    {
         private IQuoteOPMContext context;
        public QuoteOPPriceStepsListQueryService(IQuoteOPMContext context)
        {
            this.context = context;
        }

        public List<QuoteOPPriceStepsList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuoteOPPriceSteps> iQueryable = (from a in context.QuoteOPPriceSteps
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<QuoteOPPriceSteps>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<QuoteOPPriceStepsList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<QuoteOPPriceStepsList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteOPPriceStepsList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> QuoteOPPriceStepsObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QuoteOPPriceSteps",tenant).ToList();

                ObjectField objectField = (from a in QuoteOPPriceStepsObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<QuoteOPPriceStepsList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPPriceStepsList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPPriceStepsList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPPriceStepsList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPPriceStepsList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPPriceStepsList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPPriceStepsList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Id);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.Id);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<QuoteOPPriceStepsList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public QuoteOPPriceStepsList GetSingle(string id)
        {
            IQueryable<QuoteOPPriceSteps> QuoteOPPriceStepsQuery = (from a in context.QuoteOPPriceSteps
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<QuoteOPPriceStepsList> QuoteOPPriceStepsListQuery = GetIqueryableList( QuoteOPPriceStepsQuery);
            QuoteOPPriceStepsList QuoteOPPriceStepsList = QuoteOPPriceStepsListQuery.FirstOrDefault();
            return QuoteOPPriceStepsList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuoteOPPriceSteps> iQueryable = (from a in context.QuoteOPPriceSteps 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<QuoteOPPriceSteps>(nonListQueryOperation, iQueryable);

            IQueryable<QuoteOPPriceStepsList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<QuoteOPPriceStepsList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 