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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var TicketClassificationListService_1 = require("../../../../../CRM/Services/StandardLists/TicketClassificationListService");
var TicketStageListService_1 = require("../../../../../CRM/Services/StandardLists/TicketStageListService");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var Cloner_1 = require("../../../../../Infrastructure/Utilities/Cloner");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var TicketClosureComponent = /** @class */ (function (_super) {
    __extends(TicketClosureComponent, _super);
    function TicketClosureComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Ticket";
        _this.DataContext = _this;
        _this.IsOkClosed = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.FirstClassificationId = "";
        return _this;
    }
    TicketClosureComponent.prototype.SetWindowArgs = function (args) {
        if (args) {
            this.EntityPM = args.Ticket;
            this.StageButtonCode = args.StageCode;
        }
        this.SetUIProperties();
        this.getGeneralClassification();
        this.Clone();
    };
    TicketClosureComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(this.MainClassificationId));
    };
    Object.defineProperty(TicketClosureComponent.prototype, "ClosureDescription", {
        // Properties
        get: function () { return this.EntityPM.ClosureDescription; },
        set: function (newValue) {
            if (this.EntityPM.ClosureDescription != newValue) {
                this.EntityPM.ClosureDescription = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketClosureComponent.prototype, "MainClassificationId", {
        get: function () { return this.EntityPM.MainClassificationId; },
        set: function (newValue) {
            if (this.EntityPM.MainClassificationId != newValue) {
                this.EntityPM.MainClassificationId = newValue;
                this.getGeneralClassification();
                this.SecondaryClassificationId = null;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketClosureComponent.prototype, "SecondaryClassificationId", {
        get: function () { return this.EntityPM.SecondaryClassificationId; },
        set: function (newValue) {
            if (this.EntityPM.SecondaryClassificationId != newValue) {
                this.EntityPM.SecondaryClassificationId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    TicketClosureComponent.prototype.getGeneralClassification = function () {
        var _this = this;
        this.FirstClassificationId = "";
        var myService = new TicketClassificationListService_1.TicketClassificationListService();
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        myService.getAllFromCache(filters).subscribe(function (resp) {
            if (!resp.HasError) {
                var result = resp.Result;
                var myClassification = result.filter(function (d) { return d.Name == "General" && d.Tenant == SessionLocator_1.SessionLocator.TenantPM.Id; })[0];
                if (myClassification != null) {
                    var filter = "!F";
                    _this.FirstClassificationId = myClassification.Id.concat(filter);
                }
            }
        });
    };
    Object.defineProperty(TicketClosureComponent.prototype, "SecondClassificationId", {
        get: function () {
            var myGeneralId = "";
            if (this.MainClassificationId != null) {
                var filter = "!S";
                myGeneralId = this.MainClassificationId.concat(filter);
            }
            return myGeneralId;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketClosureComponent.prototype, "TicketTypeId", {
        get: function () { return this.EntityPM.TicketTypeId; },
        set: function (newValue) {
            if (this.EntityPM.TicketTypeId != newValue) {
                this.EntityPM.TicketTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    TicketClosureComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.IsOkClosed = false;
        this.CurrentSession.CloseCurrentWindow();
    };
    TicketClosureComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (this.StageButtonCode == "CS") {
            this.EntityPM.IsClosed = true;
            this.EntityPM.LastCloseDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        }
        else {
            this.EntityPM.IsClosed = false;
        }
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            var myService = new TicketStageListService_1.TicketStageListService();
            myService.getAll().subscribe(function (resp) {
                if (!resp.HasError) {
                    var list = resp.Result;
                    var stage = list.filter(function (d) { return d.Code == _this.StageButtonCode && d.Tenant == SessionLocator_1.SessionLocator.TenantPM.Id; })[0];
                    if (stage != null) {
                        _this.EntityPM.StageId = stage.Id;
                        _this.EntityPM.StageCode = stage.Code;
                        _this.EntityPM.StageName = stage.Name;
                    }
                    _this.IsOkClosed = true;
                    _this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
                else {
                    _this.ValidationErrorsList = resp.ErrorsArray;
                }
            });
        }
    };
    TicketClosureComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('TicketTypeId');
        this.myCloner.AddField('MainClassificationId');
        this.myCloner.AddField('SecondaryClassificationId');
        this.myCloner.AddField('ClosureDescription');
        this.myCloner.AddEntity(this.EntityPM);
    };
    TicketClosureComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    TicketClosureComponent = __decorate([
        core_1.Component({
            selector: 'TicketClosureComponent',
            moduleId: module.id,
            templateUrl: './TicketClosureComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], TicketClosureComponent);
    return TicketClosureComponent;
}(BaseComponent_1.BaseComponent));
exports.TicketClosureComponent = TicketClosureComponent;
//# sourceMappingURL=TicketClosureComponent.js.map