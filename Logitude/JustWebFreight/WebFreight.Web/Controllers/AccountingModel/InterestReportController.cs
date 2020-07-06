using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.InterestService.HelperClasses;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.AccountingModel
{
    public class InterestReportController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetInterestReportsByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                int tenant = AuthinticateTenant();          
                var accountingContext = AccountingContext.GetContext(tenant);
             
                ServiceResponse response = new ServiceResponse();
                InterestReportListQueryService interestReportListQueryService = new InterestReportListQueryService(accountingContext);
                QueryOperations queryOperations = CreateQueryOperations(filters, tenant);            
                List<InterestReportList> interestReports = interestReportListQueryService.GetList(queryOperations, tenant);
                if (filters.GetCount)
                {
                    int count = interestReportListQueryService.GetListCount(queryOperations, tenant);
                    response.Count = count;
                }
                response.Result = interestReports;
                return Request.CreateResponse(HttpStatusCode.OK, response);

              
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PutInterestReportStatus(InterestReportArguments interestReportArgs)
        {
            try
            {
                int tenant = AuthinticateTenant();
                string email = HttpContext.Current.User.Identity.Name;
                interestReportArgs.Tenant = tenant;
                interestReportArgs.Email = email;
                string BatchId = CreateBatchTaskExecution(interestReportArgs, "Create Batch Invoice", "Logitude.Accounting.BL.CoreBL.Batch.BatchInterestReportInvoiceService,Logitude.Accounting.BL");
                return Request.CreateResponse(HttpStatusCode.OK, BatchId);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetInterestLastBatchServiceByTenant()
        {
            try
            {
                int tenant = AuthinticateTenant();
                InterestLastBatchServiceQueryService interestLastBatchServiceQueryService = new InterestLastBatchServiceQueryService(tenant);
                InterestLastBatchServicePM InterestLastBatchService = interestLastBatchServiceQueryService.CheckInterestLastBatchServicesByTenant(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, InterestLastBatchService);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PutCheckNumberOfInterestReportInvoicingWithoutInvoice(InterestReportArguments interestReportArgs)
        {
            try
            {
                int tenant = AuthinticateTenant();
                string email = HttpContext.Current.User.Identity.Name;
                interestReportArgs.Tenant = tenant;
                interestReportArgs.Email = email;

                InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
                int NumberOfInterestReportsWithoutInvoice = interestReportQueryService.GetInterestReportsBySelectedIds(interestReportArgs).Count();

                return Request.CreateResponse(HttpStatusCode.OK, NumberOfInterestReportsWithoutInvoice);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private void  UpdateStatusForALLNotInvoicedInterestReports(InterestReportArguments interestReportArgs, int tenant)
        {            
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            
            List<InterestReportPM> interestReports = interestReportQueryService.GetNotInvoicedInterestReportsByDates(interestReportArgs.FromDate , interestReportArgs.ToDate, tenant);
           
            if (interestReportArgs.ExcludedIds != null)
            {
                interestReports = (from a in interestReports
                                   where !interestReportArgs.ExcludedIds.Contains(a.Id)
                                   select a).ToList();
            }
            UpdateInterestReports(interestReports, tenant);
        }
        private void UpdateStatusForSelectedInterestReport(InterestReportArguments interestReportArgs,int tenant)
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            List<InterestReportPM> interestReports = interestReportQueryService.GetInterestReportsByIds(interestReportArgs.SelectedIds, tenant);
            UpdateInterestReports(interestReports,tenant);

        }
        private void UpdateInterestReports(List<InterestReportPM> interestReports, int tenant)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            foreach (InterestReportPM report in interestReports)
            {
                report.InterestReportStatusCode = "8";
                report.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                InterestReportUpdateService service = new InterestReportUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
                service.Update(report, true);
            }
        }
        private static int AuthinticateTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("InterestReport", "READ", authToken.Tenant);
            int tenant = authToken.Tenant;
            return tenant;
        }

        private QueryOperations CreateQueryOperations(ApiQueryFilters filters, int tenant)
        {
            QueryOperations queryOperations = new QueryOperations()
            {
                ObjectTableName = "InterestReport",
                PageIndex = filters.PageIndex,
                PageSize = filters.PageSize,
                QuerySection = "InterestReport",
                SortByColumnName = filters.SortBy,
                SortDirectin = filters.SortDirection,
                GetAll = filters.GetAll,
            };
            List<ObjectField> interestReportObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("InterestReport", tenant);
            if (!string.IsNullOrEmpty(filters.AdditionalFilters))
            {
                JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                foreach (QueryFilterItem filter in filters_list)
                {
                    
                    ObjectField field = interestReportObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);

                    if (field != null)
                    {
                        string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                        object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                        string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                        object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                        queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                    }
                    else
                    {
                        queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                    }
                }
            }

            return queryOperations;
        }

        private string CreateBatchTaskExecution(InterestReportArguments args,string Subject , string ClassName)
        {
            // 1- create BTE record
            BatchTaskExecutionPM taskExe;

            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(InterestReportArguments));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();


            taskExe = new BatchTaskExecutionPM()
            {
                Subject = Subject,
                Tenant = args.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = ClassName,
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = "C",

            };


            IInfrastructureContext MyContext = InfrastructureContext.GetContext(args.Tenant);
            BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), args.Tenant);
            bteUpdateService.Update(taskExe, true);

            // 2- Send to queue
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant",  args.Tenant.ToString() }
                }, args.Tenant);


            return taskExe.Id;
        }

 

    }
}