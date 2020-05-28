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
var VatTypesValidator_1 = require("../../../../Infrastructure/Validators/VatTypesValidator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var AddEditAPGeneralInvoiceLineComponent = /** @class */ (function () {
    function AddEditAPGeneralInvoiceLineComponent() {
        this.EntityPM = null;
        this.ObjectTableName = "APInvoiceLine";
        this.ValidationErrorsList = [];
        this.EnableMultiRateAPInvoices = false;
        this.isRTL = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        if (SessionLocator_1.SessionLocator.AccountingSettingPM) {
            this.EnableMultiRateAPInvoices = SessionLocator_1.SessionLocator.AccountingSettingPM.EnableMultiRateAPInvoices;
        }
    }
    AddEditAPGeneralInvoiceLineComponent.prototype.SetDataContext = function (dataContext) {
        this.EntityPM = dataContext.EntityPM;
        this.DataContext = dataContext;
        this.EntityPM = dataContext.invoiceLinePM;
        this.Clone();
    };
    AddEditAPGeneralInvoiceLineComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditAPGeneralInvoiceLineComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VatTypeId)) {
            var field = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoiceLine.F.VatTypeId");
            errors.push(msg.replace("%FieldName", field));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VatPercentage)) {
            if (!this.EntityPM.VatIsMultiPercentage) {
                var field = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoiceLine.F.VatPercentage");
                errors.push(msg.replace("%FieldName", field));
            }
        }
        if (this.EntityPM.VatIsMultiPercentage) {
            if (!SessionLocator_1.SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
                errors.push(VatTypesValidator_1.VatTypesValidator.GetError());
            }
        }
        if (this.DataContext.chargesTypeList != null && Tools_1.AppTool.IsNullOrEmpty(this.DataContext.chargesTypeList.PayableDebitGLAcountId)) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.M.NoGLAccount"));
        }
        if (this.DataContext.Glaccount != null && this.DataContext.Glaccount.IsMultiCurrency == false) {
            if (this.DataContext.Glaccount.CurrencyId != this.DataContext.InvoiceCurrencyId) {
                errors.push("Line currency is " + this.DataContext.InvoiceCurrencyCode + " but the GLAccount of the charge type is " + this.DataContext.Glaccount.CurrencyCode);
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (this.DataContext.AddNewLineMode) {
                this.DataContext.AddNewLineMode = false;
                if (this.DataContext.fatherComponent.ItemsSource.Collection.indexOf(this.DataContext) == -1) {
                    this.DataContext.fatherComponent.ItemsSource.Insert(this.DataContext);
                }
                if (this.DataContext.fatherComponent.EntityPM.InvoiceLines.indexOf(this.EntityPM) == -1) {
                    this.DataContext.fatherComponent.EntityPM.AddAPInvoiceLinePM(this.EntityPM);
                }
                this.DataContext.Exists = true;
            }
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    AddEditAPGeneralInvoiceLineComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('ChargesTypeId');
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('LocalDescription');
        this.myCloner.AddField('VatTypeId');
        this.myCloner.AddField('VatPercentage');
        this.myCloner.AddField('VendorId');
        this.myCloner.AddField('ForiegnCurrencyId');
        this.myCloner.AddField('ExpectedAmount');
        this.myCloner.AddField('OtherInvoicesAmounts');
        this.myCloner.AddField('InvoiceCurrencyAmount');
        this.myCloner.AddField('OpenAmount');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.fatherComponent.EntityPM);
    };
    AddEditAPGeneralInvoiceLineComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditAPGeneralInvoiceLineComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditAPGeneralInvoiceLineComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditAPGeneralInvoiceLineComponent);
    return AddEditAPGeneralInvoiceLineComponent;
}());
exports.AddEditAPGeneralInvoiceLineComponent = AddEditAPGeneralInvoiceLineComponent;
//# sourceMappingURL=AddEditAPGeneralInvoiceLineComponent.js.map