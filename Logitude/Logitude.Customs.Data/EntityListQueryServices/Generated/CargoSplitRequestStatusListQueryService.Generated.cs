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

    public partial class CargoSplitRequestStatusListQueryService
    {
         private ICustomContext context;
        public CargoSplitRequestStatusListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CargoSplitRequestStatusList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CargoSplitRequestStatus> iQueryable = (from a in context.CargoSplitRequestStatuses
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CargoSplitRequestStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CargoSplitRequestStatusList> query2 = GetIqueryableList(iQueryable);
					  query2 = filter.GetFilteredQuery<CargoSplitRequestStatusList>(listQueryOperation, query2);
		
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CargoSplitRequestStatusList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CargoSplitRequestStatusObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CargoSplitRequestStatus",tenant).ToList();

                ObjectField objectField = (from a in CargoSplitRequestStatusObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CargoSplitRequestStatusList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CargoSplitRequestStatusList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CargoSplitRequestStatusList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CargoSplitRequestStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CargoSplitRequestStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CargoSplitRequestStatusList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CargoSplitRequestStatusList, decimal>(queryOperations, query2);
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

         public List<CargoSplitRequestStatusList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CargoSplitRequestStatusList GetSingle(string code)
        {
            IQueryable<CargoSplitRequestStatus> CargoSplitRequestStatusQuery = (from a in context.CargoSplitRequestStatuses
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<CargoSplitRequestStatusList> CargoSplitRequestStatusListQuery = GetIqueryableList( CargoSplitRequestStatusQuery);
            CargoSplitRequestStatusList CargoSplitRequestStatusList = CargoSplitRequestStatusListQuery.FirstOrDefault();
            return CargoSplitRequestStatusList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CargoSplitRequestStatus> iQueryable = (from a in context.CargoSplitRequestStatuses  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CargoSplitRequestStatus>(nonListQueryOperation, iQueryable);

            IQueryable<CargoSplitRequestStatusList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<CargoSplitRequestStatusList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 