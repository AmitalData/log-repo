using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Customs.BL.AzureSearch;
using Logitude.Customs.BL.AzureSearch.Objects;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.WebServices
{
    public class AzureSearchController : ApiController
    {
        [HttpGet]
        [Route(template: "api/AzureSearch/GetSettings")]
        public async Task<HttpResponseMessage> GetSettings(string index)
        {
            int tenant = HeaderHelper.Authenticate().Tenant;

            try
            {
                if (string.IsNullOrEmpty(index))
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "index is required");

                FastSearchSettings settings = await FastSearchService.GetIndexSettingsAsync(tenant, index);
                return Request.CreateResponse(HttpStatusCode.OK, settings);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route(template: "api/AzureSearch/GetIndex")]
        public async Task<HttpResponseMessage> GetFastSearch([FromUri] ApiQueryFilters filters, string searchText, string index)
        {
            int tenant = HeaderHelper.Authenticate().Tenant;

            try
            {
                List<dynamic> result = await FastSearchService.Search(filters, searchText, index, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        [Route(template: "api/AzureSearch/GetIndexCount")]
        public HttpResponseMessage GetRecentSearches(string screen, string entname, int size = 20)
        {
            int tenant = HeaderHelper.Authenticate().Tenant;

            if (string.IsNullOrEmpty(screen))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "screen cannot be null or empty");

            if (string.IsNullOrEmpty(entname))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "entname cannot be null or empty");

            try
            {
                ContactPM loggedUser = new LoggedContactUtil().GetLoggedContact(tenant);
                List<dynamic> result = new SearchIndexEditHistoryQuery(tenant).GetRecent(tenant, screen, entname, loggedUser.Id, size);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}