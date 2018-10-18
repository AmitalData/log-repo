using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public IQueryable<BusinessHourPM> GetBusinessHoursByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BusinessHour", "READ", tenant);

            BusinessHourQuery BusinessHourQuery = new BusinessHourQuery(tenant);
            return BusinessHourQuery.GetBusinessHoursPMsByTenant(tenant);
        }

        public BusinessHourPM GetBusinessHourById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BusinessHour", "READ", tenant);

            BusinessHourQuery BusinessHourQuery = new BusinessHourQuery(tenant);
            BusinessHourPM BusinessHour = BusinessHourQuery.GetSinglePM(id, tenant);
            return BusinessHour;
        }

        public BusinessHourList GetSingleBusinessHourList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BusinessHour", "READ", tenant);
            BusinessHourRepository BusinessHourRepository;
            BusinessHourRepository = new BusinessHourRepository(tenant);
            BusinessHourQuery BusinessHourQuery = new BusinessHourQuery(BusinessHourRepository);
            BusinessHourList BusinessHourList = null;
            BusinessHour BusinessHour = BusinessHourRepository.GetSingleBusinessHours(id, tenant);

            if (BusinessHour != null)
            {
                List<BusinessHour> singleEntityList = new List<BusinessHour>();
                singleEntityList.Add(BusinessHour);

                IQueryable<BusinessHour> iQueryable = singleEntityList.AsQueryable();
                IQueryable<BusinessHourList> iQueryableEntityList = BusinessHourQuery.GetIQueryableEntityList(iQueryable);
                BusinessHourList = iQueryableEntityList.FirstOrDefault();
            }
            return BusinessHourList;
        }

        public IQueryable<BusinessHourList> GetBusinessHourList(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BusinessHour", "READ", tenant);
            BusinessHourRepository BusinessHourRepository;
            BusinessHourRepository = new BusinessHourRepository(tenant);
            BusinessHourQuery BusinessHourQuery = new BusinessHourQuery(BusinessHourRepository);
            IQueryable<BusinessHour> BusinessHours = BusinessHourRepository.GetBusinessHours(tenant);
            IQueryable<BusinessHourList> query2 = BusinessHourQuery.GetIQueryableEntityList(BusinessHours);
            return query2;
        }

        public IQueryable<BusinessHourList> GetBusinessHourFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BusinessHour", "READ", tenant);
            BusinessHourRepository BusinessHourRepository;
            BusinessHourRepository = new BusinessHourRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<BusinessHour> BusinessHours = BusinessHourRepository.GetBusinessHours(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            BusinessHours = filter.GetFilteredQuery<BusinessHour>(nonListQueryOperation, BusinessHours);
            int skippedPorts = queryOperations.PageIndex;
            BusinessHourQuery BusinessHourQuery = new BusinessHourQuery(BusinessHourRepository);
            IQueryable<BusinessHourList> query2 = BusinessHourQuery.GetIQueryableEntityList(BusinessHours);

            query2 = filter.GetFilteredQuery<BusinessHourList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(BusinessHourList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> BusinessHourObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("BusinessHour", tenant).ToList();

                ObjectField objectField = (from a in BusinessHourObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<BusinessHourList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<BusinessHourList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<BusinessHourList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<BusinessHourList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<BusinessHourList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Id);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetBusinessHourFiltersCount(byte[] xmlFilters, int tenant)
        {
            BusinessHourRepository BusinessHourRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BusinessHour", "READ", tenant);

            BusinessHourRepository = new BusinessHourRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<BusinessHour> BusinessHours = BusinessHourRepository.GetBusinessHours(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            BusinessHours = filter.GetFilteredQuery<BusinessHour>(nonListQueryOperation, BusinessHours);
            BusinessHourQuery BusinessHourQuery = new BusinessHourQuery(BusinessHourRepository);
            IQueryable<BusinessHourList> query2 = BusinessHourQuery.GetIQueryableEntityList(BusinessHours);

            query2 = filter.GetFilteredQuery<BusinessHourList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertBusinessHour(BusinessHourPM currentBusinessHour)
        {
            SecurityUtility.AuthenticationOnTenant(currentBusinessHour.Tenant);
            SecurityUtility.CheckContactFeature("BusinessHour", "NEW", currentBusinessHour.Tenant);
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentBusinessHour.Tenant);
            }

            BusinessHourService service = new BusinessHourService(objectContext, currentBusinessHour.Tenant);
            service.Create(currentBusinessHour);

            TableLastUpdateClass.UpdateTableHistory(currentBusinessHour.Tenant, "BusinessHour");

        }

        public void UpdateBusinessHour(BusinessHourPM currentBusinessHour)
        {
            SecurityUtility.AuthenticationOnTenant(currentBusinessHour.Tenant);
            SecurityUtility.CheckContactFeature("BusinessHour", "NEW", currentBusinessHour.Tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentBusinessHour.Tenant);
            }

            string entityName = "BusinessHour" + currentBusinessHour.Id + currentBusinessHour.Tenant;
            string entityPmName = "BusinessHourPM" + currentBusinessHour.Id + currentBusinessHour.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            List<BusinessHoursHolidayPM> businessHoursHolidaysChangeSet = ChangeSet.GetAssociatedChanges(currentBusinessHour, d => d.BusinessHoursHolidays).Cast<BusinessHoursHolidayPM>().ToList();
            foreach (BusinessHoursHolidayPM itemPM in businessHoursHolidaysChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            BusinessHourService service = new BusinessHourService(objectContext, currentBusinessHour.Tenant);
            service.SetChangeSet(businessHoursHolidaysChangeSet);

            service.Update(currentBusinessHour);
            TableLastUpdateClass.UpdateTableHistory(currentBusinessHour.Tenant, "BusinessHour");
        }

        public BusinessHourPM GetSingleBusinessHourPM(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BusinessHour", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }

            BusinessHourQuery BusinessHourQuery = new BusinessHourQuery(tenant);
            BusinessHourPM entityPM = BusinessHourQuery.GetSinglePMByTenant(tenant);

            if (entityPM != null)
            {
                entityPM.BusinessHoursHolidays = (from itemPM in objectContext.BusinessHoursHolidays
                                                  where itemPM.Tenant == tenant  && itemPM.BusinessHourId == entityPM.Id
                                                  select new BusinessHoursHolidayPM()
                                                  {
                                                      Id = itemPM.Id,
                                                      Tenant = itemPM.Tenant,
                                                      BusinessHourId = itemPM.BusinessHourId,
                                                      Day = itemPM.Day,
                                                      Month = itemPM.Month,
                                                      Year = itemPM.Year,
                                                      HolidayName = itemPM.HolidayName,
                                                      IsRecurring = itemPM.IsRecurring,
                                                      Inactive = itemPM.Inactive,
                                                      CreateDate = itemPM.CreateDate,
                                                      UpdateDate = itemPM.UpdateDate,
                                                      CreatedByUserId = itemPM.CreatedByUserId,
                                                      UpdatedByUserId = itemPM.UpdatedByUserId,
                                                  }).ToList();
            }
            return entityPM;
        }
    }
}