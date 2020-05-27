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
var EntityArgs_1 = require("../DataContracts/EntityArgs");
var BaseComponent_1 = require("../Components/LogitudeComponents/BaseComponent");
var SessionInfo_1 = require("../Utilities/SessionInfo");
var Tools_1 = require("../Tools");
var SessionLocator_1 = require("../Utilities/SessionLocator");
var GeneratedComponent = /** @class */ (function (_super) {
    __extends(GeneratedComponent, _super);
    function GeneratedComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.LabelWidth = 160;
        _this.ShowNoFieldsText = false;
        _this.ShowTitle = false;
        _this.IsCustomerCare = false;
        _this.LoadCompleted = new core_1.EventEmitter();
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.isViewEnited = false;
        _this.isScreenEnabled = true;
        _this.IsCustomerCare = SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare;
        return _this;
    }
    GeneratedComponent.prototype.Run = function (entityPM, objectTableName, screenCode, isNewEntityCall, showTitle) {
        var _this = this;
        if (isNewEntityCall === void 0) { isNewEntityCall = false; }
        if (showTitle === void 0) { showTitle = false; }
        this.EntityPM = entityPM;
        this.ScreenCode = screenCode ? screenCode.replace("Customs.", "") : screenCode;
        this.ObjectTableName = objectTableName;
        this.ObjectTableId = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0].Id;
        this.IsNewEntityCall = isNewEntityCall;
        this.ShowTitle = showTitle;
        this.BuildScreen();
        this.Listen();
    };
    GeneratedComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs) {
            if (this.entityArgs.EditComponent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                        _this.BuildScreen();
                    }
                });
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                        _this.BuildScreen();
                    }
                });
            }
        }
    };
    GeneratedComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    GeneratedComponent.prototype.ngAfterContentInit = function () {
        this.isViewEnited = true;
        this.BuildScreen(true);
    };
    GeneratedComponent.prototype.BuildScreen = function (fireEmit) {
        var _this = this;
        if (fireEmit === void 0) { fireEmit = false; }
        if (this.EntityPM != null) {
            if (this.isViewEnited == true) {
                var myScreenColumns = [];
                var myScreen = window.Screens.filter(function (x) { return x.ObjectTableId === _this.ObjectTableId && x.Code.toLowerCase() == _this.ScreenCode.toLowerCase(); })[0];
                if (myScreen != null) {
                    var myScreenFields = window.ScreenFields.filter(function (x) { return x.ScreenId === myScreen.Id && x.Tenant === SessionInfo_1.SessionInfo.LoggedUserTenant; });
                    if (myScreenFields.length == 0) {
                        myScreenFields = window.ScreenFields.filter(function (x) { return x.ScreenId === myScreen.Id; });
                    }
                    if (myScreenFields.length == 0) {
                        this.ShowNoFieldsText = true;
                    }
                    else {
                        var myObjectFields = window.ObjectFields.filter(function (x) { return x.ObjectTableId === _this.ObjectTableId; });
                        for (var c = 0; c < myScreen.NumberOfColumns; c++) {
                            var myScreenColumn = new ScreenColumn(c);
                            for (var r = 0; r < myScreen.NumberOfRows; r++) {
                                var myScreenField = myScreenFields.filter(function (f) { return f.Column == c && f.Row == r; })[0];
                                if (myScreenField != null) {
                                    var myObjectField = myObjectFields.filter(function (f) { return f.Id == myScreenField.ObjectFieldId; })[0];
                                    if (myObjectField != null) {
                                        if (this.ObjectTableName == "CommunicationLog") {
                                            this.EntityPM.UIProperties.SetEnabled(myObjectField.FieldName, this.ObjectTableName, false);
                                            this.EntityPM.UIProperties.SetRequired(myObjectField.FieldName, this.ObjectTableName, false);
                                        }
                                        myScreenColumn.ObjectFields.push(myObjectField);
                                    }
                                }
                            }
                            myScreenColumns.push(myScreenColumn);
                        }
                    }
                }
                this.ScreenColumns = myScreenColumns;
                if (fireEmit) {
                    this.LoadCompleted.emit(true);
                }
            }
        }
    };
    GeneratedComponent.prototype.SetEnabled = function (isEnabled) {
        var _this = this;
        this.isScreenEnabled = isEnabled;
        if (this.EntityPM) {
            if (this.ScreenColumns) {
                this.ScreenColumns.forEach(function (item) {
                    item.ObjectFields.forEach(function (field) {
                        _this.EntityPM.UIProperties.SetEnabled(field.FieldName, _this.ObjectTableName, isEnabled);
                    });
                });
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], GeneratedComponent.prototype, "LoadCompleted", void 0);
    GeneratedComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GeneratedComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], GeneratedComponent);
    return GeneratedComponent;
}(BaseComponent_1.BaseComponent));
exports.GeneratedComponent = GeneratedComponent;
var ScreenColumn = /** @class */ (function () {
    function ScreenColumn(index) {
        this.Index = index;
        this.ObjectFields = [];
    }
    return ScreenColumn;
}());
exports.ScreenColumn = ScreenColumn;
//# sourceMappingURL=GeneratedComponent.js.map