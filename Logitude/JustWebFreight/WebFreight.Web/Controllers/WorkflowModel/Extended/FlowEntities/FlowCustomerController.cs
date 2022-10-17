using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.TreeFilterQuery;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.Controllers.WorkflowModel.Models;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WorkflowModel.Extended.FlowEntities
{
    public class FlowCustomerController : ApiController
    {
        public HttpResponseMessage PostByFilterTree(ApiQueryTreeFilters apiQueryTreeFilters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                ICommonDataContext customerContext = CommonDataContext.GetContext(authToken.Tenant);
                CustomerRepository customerRepository = new CustomerRepository(customerContext);
                IQueryable<Customer> customerQuery = customerRepository.GetCustomers(authToken.Tenant);

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                TreeFilterQueryArgs treeFilterQueryArgs = new TreeFilterQueryArgs()
                {
                    AdditionalTreeFilter = javaScriptSerializer.Serialize(apiQueryTreeFilters.QueryFilterItem),
                    ObjectTableName = "Customer",
                    Tenant = tenant,
                };

                customerQuery = new TreeFilterQueryService().Apply(customerQuery, treeFilterQueryArgs);
                customerQuery = customerQuery.OrderBy(string.IsNullOrEmpty(apiQueryTreeFilters.OrderBy) ? "Id" : apiQueryTreeFilters.OrderBy);
                customerQuery = customerQuery.Skip(0);
                customerQuery = customerQuery.Take(apiQueryTreeFilters.PageSize);

                var customers = customerQuery.Select("new { " + apiQueryTreeFilters.ReturnedColumns + " }").ToDynamicList();

                ServiceResponse response = new ServiceResponse();
                response.Result = customers;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}