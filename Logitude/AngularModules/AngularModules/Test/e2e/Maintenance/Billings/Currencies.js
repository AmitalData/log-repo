
// spec.js
describe('Logitude Protractor Testing', function () {
    it('Login', function () {
        Login();
    });
    it('add incoterm', function () {
        GoToMaintenance();
        GoToBillings();
        CLickCurrencies();
        NewCurrencies();
        FillCourenciesField();
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
        }, 40000);


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

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.BoxItem'));
        }, 5000);
    }
    function CLickCurrencies() {
        browser.driver.sleep(1000);
        var Currencies = element(by.cssContainingText('.Title', 'Currencies'));
        Currencies.click();
        browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.IconButton'));
        }, 4000);
    }
    function NewCurrencies() {
        browser.driver.sleep(1000);
        var newCurrencies = element(by.cssContainingText('.Button', 'New Currency'));
        newCurrencies.click();
        browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.RedButton'));
        }, 4000);
    }
    function FillCourenciesField() {
        browser.driver.sleep(1000);
        var currency = element(by.id('Search - CurrencyId-1')).sendKeys("w");
        browser.driver.sleep(2000);
        element(by.cssContainingText('.DropDownListItem', 'ILS')).click();
        browser.driver.sleep(2000);

        var exchangeRate = element(by.id('textbox_2')).sendKeys('10');
        browser.driver.sleep(1000);

        var exchangeRateDate = element(by.css('.CalendarButton')).click();
        browser.driver.sleep(1000);
        element(by.cssContainingText('.DayCellDev', '29')).click();
        browser.driver.sleep(2000);

        var okBtn = element(by.cssContainingText('.RedButton', 'Ok'));
        okBtn.click();
        browser.driver.sleep(2000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.Label'));
        }, 4000);
    }


});