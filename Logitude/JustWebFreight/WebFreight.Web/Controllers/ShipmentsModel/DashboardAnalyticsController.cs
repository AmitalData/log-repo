using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebFreight.Web.Helpers;


using WebFreight.Web.Security;

using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.Server.Tools.TreeFilterQuery;
using Logitude.Server.Tools.TreeFilterQuery.Interpreter;
using Simplog.Server.Infrastructure.DataContracts;

namespace WebFreight.Web.Controllers.ShipmentsModel
{
    public class DashboardAnalyticsController : ApiController
    {
        public HttpResponseMessage GetShipmentAnalytic([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);

                ShipmentAnalyticRepository shipmentAnalyticRepository = new ShipmentAnalyticRepository(MyContext);
                TreeFilterQueryService treeFilterQueryService = new TreeFilterQueryService(new QueryTreeFilterContext() { AdditionalTreeFilter = filters.TreeFilters ,ObjectTableName = "", Tenant = authToken.Tenant });
                var shipmentAnalyticIQueryable = shipmentAnalyticRepository.GetAll();
                treeFilterQueryService.Apply(shipmentAnalyticIQueryable);
                var result = shipmentAnalyticIQueryable.ToList();
                return Request.CreateResponse(result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}