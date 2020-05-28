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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var CachedDataManager_1 = require("../../../../Infrastructure/Utilities/CachedDataManager");
var AirlineMessagingRulePMService_1 = require("../../../../Common/Services/StandardPMs/AirlineMessagingRulePMService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var AddEditAirlineMessagingRuleComponent = /** @class */ (function (_super) {
    __extends(AddEditAirlineMessagingRuleComponent, _super);
    function AddEditAirlineMessagingRuleComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AddEditAirlineMessagingRuleComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.AirlinePM = windowArgs['AirlinePM'];
        this.EntityPM = windowArgs['EntityPM'];
        this.IsNew = windowArgs['IsNew'];
        this.ObjectTableName = windowArgs['ObjectTableName'];
        this.SetUIProperties();
        this.BuildMessageTypesList();
        this.Clone();
    };
    AddEditAirlineMessagingRuleComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetVisibility("InActive", this.ObjectTableName, !this.IsNew);
        this.UIProperties.SetRequired("MessageTypeCode", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.MessageTypeCode));
    };
    AddEditAirlineMessagingRuleComponent.prototype.BuildMessageTypesList = function () {
        var _this = this;
        this.MessageTypesList = [];
        this.MessageTypesList.push(new CodeNameClass_1.CodeNameClass("FWB", "FWB"));
        this.MessageTypesList.push(new CodeNameClass_1.CodeNameClass("FHL", "FHL"));
        this.MessageTypesList.push(new CodeNameClass_1.CodeNameClass("FFR", "FFR"));
        this.MessageTypesList.push(new CodeNameClass_1.CodeNameClass("FVR", "FVR"));
        if (!this.IsNew) {
            this.SelectedMessageType = this.MessageTypesList.filter(function (d) { return d.Code == _this.EntityPM.MessageTypeCode; })[0];
            this.LoadAllowedObjectFields();
        }
    };
    Object.defineProperty(AddEditAirlineMessagingRuleComponent.prototype, "SelectedMessageType", {
        get: function () { return this.selectedMessageType; },
        set: function (value) {
            if (this.selectedMessageType != value) {
                this.selectedMessageType = value;
                if (value != null) {
                    this.MessageTypeCode = value.Code;
                    this.LoadAllowedObjectFields();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditAirlineMessagingRuleComponent.prototype.LoadAllowedObjectFields = function () {
        var _this = this;
        var tableName;
        if (this.MessageTypeCode == "FWB" || this.MessageTypeCode == "FHL") {
            tableName = "Shipment";
        }
        else if (this.MessageTypeCode == "FFR") {
            tableName = "Booking";
        }
        else if (this.MessageTypeCode == "FVR") {
            tableName = "FlightsSchedulesRequest";
        }
        var service = new EntityResourceService_1.EntityResourceService();
        service.getEntityResourceByTableName(tableName).subscribe(function (response) {
            var objectTable = window.ObjectTables.filter(function (d) { return d.Name == tableName; })[0];
            var objectFields = window.ObjectFields.filter(function (d) { return d.ObjectTableId == objectTable.Id && d.AllowedInAirlineMessaging; });
            if (objectFields.length > 0) {
                _this.RuleFieldsList = [];
                objectFields.forEach(function (item) {
                    _this.RuleFieldsList.push(new CodeNameClass_1.CodeNameClass(item.Id, item.FullNameTextCodeDefaultText));
                });
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id) && !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.RuleFieldId)) {
                    _this.selectedRuleField = _this.RuleFieldsList.filter(function (d) { return d.Code == _this.RuleFieldId; })[0];
                }
            }
        });
    };
    Object.defineProperty(AddEditAirlineMessagingRuleComponent.prototype, "SelectedRuleField", {
        get: function () { return this.selectedRuleField; },
        set: function (value) {
            if (this.selectedRuleField != value) {
                this.selectedRuleField = value;
                this.RuleFieldId = value.Code;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAirlineMessagingRuleComponent.prototype, "MessageTypeCode", {
        get: function () { return this.EntityPM.MessageTypeCode; },
        set: function (value) {
            if (this.EntityPM.MessageTypeCode != value) {
                this.EntityPM.MessageTypeCode = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAirlineMessagingRuleComponent.prototype, "RuleFieldId", {
        get: function () { return this.EntityPM.RuleFieldId; },
        set: function (value) {
            if (this.EntityPM.RuleFieldId != value) {
                this.EntityPM.RuleFieldId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAirlineMessagingRuleComponent.prototype, "MaxSize", {
        get: function () { return this.EntityPM.MaxSize; },
        set: function (value) {
            if (this.EntityPM.MaxSize != value) {
                this.EntityPM.MaxSize = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAirlineMessagingRuleComponent.prototype, "IsMandatoryForSending", {
        get: function () { return this.EntityPM.IsMandatoryForSending; },
        set: function (value) {
            if (this.EntityPM.IsMandatoryForSending != value) {
                this.EntityPM.IsMandatoryForSending = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAirlineMessagingRuleComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) {
            if (this.EntityPM.InActive != value) {
                this.EntityPM.InActive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditAirlineMessagingRuleComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditAirlineMessagingRuleComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (!this.IsMandatoryForSending && (this.MaxSize == null || this.MaxSize == 0)) {
            errors.push("You have to choose at least one validation");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            var myService = new AirlineMessagingRulePMService_1.AirlineMessagingRulePMService();
            if (this.IsNew) {
                this.CurrentSession.StartBusyIndicatorSaving();
                myService.insert(this.EntityPM).subscribe(function (Result) {
                    var mm = Result;
                    if (!mm.HasError) {
                        CachedDataManager_1.CachedDataManager.RefreshTableData(_this.ObjectTableName, true);
                        _this.CurrentSession.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        _this.ValidationErrorsList = mm.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
            else {
                this.CurrentSession.StartBusyIndicatorSaving();
                myService.update(this.EntityPM).subscribe(function (Result) {
                    var mm = Result;
                    if (!mm.HasError) {
                        CachedDataManager_1.CachedDataManager.RefreshTableData(_this.ObjectTableName, true);
                        _this.CurrentSession.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        _this.ValidationErrorsList = mm.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        }
    };
    AddEditAirlineMessagingRuleComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('MessageTypeCode');
        this.myCloner.AddField('RuleFieldId');
        this.myCloner.AddField('MaxSize');
        this.myCloner.AddField('IsMandatoryForSending');
        this.myCloner.AddField('InActive');
        this.myCloner.AddEntity(this.EntityPM);
    };
    AddEditAirlineMessagingRuleComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditAirlineMessagingRuleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditAirlineMessagingRuleComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditAirlineMessagingRuleComponent);
    return AddEditAirlineMessagingRuleComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditAirlineMessagingRuleComponent = AddEditAirlineMessagingRuleComponent;
//# sourceMappingURL=AddEditAirlineMessagingRuleComponent.js.map