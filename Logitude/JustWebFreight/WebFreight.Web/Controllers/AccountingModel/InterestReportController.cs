using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
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

        public HttpResponseMessage PutInterestReportStatus(InterestReportArgs interestReportArgs)
        {
            try
            {
                int tenant = AuthinticateTenant();
                var accountingContext = AccountingContext.GetContext(tenant);
                if (interestReportArgs.AllSelected)
                {
                    UpdateStatusForALLNotInvoicedInterestReports(interestReportArgs, tenant);
                }
                else
                {
                    UpdateStatusForSelectedInterestReport(interestReportArgs, tenant);
                }
                ServiceResponse response = new ServiceResponse();
                InterestReportListQueryService interestReportListQueryService = new InterestReportListQueryService(accountingContext);
              
                return Request.CreateResponse(HttpStatusCode.OK, "Ok");


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        private void  UpdateStatusForALLNotInvoicedInterestReports(InterestReportArgs interestReportArgs, int tenant)
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
        private void UpdateStatusForSelectedInterestReport(InterestReportArgs interestReportArgs,int tenant)
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
    }
}