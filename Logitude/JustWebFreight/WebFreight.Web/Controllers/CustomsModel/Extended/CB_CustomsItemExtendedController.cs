using Intuit.Ipp.Core.Configuration;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
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
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using System.Runtime.Remoting.Contexts;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.PlatformUI;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CB_CustomsItemExtendedController : ApiController
    {

        public HttpResponseMessage GetCustomsBookMainView(Filters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(0);

                CB_CustomsItemQueryService customsItemQueryService = new CB_CustomsItemQueryService(0);
                List<CB_CustomsItemList> result = customsItemQueryService.GetCustomsBookMainView(filters.CustomsBookType, filters.SkippedRows, filters.PageSize);

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

                CB_CustomsItemQueryService customsItemQueryService = new CB_CustomsItemQueryService(0);
                List<CB_CustomsItemList> result = customsItemQueryService.GetCustomsBookMainViewSearchByClassification(filters.SearchFields,
                    filters.CustomsBookType,filters.CustomsItemHierarchic, filters.SkippedRows, filters.PageSize);

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

                CB_CustomsItemQueryService customsItemQueryService = new CB_CustomsItemQueryService(0);
                List<CB_CustomsItemList> result = customsItemQueryService.GetCustomsBookMainViewSearchByText(filters.SearchFields,
                    filters.CustomsBookType, filters.CustomsItemHierarchic, filters.Reamarks, filters.Rules, filters.SkippedRows, filters.PageSize);

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
        public int SkippedRows { get; set; } = 0;
        public int PageSize { get; set; } = 20;
        public string SearchFields { get; set; } = null;
        public string CustomsItemHierarchic { get; set; } = null;
        public bool Reamarks { get; set; } = false;
        public bool Rules { get; set; } = false;

    }
}