"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../Tools");
var SessionLocator_1 = require("../Utilities/SessionLocator");
var VatTypeListService_1 = require("../../Common/Services/StandardLists/VatTypeListService");
var VatTypesValidator = /** @class */ (function () {
    function VatTypesValidator() {
    }
    VatTypesValidator.GetVatTypeDependency1 = function () {
        var myResult = false;
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
            myResult = null;
        }
        return myResult;
    };
    VatTypesValidator.ValidateMultiPercentages = function (vatTypesIds) {
        if (vatTypesIds === void 0) { vatTypesIds = []; }
        var myResult = true;
        if (!SessionLocator_1.SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
            if (vatTypesIds.length > 0) {
                var myService = new VatTypeListService_1.VatTypeListService();
                myService.getAllFromCache().subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var allVatTypes = myResponse.Result;
                        vatTypesIds.forEach(function (id) {
                            var item = allVatTypes.filter(function (f) { return f.Id == id; })[0];
                            if (item) {
                                if (item.IsMultiPercentage) {
                                    myResult = false;
                                }
                            }
                        });
                    }
                });
            }
        }
        return myResult;
    };
    VatTypesValidator.FilterChargesByVATs = function (allChargesTypes) {
        var myResult = [];
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
            myResult = allChargesTypes;
        }
        else {
            var myService = new VatTypeListService_1.VatTypeListService();
            myService.getAllFromCache().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var allVatTypes = myResponse.Result;
                    allChargesTypes.forEach(function (item) {
                        if (item.VatTypeId == null) {
                            myResult.push(item);
                        }
                        else {
                            var vatType = allVatTypes.filter(function (f) { return f.Id == item.VatTypeId; })[0];
                            if (vatType) {
                                if (vatType.IsMultiPercentage == false) {
                                    myResult.push(item);
                                }
                            }
                        }
                    });
                }
            });
        }
        return myResult;
    };
    VatTypesValidator.GetAllVatTypes = function () {
        var myResult = [];
        var myService = new VatTypeListService_1.VatTypeListService();
        myService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                myResult = myResponse.Result;
            }
        });
        return myResult;
    };
    VatTypesValidator.IsVatAllowed = function (vatTypeId, allVatTypes) {
        var myResult = true;
        if (!Tools_1.AppTool.IsNullOrEmpty(vatTypeId)) {
            if (!SessionLocator_1.SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
                var vatType = allVatTypes.filter(function (f) { return f.Id == vatTypeId; })[0];
                if (vatType) {
                    if (vatType.IsMultiPercentage) {
                        myResult = false;
                    }
                }
            }
        }
        return myResult;
    };
    VatTypesValidator.GetError = function () {
        return "Your accounting settings doesn't enable Multi-percentage VATs";
    };
    return VatTypesValidator;
}());
exports.VatTypesValidator = VatTypesValidator;
//# sourceMappingURL=VatTypesValidator.js.map