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

    public partial class CustomerIdentificationTypeListQueryService
    {
         private ICustomContext context;
        public CustomerIdentificationTypeListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CustomerIdentificationTypeList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomerIdentificationType> iQueryable = (from a in context.CustomerIdentificationTypes
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomerIdentificationType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CustomerIdentificationTypeList> query2 = GetIqueryableList(iQueryable);
					  query2 = filter.GetFilteredQuery<CustomerIdentificationTypeList>(listQueryOperation, query2);
		
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomerIdentificationTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CustomerIdentificationTypeObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CustomerIdentificationType",tenant).ToList();

                ObjectField objectField = (from a in CustomerIdentificationTypeObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CustomerIdentificationTypeList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerIdentificationTypeList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerIdentificationTypeList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerIdentificationTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerIdentificationTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerIdentificationTypeList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerIdentificationTypeList, decimal>(queryOperations, query2);
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

         public List<CustomerIdentificationTypeList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CustomerIdentificationTypeList GetSingle(string code)
        {
            IQueryable<CustomerIdentificationType> CustomerIdentificationTypeQuery = (from a in context.CustomerIdentificationTypes
                                                       where a.Code == code
                                                       select a);
          
		  
		  			IQueryable<CustomerIdentificationTypeList> CustomerIdentificationTypeListQuery = GetIqueryableList( CustomerIdentificationTypeQuery);
			            CustomerIdentificationTypeList CustomerIdentificationTypeList = CustomerIdentificationTypeListQuery.FirstOrDefault();
            return CustomerIdentificationTypeList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomerIdentificationType> iQueryable = (from a in context.CustomerIdentificationTypes  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CustomerIdentificationType>(nonListQueryOperation, iQueryable);

            IQueryable<CustomerIdentificationTypeList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<CustomerIdentificationTypeList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 