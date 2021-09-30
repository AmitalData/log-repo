using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using WebFreight.Web.CustomWebServices;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class NewQuoteOPWebServiceController : ApiController
    {
        List<AmitalContext> _AmitalContextList = new List<AmitalContext>();
        AmitalContext GetAmitalContext(int tenant)
        {
            var tenantAmitalContext = _AmitalContextList.FirstOrDefault(rec => rec.TenantSeed == tenant);
            if (tenantAmitalContext == null)
            {

                tenantAmitalContext = AmitalContext.GetContext(tenant);
                _AmitalContextList.Add(tenantAmitalContext);
            }
            return tenantAmitalContext;
        }
        public HttpResponseMessage GetETBPAYTRitemList(string PTERMID, string search, int top, bool searchNULLVendor = true)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                if (search != null && (search.ToLower() == "undefined" || search.ToLower() == "null"))
                {
                    search = null;
                }

                if (PTERMID != null && (PTERMID.ToLower() == "undefined" || PTERMID.ToLower() == "null"))
                {
                    PTERMID = null;
                }

                #region get data from Unifri

                CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
                CustomsSettingPM setting = settingService.GetSettingByTenantN(tenant);

                if (setting.IsConnectedToUniFreight)
                {
                    var ETBPAYTR = new ETBPAYTRRepository(GetAmitalContext(tenant));
                    var q =
                    from itm in ETBPAYTR
                        .GetAll().Select(o => new
                        {
                            Name = o.NAMEENG,
                            PTERMID = o.PTERMID ,
                        })
                    select new { itm };

                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        search = search.ToUpper();
                        q = q.Where(rec => rec.itm.PTERMID.Contains(search));
                    }
                    q = q.Distinct();
                    q = q.Take(top);

                    var ETBPAYTRList = q.ToList();
                    #endregion

                    return Request.CreateResponse(HttpStatusCode.OK, ETBPAYTRList);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}

