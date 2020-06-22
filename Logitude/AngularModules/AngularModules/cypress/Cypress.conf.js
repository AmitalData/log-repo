// @ts-nocheck

module.exports = require('./lib/cypress');
/**
 * @type { import("cypress").Config }
 */
export const config = {

    capabilities: {
      browserName: 'chrome',
    },
    baseUrl: 'http://localhost:4200/',
   


    params: {
        Env: null,
        URL: null,
        Team: null,
        Login: {
            Email: null,
            Password: null,
        },
        ShipParams: {
            ShipmentLevelCode: null,
            Direction: null,
            TransportMode: null,
            ShipmentType: null,
            ShipmentEditTabs: null,
        },
        QuoteParams: {
            Direction: null,
            TransportMode: null,
            ShipmentType: null,
            QuoteType: null,
        },
        ReportDoc: {
            SenarioType: null,

        },
        FullAccount: {
            FullAccountingType: null,
        },
        CRM: {
            CRMType: null,
            ActivityType: null,
        },
        Accounting: {
            AccountingType: null,
        }
    },

    suites: {
        // ********************* Login **********************************
        login: './Login/**/Login.e2e-spec.ts',
        NewQuote: './CRM/Quotes/NewEntity/**/NewQuote-spec.ts',
        // CustomerGLA: './FullAccounting/'

        CRM: './CRM/**/CRMModule-spec.ts',
        NewShipment: './Operations/Shipments/NewEntity/**/Operations.e2e-spec.ts',
        NewEAWB: './Operations/Shipments/NewEntity/**/OpEAWB-spec.ts',
        NewShipmentlogbox: './LogBox/Shipments/**/ShipmentSearch.e2e-spec.ts',
        Contact: './Contacts/**/Contacts-spec.ts',

        // ********************* FullAccounting **********************************
        PaymentCheque: './FullAccounting/PaymentCheque/**/NewPaymentCheque-spec.ts',
        ARPayment: './FullAccounting/**/ARPayment-spec.ts',
        NewChartOfAccount: './FullAccounting/ChartOfAccount/**/ChartOfAccount-spec.ts',
        FullAccProcess: './FullAccounting/**/FullAccScenarios-spec.ts',
        ARInvoice: './FullAccounting/ARInvoice/**/ARInvoice-spec.ts',
        BankAccount: './FullAccounting/BankAccount/**/NewBank-spec.ts',
        VendorGLAccount: './FullAccounting/GlAccounts/**/VendorGLAccount-spec.ts',
        CustomerGLAccount: './FullAccounting/**/CustomerGLAccount-spec.ts',
        APInvoice: './FullAccounting/APInvoice/**/APInvoice-spec.ts',
        RevGLAccount: './FullAccounting/**/GlAccount-spec.ts',
        //   CashDeposit: './FullAccounting/**/Deposit/NewDposit-spec.ts',
        //*************Report********************
        Reports: './Report/**/Report-spec.ts',

        //*************ShipmentView********************
        ShipmentView: './**/ShipmentView-spec.ts',

        //*************Maintenance********************
        CompanyAddressSetting: './Maintenance/**/CompanyAddressSetting-spec.ts',
        NewAgent: './Maintenance/**/Agent-spec.ts',
        NewUser: './Maintenance/**/Users-spec.ts',
        NewShipper: './Maintenance/**/Shipper-spec.ts',

        //*************DocOutTab***************
        DocOut: './**/DocsOut.e2e-spec.ts',
        LogitudeAccounting: './Accounting/**/AccountingModule-spec.ts'
    },
    onPrepare() {
        
    if (browser.params.Env == "Test") {
        browser.params.URL = "https://test.logitudeworld.com/test";
        browser.params.Login.Email = "protractor2@test.com";
        browser.params.Login.Password = "!P123p456";
    }
    else if (browser.params.Env == "Staging") {
        browser.params.URL = "https://test.logitudeworld.com/staging";
        browser.params.Login.Email = "protractor2@test.com";
        browser.params.Login.Password = "!P123p456";
    }
}
}