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
var ApiCredintialsPM_1 = require("../../../../Infrastructure/EntityPMs/ApiCredintialsPM");
var ApiCredintialsPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/ApiCredintialsPMService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var ApiCredintialsComponent = /** @class */ (function (_super) {
    __extends(ApiCredintialsComponent, _super);
    function ApiCredintialsComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.EntityPM = null;
        _this.ObjectTableName = "ApiCredintials";
        _this.DataContext = _this;
        _this.EntityId = null;
        _this.IsNewEntity = false;
        _this.ValidationErrorsList = [];
        _this.IsEntityReady = false;
        _this.IsResourcesReady = false;
        _this.isPrimaryGenerated = false;
        _this.isSecondaryGenerated = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.myService = new ApiCredintialsPMService_1.ApiCredintialsPMService();
        return _this;
    }
    ApiCredintialsComponent.prototype.SetWindowArgs = function (args) {
        this.EntityId = args['EntityId'];
        this.InitializeComponent();
    };
    ApiCredintialsComponent.prototype.SetNewWizardArgs = function (args) {
        this.IsNewEntity = args['IsNewEntity'];
        this.InitializeComponent();
    };
    ApiCredintialsComponent.prototype.InitializeComponent = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.IsResourcesReady = true;
            if (_this.IsNewEntity) {
                _this.EntityPM = new ApiCredintialsPM_1.ApiCredintialsPM();
                _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                _this.EntityPM.AllowedIPs = "*";
                _this.EntityPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                _this.EntityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                _this.EntityPM.CreatedBy = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                _this.EntityPM.UpdatedBy = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
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
    Object.defineProperty(ApiCredintialsComponent.prototype, "UsedFor", {
        get: function () { return this.EntityPM.UsedFor; },
        set: function (value) {
            if (this.EntityPM.UsedFor != value) {
                this.EntityPM.UsedFor = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ApiCredintialsComponent.prototype, "AllowedIPs", {
        get: function () { return this.EntityPM.AllowedIPs; },
        set: function (value) {
            if (this.EntityPM.AllowedIPs != value) {
                this.EntityPM.AllowedIPs = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ApiCredintialsComponent.prototype, "HashedPrimaryAccessKey", {
        get: function () {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.HashedPrimaryAccessKey)) {
                this.EntityPM.HashedPrimaryAccessKey = Tools_1.AppTool.GetNewGuid();
            }
            if (this.IsNewEntity || this.isPrimaryGenerated) {
                return this.EntityPM.HashedPrimaryAccessKey;
            }
            else {
                return this.EntityPM.MaskedPrimaryAccessKey;
            }
        },
        set: function (value) {
            if (this.EntityPM.HashedPrimaryAccessKey != value) {
                this.EntityPM.HashedPrimaryAccessKey = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ApiCredintialsComponent.prototype, "HashedSeconderyAccessKey", {
        get: function () {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.HashedSeconderyAccessKey)) {
                this.EntityPM.HashedSeconderyAccessKey = Tools_1.AppTool.GetNewGuid();
            }
            if (this.IsNewEntity || this.isSecondaryGenerated) {
                return this.EntityPM.HashedSeconderyAccessKey;
            }
            else {
                return this.EntityPM.MaskedSeconderyAccessKey;
            }
        },
        set: function (value) {
            if (this.EntityPM.HashedSeconderyAccessKey != value) {
                this.EntityPM.HashedSeconderyAccessKey = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ApiCredintialsComponent.prototype, "MaskedPrimaryAccessKey", {
        get: function () { return this.EntityPM.MaskedPrimaryAccessKey; },
        set: function (value) {
            if (this.EntityPM.MaskedPrimaryAccessKey != value) {
                this.EntityPM.MaskedPrimaryAccessKey = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ApiCredintialsComponent.prototype, "MaskedSeconderyAccessKey", {
        get: function () { return this.EntityPM.MaskedSeconderyAccessKey; },
        set: function (value) {
            if (this.EntityPM.MaskedSeconderyAccessKey != value) {
                this.EntityPM.MaskedSeconderyAccessKey = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ApiCredintialsComponent.prototype, "CreatedBy", {
        get: function () { return this.EntityPM.CreatedBy; },
        set: function (value) {
            if (this.EntityPM.CreatedBy != value) {
                this.EntityPM.CreatedBy = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ApiCredintialsComponent.prototype, "UpdatedBy", {
        get: function () { return this.EntityPM.UpdatedBy; },
        set: function (value) {
            if (this.EntityPM.UpdatedBy != value) {
                this.EntityPM.UpdatedBy = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ApiCredintialsComponent.prototype, "CreateDate", {
        get: function () { return this.EntityPM.CreateDate; },
        set: function (value) {
            if (this.EntityPM.CreateDate != value) {
                this.EntityPM.CreateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ApiCredintialsComponent.prototype, "UpdateDate", {
        get: function () { return this.EntityPM.UpdateDate; },
        set: function (value) {
            if (this.EntityPM.UpdateDate != value) {
                this.EntityPM.UpdateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //get ComputingPartnerId() { return this.EntityPM.ComputingPartnerId; }
    //set ComputingPartnerId(value: string) {
    //    if (this.EntityPM.ComputingPartnerId != value) {
    //        this.EntityPM.ComputingPartnerId = value;
    //    }
    //}
    ApiCredintialsComponent.prototype.GeneratePrimaryKeyClicked = function () {
        this.isPrimaryGenerated = true;
        this.HashedPrimaryAccessKey = Tools_1.AppTool.GetNewGuid();
    };
    ApiCredintialsComponent.prototype.GenerateSeconderyKeyClicked = function () {
        this.isSecondaryGenerated = true;
        this.HashedSeconderyAccessKey = Tools_1.AppTool.GetNewGuid();
    };
    ApiCredintialsComponent.prototype.CreateMaskedString = function (Key) {
        var maskedPKey = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(Key)) {
            var i = 0;
            for (var index = 0; index < Key.length; index++) {
                var item = Key[index];
                if (item != '-' && i < 32) {
                    maskedPKey += "*";
                }
                else {
                    maskedPKey += item;
                }
                i++;
            }
        }
        return maskedPKey;
    };
    ApiCredintialsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ApiCredintialsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.UsedFor) && !Tools_1.AppTool.IsNullOrEmpty(this.AllowedIPs)) {
            this.MaskedPrimaryAccessKey = this.CreateMaskedString(this.HashedPrimaryAccessKey);
            this.MaskedSeconderyAccessKey = this.CreateMaskedString(this.HashedSeconderyAccessKey);
        }
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
    ApiCredintialsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ApiCredintialsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], ApiCredintialsComponent);
    return ApiCredintialsComponent;
}(BaseComponent_1.BaseComponent));
exports.ApiCredintialsComponent = ApiCredintialsComponent;
//# sourceMappingURL=ApiCredintialsComponent.js.map