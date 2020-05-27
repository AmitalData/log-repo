"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var VatTypesValidator_1 = require("../../../../Infrastructure/Validators/VatTypesValidator");
var AddEditARInvoiceLineComponent = /** @class */ (function () {
    function AddEditARInvoiceLineComponent() {
        this.ObjectTableName = "ARInvoiceLine";
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.AmountForiegnLabel = null;
        this.AmountLocalLabel = null;
        this.AmountInvoiceLabel = null;
    }
    AddEditARInvoiceLineComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        //this.DataContext.SetUIProperties();
        this.SetLabels();
        this.Clone();
    };
    AddEditARInvoiceLineComponent.prototype.SetLabels = function () {
        this.AmountForiegnLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.F.ForiegnCurrencyAmount").replace("%ForiegnCurrencyCode", this.DataContext.ForiegnCurrencyCode);
        this.AmountLocalLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.F.LocalCurrencyAmount").replace("%LocalCurrencyCode", SessionLocator_1.SessionLocator.LocalCurrencyCode);
        this.AmountInvoiceLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.F.InvoiceCurrencyAmount").replace("%InvoiceCurrencyCode", this.DataContext.fatherComponent.InvoiceCurrencyCode);
    };
    AddEditARInvoiceLineComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditARInvoiceLineComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VatTypeId)) {
            var field = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.F.VatTypeId");
            errors.push(msg.replace("%FieldName", field));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VatPercentage)) {
            if (!this.EntityPM.VatIsMultiPercentage) {
                var field = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.F.VatPercentage");
                errors.push(msg.replace("%FieldName", field));
            }
        }
        if (this.EntityPM.VatIsMultiPercentage) {
            if (!SessionLocator_1.SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
                errors.push(VatTypesValidator_1.VatTypesValidator.GetError());
            }
        }
        if (this.DataContext.UnitPrice == 0) {
            var field = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.F.UnitPrice");
            errors.push(msg.replace("%FieldName", field));
        }
        else {
            if (this.DataContext.fatherComponent.EntityPM.ARInvoiceTypeCode == "CD" || this.DataContext.fatherComponent.EntityPM.ARInvoiceTypeCode == "CC") {
                if (this.DataContext.UnitPrice > 0) {
                    if (!SessionLocator_1.SessionLocator.AccountingSettingPM.AllowPositiveAmountsInTheCreditNote) {
                        errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.NoPositivePrice"));
                    }
                }
            }
            else {
                if (this.DataContext.UnitPrice < 0) {
                    if (!SessionLocator_1.SessionLocator.AccountingSettingPM.AllowMinusInvoicelines) {
                        errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.NoMinusPrice"));
                    }
                }
            }
        }
        this.ValidationErrorsList = [];
        errors.forEach(function (error) {
            if (error.indexOf("%ForiegnCurrencyCode") > -1) {
                error = error.replace("%ForiegnCurrencyCode", _this.DataContext.ForiegnCurrencyCode);
            }
            _this.ValidationErrorsList.push(error);
        });
        if (errors.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    AddEditARInvoiceLineComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Description');
        this.myCloner.AddField('LocalDescription');
        this.myCloner.AddField('VatTypeId');
        this.myCloner.AddField('VatPercentage');
        this.myCloner.AddField('ForiegnExchangeRate');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('UnitPrice');
        this.myCloner.AddField('ForiegnCurrencyAmount');
        this.myCloner.AddField('LocalCurrencyAmount');
        this.myCloner.AddField('InvoiceCurrencyAmount');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.fatherComponent.EntityPM);
    };
    AddEditARInvoiceLineComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditARInvoiceLineComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditARInvoiceLineComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditARInvoiceLineComponent);
    return AddEditARInvoiceLineComponent;
}());
exports.AddEditARInvoiceLineComponent = AddEditARInvoiceLineComponent;
//# sourceMappingURL=AddEditARInvoiceLineComponent.js.map