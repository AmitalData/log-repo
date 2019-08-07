"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../Tools");
var AddressValidator = /** @class */ (function () {
    function AddressValidator() {
    }
    AddressValidator.IsMainAddressEnglishCharacters = function (entityPM) {
        var isValid = true;
        if (entityPM.AddressTypeId == "M") {
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Name) && !Tools_1.FormatTool.IsEnglishText(entityPM.Name)) {
                isValid = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Description) && !Tools_1.FormatTool.IsEnglishText(entityPM.Description)) {
                isValid = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Address1) && !Tools_1.FormatTool.IsEnglishText(entityPM.Address1)) {
                isValid = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Address2) && !Tools_1.FormatTool.IsEnglishText(entityPM.Address2)) {
                isValid = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.City) && !Tools_1.FormatTool.IsEnglishText(entityPM.City)) {
                isValid = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.ATTN) && !Tools_1.FormatTool.IsEnglishText(entityPM.ATTN)) {
                isValid = false;
            }
        }
        return isValid;
    };
    AddressValidator.IsMainAddressEnglishCharacters_Potential = function (entityPM) {
        var isValid = true;
        if (!entityPM.IsLocalLanguage) {
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.EnglishName) && !Tools_1.FormatTool.IsEnglishText(entityPM.EnglishName)) {
                isValid = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Address1_Potential) && !Tools_1.FormatTool.IsEnglishText(entityPM.Address1_Potential)) {
                isValid = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.Address2_Potential) && !Tools_1.FormatTool.IsEnglishText(entityPM.Address2_Potential)) {
                isValid = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.City_Potential) && !Tools_1.FormatTool.IsEnglishText(entityPM.City_Potential)) {
                isValid = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.ATTN_Potential) && !Tools_1.FormatTool.IsEnglishText(entityPM.ATTN_Potential)) {
                isValid = false;
            }
        }
        return isValid;
    };
    return AddressValidator;
}());
exports.AddressValidator = AddressValidator;
//# sourceMappingURL=AddressValidator.js.map