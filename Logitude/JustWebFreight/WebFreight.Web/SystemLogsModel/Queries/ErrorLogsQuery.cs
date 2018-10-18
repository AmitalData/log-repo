using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Data.Azure.Repositories;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.Azure;
using WebFreight.Web.Security;
using WebFreight.Web.SystemLogsModel.EntityPMs;
using WebFreight.Web.SystemLogsModel.EntityList;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;

namespace WebFreight.Web.SystemLogsModel.Queries
{
    public  class ErrorLogsQuery
    {
        ErrorLogRepository repository;
        public ErrorLogsQuery()
        {
               repository = new ErrorLogRepository(); 
        }

        public ErrorLogsQuery(int tenant)
        {
            repository = new ErrorLogRepository();
        }

        public ErrorLogsQuery(ErrorLogRepository errorLogRepository)
        {
            repository = errorLogRepository;
        }

        public ErrorLogPM GetSinglePM(string id)
        {
            
            ErrorLogPM securedPm = new ErrorLogPM();


            ErrorLog log = repository.GetSingleErrorLog(id);

            ErrorLogPM errorLogs = new ErrorLogPM()
            {
                Id= log.Id,
                Tenant = log.Tenant,
                UserName = log.UserName,
                LogDate = log.LogDate,
                ClientDate = log.ClientDate,
                Tier = log.Tier,
                Exception = log.Exception,
                StackTrace = log.StackTrace,
                SearchFields = log.SearchFields,
                IP=log.IP,

            };

            SecuredMapping.GetMappedPM(errorLogs, securedPm, "ErrorLog", errorLogs.Tenant);

            return securedPm;
        }


        public IQueryable<ErrorLogPM> GetErrorLogsPMsByTenant(int tenant)
        {
            IQueryable<ErrorLogPM> errorLogs = from a in repository.GetAllErrorLogs()
                                                where a.Tenant == tenant
                                                select new ErrorLogPM()
                                                {
                                                  Id =a.Id,
                                                  UserName = a.UserName,
                                                  LogDate = a.LogDate,
                                                  ClientDate = a.ClientDate,
                                                  Tier = a.Tier,
                                                  Exception = a.Exception,
                                                  StackTrace = a.StackTrace,
                                                  SearchFields = a.SearchFields,
                                                  
                                                };
            return errorLogs;
        }


        public IQueryable<ErrorLogList> GetIQueryableEntityList(IQueryable<ErrorLog> iQueryable)
        {
            IQueryable<ErrorLogList> result = from errorLogs in iQueryable
                                               select new ErrorLogList()
                                                  {
                                                      Id = errorLogs.Id,
                                                      Tenant = errorLogs.Tenant,
                                                      UserName = errorLogs.UserName,
                                                      LogDate = errorLogs.LogDate,
                                                      ClientDate = errorLogs.ClientDate,
                                                      Tier = errorLogs.Tier,
                                                      Exception = errorLogs.Exception,
                                                      StackTrace = errorLogs.StackTrace,
                                                      SearchFields = errorLogs.SearchFields,
                                                  };
            return result;
        }

    }
}
