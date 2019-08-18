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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var CourierPendingReasonPM_1 = require("../../../../Customs/EntityPMs/CourierPendingReasonPM");
var CourierPendingReasonPMService_1 = require("../../../../Customs/Services/StandardPMs/CourierPendingReasonPMService");
var CourierPendingReasonExtendedListService_1 = require("../../../../Customs/Services/ExtendedLists/CourierPendingReasonExtendedListService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var AddEditCourierPendingReasonComponent = /** @class */ (function (_super) {
    __extends(AddEditCourierPendingReasonComponent, _super);
    function AddEditCourierPendingReasonComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.CourierPendingReason";
        _this.isWindowMode = false;
        _this.isNewRecord = false;
        _this.isFromUnifreight = false;
        _this.ValidationErrorsList = [];
        _this._EntityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._CourierPendingReasonPMService = new CourierPendingReasonPMService_1.CourierPendingReasonPMService();
        _this._CourierPendingReasonExtendedListService = new CourierPendingReasonExtendedListService_1.CourierPendingReasonExtendedListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Loaded = false;
        _this.CurrentSession.StartBusyIndicator("");
        _this._EntityResourceService.getEntityResourceByTableName(_this.ObjectTableName).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            if (Tools_1.AppTool.IsNullOrEmpty(entityArgs.EntityPM)) {
                _this.EntityPM = new CourierPendingReasonPM_1.CourierPendingReasonPM();
                _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                _this.isWindowMode = true;
                _this.isNewRecord = true;
            }
            else {
                _this.EntityPM = _this.entityArgs.EntityPM;
                _this.UnifreightStatusCode = _this.EntityPM.UnifreightStatusCode;
            }
            _this.UIProperties.SetEnabled("UnifreightStatusCode", _this.ObjectTableName, false);
        });
        return _this;
    }
    AddEditCourierPendingReasonComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            _this.Loaded = true;
        });
    };
    AddEditCourierPendingReasonComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.isWindowMode = true;
            this.isFromUnifreight = true;
            if (args.FromUnifreight && !Tools_1.AppTool.IsNullOrEmpty(args.UnifreightStatusCode)) {
                this.isNewRecord = true;
                this.EntityPM = new CourierPendingReasonPM_1.CourierPendingReasonPM();
                this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                this.UnifreightStatusCode = args.UnifreightStatusCode;
                this.UIProperties.SetEnabled("UnifreightStatusCode", this.ObjectTableName, false);
                this._CourierPendingReasonExtendedListService.GetCourierPendingReasonByUnifreightStatus(this.UnifreightStatusCode).subscribe(function (response) {
                    var courierPendingReasonResult = response.Result;
                    if (courierPendingReasonResult != null && courierPendingReasonResult.length > 0) {
                        _this.isNewRecord = false;
                        _this.EntityPM = courierPendingReasonResult[0];
                        if (courierPendingReasonResult.length > 1) {
                            _this.WarningMessage = "סטטוס " + _this.UnifreightStatusCode + " מקושר למספר קודים. מוצגת הרשומה הראשונה בלבד";
                        }
                    }
                });
            }
        }
    };
    Object.defineProperty(AddEditCourierPendingReasonComponent.prototype, "WarningMessage", {
        get: function () { return this._WarningMessage; },
        set: function (newValue) {
            this._WarningMessage = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCourierPendingReasonComponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        set: function (newValue) {
            this.EntityPM.Code = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCourierPendingReasonComponent.prototype, "PendingCode", {
        get: function () { return this.EntityPM.Code; },
        set: function (newValue) {
            var _this = this;
            if (this.isFromUnifreight) {
                if (this.EntityPM != null && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Code)) {
                    this.EntityPM.UnifreightStatusCode = null;
                    this.CurrentSession.StartBusyIndicatorLoading();
                    this._CourierPendingReasonPMService.update(this.EntityPM).subscribe(function (myResult) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (myResult.HasError) {
                            _this.ValidationErrorsList = [];
                            _this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                            return;
                        }
                        _this.EntityPM = new CourierPendingReasonPM_1.CourierPendingReasonPM();
                    });
                }
                if (newValue) {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    this._CourierPendingReasonPMService.get(newValue).subscribe(function (response) {
                        if (!response.HasError && response.Result != null) {
                            _this.CurrentSession.StopBusyIndicator();
                            if (!Tools_1.AppTool.IsNullOrEmpty(response.Result.UnifreightStatusCode) && response.Result.UnifreightStatusCode != _this.UnifreightStatusCode) {
                                var confirm = new ConfirmWindow_1.ConfirmWindow();
                                confirm.Width = 350;
                                confirm.Height = 200;
                                confirm.Title = "קישור Pending לסטטוס";
                                confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
                                confirm.ShowNoButton = true;
                                confirm.Show("לקוד זה כבר קושר סטטוס " + response.Result.UnifreightStatusCode + " האם להחליף לסטטוס " + _this.UnifreightStatusCode + "?");
                                confirm.WindowClosed.subscribe(function (event) {
                                    if (confirm.No) {
                                        confirm.Close();
                                        _this.EntityPM = new CourierPendingReasonPM_1.CourierPendingReasonPM();
                                        return;
                                    }
                                    confirm.Close();
                                });
                            }
                            _this.isNewRecord = false;
                            _this.EntityPM = response.Result;
                            _this.EntityPM.UnifreightStatusCode = _this.UnifreightStatusCode;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCourierPendingReasonComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (newValue) {
            this.EntityPM.EnglishName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCourierPendingReasonComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (newValue) {
            this.EntityPM.LocalName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCourierPendingReasonComponent.prototype, "Inactive", {
        get: function () { return this.EntityPM.Inactive; },
        set: function (newValue) {
            this.EntityPM.Inactive = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCourierPendingReasonComponent.prototype, "ErrorPlace", {
        get: function () { return this.EntityPM.ErrorPlace; },
        set: function (newValue) {
            this.EntityPM.ErrorPlace = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCourierPendingReasonComponent.prototype, "UnifreightStatusCode", {
        get: function () { return this._UnifreightStatusCode; },
        set: function (newValue) {
            this._UnifreightStatusCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    //#endregion\
    AddEditCourierPendingReasonComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        }
        else {
            if (this.EntityPM == null || (this.EntityPM != null && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Code))) {
                this.CancelButtonClicked();
            }
            this.EntityPM.UnifreightStatusCode = this._UnifreightStatusCode;
            if (this.isNewRecord) {
                this._CourierPendingReasonPMService.insert(this.EntityPM).subscribe(function (myResult) {
                    if (myResult.HasError) {
                        _this.ValidationErrorsList = [];
                        _this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                        return;
                    }
                    _this.CancelButtonClicked();
                });
            }
            else {
                this._CourierPendingReasonPMService.update(this.EntityPM).subscribe(function (myResult) {
                    if (myResult.HasError) {
                        _this.ValidationErrorsList = [];
                        _this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                        return;
                    }
                    _this.CancelButtonClicked();
                });
            }
        }
    };
    AddEditCourierPendingReasonComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditCourierPendingReasonComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditCourierPendingReasonComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AddEditCourierPendingReasonComponent);
    return AddEditCourierPendingReasonComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditCourierPendingReasonComponent = AddEditCourierPendingReasonComponent;
//# sourceMappingURL=AddEditCourierPendingReasonComponent.js.map