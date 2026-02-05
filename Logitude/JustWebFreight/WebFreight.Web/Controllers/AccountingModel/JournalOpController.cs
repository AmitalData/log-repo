
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.BL.EntityQueryServices;

using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data.Repositories;
using System.Transactions;
using WebFreight.Web.AccountingModel.Reports.Journal;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Accounting.BL.CoreBL.Batch;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.ExternalAPIHelpers;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated //AccountingPeriodViewsController.cs
{
    //[RoutePrefix("api/ReconciliationOp")]
    public partial class JournalOpController : ApiController
    {
        public JournalOpController()
        {

        }
        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(string JournalOp, string JournalId, int tenant, 
            string AccountingEntityCode, string AccountingEntityReference, string AccountingEntityId)
        {

            JournalPM journalPM = null;
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.AuthenticationOnTenant(tenant);
                    SecurityUtility.CheckContactFeature("Journal", "UPDATE", authToken.Tenant);

                    IAccountingContext MyContext = AccountingContext.GetContext(tenant);
                    
                    //entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    //service.Update(entityPM, true);
                    JournalOp=JournalOp??string.Empty;
                    switch (JournalOp.ToLower())
                    {
                        case "void":
                            {
                                var service = new JournalVoidUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                                
                                var StornoOverrideM = new StornoOverrideM()
                                {
                                    AccountingEntityCode = AccountingEntityCode,
                                    AccountingEntityId = AccountingEntityId,
                                    AccountingEntityReference = AccountingEntityReference,
                                };
                                journalPM = service.VoidJournal(JournalId, tenant, StornoOverrideM);
                            }
                            break;
                        case "purge":
                            {
                                throw new Exception("Journal Operation purge is not implement yet ...");
                            }
                            break;
                        default:
                            throw new Exception("Journal Operation unknown");
                            break;
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, journalPM);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetYearTransferJournal(int year,string myOperation, string lastYearTransferJournalPMId)
        {

            
            try
            {
                //JournalPM journal = null;
                using (TransactionScope scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("Journal", "UPDATE", authToken.Tenant);

                    IAccountingContext accountingContext = AccountingContext.GetContext(authToken.Tenant);
                    string userId = AuthenticationUtil.ResolveUserId(authToken.Tenant);

                    //entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    //service.Update(entityPM, true);
                    switch (myOperation)
                    {
                        case "Check_CreateQBatchTaskYearTransfer":
                            {
                                ICheckAndQYearTransferService yearTransferService = new YearTransferService();
                                //yearTransferService.CheckThrowExceptionIfNeeded(accountingContext, year, authToken.Tenant);
                                string BatchTaskYearTransferId = yearTransferService.Check_CreateQBatchTaskYearTransfer(year, authToken.Tenant, userId);
                                scope.Complete();
                                return Request.CreateResponse(HttpStatusCode.OK, new { BatchTaskYearTransferId  = BatchTaskYearTransferId });
                            }
                            break;
                        case "CheckCancelYear":
                            {
                                ICancelYearTransferService yearTransferService = new YearTransferService();
                                //yearTransferService.CheckThrowExceptionIfNeeded(accountingContext, year, authToken.Tenant);
                                var LastYearTransferJournalPM = yearTransferService.CheckCancelYear(accountingContext,year, authToken.Tenant);
                                scope.Complete();
                                return Request.CreateResponse(HttpStatusCode.OK, new { lastYearTransferJournalPMId = LastYearTransferJournalPM.Id });

                            }
                            break;

                        case "DoCancelYear":
                            {

                                ICancelYearTransferService yearTransferService = new YearTransferService();
                                //yearTransferService.CheckThrowExceptionIfNeeded(accountingContext, year, authToken.Tenant);
                                var LastYearTransferJournalPM = yearTransferService.CheckCancelYear(accountingContext, year, authToken.Tenant);
                                if (LastYearTransferJournalPM.Id != lastYearTransferJournalPMId)
                                {
                                    throw new Exception("Its seemed onther thread Cancel Journal " + lastYearTransferJournalPMId);
                                }
                                JournalPM stornoJournalPM = yearTransferService.DoCancelYear(accountingContext, LastYearTransferJournalPM, authToken.Tenant/*, userId*/);
                                scope.Complete();
                                return Request.CreateResponse(HttpStatusCode.OK, stornoJournalPM);

                            }
                            break;

                        default:
                            throw new Exception("myOperation must be ");
                            break;
                    }
                    


                    //journal = yearTransferService.ProccessJournal(accountingContext, year, authToken.Tenant);
                    //bool SuppressCheckGLAccountIsMultiCurrencyWI40640 = false;
                    //var parser = new JournalApproveParser(journal, false,
                    //AccountingValidationContextServiceProvider.NewJournalValidatorContextByAContext(accountingContext, journal, SuppressCheckGLAccountIsMultiCurrencyWI40640)
                    //);
                    //parser.ParseIt();

                }
            }

            
            

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetTaskLoadTest(int tenant, string actionType, int amount, int sleepEveryMinute, int year)
        {


            try
            {
                JournalPM journal = null;
                using (TransactionScope scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("Journal", "UPDATE", authToken.Tenant);

                    IAccountingContext accountingContext = AccountingContext.GetContext(authToken.Tenant);

                    //entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    //service.Update(entityPM, true);
                    var myBatchAccountingLoadTestTask = new BatchAccountingLoadTestTask( new Logitude.Infrastructure.BL.EntityPMs.BatchTaskExecutionPM());
                    myBatchAccountingLoadTestTask.CreateBatchAccountingLoadTestTask(tenant, actionType, amount, sleepEveryMinute, year);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, journal);
                }
            }




            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetJournalByAccountingEntityId(string accountingEntityId, string accountingEntityCode)
        {
            try
            {
                //JournalPM journal = null;
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("Journal", "READ", authToken.Tenant);

                    IAccountingContext accountingContext = AccountingContext.GetContext(authToken.Tenant);
                    JournalQueryService journalQueryService = new JournalQueryService(accountingContext);

                    JournalPM journalPM = journalQueryService.GetByAccountingEntityIdAndAccountingEntityCode(accountingEntityId, accountingEntityCode, authToken.Tenant);

                    return Request.CreateResponse(HttpStatusCode.OK, journalPM);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostJournalAsCSV(ImageParameter fileUploadParamerter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if (fileUploadParamerter != null && !string.IsNullOrEmpty(fileUploadParamerter.Base64String))
                {
                    byte[] dosBytes = Convert.FromBase64String(fileUploadParamerter.Base64String);

                    string winHebrewString = DecodeHebrewBytes(dosBytes);

                    var myJournalsCSVFlatFileAnalyser_ISL = new JournalsCSVFlatFileAnalyser_ISL();
                    var journalPM =myJournalsCSVFlatFileAnalyser_ISL.Analyse(authToken.Tenant, winHebrewString);


                    return Request.CreateResponse(HttpStatusCode.OK, journalPM);


                    
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



        public HttpResponseMessage PostJournalAsCSVWithSkip(ImageParameter fileUploadParamerter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if (fileUploadParamerter != null && !string.IsNullOrEmpty(fileUploadParamerter.Base64String))
                {
                    byte[] dosBytes = Convert.FromBase64String(fileUploadParamerter.Base64String);
                    string winHebrewString = DecodeHebrewBytes(dosBytes);

                    var myJournalsCSVFlatFileAnalyser_ISL = new JournalsCSVFlatFileAnalyser_ISL();
                    var journalAnalyseResult = myJournalsCSVFlatFileAnalyser_ISL.AnalyseWithSkip(authToken.Tenant, winHebrewString);


                    return Request.CreateResponse(HttpStatusCode.OK, journalAnalyseResult);



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

        public HttpResponseMessage PostJournalAsMichpal(JournalPM entityPM)
        {
              
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string logKey = PerformanceLogger.LogCurrentTime();
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.CheckContactFeature("Journal", "NEW", authToken.Tenant);
                        SecurityUtility.AuthenticationOnEntityTenant("Journal", entityPM.Tenant, authToken.Tenant);

                        IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
                        JournalUpdateService service = new JournalUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;                       
                        service.JournalAsMichpal(entityPM);         
                        service.Update(entityPM, true);
                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
        }

        [HttpPut]
        public HttpResponseMessage PutCreateInterestTransactions(string date)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        int tenant = authToken.Tenant;
                        SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                        SecurityUtility.AuthenticateAccessibleAPI("Journal", authToken.Tenant);
                        JournalApproveService journalApproveService = new JournalApproveService(tenant, null, null, null);
                        journalApproveService.CreateInterestTransactionsByDate(DateTime.Parse(date));


                        scope.Complete();

                        return Request.CreateResponse(HttpStatusCode.OK, "Done");
                    }
                }
                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }


        public static string DecodeHebrewBytes(byte[] dosBytes)
        {
            try
            {
                // Try UTF-8 first
                string decoded = Encoding.UTF8.GetString(dosBytes);

                // If the text has replacement characters (�), it likely means wrong encoding
                if (decoded.Contains("�"))
                {
                    decoded = Encoding.GetEncoding(1255).GetString(dosBytes); // Windows-1255
                }

                return decoded;
            }
            catch
            {
                // Fallback to Windows-1255 if UTF-8 throws
                return Encoding.GetEncoding(1255).GetString(dosBytes);
            }
        }


        public HttpResponseMessage GetFailedJournalInReconcileProcess(string accountId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;
                SecurityUtility.CheckContactFeature("Journal", "READ", authToken.Tenant);

                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                JournalQueryService journalQuery = new JournalQueryService(MyContext);
                List<Journal> journals = journalQuery.GetFailedJournalsInReconcileProcess(accountId, tenant);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, journals);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [HttpPut]
        public HttpResponseMessage FixFailedReconcileJournals()
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;
                SecurityUtility.CheckContactFeature("Journal", "READ", authToken.Tenant);

                IAccountingContext context = AccountingContext.GetContext(authToken.Tenant);
                JournalQueryService journalQueryService = new JournalQueryService(context);
                journalQueryService.FixFailedReconcileJournals(tenant);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }

}