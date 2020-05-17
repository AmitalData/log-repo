using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;
using System.IO;
using System.Net.Http.Headers;
using WebFreight.Web.CustomWebServices.BL.XLSImport;
using System.Text;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class SupplierInvioceItemCertificatsController : ApiController
    {
        public HttpResponseMessage PutSupplierInvioceItemCertificatFromFileRequest(int tenant, string clientId, ImageParameter fileUploadParamerter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                List<CertificateErrorView> errors = new List<CertificateErrorView>();
                if (fileUploadParamerter != null && !string.IsNullOrEmpty(fileUploadParamerter.Base64String))
                {
                    byte[] data = Convert.FromBase64String(fileUploadParamerter.Base64String);
                    string decodedString = Encoding.UTF8.GetString(data);
                    SupplierInvioceItemCertificats supplierInvioceItemCertificats = new SupplierInvioceItemCertificats();
                    errors = supplierInvioceItemCertificats.RecallSuppliersFromFileRequest(fileUploadParamerter.Key, tenant , decodedString,clientId);
                    CacheManager.CacheWrapper.Insert(fileUploadParamerter.Key+"IKEA-ErrorList", errors);
                }
                return Request.CreateResponse(HttpStatusCode.OK, errors);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetSupplierInvoiceItemCertificatErrors2Excel(int tenant, string key)
        {

            try
            {
                var ErrorObject = (List< CertificateErrorView>)CacheManager.CacheWrapper.Get(key+ "IKEA-ErrorList");
                var o = new SupplierInvioceItemCertificats();
                var result = o.ExportErrors(tenant, ErrorObject);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(new MemoryStream(result));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName =
                Guid.NewGuid().ToString() + "_" + key + ".xls";
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}


