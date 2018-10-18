using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        //public IQueryable<BusinessHoursHolidayPM> GetBusinessHoursHolidaysByTenant(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("BusinessHoursHoliday", "READ", tenant);

        //    BusinessHoursHolidayQuery BusinessHoursHolidayQuery = new BusinessHoursHolidayQuery(tenant);
        //    return BusinessHoursHolidayQuery.GetBusinessHoursHolidaysPMsByTenant(tenant);
        //}

        //public BusinessHoursHolidayPM GetBusinessHoursHolidayById(string id, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("BusinessHoursHoliday", "READ", tenant);

        //    BusinessHoursHolidayQuery BusinessHoursHolidayQuery = new BusinessHoursHolidayQuery(tenant);
        //    BusinessHoursHolidayPM BusinessHoursHoliday = BusinessHoursHolidayQuery.GetSinglePM(id, tenant);
        //    return BusinessHoursHoliday;
        //}

        //public BusinessHoursHolidayList GetSingleBusinessHoursHolidayList(string id, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("BusinessHoursHoliday", "READ", tenant);
        //    BusinessHoursHolidayRepository BusinessHoursHolidayRepository;
        //    BusinessHoursHolidayRepository = new BusinessHoursHolidayRepository(tenant);
        //    BusinessHoursHolidayQuery BusinessHoursHolidayQuery = new BusinessHoursHolidayQuery(BusinessHoursHolidayRepository);
        //    BusinessHoursHolidayList BusinessHoursHolidayList = null;
        //    BusinessHoursHoliday BusinessHoursHoliday = BusinessHoursHolidayRepository.GetSingleBusinessHoursHolidays(id, tenant);

        //    if (BusinessHoursHoliday != null)
        //    {
        //        List<BusinessHoursHoliday> singleEntityList = new List<BusinessHoursHoliday>();
        //        singleEntityList.Add(BusinessHoursHoliday);

        //        IQueryable<BusinessHoursHoliday> iQueryable = singleEntityList.AsQueryable();
        //        IQueryable<BusinessHoursHolidayList> iQueryableEntityList = BusinessHoursHolidayQuery.GetIQueryableEntityList(iQueryable);
        //        BusinessHoursHolidayList = iQueryableEntityList.FirstOrDefault();
        //    }
        //    return BusinessHoursHolidayList;
        //}

        //public IQueryable<BusinessHoursHolidayList> GetBusinessHoursHolidayList(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("BusinessHoursHoliday", "READ", tenant);
        //    BusinessHoursHolidayRepository BusinessHoursHolidayRepository;
        //    BusinessHoursHolidayRepository = new BusinessHoursHolidayRepository(tenant);
        //    BusinessHoursHolidayQuery BusinessHoursHolidayQuery = new BusinessHoursHolidayQuery(BusinessHoursHolidayRepository);
        //    IQueryable<BusinessHoursHoliday> BusinessHoursHolidays = BusinessHoursHolidayRepository.GetBusinessHoursHolidays(tenant);
        //    IQueryable<BusinessHoursHolidayList> query2 = BusinessHoursHolidayQuery.GetIQueryableEntityList(BusinessHoursHolidays);
        //    return query2;
        //}

        //public IQueryable<BusinessHoursHolidayList> GetBusinessHoursHolidayFilters(byte[] xmlFilters, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("BusinessHoursHoliday", "READ", tenant);
        //    BusinessHoursHolidayRepository BusinessHoursHolidayRepository;
        //    BusinessHoursHolidayRepository = new BusinessHoursHolidayRepository(tenant);
        //    MemoryStream memorystream = new MemoryStream(xmlFilters);
        //    XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
        //    QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
        //    GenericFilter filter = new GenericFilter();
        //    GenericSort sortClass = new GenericSort();
        //    IQueryable<BusinessHoursHoliday> BusinessHoursHolidays = BusinessHoursHolidayRepository.GetBusinessHoursHolidays(tenant);

        //    QueryOperations nonListQueryOperation = new QueryOperations();
        //    nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
        //    QueryOperations listQueryOperation = new QueryOperations();
        //    listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

        //    BusinessHoursHolidays = filter.GetFilteredQuery<BusinessHoursHoliday>(nonListQueryOperation, BusinessHoursHolidays);
        //    int skippedPorts = queryOperations.PageIndex;
        //    BusinessHoursHolidayQuery BusinessHoursHolidayQuery = new BusinessHoursHolidayQuery(BusinessHoursHolidayRepository);
        //    IQueryable<BusinessHoursHolidayList> query2 = BusinessHoursHolidayQuery.GetIQueryableEntityList(BusinessHoursHolidays);

        //    query2 = filter.GetFilteredQuery<BusinessHoursHolidayList>(listQueryOperation, query2);

        //    if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
        //    {
        //        PropertyInfo propInfo = typeof(BusinessHoursHolidayList).GetProperty(queryOperations.SortByColumnName);
        //        List<ObjectField> BusinessHoursHolidayObjectFields = ObjectFieldsRepository.GetObjectFieldsByObjectTableName("BusinessHoursHoliday", tenant).ToList();

        //        ObjectField objectField = (from a in BusinessHoursHolidayObjectFields
        //                                   where a.FieldName == queryOperations.SortByColumnName
        //                                   select a).FirstOrDefault();

        //        if (objectField != null)
        //        {
        //            switch (objectField.DataTypeCode.ToLower())
        //            {
        //                case "text":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<BusinessHoursHolidayList, string>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "double":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<BusinessHoursHolidayList, double>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "datetime":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<BusinessHoursHolidayList, DateTime>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "integer":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<BusinessHoursHolidayList, int>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "boolean":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<BusinessHoursHolidayList, bool>(queryOperations, query2);
        //                        break;
        //                    }
        //                default:
        //                    {
        //                        query2 = query2.OrderByDescending(d => d.Id);
        //                        break;
        //                    }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        query2 = query2.OrderByDescending(d => d.Id);
        //    }

        //    query2 = query2.Skip(skippedPorts);
        //    query2 = query2.Take(queryOperations.PageSize);
        //    return query2;
        //}

        //public int GetBusinessHoursHolidayFiltersCount(byte[] xmlFilters, int tenant)
        //{
        //    BusinessHoursHolidayRepository BusinessHoursHolidayRepository;
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("BusinessHoursHoliday", "READ", tenant);

        //    BusinessHoursHolidayRepository = new BusinessHoursHolidayRepository(tenant);

        //    MemoryStream memorystream = new MemoryStream(xmlFilters);
        //    XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
        //    QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
        //    GenericFilter filter = new GenericFilter();
        //    GenericSort sortClass = new GenericSort();
        //    IQueryable<BusinessHoursHoliday> BusinessHoursHolidays = BusinessHoursHolidayRepository.GetBusinessHoursHolidays(tenant);

        //    QueryOperations nonListQueryOperation = new QueryOperations();
        //    nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
        //    QueryOperations listQueryOperation = new QueryOperations();
        //    listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
        //    BusinessHoursHolidays = filter.GetFilteredQuery<BusinessHoursHoliday>(nonListQueryOperation, BusinessHoursHolidays);
        //    BusinessHoursHolidayQuery BusinessHoursHolidayQuery = new BusinessHoursHolidayQuery(BusinessHoursHolidayRepository);
        //    IQueryable<BusinessHoursHolidayList> query2 = BusinessHoursHolidayQuery.GetIQueryableEntityList(BusinessHoursHolidays);

        //    query2 = filter.GetFilteredQuery<BusinessHoursHolidayList>(listQueryOperation, query2);
        //    int count = query2.Count();
        //    return count;
        //}

        //public void InsertBusinessHoursHoliday(BusinessHoursHolidayPM currentBusinessHoursHoliday)
        //{
        //    SecurityUtility.AuthenticationOnTenant(currentBusinessHoursHoliday.Tenant);
        //    SecurityUtility.CheckContactFeature("BusinessHoursHoliday", "NEW", currentBusinessHoursHoliday.Tenant);
        //    if (objectContext == null)
        //    {
        //        objectContext = WebFreightContext.GetContext(currentBusinessHoursHoliday.Tenant);
        //    }
            
        //    BusinessHoursHolidayService service = new BusinessHoursHolidayService(objectContext, currentBusinessHoursHoliday.Tenant);
        //    service.Create(currentBusinessHoursHoliday);

        //    TableLastUpdateClass.UpdateTableHistory(currentBusinessHoursHoliday.Tenant, "BusinessHoursHoliday");

        //}

        //public void UpdateBusinessHoursHoliday(BusinessHoursHolidayPM currentBusinessHoursHoliday)
        //{
        //    SecurityUtility.AuthenticationOnTenant(currentBusinessHoursHoliday.Tenant);
        //    SecurityUtility.CheckContactFeature("BusinessHoursHoliday", "NEW", currentBusinessHoursHoliday.Tenant);

        //    if (objectContext == null)
        //    {
        //        objectContext = WebFreightContext.GetContext(currentBusinessHoursHoliday.Tenant);
        //    }

        //    string entityName = "BusinessHoursHoliday" + currentBusinessHoursHoliday.Id + currentBusinessHoursHoliday.Tenant;
        //    string entityPmName = "BusinessHoursHolidayPM" + currentBusinessHoursHoliday.Id + currentBusinessHoursHoliday.Tenant;
        //    if (CacheManager.CacheWrapper.Get(entityName) != null)
        //    {
        //        CacheManager.CacheWrapper.Invalidate(entityName);
        //    }
        //    if (CacheManager.CacheWrapper.Get(entityPmName) != null)
        //    {
        //        CacheManager.CacheWrapper.Invalidate(entityPmName);
        //    }

        //    BusinessHoursHolidayService service = new BusinessHoursHolidayService(objectContext, currentBusinessHoursHoliday.Tenant);
        //    service.Update(currentBusinessHoursHoliday);
        //    TableLastUpdateClass.UpdateTableHistory(currentBusinessHoursHoliday.Tenant, "BusinessHoursHoliday");
        //}

        //public List<BusinessHoursHolidayList> GetHolidaysListByBusinessHourId(string businessId, int tenant)
        //{
        //    if (objectContext == null)
        //    {
        //        objectContext = WebFreightContext.GetContext(tenant);
        //    }

        //    BusinessHoursHolidayQuery listService = new BusinessHoursHolidayQuery(tenant);
        //    List<BusinessHoursHolidayList> myResult = listService.GetBusinessHoursHolidayListByBusinessHourId(businessId, tenant);

        //    CustomFieldResolver customFieldResolver = new CustomFieldResolver();
        //    customFieldResolver.SetCustomFieldsValues("BusinessHoursHoliday", tenant, myResult.Cast<object>().ToList());
        //    return myResult.ToList();

        //}
    }
}