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

    public partial class DangerousGoodsPackingReqListQueryService
    {
         private ICustomContext context;
        public DangerousGoodsPackingReqListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<DangerousGoodsPackingReqList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DangerousGoodsPackingReq> iQueryable = (from a in context.DangerousGoodsPackingReqs
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DangerousGoodsPackingReq>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<DangerousGoodsPackingReqList> query2 = GetIqueryableList(iQueryable);
<<<<<<< HEAD
					  query2 = filter.GetFilteredQuery<DangerousGoodsPackingReqList>(listQueryOperation, query2);
		
=======
           
            query2 = filter.GetFilteredQuery<DangerousGoodsPackingReqList>(listQueryOperation, query2);

>>>>>>> parent of 094118ae335 (#181601)
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DangerousGoodsPackingReqList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> DangerousGoodsPackingReqObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.DangerousGoodsPackingReq",tenant).ToList();

                ObjectField objectField = (from a in DangerousGoodsPackingReqObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<DangerousGoodsPackingReqList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<DangerousGoodsPackingReqList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DangerousGoodsPackingReqList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DangerousGoodsPackingReqList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DangerousGoodsPackingReqList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DangerousGoodsPackingReqList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<DangerousGoodsPackingReqList, decimal>(queryOperations, query2);
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

         public List<DangerousGoodsPackingReqList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public DangerousGoodsPackingReqList GetSingle(string code)
        {
            IQueryable<DangerousGoodsPackingReq> DangerousGoodsPackingReqQuery = (from a in context.DangerousGoodsPackingReqs
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<DangerousGoodsPackingReqList> DangerousGoodsPackingReqListQuery = GetIqueryableList( DangerousGoodsPackingReqQuery);
            DangerousGoodsPackingReqList DangerousGoodsPackingReqList = DangerousGoodsPackingReqListQuery.FirstOrDefault();
            return DangerousGoodsPackingReqList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DangerousGoodsPackingReq> iQueryable = (from a in context.DangerousGoodsPackingReqs  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<DangerousGoodsPackingReq>(nonListQueryOperation, iQueryable);

            IQueryable<DangerousGoodsPackingReqList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<DangerousGoodsPackingReqList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 