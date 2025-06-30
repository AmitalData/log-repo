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
      

      

      
       

      

        }
    }
