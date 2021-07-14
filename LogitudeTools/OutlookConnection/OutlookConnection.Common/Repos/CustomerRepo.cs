using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using OutlookConnection.Common.Contracts;
using OutlookConnection.Common.CustomerWcfServiceReference;
using OutlookConnection.Common.Utils;
using System.ServiceModel;

namespace OutlookConnection.Common.Repos
{
    public class CustomerRepo : ICustomerRepo, IDisposable
    {
        string _CustomerWcfService;
        string _token;


        public CustomerRepo()
        {
            _CustomerWcfService = SettingServiceLocator.Instance.CRMSettings.ServerURL.Replace("ActivityWcfService.svc", "CustomerWcfService.svc");
            _token = SettingServiceLocator.Instance.CRMSettings.Token;
        }

        CustomerWcfServiceClient GetserviceClient()
        {
            try
            {
                var remoteAddress = new System.ServiceModel.EndpointAddress(_CustomerWcfService);
                var my = new CustomerWcfServiceClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);
                (my.Endpoint.Binding as BasicHttpBinding).MaxReceivedMessageSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).MaxBufferSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxStringContentLength = 67108864;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxArrayLength = 67108864;
                return my;
            }
            catch (Exception e)
            {
                LogFileUtil.Log("CustomerRepo GetserviceClient() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }

        }

        public CustomerList[] CustomersByContact(string email, string search)
        {
            return CustomersByContact(email, search, false, 30);
        }

        public CustomerList[] CustomersByContact(string email, string search, bool myOnly, int take)
        {
            try
            {
                using (var myCustomerWcfServiceClient = GetserviceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)myCustomerWcfServiceClient.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        Response myResponse = new Response();
                        OutlookConnection.Common.CustomerWcfServiceReference.CustomerList[] customerList;

                        if (true || email == null)
                        {
                            customerList = myCustomerWcfServiceClient.GetCustomerList(search, email, myOnly, SettingServiceLocator.Instance.CRMSettings.MyTenantInfo.Tenant, 0, take, ref myResponse);
                        }
                        else if (search == null)
                        {
                            customerList = myCustomerWcfServiceClient.GetCustomerListByEmail(email, SettingServiceLocator.Instance.CRMSettings.MyTenantInfo.Tenant, ref myResponse);
                        }
                        else
                        {
                            customerList = myCustomerWcfServiceClient.GetCustomerListByEmail(null, SettingServiceLocator.Instance.CRMSettings.MyTenantInfo.Tenant, ref myResponse);
                        }

                        if (customerList.Length > 0)
                        {
                            //Finished successfully
                            LogFileUtil.Log("CustomerRepo CustomersByContact() Finished successfully: ", LogFileUtil.LogLevel.Debug);
                            return customerList;
                        }
                        if (myResponse == null)
                        {
                            //Failed - return null
                            LogFileUtil.Log("CustomerRepo CustomersByContact() Failed - return null", LogFileUtil.LogLevel.Debug);
                            return new CustomerList[0];
                        }
                        else if (myResponse.HasError)
                        {
                            LogFileUtil.Log("CustomerRepo CustomersByContact() Failed - return Error: " + myResponse.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return new CustomerList[0];
                        }
                        else
                        {
                            //Finished successfully
                            LogFileUtil.Log("CustomerRepo CustomersByContact() Finished successfully", LogFileUtil.LogLevel.Debug);
                            return customerList;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("CustomerRepo CustomersByContact() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }



        public CustomerList CustomersByID(string Id)
        {
            if (String.IsNullOrWhiteSpace(Id))
            {
                return null;
            }
            try
            {
                using (var myCustomerWcfServiceClient = GetserviceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)myCustomerWcfServiceClient.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        Response myResponse = new Response();
                        var customerList = myCustomerWcfServiceClient.GetCustomerListById(Id, SettingServiceLocator.Instance.CRMSettings.MyTenantInfo.Tenant, ref myResponse);



                        if (myResponse == null)
                        {
                            LogFileUtil.Log("CustomerRepo CustomersByID() Failed - return null", LogFileUtil.LogLevel.Debug);
                            return new CustomerList();
                        }
                        else if (myResponse.HasError)
                        {
                            LogFileUtil.Log("CustomerRepo CustomersByID() Failed - return Error: " + myResponse.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return new CustomerList();
                        }
                        else
                        {
                            LogFileUtil.Log("CustomerRepo CustomersByID() Finished successfully: ", LogFileUtil.LogLevel.Debug);
                            return customerList;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("CustomersByContact :" + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }





        public void Dispose()
        {
            _CustomerWcfService = null;
            _token = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

        }

    }

    public class MockCustomerRepo : ICustomerRepo
    {


        public OutlookConnection.Common.CustomerWcfServiceReference.CustomerList[] CustomersByContact(string email, string search)
        {
            var list = new List<CustomerList>();

            for (int i = 0; i < 30; i++)
            {
                list.Add(GetNewCustomer(i));
            }

            return list.ToArray();
        }

        private CustomerList GetNewCustomer(int i)
        {

            return new CustomerList()
            {
                Id = i.ToString(),
                CityName = "CityName:" + i,
                //LocalName = "LocalName" + i,
                EnglishName = "EnglishName" + i,
                SalesmanUserEnglishName = "SalesmanUserEnglishName" + i

            }
                ;
        }


        public CustomerList CustomersByID(string Id)
        {
            return null;
        }


        public CustomerList[] CustomersByContact(string email, string search, bool myOnly, int take)
        {
            return CustomersByContact(email, search, false, take);
        }
    }
}
