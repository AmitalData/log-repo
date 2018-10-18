using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.DataContracts;
using System.Net.Http;
using Newtonsoft.Json;
using Simplog.Global.Data.GlobalModel.Repositories;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LoginWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select LoginWcfService.svc or LoginWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class LoginWcfService : ILoginWcfService
    {

        public Response LoginByCredential(string email, APICredentialsParameters apiCredentialsParam)
        {

            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(apiCredentialsParam.PrimaryKey))
                {
                    response.HasError = true;
                    response.ErrorMessage = "You should fill the primary key!";
                    return response;
                }

                AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(apiCredentialsParam.Tenant);
                APICredentialsHelper helper = new APICredentialsHelper();
                ApiCredential data = helper.CheckUserState(apiCredentialsParam);
             
                if (!data.HasError)
                {
                    string PrimaryhashedKey = PasswordGenerator.GetOldHashedPassword(apiCredentialsParam.PrimaryKey);

                    #region apiCredintialsId
                    ApiCredintialsRepository apiCredintialsRepository = new ApiCredintialsRepository();
                    string apiCredintialsId = apiCredintialsRepository.GetApiCredintialsIdByHashedPrimaryAccessKey(PrimaryhashedKey);
                    #endregion

                    if (string.IsNullOrEmpty(email)) email = "system@tenant" + data.Tenant + ".com";
              
                    AuthenticationToken Primaryauthentication = new AuthenticationToken() { APICredentialID = apiCredintialsId ,  CreateDate = DateTime.Now, Email = email, Password = PrimaryhashedKey, Token = data.Token, Tenant = apiCredentialsParam.Tenant, APIToken = true };
                    authenticationTokenRepository.Add(Primaryauthentication);
                    authenticationTokenRepository.SubmitChanges();
                    response.Result = data.Token;
                }
                else
                {
                    response.HasError = true;
                    response.ErrorMessage = "bad user email or Api Credentials";
                }

            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
            }
            return response;
        }

        public Response Login(string email, string password)
        {
            Response response = new Response();
            try
            {
                string hashedPassword = PasswordGenerator.GetHashedPassword(email, password);
                IGlobalContext globalObjectContext = GlobalContext.GetContext();
                ContactPassword contactPassword = AuthenticationUtil.VerifyContactPassword(email, password, globalObjectContext);
                if (contactPassword != null)
                {
                    hashedPassword = contactPassword.Password;
                    if (contactPassword.IsLocked) contactPassword = null;

                }


                GlobalContact contact = globalObjectContext.GlobalContacts.Where(m => m.Email == email && m.InActive == false).FirstOrDefault();
                if (contactPassword != null && contact != null && contact.InActive == false)
                {
                    string token = AuthenticationUtil.GenerateToken();//Guid.NewGuid().ToString();
                    AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(0);
                    AuthenticationToken authentication = new AuthenticationToken() { CreateDate = DateTime.Now, Email = email, Password = hashedPassword, Token = token };
                    authenticationTokenRepository.Add(authentication);
                    authenticationTokenRepository.SubmitChanges();

                    response.Result = token;

                }
                else
                {
                    response.HasError = true;
                    response.ErrorMessage = "bad user email or password";
                }
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
            }
            return response;
        }

        public List<Helpers.TenantInfo> GetUserTenants(string email, ref Response response)
        {

            List<Helpers.TenantInfo> tenantsList = new List<Helpers.TenantInfo>(); 
            response = new Response();
            bool authorized = false;
            try
            {

                if (HttpContext.Current != null)
                {
                    if (email == HttpContext.Current.User.Identity.Name)
                    {
                        authorized = true;
                        IGlobalContext globalObjectContext = GlobalContext.GetContext();
                        List<GlobalTenant> globalTenants = globalObjectContext.GlobalTenants.ToList();
                        List<GlobalContact> contacts = globalObjectContext.GlobalContacts.Where(m => m.Email == email && m.GlobalTenant.IsActive == true && m.InActive == false && m.IsUser == true).ToList();
                        foreach (GlobalContact contact in contacts)
                        {
                            GlobalTenant globalTenant = globalTenants.Where(t => t.Id == contact.GlobalTenantId).FirstOrDefault();
                            tenantsList.Add(new Helpers.TenantInfo() { Tenant = globalTenant.Id, Name = globalTenant.CompanyName });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }

            if (!authorized)
            {
                response.HasError = true;
                response.IsAuthenticationError = true;
                response.ErrorMessage = "Sorry! this user is not authorized!";
            }

            return tenantsList;


        }
    }
}
