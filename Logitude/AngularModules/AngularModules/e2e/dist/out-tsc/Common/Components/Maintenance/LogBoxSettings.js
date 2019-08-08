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
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TenantPM_1 = require("../../EntityPMs/TenantPM");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var TenantPMService_1 = require("../../Services/StandardPMs/TenantPMService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var LogBoxSettings = /** @class */ (function (_super) {
    __extends(LogBoxSettings, _super);
    function LogBoxSettings(_entityResourceService) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Tenant";
        _this.TenantPm = new TenantPM_1.TenantPM();
        _this.IsVisibile = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Load Tenant 
        _this.IsTenantUS = false;
        _this.Agent = new StockTypesDetails("A", "Agent");
        _this.Customer = new StockTypesDetails("C", "Customer");
        _this.LoadTenantPMMethod();
        _this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
        });
        return _this;
    }
    LogBoxSettings.prototype.ngOnInit = function () {
        this.StockTypes = this.GetStockTypes();
    };
    LogBoxSettings.prototype.ngAfterViewInit = function () {
    };
    LogBoxSettings.prototype.LoadTenantPMMethod = function () {
        var _this = this;
        var myService = new TenantPMService_1.TenantPMService();
        myService.get(SessionLocator_1.SessionLocator.TenantPM.Id).subscribe(function (response) {
            _this.TenantPm = response.Result;
            _this.IsVisibile = true;
            if (_this.TenantPm.CountryCode.toUpperCase() == "US") {
                _this.IsTenantUS = true;
            }
        });
    };
    LogBoxSettings.prototype.GetStockTypes = function () {
        var list = [];
        list.push(this.Agent);
        list.push(this.Customer);
        return list;
    };
    LogBoxSettings.prototype.StockTypeValueChanged = function (event) {
        this.StockTypeCode = event.Code;
        this.StockType = event;
    };
    Object.defineProperty(LogBoxSettings.prototype, "StockType", {
        get: function () {
            if (this.TenantPm.StockTypeCode == "A") {
                return this.Agent;
            }
            else if (this.TenantPm.StockTypeCode == "C") {
                return this.Customer;
            }
            return this.stockType;
        },
        set: function (value) {
            if (this.stockType != value) {
                this.stockType = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogBoxSettings.prototype, "StockTypes", {
        get: function () { return this.stockTypes; },
        set: function (newValue) {
            this.stockTypes = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogBoxSettings.prototype, "CustomerId", {
        get: function () { return this.TenantPm.CustomerId; },
        set: function (value) {
            if (this.TenantPm.CustomerId != value) {
                this.TenantPm.CustomerId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogBoxSettings.prototype, "StockTypeCode", {
        get: function () { return this.TenantPm.StockTypeCode; },
        set: function (value) {
            if (this.TenantPm.StockTypeCode != value) {
                this.TenantPm.StockTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogBoxSettings.prototype, "DocumentShareAsDefault", {
        get: function () { return this.TenantPm.DocumentShareAsDefault; },
        set: function (newValue) {
            if (this.TenantPm.DocumentShareAsDefault != newValue) {
                this.TenantPm.DocumentShareAsDefault = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogBoxSettings.prototype, "AutoArchiveOnInvoice", {
        get: function () { return this.TenantPm.AutoArchiveOnInvoice; },
        set: function (newValue) {
            if (this.TenantPm.AutoArchiveOnInvoice != newValue) {
                this.TenantPm.AutoArchiveOnInvoice = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogBoxSettings.prototype, "IsCustomerTenantShare", {
        get: function () { return this.TenantPm.IsCustomerTenantShare; },
        set: function (value) {
            if (this.TenantPm.IsCustomerTenantShare != value) {
                this.TenantPm.IsCustomerTenantShare = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogBoxSettings.prototype, "CustomerTenantShareImportFile", {
        get: function () { return this.TenantPm.CustomerTenantShareImportFile; },
        set: function (value) {
            if (this.TenantPm.CustomerTenantShareImportFile != value) {
                this.TenantPm.CustomerTenantShareImportFile = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogBoxSettings.prototype, "CustomerTenantShareImportFileVisible", {
        get: function () {
            var result = false;
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES")) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogBoxSettings.prototype, "IsCustomerTenantShareVisible", {
        get: function () {
            var result = false;
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES")) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    //Commands 
    LogBoxSettings.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    LogBoxSettings.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.DataContext.TenantPm, this.DataContext.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitTenantChanges();
        }
    };
    LogBoxSettings.prototype.SubmitTenantChanges = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Saving...");
        var myService = new TenantPMService_1.TenantPMService();
        myService.update(this.TenantPm).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    InfraSettings_1.InfraSettings.TenantPM = _this.TenantPm;
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    LogBoxSettings = __decorate([
        core_1.Component({
            selector: 'LogBoxSettings',
            moduleId: module.id,
            templateUrl: './LogBoxSettings.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], LogBoxSettings);
    return LogBoxSettings;
}(BaseComponent_1.BaseComponent));
exports.LogBoxSettings = LogBoxSettings;
var StockTypesDetails = /** @class */ (function () {
    function StockTypesDetails(code, name) {
        this.Code = code;
        this.Name = name;
    }
    Object.defineProperty(StockTypesDetails.prototype, "Code", {
        get: function () { return this.code; },
        set: function (newValue) { this.code = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockTypesDetails.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; },
        enumerable: true,
        configurable: true
    });
    return StockTypesDetails;
}());
exports.StockTypesDetails = StockTypesDetails;
//# sourceMappingURL=LogBoxSettings.js.map