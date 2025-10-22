
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;


namespace WebFreight.Web.Controllers.InvoiceModel.Extended
{
    public class ExpenseAllocationFlowExtendedController : ApiController
    {

        public HttpResponseMessage GetListByEntityIdAndObjectTableId(string entityId, string objectTableId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                IInvoiceContext MyContext = InvoiceContext.GetContext(authToken.Tenant);
                ExpenseAllocationFlowRepository expenseAllocationFlowRepository = new ExpenseAllocationFlowRepository(MyContext);
                IQueryable<ExpenseAllocationFlow> entityPocos = expenseAllocationFlowRepository.GetListByEntityIAndObjectTable(authToken.Tenant,objectTableId,entityId);

                ExpenseAllocationFlowQuery expenseAllocationFlowQuery = new ExpenseAllocationFlowQuery(expenseAllocationFlowRepository);
                List<ExpenseAllocationFlowList> entityLists = expenseAllocationFlowQuery.GetList(entityPocos);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, entityLists);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}