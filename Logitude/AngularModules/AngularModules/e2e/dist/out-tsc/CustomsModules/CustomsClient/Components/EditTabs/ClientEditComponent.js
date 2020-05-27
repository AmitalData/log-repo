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
var LocationDirective_1 = require("../../../../Infrastructure/Utilities/LocationDirective");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ClientPMService_1 = require("../../../../Customs/Services/StandardPMs/ClientPMService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var CreateClientRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/CreateClientRequestParams");
var INF_MSG_GenericResponseData_1 = require("../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData");
var ClientMessagesService_1 = require("../../../../Customs/Services/WebServices/ClientMessagesService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ClientEditComponent = /** @class */ (function (_super) {
    __extends(ClientEditComponent, _super);
    function ClientEditComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.TabsItemsSource = [];
        _this.clientPMService = new ClientPMService_1.ClientPMService();
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.DataContext = _this;
        _this.clientMessageService = new ClientMessagesService_1.ClientMessagesService();
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Retries = 0;
        _this.isViewInited = false;
        _this.GENERAL = null;
        _this.ADDRESSES = null;
        _this.LICENSE = null;
        _this.COMMUNICATION = null;
        _this.EVENTS = null;
        _this.REQUESTSHEET = null;
        _this.entityArgs.EntityArgEventEmitter.subscribe(function (theMessage) {
            if (theMessage == "ReloadEntity") {
                _this.clientPMService.get(_this.CurrentEntity.Id).subscribe(function (response) {
                    var result = response.Result;
                    if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                        _this.CurrentEntity = result;
                        _this.entityArgs.EntityPM = _this.CurrentEntity;
                    }
                });
            }
        });
        return _this;
    }
    ClientEditComponent.prototype.SetWindowArgs = function (args) {
        this.CurrentEntity = args.CurrentEntity;
        this.isNewClient = args.isNewClient;
        this.isExternalId = args.IsExternalId;
        this.BuildTabs();
        this.RunComponent();
        this.entityArgs.EntityPM = this.CurrentEntity;
        this.entityArgs.ObjectTableName = "Customs.Client";
    };
    Object.defineProperty(ClientEditComponent.prototype, "SelectedTabCode", {
        get: function () { return this.selectedTabCode; },
        set: function (newValue) {
            if (this.selectedTabCode != newValue) {
                this.selectedTabCode = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClientEditComponent.prototype, "FacilitationTypeCode", {
        get: function () { return this.CurrentEntity.FacilitationTypeCode; },
        set: function (newValue) { this.CurrentEntity.FacilitationTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    ClientEditComponent.prototype.BuildTabs = function () {
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("GENERAL", "Customs.Declaration.TH.General"));
        this.TabsItemsSource.push(new TabItem("ADDRESSES", "General.O.Addresses"));
        this.TabsItemsSource.push(new TabItem("LICENSE", "General.O.DrivingLicense"));
        this.TabsItemsSource.push(new TabItem("COMMUNICATION", "General.O.Communications"));
        this.TabsItemsSource.push(new TabItem("EVENTS", "General.O.Events"));
        this.TabsItemsSource.push(new TabItem("REQUESTSHEET", "General.O.RequestSheets"));
        this.TabsItemsSource.push(new TabItem("MOREDATA", "Customs.Client.TH.MoreData"));
        this.selectedTabCode = "GENERAL";
    };
    ClientEditComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isViewInited = true;
                this.InitializeComponent();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    ClientEditComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    ClientEditComponent.prototype.InitializeComponent = function () {
        if (this.isViewInited) {
            this.SelectionChanged();
        }
    };
    ClientEditComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            var myLocation_1 = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedTabCode; })[0];
            if (myLocation_1 != null) {
                switch (this.SelectedTabCode) {
                    case "GENERAL": {
                        if (this.GENERAL == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClient/Components/EditTabs/General/ClientGeneralTabComponent', myLocation_1.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.GENERAL = cmpRef.instance;
                                _this.GENERAL.InitTab(_this.CurrentEntity, _this.isNewClient);
                            });
                        }
                        break;
                    }
                    case "ADDRESSES": {
                        if (this.ADDRESSES == null) {
                            this.entityResourceService.getEntityResourceByTableName("Address").subscribe(function (response) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClient/Components/EditTabs/Addresses/ClientAddressesTabComponent', myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.ADDRESSES = cmpRef.instance;
                                    _this.ADDRESSES.InitTab(_this.CurrentEntity, _this.isNewClient);
                                });
                            });
                        }
                        break;
                    }
                    case "LICENSE": {
                        if (this.LICENSE == null) {
                            this.entityResourceService.getEntityResourceByTableName("Customs.ClientDrivingLicense").subscribe(function (response) {
                                _this.entityResourceService.getEntityResourceByTableName("Customs.ClientDrivingLicenseType").subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsClient/Components/EditTabs/License/ClientDrivingLicenseTabComponent', myLocation_1.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.LICENSE = cmpRef.instance;
                                        _this.LICENSE.InitTab(_this.CurrentEntity, _this.isNewClient);
                                    });
                                });
                            });
                        }
                        break;
                    }
                    case "COMMUNICATION": {
                        if (this.COMMUNICATION == null) {
                            this.entityResourceService.getEntityResourceByTableName("CommunicationLog").subscribe(function (response) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationsTabComponent", myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.COMMUNICATION = cmpRef.instance;
                                    _this.COMMUNICATION.IsTitleHidden = false;
                                });
                            });
                        }
                        break;
                    }
                    case "EVENTS": {
                        if (this.EVENTS == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load("./Common/Components/Events/EventsTabComponent", myLocation_1.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.EVENTS = cmpRef.instance;
                                _this.EVENTS.IsTitleHidden = false;
                            });
                        }
                        break;
                    }
                    case "REQUESTSHEET": {
                        if (this.REQUESTSHEET == null) {
                            this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load("./CustomsModules/CustomsControls/Components/CustomsRequestsSheetsComponent", myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.REQUESTSHEET = cmpRef.instance;
                                    _this.REQUESTSHEET.IsTitleHidden = false;
                                    _this.REQUESTSHEET.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.TH.RequestSheet");
                                    _this.REQUESTSHEET.MyRequestOnly = false;
                                    _this.REQUESTSHEET.SetEntityArgs(_this.entityArgs);
                                });
                            });
                        }
                        break;
                    }
                }
            }
        }
    };
    ClientEditComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (this.isNewClient) {
            this.clientPMService.insert(this.CurrentEntity).subscribe(function (response) {
                var result = response.Result;
                _this.CurrentSession.CloseCurrentWindow();
            });
        }
        else {
            this.clientPMService.update(this.CurrentEntity).subscribe(function (response) {
                var result = response.Result;
                // this.CurrentSession.CurrentEditComponent.SaveChanges();
                _this.CurrentSession.CloseCurrentWindow();
            });
        }
    };
    ClientEditComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        //  alert(customSendOptionsArgs);
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.CurrentEntity, "Customs.Client", errors);
        for (var _i = 0, _a = this.CurrentEntity.ClientAddresses; _i < _a.length; _i++) {
            var item = _a[_i];
            Validator_1.Validator.TryValidateObject(item, "Customs.ClientAddress", errors);
        }
        if (this.CurrentEntity.ClientAddresses == null || (this.CurrentEntity.ClientAddresses != null && this.CurrentEntity.ClientAddresses.length == 0)) {
            errors.push("חובה להזין לפחות כתובת אחת ללקוח");
        }
        if (this.CurrentEntity.ClientDrivingLicenses != null && this.CurrentEntity.ClientDrivingLicenses.length > 0) {
            for (var _b = 0, _c = this.CurrentEntity.ClientDrivingLicenses; _b < _c.length; _b++) {
                var item = _c[_b];
                if (item.ClientDrivingLicenseTypes == null || (item.ClientDrivingLicenseTypes != null && item.ClientDrivingLicenseTypes.length == 0)) {
                    errors.push("חובה להזין לפחות סוג רישיון אחד לכל רישיון");
                }
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicator("");
            var currRequestParams = new CreateClientRequestParams_1.CreateClientRequestParams();
            currRequestParams.LoggingEnabled = true;
            currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
            currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
            currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
            currRequestParams.LoggingEntityId = this.CurrentEntity.Id;
            currRequestParams.LoggingObjectTableId = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Client'; })[0].Id;
            currRequestParams.LoggingEntityReference = this.CurrentEntity.Code;
            currRequestParams.IsFakeResponse = true;
            currRequestParams.RequestName = "Add Client Request";
            currRequestParams.ResponseName = "Add Client Response";
            currRequestParams.IsExternalId = this.isExternalId;
            currRequestParams.ClientTypeSpecificCode = this.CurrentEntity.ClientTypeSpecificCode;
            currRequestParams.IsActive = this.CurrentEntity.IsActive;
            currRequestParams.DunsNumber = this.CurrentEntity.DunsNumber;
            currRequestParams.EnglishBirthPlace = this.CurrentEntity.EnglishBirthPlace;
            currRequestParams.EnglishCorporationName = this.CurrentEntity.EnglishCorporationName;
            currRequestParams.EnglishFatherName = this.CurrentEntity.EnglishFatherName;
            currRequestParams.EnglishFirstName = this.CurrentEntity.EnglishFirstName;
            currRequestParams.EnglishLastName = this.CurrentEntity.EnglishLastName;
            currRequestParams.FullName = this.CurrentEntity.FullName;
            currRequestParams.GenderCode = this.CurrentEntity.GenderCode;
            currRequestParams.LocalCorporationName = this.CurrentEntity.LocalCorporationName;
            currRequestParams.LocalFirstName = this.CurrentEntity.LocalFirstName;
            currRequestParams.LocalLastName = this.CurrentEntity.LocalLastName;
            currRequestParams.PassportCountryCode = this.CurrentEntity.PassportCountryCode;
            currRequestParams.PassportExpirationDate = this.CurrentEntity.PassportExpirationDate;
            currRequestParams.PassportFirstName = this.CurrentEntity.PassportFirstName;
            currRequestParams.PassportIssueDate = this.CurrentEntity.PassportIssueDate;
            currRequestParams.PassportLastName = this.CurrentEntity.PassportLastName;
            currRequestParams.PassportNumber = this.CurrentEntity.PassportNumber;
            currRequestParams.PassportTypeCode = this.CurrentEntity.PassportTypeCode;
            currRequestParams.BirthDate = this.CurrentEntity.BirthDate;
            currRequestParams.IsImporter = this.CurrentEntity.IsImporter;
            currRequestParams.IsExporter = this.CurrentEntity.IsExporter;
            currRequestParams.ConcurrencyGUID = this.CurrentEntity.ConcurrencyGUID;
            currRequestParams.ClientAddresses = [];
            for (var _d = 0, _e = this.CurrentEntity.ClientAddresses; _d < _e.length; _d++) {
                var clientAddress = _e[_d];
                var addressParams = new CreateClientRequestParams_1.ClientAdressParams();
                addressParams.AddressPurposeCode = clientAddress.AddressPurposeCode;
                addressParams.AddressTypeCode = clientAddress.AddressTypeCode;
                addressParams.AuthorizedSignerPermit1 = clientAddress.AuthorizedSignerPermit1;
                addressParams.AuthorizedSignerPermit2 = clientAddress.AuthorizedSignerPermit2;
                addressParams.AuthorizedSignerPermit3 = clientAddress.AuthorizedSignerPermit3;
                addressParams.BranchName = clientAddress.BranchName;
                addressParams.ContactFirstName = clientAddress.ContactFirstName;
                addressParams.ContactIdentifier = clientAddress.ContactIdentifier;
                addressParams.ContactLastName = clientAddress.ContactLastName;
                addressParams.ContactRoleTypeCode = clientAddress.ContactRoleTypeCode;
                addressParams.ContactStateCode = clientAddress.ContactStateCode;
                addressParams.EnglishCityName = clientAddress.EnglishCityName;
                addressParams.EnglishCountryCode = clientAddress.EnglishCountryCode;
                addressParams.EnglishMainAddressLine = clientAddress.EnglishMainAddressLine;
                addressParams.EnglishPostalCode = clientAddress.EnglishPostalCode;
                addressParams.IsHebrewAddress = clientAddress.IsHebrewAddress;
                addressParams.LocalApartment = clientAddress.LocalApartment;
                addressParams.EnglishSubCountryCode = clientAddress.EnglishSubCountryCode;
                addressParams.IsPalestinianCity = clientAddress.IsPalestinianCity;
                addressParams.LocalCityCode = clientAddress.LocalCityCode;
                addressParams.LocalEntrance = clientAddress.LocalEntrance;
                addressParams.LocalHouseLetter = clientAddress.LocalHouseLetter;
                addressParams.LocalHouseNumber = clientAddress.LocalHouseNumber;
                addressParams.LocalPOBox = clientAddress.LocalPOBox;
                addressParams.LocalPostalCode = clientAddress.LocalPostalCode;
                addressParams.LocalSecondLine = clientAddress.LocalSecondLine;
                addressParams.LocalStreetName = clientAddress.LocalStreetName;
                addressParams.CustomAddressCode = clientAddress.CustomAddressCode;
                addressParams.AddressId = clientAddress.AddressId;
                addressParams.ClientAddressCommunicationType = [];
                for (var _f = 0, _g = clientAddress.ClientsAddressCommTypes; _f < _g.length; _f++) {
                    var communicationItem = _g[_f];
                    var clientsAddressCommunication = new CreateClientRequestParams_1.ClientAddressCommunicationType();
                    clientsAddressCommunication.CommunicationAddress = communicationItem.CommunicationAddress;
                    clientsAddressCommunication.CommunicationTypeCode = communicationItem.CommunicationTypeCode;
                    clientsAddressCommunication.CommunicationTypeName = communicationItem.CommunicationTypeName;
                    addressParams.ClientAddressCommunicationType.push(clientsAddressCommunication);
                }
                currRequestParams.ClientAddresses.push(addressParams);
            }
            currRequestParams.ClientDrivingLicenses = [];
            for (var _h = 0, _j = this.CurrentEntity.ClientDrivingLicenses; _h < _j.length; _h++) {
                var clientDrivingLicenseItem = _j[_h];
                var clientDrivingLicenseParams = new CreateClientRequestParams_1.ClientDrivingLicenseParams();
                clientDrivingLicenseParams.DrivingLicenseNumber = clientDrivingLicenseItem.DrivingLicenseNumber;
                clientDrivingLicenseParams.DriverLicenseValidityDate = clientDrivingLicenseItem.DriverLicenseValidityDate;
                clientDrivingLicenseParams.DrivingLicenseCountryID = clientDrivingLicenseItem.DrivingLicenseCountryID;
                clientDrivingLicenseParams.ClientDrivingLicenseTypes = [];
                for (var _k = 0, _l = clientDrivingLicenseItem.ClientDrivingLicenseTypes; _k < _l.length; _k++) {
                    var clientDrivingLicenseTypeItem = _l[_k];
                    var clientDrivingLicenseTypeParams = new CreateClientRequestParams_1.ClientDrivingLicenseTypeParams();
                    clientDrivingLicenseTypeParams.DriversLicenseTypeCode = clientDrivingLicenseTypeItem.DriversLicenseTypeCode;
                    clientDrivingLicenseParams.ClientDrivingLicenseTypes.push(clientDrivingLicenseTypeParams);
                }
                currRequestParams.ClientDrivingLicenses.push(clientDrivingLicenseParams);
            }
            CustomMessageProgressComponent_1.CustomMessageProgressComponent
                .ShowProgressBar(currRequestParams.PBId, "שליחת מסר הקמת ספק", false)
                .then(function (res) {
                _this.responseData = res;
                _this.OnMassageDisplayMethod();
            }).catch(function (err) {
                //this.ValidationErrorsList = [];
                //     this.ValidationErrorsList.push(err);
            });
            this.clientMessageService.CreateClientRequest(currRequestParams)
                .subscribe(function (myServiceResponse) {
            });
        }
    };
    ClientEditComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.requestParams == null) {
            this.requestParams = new CreateClientRequestParams_1.CreateClientRequestParams();
        }
        if (this.responseData == null) {
            this.responseData = new INF_MSG_GenericResponseData_1.INF_MSG_GenericResponseData();
        }
    };
    ClientEditComponent.prototype.CancelButtonClicked = function () {
        this.CurrentEntity.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], ClientEditComponent.prototype, "AllLocations", void 0);
    ClientEditComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClientEditComponent.html',
            providers: [EntityArgs_1.EntityArgs],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ClientEditComponent);
    return ClientEditComponent;
}(BaseComponent_1.BaseComponent));
exports.ClientEditComponent = ClientEditComponent;
var TabItem = /** @class */ (function () {
    function TabItem(Code, TextCode) {
        this.code = Code;
        this.textCode = TextCode;
    }
    return TabItem;
}());
//# sourceMappingURL=ClientEditComponent.js.map