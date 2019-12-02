using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.LoginServiceReference;
using Logitude.HybridTest.WcfFactory;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.WcfCallers
{

    class PrepareCustomers
    {
        public static void PrepareCustomersVars()
        {
            UpsertShipperIdHCustomerExport1();
            UpsertCardContactHCustomerExport1();
            UpsertShipperIdTestShipperImport1();
            UpsertConsigneeIdTestConsigneeExport1();
            UpsertConsigneeIdTestConsigneeImport1();
        }
        private static void UpsertShipperIdHCustomerExport1()
        {
            CustomerPM customerPM = CustomerWcfFactory.GetCustomerPM();
            Response serviceResponse = AssertResponse(customerPM);
            HybridData.CustomerIdHCustomer = serviceResponse.Result;
        }
        private static void UpsertCardContactHCustomerExport1()
        {
            CardContactPM cardContactPM = new CardContactPM()
            {
                IsAll = true,
                ContactId = HybridData.ContactCode,
                CardId = HybridData.CustomerCodeHCustomer,
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            AssertResponse(cardContactPM);
        }
        private static void UpsertShipperIdTestShipperImport1()
        {
            CustomerPM customerPM = CustomerWcfFactory.GetCustomerPM();
            customerPM.Code = HybridData.CustomerCodeTestShipperImport1;
            customerPM.EnglishName = "TestShipperImport1";
            customerPM.LocalName = "TestShipperImport1";
            customerPM.Notes = "TestShipperImport1";
            customerPM.VatNumber = "TestShipperImport1";
            customerPM.Addresses[0].City = "New York";
            customerPM.Addresses[0].ZipCode = "+001";
            customerPM.Addresses[0].FaxNumber = "2325881";
            customerPM.Addresses[0].PhoneNumber = "+1598137715";
            customerPM.Addresses[0].CountryCode = HybridData.CountryCodeUS;
            customerPM.Addresses[0].Address1 = "18 West 48th Street";
            Response serviceResponse = AssertResponse(customerPM);
            HybridData.CustomerIdTestShipperImport1 = serviceResponse.Result;
        }
        private static void UpsertConsigneeIdTestConsigneeExport1()
        {
            CustomerPM customerPM = CustomerWcfFactory.GetCustomerPM();
            customerPM.Code = HybridData.CustomerCodeTestConsigneeExport1;
            customerPM.EnglishName = "TestConsigneeExport1";
            customerPM.LocalName = "TestConsigneeExport1";
            customerPM.Notes = "TestConsigneeExport1";
            customerPM.VatNumber = "TestConsigneeExport1";
            Response serviceResponse = AssertResponse(customerPM);
            HybridData.CustomerIdTestConsigneeExport1 = serviceResponse.Result;
        }
        private static void UpsertConsigneeIdTestConsigneeImport1()
        {
            CustomerPM customerPM = CustomerWcfFactory.GetCustomerPM();
            customerPM.Code = HybridData.CustomerCodeTestConsigneeImport1;
            customerPM.EnglishName = "TestConsigneeImport1";
            customerPM.LocalName = "TestConsigneeImport1";
            customerPM.Notes = "TestConsigneeImport1";
            customerPM.VatNumber = "TestConsigneeImport1";
            customerPM.Addresses[0].City = "New York";
            customerPM.Addresses[0].ZipCode = "+001";
            customerPM.Addresses[0].FaxNumber = "2325881";
            customerPM.Addresses[0].PhoneNumber = "+1598137715";
            customerPM.Addresses[0].CountryCode = HybridData.CountryCodeUS;
            customerPM.Addresses[0].Address1 = "18 West 48th Street";
            Response serviceResponse = AssertResponse(customerPM);
            HybridData.CustomerIdTestConsigneeImport1 = serviceResponse.Result;
        }
        private static Response AssertResponse<T>(T entityPM)
        {
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceResponse.HasError, "Prepare Customers Vars Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Prepare Customers Vars Failed! " + serviceResponse.ErrorMessage);
            return serviceResponse;
        }
    }
}
