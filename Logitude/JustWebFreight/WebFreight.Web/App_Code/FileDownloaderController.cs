using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WebFreight.Web.WebServices;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code
{
    public class FileDownloaderController : ApiController
    {
        public byte[] GetDownloadFile(string filename, string documentExtension, string fileLocation, int tenant)
        {

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            string result = "";
            Uploader uploader = new Uploader();



            byte[] data = uploader.DownloadFile(filename, documentExtension, fileLocation, tenant);

            return data;


        }
        public HttpResponseMessage GetFileStream(string id,string token)
        {

            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            return GetFileStream(id);
        }
        public HttpResponseMessage GetFileStream(string id)//THIS CODE USED  FROM  AmitalChromWinForm!!
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);\

            return Uploader.GetFileStream(id);
        }
    }
}