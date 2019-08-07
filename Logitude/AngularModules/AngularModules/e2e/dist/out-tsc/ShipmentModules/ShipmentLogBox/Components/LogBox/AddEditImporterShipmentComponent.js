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
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ShipmentPM_1 = require("../../../../Shipment/EntityPMs/ShipmentPM");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityStatusListService_1 = require("../../../../Infrastructure/Services/StandardLists/EntityStatusListService");
var BranchListService_1 = require("../../../../Common/Services/StandardLists/BranchListService");
var DepartmentListService_1 = require("../../../../Common/Services/StandardLists/DepartmentListService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var PortExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/PortExtendedPMService");
var AddEditImporterShipmentComponent = /** @class */ (function (_super) {
    __extends(AddEditImporterShipmentComponent, _super);
    function AddEditImporterShipmentComponent(_entityListService) {
        var _this = _super.call(this) || this;
        _this._entityListService = _entityListService;
        _this.EntityPM = new ShipmentPM_1.ShipmentPM();
        _this.DataContext = _this;
        _this.IsNew = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isSaveClicked = false;
        _this.ValidationErrorsList = [];
        _this.myShipmentDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        _this._EntityStatusListService = new EntityStatusListService_1.EntityStatusListService();
        _this._DepartmentListService = new DepartmentListService_1.DepartmentListService();
        _this._BranchListService = new BranchListService_1.BranchListService();
        _this._ShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
        return _this;
    }
    AddEditImporterShipmentComponent.prototype.ngOnInit = function () {
    };
    AddEditImporterShipmentComponent.prototype.ngAfterViewInit = function () {
    };
    AddEditImporterShipmentComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        //this.ShipmentList = args.SelectedShipment;
        this.IsNew = args.IsNew;
        if (this.IsNew) {
            this._EntityStatusListService.getAll().subscribe(function (myResult) {
                if (!myResult.HasError) {
                    _this.StatusId = myResult.Result.filter(function (a) { return a.Code == "OPOP"; })[0].Id;
                }
                else {
                    _this.ValidationErrorsList = myResult.ErrorsArray;
                }
            });
            this._DepartmentListService.getAll().subscribe(function (myResult) {
                if (!myResult.HasError) {
                    _this.DepartmentId = myResult.Result.filter(function (a) { return a.Tenant == SessionLocator_1.SessionLocator.Tenant; })[0].Id;
                }
                else {
                    _this.ValidationErrorsList = myResult.ErrorsArray;
                }
            });
            this._BranchListService.getAll().subscribe(function (myResult) {
                if (!myResult.HasError) {
                    _this.BranchId = myResult.Result.filter(function (a) { return a.Tenant == SessionLocator_1.SessionLocator.Tenant; })[0].Id;
                }
                else {
                    _this.ValidationErrorsList = myResult.ErrorsArray;
                }
            });
        }
        if (args.EntityPM) {
            this.EntityPM = args.EntityPM;
        }
    };
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "ShipperName", {
        get: function () { return this.EntityPM.ShipperName; },
        set: function (newValue) { this.EntityPM.ShipperName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "CustomerReference1", {
        get: function () { return this.EntityPM.CustomerReference1; },
        set: function (newValue) { this.EntityPM.CustomerReference1 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "CustomerReference2", {
        get: function () { return this.EntityPM.CustomerReference2; },
        set: function (newValue) { this.EntityPM.CustomerReference2 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "CustomerId", {
        get: function () { return this.EntityPM.CustomerId; },
        set: function (newValue) { this.EntityPM.CustomerId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "StatusDate", {
        get: function () { return this.EntityPM.StatusDate; },
        set: function (newValue) { this.EntityPM.StatusDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "StatusId", {
        get: function () { return this.EntityPM.StatusId; },
        set: function (newValue) { this.EntityPM.StatusId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "ForwarderPartnerId", {
        get: function () { return this.EntityPM.ForwarderPartnerId; },
        set: function (newValue) { this.EntityPM.ForwarderPartnerId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "DepartmentId", {
        get: function () { return this.EntityPM.DepartmentId; },
        set: function (newValue) { this.EntityPM.DepartmentId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "BranchId", {
        get: function () { return this.EntityPM.BranchId; },
        set: function (newValue) { this.EntityPM.BranchId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "ShipmentLevelCode", {
        get: function () { return this.EntityPM.ShipmentLevelCode; },
        set: function (newValue) { this.EntityPM.ShipmentLevelCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "DirectionId", {
        get: function () { return this.EntityPM.DirectionId; },
        set: function (newValue) { this.EntityPM.DirectionId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "TransportModeId", {
        get: function () { return this.EntityPM.TransportModeId; },
        set: function (newValue) { this.EntityPM.TransportModeId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "ToPortId", {
        get: function () { return this.EntityPM.ToPortId; },
        set: function (newValue) { this.EntityPM.ToPortId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "OtherPrepaidCollectId", {
        get: function () { return this.EntityPM.OtherPrepaidCollectId; },
        set: function (newValue) { this.EntityPM.OtherPrepaidCollectId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "FreightPrepaidCollectId", {
        get: function () { return this.EntityPM.FreightPrepaidCollectId; },
        set: function (newValue) { this.EntityPM.FreightPrepaidCollectId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterShipmentComponent.prototype, "FromPortId", {
        get: function () { return this.EntityPM.FromPortId; },
        set: function (newValue) { this.EntityPM.FromPortId = newValue; },
        enumerable: true,
        configurable: true
    });
    AddEditImporterShipmentComponent.prototype.SaveChanges = function () {
        var _this = this;
        if (this.isSaveClicked == true) {
            return;
        }
        this.isSaveClicked = true;
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.TransportModeId)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "Transportation Type"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomerReference1)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "Order Number"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.ForwarderPartnerId)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "Agent"));
        }
        this._PortExtendedPMService = new PortExtendedPMService_1.PortExtendedPMService();
        if (Tools_1.AppTool.IsNullOrEmpty(this.FromPortId)) {
            //this.ValidationErrorsList.push(msg.replace("%FieldName", "Gatway"));
            //this._PortExtendedPMService.getSinglePort("---", "IL", SessionLocator.Tenant).subscribe(Result => {
            //    this.FromPortId = Result.Result.Id;
            //});
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.ToPortId)) {
            //this.ValidationErrorsList.push(msg.replace("%FieldName", "Destination"));
        }
        //FillErrors(errors);
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            this.EntityPM.IsImporterShipment = true;
            this.EntityPM.MainCarriageFromPortId = this.EntityPM.FromPortId;
            this.EntityPM.MainCarriageToPortId = this.EntityPM.ToPortId;
            this.EntityPM.GrossWeightUnitCode = "KG";
            this.EntityPM.DimensionsUnitCode = "Cm";
            this.EntityPM.ChargeableWeightUnitCode = "KG";
            this.EntityPM.VolumeUnitCode = "CBF";
            if (this.IsNew) {
                this.EntityPM.FreightPrepaidCollectId = "C";
                this.EntityPM.ShipmentCustomerTypeCode = "SHI";
                this.EntityPM.OtherPrepaidCollectId = "C";
                this.EntityPM.DirectionId = "C";
                this.EntityPM.ShipmentLevelCode = "A";
                this.EntityPM.OrderIsDangerouseGoods = false;
                this.EntityPM.StatusDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                this.EntityPM.CustomerId = SessionLocator_1.SessionLocator.TenantPM.CustomerId;
                this.EntityPM.CustomerName = SessionLocator_1.SessionLocator.TenantPM.CustomerId;
                this.EntityPM.ConsigneeId = SessionLocator_1.SessionLocator.TenantPM.CustomerId;
                this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                this.EntityPM.NewConcurrencyGUID = Guid_1.Guid.newGuid();
                this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                this._ShipmentPMService.insert(this.EntityPM).subscribe(function (myResult) {
                    if (!myResult.HasError) {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("LogBox", "New Shipment");
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindowEmit("MyShipmentAdded");
                        //ParentViewModel.setImporterFilter();
                        //ParentViewModel.LoadAllData();
                    }
                    else {
                        _this.ValidationErrorsList = myResult.ErrorsArray;
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    }
                    _this.isSaveClicked = false;
                });
            }
            else {
                this._ShipmentPMService.update(this.EntityPM).subscribe(function (myResult) {
                    if (!myResult.HasError) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindowEmit("MyShipmentAdded");
                    }
                    else {
                        _this.ValidationErrorsList = myResult.ErrorsArray;
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    }
                    _this.isSaveClicked = false;
                });
            }
            //ShipmentContext.SubmitChanges().Completed += AddEditImporterShipmentViewModel_Completed;
        }
        else {
            this.isSaveClicked = false;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    };
    AddEditImporterShipmentComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditImporterShipmentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditImporterShipmentComponent.html',
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService])
    ], AddEditImporterShipmentComponent);
    return AddEditImporterShipmentComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditImporterShipmentComponent = AddEditImporterShipmentComponent;
//# sourceMappingURL=AddEditImporterShipmentComponent.js.map