const { SpecReporter } = require('jasmine-spec-reporter');
const HtmlReporter = require('protractor-beautiful-reporter');
const { JUnitXmlReporter } = require('jasmine-reporters');

exports.config = {
    allScriptsTimeout: 990000,
    // specs: [
    //  './e2e/Operations/**/Operations.e2e-spec.ts'
    // ],
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
    capabilities: {
        'browserName': 'chrome',
        'acceptInsecureCerts': true,
    },
    seleniumServerJar: './node_modules/selenium-standalone-jar/bin/selenium-server-standalone-3.0.1.jar',

    directConnect: true, // Direct connect with the chrome or firefox without running selenium server 
    framework: 'jasmine',
    jasmineNodeOpts: {
        showColors: true,
        defaultTimeoutInterval: 300000,
        print: function () { }
    },
    onPrepare() {
        require('ts-node').register({
            project: 'e2e/tsconfig.e2e.json'
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
            browser.params.Link = "https://system.logitudeworld.com";
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
            browser.params.Link = "https://cloud.amital.co.il/";
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

        //------------------------------------- Reporter --------------------------------
        if (browser.params.Team == "ayman") {
            jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
            jasmine.getEnv().addReporter(new HtmlReporter({ baseDirectory: 'C:/Automation e2e/TeamAyman/Test/screenshots', takeScreenShotsOnlyForFailedSpecs: true, screenshotsSubfolder: 'images' }).getJasmine2Reporter());
            jasmine.getEnv().addReporter(junitReporterAyman);

        } else if (browser.params.Team == "islam") {
            jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
            jasmine.getEnv().addReporter(new HtmlReporter({ baseDirectory: 'C:/Automation e2e/TeamIslam/Test/screenshots', takeScreenShotsOnlyForFailedSpecs: true, screenshotsSubfolder: 'images' }).getJasmine2Reporter());
            jasmine.getEnv().addReporter(junitReporterIslam);

        } else if (browser.params.Team == "mohammad") {
            jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
            jasmine.getEnv().addReporter(new HtmlReporter({ baseDirectory: 'C:/Automation e2e/TeamMohammad/Test/screenshots', takeScreenShotsOnlyForFailedSpecs: true, screenshotsSubfolder: 'images' }).getJasmine2Reporter());
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

    suites: {
        // ********************* Login **********************************
        login: 'e2e/Login/**/Login.e2e-spec.ts',
        NewQuote: 'e2e/CRM/Quotes/NewEntity/**/NewQuote-spec.ts',
        // CustomerGLA: 'e2e/FullAccounting/'

        CRM: 'e2e/CRM/**/CRMModule-spec.ts',
        NewShipment: 'e2e/Operations/Shipments/NewEntity/**/Operations.e2e-spec.ts',
        NewEAWB: 'e2e/Operations/Shipments/NewEntity/**/OpEAWB-spec.ts',
        NewShipmentlogbox: 'e2e/LogBox/Shipments/**/ShipmentSearch.e2e-spec.ts',
        Contact: 'e2e/Contacts/**/Contacts-spec.ts',

        // ********************* FullAccounting **********************************
        PaymentCheque: 'e2e/FullAccounting/PaymentCheque/**/NewPaymentCheque-spec.ts',
        ARPayment: 'e2e/FullAccounting/**/ARPayment-spec.ts',
        NewChartOfAccount: 'e2e/FullAccounting/ChartOfAccount/**/ChartOfAccount-spec.ts',
        FullAccProcess: 'e2e/FullAccounting/**/FullAccScenarios-spec.ts',
        ARInvoice: 'e2e/FullAccounting/ARInvoice/**/ARInvoice-spec.ts',
        BankAccount: 'e2e/FullAccounting/BankAccount/**/NewBank-spec.ts',
        VendorGLAccount: 'e2e/FullAccounting/GlAccounts/**/VendorGLAccount-spec.ts',
        CustomerGLAccount: 'e2e/FullAccounting/**/CustomerGLAccount-spec.ts',
        APInvoice: 'e2e/FullAccounting/APInvoice/**/APInvoice-spec.ts',
        RevGLAccount: 'e2e/FullAccounting/**/GlAccount-spec.ts',
        //   CashDeposit: 'e2e/FullAccounting/**/Deposit/NewDposit-spec.ts',
        //*************Report********************
        Reports: 'e2e/Report/**/Report-spec.ts',

        //*************ShipmentView********************
        ShipmentView: 'e2e/**/ShipmentView-spec.ts',

        //*************Maintenance********************
        CompanyAddressSetting: 'e2e/Maintenance/**/CompanyAddressSetting-spec.ts',
        NewAgent: 'e2e/Maintenance/**/Agent-spec.ts',
        NewUser: 'e2e/Maintenance/**/Users-spec.ts',
        NewShipper: 'e2e/Maintenance/**/Shipper-spec.ts',

        //*************DocOutTab***************
        DocOut: 'e2e/**/DocsOut.e2e-spec.ts',
        LogitudeAccounting: 'e2e/Accounting/**/AccountingModule-spec.ts'
    },
};

