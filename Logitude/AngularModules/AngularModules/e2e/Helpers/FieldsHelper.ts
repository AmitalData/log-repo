import { browser, by, element, WebDriver, protractor, $ } from 'protractor';

export class FieldsHelper {
    constructor() {

    }

    WaitByCssStringAndClick(className: string, Text: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.cssContainingText(className, Text))), 100000000).then(a => {
            var Button = element(by.cssContainingText(className, Text)).click();

        });
    }

    WaitByIdAndClick(Id: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.id(Id))), 10000000).then(a => {
            element(by.id(Id)).click();

        });

    }

    WaitByIdAndClickRabaia(Id: string) {
        var EC = protractor.ExpectedConditions;
        return browser.wait(EC.elementToBeClickable(element(by.id(Id))), 10000000);

    }

    ItemsVisibility(Id: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.id(Id))), 100000000).then(a => function () {
        });
    }
    ItemsPresent(Id: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.presenceOf(element(by.id(Id))), 1000000000).then(a => function () {

        });
    }
    // WaitBusyIndicator(){
    //   var EC = protractor.ExpectedConditions;
    //     browser.wait(EC.invisibilityOf(element(by.css(".BusyIndicatorControl"))), 100000).then(a=>{                             
    //     });
    // }

    WaitBusyIndicator() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.invisibilityOf(element(by.id("BusyIndecator"))),10000000).then(a => { });
    }


    WaitBusyIndicatorToShow() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.id("BusyIndecator"))), 100000000).then(a => { });
    }

    WaitWindowClosed() {
        var EC = protractor.ExpectedConditions;
        browser.wait(
            EC.invisibilityOf(element(by.css(".LogitudeWindow"))), 100000000)
            .then(a => { });
    }

    //WaitWindowClosedRabaia() {
    //    var EC = protractor.ExpectedConditions;
    //    return browser.wait(
    //        EC.invisibilityOf(element(by.css(".LogitudeWindow"))));
    //}

    //RabaiaWait(Text: string, Id: string) {
    //    var temp = "";
    //    var Text1 = element(by.id(Id)).getAttribute('textContent').then(function (OrigionalText) {
    //        temp = OrigionalText;
    //        if (Text == OrigionalText) {
    //            expect(Text).toBe(OrigionalText);
    //            this.Retries = 6;
    //        }
    //    });

    //}

    //private timerToken: any;
    //Retries = 0;
    //private RunComponentTimer(Id: string, Text: string) {
    //    this.Retries++;

    //    if (this.timerToken) {
    //        clearTimeout(this.timerToken);
    //    }

    //    if (this.Retries < 6) {
    //        this.timerToken = setTimeout(() => this.RabaiaWait(Text, Id), 1);
    //      //  console.log("Rabaia " + this.Retries);
    //    }
    //}

    CheckIfChanged(Id: string, Text1: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.textToBePresentInElement($(Id), ('Text')), 5000);
    }
    WaitByIdAndCheckText(Id: string, Text: string) {
        //var t = element(by.id(Id)).getAttribute('textContent');
        ////  var EC = protractor.ExpectedConditions;
        //// expect(browser.wait(EC.textToBePresentInElement($(Id), ('Text')), 500000));
        ////expect(t).toBe(Text);
        //browser.wait(expect(t).toBe(Text), 5000);
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.textToBePresentInElementValue($('#ARInvoiceHeaderStatusName'), 'Unpaid'), 10000).then(a => { console.log("this is inside the wait for unpaid "+a); });

    }

       // this.RunComponentTimer(Id, Text);
         //return browser.wait();
        // var RefreshTimer : any;
        // var Text1=element(by.id(Id)).getAttribute('textContent').then(function (OrigionalText) {            
        //   console.log("Rabaia in promise" + Id + " " + OrigionalText.trim()); 
        //   expect(Text).toBe(OrigionalText);
        // RefreshTimer : any;
        //   if (RefreshTimer) {
        //     clearTimeout(RefreshTimer);
        // }
        // RefreshTimer = setInterval(() => expect(Text).toBe(OrigionalText), 200);
        //expect(OrigionalText).toBe(Text);
        // return receivableCurrency;
        // });



        // browser.wait(


       // console.log("Rabaia out" + Id + " " + Text);
       // console.log( Id + " " + Text);
        // element(by.id(Id)).getAttribute('textContent').then(function (text) {


        // });
        //if (t=='Text')



        //var EC = protractor.ExpectedConditions;
        //String foo1 = element(by.id(Id)).getText();
        //if (foo1=='Text'){


        // var foo= browser.wait(EC.textToBePresentInElement($(Id),('Text')),500000);
        //   if (foo)
        //   {
        //     console.log(Text);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
        //   }
        //   else{
        //     console.log('Not'+' '+Text);
        //   }  
    




    // WaitByIdAndFill(Id: string, Value: string) {
    //   var EC = protractor.ExpectedConditions;
    //   browser.wait(EC.elementToBeClickable(element(by.id(Id))), 10000000).then(a => {
    //     var input = element(by.id(Id));
    //     input.clear();
    //     input.sendKeys(Value);
    //   });
    // }

    WaitByIdAndFill(Id: string, Value: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.id(Id))),10000000000000000).then(a => {
            console.log("this is inside the wait for" + Id);
            var input = element(by.id(Id));
            input.clear().then(() => {

                browser.wait(EC.textToBePresentInElementValue(element(by.id(Id)), '')).then(a => {
                    input.clear();
                    input.sendKeys(Value);
                });
            });


        });
    }

    WaitByCssAndClick_SelectItemFromList(className: string, index: number) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.css(className))), 10000000).then(a => {
            var shipment = element(by.css(className)).all(by.tagName('li'));
            shipment.get(index).click();
        });
    }

    WaitByCssAndClick_FromTagInsideList(className: string, index: number) {// the item exists in a tag inside li
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.css(className))), 500000).then(a => {
            element.all(by.css(className)).get(index).click();
        });
    }

    WaitByCssButtonClick(className: string, Text: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.buttonText(Text))), 100000).then(a => {
            var button = element(by.buttonText(Text)).click();
        });
    }

    public ButtonClick(BtnId: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.id(BtnId))), 100000000).then(a => {
            var button = element(by.id(BtnId)).click();
            console.log("Button Clicked");
        });
    }

    public SmarWait_1(Id: string, Text: string) {
        return browser.wait(function () {
            var temp = "";
            var Text1 = element(by.id(Id)).getAttribute('textContent').then(function (OrigionalText) {
                temp = OrigionalText;
                
                if (Text == OrigionalText) {
                    var EC = protractor.ExpectedConditions;
                    
                    console.log("smart wait trueeeeeeeee");
                    expect(Text).toBe(OrigionalText);
                    
                }
                else {
                    console.log("smart wait falseseeeeeeeee");
                    return false;
                    
                }
            });
        }, 100000);
    }
    public SmartWait(Id: string, text: string) {
        var EC = protractor.ExpectedConditions;
        let testSearchingLookingMethod = function (elementFinder) {
            let searchesForText = function () {
                return elementFinder.getText().then(function (actualTextResultedFromAPromise) {
                    return actualTextResultedFromAPromise;
                });
            };
            return EC.and(EC.presenceOf(elementFinder), searchesForText);
        };
        return browser.wait(testSearchingLookingMethod(element(by.id(Id))), 1000000000);
        //return element(by.id(Id)).getText().then(function (actualTextResultedFromAPromise) {
        //    return actualTextResultedFromAPromise;
        //});


    }
}

