using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.SystemLogs;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.SystemLogsModel.EntityList;
using WebFreight.Web.SystemLogsModel.EntityPMs;
using WebFreight.Web.SystemLogsModel.Queries;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;

namespace WebFreight.Web.SystemLogsModel.DomainServices
{
   [EnableClientAccess()]
    public class SystemLogsDomainService: LogitudeDomainService
    {
        ISystemLogContext systemLogContext;

        private ErrorLogRepository errorLogRepository;

        public SystemLogsDomainService()
        {
            
        }


        private void MapErrorLogsErrorLogsPM(ErrorLogPM errorLogPm, ErrorLog errorLog)
        {


            errorLog.Tenant = errorLogPm.Tenant;
            errorLog.UserName = errorLogPm.UserName;
            errorLog.LogDate = errorLogPm.LogDate;
            errorLog.ClientDate = errorLogPm.ClientDate;
            errorLog.Tier = errorLogPm.Tier;
            errorLog.Exception = errorLogPm.Exception;
            errorLog.StackTrace = errorLogPm.StackTrace;
            errorLog.IP = errorLogPm.IP;
            errorLog.SearchFields = errorLogPm.Tier + "," + errorLogPm.UserName + "," + errorLogPm.Exception + "," + errorLogPm.Tenant+","+errorLogPm.IP;

            if (!string.IsNullOrEmpty(errorLog.Exception) && errorLog.Exception.Length > 7000)
            {
                errorLog.Exception = errorLog.Exception.Substring(0, 7000);
        }
            if (!string.IsNullOrEmpty(errorLog.StackTrace) && errorLog.StackTrace.Length > 7000)
            {
                errorLog.StackTrace = errorLog.StackTrace.Substring(0, 7000);
            }
            if (!string.IsNullOrEmpty(errorLog.SearchFields) && errorLog.SearchFields.Length > 8000)
            {
                errorLog.SearchFields = errorLog.SearchFields.Substring(0, 8000);
            }

        }
        ErrorLogsQuery errorLogsQuery;
        public ErrorLogPM GetSingleErrorLogPM(string id)
        {
            systemLogContext=SystemLogContext.GetContext();
            errorLogRepository = new ErrorLogRepository(systemLogContext);
            errorLogsQuery = new ErrorLogsQuery(errorLogRepository);
            return errorLogsQuery.GetSinglePM(id);
        }

      
        public IQueryable<ErrorLog> GetErrorLogs()
        {
            systemLogContext = SystemLogContext.GetContext();
            errorLogRepository = new ErrorLogRepository(systemLogContext);
           
            return errorLogRepository.GetAllErrorLogs();
        }

        public ErrorLog GetSingleErrorLog(string id)
        {
            systemLogContext = SystemLogContext.GetContext();
            errorLogRepository = new ErrorLogRepository(systemLogContext);
            
            return errorLogRepository.GetSingleErrorLog(id);
        }


        //[Query(HasSideEffects = true)]

        public IQueryable<ErrorLogList> GetErrorLogsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ErrorLog", "READ", tenant);

            systemLogContext = SystemLogContext.GetContext();
            errorLogRepository = new ErrorLogRepository(systemLogContext);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ErrorLog> iQueryable = errorLogRepository.GetAllErrorLogs();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ErrorLog>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;


            IQueryable<ErrorLogList> query2 = (from a in iQueryable
                                               select new ErrorLogList()
                                               {
                                                   Id = a.Id,
                                                   UserName = a.UserName,
                                                   LogDate = a.LogDate,
                                                   ClientDate = a.ClientDate,
                                                   Tier = a.Tier,
                                                   Exception = a.Exception,
                                                   StackTrace = a.StackTrace,
                                                   SearchFields = a.SearchFields,
                                                   Tenant = a.Tenant,
                                                   IP = a.IP,
                                               }).AsQueryable();


            query2 = filter.GetFilteredQuery<ErrorLogList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ErrorLogList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> errorLogsObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ErrorLog", tenant).ToList();

                ObjectField objectField = (from a in errorLogsObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ErrorLogList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ErrorLogList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ErrorLogList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ErrorLogList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ErrorLogList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.LogDate);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.LogDate);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }


        public int GetErrorLogFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ErrorLog", "READ", tenant);
            systemLogContext = SystemLogContext.GetContext();
            errorLogRepository = new ErrorLogRepository(systemLogContext);
            errorLogsQuery = new ErrorLogsQuery(errorLogRepository);
    
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ErrorLog> errorLogs = errorLogRepository.GetAllErrorLogs();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            errorLogs = filter.GetFilteredQuery<ErrorLog>(nonListQueryOperation, errorLogs);

            IQueryable<ErrorLogList> query2 = errorLogsQuery.GetIQueryableEntityList(errorLogs);

            query2 = filter.GetFilteredQuery<ErrorLogList>(listQueryOperation, query2);
            int count = query2.Take(1001).Count();
            return count;
        }
        //public int GetErrorLogsFiltersCount(byte[] xmlFilters, int tenant)
        //{
            
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("ErrorLog", "READ", tenant);
        //    MemoryStream memorystream = new MemoryStream(xmlFilters);
        //    XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
        //    QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
        //    GenericFilter filter = new GenericFilter();
        //    GenericSort sortClass = new GenericSort();

        //    IQueryable<ErrorLog> iQueryable = errorLogRepository.GetAllErrorLogs();
            

        //    QueryOperations nonListQueryOperation = new QueryOperations();
        //    nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
        //    QueryOperations listQueryOperation = new QueryOperations();
        //    listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

        //    iQueryable = filter.GetFilteredQuery<ErrorLog>(nonListQueryOperation, iQueryable);
        //    List<ErrorLog> errorlogs = iQueryable.ToList();
        //    IQueryable<ErrorLogList> query2 = (from a in errorlogs
        //                 select new ErrorLogList()
        //                 {
        //                    Id= a.Id,
        //                    UserName = a.UserName,
        //                    LogDate = a.LogDate,
        //                    ClientDate = a.ClientDate,
        //                    Tier = a.Tier,
        //                    Exception = a.Exception,
        //                    StackTrace = a.StackTrace,
        //                    SearchField = a.SearchField //a.Tier+","+a.UserName+","+a.UserId,

        //                 }).AsQueryable();


        //    query2 = filter.GetFilteredQuery<ErrorLogList>(listQueryOperation, query2);
           
        //    int count = errorlogs.Count();
        //    return count;
        //}

        public IQueryable<ErrorLogList> GetErrorLogsLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ErrorLog", "READ", tenant);
            systemLogContext = SystemLogContext.GetContext();
            errorLogRepository = new ErrorLogRepository(systemLogContext);
            IQueryable<ErrorLog> tenants = errorLogRepository.GetAllErrorLogs();


            var query2 = from a in tenants
                         select new ErrorLogList()
                         {
                            Id =a.Id,
                            UserName =a.UserName,
                            LogDate =a.LogDate,
                            ClientDate =a.ClientDate,
                            Tier =a.Tier ,
                            Exception =a.Exception,
                            StackTrace =a.StackTrace,
                            SearchFields =a.SearchFields,
                            Tenant =a.Tenant,

                            IP = a.IP,
                         };
            return query2;
        }

       
        public IQueryable<ErrorLogPM> GetErrorLogsPMs(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ErrorLog", "READ", tenant);
            systemLogContext = SystemLogContext.GetContext();
            errorLogRepository = new ErrorLogRepository(systemLogContext);
            errorLogsQuery = new ErrorLogsQuery(errorLogRepository);
            return errorLogsQuery.GetErrorLogsPMsByTenant(tenant);
        }
        public void UpdateErrorLogsList(ErrorLogList currentEntity)
        {
        }


        public ErrorLogList GetSingleErrorLogsList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            systemLogContext = SystemLogContext.GetContext();
            errorLogRepository = new ErrorLogRepository(systemLogContext);

            ErrorLog errorLogs = errorLogRepository.GetSingleErrorLog(id);

            if (errorLogs != null)
            {
                ErrorLogList errorLogsList = new ErrorLogList()
                {
                   Id = errorLogs.Id,
                   UserName = errorLogs.UserName,
                   LogDate = errorLogs.LogDate,
                   ClientDate = errorLogs.ClientDate,
                   Tier = errorLogs.Tier,
                   Exception =errorLogs.Exception,
                   StackTrace = errorLogs.StackTrace,
                   Tenant =errorLogs.Tenant,
                   SearchFields = errorLogs.SearchFields,
                   IP = errorLogs.IP,
                };
                return errorLogsList;
            }
            else
            {
                return null;
            }
        }


        public void AddErrorLog(ErrorLogPM entity)
        {
            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                ErrorLog errorLogs = new ErrorLog();
                if (systemLogContext == null)
                {
                    systemLogContext = SystemLogContext.GetContext();

                }
                errorLogRepository = new ErrorLogRepository(systemLogContext);
                if (entity.Id == null)
                {
                    entity.Id = Guid.NewGuid().ToString();
                }

                try
                {
                    if (!systemLogContext.ErrorLogs.Where(a => a.Id == entity.Id).Any())
                    {
                        errorLogs.Id = entity.Id;
                        entity.LogDate = DateTime.Now;
                        string ip = "";
                        if (HttpContext.Current != null && HttpContext.Current.Request != null)
                        {
                            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                            if (string.IsNullOrEmpty(currentIP))
                            {
                                currentIP = HttpContext.Current.Request.UserHostAddress;
                            }
                            ip = currentIP;
                        }
                        entity.IP = ip;
                        MapErrorLogsErrorLogsPM(entity, errorLogs);
                        errorLogRepository.Add(errorLogs);

                        errorLogRepository.SubmitChanges();
                    }
                }
                catch (Exception ex){
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.Message.Contains("Violation of PRIMARY KEY constraint") || ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                        {
                            entity.Id = Guid.NewGuid().ToString();
                            errorLogRepository.SubmitChanges();
                        }
                    }
                    else
                        throw ex;
                    //Cannot insert duplicate key in object 
                
                }
              //  scope.Complete();
            }
        }

        public void DeleteMyChildEntity(ErrorLogPM entity)
        {

        }

        public void UpdateMyChildEntity(ErrorLogPM entity)
        {
            if (systemLogContext == null)
            {
                systemLogContext = SystemLogContext.GetContext();

            }
            errorLogRepository = new ErrorLogRepository(systemLogContext);
            ErrorLog errorLogs = errorLogRepository.GetSingleErrorLog(entity.Id);
            MapErrorLogsErrorLogsPM(entity, errorLogs);
            errorLogRepository.Update(errorLogs);
            errorLogRepository.SubmitChanges();
        }
       
        //public override System.Collections.IEnumerable Query(QueryDescription queryDescription, out IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> validationErrors, out int totalCount)
        //{
        //    return base.Query(queryDescription, out validationErrors, out totalCount);
        //}

        //protected override bool PersistChangeSet()
        //{
        //    SaveChanges();
        //    return base.PersistChangeSet();
        //}

        //public void SaveChanges()
        //{
        //    try
        //    {
        //        this.systemLogContext.SaveChanges();
        //    }
        //    catch
        //    {
 
        //    }
        //}

     
    }
}