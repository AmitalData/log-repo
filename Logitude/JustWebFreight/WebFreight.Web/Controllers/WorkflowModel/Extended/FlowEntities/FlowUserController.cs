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
    public class FlowUserController : ApiController
    {
        public HttpResponseMessage PostByFilterTree(ApiQueryTreeFilters apiQueryTreeFilters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                ICommonDataContext userContext = CommonDataContext.GetContext(authToken.Tenant);
                UserRepository userRepository = new UserRepository(userContext);
                IQueryable<User> userQuery = userRepository.GetUsers(authToken.Tenant);

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                TreeFilterQueryArgs treeFilterQueryArgs = new TreeFilterQueryArgs()
                {
                    AdditionalTreeFilter = javaScriptSerializer.Serialize(apiQueryTreeFilters.QueryFilterItem),
                    ObjectTableName = "User",
                    Tenant = tenant,
                };

                userQuery = new TreeFilterQueryService().Apply(userQuery, treeFilterQueryArgs);
                userQuery = userQuery.OrderBy(string.IsNullOrEmpty(apiQueryTreeFilters.OrderBy) ? "Id" : apiQueryTreeFilters.OrderBy);
                userQuery = userQuery.Skip(0);
                userQuery = userQuery.Take(apiQueryTreeFilters.PageSize);

                var users = userQuery.Select("new { " + apiQueryTreeFilters.ReturnedColumns + " }").ToDynamicList();

                ServiceResponse response = new ServiceResponse();
                response.Result = users;
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