using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Logitude.Accounting.BL.Utils;
using System.Net;
using WebFreight.Web.Helpers;
using System.Text.RegularExpressions;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.CoreBL.Batch;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.Repositories;
using System.Globalization;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;

namespace WebFreight.Web.Controllers.AccountingModel
{
    //[RoutePrefix("api/BanksCCExternalReco")]
    public class BanksCCExternalRecoController : ApiController
    {
        public BanksCCExternalRecoController()
        {

        }


        public HttpResponseMessage GetBanksCCExternalReco(int tenant, string accountId, string accountDisplayNumber, string toAccountingDate, string batch)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                BanksCCExternalRecoArg args = null;
                string message = "";
                bool isSuccess = CreateArgs(tenant, accountId, accountDisplayNumber, toAccountingDate, batch, ref args, message);
                if (!isSuccess)
                {
                    throw new Exception(message);
                }

                bool batchIt = args.Batch;
                if (batchIt)
                {

                    var accountingContext = AccountingContext.GetContext(tenant);

                    var myBatchBanksCCExternalRecoTask = new BatchBanksCCExternalRecoTask(null);
                    string subj = $"Banks and CC External Reconcile";
                    var batchTaskId = myBatchBanksCCExternalRecoTask.CreateQBatchTaskExecution<BanksCCExternalRecoArg>(
                        new BanksCCExternalRecoArg()
                        {
                            Tenant = tenant,
                            AccountId = args.AccountId,
                            ToAccountingDate = args.ToAccountingDate,
                        }, tenant, subj, false);


                    var res1 = new { Success = true, Message = $"Send to Batch Task {batchTaskId}" };
                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    BanksCCExternalRecoBatch banksCCExternalRecoBatch = new BanksCCExternalRecoBatch();
                    BanksCCExternalRecoArg banksCCExternalRecoArg = new BanksCCExternalRecoArg()
                    {
                        Tenant = tenant,
                        AccountId = args.AccountId,
                        ToAccountingDate = args.ToAccountingDate,
                    };
                    banksCCExternalRecoBatch.RunBanksCCExternalReco(banksCCExternalRecoArg);
                    string responseText = banksCCExternalRecoBatch.ResponseText();
                    HttpStatusCode StatusCode = banksCCExternalRecoBatch.StatusCode();
                    var res1 = new { Success = true, Message = responseText };

                    return Request.CreateResponse(StatusCode, res1);

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        private bool CreateArgs(int tenant, string accountId, string accountDisplayNumber, string toAccountingDate, string batch, ref BanksCCExternalRecoArg args, string message)
        {
            bool isSuccess = false;
            bool v_batch = false;
            if (String.IsNullOrWhiteSpace(accountId) && String.IsNullOrWhiteSpace(accountDisplayNumber))
            {
                message = "AccountId or DisplayNumber is a must";
                return isSuccess;
            }

            IAccountingContext accContext = AccountingContext.GetContext(tenant);
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(accContext);

            if (String.IsNullOrWhiteSpace(accountId))
            {


                List<GLAccountPM> gLAccountPMList = gLAccountQueryService.GetByDisplayNumber(accountDisplayNumber, tenant);
                if (gLAccountPMList == null || gLAccountPMList.Count == 0)
                {
                    message = "Account {accountDisplayNumber} not found";
                    return isSuccess;
                }
                if (gLAccountPMList.Count > 1)
                {
                    message = "More than one account is found - Display Number {accountDisplayNumber}";
                    return isSuccess;
                }
                accountId = gLAccountPMList.FirstOrDefault().Id;
            }
            else
            {
                var acc = gLAccountQueryService.GetSingleByAccountId(accountId, tenant);
                if (acc == null)
                {
                    message = "Account {accountId} not found";
                    return isSuccess;
                }
            }



            DateTime fromDateValue = DateTime.MinValue;
            DateTime toDateValue = DateTime.Today;


            if (!String.IsNullOrWhiteSpace(toAccountingDate))
            {
                try
                {
                    toDateValue = DateTime.ParseExact(toAccountingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                catch (FormatException ex)
                {
                    message = "Cannot parse ToAccountingDate";
                    return isSuccess;

                }
            }



            if (!String.IsNullOrEmpty(batch) && (batch == "1" || batch.ToUpperInvariant() == "TRUE"))
            {
                v_batch = true;
            }

            args = new BanksCCExternalRecoArg()
            {
                Tenant = tenant,
                AccountId = accountId,
                ToAccountingDate = toDateValue,
                Batch = v_batch,
            };
            isSuccess = true;
            return isSuccess;
        }


    }
}