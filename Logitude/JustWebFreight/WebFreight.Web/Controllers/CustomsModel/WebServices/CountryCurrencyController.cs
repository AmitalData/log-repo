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
    public class CountryCurrencyController : ApiController
    {

       
       
        public HttpResponseMessage UpadateListCurrencyByCountry(List<CountryCurrencyPM> countryCurrencyPMs)
        {
            try
            { 
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
               
                ICustomContext customContext = CustomContext.GetContext(tenant);
                CountryCurrencyUpdateService countryCurrencyUpdateService = new CountryCurrencyUpdateService(customContext, new Dictionary<string, IContext>(), tenant);

               
              
                if (countryCurrencyPMs != null && countryCurrencyPMs.Count > 0) {
                    countryCurrencyUpdateService.FastDelete(countryCurrencyPMs[0].CountryId, tenant);
                    (customContext as DbContextBase).SaveChanges();
                    countryCurrencyPMs = countryCurrencyPMs.Where(v => v.CurrencyTypeName != "-1").ToList();

                    foreach (CountryCurrencyPM countryCurrencyPM in countryCurrencyPMs)
				  {
                        countryCurrencyPM.ChangeSetOp = ChangeSetOperation.Insert;
                        countryCurrencyUpdateService.Update(countryCurrencyPM, true);

                  }
                }
              
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

     
    }
}