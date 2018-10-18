
// spec.js
describe('Logitude Protractor Testing', function () {
    it('Login', function () {
        Login();
    });
    it('add incoterm', function () {
        GoToMaintenance();
        GoToBillings();
        CLickMeasurements();
      
    });


    function Login() {

        browser.ignoreSynchronization = true;
        browser.driver.get('https://system.logitudeworld.com/login.aspx');
        browser.driver.sleep(1000);

        browser.driver.findElement(by.id('Email')).sendKeys('razan@fnarsoft.com');
        browser.driver.findElement(by.id('Password')).sendKeys('!R123456');

        browser.driver.findElement(by.id('cmdLogin')).click();
        browser.driver.sleep(1000);

        browser.driver.findElement(by.css('.promptButton')).click();
        browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.DefaultMenuItem'));
        }, 60000);


    }
    function GoToMaintenance() {
        browser.driver.sleep(1000);
        var menuItem = element(by.cssContainingText('.DefaultMenuItem', 'Maintenance')).click();
        //browser.driver.sleep(1000);
        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.PagesMenu'));
        }, 5000);
    }
    function GoToBillings() {
        let list = element.all(by.css('.PagesMenu li'));
        expect(list.get(1).getText()).toBe('Billings');
        list.get(1).getText().click();
        browser.driver.sleep(1000);
        //element(by.css('.PagesMenu')).click();
        //locationTab.click();

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.BoxItem'));
        }, 5000);
    }
    function CLickMeasurements() {
        //browser.driver.sleep(1000);
        var Measurements = element(by.cssContainingText('.Title', 'Measurements'));
        Measurements.click();
        browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.IconButton'));
        }, 4000);
    }
    function NewMeasurements() {
       // does not implemented yet 
    }

});