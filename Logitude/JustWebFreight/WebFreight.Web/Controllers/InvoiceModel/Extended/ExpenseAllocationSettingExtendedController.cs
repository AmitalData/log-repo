
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityLists;
using WebFreight.Web.DataContracts;
using System.IO;
using System.Net.Http.Headers;
using System.Data;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityPMs;


namespace WebFreight.Web.Controllers.InvoiceModel.Extended
{
    public class ExpenseAllocationSettingExtendedController : ApiController
    {
        public HttpResponseMessage GetSingleByEntityIdAndObjectTable(string entityId, string objectTableId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;


                ExpenseAllocationSettingQuery expenseAllocationSettingQuery = new ExpenseAllocationSettingQuery(tenant);
                ExpenseAllocationSettingPM expenseAllocationSettingPM = expenseAllocationSettingQuery.GetSingleByEntityIdAndObjectTable(entityId, objectTableId, tenant);
                ServiceResponse response = new ServiceResponse();               
                response.Result = expenseAllocationSettingPM;
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


      
    }
}