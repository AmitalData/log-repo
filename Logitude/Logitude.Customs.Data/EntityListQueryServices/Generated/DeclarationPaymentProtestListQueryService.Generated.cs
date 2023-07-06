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

    public partial class DeclarationPaymentProtestListQueryService
    {
         private ICustomContext context;
        public DeclarationPaymentProtestListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<DeclarationPaymentProtestList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DeclarationPaymentProtest> iQueryable = (from a in context.DeclarationPaymentProtests
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DeclarationPaymentProtest>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<DeclarationPaymentProtestList> query2 = GetIqueryableList(iQueryable);
<<<<<<< HEAD
					  query2 = filter.GetFilteredQuery<DeclarationPaymentProtestList>(listQueryOperation, query2);
		
=======
           
            query2 = filter.GetFilteredQuery<DeclarationPaymentProtestList>(listQueryOperation, query2);

>>>>>>> parent of 094118ae335 (#181601)
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DeclarationPaymentProtestList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> DeclarationPaymentProtestObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.DeclarationPaymentProtest",tenant).ToList();

                ObjectField objectField = (from a in DeclarationPaymentProtestObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<DeclarationPaymentProtestList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationPaymentProtestList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationPaymentProtestList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationPaymentProtestList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationPaymentProtestList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationPaymentProtestList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationPaymentProtestList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Line);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.Line);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<DeclarationPaymentProtestList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public DeclarationPaymentProtestList GetSingle(string declarationid, int line)
        {
            IQueryable<DeclarationPaymentProtest> DeclarationPaymentProtestQuery = (from a in context.DeclarationPaymentProtests
                                                       where a.DeclarationId == declarationid && a.Line == line
                                                       select a);

             
            IQueryable<DeclarationPaymentProtestList> DeclarationPaymentProtestListQuery = GetIqueryableList( DeclarationPaymentProtestQuery);
            DeclarationPaymentProtestList DeclarationPaymentProtestList = DeclarationPaymentProtestListQuery.FirstOrDefault();
            return DeclarationPaymentProtestList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DeclarationPaymentProtest> iQueryable = (from a in context.DeclarationPaymentProtests 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<DeclarationPaymentProtest>(nonListQueryOperation, iQueryable);

            IQueryable<DeclarationPaymentProtestList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<DeclarationPaymentProtestList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 