using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Mapping;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        public void CancelStuckReports(int tenant ,string report=null)
        {
            if(report=="null")
            {
                report = null;
            }
            string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(tenant);
            var contactRep = new ContactRepository(tenant);
            Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, tenant);
            string strConnString = TenantServerConfigration.GetDbConnection(tenant);

            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[CancelReportAndUpdateQueueMessage]", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter reportId = new SqlParameter("@ReportId", SqlDbType.NText);
                SqlParameter cancelByUser = new SqlParameter("@CancelByUser", SqlDbType.NText);
                SqlParameter tenantId = new SqlParameter("@TenantId", SqlDbType.Int);
                SqlParameter queueDefinitionCode = new SqlParameter("@QueueDefinitionCode", SqlDbType.NText);

                reportId.Direction = ParameterDirection.Input;
                cancelByUser.Direction = ParameterDirection.Input;
                tenantId.Direction = ParameterDirection.Input;
                queueDefinitionCode.Direction = ParameterDirection.Input;

                reportId.Value = report;
                cancelByUser.Value= contact?.Id;
                tenantId.Value = tenant;
                queueDefinitionCode.Value = FeatureToggleHelper.HasFeatureToggle("RE2", tenant) ? "ReportExecutionLogV2Queue" : "ReportExecutionLogQueue"; 

                cmd.Parameters.Add(reportId);
                cmd.Parameters.Add(cancelByUser);
                cmd.Parameters.Add(tenantId);
                cmd.Parameters.Add(queueDefinitionCode);

                cn.Open();
                var output = cmd.ExecuteNonQuery();
                cn.Close();
            }
       
    }

        public void Cancel(ReportExecutionLog reportExecutionLog, string cancelByUserId)
        {
           
            reportExecutionLog.StatusCode = "F";
            reportExecutionLog.ExceptionMessage = string.Format("Stopped manually by {0}", cancelByUserId);
            reportExecutionLog.DoneDate = DateTime.Now;
            repository.Update(reportExecutionLog);
            

            QueueMessageRepository messagesRepository = new QueueMessageRepository(reportExecutionLog.Tenant);
            var message = messagesRepository.GetSingleQueueMessageByReportId(reportExecutionLog.Id, reportExecutionLog.Tenant);
            string strConnString = TenantServerConfigration.GetDbConnection(reportExecutionLog.Tenant);
            if (message != null)
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[Queue_SetStatus]", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter messageIdPar = new SqlParameter("@MessageId", SqlDbType.BigInt);
                    SqlParameter statusPar = new SqlParameter("@Statud", SqlDbType.Int);


                    messageIdPar.Direction = ParameterDirection.Input;
                    statusPar.Direction = ParameterDirection.Input;

                    messageIdPar.Value = message.Id;
                    statusPar.Value = 22;

                    cmd.Parameters.Add(messageIdPar);
                    cmd.Parameters.Add(statusPar);

                    cn.Open();
                    var output = cmd.ExecuteNonQuery();
                    cn.Close();
                    }
                     }

               
          

        }


    }
}