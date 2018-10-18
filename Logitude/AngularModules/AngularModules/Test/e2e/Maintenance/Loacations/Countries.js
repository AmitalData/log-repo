
// spec.js
describe('Logitude Protractor Testing', function () {
    it('Login', function () {
        Login();
    });
    it('add country', function () {
        GoToMaintenance();
        GoToLocation();
        CLickCountries();
        NewCountry();
        FillCountryField();
        //ClickBackBtn();
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
        list.get(2).click();
        browser.driver.sleep(1000);
        //element(by.css('.PagesMenu')).click();
        //locationTab.click();

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.BoxItem'));
        }, 5000);
    }
    function CLickCountries() {
        //browser.driver.sleep(1000);
        var countries = element(by.cssContainingText('.Title', 'Countries'));
        countries.click();
        browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.IconButton'));
        }, 4000);
    }
    function NewCountry() {
        //browser.driver.sleep(1000);
        var newCountry = element(by.cssContainingText('.Button', 'New Country'));
        newCountry.click();
        //browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.RedButton'));
        }, 4000);
    }
    function FillCountryField() {
        browser.driver.sleep(1000);
        var code = element(by.id('textbox_1')).sendKeys('L8');
        browser.driver.sleep(1000);
        var name = element(by.id('textbox_2')).sendKeys('321ram');
        browser.driver.sleep(1000);
        var localName = element(by.id('textbox_3')).sendKeys('PalestineWATAN');
        browser.driver.sleep(1000);

        var globalZone = element(by.id('Search - GlobalZoneId-4')).sendKeys("a");
        browser.driver.sleep(2000);
        element(by.cssContainingText('.DropDownListItem', 'AF')).click();
        browser.driver.sleep(2000);

        var inActive = element(by.css(".CheckBox")).click();

        //var EC = element.all(by.css(".CheckBox")).click();
        //element.get(1)

       // let EC = element.all(by.css('.CheckBox'));
       //// expect(list.get(0).getText()).toBe('Locations');
       // list.get(0).click();

        //var isStateRequired = element(by.id("CheckBox_0_2")).click();
        //var hasCities = element(by.id("CheckBox_0_3")).click();

        var okBtn = element(by.cssContainingText('.RedButton', 'Ok'));
        okBtn.click();

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.Label1'));
        }, 4000);
    }


});


