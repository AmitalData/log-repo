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
using System.Transactions;
using Logitude.BL.Helpers;
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using Logitude.ShipmentOrderModule.Data;
using Logitude.ShipmentOrderModule.BL;
using Logitude.ShipmentOrderModule.Data.EntityLists;
using Logitude.ShipmentOrderModule.BL.EntityUpdateServices;
using Logitude.ShipmentOrderModule.Data.EntityListQueryServices;
using Logitude.ShipmentOrderModule.BL.EntityQueryServices;

namespace WebFreight.Web.Controllers.ShipmentOrderModel
{
    public class ShipmentOrderExtendedController : ApiController
    {

        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                int tenant = GetTenantFromAuthenticationToken();
                Authentication(tenant);
                IShipmentOrderContext context = ShipmentOrderContext.GetContext(tenant);
                ShipmentOrderQueryService shipmentOrderQuery = new ShipmentOrderQueryService(context);
                ShipmentOrderPM shipmentOrderPM = shipmentOrderQuery.GetSingle(id, true, false);

                return Request.CreateResponse(HttpStatusCode.OK, shipmentOrderPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

            }
        }

        private int GetTenantFromAuthenticationToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            return authToken.Tenant;
        }


        private static void Authentication(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.AuthenticateAPICall(tenant);

        }
    }
}