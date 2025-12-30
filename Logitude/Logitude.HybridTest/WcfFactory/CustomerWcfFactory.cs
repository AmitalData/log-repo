using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfFactory
{
    class CustomerWcfFactory
    {
        readonly private static CustomerPM customerPM = new CustomerPM()
        {
            Code = HybridData.CustomerCodeHCustomer,
            EnglishName = "TestShipperExport1",
            LocalName = "TestShipperExport1",
            Notes = "TestShipperExport1",
            VatNumber = "TestShipper1Unique",
            PartnerTypeId = "CS",
            Tenant = EnvironmentGlobalParams.MainTenant,
        };

        public static CustomerPM GetCustomerPM() {
            PreapareAddress();
            return customerPM;
        }
        public static CustomerPM GetCustomerPMWithNewCode()
        {
            var counterAdditionalParameters = new Dictionary<string, string>
            {
                ["[B]"] = "CS",
                ["[BranchName]"] = "CS"
            };
            customerPM.Code = TableCounter.DoesCounterDefinitionExist("CADC", EnvironmentGlobalParams.MainTenant, "CS") ? TableCounter.GetNumber(EnvironmentGlobalParams.MainTenant, "CADC", "CS", null, counterAdditionalParameters, true) : CodeCounter.GetNumber("Customer", EnvironmentGlobalParams.MainTenant).ToString();
            PreapareAddress();
            return customerPM;
        }
        private static void PreapareAddress()
        {
            customerPM.Addresses.Add(new AddressPM
            {
                Description = "Main Address",
                City = "London",
                ZipCode = "+207",
                FaxNumber = "24564110",
                PhoneNumber = "0598137715",
                AddressTypeId = "M",
                Address1 = "14 Tottenham Court Road",
                Address2 = "A400",
                Name = "TestShipperExport1Address",
                CountryCode = HybridData.CountryCodeUS,
                Tenant = EnvironmentGlobalParams.MainTenant,
            });
        }
    }
}
