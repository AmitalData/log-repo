//using Cloud.Sign.App.LoginWcfServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Cloud.Sign.App.Helpers
{
    public class ServicesHelper
    {
        string ServerURL = "http://localhost:9996";//"http://192.168.1.51/main";
        string _LoginWcfService;
        //public ServicesHelper()
        //{
        //    _LoginWcfService = ServerURL + "/wcfapi/LoginWcfService.svc";
        //}

        //LoginWcfServiceClient GetloginserviceClient()
        //{
        //    var remoteAddress = new System.ServiceModel.EndpointAddress(_LoginWcfService);
        //    var my = new LoginWcfServiceClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);
        //    return my;
        //}

        //public Response Login(string email, string password)
        //{
        //    try
        //    {
        //        using (var myLoginWcfServiceClient = GetloginserviceClient())
        //        {
        //            Response myResponse = myLoginWcfServiceClient.Login(email, password);
        //            return myResponse;
        //        }
        //    }
        //    catch (TimeoutException timeProblem)
        //    {
        //        var m = @"The service operation timed out. 
        //                " + _LoginWcfService + @"
        //                " + timeProblem.Message;
        //        LogFileUtil.Log(m, LogFileUtil.LogLevel.Debug);
        //        Response myResponse = new Response() { HasError = true, ErrorMessage = m };
        //        return myResponse;

        //    }
        //    catch (System.ServiceModel.CommunicationException commProblem)
        //    {
        //        var m = @"There was a communication problem. 
        //                " + _LoginWcfService + @"
        //                " + commProblem.Message;
        //        if (commProblem.InnerException != null && commProblem.InnerException is WebException)
        //        {
        //            if (commProblem.InnerException.Message.Contains("404"))
        //            {
        //                m = " Incorrect Server URL Path";
        //            }
        //        }
        //        LogFileUtil.Log(m, LogFileUtil.LogLevel.Debug);
        //        Response myResponse = new Response() { HasError = true, ErrorMessage = m };
        //        return myResponse;
        //    }
        //    catch (Exception e)
        //    {
        //        LogFileUtil.Log(e.ToString(), LogFileUtil.LogLevel.Debug);
        //        Response myResponse = new Response() { HasError = true, ErrorMessage = " Incorrect Server URL Path" };
        //        return myResponse;
        //    }

        //}

        //public TenantInfo[] GetUserTenants(string email)
        //{
        //    Response myResponse = new Response();
        //    try
        //    {
        //        using (var myLoginWcfServiceClient = GetloginserviceClient())
        //        {
        //            TenantInfo[] Tenants = myLoginWcfServiceClient.GetUserTenants(email,ref myResponse);
        //            //myResponse. = 
        //            return Tenants;
        //        }
        //    }
        //    catch (TimeoutException timeProblem)
        //    {
        //        var m = @"The service operation timed out. 
        //                " + _LoginWcfService + @"
        //                " + timeProblem.Message;
        //        LogFileUtil.Log(m, LogFileUtil.LogLevel.Debug);
        //        //Response myResponse = new Response() { HasError = true, ErrorMessage = m };
        //        return null;

        //    }
        //    catch (System.ServiceModel.CommunicationException commProblem)
        //    {
        //        var m = @"There was a communication problem. 
        //                " + _LoginWcfService + @"
        //                " + commProblem.Message;
        //        if (commProblem.InnerException != null && commProblem.InnerException is WebException)
        //        {
        //            if (commProblem.InnerException.Message.Contains("404"))
        //            {
        //                m = " Incorrect Server URL Path";
        //            }
        //        }
        //        LogFileUtil.Log(m, LogFileUtil.LogLevel.Debug);
        //        //Response myResponse = new Response() { HasError = true, ErrorMessage = m };
        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        LogFileUtil.Log(e.ToString(), LogFileUtil.LogLevel.Debug);
        //        //Response myResponse = new Response() { HasError = true, ErrorMessage = " Incorrect Server URL Path" };
        //        return null;
        //    }

        //}
    }
}
