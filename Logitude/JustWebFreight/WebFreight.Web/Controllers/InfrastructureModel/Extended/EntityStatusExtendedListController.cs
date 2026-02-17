using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class EntityStatusExtendedListController : ApiController
    {
        public HttpResponseMessage GetSingle(string code)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    var entityStatusRepository = new EntityStatusRepository(authToken.Tenant);
                    IQueryable<EntityStatus> iQueryable = entityStatusRepository.GetEntityStatusByTenant(authToken.Tenant).Where(d => d.InActive == false);
                    var entityStatusQuery = new EntityStatusQuery(entityStatusRepository);
                    //EntityStatusList query2 = entityStatusQuery.GetIQueryableEntityList(iQueryable). FirstOrDefault();
                    EntityStatus status = iQueryable.Where(a => a.Code == code).FirstOrDefault();

                    return Request.CreateResponse(HttpStatusCode.OK, status);

                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }

            }
         
    }
}