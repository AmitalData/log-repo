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
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var AddEditARGeneralInvoiceLineComponent = /** @class */ (function () {
    function AddEditARGeneralInvoiceLineComponent() {
        this.EntityPM = null;
        this.ObjectTableName = "ARInvoiceLine";
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.AmountForiegnLabel = null;
        this.AmountLocalLabel = null;
        this.AmountInvoiceLabel = null;
    }
    AddEditARGeneralInvoiceLineComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.SetLabels();
        this.Clone();
        this.BuildQueryFilters();
    };
    AddEditARGeneralInvoiceLineComponent.prototype.BuildQueryFilters = function () {
        this.ChargeTypesQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        this.ChargeTypesQueryFilters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("IsReceivable", true, null, null, "Equals", false, false, false, "Boolean");
    };
    AddEditARGeneralInvoiceLineComponent.prototype.SetLabels = function () {
        this.AmountForiegnLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.F.ForiegnCurrencyAmount").replace("%ForiegnCurrencyCode", this.DataContext.ForiegnCurrencyCode);
        this.AmountLocalLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.F.LocalCurrencyAmount").replace("%LocalCurrencyCode", SessionLocator_1.SessionLocator.LocalCurrencyCode);
        this.AmountInvoiceLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoiceLine.F.InvoiceCurrencyAmount").replace("%InvoiceCurrencyCode", this.DataContext.InvoiceCurrencyCode);
    };
    AddEditARGeneralInvoiceLineComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditARGeneralInvoiceLineComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.DataContext.UnitPrice == 0) {
            errors.push("Unit price field is required");
        }
        else {
            if (this.DataContext.fatherComponent.EntityPM.ARInvoiceTypeCode == "CD") {
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
        if (this.DataContext.chargesTypeList != null && Tools_1.AppTool.IsNullOrEmpty(this.DataContext.chargesTypeList.ReceivableCreditGLAccountId)) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.NoGLAccount"));
        }
        if (this.DataContext.fatherComponent.glaccount != null && this.DataContext.fatherComponent.glaccount.IsVATExempt == false && this.DataContext.VatPercentage > 0) {
            errors.push("The partner is VAT exempt");
        }
        if (errors.length == 0) {
            if (this.DataContext.AddNewLineMode) {
                this.DataContext.AddNewLineMode = false;
                this.DataContext.fatherComponent.EntityPM.AddARInvoiceLinePM(this.EntityPM);
                //this.DataContext.Exists = true;
                this.DataContext.fatherComponent.SetUIProperties();
                this.DataContext.fatherComponent.BuildScreenData();
            }
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
        else {
            var errors_new = [];
            errors.forEach(function (item) {
                if (item.indexOf("%ForiegnCurrencyCode") > -1) {
                    errors_new.push(item.replace("%ForiegnCurrencyCode", _this.DataContext.ForiegnCurrencyCode));
                }
                else if (item.indexOf("%LocalCurrencyCode") > -1) {
                    errors_new.push(item.replace("%LocalCurrencyCode", _this.DataContext.LocalCurrencyCode));
                }
                else if (item.indexOf("%InvoiceCurrencyCode") > -1) {
                    errors_new.push(item.replace("%InvoiceCurrencyCode", _this.DataContext.InvoiceCurrencyCode));
                }
                else {
                    errors_new.push(item);
                }
            });
            errors = errors_new;
        }
        this.ValidationErrorsList = errors;
    };
    AddEditARGeneralInvoiceLineComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Description');
        this.myCloner.AddField('LocalDescription');
        this.myCloner.AddField('VatTypeId');
        this.myCloner.AddField('VatPercentage');
        this.myCloner.AddField('ForiegnCurrencyId');
        this.myCloner.AddField('ForiegnExchangeRate');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('UnitPrice');
        this.myCloner.AddField('ForiegnCurrencyAmount');
        this.myCloner.AddField('LocalCurrencyAmount');
        this.myCloner.AddField('InvoiceCurrencyAmount');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.fatherComponent.EntityPM);
    };
    AddEditARGeneralInvoiceLineComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditARGeneralInvoiceLineComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditARGeneralInvoiceLineComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditARGeneralInvoiceLineComponent);
    return AddEditARGeneralInvoiceLineComponent;
}());
exports.AddEditARGeneralInvoiceLineComponent = AddEditARGeneralInvoiceLineComponent;
//# sourceMappingURL=AddEditARGeneralInvoiceLineComponent.js.map