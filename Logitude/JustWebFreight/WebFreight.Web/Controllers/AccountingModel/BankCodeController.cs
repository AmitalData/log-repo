
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{
    public class BankCodeController : ApiController
    {
        //public HttpResponseMessage GetTenant0BankCodeByCode(string code)
        //{
        //    try
        //    {
             
        //       // string token = HttpContext.Current.Request.Headers["Token"];
        //       // AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //       // SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
        //       // SecurityUtility.CheckContactFeature("BankCode", "READ", authToken.Tenant);
        //       // IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
        //       // BankCodeQueryService bankCodeQueryService = new BankCodeQueryService(authToken.Tenant);
        //       // BankCodePM bankCode = bankCodeQueryService.GetSingleByCode(code, 0);
        //       //string imageDetailId = null;
        //       // if (bankCode != null)
        //       // {
        //       //     ImageDetailRepository imageDetailRepository = new ImageDetailRepository(authToken.Tenant);
        //       //     string extension = imageDetailRepository.GetImageExtensionbyId(0,bankCode.LogoId);


        //       //     Uploader uploaderService = new Uploader();

        //       //     byte[] filedata = uploaderService.DownloadFile(bankCode.LogoId, extension, "images", 0);
        //       //     string[] blockIdlist = { Convert.ToBase64String(Guid.NewGuid().ToByteArray()) };
        //       //     imageDetailId= uploaderService.UploadImage(bankCode.LogoId, filedata, filedata.Length, filedata.Length, blockIdlist, 0, authToken.Tenant, extension, null, null, bankCode.LogoId);

        //       // }
        //        return Request.CreateResponse(HttpStatusCode.OK, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }

        //}
    }
}