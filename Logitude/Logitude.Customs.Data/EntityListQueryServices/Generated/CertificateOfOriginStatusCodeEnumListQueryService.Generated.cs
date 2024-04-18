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

    public partial class CertificateOfOriginStatusCodeEnumListQueryService
    {
         private ICustomContext context;
        public CertificateOfOriginStatusCodeEnumListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CertificateOfOriginStatusCodeEnumList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<CertificateOfOriginStatusCodeEnumList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CertificateOfOriginStatusCodeEnum> iQueryable = (from a in context.CertificateOfOriginStatusCodeEnums
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<CertificateOfOriginStatusCodeEnum>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CertificateOfOriginStatusCodeEnumList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CertificateOfOriginStatusCodeEnumList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<CertificateOfOriginStatusCodeEnumList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CertificateOfOriginStatusCodeEnumList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CertificateOfOriginStatusCodeEnumObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CertificateOfOriginStatusCodeEnum",tenant).ToList();

                ObjectField objectField = (from a in CertificateOfOriginStatusCodeEnumObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CertificateOfOriginStatusCodeEnumList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginStatusCodeEnumList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginStatusCodeEnumList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginStatusCodeEnumList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginStatusCodeEnumList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginStatusCodeEnumList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginStatusCodeEnumList, decimal>(queryOperations, query2);
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

         public List<CertificateOfOriginStatusCodeEnumList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CertificateOfOriginStatusCodeEnumList GetSingle(string code)
        {
            IQueryable<CertificateOfOriginStatusCodeEnum> CertificateOfOriginStatusCodeEnumQuery = (from a in context.CertificateOfOriginStatusCodeEnums
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<CertificateOfOriginStatusCodeEnumList> CertificateOfOriginStatusCodeEnumListQuery = GetIqueryableList( CertificateOfOriginStatusCodeEnumQuery);
            CertificateOfOriginStatusCodeEnumList CertificateOfOriginStatusCodeEnumList = CertificateOfOriginStatusCodeEnumListQuery.FirstOrDefault();
            return CertificateOfOriginStatusCodeEnumList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations ){
		 		  return GetListCount(queryOperations, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CertificateOfOriginStatusCodeEnum> iQueryable = (from a in context.CertificateOfOriginStatusCodeEnums  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<CertificateOfOriginStatusCodeEnum>(nonListQueryOperation, iQueryable);



            IQueryable<CertificateOfOriginStatusCodeEnumList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CertificateOfOriginStatusCodeEnumList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<CertificateOfOriginStatusCodeEnumList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 