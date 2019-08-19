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
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ClientAddressPM_1 = require("../../../../../Customs/EntityPMs/ClientAddressPM");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var CustomMessageProgressComponent_1 = require("../../../../CustomsControls/Components/CustomMessageProgressComponent");
var ClientMessagesService_1 = require("../../../../../Customs/Services/WebServices/ClientMessagesService");
var INF_MSG_GenericResponseData_1 = require("../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData");
var AddAddressContactForClientRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/AddAddressContactForClientRequestParams");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var ClientAddressesTabComponent = /** @class */ (function (_super) {
    __extends(ClientAddressesTabComponent, _super);
    function ClientAddressesTabComponent(_EntityArgs) {
        var _this = _super.call(this) || this;
        _this._EntityArgs = _EntityArgs;
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.clientMessageService = new ClientMessagesService_1.ClientMessagesService();
        _this.Mode = "";
        _this.newAddressButtonVisibility = true;
        _this.editButtonVisibility = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    ClientAddressesTabComponent.prototype.InitTab = function (EntityPM, IsNew) {
        this.entityPM = EntityPM;
        this.isNewClient = IsNew;
    };
    ClientAddressesTabComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.Mode = "Claims";
            this.entityPM = args.EntityPM;
            this.Parent = args.Parent;
            this.NewAddressButtonVisibility = false;
            this.EditButtonVisibility = false;
        }
    };
    Object.defineProperty(ClientAddressesTabComponent.prototype, "NewAddressButtonVisibility", {
        get: function () { return this.newAddressButtonVisibility; },
        set: function (newValue) { this.newAddressButtonVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientAddressesTabComponent.prototype, "EditButtonVisibility", {
        get: function () { return this.editButtonVisibility; },
        set: function (newValue) { this.editButtonVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    ClientAddressesTabComponent.prototype.NewAddressButtonClicked = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("Customs.ClientAddress").subscribe(function (response) {
            _this.entityResourceService.getEntityResourceByTableName("Customs.ClientsAddressCommType").subscribe(function (response) {
                // this.entityPM.ClientAddresses[0].IsHebrewAddress                
                var item = new ClientAddressPM_1.ClientAddressPM(_this.entityPM);
                item.Tenant = _this.entityPM.Tenant;
                item.ClientId = _this.entityPM.Id;
                item.ContactFirstName = _this.entityPM.EnglishFirstName;
                var windowArgs = {};
                windowArgs.clientAddressPM = item;
                windowArgs.clientPM = _this.entityPM;
                windowArgs.IsNew = true;
                windowArgs.isNewClient = _this.isNewClient;
                var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.AddAddress"); //"New Address";
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 800;
                logWindow.Height = 700;
                logWindow.Title = windowTitle;
                logWindow.ShowCloseButton = true;
                logWindow.WindowArgs = windowArgs;
                // logWindow.WindowClosed.subscribe(($event: any) => this.SetCertificateStatusVisibility());
                logWindow.Show('./CustomsModules/CustomsClient/Components/EditTabs/Addresses/AddEditAddressComponent');
            });
        });
    };
    ClientAddressesTabComponent.prototype.EditAddress = function (address) {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("Customs.ClientAddress").subscribe(function (response) {
            _this.entityResourceService.getEntityResourceByTableName("Customs.ClientsAddressCommType").subscribe(function (response) {
                var windowArgs = {};
                windowArgs.clientAddressPM = address;
                windowArgs.clientPM = _this.entityPM;
                windowArgs.IsNew = false;
                windowArgs.isNewClient = _this.isNewClient;
                var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.AddAddress"); //"New Address";
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 800;
                logWindow.Height = 700;
                logWindow.Title = windowTitle;
                logWindow.ShowCloseButton = true;
                logWindow.WindowArgs = windowArgs;
                //logWindow.WindowClosed.subscribe(($event: any) => this.ReloadEntityPM());
                logWindow.WindowClosed.subscribe(function (arg) {
                    if (arg == "ReloadEntity") {
                        _this._EntityArgs.SendMessage("ReloadEntity");
                    }
                });
                logWindow.Show('./CustomsModules/CustomsClient/Components/EditTabs/Addresses/AddEditAddressComponent');
            });
        });
    };
    ClientAddressesTabComponent.prototype.DeleteAddress = function (address) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
        confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Cancel");
        confirmWindow.Width = 300;
        confirmWindow.Show("האם למחוק את הכתובת?");
        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.entityPM.RemoveClientAddress(address);
                _this.operationType = AddAddressContactForClientRequestParams_1.OperationTypes.Delete;
                _this.SendAddUpdateDeleteClientAddressContactRequest(address);
                confirmWindow.Close();
            }
        });
    };
    ClientAddressesTabComponent.prototype.ReloadEntityPM = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    ClientAddressesTabComponent.prototype.SendAddUpdateDeleteClientAddressContactRequest = function (address) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("");
        var currRequestParams = new AddAddressContactForClientRequestParams_1.AddAddressContactForClientRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.AddressCode = new AddAddressContactForClientRequestParams_1.ClientAddress();
        currRequestParams.AddressCode.AddressId = address.AddressId;
        currRequestParams.AddressCode.AddressContactState = address.ContactStateCode;
        currRequestParams.AddressCode.AddressTypeCode = address.AddressTypeCode;
        currRequestParams.AddressCode.AddressPurposeCode = address.AddressPurposeCode;
        currRequestParams.AddressCode.IsPalestinianCity = address.IsPalestinianCity;
        currRequestParams.AddressCode.IsHebrewAddress = address.IsHebrewAddress;
        currRequestParams.AddressCode.BranchName = address.BranchName;
        currRequestParams.AddressCode.ContactIdentifier = address.ContactIdentifier;
        currRequestParams.AddressCode.ContactFirstName = address.ContactFirstName;
        currRequestParams.AddressCode.ContactLastName = address.ContactLastName;
        currRequestParams.AddressCode.ContactRoleTypeCode = address.ContactRoleTypeCode;
        currRequestParams.AddressCode.AuthorizedSignerPermit1 = address.AuthorizedSignerPermit1;
        currRequestParams.AddressCode.AuthorizedSignerPermit2 = address.AuthorizedSignerPermit2;
        currRequestParams.AddressCode.AuthorizedSignerPermit3 = address.AuthorizedSignerPermit3;
        currRequestParams.AddressCode.LocalCityCode = address.LocalCityCode;
        currRequestParams.AddressCode.LocalSecondLine = address.LocalSecondLine;
        currRequestParams.AddressCode.LocalStreetName = address.LocalStreetName;
        currRequestParams.AddressCode.LocalHouseLetter = address.LocalHouseLetter;
        currRequestParams.AddressCode.LocalEntrance = address.LocalEntrance;
        currRequestParams.AddressCode.EnglishCountryCode = address.EnglishCountryCode;
        currRequestParams.AddressCode.EnglishSubCountryCode = address.EnglishSubCountryCode;
        currRequestParams.AddressCode.EnglishCityName = address.EnglishCityName;
        currRequestParams.AddressCode.EnglishMainAddressLine = address.EnglishMainAddressLine;
        currRequestParams.AddressCode.EnglishPostalCode = address.EnglishPostalCode;
        if (address.LocalApartment)
            currRequestParams.AddressCode.LocalApartment = address.LocalApartment.toString();
        currRequestParams.AddressCode.LocalPOBox = address.LocalPOBox;
        currRequestParams.AddressCode.LocalPostalCode = address.LocalPostalCode;
        currRequestParams.AddressCode.LocalHouseNumber = address.LocalHouseNumber;
        currRequestParams.AddressCode.CustomAddressCode = address.CustomAddressCode;
        currRequestParams.LoggingEnabled = true,
            currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = address.Tenant;
        currRequestParams.RequestName = "Add/Update/Delete Address Contact Request";
        currRequestParams.ResponseName = "Add/Update/Delete Address Contact Request";
        currRequestParams.ClientId = this.entityPM.Id;
        currRequestParams.ExternalId = this.entityPM.Code;
        currRequestParams.PassportNumber = this.entityPM.PassportNumber;
        currRequestParams.PassportTypeCode = this.entityPM.PassportTypeCode;
        currRequestParams.PassportCountryCode = this.entityPM.PassportCountryCode;
        currRequestParams.OperationType = this.operationType;
        currRequestParams.AddressCode.ClientsAddressCommunication = [];
        for (var _i = 0, _a = address.ClientsAddressCommTypes; _i < _a.length; _i++) {
            var communicationItem = _a[_i];
            var clientsAddressCommunication = new AddAddressContactForClientRequestParams_1.ClientsAddressCommunicationResult();
            clientsAddressCommunication.CommunicationAddress = communicationItem.CommunicationAddress;
            clientsAddressCommunication.CommunicationType = communicationItem.CommunicationTypeCode;
            clientsAddressCommunication.CommunicationTypeName = communicationItem.CommunicationTypeName;
            currRequestParams.AddressCode.ClientsAddressCommunication.push(clientsAddressCommunication);
        }
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת מסר הוספה/עדכון/מחיקה כתובת לקוח", true)
            .then(function (res) {
            _this.responseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            //this.ValidationErrorsList = [];
            //     this.ValidationErrorsList.push(err);
        });
        this.clientMessageService.PostUpdateDeleteClientAddressContactRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
            //     this.clientPM.AddClientAddress(this.entityPM);
        });
    };
    ClientAddressesTabComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.requestParams == null) {
            this.requestParams = new AddAddressContactForClientRequestParams_1.AddAddressContactForClientRequestParams();
        }
        if (this.responseData == null) {
            this.responseData = new INF_MSG_GenericResponseData_1.INF_MSG_GenericResponseData();
        }
    };
    ClientAddressesTabComponent.prototype.ChooseAddressButtonClicked = function (address) {
        this.CurrentSession.CloseCurrentWindow();
        this.Parent.SelectAddresseCompleted(address);
    };
    ClientAddressesTabComponent.prototype.OnRowDoubleClick = function (item) {
        if (this.Mode == "Claims") {
            this.CurrentSession.CloseCurrentWindow();
            this.Parent.SelectAddresseCompleted(item);
        }
    };
    ClientAddressesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClientAddressesTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ClientAddressesTabComponent);
    return ClientAddressesTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ClientAddressesTabComponent = ClientAddressesTabComponent;
//# sourceMappingURL=ClientAddressesTabComponent.js.map