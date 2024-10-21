using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Accounting.BL.CoreBL.InterestReport;
using System.Xml.Serialization;
using Simplog.Server.Infrastructure;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Server.Tools.QueueService;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{
    public class InterestReportsForEligibleCustomerCreationController : ApiController
    {
        public HttpResponseMessage PutInterestReportsForEligibleCustomerCreationInBatch([FromBody]DateTime interestCalculationDate)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;
                string email = authToken.Email;

                InterestReportsForCustomersBatchCreator interestReportsForCustomersBatchCreator = new InterestReportsForCustomersBatchCreator();
                InterestReportsCreationForCustomersBatchArgs interestReportsCreationForCustomersBatchArgs = new InterestReportsCreationForCustomersBatchArgs();
                interestReportsCreationForCustomersBatchArgs.Tenant = tenant;
                interestReportsCreationForCustomersBatchArgs.InterestCalculationDate = interestCalculationDate;
                interestReportsCreationForCustomersBatchArgs.Email = email;
                string batchTaskId = interestReportsForCustomersBatchCreator.CreateBatchTaskExecution(interestReportsCreationForCustomersBatchArgs);


                return Request.CreateResponse(HttpStatusCode.OK, batchTaskId);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        

    }
}