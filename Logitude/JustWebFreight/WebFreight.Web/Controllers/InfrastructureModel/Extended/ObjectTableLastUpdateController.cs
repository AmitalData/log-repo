using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class ObjectTableLastUpdateController : ApiController
    {
        public HttpResponseMessage GetLastUpdatedTables(int tenant, DateTime sinceDate, string clientEmail)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);


                IWebFreightContext ObjectContext = WebFreightContext.GetContext(tenant);
                ObjectTableLastUpdateRepository tableLastUpdateRepository = new ObjectTableLastUpdateRepository(ObjectContext);
                ObjectTableLastUpdateQuery objectTabelQuery = new ObjectTableLastUpdateQuery(tableLastUpdateRepository);

                List<ObjectTableLastUpdatePM> list = objectTabelQuery
                    .GetLastUpdatedTablesForTenant(tenant, sinceDate);
                ///.GetLastUpdatedTables(tenant, sinceDate); // make requested to server like 
                ///http://192.116.221.103:81/AmitalNetPre/Angular2017090601/Common/Services/StandardLists/PaymentChannelListService.js at XMLHttpRequest.wrapFn [as _onreadystatechange] 
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetLastTableUpdateDate(int tenant)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                IWebFreightContext ObjectContext = WebFreightContext.GetContext(tenant);

                ObjectTableLastUpdateRepository tableLastUpdateRepository = new ObjectTableLastUpdateRepository(ObjectContext);
                ObjectTableLastUpdateQuery objectTabelQuery = new ObjectTableLastUpdateQuery(tableLastUpdateRepository);
                ObjectTableLastUpdatePM lastupdate = objectTabelQuery.GetLastUpdatedTable(tenant);
                DateTime? lastDate = null;
                if (lastupdate == null)
                {
                    lastDate = DateTime.UtcNow;
                }
                else
                {
                    lastDate = lastupdate.LastUpdateDate;
                }

                return Request.CreateResponse(HttpStatusCode.OK, lastDate);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}