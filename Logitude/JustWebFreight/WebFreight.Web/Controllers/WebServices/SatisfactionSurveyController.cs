using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Transactions;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.BL.EntityQueryServices;
using System.Text;

namespace WebFreight.Web.Controllers.WebServices
{


    public partial class SatisfactionSurveysWebServiceController : ApiController
    {

        public HttpResponseMessage Post(SatisfactionSurveyPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string logKey = PerformanceLogger.LogCurrentTime();
                        var secretKey = "secretkey";
                        var concatenatedString = secretKey + entityPM.Id;
                        if (!CreateMD5(concatenatedString).Equals(entityPM.Hash, StringComparison.InvariantCultureIgnoreCase))
                        {
                            logKey = PerformanceLogger.LogCurrentTime();
                            throw new Exception("Hash verification failed.");
                        }
                        IInfrastructureContext MyContext = InfrastructureContext.GetContext(entityPM.Tenant);
                        SatisfactionSurveyUpdateService service = new SatisfactionSurveyUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        service.Update(entityPM, true);
                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }


        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
