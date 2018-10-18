
// spec.js
describe('Logitude Protractor Testing', function () {
    it('Login', function () {
        Login();
    });
    it('add incoterm', function () {
        GoToMaintenance();
        GoToBillings();
        CLickChargesTypes();
        NewChargesTypes();
        FillChargesTypesField();
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
    function CLickChargesTypes() {
        //browser.driver.sleep(1000);
        var ChargesTypes = element(by.cssContainingText('.Title', 'Charges Types'));
        ChargesTypes.click();
        browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.IconButton'));
        }, 4000);
    }
    function NewChargesTypes() {
        //browser.driver.sleep(1000);
        var newChargesTypes = element(by.cssContainingText('.Button', 'New Charges Type'));
        newChargesTypes.click();
        browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.RedButton'));
        }, 4000);
    }
    function FillChargesTypesField() {
        browser.driver.sleep(1000);
        var code = element(by.id('textbox_1')).sendKeys('L1');
        browser.driver.sleep(1000);
        var name = element(by.id('textbox_2')).sendKeys('Ramallh133');
        browser.driver.sleep(1000);
        var localName = element(by.id('textbox_3')).sendKeys('incoterm');
        browser.driver.sleep(1000);

        var groupeCode = element(by.id('Search - ChargesGroupCode-4')).sendKeys("s");
        browser.driver.sleep(2000);
        element(by.cssContainingText('.DropDownListItem', 'SCH')).click();
        browser.driver.sleep(2000);

        var measurement = element(by.id('Search - MeasurementId-5')).sendKeys("b");
        browser.driver.sleep(2000);
        element(by.cssContainingText('.DropDownListItem', 'BCNT')).click();
        browser.driver.sleep(2000);

        var containerMst = element(by.id('Search - ContainerMeasurementId-6')).sendKeys("b");
        browser.driver.sleep(2000);
        element(by.cssContainingText('.DropDownListItem', 'BCNT')).click();
        browser.driver.sleep(2000);

        //var nextBtn = element(by.cssContainingText('.Button', 'Next'));
        //nextBtn.click();
        //browser.driver.sleep(2000);
        
        var nextBtn = element(by.cssContainingText('.Button', 'Cancel'));
        nextBtn.click();
        browser.driver.sleep(2000);

        //element(by.id('.CheckBox_0_0')).click();
        //browser.driver.sleep(2000);
        //element(by.css('.CheckBox')).click();
        //browser.driver.sleep(2000);
        //var okBtn = element(by.cssContainingText('.RedButton', 'Ok'));
        //okBtn.click();
        //browser.driver.sleep(2000);



        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.Labels'));
        }, 4000);
    }

});