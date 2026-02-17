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

    public partial class PaymentOrderMethodListQueryService
    {
         private ICustomContext context;
        public PaymentOrderMethodListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<PaymentOrderMethodList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PaymentOrderMethod> iQueryable = (from a in context.PaymentOrderMethods
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PaymentOrderMethod>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<PaymentOrderMethodList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<PaymentOrderMethodList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PaymentOrderMethodList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> PaymentOrderMethodObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.PaymentOrderMethod",tenant).ToList();

                ObjectField objectField = (from a in PaymentOrderMethodObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<PaymentOrderMethodList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentOrderMethodList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentOrderMethodList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentOrderMethodList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentOrderMethodList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentOrderMethodList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentOrderMethodList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.TypeCode);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.TypeCode);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<PaymentOrderMethodList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public PaymentOrderMethodList GetSingle(string paymentorderid, int line)
        {
            IQueryable<PaymentOrderMethod> PaymentOrderMethodQuery = (from a in context.PaymentOrderMethods
                                                       where a.PaymentOrderId == paymentorderid && a.Line == line
                                                       select a);

             
            IQueryable<PaymentOrderMethodList> PaymentOrderMethodListQuery = GetIqueryableList( PaymentOrderMethodQuery);
            PaymentOrderMethodList PaymentOrderMethodList = PaymentOrderMethodListQuery.FirstOrDefault();
            return PaymentOrderMethodList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PaymentOrderMethod> iQueryable = (from a in context.PaymentOrderMethods 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<PaymentOrderMethod>(nonListQueryOperation, iQueryable);

            IQueryable<PaymentOrderMethodList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<PaymentOrderMethodList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 