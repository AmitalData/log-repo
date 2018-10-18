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
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        //public IQueryable<EmailAlertSetting> GetEmailAlertSettings(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //   // SecurityUtility.CheckContactFeature("EmailAlertSetting", "READ", tenant);
        //    EmailAlertSettingRepository emailAlertSettingRepository;
        //    emailAlertSettingRepository = new EmailAlertSettingRepository(tenant);
        //    return emailAlertSettingRepository.GetEmailAlertSettings(0);
        //}

        public IQueryable<EmailAlertSettingPM> GetEmailAlertSettingsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("EmailAlertSetting", "READ", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            EmailAlertSettingQuery emailAlertSettingQuery = new EmailAlertSettingQuery(tenant);
            return emailAlertSettingQuery.GetEmailAlertSettingPMsByTenant(tenant);
        }



        public EmailAlertSettingPM GetEmailAlertSettingById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("EmailAlertSetting", "READ", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            EmailAlertSettingQuery emailAlertSettingQuery = new EmailAlertSettingQuery(tenant);
            EmailAlertSettingPM emailAlertSetting = emailAlertSettingQuery.GetSinglePM(id, tenant);
            return emailAlertSetting;
        }

        public EmailAlertSettingList GetSingleEmailAlertSettingList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("EmailAlertSetting", "READ", tenant);
            EmailAlertSettingRepository emailAlertSettingRepository;
            emailAlertSettingRepository = new EmailAlertSettingRepository(tenant);
            EmailAlertSettingQuery emailAlertSettingQuery = new EmailAlertSettingQuery(emailAlertSettingRepository);
            EmailAlertSettingList emailAlertSettingList = null;
            EmailAlertSetting emailAlertSetting = emailAlertSettingRepository.GetSingleEmailAlertSetting(id, tenant);

            if (emailAlertSetting != null)
            {
                List<EmailAlertSetting> singleEntityList = new List<EmailAlertSetting>();
                singleEntityList.Add(emailAlertSetting);

                IQueryable<EmailAlertSetting> iQueryable = singleEntityList.AsQueryable();
                IQueryable<EmailAlertSettingList> iQueryableEntityList = emailAlertSettingQuery.GetIQueryableEntityList(iQueryable);
                emailAlertSettingList = iQueryableEntityList.FirstOrDefault();
            }
            return emailAlertSettingList;
        }

        public IQueryable<EmailAlertSettingList> GetEmailAlertSettingList(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("EmailAlertSetting", "READ", tenant);
            EmailAlertSettingRepository emailAlertSettingRepository;
            emailAlertSettingRepository = new EmailAlertSettingRepository(tenant);
            EmailAlertSettingQuery emailAlertSettingQuery = new EmailAlertSettingQuery(emailAlertSettingRepository);
            IQueryable<EmailAlertSetting> emailAlertSettings = emailAlertSettingRepository.GetEmailAlertSettings(tenant);
            IQueryable<EmailAlertSettingList> query2 = emailAlertSettingQuery.GetIQueryableEntityList(emailAlertSettings);
            return query2;
        }

        public IQueryable<EmailAlertSettingList> GetEmailAlertSettingFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("EmailAlertSetting", "READ", tenant);
            EmailAlertSettingRepository emailAlertSettingRepository;
            emailAlertSettingRepository = new EmailAlertSettingRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<EmailAlertSetting> emailAlertSettings = emailAlertSettingRepository.GetEmailAlertSettings(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            emailAlertSettings = filter.GetFilteredQuery<EmailAlertSetting>(nonListQueryOperation, emailAlertSettings);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            EmailAlertSettingQuery emailAlertSettingQuery = new EmailAlertSettingQuery(emailAlertSettingRepository);
            IQueryable<EmailAlertSettingList> query2 = emailAlertSettingQuery.GetIQueryableEntityList(emailAlertSettings);

            query2 = filter.GetFilteredQuery<EmailAlertSettingList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(EmailAlertSettingList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> EmailAlertSettingObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("EmailAlertSetting", tenant).ToList();

                ObjectField objectField = (from a in EmailAlertSettingObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<EmailAlertSettingList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<EmailAlertSettingList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<EmailAlertSettingList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<EmailAlertSettingList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<EmailAlertSettingList, bool>(queryOperations, query2);
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
            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetEmailAlertSettingFiltersCount(byte[] xmlFilters, int tenant)
        {
            EmailAlertSettingRepository emailAlertSettingRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("EmailAlertSetting", "READ", tenant);

            emailAlertSettingRepository = new EmailAlertSettingRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<EmailAlertSetting> emailAlertSettings = emailAlertSettingRepository.GetEmailAlertSettings(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            emailAlertSettings = filter.GetFilteredQuery<EmailAlertSetting>(nonListQueryOperation, emailAlertSettings);
            EmailAlertSettingQuery EmailAlertSettingQuery = new EmailAlertSettingQuery(emailAlertSettingRepository);
            IQueryable<EmailAlertSettingList> query2 = EmailAlertSettingQuery.GetIQueryableEntityList(emailAlertSettings);

            query2 = filter.GetFilteredQuery<EmailAlertSettingList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertEmailAlertSetting(EmailAlertSettingPM currentEmailAlertSetting)
        {
           // SecurityUtility.CheckContactFeature("EmailAlertSetting", "NEW", currentEmailAlertSetting.Tenant);
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentEmailAlertSetting.Tenant);
            }
            EmailAlertSettingService service = new EmailAlertSettingService(objectContext, currentEmailAlertSetting.Tenant);
            service.Create(currentEmailAlertSetting);

            TableLastUpdateClass.UpdateTableHistory(currentEmailAlertSetting.Tenant, "EmailAlertSetting");



        }

        public void UpdateEmailAlertSetting(EmailAlertSettingPM currentEmailAlertSetting)
        {
           // SecurityUtility.CheckContactFeature("EmailAlertSetting", "UPDATE", currentEmailAlertSetting.Tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentEmailAlertSetting.Tenant);
            }

            string entityName = "EmailAlertSetting" + currentEmailAlertSetting.Id + currentEmailAlertSetting.Tenant;
            string entityPmName = "EmailAlertSettingPM" + currentEmailAlertSetting.Id + currentEmailAlertSetting.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            EmailAlertSettingService service = new EmailAlertSettingService(objectContext, currentEmailAlertSetting.Tenant);
            service.Update(currentEmailAlertSetting);
            TableLastUpdateClass.UpdateTableHistory(currentEmailAlertSetting.Tenant, "EmailAlertSetting");

        }

        public void DeleteEmailAlertSetting(EmailAlertSettingPM EmailAlertSetting)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(EmailAlertSetting.Tenant);
            }
            EmailAlertSettingRepository emailAlertSettingRepository;
            emailAlertSettingRepository = new EmailAlertSettingRepository(objectContext);
            //DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            EmailAlertSetting entity = emailAlertSettingRepository.GetSingleEmailAlertSetting(EmailAlertSetting.Id, EmailAlertSetting.Tenant);
            emailAlertSettingRepository.Remove(entity);
        }

    }
}