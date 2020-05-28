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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var JournalOpService_1 = require("../../Services/Others/JournalOpService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../Infrastructure/Tools");
var YearTransferComponent = /** @class */ (function (_super) {
    __extends(YearTransferComponent, _super);
    function YearTransferComponent(_entityResourceService, entityArgs) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "AccountingPeriod";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._JournalPM = null;
        _this._JournalOpService = new JournalOpService_1.JournalOpService();
        _this.UIProperties.SetEnabled("Year", _this.ObjectTableName, true);
        _this.UIProperties.SetRequired("Year", _this.ObjectTableName, true);
        _this._entityResourceService.getEntityResourceByTableName("Journal").subscribe(function (response) { });
        _this._entityResourceService.getEntityResourceByTableName("JournalLine").subscribe(function (response) { });
        _this.CurrentSession.StopBusyIndicator();
        return _this;
    }
    Object.defineProperty(YearTransferComponent.prototype, "Year", {
        get: function () { return this.year; },
        set: function (value) {
            if (this.year != value) {
                this.year = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    YearTransferComponent.prototype.FillErrors = function () {
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.year)) {
            //this.Year = new Date().getFullYear();
            this.ValidationErrorsList.push("Year Field is Required");
        }
        if (this.year > (new Date().getFullYear())) {
            this.ValidationErrorsList.push("Future Year!");
        }
        else if (this.year < 1900) {
        }
        else {
            this.ValidationErrorsList = [];
        }
    };
    YearTransferComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicatorCreating();
        this._JournalOpService
            .GetYearTransferJournal(this.year)
            .subscribe(function (res) {
            if (res.HasError) {
                _this.ValidationErrorsList = res.ErrorsArray;
            }
            else {
                _this._JournalPM = res.Result;
            }
        }, function (err) {
            alert(err);
        }, function () {
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    YearTransferComponent.prototype.OpenJournal = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this._JournalPM.Id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: _this._JournalPM.Id, ObjectTableName: 'Journal' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                });
            });
        }
    };
    YearTransferComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    YearTransferComponent.prototype.OnKeyUp = function (key) {
        if (!Tools_1.AppTool.IsNullOrEmpty(key)) {
            if (key.keyCode == '13') {
                this.OkButtonClicked();
            }
        }
    };
    YearTransferComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'YearTransferComponent',
            templateUrl: './YearTransferComponent.html',
            providers: [EntityArgs_1.EntityArgs],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService, EntityArgs_1.EntityArgs])
    ], YearTransferComponent);
    return YearTransferComponent;
}(BaseComponent_1.BaseComponent));
exports.YearTransferComponent = YearTransferComponent;
//# sourceMappingURL=YearTransferComponent.js.map