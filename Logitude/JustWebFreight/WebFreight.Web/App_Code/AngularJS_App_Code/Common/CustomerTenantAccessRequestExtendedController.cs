
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Common
{


    public partial class CustomerTenantAccessRequestExtendedController : ApiController
    {


        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                SecurityUtility.CheckContactFeature("CustomerTenantAccessRequest", "READ", authToken.Tenant);
                CustomerTenantAccessRequestQuery CustomerTenantAccessRequestQuery = new CustomerTenantAccessRequestQuery(authToken.Tenant);
                CustomerTenantAccessRequestPM CustomerTenantAccessRequestPM = CustomerTenantAccessRequestQuery.GetSinglePM(id, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, CustomerTenantAccessRequestPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }




        public HttpResponseMessage Post(CustomerTenantAccessRequestPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("CustomerTenantAccessRequest", "NEW", authToken.Tenant);

                    ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                    CustomerTenantAccessRequestService service = new CustomerTenantAccessRequestService(MyContext, entityPM.Tenant);
                    service.Create(entityPM);
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);

                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }


        public HttpResponseMessage Put(CustomerTenantAccessRequestPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                { 
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("CustomerTenantAccessRequest", "UPDATE", authToken.Tenant);

                    string entityName = "CustomerTenantAccessRequest" + entityPM.Id + entityPM.Tenant;
                    string entityPmName = "CustomerTenantAccessRequestPM" + entityPM.Id + entityPM.Tenant;
                    ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                    CustomerTenantAccessRequestService service = new CustomerTenantAccessRequestService(MyContext, entityPM.Tenant);
                    service.Update(entityPM);

                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);

                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }













    }
}
