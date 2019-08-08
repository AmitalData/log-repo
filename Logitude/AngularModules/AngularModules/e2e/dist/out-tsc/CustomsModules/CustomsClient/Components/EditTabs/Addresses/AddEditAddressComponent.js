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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ClientsAddressCommTypePM_1 = require("../../../../../Customs/EntityPMs/ClientsAddressCommTypePM");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var ClientPMService_1 = require("../../../../../Customs/Services/StandardPMs/ClientPMService");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var AddAddressContactForClientRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/AddAddressContactForClientRequestParams");
var CustomMessageProgressComponent_1 = require("../../../../CustomsControls/Components/CustomMessageProgressComponent");
var INF_MSG_GenericResponseData_1 = require("../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData");
var ClientMessagesService_1 = require("../../../../../Customs/Services/WebServices/ClientMessagesService");
var AddEditAddressComponent = /** @class */ (function (_super) {
    __extends(AddEditAddressComponent, _super);
    function AddEditAddressComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customs.ClientAddress";
        _this.DataContext = _this;
        _this.LayoutDirection = 'ltr';
        _this.clientPMService = new ClientPMService_1.ClientPMService();
        _this.clientMessagesService = new ClientMessagesService_1.ClientMessagesService();
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.TabsSource = [];
        _this.SelectedTab = ";";
        _this.isHebrewSelected = false;
        _this.isEnglishSelected = false;
        return _this;
    }
    AddEditAddressComponent.prototype.SetWindowArgs = function (args) {
        this.entityPM = args.clientAddressPM;
        this.clientPM = args.clientPM;
        this.isNew = args.IsNew;
        this.isNewClient = args.isNewClient;
        this.BuildTabs();
        this.CommunicationsList = new ObservableCollection_1.ObservableCollection([]);
        this.BuildCommunicationsList();
    };
    AddEditAddressComponent.prototype.BuildTabs = function () {
        if (this.IsHebrewAddress || this.isNew) {
            this.SelectedTab = "Hebrew";
            this.isHebrewSelected = true;
        }
        else {
            this.SelectedTab = "English";
            this.isEnglishSelected = true;
        }
        //this.SelectedTab = "Hebrew";
        this.TabsSource.push({ Name: "Hebrew", isSelected: this.isHebrewSelected, Header: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Client.O.Hebrew") });
        this.TabsSource.push({ Name: "English", isSelected: this.isEnglishSelected, Header: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Client.O.English") });
    };
    AddEditAddressComponent.prototype.SelectionChanged = function (tab) {
        this.TabsSource.forEach(function (item) {
            item.isSelected = false;
        });
        var index = this.TabsSource.indexOf(tab);
        if (index < 0) {
            console.log("The tab was not found, cant not delete it :( ", tab);
            return;
        }
        var item = this.TabsSource[index];
        item.isSelected = true;
        this.SelectedTab = item.Name;
    };
    AddEditAddressComponent.prototype.AddButonClicked = function () {
        var newCommunicationPM = new ClientsAddressCommTypePM_1.ClientsAddressCommTypePM(this.clientPM);
        newCommunicationPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newCommunicationPM.ClientId = this.clientPM.Id;
        if (!this.entityPM.ClientsAddressCommTypes.includes(newCommunicationPM)) {
            this.entityPM.AddClientsAddressCommType(newCommunicationPM);
            this.CommunicationsList.Insert(new CommunicationItemModel(newCommunicationPM, this));
        }
    };
    AddEditAddressComponent.prototype.BuildCommunicationsList = function () {
        for (var _i = 0, _a = this.entityPM.ClientsAddressCommTypes; _i < _a.length; _i++) {
            var item = _a[_i];
            this.CommunicationsList.Insert(new CommunicationItemModel(item, this));
        }
    };
    AddEditAddressComponent.prototype.RemoveRow = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.DeleteCommunication"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.CommunicationsList.Remove(item);
                    _this.entityPM.RemoveClientsAddressCommType(item.CommunicationPM);
                }
            });
        }
    };
    Object.defineProperty(AddEditAddressComponent.prototype, "BranchName", {
        //#region Properties
        get: function () { return this.entityPM.BranchName; },
        set: function (newValue) { this.entityPM.BranchName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "AddressTypeCode", {
        get: function () { return this.entityPM.AddressTypeCode; },
        set: function (newValue) { this.entityPM.AddressTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "AddressTypeName", {
        get: function () { return this.entityPM.AddressTypeName; },
        set: function (newValue) { this.entityPM.AddressTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "LocalCityCode", {
        get: function () { return this.entityPM.LocalCityCode; },
        set: function (newValue) { this.entityPM.LocalCityCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "LocalCityName", {
        get: function () { return this.entityPM.LocalCityName; },
        set: function (newValue) { this.entityPM.LocalCityName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "LocalStreetName", {
        get: function () { return this.entityPM.LocalStreetName; },
        set: function (newValue) { this.entityPM.LocalStreetName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "LocalHouseNumber", {
        get: function () { return this.entityPM.LocalHouseNumber; },
        set: function (newValue) { this.entityPM.LocalHouseNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "LocalPOBox", {
        get: function () { return this.entityPM.LocalPOBox; },
        set: function (newValue) { this.entityPM.LocalPOBox = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "AddressPurposeCode", {
        get: function () { return this.entityPM.AddressPurposeCode; },
        set: function (newValue) { this.entityPM.AddressPurposeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "LocalSecondLine", {
        get: function () { return this.entityPM.LocalSecondLine; },
        set: function (newValue) { this.entityPM.LocalSecondLine = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "LocalHouseLetter", {
        get: function () { return this.entityPM.LocalHouseLetter; },
        set: function (newValue) { this.entityPM.LocalHouseLetter = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "LocalPostalCode", {
        get: function () { return this.entityPM.LocalPostalCode; },
        set: function (newValue) { this.entityPM.LocalPostalCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "EnglishCountryCode", {
        get: function () { return this.entityPM.EnglishCountryCode; },
        set: function (newValue) { this.entityPM.EnglishCountryCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "EnglishCityName", {
        get: function () { return this.entityPM.EnglishCityName; },
        set: function (newValue) { this.entityPM.EnglishCityName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "EnglishPostalCode", {
        get: function () { return this.entityPM.EnglishPostalCode; },
        set: function (newValue) { this.entityPM.EnglishPostalCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "EnglishSubCountryCode", {
        get: function () { return this.entityPM.EnglishSubCountryCode; },
        set: function (newValue) { this.entityPM.EnglishSubCountryCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "EnglishMainAddressLine", {
        get: function () { return this.entityPM.EnglishMainAddressLine; },
        set: function (newValue) { this.entityPM.EnglishMainAddressLine = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "ContactIdentifier", {
        get: function () { return this.entityPM.ContactIdentifier; },
        set: function (newValue) { this.entityPM.ContactIdentifier = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "ContactFirstName", {
        get: function () { return this.entityPM.ContactFirstName; },
        set: function (newValue) { this.entityPM.ContactFirstName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "AuthorizedSignerPermit1", {
        get: function () { return this.entityPM.AuthorizedSignerPermit1; },
        set: function (newValue) { this.entityPM.AuthorizedSignerPermit1 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "AuthorizedSignerPermit2", {
        get: function () { return this.entityPM.AuthorizedSignerPermit2; },
        set: function (newValue) { this.entityPM.AuthorizedSignerPermit2 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "AuthorizedSignerPermit3", {
        get: function () { return this.entityPM.AuthorizedSignerPermit3; },
        set: function (newValue) { this.entityPM.AuthorizedSignerPermit3 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "ContactRoleTypeCode", {
        get: function () { return this.entityPM.ContactRoleTypeCode; },
        set: function (newValue) { this.entityPM.ContactRoleTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "ContactLastName", {
        get: function () { return this.entityPM.ContactLastName; },
        set: function (newValue) { this.entityPM.ContactLastName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditAddressComponent.prototype, "IsHebrewAddress", {
        //public get IsHebrewAddress() { return this.entityPM.IsHebrewAddress; }
        //public set IsHebrewAddress(newValue: boolean) { this.entityPM.IsHebrewAddress = newValue; }
        //public get IsPalestinianCity() { return this.entityPM.IsPalestinianCity; }
        //public set IsPalestinianCity(newValue: boolean) { this.entityPM.IsPalestinianCity = newValue; }
        //#endregion
        get: function () { return this.entityPM.IsHebrewAddress; },
        set: function (newValue) {
            if (this.entityPM.IsHebrewAddress != newValue) {
                this.entityPM.IsHebrewAddress = newValue;
                if (newValue) {
                    this.IsPalestinianCity = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditAddressComponent.prototype.UseHebrewAddress = function (newValue) {
        this.IsHebrewAddress = newValue;
    };
    Object.defineProperty(AddEditAddressComponent.prototype, "IsPalestinianCity", {
        get: function () { return this.entityPM.IsPalestinianCity; },
        set: function (newValue) {
            if (this.entityPM.IsPalestinianCity != newValue) {
                this.entityPM.IsPalestinianCity = newValue;
                if (newValue) {
                    this.IsHebrewAddress = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditAddressComponent.prototype.UsePalestinianAddress = function (newValue) {
        this.IsPalestinianCity = newValue;
    };
    AddEditAddressComponent.prototype.CancelButtonClicked = function () {
        this.entityPM.RejectChanges();
        //this.CurrentSession.CloseCurrentWindow();
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    AddEditAddressComponent.prototype.OkButtonClicked = function () {
        this.clientPM.AddClientAddress(this.entityPM);
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditAddressComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        //  alert(customSendOptionsArgs);
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
        for (var _i = 0, _a = this.entityPM.ClientsAddressCommTypes; _i < _a.length; _i++) {
            var item = _a[_i];
            Validator_1.Validator.TryValidateObject(item, this.ObjectTableName, errors);
        }
        // this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.operationType = AddAddressContactForClientRequestParams_1.OperationTypes.Update;
            if (this.isNew) {
                //  trigger.addressesList.Add(this);
                this.operationType = AddAddressContactForClientRequestParams_1.OperationTypes.Add;
            }
            if (!this.isNewClient) {
                this.CurrentSession.StartBusyIndicator("");
                var currRequestParams = new AddAddressContactForClientRequestParams_1.AddAddressContactForClientRequestParams();
                currRequestParams.LoggingEnabled = true;
                currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
                currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
                currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
                currRequestParams.AddressCode = new AddAddressContactForClientRequestParams_1.ClientAddress();
                currRequestParams.AddressCode.AddressId = this.entityPM.AddressId;
                currRequestParams.AddressCode.AddressContactState = this.entityPM.ContactStateCode;
                currRequestParams.AddressCode.AddressTypeCode = this.entityPM.AddressTypeCode;
                currRequestParams.AddressCode.AddressPurposeCode = this.entityPM.AddressPurposeCode;
                currRequestParams.AddressCode.IsPalestinianCity = this.entityPM.IsPalestinianCity;
                currRequestParams.AddressCode.IsHebrewAddress = this.entityPM.IsHebrewAddress;
                currRequestParams.AddressCode.BranchName = this.entityPM.BranchName;
                currRequestParams.AddressCode.ContactIdentifier = this.entityPM.ContactIdentifier;
                currRequestParams.AddressCode.ContactFirstName = this.entityPM.ContactFirstName;
                currRequestParams.AddressCode.ContactLastName = this.entityPM.ContactLastName;
                currRequestParams.AddressCode.ContactRoleTypeCode = this.entityPM.ContactRoleTypeCode;
                currRequestParams.AddressCode.AuthorizedSignerPermit1 = this.entityPM.AuthorizedSignerPermit1;
                currRequestParams.AddressCode.AuthorizedSignerPermit2 = this.entityPM.AuthorizedSignerPermit2;
                currRequestParams.AddressCode.AuthorizedSignerPermit3 = this.entityPM.AuthorizedSignerPermit3;
                currRequestParams.AddressCode.LocalCityCode = this.entityPM.LocalCityCode;
                currRequestParams.AddressCode.LocalSecondLine = this.entityPM.LocalSecondLine;
                currRequestParams.AddressCode.LocalStreetName = this.entityPM.LocalStreetName;
                currRequestParams.AddressCode.LocalHouseLetter = this.entityPM.LocalHouseLetter;
                currRequestParams.AddressCode.LocalEntrance = this.entityPM.LocalEntrance;
                currRequestParams.AddressCode.EnglishCountryCode = this.entityPM.EnglishCountryCode;
                currRequestParams.AddressCode.EnglishSubCountryCode = this.entityPM.EnglishSubCountryCode;
                currRequestParams.AddressCode.EnglishCityName = this.entityPM.EnglishCityName;
                currRequestParams.AddressCode.EnglishMainAddressLine = this.entityPM.EnglishMainAddressLine;
                currRequestParams.AddressCode.EnglishPostalCode = this.entityPM.EnglishPostalCode;
                if (this.entityPM.LocalApartment)
                    currRequestParams.AddressCode.LocalApartment = this.entityPM.LocalApartment.toString();
                currRequestParams.AddressCode.LocalPOBox = this.entityPM.LocalPOBox;
                currRequestParams.AddressCode.LocalPostalCode = this.entityPM.LocalPostalCode;
                currRequestParams.AddressCode.LocalHouseNumber = this.entityPM.LocalHouseNumber;
                currRequestParams.AddressCode.CustomAddressCode = this.entityPM.CustomAddressCode;
                currRequestParams.LoggingEnabled = true,
                    currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                currRequestParams.Tenant = this.entityPM.Tenant;
                currRequestParams.RequestName = "Add/Update/Delete Address Contact Request";
                currRequestParams.ResponseName = "Add/Update/Delete Address Contact Request";
                currRequestParams.ClientId = this.clientPM.Id;
                currRequestParams.ExternalId = this.clientPM.Code;
                currRequestParams.PassportNumber = this.clientPM.PassportNumber;
                currRequestParams.PassportTypeCode = this.clientPM.PassportTypeCode;
                currRequestParams.PassportCountryCode = this.clientPM.PassportCountryCode;
                currRequestParams.OperationType = this.operationType;
                currRequestParams.AddressCode.ClientsAddressCommunication = [];
                for (var _b = 0, _c = this.entityPM.ClientsAddressCommTypes; _b < _c.length; _b++) {
                    var communicationItem = _c[_b];
                    var clientsAddressCommunication = new AddAddressContactForClientRequestParams_1.ClientsAddressCommunicationResult();
                    clientsAddressCommunication.CommunicationAddress = communicationItem.CommunicationAddress;
                    clientsAddressCommunication.CommunicationType = communicationItem.CommunicationTypeCode;
                    clientsAddressCommunication.CommunicationTypeName = communicationItem.CommunicationTypeName;
                    currRequestParams.AddressCode.ClientsAddressCommunication.push(clientsAddressCommunication);
                }
                CustomMessageProgressComponent_1.CustomMessageProgressComponent
                    .ShowProgressBar(currRequestParams.PBId, "שליחת מסר הוספה/עדכון/מחיקה כתובת לקוח", false)
                    .then(function (res) {
                    _this.responseData = res;
                    _this.OnMassageDisplayMethod();
                }).catch(function (err) {
                    //this.ValidationErrorsList = [];
                    //     this.ValidationErrorsList.push(err);
                });
                this.clientMessagesService.PostUpdateDeleteClientAddressContactRequest(currRequestParams)
                    .subscribe(function (myServiceResponse) {
                    if (_this.isNew) {
                        _this.clientPM.AddClientAddress(_this.entityPM);
                    }
                    if (myServiceResponse.Result.Succeeded == true && myServiceResponse.Result.HasException == false) {
                        _this.CurrentSession.CloseCurrentWindowEmit("ReloadEntity");
                    }
                    else {
                        _this.CurrentSession.CloseCurrentWindow();
                    }
                });
            }
            //   this.isNew = false;
        }
    };
    AddEditAddressComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.requestParams == null) {
            this.requestParams = new AddAddressContactForClientRequestParams_1.AddAddressContactForClientRequestParams();
        }
        if (this.responseData == null) {
            this.responseData = new INF_MSG_GenericResponseData_1.INF_MSG_GenericResponseData();
        }
    };
    AddEditAddressComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditAddressComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditAddressComponent);
    return AddEditAddressComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditAddressComponent = AddEditAddressComponent;
var CommunicationItemModel = /** @class */ (function (_super) {
    __extends(CommunicationItemModel, _super);
    function CommunicationItemModel(communicationPM, parent) {
        var _this = _super.call(this) || this;
        _this.communicationPM = communicationPM;
        _this.CommunicationPM = null;
        _this.ObjectTableName = "Customs.ClientsAddressCommType";
        _this.DataContext = _this;
        _this.CommunicationPM = communicationPM;
        _this.Parent = parent;
        return _this;
    }
    Object.defineProperty(CommunicationItemModel.prototype, "CommunicationAddress", {
        //#region Properties
        get: function () { return this.CommunicationPM.CommunicationAddress; },
        set: function (value) {
            if (this.CommunicationPM.CommunicationAddress != value) {
                this.CommunicationPM.CommunicationAddress = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommunicationItemModel.prototype, "CommunicationTypeCode", {
        get: function () { return this.CommunicationPM.CommunicationTypeCode; },
        set: function (value) {
            if (this.CommunicationPM.CommunicationTypeCode != value) {
                this.CommunicationPM.CommunicationTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommunicationItemModel.prototype, "CommunicationTypeName", {
        get: function () { return this.CommunicationPM.CommunicationTypeName; },
        set: function (value) {
            if (this.CommunicationPM.CommunicationTypeName != value) {
                this.CommunicationPM.CommunicationTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    CommunicationItemModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return CommunicationItemModel;
}(BaseComponent_1.BaseComponent));
exports.CommunicationItemModel = CommunicationItemModel;
//# sourceMappingURL=AddEditAddressComponent.js.map