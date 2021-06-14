using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.Batch;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.AccountingModel //AccountingPeriodViewsController.cs
{
    //[RoutePrefix("api/ReconciliationOp")]
    public partial class ChargesTypeOpController : ApiController
    {
        public ChargesTypeOpController()
        {

        }

        public HttpResponseMessage PostChargesTypeAsCSV(ImageParameter fileUploadParamerter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string documentId = "";
                if (fileUploadParamerter != null && !string.IsNullOrEmpty(fileUploadParamerter.Base64String))
                {
                    byte[] dosBytes = Convert.FromBase64String(fileUploadParamerter.Base64String);
                    //string decodedString = Encoding.UTF8.GetString(data);

                    //var dosEnc = System.Text.Encoding.GetEncoding("DOS-862"); // ms-dos codepage ( US English )
                    //var winHebrewEncoding = Encoding.GetEncoding("Windows-1255");
                    //string dosS = dosEnc.GetString(dosBytes);

                    //var hebBytes = Encoding.Convert(dosEnc, winHebrewEncoding, dosBytes);
                    //string winHebrewString = winHebrewEncoding.GetString(hebBytes);
                    string winHebrewString = Encoding.GetEncoding("Windows-1255").GetString(dosBytes);

                    var myChargeTypesCSVFlatFileAnalyser = new ChargeTypesCSVFlatFileAnalyser();
                    myChargeTypesCSVFlatFileAnalyser.Analyse(authToken.Tenant, winHebrewString);


                    return Request.CreateResponse(HttpStatusCode.OK);//, chargesTypePM);



                }
                else
                {
                    throw new Exception("fileUploadParamerter is empty");
                }



            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

}