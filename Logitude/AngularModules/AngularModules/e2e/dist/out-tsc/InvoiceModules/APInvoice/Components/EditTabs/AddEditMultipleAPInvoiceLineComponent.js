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
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var AddEditMultipleAPInvoiceLineComponent = /** @class */ (function () {
    function AddEditMultipleAPInvoiceLineComponent() {
        this.EntityPM = null;
        this.ObjectTableName = "APInvoiceLine";
        this.ValidationErrorsList = [];
        this.isRTL = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }
    AddEditMultipleAPInvoiceLineComponent.prototype.SetDataContext = function (dataContext) {
        this.EntityPM = dataContext.EntityPM;
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.Clone();
    };
    AddEditMultipleAPInvoiceLineComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditMultipleAPInvoiceLineComponent.prototype.OkButtonClicked = function () {
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
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (this.DataContext.AddNewLineMode) {
                this.DataContext.AddNewLineMode = false;
                if (this.DataContext.fatherComponent.ItemsSource.Collection.indexOf(this.DataContext) == -1) {
                    this.DataContext.fatherComponent.ItemsSource.Insert(this.DataContext);
                }
                this.DataContext.IsChecked = true;
            }
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    AddEditMultipleAPInvoiceLineComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('ChargesTypeId');
        this.myCloner.AddField('Notes');
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
    AddEditMultipleAPInvoiceLineComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditMultipleAPInvoiceLineComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditMultipleAPInvoiceLineComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditMultipleAPInvoiceLineComponent);
    return AddEditMultipleAPInvoiceLineComponent;
}());
exports.AddEditMultipleAPInvoiceLineComponent = AddEditMultipleAPInvoiceLineComponent;
//# sourceMappingURL=AddEditMultipleAPInvoiceLineComponent.js.map