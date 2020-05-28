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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var GlobalDomainService_1 = require("../../../../Common/Services/GlobalDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var CCSSettingsTabComponent = /** @class */ (function (_super) {
    __extends(CCSSettingsTabComponent, _super);
    function CCSSettingsTabComponent(entityArgs, entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.entityResourceService = entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "TenantManagement";
        _this.QuickSearchItems = [];
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsEditingAllowed = false;
        _this.AllowAirlinesIsEnabled = true;
        _this.RestrictedLabel = "";
        _this.PortsFileHtmlId = Guid_1.Guid.NewRandomString();
        _this.tenantZeroSearchText = null;
        _this.EntityPM = _this.entityArgs.EntityPM;
        _this.myService = new PartnersDomainService_1.PartnersDomainService();
        if (_this.EntityPM != null) {
            _this.entityResourceService.getEntityResourceByTableName("Airline", 0).subscribe(function (response1) {
                _this.IsResourcesReady = true;
                _this.SetUIProperties();
                _this.LoadAirlines();
            });
        }
        return _this;
    }
    CCSSettingsTabComponent.prototype.SetUIProperties = function () {
        this.isTenantManagementEditable = this.IsTenantManagementEditable();
        this.RestrictedLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("TenantManagement.F.IsRestrictedByAirline");
        var allowAirlinesEnabled = true;
        if (!this.isTenantManagementEditable) {
            allowAirlinesEnabled = false;
        }
        else if (!this.IsRestrictedByAirline) {
            allowAirlinesEnabled = false;
        }
        this.IsEditingAllowed = this.isTenantManagementEditable;
        this.AllowAirlinesIsEnabled = allowAirlinesEnabled;
        this.UIProperties.SetEnabled("TTY", this.ObjectTableName, this.isTenantManagementEditable);
        this.UIProperties.SetEnabled("PIMA", this.ObjectTableName, this.isTenantManagementEditable);
        this.UIProperties.SetEnabled("AWBMessagesCCSTypeCode", this.ObjectTableName, this.isTenantManagementEditable);
        this.UIProperties.SetEnabled("IsCargonautEnabled", this.ObjectTableName, this.isTenantManagementEditable);
        this.UIProperties.SetEnabled("IsDEXXConnectionEnabled", this.ObjectTableName, this.isTenantManagementEditable);
        this.UIProperties.SetVisibility("IsEAWBOnlyDemo", this.ObjectTableName, (SessionLocator_1.SessionLocator.Tenant == 0 || SessionLocator_1.SessionLocator.Tenant == 341) ? true : false);
        this.UIProperties.SetVisibility("IsINTTRAOnlyDemo", this.ObjectTableName, (SessionLocator_1.SessionLocator.Tenant == 0) ? true : false);
        this.SetUIProperties_SetRequires();
    };
    CCSSettingsTabComponent.prototype.SetUIProperties_SetRequires = function () {
        //this.UIProperties.SetRequires("TTY", TargetEntityName, entityPM, false);
        this.UIProperties.SetRequired("PIMA", this.ObjectTableName, false);
        if (this.AWBMessagesCCSTypeCode == "CHAMP") {
            //if (string.IsNullOrEmpty(TTY))
            //{
            //    this.UIProperties.SetRequires("TTY", TargetEntityName, entityPM, true);
            //}
        }
        else if (this.AWBMessagesCCSTypeCode == "GLSHK") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.PIMA)) {
                this.UIProperties.SetRequired("PIMA", this.ObjectTableName, true);
            }
        }
    };
    CCSSettingsTabComponent.prototype.IsTenantManagementEditable = function () {
        var myResult = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("TenantManagement", "EnableTenantManagementEdit")) {
            myResult = true;
        }
        return myResult;
    };
    Object.defineProperty(CCSSettingsTabComponent.prototype, "AWBMessagesCCSTypeCode", {
        get: function () { return this.EntityPM.AWBMessagesCCSTypeCode; },
        set: function (newValue) {
            if (this.EntityPM.AWBMessagesCCSTypeCode != newValue) {
                this.EntityPM.AWBMessagesCCSTypeCode = newValue;
                this.SetUIProperties();
                this.LoadAirlines();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CCSSettingsTabComponent.prototype, "TTY", {
        get: function () { return this.EntityPM.TTY; },
        set: function (newValue) {
            if (this.EntityPM.TTY != newValue) {
                this.EntityPM.TTY = newValue;
                this.SetUIProperties_SetRequires();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CCSSettingsTabComponent.prototype, "PIMA", {
        get: function () { return this.EntityPM.PIMA; },
        set: function (newValue) {
            if (this.EntityPM.PIMA != newValue) {
                this.EntityPM.PIMA = newValue;
                this.SetUIProperties_SetRequires();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CCSSettingsTabComponent.prototype, "IsEAWBOnlyDemo", {
        get: function () { return this.EntityPM.IsEAWBOnlyDemo; },
        set: function (newValue) {
            if (this.EntityPM.IsEAWBOnlyDemo != newValue) {
                this.EntityPM.IsEAWBOnlyDemo = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CCSSettingsTabComponent.prototype, "IsINTTRAOnlyDemo", {
        get: function () { return this.EntityPM.IsINTTRAOnlyDemo; },
        set: function (newValue) {
            if (this.EntityPM.IsINTTRAOnlyDemo != newValue) {
                this.EntityPM.IsINTTRAOnlyDemo = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CCSSettingsTabComponent.prototype, "IsCargonautEnabled", {
        get: function () { return this.EntityPM.IsCargonautEnabled; },
        set: function (newValue) {
            if (this.EntityPM.IsCargonautEnabled != newValue) {
                this.EntityPM.IsCargonautEnabled = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CCSSettingsTabComponent.prototype, "IsDEXXConnectionEnabled", {
        get: function () { return this.EntityPM.IsDEXXConnectionEnabled; },
        set: function (newValue) {
            if (this.EntityPM.IsDEXXConnectionEnabled != newValue) {
                this.EntityPM.IsDEXXConnectionEnabled = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CCSSettingsTabComponent.prototype, "IsRestrictedByAirline", {
        get: function () { return this.EntityPM.IsRestrictedByAirline; },
        set: function (newValue) {
            if (this.EntityPM.IsRestrictedByAirline != newValue) {
                this.EntityPM.IsRestrictedByAirline = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    CCSSettingsTabComponent.prototype.QuickSearchItemClicked = function (entity) {
        //RefreshScreenEvent myEvent = SessionLocator.CurrentAssemblyLocator.EventAggregator.GetEvent<RefreshScreenEvent>();
        //myEvent.Publish(new RefreshScreenEventArgs("AirlinesPopupClose"));
        this.AllowedAirline(entity, true);
    };
    CCSSettingsTabComponent.prototype.LoadPortsClicked = function () {
        document.getElementById(this.PortsFileHtmlId).click();
    };
    CCSSettingsTabComponent.prototype.UploadFile = function (event) {
        //var file: any = UploadPortsFile(this.PortsFileHtmlId);
        //if (file && file.name && file.name.toLowerCase().indexOf("csv") != -1) {
        //    this.ArrayBufferToBase64(file, this);
        //}
    };
    CCSSettingsTabComponent.prototype.ArrayBufferToBase64 = function (file, viewmode) {
        if (file) {
            var reader = new FileReader();
            var reader = new FileReader();
            reader.onload = function (e) {
                //this.text = reader.result;
                var binary = '';
                var result = ArrayBufferToBase64(e);
                var bytes = new Uint8Array(result);
                var len = bytes.byteLength;
                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }
                viewmode.ImportPorts(window.btoa(binary));
            };
            reader.onerror = function (e) {
                console.log(e);
            };
            reader.readAsArrayBuffer(file);
        }
    };
    CCSSettingsTabComponent.prototype.ImportPorts = function (data) {
    };
    CCSSettingsTabComponent.prototype.LoadAirlines = function () {
        this.ItemsSource = [];
        this.CurrentSession.StartBusyIndicator("Loading Airlines...");
        this.LoadZeroTenantAirlines();
    };
    Object.defineProperty(CCSSettingsTabComponent.prototype, "TenantZeroSearchText", {
        get: function () { return this.tenantZeroSearchText; },
        set: function (newValue) {
            if (this.tenantZeroSearchText != newValue) {
                this.tenantZeroSearchText = newValue;
                this.LoadZeroTenantAirlines();
            }
        },
        enumerable: true,
        configurable: true
    });
    CCSSettingsTabComponent.prototype.LoadZeroTenantAirlines = function () {
        var _this = this;
        this.myZeroTenantAirlines = [];
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 500;
        filters.addAdditionalFilter("TTYPIMA", this.AWBMessagesCCSTypeCode, null, null, "Equals", true, false, false, "string");
        if (!Tools_1.AppTool.IsNullOrEmpty(this.TenantZeroSearchText)) {
            filters.addAdditionalFilter("SearchFields", this.TenantZeroSearchText, null, null, "Contains", true, true, false, "string");
        }
        this.myService.GetAirlinesByFiltersAndTenant(filters, 0).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.myZeroTenantAirlines = myResponse.Result;
                if (_this.myZeroTenantAirlines.length > 0) {
                    _this.LoadCurrentTenantAirlines();
                }
                else {
                    _this.BuildItemsSource();
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    CCSSettingsTabComponent.prototype.LoadCurrentTenantAirlines = function () {
        var _this = this;
        this.myCurrentTenantAirlines = [];
        this.myService.GetAirlinesForRequestedTenant(this.EntityPM.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.myCurrentTenantAirlines = myResponse.Result;
                _this.BuildItemsSource();
                _this.BuildAllowedAirlinesItemsSource();
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    CCSSettingsTabComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        var list = [];
        this.myZeroTenantAirlines.forEach(function (tenantZeroItem) {
            var myTenantItem = _this.myCurrentTenantAirlines.filter(function (d) { return d.Code == tenantZeroItem.Code; })[0];
            list.push(new TenantManagementAirlineItem(myTenantItem, tenantZeroItem, _this.EntityPM));
        });
        list.sort(function (a, b) { return (a.IsRegistered === b.IsRegistered) ? 0 : (a.IsRegistered > b.IsRegistered) ? -1 : 1; }).forEach(function (item) {
            _this.ItemsSource.push(item);
        });
    };
    CCSSettingsTabComponent.prototype.BuildAllowedAirlinesItemsSource = function () {
        var _this = this;
        this.AllowedAirlinesItemsSource = [];
        var list = this.myCurrentTenantAirlines.filter(function (d) { return d.IsAllowedInAirlinesRestriction; });
        list.forEach(function (item) {
            _this.AllowedAirlinesItemsSource.push(new AllowedAirlineItem(item, _this));
        });
    };
    CCSSettingsTabComponent.prototype.DeleteAllowedAirline = function (item) {
        this.AllowedAirline(item.myAirline, false);
    };
    CCSSettingsTabComponent.prototype.AllowedAirline = function (item, isAllowed) {
        if (isAllowed) {
            if (!item.IsAllowedInAirlinesRestriction) {
                this.AllowedAirlinesItemsSource.push(new AllowedAirlineItem(item, this));
            }
        }
        else {
            var itemViewModel = this.AllowedAirlinesItemsSource.filter(function (d) { return d.Id == item.Id; })[0];
            if (itemViewModel != null) {
                var index = this.AllowedAirlinesItemsSource.indexOf(itemViewModel);
                if (index > -1) {
                    this.AllowedAirlinesItemsSource.splice(index, 1);
                }
            }
        }
        this.myService.AllowAirline(isAllowed, item.Code, this.EntityPM.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
            }
        });
    };
    CCSSettingsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'CCSSettingsTabComponent',
            templateUrl: './CCSSettingsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], CCSSettingsTabComponent);
    return CCSSettingsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.CCSSettingsTabComponent = CCSSettingsTabComponent;
var AllowedAirlineItem = /** @class */ (function () {
    function AllowedAirlineItem(entityList, fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.myAirline = entityList;
    }
    Object.defineProperty(AllowedAirlineItem.prototype, "Id", {
        get: function () { return this.myAirline.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AllowedAirlineItem.prototype, "Code", {
        get: function () { return this.myAirline.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AllowedAirlineItem.prototype, "EnglishName", {
        get: function () { return this.myAirline.EnglishName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AllowedAirlineItem.prototype, "Prefix", {
        get: function () { return this.myAirline.Prefix; },
        enumerable: true,
        configurable: true
    });
    return AllowedAirlineItem;
}());
exports.AllowedAirlineItem = AllowedAirlineItem;
var TenantManagementAirlineItem = /** @class */ (function (_super) {
    __extends(TenantManagementAirlineItem, _super);
    function TenantManagementAirlineItem(myTenantItem, tenantZeroItem, entityPM) {
        var _this = _super.call(this) || this;
        _this.tenantAirlineId = null;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsRegistrationNeeded = false;
        _this.IsRequestedEnabled = false;
        _this.IsDeclinedEnabled = false;
        _this.isRegistered = false;
        _this.isRequested = false;
        _this.isDeclined = false;
        _this.isDirect = false;
        _this.IsDirectEnabled = false;
        _this.currenctAirline = myTenantItem;
        _this.zeroAirline = tenantZeroItem;
        _this.entityPM = entityPM;
        _this.partnersService = new PartnersDomainService_1.PartnersDomainService();
        if (entityPM.AWBMessagesCCSTypeCode == "GLSHK") {
            _this.isGLSHK = true;
        }
        if (myTenantItem != null) {
            _this.tenantAirlineId = myTenantItem.Id;
            var isRequestedField = false;
            var isRegisteredField = false;
            if (_this.isGLSHK) {
                isRegisteredField = myTenantItem.IsGLSHKRegistered;
                isRequestedField = myTenantItem.GLSHKRegistrationRequested;
            }
            else {
                isRegisteredField = myTenantItem.IsChampRegistered;
                isRequestedField = myTenantItem.ChampRegistrationRequested;
            }
            _this.isRequested = isRequestedField;
            _this.isRegistered = isRegisteredField;
            _this.isDeclined = myTenantItem.IsDeclined;
            _this.declineNotes = myTenantItem.DeclineNotes;
        }
        _this.SetUIProperties();
        _this.LoadAirlineTenant();
        return _this;
    }
    TenantManagementAirlineItem.prototype.SetUIProperties = function () {
        var isRegistrationNeeded = false;
        var isFieldEnabled = false;
        if (this.isGLSHK) {
            isRegistrationNeeded = this.zeroAirline.GLSHKNeedsRegistration;
        }
        else {
            isRegistrationNeeded = this.zeroAirline.ChampNeedsRegistration;
        }
        if (isRegistrationNeeded) {
            if (!this.IsRegistered) {
                isFieldEnabled = true;
            }
        }
        this.IsRegistrationNeeded = isRegistrationNeeded;
        this.IsRequestedEnabled = isFieldEnabled;
        this.IsDeclinedEnabled = isFieldEnabled;
        this.UIProperties.SetEnabled("DeclineNotes", "Airline", this.IsDeclined);
    };
    Object.defineProperty(TenantManagementAirlineItem.prototype, "Code", {
        get: function () { return this.zeroAirline.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementAirlineItem.prototype, "EnglishName", {
        get: function () { return this.zeroAirline.EnglishName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementAirlineItem.prototype, "Prefix", {
        get: function () { return this.zeroAirline.Prefix; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementAirlineItem.prototype, "RegistrationNotes", {
        get: function () { return this.zeroAirline.RegistrationNotes; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementAirlineItem.prototype, "IsRegistered", {
        get: function () { return this.isRegistered; },
        set: function (newValue) {
            var _this = this;
            if (this.isRegistered != newValue) {
                this.isRegistered = newValue;
                this.SetUIProperties();
                var message = newValue ? "Registering Airline..." : "UnRegistering Airline...";
                var loggedContactName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                this.CurrentSession.StartBusyIndicator(message);
                this.partnersService.RegisteringAirline(newValue, this.tenantAirlineId, this.zeroAirline.Id, this.entityPM.Id, this.entityPM.AWBMessagesCCSTypeCode, loggedContactName).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementAirlineItem.prototype, "IsRequested", {
        get: function () { return this.isRequested; },
        set: function (newValue) {
            var _this = this;
            if (this.isRequested != newValue) {
                this.isRequested = newValue;
                this.CurrentSession.StartBusyIndicator("Request Airline...");
                this.partnersService.RegistrationRequested(newValue, this.tenantAirlineId, this.zeroAirline.Id, this.entityPM.Id, this.entityPM.AWBMessagesCCSTypeCode).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementAirlineItem.prototype, "IsDeclined", {
        get: function () { return this.isDeclined; },
        set: function (newValue) {
            var _this = this;
            if (this.isDeclined != newValue) {
                this.isDeclined = newValue;
                this.SetUIProperties();
                this.CurrentSession.StartBusyIndicator("Decline Airline...");
                this.partnersService.SetIsDeclined(newValue, this.DeclineNotes, this.tenantAirlineId, this.zeroAirline.Id, this.entityPM.Id, this.entityPM.AWBMessagesCCSTypeCode).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.SetUIProperties();
                        _this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantManagementAirlineItem.prototype, "DeclineNotes", {
        get: function () { return this.declineNotes; },
        set: function (newValue) {
            if (this.declineNotes != newValue) {
                this.declineNotes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    TenantManagementAirlineItem.prototype.DeclineNotesLostFocusMethod = function (notes) {
        var _this = this;
        var myOriginNotes = null;
        if (this.currenctAirline != null) {
            myOriginNotes = this.currenctAirline.DeclineNotes;
        }
        if (notes != myOriginNotes) {
            this.DeclineNotes = notes;
            this.CurrentSession.StartBusyIndicator("Decline Airline...");
            this.partnersService.SetIsDeclined(this.IsDeclined, this.DeclineNotes, this.tenantAirlineId, this.zeroAirline.Id, this.entityPM.Id, this.entityPM.AWBMessagesCCSTypeCode).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    Object.defineProperty(TenantManagementAirlineItem.prototype, "IsDirect", {
        get: function () { return this.isDirect; },
        set: function (newValue) {
            var _this = this;
            if (this.isDirect != newValue) {
                this.isDirect = newValue;
                this.CurrentSession.StartBusyIndicator("Updating Participant...");
                this.partnersService.SetIsDirect(newValue, this.tenantAirlineId, this.zeroAirline.Id, this.entityPM.Id, this.entityPM.AWBMessagesCCSTypeCode).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    TenantManagementAirlineItem.prototype.LoadAirlineTenant = function () {
        var _this = this;
        var service = new GlobalDomainService_1.GlobalDomainService();
        service.GetAirlineTenantExistsForAirline(this.zeroAirline.Code).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.airlineTenant = myResponse.Result;
                if (_this.airlineTenant != null) {
                    _this.IsDirectEnabled = true;
                    _this.GetParticipant();
                }
            }
        });
    };
    TenantManagementAirlineItem.prototype.GetParticipant = function () {
        var _this = this;
        this.partnersService.GetIsDirect(this.entityPM.Id, this.airlineTenant.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.isDirect = myResponse.Result;
            }
        });
    };
    return TenantManagementAirlineItem;
}(BaseComponent_1.BaseComponent));
exports.TenantManagementAirlineItem = TenantManagementAirlineItem;
//# sourceMappingURL=CCSSettingsTabComponent.js.map