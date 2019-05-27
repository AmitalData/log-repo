
const { SpecReporter } = require('jasmine-spec-reporter');

exports.config = {
  allScriptsTimeout: 990000,
  // specs: [
  //  './e2e/Operations/**/Operations.e2e-spec.ts'
  // ],
    params: {
        Login:{
            Email: null,
            Password:null,
        },
        Env: "prod",
        Link: null,        
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
          browser.params.Login.Email = "razantest@protractor.com";
          browser.params.Login.Password = "!R123j456";
          browser.params.Link = "https://system.logitudeworld.com";

      }
      else if (browser.params.Env == "test") {
          browser.params.Login.Email = "razan@logitudeworld.com";
          browser.params.Login.Password = "!R123j456";
          browser.params.Link = "https://test.logitudeworld.com/test";
      }
    
  
    jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
  },

  suites: {
    // ********************* Login **********************************
     login:// 'e2e/Login/**/Login.e2e-spec.ts',
        //  CRM: 'e2e/CRM/**/CRMModule-spec.ts',
           'e2e/LogBox/Login/**/Login.e2e-spec.ts',

      //NewShipment: 'e2e/Operations/Shipments/NewEntity/**/Operations.e2e-spec.ts',
      NewShipmentlogbox: 'e2e/LogBox/Shipments/**/ShipmentSearch.e2e-spec.ts',
    //  Contact: 'e2e/Contacts/**/Contacts-spec.ts',

     //EditTabs: 'e2e/Operations/Shipments/EditEntity/**/EditShipmentTabs.e2e-spec.ts',
    //  ShipmentSearch: 'e2e/Operations/**/ShipmentSearch.e2e-spec.ts',

    // ********************* FullAccounting **********************************
      //  NewChartOfAccount: 'e2e/FullAccounting/ChartOfAccount/**/ChartOfAccount-spec.ts',
// Reports: 'e2e/Report/**/Report-spec.ts',


  },
};
