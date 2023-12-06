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

    public partial class ConfirmationNumberTokenLogListQueryService
    {
         private ICustomContext context;
        public ConfirmationNumberTokenLogListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<ConfirmationNumberTokenLogList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ConfirmationNumberTokenLog> iQueryable = (from a in context.ConfirmationNumberTokenLogs
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ConfirmationNumberTokenLog>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ConfirmationNumberTokenLogList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ConfirmationNumberTokenLogList>(listQueryOperation, query2);

     //       if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
     //       {
     //           PropertyInfo propInfo = typeof(ConfirmationNumberTokenLogList).GetProperty(queryOperations.SortByColumnName);
     //           List<ObjectField> ConfirmationNumberTokenLogObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.ConfirmationNumberTokenLog",tenant).ToList();

     //           ObjectField objectField = (from a in ConfirmationNumberTokenLogObjectFields
     //                                      where a.FieldName == queryOperations.SortByColumnName
     //                                      select a).FirstOrDefault();

     //           if (objectField != null)
     //           {
				 //if (objectField.IsCustom)
     //               {
     //                   query2 = sortClass.GetSorterQuery<ConfirmationNumberTokenLogList, string>(queryOperations, query2);
     //               }
     //               else
     //               {
     //                switch (objectField.DataTypeCode.ToLower())
     //                {
     //                    case "ntext":
     //                   case "text":
     //                       {
     //                           query2 = sortClass.GetSorterQuery<ConfirmationNumberTokenLogList, string>(queryOperations, query2);
     //                           break;
     //                       }
					//	case "sigdouble":
					//	case "double":
     //                       {
     //                           query2 = sortClass.GetSorterQuery<ConfirmationNumberTokenLogList, double>(queryOperations, query2);
     //                           break;
     //                       }
					//	case "date":
     //                   case "datetime":
     //                       {
     //                           query2 = sortClass.GetSorterQuery<ConfirmationNumberTokenLogList, DateTime>(queryOperations, query2);
     //                           break;
     //                       }
					//	case "unsinteger":
     //                   case "integer":
     //                       {
     //                           query2 = sortClass.GetSorterQuery<ConfirmationNumberTokenLogList, int>(queryOperations, query2);
     //                           break;
     //                       }
     //                   case "boolean":
     //                       {
     //                           query2 = sortClass.GetSorterQuery<ConfirmationNumberTokenLogList, bool>(queryOperations, query2);
     //                           break;
     //                       }
					//	case "unsdecimal":
					//	case "decimal":
     //                       {
     //                           query2 = sortClass.GetSorterQuery<ConfirmationNumberTokenLogList, decimal>(queryOperations, query2);
     //                           break;
     //                       }
     //                   default:
     //                       {
     //                           query2 = query2.OrderByDescending(d => d.CreateDate);
     //                           break;
     //                       }
     //               }
				 //}
     //           }
     //       }
		   // else
     //       {
                query2 = query2.OrderByDescending(d => d.CreateDate);
            //}
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<ConfirmationNumberTokenLogList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ConfirmationNumberTokenLogList GetSingle(string id)
        {
            IQueryable<ConfirmationNumberTokenLog> ConfirmationNumberTokenLogQuery = (from a in context.ConfirmationNumberTokenLogs
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<ConfirmationNumberTokenLogList> ConfirmationNumberTokenLogListQuery = GetIqueryableList( ConfirmationNumberTokenLogQuery);
            ConfirmationNumberTokenLogList ConfirmationNumberTokenLogList = ConfirmationNumberTokenLogListQuery.FirstOrDefault();
            return ConfirmationNumberTokenLogList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ConfirmationNumberTokenLog> iQueryable = (from a in context.ConfirmationNumberTokenLogs 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<ConfirmationNumberTokenLog>(nonListQueryOperation, iQueryable);

            IQueryable<ConfirmationNumberTokenLogList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ConfirmationNumberTokenLogList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 