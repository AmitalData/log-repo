
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Customs.Data.DataContracts;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class VendorExtendedController : ApiController
    {

        public HttpResponseMessage GetVendorsWithImporterDespositions(string vendorId, string importerId, bool ShowOnlyValid,bool useImporterFilter, string searchText)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                CustomsVendorRepository rep = new CustomsVendorRepository(customContext);
                IQueryable<ImporterDespositionClass> despositions = rep.GetVendorsWithImporterDespositions(vendorId, importerId, ShowOnlyValid, useImporterFilter, searchText, tenant);
                ServiceResponse response = new ServiceResponse();
                response.Count = despositions.Count();
                response.Result = despositions;

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }




    }
}