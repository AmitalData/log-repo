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
var CustomerGeneralTabComponent_1 = require("./CustomerGeneralTabComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var ProductTypeListService_1 = require("../../../../Common/Services/StandardLists/ProductTypeListService");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var CustomerCommitmentsTabComponent_1 = require("./CustomerCommitmentsTabComponent");
var Args_1 = require("../../../../Infrastructure/Args");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var CustomerProductsTabComponent = /** @class */ (function (_super) {
    __extends(CustomerProductsTabComponent, _super);
    function CustomerProductsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObsList = [];
        //public ActualObsList: Array<ProductActualViewModelData> = [];
        _this.ToggleButtonList = [];
        _this.ObjectTableName = "Customer";
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.SearchProductDropButtonId = "SearchProductDropButtonId";
        _this.SearchProductsModeId = "SearchProductsModeId";
        _this.ActualSelectedItem = null;
        _this.TEUActualVisibile = true;
        _this.LoadedActualData = false;
        _this.LoadedData = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Watch
        _this.WatchToolTip = "";
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.ActualObsList = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityPM = entityArgs.EntityPM;
        _this.InitServices();
        _this.BuildProductsObsList();
        _this.BuildToggleButtonList();
        _this.getToolTip();
        return _this;
    }
    CustomerProductsTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._currencyListService = new CurrencyListService_1.CurrencyListService();
        this._currencyListService.getAllFromCache().subscribe(function (result) {
            var myCurrencyCode = "";
            var list = result.Result.filter(function (d) { return d.Id == (SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId); })[0];
            if (list != null) {
                myCurrencyCode = list.Code;
            }
            _this._entityResourceService.getEntityResourceByTableName("CustomerProduct").subscribe(function (response) {
                _this.LoadedData = true;
                _this.RevenueHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("CustomerProduct.F.PotentialRevenue") + " (" + myCurrencyCode + ")";
            });
            _this._entityResourceService.getEntityResourceByTableName("CustomerProductActualData").subscribe(function (response) {
                _this.LoadedActualData = true;
                _this.RevenueActualHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("CustomerProductActualData.F.Revenue") + " (" + myCurrencyCode + ")";
            });
        });
    };
    CustomerProductsTabComponent.prototype.InitServices = function () {
        this.partnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        this.commonDomainService = new CommonDomainService_1.CommonDomainService();
    };
    CustomerProductsTabComponent.prototype.LoadCutomerProducts = function () {
        var _this = this;
        this.partnersDomainService.GetCustomerProducts(this.EntityPM.Id).subscribe(function (result) {
            if (!result.HasError)
                _this.BuildProductsObsList();
        });
    };
    CustomerProductsTabComponent.prototype.BuildToggleButtonList = function () {
        var _this = this;
        this.ToggleButtonList = [];
        var proeductTypeListService = new ProductTypeListService_1.ProductTypeListService();
        proeductTypeListService.getAllFromCache().subscribe(function (result) {
            var FullProductsList = result.Result.filter(function (i) { return i.InActive == false; }).sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; });
            FullProductsList.forEach(function (item) {
                _this.ToggleButtonList.push(new CustomerGeneralTabComponent_1.ProductTypeItemClass(item, _this.EntityPM, _this, FullProductsList));
            });
        });
    };
    CustomerProductsTabComponent.prototype.BuildProductsObsList = function () {
        var _this = this;
        var selectedItem = null;
        this.ObsList = [];
        this.EntityPM.CustomerProducts.sort(function (a, b) { return (a.ProductTypeCode === b.ProductTypeCode) ? 0 : (a.ProductTypeCode < b.ProductTypeCode) ? -1 : 1; }).forEach(function (item) {
            if (item.ProductTypeCode == "AD" || item.ProductTypeCode == "OD" || item.ProductTypeCode == "ID") {
                // continue;
            }
            else {
                _this.ObsList.push(new CustomerCommitmentsTabComponent_1.ProductViewModelData(_this.EntityPM, item, true, "CustomerProductLocation"));
            }
        });
        if (this.ObsList.length > 0)
            this.SelectedItem = this.ObsList[0];
        else
            this.SelectedItem = null;
        this.ItemsSource.Clear();
        this.ItemsSource.InsertCollection(this.ObsList);
    };
    Object.defineProperty(CustomerProductsTabComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (value) {
            if (this.selectedItem != value) {
                this.selectedItem = value;
                if (value != null) {
                    if (value.TransportModeId == "A") {
                        this.TEUActualVisibile = false;
                    }
                    else {
                        this.TEUActualVisibile = true;
                    }
                }
                this.LoadActualData();
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerProductsTabComponent.prototype.rowChanged = function (event) {
        this.SelectedItem = event;
    };
    CustomerProductsTabComponent.prototype.actualRowSelected = function (event) {
        this.ActualSelectedItem = event;
    };
    CustomerProductsTabComponent.prototype.setToggleButtonMenuTemp = function () {
        var ToggleBTN = document.getElementById(this.SearchProductDropButtonId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
    };
    CustomerProductsTabComponent.prototype.setToggleButtonMenu = function () {
        var ToggleBTN = document.getElementById(this.SearchProductDropButtonId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    CustomerProductsTabComponent.prototype.ProductsToggleButtonClicked = function (item, i) {
        if (item.IsChecked == true && !this.EntityPM.CustomerProducts.filter(function (d) { return d.ProductTypeCode == item.Code; }))
            this.BuildToggleButtonList();
        this.BuildProductsObsList();
    };
    CustomerProductsTabComponent.prototype.LoadActualData = function () {
        var _this = this;
        this.ActualObsList.Clear();
        var list = [];
        if (this.SelectedItem != null) {
            this.partnersDomainService.GetCustomerProductHistoryActualData(this.EntityPM.Id, this.SelectedItem.ProductTypeCode).subscribe(function (result) {
                result.Result.filter(function (d) { return d.NumberOfShipments > 0; }).sort(function (a, b) { return ((a.Year === b.Year) ? ((a.Month === b.Month) ? 0 : (a.Month < b.Month) ? -1 : 1) : (a.Year < b.Year ? -1 : 1)); }).reverse().forEach(function (item) {
                    list.push(new CustomerCommitmentsTabComponent_1.ProductActualViewModelData(item));
                });
                _this.ActualObsList.InsertCollection(list);
            });
        }
    };
    CustomerProductsTabComponent.prototype.DeleteProduct = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Delete this product?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                if (_this.EntityPM.CustomerProducts.includes(item.entityPM)) {
                    _this.EntityPM.RemoveCustomerProductPM(item.entityPM);
                    _this.BuildProductsObsList();
                    _this.BuildToggleButtonList();
                }
            }
        });
    };
    CustomerProductsTabComponent.prototype.EditProduct = function (item) {
        var _this = this;
        var control = "";
        var windowTitle = "Edit Product";
        var proeductTypeListService = new ProductTypeListService_1.ProductTypeListService();
        this._entityResourceService.getEntityResourceByTableName("CustomerProductLocation", 0).subscribe(function (p) {
            _this.Clone(item);
            proeductTypeListService.getAllFromCache().subscribe(function (result) {
                var list = result.Result.filter(function (d) { return d.Code == item.ProductTypeCode; })[0];
                if (list != null)
                    windowTitle += ": " + list.Name;
            });
            if (item.isPotential) {
                control = "./CommonModules/CommonCustomer/Components/EditTabs/EditProductPotentialComponent";
            }
            else {
                control = "./CommonModules/CommonCustomer/Components/EditTabs/EditProductCommitmentComponent";
            }
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = item;
            logWindow.WindowClosed.subscribe(function (event) {
                if (event == "Cancel") {
                    _this.RejectChanges();
                }
            });
            logWindow.Show(control);
        });
    };
    CustomerProductsTabComponent.prototype.Clone = function (EntityPM) {
        var _this = this;
        this.myCloner = new Cloner_1.Cloner(EntityPM);
        this.myCloner.AddField('PotentialNumberOfShipments');
        this.myCloner.AddField('PotentialChargeable');
        this.myCloner.AddField('PotentialRevenue');
        this.myCloner.AddField('PotentialTEU');
        this.myCloner.AddField('CommitmentNumberOfShipments');
        this.myCloner.AddField('CommitmentChargeableWeight');
        this.myCloner.AddField('CommitmentRevenue');
        this.myCloner.AddField('CommitmentTEU');
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(EntityPM);
        this.myCloner.AddEntity(EntityPM.entityPM);
        EntityPM.ProductLocations.forEach(function (p) {
            _this.myCloner.AddEntity(p);
            _this.myCloner.AddEntity(p.entityPM);
            _this.myCloner.AddEntity(p.actualEntityPM);
        });
    };
    CustomerProductsTabComponent.prototype.ViewProductActualData = function (Item) {
        var _this = this;
        var objectTableName = "Shipment";
        var queryCode = "CustomerShipmentActualData";
        var myProductCode = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(Item.ProductTypeCode)) {
            myProductCode = Item.ProductTypeCode;
        }
        var actualDate = Tools_1.DateTool.GetDateParts(new Date(Item.Year, Item.Month, 1)).DateObject;
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
        filterAgrs.addAdditionalFilter("ProductCode", myProductCode, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("CustomerId", Item.entityPM.CustomerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ActualDataDateYearMonth", Item.Year, Item.Month, null, "Equals", true, true, false, "Date");
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = objectTableName;
        listArgs.DisplayTitle = "Customer Actual Data";
        listArgs.BackButtonTitle = "Back";
        listArgs.ShowViews = false;
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
            });
        });
    };
    CustomerProductsTabComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    CustomerProductsTabComponent.prototype.getToolTip = function () {
        if (this.ActivityWatch) {
            this.WatchToolTip = "Disable Activity Watch";
        }
        else {
            this.WatchToolTip = "Enable Activity Watch";
        }
    };
    Object.defineProperty(CustomerProductsTabComponent.prototype, "ActivityWatch", {
        get: function () {
            return this.EntityPM.ActivityWatch;
        },
        set: function (value) {
            this.EntityPM.ActivityWatch = value;
            this.getToolTip();
        },
        enumerable: true,
        configurable: true
    });
    CustomerProductsTabComponent.prototype.SetActivity = function (value) {
        this.ActivityWatch = value;
    };
    CustomerProductsTabComponent.prototype.UpdateActualData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Updating ..");
        this.commonDomainService.GetUpdateCustomerActualData(this.EntityPM.Id).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            if (!response.HasError) {
                _this.LoadCutomerProducts();
            }
        });
    };
    CustomerProductsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomerProductsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CustomerProductsTabComponent);
    return CustomerProductsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerProductsTabComponent = CustomerProductsTabComponent;
//# sourceMappingURL=CustomerProductsTabComponent.js.map