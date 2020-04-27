// @ts-check
// Protractor configuration file, see link for more information
// https://github.com/angular/protractor/blob/master/lib/config.ts

const { SpecReporter } = require('jasmine-spec-reporter');
const HtmlReporter = require('protractor-beautiful-reporter');
const { JUnitXmlReporter } = require('jasmine-reporters');

/**
 * @type { import("protractor").Config }
 */
exports.config = {
  allScriptsTimeout: 480000,
  // specs: [
    // './src/**/*.e2e-spec.ts'                       Angular 9
    // './e2e/Operations/**/Operations.e2e-spec.ts'   Master Branch
  // ],
  capabilities: {
    browserName: 'chrome',
    acceptInsecureCerts: true,
  },
  seleniumServerJar: './node_modules/selenium-standalone-jar/bin/selenium-server-standalone-3.0.1.jar',
  directConnect: true, // Direct connect with the chrome or firefox without running selenium server
  baseUrl: 'http://localhost:4200/',
  framework: 'jasmine',

  jasmineNodeOpts: {
    showColors: true,
    defaultTimeoutInterval: 10000000,
    print: function() {}
  },

    params: {
        Env: null,
        Link: null,
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
        require('ts-node').register({
          project: require('path').join(__dirname, './tsconfig.json')
        });

        const junitReporterAyman = new JUnitXmlReporter({
            savePath: 'C:/Program Files (x86)/Jenkins/workspace/TeamAymanE2EScripts',
            consolidateAll: false
        });
        const junitReporterIslam = new JUnitXmlReporter({
            savePath: 'C:/Program Files (x86)/Jenkins/workspace/TeamIslamE2EScripts',
            consolidateAll: false
        });
        const junitReporterMohammad = new JUnitXmlReporter({
            savePath: 'C:/Program Files (x86)/Jenkins/workspace/TeamMohammadE2EScripts',
            consolidateAll: false
        });

        if (browser.params.Env == "prod") {
            browser.params.Link = "https://system.logitudeworld.com";
            browser.params.Login.Email = "razantest@protractor.com";
            browser.params.Login.Password = "!R123j456";
        }
        else if (browser.params.Env == "cloudStaging") {
            browser.params.Link = "https://staging.amital.co.il/";
            browser.params.Login.Email = "protractor@test.com";
            browser.params.Login.Password = "!P123t456";
        }
        else if (browser.params.Env == "staging") {
            browser.params.Link = "https://staging.logitudeworld.com";
            browser.params.Login.Email = "protractor@test.com";
            browser.params.Login.Password = "!P123t456";
        }
        else if (browser.params.Env == "test") {
            browser.params.Link = "https://test.logitudeworld.com/test";
            browser.params.Login.Email = "protractor@test.com";
            browser.params.Login.Password = "!P123t456";
        }
        else if (browser.params.Env == "test_staging") {
            browser.params.Link = "https://test.logitudeworld.com/staging";
            browser.params.Login.Email = "protractor@test.com";
            browser.params.Login.Password = "!P123t456";
        }
        else if (browser.params.Env == "logboxtest") {
            browser.params.Link = "https://test.logitudeworld.com/test";
            browser.params.Login.Email = "ahmadb@logbox.com";
            browser.params.Login.Password = "ahmed!A123";
        }
        else if (browser.params.Env == "logbox") {
            browser.params.Link = "https://system.logbox.co.il";
            browser.params.Login.Email = "ahmadb@test.com";
            browser.params.Login.Password = "ahmed!A123";
        }
        else if (browser.params.Env == "localhost") {
            browser.params.Link = "http://localhost:4200/";
            browser.params.Login.Email = "angular@fnarsoft.com";
            browser.params.Login.Password = "1";
        }
        else if (browser.params.Env == "test_951") {
            browser.params.Link = "https://test.logitudeworld.com/test";
            browser.params.Login.Email = "raghad@protractor.com";
            browser.params.Login.Password = "!RS123Rs";
        }
        else if (browser.params.Env == "testStaging") {
            browser.params.Link = "https://staging.logitudeworld.com";
            browser.params.Login.Email = "Raghad@protractor.com";
            browser.params.Login.Password = "!RS123Rs";
        }
        else if (browser.params.Env == "testEnvStaging") {
            browser.params.Link = "https://test.logitudeworld.com/staging";
            browser.params.Login.Email = "Raghad@protractor.com";
            browser.params.Login.Password = "!RS123Rs";
        }
        else if (browser.params.Env == "prod_1") {
            browser.params.Link = "https://staging.logitudeworld.com";
            browser.params.Login.Email = "Raghad@protractor.com";
            browser.params.Login.Password = "!RS123Rs";
        }
        else if (browser.params.Env == "testEnvStaging") {
            browser.params.Link = "https://test.logitudeworld.com/staging";
            browser.params.Login.Email = "Raghad@protractor.com";
            browser.params.Login.Password = "!RS123Rs";
        }
        else if (browser.params.Env == "testStaging") {
            browser.params.Link = "https://staging.logitudeworld.com";
            browser.params.Login.Email = "Raghad@protractor.com";
            browser.params.Login.Password = "!RS123Rs";
        }
        else if (browser.params.Env == "test_1071") {
            browser.params.Link = "https://test.logitudeworld.com/staging";
            browser.params.Login.Email = "sgautomation@pro.com";
            browser.params.Login.Password = "Sg0592463934!";
        }
        else if (browser.params.Env == "test_locally") {
            browser.params.Link = "http://localhost:4200/";
            browser.params.Login.Email = "sgautomation@pro.com";
            browser.params.Login.Password = "Sg0592463934!";
        }
        else if (browser.params.Env == "cloud") {
            browser.params.Link = "http://staging.amital.co.il/";
            browser.params.Login.Email = "sumaya@cloud.com";
            browser.params.Login.Password = "Sg0592463934!";
        }
        else if (browser.params.Env == "logboxStaging") {
            browser.params.Link = "https://staging.logbox.co.il";
            browser.params.Login.Email = "ahmadb@test.com";
            browser.params.Login.Password = "ahmed!A123";
        }
        else if (browser.params.Env == "test_1109") {
            browser.params.Link = "https://test.logitudeworld.com/staging";
            browser.params.Login.Email = "sumaya@automation.com";
            browser.params.Login.Password = "Sg0592463934!";
        }

        else if (browser.params.Env == "test_1209") {
            browser.params.Link = "https://test.logitudeworld.com/staging";
            browser.params.Login.Email = "sg1209@test.com";
            browser.params.Login.Password = "!Sg13579";
        }

        else if (browser.params.Env == "Pre_Racing") {
            browser.params.Link = "https://pre.logitudeworld.com/";
            browser.params.Login.Email = "lana3@test.com";
            browser.params.Login.Password = "12La34Na56!";
        }

        else if (browser.params.Env == "test_staging1") {
            browser.params.Link = "https://test.logitudeworld.com/staging";
            browser.params.Login.Email = "protractor2@test.com";
            browser.params.Login.Password = "!P123p456";
        }

        else if (browser.params.Env == "test_live1") {
            browser.params.Link = "https://test.logitudeworld.com/test";
            browser.params.Login.Email = "protractor2@test.com";
            browser.params.Login.Password = "!P123p456";
        }

        else if (browser.params.Env == "Pre_production") {
            browser.params.Link = "https://pre.logitudeworld.com/";
            browser.params.Login.Email = "Raghad@protractor.com";
            browser.params.Login.Password = "!RS123Rs";
        }
        else if (browser.params.Env == "Pre_prod") {
            browser.params.Link = "https://pre.logitudeworld.com/";
            browser.params.Login.Email = "raghad@automation.com";
            browser.params.Login.Password = "!RS123Rs";
        }

        else if (browser.params.Env == "test_staging_951") {
            browser.params.Link = "https://test.logitudeworld.com/staging";
            browser.params.Login.Email = "raghad@protractor.com";
            browser.params.Login.Password = "!RS123Rs";
        }

 	 else if (browser.params.Env == "Prod_Staging") {
            browser.params.Link = "https://staging.logitudeworld.com/";
            browser.params.Login.Email = "protractor2@test.com";
            browser.params.Login.Password = "!P123p123";
        }
        //------------------------------------- Reporter --------------------------------
        if (browser.params.Team == "ayman") {
            jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
            jasmine.getEnv().addReporter(new HtmlReporter({ baseDirectory: 'C:/Automation e2e/TeamAyman/Test/screenshots', takeScreenShotsOnlyForFailedSpecs: true, screenshotsSubfolder: 'images' }).getJasmine2Reporter());
            jasmine.getEnv().addReporter(junitReporterAyman);

        }
        else if (browser.params.Team == "aymanProd") {
            jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
            jasmine.getEnv().addReporter(new HtmlReporter({ baseDirectory: 'C:/Automation e2e/TeamAyman/Prod/screenshots', takeScreenShotsOnlyForFailedSpecs: true, screenshotsSubfolder: 'images' }).getJasmine2Reporter());
            jasmine.getEnv().addReporter(junitReporterAyman);

        }
        else if (browser.params.Team == "aymancloud") {
            jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
            jasmine.getEnv().addReporter(new HtmlReporter({ baseDirectory: 'C:/Automation e2e/TeamAyman/Cloud/screenshots' , takeScreenShotsOnlyForFailedSpecs: true, screenshotsSubfolder: 'images' }).getJasmine2Reporter());
            jasmine.getEnv().addReporter(junitReporterIslam);

        }else if (browser.params.Team == "islam") {
            jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
            jasmine.getEnv().addReporter(new HtmlReporter({ baseDirectory: 'C:/Automation e2e/TeamIslam/Test/screenshots', takeScreenShotsOnlyForFailedSpecs: true, screenshotsSubfolder: 'images' }).getJasmine2Reporter());
            jasmine.getEnv().addReporter(junitReporterIslam);

        
        }else if(browser.params.Team == "islamProd") {
            jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
            jasmine.getEnv().addReporter(new HtmlReporter({ baseDirectory: 'C:/Automation e2e/TeamIslam/Prod/screenshots', takeScreenShotsOnlyForFailedSpecs: true, screenshotsSubfolder: 'images' }).getJasmine2Reporter());
            jasmine.getEnv().addReporter(junitReporterIslam);

        }else if (browser.params.Team == "mohammad") {
            jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
            jasmine.getEnv().addReporter(new HtmlReporter({ baseDirectory: 'C:/Automation e2e/TeamMohammad/Test/screenshots', takeScreenShotsOnlyForFailedSpecs: true, screenshotsSubfolder: 'images' }).getJasmine2Reporter());
            jasmine.getEnv().addReporter(junitReporterMohammad);

        }
        else if (browser.params.Team == "mohammadcloud") {
            jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
            jasmine.getEnv().addReporter(new HtmlReporter({ baseDirectory: 'C:/Automation e2e/TeamMohammad/Cloud/screenshots', takeScreenShotsOnlyForFailedSpecs: true, screenshotsSubfolder: 'images' }).getJasmine2Reporter());
            jasmine.getEnv().addReporter(junitReporterMohammad);

        }
        else if (browser.params.Team == "test") {
            jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
            jasmine.getEnv().addReporter(new HtmlReporter({ baseDirectory: 'C:/E2ETeamMohammad/test' }).getJasmine2Reporter());
            jasmine.getEnv().addReporter(junitReporterMohammad);

        }
        else {
            jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
            jasmine.getEnv().addReporter(new HtmlReporter({ baseDirectory: 'C:/e2eTracking/screenshots' }).getJasmine2Reporter());
        }
    },
};
