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
var CustomerProductPM_1 = require("../../../../Common/EntityPMs/CustomerProductPM");
var CustomerProductActualDataPM_1 = require("../../../../Common/EntityPMs/CustomerProductActualDataPM");
var OpportunityProductPM_1 = require("../../../../CRM/EntityPMs/OpportunityProductPM");
var OpportunityProductLocationPM_1 = require("../../../../CRM/EntityPMs/OpportunityProductLocationPM");
var ProductTypeListService_1 = require("../../../../Common/Services/StandardLists/ProductTypeListService");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var CountryFlagPipe_1 = require("../../../../Controls/Pipes/CountryFlagPipe");
var OpportunityProductsTabComponent = /** @class */ (function (_super) {
    __extends(OpportunityProductsTabComponent, _super);
    function OpportunityProductsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Opportunity";
        _this.DataContext = _this;
        _this.AccountsDataList = [];
        _this.ActualDataList = [];
        _this.PotentialRevenueHeader = "";
        _this.RevenueActualHeader = "";
        _this.LoadedData = false;
        _this.LoadedActualData = false;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.TEUActualVisibile = true;
        _this.ActualSelectedItem = null;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = entityArgs.EntityPM;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.HistoryObsList = new ObservableCollection_1.ObservableCollection([]);
        _this.AccountsDataList = [];
        _this.ActualDataList = [];
        _this.myDateTime = _this.GetDefaultDate();
        _this.IsShowActual = true;
        var currencyListService = new CurrencyListService_1.CurrencyListService();
        currencyListService.getAllFromCache().subscribe(function (result) {
            var myCurrencyCode = "";
            var list = result.Result.filter(function (d) { return d.Id == (SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId); })[0];
            if (list != null) {
                myCurrencyCode = list.Code;
            }
            _this._entityResourceService.getEntityResourceByTableName("OpportunityProduct").subscribe(function (response) {
                _this.LoadedData = true;
                _this.PotentialRevenueHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("OpportunityProduct.F.Revenue") + " (" + myCurrencyCode + ")";
            });
            _this._entityResourceService.getEntityResourceByTableName("CustomerProductActualData").subscribe(function (response) {
                _this.LoadedActualData = true;
                _this.RevenueActualHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("CustomerProductActualData.F.Revenue") + " (" + myCurrencyCode + ")";
            });
        });
        return _this;
        //this.BuildObsList();
    }
    Object.defineProperty(OpportunityProductsTabComponent.prototype, "MyDateTime", {
        get: function () { return this.myDateTime; },
        set: function (value) {
            if (this.myDateTime != value) {
                this.myDateTime = value;
                if (value == null) {
                    this.myDateTime = this.GetDefaultDate();
                }
                this.SetShowActualTimer();
            }
        },
        enumerable: true,
        configurable: true
    });
    OpportunityProductsTabComponent.prototype.GetDefaultDate = function () {
        var date;
        var now = Tools_1.DateTool.GetCurrentDateAsUtc();
        var myMonth = now.getUTCMonth();
        var myYear = now.getUTCFullYear();
        if (myMonth == 1) {
            myMonth = 12;
            myYear = myYear - 1;
        }
        else {
            myMonth = myMonth - 1;
        }
        date = new Date(myYear, myMonth, 1);
        return date;
    };
    //BuildObsList
    OpportunityProductsTabComponent.prototype.BuildObsList = function () {
        var _this = this;
        var ObsList = [];
        this.ItemsSource.Clear();
        this.CurrentSession.StartBusyIndicator("");
        var proeductTypeListService = new ProductTypeListService_1.ProductTypeListService();
        proeductTypeListService.getAllFromCache().subscribe(function (resp) {
            _this.CurrentSession.StopBusyIndicator();
            if (!resp.HasError) {
                var list = resp.Result.filter(function (d) { return !d.InActive; }).sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; });
                list.forEach(function (item) {
                    var productPM = _this.EntityPM.OpportunityProducts.filter(function (d) { return d.OpportunityProductTypeCode == item.Code; })[0];
                    if (productPM == null) {
                        var notesRightToLeft = false;
                        if (SessionLocator_1.SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
                            notesRightToLeft = true;
                        }
                        productPM = new OpportunityProductPM_1.OpportunityProductPM(null);
                        productPM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
                        productPM.OpportunityId = _this.EntityPM.Id;
                        productPM.OpportunityProductTypeCode = item.Code;
                        productPM.OpportunityProductTypeName = item.Name;
                        productPM.PrepaidCollectId = "B";
                        productPM.ChargeableWeight = 0;
                        productPM.TEU = 0;
                        productPM.NumberOfShipments = 0;
                        productPM.Revenue = 0;
                        productPM.NotesRightToLeft = notesRightToLeft;
                    }
                    ObsList.push(new ProductData(productPM, null, null, _this));
                    if (_this.IsShowActual) {
                        var actualRecord = _this.ActualDataList.filter(function (d) { return d.ProductTypeCode == item.Code; })[0];
                        if (actualRecord == null) {
                            actualRecord = new CustomerProductActualDataPM_1.CustomerProductActualDataPM(null);
                            actualRecord.CustomerId = _this.EntityPM.CustomerId;
                            actualRecord.ProductTypeCode = item.Code;
                            actualRecord.Year = Tools_1.DateTool.GetCurrentDateAsUtc().getUTCFullYear();
                            actualRecord.Month = Tools_1.DateTool.GetCurrentDateAsUtc().getUTCMonth();
                            actualRecord.Tenant = SessionLocator_1.SessionLocator.Tenant;
                            actualRecord.ChargeableWeight = 0;
                            actualRecord.NumberOfShipments = 0;
                            actualRecord.TEU = 0;
                            actualRecord.Revenue = 0;
                        }
                        ObsList.push(new ProductData(productPM, null, actualRecord, _this));
                    }
                    if (_this.IsShowAccount) {
                        var accountRecord = _this.AccountsDataList.filter(function (d) { return d.ProductTypeCode == item.Code; })[0];
                        if (accountRecord == null) {
                            accountRecord = new CustomerProductPM_1.CustomerProductPM(null);
                            accountRecord.CustomerId = _this.EntityPM.CustomerId;
                            accountRecord.ProductTypeCode = item.Code;
                            accountRecord.Tenant = SessionLocator_1.SessionLocator.Tenant;
                            accountRecord.PotentialTEU = 0;
                            accountRecord.PotentialRevenue = 0;
                            accountRecord.PotentialChargeableWeight = 0;
                            accountRecord.PotentialNumberOfShipments = 0;
                        }
                        ObsList.push(new ProductData(productPM, accountRecord, null, _this));
                    }
                });
                _this.ItemsSource.InsertCollection(ObsList);
                if (_this.ProductsSelectedItem == null) {
                    _this.ProductsSelectedItem = _this.ItemsSource.Collection[0];
                }
            }
        });
    };
    Object.defineProperty(OpportunityProductsTabComponent.prototype, "IsShowActual", {
        get: function () { return this.isShowActual; },
        set: function (value) {
            if (this.isShowActual != value) {
                this.isShowActual = value;
                this.SetShowActualTimer();
            }
        },
        enumerable: true,
        configurable: true
    });
    OpportunityProductsTabComponent.prototype.SetShowActualTimer = function () {
        if (this.IsShowActual) {
            this.LoadActuals();
        }
        else {
            this.BuildObsList();
        }
    };
    OpportunityProductsTabComponent.prototype.LoadActuals = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomerId) && this.IsShowActual) {
            var partnersdomainService = new PartnersDomainService_1.PartnersDomainService();
            partnersdomainService.GetCustomerActualData(this.EntityPM.CustomerId, this.MyDateTime.getUTCFullYear(), this.MyDateTime.getUTCMonth()).subscribe(function (response) {
                _this.ActualDataList = response;
                if (_this.ActualDataList == null)
                    _this.ActualDataList = [];
                _this.BuildObsList();
            });
        }
    };
    Object.defineProperty(OpportunityProductsTabComponent.prototype, "IsShowAccount", {
        get: function () { return this.isShowAccount; },
        set: function (value) {
            if (this.isShowAccount != value) {
                this.isShowAccount = value;
                this.SetShowAccountTimer();
            }
        },
        enumerable: true,
        configurable: true
    });
    OpportunityProductsTabComponent.prototype.SetShowAccountTimer = function () {
        if (this.IsShowAccount) {
            this.LoadAccounts();
        }
        else {
            this.BuildObsList();
        }
    };
    OpportunityProductsTabComponent.prototype.LoadAccounts = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
            var partnersdomainService = new PartnersDomainService_1.PartnersDomainService();
            partnersdomainService.GetCustomerProducts(this.EntityPM.CustomerId).subscribe(function (response) {
                if (!response.HasError) {
                    _this.AccountsDataList = response;
                    if (_this.AccountsDataList == null)
                        _this.AccountsDataList = [];
                    _this.BuildObsList();
                }
            });
        }
    };
    Object.defineProperty(OpportunityProductsTabComponent.prototype, "HistoryTitle", {
        // History 
        get: function () {
            var myResult = "Actual data history";
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedProductTypeName)) {
                myResult = this.SelectedProductTypeName + " actual data history";
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityProductsTabComponent.prototype, "ProductsSelectedItem", {
        get: function () { return this.productsSelectedItem; },
        set: function (value) {
            if (this.productsSelectedItem != value) {
                this.productsSelectedItem = value;
                this.SelectedProductTypeCode = value == null ? null : value.ProductCode;
                this.SelectedProductTypeName = value == null ? null : value.ProductName;
                if (value != null) {
                    if (value.TransportModeId == "A") {
                        this.TEUActualVisibile = false;
                    }
                    else {
                        this.TEUActualVisibile = true;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityProductsTabComponent.prototype, "SelectedProductTypeCode", {
        get: function () { return this.selectedProductTypeCode; },
        set: function (value) {
            if (this.selectedProductTypeCode != value) {
                this.selectedProductTypeCode = value;
                this.LoadHistory();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityProductsTabComponent.prototype, "SelectedProductTypeName", {
        get: function () { return this.selectedProductTypeName; },
        set: function (value) {
            if (this.selectedProductTypeName != value) {
                this.selectedProductTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    OpportunityProductsTabComponent.prototype.LoadHistory = function () {
        var _this = this;
        this.HistoryObsList.Clear();
        var list = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedProductTypeCode)) {
            var partnersdomainService = new PartnersDomainService_1.PartnersDomainService();
            partnersdomainService.GetCustomerProductHistoryActualData(this.EntityPM.CustomerId, this.SelectedProductTypeCode).subscribe(function (result) {
                result.Result.sort(function (a, b) { return ((a.Year === b.Year) ? ((a.Month === b.Month) ? 0 : (a.Month < b.Month) ? -1 : 1) : (a.Year < b.Year ? -1 : 1)); }).reverse().forEach(function (item) {
                    list.push(new ProductHistoryArgs(item));
                });
                _this.HistoryObsList.InsertCollection(list);
            });
        }
    };
    OpportunityProductsTabComponent.prototype.rowChanged = function (event) {
        this.ProductsSelectedItem = event;
    };
    OpportunityProductsTabComponent.prototype.actualRowSelected = function (event) {
        this.ActualSelectedItem = event;
    };
    OpportunityProductsTabComponent = __decorate([
        core_1.Component({
            selector: 'OpportunityProductsTabComponent',
            moduleId: module.id,
            templateUrl: './OpportunityProductsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], OpportunityProductsTabComponent);
    return OpportunityProductsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.OpportunityProductsTabComponent = OpportunityProductsTabComponent;
var ProductData = /** @class */ (function (_super) {
    __extends(ProductData, _super);
    function ProductData(entity, account, actual, trigger) {
        if (account === void 0) { account = null; }
        if (actual === void 0) { actual = null; }
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "";
        _this._ProductTypeList = [];
        _this.ProductLocations = [];
        _this.CellProductLocations = [];
        _this.CountriesToggleObsList = [];
        _this.ProductCode = "";
        _this.ProductName = "";
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.isChecked = false;
        _this.EstimatedVisibility = false;
        _this.NotesFlowDirection = "ltr";
        _this.NotesBackgroundAlignRight = "transparent";
        _this.NotesBackgroundAlignLeft = "transparent";
        _this.entityPM = entity;
        if (_this.entityPM == null) {
            _this.entityPM = new OpportunityProductPM_1.OpportunityProductPM(null);
        }
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.ObjectTableName = "OpportunityProduct";
        _this.trigger = trigger;
        _this.ProductCode = entity.OpportunityProductTypeCode;
        _this.ProductName = entity.OpportunityProductTypeName;
        if (account != null) {
            _this.accountEntityPM = account;
            _this.isAccount = true;
        }
        if (actual != null) {
            _this.actualEntityPM = actual;
            _this.isActual = true;
        }
        _this.IsChecked = trigger.EntityPM.OpportunityProducts.indexOf(_this.entityPM) != -1 ? true : false;
        _this.InitializeComponent();
        _this.GetEstimatedVisibility();
        _this.InitializeRightToLeft();
        _this.SetProductTypeName();
        return _this;
    }
    Object.defineProperty(ProductData.prototype, "ProductTypeName", {
        get: function () { return this.productTypeName; },
        set: function (value) {
            if (this.productTypeName != value) {
                this.productTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ProductData.prototype.SetProductTypeName = function () {
        var myResult = "";
        if (this.isAccount) {
            myResult = "Potential data";
        }
        else if (this.isActual) {
            var s = "";
            var date = Tools_1.DateTool.GetCurrentDateAsUtc();
            var myMonth = date.getUTCMonth();
            var myYear = date.getUTCFullYear();
            if (myMonth == 1) {
                myMonth = 12;
                myYear = myYear - 1;
            }
            else {
                myMonth = myMonth - 1;
            }
            var date = new Date(myYear, myMonth, 1);
            s = Tools_1.DateTool.GetDateFormats(date).MonthNameShort;
            s += "-" + myYear;
            myResult = "Actual data" + "  (" + s + ")";
        }
        else {
            var proeductTypeListService = new ProductTypeListService_1.ProductTypeListService();
            proeductTypeListService.getSingleFromCache(this.entityPM.OpportunityProductTypeCode).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        myResult = list.Name;
                    }
                }
            });
        }
        this.ProductTypeName = myResult;
    };
    ProductData.prototype.InitializeRightToLeft = function () {
        if (SessionLocator_1.SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
            if (this.entityPM != null) {
                this.entityPM.NotesRightToLeft = true;
            }
        }
    };
    ProductData.prototype.InitializeComponent = function () {
        this.ProductLocations = [];
        this.CellProductLocations = [];
        this.CountriesToggleObsList = [];
        this.BuildProductLocations();
        if (this.TransportModeId == "A") {
            if (this.isActual) {
                this.UIProperties.SetVisibility("TEU", "CustomerProductActualData", false);
            }
            else if (this.isAccount) {
                this.UIProperties.SetVisibility("PotentialTEU", "CustomerProduct", false);
            }
            else {
                this.UIProperties.SetVisibility("TEU", "OpportunityProduct", false);
            }
        }
    };
    ProductData.prototype.ComputeOpportunityTotals = function () {
        var sum = 0;
        this.trigger.EntityPM.OpportunityProducts.forEach(function (item) {
            sum += item.NumberOfShipments;
        });
        this.trigger.EntityPM.NumberOfShipments = sum;
        var field1 = this.trigger.EntityPM.Probability == null ? 0 : this.trigger.EntityPM.Probability;
        var field2 = this.trigger.EntityPM.NumberOfShipments == null ? 0 : this.trigger.EntityPM.NumberOfShipments;
        var myValue = field1 * field2 / 100;
        this.trigger.EntityPM.ValueField = myValue;
    };
    //BuildProductLocations
    ProductData.prototype.BuildProductLocations = function () {
        var _this = this;
        this.ProductLocations = [];
        this.ItemsSource.Clear();
        if (this.isActual && this.actualEntityPM.ProductLocations != null && this.actualEntityPM.ProductLocations.length > 0) {
            this.actualEntityPM.ProductLocations.sort(function (a, b) { return (a.CountryName === b.CountryName) ? 0 : (a.CountryName < b.CountryName) ? -1 : 1; }).forEach(function (item) {
                _this.ProductLocations.push(new ProductLocation(null, null, item, _this, false));
            });
        }
        else if (this.isAccount && this.accountEntityPM.ProductLocations != null && this.accountEntityPM.ProductLocations.length > 0) {
            this.accountEntityPM.ProductLocations.sort(function (a, b) { return (a.CountryName === b.CountryName) ? 0 : (a.CountryName < b.CountryName) ? -1 : 1; }).forEach(function (item) {
                _this.ProductLocations.push(new ProductLocation(null, item, null, _this, false));
            });
        }
        else {
            if (this.entityPM.OpportunityProductLocations != null && this.entityPM.OpportunityProductLocations.length > 0) {
                this.entityPM.OpportunityProductLocations.sort(function (a, b) { return (a.LocationName === b.LocationName) ? 0 : (a.LocationName < b.LocationName) ? -1 : 1; }).forEach(function (item) {
                    _this.ProductLocations.push(new ProductLocation(item, null, null, _this, false));
                });
            }
        }
        this.BuildCellProductLocations();
        var totalTEU = 0;
        var totalRevenue = 0;
        var totalChargeable = 0;
        var totalNumberOfShipments = 0;
        this.ProductLocations.forEach(function (s) {
            if (s.TEU != null)
                totalTEU += s.TEU;
            if (s.Revenue != null)
                totalRevenue += s.Revenue;
            if (s.ChargeableWeight != null)
                totalChargeable += s.ChargeableWeight;
            if (s.NumberOfShipments != null)
                totalNumberOfShipments += s.NumberOfShipments;
        });
        var othersItem = new OpportunityProductLocationPM_1.OpportunityProductLocationPM(null);
        othersItem.LocationName = "Others";
        othersItem.TEU = this.TEU - totalTEU;
        othersItem.Revenue = this.Revenue - totalRevenue;
        othersItem.ChargeableWeight = this.ChargeableWeight - totalChargeable;
        othersItem.NumberOfShipments = this.NumberOfShipments - totalNumberOfShipments;
        this.ProductLocations.push(new ProductLocation(othersItem, null, null, this, true));
        this.ItemsSource.AppendCollection(this.ProductLocations);
    };
    ProductData.prototype.BuildCellProductLocations = function () {
        var _this = this;
        this.CellProductLocations = [];
        this.ProductLocations.filter(function (f) { return f.IsOthers == false; }).sort(function (a, b) { return (a.NumberOfShipments === b.NumberOfShipments) ? 0 : (a.NumberOfShipments < b.NumberOfShipments) ? -1 : 1; }).reverse().forEach(function (item) {
            if (_this.CellProductLocations.length < 10) {
                _this.CellProductLocations.push(item);
            }
            else {
                return;
            }
        });
    };
    ProductData.prototype.SetField = function (fieldName, isTotals) {
        switch (fieldName) {
            case "NumberOfShipments":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(function (d) {
                            if (d.NumberOfShipments != null)
                                sum += d.NumberOfShipments;
                        });
                        this.NumberOfShipments = sum;
                        this.ComputeOpportunityTotals();
                    }
                    else {
                        var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                        if (othersRecord != null) {
                            var sum = 0;
                            this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (d) {
                                if (d.NumberOfShipments != null)
                                    sum += d.NumberOfShipments;
                            });
                            if (this.entityPM.NumberOfShipments != null && this.entityPM.NumberOfShipments != 0) {
                                othersRecord.SetField(fieldName, this.entityPM.NumberOfShipments - sum);
                            }
                        }
                    }
                    this.BuildCellProductLocations();
                    break;
                }
            case "ChargeableWeight":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(function (d) {
                            if (d.ChargeableWeight != null)
                                sum += d.ChargeableWeight;
                        });
                        //this.entityPM.ChargeableWeight = sum;
                        this.ChargeableWeight = sum;
                    }
                    else {
                        var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                        if (othersRecord != null) {
                            var sum = 0;
                            this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (d) {
                                if (d.ChargeableWeight != null)
                                    sum += d.ChargeableWeight;
                            });
                            if (this.entityPM.NumberOfShipments != null && this.entityPM.ChargeableWeight != 0) {
                                othersRecord.SetField(fieldName, this.entityPM.ChargeableWeight - sum);
                            }
                        }
                    }
                    break;
                }
            case "Revenue":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(function (d) {
                            if (d.Revenue != null)
                                sum += d.Revenue;
                        });
                        this.Revenue = sum;
                        //this.entityPM.Revenue = sum;
                    }
                    else {
                        var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                        if (othersRecord != null) {
                            var sum = 0;
                            this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (d) { if (d.Revenue != null)
                                sum += d.Revenue; });
                            if (this.entityPM.NumberOfShipments != null && this.entityPM.Revenue != 0) {
                                othersRecord.SetField(fieldName, this.entityPM.Revenue - sum);
                            }
                        }
                    }
                    break;
                }
            case "TEU":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(function (d) { if (d.TEU != null)
                            sum += d.TEU; });
                        //this.entityPM.TEU = sum;
                        this.TEU = sum;
                    }
                    else {
                        var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                        if (othersRecord != null) {
                            var sum = 0;
                            this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (d) {
                                if (d.TEU != null)
                                    sum += d.TEU;
                            });
                            if (this.entityPM.NumberOfShipments != null && this.entityPM.TEU != 0) {
                                othersRecord.SetField(fieldName, this.entityPM.TEU - sum);
                            }
                        }
                    }
                    break;
                }
        }
    };
    ProductData.prototype.OnLocationsChanged = function () {
        var totalTEU = 0;
        var totalRevenue = 0;
        var totalChargeable = 0;
        var totalNumberOfShipments = 0;
        this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (item) {
            if (item.TEU != null) {
                totalTEU += item.TEU;
            }
            if (item.ChargeableWeight != null) {
                totalChargeable += item.ChargeableWeight;
            }
            if (item.NumberOfShipments != null) {
                totalNumberOfShipments += item.NumberOfShipments;
            }
        });
        var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
        if (othersRecord != null) {
            othersRecord.SetField("TEU", this.TEU - totalTEU);
            othersRecord.SetField("Revenue", this.Revenue - totalRevenue);
            othersRecord.SetField("ChargeableWeight", this.ChargeableWeight - totalChargeable);
            othersRecord.SetField("NumberOfShipments", this.NumberOfShipments - totalNumberOfShipments);
        }
    };
    ProductData.prototype.SetIsCheckedOnFieldsChanged = function () {
        var isLineHasValues = false;
        if (this.NumberOfShipments != null && this.NumberOfShipments != 0) {
            isLineHasValues = true;
        }
        else if (this.ChargeableWeight != null && this.ChargeableWeight != 0) {
            isLineHasValues = true;
        }
        else if (this.Revenue != null && this.Revenue != 0) {
            isLineHasValues = true;
        }
        else if (this.TEU != null && this.TEU != 0) {
            isLineHasValues = true;
        }
        this.IsChecked = isLineHasValues;
    };
    Object.defineProperty(ProductData.prototype, "DirectionId", {
        //Properties
        get: function () {
            var result = "";
            if (this.entityPM.OpportunityProductTypeCode == "CI") {
                result = "C";
            }
            else {
                result = this.entityPM.OpportunityProductTypeCode.substring(1);
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductData.prototype, "TransportModeId", {
        get: function () {
            var result = this.entityPM.OpportunityProductTypeCode.substring(0, 1);
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductData.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            if (this.isChecked != value) {
                this.isChecked = value;
                if (value) {
                    if (this.trigger.EntityPM.OpportunityProducts.indexOf(this.entityPM) == -1) {
                        this.trigger.EntityPM.AddOpportunityProduct(this.entityPM);
                    }
                }
                else {
                    if (this.trigger.EntityPM.OpportunityProducts.indexOf(this.entityPM) != -1) {
                        this.trigger.EntityPM.RemoveOpportunityProduct(this.entityPM);
                    }
                }
                this.ComputeOpportunityTotals();
                this.FireRefreshEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductData.prototype, "ActualVisibility", {
        get: function () { return this.isActual ? true : false; },
        enumerable: true,
        configurable: true
    });
    ProductData.prototype.GetEstimatedVisibility = function () {
        var result = this.isActual || this.isAccount ? false : true;
        this.EstimatedVisibility = result;
    };
    Object.defineProperty(ProductData.prototype, "GridViewCellBackground", {
        get: function () {
            if (this.isActual || this.isAccount || this.trigger.EntityPM.IsClosed || this.trigger.EntityPM.IsCancelled) {
                return "#E6E7E8";
            }
            return "transparent";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductData.prototype, "GridViewCellForeground", {
        get: function () {
            if (this.isActual || this.isAccount || this.trigger.EntityPM.IsClosed || this.trigger.EntityPM.IsCancelled) {
                return "#6E7172";
            }
            return "#282E30";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductData.prototype, "GridViewCellEditControlVisibility", {
        get: function () {
            var result = true;
            if (this.isActual || this.isAccount || this.trigger.EntityPM.IsClosed || this.trigger.EntityPM.IsCancelled) {
                result = false;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductData.prototype, "CheckBoxVisibility", {
        get: function () {
            var result = true;
            if (this.isActual || this.isAccount) {
                result = false;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductData.prototype, "NumberOfShipments", {
        get: function () {
            var result = null;
            if (this.isActual) {
                result = this.actualEntityPM.NumberOfShipments;
            }
            else if (this.isAccount) {
                result = this.accountEntityPM.PotentialNumberOfShipments;
            }
            else {
                result = this.entityPM.NumberOfShipments;
            }
            if (result == 0) {
                result = null;
            }
            return result;
        },
        set: function (value) {
            if (this.entityPM.NumberOfShipments != value) {
                this.entityPM.NumberOfShipments = value;
                this.ComputeOpportunityTotals();
                var total = 0;
                this.ProductLocations.filter(function (d) { return !d.IsOthers; }).forEach(function (item) {
                    total += item.NumberOfShipments;
                });
                var item = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                if (item != null) {
                    item.SetField("NumberOfShipments", value - total);
                }
                this.SetIsCheckedOnFieldsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductData.prototype, "ChargeableWeight", {
        get: function () {
            var result = null;
            if (this.isActual) {
                result = this.actualEntityPM.ChargeableWeight;
            }
            else if (this.isAccount) {
                result = this.accountEntityPM.PotentialChargeableWeight;
            }
            else {
                result = this.entityPM.ChargeableWeight;
            }
            if (result == 0) {
                result = null;
            }
            return result;
        },
        set: function (value) {
            if (this.entityPM.ChargeableWeight != value) {
                this.entityPM.ChargeableWeight = value;
                var total = 0;
                this.ProductLocations.filter(function (d) { return !d.IsOthers; }).forEach(function (item) {
                    total += item.ChargeableWeight;
                });
                var item = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                if (item != null) {
                    item.SetField("ChargeableWeight", value - total);
                }
                this.SetIsCheckedOnFieldsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductData.prototype, "Revenue", {
        get: function () {
            var result = null;
            if (this.isActual) {
                result = this.actualEntityPM.Revenue;
            }
            else if (this.isAccount) {
                result = this.accountEntityPM.PotentialRevenue;
            }
            else {
                result = this.entityPM.Revenue;
            }
            if (result == 0) {
                result = null;
            }
            return result;
        },
        set: function (value) {
            if (this.entityPM.Revenue != value) {
                this.entityPM.Revenue = value;
                var total = 0;
                this.ProductLocations.filter(function (d) { return !d.IsOthers; }).forEach(function (item) {
                    total += item.Revenue;
                });
                var item = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                if (item != null) {
                    item.SetField("Revenue", value - total);
                }
                this.SetIsCheckedOnFieldsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductData.prototype, "TEU", {
        get: function () {
            var result = null;
            if (this.isActual) {
                result = this.actualEntityPM.TEU;
            }
            else if (this.isAccount) {
                result = this.accountEntityPM.PotentialTEU;
            }
            else {
                result = this.entityPM.TEU;
            }
            if (result == 0) {
                result = null;
            }
            return result;
        },
        set: function (value) {
            if (this.entityPM.TEU != value) {
                this.entityPM.TEU = value;
                var total = 0;
                this.ProductLocations.filter(function (d) { return !d.IsOthers; }).forEach(function (item) {
                    total += item.TEU;
                });
                var item = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                if (item != null) {
                    item.SetField("TEU", value - total);
                }
                this.SetIsCheckedOnFieldsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductData.prototype, "IsFieldsEnabled", {
        get: function () {
            var myResult = true;
            if (this.trigger.EntityPM.IsClosed || this.trigger.EntityPM.IsCancelled) {
                myResult = false;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    ProductData.prototype.FireRefreshEvent = function () {
        //RefreshScreenEvent refreshScreenEvent = trigger.eventAggregator.GetEvent<RefreshScreenEvent>();
        //refreshScreenEvent.Publish(new RefreshScreenEventArIsShowAccountgs("BuildProductToggle"));
    };
    Object.defineProperty(ProductData.prototype, "PrepaidCollectId", {
        get: function () { return this.entityPM.PrepaidCollectId; },
        set: function (value) {
            if (this.entityPM.PrepaidCollectId != value) {
                this.entityPM.PrepaidCollectId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductData.prototype, "Notes", {
        //RightToLeft 
        get: function () { return this.entityPM.Notes; },
        set: function (value) {
            if (this.entityPM.Notes != value) {
                this.entityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductData.prototype, "IsNotesRightToLeftEnabled", {
        get: function () {
            var myResult = false;
            if (SessionLocator_1.SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductData.prototype, "NotesRightToLeft", {
        get: function () { return this.entityPM.NotesRightToLeft; },
        set: function (value) {
            if (this.entityPM.NotesRightToLeft != value) {
                this.entityPM.NotesRightToLeft = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ProductData.prototype.GetNotesFlowDirection = function () {
        var myResult = "ltr";
        if (SessionLocator_1.SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
            myResult = "rtl";
            if (this.entityPM.NotesRightToLeft) {
                myResult = "rtl";
            }
            else {
                myResult = "ltr";
            }
        }
        this.NotesFlowDirection = myResult;
    };
    ProductData.prototype.GetNotesBackgroundAlignRight = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.NotesFlowDirection)) {
            this.NotesBackgroundAlignRight = this.NotesFlowDirection == "rtl" ? "#FDD59D" : "transparent";
        }
    };
    ProductData.prototype.GetNotesBackgroundAlignLeft = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.NotesFlowDirection)) {
            this.NotesBackgroundAlignLeft = this.NotesFlowDirection == "ltr" ? "#FDD59D" : "transparent";
        }
    };
    ProductData.prototype.AlignNotesLeftClicked = function () {
        this.entityPM.NotesRightToLeft = false;
        this.GetNotesFlowDirection();
        this.RefreshNotesTextAlgimentVariables();
    };
    ProductData.prototype.AlignNotesRightClicked = function () {
        this.entityPM.NotesRightToLeft = true;
        this.GetNotesFlowDirection();
        this.RefreshNotesTextAlgimentVariables();
    };
    ProductData.prototype.RefreshNotesTextAlgimentVariables = function () {
        this.GetNotesBackgroundAlignLeft();
        this.GetNotesBackgroundAlignRight();
    };
    ProductData.prototype.SetEnabledFields = function () {
        var isEnabled = true;
        if (this.trigger.EntityPM.IsClosed || this.trigger.EntityPM.IsCancelled) {
            isEnabled = false;
        }
        this.UIProperties.SetEnabled("NumberOfShipments", "OpportunityProduct", isEnabled);
        this.UIProperties.SetEnabled("ChargeableWeight", "OpportunityProduct", isEnabled);
        this.UIProperties.SetEnabled("Revenue", "OpportunityProduct", isEnabled);
        this.UIProperties.SetEnabled("TEU", "OpportunityProduct", isEnabled);
        this.UIProperties.SetEnabled("Notes", "OpportunityProduct", isEnabled);
    };
    ProductData.prototype.EditProduct = function (item) {
        var _this = this;
        var control = "";
        var windowTitle = "Edit Product";
        var proeductTypeListService = new ProductTypeListService_1.ProductTypeListService();
        this._entityResourceService.getEntityResourceByTableName("OpportunityProductLocation", 0).subscribe(function (p) {
            proeductTypeListService.getAllFromCache().subscribe(function (result) {
                var list = result.Result.filter(function (d) { return d.Code == _this.entityPM.OpportunityProductTypeCode; })[0];
                if (list != null)
                    windowTitle += ": " + list.Name;
                control = "./CRMModules/CRMOpportunity/Components/EditTabs/EditProductComponent";
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 960;
                logWindow.Height = 600;
                logWindow.Title = windowTitle;
                logWindow.WindowArgs = item;
                logWindow.Show(control);
            });
        });
    };
    return ProductData;
}(BaseComponent_1.BaseComponent));
exports.ProductData = ProductData;
var CountryListViewModel = /** @class */ (function (_super) {
    __extends(CountryListViewModel, _super);
    function CountryListViewModel(item, entityPM, trigger) {
        var _this = _super.call(this) || this;
        _this.src = null;
        _this.ImgId = "";
        _this.entityList = item;
        _this.entityPM = entityPM;
        _this.trigger = trigger;
        _this.isChecked = entityPM.OpportunityProductLocations.filter(function (d) { return d.CountryId == _this.entityList.Id; })[0] != null;
        var pipe = new CountryFlagPipe_1.CountryFlagPipe();
        _this.src = pipe.transform(_this.Code);
        return _this;
    }
    Object.defineProperty(CountryListViewModel.prototype, "CountryId", {
        get: function () { return this.entityList.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CountryListViewModel.prototype, "Code", {
        get: function () { return this.entityList.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CountryListViewModel.prototype, "Name", {
        get: function () { return this.entityList.EnglishName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CountryListViewModel.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            var _this = this;
            if (this.isChecked != value) {
                this.isChecked = value;
                if (value) {
                    var line = 0;
                    if (this.entityPM.OpportunityProductLocations.length > 0) {
                        line = Tools_1.ArrayTool.Max(this.entityPM.OpportunityProductLocations, "LineNumber");
                    }
                    line += 1;
                    var newItemPM = new OpportunityProductLocationPM_1.OpportunityProductLocationPM(null);
                    newItemPM.Tenant = this.entityPM.Tenant;
                    newItemPM.CountryId = this.entityList.Id;
                    newItemPM.OpportunityId = this.entityPM.OpportunityId;
                    newItemPM.OpportunityProductTypeCode = this.entityPM.OpportunityProductTypeCode;
                    newItemPM.LineNumber = line;
                    newItemPM.TEU = 0;
                    newItemPM.Revenue = 0;
                    newItemPM.ChargeableWeight = 0;
                    newItemPM.NumberOfShipments = 0;
                    newItemPM.LocationCode = this.Code;
                    newItemPM.LocationName = this.Name;
                    if (newItemPM != null) {
                        if (this.entityPM.OpportunityProductLocations.indexOf(newItemPM) == -1) {
                            this.entityPM.AddOpportunityProductLocation(newItemPM);
                        }
                    }
                }
                else {
                    var itemPM = this.entityPM.OpportunityProductLocations.filter(function (d) { return d.CountryId == _this.entityList.Id; })[0];
                    if (itemPM != null) {
                        if (this.entityPM.OpportunityProductLocations.indexOf(itemPM) != -1) {
                            this.entityPM.RemoveOpportunityProductLocation(itemPM);
                        }
                    }
                }
                this.trigger.BuildProductLocations();
            }
        },
        enumerable: true,
        configurable: true
    });
    return CountryListViewModel;
}(BaseComponent_1.BaseComponent));
exports.CountryListViewModel = CountryListViewModel;
var ProductHistoryArgs = /** @class */ (function () {
    function ProductHistoryArgs(entity) {
        this.ProductLocations = [];
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.entityPM = entity;
        this.GetProductLocations();
    }
    Object.defineProperty(ProductHistoryArgs.prototype, "TransportModeId", {
        get: function () { return this.entityPM.ProductTypeCode.substring(0, 1); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductHistoryArgs.prototype, "MonthCode", {
        get: function () { return this.entityPM.MonthCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductHistoryArgs.prototype, "Year", {
        get: function () { return this.entityPM.Year; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductHistoryArgs.prototype, "NumberOfShipments", {
        get: function () { return this.entityPM.NumberOfShipments; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductHistoryArgs.prototype, "ChargeableWeight", {
        get: function () { return this.entityPM.ChargeableWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductHistoryArgs.prototype, "Revenue", {
        get: function () { return this.entityPM.Revenue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductHistoryArgs.prototype, "TEU", {
        get: function () { return this.entityPM.TEU; },
        enumerable: true,
        configurable: true
    });
    ProductHistoryArgs.prototype.GetProductLocations = function () {
        var _this = this;
        this.ProductLocations = [];
        var myResult = [];
        if (this.entityPM.ProductLocations.length > 0) {
            this.entityPM.ProductLocations.filter(function (p) { return p.Year == _this.entityPM.Year && p.Month == _this.entityPM.Month; }).sort(function (a, b) { return (a.NumberOfShipments === b.NumberOfShipments) ? 0 : (a.NumberOfShipments < b.NumberOfShipments) ? -1 : 1; }).reverse().forEach(function (item) {
                if (myResult.length < 10) {
                    myResult.push(item);
                }
                else {
                    return;
                }
            });
        }
        myResult.forEach(function (item) {
            _this.ProductLocations.push(new ProductLocationCountryArgs(item));
        });
        //this.ProductLocations = myResult;
    };
    Object.defineProperty(ProductHistoryArgs.prototype, "ViewDetailsIsEnabled", {
        get: function () { return this.entityPM.ProductLocations.length > 0; },
        enumerable: true,
        configurable: true
    });
    ProductHistoryArgs.prototype.ViewDetails = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("OpportunityProductLocation", 0).subscribe(function (p) {
            var windowTitle = "Locations Details";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = _this.entityPM;
            logWindow.Show("./CRMModules/CRMOpportunity/Components/EditTabs/ProductHistoryDetailsComponent");
        });
    };
    return ProductHistoryArgs;
}());
exports.ProductHistoryArgs = ProductHistoryArgs;
var ProductLocation = /** @class */ (function (_super) {
    __extends(ProductLocation, _super);
    function ProductLocation(entityPM, accountEntityPM, actualEntityPM, trigger, isOthers) {
        if (entityPM === void 0) { entityPM = null; }
        if (accountEntityPM === void 0) { accountEntityPM = null; }
        if (actualEntityPM === void 0) { actualEntityPM = null; }
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "";
        _this.src = null;
        if (entityPM != null) {
            _this.entityPM = entityPM;
            _this.ObjectTableName = "CustomerProductLocation";
        }
        if (accountEntityPM != null) {
            _this.isAccount = true;
            _this.accountEntityPM = accountEntityPM;
            _this.ObjectTableName = "CustomerProductLocation";
        }
        if (actualEntityPM != null) {
            _this.isActual = true;
            _this.actualEntityPM = actualEntityPM;
            _this.ObjectTableName = "CustomerProductLocationActualData";
        }
        _this.trigger = trigger;
        _this.IsOthers = isOthers;
        var pipe = new CountryFlagPipe_1.CountryFlagPipe();
        _this.src = pipe.transform(_this.CountryCode);
        return _this;
    }
    Object.defineProperty(ProductLocation.prototype, "CountryId", {
        // Properties
        get: function () {
            var result = null;
            if (this.isActual) {
                result = this.actualEntityPM.CountryId;
            }
            else if (this.isAccount) {
                result = this.accountEntityPM.CountryId;
            }
            else {
                result = this.entityPM.CountryId;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocation.prototype, "CountryCode", {
        get: function () {
            var result = null;
            if (this.isActual) {
                result = this.actualEntityPM.CountryCode;
            }
            else if (this.isAccount) {
                result = this.accountEntityPM.CountryCode;
            }
            else {
                result = this.entityPM.LocationCode;
            }
            return result;
        },
        set: function (value) {
            if (this.entityPM != null) {
                if (this.entityPM.LocationCode != value) {
                    this.entityPM.LocationCode = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocation.prototype, "CountryName", {
        get: function () {
            var result = null;
            if (this.isActual) {
                result = this.actualEntityPM.CountryName;
            }
            else if (this.isAccount) {
                result = this.accountEntityPM.CountryName;
            }
            else {
                result = this.entityPM.LocationName;
            }
            return result;
        },
        set: function (value) {
            if (this.entityPM != null) {
                if (this.entityPM.LocationName != value) {
                    this.entityPM.LocationName = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocation.prototype, "NumberOfShipments", {
        get: function () {
            var result = null;
            if (this.isActual) {
                result = this.actualEntityPM.NumberOfShipments;
            }
            else if (this.isAccount) {
                result = this.accountEntityPM.PotentialNumberOfShipments;
            }
            else {
                result = this.entityPM.NumberOfShipments;
            }
            if (result == 0) {
                result = null;
            }
            return result;
        },
        set: function (value) {
            if (this.entityPM != null) {
                if (this.entityPM.NumberOfShipments != value) {
                    this.entityPM.NumberOfShipments = value;
                    this.UpdateData("NumberOfShipments");
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocation.prototype, "ChargeableWeight", {
        get: function () {
            var result = null;
            if (this.isActual) {
                result = this.actualEntityPM.ChargeableWeight;
            }
            else if (this.isAccount) {
                result = this.accountEntityPM.PotentialChargeableWeight;
            }
            else {
                result = this.entityPM.ChargeableWeight;
            }
            if (result == 0) {
                result = null;
            }
            return result;
        },
        set: function (value) {
            if (this.entityPM != null) {
                if (this.entityPM.ChargeableWeight != value) {
                    this.entityPM.ChargeableWeight = value;
                    this.UpdateData("ChargeableWeight");
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocation.prototype, "Revenue", {
        get: function () {
            var result = null;
            if (this.isActual) {
                result = this.actualEntityPM.Revenue;
            }
            else if (this.isAccount) {
                result = this.accountEntityPM.PotentialRevenue;
            }
            else {
                result = this.entityPM.Revenue;
            }
            if (result == 0) {
                result = null;
            }
            return result;
        },
        set: function (value) {
            if (this.entityPM != null) {
                if (this.entityPM.Revenue != value) {
                    this.entityPM.Revenue = value;
                    this.UpdateData("Revenue");
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocation.prototype, "TEU", {
        get: function () {
            var result = null;
            if (this.isActual) {
                result = this.actualEntityPM.TEU;
            }
            else if (this.isAccount) {
                result = this.accountEntityPM.PotentialTEU;
            }
            else {
                result = this.entityPM.TEU;
            }
            if (result == 0) {
                result = null;
            }
            return result;
        },
        set: function (value) {
            if (this.entityPM != null) {
                if (this.entityPM.TEU != value) {
                    this.entityPM.TEU = value;
                    this.UpdateData("TEU");
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ProductLocation.prototype.UpdateData = function (fieldName) {
        if (this.IsOthers) {
            this.trigger.SetField(fieldName, true);
        }
        else {
            this.trigger.SetField(fieldName, false);
        }
        //FirePropertyChanged(fieldName + "Foreground");
        //FirePropertyChanged("GridViewCellEditControlVisibility");
    };
    Object.defineProperty(ProductLocation.prototype, "GridViewCellEditControlVisibility", {
        get: function () {
            var result = true;
            if (this.isActual || this.isAccount || this.trigger.trigger.EntityPM.IsClosed || this.trigger.trigger.EntityPM.IsCancelled) {
                result = false;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocation.prototype, "NumberOfShipmentsForeground", {
        //Foregrounds
        get: function () {
            return this.NumberOfShipments < 0 ? "#E53030" : "#282E30";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocation.prototype, "ChargeableWeightForeground", {
        get: function () {
            return this.ChargeableWeight < 0 ? "#E53030" : "#282E30";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocation.prototype, "RevenueForeground", {
        get: function () {
            return this.Revenue < 0 ? "#E53030" : "#282E30";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocation.prototype, "TEUForeground", {
        get: function () {
            return this.TEU < 0 ? "#E53030" : "#282E30";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocation.prototype, "ButtonsVisibility", {
        get: function () { return this.IsOthers ? false : true; },
        enumerable: true,
        configurable: true
    });
    ProductLocation.prototype.DeleteCountry = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Delete this country?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var item = _this.trigger.CountriesToggleObsList.filter(function (d) { return d.CountryId == _this.entityPM.CountryId && d.IsChecked; })[0];
                if (item != null) {
                    item.IsChecked = false;
                }
            }
        });
    };
    ProductLocation.prototype.SetField = function (fieldName, value) {
        if (this.entityPM != null) {
            switch (fieldName) {
                case "NumberOfShipments":
                    {
                        this.entityPM.NumberOfShipments = parseInt(value);
                        break;
                    }
                case "ChargeableWeight":
                    {
                        this.entityPM.ChargeableWeight = parseFloat(value);
                        break;
                    }
                case "Revenue":
                    {
                        this.entityPM.Revenue = parseFloat(value);
                        break;
                    }
                case "TEU":
                    {
                        this.entityPM.TEU = parseFloat(value);
                        break;
                    }
            }
        }
    };
    return ProductLocation;
}(BaseComponent_1.BaseComponent));
exports.ProductLocation = ProductLocation;
var ProductLocationCountryArgs = /** @class */ (function (_super) {
    __extends(ProductLocationCountryArgs, _super);
    function ProductLocationCountryArgs(entityPM) {
        var _this = _super.call(this) || this;
        _this.src = null;
        _this.ImgId = "";
        _this.entityPM = entityPM;
        var pipe = new CountryFlagPipe_1.CountryFlagPipe();
        _this.src = pipe.transform(_this.CountryCode);
        return _this;
    }
    ProductLocationCountryArgs.prototype.ngOnInit = function () {
    };
    Object.defineProperty(ProductLocationCountryArgs.prototype, "CountryCode", {
        get: function () { return this.entityPM.CountryCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationCountryArgs.prototype, "CountryName", {
        get: function () { return this.entityPM.CountryName; },
        enumerable: true,
        configurable: true
    });
    return ProductLocationCountryArgs;
}(BaseComponent_1.BaseComponent));
exports.ProductLocationCountryArgs = ProductLocationCountryArgs;
//# sourceMappingURL=OpportunityProductsTabComponent.js.map