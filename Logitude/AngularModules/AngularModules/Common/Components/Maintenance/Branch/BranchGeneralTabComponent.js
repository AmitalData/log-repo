var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool } from '../../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { AddressPMService } from '../../../Services/StandardPMs/AddressPMService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
export var BranchGeneralTabComponent = (function (_super) {
    __extends(BranchGeneralTabComponent, _super);
    function BranchGeneralTabComponent(args) {
        _super.call(this);
        this.args = args;
        this.DataContext = this;
        this.IsNewEntity = true;
        this.ObjectTableName = "Branch";
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.EntityPM = args.EntityPM;
        this.addressService = new AddressPMService();
        this.Listen();
        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.IsNewEntity = true;
        }
        else {
            this.IsNewEntity = false;
            this.LoadAddress();
        }
    }
    BranchGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.LoadAddress();
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = SessionLocator.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.LoadAddress();
                    }
                });
            }
        }
    };
    BranchGeneralTabComponent.prototype.ngOnDestroy = function () {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
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
        if (!AppTool.IsNullOrEmpty(this.EntityPM.AddressId)) {
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
            if (!AppTool.IsNullOrEmpty(this.Address.CountryCode)) {
                this.FlagSrc = "./Images/Flags/" + this.Address.CountryCode + ".png";
            }
            this.BuildCityString();
        }
    };
    BranchGeneralTabComponent.prototype.BuildCityString = function () {
        var myResult = this.Address.City;
        if (!AppTool.IsNullOrEmpty(this.Address.StateEnglishName)) {
            myResult += ", " + this.Address.StateEnglishName;
        }
        if (!AppTool.IsNullOrEmpty(this.Address.ZipCode)) {
            myResult += ", " + this.Address.ZipCode;
        }
        this.CityLineText = myResult;
    };
    BranchGeneralTabComponent.prototype.EditAddressClicked = function () {
        var _this = this;
        var service = new EntityResourceService();
        service.getEntityResourceByTableName("Address", 0).subscribe(function (response1) {
            var logWindow = new LogitudeWindow();
            if (AppTool.IsNullOrEmpty(_this.EntityPM.AddressId)) {
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
                SessionLocator.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                //SessionLocator.CurrentSession.FireEvent("LoadAddress");
            });
        });
    };
    BranchGeneralTabComponent.decorators = [
        { type: Component, args: [{
                    moduleId: module.id,
                    templateUrl: './BranchGeneralTabComponent.html',
                },] },
    ];
    /** @nocollapse */
    BranchGeneralTabComponent.ctorParameters = [
        { type: EntityArgs, },
    ];
    return BranchGeneralTabComponent;
}(BaseComponent));
//# sourceMappingURL=BranchGeneralTabComponent.js.map