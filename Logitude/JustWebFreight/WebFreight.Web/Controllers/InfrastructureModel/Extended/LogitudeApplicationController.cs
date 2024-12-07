using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class LogitudeApplicationController : ApiController
    {
        public HttpResponseMessage GetCurrenctUserValidity(string clientEmail,string documentToken ,  int tenant)
        {

            try
            {
                UserValidityResponse response = new UserValidityResponse() { IsValid = true };

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                //SecurityUtility.AuthenticationOnTenant(tenant);

                IWebFreightContext ObjectContext = WebFreightContext.GetContext(tenant);
                if (HttpContext.Current != null)
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                    {
                        response.IsValid = false;
                        response.ErrorCode = "AUTH";
                        response.Error = "You are not authenticated";
                    }
                }

                string authEmail = SecurityUtility.GetAuthenticatedUser();
                if (!authEmail.Trim().ToLower().Equals(clientEmail.Trim().ToLower()))
                {
                    response.IsValid = false;
                    response.ErrorCode = "SUSR";
                    response.Error = "Sorry! this user is not the last signed user!";
                    //throw new Exception("Sorry! this user is not the last signed user!");
                }

                bool isBlocking;
                using (
                    TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext globalcontext = GlobalContext.GetContext();
                    //string connection = globalcontext.GetCurrentConnection();
                    //if (connection.Contains("Main"))
                    //{ }

                    //isBlocking = (from a in globalcontext.GlobalDBs select a).FirstOrDefault().IsBlocking;


                    isBlocking = (from a in globalcontext.GlobalDBs
                                       where a.IsBlocking == true
                                       select a.IsBlocking).Count() > 0;



                    GlobalContactRepository repository = new GlobalContactRepository(globalcontext);
                    GlobalContact contact = repository.GetGlobalContactByEmailAndTenant(authEmail, tenant);
                    if (contact != null && contact.InActive)
                    {
                         
                        response.IsValid = false;
                        response.ErrorCode = "AUTH";
                        response.Error = "You are not authenticated";
                    }

                    scope.Complete();
                }
                bool isIpAuthenticated = true;
                string ipstring = LogitudeSettings.CustomerCareIP;//System.Configuration.ConfigurationManager.AppSettings.Get("CustomerCareIP");
                string[] authenticatedIPs = ipstring.Split(',');
                string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                if (string.IsNullOrEmpty(currentIP))
                {
                    currentIP = HttpContext.Current.Request.UserHostAddress;
                }
               
                if (!authenticatedIPs.Contains(currentIP))
                {
                    isIpAuthenticated = false;
                }

                if (isBlocking)
                {
                    if (!isIpAuthenticated)
                    {
                        response.IsValid = false;
                        response.ErrorCode = "SUPG";
                        response.Error = "System Upgrading";
                        //throw new Exception("System Upgrading");
                    }
                }

                if (authToken != null)
                {
                    AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(authToken.Tenant);
                    if (!string.IsNullOrEmpty(documentToken))
                    {
                        AuthenticationToken documentAuthenticationToken = AuthenticationTokenRepository.GetSingleTokenFromCache(documentToken);
                        if (documentAuthenticationToken != null)
                        {
                            DateTime nowDate = DateTime.Now;
                            DateTime endDate = (DateTime)documentAuthenticationToken.ExpirationDate;
                            if (endDate.AddMinutes(-5) > nowDate)
                            {
                                response.DocumentDownloadToken = documentAuthenticationToken.Token;
                            }
                        }
                    }
                
                    if (string.IsNullOrEmpty(response.DocumentDownloadToken))
                    {
                        AuthenticationToken authentication = new AuthenticationToken() { CreateDate = DateTime.Now,ExpirationDate = DateTime.Now.AddMinutes(15), Email = authToken.Email, Password = authToken.Password, Token = AuthenticationUtil.GenerateToken(), Tenant = authToken.Tenant
                            , ClientType = "DocumentDownload" };

                        authenticationTokenRepository.Add(authentication);
                        authenticationTokenRepository.SubmitChanges();
                        response.DocumentDownloadToken = authentication.Token;
                    }

                }



                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetCheckIsupgradingSystem()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                bool isBlocking = false;
                //string entityName = "SystemIsBlocked";

                //if (CacheManager.CacheWrapper != null)
                //{
                //    if (CacheManager.CacheWrapper.Get(entityName) == null)
                //    {
                //        isBlocking = GetIsBlockingFromDB();

                //        if (CacheManager.CacheWrapper.Get(entityName) == null)
                //        {
                //            CacheManager.CacheWrapper.Insert(entityName, isBlocking, null, DateTime.UtcNow.AddSeconds(30), TimeSpan.Zero);
                //        }

                //    }
                //    else
                //    {
                //        var cachedEntity = CacheManager.CacheWrapper.Get(entityName);
                //        if (cachedEntity != null)
                //            isBlocking = (bool)cachedEntity;
                //        else
                //        {
                //            isBlocking = GetIsBlockingFromDB();
                //            if (CacheManager.CacheWrapper.Get(entityName) == null)
                //            {
                //                CacheManager.CacheWrapper.Insert(entityName, isBlocking, null, DateTime.UtcNow.AddSeconds(30), TimeSpan.Zero);
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    isBlocking = GetIsBlockingFromDB();
                //}

                isBlocking = GetIsBlockingFromDB();

                return Request.CreateResponse(HttpStatusCode.OK, isBlocking);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private bool GetIsBlockingFromDB()
        {
            IGlobalContext globalcontext = GlobalContext.GetContext();
            // bool isBlocking = (from a in globalcontext.GlobalDBs select a).FirstOrDefault().IsBlocking;

            bool isBlocking = (from a in globalcontext.GlobalDBs 
                               where a.IsBlocking == true
                               select a.IsBlocking).Count()>0;


            bool isIpAuthenticated = true;

            string ipstring = LogitudeSettings.CustomerCareIP;
            string[] authenticatedIPs = ipstring.Split(',');

            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
            if (string.IsNullOrEmpty(currentIP))
            {
                currentIP = HttpContext.Current.Request.UserHostAddress;
            }
            if (!authenticatedIPs.Contains(currentIP))
            {
                isIpAuthenticated = false;
            }

            if (isIpAuthenticated)
            {
                isBlocking = false;
            }

            return isBlocking;
        }
    }

    public class UserValidityResponse
    {
        public bool IsValid { get; set; }
        public string Error { get; set; }
        public string ErrorCode { get; set; }
        public string DocumentDownloadToken { get; set; }
    }
}