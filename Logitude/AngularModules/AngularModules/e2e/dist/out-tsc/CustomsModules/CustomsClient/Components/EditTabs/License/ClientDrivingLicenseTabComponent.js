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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ClientDrivingLicensePM_1 = require("../../../../../Customs/EntityPMs/ClientDrivingLicensePM");
var ClientDrivingLicenseTypePM_1 = require("../../../../../Customs/EntityPMs/ClientDrivingLicenseTypePM");
var ClientPM_1 = require("../../../../../Customs/EntityPMs/ClientPM");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var ClientDrivingLicenseTabComponent = /** @class */ (function (_super) {
    __extends(ClientDrivingLicenseTabComponent, _super);
    function ClientDrivingLicenseTabComponent(_EntityArgs) {
        var _this = _super.call(this) || this;
        _this._EntityArgs = _EntityArgs;
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.DataContext = _this;
        _this.EntityPM = new ClientPM_1.ClientPM();
        _this.ObjectTableName = "Customs.Client";
        _this.isControlEnabled = true;
        _this.ValidationErrorsList = [];
        _this.Mode = "";
        _this.newAddressButtonVisibility = true;
        _this.editButtonVisibility = true;
        _this.ClientDrivingLicenseList = new ObservableCollection_1.ObservableCollection([]);
        _this.ClientDrivingLicenseTypeList = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._SelectedRow = null;
        return _this;
    }
    ;
    ;
    ClientDrivingLicenseTabComponent.prototype.InitTab = function (EntityPM, IsNew) {
        this.ValidationErrorsList = [];
        this.EntityPM = EntityPM;
        this.isNewClient = IsNew;
        if (!this.EntityPM.Code.startsWith("5")) {
            this.isControlEnabled = false;
        }
        this.BuildClientDrivingLicenseList();
    };
    Object.defineProperty(ClientDrivingLicenseTabComponent.prototype, "NewAddressButtonVisibility", {
        get: function () { return this.newAddressButtonVisibility; },
        set: function (newValue) { this.newAddressButtonVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientDrivingLicenseTabComponent.prototype, "EditButtonVisibility", {
        get: function () { return this.editButtonVisibility; },
        set: function (newValue) { this.editButtonVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientDrivingLicenseTabComponent.prototype, "SelectedRow", {
        get: function () {
            return this._SelectedRow;
        },
        set: function (value) {
            if (this._SelectedRow != value) {
                this._SelectedRow = value;
                this.SelectedRow.BuildClientDrivingLicenseTypeList();
            }
        },
        enumerable: true,
        configurable: true
    });
    ClientDrivingLicenseTabComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
        //if (itemComponent != null) {
        //    this.SelectedRow.BuildClientDrivingLicenseTypeList();
        //}
    };
    ClientDrivingLicenseTabComponent.prototype.onCellSelected = function ($event, Item) {
        if (this.SelectedRow != Item) {
            this.OnRowSelected(Item);
        }
    };
    ClientDrivingLicenseTabComponent.prototype.ReloadEntityPM = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    ClientDrivingLicenseTabComponent.prototype.AddClientDrivingLicense = function () {
        this.ValidationErrorsList = [];
        var newClientDrivingLicensePM = new ClientDrivingLicensePM_1.ClientDrivingLicensePM(this.EntityPM);
        newClientDrivingLicensePM.Tenant = this.EntityPM.Tenant;
        newClientDrivingLicensePM.ClientId = this.EntityPM.Id;
        if (!this.EntityPM.ClientDrivingLicenses.includes(newClientDrivingLicensePM)) {
            this.EntityPM.AddClientDrivingLicense(newClientDrivingLicensePM);
            var newClientDrivingLicenseItemModel = new ClientDrivingLicenseItemModel(newClientDrivingLicensePM, this);
            this.ClientDrivingLicenseList.Insert(newClientDrivingLicenseItemModel);
            this.OnRowSelected(newClientDrivingLicenseItemModel);
        }
    };
    ClientDrivingLicenseTabComponent.prototype.BuildClientDrivingLicenseList = function () {
        this.ClientDrivingLicenseList = new ObservableCollection_1.ObservableCollection([]);
        this.ClientDrivingLicenseTypeList = new ObservableCollection_1.ObservableCollection([]);
        if (this.EntityPM.ClientDrivingLicenses != null && this.EntityPM.ClientDrivingLicenses.length > 0) {
            for (var _i = 0, _a = this.EntityPM.ClientDrivingLicenses; _i < _a.length; _i++) {
                var item = _a[_i];
                this.ClientDrivingLicenseList.Insert(new ClientDrivingLicenseItemModel(item, this));
            }
            this.OnRowSelected(this.ClientDrivingLicenseList.Collection[0]);
        }
    };
    ClientDrivingLicenseTabComponent.prototype.RemoveClientDrivingLicense = function (item) {
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            item.RemoveAllDrivingLicenseType();
            this.ClientDrivingLicenseList.Remove(item);
            this.EntityPM.RemoveClientDrivingLicense(item.ClientDrivingLicensePM);
        }
        if (this.SelectedRow == item && this.ClientDrivingLicenseList != null) {
            this.ClientDrivingLicenseTypeList = new ObservableCollection_1.ObservableCollection([]);
            if (this.ClientDrivingLicenseList.Collection.length == 0) {
                this.OnRowSelected(null);
            }
            else {
                this.OnRowSelected(this.ClientDrivingLicenseList.Collection[this.ClientDrivingLicenseList.Collection.length - 1]);
            }
        }
    };
    ClientDrivingLicenseTabComponent.prototype.AddClientDrivingLicenseType = function () {
        this.ValidationErrorsList = [];
        if (this.SelectedRow == null) {
            this.ValidationErrorsList.push("חובה להזין/לבחור רישיון נהיגה");
            return;
        }
        var newClientDrivingLicenseTypePM = new ClientDrivingLicenseTypePM_1.ClientDrivingLicenseTypePM(this.EntityPM);
        newClientDrivingLicenseTypePM.Tenant = this.EntityPM.Tenant;
        newClientDrivingLicenseTypePM.ClientId = this.EntityPM.Id;
        if (!this.ClientDrivingLicenseTypeList.Collection.includes(newClientDrivingLicenseTypePM)) {
            this.SelectedRow.AddClientDrivingLicenseType(newClientDrivingLicenseTypePM);
            this.ClientDrivingLicenseTypeList.Insert(new ClientDrivingLicenseTypeItemModel(newClientDrivingLicenseTypePM, this));
        }
    };
    ClientDrivingLicenseTabComponent.prototype.RemoveClientDrivingLicenseType = function (item) {
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.ClientDrivingLicenseTypeList.Remove(item);
            this.SelectedRow.RemoveClientDrivingLicenseType(item.ClientDrivingLicenseTypePM);
        }
    };
    ClientDrivingLicenseTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClientDrivingLicenseTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ClientDrivingLicenseTabComponent);
    return ClientDrivingLicenseTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ClientDrivingLicenseTabComponent = ClientDrivingLicenseTabComponent;
var ClientDrivingLicenseItemModel = /** @class */ (function (_super) {
    __extends(ClientDrivingLicenseItemModel, _super);
    function ClientDrivingLicenseItemModel(clientDrivingLicensePM, parent) {
        var _this = _super.call(this) || this;
        _this.clientDrivingLicensePM = clientDrivingLicensePM;
        _this.ClientDrivingLicensePM = null;
        _this.ObjectTableName = "Customs.ClientDrivingLicense";
        _this.DataContext = _this;
        _this.ClientDrivingLicensePM = clientDrivingLicensePM;
        _this.Parent = parent;
        _this.BuildClientDrivingLicenseTypeList();
        return _this;
    }
    Object.defineProperty(ClientDrivingLicenseItemModel.prototype, "DrivingLicenseNumber", {
        //#region Properties
        get: function () { return this.ClientDrivingLicensePM.DrivingLicenseNumber; },
        set: function (value) {
            if (this.ClientDrivingLicensePM.DrivingLicenseNumber != value) {
                this.ClientDrivingLicensePM.DrivingLicenseNumber = value;
                if (this.Parent.SelectedRow != this) {
                    this.Parent.OnRowSelected(this);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientDrivingLicenseItemModel.prototype, "DriverLicenseValidityDate", {
        get: function () { return this.ClientDrivingLicensePM.DriverLicenseValidityDate; },
        set: function (value) {
            if (this.ClientDrivingLicensePM.DriverLicenseValidityDate != value) {
                this.ClientDrivingLicensePM.DriverLicenseValidityDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientDrivingLicenseItemModel.prototype, "DrivingLicenseCountryID", {
        get: function () { return this.ClientDrivingLicensePM.DrivingLicenseCountryID; },
        set: function (value) {
            if (this.ClientDrivingLicensePM.DrivingLicenseCountryID != value) {
                this.ClientDrivingLicensePM.DrivingLicenseCountryID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientDrivingLicenseItemModel.prototype, "DrivingLicenseCountryName", {
        get: function () { return this.ClientDrivingLicensePM.DrivingLicenseCountryName; },
        set: function (value) {
            if (this.ClientDrivingLicensePM.DrivingLicenseCountryName != value) {
                this.ClientDrivingLicensePM.DrivingLicenseCountryName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    ClientDrivingLicenseItemModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    ClientDrivingLicenseItemModel.prototype.BuildClientDrivingLicenseTypeList = function () {
        this.Parent.ClientDrivingLicenseTypeList = new ObservableCollection_1.ObservableCollection([]);
        if (this.ClientDrivingLicensePM.ClientDrivingLicenseTypes != null && this.ClientDrivingLicensePM.ClientDrivingLicenseTypes.length > 0) {
            for (var _i = 0, _a = this.ClientDrivingLicensePM.ClientDrivingLicenseTypes; _i < _a.length; _i++) {
                var item = _a[_i];
                this.Parent.ClientDrivingLicenseTypeList.Insert(new ClientDrivingLicenseTypeItemModel(item, this.Parent));
            }
        }
    };
    ClientDrivingLicenseItemModel.prototype.AddClientDrivingLicenseType = function (newClientDrivingLicenseTypePM) {
        this.ClientDrivingLicensePM.AddClientDrivingLicenseType(newClientDrivingLicenseTypePM);
    };
    ClientDrivingLicenseItemModel.prototype.RemoveAllDrivingLicenseType = function () {
        if (this.ClientDrivingLicensePM.ClientDrivingLicenseTypes != null && this.ClientDrivingLicensePM.ClientDrivingLicenseTypes.length > 0) {
            for (var _i = 0, _a = this.ClientDrivingLicensePM.ClientDrivingLicenseTypes; _i < _a.length; _i++) {
                var item = _a[_i];
                this.ClientDrivingLicensePM.RemoveClientDrivingLicenseType(item);
            }
        }
    };
    ClientDrivingLicenseItemModel.prototype.RemoveClientDrivingLicenseType = function (item) {
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.ClientDrivingLicensePM.RemoveClientDrivingLicenseType(item);
        }
    };
    return ClientDrivingLicenseItemModel;
}(BaseComponent_1.BaseComponent));
exports.ClientDrivingLicenseItemModel = ClientDrivingLicenseItemModel;
var ClientDrivingLicenseTypeItemModel = /** @class */ (function (_super) {
    __extends(ClientDrivingLicenseTypeItemModel, _super);
    function ClientDrivingLicenseTypeItemModel(clientDrivingLicenseTypePM, parent) {
        var _this = _super.call(this) || this;
        _this.clientDrivingLicenseTypePM = clientDrivingLicenseTypePM;
        _this.ClientDrivingLicenseTypePM = null;
        _this.ObjectTableName = "Customs.ClientDrivingLicenseType";
        _this.DataContext = _this;
        _this.ClientDrivingLicenseTypePM = clientDrivingLicenseTypePM;
        _this.Parent = parent;
        return _this;
    }
    Object.defineProperty(ClientDrivingLicenseTypeItemModel.prototype, "DriversLicenseTypeCode", {
        //#region Properties
        get: function () { return this.ClientDrivingLicenseTypePM.DriversLicenseTypeCode; },
        set: function (value) {
            if (this.ClientDrivingLicenseTypePM.DriversLicenseTypeCode != value) {
                this.ClientDrivingLicenseTypePM.DriversLicenseTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    ClientDrivingLicenseTypeItemModel.prototype.OnDriversLicenseLostFocus = function (event) {
        this.Parent.ValidationErrorsList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DriversLicenseTypeCode)) {
            if (this.DriversLicenseTypeCode.toString().length > 2) {
                this.Parent.ValidationErrorsList.push("סוג רשיון ארוך מדי (ניתן להזין עד 2 תווים)");
            }
        }
    };
    return ClientDrivingLicenseTypeItemModel;
}(BaseComponent_1.BaseComponent));
exports.ClientDrivingLicenseTypeItemModel = ClientDrivingLicenseTypeItemModel;
//# sourceMappingURL=ClientDrivingLicenseTabComponent.js.map