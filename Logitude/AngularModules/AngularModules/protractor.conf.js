const { SpecReporter } = require('jasmine-spec-reporter');

exports.config = {
  allScriptsTimeout: 990000,
  // specs: [
  //  './e2e/Operations/**/Operations.e2e-spec.ts'
  // ],
  params: {
    Env: null,
    Link: null,
    Login: {
      Email: null,
      Password: null,
    },
    ShipParams:{
      ShipmentLevelCode:null,
      Direction:null,
      TransportMode:null,
      ShipmentType:null,
    },
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

    jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
  },

  suites: {
    // ********************* Login **********************************
    login: 'e2e/Login/**/Login.e2e-spec.ts',
    //  CRM: 'e2e/CRM/**/CRMModule-spec.ts',
    //  'e2e/LogBox/Login/**/Login.e2e-spec.ts',

   // NewShipment: 'e2e/Operations/Shipments/NewEntity/**/Operations.e2e-spec.ts',
     NewShipmentlogbox: 'e2e/LogBox/Shipments/**/ShipmentSearch.e2e-spec.ts',
    //  Contact: 'e2e/Contacts/**/Contacts-spec.ts',

    //EditTabs: 'e2e/Operations/Shipments/EditEntity/**/EditShipmentTabs.e2e-spec.ts',
    //  ShipmentSearch: 'e2e/Operations/**/ShipmentSearch.e2e-spec.ts',

    // ********************* FullAccounting **********************************
    //  NewChartOfAccount: 'e2e/FullAccounting/ChartOfAccount/**/ChartOfAccount-spec.ts',
    // Reports: 'e2e/Report/**/Report-spec.ts',


  },
};
