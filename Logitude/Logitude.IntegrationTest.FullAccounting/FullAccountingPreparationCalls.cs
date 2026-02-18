using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.FullAccounting
{
    public class FullAccountingPreparationCalls
    {
        public static async Task PrepareVariables()
        {
            await GetFullAccountingSettingsTenant();
            await GetAccounntingPeriods("1");
            await GetAccounntingPeriods("2");
            await GetAccounntingPeriods("3");
            await OpenCurrentMonth();
            await GetAccountingCurriencyTenant();
            await GetCountryAX();
            await GetVatTypeExempt();
            await GetPaymentTermCash();
            await GetPaymnetMethodCash();
            await GetBranchMainOffice();
            await GetChargeTypesAirFreight();
            await GetChartOfAccountVendor1PMCF();
            await GetGlAccountCustomer54l4CSPM();
            await GetGlAccountVendor458GLPM();
            await GetChartOfAccountBankBK771();
            await CreateGlAccountBank1414BKPM();
            await CreateBranchCashBookBK14();
            await GetCustomerTestGlCustomer12PMCS();
            await GetVendorTestGlVendor1s5PMV2();
            await GetAddressCustomer12PMCS();
            await GetAddressVendor1s5PMV2();
            await GetJournalPrepration();
            await GetARInvoice12PMCS();
            await GetAPInvoice1205();
            await GetCashBook1421Test();
            await GetARPayment12PMCS();
        }

        private static async Task GetFullAccountingSettingsTenant()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("FullAccountingSettings/GetSingle?id=" + IntegrationTestLoginParameters.Tenant);
            FullAccountingSettingPM fullAccountingSettingPM = RestClientService.ParseResponse<FullAccountingSettingPM>(response);
            FullAccountingVariables.VendorControlAccountTenantId = fullAccountingSettingPM.VendorControlAccountId;
            FullAccountingVariables.AccountingActivatedTenant = fullAccountingSettingPM.AccountingActivated;
        }
        private static async Task GetAccounntingPeriods(string PeriodTypeCode)
        {
            ApiQueryFilters filters = new ApiQueryFilters();
            filters.Filter1Name = "PeriodTypeCode";
            filters.Filter2Name = "Year";
            filters.Filter1Operator = "Equal";
            filters.PageSize = 23;
            filters.Filter1Value = PeriodTypeCode;
            filters.Filter2Value = DateTime.Now.Year + "";

            HttpResponseMessage response = await RestClientService.GetAsync("AccountingPeriodViews"+ QueryFiltersPreparation.GetUrlParameters(DateTime.Now.Year + "", filters));
            AccountingPeriodList accountingPeriodList = RestClientService.ParseResponse<AccountingPeriodList>(response);
            if (accountingPeriodList == null)
                await CreateaccountingPeriod(PeriodTypeCode);
            else
            {
                FullAccountingVariables.AcocuntingPeriodsId = accountingPeriodList.Id;
                FullAccountingVariables.AcocuntingPeriodsTenant = accountingPeriodList.Tenant;
                FullAccountingVariables.AcocuntingPeriodsYear = accountingPeriodList.Year;
                FullAccountingVariables.AcocuntingPeriodsPeriodTypeCode = accountingPeriodList.PeriodTypeCode;
                FullAccountingVariables.AcocuntingPeriodsPeriodTypeName = accountingPeriodList.PeriodTypeName;
                FullAccountingVariables.AcocuntingPeriodsClosedMonth = accountingPeriodList.ClosedMonth;

                if (accountingPeriodList.OpenMonth < DateTime.Now.Month || accountingPeriodList.ClosedMonth >= DateTime.Now.Month   )
                {
                   await UpdateAccounntingPeriodsOpenMonth( new AccountingPeriodPM()
                    {
                        Id = accountingPeriodList.Id,
                        ClosedMonth = null,
                        OpenMonth = DateTime.Now.Month,
                        PeriodTypeCode = accountingPeriodList.PeriodTypeCode,
                        Tenant = accountingPeriodList.Tenant,
                        Year = accountingPeriodList.Year,

                    }); 
                }
            }
          
        }
        private static async Task UpdateAccounntingPeriodsOpenMonth(AccountingPeriodPM accountingPeriodPM)
        {
            HttpResponseMessage response = await RestClientService.PutAsync(accountingPeriodPM, "AccountingPeriods");
            AccountingPeriodPM AccountingPeriodPM = RestClientService.ParseResponse<AccountingPeriodPM>(response);
            FullAccountingVariables.AcocuntingPeriodsId = AccountingPeriodPM.Id;
            FullAccountingVariables.AcocuntingPeriodsTenant = AccountingPeriodPM.Tenant;
            FullAccountingVariables.AcocuntingPeriodsYear = AccountingPeriodPM.Year;
            FullAccountingVariables.AcocuntingPeriodsPeriodTypeCode = AccountingPeriodPM.PeriodTypeCode;
            FullAccountingVariables.AcocuntingPeriodsPeriodTypeName = AccountingPeriodPM.PeriodTypeName;
            FullAccountingVariables.AcocuntingPeriodsClosedMonth = AccountingPeriodPM.ClosedMonth;
        }

        private static async Task CreateaccountingPeriod(string PeriodTypeCode)
        {
            AccountingPeriodPM accountingPeriodPM = GetNewAccountingPeriodPM(PeriodTypeCode);
            HttpResponseMessage response = await RestClientService.PostAsync(accountingPeriodPM, "AccountingPeriods");
            AccountingPeriodPM AccountingPeriodPM = RestClientService.ParseResponse<AccountingPeriodPM>(response);
            FullAccountingVariables.AcocuntingPeriodsId = AccountingPeriodPM.Id;
            FullAccountingVariables.AcocuntingPeriodsTenant = AccountingPeriodPM.Tenant;
            FullAccountingVariables.AcocuntingPeriodsYear = AccountingPeriodPM.Year;
            FullAccountingVariables.AcocuntingPeriodsPeriodTypeCode = AccountingPeriodPM.PeriodTypeCode;
            FullAccountingVariables.AcocuntingPeriodsPeriodTypeName = AccountingPeriodPM.PeriodTypeName;
            FullAccountingVariables.AcocuntingPeriodsClosedMonth = AccountingPeriodPM.ClosedMonth;
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

        public static async Task<CashBookPM> GetSingleCashBook1421Test()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CashBooks/GetSingle?id=" + FullAccountingVariables.CashBook1421TestId);
            CashBookPM CashBookPM = RestClientService.ParseResponse<CashBookPM>(response);
            return CashBookPM;
        }
        public static async Task<ARPaymentPM> GetSingleARPayment12PMCS()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ARPayments/GetSingle?id=" + FullAccountingVariables.ARPayment12PMCSId);
            ARPaymentPM ARPaymentPM = RestClientService.ParseResponse<ARPaymentPM>(response);
            return ARPaymentPM;
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

        private static async Task GetCashBook1421Test()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CashBookViews" + QueryFiltersPreparation.GetUrlParameters("CashBook1421"));
            CashBookList CashBookList = RestClientService.ParseResponse<CashBookList>(response);
            if (CashBookList == null)
                await CreateCashBook1421();
            else
            {
                FullAccountingVariables.CashBook1421TestId = CashBookList.Id;
                await UpdateCashBook1421();
            }
        }

        private static async Task GetARInvoice12PMCS()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ARInvoiceViews" + QueryFiltersPreparation.GetUrlParameters("GE:Customer"));
            ARInvoiceList aRInvoice = RestClientService.ParseResponse<ARInvoiceList>(response);
            if (aRInvoice == null)
                await CreateARInvoice12PMCS();
            else
            {
                FullAccountingVariables.InvoiceNumber12PMCSId = aRInvoice.Id;
                FullAccountingVariables.InvoiceNumber12PMCSNumber = aRInvoice.InvoiceNumber;
            }
        }

        private static async Task GetARPayment12PMCS()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ARPaymentViews" + QueryFiltersPreparation.GetUrlParameters("GE:Customer"));
            ARPaymentList ARPaymentList = RestClientService.ParseResponse<ARPaymentList>(response);
            if (ARPaymentList == null)
                await CreateARPayment12PMCS();
            else
            {
                FullAccountingVariables.ARPayment12PMCSId = ARPaymentList.Id;
                FullAccountingVariables.ARPayment12PMCSNumber = ARPaymentList.PaymentNo;
                //await UpdateARPayment12PMCS();
            }
        }
 

        private static async Task GetAPInvoice1205()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("APInvoiceViews" + QueryFiltersPreparation.GetUrlParameters("GE:Vendor"));
            APInvoiceList aPInvoice = RestClientService.ParseResponse<APInvoiceList>(response);
            if (aPInvoice == null)
                await CreateAPInvoice1205();
            else
            {
                FullAccountingVariables.APInvoice1205Id = aPInvoice.Id;
                FullAccountingVariables.APInvoice1205Number = aPInvoice.InvoiceNumber;
            }
        }
        private static async Task CreateVatTypeExempt()
        {
            VatTypePM vatTypeExemptPM = GetNewVatTypeExemptPM();
            HttpResponseMessage response = await RestClientService.PostAsync(vatTypeExemptPM,"VatTypes");
            VatTypePM vatTypePM = RestClientService.ParseResponse<VatTypePM>(response);
            FullAccountingVariables.VatEXEMPTId = vatTypePM.Id;
        }
        private static async Task CreateCashBook1421()
        {
            CashBookPM CashBookPM = GetNewCashBook1421PM();
            HttpResponseMessage response = await RestClientService.PostAsync(CashBookPM, "CashBooks");
            CashBookPM cashBookPM = RestClientService.ParseResponse<CashBookPM>(response);
            FullAccountingVariables.CashBook1421TestId = cashBookPM.Id;
        }
        private static async Task UpdateCashBook1421()
        {
            CashBookPM CashBookPM = await GetSingleCashBook1421Test();
            FullAccountingVariables.BranchCashBookBK14Id = CashBookPM.BranchId;
            CashBookPM.TotalAmount = 50000;
            HttpResponseMessage response = await RestClientService.PutAsync(CashBookPM, "CashBooks");
            CashBookPM cashBookPM = RestClientService.ParseResponse<CashBookPM>(response);
            FullAccountingVariables.CashBook1421TestId = cashBookPM.Id;
        }
        private static async Task UpdateARPayment12PMCS()
        {
            ARPaymentPM ARPaymentPM = await GetSingleARPayment12PMCS();
            ARPaymentPM.BranchId = FullAccountingVariables.BranchCashBookBK14Id;
            HttpResponseMessage response = await RestClientService.PutAsync(ARPaymentPM, "ARPayments");
            ARPaymentPM aRPaymentPM = RestClientService.ParseResponse<ARPaymentPM>(response);
            FullAccountingVariables.ARPayment12PMCSId = aRPaymentPM.Id;
        }
        private static CashBookPM GetNewCashBook1421PM()
        {
            CashBookPM CashBookPM = new CashBookPM();
            CashBookPM.Tenant = IntegrationTestLoginParameters.Tenant;
            CashBookPM.EnglishName = "CashBook1421";
            CashBookPM.LocalName = "CashBook1421";
            CashBookPM.CurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            CashBookPM.CashBookTypeCode = "1";
            CashBookPM.BranchId = FullAccountingVariables.BranchCashBookBK14Id;
            CashBookPM.AccountId = FullAccountingVariables.GLAccountBank1414BKPMId;
            CashBookPM.TotalAmount = 50000;
            CashBookPM.UpdateDate = DateTime.UtcNow;
            CashBookPM.UpdatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            CashBookPM.CreateDate = DateTime.UtcNow;
            CashBookPM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;

            return CashBookPM;
        }
        
        private static AddressPM GetNewAddressPM()
        {
            AddressPM AddressPM = new AddressPM();
            AddressPM.Tenant = IntegrationTestLoginParameters.Tenant;
            AddressPM.Description = "Main Address";
            AddressPM.City = "Main Address";
            AddressPM.Name = "Main Address";
            AddressPM.SearchFields = "Main Address";
            AddressPM.AddressTypeId = "M";
            AddressPM.ZipCode = "Address";
            AddressPM.CountryId = FullAccountingVariables.CountryAXId;

            return AddressPM;
        }
        private static VatTypePM GetNewVatTypeExemptPM()
        {
            VatTypePM vatTypePM = new VatTypePM();
            vatTypePM.Tenant = IntegrationTestLoginParameters.Tenant;
            vatTypePM.Code = "EPT";
            vatTypePM.EnglishName = "Exempt";
            vatTypePM.LocalName = "Exempt";
            vatTypePM.SearchFields = "EPT,Exempt";
            vatTypePM.NewEntityPercentage = 3.5;
            vatTypePM.NewEntityPercentageDate = DateTime.UtcNow;
            vatTypePM.VatTypePercentages = new List<VatTypePercentagePM>();
            VatTypePercentagePM vatTypePercentagePM = new VatTypePercentagePM();
            vatTypePercentagePM.Tenant = IntegrationTestLoginParameters.Tenant;
            vatTypePercentagePM.FromDate = DateTime.UtcNow;
            vatTypePercentagePM.Percentage = 3.5;
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
            paymentTermPM.EndOfMonth = false;
            paymentTermPM.NumberOfMonths = 0;
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
            HttpResponseMessage response = await RestClientService.GetAsync("CustomerViews"+ QueryFiltersPreparation.GetUrlParameters("1122PMCS"));
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
            if(customerPM!= null)
                await UpdateCustomerTestGlCustomer12PMCS(customerPM);
        }

        private static async Task UpdateCustomerTestGlCustomer12PMCS(CustomerPM CustomerTestGlCustomerPM)
        {
            
            CustomerTestGlCustomerPM.GLAccountId = FullAccountingVariables.GLAccountCustomer54l4CSPMId;
            await RestClientService.GetAsync("GLAccounts/GetConnectCardToGLAccount?accountId=" + FullAccountingVariables.GLAccountCustomer54l4CSPMId + "&cardId=" + CustomerTestGlCustomerPM.Id + "&skipConnectedCardsValidation=true");
            FullAccountingVariables.CustomerTestGlCust12PMCSId = CustomerTestGlCustomerPM.Id;
        }
        private static CustomerPM GetNewCustomerTestGlCustomer12PMCS()
        {
            CustomerPM customerPM = new CustomerPM();
            customerPM.Tenant = IntegrationTestLoginParameters.Tenant;
            customerPM.EnglishName = "GE:Customer";
            customerPM.LocalName = "GE:Customer";
            customerPM.SearchFields = "1122PMCS,GE:Customer";
            customerPM.GLAccountId = FullAccountingVariables.GLAccountCustomer54l4CSPMId;
            customerPM.Code = "1122PMCS";
            customerPM.PartnerTypeId = "CS";
            customerPM.CityName = "TEST";
            customerPM.CountryId = FullAccountingVariables.CountryAXId;
            customerPM.CountryCode = "1";
            customerPM.CountryName = "palestine";
            customerPM.IsCustomer = true;
            customerPM.CustomerStatusCode = "ACT";
            customerPM.Addresses = new List<AddressPM>();
            customerPM.Addresses.Add(GetNewAddressPM());
            return customerPM;
        }
        private static async Task GetVendorTestGlVendor1s5PMV2()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("VendorViews"+QueryFiltersPreparation.GetUrlParameters("11ss5PMV2"));
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
            if (vendorPM != null)
                await UpdateVendorTestGlVendPM(vendorPM);
        }

        private static async Task UpdateVendorTestGlVendPM(VendorPM VendorTestGlVendPM)
        {
            VendorTestGlVendPM.GLAccountId = FullAccountingVariables.GLAccountVendor458GLPMId;
            await RestClientService.GetAsync("GLAccounts/GetConnectCardToGLAccount?accountId=" + FullAccountingVariables.GLAccountVendor458GLPMId + "&cardId=" + VendorTestGlVendPM.Id + "&skipConnectedCardsValidation=true");
            FullAccountingVariables.VendorTestGlVendor1s5PMV2Id = VendorTestGlVendPM.Id;
        }
        private static VendorPM GetNewVendorTestGlVen1s5PMV2()
        {
            VendorPM vendorPM = new VendorPM();
            vendorPM.Tenant = IntegrationTestLoginParameters.Tenant;
            vendorPM.EnglishName = "GE:Vendor";
            vendorPM.LocalName = "GE:Vendor";
            vendorPM.GLAccountId = FullAccountingVariables.GLAccountVendor458GLPMId;
            vendorPM.SearchFields = "11ss5PMV2,GE:Vendor";
            vendorPM.Code = "11ss5PMV2";
            vendorPM.PartnerTypeId = "VD";
            vendorPM.CityName = "TEST";
            vendorPM.CountryId = FullAccountingVariables.CountryAXId;
            vendorPM.CountryCode = "1";
            vendorPM.CountryName = "palestine";
            vendorPM.Addresses = new List<AddressPM>();
            vendorPM.Addresses.Add(GetNewAddressPM());
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
           // gLAccountPM.NewGLAccountCardId = FullAccountingVariables.CustomerTestGlCust12PMCSId;
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
            //gLAccountPM.NewGLAccountCardId = FullAccountingVariables.VendorTestGlVendor1s5PMV2Id;
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
            FullAccountingVariables.NewBranchCashBookBK14Id = branchPM.Id;

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
        private static AccountingPeriodPM GetNewAccountingPeriodPM(string PeriodTypeCode)
        {
            AccountingPeriodPM AccountingPeriodPM = new AccountingPeriodPM();
            AccountingPeriodPM.Tenant = IntegrationTestLoginParameters.Tenant;
            AccountingPeriodPM.PeriodTypeCode = PeriodTypeCode;
            AccountingPeriodPM.OpenMonth = DateTime.Now.Month;
            AccountingPeriodPM.Year = DateTime.Now.Year;
            AccountingPeriodPM.ClosedMonth = null;
           
            return AccountingPeriodPM;
        }
        private static async Task GetJournalPrepration()
        {
            ApiQueryFilters filters = new ApiQueryFilters();
            filters.Filter1Name = "AccountingEntityReference";
            filters.Filter1Operator = "Contains";
            filters.PageSize = 23;
            filters.Filter1Value = "GE:JO";

            HttpResponseMessage response = await RestClientService.GetAsync("JournalViews" + QueryFiltersPreparation.GetUrlParameters("GE:JO", filters));
            JournalList journalList = RestClientService.ParseResponse<JournalList>(response);
            if (journalList == null)
                await CreateJournalPreperation();
            else
            {
                FullAccountingVariables.JournalPreprationId = journalList.Id;
            }

        }
        public static async Task CreateJournalPreperation()
        {
            JournalPM entityPM = GetNewJournal();
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "Journals");
            JournalPM JournalPM = RestClientService.ParseResponse<JournalPM>(response);
            FullAccountingVariables.JournalPreprationId = JournalPM.Id;
        }
        private static JournalPM GetNewJournal()
        {
            JournalPM JournalPM = new JournalPM();
            JournalPM.Tenant = IntegrationTestLoginParameters.Tenant;
            JournalPM.CreateDate = DateTime.UtcNow;
            JournalPM.AccountingDate = DateTime.UtcNow;
            JournalPM.TypeCode = "0";
            JournalPM.StatusCode = "1";
            JournalPM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            JournalPM.CreatedByUserName = IntegrationTestLoginParameters.LoginUserName;
            JournalPM.AccountingEntityCode = "1";
            JournalPM.UpdateDate = DateTime.UtcNow;
            JournalPM.UpdatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            JournalPM.UpdatedByUserName = IntegrationTestLoginParameters.LoginUserName;
            JournalPM.ApproveDate = DateTime.UtcNow;
            JournalPM.ApprovedByUserId = IntegrationTestLoginParameters.LoginUserId;
            var RandomString = VariablesGenerater.GetRandomString(5);
            JournalPM.SearchFields = "GE:Prepration:" + RandomString;
            JournalPM.AccountingEntityReference = "GE:JO" + RandomString;
            JournalPM.IsVoided = false;
            JournalPM.IsLedgerCreated = true;
            JournalPM.JournalLines = new List<JournalLinePM>();

            JournalLinePM CreditLine = new JournalLinePM();
            CreditLine.Tenant = IntegrationTestLoginParameters.Tenant;
            CreditLine.Line = 1;
            CreditLine.ActionCode = "1";
            CreditLine.CreditAccountId = FullAccountingVariables.GLAccountCustomer54l4CSPMId;
            CreditLine.DocumentDate = DateTime.UtcNow;
            CreditLine.AccountingDate = DateTime.UtcNow;
            CreditLine.DueDate = DateTime.UtcNow;
            CreditLine.LocalAmount = 100;
            CreditLine.CurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            CreditLine.ForeignAmount = 100;
            CreditLine.ExchangeRate = 1;
            CreditLine.ActionTypeCode = null;

            JournalPM.JournalLines.Add(CreditLine);

            JournalLinePM DebitLine = new JournalLinePM();
            DebitLine.Tenant = IntegrationTestLoginParameters.Tenant;
            DebitLine.Line = 1;
            DebitLine.ActionCode = "2";
            DebitLine.DebitAccountId = FullAccountingVariables.GLAccountVendor458GLPMId;
            DebitLine.DocumentDate = DateTime.UtcNow;
            DebitLine.AccountingDate = DateTime.UtcNow;
            DebitLine.DueDate = DateTime.UtcNow;
            DebitLine.LocalAmount = 100;
            DebitLine.CurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            DebitLine.ForeignAmount = 100;
            DebitLine.ExchangeRate = 1;
            DebitLine.ActionTypeCode = null;

            JournalPM.JournalLines.Add(DebitLine);

            return JournalPM;
        }


        public static async Task CreateARInvoice12PMCS()
        {
            ARInvoicePM entityPM = GetNewARInvoice();
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "ARInvoices");
            ARInvoicePM ARInvoicePM = RestClientService.ParseResponse<ARInvoicePM>(response);
            FullAccountingVariables.InvoiceNumber12PMCSId = ARInvoicePM.Id;
            FullAccountingVariables.InvoiceNumber12PMCSNumber = ARInvoicePM.InvoiceNumber;
        }

        public static async Task CreateARPayment12PMCS()
        {
            ARPaymentPM entityPM = GetNewARPayment();
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "ARPayments");
            ARPaymentPM ARPaymentPM = RestClientService.ParseResponse<ARPaymentPM>(response);
            FullAccountingVariables.ARPayment12PMCSId = ARPaymentPM.Id;
            FullAccountingVariables.ARPayment12PMCSNumber = ARPaymentPM.PaymentNo;
        }

        private static ARInvoicePM GetNewARInvoice()
        {
            ARInvoicePM ARInvoicePM = new ARInvoicePM();
            ARInvoicePM.Tenant = IntegrationTestLoginParameters.Tenant;
            ARInvoicePM.InvoiceNumber = VariablesGenerater.GetUniqueIdByDate();
            ARInvoicePM.ARInvoiceTypeCode = "IN";
            ARInvoicePM.BillToId = FullAccountingVariables.CustomerTestGlCust12PMCSId;
            ARInvoicePM.BillToPartnerTypeId = "CS";
            ARInvoicePM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ARInvoicePM.InvoiceDate = DateTime.UtcNow;
            ARInvoicePM.DueDate = DateTime.UtcNow;
            ARInvoicePM.UpdateDate = DateTime.UtcNow;
            ARInvoicePM.UpdatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ARInvoicePM.IssuedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ARInvoicePM.InvoiceCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            ARInvoicePM.ApprovedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ARInvoicePM.LocalCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            ARInvoicePM.SubTotalInLocalCurrency = 400;
            ARInvoicePM.SubTotalInInvoiceCurrency = 400;
            ARInvoicePM.AmountInLocalCurrency = 400;
            ARInvoicePM.AmountInInvoiceCurrency = 400;
            ARInvoicePM.StatusCode = "AD";
            ARInvoicePM.StatusName = "Unpaid";
            ARInvoicePM.InvoiceCurrencyExchangeRate = 1;
            ARInvoicePM.PaymentTermId = FullAccountingVariables.PaymentTermCashId;
            ARInvoicePM.CreateDate = DateTime.UtcNow;
            ARInvoicePM.SearchFields = "1122PMCS,AccountingCustomer2";
            ARInvoicePM.IsGeneralInvoice = true;
            ARInvoicePM.ProfitCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            ARInvoicePM.ProfitCurrencyExchangeRate = 2;
            ARInvoicePM.AmountDueInLocalCurrency = 40000;
            ARInvoicePM.AmountDueInProfitCurrency = 2000;
            ARInvoicePM.BranchId = FullAccountingVariables.BranchMainOfficeId;
            ARInvoicePM.SetApproved = false;
            ARInvoicePM.TotalAmountForTaxReport = 4000;
            ARInvoicePM.TotaVatableAmountForTaxReport = 0;
            ARInvoicePM.TotalVAT = 0;
            ARInvoicePM.IsFullAccounting = true;
            ARInvoicePM.AmountDue = 4000;
            ARInvoicePM.InvoiceLines = new List<ARInvoiceLinePM>();
            ARInvoicePM.InvoiceLines.Add(GetNewARInvoiceLine());
            return ARInvoicePM;
        }

        private static ARPaymentPM GetNewARPayment()
        {
            ARPaymentPM ARPaymentPM = new ARPaymentPM();
            ARPaymentPM.Tenant = IntegrationTestLoginParameters.Tenant;
            ARPaymentPM.InvoiceNumber = VariablesGenerater.GetUniqueIdByDate();
            ARPaymentPM.PaymentNo = VariablesGenerater.GetUniqueIdByDate();
            ARPaymentPM.IsSecured = false;
            ARPaymentPM.BillToId = FullAccountingVariables.CustomerTestGlCust12PMCSId;
            ARPaymentPM.BillToPartnerTypeId = "CS";
            ARPaymentPM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ARPaymentPM.BranchId = FullAccountingVariables.BranchCashBookBK14Id;
            ARPaymentPM.PaymentCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            ARPaymentPM.UpdateDate = DateTime.UtcNow;
            ARPaymentPM.UpdatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ARPaymentPM.AccountingPaymentMethodId = FullAccountingVariables.PaymentMethodCashId;
            ARPaymentPM.AccountingPaymentMethodCode = "CA";
            ARPaymentPM.ApprovedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ARPaymentPM.LocalCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            ARPaymentPM.AmountInPaymentCurrency = 12000;
            ARPaymentPM.AmountInLocalCurrency = 12000;
            ARPaymentPM.PaymentCurrencyExchangeRate = 1;
            ARPaymentPM.StatusCode = "DR";
            ARPaymentPM.StatusName = "Unpaid";
            ARPaymentPM.CashbookId = FullAccountingVariables.CashBook1421TestId;
            ARPaymentPM.BillToAddressId = FullAccountingVariables.AddressCustomer12PMCS;
            ARPaymentPM.OpenAmount =12000;
            ARPaymentPM.CreateDate = DateTime.UtcNow;
            ARPaymentPM.SearchFields = "ARP2059,AD,CA,Accounting Customer 2 ,NIS";
            ARPaymentPM.ValueDate = DateTime.UtcNow;
            ARPaymentPM.RegisterDate = DateTime.UtcNow;
            ARPaymentPM.ProfitCurrencyExchangeRate = 2;
            ARPaymentPM.SetApproved = false;
            ARPaymentPM.SetApproved = true;
            ARPaymentPM.IsFullAccounting = true;

            return ARPaymentPM;


        }
        private static ARInvoiceLinePM GetNewARInvoiceLine()
        {
            ARInvoiceLinePM Line = new ARInvoiceLinePM
            {
                Tenant = IntegrationTestLoginParameters.Tenant,
                ChargesTypeId = FullAccountingVariables.ChargeTypesAirFreightId,
                ForiegnCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId,
                ForiegnCurrencyAmount = 400,
                LocalCurrencyAmount = 400,
                InvoiceCurrencyAmount = 400,
                VatTypeId = FullAccountingVariables.VatEXEMPTId,
                VatPercentage = 0.0,
                VatTypeName = "Exempt",
                ForiegnExchangeRate = 1,
                LineNumber = 1,
                Quantity = 20,
                UnitPrice = 200,
                ViewOrder = 0,
                ProfitCurrencyAmount = 200,
                CreditAccount = FullAccountingVariables.GLAccountCustomer54l4CSPMId,
                Description = "Air Freight",
                LocalDescription = "Air Freight",
                GLAccountId = FullAccountingVariables.GLAccountCustomer54l4CSPMId,
                LineActionCode = "1"
            };

            return Line;
        }

        public static async Task CreateAPInvoice1205()
        {

            APInvoicePM entityPM = GetNewAPInvoice();
            HttpResponseMessage response = await RestClientService.PostAsync(entityPM, "APInvoices");
            APInvoicePM APInvoicePM = RestClientService.ParseResponse<APInvoicePM>(response);
            FullAccountingVariables.APInvoice1205Id = APInvoicePM.Id;
            FullAccountingVariables.APInvoice1205Number = APInvoicePM.InvoiceNumber;
        }


        private static APInvoicePM GetNewAPInvoice()
        {
            APInvoicePM APInvoicePM = new APInvoicePM();
            APInvoicePM.Tenant = IntegrationTestLoginParameters.Tenant;
            APInvoicePM.InvoiceNumber = VariablesGenerater.GetUniqueIdByDate();
            APInvoicePM.IsSecured = false;
            APInvoicePM.InternalNumber = VariablesGenerater.GetRandomString(5);
            APInvoicePM.VendorId = FullAccountingVariables.VendorTestGlVendor1s5PMV2Id;
            APInvoicePM.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            APInvoicePM.InvoiceDate = DateTime.UtcNow;
            APInvoicePM.PaymentTermId = FullAccountingVariables.PaymentTermCashId;
            APInvoicePM.DueDate = DateTime.UtcNow;
            APInvoicePM.InvoiceCurrencyExchangeRate = 1;
            APInvoicePM.UpdateDate = DateTime.UtcNow;
            APInvoicePM.UpdatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            APInvoicePM.InvoiceCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            APInvoicePM.ApprovedByUserId = IntegrationTestLoginParameters.LoginUserId;
            APInvoicePM.LocalCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            APInvoicePM.SubTotalInLocalCurrency = 200;
            APInvoicePM.SubTotalInInvoiceCurrency = 200;
            APInvoicePM.AmountInLocalCurrency = 200;
            APInvoicePM.AmountInInvoiceCurrency = 200;
            APInvoicePM.StatusCode = "AD";
            APInvoicePM.StatusName = "Unpaid";
            APInvoicePM.VendorPartnerTypeId = "VD";
            APInvoicePM.AmountInProfitCurrency = 100;
            APInvoicePM.InvoiceExpectedAmount = 400;
            APInvoicePM.IsClosed = false;
            APInvoicePM.JournalNumber = "1230";
            APInvoicePM.CreateDate = DateTime.UtcNow;
            APInvoicePM.SearchFields = "12205,test";
            APInvoicePM.IsGeneralInvoice = true;
            if (!String.IsNullOrEmpty(FullAccountingVariables.AccountingCurrencyTenantId)) APInvoicePM.ProfitCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId;
            APInvoicePM.ProfitCurrencyExchangeRate = 2;
            APInvoicePM.AmountDueInLocalCurrency = 1200;
            APInvoicePM.AmountDueInProfitCurrency = 600;
            APInvoicePM.BranchId = FullAccountingVariables.BranchMainOfficeId;
            APInvoicePM.SetApproved = true;
            APInvoicePM.VATNumber = "5145599";
            APInvoicePM.AccountingDate = DateTime.UtcNow;
            APInvoicePM.IsExternalEntity = false;
            APInvoicePM.IsGeneralInvoice = true;
            APInvoicePM.AmountDue = 200;
            APInvoicePM.InvoiceLines = new List<APInvoiceLinePM>();
            APInvoicePM.InvoiceLines.Add(GetNewAPInvoiceLine());
            APInvoicePM.TotalVATs = new List<APInvoiceTotalVATPM>();
            APInvoicePM.TotalVATs.Add(GetNewAPInvoiceTotalVATs());
            return APInvoicePM;
        }

        private static APInvoiceLinePM GetNewAPInvoiceLine()
        {
            APInvoiceLinePM Line = new APInvoiceLinePM
            {
                Tenant = IntegrationTestLoginParameters.Tenant,
                ChargesTypeId = FullAccountingVariables.ChargeTypesAirFreightId,
                ForiegnCurrencyId = FullAccountingVariables.AccountingCurrencyTenantId,
                ForiegnCurrencyAmount = 400,
                LocalCurrencyAmount = 400,
                InvoiceCurrencyAmount = 400,
                VatTypeId = FullAccountingVariables.VatEXEMPTId,
                VatPercentage = 0.0,
                VatTypeName = "Exempt",
                ForiegnExchangeRate = 1,
                LineNumber = 1,
                Quantity = 20,
                AmountTypeCode = "NEXP",
                ProfitCurrencyAmount = 400,
                ChargeTypeGLAccountId = FullAccountingVariables.GLAccountCustomer54l4CSPMId,
                Description = "Air Freight",
                LocalDescription = "Air Freight",

            };

            return Line;
        }

        private static APInvoiceTotalVATPM GetNewAPInvoiceTotalVATs()
        {
            APInvoiceTotalVATPM Line = new APInvoiceTotalVATPM
            {
                Tenant = IntegrationTestLoginParameters.Tenant,
                VatTypeId = FullAccountingVariables.VatEXEMPTId,
                VatPercent = 0.0,
                VatTypeName = "Exempt",
                VatTypeCell = "Exempt (0%)",
                InvoiceCurrencyVatableAmount = 200,
                LocalVatableAmount = 200,
                ProfitVatableAmount = 100,
                LocalVATAmount = 0,
                ProfitCurrencyVATAmount = 0,


            };

            return Line;
        }
    }
}
