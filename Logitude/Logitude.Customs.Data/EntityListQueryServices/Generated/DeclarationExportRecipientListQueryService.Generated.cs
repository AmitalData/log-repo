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

    public partial class DeclarationExportRecipientListQueryService
    {
         private ICustomContext context;
        public DeclarationExportRecipientListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<DeclarationExportRecipientList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DeclarationExportRecipient> iQueryable = (from a in context.DeclarationExportRecipients
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DeclarationExportRecipient>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<DeclarationExportRecipientList> query2 = GetIqueryableList(iQueryable);
					  query2 = filter.GetFilteredQuery<DeclarationExportRecipientList>(listQueryOperation, query2);
		
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DeclarationExportRecipientList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> DeclarationExportRecipientObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.DeclarationExportRecipient",tenant).ToList();

                ObjectField objectField = (from a in DeclarationExportRecipientObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<DeclarationExportRecipientList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationExportRecipientList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationExportRecipientList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationExportRecipientList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationExportRecipientList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationExportRecipientList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<DeclarationExportRecipientList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.DeclarationId);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.DeclarationId);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<DeclarationExportRecipientList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public DeclarationExportRecipientList GetSingle(string declarationid, int? linenumber)
        {
            IQueryable<DeclarationExportRecipient> DeclarationExportRecipientQuery = (from a in context.DeclarationExportRecipients
                                                       where a.DeclarationId == declarationid && a.LineNumber == linenumber
                                                       select a);
          
		  
		  			IQueryable<DeclarationExportRecipientList> DeclarationExportRecipientListQuery = GetIqueryableList( DeclarationExportRecipientQuery);
			            DeclarationExportRecipientList DeclarationExportRecipientList = DeclarationExportRecipientListQuery.FirstOrDefault();
            return DeclarationExportRecipientList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DeclarationExportRecipient> iQueryable = (from a in context.DeclarationExportRecipients 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<DeclarationExportRecipient>(nonListQueryOperation, iQueryable);

            IQueryable<DeclarationExportRecipientList> query2 = GetIqueryableList(iQueryable);
			
		    query2 = filter.GetFilteredQuery<DeclarationExportRecipientList>(listQueryOperation, query2);
		            int count = query2.Count();
            return count;
        }

      
    }
}
	 