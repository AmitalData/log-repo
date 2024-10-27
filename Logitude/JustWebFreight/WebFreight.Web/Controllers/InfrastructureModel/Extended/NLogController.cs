using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Windows.Forms;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{

    public class NLogController : ApiController
    {
        public HttpResponseMessage GetRulesList()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                //  NetCommonHelper.Logger.DevLog.Instance.AddLogTarget("form2");
                List<Rule> ruleList = NetCommonHelper.Logger.DevLog.Instance.GetRulesName()?.Select(item => new Rule
                {
                    LogPattern = item.LoggerNamePattern,
                    Minlevel = item.Levels[0]?.ToString(),
                    Target = item.Targets[0]?.Name,
                    Name = item.RuleName
                }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, ruleList);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        [HttpPut]
        public HttpResponseMessage KillRule()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                NetCommonHelper.Logger.DevLog.Instance.RemoveLogTarget();



                return Request.CreateResponse(HttpStatusCode.OK, true);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [HttpPut]
        public HttpResponseMessage AddLog(string logPattern)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                NetCommonHelper.Logger.DevLog.Instance.AddLogTarget(logPattern);



                return Request.CreateResponse(HttpStatusCode.OK, true);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

    }

    public class Rule
    {
        public string LogPattern;
        public string Minlevel;
        public string Target;
        public string Name;
    }
}