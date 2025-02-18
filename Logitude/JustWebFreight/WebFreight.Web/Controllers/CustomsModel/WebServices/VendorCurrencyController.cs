using Logitude.BL.Security;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WebFreight.Web.CustomWebServices;
using WebFreight.Web.Helpers;
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class VendorCurrencyController : ApiController
    {

       
       
        public HttpResponseMessage UpadateListCurrencyByVendor(List<VendorCurrencyPM> vendorCurrencyPMs)
        {
            try
            { 
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
               
                ICustomContext customContext = CustomContext.GetContext(tenant);
                VendorCurrencyUpdateService vendorCurrencyUpdateService = new VendorCurrencyUpdateService(customContext, new Dictionary<string, IContext>(), tenant);

               
              
                if (vendorCurrencyPMs!=null && vendorCurrencyPMs.Count > 0) { 
                    vendorCurrencyUpdateService.FastDelete(vendorCurrencyPMs[0].VendorId, tenant);
                    (customContext as DbContextBase).SaveChanges();
                    vendorCurrencyPMs = vendorCurrencyPMs.Where(v => v.CurrencyTypeName!="-1").ToList();
                    foreach (VendorCurrencyPM vendorCurrencyPM in vendorCurrencyPMs)
				  {
                        vendorCurrencyPM.ChangeSetOp = ChangeSetOperation.Insert;
                        vendorCurrencyUpdateService.Update(vendorCurrencyPM,true);

                  }
                }
              
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetVendorCurrencyByVendorId(string VendorId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                var vendorCurrencyQueryService = new VendorCurrencyQueryService(customContext);


                var vendorCurrencyList = vendorCurrencyQueryService.GetVendorCurrencyByVendorId(tenant, VendorId);
                

                return Request.CreateResponse(HttpStatusCode.OK, vendorCurrencyList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}