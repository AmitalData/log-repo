using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using System.Transactions;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Logitude.Accounting.BL.Utils;
using Logitude.Accounting.BL.CoreBL;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated 
{
    //[RoutePrefix("api/PostDatedChequesRedemptionOp")]
    public partial class ARPaymentChequeOpController : ApiController
    {
        public ARPaymentChequeOpController()
        {

        }
        public HttpResponseMessage GetRunAllPayablePostDatedARPaymentCheques(int tenant)
        {
            try
            {


                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                PostDatedChequesRedemptionBatch postDatedChequesRedemptionBatch = new PostDatedChequesRedemptionBatch();
                postDatedChequesRedemptionBatch.RunAllPayablePostDatedARPaymentCheques(tenant);
                string responseText = postDatedChequesRedemptionBatch.ResponseText();
                HttpStatusCode StatusCode = postDatedChequesRedemptionBatch.StatusCode();
                var res1 = new { Success = true, Message = responseText };

                return Request.CreateResponse(StatusCode, res1);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }






        //[Route("{obj:PostDatedChequesRedemptionPM}/InsertPostDatedChequesRedemption")]
        public HttpResponseMessage PostInsertPostDatedChequesRedemption(ARPaymentChequePM entityPm)
        {
            try
            {


                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("PostDatedChequesRedemption", "NEW", authToken.Tenant);
                var accountingContext = AccountingContext.GetContext(authToken.Tenant);
                var qs = new LedgerTransactionListQueryService(accountingContext);

                ARPaymentChequeUpdateService service = new ARPaymentChequeUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPm.Tenant);
                if (entityPm.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                {
                    throw new Exception("Meanwhile Only Insert Enable ");
                }
                entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                entityPm.Tenant = authToken.Tenant;
                service.Update(entityPm, true);
                return Request.CreateResponse(HttpStatusCode.OK, entityPm);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



        public HttpResponseMessage DeletePostDatedChequesRedemption(string PostDatedChequesRedemptionOperation, string postDatedChequesRedemptionId, int tenant)
        {
            ARPaymentChequePM entitypm = null;
            try
            {

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("ARPaymentCheque", "NEW", authToken.Tenant);
                    var accountingContext = AccountingContext.GetContext(authToken.Tenant);
                    var service = new ARPaymentChequeUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
                    //entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    //service.Update(entityPM, true);
                    PostDatedChequesRedemptionOperation = PostDatedChequesRedemptionOperation ?? string.Empty;
                    switch (PostDatedChequesRedemptionOperation.ToLower())
                    {
                        //case "cancell":
                        //    {
                        //        entitypm = service.CancelARPaymentCheque(postDatedChequesRedemptionId, tenant);
                        //   }
                        //   break;
                        case "purge":
                            {
                                throw new Exception("ARPaymentCheque Operation purge is not implement yet ...");
                            }
                            break;
                        default:
                            throw new Exception("ARPaymentCheque Operation unknown");
                            break;
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, entitypm);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostReturnChequeToCustomer([FromUri] ARPaymentChequeReturnServiceArguments serviceArguments)
        {
            try
            {
                AuthinticateTenant();
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    ARPaymentChequeReturnService chequeReturnService = new ARPaymentChequeReturnService(serviceArguments);
                    chequeReturnService.ReturnChequeToCustomer();
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private static void AuthinticateTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
        }
    }



    public class OpenPostDatedChequesRedemptionAggregate
    {
        public GenericCallBack CallBack { get; set; }

        public List<GLAccountList> OpenPostDatedChequesRedemption { get; set; }
        public List<GLAccountList> OpenPostDatedChequesRedemptionDraft { get; set; }
    }
}