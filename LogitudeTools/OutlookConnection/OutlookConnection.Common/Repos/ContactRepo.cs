using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlookConnection.Common.ContactWcfServiceReference;
using System.ServiceModel;
using OutlookConnection.Common.Utils;
using OutlookConnection.Common.Contracts;

namespace OutlookConnection.Common.Repos
{
    public class ContactRepo : IDisposable, IContactRepo
    {
        string _ContactWcfService;
        string _token;

        public ContactRepo()
        {
            _ContactWcfService = SettingServiceLocator.Instance.CRMSettings.ServerURL.Replace("ActivityWcfService.svc", "ContactWcfService.svc");
            _token = SettingServiceLocator.Instance.CRMSettings.Token;
        }


        public ContactWcfServiceReference.ContactPM GetContactPMByEmail(string email)
        {
            try
            {
                using (var myContactWcfServiceClient = GetserviceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)myContactWcfServiceClient.InnerChannel))
                    {
                        Response myResponse = new Response();
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        ContactPM ContactPMEntity = myContactWcfServiceClient.GetContactPMByEmail(email, SettingServiceLocator.Instance.CRMSettings.MyTenantInfo.Tenant, ref myResponse);

                        if (myResponse == null)
                        {
                            LogFileUtil.Log("ContactRepo GetContactPMByEmail() Failed - return null", LogFileUtil.LogLevel.Debug);
                            return new ContactPM();
                        }
                        else if (myResponse.HasError)
                        {
                            LogFileUtil.Log("ContactRepo GetContactPMByEmail() Failed - return Error: " + myResponse.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return new ContactPM();
                        }
                        else
                        {
                            LogFileUtil.Log("ContactRepo GetContactPMByEmail() Finished successfully: ", LogFileUtil.LogLevel.Debug);
                            return ContactPMEntity;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("ContactRepo etContactPMByEmail() Failed :" + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }

        public OutlookConnection.Common.ContactWcfServiceReference.ContactList[] GetContactList(OutlookConnection.Common.ContactWcfServiceReference.ContactApiFilters filters, int tenant, ref OutlookConnection.Common.ContactWcfServiceReference.Response response)
        {
            try
            {
                using (var myContactWcfServiceClient = GetserviceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)myContactWcfServiceClient.InnerChannel))
                    {
                        Response myResponse = new Response();
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        ContactList[] myContactList = myContactWcfServiceClient.GetContactList(filters, SettingServiceLocator.Instance.CRMSettings.MyTenantInfo.Tenant, ref response);



                        if (myResponse == null)
                        {
                            LogFileUtil.Log("ContactRepo GetContactList() Failed - return null", LogFileUtil.LogLevel.Debug);
                            return new ContactList[0];
                        }
                        else if (myResponse.HasError)
                        {
                            LogFileUtil.Log("ContactRepo GetContactList() Failed - return Error: " + myResponse.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return new ContactList[0];
                        }
                        else
                        {
                            LogFileUtil.Log("ContactRepo GetContactList() Finished successfully", LogFileUtil.LogLevel.Debug);
                            return myContactList;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("ContactRepo GetContactList() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }




        ContactWcfServiceClient GetserviceClient()
        {
            try
            {
                var remoteAddress = new System.ServiceModel.EndpointAddress(_ContactWcfService);
                var my = new ContactWcfServiceClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);
                (my.Endpoint.Binding as BasicHttpBinding).MaxReceivedMessageSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).MaxBufferSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxStringContentLength = 67108864;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxArrayLength = 67108864;
                return my;
            }
            catch (Exception e)
            {
                LogFileUtil.Log("ContactRepo GetserviceClient() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }

        public void Dispose()
        {
            _ContactWcfService = null;
            _token = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

    }
}
