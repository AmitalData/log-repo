using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Transactions;
using Logitude.BL.Helpers;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.BL;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using Logitude.Customs.BL.Messaging.Amital;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class UrouterController : ApiController
    {
        //http://localhost:9996/API/Urouter/GetMSVGStatusList?tenant=1&customFileNo=41100314
        //Task 63001: מסך רפרנט - הערות של סיווג /מבקר
        public HttpResponseMessage GetMSVGStatusList(int tenant, string customFileNo)
        {
            try
            {
                //string token = HttpContext.Current.Request.Headers["Token"];
                //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                //int tenant1 = authToken.Tenant;
                //string loggedUserEmail = authToken.Email;
                //SecurityUtility.AuthenticationOnTenant(tenant);

                var UserverGetStatusList = new UnifreightQStatusList();
                string ErrMessage = "";
                var StatusItemlist = UserverGetStatusList.GetStatusList(tenant, customFileNo, UnifreightQStatusList.StatusPartnerEnum.MSVG,
                      out ErrMessage);
                return Request.CreateResponse(HttpStatusCode.OK, new { StatusItemlist = StatusItemlist, ErrMessage = ErrMessage });
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetMVKRStatusList(int tenant, string customFileNo)
        {
            try
            {
                //string token = HttpContext.Current.Request.Headers["Token"];
                //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                //int tenant1 = authToken.Tenant;
                //string loggedUserEmail = authToken.Email;
                //SecurityUtility.AuthenticationOnTenant(tenant);

                var UserverGetStatusList = new UnifreightQStatusList();
                string ErrMessage = "";
                var StatusItemlist = UserverGetStatusList.GetStatusList(tenant, customFileNo, UnifreightQStatusList.StatusPartnerEnum.MVKR,
                      out ErrMessage);
                return Request.CreateResponse(HttpStatusCode.OK, new { StatusItemlist = StatusItemlist, ErrMessage = ErrMessage });
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage GetInvoice()
        {
            try
            {

                var UserverGetInvoiceList = new UnifreightQInvoiceList();
                string ErrMessage = "";
                var Invoice = UserverGetInvoiceList.GetInvoice();
                return Request.CreateResponse(HttpStatusCode.OK, new { Invoice = Invoice, ErrMessage = ErrMessage });
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}