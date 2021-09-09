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

    public partial class QuoteOPDocumentVersionListQueryService
    {
         private IQuoteOPMContext context;
        public QuoteOPDocumentVersionListQueryService(IQuoteOPMContext context)
        {
            this.context = context;
        }

        public List<QuoteOPDocumentVersionList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuoteOPDocumentVersion> iQueryable = (from a in context.QuoteOPDocumentVersions
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<QuoteOPDocumentVersion>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<QuoteOPDocumentVersionList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<QuoteOPDocumentVersionList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteOPDocumentVersionList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> QuoteOPDocumentVersionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QuoteOPDocumentVersion",tenant).ToList();

                ObjectField objectField = (from a in QuoteOPDocumentVersionObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<QuoteOPDocumentVersionList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPDocumentVersionList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPDocumentVersionList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPDocumentVersionList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPDocumentVersionList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPDocumentVersionList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteOPDocumentVersionList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.QuoteOPId);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.QuoteOPId);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<QuoteOPDocumentVersionList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public QuoteOPDocumentVersionList GetSingle(string quoteopid, int versionnumber)
        {
            IQueryable<QuoteOPDocumentVersion> QuoteOPDocumentVersionQuery = (from a in context.QuoteOPDocumentVersions
                                                       where a.QuoteOPId == quoteopid && a.VersionNumber == versionnumber
                                                       select a);

             
            IQueryable<QuoteOPDocumentVersionList> QuoteOPDocumentVersionListQuery = GetIqueryableList( QuoteOPDocumentVersionQuery);
            QuoteOPDocumentVersionList QuoteOPDocumentVersionList = QuoteOPDocumentVersionListQuery.FirstOrDefault();
            return QuoteOPDocumentVersionList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuoteOPDocumentVersion> iQueryable = (from a in context.QuoteOPDocumentVersions 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<QuoteOPDocumentVersion>(nonListQueryOperation, iQueryable);

            IQueryable<QuoteOPDocumentVersionList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<QuoteOPDocumentVersionList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 