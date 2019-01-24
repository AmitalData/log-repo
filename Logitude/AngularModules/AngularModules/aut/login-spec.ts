
import { browser, by, element, WebDriver, protractor } from 'protractor';
describe("logitudelogin", () => {

    //function to wait and then fill fields
    var WaitByIdAndFill = (elemntid: string, value: string) => {

        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.id(elemntid))), 100000000).then(() => {
            var input = element(by.id(elemntid));
            input.clear();
            browser.wait(EC.textToBePresentInElementValue(element(by.id(elemntid)), ''), 10000000).then(a => { });
            input.clear();
            input.sendKeys(value);

        })
    };

    var WaitByNameAndFill = (name: string, value: string) => {

        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.name(name))), 100000000).then(() => {
            var input = element(by.name(name));
            input.clear();
            browser.wait(EC.textToBePresentInElementValue(element(by.name(name)), ''), 10000000).then(a => { });
            input.clear();
            input.sendKeys(value);

        })
    };

    //function for login button
    var clickbtn = (elemntid: string) => {

        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.id(elemntid))), 100000000).then(a => {
            element(by.id(elemntid)).click();

        });


    }

    var clickBtnByName = (elName: string) => {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.name(elName))), 100000000).then(a => {
            element(by.name(elName)).click();

        });
    }
    //function if it clickable 

    var WaitByCssAndClick = (className: string, index: number) => {// the item exists in a tag inside li
        var EC = protractor.ExpectedConditions;

        browser.wait(EC.elementToBeClickable(element(by.css(className))), 100000000).then(a => {
            element.all(by.css(className)).get(index).click();
        });
    }
    // to click on new button using name 

    var WaitByNameAndClick = (Name: string) => {// the item exists in a tag inside li
        var EC = protractor.ExpectedConditions;

        browser.wait(EC.elementToBeClickable(element(by.name(Name))), 100000000).then(a => {
            element.all(by.name(Name)).click();
        });
    }

    //


    //do that befor invoking URL
    beforeEach(() => {
        browser.driver.manage().window().maximize();

    });

    var WaitByIdAndClick = (Id: string) => {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.id(Id))), 100000000).then(a => {
            element(by.id(Id)).click();

        });

    }

    //to choose from li
    var WaitByCssAndClick_FromTagInsideList = (className: string, index: number) => {
        var EC = protractor.ExpectedConditions;

        browser.wait(EC.elementToBeClickable(element(by.css(className))), 100000000).then(a => {
            element.all(by.css(className)).get(index).click();
        });
    }
    //this is to choose something by id from li
    var WaitByIdAndClick_FromTagInsideList = (Id: string, index: number) => {
        var EC = protractor.ExpectedConditions;

        browser.wait(EC.elementToBeClickable(element(by.css(Id))), 100000000).then(a => {
            element.all(by.css(Id)).get(index).click();
        });
    }

    //then do all the test cases

    it('Login', function () {
        browser.ignoreSynchronization = true;

        browser.get('http://test.logitudeworld.com/test?Menu=protractor');

        WaitByIdAndFill('Email', 'raghad@logitudeworld.com');
        WaitByIdAndFill('Password', '!RS123RS');
        clickbtn('cmdLogin');
        //here to choose tanent
        WaitByNameAndFill('cmbTenants_input', 'Angular (Business Package) (951) (951)');
        clickbtn('cmdContinue');
        WaitByIdAndClick('PAR');
        clickbtn('General.MH.CRM');
        //WaitByIdAndClick('NEWACTIVITY');

        //this for new customer 
        WaitByIdAndClick('CRMCUS');
        WaitByIdAndClick('NewCustomer');

        // WaitByIdAndClick('NEWTASK');

        //this is also for creat new customer 
        WaitByIdAndFill('Customer_EnglishName', 'Raghad321');
        WaitByIdAndFill('Contact_EnglishName', 'Sous')
        WaitByIdAndFill('Customer_CountryId_Potential', 'R')
        // clickBtnByName('Romania');
        WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 1)
        WaitByIdAndClick('Ok-AddPotCustomer');
       
      //  WaitByIdAndClick('General.MH.TimeManagement')
        //Open Quote 
     // WaitByIdAndClick_FromTagInsideList('General.MH.TimeManagement',2);
        //here he will stop testing 

        //WaitByIdAndFill('Customer_VatNumber','232');


        // clickbtn("General.MH.CRM")





    });











});