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
using Logitude.Infrastructure.Data.EntityMapping;
using Logitude.Infrastructure.Data.Repsitories;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{
    public partial class BIReportListQueryService
    {
        private IQueryable<BIReportList> GetIqueryableList(IQueryable<BIReport> iQueryable)
        {
            IQueryable<BIReportList> query = (from a in iQueryable.Include("UpdatedByUser").Include("UpdatedByUser.Contact").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("LastRunDetail")
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
                                                  LastRunDate = a.LastRunDetail == null ? null : (DateTime?)a.LastRunDetail.LastRunDate,
                                                  LastRunByUserName = a.LastRunDetail == null ? null : (a.LastRunDetail.LastRunByUser == null ? null : (a.LastRunDetail.LastRunByUser.Contact == null ? null : a.LastRunDetail.LastRunByUser.Contact.EnglishName)),
                                                  FactTableName = a.FactTableName
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

        public List<BIReportList> GetAllLists(BIReportsFilterArguments bIReportsFilterArguments)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<BIReport> iQueryable;
            if (bIReportsFilterArguments.GetAll) iQueryable = (from a in context.BIReports select a);
            else iQueryable = (from a in context.BIReports where a.Tenant == bIReportsFilterArguments.Tenant select a);

            iQueryable = ApplyBusinessUnitFilters(bIReportsFilterArguments.QueryOperations, iQueryable, bIReportsFilterArguments.Tenant);
            iQueryable = ApplyCustomFilters(bIReportsFilterArguments.QueryOperations, iQueryable, bIReportsFilterArguments.Tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = bIReportsFilterArguments.QueryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = bIReportsFilterArguments.QueryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<BIReport>(nonListQueryOperation, iQueryable);

            int skippedPorts = bIReportsFilterArguments.QueryOperations.PageIndex;

            IQueryable<BIReportList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<BIReportList>(listQueryOperation, query2);

            if (bIReportsFilterArguments.FactTableCodes != null) query2 = query2.Where(x => bIReportsFilterArguments.FactTableCodes.Contains(x.FactTableName));

            if (!string.IsNullOrEmpty(bIReportsFilterArguments.QueryOperations.SortByColumnName) && !string.IsNullOrEmpty(bIReportsFilterArguments.QueryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(BIReportList).GetProperty(bIReportsFilterArguments.QueryOperations.SortByColumnName);
                List<ObjectField> BIReportObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("BIReport", bIReportsFilterArguments.Tenant).ToList();

                ObjectField objectField = (from a in BIReportObjectFields
                                           where a.FieldName == bIReportsFilterArguments.QueryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<BIReportList, string>(bIReportsFilterArguments.QueryOperations, query2);
                    }
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                                {
                                    query2 = sortClass.GetSorterQuery<BIReportList, string>(bIReportsFilterArguments.QueryOperations, query2);
                                    break;
                                }
                            case "sigdouble":
                            case "double":
                                {
                                    query2 = sortClass.GetSorterQuery<BIReportList, double>(bIReportsFilterArguments.QueryOperations, query2);
                                    break;
                                }
                            case "date":
                            case "datetime":
                                {
                                    query2 = sortClass.GetSorterQuery<BIReportList, DateTime>(bIReportsFilterArguments.QueryOperations, query2);
                                    break;
                                }
                            case "unsinteger":
                            case "integer":
                                {
                                    query2 = sortClass.GetSorterQuery<BIReportList, int>(bIReportsFilterArguments.QueryOperations, query2);
                                    break;
                                }
                            case "boolean":
                                {
                                    query2 = sortClass.GetSorterQuery<BIReportList, bool>(bIReportsFilterArguments.QueryOperations, query2);
                                    break;
                                }
                            case "unsdecimal":
                            case "decimal":
                                {
                                    query2 = sortClass.GetSorterQuery<BIReportList, decimal>(bIReportsFilterArguments.QueryOperations, query2);
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
            if (!bIReportsFilterArguments.QueryOperations.GetAll)
            {
                query2 = query2.Skip(skippedPorts);
                query2 = query2.Take(bIReportsFilterArguments.QueryOperations.PageSize);
            }
            return query2.ToList();


        }

        public List<BIReportList> GetAllLists(int tenant)
        {
            return GetAllLists(new BIReportsFilterArguments { QueryOperations = new QueryOperations() { QueryFilterItems = new List<QueryFilterItem>(), PageIndex = 0, GetAll = true }, Tenant = tenant});
        }

        public int GetAllListsCount(BIReportsFilterArguments bIReportsFilterArguments)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<BIReport> iQueryable;
            if (bIReportsFilterArguments.GetAll) iQueryable = (from a in context.BIReports select a);
            else iQueryable = (from a in context.BIReports where a.Tenant == bIReportsFilterArguments.Tenant select a);

            iQueryable = ApplyBusinessUnitFilters(bIReportsFilterArguments.QueryOperations, iQueryable, bIReportsFilterArguments.Tenant);
            iQueryable = ApplyCustomFilters(bIReportsFilterArguments.QueryOperations, iQueryable, bIReportsFilterArguments.Tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = bIReportsFilterArguments.QueryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = bIReportsFilterArguments.QueryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<BIReport>(nonListQueryOperation, iQueryable);

            IQueryable<BIReportList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<BIReportList>(listQueryOperation, query2);
            if (bIReportsFilterArguments.FactTableCodes != null) query2 = query2.Where(x => bIReportsFilterArguments.FactTableCodes.Contains(x.FactTableName));
            int count = query2.Count();
            return count;
        }


        public class BIReportsFilterArguments
        {
            public QueryOperations QueryOperations { get; set; }
            public int Tenant { get; set; }
            public bool GetAll { get; set; } = true;
            public string[] FactTableCodes { get; set; }
        }

    }
}
	