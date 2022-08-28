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

using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.EntityLists;

namespace Logitude.CargoTracking.Data.EntityListQueryServices
{ 

    public partial class CargoTrackingShipmentSearchListQueryService
    {
         private ICargoTrackingContext context;
        public CargoTrackingShipmentSearchListQueryService(ICargoTrackingContext context)
        {
            this.context = context;
        }

        public List<CargoTrackingShipmentSearchList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<CargoTrackingShipmentSearchList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CargoTrackingShipmentSearch> iQueryable = (from a in context.CargoTrackingShipmentSearches
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<CargoTrackingShipmentSearch>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CargoTrackingShipmentSearchList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CargoTrackingShipmentSearchList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<CargoTrackingShipmentSearchList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CargoTrackingShipmentSearchList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CargoTrackingShipmentSearchObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CargoTrackingShipmentSearch",tenant).ToList();

                ObjectField objectField = (from a in CargoTrackingShipmentSearchObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CargoTrackingShipmentSearchList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CargoTrackingShipmentSearchList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CargoTrackingShipmentSearchList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CargoTrackingShipmentSearchList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CargoTrackingShipmentSearchList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CargoTrackingShipmentSearchList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CargoTrackingShipmentSearchList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.ShipmentDate);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.ShipmentDate);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<CargoTrackingShipmentSearchList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CargoTrackingShipmentSearchList GetSingle(int id)
        {
            IQueryable<CargoTrackingShipmentSearch> CargoTrackingShipmentSearchQuery = (from a in context.CargoTrackingShipmentSearches
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<CargoTrackingShipmentSearchList> CargoTrackingShipmentSearchListQuery = GetIqueryableList( CargoTrackingShipmentSearchQuery);
            CargoTrackingShipmentSearchList CargoTrackingShipmentSearchList = CargoTrackingShipmentSearchListQuery.FirstOrDefault();
            return CargoTrackingShipmentSearchList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations, int tenant ){
		 		  return GetListCount(queryOperations,tenant, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations, int tenant  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CargoTrackingShipmentSearch> iQueryable = (from a in context.CargoTrackingShipmentSearches 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<CargoTrackingShipmentSearch>(nonListQueryOperation, iQueryable);



            IQueryable<CargoTrackingShipmentSearchList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CargoTrackingShipmentSearchList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<CargoTrackingShipmentSearchList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 