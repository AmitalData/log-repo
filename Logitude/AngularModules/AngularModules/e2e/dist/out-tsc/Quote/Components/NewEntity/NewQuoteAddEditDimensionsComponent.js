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
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var NewQuoteAddEditDimensionsComponent = /** @class */ (function () {
    function NewQuoteAddEditDimensionsComponent() {
        this.ObjectTableName = "QuotePackage";
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    NewQuoteAddEditDimensionsComponent.prototype.SetWindowArgs = function (item) {
        this.DataContext = item;
        this.EntityPM = item.EntityPM;
        this.DataContext.IsWindowMode = true;
        this.DimensionsLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("QuotePackage.F.Dimensions").replace('%UnitCode', this.DataContext.QuotePM.DimensionsUnitCode);
        this.VolumeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("QuotePackage.F.Volume").replace('%VolumeCode', this.DataContext.QuotePM.VolumeUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("QuotePackage.F.VolumetricWeight").replace('%WeightCode', this.DataContext.QuotePM.ChargeableWeightUnitCode);
        this.GrossWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("QuotePackage.F.GrossWeight").replace('%WeightCode', this.DataContext.QuotePM.GrossWeightUnitCode);
    };
    NewQuoteAddEditDimensionsComponent.prototype.CancelButtonClicked = function () {
        this.DataContext.IsWindowMode = false;
        this.CurrentSession.CloseCurrentWindow();
    };
    NewQuoteAddEditDimensionsComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (errors.length == 0) {
            this.DataContext.fatherComponent.EntityPM.AddQuotePackagePM(this.DataContext.EntityPM);
            if (this.DataContext.fatherComponent.ItemsSource.indexOf(this.DataContext) == -1) {
                this.DataContext.fatherComponent.ItemsSource.push(this.DataContext);
            }
            this.DataContext.IsWindowMode = false;
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    NewQuoteAddEditDimensionsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewQuoteAddEditDimensionsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewQuoteAddEditDimensionsComponent);
    return NewQuoteAddEditDimensionsComponent;
}());
exports.NewQuoteAddEditDimensionsComponent = NewQuoteAddEditDimensionsComponent;
//# sourceMappingURL=NewQuoteAddEditDimensionsComponent.js.map