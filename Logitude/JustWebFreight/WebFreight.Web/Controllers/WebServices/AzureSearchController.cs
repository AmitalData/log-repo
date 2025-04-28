using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Customs.BL.AzureSearch;
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
        public async Task<HttpResponseMessage> GetSettings(string index)
        {
            int tenant = HeaderHelper.Authenticate().Tenant;

            try
            {
                if (string.IsNullOrEmpty(index))
                    throw new ArgumentNullException("index", "index cannot be null or empty");

                dynamic settings = await ASHelper.GetIndexSettingsAsync(tenant, index);
                return Request.CreateResponse(HttpStatusCode.OK, (object)settings);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public async Task<HttpResponseMessage> GetFastSearch([FromUri] ApiQueryFilters filters, string searchText, string index)
        {
            int tenant = HeaderHelper.Authenticate().Tenant;

            try
            {
                object result = null;
                switch (index)
                {
                    case "declarations":
                        result = await DeclarationAzureSearchService.Search(filters, tenant, searchText);
                        break;

                    default:
                        throw new Exception($"Index {index} not found");
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetRecentSearches(string screen, string entname, int size = 20)
        {
            int tenant = HeaderHelper.Authenticate().Tenant;

            if (string.IsNullOrEmpty(screen))
                throw new ArgumentNullException("screen", "screen cannot be null or empty");
            if (string.IsNullOrEmpty(entname))
                throw new ArgumentNullException("entname", "entname cannot be null or empty");

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