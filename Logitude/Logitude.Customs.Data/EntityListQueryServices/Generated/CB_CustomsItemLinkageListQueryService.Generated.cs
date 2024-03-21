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

    public partial class CB_CustomsItemLinkageListQueryService
    {
         private ICustomContext context;
        public CB_CustomsItemLinkageListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CB_CustomsItemLinkageList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CB_CustomsItemLinkage> iQueryable = (from a in context.CB_CustomsItemLinkages
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CB_CustomsItemLinkage>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CB_CustomsItemLinkageList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CB_CustomsItemLinkageList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CB_CustomsItemLinkageList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CB_CustomsItemLinkageObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CB_CustomsItemLinkage",tenant).ToList();

                ObjectField objectField = (from a in CB_CustomsItemLinkageObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CB_CustomsItemLinkageList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CB_CustomsItemLinkageList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CB_CustomsItemLinkageList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CB_CustomsItemLinkageList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CB_CustomsItemLinkageList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CB_CustomsItemLinkageList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CB_CustomsItemLinkageList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.CB_ID);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.CB_ID);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<CB_CustomsItemLinkageList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CB_CustomsItemLinkageList GetSingle(string cb_id)
        {
            IQueryable<CB_CustomsItemLinkage> CB_CustomsItemLinkageQuery = (from a in context.CB_CustomsItemLinkages
                                                       where a.CB_ID == cb_id
                                                       select a);

             
            IQueryable<CB_CustomsItemLinkageList> CB_CustomsItemLinkageListQuery = GetIqueryableList( CB_CustomsItemLinkageQuery);
            CB_CustomsItemLinkageList CB_CustomsItemLinkageList = CB_CustomsItemLinkageListQuery.FirstOrDefault();
            return CB_CustomsItemLinkageList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CB_CustomsItemLinkage> iQueryable = (from a in context.CB_CustomsItemLinkages  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CB_CustomsItemLinkage>(nonListQueryOperation, iQueryable);

            IQueryable<CB_CustomsItemLinkageList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CB_CustomsItemLinkageList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 