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
using System.Transactions;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.EntityQueryServices;

namespace WebFreight.Web.Controllers.AccountingModel
{
    public class ChartOfAccountsTypesController : ApiController
    {

        public HttpResponseMessage UpdateChartOfAccountsTypesOrder(int[] orders)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string logKey = PerformanceLogger.LogCurrentTime();
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.AuthenticationOnEntityTenant("ChartOfAccountsTypes", 0, authToken.Tenant);
                        ChartOfAccountsTypeQueryService ChartOfAccountsTypesQuery = new ChartOfAccountsTypeQueryService(0);
                        var chartOfAccountsTypes = ChartOfAccountsTypesQuery.GetAllChartOfAccounts().OrderBy(x=>x.Order).ToList();
                        IAccountingContext MyContext = AccountingContext.GetContext(0);
                        ChartOfAccountsTypeUpdateService service = new ChartOfAccountsTypeUpdateService(MyContext, new Dictionary<string, IContext>(), 0);
                        

                        for (int i = 0; i < chartOfAccountsTypes.Count; i++) {
                            chartOfAccountsTypes[i].Order = orders[i];
                            chartOfAccountsTypes[i].ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                            service.Update(chartOfAccountsTypes[i], true);
                        }
                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                        return Request.CreateResponse(HttpStatusCode.OK);
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
}