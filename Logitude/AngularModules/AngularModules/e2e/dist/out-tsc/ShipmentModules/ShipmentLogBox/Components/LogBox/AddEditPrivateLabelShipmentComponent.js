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
var ShipmentPackagePM_1 = require("../../../../Shipment/EntityPMs/ShipmentPackagePM");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityStatusListService_1 = require("../../../../Infrastructure/Services/StandardLists/EntityStatusListService");
var BranchListService_1 = require("../../../../Common/Services/StandardLists/BranchListService");
var PackageTypeListService_1 = require("../../../../Common/Services/StandardLists/PackageTypeListService");
var DepartmentListService_1 = require("../../../../Common/Services/StandardLists/DepartmentListService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var PortExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/PortExtendedPMService");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var AddEditPrivateLabelShipmentComponent = /** @class */ (function (_super) {
    __extends(AddEditPrivateLabelShipmentComponent, _super);
    function AddEditPrivateLabelShipmentComponent(_entityListService) {
        var _this = _super.call(this) || this;
        _this._entityListService = _entityListService;
        _this.EntityPM = new ShipmentPM_1.ShipmentPM();
        _this.DataContext = _this;
        _this.IsNew = true;
        _this.PLShortName = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.UnAssignedPackageTypeId = '';
        _this.pLForwarding = false;
        _this.isSaveClicked = false;
        if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
            _this.PLShortName = SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName;
        }
        if (_this.CurrentSession == null) {
            _this.FilterId_A = "TransportFilter_A_-1_-1";
            _this.FilterId_O = "TransportFilter_O_-1_-1";
            _this.FilterId_I = "TransportFilter_I_-1_-1";
        }
        else {
            var idIndex = _this.CurrentSession.GetNewId("TransportsFilter");
            _this.FilterId_A = "TransportFilter_A_" + idIndex;
            _this.FilterId_O = "TransportFilter_O_" + idIndex;
            _this.FilterId_I = "TransportFilter_I_" + idIndex;
        }
        _this.ValidationErrorsList = [];
        _this.myShipmentDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        _this._EntityStatusListService = new EntityStatusListService_1.EntityStatusListService();
        _this._DepartmentListService = new DepartmentListService_1.DepartmentListService();
        _this._BranchListService = new BranchListService_1.BranchListService();
        _this._ShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
        _this._PortExtendedPMService = new PortExtendedPMService_1.PortExtendedPMService();
        _this._PackageTypeListService = new PackageTypeListService_1.PackageTypeListService();
        return _this;
    }
    AddEditPrivateLabelShipmentComponent.prototype.transportItemClicked = function (itemValue) {
        if (this.TransportModeId != itemValue) {
            this.TransportModeId = itemValue;
            if (this.TransportModeId == "O") {
                this.TransportationTypes = [new TransportationTypes("Ashdod", "O", "ASH", "IL"), new TransportationTypes("Haifa", "O", "HFA", "IL"), new TransportationTypes("Eilat", "O", "ETH", "IL")];
                this.onTransportationTypeChange(new TransportationTypes("Haifa", "O", "HFA", "IL"));
            }
            else if (this.TransportModeId == "I") {
                this.TransportationTypes = [new TransportationTypes("Nitzana", "I", "NZN", "IL"), new TransportationTypes("Arava", "I", "ARV", "IL"), new TransportationTypes("Alenbi", "I", "ALN", "IL"), new TransportationTypes("Jordan", "I", "JOR", "IL")];
                this.onTransportationTypeChange(new TransportationTypes("Nitzana", "I", "NZN", "IL"));
            }
            else if (this.TransportModeId == "A") {
                this.TransportationTypes = [new TransportationTypes("Tel-Aviv", "A", "TLV", "IL")];
                this.onTransportationTypeChange(new TransportationTypes("Tel-Aviv", "A", "TLV", "IL"));
            }
        }
    };
    AddEditPrivateLabelShipmentComponent.prototype.transportItemMouseOver = function (itemValue) {
        if (this.TransportModeId != itemValue) {
            var img_A = document.getElementById(this.FilterId_A);
            var img_O = document.getElementById(this.FilterId_O);
            var img_I = document.getElementById(this.FilterId_I);
            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A.png");
                    break;
                }
                case "O": {
                    img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O.png");
                    break;
                }
                case "I": {
                    img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I.png");
                    //img_I.style.top = "1px";
                    break;
                }
            }
        }
    };
    AddEditPrivateLabelShipmentComponent.prototype.transportItemMouseLeave = function (itemValue) {
        if (this.TransportModeId != itemValue) {
            var img_A = document.getElementById(this.FilterId_A);
            var img_O = document.getElementById(this.FilterId_O);
            var img_I = document.getElementById(this.FilterId_I);
            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A_g.png");
                    break;
                }
                case "O": {
                    img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O_g.png");
                    break;
                }
                case "I": {
                    img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I_g.png");
                    break;
                }
            }
        }
    };
    AddEditPrivateLabelShipmentComponent.prototype.ApplySelectedStyle = function () {
        var img_A = document.getElementById(this.FilterId_A);
        var img_O = document.getElementById(this.FilterId_O);
        var img_I = document.getElementById(this.FilterId_I);
        if (img_A) {
            img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A_g.png");
        }
        if (img_O) {
            img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O_g.png");
        }
        if (img_I) {
            img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I_G.png");
        }
        if (img_A) {
            switch (this.TransportModeId) {
                case "A": {
                    img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A_w.png");
                    break;
                }
                case "O": {
                    img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O_w.png");
                    break;
                }
                case "I": {
                    img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I_w.png");
                    break;
                }
            }
        }
    };
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "SelectedTransportationTypes", {
        get: function () {
            return this.selectedTransportationTypes;
        },
        set: function (newValue) {
            if (newValue) {
                this.selectedTransportationTypes = newValue;
                this.TransportModeId = newValue.TransporationType;
                this.ToPortId = newValue.ToPortCode;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditPrivateLabelShipmentComponent.prototype.onTransportationTypeChange = function ($event) {
        this.SelectedTransportationTypes = $event;
    };
    AddEditPrivateLabelShipmentComponent.prototype.ngOnInit = function () {
    };
    AddEditPrivateLabelShipmentComponent.prototype.ngAfterViewInit = function () {
    };
    AddEditPrivateLabelShipmentComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        //this.ShipmentList = args.SelectedShipment;
        this.IsNew = args.IsNew;
        this._PackageTypeListService.getAll().subscribe(function (myResult) {
            if (!myResult.HasError) {
                _this.UnAssignedPackageTypeId = myResult.Result.filter(function (a) { return a.Tenant == SessionLocator_1.SessionLocator.Tenant && a.Code == '---'; })[0].Id;
            }
            else {
                _this.ValidationErrorsList = myResult.ErrorsArray;
            }
        });
        if (this.IsNew) {
            this.TransportModeId = "O";
            this.TransportationTypes = [new TransportationTypes("Ashdod", "O", "ASH", "IL"), new TransportationTypes("Haifa", "O", "HFA", "IL"), new TransportationTypes("Eilat", "O", "ETH", "IL")];
            this.SelectedTransportationTypes = this.TransportationTypes[1];
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
            //if (!this.IsNew) {
            if (this.EntityPM.TransportModeId == "O") {
                this.TransportationTypes = [new TransportationTypes("Ashdod", "O", "ASH", "IL"), new TransportationTypes("Haifa", "O", "HFA", "IL"), new TransportationTypes("Eilat", "O", "ETH", "IL")];
                //this.SelectedTransportationTypes = this.TransportationTypes[1];
            }
            else if (this.EntityPM.TransportModeId == "I") {
                this.TransportationTypes = [new TransportationTypes("Nitzana", "I", "NZN", "IL"), new TransportationTypes("Arava", "I", "ARV", "IL"), new TransportationTypes("Alenbi", "I", "ALN", "IL"), new TransportationTypes("Jordan", "I", "JOR", "IL")];
            }
            else if (this.EntityPM.TransportModeId == "A") {
                this.TransportationTypes = [new TransportationTypes("Tel-Aviv", "A", "TLV", "IL")];
                this.onTransportationTypeChange(new TransportationTypes("Tel-Aviv", "A", "TLV", "IL"));
            }
            // Keep it true until Sprint D
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipmentAddtionalDataXML)) {
                this.PLForwarding = false;
            }
            else {
                this.PLForwarding = true;
            }
            ///////////////////////////////
            //var isLcl = this.IsLCLEntity(this.EntityPM);
            //if (isLcl == true) {
            //    this.PackageType = "LCL";
            //}
            //else if (this.EntityPM){
            //    this.PackageType = "FCL";
            //}
            //}
            this.SelectedTransportationTypes = new TransportationTypes("Ashdod", "O", "ASH", "IL");
            if (this.EntityPM.MainCarriageToPortCode == "ASH") {
                this.SelectedTransportationTypes = new TransportationTypes("Ashdod", "O", "ASH", "IL");
            }
            else if (this.EntityPM.MainCarriageToPortCode == "HFA") {
                this.SelectedTransportationTypes = new TransportationTypes("Haifa", "O", "HFA", "IL");
            }
            else if (this.EntityPM.MainCarriageToPortCode == "ETH") {
                this.SelectedTransportationTypes = new TransportationTypes("Eilat", "O", "ETH", "IL");
            }
            else if (this.EntityPM.MainCarriageToPortCode == "NZN") {
                this.SelectedTransportationTypes = new TransportationTypes("Nitzana", "I", "NZN", "IL");
            }
            else if (this.EntityPM.MainCarriageToPortCode == "ARV") {
                this.SelectedTransportationTypes = new TransportationTypes("Arava", "I", "ARV", "IL");
            }
            else if (this.EntityPM.MainCarriageToPortCode == "ALN") {
                this.SelectedTransportationTypes = new TransportationTypes("Alenbi", "I", "ALN", "IL");
            }
            else if (this.EntityPM.MainCarriageToPortCode == "JOR") {
                this.SelectedTransportationTypes = new TransportationTypes("Jordan", "I", "JOR", "IL");
            }
            else {
                this.SelectedTransportationTypes = new TransportationTypes("Tel-Aviv", "A", "TLV", "IL");
            }
            if (this.EntityPM.ShipmentAddtionalDataXML == "<PLForwarding>true</PLForwarding>") {
                this.PLForwarding = true;
            }
            else {
                this.PLForwarding = false;
            }
        }
    };
    AddEditPrivateLabelShipmentComponent.prototype.PLForwardingClicked = function (PLF) {
        if (PLF == "Yes") {
            this.PLForwarding = true;
        }
        else {
            this.PLForwarding = false;
        }
    };
    AddEditPrivateLabelShipmentComponent.prototype.PackageTypeClicked = function (Type) {
        if (Type == "OTHER") {
            this.PackageType = null;
        }
        else {
            this.PackageType = Type;
        }
    };
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "PackageType", {
        // private packageType: string;
        get: function () { return this.EntityPM.ShipmentTypeId; },
        set: function (newValue) { this.EntityPM.ShipmentTypeId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "PLForwarding", {
        get: function () {
            return this.pLForwarding;
        },
        set: function (newValue) { this.pLForwarding = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "ShipperName", {
        get: function () { return this.EntityPM.ShipperName; },
        set: function (newValue) { this.EntityPM.ShipperName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "CustomerReference1", {
        get: function () { return this.EntityPM.CustomerReference1; },
        set: function (newValue) { this.EntityPM.CustomerReference1 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "CustomerReference2", {
        get: function () { return this.EntityPM.CustomerReference2; },
        set: function (newValue) { this.EntityPM.CustomerReference2 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "CustomerId", {
        get: function () { return this.EntityPM.CustomerId; },
        set: function (newValue) { this.EntityPM.CustomerId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "StatusDate", {
        get: function () { return this.EntityPM.StatusDate; },
        set: function (newValue) { this.EntityPM.StatusDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "StatusId", {
        get: function () { return this.EntityPM.StatusId; },
        set: function (newValue) { this.EntityPM.StatusId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "ForwarderPartnerId", {
        get: function () { return this.EntityPM.ForwarderPartnerId; },
        set: function (newValue) { this.EntityPM.ForwarderPartnerId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "DepartmentId", {
        get: function () { return this.EntityPM.DepartmentId; },
        set: function (newValue) { this.EntityPM.DepartmentId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "BranchId", {
        get: function () { return this.EntityPM.BranchId; },
        set: function (newValue) { this.EntityPM.BranchId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "ShipmentLevelCode", {
        get: function () { return this.EntityPM.ShipmentLevelCode; },
        set: function (newValue) { this.EntityPM.ShipmentLevelCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "DirectionId", {
        get: function () { return this.EntityPM.DirectionId; },
        set: function (newValue) { this.EntityPM.DirectionId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "TransportModeId", {
        get: function () { return this.EntityPM.TransportModeId; },
        set: function (newValue) { this.EntityPM.TransportModeId = newValue; this.ApplySelectedStyle(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "ToPortId", {
        get: function () { return this.EntityPM.ToPortId; },
        set: function (newValue) { this.EntityPM.ToPortId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "OtherPrepaidCollectId", {
        get: function () { return this.EntityPM.OtherPrepaidCollectId; },
        set: function (newValue) { this.EntityPM.OtherPrepaidCollectId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "FreightPrepaidCollectId", {
        get: function () { return this.EntityPM.FreightPrepaidCollectId; },
        set: function (newValue) { this.EntityPM.FreightPrepaidCollectId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "FromPortId", {
        get: function () { return this.EntityPM.FromPortId; },
        set: function (newValue) { this.EntityPM.FromPortId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "PackagesQuantity", {
        get: function () { return this.EntityPM.PackagesQuantity; },
        set: function (newValue) { this.EntityPM.PackagesQuantity = +(newValue); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "GrossWeight", {
        get: function () { return this.EntityPM.GrossWeight; },
        set: function (newValue) { this.EntityPM.GrossWeight = +(newValue); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "Master", {
        get: function () { return this.EntityPM.Master; },
        set: function (newValue) { this.EntityPM.Master = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "House", {
        get: function () { return this.EntityPM.House; },
        set: function (newValue) { this.EntityPM.House = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPrivateLabelShipmentComponent.prototype, "ContainerNumber", {
        get: function () {
            if (this.EntityPM.ShipmentPackages && this.EntityPM.ShipmentPackages.length > 0) {
                this.containerNumber = this.EntityPM.ShipmentPackages[0].ContainerNumber;
            }
            return this.containerNumber;
        },
        set: function (newValue) {
            if (this.EntityPM.ShipmentPackages && this.EntityPM.ShipmentPackages.length == 0) {
                this.EntityPM.ShipmentPackages = [];
                var MyPackage = new ShipmentPackagePM_1.ShipmentPackagePM(this.EntityPM);
                MyPackage.ContainerNumber = newValue;
                MyPackage.Weight = this.GrossWeight;
                MyPackage.PackageTypeId = this.UnAssignedPackageTypeId;
                MyPackage.Quantity = this.PackagesQuantity;
                this.EntityPM.ShipmentPackages.push(MyPackage);
            }
            else if (this.EntityPM.ShipmentPackages.length > 0) {
                this.EntityPM.ShipmentPackages[0].ContainerNumber = newValue;
                this.EntityPM.ShipmentPackages[0].Weight = this.GrossWeight;
                this.EntityPM.ShipmentPackages[0].PackageTypeId = this.UnAssignedPackageTypeId;
                this.EntityPM.ShipmentPackages[0].Quantity = this.PackagesQuantity;
            }
            //this.ValidateContainerNumber(newValue);
        },
        enumerable: true,
        configurable: true
    });
    AddEditPrivateLabelShipmentComponent.prototype.ValidateContainerNumber = function (input) {
        this.ValidationErrorsList = [];
        this.WarningErrorsList = [];
        var error = Tools_1.FormatTool.ValidateContainerNumber(input);
        if (!Tools_1.AppTool.IsNullOrEmpty(error)) {
            this.WarningErrorsList.push(error);
        }
    };
    AddEditPrivateLabelShipmentComponent.prototype.SaveChanges = function () {
        var _this = this;
        if (this.isSaveClicked == true) {
            return;
        }
        this.isSaveClicked = true;
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (!this.SelectedTransportationTypes) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "TransportationTypes"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomerReference1)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "OrderNumber"));
        }
        if (this.CustomerReference2 && this.CustomerReference2.length > 30) {
            this.ValidationErrorsList.push("My Reference can't be more than 30 characters");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.PackagesQuantity) && this.isInt(this.PackagesQuantity) == false) {
            this.ValidationErrorsList.push("Quantity Must be integer.");
        }
        if ((typeof this.PackagesQuantity != 'number' || this.PackagesQuantity.toString() == "NaN") && this.PackagesQuantity != null) {
            this.ValidationErrorsList.push("Quantity must be numaric value");
        }
        if ((typeof this.GrossWeight != 'number' || this.GrossWeight.toString() == "NaN") && this.GrossWeight != null) {
            this.ValidationErrorsList.push("Weight must be numaric value");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ContainerNumber) && (Tools_1.AppTool.IsNullOrEmpty(this.PackagesQuantity) || Tools_1.AppTool.IsNullOrEmpty(this.GrossWeight))) {
            this.ValidationErrorsList.push("Weight and Quantity are required");
        }
        else {
            if (this.EntityPM.ShipmentPackages.length > 0) {
                //this.EntityPM.ShipmentPackages[0].ContainerNumber = newValue;
                this.EntityPM.ShipmentPackages[0].Weight = this.GrossWeight;
                this.EntityPM.ShipmentPackages[0].PackageTypeId = this.UnAssignedPackageTypeId;
                this.EntityPM.ShipmentPackages[0].Quantity = this.PackagesQuantity;
            }
        }
        if (this.ValidationErrorsList.length == 0) {
            this._PortExtendedPMService.getSinglePort(this.SelectedTransportationTypes.ToPortCode, this.SelectedTransportationTypes.CountryCode, SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
                if (myResult.Result) {
                    _this.ToPortId = myResult.Result.Id;
                    if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.FromPortId)) {
                        _this._PortExtendedPMService.getSinglePort("---", "IL", SessionLocator_1.SessionLocator.Tenant).subscribe(function (Result) {
                            _this.FromPortId = Result.Result.Id;
                            if (_this.IsNew) {
                                _this._ShipmentPMService.GetByCustomerReference1(_this.CustomerReference1, false).subscribe(function (myResult) {
                                    if (myResult.Result) {
                                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                                        confirmWindow.Title = "Warning !";
                                        confirmWindow.Width = 300;
                                        confirmWindow.Height = 150;
                                        confirmWindow.YesButtonText = "Continue";
                                        confirmWindow.NoButtonText = "Cancel";
                                        confirmWindow.Show("There are already shipments with the same order number");
                                        confirmWindow.WindowClosed.subscribe(function (event) {
                                            if (confirmWindow.Yes) {
                                                _this.SaveData();
                                            }
                                            else {
                                                _this.isSaveClicked = false;
                                                //this.LoadImporterShipments(true);
                                            }
                                        });
                                    }
                                    else {
                                        _this.SaveData();
                                    }
                                });
                            }
                            else {
                                _this._ShipmentPMService.GetByCustomerReference1ForUpdate(_this.CustomerReference1, _this.EntityPM.Id, false).subscribe(function (myResult) {
                                    if (myResult.Result) {
                                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                                        confirmWindow.Title = "Warning !";
                                        confirmWindow.Width = 300;
                                        confirmWindow.Height = 150;
                                        confirmWindow.YesButtonText = "Continue";
                                        confirmWindow.NoButtonText = "Cancel";
                                        confirmWindow.Show("There are already shipments with the same order number");
                                        confirmWindow.WindowClosed.subscribe(function (event) {
                                            if (confirmWindow.Yes) {
                                                _this.SaveData();
                                            }
                                            else {
                                                _this.isSaveClicked = false;
                                                //this.LoadImporterShipments(true);
                                            }
                                        });
                                    }
                                    else {
                                        _this.SaveData();
                                    }
                                });
                            }
                        });
                    }
                    else {
                        if (_this.IsNew == true) {
                            _this._ShipmentPMService.GetByCustomerReference1(_this.CustomerReference1, false).subscribe(function (myResult) {
                                if (myResult.Result) {
                                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                                    confirmWindow.Title = "Warning !";
                                    confirmWindow.Width = 300;
                                    confirmWindow.Height = 150;
                                    confirmWindow.YesButtonText = "Continue";
                                    confirmWindow.NoButtonText = "Cancel";
                                    confirmWindow.Show("There are already shipments with the same order number");
                                    confirmWindow.WindowClosed.subscribe(function (event) {
                                        if (confirmWindow.Yes) {
                                            _this.SaveData();
                                        }
                                        else {
                                            _this.isSaveClicked = false;
                                            //this.LoadImporterShipments(true);
                                        }
                                    });
                                }
                                else {
                                    _this.SaveData();
                                }
                            });
                        }
                        else {
                            _this._ShipmentPMService.GetByCustomerReference1ForUpdate(_this.CustomerReference1, _this.EntityPM.Id, false).subscribe(function (myResult) {
                                if (myResult.Result) {
                                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                                    confirmWindow.Title = "Warning !";
                                    confirmWindow.Width = 300;
                                    confirmWindow.Height = 150;
                                    confirmWindow.YesButtonText = "Continue";
                                    confirmWindow.NoButtonText = "Cancel";
                                    confirmWindow.Show("There are already shipments with the same order number");
                                    confirmWindow.WindowClosed.subscribe(function (event) {
                                        if (confirmWindow.Yes) {
                                            _this.SaveData();
                                        }
                                        else {
                                            _this.isSaveClicked = false;
                                            //this.LoadImporterShipments(true);
                                        }
                                    });
                                }
                                else {
                                    _this.SaveData();
                                }
                            });
                        }
                    }
                }
                else {
                    _this.ValidationErrorsList = [];
                    //var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
                    //if (!this.SelectedTransportationTypes) {
                    _this.ValidationErrorsList.push("Transportation Type Port is not defined in your tenant.");
                    _this.isSaveClicked = false;
                    //}
                    //this.SaveData();
                }
            });
        }
        else {
            this.isSaveClicked = false;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    };
    AddEditPrivateLabelShipmentComponent.prototype.isInt = function (n) {
        return n % 1 === 0;
    };
    AddEditPrivateLabelShipmentComponent.prototype.SaveData = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (!this.SelectedTransportationTypes) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "TransportationTypes"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomerReference1)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "OrderNumber"));
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.PackagesQuantity) && this.isInt(this.PackagesQuantity) == false) {
            this.ValidationErrorsList.push("Quantity Must be integer.");
        }
        //if (AppTool.IsNullOrEmpty(this.CustomerReference2)) {
        //    this.ValidationErrorsList.push(msg.replace("%FieldName", "My Reference"));
        //}
        if ((typeof this.PackagesQuantity != 'number' || this.PackagesQuantity.toString() == "NaN") && this.PackagesQuantity != null) {
            this.ValidationErrorsList.push("Quantity must be numaric value");
        }
        if ((typeof this.GrossWeight != 'number' || this.GrossWeight.toString() == "NaN") && this.GrossWeight != null) {
            this.ValidationErrorsList.push("Weight must be numaric value");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ContainerNumber) && (Tools_1.AppTool.IsNullOrEmpty(this.PackagesQuantity) || Tools_1.AppTool.IsNullOrEmpty(this.GrossWeight))) {
            this.ValidationErrorsList.push("Weight and Quantity are required");
        }
        else {
            if (this.EntityPM.ShipmentPackages.length > 0) {
                //this.EntityPM.ShipmentPackages[0].ContainerNumber = newValue;
                this.EntityPM.ShipmentPackages[0].Weight = this.GrossWeight;
                this.EntityPM.ShipmentPackages[0].PackageTypeId = this.UnAssignedPackageTypeId;
                this.EntityPM.ShipmentPackages[0].Quantity = this.PackagesQuantity;
            }
        }
        //if (AppTool.IsNullOrEmpty(this.FromPortId)) {
        //    this.ValidationErrorsList.push(msg.replace("%FieldName", "Gatway"));
        //}
        //if (AppTool.IsNullOrEmpty(this.ToPortId)) {
        //    this.ValidationErrorsList.push(msg.replace("%FieldName", "Destination"));
        //}
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            this.isSaveClicked = false;
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
                //if (this.TransportModeId == "O" && this.PackageType == "LCL") {
                //    this.EntityPM.ShipmentTypeId = "LCLD";
                //}
                //else if (this.TransportModeId == "I" && this.PackageType == "LCL") {
                //    this.EntityPM.ShipmentTypeId = "LTL";
                //}
                //else {
                //    this.EntityPM.ShipmentTypeId = this.PackageType;
                //}
                //var myResult = false;
                //if (this.TransportModeId == "A") {
                //    myResult = true;
                //}
                //else if (this.TransportModeId == "O" && this.EntityPM.ShipmentTypeId == "LCLD") {
                //    myResult = true;
                //}
                //else if (this.TransportModeId == "I" && this.EntityPM.ShipmentTypeId == "LTL") {
                //    myResult = true;
                //}
                //if (myResult == false) {
                this.EntityPM.NumberOfContainers = this.PackagesQuantity;
                //}
                //else {
                this.EntityPM.NumberOfPackages = this.PackagesQuantity;
                //}
                //if (this.PLForwarding == true) {
                this.EntityPM.ForwarderPartnerId = SessionLocator_1.SessionLocator.PrivateLableSettings.HybridPartnerId;
                //}
                if (this.PLForwarding == true) {
                    this.EntityPM.ShipmentAddtionalDataXML = "<PLForwarding>true</PLForwarding>";
                }
                else {
                    this.EntityPM.ShipmentAddtionalDataXML = "<PLForwarding>false</PLForwarding>";
                }
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
                });
            }
            else {
                this.EntityPM.NumberOfContainers = this.PackagesQuantity;
                this.EntityPM.NumberOfPackages = this.PackagesQuantity;
                this.EntityPM.ForwarderPartnerId = SessionLocator_1.SessionLocator.PrivateLableSettings.HybridPartnerId;
                if (this.PLForwarding == true) {
                    this.EntityPM.ShipmentAddtionalDataXML = "<PLForwarding>true</PLForwarding>";
                }
                else {
                    this.EntityPM.ShipmentAddtionalDataXML = "<PLForwarding>false</PLForwarding>";
                }
                this._ShipmentPMService.update(this.EntityPM).subscribe(function (myResult) {
                    if (!myResult.HasError) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindowEmit("MyShipmentAdded");
                    }
                    else {
                        //this.ValidationErrorsList = myResult.ErrorsArray;
                        _this.ValidationErrorsList = myResult.ErrorsArray; //.push("There Are Validation Errors.");
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    }
                });
            }
            //ShipmentContext.SubmitChanges().Completed += AddEditImporterShipmentViewModel_Completed;
        }
        else {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    };
    AddEditPrivateLabelShipmentComponent.prototype.IsLCLEntity = function (Entity) {
        var myResult = false;
        if (Entity.TransportModeId == "A") {
            myResult = true;
        }
        else if (Entity.TransportModeId == "O" && Entity.ShipmentTypeId == "LCLD") {
            myResult = true;
        }
        else if (Entity.TransportModeId == "I" && Entity.ShipmentTypeId == "LTL") {
            myResult = true;
        }
        return myResult;
    };
    AddEditPrivateLabelShipmentComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditPrivateLabelShipmentComponent.prototype.itemMouseOver = function (itemValue) {
        //if (this.SelectedValue != itemValue) {
        //    var img_A = document.getElementById("TransportFilter_A");
        //    var img_O = document.getElementById("TransportFilter_O");
        //    var img_I = document.getElementById("TransportFilter_I");
        //    switch (itemValue) {
        //        case "A": {
        //            img_A.setAttribute("src", "./Images/TransportModes/A.png");
        //            break;
        //        }
        //        case "O": {
        //            img_O.setAttribute("src", "./Images/TransportModes/O.png");
        //            break;
        //        }
        //        case "I": {
        //            img_I.setAttribute("src", "./Images/TransportModes/I.png");
        //            //img_I.style.top = "1px";
        //            break;
        //        }
        //    }
        //}
    };
    AddEditPrivateLabelShipmentComponent.prototype.itemMouseLeave = function (itemValue) {
        //if (this.SelectedValue != itemValue) {
        //    var img_A = document.getElementById("TransportFilter_A");
        //    var img_O = document.getElementById("TransportFilter_O");
        //    var img_I = document.getElementById("TransportFilter_I");
        //    switch (itemValue) {
        //        case "A": {
        //            img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
        //            break;
        //        }
        //        case "O": {
        //            img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
        //            break;
        //        }
        //        case "I": {
        //            img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
        //            break;
        //        }
        //    }
        //}
    };
    AddEditPrivateLabelShipmentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditPrivateLabelShipmentComponent.html',
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService])
    ], AddEditPrivateLabelShipmentComponent);
    return AddEditPrivateLabelShipmentComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditPrivateLabelShipmentComponent = AddEditPrivateLabelShipmentComponent;
var TransportationTypes = /** @class */ (function () {
    function TransportationTypes(name, transporationType, toPortCode, CountryCode) {
        this.name = name;
        this.transporationType = transporationType;
        this.toPortCode = toPortCode;
        this.Name = name;
        this.TransporationType = transporationType;
        this.ToPortCode = toPortCode;
        this.CountryCode = CountryCode;
    }
    return TransportationTypes;
}());
exports.TransportationTypes = TransportationTypes;
//# sourceMappingURL=AddEditPrivateLabelShipmentComponent.js.map