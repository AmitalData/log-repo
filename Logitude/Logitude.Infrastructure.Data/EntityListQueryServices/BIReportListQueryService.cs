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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{
    public partial class BIReportListQueryService
    {
        private IQueryable<BIReportList> GetIqueryableList(IQueryable<BIReport> iQueryable)
        {
            IQueryable<BIReportList> query = (from a in iQueryable.Include("UpdatedByUser").Include("UpdatedByUser.Contact").Include("CreatedByUser").Include("CreatedByUser.Contact")
                                              select new BIReportList()
                                              {
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  CreateDate = a.CreateDate,
                                                  CreatedByUserId = a.CreatedByUserId,
                                                  UpdateDate = a.UpdateDate,
                                                  UpdatedByUserId = a.UpdatedByUserId,
                                                  SearchFields = a.SearchFields,
                                                  Name = a.Name,
                                                  Description = a.Description,
                                                  DWQueryId = a.DWQueryId,
                                                  Inactive = a.Inactive,
                                                  TypeCode = a.TypeCode,
                                                  AGGridOptionsXML = a.AGGridOptionsXML,
                                                  BIReportFolderId = a.BIReportFolderId,
                                                  UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                                                  CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                                                  LastRunDate = a.LastRunDate,
                                                  LastRunByUserName = a.LastRunByUser == null ? null : (a.LastRunByUser.Contact == null ? null : a.LastRunByUser.Contact.EnglishName),
                                              });
            return query;
        }

        private IQueryable<BIReport> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<BIReport> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<BIReport> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<BIReport> iQueryable, int tenant)
        {
            return iQueryable;
        }
        public List<BIReportList> GetAllLists(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<BIReport> iQueryable = (from a in context.BIReports
                                               select a);
            iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable, tenant);
            iQueryable = ApplyCustomFilters(queryOperations, iQueryable, tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<BIReport>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<BIReportList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<BIReportList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(BIReportList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> BIReportObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("BIReport", tenant).ToList();

                ObjectField objectField = (from a in BIReportObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<BIReportList, string>(queryOperations, query2);
                    }
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                                {
                                    query2 = sortClass.GetSorterQuery<BIReportList, string>(queryOperations, query2);
                                    break;
                                }
                            case "sigdouble":
                            case "double":
                                {
                                    query2 = sortClass.GetSorterQuery<BIReportList, double>(queryOperations, query2);
                                    break;
                                }
                            case "date":
                            case "datetime":
                                {
                                    query2 = sortClass.GetSorterQuery<BIReportList, DateTime>(queryOperations, query2);
                                    break;
                                }
                            case "unsinteger":
                            case "integer":
                                {
                                    query2 = sortClass.GetSorterQuery<BIReportList, int>(queryOperations, query2);
                                    break;
                                }
                            case "boolean":
                                {
                                    query2 = sortClass.GetSorterQuery<BIReportList, bool>(queryOperations, query2);
                                    break;
                                }
                            case "unsdecimal":
                            case "decimal":
                                {
                                    query2 = sortClass.GetSorterQuery<BIReportList, decimal>(queryOperations, query2);
                                    break;
                                }
                            default:
                                {
                                    query2 = query2.OrderByDescending(d => d.CreateDate);
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.CreateDate);
            }
            if (!queryOperations.GetAll)
            {
                query2 = query2.Skip(skippedPorts);
                query2 = query2.Take(queryOperations.PageSize);
            }
            return query2.ToList();


        }

        public List<BIReportList> GetAllLists(int tenant)
        {
            return GetAllLists(new QueryOperations() { QueryFilterItems = new List<QueryFilterItem>(), PageIndex = 0, GetAll = true }, tenant);
        }

        public int GetAllListsCount(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<BIReport> iQueryable = (from a in context.BIReports
                                               select a);

            iQueryable = ApplyBusinessUnitFilters(queryOperations, iQueryable, tenant);
            iQueryable = ApplyCustomFilters(queryOperations, iQueryable, tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<BIReport>(nonListQueryOperation, iQueryable);

            IQueryable<BIReportList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<BIReportList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}
	