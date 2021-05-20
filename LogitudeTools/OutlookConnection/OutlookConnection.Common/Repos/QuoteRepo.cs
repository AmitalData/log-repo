using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlookConnection.Common.Contracts;
using System.ServiceModel;
using OutlookConnection.Common.QuoteWcfServiceReference;
using OutlookConnection.Common.Utils;

namespace OutlookConnection.Common.Repos
{
    public class QuoteRepo : IQuoteRepo, IDisposable
    {
        string _QuoteWcfService;
        string _token;

        public QuoteRepo()
        {
            _QuoteWcfService = SettingServiceLocator.Instance.CRMSettings.ServerURL.Replace("ActivityWcfService.svc", "QuoteWcfService.svc");
            _token = SettingServiceLocator.Instance.CRMSettings.Token;
        }

        QuoteWcfServiceClient GetserviceClient()
        {
            try
            {
                var remoteAddress = new System.ServiceModel.EndpointAddress(_QuoteWcfService);
                var my = new QuoteWcfServiceClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);
                (my.Endpoint.Binding as BasicHttpBinding).MaxReceivedMessageSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).MaxBufferSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxStringContentLength = 67108864;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxArrayLength = 67108864;
                return my;
            }
            catch (Exception e)
            {
                LogFileUtil.Log("QuoteRepo GetserviceClient() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }

        public OutlookConnection.Common.QuoteWcfServiceReference.QuoteList[] GetQuoteList(OutlookConnection.Common.QuoteWcfServiceReference.QuoteApiFilters filters, int tenant, ref OutlookConnection.Common.QuoteWcfServiceReference.Response response)
        {
            try
            {
                using (var myQuoteWcfServiceClient = GetserviceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)myQuoteWcfServiceClient.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        QuoteList[] quoteList = myQuoteWcfServiceClient.GetQuoteList(filters, tenant, ref response);

                        if (quoteList.Length > 0)
                        {
                            //Finished successfully
                            //LogFileUtil.Log("QuoteList GetQuoteList() Finished successfully: ", LogFileUtil.LogLevel.Debug);
                            return quoteList;
                        }
                        if (response == null)
                        {
                            //Failed - return null
                            //LogFileUtil.Log("QuoteRepo GetQuoteList() Failed - return null", LogFileUtil.LogLevel.Debug);
                            return new QuoteList[0];
                        }
                        else if (response.HasError)
                        {

                            LogFileUtil.Log("QuoteRepo GetQuoteList() Failed - return Error: " + response.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return new QuoteList[0];
                        }
                        else
                        {
                            //Finished successfully
                            //LogFileUtil.Log("QuoteRepo GetQuoteList() Finished successfully: ", LogFileUtil.LogLevel.Debug);
                            return quoteList;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("QuoteRepo GetQuoteList() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }


        public void Dispose()
        {
            _QuoteWcfService = null;
            _token = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }


    }
}
