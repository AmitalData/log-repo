using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;
namespace Logitude.BL.CommonDataModel.EntityQueries
{ 
    public class ReportExecutionLogQuery
    {
        ReportExecutionLogRepository repository;
        public ReportExecutionLogQuery()
        {
            repository = new ReportExecutionLogRepository();
        }
        public ReportExecutionLogQuery(int tenant)
        {
            repository = new ReportExecutionLogRepository(tenant);
        }
        public ReportExecutionLogQuery(ReportExecutionLogRepository communicationLogRepository)
        {
            repository = communicationLogRepository;
        }
        public ReportExecutionLogPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.ReportExecutionLogs.Include("CommunicationStatusType").Include("CreatedByUser.Contact").Include("ObjectTable").Include("CurrentTenant")
                    where a.Id == id
                    select new ReportExecutionLogPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,  
                        CreatedByUserId = a.CreatedByUserId,
                        StatusCode = a.StatusCode, 
                        ExceptionMessage = a.ExceptionMessage, 
                        CreateDate = a.CreateDate,
                        DoneDate = a.DoneDate,
                        ReportFilterXML = a.ReportFilterXML,
                        ReportId = a.ReportId,
                        ReportTemplateId = a.ReportTemplateId,
                        RetryNumber = a.RetryNumber,
                        StartDate = a.StartDate,
                        ExecutedByServerName = a.ExecutedByServerName,
                        DisablePreview = a.DisablePreview, 
                    }).FirstOrDefault();
        }
      
        public IQueryable<ReportExecutionLogPM> GetReportExecutionLogPMsByTenant(int tenant)
        {
            return (from a in repository.context.ReportExecutionLogs.Include("CommunicationStatusType").Include("CreatedByUser.Contact").Include("ObjectTable").Include("CurrentTenant")
                    where a.Tenant == tenant
                    select new ReportExecutionLogPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreatedByUserId = a.CreatedByUserId,
                        StatusCode = a.StatusCode,
                        ExceptionMessage = a.ExceptionMessage,
                        CreateDate = a.CreateDate,
                        DoneDate = a.DoneDate,
                        ReportFilterXML = a.ReportFilterXML,
                        ReportId = a.ReportId,
                        ReportTemplateId = a.ReportTemplateId,
                        RetryNumber = a.RetryNumber,
                        StartDate = a.StartDate,
                        ExecutedByServerName = a.ExecutedByServerName,
                        DisablePreview = a.DisablePreview,
                    });
        }
        public IQueryable<ReportExecutionLogList> GetIQueryableEntityList(IQueryable<ReportExecutionLog> iQueryable)
        {
            IQueryable<ReportExecutionLogList> result = from a in iQueryable.Include("CommunicationStatusType").Include("CreatedByUser.Contact").Include("ObjectTable").Include("CurrentTenant")
                                                      select new ReportExecutionLogList()
                                                      {
                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          CreatedByUserId = a.CreatedByUserId,
                                                          StatusCode = a.StatusCode,
                                                          ExceptionMessage = a.ExceptionMessage,
                                                          CreateDate = a.CreateDate,
                                                          DoneDate = a.DoneDate,
                                                          ReportFilterXML = a.ReportFilterXML,
                                                          ReportId = a.ReportId,
                                                          ReportTemplateId = a.ReportTemplateId,
                                                          RetryNumber = a.RetryNumber,
                                                          StartDate = a.StartDate,
                                                          ExecutedByServerName = a.ExecutedByServerName,
                                                          DisablePreview = a.DisablePreview,
                                                      };
            return result;
        }
         
    }
}