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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var ArtemusWebService_1 = require("../../../../Infrastructure/Services/WebServices/ArtemusWebService");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var ABMWebService_1 = require("../../../../Infrastructure/Services/WebServices/ABMWebService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var SentToCustomComponent = /** @class */ (function (_super) {
    __extends(SentToCustomComponent, _super);
    function SentToCustomComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.DataContext = _this;
        _this.ObjectTableName = "ShipmentCustomsTransmission";
        _this.LoadCompleted = new core_1.EventEmitter();
        _this.IsABMVisible = false;
        _this.IsAESVisible = false;
        _this.IsATMSVisible_BOL = false;
        _this.IsATMSVisible_VOG = false;
        _this.IsABMDisabled = false;
        _this.IsAESDisabled = false;
        _this.IsATMSDisabled = false;
        _this.ShipmentCustomsTransmissionList = [];
        _this.IsVisible = false;
        _this.notSent = "Not Sent";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CustomsInterfaceList = [];
        _this.Initialize();
        return _this;
    }
    SentToCustomComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.EntityPM = windowArgs;
        this.FillData();
    };
    SentToCustomComponent.prototype.FillData = function () {
        this.LoadShipmentData();
        this.LoadDataList();
        this.LoadCustomsInterfaceListMethod();
    };
    SentToCustomComponent.prototype.Initialize = function () {
        this.myArtemusWebService = new ArtemusWebService_1.ArtemusWebService();
        this.myShipmentDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        this.myABMWebService = new ABMWebService_1.ABMWebService();
        this.myCommonDomainService = new CommonDomainService_1.CommonDomainService();
        this.myShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
    };
    SentToCustomComponent.prototype.LoadCustomsInterfaceListMethod = function () {
        var _this = this;
        this.myCommonDomainService.GetCustomsInterfaceListByTenant().subscribe(function (response) {
            if (!response.HasError) {
                _this.CustomsInterfaceList = response.Result;
            }
            _this.IsVisible = true;
            _this.CheckVisibility();
        });
    };
    SentToCustomComponent.prototype.LoadShipmentData = function () {
        this.LocalCustomsTransmissionsStatusName = !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LocalCustomsTransmissionsStatusName) ? this.EntityPM.LocalCustomsTransmissionsStatusName : this.notSent;
        this.LocalCustomsTransmissionsStatusDate = this.EntityPM.LocalCustomsTransmissionsStatusDate;
        this.LocalCustomsTransmissionsStatusCode = this.EntityPM.LocalCustomsTransmissionsStatusCode;
        this.LocalCustomsTransmissionsByUserName = this.EntityPM.LocalCustomsSentByUserName;
        this.LocalCustomsTransmissionsError = this.EntityPM.LocalCustomsTransmissionsStatusError;
    };
    SentToCustomComponent.prototype.LoadDataList = function () {
        var _this = this;
        this.myShipmentDomainService.GetShipmentCustomsTransmissionByShipmnetId(this.EntityPM.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.ShipmentCustomsTransmissionList = myResponse.Result;
                _this.ShipmentCustomsTransmissionList.forEach(function (item) {
                    if (item.MessageCode == 'ARBL') {
                        _this.ArtemusBOLStatus = !Tools_1.AppTool.IsNullOrEmpty(item.StatusName) ? item.StatusName : _this.notSent;
                        _this.ArtemusBOLLastSendDate = item.LastSendDate;
                        _this.ArtemusBOLByUserName = item.ByUserName;
                        _this.ArtemusBOLStatusCode = item.Status;
                        _this.ArtemusBOLError = item.Error;
                    }
                    if (item.MessageCode == 'ASVO') {
                        _this.ArtemusVoyageStatus = !Tools_1.AppTool.IsNullOrEmpty(item.StatusName) ? item.StatusName : _this.notSent;
                        _this.ArtemusVoyageLastSendDate = item.LastSendDate;
                        _this.ArtemusVoyageByUserName = item.ByUserName;
                        _this.ArtemusVoyageStatusCode = item.Status;
                        _this.ArtemusVoyageError = item.Error;
                    }
                    if (item.MessageCode == 'CBAS') {
                        _this.CBPStatus = !Tools_1.AppTool.IsNullOrEmpty(item.StatusName) ? item.StatusName : _this.notSent;
                        _this.CBPLastSendDate = item.LastSendDate;
                        _this.CBPByUserName = item.ByUserName;
                        _this.CBPStatusCode = item.Status;
                        _this.CBPError = item.Error;
                    }
                });
                if (_this.ShipmentCustomsTransmissionList.length == 0) {
                    _this.ArtemusBOLStatus = _this.notSent;
                    _this.ArtemusVoyageStatus = _this.notSent;
                    _this.CBPStatus = _this.notSent;
                }
                _this.FireEvent();
            }
        });
    };
    SentToCustomComponent.prototype.CheckVisibility = function () {
        if ((ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == null || ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == "NO")
            &&
                (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == null || ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == "NO")
            &&
                (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == null || ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == "NO")) {
        }
        else {
            this.CheckArtemusVisibility_BOL();
            this.CheckArtemusVisibility_VOG();
            this.CheckABMVisibility();
            this.CheckAESVisibility();
        }
    };
    SentToCustomComponent.prototype.CheckABMVisibility = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SendToCustoms")) {
            if (this.EntityPM.ShipmentLevelCode != "C") {
                if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM != null) {
                    if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == "ABM") {
                        this.IsABMVisible = true;
                    }
                }
            }
            else {
                this.IsABMVisible = false;
            }
        }
    };
    SentToCustomComponent.prototype.CheckArtemusVisibility_BOL = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SendToArtemus")) {
            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "I" && (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H")) {
                if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == "ART") {
                    this.IsATMSVisible_BOL = true;
                }
            }
            else {
                this.IsATMSVisible_BOL = false;
            }
        }
    };
    SentToCustomComponent.prototype.CheckArtemusVisibility_VOG = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SendToArtemus")) {
            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "I" && (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "C")) {
                if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == "ART") {
                    this.IsATMSVisible_VOG = true;
                }
            }
            else {
                this.IsATMSVisible_VOG = false;
            }
        }
    };
    SentToCustomComponent.prototype.CheckAESVisibility = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SENDTOAES")) {
            if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM != null) {
                if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == "CBP") {
                    if (this.EntityPM.DirectionId == "E") {
                        this.IsAESVisible = true;
                    }
                }
            }
        }
    };
    SentToCustomComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SentToCustomComponent.prototype.ViewCustomsSettings = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Customs Settings";
        logWindow.Show('./Common/Components/Maintenance/CustomsInterface/CustomsInterfaceSettingsComponent');
        logWindow.WindowClosed.subscribe(function (comp) {
            _this.FillData();
        });
    };
    SentToCustomComponent.prototype.SendButtonClicked = function (arg) {
        switch (arg) {
            case "ABM":
                {
                    this.SendToCustoms();
                    break;
                }
            case "ASV":
                {
                    this.SendToArtemus_Voyage();
                    break;
                }
            case "BOL":
                {
                    if (this.ArtemusVoyageStatusCode != "SENT" && this.ArtemusVoyageStatusCode != "ACPT" && this.EntityPM.ShipmentLevelCode == "D") {
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Show("Can't send Bill of Lading message before sending the voyage message");
                    }
                    else {
                        this.SendToArtemus_Bill();
                    }
                    break;
                }
            case "CBP":
                {
                    this.SendToAES();
                }
        }
    };
    SentToCustomComponent.prototype.CheckInterfaceByCode = function (code) {
        return this.CustomsInterfaceList.filter(function (a) { return a.LocalCustomsInterfaceCode == code; })[0];
    };
    SentToCustomComponent.prototype.SendToCustoms = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("Sending in Progress..");
            this.myABMWebService.Send(this.EntityPM.Id).subscribe(function (myResponse) {
                if (myResponse == null) {
                    _this.CurrentSession.StopBusyIndicator();
                }
                else if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    var myResult = myResponse.Result;
                    if (myResult == null) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                        _this.IsMessageValid = false;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        _this.ReloadEntity();
                    }
                }
            });
        }
    };
    SentToCustomComponent.prototype.SendToArtemus_Voyage = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        this.CurrentSession.StartBusyIndicator("Sending...");
        this.myArtemusWebService.SendAMS_Voyage(this.EntityPM.Id).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
                _this.MessageText = "Checking Required Fields in Shipment...";
                _this.IsMessageValid = false;
            }
            else {
                _this.MessageText = "Voyage message has been sent successfully";
                _this.IsMessageValid = true;
                _this.LoadDataList();
            }
        });
    };
    SentToCustomComponent.prototype.SendToArtemus_Bill = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        this.CurrentSession.StartBusyIndicator("Sending...");
        this.myArtemusWebService.SendAMS_Bill(this.EntityPM.Id).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
                _this.MessageText = "Checking Required Fields in Shipment...";
                _this.IsMessageValid = false;
            }
            else {
                _this.MessageText = "BOL message has been sent successfully";
                _this.IsMessageValid = true;
                _this.LoadDataList();
            }
        });
    };
    SentToCustomComponent.prototype.SendToAES = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Exporting AES File";
        logWindow.Width = 500;
        logWindow.Height = 200;
        logWindow.Show('./ShipmentModules/ShipmentTabs/Components/Customs/ExportFileComponent');
        logWindow.ComponentLoaded.subscribe(function (comp) {
            comp.Export(_this.EntityPM.Id);
        });
        logWindow.WindowClosed.subscribe(function (comp) {
            _this.LoadDataList();
        });
    };
    SentToCustomComponent.prototype.ReloadEntity = function () {
        var _this = this;
        if (this.myShipmentPMService == null) {
            this.myShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
        }
        this.myShipmentPMService.get(this.EntityPM.Id).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.EntityPM = myResponse.Result;
                    _this.CurrentSession.CurrentEditComponent.EntityPM = myResponse.Result;
                    _this.MessageText = "The message has been sent successfully";
                    _this.IsMessageValid = true;
                    _this.LoadCompleted.emit(true);
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.LoadCompleted.emit(false);
                }
                _this.CurrentSession.StopBusyIndicator();
                _this.LoadShipmentData();
                _this.CurrentSession.FireEvent("CustomsWizardClosed");
            }
        });
    };
    SentToCustomComponent.prototype.SetCellNotesWidth = function (text) {
        var myColumnWidth = 0;
        var widthOfLabel = 0;
        if (!Tools_1.AppTool.IsNullOrEmpty(text)) {
            var widthOfLabel = Tools_1.AppTool.GetTextWidth(text) + 10;
        }
        return widthOfLabel;
    };
    SentToCustomComponent.prototype.FireEvent = function () {
        this.CurrentSession.FireEvent("RefreshCustomsSummary");
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SentToCustomComponent.prototype, "LoadCompleted", void 0);
    SentToCustomComponent = __decorate([
        core_1.Component({
            selector: 'SentToCustomComponent',
            moduleId: module.id,
            templateUrl: './SentToCustomComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SentToCustomComponent);
    return SentToCustomComponent;
}(BaseComponent_1.BaseComponent));
exports.SentToCustomComponent = SentToCustomComponent;
//# sourceMappingURL=SentToCustomComponent.js.map