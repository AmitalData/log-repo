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

    public partial class ClosedTableStatusListQueryService
    {
         private ICustomContext context;
        public ClosedTableStatusListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<ClosedTableStatusList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ClosedTableStatus> iQueryable = (from a in context.ClosedTableStatus
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ClosedTableStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ClosedTableStatusList> query2 = GetIqueryableList(iQueryable);
					  query2 = filter.GetFilteredQuery<ClosedTableStatusList>(listQueryOperation, query2);
		
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ClosedTableStatusList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ClosedTableStatusObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.ClosedTableStatus",tenant).ToList();

                ObjectField objectField = (from a in ClosedTableStatusObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ClosedTableStatusList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ClosedTableStatusList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ClosedTableStatusList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ClosedTableStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ClosedTableStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ClosedTableStatusList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ClosedTableStatusList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Code);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.Code);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<ClosedTableStatusList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ClosedTableStatusList GetSingle(string code)
        {
            IQueryable<ClosedTableStatus> ClosedTableStatusQuery = (from a in context.ClosedTableStatus
                                                       where a.Code == code
                                                       select a);
          
		  
		  			IQueryable<ClosedTableStatusList> ClosedTableStatusListQuery = GetIqueryableList( ClosedTableStatusQuery);
			            ClosedTableStatusList ClosedTableStatusList = ClosedTableStatusListQuery.FirstOrDefault();
            return ClosedTableStatusList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ClosedTableStatus> iQueryable = (from a in context.ClosedTableStatus  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<ClosedTableStatus>(nonListQueryOperation, iQueryable);

            IQueryable<ClosedTableStatusList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<ClosedTableStatusList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 