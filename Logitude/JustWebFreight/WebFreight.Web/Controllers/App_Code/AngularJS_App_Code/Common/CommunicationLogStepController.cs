//This controller is not generated, you can add your methods there.
//All controllers under App_Code  should be moved to the Controllers Folder, we moved some of it, but still there is many old ones there.

//Regards,
//Islam.

//using Logitude.BL.CommonDataModel.EntityLists;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using Simplog.Data.CommonDataModel.EntityPOCOs;
//using Simplog.Data.CommonDataModel.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web;
//using System.Web.Http;
//using WebFreight.Web.Security;

//namespace WebFreight.Web.App_Code.AngularJS_App_Code.Common
//{
//    public class CommunicationLogStepController : ApiController
//    {
//        public HttpResponseMessage GetCommunicationLogStepsListsByLogId(string logId, int tenant)
//        {
//          //  Authentication();

//            CommunicationLogStepQuery communicationLogStepQuery = new CommunicationLogStepQuery(tenant);
//            List<CommunicationLogStepList> myResult  = communicationLogStepQuery.GetCommunicationLogStepListsByLogId(logId, tenant);

//            return Request.CreateResponse(HttpStatusCode.OK, myResult);
//        }



//        private static void Authentication()
//        {
//            string token = HttpContext.Current.Request.Headers["Token"];
//            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
//            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
//            SecurityUtility.CheckContactFeature("CommunicationLogStep", "READ", authToken.Tenant);
//        }

//    }
//}