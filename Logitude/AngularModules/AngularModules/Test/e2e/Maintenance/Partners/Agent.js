

// spec.js
describe('Logitude Protractor Testing', function () {

    it('Login', function() {
        Login();

    });
    it('Fill RequiredField in customer scenario', function () {
        GoToMaintenance();
        ClickAgent();
        NewAgent();
        FillAgentRequire();
        //browser.driver.sleep(2000);		
    });
    it('Fill All field in agent scenario', function () {
        browser.driver.sleep(2000);
        NewAgent();
        FillAllAgentField();
    });
    it('Fill Contact in agent scenario', function () {
        browser.driver.sleep(2000);
        NewAgent();
        FillAgentWithContact();
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
    function ClickAgent() {
        //browser.driver.sleep(1000);
        var Agents = element(by.cssContainingText('.Title', 'Agents'));
        Agents.click();
        //browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.IconButton'));
        }, 20000);
    }
    function NewAgent() {
        //browser.driver.sleep(1000);
        var newAgent = element(by.cssContainingText('.Button', 'New Agent'));
        newAgent.click();
        //browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.RedButton'));
        }, 20000);
    }
  
    function FillAgentRequire() {
        browser.driver.sleep(1000);
        var companyName = element(by.id('textbox_1')).sendKeys('Fanar_26');
        browser.driver.sleep(1000);
        var City = element(by.id('textbox_5')).sendKeys('926');
        browser.driver.sleep(1000);

        var Country = element(by.id('Search - CountryId-6')).sendKeys('ae');
        browser.driver.sleep(1000);

        element(by.cssContainingText('.DropDownListItem', 'United Arab Emirates')).click();
        browser.driver.sleep(2000);

        var okBtn = element(by.cssContainingText('.RedButton', 'Ok'));
        okBtn.click();

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.Label'));
        }, 4000);
    }
    function FillAllAgentField() {
        browser.driver.sleep(1000);
        var companyName = element(by.id('textbox_16')).sendKeys('AllComp');
        browser.driver.sleep(1000);

        var address1 = element(by.id('textbox_17')).sendKeys('alberh');
        browser.driver.sleep(1000);

        var address2 = element(by.id('textbox_18')).sendKeys('Betonya');
        browser.driver.sleep(1000);

        var zipCode = element(by.id('textbox_19')).sendKeys('11');
        browser.driver.sleep(1000);

        var City = element(by.id('textbox_20')).sendKeys('Nablus');
        browser.driver.sleep(1000);

        var Country = element(by.id('Search - CountryId-21')).sendKeys('ae');
        browser.driver.sleep(1000);

        element(by.cssContainingText('.DropDownListItem', 'United Arab Emirates')).click();
        browser.driver.sleep(1000);

        var phone = element(by.id('textbox_23')).sendKeys('11');
        browser.driver.sleep(1000);

        var fax = element(by.id('textbox_24')).sendKeys('11');
        browser.driver.sleep(1000);


        var okBtn = element(by.cssContainingText('.RedButton', 'Ok'));
        okBtn.click();

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.Label'));
        }, 4000);
    }
    function FillAgentWithContact() {

        browser.driver.sleep(1000);
        var companyName = element(by.id('textbox_31')).sendKeys('ToTestValid');
        browser.driver.sleep(1000);
        var City = element(by.id('textbox_35')).sendKeys('926');
        browser.driver.sleep(1000);


        var Country = element(by.id('Search - CountryId-36')).sendKeys('ae');
        browser.driver.sleep(1000);

        element(by.cssContainingText('.DropDownListItem', 'United Arab Emirates')).click();
        browser.driver.sleep(1000);


        //var Country=element(by.css('.LogLovButton')).click();
        //browser.driver.sleep(2000);
        //element(by.cssContainingText('.DropDownListItem','FJ')).click();
        //browser.driver.sleep(2000);

        element(by.css('.CheckBox')).click();
        browser.driver.sleep(2000);

        var Email = element(by.id('textbox_40')).sendKeys("RazanJararah");
        browser.driver.sleep(1000);

        var englishName = element(by.id('textbox_41')).sendKeys("RazanJararah");
        browser.driver.sleep(1000);

        var position = element(by.id('textbox_42')).sendKeys('eng');
        browser.driver.sleep(1000);

        var businessPhone = element(by.id('textbox_43')).sendKeys('35');
        browser.driver.sleep(1000);

        var mobile = element(by.id('textbox_44')).sendKeys('123456444');
        browser.driver.sleep(1000);

        var fax = element(by.id('textbox_45')).sendKeys('254');
        browser.driver.sleep(1000);

        var okBtn = element(by.cssContainingText('.RedButton', 'Ok'));
        okBtn.click();

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.Labela'));
        }, 4000);

    }
     
});