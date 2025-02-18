using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.Security;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
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

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    
    public class CB_PreferenceController : ApiController
    {
		public HttpResponseMessage GetCB_PreferenceByUserIdAndTenant(string userId, int tenant = 0)
		{
			try
			{
                CB_PreferenceQueryService preferenceQueryServiceQueryService =  new CB_PreferenceQueryService(tenant);
                var CBPreferences = preferenceQueryServiceQueryService.GetCB_PreferenceByUserIdAndTenant(userId, tenant);
				return Request.CreateResponse(HttpStatusCode.OK, CBPreferences);
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}
		}
    }
}