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

using Logitude.ShipmentOrderLib.Data.EntityPOCOs;
using Logitude.ShipmentOrderLib.Data.EntityLists;

namespace Logitude.ShipmentOrderLib.Data.EntityListQueryServices
{ 

    public partial class ShipmentOrderListQueryService
    {
         private IShipmentOrderContext context;
        public ShipmentOrderListQueryService(IShipmentOrderContext context)
        {
            this.context = context;
        }

        public List<ShipmentOrderList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentOrder> iQueryable = (from a in context.ShipmentOrders
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentOrder>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ShipmentOrderList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ShipmentOrderList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ShipmentOrderList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ShipmentOrderObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ShipmentOrder",tenant).ToList();

                ObjectField objectField = (from a in ShipmentOrderObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ShipmentOrderList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentOrderList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentOrderList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentOrderList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentOrderList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentOrderList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentOrderList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.CreateDate);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.CreateDate);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<ShipmentOrderList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ShipmentOrderList GetSingle(string id)
        {
            IQueryable<ShipmentOrder> ShipmentOrderQuery = (from a in context.ShipmentOrders
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<ShipmentOrderList> ShipmentOrderListQuery = GetIqueryableList( ShipmentOrderQuery);
            ShipmentOrderList ShipmentOrderList = ShipmentOrderListQuery.FirstOrDefault();
            return ShipmentOrderList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentOrder> iQueryable = (from a in context.ShipmentOrders 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<ShipmentOrder>(nonListQueryOperation, iQueryable);

            IQueryable<ShipmentOrderList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ShipmentOrderList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 