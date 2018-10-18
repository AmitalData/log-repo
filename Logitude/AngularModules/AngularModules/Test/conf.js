
var HtmlScreenshotReporter = require('protractor-jasmine2-screenshot-reporter');
var reporter = new HtmlScreenshotReporter({
    dest: 'target/',
    cleanDestination: true,
    showConfiguration: true,
    reportTitle: null,

    showSummary: true,
    //showQuickLinks: true
    //reportOnlyFailedSpecs: false,
    //captureOnlyFailedSpecs: true
    //reportTitle: "Report Title"
    // filename: 'my-report.html'
});


// conf.js
exports.config = {
    framework: 'jasmine',
    seleniumAddress: 'http://localhost:4444/wd/hub',
    jasmineNodeOpts: { showColors: true, defaultTimeoutInterval: 50000, realtimeFailure: true, onComplete: null },
    params: {
        login: {
            user: 'protractor@angular.com',
            password: '!P123456'
        }
    },
    capabilities: { 'browserName': 'chrome' },
    useAllAngular2AppRoots: true,
    //ignoreSynchronization:true,
    isverbose: true,
    includeStackTrace: true,
    //getPageTimeout:60000,
    //defaultTimeoutInterval: 2500000
    //elementExplorer:true

    //Setup the report before any tests start
    beforeLaunch: function () {
        return new Promise(function (resolve) {
            reporter.beforeLaunch(resolve);
        });
    },

    // Assign the test reporter to each running instance
    onPrepare: function () {
        //browser.manage().window().setSize(1920, 1080);

        jasmine.getEnv().addReporter(reporter);

        var jasmineReporters = require('jasmine-reporters');
        jasmine.getEnv().addReporter(
            new jasmineReporters.JUnitXmlReporter('xmloutputs', true, true)
        );

    },
    suites: {
        // ********************* Login **********************************
        login: 'e2e/Login/**/login_spec.js',
        forgotPassword: 'e2e/Login/**/forgot_password_spec.js',

        // ********************* Operation **********************************
        bookings: 'e2e/Operation/Booking/**/Bookings.js',
        awbshipment: 'e2e/Operation/Shipment/E-AWB/**/Shipments.js',
        awbedit: 'e2e/Operation/Shipment/E-AWB/**/Edit.js',

        
        airshipment: 'e2e/Operation/Shipment/Business/Direct/**/AirShipment.js',
        oceanshipment: 'e2e/Operation/Shipment/Business/Direct/**/OceanShipment.js',
        inlandshipment: 'e2e/Operation/Shipment/Business/Direct/**/InlandShipment.js',

        houseairshipment: 'e2e/Operation/Shipment/Business/House/**/HouseAirShipment.js',
        houseoceanshipment: 'e2e/Operation/Shipment/Business/House/**/HouseOceanShipment.js',
        //houseinlandshipment: 'e2e/Operation/Shipment/Business/House/**/HouseInlandShipment.js',

        businessshipment: 'e2e/Operation/Shipment/Business/**/**.js',

        // ********************* Maintenance/Partners **********************************
        customers: 'e2e/Maintenance/Partners/**/Customer.js',
        agents: 'e2e/Maintenance/Partners/**/Agent.js',
        airlines: 'e2e/Maintenance/Partners/**/AirLine.js',

        // ********************* Maintenance/Billings **********************************
        incoterms: 'e2e/Maintenance/Billings/**/Incoterms.js',
        currencies: 'e2e/Maintenance/Billings/**/Currencies.js',
        measurements: 'e2e/Maintenance/Billings/**/Measurements.js',
        chargestypes: 'e2e/Maintenance/Billings/**/ChargesTypes.js',

        // ********************* Maintenance/Locations **********************************
        ports: 'e2e/Maintenance/Loacations/**/Ports.js',
        countries: 'e2e/Maintenance/Loacations/**/Countries.js',
        branches: 'e2e/Maintenance/Loacations/**/Branches.js',
        states: 'e2e/Maintenance/Loacations/**/States.js',
        cities: 'e2e/Maintenance/Loacations/**/Cities.js',
        maintenance: 'e2e/Maintenance/**/*.js',
        countrycity: './**/CountiesAndCities.js',

        //pass:'test/login/**/forgot_password_spec.js',
        //search: ['tests/e2e/contact_search/**/*Spec.js']
    },

}
