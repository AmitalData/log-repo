
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.Batch;
using Logitude.Accounting.BL.CoreBL.FunctionalTests;
using Logitude.Accounting.BL.CoreBL.Testers;
using Logitude.Accounting.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
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


                    bool batchIt = true;
                    
                    if (batchIt)
                    {
                        string batchTaskId;
                        using (var scope = TransactionFactory.GetNewTransaction())
                        {


                            var _resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(authToken.Tenant);
                            string communicationLogId;

                            var myByteData = Encoding.UTF8.GetBytes(winHebrewString);
                            communicationLogId = Communications.AddCommunicationLog(new CommunicationsParams()
                            {
                                Tenant = authToken.Tenant,
                                CommunicationLogTypeCode = "Q",
                                QueueName = "externaltasksqueue" + authToken.Tenant + 1,
                                Priority = 1,
                                InOut = "O",
                                Status = "D",
                                FileExtension = "xml",
                                //LoggingUserId = loggedUserId,
                                //LoggingObjectTableId = table.Id,
                                //LoggingEntityId = extDocPM.Id,

                                FolderName = "System1000FlatFileAnalyser",

                                To = "System1000FlatFileAnalyser",


                                Subject = "System1000FlatFileAnalyser holder ",
                                ByteData = myByteData


                            });



                            var myBatchTask = new BatchSystem1000FlatFileAnalyser(null);
                            string subj = $"System1000FlatFileAnalyser";
                             batchTaskId = myBatchTask.CreateQBatchTaskExecution<System1000FlatFileAnalyserArgs>(
                                new System1000FlatFileAnalyserArgs()
                                {
                                    Tenant = authToken.Tenant,
                                    LoggingUserId = _resolveLoggingUserId,
                                    CommunicationLogId = communicationLogId,
                                }, authToken.Tenant, subj, false);

                            scope.Complete();
                           
                        }
                        string transText = "";
                        bool useLocal = true;
                        transText = TranslateTextsClassTranslate("Accounting.O.Sys1000Background", 0, useLocal);
                        if (String.IsNullOrWhiteSpace(transText))
                        {
                            transText = "The System 1000 file load process will be performed in the background";
                        }

                        var res1 = new { Success = true, Message = transText }; 
                        return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                    }
                    //response.Result = bankAccountPageAnalyzer.MyResultLoadBankPage;
                    var system1000FlatFileAnalyser = new System1000FlatFileAnalyser();
                    system1000FlatFileAnalyser.Analyse(authToken.Tenant, winHebrewString,null);
                    
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
        public static ITextCodeTranslator OverrideITextCodeTranslator { get; set; }

        public virtual string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            if (OverrideITextCodeTranslator != null)
            {
                return OverrideITextCodeTranslator.Translate(textCodeCode, tenant);
            }
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
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

        public HttpResponseMessage GetTestOperation(string operationId,string myparams)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                var gateWayTester = new GateWayTester();
                var res=gateWayTester.TestIt(operationId, authToken.Tenant, myparams);
                bool testWithOutToken=false;
                if (testWithOutToken)
                {
                    int tenant = 10;
                    res = gateWayTester.TestIt(operationId, tenant, myparams);
                }

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PostTestOperation(FlatFileClass myparams)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                myparams.FlatFile=myparams.FlatFile.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
                var gateWayTester = new GateWayTester();
                var res = gateWayTester.TestIt(myparams.OperationId, authToken.Tenant, myparams.FlatFile);

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        
    }
    public class FlatFileClass
    {
        
        public string OperationId { get; set; }
        public string FlatFile { get; set; }
    }
}