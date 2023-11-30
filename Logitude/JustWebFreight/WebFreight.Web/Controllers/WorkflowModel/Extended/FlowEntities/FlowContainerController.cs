using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.TreeFilterQuery;
using Marvin.JsonPatch;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.Controllers.WorkflowModel.Models;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.Workflow;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WorkflowModel.Extended.FlowEntities
{
    public class FlowContainerController : ApiController
    {
        public HttpResponseMessage GetSingle(string id)
        {
            int tenant = 0;

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                tenant = authToken.Tenant;

                ContainerQuery containerQuery = new ContainerQuery(tenant);
                ContainerPM containerPM = containerQuery.GetSinglePM(id, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, containerPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, WorkflowApiExceptionBuilder.BuildException(ex, tenant));
            }
        }

        public HttpResponseMessage PostByFilterTree(ApiQueryTreeFilters apiQueryTreeFilters)
        {
            int tenant = 0;

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                tenant = authToken.Tenant;

                IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(authToken.Tenant);
                ContainerRepository containerRepository = new ContainerRepository(shipmentsContext);
                IQueryable<Container> containerQuery = containerRepository.GetContainers(authToken.Tenant);

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                TreeFilterQueryArgs treeFilterQueryArgs = new TreeFilterQueryArgs()
                {
                    AdditionalTreeFilter = javaScriptSerializer.Serialize(apiQueryTreeFilters.QueryFilterItem),
                    ObjectTableName = "Container",
                    Tenant = tenant,
                };

                containerQuery = new TreeFilterQueryService().Apply(containerQuery, treeFilterQueryArgs);
                containerQuery = containerQuery.OrderBy(string.IsNullOrEmpty(apiQueryTreeFilters.OrderBy) ? "Id" : apiQueryTreeFilters.OrderBy);
                containerQuery = containerQuery.Skip(0);
                containerQuery = containerQuery.Take(apiQueryTreeFilters.PageSize);

                var containers = containerQuery.Select("new { " + apiQueryTreeFilters.ReturnedColumns + " }").ToDynamicList();

                ServiceResponse response = new ServiceResponse();
                response.Result = containers;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, WorkflowApiExceptionBuilder.BuildException(ex, tenant));
            }
        }

        public HttpResponseMessage Patch(string id, JsonPatchDocument<ContainerPM> containerJsonPatch)
        {
            int tenant = 0;

            try
            {
                using (TransactionScope transactionScope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    tenant = authToken.Tenant;

                    ContainerQuery containerQuery = new ContainerQuery(tenant);
                    ContainerPM containerPM = containerQuery.GetSinglePM(id, tenant);

                    if (containerPM == null) { throw new Exception("Cannot find the container"); }

                    containerJsonPatch.ApplyTo(containerPM);

                    IShipmentsContext MyContext = ShipmentsContext.GetContext(containerPM.Tenant);
                    ContainerService service = new ContainerService(MyContext, containerPM.Tenant);
                    service.Update(containerPM);

                    ContainerQuery updatedContainerQuery = new ContainerQuery(tenant);
                    ContainerPM updatedContainerPM = updatedContainerQuery.GetSinglePM(id, tenant);

                    transactionScope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, updatedContainerPM);
                }
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, WorkflowApiExceptionBuilder.BuildException(exception, tenant));
            }
        }
    }
}