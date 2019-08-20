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
var Tools_1 = require("../../../Infrastructure/Tools");
var ComputingPartnerPM_1 = require("../../../Common/EntityPMs/ComputingPartnerPM");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var ComputingPartnerTablePM_1 = require("../../../Common/EntityPMs/ComputingPartnerTablePM");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ComputingPartnerPMService_1 = require("../../../Common/Services/StandardPMs/ComputingPartnerPMService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var NewComputingPartnerConmponent = /** @class */ (function (_super) {
    __extends(NewComputingPartnerConmponent, _super);
    function NewComputingPartnerConmponent() {
        var _this = _super.call(this) || this;
        _this.Session = SessionLocator_1.SessionLocator.Tenant;
        _this.DataContext = _this;
        _this.ObjectTableName = "ComputingPartner";
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = new ComputingPartnerPM_1.ComputingPartnerPM();
        _this.EntityPM.LoggedTenantId = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.ItemSourceCollection = new ObservableCollection_1.ObservableCollection([]);
        _this.BuildObsList();
        return _this;
    }
    NewComputingPartnerConmponent.prototype.BuildObsList = function () {
        this.ItemSourceCollection.Clear();
        this.ItemSourceCollection.InsertCollection(this.EntityPM.PartnerTables);
    };
    NewComputingPartnerConmponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var myService = new ComputingPartnerPMService_1.ComputingPartnerPMService();
            myService.insert(this.EntityPM).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit(_this.EntityPM.Id);
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    };
    NewComputingPartnerConmponent.prototype.AddTable = function () {
        var _this = this;
        var newEntityPM = new ComputingPartnerTablePM_1.ComputingPartnerTablePM(this.EntityPM);
        newEntityPM.ComputingPartnerId = this.EntityPM.Id;
        newEntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newEntityPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        newEntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        var args = new Args();
        args.entity = newEntityPM;
        args.FatherEntity = this.EntityPM;
        args.IsNewEntity = true;
        var entityResource = new EntityResourceService_1.EntityResourceService();
        entityResource.getEntityResourceByTableName("ComputingPartnerTable", 0).subscribe(function (p) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Title = "New Table";
            logitudeWindow.WindowArgs = args;
            logitudeWindow.Show('./InfrastructureModules/InfrastructureComputingPartner/Components/AddEditComputingPartnerComponent');
            logitudeWindow.WindowClosed.subscribe(function (p) {
                _this.BuildObsList();
            });
        });
    };
    NewComputingPartnerConmponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewComputingPartnerConmponent.prototype.EditComputingTable = function (item) {
        var args = new Args();
        args.entity = item;
        args.FatherEntity = this.EntityPM;
        args.IsNewEntity = false;
        var entityResource = new EntityResourceService_1.EntityResourceService();
        entityResource.getEntityResourceByTableName("ComputingPartnerTable", 0).subscribe(function (p) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Title = "Edit Table";
            logitudeWindow.WindowArgs = args;
            logitudeWindow.Show('./InfrastructureModules/InfrastructureComputingPartner/Components/AddEditComputingPartnerComponent');
        });
    };
    Object.defineProperty(NewComputingPartnerConmponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value)
                this.EntityPM.Name = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewComputingPartnerConmponent.prototype, "Remarks", {
        get: function () { return this.EntityPM.Remarks; },
        set: function (value) {
            if (this.EntityPM.Remarks != value)
                this.EntityPM.Remarks = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewComputingPartnerConmponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        set: function (value) {
            if (this.EntityPM.Code != value)
                this.EntityPM.Code = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewComputingPartnerConmponent.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (value) {
            if (this.EntityPM.Description != value)
                this.EntityPM.Description = value;
        },
        enumerable: true,
        configurable: true
    });
    NewComputingPartnerConmponent = __decorate([
        core_1.Component({
            selector: 'NewComputingPartnerConmponent',
            moduleId: module.id,
            templateUrl: './NewComputingPartnerConmponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewComputingPartnerConmponent);
    return NewComputingPartnerConmponent;
}(BaseComponent_1.BaseComponent));
exports.NewComputingPartnerConmponent = NewComputingPartnerConmponent;
var Args = /** @class */ (function () {
    function Args() {
    }
    return Args;
}());
//# sourceMappingURL=NewComputingPartnerConmponent.js.map