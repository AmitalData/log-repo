using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.TreeFilterQuery;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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
using WebFreight.Web.Helpers.Workflow;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WorkflowModel.Extended.FlowEntities
{
    public class FlowCustomEntityController : ApiController
    {
        public HttpResponseMessage PostByFilterTree(ApiQueryTreeFilters apiQueryTreeFilters)
        {
            int tenant = 0;

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                tenant = authToken.Tenant;


                IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
                DataCustomObjectRepository dataCustomObjectRepository = new DataCustomObjectRepository(webFreightContext);
                ObjectTable objectTable = webFreightContext.ObjectTables.Where(d => d.Name == apiQueryTreeFilters.CustomEntityName).FirstOrDefault();

                IQueryable<DataCustomObject> entityPocos = dataCustomObjectRepository.GetDataCustomObjectsByObjectTableId(tenant, objectTable.Id);

                DataCustomObjectQuery dataCustomObjectQuery = new DataCustomObjectQuery(dataCustomObjectRepository);
                IQueryable<DataCustomObjectList> customEntityQuery = dataCustomObjectQuery.GetIQueryableEntityList(entityPocos);

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                TreeFilterQueryArgs treeFilterQueryArgs = new TreeFilterQueryArgs()
                {
                    AdditionalTreeFilter = javaScriptSerializer.Serialize(apiQueryTreeFilters.QueryFilterItem),
                    ObjectTableName = apiQueryTreeFilters.CustomEntityName,
                    Tenant = tenant,
                };
                customEntityQuery = new TreeFilterQueryService().Apply<DataCustomObjectList>(customEntityQuery, treeFilterQueryArgs);

                customEntityQuery = new TreeFilterQueryService().Apply(customEntityQuery, treeFilterQueryArgs);
                customEntityQuery = customEntityQuery.OrderBy(string.IsNullOrEmpty(apiQueryTreeFilters.OrderBy) ? "Id" : apiQueryTreeFilters.OrderBy);
                customEntityQuery = customEntityQuery.Skip(0);
                customEntityQuery = customEntityQuery.Take(apiQueryTreeFilters.PageSize);

                var customEntities = customEntityQuery.Select("new { " + apiQueryTreeFilters.ReturnedColumns + " }").ToDynamicList();

                ServiceResponse response = new ServiceResponse();
                response.Result = customEntities;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, WorkflowApiExceptionBuilder.BuildException(ex, tenant));
            }
        }
    }
}