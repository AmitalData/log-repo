
// spec.js
describe('Logitude Protractor Testing', function () {
    it('Login', function () {
        Login();
    });
    it('add incoterm', function () {
        GoToMaintenance();
        GoToBillings();
        ClickIncoterms();
        NewIncoterms();
        FillIncotermField();
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
        }, 10000);


    }
    function GoToMaintenance() {
        browser.driver.sleep(1000);
        var menuItem = element(by.cssContainingText('.DefaultMenuItem', 'Maintenance')).click();
        //browser.driver.sleep(1000);
        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.PagesMenu'));
        }, 2000);
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
    function ClickIncoterms() {
        //browser.driver.sleep(1000);
        var Incoterms = element(by.cssContainingText('.Title', 'Incoterms'));
        Incoterms.click();
        browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.IconButton'));
        }, 4000);
    }
    function NewIncoterms() {
        browser.driver.sleep(1000);
        var newIncoterms = element(by.cssContainingText('.Button', 'New Incoterm'));
        newIncoterms.click();
        browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.RedButton'));
        }, 4000);
    }
    function FillIncotermField() {
        browser.driver.sleep(1000);
        var code = element(by.id('textbox_1')).sendKeys('L3');
        browser.driver.sleep(1000);
        var name = element(by.id('textbox_2')).sendKeys('Ramallh133');
        browser.driver.sleep(1000);
        var localName = element(by.id('textbox_3')).sendKeys('incoterm');
        browser.driver.sleep(1000);


        var frieght = element(by.id('Search - Freight-4')).sendKeys('co');
        browser.driver.sleep(2000);
        element(by.cssContainingText('.DropDownListItem', 'Collect')).click();
        browser.driver.sleep(2000);

        var otherCharges = element(by.id('Search - OtherCharges-5')).sendKeys('co');
        browser.driver.sleep(2000);
        element(by.cssContainingText('.DropDownListItem', 'Collect')).click();
        browser.driver.sleep(2000);

        //var frieght = element(by.css('.LogLovButton')).click();
        //browser.driver.sleep(2000);
        //element(by.cssContainingText('.DropDownListItem', 'Collect')).click();
        //browser.driver.sleep(2000);
       
        //var frieght = element(by.id('LogLov - OtherCharges-5')).click();
        //browser.driver.sleep(2000);
        //element(by.cssContainingText('.DropDownListItem', 'Collect')).click();
        //browser.driver.sleep(2000);

        var okBtn = element(by.cssContainingText('.RedButton', 'Ok'));
        okBtn.click();
        browser.driver.sleep(2000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.Label'));
        }, 4000);
    }

});