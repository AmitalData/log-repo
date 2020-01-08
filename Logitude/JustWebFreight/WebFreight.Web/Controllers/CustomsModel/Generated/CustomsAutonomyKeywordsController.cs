
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.BL;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.BL.EntityQueryServices;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{


    public partial class CustomsAutonomyKeywordsController : ApiController
    {

        

        public HttpResponseMessage GetByKeywordtypeCode(string KeywordtypeCode,int tenant)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Customs.CustomsAutonomyKeyword", "READ", authToken.Tenant);

                ICustomContext MyContext = CustomContext.GetContext(authToken.Tenant);
                CustomsAutonomyKeywordQueryService customsAutonomyKeywordQuery = new CustomsAutonomyKeywordQueryService(MyContext);
                customsAutonomyKeywordQuery.InitializeSettings();
                CustomsAutonomyKeywordPM customsAutonomyKeywordPM = customsAutonomyKeywordQuery.GetByKeywordtypeCode(KeywordtypeCode, tenant);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, customsAutonomyKeywordPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


    }
}
