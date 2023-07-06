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

    public partial class AmendmentTypeListQueryService
    {
         private ICustomContext context;
        public AmendmentTypeListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<AmendmentTypeList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AmendmentType> iQueryable = (from a in context.AmendmentTypes
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AmendmentType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<AmendmentTypeList> query2 = GetIqueryableList(iQueryable);
<<<<<<< HEAD
					  query2 = filter.GetFilteredQuery<AmendmentTypeList>(listQueryOperation, query2);
		
=======
           
            query2 = filter.GetFilteredQuery<AmendmentTypeList>(listQueryOperation, query2);

>>>>>>> parent of 094118ae335 (#181601)
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AmendmentTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> AmendmentTypeObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.AmendmentType",tenant).ToList();

                ObjectField objectField = (from a in AmendmentTypeObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<AmendmentTypeList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AmendmentTypeList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AmendmentTypeList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AmendmentTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AmendmentTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AmendmentTypeList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<AmendmentTypeList, decimal>(queryOperations, query2);
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

         public List<AmendmentTypeList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public AmendmentTypeList GetSingle(string code)
        {
            IQueryable<AmendmentType> AmendmentTypeQuery = (from a in context.AmendmentTypes
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<AmendmentTypeList> AmendmentTypeListQuery = GetIqueryableList( AmendmentTypeQuery);
            AmendmentTypeList AmendmentTypeList = AmendmentTypeListQuery.FirstOrDefault();
            return AmendmentTypeList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AmendmentType> iQueryable = (from a in context.AmendmentTypes  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<AmendmentType>(nonListQueryOperation, iQueryable);

            IQueryable<AmendmentTypeList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<AmendmentTypeList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 