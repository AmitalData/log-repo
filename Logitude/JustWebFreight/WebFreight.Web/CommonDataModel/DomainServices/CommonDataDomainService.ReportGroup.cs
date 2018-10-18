using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        ICommonDataContext commonContext;
        public void UpdateReportGroupList(ReportGroupList currentEntity)
        {
        }

        public IQueryable<ReportGroup> GetReportGroups(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ReportGroup", "READ", tenant);

            reportGroupRepository = new ReportGroupRepository(tenant);
            return reportGroupRepository.GetReportGroups(0);
        }

        public IQueryable<ReportGroupPM> GetReportGroupsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ReportGroup", "READ", tenant);

            reportGroupQuery = new ReportGroupQuery(tenant);
            IQueryable<ReportGroupPM> ReportGroups = reportGroupQuery.GetReportGroupPMsByTenant(tenant);
            return ReportGroups;
        }

        public ReportGroupPM GetSingleReportGroup(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ReportGroup", "READ", tenant);

            commonContext = CommonDataContext.GetContext(tenant);
            reportGroupRepository = new ReportGroupRepository(commonContext);
            reportGroupQuery = new ReportGroupQuery(reportGroupRepository);
            return reportGroupQuery.GetSinglePM(id, tenant);
        }

        public ReportGroupPM GetReportGroupById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ReportGroup", "READ", tenant);


            reportGroupQuery = new ReportGroupQuery(tenant);
            return reportGroupQuery.GetSinglePM(id, tenant);

        }

        public ReportGroupList GetSingleReportGroupList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ReportGroup", "READ", tenant);

            reportGroupRepository = new ReportGroupRepository(tenant);
            reportGroupQuery = new ReportGroupQuery(reportGroupRepository);
            ReportGroupList ReportGroupList = null;
            ReportGroup ReportGroup = reportGroupRepository.GetSingleReportGroup(id, tenant);

            if (ReportGroup != null)
            {
                List<ReportGroup> singleEntityList = new List<ReportGroup>();
                singleEntityList.Add(ReportGroup);

                IQueryable<ReportGroup> iQueryable = singleEntityList.AsQueryable();
                IQueryable<ReportGroupList> iQueryableEntityList = reportGroupQuery.GetIQueryableEntityList(iQueryable);
                ReportGroupList = iQueryableEntityList.FirstOrDefault();
            }
            return ReportGroupList;
        }

        public IQueryable<ReportGroupList> GetReportGroupLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ReportGroup", "READ", tenant);

            reportGroupRepository = new ReportGroupRepository(tenant);
            reportGroupQuery = new ReportGroupQuery(reportGroupRepository);
            IQueryable<ReportGroup> ReportGroups = reportGroupRepository.GetReportGroups(tenant);
            IQueryable<ReportGroupList> query2 = reportGroupQuery.GetIQueryableEntityList(ReportGroups);
            return query2;
        }

        //   [Query(HasSideEffects = true)]
        public IQueryable<ReportGroupList> GetReportGroupFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ReportGroup", "READ", tenant);

            reportGroupRepository = new ReportGroupRepository(tenant);
            reportGroupQuery = new ReportGroupQuery(reportGroupRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ReportGroup> ReportGroups = reportGroupRepository.GetReportGroups(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            ReportGroups = filter.GetFilteredQuery<ReportGroup>(nonListQueryOperation, ReportGroups);
            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ReportGroupList> query2 = reportGroupQuery.GetIQueryableEntityList(ReportGroups);

            query2 = filter.GetFilteredQuery<ReportGroupList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ReportGroupList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> ReportGroupObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ReportGroup", tenant).ToList();

                ObjectField objectField = (from a in ReportGroupObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ReportGroupList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ReportGroupList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ReportGroupList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ReportGroupList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ReportGroupList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.EnglishName);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.EnglishName);
            }
            //--------------------------------------------------------------------------------------------------

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetReportGroupFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ReportGroup", "READ", tenant);

            reportGroupRepository = new ReportGroupRepository(tenant);
           reportGroupQuery = new ReportGroupQuery(reportGroupRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ReportGroup> ReportGroups = reportGroupRepository.GetReportGroups(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            ReportGroups = filter.GetFilteredQuery<ReportGroup>(nonListQueryOperation, ReportGroups);

            IQueryable<ReportGroupList> query2 = reportGroupQuery.GetIQueryableEntityList(ReportGroups);

            query2 = filter.GetFilteredQuery<ReportGroupList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        //public void MapReportGroupReportGroupPM(ReportGroupPM ReportGroupPM, ReportGroup ReportGroup)
        //{
        //    ReportGroup.Name = ReportGroupPM.Name;
        //    ReportGroup.FilterControlName = ReportGroupPM.FilterControlName;
        //    ReportGroup.Description = ReportGroupPM.Description;
        //    ReportGroup.SearchFields = ReportGroupPM.SearchFields;
        //}

        //public void InsertReportGroup(ReportGroupPM ReportGroup)
        //{
        //    SecurityUtility.CheckContactFeature("ReportGroup", "NEW", ReportGroup.Tenant);

        //    if (objectContext == null)
        //    {
        //        objectContext = CommonDataContext.GetContext(ReportGroup.Tenant);
        //    }
        //    ReportGroupService service = new ReportGroupService(objectContext, ReportGroup.Tenant);
        //    service.Create(ReportGroup);


        //    TableLastUpdateClass.UpdateTableHistory(ReportGroup.Tenant, "ReportGroup");
        //}

        //public void UpdateReportGroup(ReportGroupPM currentReportGroup)
        //{
        //    SecurityUtility.CheckContactFeature("ReportGroup", "UPDATE", currentReportGroup.Tenant);

        //    if (objectContext == null)
        //    {
        //        objectContext = CommonDataContext.GetContext(currentReportGroup.Tenant);
        //    }


        //    reportGroupRepository = new reportGroupRepository(objectContext);

        //    ReportGroupService service = new ReportGroupService(objectContext, currentReportGroup.Tenant);
        //    service.Update(currentReportGroup);
        //    TableLastUpdateClass.UpdateTableHistory(currentReportGroup.Tenant, "ReportGroup");


        //}

        public void DeleteReportGroup(ReportGroupPM ReportGroup)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(ReportGroup.Tenant);
            }
            reportGroupRepository = new ReportGroupRepository(objectContext);
            ReportGroup entity = reportGroupRepository.GetSingleReportGroup(ReportGroup.Id, ReportGroup.Tenant);
            reportGroupRepository.Remove(entity);
        }
    }
}