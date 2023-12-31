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

    public partial class CertificateOfOriginItemListQueryService
    {
         private ICustomContext context;
        public CertificateOfOriginItemListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CertificateOfOriginItemList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CertificateOfOriginItem> iQueryable = (from a in context.CertificateOfOriginItems
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CertificateOfOriginItem>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CertificateOfOriginItemList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CertificateOfOriginItemList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CertificateOfOriginItemList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CertificateOfOriginItemObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CertificateOfOriginItem",tenant).ToList();

                ObjectField objectField = (from a in CertificateOfOriginItemObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CertificateOfOriginItemList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginItemList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginItemList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginItemList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginItemList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginItemList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginItemList, decimal>(queryOperations, query2);
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

         public List<CertificateOfOriginItemList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CertificateOfOriginItemList GetSingle(string id)
        {
            IQueryable<CertificateOfOriginItem> CertificateOfOriginItemQuery = (from a in context.CertificateOfOriginItems
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<CertificateOfOriginItemList> CertificateOfOriginItemListQuery = GetIqueryableList( CertificateOfOriginItemQuery);
            CertificateOfOriginItemList CertificateOfOriginItemList = CertificateOfOriginItemListQuery.FirstOrDefault();
            return CertificateOfOriginItemList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CertificateOfOriginItem> iQueryable = (from a in context.CertificateOfOriginItems 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CertificateOfOriginItem>(nonListQueryOperation, iQueryable);

            IQueryable<CertificateOfOriginItemList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CertificateOfOriginItemList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 