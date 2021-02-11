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
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public static void PrepareCustomersVars()
        {
            UpsertShipperCodeHCustomerExport1();
            UpsertCardContactHCustomerExport1();
            UpsertShipperCodeTestShipperImport1();
            UpsertConsigneeCodeTestConsigneeExport1();
            UpsertConsigneeCodeTestConsigneeImport1();
        }
        private static void UpsertShipperCodeHCustomerExport1()
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
        private static void UpsertShipperCodeTestShipperImport1()
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
        }
        private static void UpsertConsigneeCodeTestConsigneeExport1()
        {
            CustomerPM customerPM = CustomerWcfFactory.GetCustomerPM();
            customerPM.Code = HybridData.CustomerCodeTestConsigneeExport1;
            customerPM.EnglishName = "TestConsigneeExport1";
            customerPM.LocalName = "TestConsigneeExport1";
            customerPM.Notes = "TestConsigneeExport1";
            customerPM.VatNumber = "TestConsigneeExport1";
            Response serviceResponse = AssertResponse(customerPM);
        }
        private static void UpsertConsigneeCodeTestConsigneeImport1()
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
        }
        private static Response AssertResponse<T>(T entityPM)
        {
            ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Prepare Customers Vars Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Prepare Customers Vars Failed! " + serviceOutcome.Response.ErrorMessage);
            return serviceOutcome.Response;
        }
    }
}
