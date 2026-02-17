

using System;

using WebFreight.Web.Security;
using WebFreight.Web.Helpers;

using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.CustomsMessaging.Common.Gen;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class SignStationStatusController : ApiController
    {
        public HttpResponseMessage Get(String MyId)
        {
            try
            {
                var signStationM = new SignStationStatus()
                {
                     CurrentSignCertificate = MyId,
                    
                };
                return Request.CreateResponse(HttpStatusCode.OK, signStationM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Post(SignStationStatus signStationM)
        {
            try
            {
                Logitude.Customs.BL.Messaging.Customs.SignQueue.Instance.UpsertSignStationStatus(signStationM);
                return Request.CreateResponse(HttpStatusCode.OK, signStationM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }

    
    public class SignStationM
    {
        public string MachineName { get; set; }
        public string UserName { get; set; }
    }

}