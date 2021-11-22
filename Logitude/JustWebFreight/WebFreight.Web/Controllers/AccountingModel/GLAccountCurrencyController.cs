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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.BL.EntityQueryServices;
using System.Web.Http.ModelBinding;
using Logitude.Accounting.Data.Repositories;

namespace WebFreight.Web.Controllers.AccountingModel
{
    public class GLAccountCurrencyController: ApiController
    {


        public HttpResponseMessage Post(GLAccountCurrencyPM entityPM)
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
                      //  SecurityUtility.CheckContactFeature("GLAccountCurrency", "NEW", authToken.Tenant);

                        IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
                        GLAccountCurrencyUpdateService service = new GLAccountCurrencyUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                       
                        service.ValidateSplitGLAccount(entityPM);
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
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        public HttpResponseMessage Put(GLAccountCurrencyPM entityPM)
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
                        GLAccountCurrencyQueryService GLAccountCurrencyQuery = new GLAccountCurrencyQueryService(entityPM.Tenant);
                        GLAccountCurrencyPM accountCurrency = GLAccountCurrencyQuery.GetEntityByCurrencyAndGLAccountId(entityPM.MainGLAccountId, entityPM.CurrencyId, entityPM.Tenant);
                        entityPM.Id = accountCurrency.Id;
                        IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
                        GLAccountCurrencyUpdateService service = new GLAccountCurrencyUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
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
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }


    }
}