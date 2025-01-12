using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
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
        public ReportExecutionLogQuery(ReportExecutionLogRepository reportExecutionLogRepository)
        {
            repository = reportExecutionLogRepository;
        }
        public ReportExecutionLogPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.ReportExecutionLogs.Include("CommunicationStatusType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("Report")
                    where a.Id == id
                    select new ReportExecutionLogPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreatedByUserId = a.CreatedByUserId,
                        CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                        StatusCode = a.StatusCode,
                        StatusName = a.CommunicationStatusType != null ? a.CommunicationStatusType.Name : null, 
                        ExceptionMessage = a.ExceptionMessage,
                        CreateDate = a.CreateDate,
                        DoneDate = a.DoneDate,
                        ReportFilterXML = a.ReportFilterXML,
                        ReportId = a.ReportId,
                        ReportName = a.Report != null ? a.Report.Name : null,
                        ReportTemplateId = a.ReportTemplateId,
                        RetryNumber = a.RetryNumber,
                        StartDate = a.StartDate,
                        ExecutedByServerName = a.ExecutedByServerName,
                        DisablePreview = a.DisablePreview,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }
      
        public IQueryable<ReportExecutionLogPM> GetReportExecutionLogPMsByTenant(int tenant)
        {
            return (from a in repository.context.ReportExecutionLogs.Include("CommunicationStatusType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("Report")
                    where a.Tenant == tenant
                    select new ReportExecutionLogPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreatedByUserId = a.CreatedByUserId,
                        CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                        StatusCode = a.StatusCode,
                        StatusName = a.CommunicationStatusType != null ? a.CommunicationStatusType.Name : null, 
                        ExceptionMessage = a.ExceptionMessage,
                        CreateDate = a.CreateDate,
                        DoneDate = a.DoneDate,
                        ReportFilterXML = a.ReportFilterXML,
                        ReportId = a.ReportId,
                        ReportName = a.Report != null ? a.Report.Name : null,
                        ReportTemplateId = a.ReportTemplateId,
                        RetryNumber = a.RetryNumber,
                        StartDate = a.StartDate,
                        ExecutedByServerName = a.ExecutedByServerName,
                        DisablePreview = a.DisablePreview,
                        SearchFields = a.SearchFields,
                    });
        }

        public IQueryable<ReportExecutionLogList> GetIQueryableEntityList(IQueryable<ReportExecutionLog> iQueryable)
        {
            IQueryable<ReportExecutionLogList> result = from a in iQueryable.Include("CommunicationStatusType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("Report")
                                                        select new ReportExecutionLogList()
                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            CreatedByUserId = a.CreatedByUserId,
                                                            CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                            StatusCode = a.StatusCode,
                                                            StatusName = a.CommunicationStatusType != null ? a.CommunicationStatusType.Name : null, 
                                                            ExceptionMessage = a.ExceptionMessage,
                                                            CreateDate = a.CreateDate,
                                                            DoneDate = a.DoneDate,
                                                            ReportFilterXML = a.ReportFilterXML,
                                                            ReportId = a.ReportId,
                                                            ReportName = a.Report != null ? a.Report.Name : null,
                                                            ReportTemplateId = a.ReportTemplateId,
                                                            RetryNumber = a.RetryNumber,
                                                            StartDate = a.StartDate,
                                                            ExecutedByServerName = a.ExecutedByServerName,
                                                            DisablePreview = a.DisablePreview,
                                                            SearchFields = a.SearchFields,
                                                        };
            return result;
        }
        public IQueryable<ReportExecutionLogPM> GetReportExecutionLogPMsByTenantAndUserLastWeek(int tenant,string id)
        {
            var oneWeekAgo = DateTime.Now.AddDays(-7);
            (repository.context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            return (from a in repository.context.ReportExecutionLogs.Include("CommunicationStatusType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("Report")
                    where a.Tenant == tenant && a.CreateDate >= oneWeekAgo && a.CreatedByUserId == id   && a.NotDisplayInMenu == false
                    select new ReportExecutionLogPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreatedByUserId = a.CreatedByUserId,
                        CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                        StatusCode = a.StatusCode,
                        StatusName = a.CommunicationStatusType != null ? a.CommunicationStatusType.Name : null,
                        ExceptionMessage = a.ExceptionMessage,
                        CreateDate = a.CreateDate,
                        DoneDate = a.DoneDate,
                        ReportFilterXML = a.ReportFilterXML,
                        ReportId = a.ReportId,
                        ReportName = a.Report != null ? a.Report.Name : null,
                        ReportTemplateId = a.ReportTemplateId,
                        RetryNumber = a.RetryNumber,
                        StartDate = a.StartDate,
                        ExecutedByServerName = a.ExecutedByServerName,
                        DisablePreview = a.DisablePreview,
                        SearchFields = a.SearchFields,
                        NotDisplayInMenu=a.NotDisplayInMenu,
                    });
        }

        public IQueryable<ReportExecutionLogPM> GetReportExecutionLogPMsByIds(List<string> ids, int tenant)
        {
            (repository.context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            return (from a in repository.context.ReportExecutionLogs.Include("CommunicationStatusType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("Report")
                    where ids.Contains(a.Id) && a.Tenant == tenant 
                    select new ReportExecutionLogPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreatedByUserId = a.CreatedByUserId,
                        CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                        StatusCode = a.StatusCode,
                        StatusName = a.CommunicationStatusType != null ? a.CommunicationStatusType.Name : null,
                        ExceptionMessage = a.ExceptionMessage,
                        CreateDate = a.CreateDate,
                        DoneDate = a.DoneDate,
                        ReportFilterXML = a.ReportFilterXML,
                        ReportId = a.ReportId,
                        ReportName = a.Report != null ? a.Report.Name : null,
                        ReportTemplateId = a.ReportTemplateId,
                        RetryNumber = a.RetryNumber,
                        StartDate = a.StartDate,
                        ExecutedByServerName = a.ExecutedByServerName,
                        DisablePreview = a.DisablePreview,
                        SearchFields = a.SearchFields,
                        NotDisplayInMenu = a.NotDisplayInMenu
                    });
        }
        public void DeleteFromMenu(string reportId , int tenant)
        {
            var reportExecutionLogRepository = new ReportExecutionLogRepository(tenant);
            var reportExecutionLog = reportExecutionLogRepository.GetReportExecutionLog(reportId);
            reportExecutionLog.NotDisplayInMenu = true;
            reportExecutionLogRepository.Update(reportExecutionLog);
            reportExecutionLogRepository.SubmitChanges();


        }
    }
}