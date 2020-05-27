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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ClaimPM_1 = require("../../../../../Customs/EntityPMs/ClaimPM");
var ClientAddressPM_1 = require("../../../../../Customs/EntityPMs/ClientAddressPM");
var ClaimsRelatedEntityPM_1 = require("../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM");
var ClientPM_1 = require("../../../../../Customs/EntityPMs/ClientPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var ClientMessagesService_1 = require("../../../../../Customs/Services/WebServices/ClientMessagesService");
var ClaimPMService_1 = require("../../../../../Customs/Services/StandardPMs/ClaimPMService");
var ClientPMService_1 = require("../../../../../Customs/Services/StandardPMs/ClientPMService");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var ClaimGeneralTabComponent = /** @class */ (function (_super) {
    __extends(ClaimGeneralTabComponent, _super);
    function ClaimGeneralTabComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.EntityPM = new ClaimPM_1.ClaimPM();
        _this.ObjectTableName = "Customs.Claim";
        _this.AgentAddressesList = [];
        _this.ClienAddressesList = [];
        _this.ContactAddressesList = [];
        _this.isControlEnabled = true;
        _this.ClaimPMService = new ClaimPMService_1.ClaimPMService;
        _this.ClientMessagesService = new ClientMessagesService_1.ClientMessagesService;
        _this.ClientPMService = new ClientPMService_1.ClientPMService;
        _this.IsLoaded = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CustomerDependencyProperty1 = "CS";
        _this.ClaimsRelatedEntitiesObslist = new ObservableCollection_1.ObservableCollection([]);
        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        _this.CurrentSession.StartBusyIndicator("");
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntity").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderMethod").subscribe(function (response) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (_this.entityArgs.EntityPM != null) {
                        _this.EntityPM = _this.entityArgs.EntityPM;
                        _this.BuildRelatedEntitiesList();
                        _this.BuildAddressesList();
                    }
                    _this.Listen();
                    _this.IsLoaded = true;
                });
            });
        });
        return _this;
    }
    ClaimGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildRelatedEntitiesList();
                    //this.BuildAddressesList();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "CLMG") {
                        //this.RefreshEntity();
                    }
                }
            }));
        }
    };
    ClaimGeneralTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    ClaimGeneralTabComponent.prototype.BuildRelatedEntitiesList = function () {
        this.ClaimsRelatedEntitiesObslist = new ObservableCollection_1.ObservableCollection([]);
        if (this.EntityPM.ClaimsRelatedEntities != null && this.EntityPM.ClaimsRelatedEntities.length > 0) {
            this.EntityPM.ClaimsRelatedEntities.sort(function (a, b) { return (a.EntityCounterKey === b.EntityCounterKey) ? 0 : (a.EntityCounterKey < b.EntityCounterKey) ? -1 : 1; });
            for (var _i = 0, _a = this.EntityPM.ClaimsRelatedEntities; _i < _a.length; _i++) {
                var item = _a[_i];
                this.ClaimsRelatedEntitiesObslist.Insert(new ClaimsRelatedEntityLineComponent(item, this.EntityPM, false));
            }
        }
    };
    ClaimGeneralTabComponent.prototype.BuildAddressesList = function () {
        //AddressCode
        this.BuildAgentAddressesList();
        //CustomsAddressCode & ContactPhoneAddressCode
        this.BuildClienAddressesList();
    };
    ClaimGeneralTabComponent.prototype.BuildAgentAddressesList = function () {
        var _this = this;
        this.AgentAddressesList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ClaimSubmiterNumber)) {
            this.ClientMessagesService.GetSingleClientPMByCode(this.EntityPM.ClaimSubmiterNumber, true)
                .subscribe(function (myResponse) {
                _this.GetSingleCustomerOp_Completed(myResponse, false);
            });
        }
        else {
            var clientPM = new ClientPM_1.ClientPM();
            var clientAddressPM = new ClientAddressPM_1.ClientAddressPM(clientPM);
            var addressItemViewModel = new AddressItemComponent(clientAddressPM, clientPM, false, "AddressCode");
            this.AgentAddressesList.push(addressItemViewModel);
        }
    };
    ClaimGeneralTabComponent.prototype.BuildClienAddressesList = function () {
        var _this = this;
        this.ClienAddressesList = [];
        this.ContactAddressesList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ClientId)) {
            this.ClientPMService.get(this.EntityPM.ClientId)
                .subscribe(function (myResponse) {
                _this.GetSingleClientOp_Completed(myResponse, false);
            });
        }
        else {
            var clientPM = new ClientPM_1.ClientPM();
            var clientAddressPM = new ClientAddressPM_1.ClientAddressPM(clientPM);
            var addressItemViewModelClient = new AddressItemComponent(clientAddressPM, clientPM, false, "CustomsAddressCode");
            this.ClienAddressesList.push(addressItemViewModelClient);
            var addressItemViewModelContact = new AddressItemComponent(clientAddressPM, clientPM, false, "ContactPhoneAddressCode");
            this.ContactAddressesList.push(addressItemViewModelContact);
        }
    };
    ClaimGeneralTabComponent.prototype.GetSingleCustomerOp_Completed = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        if (myResponse.Result != null) {
            var clientPM = myResponse.Result;
            var clientAddressPM = new ClientAddressPM_1.ClientAddressPM(clientPM);
            if (!Tools_1.AppTool.IsNullOrEmpty(this.AddressCode) && (clientPM.ClientAddresses != null && clientPM.ClientAddresses.length > 0)) {
                clientAddressPM = clientPM.ClientAddresses.filter(function (d) { return d.CustomAddressCode == _this.AddressCode; })[0];
                if (clientAddressPM == null) {
                    clientAddressPM = new ClientAddressPM_1.ClientAddressPM(clientPM);
                }
            }
            var addressItemViewModel = new AddressItemComponent(clientAddressPM, clientPM, false, "AddressCode");
            this.AgentAddressesList.push(addressItemViewModel);
        }
        else {
            var clientPM = new ClientPM_1.ClientPM();
            var clientAddressPM = new ClientAddressPM_1.ClientAddressPM(clientPM);
            var addressItemViewModel = new AddressItemComponent(clientAddressPM, clientPM, false, "AddressCode");
            this.AgentAddressesList.push(addressItemViewModel);
        }
    };
    ClaimGeneralTabComponent.prototype.GetSingleClientOp_Completed = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        if (myResponse.Result != null) {
            var clientPM = myResponse.Result;
            //Get CustomsAddressCode
            var clientCustomsAddressPM = new ClientAddressPM_1.ClientAddressPM(clientPM);
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomsAddressCode) && (clientPM.ClientAddresses != null && clientPM.ClientAddresses.length > 0)) {
                clientCustomsAddressPM = clientPM.ClientAddresses.filter(function (d) { return d.CustomAddressCode == _this.CustomsAddressCode; })[0];
                if (clientCustomsAddressPM == null) {
                    clientCustomsAddressPM = new ClientAddressPM_1.ClientAddressPM(clientPM);
                }
            }
            var customsAddressItemViewModel = new AddressItemComponent(clientCustomsAddressPM, clientPM, false, "CustomsAddressCode");
            this.ClienAddressesList.push(customsAddressItemViewModel);
            //Get ContactPhoneAddressCode
            var clientAddressPM = new ClientAddressPM_1.ClientAddressPM(clientPM);
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ContactPhoneAddressCode) && (clientPM.ClientAddresses != null && clientPM.ClientAddresses.length > 0)) {
                clientAddressPM = clientPM.ClientAddresses.filter(function (d) { return d.CustomAddressCode == _this.ContactPhoneAddressCode; })[0];
                if (clientAddressPM == null) {
                    clientAddressPM = new ClientAddressPM_1.ClientAddressPM(clientPM);
                }
            }
            var addressItemViewModel = new AddressItemComponent(clientAddressPM, clientPM, false, "ContactPhoneAddressCode");
            this.ContactAddressesList.push(addressItemViewModel);
        }
    };
    Object.defineProperty(ClaimGeneralTabComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    ClaimGeneralTabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    };
    Object.defineProperty(ClaimGeneralTabComponent.prototype, "IsControlEnabled", {
        get: function () { return this.isControlEnabled; },
        set: function (newValue) { this.isControlEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimGeneralTabComponent.prototype, "SubmitDate", {
        get: function () { return this.EntityPM.SubmitDate; },
        set: function (newValue) { this.EntityPM.SubmitDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimGeneralTabComponent.prototype, "ReferantId", {
        get: function () { return this.EntityPM.ReferantId; },
        set: function (newValue) { this.EntityPM.ReferantId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimGeneralTabComponent.prototype, "ClientId", {
        get: function () { return this.EntityPM.ClientId; },
        set: function (newValue) {
            if (this.EntityPM.ClientId != newValue) {
                this.EntityPM.ClientId = newValue;
                this.ClientIdSelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimGeneralTabComponent.prototype, "SoldierPersonalNumber", {
        get: function () { return this.EntityPM.SoldierPersonalNumber; },
        set: function (newValue) { this.EntityPM.SoldierPersonalNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimGeneralTabComponent.prototype, "CustomerId", {
        get: function () { return this.EntityPM.CustomerId; },
        set: function (newValue) { this.EntityPM.CustomerId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimGeneralTabComponent.prototype, "AddressCode", {
        get: function () { return this.EntityPM.AddressCode; },
        set: function (newValue) { this.EntityPM.AddressCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimGeneralTabComponent.prototype, "CustomsAddressCode", {
        get: function () { return this.EntityPM.CustomsAddressCode; },
        set: function (newValue) { this.EntityPM.CustomsAddressCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimGeneralTabComponent.prototype, "ContactPhoneAddressCode", {
        get: function () { return this.EntityPM.ContactPhoneAddressCode; },
        set: function (newValue) { this.EntityPM.ContactPhoneAddressCode = newValue; },
        enumerable: true,
        configurable: true
    });
    //#region Address
    ClaimGeneralTabComponent.prototype.ClientIdSelectionChanged = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ContactPhoneAddressCode) && Tools_1.AppTool.IsNullOrEmpty(this.CustomsAddressCode)) {
            this.BuildClienAddressesList();
            return;
        }
        var clientSelectionChangedWindow = new ConfirmWindow_1.ConfirmWindow();
        clientSelectionChangedWindow.Width = 250;
        clientSelectionChangedWindow.Height = 150;
        clientSelectionChangedWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
        clientSelectionChangedWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.Cancel");
        clientSelectionChangedWindow.ShowCancelButton = false;
        clientSelectionChangedWindow.Show("בפעולה זו ימחקו כל הכתובות של הלקוח, האם להמשיך?");
        clientSelectionChangedWindow.WindowClosed.subscribe(function (event) {
            if (clientSelectionChangedWindow.Yes) {
                _this.DeleteClientAddressLines();
            }
            else if (clientSelectionChangedWindow.No) {
                var oldClientId = "";
                if (_this.ClienAddressesList.length == 1) {
                    oldClientId = _this.ClienAddressesList[0].ClientPM.Id;
                }
                else if (_this.ContactAddressesList.length == 1) {
                    oldClientId = _this.ContactAddressesList[0].ClientPM.Id;
                }
                _this.ClientId = oldClientId;
            }
        });
    };
    ClaimGeneralTabComponent.prototype.DeleteClientAddressLines = function () {
        this.CustomsAddressCode = null;
        this.ContactPhoneAddressCode = null;
        this.BuildClienAddressesList();
    };
    ClaimGeneralTabComponent.prototype.DeleteAddress = function (item) {
        if ((item.AddressMode == "AddressCode" && Tools_1.AppTool.IsNullOrEmpty(this.AddressCode))
            || (item.AddressMode == "CustomsAddressCode" && Tools_1.AppTool.IsNullOrEmpty(this.CustomsAddressCode))
            || (item.AddressMode == "ContactPhoneAddressCode" && Tools_1.AppTool.IsNullOrEmpty(this.ContactPhoneAddressCode))) {
            return;
        }
        var clientAddressPM = new ClientAddressPM_1.ClientAddressPM(item.ClientPM);
        switch (item.AddressMode) {
            case "AddressCode":
                this.EntityPM.AddressCode = null;
                this.AgentAddressesList = [];
                var addressItemViewModel = new AddressItemComponent(clientAddressPM, item.ClientPM, false, "AddressCode");
                this.AgentAddressesList.push(addressItemViewModel);
                break;
            case "CustomsAddressCode":
                this.EntityPM.CustomsAddressCode = null;
                this.ClienAddressesList = [];
                var addressItemViewModelClient = new AddressItemComponent(clientAddressPM, item.ClientPM, false, "CustomsAddressCode");
                this.ClienAddressesList.push(addressItemViewModelClient);
                break;
            case "ContactPhoneAddressCode":
                this.EntityPM.ContactPhoneAddressCode = null;
                this.ContactAddressesList = [];
                var addressItemViewModelContact = new AddressItemComponent(clientAddressPM, item.ClientPM, false, "ContactPhoneAddressCode");
                this.ContactAddressesList.push(addressItemViewModelContact);
                break;
            default:
                break;
        }
    };
    ClaimGeneralTabComponent.prototype.SearchAddress = function (item) {
        var _this = this;
        this.CurrentSearchAddressMode = "";
        if ((item.AddressMode == "AddressCode" && Tools_1.AppTool.IsNullOrEmpty(item.ClientPM.Id)
            || (item.AddressMode == "CustomsAddressCode" || item.AddressMode == "ContactPhoneAddressCode") && Tools_1.AppTool.IsNullOrEmpty(item.ClientPM.Id))) {
            var text = "ראשית חובה לבחור לקוח";
            if (item.AddressMode == "AddressCode") {
                text = "ראשית חובה לבחור מגיש לתביעה";
            }
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
            messageWindow.Width = 250;
            messageWindow.Height = 150;
            messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show(text);
            return;
        }
        this.CurrentSearchAddressMode = item.AddressMode;
        this.EntityResourceService.getEntityResourceByTableName("Customs.ClientAddress").subscribe(function (response) {
            var windowArgs = {};
            windowArgs.EntityPM = item.ClientPM;
            windowArgs.Parent = _this;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 850;
            logWindow.Height = 500;
            logWindow.ShowCloseButton = true;
            logWindow.WindowArgs = windowArgs;
            logWindow.Title = "כתובות לקוח " + item.ClientPM.Code;
            logWindow.Show('./CustomsModules/CustomsClient/Components/EditTabs/Addresses/ClientAddressesTabComponent');
        });
    };
    ClaimGeneralTabComponent.prototype.SelectAddresseCompleted = function (selectedAddresse) {
        if (selectedAddresse == null) {
            return;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(selectedAddresse.CustomAddressCode)) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
            messageWindow.Width = 250;
            messageWindow.Height = 150;
            messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show("לא ניתן לבחור כתובת זו, אנא שלוף לקוח מחדש");
            return;
        }
        switch (this.CurrentSearchAddressMode) {
            case "AddressCode":
                this.AddressCode = selectedAddresse.CustomAddressCode;
                this.BuildAgentAddressesList();
                break;
            case "CustomsAddressCode":
                this.CustomsAddressCode = selectedAddresse.CustomAddressCode;
                this.BuildClienAddressesList();
                break;
            case "ContactPhoneAddressCode":
                this.ContactPhoneAddressCode = selectedAddresse.CustomAddressCode;
                this.BuildClienAddressesList();
                break;
            default:
                break;
        }
        this.CurrentSearchAddressMode = "";
    };
    //#endregion
    //#region Related Entities
    ClaimGeneralTabComponent.prototype.EditButtonClicked = function (item) {
        var _this = this;
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        this.CurrentSession.StartBusyIndicator("");
        this.ClaimPMService.update(this.EntityPM).subscribe(function (response) {
            var claim = response.Result;
            _this.CurrentSession.StopBusyIndicator();
            if (!Tools_1.AppTool.IsNullOrEmpty(claim)) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
                    _this.EditClaimsRelatedEntityLine(item, false);
                }
            }
        });
    };
    ClaimGeneralTabComponent.prototype.EditClaimsRelatedEntityLine = function (item, isNewEntity) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("");
        var windowArgs = {};
        windowArgs.EntityCounterKey = item.entityPM.EntityCounterKey;
        windowArgs.ClaimPM = this.EntityPM;
        windowArgs.IsNewEntity = isNewEntity;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 550;
        if (isNewEntity) {
            windowArgs.WindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.O.NewClaimsRelatedEntity");
        }
        else {
            var tapagNumberAndNumeral = "";
            if (!Tools_1.AppTool.IsNullOrEmpty(item.TapagNumberAndNumeral)) {
                tapagNumberAndNumeral = item.TapagNumberAndNumeral;
            }
            windowArgs.WindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.O.EditClaimsRelatedEntity") + " " + tapagNumberAndNumeral;
        }
        //windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function (event) {
            if (event == 'ok') {
                _this.RefreshEntity();
                //this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
            }
        });
        logWindow.IsHideHeader = true;
        logWindow.Show('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntityTabComponent');
        this.CurrentSession.StopBusyIndicator();
    };
    ClaimGeneralTabComponent.prototype.DeleteButtonClicked = function (item) {
        var _this = this;
        if (!this.IsControlEnabled)
            return;
        if (!Tools_1.AppTool.IsNullOrEmpty(item.entityPM.TapagNumber)) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Width = 250;
            messageWindow.Height = 150;
            messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show("לא ניתן למחוק ישות תביעה המקושרת לתיק תפג");
            return;
        }
        var rfundDemandUncheckedWindow = new ConfirmWindow_1.ConfirmWindow();
        rfundDemandUncheckedWindow.Title = "Delete";
        rfundDemandUncheckedWindow.Width = 250;
        rfundDemandUncheckedWindow.Height = 150;
        rfundDemandUncheckedWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
        rfundDemandUncheckedWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.Cancel");
        rfundDemandUncheckedWindow.ShowCancelButton = false;
        rfundDemandUncheckedWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.G.DeleteClaimRelatedEntity"));
        rfundDemandUncheckedWindow.WindowClosed.subscribe(function (event) {
            if (rfundDemandUncheckedWindow.Yes) {
                _this.DeleteSelected(item);
            }
        });
    };
    ClaimGeneralTabComponent.prototype.DeleteSelected = function (item) {
        this.ClaimsRelatedEntitiesObslist.Remove(item);
        this.EntityPM.RemoveClaimsRelatedEntity(item.entityPM);
    };
    ClaimGeneralTabComponent.prototype.AddEntityCommand = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (errors.length > 0) {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
            return;
        }
        var newClaimsRelatedEntityPM = new ClaimsRelatedEntityPM_1.ClaimsRelatedEntityPM(this.EntityPM);
        newClaimsRelatedEntityPM.ClaimId = this.EntityPM.Id;
        newClaimsRelatedEntityPM.Tenant = this.EntityPM.Tenant;
        newClaimsRelatedEntityPM.EntityCounterKey = (Tools_1.ArrayTool.Max(this.EntityPM.ClaimsRelatedEntities, "EntityCounterKey") + 1);
        newClaimsRelatedEntityPM.IsSendClaimsRelatedEntity = true;
        var newClaimsRelatedEntityLineComponent = new ClaimsRelatedEntityLineComponent(newClaimsRelatedEntityPM, this.EntityPM, false);
        this.ClaimsRelatedEntitiesObslist.Insert(newClaimsRelatedEntityLineComponent);
        this.EntityPM.AddClaimsRelatedEntity(newClaimsRelatedEntityPM);
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        this.CurrentSession.StartBusyIndicator("");
        this.EditClaimsRelatedEntityLine(newClaimsRelatedEntityLineComponent, true);
    };
    ClaimGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClaimGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ClaimGeneralTabComponent);
    return ClaimGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimGeneralTabComponent = ClaimGeneralTabComponent;
var ClaimsRelatedEntityLineComponent = /** @class */ (function (_super) {
    __extends(ClaimsRelatedEntityLineComponent, _super);
    function ClaimsRelatedEntityLineComponent(entityPM, claimPM, isEnable) {
        var _this = _super.call(this) || this;
        _this.entityPM = entityPM;
        _this.claimPM = claimPM;
        _this.ObjectTableName = "Customs.ClaimsRelatedEntity";
        _this.DataContext = _this;
        _this._IsSendEnabled = true;
        return _this;
    }
    Object.defineProperty(ClaimsRelatedEntityLineComponent.prototype, "ClaimEntityTypeName", {
        get: function () { return this.entityPM.ClaimEntityTypeName; },
        set: function (newValue) { this.entityPM.ClaimEntityTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimsRelatedEntityLineComponent.prototype, "ClaimEntityNumber", {
        get: function () { return this.entityPM.ClaimEntityNumber; },
        set: function (newValue) { this.entityPM.ClaimEntityNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimsRelatedEntityLineComponent.prototype, "TapagNumberAndNumeral", {
        get: function () {
            if (this.entityPM != null && !Tools_1.AppTool.IsNullOrEmpty(this.entityPM.TapagNumber)) {
                this.IsSendEnabled = false;
                this.IsSendClaimsRelatedEntity = false;
                this.UIProperties.SetEnabled("IsSendClaimsRelatedEntity", "Customs.Claim", false);
                if (this.entityPM.Numeral != null && this.entityPM.Numeral > 0) {
                    return this.entityPM.TapagNumber + "-" + this.entityPM.Numeral;
                }
                return this.entityPM.TapagNumber;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimsRelatedEntityLineComponent.prototype, "IsFinancialRefundDemand", {
        get: function () { return this.entityPM.IsFinancialRefundDemand; },
        set: function (newValue) { this.entityPM.IsFinancialRefundDemand = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimsRelatedEntityLineComponent.prototype, "ClaimAmount", {
        get: function () { return this.entityPM.ClaimAmount; },
        set: function (newValue) { this.entityPM.ClaimAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimsRelatedEntityLineComponent.prototype, "ContinuousMessagesTypeName", {
        get: function () { return this.entityPM.ContinuousMessagesTypeName; },
        set: function (newValue) { this.entityPM.ContinuousMessagesTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimsRelatedEntityLineComponent.prototype, "DecisionName", {
        get: function () { return this.entityPM.DecisionName; },
        set: function (newValue) { this.entityPM.DecisionName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimsRelatedEntityLineComponent.prototype, "IsSendClaimsRelatedEntity", {
        get: function () { return this.entityPM.IsSendClaimsRelatedEntity; },
        set: function (newValue) { this.entityPM.IsSendClaimsRelatedEntity = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClaimsRelatedEntityLineComponent.prototype, "IsSendEnabled", {
        get: function () { return this._IsSendEnabled; },
        set: function (newValue) { this._IsSendEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    return ClaimsRelatedEntityLineComponent;
}(BaseComponent_1.BaseComponent));
exports.ClaimsRelatedEntityLineComponent = ClaimsRelatedEntityLineComponent;
var AddressItemComponent = /** @class */ (function (_super) {
    __extends(AddressItemComponent, _super);
    function AddressItemComponent(addressPm, clientPM, isNew, addressMode) {
        var _this = _super.call(this) || this;
        _this.IsNew = false;
        _this.CommunicationList = [];
        _this.AddressPM = addressPm;
        _this.ClientPM = clientPM;
        _this.IsNew = isNew;
        _this.AddressMode = addressMode;
        _this.BuildCommunicationList();
        return _this;
    }
    AddressItemComponent.prototype.BuildCommunicationList = function () {
        this.CommunicationList = [];
        if (this.AddressPM.ClientsAddressCommTypes != null && this.AddressPM.ClientsAddressCommTypes.length > 0) {
            this.CommunicationList.push(this.AddressPM.ClientsAddressCommTypes[0]);
        }
    };
    Object.defineProperty(AddressItemComponent.prototype, "IsHebrewAddress", {
        get: function () { return this.AddressPM.IsHebrewAddress; },
        set: function (newValue) { this.AddressPM.IsHebrewAddress = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemComponent.prototype, "AddressTypeName", {
        get: function () { return this.AddressPM.AddressTypeName; },
        set: function (newValue) { this.AddressPM.AddressTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemComponent.prototype, "AddressPurposeName", {
        get: function () { return this.AddressPM.AddressPurposeName; },
        set: function (newValue) { this.AddressPM.AddressPurposeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemComponent.prototype, "LocalStreetName", {
        get: function () { return this.AddressPM.LocalStreetName; },
        set: function (newValue) { this.AddressPM.LocalStreetName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemComponent.prototype, "LocalHouseNumber", {
        get: function () { return this.AddressPM.LocalHouseNumber; },
        set: function (newValue) { this.AddressPM.LocalHouseNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemComponent.prototype, "LocalCityName", {
        get: function () { return this.AddressPM.LocalCityName; },
        set: function (newValue) { this.AddressPM.LocalCityName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemComponent.prototype, "EnglishCountryName", {
        get: function () { return this.AddressPM.EnglishCountryName; },
        set: function (newValue) { this.AddressPM.EnglishCountryName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemComponent.prototype, "EnglishSubCountryName", {
        get: function () { return this.AddressPM.EnglishSubCountryName; },
        set: function (newValue) { this.AddressPM.EnglishSubCountryName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemComponent.prototype, "EnglishCityName", {
        get: function () { return this.AddressPM.EnglishCityName; },
        set: function (newValue) { this.AddressPM.EnglishCityName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemComponent.prototype, "ContactFirstName", {
        get: function () { return this.AddressPM.ContactFirstName; },
        set: function (newValue) { this.AddressPM.ContactFirstName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddressItemComponent.prototype, "ContactRoleTypeName", {
        get: function () { return this.AddressPM.ContactRoleTypeName; },
        set: function (newValue) { this.AddressPM.ContactRoleTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    return AddressItemComponent;
}(BaseComponent_1.BaseComponent));
exports.AddressItemComponent = AddressItemComponent;
//# sourceMappingURL=ClaimGeneralTabComponent.js.map