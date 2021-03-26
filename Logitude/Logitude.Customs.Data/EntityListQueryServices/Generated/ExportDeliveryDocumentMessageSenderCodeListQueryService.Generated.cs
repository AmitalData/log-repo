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

    public partial class ExportDeliveryDocumentMessageSenderCodeListQueryService
    {
         private ICustomContext context;
        public ExportDeliveryDocumentMessageSenderCodeListQueryService(ICustomContext context)
        {
            this.context = context;
        }

        public List<ExportDeliveryDocumentMessageSenderCodeList> GetList(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ExportDeliveryDocumentMessageSenderCode> iQueryable = (from a in context.ExportDeliveryDocumentMessageSenderCodes
                                               select a);
            			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ExportDeliveryDocumentMessageSenderCode>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ExportDeliveryDocumentMessageSenderCodeList> query2 = GetIqueryableList(iQueryable);
           
            query2 = filter.GetFilteredQuery<ExportDeliveryDocumentMessageSenderCodeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ExportDeliveryDocumentMessageSenderCodeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ExportDeliveryDocumentMessageSenderCodeObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.ExportDeliveryDocumentMessageSenderCode",tenant).ToList();

                ObjectField objectField = (from a in ExportDeliveryDocumentMessageSenderCodeObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
				 if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ExportDeliveryDocumentMessageSenderCodeList, string>(queryOperations, query2);
                    }
                    else
                    {
                     switch (objectField.DataTypeCode.ToLower())
                     {
                         case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ExportDeliveryDocumentMessageSenderCodeList, string>(queryOperations, query2);
                                break;
                            }
						case "sigdouble":
						case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ExportDeliveryDocumentMessageSenderCodeList, double>(queryOperations, query2);
                                break;
                            }
						case "date":
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ExportDeliveryDocumentMessageSenderCodeList, DateTime>(queryOperations, query2);
                                break;
                            }
						case "unsinteger":
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ExportDeliveryDocumentMessageSenderCodeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ExportDeliveryDocumentMessageSenderCodeList, bool>(queryOperations, query2);
                                break;
                            }
						case "unsdecimal":
						case "decimal":
                            {
                                query2 = sortClass.GetSorterQuery<ExportDeliveryDocumentMessageSenderCodeList, decimal>(queryOperations, query2);
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

         public List<ExportDeliveryDocumentMessageSenderCodeList> GetList(int tenant)
         {
             return GetList(new QueryOperations() { QueryFilterItems=new List<QueryFilterItem>(),PageIndex = 0,GetAll = true},tenant);
         }

        public ExportDeliveryDocumentMessageSenderCodeList GetSingle(string code)
        {
            IQueryable<ExportDeliveryDocumentMessageSenderCode> ExportDeliveryDocumentMessageSenderCodeQuery = (from a in context.ExportDeliveryDocumentMessageSenderCodes
                                                       where a.Code == code
                                                       select a);

             
            IQueryable<ExportDeliveryDocumentMessageSenderCodeList> ExportDeliveryDocumentMessageSenderCodeListQuery = GetIqueryableList( ExportDeliveryDocumentMessageSenderCodeQuery);
            ExportDeliveryDocumentMessageSenderCodeList ExportDeliveryDocumentMessageSenderCodeList = ExportDeliveryDocumentMessageSenderCodeListQuery.FirstOrDefault();
            return ExportDeliveryDocumentMessageSenderCodeList;
           
        }

        public int GetListCount(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ExportDeliveryDocumentMessageSenderCode> iQueryable = (from a in context.ExportDeliveryDocumentMessageSenderCodes  select a);

			  			iQueryable = ApplyCustomFilters(queryOperations, iQueryable);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
			iQueryable = filter.GetFilteredQuery<ExportDeliveryDocumentMessageSenderCode>(nonListQueryOperation, iQueryable);

            IQueryable<ExportDeliveryDocumentMessageSenderCodeList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ExportDeliveryDocumentMessageSenderCodeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

      
    }
}
	 