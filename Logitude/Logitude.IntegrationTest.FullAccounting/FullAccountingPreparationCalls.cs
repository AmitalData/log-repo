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
            //await GetFullAccountingSettingsTenant();
            await GetAccounntingPeriods();
            await OpenCurrentMonth();
            await GetAccountingCurriencyTenant();
            await GetCountryAX();
            await GetVatTypeExempt();
            await GetPaymentTermCash();
            await GetPaymnetMethodCash();
            await GetBranchMainOffice();
            await GetChargeTypesAirFreight();
            await GetCustomerTestGlCustomer12PMCS();
            await GetVendorTestGlVendor1s5PMV2();
            await GetChartOfAccountVendor1PMCF();
            await GetGlAccountCustomer54l4CSPM();
            await GetGlAccountVendor458GLPM();
            await GetAddressCustomer12PMCS();
            await GetAddressVendor1s5PMV2();
            await GetChartOfAccountBankBK771();
            await CreateGlAccountBank1414BKPM();
            await CreateBranchCashBookBK14();
        }

        private static async Task GetFullAccountingSettingsTenant()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("FullAccountingSettings/GetSingle?id=" + IntegrationTestLoginParameters.Tenant);
            FullAccountingSettingPM fullAccountingSettingPM = RestClientService.ParseResponse<FullAccountingSettingPM>(response);
            FullAccountingVariables.VendorControlAccountTenantId = fullAccountingSettingPM.VendorControlAccountId;
            FullAccountingVariables.AccountingActivatedTenant = fullAccountingSettingPM.AccountingActivated;
        }
        private static async Task GetAccounntingPeriods()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("AccountingPeriodViews"+ QueryFiltersPreparation.GetUrlParameters());
            AccountingPeriodList accountingPeriodList = RestClientService.ParseResponse<AccountingPeriodList>(response);
            FullAccountingVariables.AcocuntingPeriodsId = accountingPeriodList.Id;
            FullAccountingVariables.AcocuntingPeriodsTenant = accountingPeriodList.Tenant;
            FullAccountingVariables.AcocuntingPeriodsYear = accountingPeriodList.Year;
            FullAccountingVariables.AcocuntingPeriodsPeriodTypeCode = accountingPeriodList.PeriodTypeCode;
            FullAccountingVariables.AcocuntingPeriodsPeriodTypeName = accountingPeriodList.PeriodTypeName;
            FullAccountingVariables.AcocuntingPeriodsClosedMonth = accountingPeriodList.ClosedMonth;
        }
        private static async Task OpenCurrentMonth()
        {
            AccountingPeriodPM accountingPeriodPM = await GetSingleAccountingPeriods();
            accountingPeriodPM.PeriodTypeName = "Accounting";
            accountingPeriodPM.PeriodTypeCode = "1";
            HttpResponseMessage response = await RestClientService.PutAsync(accountingPeriodPM, "AccountingPeriods");
            AccountingPeriodPM PutAccountingPeriodPM = RestClientService.ParseResponse<AccountingPeriodPM>(response); 
        }
        public static async Task<AccountingPeriodPM> GetSingleAccountingPeriods()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("AccountingPeriods/GetSingle?id=" + FullAccountingVariables.AcocuntingPeriodsId);
            AccountingPeriodPM accountingPeriodPM = RestClientService.ParseResponse<AccountingPeriodPM>(response);
            return accountingPeriodPM;
        }
        private static async Task GetAccountingCurriencyTenant()
        {
            TenantPM tenantPM=null;
            if (IntegrationTestLoginParameters.TenantPM == null)
            {
                HttpResponseMessage response = await RestClientService.GetAsync("Tenants/GetSingle?id=" + IntegrationTestLoginParameters.Tenant);
                tenantPM = RestClientService.ParseResponse<TenantPM>(response);
            }
            else
            {
                tenantPM = IntegrationTestLoginParameters.TenantPM;
            }
            FullAccountingVariables.AccountingCurrencyTenantId = tenantPM.CurrencyId;
         }
        private static async Task GetCountryAX()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CountryViews" + QueryFiltersPreparation.GetUrlParameters("AX"));
           
             CountryList countryList = RestClientService.ParseResponse<CountryList>(response);
            FullAccountingVariables.CountryAXId = countryList.Id;
        }
        private static async Task GetVatTypeExempt()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("VatTypeViews" +QueryFiltersPreparation.GetUrlParameters("Exempt"));
            VatTypeList vatTypeList = RestClientService.ParseResponse<VatTypeList>(response);
            if (vatTypeList==null)
              await  CreateVatTypeExempt();
            else
            {
                FullAccountingVariables.VatEXEMPTId = vatTypeList.Id;
            }
        }
        private static async Task CreateVatTypeExempt()
        {
            VatTypePM vatTypeExemptPM = GetNewVatTypeExemptPM();
            HttpResponseMessage response = await RestClientService.PostAsync(vatTypeExemptPM,"VatType");
            VatTypePM vatTypePM = RestClientService.ParseResponse<VatTypePM>(response);
            FullAccountingVariables.VatEXEMPTId = vatTypePM.Id;
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
            HttpResponseMessage response = await RestClientService.GetAsync("PaymentTermViews"+QueryFiltersPreparation.GetUrlParameters("cash"));
            PaymentTermList paymentTermList = RestClientService.ParseResponse<PaymentTermList>(response);
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
            PaymentTermPM paymentTermPM = RestClientService.ParseResponse<PaymentTermPM>(response);
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
            HttpResponseMessage response = await RestClientService.GetAsync("AccountingPaymentMethodViews"+ QueryFiltersPreparation.GetUrlParameters("Cash"));
            AccountingPaymentMethodList accountingPaymentMethodList = RestClientService.ParseResponse<AccountingPaymentMethodList>(response);
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
            AccountingPaymentMethodPM accountingPaymentMethodPM = RestClientService.ParseResponse<AccountingPaymentMethodPM>(response);
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
            HttpResponseMessage response = await RestClientService.GetAsync("BranchViews" + QueryFiltersPreparation.GetUrlParameters("Main Office"));
            BranchList branchList = RestClientService.ParseResponse<BranchList>(response);
            if (branchList == null)
                await CreateBranchMainOffice();
            else
            {
                FullAccountingVariables.BranchMainOfficeId = branchList.Id;
            }
        }
        private static async Task CreateBranchMainOffice()
        {
            BranchPM branchMainOfficePM = GetNewBranchMainOffice();
            HttpResponseMessage response = await RestClientService.PostAsync(branchMainOfficePM, "Branches");
            BranchPM branchPM = RestClientService.ParseResponse<BranchPM>(response);
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
            HttpResponseMessage response = await RestClientService.GetAsync("ChargesTypeViews" + QueryFiltersPreparation.GetUrlParameters("AFT"));
            ChargesTypeList chargesTypeList = RestClientService.ParseResponse<ChargesTypeList>(response);
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
            ChargesTypePM chargesTypePM = RestClientService.ParseResponse<ChargesTypePM>(response);
            FullAccountingVariables.ChargeTypesAirFreightId = chargesTypePM.Id;
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
        private static async Task GetCustomerTestGlCustomer12PMCS()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CustomerViews"+ QueryFiltersPreparation.GetUrlParameters("12PMCS"));
            CustomerList customerTestGlCustomerList = RestClientService.ParseResponse<CustomerList>(response);
            if (customerTestGlCustomerList == null)
                await CreateCustomerTestGlCustomer12PMCS();
            else
            {
                FullAccountingVariables.CustomerTestGlCust12PMCSId = customerTestGlCustomerList.Id;
            }
        }
        private static async Task CreateCustomerTestGlCustomer12PMCS()
        {
            CustomerPM CustomerTestGlCustomerPM = GetNewCustomerTestGlCustomer12PMCS();
            HttpResponseMessage response = await RestClientService.PostAsync(CustomerTestGlCustomerPM, "Customers");
            CustomerPM customerPM = RestClientService.ParseResponse<CustomerPM>(response);
            FullAccountingVariables.CustomerTestGlCust12PMCSId = customerPM.Id;
        }
        private static CustomerPM GetNewCustomerTestGlCustomer12PMCS()
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
        private static async Task GetVendorTestGlVendor1s5PMV2()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("VendorViews"+QueryFiltersPreparation.GetUrlParameters("1s5PMV2"));
            VendorList VendorTestGlVendorList = RestClientService.ParseResponse<VendorList>(response);
            if (VendorTestGlVendorList == null)
                await CreateVendorTestGlVend1s5PMV2();
            else
            {
                FullAccountingVariables.VendorTestGlVendor1s5PMV2Id = VendorTestGlVendorList.Id;
            }
        }
        private static async Task CreateVendorTestGlVend1s5PMV2()
        {
            VendorPM VendorTestGlVendPM = GetNewVendorTestGlVen1s5PMV2();
            HttpResponseMessage response = await RestClientService.PostAsync(VendorTestGlVendPM, "Vendors");
            VendorPM vendorPM = RestClientService.ParseResponse<VendorPM>(response);
            FullAccountingVariables.VendorTestGlVendor1s5PMV2Id = vendorPM.Id;
        }
        private static VendorPM GetNewVendorTestGlVen1s5PMV2()
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
        private static async Task GetGlAccountCustomer54l4CSPM()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("GLAccountViews" + QueryFiltersPreparation.GetUrlParameters("54l4CSPM"));
            GLAccountList GlAccountCustomerList = RestClientService.ParseResponse<GLAccountList>(response);
            if (GlAccountCustomerList != null)
                FullAccountingVariables.GLAccountCustomer54l4CSPMId = GlAccountCustomerList.Id;
            else
                await GetChartOfAccountCustomer15CFCPM();
        }
        private static async Task GetChartOfAccountCustomer15CFCPM()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ChartOfAccountViews"+QueryFiltersPreparation.GetUrlParameters("15CFC"));
            ChartOfAccountList chartOfAccountList = RestClientService.ParseResponse<ChartOfAccountList>(response);
            if (chartOfAccountList == null)
                await CreateChartOfAccountCustomer15CFC();
            else
            {
                FullAccountingVariables.ChartOfAccountCustomer15CFCId = chartOfAccountList.Id;
                FullAccountingVariables.ChartOfAccountCustomer15CFCCode = chartOfAccountList.Code;
                await CreateGlAccountCustomer54l4CSPM();
            }
        }
        private static async Task CreateChartOfAccountCustomer15CFC()
        {
            ChartOfAccountPM ChartOfAccountCustomerPM = GetNewChartOfAccountCustomer15CFC();
            HttpResponseMessage response = await RestClientService.PostAsync(ChartOfAccountCustomerPM, "ChartOfAccounts");
            ChartOfAccountPM chartOfAccountPM = RestClientService.ParseResponse<ChartOfAccountPM>(response);
            FullAccountingVariables.ChartOfAccountCustomer15CFCId = chartOfAccountPM.Id;
            FullAccountingVariables.ChartOfAccountCustomer15CFCCode = chartOfAccountPM.Code;
            await CreateGlAccountCustomer54l4CSPM();
        }
        private static ChartOfAccountPM GetNewChartOfAccountCustomer15CFC()
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
        private static async Task CreateGlAccountCustomer54l4CSPM()
        {
            GLAccountPM GlAccountCustomerPM = GetNewGlAccountCustomer54l4CSPM();
            HttpResponseMessage response = await RestClientService.PostAsync(GlAccountCustomerPM, "GLAccounts");
            GLAccountPM gLAccountPM = RestClientService.ParseResponse<GLAccountPM>(response);
            FullAccountingVariables.GLAccountCustomer54l4CSPMId = gLAccountPM.Id;
        }
        private static GLAccountPM GetNewGlAccountCustomer54l4CSPM()
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
            gLAccountPM.ChartOfAccountsId = FullAccountingVariables.ChartOfAccountCustomer15CFCId;
            gLAccountPM.ChartOfAccountsTypeCode = "3";
            gLAccountPM.ReconcileMethodCode = "0";
            gLAccountPM.NewGLAccountCardId = FullAccountingVariables.CustomerTestGlCust12PMCSId;
            return gLAccountPM;
        }
        private static async Task GetGlAccountVendor458GLPM()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("GLAccountViews" + QueryFiltersPreparation.GetUrlParameters("458GLPM"));
            GLAccountList GlAccountVendorList = RestClientService.ParseResponse<GLAccountList>(response);
            if (GlAccountVendorList != null)
                FullAccountingVariables.GLAccountVendor458GLPMId = GlAccountVendorList.Id;
            else
                await CreateGlAccountVendor458GLPML();
        }
        private static async Task GetChartOfAccountVendor1PMCF()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ChartOfAccountViews" + QueryFiltersPreparation.GetUrlParameters("1PMCF"));
            ChartOfAccountList chartOfAccountList = RestClientService.ParseResponse<ChartOfAccountList>(response);
            if (chartOfAccountList == null)
                await CreateChartOfAccountVendor1PMCF();
            else
            {
                FullAccountingVariables.ChartOfAccountVendor1PMCFId = chartOfAccountList.Id;
                FullAccountingVariables.ChartOfAccountVendor1PMCFCode = chartOfAccountList.Code;
            }
        }
        private static async Task CreateChartOfAccountVendor1PMCF()
        {
            ChartOfAccountPM ChartOfAccountVendorPM = GetNewChartOfAccountVendor1PMCF();
            HttpResponseMessage response = await RestClientService.PostAsync(ChartOfAccountVendorPM, "ChartOfAccounts");
            ChartOfAccountPM chartOfAccountPM = RestClientService.ParseResponse<ChartOfAccountPM>(response);
            FullAccountingVariables.ChartOfAccountVendor1PMCFId = chartOfAccountPM.Id;
            FullAccountingVariables.ChartOfAccountVendor1PMCFCode = chartOfAccountPM.Code;
        }
        private static ChartOfAccountPM GetNewChartOfAccountVendor1PMCF()
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
        private static async Task CreateGlAccountVendor458GLPML()
        {
            GLAccountPM GlAccountVendorPM = GetNewGlAccountVendorPM458GLPML();
            HttpResponseMessage response = await RestClientService.PostAsync(GlAccountVendorPM, "GLAccounts");
            GLAccountPM gLAccountPM = RestClientService.ParseResponse<GLAccountPM>(response);
            FullAccountingVariables.GLAccountVendor458GLPMId = gLAccountPM.Id;
        }
        private static GLAccountPM GetNewGlAccountVendorPM458GLPML()
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
            gLAccountPM.ChartOfAccountsId = FullAccountingVariables.ChartOfAccountVendor1PMCFId;
            gLAccountPM.ChartOfAccountsTypeCode = "4";
            gLAccountPM.ReconcileMethodCode = "0";
            gLAccountPM.NewGLAccountCardId = FullAccountingVariables.VendorTestGlVendor1s5PMV2Id;
            return gLAccountPM;
        }
        public static async Task GetAddressCustomer12PMCS()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CardViews/GetSingle?id=" + FullAccountingVariables.CustomerTestGlCust12PMCSId);
            CardList cardList = RestClientService.ParseResponse<CardList>(response);
            FullAccountingVariables.AddressCustomer12PMCS = cardList.MainAddressId;

        }
        public static async Task GetAddressVendor1s5PMV2()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CardViews/GetSingle?id=" + FullAccountingVariables.VendorTestGlVendor1s5PMV2Id);
            CardList cardList = RestClientService.ParseResponse<CardList>(response);
            FullAccountingVariables.AddressVendor1s5PMV2Id = cardList.MainAddressId;

        }
        private static async Task GetChartOfAccountBankBK771()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ChartOfAccountViews" + QueryFiltersPreparation.GetUrlParameters("BK771"));
            ChartOfAccountList chartOfAccountList = RestClientService.ParseResponse<ChartOfAccountList>(response);
            if (chartOfAccountList == null)
                await CreateChartOfAccountBankBK771();
            else
            {
                FullAccountingVariables.ChartOfAccountBankBK771Id = chartOfAccountList.Id;
            }
        }
        private static async Task CreateChartOfAccountBankBK771()
        {
            ChartOfAccountPM ChartOfAccountBankPM = GetNewChartOfAccountBankBK771();
            HttpResponseMessage response = await RestClientService.PostAsync(ChartOfAccountBankPM, "ChartOfAccounts");
            ChartOfAccountPM chartOfAccountPM = RestClientService.ParseResponse<ChartOfAccountPM>(response);
            FullAccountingVariables.ChartOfAccountBankBK771Id = chartOfAccountPM.Id;
        }
        private static ChartOfAccountPM GetNewChartOfAccountBankBK771()
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
        private static async Task CreateGlAccountBank1414BKPM()
        {
            GLAccountPM GlAccountBankPM = GetNewGlAccountBankPM1414BKPM();
            HttpResponseMessage response = await RestClientService.PostAsync(GlAccountBankPM, "GLAccounts");
            GLAccountPM gLAccountPM = RestClientService.ParseResponse<GLAccountPM>(response);
            FullAccountingVariables.GLAccountBank1414BKPMId = gLAccountPM.Id;
        }
        private static GLAccountPM GetNewGlAccountBankPM1414BKPM()
        {
            GLAccountPM gLAccountPM = new GLAccountPM();
            gLAccountPM.Tenant = IntegrationTestLoginParameters.Tenant;
            gLAccountPM.EnglishName = "GE:Bank";
            gLAccountPM.LocalName = "GE:Bank";
            gLAccountPM.SearchFields = "1414BKPM,GE:Bank";
            gLAccountPM.AccountTypeCode = "1";
            gLAccountPM.DisplayNumber = "1414BKPM";
            gLAccountPM.RevenueExpenseType = "3";
            gLAccountPM.ChartOfAccountsId = FullAccountingVariables.ChartOfAccountBankBK771Id;
            gLAccountPM.ChartOfAccountsTypeCode = "5";
            gLAccountPM.ReconcileMethodCode = "0";
            gLAccountPM.CurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            return gLAccountPM;
        }
        private static async Task GetBankCodeBK14()
        {
            BankCodePM BankCodeBK14PM = GetNewBankCodeBK14PM();
            HttpResponseMessage response = await RestClientService.PostAsync(BankCodeBK14PM, "BankCodes");
            BankCodePM bankCodePM = RestClientService.ParseResponse<BankCodePM>(response);
            if (bankCodePM != null)
            {
                FullAccountingVariables.BankCodeBK14Id = bankCodePM.Id;
                FullAccountingVariables.BankCodeBK14Code = bankCodePM.Code;
            }
            else
            {
                await CreateBankCodeBK14();
            }

        }
        private static async Task CreateBankCodeBK14()
        {
            BankCodePM BankCodeBK14PM = GetNewBankCodeBK14PM();
            HttpResponseMessage response = await RestClientService.PostAsync(BankCodeBK14PM, "BankCodes");
            BankCodePM bankCodePM = RestClientService.ParseResponse<BankCodePM>(response);
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
            BranchPM branchPM = RestClientService.ParseResponse<BranchPM>(response);
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
