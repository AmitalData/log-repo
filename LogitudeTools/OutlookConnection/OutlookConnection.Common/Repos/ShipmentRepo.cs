using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlookConnection.Common.ShipmentWcfServiceReference;
using System.ServiceModel;
using OutlookConnection.Common.Utils;
using OutlookConnection.Common.Contracts;

namespace OutlookConnection.Common.Repos
{
    public class ShipmentRepo : IShipmentRepo, IDisposable
    {
        string _ShipmentWcfService;
        string _token;


        public ShipmentRepo()
        {
            _ShipmentWcfService = SettingServiceLocator.Instance.CRMSettings.ServerURL.Replace("ActivityWcfService.svc", "ShipmentWcfService.svc");
            _token = SettingServiceLocator.Instance.CRMSettings.Token;
        }


        ShipmentWcfServiceClient GetserviceClient()
        {
            try
            {
                var remoteAddress = new System.ServiceModel.EndpointAddress(_ShipmentWcfService);
                var my = new ShipmentWcfServiceClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);
                my.Endpoint.Address = new EndpointAddress(_ShipmentWcfService);
                (my.Endpoint.Binding as BasicHttpBinding).MaxReceivedMessageSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).MaxBufferSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxStringContentLength = 67108864;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxArrayLength = 67108864;
                return my;
            }
            catch (Exception e)
            {
                LogFileUtil.Log("ShipmentRepo GetserviceClient() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }


        // public CustomerList[] CustomersByContact(string email, string search, bool myOnly, int take)
        public ShipmentWcfServiceReference.ShipmentList[] GetShipmentList(OutlookConnection.Common.ShipmentWcfServiceReference.ShipmentApiFilters filters, int tenant, ref OutlookConnection.Common.ShipmentWcfServiceReference.Response response)
        {
            try
            {
                using (var myShipmentWcfServiceClient = GetserviceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)myShipmentWcfServiceClient.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        ShipmentList[] ShipmentList = myShipmentWcfServiceClient.GetShipmentList(filters, tenant, ref response);

                        if (response != null && response.HasError)
                        {

                            LogFileUtil.Log("ShipmentRepo GetShipmentList() Failed - return Error: " + response.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return new ShipmentList[0];
                        }

                        if (ShipmentList == null)
                        {
                            //Failed - return null
                            LogFileUtil.Log("ShipmentRepo GetShipmentList() Failed - return null", LogFileUtil.LogLevel.Debug);
                            return new ShipmentList[0];
                        }

                       

                        if (ShipmentList.Length > 0)
                        {
                            //Finished successfully
                            //LogFileUtil.Log("ShipmentRepo GetShipmentList() Finished successfully: ", LogFileUtil.LogLevel.Debug);
                            return ShipmentList;
                        }

                        else
                        {
                            ////Finished successfully
                           //LogFileUtil.Log("ShipmentRepo GetShipmentList() Finished with empty : ", LogFileUtil.LogLevel.Debug);
                            return ShipmentList;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("ShipmentRepo GetShipmentList() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }


        public void Dispose()
        {
            _ShipmentWcfService = null;
            _token = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }


    }
}
