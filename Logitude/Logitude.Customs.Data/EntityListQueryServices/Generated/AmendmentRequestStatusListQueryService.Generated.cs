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

    public partial class AmendmentRequestStatusListQueryService
    {
         private ICustomContext context;
        public AmendmentRequestStatusListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<AmendmentRequestStatusList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AmendmentRequestStatus> iQueryable = (from a in context.AmendmentRequestStatuses
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AmendmentRequestStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<AmendmentRequestStatusList> query2 = GetIqueryableList(iQueryable);
<<<<<<< HEAD
					  query2 = filter.GetFilteredQuery<AmendmentRequestStatusList>(listQueryOperation, query2);
		
=======
           
            query2 = filter.GetFilteredQuery<AmendmentRequestStatusList>(listQueryOperation, query2);

>>>>>>> parent of 094118ae335 (#181601)
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AmendmentRequestStatusList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> AmendmentRequestStatusObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.AmendmentRequestStatus",tenant).ToList();

                ObjectField objectField = (from a in AmendmentRequestStatusObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<AmendmentRequestStatusList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AmendmentRequestStatusList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AmendmentRequestStatusList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AmendmentRequestStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AmendmentRequestStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AmendmentRequestStatusList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<AmendmentRequestStatusList, decimal>(queryOperations, query2);
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

         public List<AmendmentRequestStatusList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public AmendmentRequestStatusList GetSingle(string code)
        {
            IQueryable<AmendmentRequestStatus> AmendmentRequestStatusQuery = (from a in context.AmendmentRequestStatuses
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<AmendmentRequestStatusList> AmendmentRequestStatusListQuery = GetIqueryableList( AmendmentRequestStatusQuery);
            AmendmentRequestStatusList AmendmentRequestStatusList = AmendmentRequestStatusListQuery.FirstOrDefault();
            return AmendmentRequestStatusList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AmendmentRequestStatus> iQueryable = (from a in context.AmendmentRequestStatuses  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<AmendmentRequestStatus>(nonListQueryOperation, iQueryable);

            IQueryable<AmendmentRequestStatusList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<AmendmentRequestStatusList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 