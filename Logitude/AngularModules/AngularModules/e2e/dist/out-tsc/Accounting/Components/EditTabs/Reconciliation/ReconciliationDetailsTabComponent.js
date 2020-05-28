"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ReconciliationLineModel = /** @class */ (function () {
    function ReconciliationLineModel(line, color) {
        this.Line = line;
        this.OddEven = color;
        // Calculate Transaction Amount
        if (Tools_1.AppTool.IsNullOrZero(this.Line.ForeignAmountCredit)) {
            this.TransactionAmount = -1 * this.Line.ForeignAmountDebit;
        }
        else {
            this.TransactionAmount = this.Line.ForeignAmountCredit;
        }
    }
    return ReconciliationLineModel;
}());
exports.ReconciliationLineModel = ReconciliationLineModel;
var ReconciliationDetailsTabComponent = /** @class */ (function (_super) {
    __extends(ReconciliationDetailsTabComponent, _super);
    function ReconciliationDetailsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "Reconciliation";
        _this.DataContext = _this;
        _this.TotalSum = 0;
        _this.NoRows = false;
        _this.searchText = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.lastColorOperation = true;
        _this.EntityPM = entityArgs.EntityPM;
        _this.LoadData();
        _this.AmountText = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Amount") + " (" + _this.EntityPM.CurrencyCode + ")";
        return _this;
    }
    ReconciliationDetailsTabComponent.prototype.TextChanged = function (searchtext) {
        var _this = this;
        this.timerToken = setTimeout(function () {
            _this.searchText = searchtext;
            _this.FilterLines();
        }, 500);
    };
    ReconciliationDetailsTabComponent.prototype.Abs = function (number) {
        return number < 0 ? number * -1 : number;
    };
    ReconciliationDetailsTabComponent.prototype.LoadData = function () {
        var _this = this;
        this.ItemSource = [];
        this.originalItemSource = [];
        this.EntityPM.ReconciliationLines.forEach(function (line) {
            var item = new ReconciliationLineModel(line, _this.ColorMe(line));
            _this.ItemSource.push(item);
        });
        this.originalItemSource = this.ItemSource;
        this.CalculateTotals();
    };
    ReconciliationDetailsTabComponent.prototype.OpenJournal = function (id) {
        // open Journal screen
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
            });
        }
    };
    ReconciliationDetailsTabComponent.prototype.CalculateTotals = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ItemSource)) {
            for (var _i = 0, _a = this.ItemSource; _i < _a.length; _i++) {
                var line = _a[_i];
                this.TotalSum += line.Line.ReconciliationAmount;
            }
        }
    };
    ReconciliationDetailsTabComponent.prototype.FilterLines = function () {
        var _this = this;
        var lines = this.originalItemSource;
        // Filtering
        if (!Tools_1.AppTool.IsNullOrEmpty(this.searchText)) {
            lines = lines.filter(function (el) {
                var line = el.Line;
                if (line.SearchFields != null)
                    if (line.SearchFields.toLowerCase().includes(_this.searchText.toLowerCase()))
                        return true;
                return false;
            });
        }
        this.ItemSource = lines;
        this.NoRows = lines.length == 0;
    };
    ReconciliationDetailsTabComponent.prototype.ColorMe = function (line) {
        if (!Tools_1.AppTool.IsNullOrEmpty(line)) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.lastGroupNumber))
                this.lastGroupNumber = line.GroupNumber;
            if (this.lastGroupNumber == line.GroupNumber) {
                return this.lastColorOperation == true;
            }
            else {
                this.lastGroupNumber = line.GroupNumber;
                this.lastColorOperation = !this.lastColorOperation;
                return this.lastColorOperation == true;
            }
        }
        return false;
    };
    //#endregion
    ReconciliationDetailsTabComponent.prototype.RefreshButtonClicked = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        this.LoadData();
    };
    ReconciliationDetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ReconciliationDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ReconciliationDetailsTabComponent);
    return ReconciliationDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ReconciliationDetailsTabComponent = ReconciliationDetailsTabComponent;
//# sourceMappingURL=ReconciliationDetailsTabComponent.js.map