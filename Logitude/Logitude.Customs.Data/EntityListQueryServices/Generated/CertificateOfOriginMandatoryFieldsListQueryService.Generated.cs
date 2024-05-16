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

    public partial class CertificateOfOriginMandatoryFieldsListQueryService
    {
         private ICustomContext context;
        public CertificateOfOriginMandatoryFieldsListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CertificateOfOriginMandatoryFieldsList> GetList(QueryOperations queryOperations, int tenant ){
		     return GetList(queryOperations,tenant, new TreeFilterQueryArgs());
		 }

        public List<CertificateOfOriginMandatoryFieldsList> GetList(QueryOperations queryOperations, int tenant , TreeFilterQueryArgs treeFilterQueryArgs)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CertificateOfOriginMandatoryFields> iQueryable = (from a in context.CertificateOfOriginMandatorys
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

            iQueryable = filter.GetFilteredQuery<CertificateOfOriginMandatoryFields>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CertificateOfOriginMandatoryFieldsList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CertificateOfOriginMandatoryFieldsList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<CertificateOfOriginMandatoryFieldsList>(query2, treeFilterQueryArgs);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CertificateOfOriginMandatoryFieldsList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CertificateOfOriginMandatoryFieldsObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CertificateOfOriginMandatoryFields",tenant).ToList();

                ObjectField objectField = (from a in CertificateOfOriginMandatoryFieldsObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CertificateOfOriginMandatoryFieldsList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginMandatoryFieldsList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginMandatoryFieldsList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginMandatoryFieldsList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginMandatoryFieldsList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginMandatoryFieldsList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateOfOriginMandatoryFieldsList, decimal>(queryOperations, query2);
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

         public List<CertificateOfOriginMandatoryFieldsList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CertificateOfOriginMandatoryFieldsList GetSingle(string code)
        {
            IQueryable<CertificateOfOriginMandatoryFields> CertificateOfOriginMandatoryFieldsQuery = (from a in context.CertificateOfOriginMandatorys
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<CertificateOfOriginMandatoryFieldsList> CertificateOfOriginMandatoryFieldsListQuery = GetIqueryableList( CertificateOfOriginMandatoryFieldsQuery);
            CertificateOfOriginMandatoryFieldsList CertificateOfOriginMandatoryFieldsList = CertificateOfOriginMandatoryFieldsListQuery.FirstOrDefault();
            return CertificateOfOriginMandatoryFieldsList;
           
        }


		
        public int GetListCount(QueryOperations queryOperations ){
		 		  return GetListCount(queryOperations, new TreeFilterQueryArgs());

		 }

        public int GetListCount(QueryOperations queryOperations  ,TreeFilterQueryArgs treeFilterQueryArgs )
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CertificateOfOriginMandatoryFields> iQueryable = (from a in context.CertificateOfOriginMandatorys  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
            
			iQueryable = filter.GetFilteredQuery<CertificateOfOriginMandatoryFields>(nonListQueryOperation, iQueryable);



            IQueryable<CertificateOfOriginMandatoryFieldsList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CertificateOfOriginMandatoryFieldsList>(listQueryOperation, query2);
		    query2 = InjectionUtil.Instance.ApplyTreeFilter<CertificateOfOriginMandatoryFieldsList>(query2, treeFilterQueryArgs);

            int count = query2.Count();
            return count;
        }


    }
}
	 