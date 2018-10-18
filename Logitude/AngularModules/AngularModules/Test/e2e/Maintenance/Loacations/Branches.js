// spec.js
describe('Logitude Protractor Testing', function () {
    it('Login', function () {
        Login();
    });
    it('add country', function () {
        GoToMaintenance();
        GoToLocation();
        CLickBranches();
        NewBranches();
        FillBranchField();

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
    function CLickBranches() {
        //browser.driver.sleep(1000);
        var branches = element(by.cssContainingText('.Title', 'Branches'));
        branches.click();
        browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.IconButton'));
        }, 4000);
    }
    function NewBranches() {
        //browser.driver.sleep(1000);
        var newBranches = element(by.cssContainingText('.Button', 'New Branch'));
        newBranches.click();
        browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.RedButton'));
        }, 4000);
    }
    function FillBranchField() {
        browser.driver.sleep(1000);
        var name = element(by.id('textbox_1')).sendKeys('alberh123');
        browser.driver.sleep(1000);
        var localName = element(by.id('textbox_2')).sendKeys('Alberh');

        element(by.css('.CheckBox')).click();
        browser.driver.sleep(2000);

        var okBtn = element(by.cssContainingText('.RedButton', 'Ok'));
        okBtn.click();
        browser.driver.sleep(2000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.Label'));
        }, 4000);
    }


});


