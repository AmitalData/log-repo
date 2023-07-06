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

    public partial class VehicleSafeAccessoryInstlTypeListQueryService
    {
         private ICustomContext context;
        public VehicleSafeAccessoryInstlTypeListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<VehicleSafeAccessoryInstlTypeList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<VehicleSafeAccessoryInstlType> iQueryable = (from a in context.VehicleSafeAccessoryInstlTypes
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<VehicleSafeAccessoryInstlType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<VehicleSafeAccessoryInstlTypeList> query2 = GetIqueryableList(iQueryable);
<<<<<<< HEAD
					  query2 = filter.GetFilteredQuery<VehicleSafeAccessoryInstlTypeList>(listQueryOperation, query2);
		
=======
           
            query2 = filter.GetFilteredQuery<VehicleSafeAccessoryInstlTypeList>(listQueryOperation, query2);

>>>>>>> parent of 094118ae335 (#181601)
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(VehicleSafeAccessoryInstlTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> VehicleSafeAccessoryInstlTypeObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.VehicleSafeAccessoryInstlType",tenant).ToList();

                ObjectField objectField = (from a in VehicleSafeAccessoryInstlTypeObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<VehicleSafeAccessoryInstlTypeList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<VehicleSafeAccessoryInstlTypeList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<VehicleSafeAccessoryInstlTypeList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<VehicleSafeAccessoryInstlTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<VehicleSafeAccessoryInstlTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<VehicleSafeAccessoryInstlTypeList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<VehicleSafeAccessoryInstlTypeList, decimal>(queryOperations, query2);
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

         public List<VehicleSafeAccessoryInstlTypeList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public VehicleSafeAccessoryInstlTypeList GetSingle(string code)
        {
            IQueryable<VehicleSafeAccessoryInstlType> VehicleSafeAccessoryInstlTypeQuery = (from a in context.VehicleSafeAccessoryInstlTypes
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<VehicleSafeAccessoryInstlTypeList> VehicleSafeAccessoryInstlTypeListQuery = GetIqueryableList( VehicleSafeAccessoryInstlTypeQuery);
            VehicleSafeAccessoryInstlTypeList VehicleSafeAccessoryInstlTypeList = VehicleSafeAccessoryInstlTypeListQuery.FirstOrDefault();
            return VehicleSafeAccessoryInstlTypeList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<VehicleSafeAccessoryInstlType> iQueryable = (from a in context.VehicleSafeAccessoryInstlTypes  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<VehicleSafeAccessoryInstlType>(nonListQueryOperation, iQueryable);

            IQueryable<VehicleSafeAccessoryInstlTypeList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<VehicleSafeAccessoryInstlTypeList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 