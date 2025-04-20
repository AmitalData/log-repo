using Logitude.Customs.BL.AzureSearch;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.WebServices
{
    public class AzureSearchController : ApiController
    {
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
    }
}