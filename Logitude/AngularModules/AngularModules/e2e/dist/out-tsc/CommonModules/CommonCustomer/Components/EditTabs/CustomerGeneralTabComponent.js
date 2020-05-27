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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var ImageLibraryService_1 = require("../../../../Common/Services/Others/ImageLibraryService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CustomerAdditionalServicePM_1 = require("../../../../Common/EntityPMs/CustomerAdditionalServicePM");
var CustomerProductPM_1 = require("../../../../Common/EntityPMs/CustomerProductPM");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var RankListService_1 = require("../../../../Common/Services/StandardLists/RankListService");
var AdditionalServiceListService_1 = require("../../../../Common/Services/StandardLists/AdditionalServiceListService");
var Tools_2 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityPMService_1 = require("../../../../Infrastructure/Services/EntityPMService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var CustomerCompetitorPM_1 = require("../../../../Common/EntityPMs/CustomerCompetitorPM");
var CompetitorListService_1 = require("../../../../Common/Services/StandardLists/CompetitorListService");
var GroupByPipe_1 = require("../../../../Infrastructure/Pipes/GroupByPipe");
var ProductTypeListService_1 = require("../../../../Common/Services/StandardLists/ProductTypeListService");
var LeadSourceListService_1 = require("../../../../Common/Services/StandardLists/LeadSourceListService");
var UserListService_1 = require("../../../../Common/Services/StandardLists/UserListService");
var CustomerFieldsUpdateSettingListService_1 = require("../../../../Common/Services/StandardLists/CustomerFieldsUpdateSettingListService");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var CustomerGeneralTabComponent = /** @class */ (function (_super) {
    __extends(CustomerGeneralTabComponent, _super);
    function CustomerGeneralTabComponent(entityArgs, _imageLibraryService, CD, entityPMService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._imageLibraryService = _imageLibraryService;
        _this.CD = CD;
        _this.entityPMService = entityPMService;
        _this.ObjectTableName = "Customer";
        _this.LabelColumnWidth = 115;
        _this.ControlColumnWidth = 200;
        _this.EntityId = "";
        _this.EntityName = "";
        _this.AllProductTypes = [];
        _this.ImageId = "";
        _this.customerFieldsUpdateSettingListService = new CustomerFieldsUpdateSettingListService_1.CustomerFieldsUpdateSettingListService();
        _this.customerFieldsUpdateSettingList = [];
        _this.IsUnifreightEditable = false;
        _this.DataContext = _this;
        _this.LogoInput = Guid_1.Guid.NewRandomString();
        _this.IsShowMessageComplate = false;
        _this.IsShowProgressLoading = false;
        _this.ScreenCode = "Customer.AdditionalFields";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SessionEvent = null;
        _this.Retries = 0;
        _this.ProducttypeList = [];
        _this.ProductsToggleButtonList = [];
        _this.ProductsObslist = [];
        _this.ToggleButtonList = [];
        _this.ToggleButtonListService = [];
        _this.Services = [];
        _this.CompetitorToggleButtonList = [];
        _this.Competitors = [];
        _this.AllCompetitors = [];
        _this.isRAFieldsVisibile = false;
        _this.IsBlockMessageVisible = false;
        _this.IsSplitted_AccountManager = false;
        _this.IsSplitted_SalesmanUser = false;
        _this.IsSplitted_Forwarder = false;
        _this.IsSplitted_CustomsAgent = false;
        _this.IsSplitted_Mediator = false;
        _this.ProductsToggleButtonListFilterd = [];
        _this.checkedProducts = [];
        _this.displayDelete = false;
        _this.noServicesVisibility = false;
        _this.noCompetitorVisibility = false;
        _this.noProductsVisibility = false;
        _this.servicesVisibility = false;
        _this.RankListArr = [];
        _this.searchText = null;
        _this.searchTextAdditionalService = null;
        _this.SearchTextAdditionalServiceCustomerId = "SearchTextAdditionalServiceId";
        _this.SearchTextCompetitorsCustomerId = "SearchTextCompetitorsId";
        _this.searchTextCompetitor = null;
        _this.SearchProductsModeCustomerId = "SearchProductsModeId";
        _this.SearchProductDropButtonCustomerId = "SearchProductDropButtonId";
        _this.SearchTextCompetitorsDropButtonCustomerId = "SearchTextCompetitorsDropButtonId";
        _this.SearchTextAdditionalServiceModeDropButtonCustomerId = "SearchTextAdditionalServiceModeDropButtonId";
        _this.RankSourceText2 = "./Images/Icons/StarGray.png";
        _this.RankSourceText1 = "./Images/Icons/StarGray.png";
        _this.RankSourceText3 = "./Images/Icons/StarGray.png";
        // More Button
        _this.IsMoreButtonVisible_AccountManager = false;
        _this.IsMoreButtonVisible_Salesman = false;
        _this.IsMoreButtonVisible_Forwarder = false;
        _this.IsMoreButtonVisible_CustomsAgent = false;
        _this.IsMoreButtonVisible_Mediator = false;
        _this.EntityPM = entityArgs.EntityPM;
        _this.ImageId = _this.EntityPM.ImageDetailId;
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.IndustryId = _this.EntityPM.IndustryId;
        _this.EntityName = "Customer";
        _this.EntityId = _this.EntityPM.Id;
        _this.LeadSourceId = _this.EntityPM.LeadSourceId;
        _this.Listen();
        var myService = new ProductTypeListService_1.ProductTypeListService();
        myService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.AllProductTypes = myResponse.Result.filter(function (o) { return !o.InActive; }).sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; });
            }
        });
        _this.customerFieldsUpdateSettingListService.getAll().subscribe(function (response) {
            if (!response.HasError) {
                _this.customerFieldsUpdateSettingList = response.Result;
            }
            _this.SetUIProperties();
            _this.CloseScreen();
        });
        _this.RunComponent();
        _this.AllCompetitors = new Array();
        _this.rankListService = new RankListService_1.RankListService();
        _this.rankListService.getAllFromCache().subscribe(function (result) {
            _this.RankListArr = result.Result;
        });
        return _this;
    }
    CustomerGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "EntityActivated") {
                    _this.CloseScreen();
                }
            });
        }
    };
    CustomerGeneralTabComponent.prototype.ngOnDestroy = function () {
        Tools_2.AppTool.KillEventEmitter(this.SessionEvent);
    };
    CustomerGeneralTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
            this.GetProductsList();
            this.GetAdditionalSerivceList();
            this.GetCompetitorList();
            this.RankSource1();
            this.RankSource2();
            this.RankSource3();
            this.SetMoreButtonsVisibility();
        }
        else {
            this.RunComponentTimer();
        }
    };
    CustomerGeneralTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    CustomerGeneralTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.Run(_this.entityArgs.EntityPM, _this.entityArgs.ObjectTableName, _this.ScreenCode);
        });
    };
    CustomerGeneralTabComponent.prototype.GetProductsList = function () {
        var _this = this;
        var service = new CommonDomainService_1.CommonDomainService();
        service.GetProductTypesByTenant(this.TenantPM.Id).subscribe(function (myResult) {
            _this.ProductsToggleButtonList = [];
            _this.ProducttypeList = myResult;
            myResult.forEach(function (i) {
                if (!i.InActive) {
                    var item = new ProductTypeList();
                    item.Code = i.Code;
                    item.Name = i.Name;
                    item.InActive = i.InActive;
                    item.Id = i.Id;
                    item.SearchFields = i.SearchFields;
                    _this.ProductsToggleButtonList.push(i);
                }
                else {
                }
            });
            _this.ProductsToggleButtonList.sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; });
            _this.BuildProductsToggleButtonList();
            _this.BuildProductsObsList();
        });
    };
    CustomerGeneralTabComponent.prototype.BuildProductsObsList = function () {
        var _this = this;
        this.ProductsObslist = [];
        this.EntityPM.CustomerProducts.forEach(function (item) {
            var newItem = new ProductObslistItemClass(item, _this);
            _this.ProductsObslist.push(newItem);
        });
        this.NoProductsVisibility = this.ProductsObslist.length == 0 ? true : false;
    };
    CustomerGeneralTabComponent.prototype.GetAdditionalSerivceList = function () {
        var _this = this;
        var AddtionalService = new AdditionalServiceListService_1.AdditionalServiceListService();
        AddtionalService.getAllFromCache().subscribe(function (result) {
            _this.ToggleButtonListService = [];
            _this.ToggleButtonListService = result.Result.filter(function (s) { return !s.InActive; });
            _this.ToggleButtonListService.sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; });
            _this.BuildToggleButtonList();
            _this.BuildObsList();
        });
    };
    CustomerGeneralTabComponent.prototype.BuildToggleButtonList = function () {
        var _this = this;
        this.ToggleButtonList = [];
        var data = this.ToggleButtonListService;
        if (!Tools_2.AppTool.IsNullOrEmpty(this.SearchTextAdditionalService)) {
            data = this.ToggleButtonListService.filter(function (f) { return f.Name.toLowerCase().indexOf(_this.SearchTextAdditionalService.toLowerCase()) > -1; });
        }
        data.forEach(function (item) {
            _this.ToggleButtonList.push(new ServiceItemClass(item, _this.EntityPM, _this));
        });
    };
    CustomerGeneralTabComponent.prototype.BuildObsList = function () {
        var _this = this;
        this.Services = [];
        this.EntityPM.CustomerAdditionalServices.forEach(function (item) {
            _this.Services.push(new ServiceViewModelData(item, _this));
        });
        this.NoServicesVisibility = this.Services.length == 0 ? true : false;
        this.ServicesVisibility = this.Services.length == 0 ? false : true;
    };
    CustomerGeneralTabComponent.prototype.GetCompetitorList = function () {
        var _this = this;
        var competitorListService = new CompetitorListService_1.CompetitorListService();
        competitorListService.getAll().subscribe(function (result) {
            _this.AllCompetitors = result.Result;
            _this.BuildCompetitorToggleButtonList();
            _this.BuildCompetitorsObsList();
        });
    };
    CustomerGeneralTabComponent.prototype.BuildCompetitorToggleButtonList = function () {
        var _this = this;
        this.CompetitorToggleButtonList = [];
        var data = this.AllCompetitors;
        if (!Tools_2.AppTool.IsNullOrEmpty(this.SearchTextCompetitor)) {
            data = this.AllCompetitors.filter(function (f) { return f.Name.toLowerCase().indexOf(_this.SearchTextCompetitor.toLowerCase()) > -1; });
        }
        data.forEach(function (item) {
            _this.CompetitorToggleButtonList.push(new CompetitorItemClass(item, _this.EntityPM, _this));
        });
    };
    CustomerGeneralTabComponent.prototype.BuildCompetitorsObsList = function () {
        var _this = this;
        this.Competitors = [];
        this.EntityPM.CustomerCompetitors.forEach(function (item) {
            _this.Competitors.push(new CompetitorViewModelData(item, _this));
        });
        if (this.Competitors.length == 0)
            this.NoCompetitorVisibility = true;
        else {
            this.NoCompetitorVisibility = false;
        }
    };
    CustomerGeneralTabComponent.prototype.RankSource1 = function (rank) {
        if (rank === void 0) { rank = null; }
        if (rank != null) {
            this.RankSourceText1 = "./Images/Icons/StarOrange.png";
        }
        else {
            var myResult = null;
            if (this.EntityPM != null) {
                var RankCode = this.EntityPM.RankCode;
                switch (RankCode) {
                    case "1": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }
                    case "2": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }
                    case "3": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarOrange.png";
                        break;
                    }
                    default: {
                        this.RankSourceText1 = "./Images/Icons/StarGray.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    }
                }
            }
        }
    };
    CustomerGeneralTabComponent.prototype.RankSource2 = function (rank) {
        if (rank === void 0) { rank = null; }
        if (rank != null) {
            this.RankSourceText2 = "./Images/Icons/StarOrange.png";
        }
        else {
            var myResult = "./Images/Icons/StarOrange.png";
            if (this.EntityPM != null) {
                var RankCode = this.EntityPM.RankCode;
                switch (RankCode) {
                    case "1": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }
                    case "2": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }
                    case "3": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarOrange.png";
                        break;
                    }
                    default: {
                        this.RankSourceText1 = "./Images/Icons/StarGray.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    }
                }
            }
        }
    };
    CustomerGeneralTabComponent.prototype.RankSource3 = function (rank) {
        if (rank === void 0) { rank = null; }
        if (rank != null) {
            this.RankSourceText3 = "./Images/Icons/StarOrange.png";
            this.RankSourceText2 = "./Images/Icons/StarOrange.png";
        }
        else {
            var RankCode = this.EntityPM.RankCode;
            switch (RankCode) {
                case "1": {
                    this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText2 = "./Images/Icons/StarGray.png";
                    this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    break;
                }
                case "2": {
                    this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    break;
                }
                case "3": {
                    this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText3 = "./Images/Icons/StarOrange.png";
                    break;
                }
                default: {
                    this.RankSourceText1 = "./Images/Icons/StarGray.png";
                    this.RankSourceText2 = "./Images/Icons/StarGray.png";
                    this.RankSourceText3 = "./Images/Icons/StarGray.png";
                }
            }
        }
    };
    CustomerGeneralTabComponent.prototype.SetUIProperties = function () {
        if (this.TenantPM.RegulatedAgentRegimeActivated) {
            this.isRAFieldsVisibile = true;
        }
        this.UIProperties.SetVisibility("KnownConsignor", this.ObjectTableName, this.isRAFieldsVisibile);
        this.UIProperties.SetVisibility("KCExpirationDate", this.ObjectTableName, this.isRAFieldsVisibile);
        this.SearchTextCompetitorsDropButtonCustomerId += this.CurrentSession.GetNewId("SearchTextCompetitorsDropButtonId_1");
        this.SearchTextCompetitorsCustomerId += this.CurrentSession.GetNewId("SearchTextCompetitorsId_1");
        this.SearchTextAdditionalServiceModeDropButtonCustomerId += this.CurrentSession.GetNewId("SearchTextAdditionalServiceModeDropButtonId_1");
        this.SearchTextAdditionalServiceCustomerId += this.CurrentSession.GetNewId("SearchTextAdditionalServiceId_1");
        this.SearchProductDropButtonCustomerId += this.CurrentSession.GetNewId("SearchProductDropButtonId_1");
        this.SearchProductsModeCustomerId += this.CurrentSession.GetNewId("SearchProductsModeId_1");
        this.SetUIProperties_Partners();
    };
    CustomerGeneralTabComponent.prototype.SetUIProperties_Partners = function () {
        this.SetUIProperties_AccountManager();
        this.SetUIProperties_Salesman();
        this.SetUIProperties_Forwarder();
        this.SetUIProperties_CustomsAgent();
        this.SetUIProperties_Mediator();
    };
    CustomerGeneralTabComponent.prototype.SetUIProperties_AccountManager = function () {
        var isEnabled = false;
        var isSplitted = false;
        var myPipe = new GroupByPipe_1.GroupByPipe();
        var Forwarders = myPipe.transform(this.EntityPM.CustomerAccountManagerByProducts.filter(function (f) { return f.AccountManagerId != null; }), "AccountManagerId");
        if (Forwarders.length == 0) {
            isEnabled = true;
        }
        else if (Forwarders.length > 1) {
            isSplitted = true;
        }
        this.IsSplitted_AccountManager = isSplitted;
        this.UIProperties.SetEnabled("AccountManagerUserId", this.ObjectTableName, isEnabled);
    };
    CustomerGeneralTabComponent.prototype.SetUIProperties_Salesman = function () {
        var isEnabled = false;
        var isSplitted = false;
        var myPipe = new GroupByPipe_1.GroupByPipe();
        var Salesmens = myPipe.transform(this.EntityPM.CustomerSalesmanByProducts.filter(function (f) { return f.SalesmanUserId != null; }), "SalesmanUserId");
        if (Salesmens.length == 0) {
            isEnabled = true;
        }
        else if (Salesmens.length > 1) {
            isSplitted = true;
        }
        this.IsSplitted_SalesmanUser = isSplitted;
        //this.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, isEnabled);
        var salesmanSettings = this.customerFieldsUpdateSettingList.filter(function (f) { return f.ObjectFieldName == "SalesmanUserId"; })[0];
        if (salesmanSettings != null) {
            if (salesmanSettings.UpdateDirection == "UNFU" && this.EntityPM.CustomerStatusCode != "POT") {
                this.IsUnifreightEditable = true;
                this.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, false);
            }
        }
    };
    CustomerGeneralTabComponent.prototype.SetUIProperties_Forwarder = function () {
        var isEnabled = false;
        var isSplitted = false;
        var myPipe = new GroupByPipe_1.GroupByPipe();
        var Forwarders = myPipe.transform(this.EntityPM.CustomerForwarderByProducts.filter(function (f) { return f.ForwarderId != null; }), "ForwarderId");
        if (Forwarders.length == 0) {
            isEnabled = true;
        }
        else if (Forwarders.length > 1) {
            isSplitted = true;
        }
        this.IsSplitted_Forwarder = isSplitted;
        this.UIProperties.SetEnabled("ForwarderId", this.ObjectTableName, isEnabled);
    };
    CustomerGeneralTabComponent.prototype.SetUIProperties_CustomsAgent = function () {
        var isEnabled = false;
        var isSplitted = false;
        var myPipe = new GroupByPipe_1.GroupByPipe();
        var Forwarders = myPipe.transform(this.EntityPM.CustomerCustomsAgentByProducts.filter(function (f) { return f.CustomsAgentId != null; }), "CustomsAgentId");
        if (Forwarders.length == 0) {
            isEnabled = true;
        }
        else if (Forwarders.length > 1) {
            isSplitted = true;
        }
        this.IsSplitted_CustomsAgent = isSplitted;
        this.UIProperties.SetEnabled("CustomsAgentId", this.ObjectTableName, isEnabled);
    };
    CustomerGeneralTabComponent.prototype.SetUIProperties_Mediator = function () {
        var isEnabled = false;
        var isSplitted = false;
        var myPipe = new GroupByPipe_1.GroupByPipe();
        var Forwarders = myPipe.transform(this.EntityPM.CustomerMediatorByProducts.filter(function (f) { return f.MediatorId != null; }), "MediatorId");
        if (Forwarders.length == 0) {
            isEnabled = true;
        }
        else if (Forwarders.length > 1) {
            isSplitted = true;
        }
        this.IsSplitted_Mediator = isSplitted;
        this.UIProperties.SetEnabled("MediatorId", this.ObjectTableName, isEnabled);
    };
    CustomerGeneralTabComponent.prototype.CloseScreen = function () {
        var enabled = true;
        if (this.TenantPM.IsHybrid && (this.EntityPM.CustomerStatusCode == "ACT" || this.EntityPM.CustomerStatusCode == "WAC")) {
            enabled = false;
            this.IsBlockMessageVisible = true;
        }
        this.UIProperties.SetEnabled("EnglishName", "Customer", enabled);
        this.UIProperties.SetEnabled("LocalName", "Customer", enabled);
        this.UIProperties.SetEnabled("VatNumber", "Customer", enabled);
        this.UIProperties.SetEnabled("PaymentTermId", "Customer", enabled);
        this.UIProperties.SetEnabled("AccountManagerUserId", "Customer", enabled);
        this.UIProperties.SetEnabled("ClassifierId", "Customer", enabled);
        this.UIProperties.SetEnabled("CollectorId", "Customer", enabled);
        this.UIProperties.SetEnabled("ForwarderId", "Customer", enabled);
        this.UIProperties.SetEnabled("CustomsAgentId", "Customer", enabled);
        this.UIProperties.SetEnabled("MediatorId", "Customer", enabled);
        this.UIProperties.SetEnabled("KnownConsignor", "Customer", enabled);
        this.UIProperties.SetEnabled("KCExpirationDate", "Customer", enabled);
    };
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "StartWorkingDate", {
        get: function () { return this.EntityPM.StartWorkingDate; },
        set: function (value) { this.EntityPM.StartWorkingDate = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "LeadDescription", {
        get: function () { return this.EntityPM.LeadDescription; },
        set: function (value) { this.EntityPM.LeadDescription = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "CustomerSizeId", {
        get: function () { return this.EntityPM.CustomerSizeId; },
        set: function (value) { this.EntityPM.CustomerSizeId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "RegionId", {
        get: function () { return this.EntityPM.RegionId; },
        set: function (value) { this.EntityPM.RegionId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "IndustryId", {
        get: function () { return this.EntityPM.IndustryId; },
        set: function (value) { this.EntityPM.IndustryId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "LeadSourceId", {
        get: function () { return this.EntityPM.LeadSourceId; },
        set: function (value) {
            if (this.EntityPM.LeadSourceId != value) {
                this.EntityPM.LeadSourceId = value;
                this.SetLeadSourceName(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerGeneralTabComponent.prototype.SetLeadSourceName = function (leadSourceId) {
        var _this = this;
        var service = new LeadSourceListService_1.LeadSourceListService();
        service.getSingleFromCache(leadSourceId).subscribe(function (result) {
            if (!result.HasError) {
                if (result.Result != null)
                    _this.EntityPM.LeadSourceName = result.Result.Name;
                else
                    _this.EntityPM.LeadSourceName = null;
            }
        });
    };
    CustomerGeneralTabComponent.prototype.ProductsToggleButtonClicked = function (item, i) {
        if (item.IsChecked == true && !this.EntityPM.CustomerProducts.filter(function (d) { return d.ProductTypeCode == item.Code; })) {
            this.BuildProductsToggleButtonList();
        }
        this.BuildProductsObsList();
    };
    CustomerGeneralTabComponent.prototype.ServicesToggleButtonClicked = function (item, i) {
        if (item.IsChecked == true && !this.EntityPM.CustomerAdditionalServices.filter(function (d) { return d.AdditionalServiceId == item.Id; })[0]) {
            this.BuildToggleButtonList();
        }
        this.BuildObsList();
    };
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "CheckedProducts", {
        get: function () {
            this.checkedProducts = [];
            for (var i = 0; i < this.ProductsToggleButtonListFilterd.length; i++) {
                if (this.ProductsToggleButtonListFilterd[i].IsChecked == true) {
                    this.checkedProducts.push(this.ProductsToggleButtonListFilterd[i]);
                }
            }
            return this.checkedProducts;
        },
        enumerable: true,
        configurable: true
    });
    CustomerGeneralTabComponent.prototype.getProductsTitle = function (Item) {
        return "Last shipment date: " + Item.LastShipmentDate;
    };
    CustomerGeneralTabComponent.prototype.ExistingItemNotes = function (Item) {
        return Tools_2.AppTool.IsNullOrEmpty(Item.Notes);
    };
    CustomerGeneralTabComponent.prototype.DeleteItemServiceObsList = function (item) {
        if (this.EntityPM.CustomerAdditionalServices.filter(function (p) { return p.AdditionalServiceId == item.Id; })[0] != null)
            this.EntityPM.RemoveCustomerAdditionalServicePM(this.EntityPM.CustomerAdditionalServices.filter(function (d) { return d.AdditionalServiceId == item.Id; })[0]);
        this.BuildToggleButtonList();
        this.BuildObsList();
    };
    CustomerGeneralTabComponent.prototype.deleteItemProductsObsList = function (item) {
        if (this.EntityPM.CustomerProducts.filter(function (p) { return p.ProductTypeCode == item.ProductTypeCode; })[0] != null)
            this.EntityPM.RemoveCustomerProductPM(this.EntityPM.CustomerProducts.filter(function (d) { return d.ProductTypeCode == item.ProductTypeCode; })[0]);
        this.BuildProductsToggleButtonList();
        this.BuildProductsObsList();
    };
    CustomerGeneralTabComponent.prototype.EditItemServiceObsList = function (item) {
        var _this = this;
        var editWindow = new LogitudeWindow_1.LogitudeWindow();
        editWindow.Title = "Edit " + item.AdditionalServiceName + " Additional Service";
        editWindow.Width = 500;
        editWindow.Height = 350;
        editWindow.WindowArgs = item;
        this.Clone(item);
        editWindow.WindowClosed.subscribe(function (result) {
            if (result == "Cancel") {
                _this.RejectChanges();
            }
            else {
            }
        });
        var entityResource = new EntityResourceService_1.EntityResourceService();
        entityResource.getEntityResourceByTableName("CustomerAdditionalService", 0).subscribe(function (p) {
            editWindow.Show('./CommonModules/CommonCustomer/Components/EditTabs/EditCustomerAdditionalServiceComponent');
        });
    };
    CustomerGeneralTabComponent.prototype.EditCompetitor = function (Item) {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: Item.CompetitorId, ObjectTableName: 'Competitor', BackButtonLabel: "CRM Details" });
            cmpRef.instance.BackCompleted.subscribe(function ($event) {
                _this.GetCompetitorList();
            });
        });
    };
    CustomerGeneralTabComponent.prototype.ImageUploadedCompleted = function (code) {
        this.ImageId = code;
        this.EntityPM.ImageDetailId = code;
    };
    CustomerGeneralTabComponent.prototype.Checked = function (code) {
        return code;
    };
    CustomerGeneralTabComponent.prototype.AddAdditionalService = function () {
        var _this = this;
        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity("AdditionalService").then(function (response) {
            var args = new EntityArgs_1.EntityArgs();
            args.EntityPM = response;
            args.ObjectTableName = "AdditionalService";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate("AdditionalService"));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewEntityWindowClosed($event); });
            logWindow.Show(componentPath);
        });
    };
    CustomerGeneralTabComponent.prototype.AddCompetitor = function () {
        var _this = this;
        var entityResource = new EntityResourceService_1.EntityResourceService();
        entityResource.getEntityResourceByTableName("Competitor", 0).subscribe(function (p) {
            var componentPath = "./Common/Components/Maintenance/CompetitorComponent";
            var args = new EntityArgs_1.EntityArgs();
            args.ObjectTableName = "Competitor";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Height = 568;
            logWindow.Width = 958;
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate("Competitor"));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewEntityWindowClosedCompetitor($event); });
            logWindow.Show(componentPath);
        });
    };
    CustomerGeneralTabComponent.prototype.Clone = function (EntityPM) {
        this.myCloner = new Cloner_1.Cloner(EntityPM);
        this.myCloner.AddField('InUse');
        this.myCloner.AddField('Potential');
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(EntityPM);
        this.myCloner.AddEntity(this.EntityPM);
    };
    CustomerGeneralTabComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    CustomerGeneralTabComponent.prototype.SetActivity = function (value) {
        this.EntityPM.ActivityWatch = value;
    };
    CustomerGeneralTabComponent.prototype.OnNewEntityWindowClosedCompetitor = function (event) {
        if (event == "OK") {
            this.GetCompetitorList();
        }
    };
    CustomerGeneralTabComponent.prototype.OnNewEntityWindowClosed = function (event) {
        this.GetAdditionalSerivceList();
    };
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "NoServicesVisibility", {
        get: function () { return this.noServicesVisibility; },
        set: function (value) { this.noServicesVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "NoCompetitorVisibility", {
        get: function () { return this.noCompetitorVisibility; },
        set: function (value) { this.noCompetitorVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "NoProductsVisibility", {
        get: function () { return this.noProductsVisibility; },
        set: function (value) { this.noProductsVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "ServicesVisibility", {
        get: function () { return this.servicesVisibility; },
        set: function (value) { this.servicesVisibility = value; },
        enumerable: true,
        configurable: true
    });
    CustomerGeneralTabComponent.prototype.ChangeRank = function (code) {
        var filteredData = this.RankListArr.filter(function (a) { return a.Code === code && a.Tenant == SessionLocator_1.SessionLocator.TenantPM.Id; })[0];
        this.EntityPM.RankCode = filteredData.Code;
        this.EntityPM.RankName = filteredData.Name;
        this.EntityPM.RankId = filteredData.Id;
        this.RankSource1();
        this.RankSource2();
        this.RankSource3();
    };
    CustomerGeneralTabComponent.prototype.clickItem = function (item) { };
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            this.searchText = newValue;
            this.BuildProductsToggleButtonList();
            this.CD.detectChanges();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "SearchTextAdditionalService", {
        get: function () { return this.searchTextAdditionalService; },
        set: function (newValue) {
            this.searchTextAdditionalService = newValue;
            this.BuildToggleButtonList();
            this.CD.detectChanges();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "SearchTextCompetitor", {
        get: function () { return this.searchTextCompetitor; },
        set: function (newValue) {
            this.searchTextCompetitor = newValue;
            this.BuildCompetitorToggleButtonList();
            this.CD.detectChanges();
        },
        enumerable: true,
        configurable: true
    });
    CustomerGeneralTabComponent.prototype.setToggleButtonMenuTemp = function () {
        var ToggleBTN = document.getElementById(this.SearchProductDropButtonCustomerId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
    };
    CustomerGeneralTabComponent.prototype.setToggleButtonMenuAdditionalServicesTemp = function () {
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonCustomerId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
    };
    CustomerGeneralTabComponent.prototype.setToggleButtonMenuCompetitorTemp = function () {
        var ToggleBTN = document.getElementById(this.SearchTextCompetitorsDropButtonCustomerId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
    };
    CustomerGeneralTabComponent.prototype.setToggleButtonMenu = function () {
        var ToggleBTN = document.getElementById(this.SearchProductDropButtonCustomerId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    CustomerGeneralTabComponent.prototype.setToggleButtonMenuAdditionalServices = function () {
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonCustomerId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    CustomerGeneralTabComponent.prototype.setToggleButtonMenuCompetitor = function () {
        var ToggleBTN = document.getElementById(this.SearchTextCompetitorsDropButtonCustomerId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "ActivityWatch", {
        get: function () { return this.EntityPM.ActivityWatch; },
        set: function (value) {
            if (this.EntityPM.ActivityWatch != value)
                this.EntityPM.ActivityWatch = value;
        },
        enumerable: true,
        configurable: true
    });
    CustomerGeneralTabComponent.prototype.ClearPlaceHolder = function () {
        var temp = document.getElementById(this.SearchProductsModeCustomerId);
        temp.placeholder = "";
        //this.SearchText = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchProductDropButtonCustomerId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
        //this.CD.detectChanges();
    };
    CustomerGeneralTabComponent.prototype.ClearPlaceHolderAdditionalService = function () {
        var temp = document.getElementById(this.SearchTextAdditionalServiceCustomerId);
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonCustomerId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
        //this.CD.detectChanges();
    };
    CustomerGeneralTabComponent.prototype.ClearPlaceHolderCompetitor = function () {
        var temp = document.getElementById(this.SearchTextCompetitorsCustomerId);
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchTextCompetitorsDropButtonCustomerId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
        //this.CD.detectChanges();
    };
    CustomerGeneralTabComponent.prototype.OnDeleteValue = function () {
        var temp = document.getElementById(this.SearchProductsModeCustomerId);
        temp.value = null;
        this.SearchText = null;
        temp.focus();
    };
    CustomerGeneralTabComponent.prototype.OnDeleteValueAddtionalService = function () {
        var temp = document.getElementById(this.SearchTextAdditionalServiceCustomerId);
        temp.value = null;
        this.SearchTextAdditionalService = null;
        temp.focus();
    };
    CustomerGeneralTabComponent.prototype.OnDeleteValueCompetitor = function () {
        var temp = document.getElementById(this.SearchTextCompetitorsCustomerId);
        temp.value = null;
        this.SearchTextCompetitor = null;
        temp.focus();
    };
    CustomerGeneralTabComponent.prototype.FillPlaceHolder = function () {
        if (!this.SearchText) {
            var temp = document.getElementById(this.SearchProductsModeCustomerId);
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.SearchProductDropButtonCustomerId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    CustomerGeneralTabComponent.prototype.FillPlaceHolderAdditionalService = function () {
        if (!this.SearchTextAdditionalService) {
            var temp = document.getElementById(this.SearchTextAdditionalServiceCustomerId);
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonCustomerId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    CustomerGeneralTabComponent.prototype.FillPlaceHoldeCompetitor = function () {
        if (!this.SearchTextCompetitor) {
            var temp = document.getElementById(this.SearchTextCompetitorsCustomerId);
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.SearchTextCompetitorsDropButtonCustomerId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    CustomerGeneralTabComponent.prototype.DeleteItemCompetitorList = function (Item) {
        if (this.EntityPM.CustomerCompetitors.filter(function (p) { return p.CompetitorId == Item.CompetitorId; })[0] != null)
            this.EntityPM.RemoveCustomerCompetitorPM(this.EntityPM.CustomerCompetitors.filter(function (d) { return d.CompetitorId == Item.CompetitorId; })[0]);
        this.BuildCompetitorToggleButtonList();
        this.BuildCompetitorsObsList();
    };
    CustomerGeneralTabComponent.prototype.ngOnInit = function () {
    };
    CustomerGeneralTabComponent.prototype.SearchTextChanged = function (text) {
        this.SearchText = text;
    };
    CustomerGeneralTabComponent.prototype.TextChanged = function (text) {
        this.SearchTextAdditionalService = text;
    };
    CustomerGeneralTabComponent.prototype.BuildProductsToggleButtonList = function () {
        var _this = this;
        var data = null;
        if (this.SearchText == null || this.SearchText == "") {
            data = this.ProductsToggleButtonList;
        }
        else {
            data = this.ProductsToggleButtonList.filter(function (f) { return f.Name.toLowerCase().indexOf(_this.SearchText.toLowerCase()) > -1; });
        }
        this.ProductsToggleButtonListFilterd = [];
        data.forEach(function (i) {
            var itemTogleButton = new ProductTypeItemClass(i, _this.EntityPM, _this, _this.ProducttypeList);
            _this.ProductsToggleButtonListFilterd.push(itemTogleButton);
        });
    };
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "EnglishName", {
        //Props
        get: function () { return this.EntityPM.EnglishName; },
        set: function (newValue) {
            if (this.EntityPM.EnglishName != newValue) {
                this.EntityPM.EnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (newValue) {
            if (this.EntityPM.LocalName != newValue) {
                this.EntityPM.LocalName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "VatNumber", {
        get: function () { return this.EntityPM.VatNumber; },
        set: function (newValue) {
            if (this.EntityPM.VatNumber != newValue) {
                this.EntityPM.VatNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "PaymentTermId", {
        get: function () { return this.EntityPM.PaymentTermId; },
        set: function (newValue) {
            if (this.EntityPM.PaymentTermId != newValue) {
                this.EntityPM.PaymentTermId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "Website", {
        get: function () { return this.EntityPM.Website; },
        set: function (newValue) {
            if (this.EntityPM.Website != newValue) {
                this.EntityPM.Website = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "KnownConsignor", {
        get: function () { return this.EntityPM.KnownConsignor; },
        set: function (newValue) {
            if (this.EntityPM.KnownConsignor != newValue) {
                this.EntityPM.KnownConsignor = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "KCExpirationDate", {
        get: function () { return this.EntityPM.KCExpirationDate; },
        set: function (newValue) {
            if (this.EntityPM.KCExpirationDate != newValue) {
                this.EntityPM.KCExpirationDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "AccountManagerUserId", {
        // Responsibilities
        get: function () { return this.EntityPM.AccountManagerUserId; },
        set: function (value) {
            if (this.EntityPM.AccountManagerUserId != value) {
                this.EntityPM.AccountManagerUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "SalesmanUserId", {
        get: function () { return this.EntityPM.SalesmanUserId; },
        set: function (value) {
            if (this.EntityPM.SalesmanUserId != value) {
                this.EntityPM.SalesmanUserId = value;
                this.setSalesmanName(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerGeneralTabComponent.prototype.setSalesmanName = function (salesmanId) {
        var _this = this;
        var service = new UserListService_1.UserListService();
        service.getSingleFromCache(salesmanId).subscribe(function (result) {
            if (!result.HasError) {
                if (result.Result != null)
                    _this.EntityPM.SalesmanUserEnglishName = result.Result.EnglishName;
                else
                    _this.EntityPM.SalesmanUserEnglishName = null;
            }
        });
    };
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "ClassifierId", {
        get: function () { return this.EntityPM.ClassifierId; },
        set: function (value) {
            if (this.EntityPM.ClassifierId != value) {
                this.EntityPM.ClassifierId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "CollectorId", {
        get: function () { return this.EntityPM.CollectorId; },
        set: function (value) {
            if (this.EntityPM.CollectorId != value) {
                this.EntityPM.CollectorId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "ForwarderId", {
        // Partners
        get: function () { return this.EntityPM.ForwarderId; },
        set: function (value) {
            if (this.EntityPM.ForwarderId != value) {
                this.EntityPM.ForwarderId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "CustomsAgentId", {
        get: function () { return this.EntityPM.CustomsAgentId; },
        set: function (value) {
            if (this.EntityPM.CustomsAgentId != value) {
                this.EntityPM.CustomsAgentId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerGeneralTabComponent.prototype, "MediatorId", {
        get: function () { return this.EntityPM.MediatorId; },
        set: function (value) {
            if (this.EntityPM.MediatorId != value) {
                this.EntityPM.MediatorId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerGeneralTabComponent.prototype.SetMoreButtonsVisibility = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "CUSTOMERACCOUNTMANAGERBYPRODUCT")) {
            this.IsMoreButtonVisible_AccountManager = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "CUSTOMERSALESMANBYPRODUCT")) {
            this.IsMoreButtonVisible_Salesman = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "CUSTOMERFORWARDERBYPRODUCT")) {
            this.IsMoreButtonVisible_Forwarder = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "CUSTOMERCUSTOMSAGENTBYPRODUCT")) {
            this.IsMoreButtonVisible_CustomsAgent = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "CUSTOMERMEDIATORBYPRODUCT")) {
            this.IsMoreButtonVisible_Mediator = true;
        }
    };
    CustomerGeneralTabComponent.prototype.MoreButtonClicked = function (field) {
        var _this = this;
        var windowTitle = null;
        var windowComponent = null;
        switch (field) {
            case "AccountManagerUserId": {
                windowTitle = "Customer Account Manager By Product";
                windowComponent = "./CommonModules/CommonCustomer/Components/EditTabs/MoreButtons/CustomerAccountManagerByProductSplitComponent";
                break;
            }
            case "SalesmanUserId": {
                windowTitle = "Customer Salesman By Product";
                windowComponent = "./CommonModules/CommonCustomer/Components/EditTabs/MoreButtons/CustomerSalesmanByProductSplitComponent";
                break;
            }
            case "ForwarderId": {
                windowTitle = "Customer Forwarder By Product";
                windowComponent = "./CommonModules/CommonCustomer/Components/EditTabs/MoreButtons/CustomerForwarderByProductSplitComponent";
                break;
            }
            case "CustomsAgentId": {
                windowTitle = "Customer Customs Agent By Product";
                windowComponent = "./CommonModules/CommonCustomer/Components/EditTabs/MoreButtons/CustomerCustomsAgentByProductSplitComponent";
                break;
            }
            case "MediatorId": {
                windowTitle = "Customer Mediator By Product";
                windowComponent = "./CommonModules/CommonCustomer/Components/EditTabs/MoreButtons/CustomerMediatorByProductSplitComponent";
                break;
            }
        }
        if (windowComponent != null) {
            var window = new LogitudeWindow_1.LogitudeWindow();
            window.Title = windowTitle;
            window.WindowArgs = { EntityPM: this.EntityPM, ProductTypes: this.AllProductTypes, IsUnifreightEditable: this.IsUnifreightEditable };
            window.Show(windowComponent);
            window.WindowClosed.subscribe(function (s) {
                if (s == "OK") {
                    _this.SetUIProperties_Partners();
                    _this.CloseScreen();
                }
            });
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], CustomerGeneralTabComponent.prototype, "viewContainerRef", void 0);
    CustomerGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomerGeneralTabComponent.html',
            providers: [ImageLibraryService_1.ImageLibraryService, EntityPMService_1.EntityPMService]
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, ImageLibraryService_1.ImageLibraryService, core_1.ChangeDetectorRef, EntityPMService_1.EntityPMService])
    ], CustomerGeneralTabComponent);
    return CustomerGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerGeneralTabComponent = CustomerGeneralTabComponent;
var ProductTypeItemClass = /** @class */ (function () {
    function ProductTypeItemClass(itemList, itemPM, Parent, productTypeList) {
        var _this = this;
        this.Parent = Parent;
        this.productTypeList = productTypeList;
        this.ProductTypesByTenantList = [];
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityPM = itemPM;
        this.ProductTypesByTenantList = productTypeList;
        this.entityList = itemList;
        var isCheckBoxEnabled = true;
        var isChecked = null;
        var productPM = this.entityPM.CustomerProducts.filter(function (d) { return d.ProductTypeCode == _this.entityList.Code; })[0];
        this.isChecked = false;
        if (productPM != null) {
            isChecked = true;
            this.IsChecked = true;
        }
        if (isChecked) {
            if (productPM != null) {
                var lastShipmentDate = productPM.LastShipmentDate;
                if (lastShipmentDate != null) {
                    isCheckBoxEnabled = false;
                }
            }
        }
        this.IsCheckBoxEnabled = isCheckBoxEnabled;
    }
    Object.defineProperty(ProductTypeItemClass.prototype, "Name", {
        get: function () { return this.entityList.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeItemClass.prototype, "Foreground", {
        get: function () { return this.IsChecked ? "#FF6E7172" : "#FF282E30"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeItemClass.prototype, "DirectionId", {
        get: function () { return this.entityList.Code.substr(1, 1); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeItemClass.prototype, "TransportModeId", {
        get: function () { return this.entityList.Code.substr(0, 1); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeItemClass.prototype, "IsCheckBoxEnabled", {
        get: function () {
            return this.isCheckBoxEnabled;
        },
        set: function (value) {
            if (this.isCheckBoxEnabled != value)
                this.isCheckBoxEnabled = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeItemClass.prototype, "Code", {
        get: function () {
            return this.entityList.Code;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeItemClass.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            var _this = this;
            if (this.isChecked != value) {
                this.isChecked = value;
                if (value) {
                    var newItem = new CustomerProductPM_1.CustomerProductPM(null);
                    newItem.Tenant = this.TenantPM.Id;
                    newItem.CustomerId = this.entityPM.Id;
                    newItem.ProductTypeCode = this.Code;
                    newItem.CommitmentChargeableWeight = 0;
                    newItem.PotentialChargeableWeight = 0;
                    newItem.CommitmentTEU = 0;
                    newItem.PotentialTEU = 0;
                    newItem.CommitmentNumberOfShipments = 0;
                    newItem.PotentialNumberOfShipments = 0;
                    newItem.CommitmentRevenue = 0;
                    newItem.PotentialRevenue = 0;
                    var type = null;
                    var ProductsToggleButtonList = [];
                    this.ProductTypesByTenantList.forEach(function (i) {
                        if (!i.InActive) {
                            var item = new ProductTypeList();
                            item.Code = i.Code;
                            item.Name = i.Name;
                            item.InActive = i.InActive;
                            item.Id = i.Id;
                            item.SearchFields = i.SearchFields;
                            ProductsToggleButtonList.push(i);
                        }
                        else {
                        }
                    });
                    ProductsToggleButtonList.sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; });
                    var typelist = ProductsToggleButtonList.filter(function (d) { return d.Code == _this.Code; })[0];
                    if (typelist != null) {
                        type = typelist.Name;
                    }
                    newItem.ProductTypeName = type;
                    var flag = true;
                    for (var i = 0; i < this.entityPM.CustomerProducts.length; i++) {
                        if (this.entityPM.CustomerProducts[i].ProductTypeCode == newItem.ProductTypeCode) {
                            flag = false;
                            break;
                        }
                    }
                    if (flag) {
                        this.entityPM.AddCustomerProductPM(newItem);
                    }
                    if (!this.entityPM.ActivityWatch)
                        this.entityPM.ActivityWatch = true;
                }
                else {
                    var item = this.entityPM.CustomerProducts.filter(function (d) { return d.ProductTypeCode == _this.Code; })[0];
                    if (item != null) {
                        if (this.entityPM.CustomerProducts.includes(item)) {
                            var CustomerProdArr = [];
                            this.entityPM.CustomerProducts.forEach(function (i) {
                                if (i.ProductTypeCode != item.ProductTypeCode) {
                                    CustomerProdArr.push(i);
                                }
                            });
                            this.entityPM.RemoveCustomerProductPM(this.entityPM.CustomerProducts.filter(function (p) { return p.ProductTypeCode == _this.Code; })[0]);
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    return ProductTypeItemClass;
}());
exports.ProductTypeItemClass = ProductTypeItemClass;
var ProductTypeList = /** @class */ (function () {
    function ProductTypeList() {
    }
    Object.defineProperty(ProductTypeList.prototype, "Id", {
        get: function () { return this.id; },
        set: function (value) { this.id = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeList.prototype, "Code", {
        get: function () { return this.code; },
        set: function (value) { this.code = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeList.prototype, "Name", {
        get: function () { return this.name; },
        set: function (value) { this.name = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeList.prototype, "InActive", {
        get: function () { return this.inActive; },
        set: function (value) { this.inActive = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeList.prototype, "SearchFields", {
        get: function () { return this.searchFields; },
        set: function (value) { this.searchFields = value; },
        enumerable: true,
        configurable: true
    });
    return ProductTypeList;
}());
var AdditionalServiceList = /** @class */ (function () {
    function AdditionalServiceList() {
    }
    Object.defineProperty(AdditionalServiceList.prototype, "Id", {
        get: function () { return this.id; },
        set: function (value) { this.id = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdditionalServiceList.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (value) { this.tenant = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdditionalServiceList.prototype, "Name", {
        get: function () { return this.name; },
        set: function (value) { this.name = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdditionalServiceList.prototype, "InActive", {
        get: function () { return this.inActive; },
        set: function (value) { this.inActive = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdditionalServiceList.prototype, "SearchFields", {
        get: function () { return this.searchFields; },
        set: function (value) { this.searchFields = value; },
        enumerable: true,
        configurable: true
    });
    return AdditionalServiceList;
}());
var ServiceViewModelData = /** @class */ (function () {
    function ServiceViewModelData(item, trigger) {
        this.entityPM = item;
        this.trigger = trigger;
        this.InUse = !this.Potential;
    }
    Object.defineProperty(ServiceViewModelData.prototype, "InUse", {
        get: function () {
            return !this.entityPM.Potential;
        },
        set: function (value) {
            if (this.entityPM.Potential != !value)
                this.entityPM.Potential = !value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceViewModelData.prototype, "TypeLabel", {
        get: function () {
            if (this.entityPM.Potential)
                return "Potential";
            return "In Use";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceViewModelData.prototype, "Potential", {
        get: function () {
            return this.entityPM.Potential;
        },
        set: function (value) {
            if (this.entityPM.Potential != value)
                this.entityPM.Potential = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceViewModelData.prototype, "AdditionalServiceName", {
        get: function () { return this.entityPM.AdditionalServiceName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceViewModelData.prototype, "Notes", {
        get: function () {
            return this.entityPM.Notes;
        },
        set: function (value) {
            if (this.entityPM.Notes != value) {
                this.entityPM.Notes = value;
                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Customer", "Notes update");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceViewModelData.prototype, "BrushedNotesIconVisibility", {
        get: function () {
            if (this.entityPM != null && !(this.entityPM.Notes == null || this.entityPM.Notes == ""))
                return true;
            return false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceViewModelData.prototype, "DefaultNotesIconVisibility", {
        get: function () {
            if (this.entityPM != null && (this.entityPM.Notes == null || this.entityPM.Notes == ""))
                return true;
            return false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceViewModelData.prototype, "Id", {
        get: function () { return this.entityPM.AdditionalServiceId; },
        enumerable: true,
        configurable: true
    });
    return ServiceViewModelData;
}());
exports.ServiceViewModelData = ServiceViewModelData;
var CompetitorViewModelData = /** @class */ (function () {
    function CompetitorViewModelData(item, trigger) {
        this.entityPM = item;
        this.trigger = trigger;
    }
    Object.defineProperty(CompetitorViewModelData.prototype, "CompetitorId", {
        get: function () { return this.entityPM.CompetitorId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorViewModelData.prototype, "Name", {
        get: function () {
            var _this = this;
            var result = "";
            var list = this.trigger.AllCompetitors.filter(function (d) { return d.Id == _this.entityPM.CompetitorId; })[0];
            if (list != null) {
                result = list.Name;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    return CompetitorViewModelData;
}());
exports.CompetitorViewModelData = CompetitorViewModelData;
var ProductObslistItemClass = /** @class */ (function () {
    function ProductObslistItemClass(item, trigger) {
        this.IsDeleteButtonEnabled = false;
        this.PrepaidCollectTypeLabel = "";
        this.TypeLabel = "Potential";
        this.LastShipmentDate = "No Shipmnets";
        this.entityPM = item;
        this.trigger = trigger;
        this.GetProperties();
    }
    Object.defineProperty(ProductObslistItemClass.prototype, "Name", {
        get: function () { return this.entityPM.ProductTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductObslistItemClass.prototype, "ProductTypeCode", {
        get: function () { return this.entityPM.ProductTypeCode; },
        enumerable: true,
        configurable: true
    });
    ProductObslistItemClass.prototype.GetProperties = function () {
        this.IsDeleteButtonEnabled = this.entityPM.LastShipmentDate == null ? true : false;
        if (!Tools_2.AppTool.IsNullOrEmpty(this.entityPM.PrepaidCollectId)) {
            if (this.entityPM.PrepaidCollectId == "P") {
                this.PrepaidCollectTypeLabel = "Prepaid";
            }
            else if (this.entityPM.PrepaidCollectId == "C") {
                this.PrepaidCollectTypeLabel = "Collect";
            }
        }
        if (this.entityPM.LastShipmentDate != null) {
            var myDateFormats = Tools_1.DateTool.GetDateFormats(this.entityPM.LastShipmentDate);
            this.LastShipmentDate = myDateFormats.DateString;
            this.TypeLabel = myDateFormats.ShortDateString;
        }
    };
    return ProductObslistItemClass;
}());
var CompetitorItemClass = /** @class */ (function () {
    function CompetitorItemClass(item, entityPM, trigger) {
        var _this = this;
        this.entityList = item;
        this.entityPM = entityPM;
        this.trigger = trigger;
        this.isChecked = entityPM.CustomerCompetitors.filter(function (d) { return d.CompetitorId == _this.entityList.Id; })[0] != null;
    }
    Object.defineProperty(CompetitorItemClass.prototype, "Name", {
        get: function () { return this.entityList.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorItemClass.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            var _this = this;
            if (this.isChecked != value) {
                this.isChecked = value;
                if (value) {
                    var newItem = new CustomerCompetitorPM_1.CustomerCompetitorPM(null);
                    newItem.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    newItem.CustomerId = this.entityPM.Id;
                    newItem.CompetitorId = this.entityList.Id;
                    newItem.CompetitorName = this.entityList.Name;
                    if (!this.entityPM.CustomerCompetitors.includes(newItem)) {
                        this.entityPM.AddCustomerCompetitorPM(newItem);
                    }
                }
                else {
                    var item = this.entityPM.CustomerCompetitors.filter(function (d) { return d.CompetitorId == _this.entityList.Id; })[0];
                    if (item != null) {
                        if (this.entityPM.CustomerCompetitors.includes(item)) {
                            this.entityPM.RemoveCustomerCompetitorPM(item);
                        }
                    }
                }
                this.trigger.BuildCompetitorsObsList();
                this.trigger.BuildToggleButtonList();
            }
        },
        enumerable: true,
        configurable: true
    });
    return CompetitorItemClass;
}());
var ServiceItemClass = /** @class */ (function () {
    function ServiceItemClass(itemList, itemPM, Parent) {
        var _this = this;
        this.Parent = Parent;
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityPM = itemPM;
        this.entityList = itemList;
        this.isChecked = this.entityPM.CustomerAdditionalServices.filter(function (d) { return d.AdditionalServiceId == _this.entityList.Id; })[0] != null;
    }
    Object.defineProperty(ServiceItemClass.prototype, "Name", {
        get: function () { return this.entityList.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceItemClass.prototype, "Foreground", {
        get: function () { return this.IsChecked ? "#FF6E7172" : "#FF282E30"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceItemClass.prototype, "Id", {
        get: function () { return this.entityList.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceItemClass.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            var _this = this;
            if (this.isChecked != value) {
                this.isChecked = value;
                if (value) {
                    var notesRightToLeft = false;
                    if (SessionLocator_1.SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
                        notesRightToLeft = true;
                    }
                    var newItem = new CustomerAdditionalServicePM_1.CustomerAdditionalServicePM(null);
                    newItem.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    newItem.CustomerId = this.entityPM.Id;
                    newItem.AdditionalServiceId = this.Id;
                    newItem.Potential = true;
                    newItem.NotesRightToLeft = notesRightToLeft;
                    var type = null;
                    var addtionalService = new AdditionalServiceListService_1.AdditionalServiceListService();
                    addtionalService.getSingleFromCache(this.Id).subscribe(function (result) {
                        var typeList = result.Result;
                        if (typeList != null) {
                            type = typeList.Name;
                        }
                        newItem.AdditionalServiceName = type;
                        if (!_this.entityPM.CustomerAdditionalServices.includes(newItem)) {
                            _this.entityPM.AddCustomerAdditionalServicePM(newItem);
                        }
                    });
                    if (!this.entityPM.ActivityWatch) {
                        this.entityPM.ActivityWatch = true;
                    }
                }
                else {
                    var item = this.entityPM.CustomerAdditionalServices.filter(function (d) { return d.AdditionalServiceId == _this.Id; })[0];
                    if (item != null) {
                        if (this.entityPM.CustomerAdditionalServices.includes(item)) {
                            this.entityPM.RemoveCustomerAdditionalServicePM(item);
                        }
                    }
                }
                this.Parent.BuildObsList();
                this.Parent.BuildToggleButtonList();
            }
        },
        enumerable: true,
        configurable: true
    });
    return ServiceItemClass;
}());
//# sourceMappingURL=CustomerGeneralTabComponent.js.map