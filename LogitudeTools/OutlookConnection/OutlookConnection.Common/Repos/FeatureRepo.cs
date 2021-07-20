using OutlookConnection.Common.Contracts;
using OutlookConnection.Common.FeatureWcfServiceReference;
using OutlookConnection.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OutlookConnection.Common.Repos
{
    public class FeatureRepo : IDisposable, IFeatureRepo
    {
        string _FeatureWcfService;
        string _token;

        public FeatureRepo ()
	    {
             _FeatureWcfService = SettingServiceLocator.Instance.CRMSettings.ServerURL.Replace("ActivityWcfService.svc", "FeatureWcfService.svc");
            _token = SettingServiceLocator.Instance.CRMSettings.Token;
	    }

       
        public FeatureWcfServiceReference.FeatureAccessInfo[] GetActiveFeaturesForUser(FeatureWcfServiceReference.FeatureAccessInfo[] featuresList, int tenant, ref FeatureWcfServiceReference.Response response)
        {
            try
            {
                using (var myFeatureWcfServiceReference = GetserviceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)myFeatureWcfServiceReference.InnerChannel))
                    {
                        Response myResponse = new Response();
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        FeatureAccessInfo[] FeatureAccessInfoArray = myFeatureWcfServiceReference.GetActiveFeaturesForUser(featuresList, tenant, ref response);
                        if (myResponse == null)
                        {
                            LogFileUtil.Log("FeatureRepo GetActiveFeaturesForUser() Failed - return null", LogFileUtil.LogLevel.Debug);
                            return new FeatureAccessInfo[0];
                        }
                        else if (myResponse.HasError)
                        {
                            LogFileUtil.Log("FeatureRepo GetActiveFeaturesForUser() Failed - return Error: " + myResponse.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return new FeatureAccessInfo[0];
                        }
                        else
                        {
                            LogFileUtil.Log("FeatureRepo GetActiveFeaturesForUser() Finished successfully", LogFileUtil.LogLevel.Debug);
                            return FeatureAccessInfoArray;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("FeatureRepo GetActiveFeaturesForUser() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return new FeatureAccessInfo[0];
            }
        }

        public bool CheckOutlookVersion(string Version)
        {
            try
            {
                using (var myFeatureWcfServiceReference = GetserviceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)myFeatureWcfServiceReference.InnerChannel))
                    { 
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        bool temp = myFeatureWcfServiceReference.CheckOutlookVersion(Version);
                        return temp;
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("FeatureRepo GetActiveFeaturesForUser() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return true;
            }
        }



        FeatureWcfServiceClient GetserviceClient()
        {
            try
            {
                var remoteAddress = new System.ServiceModel.EndpointAddress(_FeatureWcfService);
                var my = new FeatureWcfServiceClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);
                (my.Endpoint.Binding as BasicHttpBinding).MaxReceivedMessageSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).MaxBufferSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxStringContentLength = 67108864;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxArrayLength = 67108864;
                return my;
            }
            catch (Exception e)
            {
                LogFileUtil.Log("FeatureRepo GetserviceClient() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }

        public void Dispose()
        {
            _FeatureWcfService = null;
            _token = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }




        public Task<GetActiveFeaturesForUserResponse> GetActiveFeaturesForUserAsync(GetActiveFeaturesForUserRequest request)
        {
            throw new NotImplementedException();
        }
    }

   

}
