using Logitude.BL.CommonDataModel.Helpers;
using Logitude.Infrastructure.Data;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System;
using Simplog.Data.CommonDataModel.Repositories;
using System.Data.Entity;
using System.Linq;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.Controllers.WebServices.Services
{
    public class ProcessMenuService
    {
        public List<MenuItemClass> GetProcessesByTenantAndUserLastWeek(int tenant, string id)
        {
            var oneWeekAgo = DateTime.Now.AddDays(-7);
            ReportExecutionLogRepository repository = new ReportExecutionLogRepository();

            (repository.context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            var reportLogs = repository.context.ReportExecutionLogs
                .Include(a => a.Report)
                .Where(a => a.Tenant == tenant && a.CreateDate >= oneWeekAgo && a.CreatedByUserId == id && !a.NotDisplayInMenu)
                .Select(a => new MenuItemClass
                {
                    Id = a.Id,
                    ItemId = a.ReportId,
                    StatusCode = a.StatusCode,
                    ExceptionMessage = a.ExceptionMessage,
                    CreateDate = a.CreateDate,
                    FilterXML = a.ReportFilterXML,
                    Name = a.Report != null ? a.Report.Name : null,
                    LocalName = a.Report != null ? a.Report.LocalName : null,
                    TemplateId = a.ReportTemplateId,
                    NotDisplayInMenu = a.NotDisplayInMenu,
                    ItemType = (int)MenuTypes.ReportExecutionLog
                }).ToList();

            var batchTasks = InfrastructureContext.GetContext(tenant).BatchTaskExecutions
                .Where(a => (a.Subject == "Create a new Tax Report" || a.Subject == "Cancel Tax Report") && a.Tenant == tenant && a.CreatedByUserId == id && a.CreateDate >= oneWeekAgo && !a.NotDisplayInMenu)
                .AsEnumerable()
                .Select(a => new MenuItemClass
                {
                    Id = a.Id,
                    StatusCode = a.StatusCode,
                    ExceptionMessage = a.ErrorLog,
                    CreateDate = a.CreateDate,
                    FilterXML = a.PrametersXml,

                    Name = TextCodesTranslator.TranslateText($"Accounting.General.O.{a.Subject.Replace(" ", string.Empty)}", tenant, false),
                    LocalName = TextCodesTranslator.TranslateText($"Accounting.General.O.{a.Subject.Replace(" ", string.Empty)}", tenant, true),
                    NotDisplayInMenu = a.NotDisplayInMenu,
                    ItemType = (int)MenuTypes.BatchTaskExecution
                }).ToList();

            return reportLogs.Concat(batchTasks).ToList();
        }

        public List<MenuItemClass> GetProcessesByIds(List<string> ids, int tenant)
        {
            ReportExecutionLogRepository  repository = new ReportExecutionLogRepository();
            (repository.context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            var reportLogs = repository.context.ReportExecutionLogs
                .Include(a => a.Report)
                .Where(a => ids.Contains(a.Id) && a.Tenant == tenant)
                .Select(a => new MenuItemClass
                {
                    Id = a.Id,
                    ItemId = a.ReportId,
                    StatusCode = a.StatusCode,
                    ExceptionMessage = a.ExceptionMessage,
                    CreateDate = a.CreateDate,
                    FilterXML = a.ReportFilterXML,
                    Name = a.Report != null ? a.Report.Name : null,
                    LocalName = a.Report != null ? a.Report.LocalName : null,
                    TemplateId = a.ReportTemplateId,
                    NotDisplayInMenu = a.NotDisplayInMenu,
                    ItemType = (int)MenuTypes.ReportExecutionLog
                }).ToList();

            var batchTasks = InfrastructureContext.GetContext(tenant).BatchTaskExecutions
                .Where(a => ids.Contains(a.Id) && a.Tenant == tenant)
                .AsEnumerable()
                .Select(a => new MenuItemClass
                {
                    Id = a.Id,
                    StatusCode = a.StatusCode,
                    ExceptionMessage = a.ErrorLog,
                    CreateDate = a.CreateDate,
                    FilterXML = a.PrametersXml,
                    Name = TextCodesTranslator.TranslateText($"Accounting.General.O.{a.Subject.Replace(" ", string.Empty)}", tenant, false),
                    LocalName = TextCodesTranslator.TranslateText($"Accounting.General.O.{a.Subject.Replace(" ", string.Empty)}", tenant, true),
                    NotDisplayInMenu =  a.NotDisplayInMenu,
                    ItemType = (int)MenuTypes.BatchTaskExecution
                }).ToList();

            return reportLogs.Concat(batchTasks).ToList();
        }
        public void DeleteFromMenu(string reportId, int tenant, int type)
        {
            switch (type)
            {
                case ((int)MenuTypes.ReportExecutionLog):
                    {
                        ReportExecutionLogRepository repository = new ReportExecutionLogRepository();
                        var reportExecutionLog = repository.context.ReportExecutionLogs.FirstOrDefault(a => a.Id == reportId && a.Tenant == tenant);
                        if (reportExecutionLog != null)
                        {
                            UpdateEntity(reportExecutionLog);
                            repository.context.SaveChanges();
                        }
                        break;
                    }
                    

                case ((int)MenuTypes.BatchTaskExecution):
                    {
                        var MyContext = InfrastructureContext.GetContext(tenant);
                        var batchTaskExecution = MyContext.BatchTaskExecutions.FirstOrDefault(a => a.Id == reportId);
                        if (batchTaskExecution != null)
                        {
                            UpdateEntity(batchTaskExecution);
                            MyContext.SaveChanges();
                        }
                        break;
                    }
                  
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