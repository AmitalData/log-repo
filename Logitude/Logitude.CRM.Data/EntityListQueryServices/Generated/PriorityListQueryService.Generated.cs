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

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class PriorityListQueryService
    {
         private ICRMContext context;
        public PriorityListQueryService(ICRMContext context)
        {
            this.context = context;
        }

        public List<PriorityList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Priority> iQueryable = (from a in context.Priorities
                                               select a);

			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);
			 
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Priority>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<PriorityList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<PriorityList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PriorityList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> PriorityObjectFields = ObjectFieldsRepository.GetObjectFieldsByObjectTableName("Priority",tenant).ToList();

                ObjectField objectField = (from a in PriorityObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				  if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<PriorityList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
					    case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PriorityList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PriorityList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PriorityList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PriorityList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PriorityList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<PriorityList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
				 }
               }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<PriorityList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public PriorityList GetSingle(string code)
        {
            IQueryable<Priority> PriorityQuery = (from a in context.Priorities
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<PriorityList> PriorityListQuery = GetIqueryableList( PriorityQuery);
            PriorityList PriorityList = PriorityListQuery.FirstOrDefault();
            return PriorityList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Priority> iQueryable = (from a in context.Priorities  select a);

			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable);
			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);


            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<Priority>(nonListQueryOperation, iQueryable);

            IQueryable<PriorityList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<PriorityList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 