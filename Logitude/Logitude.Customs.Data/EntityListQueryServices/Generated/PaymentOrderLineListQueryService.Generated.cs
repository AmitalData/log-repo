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

    public partial class PaymentOrderLineListQueryService
    {
         private ICustomContext context;
        public PaymentOrderLineListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<PaymentOrderLineList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PaymentOrderLine> iQueryable = (from a in context.PaymentOrderLines
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PaymentOrderLine>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<PaymentOrderLineList> query2 = GetIqueryableList(iQueryable);
					  query2 = filter.GetFilteredQuery<PaymentOrderLineList>(listQueryOperation, query2);
		
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PaymentOrderLineList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> PaymentOrderLineObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.PaymentOrderLine",tenant).ToList();

                ObjectField objectField = (from a in PaymentOrderLineObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<PaymentOrderLineList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentOrderLineList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentOrderLineList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentOrderLineList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentOrderLineList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentOrderLineList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<PaymentOrderLineList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.ParagraphTypeCode);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.ParagraphTypeCode);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<PaymentOrderLineList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public PaymentOrderLineList GetSingle(string paymentorderid, string paragraphtypecode)
        {
            IQueryable<PaymentOrderLine> PaymentOrderLineQuery = (from a in context.PaymentOrderLines
                                                       where a.PaymentOrderId == paymentorderid && a.ParagraphTypeCode == paragraphtypecode
                                                       select a);

             
            IQueryable<PaymentOrderLineList> PaymentOrderLineListQuery = GetIqueryableList( PaymentOrderLineQuery);
            PaymentOrderLineList PaymentOrderLineList = PaymentOrderLineListQuery.FirstOrDefault();
            return PaymentOrderLineList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PaymentOrderLine> iQueryable = (from a in context.PaymentOrderLines 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<PaymentOrderLine>(nonListQueryOperation, iQueryable);

            IQueryable<PaymentOrderLineList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<PaymentOrderLineList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 