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

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class CorrespondencesAttachmentListQueryService
    {
         private ICRMContext context;
        public CorrespondencesAttachmentListQueryService(ICRMContext context)
        {
            this.context = context;
        }

        public List<CorrespondencesAttachmentList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CorrespondencesAttachment> iQueryable = (from a in context.CorrespondencesAttachments
                                              
                   where a.Tenant == tenant select a);
            			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CorrespondencesAttachment>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CorrespondencesAttachmentList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<CorrespondencesAttachmentList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CorrespondencesAttachmentList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CorrespondencesAttachmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CorrespondencesAttachment",tenant).ToList();

                ObjectField objectField = (from a in CorrespondencesAttachmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<CorrespondencesAttachmentList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CorrespondencesAttachmentList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CorrespondencesAttachmentList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CorrespondencesAttachmentList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CorrespondencesAttachmentList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CorrespondencesAttachmentList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<CorrespondencesAttachmentList, decimal>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.CorrespondenceId);
                                break;
                            }
                    }
				 }
                }
            }
		    else
            {
                query2 = query2.OrderBy(d => d.CorrespondenceId);
            }
			if(!queryOperations.GetAll)
			{
             query2 = query2.Skip(skippedPorts);
             query2 = query2.Take(queryOperations.PageSize);
			}
            return query2.ToList();

    
        }

         public List<CorrespondencesAttachmentList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public CorrespondencesAttachmentList GetSingle(string id)
        {
            IQueryable<CorrespondencesAttachment> CorrespondencesAttachmentQuery = (from a in context.CorrespondencesAttachments
                                                       where a.Id == id
                                                       select a);

             
            IQueryable<CorrespondencesAttachmentList> CorrespondencesAttachmentListQuery = GetIqueryableList( CorrespondencesAttachmentQuery);
            CorrespondencesAttachmentList CorrespondencesAttachmentList = CorrespondencesAttachmentListQuery.FirstOrDefault();
            return CorrespondencesAttachmentList;
           
        }

        public int GetListCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CorrespondencesAttachment> iQueryable = (from a in context.CorrespondencesAttachments 
                   where a.Tenant == tenant select a);

			  			iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable,tenant);
						iQueryable = ApplyCustomFilters(queryOperations, iQueryable,tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<CorrespondencesAttachment>(nonListQueryOperation, iQueryable);

            IQueryable<CorrespondencesAttachmentList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<CorrespondencesAttachmentList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 