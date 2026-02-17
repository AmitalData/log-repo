using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateLogitudeMessagesTransmissionLogList(LogitudeMessagesTransmissionLogList currentEntity)
        {
        }

        public LogitudeMessagesTransmissionLogPM GetSingleLogitudeMessagesTransmissionLog(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LogitudeMessagesTransmissionLog", "READ", tenant);

            logitudeMessagesTransmissionLogQuery = new LogitudeMessagesTransmissionLogQuery(tenant);
            return logitudeMessagesTransmissionLogQuery.GetSinglePM(id, tenant);
        }

        public LogitudeMessagesTransmissionLogList GetSingleLogitudeMessagesTransmissionLogList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LogitudeMessagesTransmissionLog", "READ", tenant);

            logitudeMessagesTransmissionLogRepository = new LogitudeMessagesTransmissionLogRepository(tenant);
            logitudeMessagesTransmissionLogQuery = new LogitudeMessagesTransmissionLogQuery(logitudeMessagesTransmissionLogRepository);
            LogitudeMessagesTransmissionLogList LogitudeMessagesTransmissionLogList = null;
            LogitudeMessagesTransmissionLog LogitudeMessagesTransmissionLog = logitudeMessagesTransmissionLogRepository.GetSingleLogitudeMessagesTransmissionLog(tenant, id);

            if (LogitudeMessagesTransmissionLog != null)
            {
                List<LogitudeMessagesTransmissionLog> singleEntityList = new List<LogitudeMessagesTransmissionLog>();
                singleEntityList.Add(LogitudeMessagesTransmissionLog);

                IQueryable<LogitudeMessagesTransmissionLog> iQueryable = singleEntityList.AsQueryable();
                IQueryable<LogitudeMessagesTransmissionLogList> iQueryableEntityList = logitudeMessagesTransmissionLogQuery.GetIQueryableEntityList(iQueryable);
                LogitudeMessagesTransmissionLogList = iQueryableEntityList.FirstOrDefault();
            }
            return LogitudeMessagesTransmissionLogList;
        }

        public IQueryable<LogitudeMessagesTransmissionLogList> GetLogitudeMessagesTransmissionLogLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LogitudeMessagesTransmissionLog", "READ", tenant);

            logitudeMessagesTransmissionLogRepository = new LogitudeMessagesTransmissionLogRepository(tenant);
            logitudeMessagesTransmissionLogQuery = new LogitudeMessagesTransmissionLogQuery(logitudeMessagesTransmissionLogRepository);

            IQueryable<LogitudeMessagesTransmissionLog> logs = logitudeMessagesTransmissionLogRepository.GetLogitudeMessagesTransmissionLogs(tenant);
            IQueryable<LogitudeMessagesTransmissionLogList> query2 = logitudeMessagesTransmissionLogQuery.GetIQueryableEntityList(logs);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<LogitudeMessagesTransmissionLogList> GetLogitudeMessagesTransmissionLogFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LogitudeMessagesTransmissionLog", "READ", tenant);

            logitudeMessagesTransmissionLogRepository = new LogitudeMessagesTransmissionLogRepository(tenant);
            logitudeMessagesTransmissionLogQuery = new LogitudeMessagesTransmissionLogQuery(logitudeMessagesTransmissionLogRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<LogitudeMessagesTransmissionLog> iQueryable = logitudeMessagesTransmissionLogRepository.GetLogitudeMessagesTransmissionLogs(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            LogitudeMessagesTransmissionLogCustomFilter customfilters = new LogitudeMessagesTransmissionLogCustomFilter(tenant);
            iQueryable = customfilters.GetFilteredQuery(queryOperations, iQueryable);

            iQueryable = filter.GetFilteredQuery<LogitudeMessagesTransmissionLog>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<LogitudeMessagesTransmissionLogList> query2 = logitudeMessagesTransmissionLogQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<LogitudeMessagesTransmissionLogList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(LogitudeMessagesTransmissionLogList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LogitudeMessagesTransmissionLog", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<LogitudeMessagesTransmissionLogList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<LogitudeMessagesTransmissionLogList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<LogitudeMessagesTransmissionLogList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<LogitudeMessagesTransmissionLogList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<LogitudeMessagesTransmissionLogList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.SentDate);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.SentDate);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetLogitudeMessagesTransmissionLogFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LogitudeMessagesTransmissionLog", "READ", tenant);

            logitudeMessagesTransmissionLogRepository = new LogitudeMessagesTransmissionLogRepository(tenant);
            logitudeMessagesTransmissionLogQuery = new LogitudeMessagesTransmissionLogQuery(logitudeMessagesTransmissionLogRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<LogitudeMessagesTransmissionLog> iQueryable = logitudeMessagesTransmissionLogRepository.GetLogitudeMessagesTransmissionLogs(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            LogitudeMessagesTransmissionLogCustomFilter customfilters = new LogitudeMessagesTransmissionLogCustomFilter(tenant);
            iQueryable = customfilters.GetFilteredQuery(queryOperations, iQueryable);
            iQueryable = filter.GetFilteredQuery<LogitudeMessagesTransmissionLog>(nonListQueryOperation, iQueryable);

            IQueryable<LogitudeMessagesTransmissionLogList> query2 = logitudeMessagesTransmissionLogQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<LogitudeMessagesTransmissionLogList>(listQueryOperation, query2);

            int count = query2.Take(1001).Count();
            return count;
        }

        public IQueryable<LogitudeMessagesTransmissionLogPM> GetLogitudeMessagesTransmissionLogByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LogitudeMessagesTransmissionLog", "READ", tenant);

            logitudeMessagesTransmissionLogQuery = new LogitudeMessagesTransmissionLogQuery(tenant);
            return logitudeMessagesTransmissionLogQuery.GetLogitudeMessagesTransmissionLogPMsByTenant(tenant);
        }

        public void UpdateLogitudeMessagesTransmissionLog(LogitudeMessagesTransmissionLogPM entityPM)
        {
            
        }

        public List<ChartingDataClass> GetActivityStatusByMessagesLogs(int days, string messageType, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("LogitudeMessagesTransmissionLog", "READ", tenant);

            logitudeMessagesTransmissionLogQuery = new LogitudeMessagesTransmissionLogQuery(tenant);

            List<ChartingDataClass> myResult = logitudeMessagesTransmissionLogQuery.GetActivityStatusByMessagesLogs(days, messageType, tenant);

            return myResult;
        }
    }
}