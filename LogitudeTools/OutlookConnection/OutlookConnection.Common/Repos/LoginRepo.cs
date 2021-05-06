using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlookConnection.Common.Contracts;
using OutlookConnection.Common.LoginWcfServiceReference;
using System.ServiceModel;
using OutlookConnection.Common.Utils;
using System.Net;

namespace OutlookConnection.Common.Repos
{
    public class LoginRepo : ILoginRepo, IDisposable
    {

        string _LoginWcfService;
        string _token;



        public LoginRepo()
        {
            _LoginWcfService = SettingServiceLocator.Instance.CRMSettings.ServerURL.Replace("ActivityWcfService.svc", "LoginWcfService.svc");
        }



        public LoginRepo(string serverURL)
        {
            _LoginWcfService = serverURL.Replace("ActivityWcfService.svc", "LoginWcfService.svc");
        }


        public LoginRepo(string serverURL, string token)
        {
            _LoginWcfService = serverURL.Replace("ActivityWcfService.svc", "LoginWcfService.svc");
            _token = token;
        }



        LoginWcfServiceClient GetserviceClient()
        {
            var remoteAddress = new System.ServiceModel.EndpointAddress(_LoginWcfService);
            var my = new LoginWcfServiceClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);
            return my;
        }





        public TenantInfo[] GetUserTenants(string email, ref OutlookConnection.Common.LoginWcfServiceReference.Response response)
        {
            try
            {
                using (var myLoginWcfServiceClient = GetserviceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)myLoginWcfServiceClient.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", _token);
                        TenantInfo[] TenantList = myLoginWcfServiceClient.GetUserTenants(email, ref response);


                        if (response == null)
                        {
                            LogFileUtil.Log("LoginRepo GetUserTenants() Failed - return null", LogFileUtil.LogLevel.Debug);
                            return new TenantInfo[0];
                        }
                        else if (response.HasError)
                        {
                            LogFileUtil.Log("LoginRepo GetUserTenants() Failed - return Error: " + response.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return new TenantInfo[0];
                        }
                        else
                        {
                            LogFileUtil.Log("LoginRepo GetUserTenants() Finished successfully", LogFileUtil.LogLevel.Debug);
                            return TenantList;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("LoginRepo GetUserTenants() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }




        public Response Login(string email, string password)
        {
            try
            {
                using (var myLoginWcfServiceClient = GetserviceClient())
                {
                    Response myResponse = myLoginWcfServiceClient.Login(email, password);
                    return myResponse;
                }
            }
            catch (TimeoutException timeProblem)
            {
                var m = @"The service operation timed out. 
                        " + _LoginWcfService + @"
                        " + timeProblem.Message;
                LogFileUtil.Log(m, LogFileUtil.LogLevel.Debug);
                Response myResponse = new Response() { HasError = true, ErrorMessage = m };
                return myResponse;

            }
            catch (System.ServiceModel.CommunicationException commProblem)
            {
                var m = @"There was a communication problem. 
                        " + _LoginWcfService + @"
                        " + commProblem.Message;
                if (commProblem.InnerException != null && commProblem.InnerException is WebException)
                {
                    if (commProblem.InnerException.Message.Contains("404"))
                    {
                        m = " Incorrect Server URL Path";
                    }
                }
                LogFileUtil.Log(m, LogFileUtil.LogLevel.Debug);
                Response myResponse = new Response() { HasError = true, ErrorMessage = m };
                return myResponse;
            }
            catch (Exception e)
            {
                LogFileUtil.Log(e.ToString(), LogFileUtil.LogLevel.Debug);
                Response myResponse = new Response() { HasError = true, ErrorMessage = " Incorrect Server URL Path" };
                return myResponse;
            }

        }


        public void Dispose()
        {
            _LoginWcfService = null;
            _token = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }


    }
}
