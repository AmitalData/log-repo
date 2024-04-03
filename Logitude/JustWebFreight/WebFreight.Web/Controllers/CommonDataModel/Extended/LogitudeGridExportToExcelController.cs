using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using System.Web.Script.Serialization;
using Logitude.Server.Tools;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Logitude.BL.CommonDataModel.LogitudeGridExportToExcel;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class LogitudeGridExportToExcelController : ApiController
    {
         public HttpResponseMessage PostGetQueryToExcelData(LogitudeGridExportToExcelArguments Arguments)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("LogitudeGridExportToExcel", Arguments.Tenant, authToken.Tenant);
                LogitudeGridExportToExcelHelper logitudeGridExportToExcelHelper = new LogitudeGridExportToExcelHelper();
                string FileName = logitudeGridExportToExcelHelper.ExportDataToExcel(Arguments);
                return Request.CreateResponse(HttpStatusCode.OK, FileName);


             }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
 
    }

    
}