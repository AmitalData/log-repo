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
var AirlineAreaPM_1 = require("../../../../Common/EntityPMs/AirlineAreaPM");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var AirlineAreasPortPM_1 = require("../../../../Common/EntityPMs/AirlineAreasPortPM");
var PortListService_1 = require("../../../../Common/Services/StandardLists/PortListService");
var AddEditAirlineAreaComponent = /** @class */ (function (_super) {
    __extends(AddEditAirlineAreaComponent, _super);
    function AddEditAirlineAreaComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "AirlineArea";
        _this.DataContext = _this;
        _this.ItemList = [];
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.portListService = new PortListService_1.PortListService();
        _this.IsResourcesReady = false;
        return _this;
    }
    Object.defineProperty(AddEditAirlineAreaComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) { this.EntityPM.Name = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAirlineAreaComponent.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (value) { this.EntityPM.Description = value; },
        enumerable: true,
        configurable: true
    });
    AddEditAirlineAreaComponent.prototype.SetWindowArgs = function (windowArgs) {
        var _this = this;
        this.AirlinePM = windowArgs['EntityPM'];
        this.IsNew = windowArgs['IsNew'];
        if (this.IsNew) {
            this.EntityPM = new AirlineAreaPM_1.AirlineAreaPM(this.AirlinePM);
        }
        else {
            this.EntityPM = windowArgs['Entity'];
            this.EntityPM.AirlineAreasPorts.forEach(function (item) {
                _this.portListService.getSingleFromCache(item.PortId).subscribe(function (p) {
                    if (!p.HasError) {
                        if (p.Result) {
                            _this.ItemList.push(new DestinationClass(_this, p.Result, false));
                        }
                    }
                    else {
                        _this.ValidationErrorsList = p.ErrorsArray;
                    }
                });
            });
        }
        this.IsResourcesReady = true;
    };
    AddEditAirlineAreaComponent.prototype.ChoosePort = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 320;
        logWindow.Height = 170;
        var itemComponent = new DestinationClass(this, null, true);
        logWindow.DataContext = itemComponent;
        logWindow.Title = "Choose Ports";
        logWindow.Show('./CommonModules/CommonAirline/Components/AddEdit/ChoosePortComponent');
    };
    AddEditAirlineAreaComponent.prototype.DeletePort = function (Item) {
        var index = this.ItemList.indexOf(Item);
        if (index > -1) {
            this.ItemList.splice(index, 1);
        }
        this.EntityPM.RemoveAirlineAreasPortPM(Item.EntityPM);
    };
    AddEditAirlineAreaComponent.prototype.SaveButtonClicked = function () {
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Name)) {
            this.ValidationErrorsList.push("Name is required");
        }
        if (this.EntityPM.AirlineAreasPorts.filter(function (p) { return p.ChangeSetOp != "3"; })[0] == null) {
            this.ValidationErrorsList.push("At Least one port is required");
        }
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsNew) {
                this.EntityPM.CreatedByUserName = SessionInfo_1.SessionInfo.LoggedUserPM.EnglishName;
                this.EntityPM.UpdatedByUserName = SessionInfo_1.SessionInfo.LoggedUserPM.EnglishName;
                this.EntityPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                this.EntityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                this.EntityPM.AirlineId = this.AirlinePM.Id;
                this.AirlinePM.AddAirlineAreaPM(this.EntityPM);
            }
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    AddEditAirlineAreaComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditAirlineAreaComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditAirlineAreaComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditAirlineAreaComponent);
    return AddEditAirlineAreaComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditAirlineAreaComponent = AddEditAirlineAreaComponent;
var DestinationClass = /** @class */ (function (_super) {
    __extends(DestinationClass, _super);
    function DestinationClass(fatherComponent, Port, IsNew) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        if (IsNew && Port != null) {
            _this.Indication = "Port";
            _this.Name = Port.EnglishName;
            _this.Code = Port.Code;
            _this.Id = Port.Id;
            _this.EntityPM = new AirlineAreasPortPM_1.AirlineAreasPortPM(fatherComponent.EntityPM);
            _this.EntityPM.Name = _this.Name;
            _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            _this.EntityPM.AirlineAreaId = fatherComponent.EntityPM.Id;
            _this.EntityPM.PortId = _this.Id;
            fatherComponent.EntityPM.AddAirlineAreasPortPM(_this.EntityPM);
        }
        else {
            if (Port != null) {
                _this.Indication = "Port";
                _this.Name = Port.EnglishName;
                _this.Code = Port.Code;
                _this.Id = Port.Id;
                _this.EntityPM = fatherComponent.EntityPM.AirlineAreasPorts.filter(function (p) { return p.PortId == Port.Id; })[0];
            }
        }
        return _this;
    }
    return DestinationClass;
}(BaseComponent_1.BaseComponent));
exports.DestinationClass = DestinationClass;
//# sourceMappingURL=AddEditAirlineAreaComponent.js.map