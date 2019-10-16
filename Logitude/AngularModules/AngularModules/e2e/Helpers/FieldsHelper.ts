import { browser, by, element, WebDriver, protractor, $ } from 'protractor';

export class FieldsHelper {
    constructor() {

    }

    WaitByCssStringAndClick(className: string, Text: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.cssContainingText(className, Text))), 100000).then(a => {
            var Button = element(by.cssContainingText(className, Text)).click();

        });
    }

    WaitActionButtonAndClick(containerClassName: string, isLast: boolean) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.className(containerClassName))), 100000).then(a => {
            let last = element.all(by.className(containerClassName)).last();
            browser.actions().mouseMove(last).perform();
            var allBtns = last.all(by.css('.ActionButtons'));
            var btnToClick = null;
            if (isLast) {
                btnToClick = allBtns.last();
            } else {
                btnToClick = allBtns.first();
            }
            btnToClick.click();
        });
    }

    WaitByIdAndClick(Id: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.id(Id))), 100000000).then(a => {
            element(by.id(Id)).click();
        });
    }


    ItemsVisibility(Id: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.id(Id))), 1000000).then(a => function () {
        });
    }
    ItemsPresent(Id: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.presenceOf(element(by.id(Id))), 1000000).then(a => function () {

        });
    }


    WaitBusyIndicator() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.invisibilityOf(element(by.id("BusyIndicator_0"))), 100000).then(a => { });
    }
    WaitEditComponentBusyIndicator() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.invisibilityOf(element(by.id("EditComponentBusyIndicator_0"))), 100000).then(a => { });
    }

    WaitShowEditComponentBusyIndicator() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.id("EditComponentBusyIndicator_0"))), 100000).then(a => { });
    }
    WaitBusyIndicatorToShow() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.id("BusyIndicator_0"))), 100000).then(a => { });
    }

    WaitWindowClosed() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.invisibilityOf(element(by.css(".LogitudeWindow"))), 100000).then(a => { });
    }
    waitByCss(className: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.css(className))), 100000000).then(a => {
            return true;
        });
    }
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
        browser.wait(EC.textToBePresentInElementValue($('#ARInvoiceHeaderStatusName'), 'Unpaid'), 10000).then(a => { console.log("this is inside the wait for unpaid " + a); });

    }
    WaitByIdAndFill(Id: string, Value: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.id(Id))), 10000).then(a => {
            console.log("this is inside the wait for " + Id);
            var input = element(by.id(Id));
            input.clear().then(() => {

                browser.wait(EC.textToBePresentInElementValue(element(by.id(Id)), '')).then(a => {
                    input.clear();
                    input.sendKeys(Value);
                    var Newinput = element(by.id(Id)).getAttribute('value');
                    Newinput.then(p => {// This adjustment is added because sometimes the text is not filled correctly so this way makes sure that is the value we fill
                        console.log(p);
                        if (p != Value) {
                            this.WaitByIdAndFill(Id, Value);

                        }
                    });
                    //});
                });
            });
        });
    }
    WaitByCssAndClick_SelectItemFromList(className: string, index: number) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.css(className))), 100000).then(a => {
            var shipment = element(by.css(className)).all(by.tagName('li'));
            shipment.get(index).click();
        });
    }
    WaitByCssAndClick_FromTagInsideList(className: string, index: number) {// the item exists in a tag inside li
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.css(className))), 100000).then(a => {
            element.all(by.css(className)).get(index).click();
        });
    }
    WaitByCssAndClick_FromTagInsideListWithCheck(className: string, index: number, Id: string = null, input: string = null) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.css(className))), 100000).then(a => {
            var item = element.all(by.css(className)).get(index);
            if (item == null) {
                this.WaitByCssAndClick_FromTagInsideListWithCheck(className, index, Id, input);
            }
            else {
                item.click();
                var Newinput = element(by.id(Id)).getAttribute('value');
                Newinput.then(p => {
                    if (p == "") {
                        this.WaitByIdAndFill(Id, input);
                        this.WaitByCssAndClick_FromTagInsideListWithCheck(className, index, Id, input);
                    } else {
                        this.WaitDropDownToBeClosed(className);
                        this.WaitBusyIndicator();
                        // if(EC.visibilityOf(element(by.css(className)))){
                        //     this.WaitByCssAndClick_FromTagInsideListWithCheck(className, index, Id, input);
                        // }
                        // browser.wait(EC.invisibilityOf(element(by.css(className))), 100000).then(a => {
                        // });
                        // if(EC.invisibilityOf(element(by.cs(className))).){
                        //     console.log('element is selected ');

                        // }else{
                        //     this.WaitByCssAndClick_FromTagInsideListWithCheck(className, index, Id, input);

                        // }
                    }
                });
            }
        });
    }
    WaitDropDownToBeClosed(className: string) {
        var EC = protractor.ExpectedConditions;
        try {
            browser.wait(EC.invisibilityOf(element(by.css(className))), 100000).then(a => {
            });
        }
        catch (Exception) {
            console.log("Ayman Catch");
            console.log(Exception);
            this.WaitDropDownToBeClosed(className);
        }
    }


    WaitByCssButtonClick(className: string, Text: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.buttonText(Text))), 100000).then(a => {
            var button = element(by.buttonText(Text)).click();
        });
    }



}

