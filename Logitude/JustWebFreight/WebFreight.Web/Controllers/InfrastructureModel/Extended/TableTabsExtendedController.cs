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
using System.Transactions;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class TableTabsController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage UpdateTabs([FromBody] List<ObjectTableTabPM> tabs)
        {
            try
            {
                AuthenticationToken authToken = AuthinticateTenant();

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ObjectTableTabService objectTableTabService = new ObjectTableTabService(WebFreightContext.GetContext(authToken.Tenant),authToken.Tenant);
                    objectTableTabService.Updates(tabs);

                    scope.Complete();
                };

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public List<ObjectTableTabPM> GetTenantTableTabsByTableId(int tenant, string objectTableId)
        {
            AuthenticationToken authToken = AuthinticateTenant();
            ObjectTableTabQuery query = new ObjectTableTabQuery(new ObjectTableTabRepository(tenant));
            return query.GetObjectTableTabsByTenantAndObjectTable(objectTableId, authToken.Tenant).ToList();
        }

        [HttpGet]
        public List<ObjectTableTabPM> GetTenantTabs(int tenant)
        {
            AuthenticationToken authToken = AuthinticateTenant();
            ObjectTableTabQuery query = new ObjectTableTabQuery(new ObjectTableTabRepository(authToken.Tenant));
            return query.GetTenantTabs(tenant).ToList();
        }


        private AuthenticationToken AuthinticateTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            return authToken;
        }
    }
}