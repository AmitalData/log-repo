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
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityPMs; 
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Global
{
    public partial class QueryColumnsController : ApiController
    {

        public HttpResponseMessage Post(QueryColumnPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
                QueryColumnService service = new QueryColumnService(objectContext, entityPM.Tenant);
                service.Create(entityPM);

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Put(QueryColumnPM entityPM)
        { 
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
                QueryColumnService service = new QueryColumnService(objectContext, entityPM.Tenant);
                QueryColumnRepository Repo = new QueryColumnRepository(entityPM.Tenant);
                var temp = Repo.GetSingleQueryColumn(entityPM.Id, entityPM.Tenant);
                if (temp == null)
                {
                    service.Create(entityPM);
                }
                else
                {
                    service.Update(entityPM);
                }
                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [OperationContract]
        [WebGet(UriTemplate = "delete/{id}/{tenant}")]
        public HttpResponseMessage Delete(string id,int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(tenant);
                QueryColumnRepository repo = new QueryColumnRepository(tenant);
                var temp = repo.GetSingleQueryColumn(id,tenant);
                if (temp != null)
                {
                    repo.Remove(temp);
                    repo.SubmitChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, temp);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [OperationContract]
        [WebGet(UriTemplate = "getquerycolumnpms/{tenant}/{queryid}/{objecttableid}/{userid}")]
        public List<QueryColumnPM> GetQueryColumnPMs(int tenant, string queryid, string objecttableid, string userid)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            QueryColumnRepository queryColumnRepository = new QueryColumnRepository(tenant);
            QueryColumnQuery queryColumnQuery = new QueryColumnQuery(queryColumnRepository);
            var querycolumns = queryColumnQuery.GetQueryColumnsByQueryIdAndUserAngular(tenant, userid, queryid);//.ToList();
            if (querycolumns.Count() > 0)
            {
                return querycolumns.OrderBy(a => a.IndexOrder).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}