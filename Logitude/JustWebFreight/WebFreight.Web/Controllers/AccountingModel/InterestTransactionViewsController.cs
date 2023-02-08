using Simplog.Data.InfrastructureModel.EntityPOCOs;
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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.Helpers;
using WebFreight.Web.AccountingModel.Reports.BankDeposit;
using WebFreight.Web.AccountingModel.DomainServices;
using Logitude.Accounting.BL.InterestService;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{ 
   
    public partial class InterestTransactionViewsController : ApiController
    {
        public HttpResponseMessage GetAllInterestTransactionByDate(string ReportId , DateTime InterestCalculationDate)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("InterestTransaction", "READ", authToken.Tenant);
                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                InterestReportService interestTransactionQuery = new InterestReportService();
                InterestTransactionsWithTotal myResult  = interestTransactionQuery.GetAllInterestTransactionByDate(ReportId, InterestCalculationDate, tenant, MyContext);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutConfirmCreateInvoice(InterestReportPM interestReportPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("InterestReport", interestReportPM.Tenant, authToken.Tenant);
                SecurityUtility.CheckContactFeature("InterestTransaction", "READ", authToken.Tenant);
                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);


                InterestReportService interestTransactionQuery = new InterestReportService();
                interestReportPM = interestTransactionQuery.PutConfirmCreateInvoice(interestReportPM, tenant, MyContext);


                return Request.CreateResponse(HttpStatusCode.OK, interestReportPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutInterestTransactionNotes(InterestTransactionPM interestTransactionPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("InterestTransaction", interestTransactionPM.Tenant, authToken.Tenant);
                SecurityUtility.CheckContactFeature("InterestTransaction", "UPDATE", authToken.Tenant);
                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                InterestTransactionQueryService interestTransactionQuery = new InterestTransactionQueryService(MyContext);
                var interestTransaction = interestTransactionQuery.MapInterestTransactionNotes(interestTransactionPM);
                return Request.CreateResponse(HttpStatusCode.OK, interestTransaction);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCheckRecentReports(DateTime interestDate, string customerId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("InterestTransaction", "READ", authToken.Tenant);
                bool recentCustomerReportExist = CheckRecentCustomerReports(tenant, interestDate, customerId);

                return Request.CreateResponse(HttpStatusCode.OK, recentCustomerReportExist);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private bool CheckRecentCustomerReports(int tenant, DateTime date, string customerId)
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            return interestReportQueryService.CheckRecentCustomerReports(tenant, date, customerId);

        }
    }

}
	 