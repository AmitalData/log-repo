using Logitude.SystemLogs;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.SystemLogsModel.EntityList;
using WebFreight.Web.SystemLogsModel.EntityPMs;
using WebFreight.Web.SystemLogsModel.Queries;
using WebFreight.Web.SystemLogsModel.Tools;
using WebFreight.Web.SystemLogsModel.Tools.EntityService;

namespace WebFreight.Web.SystemLogsModel.DomainServices
{
    [EnableClientAccess()]
    public class BatchServicesLogDomainService:LogitudeDomainService
    {

        ISystemLogContext systemLogContext;
        private BatchServicesLogQuery batchServicesLogQuery;
        private BatchServicesLogRepository batchServicesLogRepository;
        public BatchServicesLogDomainService()
        {
            if (systemLogContext == null)
            {
                systemLogContext = SystemLogContext.GetContext();
            }

            batchServicesLogRepository = new BatchServicesLogRepository(systemLogContext);
        }




        public BatchServicesLogPM GetSingleBatchServicesLogPM(string entityId)
        {

         
           
            batchServicesLogQuery = new BatchServicesLogQuery(batchServicesLogRepository);
            BatchServicesLogPM batchServicesLog = batchServicesLogQuery.GetSinglePM(entityId);

            return batchServicesLog;
        }

        public BatchServicesLogList GetSingleBatchServicesLogList(string id)
        {


          
            batchServicesLogQuery = new BatchServicesLogQuery(batchServicesLogRepository);

            BatchServicesLogList entityList = null;
            BatchServicesLog entityPOCO = batchServicesLogRepository.GetSingleBatchServicesLog(id);

            if (entityPOCO != null)
            {
                List<BatchServicesLog> singleEntityList = new List<BatchServicesLog>();
                singleEntityList.Add(entityPOCO);

                IQueryable<BatchServicesLog> iQueryable = singleEntityList.AsQueryable();
                IQueryable<BatchServicesLogList> iQueryableEntityList = batchServicesLogQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public void UpdateBatchServicesLogList(BatchServicesLogList entityList)
        {

        }

        public List<BatchServicesLogList> GetBatchServicesLogLists()
        {
          
     
            batchServicesLogQuery = new BatchServicesLogQuery(batchServicesLogRepository);

            IQueryable<BatchServicesLog> iQueryable = batchServicesLogRepository.GetBatchServicesLogs();
            IQueryable<BatchServicesLogList> query2 =batchServicesLogQuery.GetIQueryableEntityList(iQueryable);

            List<BatchServicesLogList> myResult = query2.ToList();
            return myResult;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<BatchServicesLogList> GetBatchServicesLogFilters(byte[] xmlFilters, int tenant)
        {
      

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

         
            batchServicesLogQuery = new BatchServicesLogQuery(batchServicesLogRepository);

            IQueryable<BatchServicesLog> iQueryable = batchServicesLogRepository.GetBatchServicesLogs();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<BatchServicesLog>(nonListQueryOperation, iQueryable);
            int skippedEntities = queryOperations.PageIndex;

            IQueryable<BatchServicesLogList> query2 = batchServicesLogQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<BatchServicesLogList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(BatchServicesLogList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> objectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("BatchServicesLog", tenant).ToList();

                ObjectField objectField = (from a in objectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesLogList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesLogList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesLogList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesLogList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<BatchServicesLogList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.LastActivity);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.LastActivity);
            }

            query2 = query2.Skip(skippedEntities);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetBatchServicesLogFiltersCount(byte[] xmlFilters, int tenant)
        {
       

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

          
           batchServicesLogQuery = new BatchServicesLogQuery(batchServicesLogRepository);

            IQueryable<BatchServicesLog> iQueryable = batchServicesLogRepository.GetBatchServicesLogs();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            iQueryable = filter.GetFilteredQuery<BatchServicesLog>(nonListQueryOperation, iQueryable);

            IQueryable<BatchServicesLogList> query2 =batchServicesLogQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<BatchServicesLogList>(listQueryOperation, query2);
            int count = query2.Count();

            return count;
        }

        public void InsertBatchServicesLog(BatchServicesLogPM entityPM)
        {
            if (systemLogContext == null)
            {
                systemLogContext = SystemLogContext.GetContext();
            }


            BatchServicesLogUpdateService service = new BatchServicesLogUpdateService(systemLogContext);
            service.Create(entityPM);

            
        }

        public void UpdateBatchServicesLog(BatchServicesLogPM entityPM)
        {
            if (systemLogContext == null)
            {
                systemLogContext = SystemLogContext.GetContext();
            }


            BatchServicesLogUpdateService service = new BatchServicesLogUpdateService(systemLogContext);
            service.Update(entityPM);

          
        }

        public IQueryable<BatchServicesLogList> GetBatchServicesLogListsByCode(string Code,DateTime? LastActivity)
        {
            batchServicesLogQuery = new BatchServicesLogQuery(batchServicesLogRepository);
            List<BatchServicesLog> BatchServicesLogList = batchServicesLogRepository.GetBatchServicesLogsByCode(Code, LastActivity);
            IQueryable<BatchServicesLogList> query2 = batchServicesLogQuery.GetIQueryableEntityList(BatchServicesLogList.AsQueryable());

            //List<BatchServicesLogList> myResult = query2.ToList();
            return query2;
        }
    }
}