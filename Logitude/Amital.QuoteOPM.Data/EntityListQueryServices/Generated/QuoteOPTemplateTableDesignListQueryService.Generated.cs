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

    public partial class QuoteOPTemplateTableDesignListQueryService
    {
         private IQuoteOPMContext context;
        public QuoteOPTemplateTableDesignListQueryService(IQuoteOPMContext context)
        {
            this.context = context;
        }

        public List<QuoteOPTemplateTableDesignList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuoteOPTemplateTableDesign> iQueryable = (from a in context.QuoteOPTemplateTableDesigns
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<QuoteOPTemplateTableDesign>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<QuoteOPTemplateTableDesignList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<QuoteOPTemplateTableDesignList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteOPTemplateTableDesignList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> QuoteOPTemplateTableDesignObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QuoteOPTemplateTableDesign",tenant).ToList();

                ObjectField objectField = (from a in QuoteOPTemplateTableDesignObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<QuoteOPTemplateTableDesignList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPTemplateTableDesignList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPTemplateTableDesignList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPTemplateTableDesignList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPTemplateTableDesignList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPTemplateTableDesignList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPTemplateTableDesignList, decimal>(queryOperations, query2);
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

         public List<QuoteOPTemplateTableDesignList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public QuoteOPTemplateTableDesignList GetSingle(string id)
        {
            IQueryable<QuoteOPTemplateTableDesign> QuoteOPTemplateTableDesignQuery = (from a in context.QuoteOPTemplateTableDesigns
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<QuoteOPTemplateTableDesignList> QuoteOPTemplateTableDesignListQuery = GetIqueryableList( QuoteOPTemplateTableDesignQuery);
            QuoteOPTemplateTableDesignList QuoteOPTemplateTableDesignList = QuoteOPTemplateTableDesignListQuery.FirstOrDefault();
            return QuoteOPTemplateTableDesignList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuoteOPTemplateTableDesign> iQueryable = (from a in context.QuoteOPTemplateTableDesigns 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<QuoteOPTemplateTableDesign>(nonListQueryOperation, iQueryable);

            IQueryable<QuoteOPTemplateTableDesignList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<QuoteOPTemplateTableDesignList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 