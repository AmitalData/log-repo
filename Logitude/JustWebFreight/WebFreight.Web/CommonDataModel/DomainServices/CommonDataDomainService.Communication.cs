using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.Azure;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using System.Text;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.BL.EntityQueryServices;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        
        #region CommunicationLogs
        public void UpdateCommunicationLogList(CommunicationLogList currentEntity)
        {
        }

        public IQueryable<CommunicationLog> GetCommunicationLogs(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CommunicationLog", "READ", tenant);

            communicationLogRepository = new CommunicationLogRepository(tenant);
            return communicationLogRepository.GetCommunicationLogsByTenant(0);
        }

        public CommunicationLogPM GetSingleCommunicationLog(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CommunicationLog", "READ", tenant);

            communicationLogQuery = new CommunicationLogQuery(tenant);
            return communicationLogQuery.GetSinglePM(id, tenant);
        }

        public IQueryable<CommunicationLogPM> GetCommunicationLogPMsByEntityId(string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CommunicationLog", "READ", tenant);

            communicationLogQuery = new CommunicationLogQuery(tenant);
            return communicationLogQuery.GetCommunicationLogPMsByEntityId(entityId, tenant);
        }

        public CommunicationLogList GetSingleCommunicationLogList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CommunicationLog", "READ", tenant);

            communicationLogRepository = new CommunicationLogRepository(tenant);
            communicationLogQuery = new CommunicationLogQuery(communicationLogRepository);
            CommunicationLogList communicationLogList = null;
            CommunicationLog communicationLog = communicationLogRepository.GetSingleCommunicationLog(id, tenant);

            if (communicationLog != null)
            {
                List<CommunicationLog> singleEntityList = new List<CommunicationLog>();
                singleEntityList.Add(communicationLog);

                IQueryable<CommunicationLog> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CommunicationLogList> iQueryableEntityList = communicationLogQuery.GetIQueryableEntityList(iQueryable);
                communicationLogList = iQueryableEntityList.FirstOrDefault();
            }
            return communicationLogList;
        }

        public IQueryable<CommunicationLogList> GetCommunicationLogLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CommunicationLog", "READ", tenant);

            communicationLogRepository = new CommunicationLogRepository(tenant);
            communicationLogQuery = new CommunicationLogQuery(communicationLogRepository);

            IQueryable<CommunicationLog> logs = communicationLogRepository.GetCommunicationLogsByTenant(tenant);
            IQueryable<CommunicationLogList> query2 = communicationLogQuery.GetIQueryableEntityList(logs);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CommunicationLogList> GetCommunicationLogFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CommunicationLog", "READ", tenant);

            communicationLogRepository = new CommunicationLogRepository(tenant);
            communicationLogQuery = new CommunicationLogQuery(communicationLogRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CommunicationLog> iQueryable = communicationLogRepository.GetCommunicationLogsByTenant(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            iQueryable = filter.GetFilteredQuery<CommunicationLog>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CommunicationLogList> query2 = communicationLogQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CommunicationLogList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CommunicationLogList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CommunicationLog", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogList, bool>(queryOperations, query2);
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

            else
            {
                query2 = query2.OrderByDescending(d => d.CreateDate);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetCommunicationLogFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CommunicationLog", "READ", tenant);

            communicationLogRepository = new CommunicationLogRepository(tenant);
            communicationLogQuery = new CommunicationLogQuery(communicationLogRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CommunicationLog> iQueryable = communicationLogRepository.GetCommunicationLogsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CommunicationLog>(nonListQueryOperation, iQueryable);

            IQueryable<CommunicationLogList> query2 = communicationLogQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CommunicationLogList>(listQueryOperation, query2);

           int count = query2.Take(1001).Count();
            return count;
        }

        public IQueryable<CommunicationLogPM> GetCommunicationLogsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CommunicationLog", "READ", tenant);

            communicationLogQuery = new CommunicationLogQuery(tenant);
            return communicationLogQuery.GetCommunicationLogPMsByTenant(tenant);
        }

        public void InsertCommunicationLog(CommunicationLogPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CommunicationLogService service = new CommunicationLogService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "CommunicationLog");
        }

        public void UpdateCommunicationLog(CommunicationLogPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CommunicationLogService service = new CommunicationLogService(objectContext, entityPM.Tenant);
            service.Update(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "CommunicationLog");
        }

        public void DeleteCommunicationLog(CommunicationLogPM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }

            communicationLogRepository = new CommunicationLogRepository(objectContext);
            CommunicationLog deletedEntity = communicationLogRepository.GetSingleCommunicationLog(entity.Id, entity.Tenant);
            communicationLogRepository.Remove(deletedEntity);
        }
        #endregion

        #region CommunicationLogType
        public IQueryable<CommunicationLogType> GetCommunicationLogTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationLogTypeRepository = new CommunicationLogTypeRepository(tenant);
            return communicationLogTypeRepository.GetCommunicationLogTypes();
        }

        public CommunicationLogTypePM GetSingleCommunicationLogType(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationLogTypeQuery = new CommunicationLogTypeQuery(tenant);
            return communicationLogTypeQuery.GetSingleCommunicationLogTypePM(code);
        }

        public CommunicationLogTypeList GetSingleCommunicationLogTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationLogTypeRepository = new CommunicationLogTypeRepository(tenant);
            communicationLogTypeQuery = new CommunicationLogTypeQuery(communicationLogTypeRepository);

            CommunicationLogTypeList communicationLogTypeList = null;
            CommunicationLogType communicationLogType = communicationLogTypeRepository.GetSingleCommunicationLogType(code);

            if (communicationLogType != null)
            {
                List<CommunicationLogType> singleEntityList = new List<CommunicationLogType>();
                singleEntityList.Add(communicationLogType);

                IQueryable<CommunicationLogType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CommunicationLogTypeList> iQueryableEntityList = communicationLogTypeQuery.GetIQueryableEntityList(iQueryable);
                communicationLogTypeList = iQueryableEntityList.FirstOrDefault();
            }
            return communicationLogTypeList;
        }

        public IQueryable<CommunicationLogTypeList> GetCommunicationLogTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationLogTypeRepository = new CommunicationLogTypeRepository(tenant);
            communicationLogTypeQuery = new CommunicationLogTypeQuery(communicationLogTypeRepository);

            IQueryable<CommunicationLogType> iQueryable = communicationLogTypeRepository.GetCommunicationLogTypes();
            IQueryable<CommunicationLogTypeList> query2 = communicationLogTypeQuery.GetIQueryableEntityList(iQueryable);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CommunicationLogTypeList> GetCommunicationLogTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationLogTypeRepository = new CommunicationLogTypeRepository(tenant);
            communicationLogTypeQuery = new CommunicationLogTypeQuery(communicationLogTypeRepository);
            
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CommunicationLogType> iQueryable = communicationLogTypeRepository.GetCommunicationLogTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CommunicationLogType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CommunicationLogTypeList> query2 = communicationLogTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CommunicationLogTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CommunicationLogTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CommunicationLogType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogTypeList, bool>(queryOperations, query2);
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

        public int GetCommunicationLogTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationLogTypeRepository = new CommunicationLogTypeRepository(tenant);
            communicationLogTypeQuery = new CommunicationLogTypeQuery(communicationLogTypeRepository);
            
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CommunicationLogType> iQueryable = communicationLogTypeRepository.GetCommunicationLogTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CommunicationLogType>(nonListQueryOperation, iQueryable);

            IQueryable<CommunicationLogTypeList> query2 = communicationLogTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CommunicationLogTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertCommunicationLogType(CommunicationLogType entity)
        {
            communicationLogTypeRepository.Add(entity);
        }

        public void UpdateCommunicationLogType(CommunicationLogType currentEntity)
        {
            communicationLogTypeRepository.Update(currentEntity);
        }

        public void DeleteCommunicationLogType(CommunicationLogType entity)
        {
            communicationLogTypeRepository.Remove(entity);
        }
        #endregion

        #region CommunicationStatusType
        public IQueryable<CommunicationStatusType> GetCommunicationStatusTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationStatusTypeRepository = new CommunicationStatusTypeRepository(tenant);
            return communicationStatusTypeRepository.GetCommunicationStatusTypes();
        }

        public CommunicationStatusTypePM GetSingleCommunicationStatusType(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationStatusTypeQuery = new CommunicationStatusTypeQuery(tenant);
            return communicationStatusTypeQuery.GetSingleCommunicationLogTypePM(code);
        }

        public CommunicationStatusTypeList GetSingleCommunicationStatusTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationStatusTypeRepository = new CommunicationStatusTypeRepository(tenant);
            CommunicationStatusTypeList communicationStatusTypeList = null;
            CommunicationStatusType communicationStatusType = communicationStatusTypeRepository.GetSingleCommunicationStatusType(code);

            if (communicationStatusType != null)
            {
                List<CommunicationStatusType> singleEntityList = new List<CommunicationStatusType>();
                singleEntityList.Add(communicationStatusType);

                communicationStatusTypeQuery = new CommunicationStatusTypeQuery(communicationStatusTypeRepository);
                IQueryable<CommunicationStatusType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CommunicationStatusTypeList> iQueryableEntityList = communicationStatusTypeQuery.GetIQueryableEntityList(iQueryable);
                communicationStatusTypeList = iQueryableEntityList.FirstOrDefault();
            }
            return communicationStatusTypeList;
        }

        public IQueryable<CommunicationStatusTypeList> GetCommunicationStatusTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationStatusTypeRepository = new CommunicationStatusTypeRepository(tenant);
            communicationStatusTypeQuery = new CommunicationStatusTypeQuery(communicationStatusTypeRepository);
            
            IQueryable<CommunicationStatusType> iQueryable = communicationStatusTypeRepository.GetCommunicationStatusTypes();
            IQueryable<CommunicationStatusTypeList> query2 = communicationStatusTypeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CommunicationStatusTypeList> GetCommunicationStatusTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationStatusTypeRepository = new CommunicationStatusTypeRepository(tenant);
            communicationStatusTypeQuery = new CommunicationStatusTypeQuery(communicationStatusTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CommunicationStatusType> iQueryable = communicationStatusTypeRepository.GetCommunicationStatusTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CommunicationStatusType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CommunicationStatusTypeList> query2 = communicationStatusTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CommunicationStatusTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CommunicationStatusTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CommunicationStatusType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationStatusTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationStatusTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationStatusTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationStatusTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationStatusTypeList, bool>(queryOperations, query2);
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

        public int GetCommunicationStatusTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationStatusTypeRepository = new CommunicationStatusTypeRepository(tenant);
            communicationStatusTypeQuery = new CommunicationStatusTypeQuery(communicationStatusTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CommunicationStatusType> iQueryable = communicationStatusTypeRepository.GetCommunicationStatusTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CommunicationStatusType>(nonListQueryOperation, iQueryable);

            IQueryable<CommunicationStatusTypeList> query2 = communicationStatusTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CommunicationStatusTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertCommunicationStatusType(CommunicationStatusType entity)
        {
            communicationStatusTypeRepository.Add(entity);
        }

        public void UpdateCommunicationStatusType(CommunicationStatusType currentEntity)
        {
            communicationStatusTypeRepository.Update(currentEntity);
        }

        public void DeleteCommunicationStatusType(CommunicationStatusType entity)
        {
            communicationStatusTypeRepository.Remove(entity);
        }
        #endregion

        #region CommunicationAttachments
        public void UpdateCommunicationAttachmentList(CommunicationAttachmentList currentEntity)
        {
        }

        public IQueryable<CommunicationAttachment> GetCommunicationAttachments(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationAttachmentRepository = new CommunicationAttachmentRepository(tenant);
            return communicationAttachmentRepository.GetCommunicationAttachments(0);
        }

        public IQueryable<CommunicationAttachment> GetCommunicationAttachmentsForCommLog(string comLogId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationAttachmentRepository = new CommunicationAttachmentRepository(tenant);
            return communicationAttachmentRepository.GetCommunicationAttachmentsForCommLog(comLogId, tenant);
        }

        public CommunicationAttachmentPM GetSingleCommunicationAttachment(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationAttachmentQuery = new CommunicationAttachmentQuery(tenant);
            return communicationAttachmentQuery.GetSinglePM(id, tenant);
        }

        public CommunicationAttachmentList GetSingleCommunicationAttachmentList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationAttachmentRepository = new CommunicationAttachmentRepository(tenant);
            communicationAttachmentQuery = new CommunicationAttachmentQuery(communicationAttachmentRepository);

            CommunicationAttachmentList communicationAttachmentList = null;
            CommunicationAttachment communicationAttachment = communicationAttachmentRepository.GetSingleCommunicationAttachment(id, tenant);

            if (communicationAttachment != null)
            {
                List<CommunicationAttachment> singleEntityList = new List<CommunicationAttachment>();
                singleEntityList.Add(communicationAttachment);

                IQueryable<CommunicationAttachment> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CommunicationAttachmentList> iQueryableEntityList = communicationAttachmentQuery.GetIQueryableEntityList(iQueryable);
                communicationAttachmentList = iQueryableEntityList.FirstOrDefault();
            }
            return communicationAttachmentList;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CommunicationAttachmentList> GetCommunicationAttachmentFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationAttachmentRepository = new CommunicationAttachmentRepository(tenant);
            communicationAttachmentQuery = new CommunicationAttachmentQuery(communicationAttachmentRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CommunicationAttachment> iQueryable = communicationAttachmentRepository.GetCommunicationAttachments(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            iQueryable = filter.GetFilteredQuery<CommunicationAttachment>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;
            IQueryable<CommunicationAttachmentList> query2 = communicationAttachmentQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CommunicationAttachmentList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CommunicationAttachmentList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CommunicationAttachment", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationAttachmentList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationAttachmentList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationAttachmentList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationAttachmentList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationAttachmentList, bool>(queryOperations, query2);
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

        public int GetCommunicationAttachmentFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationAttachmentRepository = new CommunicationAttachmentRepository(tenant);
            communicationAttachmentQuery = new CommunicationAttachmentQuery(communicationAttachmentRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CommunicationAttachment> iQueryable = communicationAttachmentRepository.GetCommunicationAttachments(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CommunicationAttachment>(nonListQueryOperation, iQueryable);
            IQueryable<CommunicationAttachmentList> query2 = communicationAttachmentQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CommunicationAttachmentList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public IQueryable<CommunicationAttachmentPM> GetCommunicationAttachmentsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationAttachmentQuery = new CommunicationAttachmentQuery(tenant);
            return communicationAttachmentQuery.GetCommunicationAttachmentPMsByTenant(tenant);
        }

        public IQueryable<CommunicationAttachmentPM> GetCommunicationAttachmentsByLogId(string logId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationAttachmentQuery = new CommunicationAttachmentQuery(tenant);
            return communicationAttachmentQuery.GetCommunicationAttachmentPMsByLogId(logId, tenant);
        }

        public void InsertCommunicationAttachment(CommunicationAttachmentPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CommunicationAttachmentService service = new CommunicationAttachmentService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "CommunicationAttachment");
        }

        public void UpdateCommunicationAttachment(CommunicationAttachmentPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CommunicationAttachmentService service = new CommunicationAttachmentService(objectContext, entityPM.Tenant);
            service.Update(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "CommunicationAttachment");
        }

        public void DeleteCommunicationAttachment(CommunicationAttachmentPM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }

            communicationAttachmentRepository = new CommunicationAttachmentRepository(objectContext);
            CommunicationAttachment deletedEntity = communicationAttachmentRepository.GetSingleCommunicationAttachment(entity.Id, entity.Tenant);
            communicationAttachmentRepository.Remove(deletedEntity);
        }
        #endregion

        #region CommunicationLogSteps
        public void UpdateCommunicationLogStepList(CommunicationLogStepList currentEntity)
        {
        }



        public IQueryable<CommunicationLogStep> GetCommunicationLogStepsForCommLog(string comLogId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationLogStepRepository = new CommunicationLogStepRepository(tenant);
            return communicationLogStepRepository.GetCommunicationStepsForCommLog(comLogId, tenant);
        }

        public CommunicationLogStepPM GetSingleCommunicationLogStep(string id,int stepNumber ,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationLogStepQuery = new CommunicationLogStepQuery(tenant);
            return communicationLogStepQuery.GetSinglePM(id,stepNumber, tenant);
        }

        public CommunicationLogStepList GetSingleCommunicationLogStepList(string id, int stepNumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationLogStepRepository = new CommunicationLogStepRepository(tenant);
           communicationLogStepQuery = new CommunicationLogStepQuery(communicationLogStepRepository);

            CommunicationLogStepList CommunicationLogStepList = null;
            CommunicationLogStep CommunicationLogStep = communicationLogStepRepository.CommunicationLogStep(id,stepNumber, tenant);

            if (CommunicationLogStep != null)
            {
                List<CommunicationLogStep> singleEntityList = new List<CommunicationLogStep>();
                singleEntityList.Add(CommunicationLogStep);

                IQueryable<CommunicationLogStep> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CommunicationLogStepList> iQueryableEntityList =communicationLogStepQuery.GetIQueryableEntityList(iQueryable);
                CommunicationLogStepList = iQueryableEntityList.FirstOrDefault();
            }
            return CommunicationLogStepList;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CommunicationLogStepList> GetCommunicationLogStepFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationLogStepRepository = new CommunicationLogStepRepository(tenant);
            communicationLogStepQuery = new CommunicationLogStepQuery(communicationLogStepRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CommunicationLogStep> iQueryable = communicationLogStepRepository.GetCommunicationSteps(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            iQueryable = filter.GetFilteredQuery<CommunicationLogStep>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;
            IQueryable<CommunicationLogStepList> query2 =communicationLogStepQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CommunicationLogStepList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CommunicationLogStepList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CommunicationLogStep", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogStepList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogStepList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogStepList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogStepList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CommunicationLogStepList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Name);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetCommunicationLogStepFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

           communicationLogStepRepository = new CommunicationLogStepRepository(tenant);
            communicationLogStepQuery = new CommunicationLogStepQuery(communicationLogStepRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CommunicationLogStep> iQueryable = communicationLogStepRepository.GetCommunicationSteps(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CommunicationLogStep>(nonListQueryOperation, iQueryable);
            IQueryable<CommunicationLogStepList> query2 = communicationLogStepQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CommunicationLogStepList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public IQueryable<CommunicationLogStepPM> GetCommunicationLogStepsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationLogStepQuery = new CommunicationLogStepQuery(tenant);
            return communicationLogStepQuery.GetCommunicationLogStepPMsByTenant(tenant);
        }

        public IQueryable<CommunicationLogStepPM> GetCommunicationLogStepsByLogId(string logId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationLogStepQuery = new CommunicationLogStepQuery(tenant);
            return communicationLogStepQuery.GetCommunicationLogStepPMsByLogId(logId, tenant);
        }

        public List<CommunicationLogStepList> GetCommunicationLogStepsListsByLogId(string logId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            communicationLogStepQuery = new CommunicationLogStepQuery(tenant);
            return communicationLogStepQuery.GetCommunicationLogStepListsByLogId(logId, tenant);
        }
        public List<CommunicationLogStepList> GetLogStepsRequestParamResponseDataByEntityId2(int tenant, string InterfaceTypeCode, string RequestStatusCode,string ObjectTableId2 , string EntityId2)
        {

            
            var listService = new CustomsRequestsSheetQueryService(tenant);
            var CustomsRequestsSheet = listService.GetByEntityId2(tenant, InterfaceTypeCode, RequestStatusCode, ObjectTableId2, EntityId2);
            if (CustomsRequestsSheet == null)
            {
                return null;
            }
            var logid = CustomsRequestsSheet.RequestComminicationId;
            var stepFilter = new CustomsStepEnum[] { CustomsStepEnum.StartRequestParams, CustomsStepEnum.AnalyzeResponseData };
            var myFilter = new int[] { 0, 30 };
            return GetCommunicationLogStepsDocumentData(logid, tenant, myFilter);
        }

        public List<CommunicationLogStepList> GetCommunicationLogStepsRequestParamResponseData(string logId, int tenant)
        {

            var stepFilter = new CustomsStepEnum[] { CustomsStepEnum.StartRequestParams, CustomsStepEnum.AnalyzeResponseData };
            var myFilter = new int[] { 0, 30 };
            return GetCommunicationLogStepsDocumentData(logId, tenant, myFilter);
        }
        public List<CommunicationLogStepList> GetCommunicationLogStepsDocumentData(string communicationLogId, int tenant, int[] stepFilter)
        {
            var communicationLogStepQuery = new CommunicationLogStepQuery(tenant);

            List<CommunicationLogStepList> stepLIstOut = communicationLogStepQuery.GetCommunicationLogStepsDocumentData(communicationLogId, tenant, stepFilter);
            return stepLIstOut;
#if false
           
            //var myFilter = new int[] { 0, 30 };
            var stepNumberList = stepFilter.Select(step => (int)step).ToList(); 
            
            
            var stepList = communicationLogStepRepository.GetMultiCommunicationLog(communicationLogId, tenant);
            //var stepList = GetCommunicationLogStepsListsByLogId(logId, tenant);
            var filter = stepList.Where(rec => stepNumberList.Contains(rec.StepNumber)).ToList();
            var stepLIstOut = new List<CommunicationLogStepList>();
            foreach (var step in filter)
            {
                byte[] ArryByte = null;
                var stepListVersion = CommunicationLogStepQuery.GetStepListVersion(step);
                if (GetBlob(tenant, step.Document, out ArryByte))
                {
                    string xml = Encoding.UTF8.GetString(ArryByte);
                    stepListVersion.DocumentData = xml;
                }

                stepLIstOut.Add(stepListVersion);


            }
#endif

        }
   
  
        #endregion
    }
}