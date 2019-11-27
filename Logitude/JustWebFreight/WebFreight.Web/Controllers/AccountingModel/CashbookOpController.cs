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
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Simplog.Data.Helpers;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.CloseTables;

namespace WebFreight.Web.Controllers.AccountingModel 
{
    public partial class CashBookOpController : ApiController
    {
        public CashBookOpController()
        {

        }
        
        public HttpResponseMessage GetSingleWithoutLines(string id)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Cashbook", "READ", authToken.Tenant);

                CashBookPM cashBookPM = GetCashbookWithoutLines(id, authToken.Tenant);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, cashBookPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetCashbookChequesCounter(string cashbookId)
        {
            try
            {
                AuthenticationToken authToken = AuthinticateTenant();

                CashbookChequesCounter chequeCounter = GetChequesCounterForCashbook(cashbookId, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, chequeCounter);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetCashbookUndepositedChequesCount(string cashbookId)
        {
            try
            {
                AuthenticationToken authToken = AuthinticateTenant();

                int count = GetUndepositedChequesCountForCashbook(cashbookId, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, count);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cashbookId"></param>
        /// <param name="chequeFilterType">
        /// should be one of these values: All, PostdatedCheque, CashCheque
        /// </param>
        /// <returns></returns>
        public HttpResponseMessage GetCashbookTotalAmount(string cashbookId, string chequeFilterType)
        {
            try
            {
                AuthenticationToken authToken = AuthinticateTenant();

                decimal totalAmount = 0;
                CashBookPM cashbook = GetCashbookWithoutLines(cashbookId, authToken.Tenant);
                if (cashbook.CashBookTypeCode == CashBookTypeValues.Cash)
                {
                    totalAmount = cashbook.TotalAmount ?? 0;
                }
                else
                {
                    totalAmount = GetChequesTotalAmount(cashbookId, chequeFilterType, authToken.Tenant);
                }

                return Request.CreateResponse(HttpStatusCode.OK, totalAmount);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private static decimal GetChequesTotalAmount(string cashbookId, string chequeFilterType, int tenant)
        {
            decimal totalAmount;
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            CashBookQueryService cashBookQuery = new CashBookQueryService(MyContext);
            totalAmount = cashBookQuery.GetCashbookChequesTotal(cashbookId, chequeFilterType, tenant);
            return totalAmount;
        }


        // --------------------------- PRIVATE MEMBERS 

        private CashbookChequesCounter GetChequesCounterForCashbook(string id, int tenant)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            CashBookQueryService cashBookQuery = new CashBookQueryService(MyContext);
            CashbookChequesCounter chequeCounter = cashBookQuery.GetCashbookChequesCounter(id, tenant);
            return chequeCounter;
        }
        private int GetUndepositedChequesCountForCashbook(string cashbookId, int tenant)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            CashBookQueryService cashBookQuery = new CashBookQueryService(MyContext);
            return cashBookQuery.GetUndepositedChequesCount(cashbookId, tenant);
        }

        private CashBookPM GetCashbookWithoutLines(string id, int tenant)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            CashBookQueryService query = new CashBookQueryService(MyContext);
            query.InitializeSettings();
            CashBookPM cashBookPM = query.GetSingle(id, false, false);
            return cashBookPM;
        }

        private AuthenticationToken AuthinticateTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("Cashbook", "READ", authToken.Tenant);
            return authToken;
        }
    }

}