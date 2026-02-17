using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.SystemLogsModel.EntityPMs;
using WebFreight.Web.SystemLogsModel.Queries;
using WebFreight.Web.SystemLogsModel.Tools.EntityService;


namespace WebFreight.Web.Controllers.SystemLogsModel
{
    public class FileLoggerController : ApiController
    {

        public HttpResponseMessage GetSingle(string appSettingKeyValueIsLogUntilDateyyyyMMdd)// is Feature is On  ??
        {
            try
            {
                bool IsLogInOn;
                DateTime stopLogAt;
                CheckConfig(appSettingKeyValueIsLogUntilDateyyyyMMdd, out IsLogInOn, out stopLogAt);
                var out1 = new
                {
                    AppSettingKeyValueIsLogUntilDateyyyyMMdd = appSettingKeyValueIsLogUntilDateyyyyMMdd,
                    StopLogAt = stopLogAt,
                    IsLogInOn = IsLogInOn

                };

                return Request.CreateResponse(HttpStatusCode.OK, out1);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private static void CheckConfig(string appSettingKeyValueIsLogUntilDateyyyyMMdd, out bool IsLogInOn, out DateTime stopLogAt)
        {
            IsLogInOn = false;
            stopLogAt = DateTime.MinValue;
            string UntilDateyyyyMMdd = ConfigurationManager.AppSettings[appSettingKeyValueIsLogUntilDateyyyyMMdd];//"2018062018HD312280.LogUntilDateyyyyMMdd"];
            if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
            {




                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                        "yyyyMMdd",
                                                        CultureInfo.InvariantCulture,
                                                        DateTimeStyles.None);
                IsLogInOn = DateTime.Now <= stopLogAt;

            }
        }

        public HttpResponseMessage Post(ErrorLogPM entityPM)
        {
            try
            {
                string appSettingKeyValueIsLogUntilDateyyyyMMdd = entityPM.Id;
                bool IsLogInOn;
                DateTime stopLogAt;
                CheckConfig(appSettingKeyValueIsLogUntilDateyyyyMMdd, out IsLogInOn, out stopLogAt);
                if (IsLogInOn)
                {

                    string resolveUserIdentityName = entityPM.UserName;
                    if (string.IsNullOrWhiteSpace(resolveUserIdentityName))
                    {


                        try
                        {
                            resolveUserIdentityName = AuthenticationUtil.ResolveUserIdentityName(0);
                        }
                        catch
                        {

                        }
                    }
                    string jsonPM = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(entityPM);
                    //var sb = new StringBuilder();
                    //sb
                    //    .AppendLine("ResolveUserIdentityName:")
                    //    .AppendLine(resolveUserIdentityName)
                    //    .AppendLine("**Stack:")
                    //    .AppendLine(entityPM.StackTrace)
                    //    .AppendLine("**PM:New:")
                    //    .AppendLine(entityPM.Exception)
                    //    .AppendLine("**POCO:old:")
                    //    .AppendLine(jsonPOCO);

                    LogitudeSettings.HandleLogMe
                        //(mess, err, suffix, stopLogAt)
                        (jsonPM, false, appSettingKeyValueIsLogUntilDateyyyyMMdd, DateTime.MaxValue);
                }

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            //  scope.Complete();


            return Request.CreateResponse(HttpStatusCode.OK, entityPM);

        }


    }
}