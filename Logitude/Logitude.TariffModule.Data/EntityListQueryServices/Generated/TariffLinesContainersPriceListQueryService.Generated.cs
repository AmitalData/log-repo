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

using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityLists;

namespace Logitude.TariffModule.Data.EntityListQueryServices
{ 

    public partial class TariffLinesContainersPriceListQueryService
    {
         private ITariffModuleContext context;
        public TariffLinesContainersPriceListQueryService(ITariffModuleContext context)
        {
            this.context = context;
        }

        public List<TariffLinesContainersPriceList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<TariffLinesContainersPrice> iQueryable = (from a in context.TariffLinesContainersPrices
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TariffLinesContainersPrice>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<TariffLinesContainersPriceList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<TariffLinesContainersPriceList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TariffLinesContainersPriceList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> TariffLinesContainersPriceObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TariffLinesContainersPrice",tenant).ToList();

                ObjectField objectField = (from a in TariffLinesContainersPriceObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<TariffLinesContainersPriceList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<TariffLinesContainersPriceList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TariffLinesContainersPriceList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TariffLinesContainersPriceList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TariffLinesContainersPriceList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TariffLinesContainersPriceList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<TariffLinesContainersPriceList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Id);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.Id);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<TariffLinesContainersPriceList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public TariffLinesContainersPriceList GetSingle(string id)
        {
            IQueryable<TariffLinesContainersPrice> TariffLinesContainersPriceQuery = (from a in context.TariffLinesContainersPrices
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<TariffLinesContainersPriceList> TariffLinesContainersPriceListQuery = GetIqueryableList( TariffLinesContainersPriceQuery);
            TariffLinesContainersPriceList TariffLinesContainersPriceList = TariffLinesContainersPriceListQuery.FirstOrDefault();
            return TariffLinesContainersPriceList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<TariffLinesContainersPrice> iQueryable = (from a in context.TariffLinesContainersPrices 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<TariffLinesContainersPrice>(nonListQueryOperation, iQueryable);

            IQueryable<TariffLinesContainersPriceList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<TariffLinesContainersPriceList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 