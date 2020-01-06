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
    WaitElementToBeDisplayedInTheList(className: string, Text: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.cssContainingText(className, Text))), 100000).then(a => {
            browser.wait(EC.elementToBeClickable(element(by.cssContainingText(className, Text))), 100000).then(a => {
                browser.wait(EC.presenceOf(element(by.id('ListDataLoaded'))), 1000000).then(a => function () {
                });
            });
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
        this.ItemsPresent(Id);
        this.ItemsVisibility(Id);
        browser.wait(EC.elementToBeClickable(element(by.id(Id))), 100000000).then(a => {
            element(by.id(Id)).click();
        });
    }


    ItemsVisibility(Id: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.id(Id))), 100000000).then(a => function () {
        });
    }

    ItemsPresent(Id: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.presenceOf(element(by.id(Id))), 100000000).then(a => function () {

        });
    }

    WaitEditComponentBusyIndicator() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.invisibilityOf(element(by.id("EditComponentBusyIndicator_0"))), 100000000).then(a => { });
    }

    WaitShowEditComponentBusyIndicator() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.id("EditComponentBusyIndicator_0"))), 100000000).then(a => { });
    }

    WaitBusyIndicator() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.invisibilityOf(element(by.id("BusyIndicator_0"))), 100000000).then(a => { });
    }
    WaitBusyIndicatorToShow() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.id("BusyIndicator_0"))), 100000000).then(a => { });
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
                    Newinput.then(p => {
                        console.log(p);
                        if (p != Value) {
                            this.WaitByIdAndFill(Id, Value);

                        }
                    });
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
                try {
                    item.click();
                }
                catch (Exception) {
                    console.log(Exception);
                    this.WaitByCssAndClick_FromTagInsideListWithCheck(className, index, Id, input);
                }
                var Newinput = element(by.id(Id)).getAttribute('value');
                Newinput.then(p => {
                    if (p == "") {
                        this.WaitByIdAndFill(Id, input);
                        this.WaitByCssAndClick_FromTagInsideListWithCheck(className, index, Id, input);
                    } else {
                        this.WaitDropDownToBeClosed(className);
                        this.WaitBusyIndicator();
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
            console.log("Catch drop down to be closed");
            console.log(Exception);
            this.WaitDropDownToBeClosed(className);
        }
    }





}

