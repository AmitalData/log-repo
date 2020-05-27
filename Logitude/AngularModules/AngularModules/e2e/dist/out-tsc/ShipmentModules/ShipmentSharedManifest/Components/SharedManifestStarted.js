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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var SharedAgentManifestService_1 = require("../../../Shipment/Services/Others/SharedAgentManifestService");
var Tools_1 = require("../../../Infrastructure/Tools");
var CardListService_1 = require("../../../Common/Services/StandardLists/CardListService");
var AgentSharedLogisticsKeyPMService_1 = require("../../../Common/Services/ExtendedPMs/AgentSharedLogisticsKeyPMService");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var logitudewindow_1 = require("../../../Controls/Windows/logitudewindow");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Args_1 = require("../../../Infrastructure/Args");
var AddressListService_1 = require("../../../Common/Services/StandardLists/AddressListService");
var ContactListService_1 = require("../../../Common/Services/StandardLists/ContactListService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var AddressPM_1 = require("../../../Common/EntityPMs/AddressPM");
var SharedManifestStarted = /** @class */ (function () {
    function SharedManifestStarted(_sharedAgentManifestService, _agentSharedLogisticsKeyPMService) {
        this._sharedAgentManifestService = _sharedAgentManifestService;
        this._agentSharedLogisticsKeyPMService = _agentSharedLogisticsKeyPMService;
        //private myCardListService: CardListService;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.LableShareBoutton = "Share";
        this.ValidationErrorsList = [];
        this.HasError = false;
        this.IsSuccessfullySharedManifest = false;
        this.IsEnableButtonSharedManifest = false;
        this.FromSharedManifestEditAgentComponent = false;
        this.IsShareUpdatedAgent = false;
        this.IsShowUpdateAgentArea = false;
        this.isSharingManifesRequested = false;
        this.IsEditingEnabled = true;
        this.AllAddresses = [];
        this.AllContacts = [];
        this.Listen();
    }
    SharedManifestStarted.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityPM = args.EntityPM;
        this.FromSharedManifestEditAgentComponent = args.FromSharedManifestEditAgentComponent;
        this.IsShareUpdatedAgent = args.IsShareUpdatedAgent;
        if (this.IsShareUpdatedAgent) {
            this.LableShareBoutton = "Next";
        }
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AgentId)) {
            this.ValidationErrorsList.push("Please define shipment agent in the Partners tab");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeId)) {
            this.ValidationErrorsList.push("The consignee partner is missing");
        }
        if (this.EntityPM.TransportModeId == "A" && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Master)) {
            this.ValidationErrorsList.push("The master number is missing");
        }
        if (this.ValidationErrorsList.length == 0) {
            if (!this.FromSharedManifestEditAgentComponent) {
                this.CurrentSession.StartBusyIndicator("Loading...");
                this._agentSharedLogisticsKeyPMService.GetSingleByAgentId(this.EntityPM.AgentId).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        var agentSharedKey = myResponse.Result;
                        if (agentSharedKey) {
                            switch (agentSharedKey.StatusCode) {
                                case "A":
                                    if (_this.EntityPM.ShipmentLevelCode == "C") {
                                        _this.CurrentSession.StartBusyIndicator("Loading...");
                                        _this._sharedAgentManifestService.GetCheckIfMasterShipmentHaveHouseWithOtherAgent(_this.EntityPM.Id, _this.EntityPM.AgentId, _this.EntityPM.Tenant).subscribe(function (myResponse) {
                                            _this.CurrentSession.StopBusyIndicator();
                                            if (!myResponse.HasError) {
                                                if (myResponse.Result == true) {
                                                    _this.ValidationErrorsList.push("One of the houses has agent different from the master shipment");
                                                }
                                                else {
                                                    _this.IsEnableButtonSharedManifest = true;
                                                    if (_this.EntityPM.IsManifestSentToAgent)
                                                        _this.UpdateAgent();
                                                }
                                            }
                                        });
                                    }
                                    else {
                                        _this.IsEnableButtonSharedManifest = true;
                                        if (_this.EntityPM.IsManifestSentToAgent)
                                            _this.UpdateAgent();
                                    }
                                    break;
                                case "W":
                                    _this.ValidationErrorsList.push("Waiting for the Agent’s approval to enable sharing");
                                    _this.HasError = true;
                                    break;
                                case "I":
                                    _this.ValidationErrorsList.push("Please connect with the agent from the agent’s shared logistics tab");
                                    _this.HasError = true;
                                    break;
                            }
                        }
                        else {
                            _this.ValidationErrorsList.push("Please connect with the agent from the agent’s shared logistics tab");
                            _this.HasError = true;
                        }
                    }
                    else {
                        _this.ValidationErrorsList.push(myResponse.ErrorsArray[0]);
                        _this.HasError = true;
                    }
                });
            }
            else
                this.UpdateAgent();
        }
        else
            this.HasError = true;
    };
    SharedManifestStarted.prototype.SharingManifesButtonClick = function () {
        if (this.EntityPM.IsDirty) {
            this.isSharingManifesRequested = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else {
            this.StartSharingManifest();
        }
    };
    ;
    SharedManifestStarted.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    if (_this.isSharingManifesRequested) {
                        _this.StartSharingManifest();
                    }
                    _this.isSharingManifesRequested = false;
                }
            });
        }
    };
    SharedManifestStarted.prototype.UpdateAgent = function () {
        var _this = this;
        if (!this.FromSharedManifestEditAgentComponent) {
            this.InitializeServices();
            var partnerItem = new PartnerItem(this, "AGENT");
            var logitudeWindow = new logitudewindow_1.LogitudeWindow();
            logitudeWindow.Title = "Share Updated Agent";
            logitudeWindow.DataContext = partnerItem;
            logitudeWindow.Show("./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestEditAgentComponent");
            logitudeWindow.WindowClosed.subscribe(function (s) {
                if (s == "OK") {
                    _this.StartUpdateAgentManifest();
                }
            });
            this.CloseButtonClicked();
        }
        else {
            this.IsShowUpdateAgentArea = true;
            this.CurrentSession.CurrentWindow.Title = "Share Manifest with Updated Agent";
        }
    };
    SharedManifestStarted.prototype.StartUpdateAgentManifest = function () {
        var windowArgs = {};
        windowArgs.EntityPM = this.EntityPM;
        var logWindow = new logitudewindow_1.LogitudeWindow();
        windowArgs.FromSharedManifestEditAgentComponent = true;
        logWindow.Title = "Share Updated Agent";
        windowArgs.IsShareUpdatedAgent = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestStarted");
    };
    SharedManifestStarted.prototype.StartSharingManifest = function (isUpdateAgent) {
        var _this = this;
        if (isUpdateAgent === void 0) { isUpdateAgent = false; }
        this.CurrentSession.StartBusyIndicator("Sharing Manifest...");
        this._sharedAgentManifestService.ShareAgentManifest(this.EntityPM.Id, isUpdateAgent).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.IsEnableButtonSharedManifest = false;
            if (!myResponse.HasError) {
                _this.IsSuccessfullySharedManifest = true;
                _this.EntityPM.IsManifestSentToAgent = true;
                _this.IsShowUpdateAgentArea = false;
                _this.IsShareUpdatedAgent = false;
                var activity = !isUpdateAgent ? "Share Manifests" : "Share Updated Agent";
                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Agents Shared Logistics", activity);
            }
            else {
                if (myResponse.ErrorsArray && myResponse.ErrorsArray.length > 0) {
                    myResponse.ErrorsArray.forEach(function (item) {
                        _this.ValidationErrorsList.push(item);
                    });
                }
                _this.HasError = true;
            }
        });
    };
    SharedManifestStarted.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SharedManifestStarted.prototype.InitializeServices = function () {
        this.CardListService = new CardListService_1.CardListService();
        this.AddressListService = new AddressListService_1.AddressListService();
        this.ContactListService = new ContactListService_1.ContactListService();
    };
    SharedManifestStarted = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SharedManifestStarted',
            templateUrl: './SharedManifestStarted.html',
            providers: [SharedAgentManifestService_1.SharedAgentManifestService, AgentSharedLogisticsKeyPMService_1.AgentSharedLogisticsKeyPMService],
        }),
        __metadata("design:paramtypes", [SharedAgentManifestService_1.SharedAgentManifestService, AgentSharedLogisticsKeyPMService_1.AgentSharedLogisticsKeyPMService])
    ], SharedManifestStarted);
    return SharedManifestStarted;
}());
exports.SharedManifestStarted = SharedManifestStarted;
var PartnerItem = /** @class */ (function (_super) {
    __extends(PartnerItem, _super);
    function PartnerItem(fatherComponent, typeCode) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "Shipment";
        _this.IsDisableNextButton = false;
        _this.IsEditingEnabled = true;
        // PartnerId
        _this.PartnerCardList = null;
        _this.IsReseting = false;
        _this.isAddressLoaded = false;
        _this.isContactLoaded = false;
        _this.ShowNoTemplateText = false;
        _this.AddressCityText = null;
        // Add|Edit Partner
        _this.isEditButtonClicked = false;
        _this.EntityPM = fatherComponent.EntityPM;
        _this.IsEditingEnabled = fatherComponent.IsEditingEnabled;
        _this.Code = typeCode;
        _this.InitializeProperties();
        _this.GetPartnerAddress();
        _this.GetPartnerContact();
        return _this;
    }
    PartnerItem.prototype.SetUIProperties = function () {
        var isPartnerFilled = this.PartnerId == null ? false : true;
        var isFieldsEnabled = false;
        if (this.IsEditingEnabled) {
            if (isPartnerFilled) {
                isFieldsEnabled = true;
            }
        }
        this.UIProperties.SetRequired(this.PartnerIdProperty, this.ObjectTableName, !isPartnerFilled);
        this.UIProperties.SetEnabled(this.PartnerIdProperty, this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled(this.PartnerAddressIdProperty, this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled(this.PartnerContactIdProperty, this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled(this.Reference1Property, this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled(this.Reference2Property, this.ObjectTableName, this.IsEditingEnabled);
    };
    PartnerItem.prototype.InitializeProperties = function () {
        this.PartnerIndex = 3;
        this.FullCode = "Agent";
        this.PartnerTypeName = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F." + this.FullCode + "Id");
    };
    Object.defineProperty(PartnerItem.prototype, "CardDependencyProperty1", {
        get: function () {
            var myResult = "AG";
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "CardDependencyProperty1IsList", {
        get: function () {
            var myResult = false;
            switch (this.Code) {
                case "SHIPR":
                case "CONSI":
                case "CSTMR":
                    {
                        if (this.EntityPM.ShipmentLevelCode != "C") {
                            if (SessionLocator_1.SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
                                myResult = true;
                            }
                        }
                        break;
                    }
                case "REAGT":
                case "NOTF1":
                case "NOTF2":
                case "SHPNT":
                case "CONNT":
                case "CONSL":
                    {
                        myResult = true;
                        break;
                    }
                default: {
                    myResult = false;
                    break;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Name", {
        get: function () {
            switch (this.Code) {
                case "AGENT": {
                    return this.EntityPM.AgentName;
                }
                default: {
                    return null;
                }
            }
        },
        set: function (newValue) {
            if (this.EntityPM.AgentName != newValue) {
                this.EntityPM.AgentName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Note", {
        get: function () {
            return this.EntityPM.AgentNote;
        },
        set: function (newValue) {
            if (this.EntityPM.AgentNote != newValue) {
                this.EntityPM.AgentNote = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "IsCustomer", {
        get: function () {
            return (this.PartnerId == this.EntityPM.CustomerId) ? true : false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "PartnerIdProperty", {
        get: function () {
            return "AgentId";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "PartnerId", {
        get: function () {
            return this.AgentId;
        },
        set: function (newValue) {
            this.AgentId = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "AgentId", {
        get: function () {
            return this.EntityPM.AgentId;
        },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.AgentId != newValue) {
                this.EntityPM.AgentId = newValue;
                this.IsDisableNextButton = true;
                this.fatherComponent._sharedAgentManifestService.GetIsAgentSharedManifests(this.EntityPM.AgentId, this.EntityPM.Id).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var isDisableNextButton = myResponse.Result ? true : false;
                        _this.IsDisableNextButton = isDisableNextButton;
                    }
                });
                this.GetPartnerCard();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "PartnerAddressIdProperty", {
        get: function () {
            return "AgentAddressId";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "AddressId", {
        get: function () {
            return this.AgentAddressId;
        },
        set: function (newValue) {
            this.AgentAddressId = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "AgentAddressId", {
        get: function () {
            return this.EntityPM.AgentAddressId;
        },
        set: function (newValue) {
            if (this.EntityPM.AgentAddressId != newValue) {
                this.EntityPM.AgentAddressId = newValue;
                this.GetPartnerAddress();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "PartnerContactIdProperty", {
        // ContactId
        get: function () {
            return "AgentContactId";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "ContactId", {
        get: function () {
            return this.AgentContactId;
        },
        set: function (newValue) {
            this.AgentContactId = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "AgentContactId", {
        get: function () {
            return this.EntityPM.AgentContactId;
        },
        set: function (newValue) {
            if (this.EntityPM.AgentContactId != newValue) {
                this.EntityPM.AgentContactId = newValue;
                this.GetPartnerContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "HasReference1", {
        // Reference1
        get: function () {
            var myResult = true;
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Reference1Property", {
        get: function () {
            return "AgentReference1";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Reference1", {
        get: function () {
            return this.AgentReference1;
        },
        set: function (newValue) {
            this.AgentReference1 = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "AgentReference1", {
        get: function () {
            return this.EntityPM.AgentReference1;
        },
        set: function (newValue) {
            if (this.EntityPM.AgentReference1 != newValue) {
                this.EntityPM.AgentReference1 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "HasReference2", {
        // Reference2
        get: function () {
            var myResult = true;
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Reference2Property", {
        get: function () {
            return "AgentReference2";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "Reference2", {
        get: function () {
            return this.AgentReference2;
        },
        set: function (newValue) {
            this.AgentReference2 = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartnerItem.prototype, "AgentReference2", {
        get: function () {
            return this.EntityPM.AgentReference2;
        },
        set: function (newValue) {
            if (this.EntityPM.AgentReference2 != newValue) {
                this.EntityPM.AgentReference2 = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    PartnerItem.prototype.GetPartnerCard = function () {
        var _this = this;
        this.SetUIProperties();
        var myCardId = this.PartnerId;
        if (Tools_1.AppTool.IsNullOrEmpty(myCardId)) {
            this.Name = null;
            this.Note = null;
            this.PartnerCardList = null;
            this.AddressId = null;
            this.ContactId = null;
            if (this.Code == "SHIPR") {
                if (this.EntityPM.KnownConsignorNumber != null) {
                    this.EntityPM.KnownConsignorNumber = null;
                }
                if (this.EntityPM.KCExpirationDate != null) {
                    this.EntityPM.KCExpirationDate = null;
                }
            }
        }
        else {
            var myService = this.fatherComponent.CardListService;
            myService.getSingle(myCardId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.PartnerCardList = myResponse.Result;
                        if (_this.PartnerCardList != null) {
                            _this.Name = _this.PartnerCardList.EnglishName;
                            _this.Note = _this.PartnerCardList.Notes;
                            if (_this.Code == "SHIPR") {
                                if (_this.EntityPM.KnownConsignorNumber != _this.PartnerCardList.KnownConsignor) {
                                    _this.EntityPM.KnownConsignorNumber = _this.PartnerCardList.KnownConsignor;
                                }
                                if (_this.EntityPM.KCExpirationDate != _this.PartnerCardList.KCExpirationDate) {
                                    _this.EntityPM.KCExpirationDate = _this.PartnerCardList.KCExpirationDate;
                                }
                            }
                            if (!_this.IsReseting) {
                                _this.AddressId = _this.PartnerCardList.MainAddressId;
                                _this.ContactId = _this.PartnerCardList.PrimaryContactId;
                            }
                        }
                    }
                }
            });
        }
    };
    PartnerItem.prototype.GetPartnerAddress = function () {
        var _this = this;
        this.isAddressLoaded = false;
        this.PartnerAddressList = null;
        this.ShowNoTemplateText = false;
        this.AddressCityText = null;
        var myAddressId = this.AddressId;
        if (myAddressId != null) {
            var list = this.fatherComponent.AllAddresses.filter(function (f) { return f.Id == myAddressId; })[0];
            if (list) {
                this.PartnerAddressList = list;
                this.isAddressLoaded = true;
                this.BuildAddressCityText();
                this.OnLoadCompleted();
            }
            else {
                var myService = this.fatherComponent.AddressListService;
                myService.getSingle(myAddressId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.PartnerAddressList = myResponse.Result;
                            _this.BuildAddressCityText();
                            if (_this.PartnerAddressList) {
                                _this.fatherComponent.AllAddresses.push(_this.PartnerAddressList);
                            }
                        }
                    }
                    _this.isAddressLoaded = true;
                    _this.OnLoadCompleted();
                });
            }
        }
        else {
            this.isAddressLoaded = true;
            this.OnLoadCompleted();
        }
    };
    PartnerItem.prototype.GetPartnerContact = function () {
        var _this = this;
        this.isContactLoaded = false;
        this.PartnerContactList = null;
        this.ShowNoTemplateText = false;
        var myContactId = this.ContactId;
        if (myContactId != null) {
            var list = this.fatherComponent.AllContacts.filter(function (f) { return f.Id == myContactId; })[0];
            if (list) {
                this.PartnerContactList = list;
                this.isContactLoaded = true;
                this.OnLoadCompleted();
            }
            var myService = this.fatherComponent.ContactListService;
            myService.getSingle(myContactId).subscribe(function (myResponse) {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        _this.PartnerContactList = myResponse.Result;
                        if (_this.PartnerContactList) {
                            _this.fatherComponent.AllContacts.push(_this.PartnerContactList);
                        }
                    }
                }
                _this.isContactLoaded = true;
                _this.OnLoadCompleted();
            });
        }
        else {
            this.isContactLoaded = true;
            this.OnLoadCompleted();
        }
    };
    PartnerItem.prototype.OnLoadCompleted = function () {
        if (this.isAddressLoaded && this.isContactLoaded) {
            if (this.PartnerAddressList == null && this.PartnerContactList == null) {
                this.ShowNoTemplateText = true;
            }
            else {
                this.ShowNoTemplateText = false;
            }
        }
    };
    PartnerItem.prototype.BuildAddressCityText = function () {
        var myResult = null;
        if (this.PartnerAddressList) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PartnerAddressList.City)) {
                myResult = this.PartnerAddressList.City;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PartnerAddressList.StateName)) {
                myResult = Tools_1.AppTool.IsNullOrEmpty(myResult) ? this.PartnerAddressList.StateName : myResult + ", " + this.PartnerAddressList.StateName;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PartnerAddressList.ZipCode)) {
                myResult = Tools_1.AppTool.IsNullOrEmpty(myResult) ? this.PartnerAddressList.ZipCode : myResult + ", " + this.PartnerAddressList.ZipCode;
            }
        }
        this.AddressCityText = myResult;
    };
    PartnerItem.prototype.AddPartnerClicked = function () {
        var _this = this;
        var myPerspective = null;
        var myComponentPath = null;
        if (this.CardDependencyProperty1 == "AG") {
            myComponentPath = "./CommonModules/CommonAgent/Components/NewEntity/NewAgentComponent";
        }
        if (!this.IsCustomer) {
            myPerspective = "ShippersAndConsignees";
        }
        if (myComponentPath != null) {
            var logWindow = new logitudewindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = "New " + this.PartnerTypeName;
            if (!Tools_1.AppTool.IsNullOrEmpty(myPerspective)) {
                var args = new Args_1.NewEntityArgs();
                args.Perspective = myPerspective;
                logWindow.WindowArgs = args;
            }
            logWindow.Show(myComponentPath);
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.PartnerId = comp.EntityPM.Id;
                    }
                });
            });
        }
    };
    PartnerItem.prototype.EditPartnerClicked = function () {
        var _this = this;
        if (!this.isEditButtonClicked) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PartnerId)) {
                this.isEditButtonClicked = true;
                var myService = this.fatherComponent.CardListService;
                myService.getSingle(this.PartnerId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        if (list) {
                            var objectTableName = "Agent";
                            if (objectTableName != null) {
                                var logWindow = new logitudewindow_1.LogitudeWindow();
                                logWindow.Title = "Edit " + _this.PartnerTypeName;
                                logWindow.IsFillScreen = true;
                                logWindow.ShowEditComponent(_this.PartnerId, objectTableName);
                                logWindow.ComponentLoaded.subscribe(function (comp) {
                                    logWindow.WindowClosed.subscribe(function (s) {
                                        _this.Name = comp.EntityPM.EnglishName;
                                        _this.Note = comp.EntityPM.Notes;
                                        if (_this.Code == "SHIPR") {
                                            if (_this.EntityPM.KnownConsignorNumber != comp.EntityPM.KnownConsignor) {
                                                _this.EntityPM.KnownConsignorNumber = comp.EntityPM.KnownConsignor;
                                            }
                                            if (_this.EntityPM.KCExpirationDate != comp.EntityPM.KCExpirationDate) {
                                                _this.EntityPM.KCExpirationDate = comp.EntityPM.KCExpirationDate;
                                            }
                                        }
                                        _this.GetPartnerAddress();
                                        _this.GetPartnerContact();
                                        _this.isEditButtonClicked = false;
                                    });
                                });
                            }
                            else {
                                _this.isEditButtonClicked = false;
                            }
                        }
                        else {
                            _this.isEditButtonClicked = false;
                        }
                    }
                    else {
                        _this.isEditButtonClicked = false;
                    }
                });
            }
        }
    };
    // Add|Edit Address
    PartnerItem.prototype.AddAddressClicked = function () {
        var _this = this;
        var entityPM = new AddressPM_1.AddressPM();
        entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        entityPM.AddressTypeId = "O";
        entityPM.CardId = this.PartnerId;
        var myPartnerTypeId = null;
        var isCustomer;
        if (this.PartnerCardList != null) {
            myPartnerTypeId = this.PartnerCardList.PartnerTypeId;
            isCustomer = this.PartnerCardList.IsCustomer;
        }
        if (entityPM != null) {
            var logeWindow = new logitudewindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Add Address";
            logeWindow.WindowArgs = { EntityPM: entityPM, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Partners/AddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.AddressId = null;
                    _this.AddressId = entityPM.Id;
                }
            });
        }
    };
    PartnerItem.prototype.EditAddressClicked = function () {
        var _this = this;
        var myAddressId = this.AddressId;
        var myPartnerTypeId = null;
        var isCustomer;
        if (this.PartnerCardList != null) {
            myPartnerTypeId = this.PartnerCardList.PartnerTypeId;
            isCustomer = this.PartnerCardList.IsCustomer;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(myAddressId)) {
            var logeWindow = new logitudewindow_1.LogitudeWindow();
            logeWindow.Width = 630;
            logeWindow.Height = 430;
            logeWindow.Title = "Edit Address";
            logeWindow.WindowArgs = { EntityId: myAddressId, PartnerTypeId: myPartnerTypeId, IsCustomer: isCustomer };
            logeWindow.Show("./ShipmentModules/ShipmentTabs/Components/Partners/AddEditAddressComponent");
            logeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    var list = _this.fatherComponent.AllAddresses.filter(function (f) { return f.Id == myAddressId; })[0];
                    if (list) {
                        var indexOfList = _this.fatherComponent.AllAddresses.indexOf(list);
                        _this.fatherComponent.AllAddresses.splice(indexOfList, 1);
                    }
                    _this.AddressId = null;
                    _this.AddressId = myAddressId;
                }
            });
        }
    };
    return PartnerItem;
}(BaseComponent_1.BaseComponent));
exports.PartnerItem = PartnerItem;
//# sourceMappingURL=SharedManifestStarted.js.map