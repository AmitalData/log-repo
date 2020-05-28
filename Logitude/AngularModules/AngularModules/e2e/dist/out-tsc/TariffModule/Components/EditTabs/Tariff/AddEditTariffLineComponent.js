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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var AddEditTariffLineComponent = /** @class */ (function () {
    function AddEditTariffLineComponent() {
        this.ObjectTableName = "TariffLine";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AddEditTariffLineComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.TariffType = dataContext.FatherComponent.EntityPM.TypeCode;
        this.Clone();
    };
    Object.defineProperty(AddEditTariffLineComponent.prototype, "OriginPortText", {
        get: function () { return this.EntityPM.OriginPortText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "DestinationPortText", {
        get: function () { return this.EntityPM.DestinationPortText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "MinPriceText", {
        get: function () { return this.EntityPM.MinPriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Step1PriceText", {
        get: function () { return this.EntityPM.Step1PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Step2PriceText", {
        get: function () { return this.EntityPM.Step2PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Step3PriceText", {
        get: function () { return this.EntityPM.Step3PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Step4PriceText", {
        get: function () { return this.EntityPM.Step4PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Step5PriceText", {
        get: function () { return this.EntityPM.Step5PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Step6PriceText", {
        get: function () { return this.EntityPM.Step6PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Step7PriceText", {
        get: function () { return this.EntityPM.Step7PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Step8PriceText", {
        get: function () { return this.EntityPM.Step8PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Surcharge1PriceText", {
        get: function () { return this.EntityPM.Surcharge1PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Surcharge2PriceText", {
        get: function () { return this.EntityPM.Surcharge2PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Surcharge3PriceText", {
        get: function () { return this.EntityPM.Surcharge3PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Surcharge4PriceText", {
        get: function () { return this.EntityPM.Surcharge4PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Surcharge5PriceText", {
        get: function () { return this.EntityPM.Surcharge5PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Surcharge6PriceText", {
        get: function () { return this.EntityPM.Surcharge6PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Surcharge7PriceText", {
        get: function () { return this.EntityPM.Surcharge7PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Surcharge8PriceText", {
        get: function () { return this.EntityPM.Surcharge8PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Surcharge9PriceText", {
        get: function () { return this.EntityPM.Surcharge9PriceText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTariffLineComponent.prototype, "Surcharge10PriceText", {
        get: function () { return this.EntityPM.Surcharge10PriceText; },
        enumerable: true,
        configurable: true
    });
    AddEditTariffLineComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditTariffLineComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.DestinationPortId)) {
            errors.push(msg.replace("%FieldName", "To"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.OriginPortId)) {
            errors.push(msg.replace("%FieldName", "From"));
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.EntityPM.AddedManually = true;
            if (this.DataContext.IsNewEntity) {
                this.DataContext.IsNewEntity = false;
                if (this.DataContext.FatherComponent.CurrentVersion.TariffLines.indexOf(this.EntityPM) == -1) {
                    this.DataContext.FatherComponent.CurrentVersion.AddTariffLine(this.EntityPM);
                    this.DataContext.FatherComponent.EntityPM.TariffLinesAdded = true;
                }
            }
            this.DataContext.FatherComponent.FillTariffLines(this.DataContext.FatherComponent.CurrentVersion.TariffLines);
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    AddEditTariffLineComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('OriginPortId');
        this.myCloner.AddField('OriginPortCode');
        this.myCloner.AddField('OriginPortName');
        this.myCloner.AddField('DestinationPortId');
        this.myCloner.AddField('DestinationPortCode');
        this.myCloner.AddField('DestinationPortName');
        this.myCloner.AddField('MinPrice');
        this.myCloner.AddField('Step1Price');
        this.myCloner.AddField('Step2Price');
        this.myCloner.AddField('Step3Price');
        this.myCloner.AddField('Step4Price');
        this.myCloner.AddField('Step5Price');
        this.myCloner.AddField('Step6Price');
        this.myCloner.AddField('Step7Price');
        this.myCloner.AddField('Step8Price');
        this.myCloner.AddField('StartDate');
        this.myCloner.AddField('ExpirationDate');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.FatherComponent.EntityPM);
    };
    AddEditTariffLineComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditTariffLineComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditTariffLineComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditTariffLineComponent);
    return AddEditTariffLineComponent;
}());
exports.AddEditTariffLineComponent = AddEditTariffLineComponent;
//# sourceMappingURL=AddEditTariffLineComponent.js.map