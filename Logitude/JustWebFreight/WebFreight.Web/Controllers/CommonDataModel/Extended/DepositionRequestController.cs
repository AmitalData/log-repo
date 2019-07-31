using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class DepositionRequestController : ApiController
    {

        public HttpResponseMessage PostDepositionRequest(DepositionRequestAM depositionRequestAM)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                    DepositionRequestHelper depositionRequestHelper = new DepositionRequestHelper();
                    string logId = depositionRequestHelper.AddAPILogs(depositionRequestAM, depositionRequestAM.CustomerTenant);

                    string ShipmentId = depositionRequestHelper.StartDepositionRequestTask(depositionRequestAM);
                    var msg = "Importer Deposition Send to cloud Successfully";
                    if (string.IsNullOrEmpty(ShipmentId)) msg = "Shipment not found in logbox";
                    APILogsUtility.UpdateAPILogStatus(logId, depositionRequestAM.CustomerTenant, "D", 0, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(depositionRequestAM), null, null, "");

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, msg);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}