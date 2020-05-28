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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var ClaimsRelatedEntityPM_1 = require("../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM");
var ClaimPM_1 = require("../../../../../Customs/EntityPMs/ClaimPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
//import { ClaimRelatedEntReasonExpLineComponent } from './ClaimRelatedEntityReasonsTabComponent';
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var ClaimRelatedEntityCustomAnswerTabComponent = /** @class */ (function (_super) {
    __extends(ClaimRelatedEntityCustomAnswerTabComponent, _super);
    function ClaimRelatedEntityCustomAnswerTabComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.EntityPM = new ClaimsRelatedEntityPM_1.ClaimsRelatedEntityPM(new ClaimPM_1.ClaimPM);
        _this.ObjectTableName = "Customs.ClaimsRelatedEntity";
        _this.isControlEnabled = true;
        _this._ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._MyResponseObjectToShow = null;
        _this.ClaimRelatedEntityCustomAnswerlist = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        return _this;
    }
    ClaimRelatedEntityCustomAnswerTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildCustomAnswerList();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    //if (tabCode == "CLMG") {
                    //    this.RefreshEntity();
                    //    this.BuildReasonslist();
                    //}
                }
            }));
        }
    };
    ClaimRelatedEntityCustomAnswerTabComponent.prototype.InitTab = function (entityPM, claimPM, isEnable) {
        var _this = this;
        this.EntityPM = entityPM;
        this.isControlEnabled = isEnable;
        this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntity").subscribe(function (response) {
                _this.BuildCustomAnswerList();
                _this.Listen();
            });
        });
    };
    ClaimRelatedEntityCustomAnswerTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    Object.defineProperty(ClaimRelatedEntityCustomAnswerTabComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntityCustomAnswerTabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    };
    Object.defineProperty(ClaimRelatedEntityCustomAnswerTabComponent.prototype, "IsControlEnabled", {
        get: function () { return this.isControlEnabled; },
        set: function (newValue) { this.isControlEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimRelatedEntityCustomAnswerTabComponent.prototype, "ValidationErrorsList", {
        get: function () { return this._ValidationErrorsList; },
        set: function (newValue) { this._ValidationErrorsList = newValue; },
        enumerable: true,
        configurable: true
    });
    ClaimRelatedEntityCustomAnswerTabComponent.prototype.BuildCustomAnswerList = function () {
        this.ClaimRelatedEntityCustomAnswerlist = new ObservableCollection_1.ObservableCollection([]);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomsExceptions)) {
            this._MyResponseObjectToShow = JSON.parse(this.EntityPM.CustomsExceptions);
            if (this._MyResponseObjectToShow != null
                && this._MyResponseObjectToShow.ClaimCustomsExceptions != null
                && this._MyResponseObjectToShow.ClaimCustomsExceptions.CustomsExceptions != null)
                this.ClaimRelatedEntityCustomAnswerlist.Insert(this._MyResponseObjectToShow.ClaimCustomsExceptions.CustomsExceptions.CustomsException);
        }
    };
    ClaimRelatedEntityCustomAnswerTabComponent.prototype.ShowMore = function (itemContent, itemSubject) {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 1000;
        logitudeWindow.Height = 500;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = itemSubject;
        logitudeWindow.WindowArgs = itemContent;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCommunications/Components/Communications/LogFieldComponent');
    };
    ClaimRelatedEntityCustomAnswerTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClaimRelatedEntityCustomAnswerTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ClaimRelatedEntityCustomAnswerTabComponent);
    return ClaimRelatedEntityCustomAnswerTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimRelatedEntityCustomAnswerTabComponent = ClaimRelatedEntityCustomAnswerTabComponent;
//#endregion
var CustomsException = /** @class */ (function (_super) {
    __extends(CustomsException, _super);
    function CustomsException(exceptionType, exceptionDescription) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ExceptionType = exceptionType;
        _this.ExceptionDescription = exceptionDescription;
        return _this;
    }
    Object.defineProperty(CustomsException.prototype, "ExceptionType", {
        get: function () { return this._ExceptionType; },
        set: function (newValue) { this._ExceptionType; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsException.prototype, "ExceptionDescription", {
        get: function () { return this._ExceptionDescription; },
        set: function (newValue) { this._ExceptionDescription; },
        enumerable: true,
        configurable: true
    });
    return CustomsException;
}(BaseComponent_1.BaseComponent));
exports.CustomsException = CustomsException;
var CustomsExceptions = /** @class */ (function (_super) {
    __extends(CustomsExceptions, _super);
    function CustomsExceptions() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        return _this;
    }
    Object.defineProperty(CustomsExceptions.prototype, "CustomsNotes", {
        get: function () { return this._CustomsNotes; },
        set: function (newValue) { this._CustomsNotes = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsExceptions.prototype, "CustomsExceptions", {
        get: function () { return this._CustomsExceptions; },
        set: function (newValue) { this._CustomsExceptions = newValue; },
        enumerable: true,
        configurable: true
    });
    return CustomsExceptions;
}(BaseComponent_1.BaseComponent));
exports.CustomsExceptions = CustomsExceptions;
//# sourceMappingURL=ClaimRelatedEntityCustomAnswerTabComponent.js.map