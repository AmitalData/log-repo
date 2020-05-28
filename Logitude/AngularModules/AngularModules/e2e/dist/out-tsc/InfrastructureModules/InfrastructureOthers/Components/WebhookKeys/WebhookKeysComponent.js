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
var WebhookKeysPM_1 = require("../../../../Infrastructure/EntityPMs/WebhookKeysPM");
var WebhookKeysExtendedPMService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/WebhookKeysExtendedPMService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var WebhookKeysComponent = /** @class */ (function (_super) {
    __extends(WebhookKeysComponent, _super);
    function WebhookKeysComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.EntityPM = null;
        _this.ObjectTableName = "WebhookKeys";
        _this.DataContext = _this;
        _this.EntityId = null;
        _this.IsNewEntity = false;
        _this.ShowTesterButton = false;
        _this.ValidationErrorsList = [];
        _this.IsEntityReady = false;
        _this.IsResourcesReady = false;
        _this.isPrimaryGenerated = false;
        _this.isSecondaryGenerated = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.myService = new WebhookKeysExtendedPMService_1.WebhookKeysExtendedPMService();
        return _this;
    }
    WebhookKeysComponent.prototype.SetWindowArgs = function (args) {
        this.EntityId = args['EntityId'];
        this.InitializeComponent();
    };
    WebhookKeysComponent.prototype.SetNewWizardArgs = function (args) {
        this.IsNewEntity = args['IsNewEntity'];
        this.InitializeComponent();
    };
    WebhookKeysComponent.prototype.InitializeComponent = function () {
        var _this = this;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("WebhookKeys", "WebhookKeysTester")) {
            this.ShowTesterButton = true;
        }
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            //WebhookKeysTester
            _this.IsResourcesReady = true;
            if (_this.IsNewEntity) {
                _this.EntityPM = new WebhookKeysPM_1.WebhookKeysPM();
                _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                _this.EntityPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                _this.EntityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                _this.EntityPM.CreatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                _this.EntityPM.UpdatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                _this.IsEntityReady = true;
            }
            else {
                _this.CurrentSession.StartBusyIndicatorLoading();
                _this.myService.get(_this.EntityId).subscribe(function (myResponse) {
                    if (myResponse.HasError) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        _this.EntityPM = myResponse.Result;
                        if (_this.EntityPM) {
                            _this.IsEntityReady = true;
                        }
                    }
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
        });
    };
    Object.defineProperty(WebhookKeysComponent.prototype, "Description", {
        /*
            Id: string;
            AccessKey: string;
            Tenant: number;
            PartnerName: string;
            InActive: boolean;
            CreatedByUserName: string;
            CreateDate: Date;
            UpdatedByUserName: string;
            UpdateDate: Date;
            Description: string;
        */
        get: function () { return this.EntityPM.Description; },
        set: function (value) {
            if (this.EntityPM.Description != value) {
                this.EntityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WebhookKeysComponent.prototype, "AccessKey", {
        get: function () { return this.EntityPM.AccessKey; },
        set: function (value) {
            if (this.EntityPM.AccessKey != value) {
                this.EntityPM.AccessKey = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WebhookKeysComponent.prototype, "PartnerName", {
        get: function () { return this.EntityPM.PartnerName; },
        set: function (value) {
            if (this.EntityPM.PartnerName != value) {
                this.EntityPM.PartnerName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WebhookKeysComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) {
            if (this.EntityPM.InActive != value) {
                this.EntityPM.InActive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WebhookKeysComponent.prototype, "CreatedByUserName", {
        get: function () { return this.EntityPM.CreatedByUserName; },
        set: function (value) {
            if (this.EntityPM.CreatedByUserName != value) {
                this.EntityPM.CreatedByUserName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WebhookKeysComponent.prototype, "UpdatedByUserName", {
        get: function () { return this.EntityPM.UpdatedByUserName; },
        set: function (value) {
            if (this.EntityPM.UpdatedByUserName != value) {
                this.EntityPM.UpdatedByUserName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WebhookKeysComponent.prototype, "CreateDate", {
        get: function () { return this.EntityPM.CreateDate; },
        set: function (value) {
            if (this.EntityPM.CreateDate != value) {
                this.EntityPM.CreateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WebhookKeysComponent.prototype, "UpdateDate", {
        get: function () { return this.EntityPM.UpdateDate; },
        set: function (value) {
            if (this.EntityPM.UpdateDate != value) {
                this.EntityPM.UpdateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    WebhookKeysComponent.prototype.GeneratePrimaryKeyClicked = function () {
        this.isPrimaryGenerated = true;
        this.AccessKey = Tools_1.AppTool.GetNewGuid();
    };
    WebhookKeysComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    WebhookKeysComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        //if (!AppTool.IsNullOrEmpty(this.UsedFor) && !AppTool.IsNullOrEmpty(this.AllowedIPs)) {
        //    this.MaskedPrimaryAccessKey = this.CreateMaskedString(this.HashedPrimaryAccessKey);
        //    this.MaskedSeconderyAccessKey = this.CreateMaskedString(this.HashedSeconderyAccessKey);
        //}
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (this.IsNewEntity) {
                this.CurrentSession.StartBusyIndicatorCreating();
                this.myService.insert(this.EntityPM).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (myResponse.HasError) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        _this.CurrentSession.CloseCurrentWindowEmit(_this.EntityPM.Id);
                    }
                });
            }
            else {
                this.CurrentSession.StartBusyIndicatorSaving();
                this.myService.update(this.EntityPM).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (myResponse.HasError) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        _this.CurrentSession.CloseCurrentWindowEmit(_this.EntityPM.Id);
                    }
                });
            }
        }
    };
    WebhookKeysComponent.prototype.TestButtonClicked = function () {
        var windowArgs = {};
        windowArgs.AccessKey = this.AccessKey;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Width = 570;
        logitudeWindow.Height = 600;
        logitudeWindow.Title = "WebHook Tester";
        logitudeWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/WebhookKeys/WebhookTesterComponent');
    };
    WebhookKeysComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './WebhookKeysComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], WebhookKeysComponent);
    return WebhookKeysComponent;
}(BaseComponent_1.BaseComponent));
exports.WebhookKeysComponent = WebhookKeysComponent;
//# sourceMappingURL=WebhookKeysComponent.js.map