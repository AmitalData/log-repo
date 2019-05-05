// Protractor configuration file, see link for more information
// https://github.com/angular/protractor/blob/master/lib/config.ts

const { SpecReporter } = require('jasmine-spec-reporter');

exports.config = {
  allScriptsTimeout: 990000,
  // specs: [
  //  './e2e/Operations/**/Operations.e2e-spec.ts'
  // ],
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
    jasmine.getEnv().addReporter(new SpecReporter({ spec: { displayStacktrace: true } }));
  },

  suites: {
    // ********************* Login **********************************
     login: 'e2e/Login/**/Login.e2e-spec.ts',
    //  CRM: 'e2e/CRM/**/CRMModule-spec.ts',

     //NewShipment: 'e2e/Operations/Shipments/NewEntity/**/Operations.e2e-spec.ts',
    //  Contact: 'e2e/Contacts/**/Contacts-spec.ts',

     //EditTabs: 'e2e/Operations/Shipments/EditEntity/**/EditShipmentTabs.e2e-spec.ts',
    //  ShipmentSearch: 'e2e/Operations/**/ShipmentSearch.e2e-spec.ts',

    // ********************* FullAccounting **********************************
       NewChartOfAccount: 'e2e/FullAccounting/ChartOfAccount/**/ChartOfAccount-spec.ts',



  },
};
