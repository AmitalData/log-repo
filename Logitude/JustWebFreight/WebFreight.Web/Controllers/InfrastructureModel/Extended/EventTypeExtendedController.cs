using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class EventTypeExtendedController : ApiController
    {
        public HttpResponseMessage GetEventTypesByObjectTable(string objectTableId, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                EventTypeQuery eventTypesRepository = new EventTypeQuery(authToken.Tenant);
                IQueryable<EventTypePM> iQueryable = eventTypesRepository.GetEventTypesByObjectTable(objectTableId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, iQueryable);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



        public HttpResponseMessage GetEventTypeByCode(string code, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                EventTypeQuery eventTypesRepository = new EventTypeQuery(authToken.Tenant);
                EventTypePM eventTypePM  = eventTypesRepository.GetSinglePMByCode(code, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, eventTypePM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



        public HttpResponseMessage Put(List<EventTypePM> eventTypePMLists)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                        IWebFreightContext MyContext = WebFreightContext.GetContext(authToken.Tenant);
                        EventTypeService service = new EventTypeService(MyContext, authToken.Tenant);

                        foreach (EventTypePM eventTypePm in eventTypePMLists)
                        {
                            SecurityUtility.AuthenticationOnEntityTenant("EventType", eventTypePm.Tenant, authToken.Tenant);

                            service.Update(eventTypePm);
                        }

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, eventTypePMLists);
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