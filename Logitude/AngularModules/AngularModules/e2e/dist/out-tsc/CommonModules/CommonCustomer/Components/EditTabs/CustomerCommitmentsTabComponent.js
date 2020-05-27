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
var CustomerProductLocationPM_1 = require("../../../../Common/EntityPMs/CustomerProductLocationPM");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var ProductTypeListService_1 = require("../../../../Common/Services/StandardLists/ProductTypeListService");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var Args_1 = require("../../../../Infrastructure/Args");
var CountryFlagPipe_1 = require("../../../../Controls/Pipes/CountryFlagPipe");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var CustomerCommitmentsTabComponent = /** @class */ (function (_super) {
    __extends(CustomerCommitmentsTabComponent, _super);
    function CustomerCommitmentsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObsList = [];
        _this.ToggleButtonList = [];
        _this.ObjectTableName = "Customer";
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.SearchProductDropButtonId = "SearchProductDropButtonId" + SessionLocator_1.SessionLocator.Index;
        _this.SearchProductsModeId = "SearchProductsModeId";
        _this.LoadedActualData = false;
        _this.LoadedData = false;
        _this.ActualSelectedItem = null;
        _this.TEUActualVisibile = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.ActualObsList = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityPM = entityArgs.EntityPM;
        _this._currencyListService = new CurrencyListService_1.CurrencyListService();
        _this._currencyListService.getAllFromCache().subscribe(function (result) {
            var myCurrencyCode = "";
            var list = result.Result.filter(function (d) { return d.Id == (SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId); })[0];
            if (list != null) {
                myCurrencyCode = list.Code;
            }
            _this._entityResourceService.getEntityResourceByTableName("CustomerProduct").subscribe(function (response) {
                _this.LoadedData = true;
                _this.CommitmentRevenueHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("CustomerProduct.F.CommitmentRevenue") + " (" + myCurrencyCode + ")";
            });
            _this._entityResourceService.getEntityResourceByTableName("CustomerProductActualData").subscribe(function (response) {
                _this.LoadedActualData = true;
                _this.RevenueActualHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("CustomerProductActualData.F.Revenue") + " (" + myCurrencyCode + ")";
            });
        });
        _this.InitServices();
        _this.BuildProductsObsList();
        _this.BuildToggleButtonList();
        return _this;
    }
    Object.defineProperty(CustomerCommitmentsTabComponent.prototype, "SelectedItem", {
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
    CustomerCommitmentsTabComponent.prototype.rowChanged = function (event) {
        this.SelectedItem = event;
    };
    CustomerCommitmentsTabComponent.prototype.actualRowSelected = function (event) {
        this.ActualSelectedItem = event;
    };
    CustomerCommitmentsTabComponent.prototype.setToggleButtonMenuTemp = function () {
        var ToggleBTN = document.getElementById(this.SearchProductDropButtonId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
    };
    CustomerCommitmentsTabComponent.prototype.setToggleButtonMenu = function () {
        var ToggleBTN = document.getElementById(this.SearchProductDropButtonId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    CustomerCommitmentsTabComponent.prototype.ProductsToggleButtonClicked = function (item, i) {
        if (item.IsChecked == true && !this.EntityPM.CustomerProducts.filter(function (d) { return d.ProductTypeCode == item.Code; }))
            this.BuildToggleButtonList();
        this.BuildProductsObsList();
    };
    CustomerCommitmentsTabComponent.prototype.LoadActualData = function () {
        var _this = this;
        this.ActualObsList.Clear();
        //this.ActualObsList = [];
        var list = [];
        if (this.SelectedItem != null) {
            this.partnersDomainService.GetCustomerProductHistoryActualData(this.EntityPM.Id, this.SelectedItem.ProductTypeCode).subscribe(function (result) {
                result.Result.filter(function (d) { return d.NumberOfShipments > 0; }).sort(function (a, b) { return ((a.Year === b.Year) ? ((a.Month === b.Month) ? 0 : (a.Month < b.Month) ? -1 : 1) : (a.Year < b.Year ? -1 : 1)); }).reverse().forEach(function (item) {
                    list.push(new ProductActualViewModelData(item));
                });
                _this.ActualObsList.InsertCollection(list);
            });
        }
    };
    CustomerCommitmentsTabComponent.prototype.DeleteProduct = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Delete this product?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                if (_this.EntityPM.CustomerProducts.indexOf(item.entityPM) != -1) {
                    _this.EntityPM.RemoveCustomerProductPM(item.entityPM);
                    _this.BuildProductsObsList();
                    _this.BuildToggleButtonList();
                }
            }
        });
    };
    CustomerCommitmentsTabComponent.prototype.EditProduct = function (item) {
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
                    //this.RejectChanges();
                    item.BuildProductLocations();
                }
            });
            logWindow.Show(control);
        });
    };
    CustomerCommitmentsTabComponent.prototype.Clone = function (EntityPM) {
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
    CustomerCommitmentsTabComponent.prototype.ViewProductActualData = function (Item) {
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
    CustomerCommitmentsTabComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    CustomerCommitmentsTabComponent.prototype.InitServices = function () {
        this.partnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        this.commonDomainService = new CommonDomainService_1.CommonDomainService();
    };
    CustomerCommitmentsTabComponent.prototype.LoadCutomerProducts = function () {
        var _this = this;
        this.partnersDomainService.GetCustomerProducts(this.EntityPM.Id).subscribe(function (result) {
            if (!result.HasError)
                _this.BuildProductsObsList();
        });
    };
    CustomerCommitmentsTabComponent.prototype.BuildToggleButtonList = function () {
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
    CustomerCommitmentsTabComponent.prototype.BuildProductsObsList = function () {
        var _this = this;
        var selectedItem = null;
        this.ObsList = [];
        this.EntityPM.CustomerProducts.sort(function (a, b) { return (a.ProductTypeCode === b.ProductTypeCode) ? 0 : (a.ProductTypeCode < b.ProductTypeCode) ? -1 : 1; }).forEach(function (item) {
            if (item.ProductTypeCode == "AD" || item.ProductTypeCode == "OD" || item.ProductTypeCode == "ID") {
                // continue;
            }
            else {
                _this.ObsList.push(new ProductViewModelData(_this.EntityPM, item, false, "CustomerProductLocation"));
            }
        });
        if (this.ObsList.length > 0)
            this.SelectedItem = this.ObsList[0];
        else
            this.SelectedItem = null;
        this.ItemsSource.Clear();
        this.ItemsSource.InsertCollection(this.ObsList);
    };
    CustomerCommitmentsTabComponent.prototype.UpdateActualData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Updating ..");
        this.commonDomainService.GetUpdateCustomerActualData(this.EntityPM.Id).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            if (!response.HasError) {
                _this.LoadCutomerProducts();
            }
        });
    };
    CustomerCommitmentsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomerCommitmentsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CustomerCommitmentsTabComponent);
    return CustomerCommitmentsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerCommitmentsTabComponent = CustomerCommitmentsTabComponent;
var ProductViewModelData = /** @class */ (function (_super) {
    __extends(ProductViewModelData, _super);
    function ProductViewModelData(customerPM, product, isPotential, modelName) {
        var _this = _super.call(this) || this;
        _this.modelName = modelName;
        _this._ProductTypeList = [];
        _this.ProductLocations = [];
        _this.CellProductLocations = [];
        _this.CountriesToggleObsList = [];
        _this.DataContext = _this;
        _this.NotesFlowDirection = "ltr";
        _this.NotesBackgroundAlignRight = "transparent";
        _this.NotesBackgroundAlignLeft = "transparent";
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.entityPM = product;
        _this.customerPM = customerPM;
        _this.isPotential = isPotential;
        _this.TargetEntityName = "CustomerProduct";
        var _productTypeListService = new ProductTypeListService_1.ProductTypeListService();
        _productTypeListService.getAllFromCache().subscribe(function (result) {
            _this._ProductTypeList = result.Result;
        });
        if (_this.TransportModeId == "A") {
            _this.UIProperties.SetVisibility("CommitmentTEU", _this.TargetEntityName, false);
            _this.UIProperties.SetVisibility("PotentialTEU", _this.TargetEntityName, false);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(_this.entityPM.PrepaidCollectId)) {
            _this.PrepaidCollectId = "B";
        }
        _this.BuildProductLocations();
        _this.InitializeRightToLeft();
        _this.GetNotesFlowDirection();
        _this.RefreshNotesTextAlgimentVariables();
        return _this;
    }
    ProductViewModelData.prototype.InitializeRightToLeft = function () {
        if (SessionLocator_1.SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
            if (this.entityPM != null) {
                this.entityPM.NotesRightToLeft = true;
            }
        }
    };
    Object.defineProperty(ProductViewModelData.prototype, "PrepaidCollectId", {
        get: function () { return this.entityPM.PrepaidCollectId; },
        set: function (value) {
            if (this.entityPM.PrepaidCollectId != value) {
                // this.entityPM.PrepaidCollectId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ProductViewModelData.prototype.BuildProductLocations = function () {
        var _this = this;
        this.ProductLocations = [];
        this.ItemsSource.Clear();
        if (this.entityPM.ProductLocations == null)
            this.entityPM.ProductLocations = [];
        this.entityPM.ProductLocations.sort(function (a, b) { return (a.CountryName === b.CountryName) ? 0 : (a.CountryName < b.CountryName) ? -1 : 1; }).forEach(function (item) {
            _this.ProductLocations.push(new ProductLocationViewModel(item, _this, false, _this.modelName));
        });
        this.BuildCellProductLocations();
        var totalTEU = 0;
        var totalRevenue = 0;
        var totalChargeable = 0;
        var totalNumberOfShipments = 0;
        if (this.isPotential) {
            this.ProductLocations.forEach(function (s) { if (s.PotentialTEU != null)
                totalTEU += s.PotentialTEU; });
            this.ProductLocations.forEach(function (s) { if (s.PotentialRevenue != null)
                totalRevenue += s.PotentialRevenue; });
            this.ProductLocations.forEach(function (s) { if (s.PotentialChargeableWeight != null)
                totalChargeable += s.PotentialChargeableWeight; });
            this.ProductLocations.forEach(function (s) { if (s.PotentialNumberOfShipments != null)
                totalNumberOfShipments += s.PotentialNumberOfShipments; });
            var othersItem = new CustomerProductLocationPM_1.CustomerProductLocationPM(null);
            othersItem.CountryName = "Others";
            var potentialTEU = this.PotentialTEU;
            if (potentialTEU != null) {
                othersItem.PotentialTEU = potentialTEU - totalTEU;
            }
            var potentialRevenue = this.PotentialRevenue;
            if (potentialRevenue != null) {
                othersItem.PotentialRevenue = potentialRevenue - totalRevenue;
            }
            var potentialChargeableWeight = this.PotentialChargeableWeight;
            if (potentialChargeableWeight != null) {
                othersItem.PotentialChargeableWeight = potentialChargeableWeight - totalChargeable;
            }
            var potentialNumberOfShipments = this.PotentialNumberOfShipments;
            if (potentialNumberOfShipments != null) {
                othersItem.PotentialNumberOfShipments = potentialNumberOfShipments - totalNumberOfShipments;
            }
            this.ProductLocations.push(new ProductLocationViewModel(othersItem, this, true, this.modelName));
        }
        else {
            this.ProductLocations.forEach(function (s) { if (s.CommitmentTEU != null)
                totalTEU += s.CommitmentTEU; });
            this.ProductLocations.forEach(function (s) { if (s.CommitmentRevenue != null)
                totalRevenue += s.CommitmentRevenue; });
            this.ProductLocations.forEach(function (s) { if (s.CommitmentChargeableWeight != null)
                totalChargeable += s.CommitmentChargeableWeight; });
            this.ProductLocations.forEach(function (s) { if (s.CommitmentNumberOfShipments != null)
                totalNumberOfShipments += s.CommitmentNumberOfShipments; });
            var othersItem = new CustomerProductLocationPM_1.CustomerProductLocationPM(null);
            othersItem.CountryName = "Others";
            var commitmentTEU = this.CommitmentTEU;
            if (commitmentTEU != null) {
                othersItem.CommitmentTEU = commitmentTEU - totalTEU;
            }
            var commitmentRevenue = this.CommitmentRevenue;
            if (commitmentRevenue != null) {
                othersItem.CommitmentRevenue = commitmentRevenue - totalRevenue;
            }
            var commitmentChargeableWeight = this.CommitmentChargeableWeight;
            if (commitmentChargeableWeight != null) {
                othersItem.CommitmentChargeableWeight = commitmentChargeableWeight - totalChargeable;
            }
            var commitmentNumberOfShipments = this.CommitmentNumberOfShipments;
            if (commitmentNumberOfShipments != null) {
                othersItem.CommitmentNumberOfShipments = commitmentNumberOfShipments - totalNumberOfShipments;
            }
            this.ProductLocations.push(new ProductLocationViewModel(othersItem, this, true, this.modelName));
        }
        this.ItemsSource.AppendCollection(this.ProductLocations);
    };
    ProductViewModelData.prototype.SetField = function (fieldName, isTotals) {
        switch (fieldName) {
            case "CommitmentNumberOfShipments":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(function (d) { if (d.CommitmentNumberOfShipments != null)
                            sum += d.CommitmentNumberOfShipments; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.CommitmentNumberOfShipments = sum;
                    }
                    else {
                        var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                        if (othersRecord != null) {
                            var sum = 0;
                            this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (d) { if (d.CommitmentNumberOfShipments != null)
                                sum += d.CommitmentNumberOfShipments; });
                            var numberOfShipments = this.entityPM.CommitmentNumberOfShipments;
                            if (numberOfShipments != null && numberOfShipments != 0) {
                                var result = numberOfShipments - sum;
                                if (result == 0)
                                    result = null;
                                othersRecord.SetField(fieldName, result);
                            }
                        }
                    }
                    this.BuildCellProductLocations();
                    break;
                }
            case "CommitmentChargeableWeight":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(function (d) { if (d.CommitmentChargeableWeight != null)
                            sum += d.CommitmentChargeableWeight; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.CommitmentChargeableWeight = sum;
                    }
                    var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                    if (othersRecord != null) {
                        var sum = 0;
                        this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (d) { if (d.CommitmentChargeableWeight != null)
                            sum += d.CommitmentChargeableWeight; });
                        var chargeableWeight = this.entityPM.CommitmentChargeableWeight;
                        if (chargeableWeight != null && chargeableWeight != 0) {
                            var result = chargeableWeight - sum;
                            if (result == 0)
                                result = null;
                            othersRecord.SetField(fieldName, result);
                        }
                    }
                    break;
                }
            case "CommitmentRevenue":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(function (d) { if (d.CommitmentRevenue != null)
                            sum += d.CommitmentRevenue; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.CommitmentRevenue = sum;
                    }
                    var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                    if (othersRecord != null) {
                        var sum = 0;
                        this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (d) { if (d.CommitmentRevenue != null)
                            sum += d.CommitmentRevenue; });
                        var revenue = this.entityPM.CommitmentRevenue;
                        if (revenue != null && revenue != 0) {
                            var result = revenue - sum;
                            if (result == 0)
                                result = null;
                            othersRecord.SetField(fieldName, result);
                        }
                    }
                    break;
                }
            case "CommitmentTEU":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(function (d) { if (d.CommitmentTEU != null)
                            sum += d.CommitmentTEU; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.CommitmentTEU = sum;
                    }
                    var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                    if (othersRecord != null) {
                        var sum = 0;
                        this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (d) { if (d.CommitmentTEU != null)
                            sum += d.CommitmentTEU; });
                        var tEU = this.entityPM.CommitmentTEU;
                        if (tEU != null && tEU != 0) {
                            var result = tEU - sum;
                            if (result == 0)
                                result = null;
                            othersRecord.SetField(fieldName, result);
                        }
                    }
                    break;
                }
            case "PotentialNumberOfShipments":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(function (d) { if (d.PotentialNumberOfShipments != null)
                            sum += d.PotentialNumberOfShipments; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.PotentialNumberOfShipments = sum;
                    }
                    var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                    if (othersRecord != null) {
                        var sum = 0;
                        this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (d) { if (d.PotentialNumberOfShipments != null)
                            sum += d.PotentialNumberOfShipments; });
                        var numberOfShipments = this.entityPM.PotentialNumberOfShipments;
                        if (numberOfShipments != null && numberOfShipments != 0) {
                            var result = numberOfShipments - sum;
                            if (result == 0)
                                result = null;
                            othersRecord.SetField(fieldName, result);
                        }
                    }
                    this.BuildCellProductLocations();
                    break;
                }
            case "PotentialChargeableWeight":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(function (d) { if (d.PotentialChargeableWeight != null)
                            sum += d.PotentialChargeableWeight; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.PotentialChargeableWeight = sum;
                    }
                    var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                    if (othersRecord != null) {
                        var sum = 0;
                        this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (d) { if (d.PotentialChargeableWeight != null)
                            sum += d.PotentialChargeableWeight; });
                        var chargeableWeight = this.entityPM.PotentialChargeableWeight;
                        if (chargeableWeight != null && chargeableWeight != 0) {
                            var result = chargeableWeight - sum;
                            if (result == 0)
                                result = null;
                            othersRecord.SetField(fieldName, result);
                        }
                    }
                    break;
                }
            case "PotentialRevenue":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(function (d) { if (d.PotentialRevenue != null)
                            sum += d.PotentialRevenue; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.PotentialRevenue = sum;
                    }
                    var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                    if (othersRecord != null) {
                        var sum = 0;
                        this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (d) { if (d.PotentialRevenue != null)
                            sum += d.PotentialRevenue; });
                        var revenue = this.entityPM.PotentialRevenue;
                        if (revenue != null && revenue != 0) {
                            var result = revenue - sum;
                            if (result == 0)
                                result = null;
                            othersRecord.SetField(fieldName, result);
                        }
                    }
                    break;
                }
            case "PotentialTEU":
                {
                    if (isTotals) {
                        var sum = 0;
                        this.ProductLocations.forEach(function (d) { if (d.PotentialTEU != null)
                            sum += d.PotentialTEU; });
                        if (sum == 0) {
                            sum = null;
                        }
                        this.entityPM.PotentialTEU = sum;
                    }
                    var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                    if (othersRecord != null) {
                        var sum = 0;
                        this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (d) { if (d.PotentialTEU != null)
                            sum += d.PotentialTEU; });
                        var tEU = this.entityPM.PotentialTEU;
                        if (tEU != null && tEU != 0) {
                            var result = tEU - sum;
                            if (result == 0)
                                result = null;
                            othersRecord.SetField(fieldName, result);
                        }
                    }
                    break;
                }
        }
    };
    ProductViewModelData.prototype.OnLocationsChanged = function () {
        var totalTEU = 0;
        var totalRevenue = 0;
        var totalChargeable = 0;
        var totalNumberOfShipments = 0;
        if (this.isPotential) {
            this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (s) { if (s.PotentialTEU != null)
                totalTEU += s.PotentialTEU; });
            this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (s) { if (s.PotentialRevenue != null)
                totalRevenue += s.PotentialRevenue; });
            this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (s) { if (s.PotentialChargeableWeight != null)
                totalChargeable += s.PotentialChargeableWeight; });
            this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (s) { if (s.PotentialNumberOfShipments != null)
                totalNumberOfShipments += s.PotentialNumberOfShipments; });
            var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
            if (othersRecord != null) {
                var potentialTEU = this.PotentialTEU;
                if (potentialTEU == null) {
                    potentialTEU = 0;
                }
                othersRecord.SetField("PotentialTEU", potentialTEU - totalTEU);
                var potentialRevenue = this.PotentialRevenue;
                if (potentialRevenue == null) {
                    potentialRevenue = 0;
                }
                othersRecord.SetField("PotentialRevenue", potentialRevenue - totalRevenue);
                var potentialChargeableWeight = this.PotentialChargeableWeight;
                if (potentialChargeableWeight == null) {
                    potentialChargeableWeight = 0;
                }
                othersRecord.SetField("PotentialChargeableWeight", potentialChargeableWeight - totalChargeable);
                var potentialNumberOfShipments = this.PotentialNumberOfShipments;
                if (potentialNumberOfShipments == null) {
                    potentialNumberOfShipments = 0;
                }
                othersRecord.SetField("PotentialNumberOfShipments", potentialNumberOfShipments - totalNumberOfShipments);
            }
        }
        else {
            this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (s) { if (s.CommitmentTEU != null)
                totalTEU += s.CommitmentTEU; });
            this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (s) { if (s.CommitmentRevenue != null)
                totalRevenue += s.CommitmentRevenue; });
            this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (s) { if (s.CommitmentChargeableWeight != null)
                totalChargeable += s.CommitmentChargeableWeight; });
            this.ProductLocations.filter(function (d) { return d.IsOthers == false; }).forEach(function (s) { if (s.CommitmentNumberOfShipments != null)
                totalNumberOfShipments += s.CommitmentNumberOfShipments; });
            var othersRecord = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
            if (othersRecord != null) {
                var commitmentTEU = this.CommitmentTEU;
                if (commitmentTEU == null) {
                    commitmentTEU = 0;
                }
                othersRecord.SetField("CommitmentTEU", commitmentTEU - totalTEU);
                var commitmentRevenue = this.CommitmentRevenue;
                if (commitmentRevenue == null) {
                    commitmentRevenue = 0;
                }
                othersRecord.SetField("CommitmentRevenue", commitmentRevenue - totalRevenue);
                var commitmentChargeableWeight = this.CommitmentChargeableWeight;
                if (commitmentChargeableWeight == null) {
                    commitmentChargeableWeight = 0;
                }
                othersRecord.SetField("CommitmentChargeableWeight", commitmentChargeableWeight - totalChargeable);
                var commitmentNumberOfShipments = this.CommitmentNumberOfShipments;
                if (commitmentNumberOfShipments == null) {
                    commitmentNumberOfShipments = 0;
                }
                othersRecord.SetField("CommitmentNumberOfShipments", commitmentNumberOfShipments - totalNumberOfShipments);
            }
        }
    };
    Object.defineProperty(ProductViewModelData.prototype, "DirectionId", {
        //Region Properties
        get: function () {
            if (this.entityPM.ProductTypeCode == "CI") {
                return "C";
            }
            else {
                return this.entityPM.ProductTypeCode.substr(1, 1);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductViewModelData.prototype, "TargetEntityName", {
        get: function () { return this.targetEntityName; },
        set: function (value) { this.targetEntityName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductViewModelData.prototype, "TransportModeId", {
        get: function () { return this.entityPM.ProductTypeCode.substr(0, 1); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductViewModelData.prototype, "ProductTypeCode", {
        get: function () { return this.entityPM.ProductTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductViewModelData.prototype, "ProductTypeName", {
        get: function () {
            var _this = this;
            var result = "";
            var list = this._ProductTypeList.filter(function (d) { return d.Code == _this.entityPM.ProductTypeCode; })[0];
            if (list != null) {
                result = list.Name;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductViewModelData.prototype, "PotentialNumberOfShipments", {
        get: function () { return this.entityPM.PotentialNumberOfShipments == 0 ? null : this.entityPM.PotentialNumberOfShipments; },
        set: function (value) {
            if (this.entityPM.PotentialNumberOfShipments != value) {
                this.entityPM.PotentialNumberOfShipments = value;
                var total = 0;
                this.ProductLocations.filter(function (d) { return !d.IsOthers; }).forEach(function (s) { if (s.PotentialNumberOfShipments != null)
                    total += s.PotentialNumberOfShipments; });
                if (total == null) {
                    total = 0;
                }
                var item = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                if (item != null) {
                    item.SetField("PotentialNumberOfShipments", value - total);
                }
                this.SetActivityWatch();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductViewModelData.prototype, "PotentialChargeableWeight", {
        get: function () { return this.entityPM.PotentialChargeableWeight == 0 ? null : this.entityPM.PotentialChargeableWeight; },
        set: function (value) {
            if (this.entityPM.PotentialChargeableWeight != value) {
                this.entityPM.PotentialChargeableWeight = value;
                var total = 0;
                this.ProductLocations.filter(function (d) { return !d.IsOthers; }).forEach(function (s) { if (s.PotentialChargeableWeight != null)
                    total += s.PotentialChargeableWeight; });
                if (total == null) {
                    total = 0;
                }
                var item = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                if (item != null) {
                    item.SetField("PotentialChargeableWeight", value - total);
                }
                this.SetActivityWatch();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductViewModelData.prototype, "PotentialRevenue", {
        get: function () { return this.entityPM.PotentialRevenue == 0 ? null : this.entityPM.PotentialRevenue; },
        set: function (value) {
            if (this.entityPM.PotentialRevenue != value) {
                this.entityPM.PotentialRevenue = value;
                var total = 0;
                this.ProductLocations.filter(function (d) { return !d.IsOthers; }).forEach(function (s) { if (s.PotentialRevenue != null)
                    total += s.PotentialRevenue; });
                if (total == null) {
                    total = 0;
                }
                var item = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                if (item != null) {
                    item.SetField("PotentialRevenue", value - total);
                }
                this.SetActivityWatch();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductViewModelData.prototype, "PotentialTEU", {
        get: function () { return this.entityPM.PotentialTEU == 0 ? null : this.entityPM.PotentialTEU; },
        set: function (value) {
            if (this.entityPM.PotentialTEU != value) {
                this.entityPM.PotentialTEU = value;
                var total = 0;
                this.ProductLocations.filter(function (d) { return !d.IsOthers; }).forEach(function (s) {
                    if (s.PotentialTEU != null)
                        total += s.PotentialTEU;
                });
                if (total == null) {
                    total = 0;
                }
                var item = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                if (item != null) {
                    item.SetField("PotentialTEU", value - total);
                }
                this.SetActivityWatch();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductViewModelData.prototype, "CommitmentNumberOfShipments", {
        get: function () { return this.entityPM.CommitmentNumberOfShipments == 0 ? null : this.entityPM.CommitmentNumberOfShipments; },
        set: function (value) {
            if (this.entityPM.CommitmentNumberOfShipments != value) {
                this.entityPM.CommitmentNumberOfShipments = value;
                var total = 0;
                this.ProductLocations.filter(function (d) { return !d.IsOthers; }).forEach(function (s) { if (s.CommitmentNumberOfShipments != null)
                    total += s.CommitmentNumberOfShipments; });
                if (total == null) {
                    total = 0;
                }
                var item = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                if (item != null) {
                    item.SetField("CommitmentNumberOfShipments", value - total);
                }
                this.SetActivityWatch();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductViewModelData.prototype, "CommitmentChargeableWeight", {
        get: function () { return this.entityPM.CommitmentChargeableWeight == 0 ? null : this.entityPM.CommitmentChargeableWeight; },
        set: function (value) {
            if (this.entityPM.CommitmentChargeableWeight != value) {
                this.entityPM.CommitmentChargeableWeight = value;
                var total = 0;
                this.ProductLocations.filter(function (d) { return !d.IsOthers; }).forEach(function (s) { if (s.CommitmentChargeableWeight != null)
                    total += s.CommitmentChargeableWeight; });
                if (total == null) {
                    total = 0;
                }
                var item = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                if (item != null) {
                    item.SetField("CommitmentChargeableWeight", value - total);
                }
                this.SetActivityWatch();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductViewModelData.prototype, "CommitmentRevenue", {
        get: function () { return this.entityPM.CommitmentRevenue == 0 ? null : this.entityPM.CommitmentRevenue; },
        set: function (value) {
            if (this.entityPM.CommitmentRevenue != value) {
                this.entityPM.CommitmentRevenue = value;
                var total = 0;
                this.ProductLocations.filter(function (d) { return !d.IsOthers; }).forEach(function (s) { if (s.CommitmentRevenue != null)
                    total += s.CommitmentRevenue; });
                if (total == null) {
                    total = 0;
                }
                var item = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                if (item != null) {
                    item.SetField("CommitmentRevenue", value - total);
                }
                this.SetActivityWatch();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductViewModelData.prototype, "CommitmentTEU", {
        get: function () { return this.entityPM.CommitmentTEU == 0 ? null : this.entityPM.CommitmentTEU; },
        set: function (value) {
            if (this.entityPM.CommitmentTEU != value) {
                this.entityPM.CommitmentTEU = value;
                var total = 0;
                this.ProductLocations.filter(function (d) { return !d.IsOthers; }).forEach(function (s) { if (s.CommitmentTEU != null)
                    total += s.CommitmentTEU; });
                if (total == null) {
                    total = 0;
                }
                var item = this.ProductLocations.filter(function (d) { return d.IsOthers; })[0];
                if (item != null) {
                    item.SetField("CommitmentTEU", value - total);
                }
                this.SetActivityWatch();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductViewModelData.prototype, "Notes", {
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
    Object.defineProperty(ProductViewModelData.prototype, "IsNotesRightToLeftEnabled", {
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
    Object.defineProperty(ProductViewModelData.prototype, "NotesRightToLeft", {
        get: function () { return this.entityPM.NotesRightToLeft; },
        set: function (value) {
            if (this.entityPM.NotesRightToLeft != value) {
                this.entityPM.NotesRightToLeft = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ProductViewModelData.prototype.GetNotesFlowDirection = function () {
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
    ProductViewModelData.prototype.GetNotesBackgroundAlignRight = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.NotesFlowDirection)) {
            this.NotesBackgroundAlignRight = this.NotesFlowDirection == "rtl" ? "#FDD59D" : "transparent";
        }
    };
    ProductViewModelData.prototype.GetNotesBackgroundAlignLeft = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.NotesFlowDirection)) {
            this.NotesBackgroundAlignLeft = this.NotesFlowDirection == "ltr" ? "#FDD59D" : "transparent";
        }
    };
    ProductViewModelData.prototype.AlignNotesLeftClicked = function () {
        this.entityPM.NotesRightToLeft = false;
        this.GetNotesFlowDirection();
        this.RefreshNotesTextAlgimentVariables();
    };
    ProductViewModelData.prototype.AlignNotesRightClicked = function () {
        this.entityPM.NotesRightToLeft = true;
        this.GetNotesFlowDirection();
        this.RefreshNotesTextAlgimentVariables();
    };
    ProductViewModelData.prototype.RefreshNotesTextAlgimentVariables = function () {
        this.GetNotesBackgroundAlignLeft();
        this.GetNotesBackgroundAlignRight();
    };
    Object.defineProperty(ProductViewModelData.prototype, "LastShipmentDate", {
        get: function () { return this.entityPM.LastShipmentDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductViewModelData.prototype, "IsDeleteButtonEnabled", {
        get: function () { return this.LastShipmentDate == null; },
        enumerable: true,
        configurable: true
    });
    ProductViewModelData.prototype.SetActivityWatch = function () {
        if (this.customerPM.CustomerAdditionalServices.length > 0) {
            // this.customerPM.ActivityWatch = true;
        }
        if (this.CommitmentChargeableWeight != null
            || this.CommitmentNumberOfShipments != null
            || this.CommitmentRevenue != null
            || this.CommitmentTEU != null
            || this.PotentialChargeableWeight != null
            || this.PotentialNumberOfShipments != null
            || this.PotentialRevenue != null
            || this.PotentialTEU != null) {
            // this.customerPM.ActivityWatch = true;
        }
        else {
            //  this.customerPM.ActivityWatch = false;
        }
    };
    ProductViewModelData.prototype.BuildCellProductLocations = function () {
        var _this = this;
        this.CellProductLocations = [];
        if (this.isPotential) {
            this.ProductLocations.filter(function (f) { return f.IsOthers == false; }).sort(function (a, b) { return (a.PotentialNumberOfShipments === b.PotentialNumberOfShipments) ? 0 : (a.PotentialNumberOfShipments < b.PotentialNumberOfShipments) ? -1 : 1; }).reverse().forEach(function (item) {
                if (_this.CellProductLocations.length < 10) {
                    _this.CellProductLocations.push(item);
                }
                else {
                    return;
                }
            });
        }
        else {
            this.ProductLocations.filter(function (f) { return f.IsOthers == false; }).sort(function (a, b) { return (a.CommitmentNumberOfShipments === b.CommitmentNumberOfShipments) ? 0 : (a.CommitmentNumberOfShipments < b.CommitmentNumberOfShipments) ? -1 : 1; }).reverse().forEach(function (item) {
                if (_this.CellProductLocations.length < 10) {
                    _this.CellProductLocations.push(item);
                }
                else {
                    return;
                }
            });
        }
    };
    ProductViewModelData.prototype.ResetCountriesItems = function () {
        var _this = this;
        if (this.ItemsSource != null) {
            var items = this.ItemsSource.Collection;
            ;
            items.forEach(function (item) {
                var location = _this.entityPM.ProductLocations.filter(function (d) { return d == item.entityPM; })[0];
                if (location == null) {
                    if (_this.entityPM.ProductLocations.indexOf(location) != -1) {
                        _this.entityPM.RemoveCustomerProductLocationPM(location);
                    }
                }
                else {
                    item.CountryCode = location.CountryCode;
                    item.CountryName = location.CountryName;
                    item.CommitmentNumberOfShipments = location.CommitmentNumberOfShipments;
                    item.CommitmentRevenue = location.CommitmentRevenue;
                    item.CommitmentTEU = location.CommitmentTEU;
                }
            });
            this.ItemsSource.Collection.forEach(function (item) {
                var list = _this.entityPM.ProductLocations.filter(function (d) { return d == item.entityPM; });
                if (list == null) {
                    _this.entityPM.ProductLocations.push(item);
                }
            });
        }
    };
    return ProductViewModelData;
}(BaseComponent_1.BaseComponent));
exports.ProductViewModelData = ProductViewModelData;
var ProductActualViewModelData = /** @class */ (function () {
    function ProductActualViewModelData(entityPM) {
        // public ProductLocations: Array<CustomerProductLocationActualDataPM> = [];
        this.ProductLocations = [];
        this.src = null;
        this.CellReadOnlyBackground = "rgba(230, 231, 232, 0.5)";
        this.GridViewCellBackground = "#E6E7E8";
        this.IsHover = false;
        this.entityPM = entityPM;
        this.SetProductLocations();
    }
    Object.defineProperty(ProductActualViewModelData.prototype, "ProductTypeCode", {
        get: function () { return this.entityPM.ProductTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductActualViewModelData.prototype, "Year", {
        get: function () { return this.entityPM.Year; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductActualViewModelData.prototype, "Month", {
        get: function () { return this.entityPM.Month; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductActualViewModelData.prototype, "MonthCode", {
        get: function () { return this.entityPM.MonthCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductActualViewModelData.prototype, "TEU", {
        get: function () { return this.entityPM.TEU == 0 ? null : this.entityPM.TEU; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductActualViewModelData.prototype, "Revenue", {
        get: function () { return this.entityPM.Revenue == 0 ? null : this.entityPM.Revenue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductActualViewModelData.prototype, "ChargeableWeight", {
        get: function () { return this.entityPM.ChargeableWeight == 0 ? null : this.entityPM.ChargeableWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductActualViewModelData.prototype, "NumberOfShipments", {
        get: function () { return this.entityPM.NumberOfShipments == 0 ? null : this.entityPM.NumberOfShipments; },
        enumerable: true,
        configurable: true
    });
    ProductActualViewModelData.prototype.SetProductLocations = function () {
        var _this = this;
        var filterdProductLocations = this.entityPM.ProductLocations.filter(function (p) { return p.Year == _this.entityPM.Year && p.Month == _this.entityPM.Month; });
        if (filterdProductLocations.length > 10) {
            filterdProductLocations = filterdProductLocations.sort(function (a, b) { return (a.NumberOfShipments === b.NumberOfShipments) ? 0 : (a.NumberOfShipments < b.NumberOfShipments) ? -1 : 1; }).reverse().slice(filterdProductLocations.length - 11, filterdProductLocations.length - 1);
        }
        else {
            filterdProductLocations = filterdProductLocations.sort(function (a, b) { return (a.NumberOfShipments === b.NumberOfShipments) ? 0 : (a.NumberOfShipments < b.NumberOfShipments) ? -1 : 1; }).reverse();
        }
        filterdProductLocations.forEach(function (item) {
            _this.ProductLocations.push(new ProductLocationCountryArgs(item));
        });
    };
    ProductActualViewModelData.prototype.Zoom = function () {
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Shipment"; })[0];
        if (ObjectTable != null) {
            var myQueryPM = window.Queries.filter(function (x) { return x.ObjectTableId === ObjectTable.Id && x.Code == "CustomerShipmentActualData"; })[0];
            if (myQueryPM != null) {
            }
        }
    };
    return ProductActualViewModelData;
}());
exports.ProductActualViewModelData = ProductActualViewModelData;
var ProductLocationViewModel = /** @class */ (function (_super) {
    __extends(ProductLocationViewModel, _super);
    function ProductLocationViewModel(item, trigger, isOthers, TargetEntityName) {
        var _this = _super.call(this) || this;
        _this.src = null;
        if (TargetEntityName == "CustomerProductLocation") {
            _this.entityPM = item;
            _this.TargetEntityName = TargetEntityName;
            _this.trigger = trigger;
            _this.IsOthers = isOthers;
        }
        else if (TargetEntityName == "CustomerProductLocationActualData") {
            _this.isActual = true;
            _this.actualEntityPM = item;
            _this.trigger = trigger;
            _this.TargetEntityName = TargetEntityName;
            _this.IsOthers = isOthers;
        }
        var pipe = new CountryFlagPipe_1.CountryFlagPipe();
        _this.src = pipe.transform(_this.CountryCode);
        return _this;
    }
    Object.defineProperty(ProductLocationViewModel.prototype, "IsOthers", {
        get: function () { return this.isOthers; },
        set: function (value) { this.isOthers = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "TargetEntityName", {
        get: function () { return this.targetEntityName; },
        set: function (value) { this.targetEntityName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "CountryId", {
        get: function () { return this.isActual ? this.actualEntityPM.CountryId : this.entityPM.CountryId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "CountryCode", {
        get: function () { return this.isActual ? this.actualEntityPM.CountryCode : this.entityPM.CountryCode; },
        set: function (value) {
            if (this.entityPM.CountryCode != value) {
                this.entityPM.CountryCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "CountryName", {
        get: function () { return this.isActual ? this.actualEntityPM.CountryName : this.entityPM.CountryName; },
        set: function (value) {
            if (this.entityPM.CountryName != value) {
                this.entityPM.CountryName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "CommitmentNumberOfShipments", {
        get: function () { return this.isActual ? this.actualEntityPM.NumberOfShipments : this.entityPM.CommitmentNumberOfShipments; },
        set: function (value) {
            if (this.entityPM.CommitmentNumberOfShipments != value) {
                this.entityPM.CommitmentNumberOfShipments = value;
                this.UpdateData("CommitmentNumberOfShipments");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "CommitmentChargeableWeight", {
        get: function () { return this.isActual ? this.actualEntityPM.ChargeableWeight : this.entityPM.CommitmentChargeableWeight; },
        set: function (value) {
            if (this.entityPM.CommitmentChargeableWeight != value) {
                this.entityPM.CommitmentChargeableWeight = value;
                this.UpdateData("CommitmentChargeableWeight");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "CommitmentRevenue", {
        get: function () { return this.isActual ? this.actualEntityPM.Revenue : this.entityPM.CommitmentRevenue; },
        set: function (value) {
            if (this.entityPM.CommitmentRevenue != value) {
                this.entityPM.CommitmentRevenue = value;
                this.UpdateData("CommitmentRevenue");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "CommitmentTEU", {
        get: function () { return this.isActual ? this.actualEntityPM.TEU : this.entityPM.CommitmentTEU; },
        set: function (value) {
            if (this.entityPM.CommitmentTEU != value) {
                this.entityPM.CommitmentTEU = value;
                this.UpdateData("CommitmentTEU");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "PotentialNumberOfShipments", {
        get: function () { return this.isActual ? this.actualEntityPM.NumberOfShipments : this.entityPM.PotentialNumberOfShipments; },
        set: function (value) {
            if (this.entityPM.PotentialNumberOfShipments != value) {
                this.entityPM.PotentialNumberOfShipments = value;
                this.UpdateData("PotentialNumberOfShipments");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "PotentialChargeableWeight", {
        get: function () { return this.isActual ? this.actualEntityPM.ChargeableWeight : this.entityPM.PotentialChargeableWeight; },
        set: function (value) {
            if (this.entityPM.PotentialChargeableWeight != value) {
                this.entityPM.PotentialChargeableWeight = value;
                this.UpdateData("PotentialChargeableWeight");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "PotentialRevenue", {
        get: function () { return this.isActual ? this.actualEntityPM.Revenue : this.entityPM.PotentialRevenue; },
        set: function (value) {
            if (this.entityPM.PotentialRevenue != value) {
                this.entityPM.PotentialRevenue = value;
                this.UpdateData("PotentialRevenue");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "PotentialTEU", {
        get: function () { return this.isActual ? this.actualEntityPM.TEU : this.entityPM.PotentialTEU; },
        set: function (value) {
            if (this.entityPM.PotentialTEU != value) {
                this.entityPM.PotentialTEU = value;
                this.UpdateData("PotentialTEU");
            }
        },
        enumerable: true,
        configurable: true
    });
    ProductLocationViewModel.prototype.UpdateData = function (fieldName) {
        if (this.IsOthers) {
            this.trigger.SetField(fieldName, true);
        }
        else {
            this.trigger.SetField(fieldName, false);
        }
    };
    Object.defineProperty(ProductLocationViewModel.prototype, "ButtonsVisibility", {
        get: function () { return this.IsOthers ? false : true; },
        enumerable: true,
        configurable: true
    });
    ProductLocationViewModel.prototype.DeleteCountry = function () {
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
    ProductLocationViewModel.prototype.SetField = function (fieldName, value) {
        if (!this.isActual) {
            switch (fieldName) {
                case "CommitmentNumberOfShipments":
                    {
                        if (value == null || value == 0)
                            this.entityPM.CommitmentNumberOfShipments = null;
                        else
                            this.entityPM.CommitmentNumberOfShipments = parseInt(value);
                        break;
                    }
                case "CommitmentChargeableWeight":
                    {
                        if (value == null || value == 0)
                            this.entityPM.CommitmentChargeableWeight = null;
                        else
                            this.entityPM.CommitmentChargeableWeight = parseFloat(value);
                        break;
                    }
                case "CommitmentRevenue":
                    {
                        if (value == null || value == 0)
                            this.entityPM.CommitmentRevenue = null;
                        else
                            this.entityPM.CommitmentRevenue = parseFloat(value);
                        break;
                    }
                case "CommitmentTEU":
                    {
                        if (value == null || value == 0)
                            this.entityPM.CommitmentTEU = null;
                        else
                            this.entityPM.CommitmentTEU = parseFloat(value);
                        break;
                    }
                case "PotentialNumberOfShipments":
                    {
                        if (value == null || value == 0)
                            this.entityPM.PotentialNumberOfShipments = null;
                        else
                            this.entityPM.PotentialNumberOfShipments = parseInt(value);
                        break;
                    }
                case "PotentialChargeableWeight":
                    {
                        if (value == null || value == 0)
                            this.entityPM.PotentialChargeableWeight = null;
                        else
                            this.entityPM.PotentialChargeableWeight = parseFloat(value);
                        break;
                    }
                case "PotentialRevenue":
                    {
                        if (value == null || value == 0)
                            this.entityPM.PotentialRevenue = null;
                        else
                            this.entityPM.PotentialRevenue = parseFloat(value);
                        break;
                    }
                case "PotentialTEU":
                    {
                        if (value == null || value == 0)
                            this.entityPM.PotentialTEU = null;
                        else
                            this.entityPM.PotentialTEU = parseFloat(value);
                        break;
                    }
            }
        }
    };
    Object.defineProperty(ProductLocationViewModel.prototype, "CommitmentNumberOfShipmentsForeground", {
        get: function () { return this.CommitmentNumberOfShipments < 0 ? "#E53030" : "#282E30"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "CommitmentChargeableWeightForeground", {
        get: function () { return this.CommitmentChargeableWeight < 0 ? "#E53030" : "#282E30"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "CommitmentRevenueForeground", {
        get: function () { return this.CommitmentRevenue < 0 ? "#E53030" : "#282E30"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "CommitmentTEUForeground", {
        get: function () { return this.CommitmentTEU < 0 ? "#E53030" : "#282E30"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "PotentialNumberOfShipmentsForeground", {
        get: function () { return this.PotentialNumberOfShipments < 0 ? "#E53030" : "#282E30"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "PotentialChargeableWeightForeground", {
        get: function () { return this.PotentialChargeableWeight < 0 ? "#E53030" : "#282E30"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "PotentialRevenueForeground", {
        get: function () { return this.PotentialRevenue < 0 ? "#E53030" : "#282E30"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductLocationViewModel.prototype, "PotentialTEUForeground", {
        get: function () { return this.PotentialTEU < 0 ? "#E53030" : "#282E30"; },
        enumerable: true,
        configurable: true
    });
    return ProductLocationViewModel;
}(BaseComponent_1.BaseComponent));
exports.ProductLocationViewModel = ProductLocationViewModel;
var CountryListViewModel = /** @class */ (function (_super) {
    __extends(CountryListViewModel, _super);
    function CountryListViewModel(item, entityPM, trigger) {
        var _this = _super.call(this) || this;
        _this.src = null;
        _this.ImgId = "";
        _this.Isvisible = false;
        _this.entityList = item;
        _this.entityPM = entityPM;
        _this.trigger = trigger;
        _this.isChecked = entityPM.ProductLocations.filter(function (d) { return d.CountryId == _this.entityList.Id; })[0] != null;
        var pipe = new CountryFlagPipe_1.CountryFlagPipe();
        _this.src = pipe.transform(_this.Code);
        return _this;
    }
    CountryListViewModel.prototype.ngOnInit = function () {
    };
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
                    var newItemPM = new CustomerProductLocationPM_1.CustomerProductLocationPM(null);
                    newItemPM.Tenant = this.entityPM.Tenant;
                    newItemPM.CountryId = this.entityList.Id;
                    newItemPM.CustomerId = this.entityPM.CustomerId;
                    newItemPM.ProductTypeCode = this.entityPM.ProductTypeCode;
                    newItemPM.CommitmentTEU = 0;
                    newItemPM.CommitmentRevenue = 0;
                    newItemPM.CommitmentChargeableWeight = 0;
                    newItemPM.CommitmentNumberOfShipments = 0;
                    newItemPM.PotentialTEU = 0;
                    newItemPM.PotentialRevenue = 0;
                    newItemPM.PotentialChargeableWeight = 0;
                    newItemPM.PotentialNumberOfShipments = 0;
                    newItemPM.CountryCode = this.Code;
                    newItemPM.CountryName = this.Name;
                    if (newItemPM != null) {
                        if (this.entityPM.ProductLocations.indexOf(newItemPM) == -1) {
                            this.entityPM.AddCustomerProductLocationPM(newItemPM);
                        }
                    }
                }
                else {
                    var itemPM = this.entityPM.ProductLocations.filter(function (d) { return d.CountryId == _this.entityList.Id; })[0];
                    if (itemPM != null) {
                        //var tempProductLocations: Array<CustomerProductLocationPM> = [];
                        //if (this.entityPM.ProductLocations.includes(itemPM)) {
                        //    this.entityPM.ProductLocations.forEach(item => {
                        //        if (item != itemPM)
                        //            tempProductLocations.push(item);
                        //    });
                        //    this.entityPM.ProductLocations = tempProductLocations;
                        //    this.trigger.entityPM.ProductLocations = this.entityPM.ProductLocations;
                        //}
                        if (this.entityPM.ProductLocations.indexOf(itemPM) != -1) {
                            this.entityPM.RemoveCustomerProductLocationPM(itemPM);
                        }
                    }
                }
                this.trigger.BuildProductLocations();
                this.trigger.OnLocationsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    return CountryListViewModel;
}(BaseComponent_1.BaseComponent));
exports.CountryListViewModel = CountryListViewModel;
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
//# sourceMappingURL=CustomerCommitmentsTabComponent.js.map