"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper = /** @class */ (function () {
    function FieldsHelper() {
    }
    FieldsHelper.prototype.WaitByCssStringAndClick = function (className, Text) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.cssContainingText(className, Text))), 100000).then(function (a) {
            var Button = protractor_1.element(protractor_1.by.cssContainingText(className, Text)).click();
        });
    };
    FieldsHelper.prototype.WaitByIdAndClick = function (Id) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.id(Id))), 1000000).then(function (a) {
            protractor_1.element(protractor_1.by.id(Id)).click();
        });
    };
    FieldsHelper.prototype.WaitByIdAndClickRabaia = function (Id) {
        var EC = protractor_1.protractor.ExpectedConditions;
        return protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.id(Id))), 1000000);
    };
    FieldsHelper.prototype.ItemsVisibility = function (Id) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.visibilityOf(protractor_1.element(protractor_1.by.id(Id))), 1000000).then(function (a) { return function () {
        }; });
    };
    FieldsHelper.prototype.ItemsPresent = function (Id) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.presenceOf(protractor_1.element(protractor_1.by.id(Id))), 1000000).then(function (a) { return function () {
        }; });
    };
    // WaitBusyIndicator(){
    //   var EC = protractor.ExpectedConditions;
    //     browser.wait(EC.invisibilityOf(element(by.css(".BusyIndicatorControl"))), 100000).then(a=>{                             
    //     });
    // }
    FieldsHelper.prototype.WaitBusyIndicator = function () {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.invisibilityOf(protractor_1.element(protractor_1.by.id("BusyIndecator"))), 100000).then(function (a) { });
    };
    FieldsHelper.prototype.WaitBusyIndicatorToShow = function () {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.visibilityOf(protractor_1.element(protractor_1.by.id("BusyIndecator"))), 100000).then(function (a) { });
    };
    FieldsHelper.prototype.WaitWindowClosed = function () {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.invisibilityOf(protractor_1.element(protractor_1.by.css(".LogitudeWindow"))), 100000)
            .then(function (a) { });
    };
    FieldsHelper.prototype.waitByCss = function (className) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.css(className))), 100000000).then(function (a) {
            return true;
        });
    };
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
    FieldsHelper.prototype.CheckIfChanged = function (Id, Text1) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.textToBePresentInElement(protractor_1.$(Id), ('Text')), 5000);
    };
    FieldsHelper.prototype.WaitByIdAndCheckText = function (Id, Text) {
        //var t = element(by.id(Id)).getAttribute('textContent');
        ////  var EC = protractor.ExpectedConditions;
        //// expect(browser.wait(EC.textToBePresentInElement($(Id), ('Text')), 500000));
        ////expect(t).toBe(Text);
        //browser.wait(expect(t).toBe(Text), 5000);
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.textToBePresentInElementValue(protractor_1.$('#ARInvoiceHeaderStatusName'), 'Unpaid'), 10000).then(function (a) { console.log("this is inside the wait for unpaid " + a); });
    };
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
    FieldsHelper.prototype.WaitByIdAndFill = function (Id, Value) {
        var _this = this;
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.id(Id))), 100000).then(function (a) {
            console.log("this is inside the wait for" + Id);
            var input = protractor_1.element(protractor_1.by.id(Id));
            input.clear().then(function () {
                protractor_1.browser.wait(EC.textToBePresentInElementValue(protractor_1.element(protractor_1.by.id(Id)), '')).then(function (a) {
                    input.clear();
                    input.sendKeys(Value);
                    var Newinput = protractor_1.element(protractor_1.by.id(Id)).getAttribute('value');
                    Newinput.then(function (p) {
                        console.log(p);
                        if (p != Value) {
                            _this.WaitByIdAndFill(Id, Value);
                        }
                    });
                    //});
                });
            });
        });
    };
    FieldsHelper.prototype.WaitByCssAndClick_SelectItemFromList = function (className, index) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.visibilityOf(protractor_1.element(protractor_1.by.css(className))), 100000).then(function (a) {
            var shipment = protractor_1.element(protractor_1.by.css(className)).all(protractor_1.by.tagName('li'));
            shipment.get(index).click();
        });
    };
    FieldsHelper.prototype.WaitByCssAndClick_FromTagInsideList = function (className, index) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.css(className))), 100000).then(function (a) {
            protractor_1.element.all(protractor_1.by.css(className)).get(index).click();
        });
    };
    FieldsHelper.prototype.WaitByCssButtonClick = function (className, Text) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.buttonText(Text))), 100000).then(function (a) {
            var button = protractor_1.element(protractor_1.by.buttonText(Text)).click();
        });
    };
    FieldsHelper.prototype.ButtonClick = function (BtnId) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.id(BtnId))), 100000).then(function (a) {
            var button = protractor_1.element(protractor_1.by.id(BtnId)).click();
            console.log("Button Clicked");
        });
    };
    FieldsHelper.prototype.SmarWait_1 = function (Id, Text) {
        return protractor_1.browser.wait(function () {
            var temp = "";
            var Text1 = protractor_1.element(protractor_1.by.id(Id)).getAttribute('textContent').then(function (OrigionalText) {
                temp = OrigionalText;
                if (Text == OrigionalText) {
                    var EC = protractor_1.protractor.ExpectedConditions;
                    console.log("smart wait trueeeeeeeee");
                    expect(Text).toBe(OrigionalText);
                }
                else {
                    console.log("smart wait falseseeeeeeeee");
                    return false;
                }
            });
        }, 100000);
    };
    FieldsHelper.prototype.SmartWait = function (Id, text) {
        var EC = protractor_1.protractor.ExpectedConditions;
        var testSearchingLookingMethod = function (elementFinder) {
            var searchesForText = function () {
                return elementFinder.getText().then(function (actualTextResultedFromAPromise) {
                    return actualTextResultedFromAPromise;
                });
            };
            return EC.and(EC.presenceOf(elementFinder), searchesForText);
        };
        return protractor_1.browser.wait(testSearchingLookingMethod(protractor_1.element(protractor_1.by.id(Id))), 1000000000);
        //return element(by.id(Id)).getText().then(function (actualTextResultedFromAPromise) {
        //    return actualTextResultedFromAPromise;
        //});
    };
    return FieldsHelper;
}());
exports.FieldsHelper = FieldsHelper;
//# sourceMappingURL=FieldsHelper.js.map