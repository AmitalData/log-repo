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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class TaxWithholdingAssessOfficeListQueryService
    {
         private IAccountingContext context;
        public TaxWithholdingAssessOfficeListQueryService(IAccountingContext context)
        {
            this.context = context;
        }

        public List<TaxWithholdingAssessOfficeList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<TaxWithholdingAssessOffice> iQueryable = (from a in context.TaxWithholdingAssessOffices
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TaxWithholdingAssessOffice>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<TaxWithholdingAssessOfficeList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<TaxWithholdingAssessOfficeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TaxWithholdingAssessOfficeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> TaxWithholdingAssessOfficeObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TaxWithholdingAssessOffice",tenant).ToList();

                ObjectField objectField = (from a in TaxWithholdingAssessOfficeObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<TaxWithholdingAssessOfficeList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<TaxWithholdingAssessOfficeList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TaxWithholdingAssessOfficeList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TaxWithholdingAssessOfficeList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TaxWithholdingAssessOfficeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TaxWithholdingAssessOfficeList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<TaxWithholdingAssessOfficeList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<TaxWithholdingAssessOfficeList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public TaxWithholdingAssessOfficeList GetSingle(string id)
        {
            IQueryable<TaxWithholdingAssessOffice> TaxWithholdingAssessOfficeQuery = (from a in context.TaxWithholdingAssessOffices
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<TaxWithholdingAssessOfficeList> TaxWithholdingAssessOfficeListQuery = GetIqueryableList( TaxWithholdingAssessOfficeQuery);
            TaxWithholdingAssessOfficeList TaxWithholdingAssessOfficeList = TaxWithholdingAssessOfficeListQuery.FirstOrDefault();
            return TaxWithholdingAssessOfficeList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<TaxWithholdingAssessOffice> iQueryable = (from a in context.TaxWithholdingAssessOffices 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<TaxWithholdingAssessOffice>(nonListQueryOperation, iQueryable);

            IQueryable<TaxWithholdingAssessOfficeList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<TaxWithholdingAssessOfficeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 