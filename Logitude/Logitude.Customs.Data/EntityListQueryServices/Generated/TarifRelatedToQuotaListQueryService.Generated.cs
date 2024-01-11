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

    public partial class TarifRelatedToQuotaListQueryService
    {
         private ICustomContext context;
        public TarifRelatedToQuotaListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<TarifRelatedToQuotaList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<TarifRelatedToQuota> iQueryable = (from a in context.TarifRelatedToQuotas
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TarifRelatedToQuota>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<TarifRelatedToQuotaList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<TarifRelatedToQuotaList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TarifRelatedToQuotaList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> TarifRelatedToQuotaObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.TarifRelatedToQuota",tenant).ToList();

                ObjectField objectField = (from a in TarifRelatedToQuotaObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<TarifRelatedToQuotaList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<TarifRelatedToQuotaList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TarifRelatedToQuotaList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TarifRelatedToQuotaList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TarifRelatedToQuotaList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TarifRelatedToQuotaList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<TarifRelatedToQuotaList, decimal>(queryOperations, query2);
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

         public List<TarifRelatedToQuotaList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public TarifRelatedToQuotaList GetSingle(string code)
        {
            IQueryable<TarifRelatedToQuota> TarifRelatedToQuotaQuery = (from a in context.TarifRelatedToQuotas
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<TarifRelatedToQuotaList> TarifRelatedToQuotaListQuery = GetIqueryableList( TarifRelatedToQuotaQuery);
            TarifRelatedToQuotaList TarifRelatedToQuotaList = TarifRelatedToQuotaListQuery.FirstOrDefault();
            return TarifRelatedToQuotaList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<TarifRelatedToQuota> iQueryable = (from a in context.TarifRelatedToQuotas  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<TarifRelatedToQuota>(nonListQueryOperation, iQueryable);

            IQueryable<TarifRelatedToQuotaList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<TarifRelatedToQuotaList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 