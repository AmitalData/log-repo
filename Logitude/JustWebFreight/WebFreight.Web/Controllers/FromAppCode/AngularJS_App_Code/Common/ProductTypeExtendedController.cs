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
    public class ProductTypeExtendedController : ApiController
    {
        public HttpResponseMessage GetProductTypeLists(int tenant)
        {
            Authentication();
            SecurityUtility.AuthenticationOnTenant(tenant);
            ProductTypeRepository productTypeRepository = new ProductTypeRepository(tenant);
            ProductTypeQuery productTypeQuery = new ProductTypeQuery(productTypeRepository);

            IQueryable<ProductType> iQueryable = productTypeRepository.GetActiveProductTypes(tenant);
            IQueryable<ProductTypeList> query2 = productTypeQuery.GetIQueryableEntityList(iQueryable, tenant);
            return Request.CreateResponse(HttpStatusCode.OK, query2);
        }





        private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("ProductType", "READ", authToken.Tenant);
        }



    }


     

}