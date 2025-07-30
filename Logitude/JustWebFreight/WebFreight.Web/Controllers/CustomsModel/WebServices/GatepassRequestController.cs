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
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

	public class GatepassRequestController : ApiController
	{
		public HttpResponseMessage GetGatepassRequestByMasterCourierId(string masterCourierId, int tenant)
		{
			try
			{
				GatepassRequestQueryService gatepassRequestQueryService = new GatepassRequestQueryService(tenant);
				GatepassRequestPM gatepassRequest = gatepassRequestQueryService.GetGatepassRequestByMasterCourierId(masterCourierId, tenant);

				return Request.CreateResponse(HttpStatusCode.OK, gatepassRequest);
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}
		}

	}
}