"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper = /** @class */ (function () {
    function FieldsHelper() {
        this.WaitByNameAndFill = function (name, value) {
            var EC = protractor_1.protractor.ExpectedConditions;
            protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.name(name))), 100000000).then(function () {
                var input = protractor_1.element(protractor_1.by.name(name));
                input.clear();
                protractor_1.browser.wait(EC.textToBePresentInElementValue(protractor_1.element(protractor_1.by.name(name)), ''), 10000000).then(function (a) { });
                input.clear();
                input.sendKeys(value);
            });
        };
    }
    FieldsHelper.prototype.WaitByCssStringAndClick = function (className, Text) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.cssContainingText(className, Text))), 100000000).then(function (a) {
            var Button = protractor_1.element(protractor_1.by.cssContainingText(className, Text)).click();
        });
    };
    FieldsHelper.prototype.WaitByIdAndClick = function (Id) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.id(Id))), 100000).then(function (a) {
            protractor_1.element(protractor_1.by.id(Id)).click();
        });
    };
    FieldsHelper.prototype.ItemsVisibility = function (Id) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.visibilityOf(protractor_1.element(protractor_1.by.id(Id))), 5000).then(function (a) { return function () {
        }; });
    };
    FieldsHelper.prototype.ItemsAvailability = function (Id) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.id(Id))), 5000).then(function (a) { return function () {
        }; });
    };
    // WaitBusyIndicator(){
    //   var EC = protractor.ExpectedConditions;
    //     browser.wait(EC.invisibilityOf(element(by.css(".BusyIndicatorControl"))), 100000).then(a=>{
    //     });
    // }
    FieldsHelper.prototype.WaitBusyIndicator = function () {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.invisibilityOf(protractor_1.element(protractor_1.by.id("BusyIndecator"))), 100000).then(function (a) {
        });
    };
    FieldsHelper.prototype.WaitWindowClosed = function () {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.invisibilityOf(protractor_1.element(protractor_1.by.css(".LogitudeWindow"))), 100000000).then(function (a) {
        });
    };
    // WaitByIdAndFill(Id: string, Value: string) {
    //   var EC = protractor.ExpectedConditions;
    //   browser.wait(EC.elementToBeClickable(element(by.id(Id))), 10000000).then(a => {
    //     var input = element(by.id(Id));
    //     input.clear();
    //     input.sendKeys(Value);
    //   });
    // }
    FieldsHelper.prototype.WaitByIdAndFill = function (Id, Value) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.id(Id))), 100000000).then(function (a) {
            var input = protractor_1.element(protractor_1.by.id(Id));
            input.clear();
            protractor_1.browser.wait(EC.textToBePresentInElementValue(protractor_1.element(protractor_1.by.id(Id)), ''), 10000000).then(function (a) { });
            input.clear();
            input.sendKeys(Value);
        });
    };
    FieldsHelper.prototype.WaitByCssAndClick_SelectItemFromList = function (className, index) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.visibilityOf(protractor_1.element(protractor_1.by.css(className))), 100000000).then(function (a) {
            var shipment = protractor_1.element(protractor_1.by.css(className)).all(protractor_1.by.tagName('li'));
            shipment.get(index).click();
        });
    };
    FieldsHelper.prototype.WaitByCssAndClick_FromTagInsideList = function (className, index) {
        var EC = protractor_1.protractor.ExpectedConditions;
        protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.css(className))), 100000000).then(function (a) {
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
        protractor_1.browser.wait(EC.elementToBeClickable(protractor_1.element(protractor_1.by.id(BtnId))), 100000000).then(function (a) {
            var button = protractor_1.element(protractor_1.by.id(BtnId)).click();
            console.log("Button Clicked");
        });
    };
    return FieldsHelper;
}());
exports.FieldsHelper = FieldsHelper;
//# sourceMappingURL=FieldsHelper.js.map