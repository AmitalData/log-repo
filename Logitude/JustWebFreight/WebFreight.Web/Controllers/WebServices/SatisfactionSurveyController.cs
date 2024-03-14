using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Transactions;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using System.Text;
using System.Security.Cryptography;

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
                        string originalData = secretKey + entityPM.Id;
                        string encryptedData, status, errMessage;
                        EncryptMD5(originalData, out encryptedData, out status, out errMessage);
                        if (!string.IsNullOrEmpty(errMessage))
                            throw new Exception("Error Message: " + errMessage);
                        if (!encryptedData.Equals(entityPM.Hash, StringComparison.InvariantCultureIgnoreCase))
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

        public void EncryptMD5(string originalData, out string encryptedData, out string status, out string errMessage)
        {
            encryptedData = "";
            status = "";
            errMessage = "";
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            try
            {
                md5 = new MD5CryptoServiceProvider();
                originalBytes = ASCIIEncoding.Default.GetBytes(originalData);
                encodedBytes = md5.ComputeHash(originalBytes);
                encodedBytes = md5.ComputeHash(encodedBytes);
                encodedBytes = md5.ComputeHash(encodedBytes);
                encryptedData = BitConverter.ToString(encodedBytes);
                encryptedData = encryptedData.Replace("-", "");
            }
            catch (Exception ex)
            {
                status = "-1";
                errMessage = "Encrypted to MD5 failed: " + ex.Message;
            }
            return;
        }



    }
}
