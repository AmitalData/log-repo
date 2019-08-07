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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var CourierPendingReasonPM_1 = require("../../../../Customs/EntityPMs/CourierPendingReasonPM");
var CourierPendingReasonPMService_1 = require("../../../../Customs/Services/StandardPMs/CourierPendingReasonPMService");
var CourierPendingReasonExtendedListService_1 = require("../../../../Customs/Services/ExtendedLists/CourierPendingReasonExtendedListService");
var AddCourierPendingToUnifreightStatusComponent = /** @class */ (function (_super) {
    __extends(AddCourierPendingToUnifreightStatusComponent, _super);
    function AddCourierPendingToUnifreightStatusComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.CourierPendingReason";
        _this.ValidationErrorsList = [];
        _this._CourierPendingReasonPMService = new CourierPendingReasonPMService_1.CourierPendingReasonPMService();
        _this._CourierPendingReasonExtendedListService = new CourierPendingReasonExtendedListService_1.CourierPendingReasonExtendedListService();
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.CourierPendingReasonList = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Loaded = false;
        return _this;
    }
    AddCourierPendingToUnifreightStatusComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            _this.Loaded = true;
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    AddCourierPendingToUnifreightStatusComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            if (args.FromUnifreight && !Tools_1.AppTool.IsNullOrEmpty(args.UnifreightStatusCode)) {
                this.UnifreightStatusCode = args.UnifreightStatusCode;
                this.CurrentSession.StartBusyIndicatorLoading();
                this._CourierPendingReasonExtendedListService.GetCourierPendingReasonByUnifreightStatus(this.UnifreightStatusCode).subscribe(function (response) {
                    _this.CurrentSession.StopBusyIndicator();
                    var courierPendingReasonResult = response.Result;
                    _this.BuildCourierPendingReasonList(courierPendingReasonResult);
                });
            }
        }
    };
    AddCourierPendingToUnifreightStatusComponent.prototype.BuildCourierPendingReasonList = function (courierPendingReasonResult) {
        this.CourierPendingReasonList = new ObservableCollection_1.ObservableCollection([]);
        if (courierPendingReasonResult != null && courierPendingReasonResult.length > 0) {
            for (var _i = 0, courierPendingReasonResult_1 = courierPendingReasonResult; _i < courierPendingReasonResult_1.length; _i++) {
                var item = courierPendingReasonResult_1[_i];
                this.CourierPendingReasonList.Insert(new CourierPendingReasonLineComponent(item, false, this));
            }
        }
    };
    Object.defineProperty(AddCourierPendingToUnifreightStatusComponent.prototype, "UnifreightStatusCode", {
        get: function () { return this._UnifreightStatusCode; },
        set: function (newValue) {
            this._UnifreightStatusCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    AddCourierPendingToUnifreightStatusComponent.prototype.AddNewPendingCommand = function () {
        var newClaimsRelatedEntityPM = new CourierPendingReasonPM_1.CourierPendingReasonPM();
        newClaimsRelatedEntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newClaimsRelatedEntityPM.UnifreightStatusCode = this.UnifreightStatusCode;
        var newClaimsRelatedEntityLineComponent = new CourierPendingReasonLineComponent(newClaimsRelatedEntityPM, true, this);
        this.CourierPendingReasonList.Insert(newClaimsRelatedEntityLineComponent);
    };
    AddCourierPendingToUnifreightStatusComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddCourierPendingToUnifreightStatusComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (this.CourierPendingReasonList != null) {
            this.CourierPendingReasonList.Collection.forEach(function (item) {
                if (item.isNew) {
                    item.entityPM.UnifreightStatusCode = _this.UnifreightStatusCode;
                    _this._CourierPendingReasonPMService.update(item.entityPM).subscribe(function (myResult) {
                        if (myResult.HasError) {
                            _this.ValidationErrorsList = [];
                            _this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                            return;
                        }
                    });
                }
            });
        }
        this.CancelButtonClicked();
    };
    AddCourierPendingToUnifreightStatusComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddCourierPendingToUnifreightStatusComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddCourierPendingToUnifreightStatusComponent);
    return AddCourierPendingToUnifreightStatusComponent;
}(BaseComponent_1.BaseComponent));
exports.AddCourierPendingToUnifreightStatusComponent = AddCourierPendingToUnifreightStatusComponent;
var CourierPendingReasonLineComponent = /** @class */ (function (_super) {
    __extends(CourierPendingReasonLineComponent, _super);
    function CourierPendingReasonLineComponent(entityPM, isNew, parent) {
        var _this = _super.call(this) || this;
        _this.entityPM = entityPM;
        _this.isNew = isNew;
        _this.parent = parent;
        _this.ObjectTableName = "Customs.CourierPendingReason";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (!isNew) {
            _this.UIProperties.SetEnabled("PendingCode", _this.ObjectTableName, false);
        }
        return _this;
    }
    Object.defineProperty(CourierPendingReasonLineComponent.prototype, "PendingCode", {
        //private _PendingCode: string;
        get: function () { return this.entityPM.Code; },
        set: function (newValue) {
            var _this = this;
            this.CurrentSession.StartBusyIndicatorLoading();
            if (newValue) {
                this.parent._CourierPendingReasonPMService.get(newValue).subscribe(function (response) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!response.HasError && response.Result != null) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(response.Result.UnifreightStatusCode) && response.Result.UnifreightStatusCode != _this.parent.UnifreightStatusCode) {
                            var confirm = new ConfirmWindow_1.ConfirmWindow();
                            confirm.Width = 350;
                            confirm.Height = 200;
                            confirm.Title = "קישור Pending לסטטוס";
                            confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
                            confirm.ShowNoButton = true;
                            confirm.Show("לקוד זה כבר קושר סטטוס " + response.Result.UnifreightStatusCode + " האם להחליף לסטטוס " + _this.parent.UnifreightStatusCode + "?");
                            confirm.WindowClosed.subscribe(function (event) {
                                if (confirm.Yes) {
                                    _this.entityPM = response.Result;
                                }
                                confirm.Close();
                            });
                        }
                        else {
                            _this.entityPM = response.Result;
                        }
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierPendingReasonLineComponent.prototype, "PendingLocalName", {
        get: function () { return this.entityPM.LocalName; },
        set: function (newValue) { this.entityPM.LocalName = newValue; },
        enumerable: true,
        configurable: true
    });
    CourierPendingReasonLineComponent.prototype.DeleteCommand = function () {
        var _this = this;
        if (!this.isNew) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.parent._CourierPendingReasonExtendedListService.DeleteCourierPendingReasonUnifreightStatus(this.PendingCode).subscribe(function (response) {
                _this.CurrentSession.StopBusyIndicator();
                if (response.HasError) {
                    _this.parent.ValidationErrorsList = [];
                    _this.parent.ValidationErrorsList.push(response.ErrorsArray[0]);
                    return;
                }
            });
        }
        this.parent.CourierPendingReasonList.Remove(this);
    };
    return CourierPendingReasonLineComponent;
}(BaseComponent_1.BaseComponent));
exports.CourierPendingReasonLineComponent = CourierPendingReasonLineComponent;
//# sourceMappingURL=AddCourierPendingToUnifreightStatusComponent.js.map