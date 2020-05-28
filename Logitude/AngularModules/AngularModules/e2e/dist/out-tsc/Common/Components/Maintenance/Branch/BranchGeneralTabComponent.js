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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var AddressPMService_1 = require("../../../Services/StandardPMs/AddressPMService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var BranchGeneralTabComponent = /** @class */ (function (_super) {
    __extends(BranchGeneralTabComponent, _super);
    function BranchGeneralTabComponent(args) {
        var _this = _super.call(this) || this;
        _this.args = args;
        _this.DataContext = _this;
        _this.IsNewEntity = true;
        _this.ObjectTableName = "Branch";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.EntityPM = args.EntityPM;
        _this.addressService = new AddressPMService_1.AddressPMService();
        _this.Listen();
        if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id)) {
            _this.IsNewEntity = true;
        }
        else {
            _this.IsNewEntity = false;
            _this.LoadAddress();
        }
        return _this;
    }
    BranchGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.LoadAddress();
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.LoadAddress();
                    }
                });
            }
        }
    };
    BranchGeneralTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    Object.defineProperty(BranchGeneralTabComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (value) {
            if (this.EntityPM.EnglishName != value) {
                this.EntityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BranchGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BranchGeneralTabComponent.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        set: function (value) {
            if (this.EntityPM.Code != value) {
                this.EntityPM.Code = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BranchGeneralTabComponent.prototype, "Signature", {
        get: function () { return this.EntityPM.Signature; },
        set: function (value) {
            if (this.EntityPM.Signature != value) {
                this.EntityPM.Signature = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BranchGeneralTabComponent.prototype, "CounterCode", {
        get: function () { return this.EntityPM.CounterCode; },
        set: function (value) {
            if (this.EntityPM.CounterCode != value) {
                this.EntityPM.CounterCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BranchGeneralTabComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) {
            if (this.EntityPM.InActive != value) {
                this.EntityPM.InActive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    BranchGeneralTabComponent.prototype.LoadAddress = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AddressId)) {
            this.addressService.get(this.EntityPM.AddressId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.Address = myResponse.Result;
                    _this.FillAddressProperties();
                }
            });
        }
    };
    BranchGeneralTabComponent.prototype.FillAddressProperties = function () {
        if (this.Address != null) {
            this.AddressName = this.Address.Name;
            this.Address1 = this.Address.Address1;
            this.Address2 = this.Address.Address2;
            this.CountryName = this.Address.CountryName;
            this.PhoneNumber = this.Address.PhoneNumber;
            this.FaxNumber = this.Address.FaxNumber;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Address.CountryCode)) {
                this.FlagSrc = "./Images/Flags/" + this.Address.CountryCode + ".png";
            }
            this.BuildCityString();
        }
    };
    BranchGeneralTabComponent.prototype.BuildCityString = function () {
        var myResult = this.Address.City;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Address.StateEnglishName)) {
            myResult += ", " + this.Address.StateEnglishName;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Address.ZipCode)) {
            myResult += ", " + this.Address.ZipCode;
        }
        this.CityLineText = myResult;
    };
    BranchGeneralTabComponent.prototype.EditAddressClicked = function () {
        var _this = this;
        var service = new EntityResourceService_1.EntityResourceService();
        service.getEntityResourceByTableName("Address", 0).subscribe(function (response1) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.AddressId)) {
                logWindow.Title = "Add Address";
                var address = _this.addressService.GetNewEntityPM();
                address.AddressTypeId = "M";
                address.Description = "Main Address";
                address.BranchId = _this.EntityPM.Id;
                logWindow.WindowArgs = address;
            }
            else {
                logWindow.Title = "Edit Address";
                logWindow.WindowArgs = _this.Address;
            }
            logWindow.Show('./Common/Components/Maintenance/Branch/AddEditBranchAddressComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                //this.CurrentSession.FireEvent("LoadAddress");
            });
        });
    };
    BranchGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './BranchGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], BranchGeneralTabComponent);
    return BranchGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.BranchGeneralTabComponent = BranchGeneralTabComponent;
//# sourceMappingURL=BranchGeneralTabComponent.js.map