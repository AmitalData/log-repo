
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.FunctionalTests;
using Logitude.Accounting.Data;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.AccountingModel
{
    public class AccountingOpController : ApiController
    {
        public AccountingOpController()
        {

        }
        public HttpResponseMessage GetGenerate1000(string email)
        {
            try
            {
                
                using (TransactionScope scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(1)))
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("Journal", "UPDATE", authToken.Tenant);



                    //entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    //service.Update(entityPM, true);
                    string message = "NO Vendor Cards Having Deduction";
                    var accountingContext = AccountingContext.GetContext(authToken.Tenant);
                    ISystem1000Service System1000Service = new System1000Service();
                    var flatFiles = System1000Service.GetSystem1000FlatFile(accountingContext, authToken.Tenant);
                    if (flatFiles.Count > 0)
                    {
                        System1000Service.EmailIt(email, flatFiles, authToken.Tenant);
                        message = "System 1000 Flat File have sent ";
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, new { Message = message });
                }
            }




            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutSystem1000File(ImageParameter fileUploadParamerter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string documentId = "";
                if (fileUploadParamerter != null && !string.IsNullOrEmpty(fileUploadParamerter.Base64String))
                {
                    byte[] dosBytes = Convert.FromBase64String(fileUploadParamerter.Base64String);
                    //string decodedString = Encoding.UTF8.GetString(dosBytes);

                    var dosEnc = System.Text.Encoding.GetEncoding("DOS-862"); // ms-dos codepage ( US English )
                    var winHebrewEncoding = Encoding.GetEncoding("Windows-1255");
                    string dosS = dosEnc.GetString(dosBytes);

                    var hebBytes = Encoding.Convert(dosEnc, winHebrewEncoding, dosBytes);
                    string winHebrewString = winHebrewEncoding.GetString(hebBytes);


                    var response = new ServiceResponse();
                    //response.Result = bankAccountPageAnalyzer.MyResultLoadBankPage;
                    var system1000FlatFileAnalyser = new System1000FlatFileAnalyser();
                    system1000FlatFileAnalyser.Analyse(authToken.Tenant, winHebrewString);
                    
                    return Request.CreateResponse(HttpStatusCode.OK, new { Message = "Done" });
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

        public HttpResponseMessage PutFunctionalTestXLS(ImageParameter fileUploadParamerter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string documentId = "";
                if (fileUploadParamerter != null && !string.IsNullOrEmpty(fileUploadParamerter.Base64String))
                {
                    byte[] dosBytes = Convert.FromBase64String(fileUploadParamerter.Base64String);
                    //string decodedString = Encoding.UTF8.GetString(dosBytes);

                    //var dosEnc = System.Text.Encoding.GetEncoding("DOS-862"); // ms-dos codepage ( US English )
                    //var winHebrewEncoding = Encoding.GetEncoding("Windows-1255");
                    //string dosS = dosEnc.GetString(dosBytes);

                    //var hebBytes = Encoding.Convert(dosEnc, winHebrewEncoding, dosBytes);
                    //string winHebrewString = winHebrewEncoding.GetString(hebBytes);


                    var response = new ServiceResponse();
                    //response.Result = bankAccountPageAnalyzer.MyResultLoadBankPage;
                    var journalToGLAccountMoreData = new JournalToGLAccountMoreData();
                    journalToGLAccountMoreData.LoadXLSBuildTest(authToken.Tenant, dosBytes, "Journals");


                    return Request.CreateResponse(HttpStatusCode.OK, new { Message = "Done" });
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