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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var VehicleSafetyAccessoryPM_1 = require("../../../../Customs/EntityPMs/VehicleSafetyAccessoryPM");
var VehicleOwnerPM_1 = require("../../../../Customs/EntityPMs/VehicleOwnerPM");
//import { VehicleMessagesService } from '../../../Services/WebServices/VehicleMessagesService';
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var VehiclesOwnersAndSafetyTabComponent = /** @class */ (function (_super) {
    __extends(VehiclesOwnersAndSafetyTabComponent, _super);
    function VehiclesOwnersAndSafetyTabComponent(entityArgs, cd, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.cd = cd;
        _this.EntityResourceService = EntityResourceService;
        _this.FillValidationErrorList = new core_1.EventEmitter();
        _this.ObjectTableName = "Customs.Vehicle";
        _this.DataContext = _this;
        _this.IsNewEntity = false;
        _this.SubCountryCodeEnabled = false;
        _this.IsDelete = false;
        _this.Loaded = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        ///#region Properties
        _this.SendButtonsVisibility = false;
        //#endregion
        _this.line = 0;
        _this.SafetiesList = new ObservableCollection_1.ObservableCollection([]);
        _this.OwnersList = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityResourceService.getEntityResourceByTableName("Customs.VehicleOwner").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.VehicleSafetyAccessory").subscribe(function (response) {
                _this.ObjectTableName = _this.entityArgs.ObjectTableName;
                _this.EntityPM = _this.entityArgs.EntityPM;
                _this.OnEditTabSelected();
                _this.Loaded = true;
                _this.Listen();
            });
        });
        return _this;
    }
    VehiclesOwnersAndSafetyTabComponent.prototype.OnEditTabSelected = function () {
        var _this = this;
        this.SafetiesList.Clear();
        this.OwnersList.Clear();
        this.EntityPM.VehicleSafetyAccessories.forEach(function (item) { return _this.SafetiesList.Insert(item); });
        this.EntityPM.VehicleOwners.forEach(function (item) { return _this.OwnersList.Insert(item); });
        this.CheckErrorState();
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.DeleteOwnersList = function (item) {
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.EntityPM.RemoveVehicleOwner(item);
            this.OwnersList.Remove(item);
        }
        this.CheckErrorState();
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.DeleteSafetiesList = function (item) {
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.EntityPM.RemoveVehicleSafetyAccessory(item);
            this.SafetiesList.Remove(item);
        }
        this.CheckErrorState();
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.CheckErrorState = function () {
        this.SetOwnersErrorMessage();
        this.SetSafetiesErrorMessage();
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.VehicleOwnerChanged = function (item, $event) {
        var myClientList = $event;
        var myVehicleOwnerPM = item;
        if (!Tools_1.AppTool.IsNullOrEmpty(myClientList)) {
            myVehicleOwnerPM.ClientName = myClientList.FullName;
            myVehicleOwnerPM.FirstName = myClientList.LocalFirstName;
            if (Tools_1.AppTool.IsNullOrEmpty(myClientList.LocalFirstName) && Tools_1.AppTool.IsNullOrEmpty(myClientList.LocalLastName)) {
                myVehicleOwnerPM.LastNameOrCorporationName = myClientList.LocalCorporationName;
            }
            else {
                myVehicleOwnerPM.LastNameOrCorporationName = myClientList.LocalLastName;
            }
        }
        else {
            myVehicleOwnerPM.LastNameOrCorporationName = null;
            myVehicleOwnerPM.ClientName = null;
            myVehicleOwnerPM.FirstName = null;
        }
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.SetVehicleSafeAccessoryInstlType = function (item, lookupEntity) {
        if (!Tools_1.AppTool.IsNullOrEmpty(lookupEntity)) {
            item.VehicleSafAccessoryInstlTypName = lookupEntity.LocalName;
        }
        else {
            item.VehicleSafAccessoryInstlTypName = null;
        }
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.SetVehicleSafetyAccessoryTypeLocalName = function (item, lookupEntity) {
        //alert("SetLocalName");
        ///console.log(lookupEntity);
        if (!Tools_1.AppTool.IsNullOrEmpty(lookupEntity)) {
            item.VehicleSafetyAccessoryName = lookupEntity.LocalName;
        }
        else {
            item.VehicleSafetyAccessoryName = null;
        }
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.SetOwnersErrorMessage = function () {
        this._OwnersErrorMessage = null;
        if (this.EntityPM.VehicleOwners.length >= 10) {
            this._OwnersErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vehicle.O.CantEnterMore");
        }
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.AddOwnersList = function () {
        this.CheckErrorState();
        if (this._OwnersErrorMessage)
            return;
        var newVehicleOwnerPM = new VehicleOwnerPM_1.VehicleOwnerPM(this.EntityPM);
        newVehicleOwnerPM.Tenant = this.EntityPM.Tenant;
        newVehicleOwnerPM.LineNumber = (Tools_1.ArrayTool.Max(this.EntityPM.VehicleSafetyAccessories, "LineNumber") + 1);
        //this.SafetiesList.Clear();
        this.OwnersList.Insert(newVehicleOwnerPM);
        this.EntityPM.AddVehicleOwner(newVehicleOwnerPM);
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.SetSafetiesErrorMessage = function () {
        this._SafetiesErrorMessage = null;
        if (this.EntityPM.VehicleSafetyAccessories.length >= 10) {
            this._SafetiesErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vehicle.O.CantEnterMore");
        }
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.AddRowSafetiesList = function () {
        //alert("AddRowSafetiesList");
        this.CheckErrorState();
        if (this._SafetiesErrorMessage)
            return;
        var newVehicleSafetyAccessoryPM = new VehicleSafetyAccessoryPM_1.VehicleSafetyAccessoryPM(this.EntityPM);
        newVehicleSafetyAccessoryPM.Tenant = this.EntityPM.Tenant;
        newVehicleSafetyAccessoryPM.LineNumber = (Tools_1.ArrayTool.Max(this.EntityPM.VehicleSafetyAccessories, "LineNumber") + 1);
        //this.SafetiesList.Clear();
        this.SafetiesList.Insert(newVehicleSafetyAccessoryPM);
        this.EntityPM.AddVehicleSafetyAccessory(newVehicleSafetyAccessoryPM);
    };
    Object.defineProperty(VehiclesOwnersAndSafetyTabComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    VehiclesOwnersAndSafetyTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.OnEditTabSelected();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    //this.DisplayOnlyCheck();
                    _this.OnEditTabSelected();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DEGC") {
                        //this.DisplayOnlyCheck();
                        _this.OnEditTabSelected();
                    }
                }
            }));
        }
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;
        ///console.log("EntityPM", this.EntityPM);
        this.OnEditTabSelected();
        this.SetFieldsEditability();
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.SetFieldsEditability = function () {
        //this.UIProperties.SetEnabled("VehicleTypeCode", this.ObjectTableName, this.IsNewEntity);
        //this.UIProperties.SetEnabled("SubCountryCode", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.EntityPM.CountryCode));
    };
    //#endregion
    VehiclesOwnersAndSafetyTabComponent.prototype.checkForChassisNumberOp_Completed = function (exists) {
        if (exists) {
            this.UIProperties.SetValidity("InternalCode", "Customs.CustomBank", false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomBank.O.InternalCodekAlreadyExist"));
            //this.InvalidChassisNumber = true;
        }
        else {
            //this.InvalidChassisNumber = false;
        }
    };
    //#region Send + Delete
    VehiclesOwnersAndSafetyTabComponent.prototype.SendButtonClicked = function () {
        var errors = [];
        this.FillValidationErrorList.emit(errors); // clear validation msgs
        // validate Vehicle
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Customs.Vehicle", errors);
        //if (this.EntityPM.VehicleCommunications.length == 0) {
        //    errors.push(TextCodeTranslator.Translate("Customs.Vehicle.O.RequierdCommunication"));
        //} else {
        //    // validate Vehicle communication items
        //    this.EntityPM.VehicleCommunications.forEach((item) => {
        //        Validator.TryValidateObject(item, "Customs.VehicleCommunication", errors);
        //    });
        //}
        if (errors.length > 0) {
            this.ValdationErrorList = errors;
            this.FillValidationErrorList.emit(errors);
        }
        else {
            // send request
            //this.SendRequest(false);
        }
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.OnSendCompleted = function () {
        if (this.IsDelete) {
            this.ApplyDeleteVehicle();
        }
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.ApplyDeleteVehicle = function () {
        this.IsDelete = false;
        //RefreshDataEvent refreshDataEvent = eventAggregator.GetEvent<RefreshDataEvent>();
        //refreshDataEvent.Publish(new RefreshDataEventArgs() { });
        this.CurrentSession.CloseCurrentWindow(); //currentAssemlyLocator.CurrentSimplogWindow.Close();
        //TenantContext.Current.RefreshTableData("Customs.Vehicle", DateTime.UtcNow, true);
        //this.Dispose();
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.SetPassportTypeLocalName = function (item, lookupEntity) {
        if (!Tools_1.AppTool.IsNullOrEmpty(lookupEntity)) {
            item.ImporterPassportTypeName = lookupEntity.LocalName;
        }
        else {
            item.ImporterPassportTypeName = null;
        }
    };
    VehiclesOwnersAndSafetyTabComponent.prototype.SetPassportCountryLocalName = function (item, lookupEntity) {
        if (!Tools_1.AppTool.IsNullOrEmpty(lookupEntity)) {
            item.PassCountryName = lookupEntity.LocalName;
        }
        else {
            item.PassCountryName = null;
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], VehiclesOwnersAndSafetyTabComponent.prototype, "FillValidationErrorList", void 0);
    VehiclesOwnersAndSafetyTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VehiclesOwnersAndSafetyTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef, EntityResourceService_1.EntityResourceService])
    ], VehiclesOwnersAndSafetyTabComponent);
    return VehiclesOwnersAndSafetyTabComponent;
}(BaseComponent_1.BaseComponent));
exports.VehiclesOwnersAndSafetyTabComponent = VehiclesOwnersAndSafetyTabComponent;
//# sourceMappingURL=VehiclesOwnersAndSafetyTabComponent.js.map