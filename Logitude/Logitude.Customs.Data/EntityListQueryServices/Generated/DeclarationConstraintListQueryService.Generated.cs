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

    public partial class DeclarationConstraintListQueryService
    {
         private ICustomContext context;
        public DeclarationConstraintListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<DeclarationConstraintList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DeclarationConstraint> iQueryable = (from a in context.DeclarationConstraints
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DeclarationConstraint>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<DeclarationConstraintList> query2 = GetIqueryableList(iQueryable);
					  query2 = filter.GetFilteredQuery<DeclarationConstraintList>(listQueryOperation, query2);
		
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DeclarationConstraintList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> DeclarationConstraintObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.DeclarationConstraint",tenant).ToList();

                ObjectField objectField = (from a in DeclarationConstraintObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<DeclarationConstraintList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationConstraintList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationConstraintList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationConstraintList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationConstraintList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationConstraintList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationConstraintList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.DeclarationID);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.DeclarationID);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<DeclarationConstraintList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public DeclarationConstraintList GetSingle(string declarationid, string constraintnumber)
        {
            IQueryable<DeclarationConstraint> DeclarationConstraintQuery = (from a in context.DeclarationConstraints
                                                       where a.DeclarationID == declarationid && a.ConstraintNumber == constraintnumber
                                                       select a);

             
            IQueryable<DeclarationConstraintList> DeclarationConstraintListQuery = GetIqueryableList( DeclarationConstraintQuery);
            DeclarationConstraintList DeclarationConstraintList = DeclarationConstraintListQuery.FirstOrDefault();
            return DeclarationConstraintList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DeclarationConstraint> iQueryable = (from a in context.DeclarationConstraints 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<DeclarationConstraint>(nonListQueryOperation, iQueryable);

            IQueryable<DeclarationConstraintList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<DeclarationConstraintList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 