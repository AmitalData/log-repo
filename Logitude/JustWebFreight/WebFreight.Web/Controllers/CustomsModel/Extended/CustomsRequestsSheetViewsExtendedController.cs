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
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using System.Transactions;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.DataContracts;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public partial class CustomsRequestsSheetViewsExtendedController : ApiController
    {
        public HttpResponseMessage GetStatistics()
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                ICustomContext context = CustomContext.GetContext(tenant);
                CustomsRequestsSheetListQueryService customsRequestsSheetQuery = new CustomsRequestsSheetListQueryService(context);
                var summary = customsRequestsSheetQuery.GetStatistics(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, summary);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
           
        }
    }
}