
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;


namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CB_CustomsItemExtendedController : ApiController
    {

        public HttpResponseMessage GetCustomsBookMainView(string customsBookType, int Tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                Filters filters = new Filters();
                filters.CustomsBookType = customsBookType;
                filters.Tenant = Tenant;
               
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                //string loggedUserEmail = authToken.Email;
                //SecurityUtility.AuthenticationOnTenant(0);

                CB_CustomsItemComputedDataQueryService customsItemComputedDataQueryService = new CB_CustomsItemComputedDataQueryService(0);
                List<CB_CustomsItemComputedDataList> result = customsItemComputedDataQueryService.GetCustomsBookMainView(filters.CustomsBookType, filters.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCustomsBookMainViewSearchByClassification(Filters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(0);

                CB_CustomsItemComputedDataQueryService customsItemComputedDataQueryService = new CB_CustomsItemComputedDataQueryService(0);
                List<CB_CustomsItemComputedDataList> result = customsItemComputedDataQueryService.GetCustomsBookMainViewSearchByClassification(filters.CustomsBookType,
                    filters.SearchFields, filters.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetCustomsBookMainViewSearchByText(Filters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(0);

                CB_CustomsItemComputedDataQueryService customsItemComputedDataQueryService = new CB_CustomsItemComputedDataQueryService(0);
                List<CB_CustomsItemComputedDataList> result = customsItemComputedDataQueryService.GetCustomsBookMainViewSearchByText(filters.SearchFields,
                    filters.CustomsBookType, filters.CustomsItemHierarchic, filters.Reamarks, filters.Rules, filters.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

    public class Filters
    {
        public string CustomsBookType { get; set; } = "1";
        public int Tenant { get; set; } = 0;
        public string SearchFields { get; set; } = null;
        public string CustomsItemHierarchic { get; set; } = null;
        public bool Reamarks { get; set; } = false;
        public bool Rules { get; set; } = false;

    }
}