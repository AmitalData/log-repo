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

    public partial class CB_TradeAgreementListQueryService
    {
         private ICustomContext context;
        public CB_TradeAgreementListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<CB_TradeAgreementList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CB_TradeAgreement> iQueryable = (from a in context.CB_TradeAgreements
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CB_TradeAgreement>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CB_TradeAgreementList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CB_TradeAgreementList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CB_TradeAgreementList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CB_TradeAgreementObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CB_TradeAgreement",tenant).ToList();

                ObjectField objectField = (from a in CB_TradeAgreementObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CB_TradeAgreementList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CB_TradeAgreementList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CB_TradeAgreementList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CB_TradeAgreementList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CB_TradeAgreementList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CB_TradeAgreementList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CB_TradeAgreementList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.ID);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderByDescending(d => d.ID);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<CB_TradeAgreementList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CB_TradeAgreementList GetSingle(string id)
        {
            IQueryable<CB_TradeAgreement> CB_TradeAgreementQuery = (from a in context.CB_TradeAgreements
                                                       where a.ID == id
                                                       select a);

             
            IQueryable<CB_TradeAgreementList> CB_TradeAgreementListQuery = GetIqueryableList( CB_TradeAgreementQuery);
            CB_TradeAgreementList CB_TradeAgreementList = CB_TradeAgreementListQuery.FirstOrDefault();
            return CB_TradeAgreementList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CB_TradeAgreement> iQueryable = (from a in context.CB_TradeAgreements  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CB_TradeAgreement>(nonListQueryOperation, iQueryable);

            IQueryable<CB_TradeAgreementList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CB_TradeAgreementList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 