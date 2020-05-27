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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var GlobalDomainService_1 = require("../../../../Common/Services/GlobalDomainService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var MessagingStockPM_1 = require("../../../../Shipment/EntityPMs/MessagingStockPM");
var MessagingStockPMService_1 = require("../../../../Shipment/Services/StandardPMs/MessagingStockPMService");
var StockNewWizardComponent = /** @class */ (function (_super) {
    __extends(StockNewWizardComponent, _super);
    function StockNewWizardComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "MessagingStock";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.LoadedTenantsList = [];
        _this.SelectedTenantItem = null;
        _this.SelectedStockTypeItem = null;
        _this.TenantsList = [];
        _this.StockTypesList = [];
        _this.StockTypesList.push(new CodeNameClass(1, 'Champ'));
        _this.StockTypesList.push(new CodeNameClass(2, 'INTTRA'));
        _this.CreateEntityPM();
        _this.LoadAWBTenants();
        _this.SetUIProperties();
        return _this;
    }
    StockNewWizardComponent.prototype.CreateEntityPM = function () {
        this.EntityPM = new MessagingStockPM_1.MessagingStockPM();
        this.EntityPM.DummyTenant = InfraSettings_1.InfraSettings.TenantPM.Id;
        this.EntityPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.UpdateDate = this.EntityPM.CreateDate;
        this.EntityPM.CreatedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
        this.EntityPM.UpdatedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
    };
    StockNewWizardComponent.prototype.LoadAWBTenants = function () {
        var _this = this;
        this.TenantsList = [];
        if (this.myDomainService == null) {
            this.myDomainService = new GlobalDomainService_1.GlobalDomainService();
        }
        this.myDomainService.GetMessagingStockTenantsList(InfraSettings_1.InfraSettings.TenantPM.Id).subscribe(function (myResponse) {
            if (myResponse != null) {
                _this.LoadedTenantsList = myResponse.Result;
            }
        });
    };
    StockNewWizardComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("StockType", this.ObjectTableName, this.SelectedStockTypeItem == null ? true : false);
        this.UIProperties.SetRequired("TenantNumber", this.ObjectTableName, this.SelectedTenantItem == null ? true : false);
    };
    StockNewWizardComponent.prototype.SelectedItemChanged = function (item) {
        if (this.SelectedTenantItem != item) {
            this.SelectedTenantItem = item;
            var myResult = null;
            if (item != null) {
                myResult = item.Code;
            }
            this.TenantNumber = myResult;
        }
    };
    StockNewWizardComponent.prototype.SelectedStockTypeChanged = function (item) {
        if (this.SelectedStockTypeItem != item) {
            this.SelectedStockTypeItem = item;
            var myResult = null;
            if (item != null) {
                myResult = item.Name;
            }
            this.StockType = myResult;
        }
    };
    Object.defineProperty(StockNewWizardComponent.prototype, "StockType", {
        get: function () { return this.EntityPM.StockType; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.StockType != value) {
                this.EntityPM.StockType = value;
                this.SetUIProperties();
                this.TenantsList = [];
                this.SelectedItemChanged(null);
                this.LoadedTenantsList.forEach(function (item) {
                    if (value == "Champ") {
                        if (item.IsAWBStockPrepaid) {
                            _this.TenantsList.push(new CodeNameClass(item.Id, item.Name));
                        }
                    }
                    else {
                        if (item.IsINTTRAStockPrepaid) {
                            _this.TenantsList.push(new CodeNameClass(item.Id, item.Name));
                        }
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockNewWizardComponent.prototype, "TenantNumber", {
        get: function () { return this.EntityPM.TenantNumber; },
        set: function (newValue) {
            if (this.EntityPM.TenantNumber != newValue) {
                this.EntityPM.TenantNumber = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockNewWizardComponent.prototype, "StartDate", {
        get: function () { return this.EntityPM.StartDate; },
        set: function (newValue) {
            if (this.EntityPM.StartDate != newValue) {
                this.EntityPM.StartDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockNewWizardComponent.prototype, "EndDate", {
        get: function () { return this.EntityPM.EndDate; },
        set: function (newValue) {
            if (this.EntityPM.EndDate != newValue) {
                this.EntityPM.EndDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockNewWizardComponent.prototype, "Amount", {
        get: function () { return this.EntityPM.Amount; },
        set: function (newValue) {
            if (this.EntityPM.Amount != newValue) {
                this.EntityPM.Amount = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockNewWizardComponent.prototype, "Remaining", {
        get: function () { return this.EntityPM.Remaining; },
        set: function (newValue) {
            if (this.EntityPM.Remaining != newValue) {
                this.EntityPM.Remaining = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockNewWizardComponent.prototype, "TotalPrice", {
        get: function () { return this.EntityPM.TotalPrice; },
        set: function (newValue) {
            if (this.EntityPM.TotalPrice != newValue) {
                this.EntityPM.TotalPrice = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockNewWizardComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    StockNewWizardComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    StockNewWizardComponent.prototype.OkButtonClicked = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.SelectedTenantItem == null) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("MessagingStock.F.TenantNumber")));
        }
        if (this.StartDate != null && this.EndDate != null) {
            var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            if (this.EndDate.valueOf() < todayDate.valueOf()) {
                errors.push("End date cant be past date");
            }
            else if (this.EndDate <= this.StartDate) {
                errors.push("End date must be bigger than start date");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitCreating();
        }
    };
    StockNewWizardComponent.prototype.SubmitCreating = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorCreating();
        var myService = new MessagingStockPMService_1.MessagingStockPMService();
        myService.insert(this.EntityPM).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit('OK');
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        }, function (error) {
            _this.CurrentSession.StopBusyIndicator();
            var dd = error;
            console.log(dd.text);
        });
    };
    StockNewWizardComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './StockNewWizardComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], StockNewWizardComponent);
    return StockNewWizardComponent;
}(BaseComponent_1.BaseComponent));
exports.StockNewWizardComponent = StockNewWizardComponent;
var CodeNameClass = /** @class */ (function () {
    function CodeNameClass(code, name) {
        this.Code = code;
        this.Name = name;
    }
    return CodeNameClass;
}());
//# sourceMappingURL=StockNewWizardComponent.js.map