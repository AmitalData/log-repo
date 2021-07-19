using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlookConnection.Common.OpportunityWcfServiceReference;
using System.ServiceModel;
using OutlookConnection.Common.Contracts;
using OutlookConnection.Common.Utils;

namespace OutlookConnection.Common.Repos
{
    public class OpportunityRepo : IOpportunityRepo, IDisposable
    {
        string _OpportunityWcfService;
        string _token;

        public OpportunityRepo()
        {
            _OpportunityWcfService = SettingServiceLocator.Instance.CRMSettings.ServerURL.Replace("ActivityWcfService.svc", "OpportunityWcfService.svc");
            _token = SettingServiceLocator.Instance.CRMSettings.Token;
        }

        OpportunityWcfServiceClient GetserviceClient()
        {
            try
            {
                var remoteAddress = new System.ServiceModel.EndpointAddress(_OpportunityWcfService);
                var my = new OpportunityWcfServiceClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);
                (my.Endpoint.Binding as BasicHttpBinding).MaxReceivedMessageSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).MaxBufferSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxStringContentLength = 67108864;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxArrayLength = 67108864;
                return my;
            }
            catch (Exception e)
            {
                LogFileUtil.Log("OpportunityRepo GetserviceClient() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }

        }

        public OpportunityList[] GetOpportunityList(string email, string searchText, int tenant, int skip, int take, OutlookConnection.Common.OpportunityWcfServiceReference.OpportunityApiFilters filters, ref OutlookConnection.Common.OpportunityWcfServiceReference.Response response)
        {
            try
            {
                using (var myWcfServiceClient = GetserviceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)myWcfServiceClient.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", _token);
                        OpportunityList[] OpportunityList = myWcfServiceClient.GetOpportunityList(email, searchText, tenant, skip, take, filters, ref response);
                        if (OpportunityList.Length > 0)
                        {
                            //Finished successfully
                            //LogFileUtil.Log("OpportunityRepo GetOpportunityList() Finished successfully: ", LogFileUtil.LogLevel.Debug);
                            return OpportunityList;
                        }

                        if (response == null)
                        {
                            //Failed - return null
                            //LogFileUtil.Log("OpportunityRepo GetOpportunityList() Failed - return null", LogFileUtil.LogLevel.Debug);
                            return new OpportunityList[0];
                        }
                        else if (response.HasError)
                        {
                            LogFileUtil.Log("OpportunityRepo GetOpportunityList() Failed - return Error: " + response.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return new OpportunityList[0];
                        }
                        else
                        {
                            //Failed - return null
                            //LogFileUtil.Log("OpportunityRepo GetOpportunityList() Finished successfully: ", LogFileUtil.LogLevel.Debug);
                            return OpportunityList;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("OpportunityRepo GetOpportunityList() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }

        }

        public OutlookConnection.Common.OpportunityWcfServiceReference.CustomerList GetCustomerListByOpportunityId(string opportunityId, int tenant, ref OutlookConnection.Common.OpportunityWcfServiceReference.Response response)
        {
            try
            {
                using (var myWcfServiceClient = GetserviceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)myWcfServiceClient.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", _token);
                        OutlookConnection.Common.OpportunityWcfServiceReference.CustomerList CustomerList = myWcfServiceClient.GetCustomerListByOpportunityId(opportunityId, tenant, ref  response);
                        return CustomerList;
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("OpportunityRepo GetCustomerListByOpportunityId() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }

        public void Dispose()
        {
            _OpportunityWcfService = null;
            _token = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }


    }
}
