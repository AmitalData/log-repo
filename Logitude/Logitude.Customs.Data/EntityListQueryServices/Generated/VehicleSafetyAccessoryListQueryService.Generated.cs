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

    public partial class VehicleSafetyAccessoryListQueryService
    {
         private ICustomContext context;
        public VehicleSafetyAccessoryListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<VehicleSafetyAccessoryList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<VehicleSafetyAccessory> iQueryable = (from a in context.VehicleSafetyAccessories
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<VehicleSafetyAccessory>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<VehicleSafetyAccessoryList> query2 = GetIqueryableList(iQueryable);
					  query2 = filter.GetFilteredQuery<VehicleSafetyAccessoryList>(listQueryOperation, query2);
		
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(VehicleSafetyAccessoryList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> VehicleSafetyAccessoryObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.VehicleSafetyAccessory",tenant).ToList();

                ObjectField objectField = (from a in VehicleSafetyAccessoryObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<VehicleSafetyAccessoryList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<VehicleSafetyAccessoryList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<VehicleSafetyAccessoryList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<VehicleSafetyAccessoryList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<VehicleSafetyAccessoryList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<VehicleSafetyAccessoryList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<VehicleSafetyAccessoryList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.VehicleId);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.VehicleId);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<VehicleSafetyAccessoryList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public VehicleSafetyAccessoryList GetSingle(string vehicleid, int linenumber)
        {
            IQueryable<VehicleSafetyAccessory> VehicleSafetyAccessoryQuery = (from a in context.VehicleSafetyAccessories
                                                       where a.VehicleId == vehicleid && a.LineNumber == linenumber
                                                       select a);

             
            IQueryable<VehicleSafetyAccessoryList> VehicleSafetyAccessoryListQuery = GetIqueryableList( VehicleSafetyAccessoryQuery);
            VehicleSafetyAccessoryList VehicleSafetyAccessoryList = VehicleSafetyAccessoryListQuery.FirstOrDefault();
            return VehicleSafetyAccessoryList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<VehicleSafetyAccessory> iQueryable = (from a in context.VehicleSafetyAccessories 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<VehicleSafetyAccessory>(nonListQueryOperation, iQueryable);

            IQueryable<VehicleSafetyAccessoryList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<VehicleSafetyAccessoryList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 