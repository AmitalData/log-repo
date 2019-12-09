using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.FullAccounting
{
    public class FullAccountingPreparationCalls
    {
        public static async Task PrepareVariables()
        {
            await SetChartOfAccountId();
            await GetFullAccountingSettings();
            await GetAccounntingPeriods();
            await OpenCurrentMonth();
            await GetAccountingCurriency();
            await GetCountryAX();
            await GetVatTypeExempt();
            await GetPaymentTermCash();
            await GetPaymnetMethodCash();
            await GetBranchMainOffice();
            await GetChargeTypesAirFreight();
            await GetCustomerTestGlCustomer();
            await GetVendorTestGlVendor();
            await GetGlAccountCustomer();
            await GetGlAccountVendor();
            await GetAddressCustomer();
            await GetAddressVendor();
            await GetChartOfAccountBank();
            await CreateGlAccountBank();
            await CreateBankCodeBK14();
            await CreateBranchCashBookBK14();
        }
        public static async Task<ChartOfAccountList> GetSingleChartOfAccountByCode()
        {
            string urlparameters = QueryFiltersPreparation.GetUrlParameters(FullAccountingVariables.ChartOfAccountCode);
            HttpResponseMessage response = await RestClientService.GetAsync("chartofaccountviews" + urlparameters);
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            ChartOfAccountList chartOfAccountList = result != null ? JsonConvert.DeserializeObject<ChartOfAccountList>(result):null;
            //Assert.IsNotNull(chartOfAccountList);
            return chartOfAccountList;
        }
        public static async Task<ChartOfAccountPM> GetSingleChartOfAccount()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ChartOfAccounts/GetSingle?id=" + FullAccountingVariables.ChartOfAccountId);
            var stringResult = response.Content.ReadAsStringAsync().Result;
            ChartOfAccountPM chartOfAccountPM = JsonConvert.DeserializeObject<ChartOfAccountPM>(stringResult);
            //Assert.IsNotNull(chartOfAccountPM);
            return chartOfAccountPM;
        }
        private static async Task GetFullAccountingSettings()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("FullAccountingSettings/GetSingle?id=" + IntegrationTestLoginParameters.Tenant);
            var stringResult = response.Content.ReadAsStringAsync().Result;
            FullAccountingSettingPM fullAccountingSettingPM = JsonConvert.DeserializeObject<FullAccountingSettingPM>(stringResult);
            //Assert.IsNotNull(fullAccountingSettingPM);
            FullAccountingVariables.VendorControlAccountId = fullAccountingSettingPM.VendorControlAccountId;
            FullAccountingVariables.AccountingActivated = fullAccountingSettingPM.AccountingActivated;
        }
        private static async Task GetAccounntingPeriods()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("AccountingPeriodViews/GetByFilters?GetAll=true");
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            AccountingPeriodList accountingPeriodList = result != null ? JsonConvert.DeserializeObject<AccountingPeriodList>(result):null;
            //Assert.IsNotNull(accountingPeriodList);
            FullAccountingVariables.AcocuntingPeriodsId = accountingPeriodList.Id;
            FullAccountingVariables.AcocuntingPeriodsTenant = accountingPeriodList.Tenant;
            FullAccountingVariables.AcocuntingPeriodsYear = accountingPeriodList.Year;
            FullAccountingVariables.AcocuntingPeriodsPeriodTypeCode = accountingPeriodList.PeriodTypeCode;
            FullAccountingVariables.AcocuntingPeriodsPeriodTypeName = accountingPeriodList.PeriodTypeName;
            FullAccountingVariables.AcocuntingPeriodsClosedMonth = accountingPeriodList.ClosedMonth;
        }
        public static async Task<AccountingPeriodPM> GetSingleAccountingPeriods()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("AccountingPeriods/GetSingle?id=" + FullAccountingVariables.AcocuntingPeriodsId);
            var stringResult = response.Content.ReadAsStringAsync().Result;
            AccountingPeriodPM accountingPeriodPM = JsonConvert.DeserializeObject<AccountingPeriodPM>(stringResult);
            //Assert.IsNotNull(accountingPeriodPM);
            return accountingPeriodPM;
        }
        private static async Task OpenCurrentMonth()
        {
            AccountingPeriodPM accountingPeriodPM = await GetSingleAccountingPeriods();
            accountingPeriodPM.PeriodTypeName = "Accounting";
            accountingPeriodPM.PeriodTypeCode = "1";
            HttpResponseMessage response = await RestClientService.PutAsync(accountingPeriodPM, "AccountingPeriods");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            AccountingPeriodPM PutAccountingPeriodPM = JsonConvert.DeserializeObject<AccountingPeriodPM>(stringResult);
            //Assert.IsNotNull(PutAccountingPeriodPM);
        }
        private static async Task  SetChartOfAccountId()
        {
            ChartOfAccountList chartOfAccountList = await GetSingleChartOfAccountByCode();
            FullAccountingVariables.ChartOfAccountId = chartOfAccountList.Id;
            //Assert.IsNotNull(chartOfAccountList);
        }
        private static async Task GetAccountingCurriency()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("Tenants/GetSingle?id=" + IntegrationTestLoginParameters.Tenant);
            var stringResult = response.Content.ReadAsStringAsync().Result;
            TenantPM tenantPM = JsonConvert.DeserializeObject<TenantPM>(stringResult);
            //Assert.IsNotNull(tenantPM);
            FullAccountingVariables.AccountingCurrencyId = tenantPM.CurrencyId;
         }
        private static async Task GetCountryAX()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CountryViews/getbyfilters?Filter1Name=Code&Filter1Operator=equals&Filter1Value=AX&PageSize=22");
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            CountryList countryList = result != null ? JsonConvert.DeserializeObject<CountryList>(result):null;
            //Assert.IsNotNull(countryList);
            FullAccountingVariables.CountryAXId = countryList.Id;
        }
        private static async Task GetVatTypeExempt()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("VatTypeViews/getbyfilters?Filter1Name=SearchFields&Filter1Operator=Contains&GetCount=true&PageSize=23&Filter1Value=EPT");
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            VatTypeList vatTypeList = result!=null? JsonConvert.DeserializeObject<VatTypeList>(result):null;
            if (vatTypeList==null)
              await  CreateVatTypeExempt();
            else
            {
                FullAccountingVariables.VatEXMPTId = vatTypeList.Id;
            }
        }
        private static async Task CreateVatTypeExempt()
        {
            VatTypePM vatTypeExemptPM = GetNewVatTypeExemptPM();
            HttpResponseMessage response = await RestClientService.PostAsync(vatTypeExemptPM,"VatType");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            VatTypePM vatTypePM = JsonConvert.DeserializeObject<VatTypePM>(stringResult);
            //Assert.IsNotNull(vatTypePM);
            FullAccountingVariables.VatEXMPTId = vatTypePM.Id;
        }
        private static VatTypePM GetNewVatTypeExemptPM()
        {
            VatTypePM vatTypePM = new VatTypePM();
            vatTypePM.Tenant = IntegrationTestLoginParameters.Tenant;
            vatTypePM.Code = "EPT";
            vatTypePM.EnglishName = "Exempt";
            vatTypePM.LocalName = "Exempt";
            vatTypePM.SearchFields = "EPT,Exempt";
            vatTypePM.NewEntityPercentage = 0.0;
            vatTypePM.NewEntityPercentageDate = new DateTime();
            vatTypePM.VatTypePercentages = new List<VatTypePercentagePM>();
            VatTypePercentagePM vatTypePercentagePM = new VatTypePercentagePM();
            vatTypePercentagePM.Tenant = IntegrationTestLoginParameters.Tenant;
            vatTypePercentagePM.FromDate = new DateTime();
            vatTypePercentagePM.Percentage = 0.0;
            vatTypePM.VatTypePercentages.Add(vatTypePercentagePM);
            return vatTypePM;
        }
        private static async Task GetPaymentTermCash()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("PaymentTermViews/getbyfilters?Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=cash&PageSize=23");
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            PaymentTermList paymentTermList = result != null ? JsonConvert.DeserializeObject<PaymentTermList>(result):null;
            if (paymentTermList == null)
                await CreatePaymentTermCash();
            else
            {
                FullAccountingVariables.PaymentTermCashId = paymentTermList.Id;
            }
        }
        private static async Task CreatePaymentTermCash()
        {
            PaymentTermPM paymentTermCashPM = GetNewPaymentTermCashPM();
            HttpResponseMessage response = await RestClientService.PostAsync(paymentTermCashPM, "PaymentTerm");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            PaymentTermPM paymentTermPM = JsonConvert.DeserializeObject<PaymentTermPM>(stringResult);
            //Assert.IsNotNull(paymentTermPM);
            FullAccountingVariables.PaymentTermCashId = paymentTermPM.Id;
        }
        private static PaymentTermPM GetNewPaymentTermCashPM()
        {
            PaymentTermPM paymentTermPM = new PaymentTermPM();
            paymentTermPM.Tenant = IntegrationTestLoginParameters.Tenant;
            paymentTermPM.EnglishName = "cash";
            paymentTermPM.LocalName = "cash";
            paymentTermPM.SearchFields = "cash";
            paymentTermPM.Days = 0;
            paymentTermPM.FromDateTypeCode = "INV";
            paymentTermPM.InActive = false;
            paymentTermPM.CurrentMonth = false;
            paymentTermPM.IsManuallySet = false;
            paymentTermPM.DisplayInLOV = true;
            return paymentTermPM;
        }
        private static async Task GetPaymnetMethodCash()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("AccountingPaymentMethodViews/getbyfilters?Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=Cash&GetCount=true&PageSize=23");
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            AccountingPaymentMethodList accountingPaymentMethodList = result != null ? JsonConvert.DeserializeObject<AccountingPaymentMethodList>(result) : null;
            if (accountingPaymentMethodList == null)
                await CreatePaymentMethodCash();
            else
            {
                FullAccountingVariables.PaymentMethodCashId = accountingPaymentMethodList.Id;
            }
        }
        private static async Task CreatePaymentMethodCash()
        {
            AccountingPaymentMethodPM accountingPaymentMethodCashPM = GetAccountingPaymentMethodPM();
            HttpResponseMessage response = await RestClientService.PostAsync(accountingPaymentMethodCashPM, "AccountingPaymentMethods");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            AccountingPaymentMethodPM accountingPaymentMethodPM = JsonConvert.DeserializeObject<AccountingPaymentMethodPM>(stringResult);
            //Assert.IsNotNull(accountingPaymentMethodPM);
            FullAccountingVariables.PaymentMethodCashId = accountingPaymentMethodPM.Id;
        }
        private static AccountingPaymentMethodPM GetAccountingPaymentMethodPM()
        {
            AccountingPaymentMethodPM accountingPaymentMethodPM = new AccountingPaymentMethodPM();
            accountingPaymentMethodPM.Tenant = IntegrationTestLoginParameters.Tenant;
            accountingPaymentMethodPM.Name = "Cash";
            accountingPaymentMethodPM.LocalName = "Cash";
            accountingPaymentMethodPM.SearchFields = "CA,Cash";
            accountingPaymentMethodPM.Code = "CA";
            accountingPaymentMethodPM.IsAP = true;
            accountingPaymentMethodPM.IsAR = true;
            return accountingPaymentMethodPM;
        }
        private static async Task GetBranchMainOffice()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("BranchViews/getbyfilters?Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=main&GetCount=true&PageSize=23");
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            BranchList branchList = result != null ? JsonConvert.DeserializeObject<BranchList>(result) : null;
            if (branchList == null)
                await CreateBranchMainOffice();
            else
            {
                FullAccountingVariables.PaymentMethodCashId = branchList.Id;
            }
        }
        private static async Task CreateBranchMainOffice()
        {
            BranchPM branchMainOfficePM = GetNewBranchMainOffice();
            HttpResponseMessage response = await RestClientService.PostAsync(branchMainOfficePM, "Branches");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            BranchPM branchPM = JsonConvert.DeserializeObject<BranchPM>(stringResult);
            //Assert.IsNotNull(branchPM);
            FullAccountingVariables.BranchMainOfficeId = branchPM.Id;
        }
        private static BranchPM GetNewBranchMainOffice()
        {
            BranchPM branchPM = new BranchPM();
            branchPM.Tenant = IntegrationTestLoginParameters.Tenant;
            branchPM.EnglishName = "Main Office";
            branchPM.LocalName = "Main Office";
            branchPM.SearchFields = "Main Office";
            return branchPM;
        }
        private static async Task GetChargeTypesAirFreight()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ChargesTypeViews/getbyfilters?Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=air%20freight&GetCount=true&PageSize=23");
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            ChargesTypeList chargesTypeList = result != null ? JsonConvert.DeserializeObject<ChargesTypeList>(result) : null;
            if (chargesTypeList == null)
                await CreateChargeTypesAirFreight();
            else
            {
                FullAccountingVariables.ChargeTypesAirFreightId = chargesTypeList.Id;
            }
        }
        private static async Task CreateChargeTypesAirFreight()
        {
            ChargesTypePM chargesTypeAirFreightPM = GetNewChargeTypesAirFreight();
            HttpResponseMessage response = await RestClientService.PostAsync(chargesTypeAirFreightPM, "ChargesTypes");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            ChargesTypePM chargesTypePM = JsonConvert.DeserializeObject<ChargesTypePM>(stringResult);
            //Assert.IsNotNull(chargesTypePM);
            FullAccountingVariables.BranchMainOfficeId = chargesTypePM.Id;
        }
        private static ChargesTypePM GetNewChargeTypesAirFreight()
        {
            ChargesTypePM chargesTypePM = new ChargesTypePM();
            chargesTypePM.Tenant = IntegrationTestLoginParameters.Tenant;
            chargesTypePM.EnglishName = "GE:AirFreight";
            chargesTypePM.LocalName = "GE:AirFreight";
            chargesTypePM.SearchFields = "AFT,GE:AirFreight";
            chargesTypePM.Code = "AFT";
            chargesTypePM.ChargesGroupCode = "COMM";
            chargesTypePM.ChargesGroupId = "1-6036";
            chargesTypePM.MeasurementId = "1-29726";
            chargesTypePM.IsReceivable = true;
            chargesTypePM.IsPayable = true;
            chargesTypePM.IsAir = true;
            chargesTypePM.IsOcean = true;
            chargesTypePM.IsInland = true;
            chargesTypePM.AWBPrintDescription = true;
            chargesTypePM.ViewOrder = 100;
            return chargesTypePM;
        }
        private static async Task GetCustomerTestGlCustomer()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CustomerViews/getbyfilters?GetAll=true&Filter1Name=SearchFields&Filter1Operator=Contains&GetCount=true&PageSize=22&Filter1Value=12PMCS");
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            CustomerList customerTestGlCustomerList = result != null ? JsonConvert.DeserializeObject<CustomerList>(result) : null;
            if (customerTestGlCustomerList == null)
                await CreateCustomerTestGlCustomer();
            else
            {
                FullAccountingVariables.CustomerTestGlCustId = customerTestGlCustomerList.Id;
            }
        }
        private static async Task CreateCustomerTestGlCustomer()
        {
            CustomerPM CustomerTestGlCustomerPM = GetNewCustomerTestGlCustomer();
            HttpResponseMessage response = await RestClientService.PostAsync(CustomerTestGlCustomerPM, "Customers");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            CustomerPM customerPM = JsonConvert.DeserializeObject<CustomerPM>(stringResult);
            //Assert.IsNotNull(customerPM);
            FullAccountingVariables.CustomerTestGlCustId = customerPM.Id;
        }
        private static CustomerPM GetNewCustomerTestGlCustomer()
        {
            CustomerPM customerPM = new CustomerPM();
            customerPM.Tenant = IntegrationTestLoginParameters.Tenant;
            customerPM.EnglishName = "GE:Customer";
            customerPM.LocalName = "GE:Customer";
            customerPM.SearchFields = "12PMCS,GE:Customer";
            customerPM.Code = "12PMCS";
            customerPM.PartnerTypeId = "CS";
            customerPM.CityName = "TEST";
            customerPM.CountryId = FullAccountingVariables.CountryAXId;
            customerPM.CountryCode = "1";
            customerPM.CountryName = "palestine";
            customerPM.IsCustomer = true;
            customerPM.CustomerStatusCode = "ACT";
            return customerPM;
        }
        private static async Task GetVendorTestGlVendor()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("VendorViews/getbyfilters?Filter1Name=SearchFields&Filter1Operator=Contains&GetCount=true&PageSize=22&Filter1Value=1s5PMV2");
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            VendorList VendorTestGlVendorList = result != null ? JsonConvert.DeserializeObject<VendorList>(result) : null;
            if (VendorTestGlVendorList == null)
                await CreateVendorTestGlVend();
            else
            {
                FullAccountingVariables.VendorTestGlVendorId = VendorTestGlVendorList.Id;
            }
        }
        private static async Task CreateVendorTestGlVend()
        {
            VendorPM VendorTestGlVendPM = GetNewVendorTestGlVen();
            HttpResponseMessage response = await RestClientService.PostAsync(VendorTestGlVendPM, "Vendors");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            VendorPM vendorPM = JsonConvert.DeserializeObject<VendorPM>(stringResult);
            //Assert.IsNotNull(vendorPM);
            FullAccountingVariables.VendorTestGlVendorId = vendorPM.Id;
        }
        private static VendorPM GetNewVendorTestGlVen()
        {
            VendorPM vendorPM = new VendorPM();
            vendorPM.Tenant = IntegrationTestLoginParameters.Tenant;
            vendorPM.EnglishName = "GE:Vendor";
            vendorPM.LocalName = "GE:Vendor";
            vendorPM.SearchFields = "1s5PMV2,GE:Vendor";
            vendorPM.Code = "1s5PMV2";
            vendorPM.PartnerTypeId = "VD";
            vendorPM.CityName = "TEST";
            vendorPM.CountryId = FullAccountingVariables.CountryAXId;
            vendorPM.CountryCode = "1";
            vendorPM.CountryName = "palestine";
            return vendorPM;
        }
        private static async Task GetGlAccountCustomer()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("GLAccountViews/getbyfilters?Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=54l4CSPM&PageSize=23");
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            GLAccountList GlAccountCustomerList = result != null ? JsonConvert.DeserializeObject<GLAccountList>(result) : null;
            if (GlAccountCustomerList != null)
                FullAccountingVariables.GLAccountCustomerId = GlAccountCustomerList.Id;
            else
                await GetChartOfAccountCustomer();
        }
        private static async Task GetChartOfAccountCustomer()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ChartOfAccountViews/getbyfilters?Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=15CFC&PageSize=23");
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            ChartOfAccountList chartOfAccountList = result != null ? JsonConvert.DeserializeObject<ChartOfAccountList>(result) : null;
            if (chartOfAccountList == null)
                await CreateChartOfAccountCustomer();
            else
            {
                FullAccountingVariables.ChartOfAccountCustomerId = chartOfAccountList.Id;
                FullAccountingVariables.ChartOfAccountCustomerCode = chartOfAccountList.Code;
                FullAccountingVariables.ChartOfAccountCustomerLocalName = chartOfAccountList.LocalName;
                FullAccountingVariables.ChartOfAccountCustomerEnglishName = chartOfAccountList.EnglishName;
                FullAccountingVariables.ChartOfAccountCustomerParentId = chartOfAccountList.ParentId;
                FullAccountingVariables.ChartOfAccountCustomerTypeCode = chartOfAccountList.TypeCode;
                FullAccountingVariables.ChartOfAccountCustomerInactive = chartOfAccountList.Inactive;
                FullAccountingVariables.ChartOfAccountCustomerTypeName = chartOfAccountList.TypeName;
                FullAccountingVariables.ChartOfAccountCustomerParentName = chartOfAccountList.ParentName;
                FullAccountingVariables.ChartOfAccountCustomerSearchFields = chartOfAccountList.SearchFields;
                await CreateGlAccountCustomer();
            }
        }
        private static async Task CreateChartOfAccountCustomer()
        {
            ChartOfAccountPM ChartOfAccountCustomerPM = GetNewChartOfAccountCustomer();
            HttpResponseMessage response = await RestClientService.PostAsync(ChartOfAccountCustomerPM, "ChartOfAccounts");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            ChartOfAccountPM chartOfAccountPM = JsonConvert.DeserializeObject<ChartOfAccountPM>(stringResult);
            //Assert.IsNotNull(chartOfAccountPM);
            FullAccountingVariables.ChartOfAccountCustomerId = chartOfAccountPM.Id;
            FullAccountingVariables.ChartOfAccountCustomerCode = chartOfAccountPM.Code;
            FullAccountingVariables.ChartOfAccountCustomerLocalName = chartOfAccountPM.LocalName;
            FullAccountingVariables.ChartOfAccountCustomerEnglishName = chartOfAccountPM.EnglishName;
            FullAccountingVariables.ChartOfAccountCustomerParentId = chartOfAccountPM.ParentId;
            FullAccountingVariables.ChartOfAccountCustomerTypeCode = chartOfAccountPM.TypeCode;
            FullAccountingVariables.ChartOfAccountCustomerInactive = chartOfAccountPM.Inactive;
            FullAccountingVariables.ChartOfAccountCustomerTypeName = chartOfAccountPM.TypeName;
            FullAccountingVariables.ChartOfAccountCustomerParentName = chartOfAccountPM.ParentName;
            FullAccountingVariables.ChartOfAccountCustomerSearchFields = chartOfAccountPM.SearchFields;
            await CreateGlAccountCustomer();
        }
        private static ChartOfAccountPM GetNewChartOfAccountCustomer()
        {
            ChartOfAccountPM chartOfAccountPM = new ChartOfAccountPM();
            chartOfAccountPM.Tenant = IntegrationTestLoginParameters.Tenant;
            chartOfAccountPM.EnglishName = "GE:Customer";
            chartOfAccountPM.LocalName = "GE:Customer";
            chartOfAccountPM.SearchFields = "15CFC,GE:Customer";
            chartOfAccountPM.Code = "15CFC";
            chartOfAccountPM.TypeCode = "3";
            chartOfAccountPM.Inactive = true;
            return chartOfAccountPM;
        }
        private static async Task CreateGlAccountCustomer()
        {
            GLAccountPM GlAccountCustomerPM = GetNewGlAccountCustomerPM();
            HttpResponseMessage response = await RestClientService.PostAsync(GlAccountCustomerPM, "GLAccounts");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            GLAccountPM gLAccountPM = JsonConvert.DeserializeObject<GLAccountPM>(stringResult);
            //Assert.IsNotNull(gLAccountPM);
            FullAccountingVariables.GLAccountCustomerId = gLAccountPM.Id;
        }
        private static GLAccountPM GetNewGlAccountCustomerPM()
        {
            GLAccountPM gLAccountPM = new GLAccountPM();
            gLAccountPM.Tenant = IntegrationTestLoginParameters.Tenant;
            gLAccountPM.EnglishName = "GE:Customer";
            gLAccountPM.LocalName = "GE:Customer";
            gLAccountPM.SearchFields = "54l4CSPM,GE:Customer";
            gLAccountPM.AccountTypeCode = "2";
            gLAccountPM.DisplayNumber = "54l4CSPM";
            gLAccountPM.IsMultiCurrency = true;
            gLAccountPM.RevenueExpenseType = "3";
            gLAccountPM.ChartOfAccountsId = FullAccountingVariables.ChartOfAccountCustomerId;
            gLAccountPM.ChartOfAccountsTypeCode = "3";
            gLAccountPM.ReconcileMethodCode = "0";
            gLAccountPM.NewGLAccountCardId = FullAccountingVariables.CustomerTestGlCustId;
            return gLAccountPM;
        }
        private static async Task GetGlAccountVendor()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("GLAccountViews/getbyfilters?Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=458GLPM&PageSize=23");
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            GLAccountList GlAccountVendorList = result != null ? JsonConvert.DeserializeObject<GLAccountList>(result) : null;
            if (GlAccountVendorList != null)
                FullAccountingVariables.GLAccountVendorId = GlAccountVendorList.Id;
            else
                await GetChartOfAccountVendor();
        }
        private static async Task GetChartOfAccountVendor()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ChartOfAccountViews/getbyfilters?Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=1PMCF&PageSize=23");
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            ChartOfAccountList chartOfAccountList = result != null ? JsonConvert.DeserializeObject<ChartOfAccountList>(result) : null;
            if (chartOfAccountList == null)
                await CreateChartOfAccountVendor();
            else
            {
                FullAccountingVariables.ChartOfAccountVendorId = chartOfAccountList.Id;
                FullAccountingVariables.ChartOfAccountVendorCode = chartOfAccountList.Code;
                FullAccountingVariables.ChartOfAccountVendorLocalName = chartOfAccountList.LocalName;
                FullAccountingVariables.ChartOfAccountVendorEnglishName = chartOfAccountList.EnglishName;
                FullAccountingVariables.ChartOfAccountVendorParentId = chartOfAccountList.ParentId;
                FullAccountingVariables.ChartOfAccountVendorTypeCode = chartOfAccountList.TypeCode;
                FullAccountingVariables.ChartOfAccountVendorInactive = chartOfAccountList.Inactive;
                FullAccountingVariables.ChartOfAccountVendorTypeName = chartOfAccountList.TypeName;
                FullAccountingVariables.ChartOfAccountVendorParentName = chartOfAccountList.ParentName;
                FullAccountingVariables.ChartOfAccountVendorSearchFields = chartOfAccountList.SearchFields;
                await CreateGlAccountVendor();
            }
        }
        private static async Task CreateChartOfAccountVendor()
        {
            ChartOfAccountPM ChartOfAccountVendorPM = GetNewChartOfAccountVendor();
            HttpResponseMessage response = await RestClientService.PostAsync(ChartOfAccountVendorPM, "ChartOfAccounts");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            ChartOfAccountPM chartOfAccountPM = JsonConvert.DeserializeObject<ChartOfAccountPM>(stringResult);
            //Assert.IsNotNull(chartOfAccountPM);
            FullAccountingVariables.ChartOfAccountVendorId = chartOfAccountPM.Id;
            FullAccountingVariables.ChartOfAccountVendorCode = chartOfAccountPM.Code;
            FullAccountingVariables.ChartOfAccountVendorLocalName = chartOfAccountPM.LocalName;
            FullAccountingVariables.ChartOfAccountVendorEnglishName = chartOfAccountPM.EnglishName;
            FullAccountingVariables.ChartOfAccountVendorParentId = chartOfAccountPM.ParentId;
            FullAccountingVariables.ChartOfAccountVendorTypeCode = chartOfAccountPM.TypeCode;
            FullAccountingVariables.ChartOfAccountVendorInactive = chartOfAccountPM.Inactive;
            FullAccountingVariables.ChartOfAccountVendorTypeName = chartOfAccountPM.TypeName;
            FullAccountingVariables.ChartOfAccountVendorParentName = chartOfAccountPM.ParentName;
            FullAccountingVariables.ChartOfAccountVendorSearchFields = chartOfAccountPM.SearchFields;
            await CreateGlAccountVendor();
        }
        private static ChartOfAccountPM GetNewChartOfAccountVendor()
        {
            ChartOfAccountPM chartOfAccountPM = new ChartOfAccountPM();
            chartOfAccountPM.Tenant = IntegrationTestLoginParameters.Tenant;
            chartOfAccountPM.EnglishName = "GE:Vendor";
            chartOfAccountPM.LocalName = "GE:Vendor";
            chartOfAccountPM.SearchFields = "1PMCF,GE:Vendor";
            chartOfAccountPM.Code = "1PMCF";
            chartOfAccountPM.TypeCode = "4";
            chartOfAccountPM.Inactive = true;
            return chartOfAccountPM;
        }
        private static async Task CreateGlAccountVendor()
        {
            GLAccountPM GlAccountVendorPM = GetNewGlAccountVendorPM();
            HttpResponseMessage response = await RestClientService.PostAsync(GlAccountVendorPM, "GLAccounts");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            GLAccountPM gLAccountPM = JsonConvert.DeserializeObject<GLAccountPM>(stringResult);
            //Assert.IsNotNull(gLAccountPM);
            FullAccountingVariables.GLAccountVendorId = gLAccountPM.Id;
        }
        private static GLAccountPM GetNewGlAccountVendorPM()
        {
            GLAccountPM gLAccountPM = new GLAccountPM();
            gLAccountPM.Tenant = IntegrationTestLoginParameters.Tenant;
            gLAccountPM.EnglishName = "GE:Vendor";
            gLAccountPM.LocalName = "GE:Vendor";
            gLAccountPM.SearchFields = "458GLPML,GE:Vendor";
            gLAccountPM.AccountTypeCode = "3";
            gLAccountPM.DisplayNumber = "458GLPML";
            gLAccountPM.IsMultiCurrency = true;
            gLAccountPM.RevenueExpenseType = "3";
            gLAccountPM.ChartOfAccountsId = FullAccountingVariables.ChartOfAccountVendorId;
            gLAccountPM.ChartOfAccountsTypeCode = "4";
            gLAccountPM.ReconcileMethodCode = "0";
            gLAccountPM.NewGLAccountCardId = FullAccountingVariables.VendorTestGlVendorId;
            return gLAccountPM;
        }
        public static async Task  GetAddressCustomer()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CardViews/GetSingle?id=" + FullAccountingVariables.CustomerTestGlCustId);
            var stringResult = response.Content.ReadAsStringAsync().Result;
            CardList cardList = JsonConvert.DeserializeObject<CardList>(stringResult);
            //Assert.IsNotNull(cardList);
            FullAccountingVariables.AddressCustomerId = cardList.MainAddressId;

        }
        public static async Task GetAddressVendor()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CardViews/GetSingle?id=" + FullAccountingVariables.VendorTestGlVendorId);
            var stringResult = response.Content.ReadAsStringAsync().Result;
            CardList cardList = JsonConvert.DeserializeObject<CardList>(stringResult);
            //Assert.IsNotNull(cardList);
            FullAccountingVariables.AddressVendorId = cardList.MainAddressId;

        }
        private static async Task GetChartOfAccountBank()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ChartOfAccountViews/getbyfilters?Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=BK771&PageSize=23");
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            ChartOfAccountList chartOfAccountList = result != null ? JsonConvert.DeserializeObject<ChartOfAccountList>(result) : null;
            if (chartOfAccountList == null)
                await CreateChartOfAccountBank();
            else
            {
                FullAccountingVariables.ChartOfAccountBankId = chartOfAccountList.Id;
            }
        }
        private static async Task CreateChartOfAccountBank()
        {
            ChartOfAccountPM ChartOfAccountBankPM = GetNewChartOfAccountBank();
            HttpResponseMessage response = await RestClientService.PostAsync(ChartOfAccountBankPM, "ChartOfAccounts");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            ChartOfAccountPM chartOfAccountPM = JsonConvert.DeserializeObject<ChartOfAccountPM>(stringResult);
            //Assert.IsNotNull(chartOfAccountPM);
            FullAccountingVariables.ChartOfAccountBankId = chartOfAccountPM.Id;
        }
        private static ChartOfAccountPM GetNewChartOfAccountBank()
        {
            ChartOfAccountPM chartOfAccountPM = new ChartOfAccountPM();
            chartOfAccountPM.Tenant = IntegrationTestLoginParameters.Tenant;
            chartOfAccountPM.EnglishName = "GE:Bank";
            chartOfAccountPM.LocalName = "GE:Bank";
            chartOfAccountPM.SearchFields = "BK771,GE:Bank";
            chartOfAccountPM.Code = "BK771";
            chartOfAccountPM.TypeCode = "5";
            chartOfAccountPM.Inactive = true;
            return chartOfAccountPM;
        }
        private static async Task CreateGlAccountBank()
        {
            GLAccountPM GlAccountBankPM = GetNewGlAccountBankPM();
            HttpResponseMessage response = await RestClientService.PostAsync(GlAccountBankPM, "GLAccounts");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            GLAccountPM gLAccountPM = JsonConvert.DeserializeObject<GLAccountPM>(stringResult);
            //Assert.IsNotNull(gLAccountPM);
            FullAccountingVariables.GLAccountBankId = gLAccountPM.Id;
        }
        private static GLAccountPM GetNewGlAccountBankPM()
        {
            GLAccountPM gLAccountPM = new GLAccountPM();
            gLAccountPM.Tenant = IntegrationTestLoginParameters.Tenant;
            gLAccountPM.EnglishName = "GE:Bank";
            gLAccountPM.LocalName = "GE:Bank";
            gLAccountPM.SearchFields = "1414BKPM,GE:Bank";
            gLAccountPM.AccountTypeCode = "1";
            gLAccountPM.DisplayNumber = "1414BKPM";
            gLAccountPM.IsMultiCurrency = true;
            gLAccountPM.RevenueExpenseType = "3";
            gLAccountPM.ChartOfAccountsId = FullAccountingVariables.ChartOfAccountBankId;
            gLAccountPM.ChartOfAccountsTypeCode = "5";
            gLAccountPM.ReconcileMethodCode = "0";
            gLAccountPM.CurrencyId = FullAccountingVariables.AccountingCurrencyId;
            return gLAccountPM;
        }
        private static async Task CreateBankCodeBK14()
        {
            BankCodePM BankCodeBK14PM = GetNewBankCodeBK14PM();
            HttpResponseMessage response = await RestClientService.PostAsync(BankCodeBK14PM, "BankCodes");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            BankCodePM bankCodePM = JsonConvert.DeserializeObject<BankCodePM>(stringResult);
            //Assert.IsNotNull(bankCodePM);
            FullAccountingVariables.BankCodeBK14Id = bankCodePM.Id;
            FullAccountingVariables.BankCodeBK14Code = bankCodePM.Code;

        }
        private static BankCodePM GetNewBankCodeBK14PM()
        {
            BankCodePM bankCodePM = new BankCodePM();
            bankCodePM.Tenant = IntegrationTestLoginParameters.Tenant;
            bankCodePM.EnglishName = "GE:Bank14";
            bankCodePM.LocalName = "GE:Bank14";
            bankCodePM.SearchFields = "BK14,GE:Bank14";
            bankCodePM.Code = "BK14";
            return bankCodePM;
        }
        private static async Task CreateBranchCashBookBK14()
        {
            BranchPM BranchBK14PM = GetNewBranchCashBookBK14();
            HttpResponseMessage response = await RestClientService.PostAsync(BranchBK14PM, "Branches");
            var stringResult = response.Content.ReadAsStringAsync().Result;
            BranchPM branchPM = JsonConvert.DeserializeObject<BranchPM>(stringResult);
            //Assert.IsNotNull(branchPM);
            FullAccountingVariables.BranchCashBookBK14Id = branchPM.Id;
 
        }
        private static BranchPM GetNewBranchCashBookBK14()
        {
            BranchPM BranchPM = new BranchPM();
            BranchPM.Tenant = IntegrationTestLoginParameters.Tenant;
            BranchPM.EnglishName = "GE:Bank14";
            BranchPM.LocalName = "GE:Bank14";
            BranchPM.SearchFields = "BK14,GE:Bank14";
            BranchPM.Code = "BK14";
            return BranchPM;
        }


    }
}
