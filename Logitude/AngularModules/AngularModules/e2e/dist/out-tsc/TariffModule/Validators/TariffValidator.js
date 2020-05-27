"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var ChargesTypeListService_1 = require("../../Common/Services/StandardLists/ChargesTypeListService");
var TariffValidator = /** @class */ (function () {
    function TariffValidator() {
        this.IdProps = [];
        this.UOMProps = [];
    }
    TariffValidator.prototype.Validate = function (entityPM) {
        var error = [];
        if (entityPM.TypeCode == "ASC") {
            this.chargesTypePMService = new ChargesTypeListService_1.ChargesTypeListService();
            this.FillChargesIDsAndUOMS();
            error = this.ValidateSurcharge(entityPM);
        }
        return error;
    };
    TariffValidator.prototype.FillChargesIDsAndUOMS = function () {
        for (var index = 1; index <= 10; index++) {
            this.IdProps.push("Surcharge" + index + "Id");
            this.UOMProps.push("Surcharge" + index + "UOM");
        }
    };
    TariffValidator.prototype.ValidateSurcharge = function (entityPM) {
        var ValidationErrorsList = [];
        var IdProps = [];
        var UOMProps = [];
        var IdPropsName = [];
        var UOMPropsName = [];
        var DuplicatedChargesIds = [];
        var EmptyIndex = 1;
        var emptyLines = false;
        var FirstLineEmpty = false;
        var tempErrors = [];
        for (var index = 1; index <= 10; index++) {
            IdProps.push("Surcharge" + index + "Id");
            UOMProps.push("Surcharge" + index + "UOM");
            IdPropsName.push("Charge Type " + index);
            UOMPropsName.push("UOM " + index);
            if (this.IdProps.filter(function (p) { return entityPM[p + ""] == entityPM[IdProps[index - 1]] && (p + "" != IdProps[index - 1] + "") && entityPM[IdProps[index - 1]] != null; })[0] != null) {
                var chargresType = this.IdProps.filter(function (p) { return entityPM[p + ""] == entityPM[IdProps[index - 1]] && (p + "" != IdProps[index - 1] + "") && entityPM[IdProps[index - 1]] != null; })[0];
                if (!DuplicatedChargesIds.includes(entityPM[chargresType + ""])) {
                    DuplicatedChargesIds.push(entityPM[chargresType + ""]);
                    this.chargesTypePMService.getSingleFromCache(entityPM[chargresType + ""]).subscribe(function (res) {
                        if (!res.HasError) {
                            var chargesTypeList = res.Result;
                            if (res) {
                                ValidationErrorsList.push("Charge type " + chargesTypeList.EnglishName + " is duplicated");
                            }
                        }
                    });
                }
            }
            if (index == 1) {
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM[IdProps[index - 1]])) {
                    tempErrors.push(IdPropsName[index - 1] + " is required");
                    FirstLineEmpty = true;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM[UOMProps[index - 1]])) {
                    tempErrors.push(UOMPropsName[index - 1] + " is required");
                }
            }
            else {
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM[IdProps[index - 1]])) {
                    if (EmptyIndex == 1) {
                        EmptyIndex = index;
                    }
                }
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM[UOMProps[index - 1]]) && !Tools_1.AppTool.IsNullOrEmpty(entityPM[IdProps[index - 1]])) {
                    ValidationErrorsList.push(IdPropsName[index - 1] + " is filled without a UOM");
                }
                if (index == 2) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(entityPM[UOMProps[index - 1]]) && !Tools_1.AppTool.IsNullOrEmpty(entityPM[IdProps[index - 1]])) {
                        if (FirstLineEmpty) {
                            emptyLines = true;
                            ValidationErrorsList.push("Empty Charge Lines aren't allowed between line 1 and line 2");
                            EmptyIndex = 1;
                        }
                    }
                }
                if (index >= 3) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(entityPM[UOMProps[index - 1]]) && !Tools_1.AppTool.IsNullOrEmpty(entityPM[IdProps[index - 1]])) {
                        if (EmptyIndex != 1) {
                            emptyLines = true;
                            ValidationErrorsList.push("Empty Charge Lines aren't allowed between line " + (EmptyIndex - 1) + " and line " + index);
                            EmptyIndex = 1;
                        }
                        if (Tools_1.AppTool.IsNullOrEmpty(entityPM[IdProps[index - 2]])) {
                            //   this.ValidationErrorsList.push("no empty line between 2 charges in line "+index+ " and "+(index-2));
                        }
                    }
                }
            }
        }
        if (!emptyLines) {
            tempErrors.forEach(function (error) {
                ValidationErrorsList.push(error);
            });
        }
        return ValidationErrorsList;
    };
    return TariffValidator;
}());
exports.TariffValidator = TariffValidator;
//# sourceMappingURL=TariffValidator.js.map