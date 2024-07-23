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
//using Logitude.Accounting.BL.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.BL.EntityQueryServices;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Accounting.Data.Repositories;
using Simplog.Data.CommonDataModel;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{


    public partial class GLAccountsController : ApiController
    {


        public HttpResponseMessage GetSingleByDispalyNumberAndTenant(string displayNumber, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("GLAccount", "READ", authToken.Tenant);

                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                GLAccountQueryService gLAccountQuery = new GLAccountQueryService(MyContext);
                GLAccountPM gLAccountPM = gLAccountQuery.GetByDisplayNumber(displayNumber, tenant).FirstOrDefault();
                if (gLAccountPM == null) gLAccountPM = new GLAccountPM();

                return Request.CreateResponse(HttpStatusCode.OK, gLAccountPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage CheckIfSplitted(string accountId)
        {
            try
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("Journal", "READ", authToken.Tenant);
                    IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                    GLAccountCurrencyListQueryService GLAccountCurrencyQuery = new GLAccountCurrencyListQueryService(MyContext);
                    GLAccountCurrencyList glAccountCurrencyList = GLAccountCurrencyQuery.GetByAccountNumber(accountId, authToken.Tenant);

                    return Request.CreateResponse(HttpStatusCode.OK, glAccountCurrencyList);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        //public HttpResponseMessage PostGLAccount(GLAccountPM entityPM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            using (TransactionScope scope = TransactionFactory.GetTransaction())
        //            {
        //                string token = HttpContext.Current.Request.Headers["Token"];
        //                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
        //                SecurityUtility.CheckContactFeature("GLAccount", "NEW", authToken.Tenant);

        //                IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
        //                System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(entityPM);
        //                ValidationResult validationResult = GLAccountValidator.IsGLAccountValid(entityPM, validationContext);
        //                if (validationResult == null)
        //                {
        //                    GLAccountUpdateService service = new GLAccountUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
        //                    entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;


        //                    service.Update(entityPM, true);

        //                    ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
        //                    ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("GLAccount", 0, true);
        //                    string email = HttpContext.Current.User.Identity.Name;
        //                    ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
        //                    Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
        //                    if (loggedContact != null)
        //                    {
        //                        //ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
        //                    }

        //                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
        //                }
        //                else
        //                {
        //                    string errorText = validationResult.ErrorMessage + ", Number=" + entityPM.DisplayNumber + ", English Name=" + entityPM.EnglishName;
        //                    throw new Exception(errorText);
        //                }
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //        }
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
        //    }
        //}


        //public HttpResponseMessage Put(GLAccountPM entityPM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            using (TransactionScope scope = TransactionFactory.GetTransaction())
        //            {
        //                string token = HttpContext.Current.Request.Headers["Token"];
        //                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
        //                SecurityUtility.CheckContactFeature("GLAccount", "UPDATE", authToken.Tenant);

        //                IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
        //                GLAccountUpdateService service = new GLAccountUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
        //                entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
        //                service.Update(entityPM, true);

        //                //ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
        //                //ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("GLAccount", 0, true);
        //                //string email = HttpContext.Current.User.Identity.Name;
        //                //ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
        //                //Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
        //                //if (loggedContact != null)
        //                //{
        //                //ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
        //                //}
        //                scope.Complete();
        //                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //        }
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
        //    }
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}


        public HttpResponseMessage GetConnectCardToGLAccount(string accountId, string cardId, bool skipConnectedCardsValidation)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("GLAccount", "READ", authToken.Tenant);

                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                GLAccountQueryService query = new GLAccountQueryService(MyContext);
                CardGLAccountConnectionArgs args = new CardGLAccountConnectionArgs()
                {
                    AccountId = accountId,
                    CardId = cardId,
                    Tenant = authToken.Tenant,
                    SkipConnectedCardsValidation = skipConnectedCardsValidation
                };
                query.ConnectCardToGLAccount(args);

                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetConnectedCardsForGLAccount(string accountId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("GLAccount", "READ", authToken.Tenant);

                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                GLAccountQueryService query = new GLAccountQueryService(MyContext);
                List<CardList> connectedCards = query.GetConnectedCards(accountId, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, connectedCards);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetGLAReconcilationCount(string accountId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("GLAccount", "READ", authToken.Tenant);

                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                LedgerTransactionRepository LedgerTransactionreop = new LedgerTransactionRepository(authToken.Tenant);
                var reconcilationCount = LedgerTransactionreop.getRecoCount(accountId, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, reconcilationCount);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }






        //[HttpPost]
        //public HttpResponseMessage UpdateFromCsv(ImageParameter fileUploadParamerter)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

        //        ICommonDataContext objectContext = CommonDataContext.GetContext(authToken.Tenant);
        //        IAccountingContext accountingContext = AccountingContext.GetContext(authToken.Tenant);

        //        GLAccountUpdateService gLAccountUpdateService = new GLAccountUpdateService(accountingContext, new Dictionary<string, IContext>(), authToken.Tenant);
        //        byte[] data = Convert.FromBase64String(fileUploadParamerter.Base64String);
        //        var res = gLAccountUpdateService.UpdateFromCsv(data, authToken.Tenant);

        //        return Request.CreateResponse(HttpStatusCode.OK, res);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }
        //}

        public HttpResponseMessage GetSingleByInternalNumberAndTenant(string internalNumber, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("GLAccount", "READ", authToken.Tenant);

                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                GLAccountQueryService gLAccountQuery = new GLAccountQueryService(MyContext);
                GLAccountPM gLAccountPM = gLAccountQuery.GetByInternalNumber(internalNumber, tenant)/*.FirstOrDefault()*/;
                if (gLAccountPM == null) gLAccountPM = new GLAccountPM();

                return Request.CreateResponse(HttpStatusCode.OK, gLAccountPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}
