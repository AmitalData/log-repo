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

    public partial class ImporterPeriodicDeclarationStatusListQueryService
    {
         private ICustomContext context;
        public ImporterPeriodicDeclarationStatusListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<ImporterPeriodicDeclarationStatusList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ImporterPeriodicDeclarationStatus> iQueryable = (from a in context.ImporterPeriodicDeclarationStatuses
                                               select a);
			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ImporterPeriodicDeclarationStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ImporterPeriodicDeclarationStatusList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ImporterPeriodicDeclarationStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ImporterPeriodicDeclarationStatusList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ImporterPeriodicDeclarationStatusObjectFields = ObjectFieldsRepository.GetObjectFieldsByObjectTableName("Customs.ImporterPeriodicDeclarationStatus",tenant).ToList();

                ObjectField objectField = (from a in ImporterPeriodicDeclarationStatusObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ImporterPeriodicDeclarationStatusList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ImporterPeriodicDeclarationStatusList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ImporterPeriodicDeclarationStatusList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ImporterPeriodicDeclarationStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ImporterPeriodicDeclarationStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ImporterPeriodicDeclarationStatusList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ImporterPeriodicDeclarationStatusList, decimal>(queryOperations, query2);
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

         public List<ImporterPeriodicDeclarationStatusList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ImporterPeriodicDeclarationStatusList GetSingle(string code)
        {
            IQueryable<ImporterPeriodicDeclarationStatus> ImporterPeriodicDeclarationStatusQuery = (from a in context.ImporterPeriodicDeclarationStatuses
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<ImporterPeriodicDeclarationStatusList> ImporterPeriodicDeclarationStatusListQuery = GetIqueryableList( ImporterPeriodicDeclarationStatusQuery);
            ImporterPeriodicDeclarationStatusList ImporterPeriodicDeclarationStatusList = ImporterPeriodicDeclarationStatusListQuery.FirstOrDefault();
            return ImporterPeriodicDeclarationStatusList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ImporterPeriodicDeclarationStatus> iQueryable = (from a in context.ImporterPeriodicDeclarationStatuses  select a);

			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<ImporterPeriodicDeclarationStatus>(nonListQueryOperation, iQueryable);

            IQueryable<ImporterPeriodicDeclarationStatusList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ImporterPeriodicDeclarationStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 