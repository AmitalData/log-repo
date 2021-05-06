using OutlookConnection.Common.Contracts;
using OutlookConnection.Common.TotangoWcfServiceReference;
using OutlookConnection.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OutlookConnection.Common.Repos
{
    public class TotangoRepo : IDisposable, ITotangoRepo
    {
        string _TotangoWcfService;
        string _token;

        public TotangoRepo()
	    {
            _TotangoWcfService = SettingServiceLocator.Instance.CRMSettings.ServerURL.Replace("ActivityWcfService.svc", "TotangoWcfService.svc");
            _token = SettingServiceLocator.Instance.CRMSettings.Token;
	    }

        public void SendUserActivity(string Email, string orgDisplayName, string module, string activity, int tenant)
        {
            try
            {
                using (var myTotangWcfServiceReference = GetserviceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)myTotangWcfServiceReference.InnerChannel))
                    {
                        //Response myResponse = new Response();
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        myTotangWcfServiceReference.SendUserActivity(Email,orgDisplayName,module,activity, tenant);
                        //if (myResponse == null)
                        //{
                        //    LogFileUtil.Log("FeatureRepo GetActiveFeaturesForUser() Failed - return null", LogFileUtil.LogLevel.Debug);
                        //    return new FeatureAccessInfo[0];
                        //}
                        //else if (myResponse.HasError)
                        //{
                        //    LogFileUtil.Log("FeatureRepo GetActiveFeaturesForUser() Failed - return Error: " + myResponse.ErrorMessage, LogFileUtil.LogLevel.Debug);
                        //    return new FeatureAccessInfo[0];
                        //}
                        //else
                        //{
                        //    LogFileUtil.Log("FeatureRepo GetActiveFeaturesForUser() Finished successfully", LogFileUtil.LogLevel.Debug);
                        //    return FeatureAccessInfoArray;
                        //}
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("TotangoRepo SendUserActivity() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return;
            }
        }

        TotangoWcfServiceClient GetserviceClient()
        {
            try
            {
                var remoteAddress = new System.ServiceModel.EndpointAddress(_TotangoWcfService);
                var my = new TotangoWcfServiceClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);
                (my.Endpoint.Binding as BasicHttpBinding).MaxReceivedMessageSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).MaxBufferSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxStringContentLength = 67108864;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxArrayLength = 67108864;
                return my;
            }
            catch (Exception e)
            {
                LogFileUtil.Log("TotangoRepo GetserviceClient() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }
       
        //public TotangoWcfServiceReference.FeatureAccessInfo[] GetActiveFeaturesForUser(FeatureWcfServiceReference.FeatureAccessInfo[] featuresList, int tenant, ref FeatureWcfServiceReference.Response response)
        //{
        //    try
        //    {
        //        using (var myFeatureWcfServiceReference = GetserviceClient())
        //        {
        //            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)myFeatureWcfServiceReference.InnerChannel))
        //            {
        //                Response myResponse = new Response();
        //                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
        //                FeatureAccessInfo[] FeatureAccessInfoArray = myFeatureWcfServiceReference.GetActiveFeaturesForUser(featuresList, tenant, ref response);
        //                if (myResponse == null)
        //                {
        //                    LogFileUtil.Log("FeatureRepo GetActiveFeaturesForUser() Failed - return null", LogFileUtil.LogLevel.Debug);
        //                    return new FeatureAccessInfo[0];
        //                }
        //                else if (myResponse.HasError)
        //                {
        //                    LogFileUtil.Log("FeatureRepo GetActiveFeaturesForUser() Failed - return Error: " + myResponse.ErrorMessage, LogFileUtil.LogLevel.Debug);
        //                    return new FeatureAccessInfo[0];
        //                }
        //                else
        //                {
        //                    LogFileUtil.Log("FeatureRepo GetActiveFeaturesForUser() Finished successfully", LogFileUtil.LogLevel.Debug);
        //                    return FeatureAccessInfoArray;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        LogFileUtil.Log("FeatureRepo GetActiveFeaturesForUser() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
        //        return new FeatureAccessInfo[0];
        //    }
        //}

        public void Dispose()
        {
            _TotangoWcfService = null;
            _token = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        } 
    }

   

}
