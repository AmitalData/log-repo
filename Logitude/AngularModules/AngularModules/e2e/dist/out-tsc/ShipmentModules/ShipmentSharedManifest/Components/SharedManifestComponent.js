"use strict";
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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var AgentSharedManifestPMService_1 = require("../../../Common/Services/StandardPMs/AgentSharedManifestPMService");
var SharedAgentManifestService_1 = require("../../../Shipment/Services/Others/SharedAgentManifestService");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var SharedManifestComponent = /** @class */ (function () {
    function SharedManifestComponent(_sharedAgentManifestService, _entityResourceService, _aentSharedManifestPMService) {
        this._sharedAgentManifestService = _sharedAgentManifestService;
        this._entityResourceService = _entityResourceService;
        this._aentSharedManifestPMService = _aentSharedManifestPMService;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsShowMarkASCompleted = false;
        this.ValidationWarningsList = [];
        this.HousesList = [];
        this.MessageNoHouseFound = "No Houses Found";
        this.IsDisableEdit = false;
        this.LableMasterCerate = "Create";
        this.LableHouseArea = "";
        this.WidthButtonStatusChange = "110px";
        this.IsShowEditButon = false;
        this.IsLoadComponent = false;
        this.ShowAreaButton = false;
        this.agentManifestSharedRefListIds = [];
        this.NumbeofCreatedHouse = 0;
        this.Retries = 0;
        this.CurrentSession.StartBusyIndicatorLoading();
    }
    SharedManifestComponent.prototype.SetWindowArgs = function (args) {
        this.EntityList = args.CurrentEntity;
        this.LoadData();
    };
    SharedManifestComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Loading...");
        this._entityResourceService.getEntityResourceByTableName("Shipment").subscribe(function (res) {
            _this._entityResourceService.getEntityResourceByTableName("Master").subscribe(function (res1) {
                _this.IsLoadComponent = true;
                _this._sharedAgentManifestService.get(_this.EntityList.Id).subscribe(function (response) {
                    if (!response.HasError) {
                        _this.CurrentEntity = response.Result;
                        _this.RunSharedManifestHeaderComponent();
                        _this.ManifestSL = _this.CurrentEntity.ManifestSL;
                        if (_this.ManifestSL.ShipmentLevelCode == 'D') {
                            _this.MessageNoHouseFound = "This is a direct shipment";
                        }
                        if (_this.CurrentEntity.CancelledBySenderAgent) {
                            _this.ValidationWarningsList = [];
                            _this.ValidationWarningsList.push("The manifest was cancelled by the sender.You are not allowed to reactivate it.");
                            _this.IsDisableEdit = true;
                        }
                        else if (!Tools_1.AppTool.IsNullOrEmpty(_this.ManifestSL.MasterNumber) && _this.ManifestSL.TransportModeId == "A") {
                            _this.CheckIfAnyShipmentHaveMasterNumber(_this.ManifestSL.MasterNumber, _this.ManifestSL.LongMaster);
                        }
                        if (_this.ManifestSL) {
                            _this.HousesList = _this.ManifestSL.Houses;
                            _this.CheckifShipmentCreateOrNotAndEnableEdit();
                        }
                        else
                            _this.CurrentSession.StopBusyIndicator();
                    }
                    else
                        _this.CurrentSession.StopBusyIndicator();
                });
            });
        });
    };
    SharedManifestComponent.prototype.CheckifShipmentCreateOrNotAndEnableEdit = function () {
        var _this = this;
        this.ManifestSL.SharedManifestRef = this.CurrentEntity.Id;
        this.agentManifestSharedRefListIds = [];
        this.agentManifestSharedRefListIds.push(this.ManifestSL.SharedManifestRef);
        if (!this.HousesList || (this.HousesList && this.HousesList.length == 0))
            this.IsNoHouses = true;
        else {
            this.IsNoHouses = false;
            var index = 0;
            this.HousesList.forEach(function (house) {
                index += 1;
                house.SharedManifestRef = _this.ManifestSL.SharedManifestRef + "/" + index.toString();
                _this.agentManifestSharedRefListIds.push(house.SharedManifestRef);
            });
        }
        // Check if Shipment Create Or Not and Enable Edit
        this._sharedAgentManifestService.getAgentSharedManifesRefShipmentListsByIds(this.agentManifestSharedRefListIds).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                var item = result.filter(function (d) { return d.AgentSharedManifestRef == _this.ManifestSL.SharedManifestRef; })[0];
                if (item) {
                    _this.ManifestSL.EntityId = item.ShipmentId;
                }
                else
                    _this.IsShowMarkASCompleted = true;
                if (!_this.IsNoHouses) {
                    _this.HousesList.forEach(function (house) {
                        var item = result.filter(function (d) { return d.AgentSharedManifestRef == house.SharedManifestRef; })[0];
                        if (item) {
                            _this.NumbeofCreatedHouse += 1;
                            house.EntityId = item.ShipmentId;
                        }
                        else {
                            _this.IsShowMarkASCompleted = true;
                        }
                    });
                    if (Tools_1.AppTool.IsNullOrEmpty(_this.ManifestSL.EntityId)) {
                        _this.LableHouseArea = "Create Master to enable houses area";
                    }
                    else {
                        _this.LableHouseArea = _this.NumbeofCreatedHouse + " of " + _this.HousesList.length + " houses created";
                    }
                }
                else {
                    _this.LableHouseArea = "";
                }
            }
            _this.RefreshSharedManifiestoStatus();
        });
    };
    SharedManifestComponent.prototype.RefreshSharedManifiestoStatus = function () {
        this.ButtonChangeStatusLable = "Cancel Manifest";
        this.WidthButtonStatusChange = "110px";
        this.IsEnableCreateMasterButton = true;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ManifestSL.EntityId)) {
            this.IsShowEditButon = true;
            this.IsEnableCreateHouse = true;
        }
        else if (this.CurrentEntity.StatusCode == "CANC") {
            this.ButtonChangeStatusLable = "reactivate";
            this.IsEnableCreateMasterButton = false;
            this.WidthButtonStatusChange = "70px";
        }
        this.ShowAreaButton = true;
        this.CurrentSession.StopBusyIndicator();
    };
    SharedManifestComponent.prototype.OpenSharedManifestAdditionalComponent = function (houseEntity) {
        var _this = this;
        var windowArgs = {};
        if (!this.IsNoHouses) {
            if ((this.NumbeofCreatedHouse + 1) == this.HousesList.length) {
                windowArgs.SharedManifestStatus = "COMP";
            }
        }
        else {
            windowArgs.SharedManifestStatus = "COMP";
        }
        windowArgs.CurrentEntity = this.CurrentEntity;
        windowArgs.ManifestSL = this.ManifestSL;
        windowArgs.HouseEntity = houseEntity;
        windowArgs.AgentSharedManifestList = this.EntityList;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 830;
        logWindow.Height = 750;
        logWindow.Title = "Shared Manifest";
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestAdditionalComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
                if (!houseEntity) {
                    _this.LableMasterCerate = "Edit";
                    _this.IsShowEditButon = true;
                    if (!_this.IsNoHouses) {
                        _this.LableHouseArea = _this.NumbeofCreatedHouse + " of " + _this.HousesList.length + " houses created";
                    }
                    else
                        _this.IsShowMarkASCompleted = false;
                    _this.RefreshSharedManifiestoStatus();
                }
                else {
                    _this.NumbeofCreatedHouse += 1;
                    _this.LableHouseArea = _this.NumbeofCreatedHouse + " of " + _this.HousesList.length + " houses created";
                    if (_this.NumbeofCreatedHouse == _this.HousesList.length) {
                        _this.IsShowMarkASCompleted = false;
                    }
                }
            }
        });
    };
    SharedManifestComponent.prototype.EditShipment = function (houseEntity) {
        var entityId = !houseEntity ? this.ManifestSL.EntityId : houseEntity.EntityId;
        if (!Tools_1.AppTool.IsNullOrEmpty(entityId)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/EditComponent/EditComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: 'Shipment' });
            });
        }
    };
    SharedManifestComponent.prototype.ChangeStatusAgentSharedManifest = function () {
        var status = this.CurrentEntity.StatusCode == "WAIT" ? "CANC" : "WAIT";
        if (status == "CANC") {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Agents Shared Logistics", "Decline Shared Manifests");
        }
        this.UpDateAgentSharedManifest(status);
    };
    SharedManifestComponent.prototype.UpDateAgentSharedManifest = function (status) {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving....");
        this.CurrentEntity.StatusCode = status;
        this.CurrentEntity.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        this._aentSharedManifestPMService.update(this.CurrentEntity).subscribe(function (res) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            _this.RefreshSharedManifiestoStatus();
        });
    };
    SharedManifestComponent.prototype.CheckIfAnyShipmentHaveMasterNumber = function (master, longMaster) {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this._sharedAgentManifestService.GetCheckIfAnyShipmentHaveMasterNumber(master, longMaster, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                if (!Tools_1.AppTool.IsNullOrEmpty(myResponse.Result)) {
                    _this.ValidationWarningsList = [];
                    _this.ValidationWarningsList.push(" Master field already used in another shipment (" + myResponse.Result + ")");
                }
            }
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });
    };
    SharedManifestComponent.prototype.MarkASCompleted = function () {
        if (this.CurrentEntity.StatusCode != "COMP") {
            this.CurrentEntity.StatusCode = "COMP";
            this.UpDateAgentSharedManifest("COMP");
        }
    };
    SharedManifestComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SharedManifestComponent.prototype.RunSharedManifestHeaderComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    SharedManifestComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestHeaderComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.Run(_this.CurrentEntity, _this.EntityList);
        });
    };
    SharedManifestComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunSharedManifestHeaderComponent(); }, 1);
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], SharedManifestComponent.prototype, "viewContainerRef", void 0);
    SharedManifestComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SharedManifestComponent',
            templateUrl: './SharedManifestComponent.html',
            providers: [SharedAgentManifestService_1.SharedAgentManifestService, AgentSharedManifestPMService_1.AgentSharedManifestPMService, EntityResourceService_1.EntityResourceService],
        }),
        __metadata("design:paramtypes", [SharedAgentManifestService_1.SharedAgentManifestService, EntityResourceService_1.EntityResourceService, AgentSharedManifestPMService_1.AgentSharedManifestPMService])
    ], SharedManifestComponent);
    return SharedManifestComponent;
}());
exports.SharedManifestComponent = SharedManifestComponent;
//# sourceMappingURL=SharedManifestComponent.js.map