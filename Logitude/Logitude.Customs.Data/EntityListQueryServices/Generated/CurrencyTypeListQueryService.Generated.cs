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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CurrencyTypeListQueryService
    {
         private ICustomContext context;
        public CurrencyTypeListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CurrencyTypeList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CurrencyType> iQueryable = (from a in context.CurrencyTypes
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CurrencyType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CurrencyTypeList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CurrencyTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CurrencyTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CurrencyTypeObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CurrencyType",tenant).ToList();

                ObjectField objectField = (from a in CurrencyTypeObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CurrencyTypeList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CurrencyTypeList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CurrencyTypeList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CurrencyTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CurrencyTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CurrencyTypeList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CurrencyTypeList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.CodeSorted);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.CodeSorted);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<CurrencyTypeList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CurrencyTypeList GetSingle(string code)
        {
            IQueryable<CurrencyType> CurrencyTypeQuery = (from a in context.CurrencyTypes
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<CurrencyTypeList> CurrencyTypeListQuery = GetIqueryableList( CurrencyTypeQuery);
            CurrencyTypeList CurrencyTypeList = CurrencyTypeListQuery.FirstOrDefault();
            return CurrencyTypeList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CurrencyType> iQueryable = (from a in context.CurrencyTypes  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CurrencyType>(nonListQueryOperation, iQueryable);

            IQueryable<CurrencyTypeList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CurrencyTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 