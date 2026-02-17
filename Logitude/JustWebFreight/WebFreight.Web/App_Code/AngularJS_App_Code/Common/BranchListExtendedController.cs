using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
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

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Common
{
    public class BranchListExtendedController : ApiController
    {
        public HttpResponseMessage GetAllBranchesByTenant(int tenant)
        {
            Authentication();
            BranchRepository branchRepository = new BranchRepository(tenant);
            BranchQuery branchQuery = new BranchQuery(branchRepository);
            IQueryable<Branch> branches = branchRepository.GetBranches(tenant);
            IQueryable<BranchList> query2 = branchQuery.GetIQueryableEntityList(branches);
            return Request.CreateResponse(HttpStatusCode.OK, query2);

        }




        private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("Branch", "READ", authToken.Tenant);
        }


    }
}