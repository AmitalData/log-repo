var Helper = require('../../../Helper.js');
var Helper = new Helper();
// spec.js
describe('Logitude Protractor Testing', function () {
    it('Login', function () {
        Login();
    });
    it('add country', function () {
        GoToMaintenance();
        GoToLocation();
        CLickCities();
        NewCity();
        FillCityField();
       
    });



    function Login() {

        browser.ignoreSynchronization = true;
        Helper.login();

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.DefaultMenuItem'));
        }, 60000);


    }

    function GoToMaintenance() {
        //browser.driver.sleep(1000);
        var menuItem = element(by.cssContainingText('.DefaultMenuItem', 'Maintenance')).click();
        //browser.driver.sleep(1000);
        Helper.waitByCss('.PagesMenu', 4000);
    }

    function GoToLocation() {
        let list = element.all(by.css('.PagesMenu li'));
        //expect(list.get(2).getText()).toBe('Locations');
        list.get(2).click();
        //browser.driver.sleep(1000);
        //element(by.css('.PagesMenu')).click();
        //locationTab.click();
        Helper.waitByCss('.BoxItem', 4000);
    }

    function CLickCities() {
        //browser.driver.sleep(1000);
        var countries = element(by.cssContainingText('.Title', 'Cities'));
        countries.click();
        //browser.driver.sleep(1000);
        Helper.waitByCss('.IconButton', 4000);

    }
    function NewCity() {
        //browser.driver.sleep(1000);
        var newCountry = element(by.cssContainingText('.Button', 'New City'));
        newCountry.click();
        //browser.driver.sleep(1000);

        Helper.waitByCss('.RedButton', 4000);
    }
    function FillCityField() {
        //browser.driver.sleep(1000);
        var code = element(by.id('CountryCity_Code')).sendKeys('L2');
        //browser.driver.sleep(1000);
        var name = element(by.id('CountryCity_EnglishName')).sendKeys('Ramallh122');
        //browser.driver.sleep(1000);
        var localName = element(by.id('CountryCity_LocalName')).sendKeys('ramallh');
        //browser.driver.sleep(1000);

        var Country = element(by.id('CountryCity_CountryId')).sendKeys('us');
        //browser.driver.sleep(2000);
        Helper.waitByCss('.DropDownListItem', 2000);
        element(by.cssContainingText('.DropDownListItem', 'US')).click();
        //browser.driver.sleep(2000);

        var inActive = element(by.css('.CheckBox')).click();

        var okBtn = element(by.cssContainingText('.RedButton', 'Ok'));
        okBtn.click();
        //browser.driver.sleep(2000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.Label'));
        }, 4000);
    }
});


