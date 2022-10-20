using Simplog.Server.Infrastructure.Helpers;
using System;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Transactions;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalPortalContactController : ApiController
    {
        [HttpGet]
        [Route("DigitalPortalContact/GetSingle")]
        public HttpResponseMessage GetSingle(string id, string cardId)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);
                ContactQuery contactQuery = new ContactQuery(authToken.Tenant);
                ContactPM contactPM = contactQuery.GetSinglePM(id, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, contactPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPut]
        [Route("DigitalPortalContact/Put")]
        public HttpResponseMessage Put(ContactPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, entityPM.DigitalPortalCardId);

                        string entityName = "Contact" + entityPM.Id + entityPM.Tenant;
                        string entityPmName = "ContactPM" + entityPM.Id + entityPM.Tenant;
                        if (CacheManager.CacheWrapper.Get(entityName) != null)
                        {
                            CacheManager.CacheWrapper.Invalidate(entityName);
                        }
                        if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                        {
                            CacheManager.CacheWrapper.Invalidate(entityPmName);
                        }

                        ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                        ContactService service = new ContactService(MyContext, entityPM.Tenant);
                        service.Update(entityPM);

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
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
    }
}