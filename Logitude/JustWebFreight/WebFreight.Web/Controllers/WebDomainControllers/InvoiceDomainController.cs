using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.InvoiceModel.Tools.Validating;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.InvoiceModel;
using WebFreight.Web.InvoiceModel.DomainServices;
using WebFreight.Web.MessageModel;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class InvoiceDomainController : ApiController
    {
        public HttpResponseMessage GetAccountingReceivablesSummary()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                InvoiceDomainService domain = new InvoiceDomainService();
                AccountReceivablesSummary myResult = domain.GetAccountingReceivablesSummary(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetMoneyStatusForTenant(string type, int months, int days, int tenant, int index, int currency)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ARInvoiceQuery arInvoiceQuery = new ARInvoiceQuery(tenant);
                List<MoneyStatusClass> myResult = arInvoiceQuery.GetMoneyStatusForTenant(type, months, days, tenant, index, currency);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetMoneyStatusForTenantCustom(string type, string ToDate, string FromDate)
        {
            try
            {
                if (FromDate == "null")
                    FromDate = null;

                if (ToDate == "null")
                    ToDate = null;

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                DateTime? FromDateOBJ = DateHelper.GetDate(FromDate);
                if (FromDate == null)
                {
                    FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                }


                DateTime? ToDateOBJ = DateHelper.GetDate(ToDate);
                if (ToDateOBJ == null)
                {
                    ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                }
                ARInvoiceQuery arInvoiceQuery = new ARInvoiceQuery(tenant);
                List<MoneyStatusClass> myResult = arInvoiceQuery.GetMoneyStatusForTenantCustom(type, ToDateOBJ, FromDateOBJ, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetMoneyOutStatusForTenant(int months, int days, int tenant, int index, int currency)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ARInvoiceQuery arInvoiceQuery = new ARInvoiceQuery(tenant);
                List<MoneyStatusClass> myResult = arInvoiceQuery.GetMoneyOutStatusForTenant(months, days, tenant, index, currency);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetDebrotExposure(int tenant, int currency)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ARInvoiceQuery arInvoiceQuery = new ARInvoiceQuery(tenant);
                List<DebtorsClass> myResult = arInvoiceQuery.GetDebtorExposure(tenant, currency);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAccountPayablesSummary()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                InvoiceDomainService domain = new InvoiceDomainService();
                AccountPayablesSummary myResult = domain.GetAccountingPayablesSummary(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetDebrotExposureForGridControl(int index)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ARInvoiceQuery arInvoiceQuery = new ARInvoiceQuery(tenant);
                List<DebtorsClass> myResult = arInvoiceQuery.GetDebtorsExposureForGridControl(tenant, index);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAgingReportARInvioceData(int index, string customerId)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(tenant);

                IQueryable<AgingReportInvoiceDataView> reports = BranchPermitionsFilter.AddUserBranchRestrictionFilters<AgingReportInvoiceDataView>(new QueryOperations(), aRInvoiceRepository.GetAgingReportInvoiceDataView(tenant, index, null), tenant);

                List<AgingReportDashboardClass> myResult = aRInvoiceRepository.GetAgingReportInvoiceData(index, reports);


                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCreditorExposure(int index)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                APInvoiceQuery apInvoiceQuery = new APInvoiceQuery(tenant);
                List<CreditorsClass> myResult = apInvoiceQuery.GetDebtorsExposureForGridControl(tenant, index);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetAgingReportAPInvioceData(int index)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                APInvoiceRepository aPInvoiceRepository = new APInvoiceRepository(tenant);

                IQueryable<APAgingReportDataView> reports = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APAgingReportDataView>(new QueryOperations(), aPInvoiceRepository.GetAgingReportAPInvoiceDataView(tenant, index).AsQueryable<APAgingReportDataView>(), tenant);

                List<AgingReportDashboardClass> myResult = aPInvoiceRepository.GetAgingReportAPInvoiceData(tenant, index, reports).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetARPaymentValidatingList(string paymentMethod, string currency, string billTo, string code, string registergDateString, string bankAccountId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ICashBookQueryServiceExt cashQuery = ContainerAccessor.Container.Resolve(typeof(ICashBookQueryServiceExt), "CashBookQueryServiceExt", new ParameterOverride("", 1)) as ICashBookQueryServiceExt;

                paymentMethod = CheckNullValue(paymentMethod);
                currency = CheckNullValue(currency);
                billTo = CheckNullValue(billTo);
                code = CheckNullValue(code);
                bankAccountId = CheckNullValue(bankAccountId);

                var methodType = paymentMethod;
                if (paymentMethod == "CA")
                {
                    methodType = "1";
                }
                else if (paymentMethod == "CH")
                {
                    methodType = "2";
                }

                if (registergDateString == "null")
                {
                    registergDateString = null;
                }

                DateTime? registergdate = DateHelper.GetDate(registergDateString);
                CashBookPM cashBook = cashQuery.GetByPaymentAndCurrency(currency, methodType, tenant);
                ARPaymentValidator.ValidateFullAccounting(tenant, billTo, currency, cashBook, code, registergdate, bankAccountId, true);

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetARInvoiceValidatingList(string currency, string billTo, string accountingDateString)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                if (accountingDateString == "null")
                {
                    accountingDateString = null;
                }

                DateTime? accountingDate = DateHelper.GetDate(accountingDateString);
                ARInvoiceValidator.ValidateFullAccounting(tenant, billTo, currency, accountingDate, true);

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetAPInvoiceValidatingList(string currency, string vendor, string accountingDateString)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                if (accountingDateString == "null")
                {
                    accountingDateString = null;
                }

                DateTime? accountingDate = DateHelper.GetDate(accountingDateString);
                APInvoiceValidator.ValidateFullAccounting(tenant, vendor, currency, accountingDate);
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetIsARInvoiceNumberExists(string InvoiceNumber)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                if (InvoiceNumber == "null")
                {
                    InvoiceNumber = null;
                }

                ARInvoiceRepository entityRepository = new ARInvoiceRepository(tenant);

                bool myResult = entityRepository.IsARInvoiceNumberExists(InvoiceNumber, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAutoCreditARInvoice(string entityId, bool IsInvoiceNumberManuallySet, string AutoCreditManualNumber, string AutoCreditDateString)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("ARInvoice", "NEW", tenant);

                if (AutoCreditManualNumber == "null")
                {
                    AutoCreditManualNumber = null;
                }

                if (AutoCreditDateString == "null")
                {
                    AutoCreditDateString = null;
                }

                DateTime? AutoCreditDate = DateHelper.GetDate(AutoCreditDateString);
                IInvoiceContext objectContext = InvoiceContext.GetContext(tenant);
                ARInvoiceService service = new ARInvoiceService(objectContext, tenant);
                string AutoCreditId = service.CreateAutoCredit(entityId, IsInvoiceNumberManuallySet, AutoCreditManualNumber, AutoCreditDate);

                return Request.CreateResponse(HttpStatusCode.OK, AutoCreditId);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostAPInvoiceNumberDuplicationCheck(APInvoiceNumberDuplicationCheckArgs args)
        {
            try
            {
                string vendorId = args.VendorId;
                string entityId = args.EntityId;
                string invoiceNumber = args.InvoiceNumber;

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                bool isDuplicated = false;

                APInvoiceRepository aPInvoiceRepository = new APInvoiceRepository(tenant);

                IQueryable<APInvoice> iQueryable = aPInvoiceRepository.GetIQueryableInvoices(tenant);

                if (string.IsNullOrEmpty(entityId))
                {
                    isDuplicated = iQueryable.Where(d => d.VendorId == vendorId && d.InvoiceNumber == invoiceNumber).Any();
                }

                else
                {
                    isDuplicated = iQueryable.Where(d => d.VendorId == vendorId && d.InvoiceNumber == invoiceNumber && d.Id != entityId).Any();
                }

                return Request.CreateResponse(HttpStatusCode.OK, isDuplicated);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetARPaymentCashBook(string paymentMethod, string currency, string branch)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ICashBookQueryServiceExt cashQuery = ContainerAccessor.Container.Resolve(typeof(ICashBookQueryServiceExt), "CashBookQueryServiceExt", new ParameterOverride("", 1)) as ICashBookQueryServiceExt;

                paymentMethod = CheckNullValue(paymentMethod);
                currency = CheckNullValue(currency);
                branch = CheckNullValue(branch);

                var methodType = paymentMethod;
                if (paymentMethod == "CA")
                {
                    methodType = "1";
                }
                else if (paymentMethod == "CH")
                {
                    methodType = "2";
                }

                List<CashBookPM> cashbook = cashQuery.GetListByPaymentAndCurrencyAndBranch(methodType, currency, branch, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, cashbook);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCustomerCreditLimitActualAmount(string myCustomerId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ARInvoiceQuery entityQuery = new ARInvoiceQuery(tenant);

                double? myResult = entityQuery.GetCustomerCreditLimitActualAmount(myCustomerId, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSingleAPInvoiceShortPM(string myInvoiceId, string myShipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("APInvoice", "READ", tenant);

                APInvoiceQuery entityQuery = new APInvoiceQuery(tenant);
                APInvoiceMultipleShortPM myResult = entityQuery.GetSingleAPInvoiceShortPM(myInvoiceId, myShipmentId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PutAPInvoiceMultipleShortPM(APInvoiceMultipleShortPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int tenant = authToken.Tenant;

                        SecurityUtility.AuthenticationOnTenant(tenant);
                        SecurityUtility.CheckContactFeature("APInvoice", "UPDATE", entityPM.Tenant);

                        IInvoiceContext myContext = InvoiceContext.GetContext(tenant);

                        APInvoiceMultipleShortService service = new APInvoiceMultipleShortService(myContext, entityPM);
                        service.Update(entityPM.InvoiceLines);

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }
        public HttpResponseMessage GetAccountingTransferSummary()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                InvoiceDomainService domain = new InvoiceDomainService();
                AccountTransferSummary myResult = domain.GetAccountingTransferSummary(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetRebuildTransferFile(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                IInvoiceContext myContext = InvoiceContext.GetContext(tenant);

                AccountingTransferHeaderRepository entityRepository = new AccountingTransferHeaderRepository(myContext);
                AccountingTransferLineRepository entityLineRepository = new AccountingTransferLineRepository(myContext);

                AccountingTransferHeader entityPoco = entityRepository.GetSingleEntity(entityId, tenant);
                List<string> entitiesIdsList = entityLineRepository.GetInvoicesIdsList(entityId, tenant);

                MessageWebService webService = new MessageWebService();

                switch (entityPoco.AccountingTransferTypeCode)
                {
                    case "ARIN":
                        {
                            ARInvoiceRepository myRepository = new ARInvoiceRepository(myContext);
                            List<ARInvoice> entities = myRepository.GetInvoicesListFromIdList(entitiesIdsList, tenant);
                            webService.RebuildTransferFile(entities, entityPoco.FileName, tenant);
                            break;
                        }

                    case "APIN":
                        {
                            APInvoiceRepository myRepository = new APInvoiceRepository(myContext);
                            List<APInvoice> entities = myRepository.GetInvoicesListFromIdList(entitiesIdsList, tenant);
                            webService.RebuildTransferFile(entities, entityPoco.FileName, tenant);
                            break;
                        }

                    case "APPA":
                        {
                            ARPaymentRepository myRepository = new ARPaymentRepository(myContext);
                            List<ARPayment> entities = myRepository.GetPaymentsListFromIdList(entitiesIdsList, tenant);
                            webService.RebuildTransferFile(entities, entityPoco.FileName, tenant);
                            break;
                        }
                }

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetNotReadyARInvoicesIds()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                List<string> myResult = new List<string>();

                if (SecurityUtility.CheckTableContactFeature("ARInvoice", "READ", tenant))
                {
                    ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(tenant);

                    IQueryable<ARInvoice> iQueryable_Data = aRInvoiceRepository.GetIQueryableInvoices(tenant);

                    iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD" && d.StatusCode != "LL" && d.IsConstituentInvoice == false);
                    iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARInvoice>(new QueryOperations(), iQueryable_Data, tenant);
                    iQueryable_Data = iQueryable_Data.Where(d => d.TransferStatusCode == "NR");

                    myResult = iQueryable_Data.Select(s => s.Id).ToList();
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetNotReadyAPInvoicesIds()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                List<string> myResult = new List<string>();

                if (SecurityUtility.CheckTableContactFeature("APInvoice", "READ", tenant))
                {
                    APInvoiceRepository aPInvoiceRepository = new APInvoiceRepository(tenant);

                    IQueryable<APInvoice> iQueryable_Data = aPInvoiceRepository.GetIQueryableInvoices(tenant);

                    iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "WA" && d.StatusCode != "VD");
                    iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APInvoice>(new QueryOperations(), iQueryable_Data, tenant);
                    iQueryable_Data = iQueryable_Data.Where(d => d.TransferStatusCode == "NR");

                    myResult = iQueryable_Data.Select(s => s.Id).ToList();
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetNotReadyARPaymentIds()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                List<string> myResult = new List<string>();

                if (SecurityUtility.CheckTableContactFeature("ARPayment", "READ", tenant))
                {
                    ARPaymentRepository repository = new ARPaymentRepository(tenant);

                    IQueryable<ARPayment> iQueryable_Data = repository.GetARPayments(tenant);
                    iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD");
                    iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARPayment>(new QueryOperations(), iQueryable_Data, tenant);
                    iQueryable_Data = iQueryable_Data.Where(d => d.TransferStatusCode == "NR");

                    myResult = iQueryable_Data.Select(s => s.Id).ToList();
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetNotReadyAPPaymentIds()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                List<string> myResult = new List<string>();

                if (SecurityUtility.CheckTableContactFeature("APPayment", "READ", tenant))
                {
                    APPaymentRepository repository = new APPaymentRepository(tenant);

                    IQueryable<APPayment> iQueryable_Data = repository.GetAPPayments(tenant);
                    iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD");
                    iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APPayment>(new QueryOperations(), iQueryable_Data, tenant);
                    iQueryable_Data = iQueryable_Data.Where(d => d.TransferStatusCode == "NR");

                    myResult = iQueryable_Data.Select(s => s.Id).ToList();
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetRecalculateTransfer(string allIdsString, string entityCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                bool myResult = true;

                if (!string.IsNullOrEmpty(allIdsString))
                {
                    List<string> ids = allIdsString.Split('.').ToList();

                    if (ids.Count > 0)
                    {
                        TransferHelper invoiceTransferHelper = new TransferHelper(tenant, entityCode);
                        invoiceTransferHelper.Recalculate(ids);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetBlockForTransfer(string allIdsString, string entityCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                bool myResult = true;

                if (!string.IsNullOrEmpty(allIdsString))
                {
                    List<string> ids = allIdsString.Split('.').ToList();
                    if (ids.Count > 0)
                    {
                        IInvoiceContext myContext = InvoiceContext.GetContext(tenant);

                        if (entityCode == "ARInvoice")
                        {
                            ARInvoiceRepository myRepository = new ARInvoiceRepository(myContext);
                            List<ARInvoice> allEntities = myRepository.GetInvoicesListFromIdList(ids, tenant);

                            foreach (ARInvoice item in allEntities)
                            {
                                item.TransferStatusCode = "BL";
                                myRepository.Update(item);
                            }

                            myRepository.SubmitChanges();
                        }

                        else if (entityCode == "APInvoice")
                        {
                            APInvoiceRepository myRepository = new APInvoiceRepository(myContext);
                            List<APInvoice> allEntities = myRepository.GetInvoicesListFromIdList(ids, tenant);

                            foreach (APInvoice item in allEntities)
                            {
                                item.TransferStatusCode = "BL";
                                myRepository.Update(item);
                            }

                            myRepository.SubmitChanges();
                        }

                        else if (entityCode == "ARPayment")
                        {
                            ARPaymentRepository myRepository = new ARPaymentRepository(myContext);
                            List<ARPayment> allEntities = myRepository.GetPaymentsListFromIdList(ids, tenant);

                            foreach (ARPayment item in allEntities)
                            {
                                item.TransferStatusCode = "BL";
                                myRepository.Update(item);
                            }

                            myRepository.SubmitChanges();
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSetAccountingSettingStartDate(string entityCode, string myStartDateString)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        string loggedUserEmail = authToken.Email;
                        int tenant = authToken.Tenant;

                        SecurityUtility.AuthenticationOnTenant(tenant);

                        AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
                        AccountingSetting entityPOCO = accountingSettingRepository.GetSingleAccountSetting(tenant);

                        if (myStartDateString == "null")
                        {
                            myStartDateString = null;
                        }

                        DateTime? myStartDate = DateHelper.GetDate(myStartDateString);

                        switch (entityCode)
                        {
                            case "ARInvoice":
                                {
                                    entityPOCO.ARInvoiceTransferStartDate = myStartDate;
                                    accountingSettingRepository.Update(entityPOCO);
                                    accountingSettingRepository.SubmitChanges();
                                    break;
                                }

                            case "APInvoice":
                                {
                                    entityPOCO.APInvoiceTransferStartDate = myStartDate;
                                    accountingSettingRepository.Update(entityPOCO);
                                    accountingSettingRepository.SubmitChanges();
                                    break;
                                }

                            case "ARPayment":
                                {
                                    entityPOCO.ARPaymentTransferStartDate = myStartDate;
                                    accountingSettingRepository.Update(entityPOCO);
                                    accountingSettingRepository.SubmitChanges();
                                    break;
                                }
                        }

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, true);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }
        public HttpResponseMessage GetOnStartDateEntitiesIds(string entityCode, string myStartDateString)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                List<string> myResult = new List<string>();

                if (myStartDateString == "null")
                {
                    myStartDateString = null;
                }

                DateTime? myStartDate = DateHelper.GetDate(myStartDateString);

                if (SecurityUtility.CheckTableContactFeature(entityCode, "READ", tenant))
                {
                    switch (entityCode)
                    {
                        case "ARInvoice":
                            {
                                ARInvoiceRepository entityRepository = new ARInvoiceRepository(tenant);

                                IQueryable<ARInvoice> iQueryable_Data = entityRepository.GetIQueryableInvoices(tenant);

                                iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD" && d.StatusCode != "LL" && d.IsConstituentInvoice == false);
                                iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARInvoice>(new QueryOperations(), iQueryable_Data, tenant);
                                iQueryable_Data = iQueryable_Data.Where(d => d.TransferStatusCode == "RD" || d.TransferStatusCode == "NR");

                                if (myStartDate != null)
                                {
                                    myStartDate = myStartDate.Value.Date;
                                    iQueryable_Data = iQueryable_Data.Where(d => DbFunctions.TruncateTime(d.InvoiceDate) < myStartDate);
                                }

                                myResult = iQueryable_Data.Select(s => s.Id).ToList();

                                break;
                            }

                        case "APInvoice":
                            {
                                APInvoiceRepository entityRepository = new APInvoiceRepository(tenant);

                                IQueryable<APInvoice> iQueryable_Data = entityRepository.GetIQueryableInvoices(tenant);

                                iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "WA" && d.StatusCode != "VD");
                                iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APInvoice>(new QueryOperations(), iQueryable_Data, tenant);
                                iQueryable_Data = iQueryable_Data.Where(d => d.TransferStatusCode == "RD" || d.TransferStatusCode == "NR");

                                if (myStartDate != null)
                                {
                                    myStartDate = myStartDate.Value.Date;
                                    iQueryable_Data = iQueryable_Data.Where(d => DbFunctions.TruncateTime(d.InvoiceDate) < myStartDate);
                                }

                                myResult = iQueryable_Data.Select(s => s.Id).ToList();

                                break;
                            }

                        case "ARPayment":
                            {
                                ARPaymentRepository entityRepository = new ARPaymentRepository(tenant);

                                IQueryable<ARPayment> iQueryable_Data = entityRepository.GetARPayments(tenant);

                                iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD");
                                iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARPayment>(new QueryOperations(), iQueryable_Data, tenant);
                                iQueryable_Data = iQueryable_Data.Where(d => d.TransferStatusCode == "RD" || d.TransferStatusCode == "NR");

                                if (myStartDate != null)
                                {
                                    myStartDate = myStartDate.Value.Date;
                                    iQueryable_Data = iQueryable_Data.Where(d => DbFunctions.TruncateTime(d.RegisterDate) < myStartDate);
                                }

                                myResult = iQueryable_Data.Select(s => s.Id).ToList();

                                break;
                            }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSingleChargeTypeAccountingList(string chargesTypeId, string vatTypeId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                CommonDataDomainService commonDomain = new CommonDataDomainService();
                ChargeTypeAccountingList result = commonDomain.GetSingleChargeTypeAccountingList(chargesTypeId, vatTypeId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTaxApprovalData(string startDateString, string endDateString, string email)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                Random rnd = new Random();
                string randomInt = String.Format("{0:yyyyMMddhhmm}", DateTime.Now) + rnd.Next(999);

                startDateString = this.CheckNullValue(startDateString);
                endDateString = this.CheckNullValue(endDateString);
                email = this.CheckNullValue(email);

                DateTime? startDate = DateHelper.GetDate(startDateString);
                DateTime? endDate = DateHelper.GetDate(endDateString);

                ContactQuery contactQuery = new ContactQuery(tenant);

                BrokeredMessage message = new BrokeredMessage();
                message.Properties["ReportId"] = randomInt;
                message.Properties["UserName"] = contactQuery.GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), tenant).EnglishName;
                message.Properties["Tenant"] = tenant;
                message.Properties["Email"] = email;
                message.Properties["Date1"] = startDate;
                message.Properties["Date2"] = endDate;

                string taxqueueName = WebFreightEntryPoint.GetQueueByEnviroment("taxdataqueue");
                QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(taxqueueName);
                client.Send(message);

                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSendARPaymentSATXML(string paymentId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        string loggedUserEmail = authToken.Email;
                        int tenant = authToken.Tenant;


                        SATInterfaceHelper satHelper = new SATInterfaceHelper();
                        satHelper.SendPaymentProfactoXML(paymentId, tenant);
                        SecurityUtility.AuthenticationOnTenant(tenant);


                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, true);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }
        public HttpResponseMessage GetARInvoiceSATStatus(string paymentId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        string loggedUserEmail = authToken.Email;
                        int tenant = authToken.Tenant;


                        ARPaymentQuery paymentQuery = new ARPaymentQuery(tenant);
                        ARPaymentPM entityPM = paymentQuery.GetSinglePM(paymentId, tenant);


                        IInvoiceContext invoiceContext = InvoiceContext.GetContext(entityPM.Tenant);
                        ARInvoiceRepository arInvoiceRepository = new ARInvoiceRepository(invoiceContext);
                        List<string> invoiceIds = entityPM.PaymentInvoices.Select(f => f.ARInvoiceId).ToList();

                        List<ARInvoice> paymentARInvoices = arInvoiceRepository.GetARInvoicesByIds(tenant, invoiceIds);
                        List<ARInvoiceSATStatus> arinvoiceSatStatus = new List<ARInvoiceSATStatus>();

                        //----------------------------

                        ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                        TenantRepository tenantRepository = new TenantRepository(commonContext);
                        AddressRepository addressReposirory = new AddressRepository(commonContext);
                        CardRepository cardRepository = new CardRepository(commonContext);
                        BranchRepository branchRepository = new BranchRepository(commonContext);
                        CurrencyRepository currencyRepository = new CurrencyRepository(commonContext);

                        List<Currency> currencies = currencyRepository.GetCurrencies(tenant).ToList();


                        //SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(entityPM.Tenant);
                        //SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(entityPM.Tenant);
                        //if (satSetting != null)
                        //{
                        //    if (satSetting.ActivationDate != null && entityPM.RegisterDate < satSetting.ActivationDate)
                        //    {
                        //        throw new ApplicationException("Payment Register Date date should be greater than SAT Activation Date");
                        //    }
                        //}

                        Address branchAddress = null;
                        Address mainAddress = null;
                        Tenant currentTenant = tenantRepository.GetSingleTenant(entityPM.Tenant);
                        if (!string.IsNullOrEmpty(entityPM.BranchId))
                        {
                            Branch branch = branchRepository.GetSingleBranch(entityPM.BranchId, entityPM.Tenant);
                            if (!string.IsNullOrEmpty(branch.AddressId))
                            {
                                branchAddress = addressReposirory.GetSingleAddress(branch.AddressId, branch.Tenant);
                            }
                        }
                        Address billToAddress = null;
                        if (!string.IsNullOrEmpty(entityPM.BillToAddressId))
                        {
                            billToAddress = addressReposirory.GetSingleAddress(entityPM.BillToAddressId, entityPM.Tenant);
                        }
                        else
                            throw new ApplicationException("Bill to Address is required ");

                        Card billToCard = cardRepository.GetSingleCard(entityPM.BillToId, entityPM.Tenant);

                        List<SATPaymentMethod> allPaymentMethods = (from d in invoiceContext.SATPaymentMethods select d).ToList();

                        if (string.IsNullOrEmpty(currentTenant.VatNumber))
                        {
                            throw new ApplicationException("Company Vat Number is required");
                        }

                        if (string.IsNullOrEmpty(billToCard.SATPaymentMethodCode))
                        {
                            throw new ApplicationException("Bill to Forma Pago is required");
                        }

                        if (string.IsNullOrEmpty(entityPM.SATPaymentMethodCode))
                        {
                            throw new ApplicationException("Forma Pago is required");
                        }

                        if (branchAddress != null)
                        {

                            if (string.IsNullOrEmpty(branchAddress.ZipCode))
                            {
                                throw new ApplicationException("Branch Address ZipCode is required ");
                            }

                            mainAddress = branchAddress;
                        }
                        else
                        {
                            if (currentTenant.Address != null)
                            {
                                if (string.IsNullOrEmpty(currentTenant.Address.ZipCode))
                                {
                                    throw new ApplicationException("Company Address ZipCode is required ");
                                }

                                mainAddress = currentTenant.Address;
                            }
                            else
                                throw new ApplicationException("Company Address is required ");
                        }

                        //------------------------


                        paymentARInvoices.ForEach(invoice =>
                        {
                            ARInvoiceSATStatus arStatus = new ARInvoiceSATStatus()
                            {
                                ARInvoiceId = invoice.Id,
                                ARInvoiceNumber = invoice.InvoiceNumber,
                                BillTo = invoice.BillTo.EnglishName,
                            };
                            arStatus.IsSATValid = false;
                            switch (invoice.SATTransferStatusCode)
                            {
                                case "TD":
                                    arStatus.SATStatusCode = "SENT";
                                    arStatus.SATStatusName = "Transferred";
                                    arStatus.IsSATValid = true;
                                    break;
                                case "NT":
                                    arStatus.SATStatusCode = "NSNT";
                                    arStatus.SATStatusName = "Not Transferred";
                                    break;
                                case "TE":
                                    arStatus.SATStatusCode = "SNTE";
                                    arStatus.SATStatusName = "Transferred with Errors";
                                    break;
                                case "TG":
                                    arStatus.SATStatusCode = "SNDG";
                                    arStatus.SATStatusName = "Transferring";
                                    break;
                                case "CS":
                                    arStatus.SATStatusCode = "CSRS";
                                    arStatus.SATStatusName = "Cancellation Request Sent";
                                    break;

                            }
                            //if (string.IsNullOrEmpty(invoice.SATXML))
                            //{
                            //    arStatus.IsSATValid = false;
                            //    if (string.IsNullOrEmpty(invoice.TransmissionError))
                            //    {
                            //        arStatus.SATStatusCode = "NSNT";
                            //        arStatus.SATStatusName = "Not Transferred";

                            //    }
                            //    else
                            //    {
                            //        arStatus.SATStatusCode = "SNTE";
                            //        arStatus.SATStatusName = "Transferred with Errors";
                            //        arStatus.SATError = invoice.TransmissionError;
                            //    }
                            //}
                            //else
                            //{
                            //    arStatus.SATStatusCode = "SENT";
                            //    arStatus.SATStatusName = "Transferred";
                            //    arStatus.IsSATValid = true;
                            //}

                            arinvoiceSatStatus.Add(arStatus);
                        });

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, arinvoiceSatStatus);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }
        public HttpResponseMessage GetShipmentLevelCode(string myShipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                string myResult = shipmentRepository.GetShipmentLevelCode(myShipmentId);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetShipmentIsAccountingClosed(string myShipmentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                bool myResult = shipmentRepository.IsShipmentAccountingClosed(myShipmentId);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetStatusOfARPaymentCheques(string paymentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                ARPaymentChequeQueryService entityQuery = new ARPaymentChequeQueryService(tenant);
                ARPaymentChequePM arpaymentCheque = entityQuery.GetSingleByPaymentId(paymentId, tenant);
                string status = "";
                if (arpaymentCheque != null)
                {
                    ARPaymentChequeStatusRepository aRPaymentChequeStatusRep = new ARPaymentChequeStatusRepository(tenant);
                    ARPaymentChequeStatus aRPaymentChequeStatus = aRPaymentChequeStatusRep.GetSingle(arpaymentCheque.StatusCode);
                    status = aRPaymentChequeStatus != null ? aRPaymentChequeStatus.EnglishName : "";
                    TenantRepository tenantRepository = new TenantRepository(tenant);
                    Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
                    if (tenantPOCO != null && tenantPOCO.AccountingActivated)
                    {
                        status = aRPaymentChequeStatus != null ? aRPaymentChequeStatus.LocalName : "";
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, status);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private string CheckNullValue(string value)
        {
            string newValue = value;
            if (newValue == "null" || newValue == "undefined")
            {
                newValue = null;
            }
            return newValue;
        }



		public HttpResponseMessage GetARPaymentSATCancellationStatus(string paymentId)
		{
			if (ModelState.IsValid)
			{
				try
				{
					using (TransactionScope scope = TransactionFactory.GetTransaction())
					{
						string token = HttpContext.Current.Request.Headers["Token"];
						AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
						string loggedUserEmail = authToken.Email;
						int tenant = authToken.Tenant;


						ARPaymentQuery paymentQuery = new ARPaymentQuery(tenant);
						ARPaymentRepository aRPaymentRepository = new ARPaymentRepository(tenant);
						ARInvoiceRepository arInvoiceRepository = new ARInvoiceRepository(tenant);

						ARPaymentPM entityPM = paymentQuery.GetSinglePM(paymentId, tenant);
						ARPayment payment = aRPaymentRepository.GetSingleARPayment(paymentId, tenant);


						Profact.TimbraCFDI.ResultadoConsultaEstatusSAT resultadoConsultaEstatusSAT = SATInterfaceHelper.GetSATStatus(tenant, payment.SATXML);
						if (resultadoConsultaEstatusSAT.EstadoComprobante == "Cancelado")
						{
							payment.SATXML = null;
							payment.SATTransferStatusCode = "TD";
							aRPaymentRepository.Update(payment);
							aRPaymentRepository.SubmitChanges();

							Profact.TimbraCFDI33.Comprobante comprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(payment.SATXML);
							SATInterfaceHelper sATInterfaceHelper = new SATInterfaceHelper();
							sATInterfaceHelper.UpdatePaymentInvoicesSATStatus(payment, comprobante, arInvoiceRepository, aRPaymentRepository);
						}
						/*
						 * Catalog EstadoCancelacion
						   EnProceso  
						   SinRespuesta
						   CanceladoSinAceptacion
						   CanceladoConAceptacion
						   PlazoVencido

						 * */
						scope.Complete();
						return Request.CreateResponse(HttpStatusCode.OK, "");
					}
				}

				catch (Exception ex)
				{
					return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
				}
			}

			else
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
			}
		}


		public HttpResponseMessage GetARInvoiceSATCancellationStatus(string invoiceId)
		{
			if (ModelState.IsValid)
			{
				try
				{
					using (TransactionScope scope = TransactionFactory.GetTransaction())
					{
						string token = HttpContext.Current.Request.Headers["Token"];
						AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
						string loggedUserEmail = authToken.Email;
						int tenant = authToken.Tenant;


						ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(tenant);
						ARInvoiceRepository arInvoiceRepository = new ARInvoiceRepository(tenant);
						ARInvoicePM entityPM = invoiceQuery.GetSinglePM(invoiceId, tenant);
						ARInvoice entity = arInvoiceRepository.GetSingleInvoice(invoiceId);

						Profact.TimbraCFDI.ResultadoConsultaEstatusSAT resultadoConsultaEstatusSAT = SATInterfaceHelper.GetSATStatus(tenant, entity.SATXML);
						if (resultadoConsultaEstatusSAT.EstadoComprobante == "Cancelado")
						{
							entity.SATTransferStatusCode = "TD";
							arInvoiceRepository.Update(entity);
							arInvoiceRepository.SubmitChanges();
						}

						scope.Complete();
						return Request.CreateResponse(HttpStatusCode.OK, "");
					}
				}

				catch (Exception ex)
				{
					return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
				}
			}

			else
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
			}
		}


		public HttpResponseMessage getConnectedARPayments(string invoiceId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        string loggedUserEmail = authToken.Email;
                        int tenant = authToken.Tenant;
                        ARInvoicePaymentRepository aRInvoicePaymentRepository = new ARInvoicePaymentRepository(tenant);

                        List<ARPayment> aRPayments = aRInvoicePaymentRepository.GetARInvoicePaymentTransferedByInvoiceId(invoiceId, tenant).ToList();
                        bool ExistPayments = aRPayments.Count>0?true:false;
                       
                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, ExistPayments);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }



        public HttpResponseMessage getConnectedAPPayments(string invoiceId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        string loggedUserEmail = authToken.Email;
                        int tenant = authToken.Tenant;
                        APInvoicePaymentRepository aRInvoicePaymentRepository = new APInvoicePaymentRepository(tenant);

                        List<APPayment> aRPayments = aRInvoicePaymentRepository.GetAPInvoicePaymentTransferedByInvoiceId(invoiceId, tenant).ToList();
                        bool ExistPayments = aRPayments.Count > 0 ? true : false;

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, ExistPayments);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }


    }
    public class APInvoiceNumberDuplicationCheckArgs
    {
        public string VendorId { get; set; }
        public string EntityId { get; set; }
        public string InvoiceNumber { get; set; }
    }
}
