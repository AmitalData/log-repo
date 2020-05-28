"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var GLAccountValidator = /** @class */ (function () {
    function GLAccountValidator() {
    }
    GLAccountValidator.ValidateGLAccount = function (entityPM) {
        return [];
    };
    GLAccountValidator.ValidateIsMultiCurrency = function (entityPM) {
        //
        // [!] THE VALIDATION MOVED TO SERVER
        //
        //var errors = [];
        //var _GLAccountExtendedListService = new GLAccountExtendedListService();
        //if (entityPM.IsMultiCurrency == false) {
        //    ////if (entityPM.AccountTypeCode == "2") { // Customer
        //    //    // Customer: LedgerTransactions check
        //    //    _GLAccountExtendedListService.CheckIfHasLedgerTransactions(entityPM.Id).subscribe((exist) => {
        //    //        if (!AppTool.IsNullOrEmpty(exist)) {
        //    //            if (exist) {
        //    //                errors.push(TextCodeTranslator.Translate("Accounting.General.O.ThereOpenTransaction")); // "There are open transactions for the GLAccount, can’t make it single currency GLAccount");
        //    //                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        //    //                this.CurrentSession.CurrentEditComponent.IsEditValid = false;
        //    //                return errors;
        //    //            } else {
        //    //                this.CurrentSession.CurrentEditComponent.IsEditValid = true;
        //    //                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        //    //            }
        //    //        }
        //    //    });
        //    ////}
        //} else {
        //    if (entityPM.AccountTypeCode == "2") { // Customer
        //        // Check if has splitted accounts
        //        _GLAccountExtendedListService.CheckIfSplitted(entityPM.Id).subscribe((splitted) => {
        //            if (!AppTool.IsNullOrEmpty(splitted)) {
        //                if (splitted) {
        //                    errors.push("This GLAccount have splitted GLAccounts by currency, Deactivate these GLAccounts before doing these action");
        //                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        //                    this.CurrentSession.CurrentEditComponent.IsEditValid = false;
        //                    return errors;
        //                } else {
        //                    this.CurrentSession.CurrentEditComponent.IsEditValid = true;
        //                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        //                }
        //            }
        //        });
        //    }
        //}
        //// splitted glaccount validation
        //var errors = [];
        //var _GLAccountExtendedListService = new GLAccountExtendedListService();
        //if (entityPM.IsMultiCurrency == true) {
        //    if (entityPM.AccountTypeCode == "2") { // Customer
        //        // Check if has splitted accounts
        //        _GLAccountExtendedListService.CheckIfSplitted(entityPM.Id).subscribe((splitted) => {
        //            if (!AppTool.IsNullOrEmpty(splitted)) {
        //                if (splitted) {
        //                    errors.push("This GLAccount have splitted GLAccounts by currency, Deactivate these GLAccounts before doing these action");
        //                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        //                    this.CurrentSession.CurrentEditComponent.IsEditValid = false;
        //                    return errors;
        //                } else {
        //                    this.CurrentSession.CurrentEditComponent.IsEditValid = true;
        //                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        //                }
        //            }
        //        });
        //    }
        //}
        ////
    };
    GLAccountValidator.ValidateWithHoldingTaxLines = function (entityPM) {
        var FIELD_IS_REQUIERD = null;
        FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        for (var _i = 0, _a = entityPM.GLAccountWithholdingTaxes; _i < _a.length; _i++) {
            var line = _a[_i];
            if (line.FromDate == undefined) {
                var s = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccountWithholdingTax.F.FromDate"));
                errors.push(s);
            }
            if (line.ToDate == undefined) {
                var s = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccountWithholdingTax.F.ToDate"));
                errors.push(s);
            }
            if (line.Percentage == undefined) {
                var s = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccountWithholdingTax.F.Percentage"));
                errors.push(s);
            }
        }
        // this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        return errors;
    };
    GLAccountValidator.ValidateCurrency = function (entityPM, oldCurrency) {
        //
        // [!] THE VALIDATION MOVED TO SERVER
        //
        //var errors = [];
        //var _GLAccountExtendedListService = new GLAccountExtendedListService();
        //if (entityPM.AccountTypeCode == "2") { // Customer
        //    if (oldCurrency != entityPM.CurrencyId) {
        //        // Customer: LedgerTransactions check
        //        _GLAccountExtendedListService.CheckIfHasLedgerTransactions(entityPM.Id).subscribe((exist) => {
        //            if (!AppTool.IsNullOrEmpty(exist)) {
        //                if (exist) {
        //                    errors.push(TextCodeTranslator.Translate("Accounting.General.O.ThereTransactions4GLAwithexistingCurrency"));
        //                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        //                    this.CurrentSession.CurrentEditComponent.IsEditValid = false;
        //                    return errors;
        //                } else {
        //                    this.CurrentSession.CurrentEditComponent.IsEditValid = true;
        //                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        //                }
        //            }
        //        });
        //    } else {
        //        this.CurrentSession.CurrentEditComponent.IsEditValid = true;
        //        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        //    }
        //} else {
        //    this.CurrentSession.CurrentEditComponent.IsEditValid = true;
        //    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        //}
    };
    GLAccountValidator.prototype.Validate = function (entityPM) {
        var errors = [];
        var result = [];
        if (entityPM.Id != undefined) {
            errors = GLAccountValidator.ValidateWithHoldingTaxLines(entityPM);
        }
        return errors;
    };
    GLAccountValidator.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    return GLAccountValidator;
}());
exports.GLAccountValidator = GLAccountValidator;
//# sourceMappingURL=GLAccountValidator.js.map