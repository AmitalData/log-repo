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


namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{
    public class ReconciliationCancelController
    {
#if false
        

        public HttpResponseMessage PostInsertReconciliation(ReconciliationPM entityPm)
        {
            try
            {


                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Reconciliation", "NEW", authToken.Tenant);
                var accountingContext = AccountingContext.GetContext(authToken.Tenant);
                var qs = new LedgerTransactionListQueryService(accountingContext);

                ReconciliationUpdateService service = new ReconciliationUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPm.Tenant);
                if (entityPm.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                {
                    throw new Exception("Meanwhile Only Insert Enable ");
                }
                entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                foreach (var item in entityPm.ReconciliationLines)
                {
                    item.ChangeSetOp = ChangeSetOperation.Insert;
                }
                entityPm.Tenant = authToken.Tenant;
                //entityPm.AccountId = gLAccountId;
                service.Update(entityPm, true);
                return Request.CreateResponse(HttpStatusCode.OK, entityPm);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        #endif
    }
}