import { browser, by, element, WebDriver, protractor, $ } from 'protractor';

export class FieldsHelper {
    constructor() {

    }
    WaitByCssStringAndClick(className: string, Text: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.cssContainingText(className, Text)))).then(a => {
            var Button = element(by.cssContainingText(className, Text)).click();
        });
    }
    WaitElementToBeDisplayedInTheList(className: string, Text: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.cssContainingText(className, Text)))).then(a => {
            browser.wait(EC.elementToBeClickable(element(by.cssContainingText(className, Text)))).then(a => {
                browser.wait(EC.presenceOf(element(by.id('ListDataLoaded')))).then(a => function () {
                });
            });
        });
    }
    WaitActionButtonAndClick(containerClassName: string, isLast: boolean) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.className(containerClassName)))).then(a => {
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
        browser.wait(EC.elementToBeClickable(element(by.id(Id)))).then(a => {
            element(by.id(Id)).click();
        });
    }
    ItemsPresentWithOutClick(Id: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.presenceOf(element(by.id(Id)))).then(a => function () {
        });
    }
    ItemsVisibility(Id: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.id(Id)))).then(a => function () {
        });
    }
    ItemsPresent(Id: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.presenceOf(element(by.id(Id)))).then(a => function () {
            browser.wait(EC.visibilityOf(element(by.id(Id))))
        }).then(function () {
            browser.wait(EC.elementToBeClickable(element(by.id(Id)))).then(a => function () {
            });
        });
    }
    ItemsPresentforCSS(CSS: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.presenceOf(element(by.css(CSS)))).then(a => function () {
            browser.wait(EC.visibilityOf(element(by.css(CSS))))
        }).then(function () {
            browser.wait(EC.elementToBeClickable(element(by.css(CSS)))).then(a => function () {
            });
        });
    }
    WaitEditComponentBusyIndicator() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.invisibilityOf(element(by.id("EditComponentBusyIndicator_0")))).then(a => { });
    }

    WaitShowEditComponentBusyIndicator() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.id("EditComponentBusyIndicator_0")))).then(a => { });
    }

    WaitBusyIndicator() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.invisibilityOf(element(by.id("BusyIndicator_0")))).then(a => { });
    }
    WaitBusyIndicatorToShow() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(element(by.id("BusyIndicator_0")))).then(a => { });
    }

    WaitWindowClosed() {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.invisibilityOf(element(by.css(".LogitudeWindow")))).then(a => { });
    }
    waitByCss(className: string) {
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.css(className)))).then(a => {
            return true;
        });
    }
    WaitByIdAndFill(Id: string, Value: string) {
        var EC = protractor.ExpectedConditions;
        this.ItemsPresent(Id);
        browser.wait(EC.elementToBeClickable(element(by.id(Id)))).then(a => {
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
        browser.wait(EC.visibilityOf(element(by.css(className)))).then(a => {
            var shipment = element(by.css(className)).all(by.tagName('li'));
            shipment.get(index).click();
        });
    }
    WaitByCssAndClick_FromTagInsideList(className: string, index: number) {// the item exists in a tag inside li
        var EC = protractor.ExpectedConditions;
        browser.wait(EC.elementToBeClickable(element(by.css(className)))).then(a => {
            element.all(by.css(className)).get(index).click();
        });
    }
    WaitByCssAndClick_FromTagInsideListWithCheck(className: string, index: number, Id: string = null, input: string = null) {
        var EC = protractor.ExpectedConditions;
        this.ItemsPresentforCSS(className);
        browser.wait(EC.elementToBeClickable(element(by.css(className)))).then(a => {
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
            browser.wait(EC.invisibilityOf(element(by.css(className)))).then(a => {
            });
        }
        catch (Exception) {
            console.log("Catch drop down to be closed");
            console.log(Exception);
            this.WaitDropDownToBeClosed(className);
        }
    }
}

