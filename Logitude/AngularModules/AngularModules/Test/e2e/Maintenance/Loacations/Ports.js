
// spec.js
describe('Logitude Protractor Testing', function () {
    it('Login', function () {
        Login();
    });
    it('add country', function () {
        GoToMaintenance();
        GoToLocation();
        ClickPorts();
        NewPort();
       // FillPortField();

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
    function GoToLocation() {
        let list = element.all(by.css('.PagesMenu li'));
        expect(list.get(2).getText()).toBe('Locations');
        list.get(2).getText().click();
        browser.driver.sleep(1000);
        //element(by.css('.PagesMenu')).click();
        //locationTab.click();

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.BoxItem'));
        }, 5000);
    }
    function ClickPorts() {
        //browser.driver.sleep(1000);
        var ports = element(by.cssContainingText('.Title', 'Ports'));
        ports.click();
        browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.IconButton'));
        }, 4000);
    }
    function NewPort() {
        //browser.driver.sleep(1000);
        var newPort = element(by.cssContainingText('.Button', 'New Port'));
        newPort.click();
        browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.RedButton'));
        }, 4000);
    }
    //function FillPortField() {
    //    browser.driver.sleep(1000);
    //    var code = element(by.id('textbox_1')).sendKeys('L3');
    //    browser.driver.sleep(1000);
    //    var name = element(by.id('textbox_2')).sendKeys('Ram122');
    //    browser.driver.sleep(1000);
    //    var localName = element(by.id('textbox_3')).sendKeys('ramallh');
    //    browser.driver.sleep(1000);

    //    var Country = element(by.css('.LogLovButton')).click();
    //    browser.driver.sleep(2000);
    //    element(by.cssContainingText('.DropDownListItem', 'AS')).click();
    //    browser.driver.sleep(2000);


    //    var okBtn = element(by.cssContainingText('.RedButton', 'Ok'));
    //    okBtn.click();
    //    browser.driver.sleep(2000);

    //    browser.driver.wait(function () {
    //        return browser.driver.isElementPresent(by.css('.Label'));
    //    }, 4000);
    //}


});


