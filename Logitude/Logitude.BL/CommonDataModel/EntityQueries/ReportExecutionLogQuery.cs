using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Helpers;
using Logitude.BL.Security;
using Logitude.Customs.Data;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                        ReportLocalName = a.Report != null ? a.Report.LocalName : null,

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
                        ReportLocalName = a.Report != null ? a.Report.LocalName : null,

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
                                                            ReportLocalName = a.Report != null ? a.Report.LocalName : null,
                                                            ReportTemplateId = a.ReportTemplateId,
                                                            RetryNumber = a.RetryNumber,
                                                            StartDate = a.StartDate,
                                                            ExecutedByServerName = a.ExecutedByServerName,
                                                            DisablePreview = a.DisablePreview,
                                                            SearchFields = a.SearchFields,
                                                        };
            return result;
        }
        public List<ReportMenuClass> GetReportByTenantAndUserLastWeek(int tenant, string id)
        {
            var oneWeekAgo = DateTime.Now.AddDays(-7);
            (repository.context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            var reportLogs = repository.context.ReportExecutionLogs
                .Include(a => a.Report)
                .Where(a => a.Tenant == tenant && a.CreateDate >= oneWeekAgo && a.CreatedByUserId == id && !a.NotDisplayInMenu)
                .Select(a => new ReportMenuClass
                {   Id=a.Id,
                    ReportId = a.ReportId,
                    StatusCode = a.StatusCode,
                    ExceptionMessage = a.ExceptionMessage,
                    CreateDate = a.CreateDate,
                    ReportFilterXML = a.ReportFilterXML,
                    ReportName = a.Report != null ? a.Report.Name : null,
                    ReportLocalName = a.Report != null ? a.Report.LocalName : null,
                    ReportTemplateId = a.ReportTemplateId,
                    NotDisplayInMenu = a.NotDisplayInMenu,
                    ItemType =(int) MenuTypes.ReportExecutionLog
                }).ToList();

            var batchTasks = InfrastructureContext.GetContext(tenant).BatchTaskExecutions
                .Where(a =>( a.Subject == "Create a new Tax Report" || a.Subject== "Cancel Tax Report") && a.Tenant == tenant && a.CreatedByUserId == id && a.CreateDate >= oneWeekAgo && !a.NotDisplayInMenu)
                .AsEnumerable() 
                .Select(a => new ReportMenuClass
                {
                    Id = a.Id,
                    StatusCode = a.StatusCode,
                    ExceptionMessage = a.ErrorLog,
                    CreateDate = a.CreateDate,
                    ReportFilterXML = a.PrametersXml,

                    ReportName = TextCodesTranslator.TranslateText($"Accounting.General.O.{a.Subject.Trim()}", tenant, false),
                    ReportLocalName = TextCodesTranslator.TranslateText($"Accounting.General.O.{a.Subject.Trim()}", tenant, true),
                    NotDisplayInMenu = a.NotDisplayInMenu,
                    ItemType = (int)MenuTypes.BatchTaskExecution
                }).ToList();

            return reportLogs.Concat(batchTasks).ToList();
        }

        public List<ReportMenuClass> GetReporByIds(List<string> ids, int tenant)
        {
            (repository.context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            var reportLogs = repository.context.ReportExecutionLogs
                .Include(a => a.Report)
                .Where(a => ids.Contains(a.Id) && a.Tenant == tenant)
                .Select(a => new ReportMenuClass
                {   Id = a.Id,
                    ReportId = a.ReportId,
                    StatusCode = a.StatusCode,
                    ExceptionMessage = a.ExceptionMessage,
                    CreateDate = a.CreateDate,
                    ReportFilterXML = a.ReportFilterXML,
                    ReportName = a.Report != null ? a.Report.Name : null,
                    ReportLocalName = a.Report != null ? a.Report.LocalName : null,
                    ReportTemplateId = a.ReportTemplateId,
                    NotDisplayInMenu = a.NotDisplayInMenu,
                    ItemType = (int)MenuTypes.ReportExecutionLog
                }).ToList();

            var batchTasks = InfrastructureContext.GetContext(tenant).BatchTaskExecutions
                .Where(a => ids.Contains(a.Id) && a.Tenant == tenant)
                .AsEnumerable()
                .Select(a => new ReportMenuClass
                {
                    Id = a.Id,
                    StatusCode = a.StatusCode,
                    ExceptionMessage = a.ErrorLog,
                    CreateDate = a.CreateDate,
                    ReportFilterXML = a.PrametersXml,
                    ReportName = a.Subject,
                    ReportLocalName = a.Subject,
                    NotDisplayInMenu = a.NotDisplayInMenu,
                    ItemType =(int)MenuTypes.BatchTaskExecution
                }).ToList();

            return reportLogs.Concat(batchTasks).ToList();
        }
        public void DeleteFromMenu(string reportId, int tenant, int type)
        {
            switch (type)
            {
                case ((int)MenuTypes.ReportExecutionLog):
                    var reportExecutionLog = repository.context.ReportExecutionLogs.FirstOrDefault(a => a.Id == reportId && a.Tenant == tenant);
                    if (reportExecutionLog != null)
                    {
                        UpdateEntity(reportExecutionLog);
                        repository.context.SaveChanges();
                    }
                    break;

                case ((int)MenuTypes.BatchTaskExecution):
                    var context = InfrastructureContext.GetContext(tenant);
                    var batchTaskExecution = context.BatchTaskExecutions.FirstOrDefault(a => a.Id == reportId);
                    if (batchTaskExecution != null)
                    {
                        UpdateEntity(batchTaskExecution);
                        context.SaveChanges();
                    }
                    break;

                default:
                    throw new ArgumentException("Invalid type", nameof(type));
            }
        }

      
        private void UpdateEntity(object entity)
        {
            var notDisplayInMenuProperty = entity.GetType().GetProperty("NotDisplayInMenu");
           
            if (notDisplayInMenuProperty != null)
            {
                notDisplayInMenuProperty.SetValue(entity, true);
            }
        }

      

        }
    }
