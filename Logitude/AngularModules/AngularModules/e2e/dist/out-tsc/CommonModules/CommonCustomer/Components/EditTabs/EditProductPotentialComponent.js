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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var CustomerCommitmentsTabComponent_1 = require("./CustomerCommitmentsTabComponent");
var CountryListService_1 = require("../../../../Common/Services/StandardLists/CountryListService");
var ClassLevelValidator_1 = require("../../../../Infrastructure/Validators/ClassLevelValidator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var CustomerProductLocationPM_1 = require("../../../../Common/EntityPMs/CustomerProductLocationPM");
var EditProductPotentialComponent = /** @class */ (function (_super) {
    __extends(EditProductPotentialComponent, _super);
    function EditProductPotentialComponent() {
        var _this = _super.call(this) || this;
        _this.myCurrencyCode = "";
        _this.ObjectTableName = "CustomerProduct";
        _this.EntityPM = null;
        _this.DataContext = _this;
        _this.PotentialTEUVisibility = true;
        _this.SearchTextId = "SearchTextId_";
        _this.SearchDropButtonId = "SearchDropButtonId_";
        _this.searchText = null;
        _this.CountriesToggleObsListTemp = [];
        _this.ValidationErrorsList = [];
        _this.DataLoaded = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.oldLocations = [];
        _this.SearchTextId += _this.CurrentSession.GetNewId(_this.SearchTextId);
        _this.SearchDropButtonId += _this.CurrentSession.GetNewId(_this.SearchTextId);
        _this._currencyListService = new CurrencyListService_1.CurrencyListService();
        if (!Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId)) {
            _this._currencyListService.getAll().subscribe(function (result) {
                var list = result.Result.filter(function (d) { return d.Id == (SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId); })[0];
                if (list != null) {
                    _this.myCurrencyCode = list.Code;
                }
                _this.CustomerProductionRevenueHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("CustomerProductLocation.F.Revenue") + " (" + _this.myCurrencyCode + ")";
            });
        }
        else {
            _this.CustomerProductionRevenueHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("CustomerProductLocation.F.Revenue");
        }
        return _this;
    }
    Object.defineProperty(EditProductPotentialComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            var _this = this;
            if (this.searchText != newValue) {
                this.searchText = newValue;
                setTimeout(function () { return _this.SearchCountries(); }, 500);
            }
        },
        enumerable: true,
        configurable: true
    });
    EditProductPotentialComponent.prototype.OnDeleteValue = function () {
        var temp = document.getElementById(this.SearchTextId);
        temp.value = null;
        this.SearchText = null;
        temp.focus();
    };
    EditProductPotentialComponent.prototype.ClearPlaceHolder = function () {
        var temp = document.getElementById(this.SearchTextId);
        temp.placeholder = "";
        this.SearchText = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchDropButtonId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
    };
    EditProductPotentialComponent.prototype.FillPlaceHolder = function () {
        if (!this.SearchText) {
            var temp = document.getElementById(this.SearchTextId);
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.SearchDropButtonId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    EditProductPotentialComponent.prototype.SearchCountries = function () {
        var _this = this;
        if (this.EntityPM != null) {
            this.EntityPM.CountriesToggleObsList = [];
            var data = [];
            var _countryListService = new CountryListService_1.CountryListService();
            if (this.CountriesToggleObsListTemp.length == 0) {
                _countryListService.getAllFromCache().subscribe(function (result) {
                    data = result.Result.filter(function (d) { return d.Tenant == SessionLocator_1.SessionLocator.Tenant; });
                    data.sort(function (a, b) { return (a.EnglishName === b.EnglishName) ? 0 : (a.EnglishName < b.EnglishName) ? -1 : 1; }).forEach(function (item) {
                        _this.CountriesToggleObsListTemp.push(new CustomerCommitmentsTabComponent_1.CountryListViewModel(item, _this.EntityPM.entityPM, _this.EntityPM.DataContext));
                    });
                    _this.EntityPM.CountriesToggleObsList = _this.CountriesToggleObsListTemp;
                });
            }
            else {
                if (Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
                    this.EntityPM.CountriesToggleObsList = this.CountriesToggleObsListTemp;
                }
                else {
                    this.EntityPM.CountriesToggleObsList = this.CountriesToggleObsListTemp.filter(function (f) { return f.Name.toLowerCase().indexOf(_this.SearchText.toLowerCase()) > -1; });
                }
            }
        }
    };
    EditProductPotentialComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args;
        this.SetColumnVisibility();
        this.SearchCountries();
        this.Clone(this.EntityPM);
    };
    EditProductPotentialComponent.prototype.CountyClicked = function (item, i) {
        // if (item.IsChecked == true && !this.EntityPM.CountriesToggleObsList.filter(d => d.Code == item.Code))
        ////      this.EntityPM.buil.BuildProductsToggleButtonList();
        // this.BuildProductsObsList();
    };
    EditProductPotentialComponent.prototype.SetColumnVisibility = function () {
        if (this.EntityPM != null) {
            if (this.EntityPM.TransportModeId == "A") {
                this.PotentialTEUVisibility = false;
            }
        }
    };
    //Commands
    EditProductPotentialComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    EditProductPotentialComponent.prototype.OkButtonClicked = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var validator;
        validator = new ClassLevelValidator_1.ClassLevelValidator();
        var errorsArray = validator.Validate("CustomerProduct", this.EntityPM.entityPM);
        var anyMinusAmount = false;
        if (this.EntityPM.isPotential) {
            if (this.EntityPM.PotentialTEU < 0 || this.EntityPM.PotentialRevenue < 0 || this.EntityPM.PotentialChargeableWeight < 0 || this.EntityPM.PotentialNumberOfShipments < 0) {
                anyMinusAmount = true;
            }
            else {
                if (this.EntityPM.ProductLocations.filter(function (d) { return d.PotentialTEU < 0; })[0]) {
                    anyMinusAmount = true;
                }
                else if (this.EntityPM.ProductLocations.filter(function (d) { return d.PotentialRevenue < 0; })[0]) {
                    anyMinusAmount = true;
                }
                else if (this.EntityPM.ProductLocations.filter(function (d) { return d.PotentialChargeableWeight < 0; })[0]) {
                    anyMinusAmount = true;
                }
                else if (this.EntityPM.ProductLocations.filter(function (d) { return d.PotentialNumberOfShipments < 0; })[0]) {
                    anyMinusAmount = true;
                }
            }
        }
        else {
            if (this.EntityPM.CommitmentTEU < 0 || this.EntityPM.CommitmentRevenue < 0 || this.EntityPM.CommitmentChargeableWeight < 0 || this.EntityPM.CommitmentNumberOfShipments < 0) {
                anyMinusAmount = true;
            }
            else {
                if (this.EntityPM.ProductLocations.filter(function (d) { return d.CommitmentTEU < 0; })[0]) {
                    anyMinusAmount = true;
                }
                else if (this.EntityPM.ProductLocations.filter(function (d) { return d.CommitmentRevenue < 0; })[0]) {
                    anyMinusAmount = true;
                }
                else if (this.EntityPM.ProductLocations.filter(function (d) { return d.CommitmentChargeableWeight < 0; })[0]) {
                    anyMinusAmount = true;
                }
                else if (this.EntityPM.ProductLocations.filter(function (d) { return d.CommitmentNumberOfShipments < 0; })[0]) {
                    anyMinusAmount = true;
                }
            }
        }
        if (anyMinusAmount) {
            errorsArray.push("Minus amounts are not allowed");
        }
        this.ValidationErrorsList = errorsArray;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    };
    EditProductPotentialComponent.prototype.Clone = function (myDataContext) {
        var _this = this;
        myDataContext.entityPM.ProductLocations.forEach(function (item) {
            var oldItem = new CustomerProductLocationPM_1.CustomerProductLocationPM(null);
            oldItem.CommitmentTEU = item.CommitmentTEU;
            oldItem.CommitmentRevenue = item.CommitmentRevenue;
            oldItem.CommitmentChargeableWeight = item.CommitmentChargeableWeight;
            oldItem.CommitmentNumberOfShipments = item.CommitmentNumberOfShipments;
            oldItem.PotentialTEU = item.PotentialTEU;
            oldItem.PotentialRevenue = item.PotentialRevenue;
            oldItem.PotentialChargeableWeight = item.PotentialChargeableWeight;
            oldItem.PotentialNumberOfShipments = item.PotentialNumberOfShipments;
            oldItem.ProductTypeCode = item.ProductTypeCode;
            oldItem.CountryId = item.CountryId;
            oldItem.CountryCode = item.CountryCode;
            oldItem.CountryName = item.CountryName;
            oldItem.CustomerId = item.CustomerId;
            oldItem.IsDirty = item.IsDirty;
            oldItem.ChangeSetOp = item.ChangeSetOp;
            oldItem.Tenant = item.Tenant;
            oldItem.EntityParentPM = item.EntityParentPM;
            oldItem.OldEntityPM = item.OldEntityPM;
            oldItem.UIProperties = item.UIProperties;
            oldItem.UniqueKey = item.UniqueKey;
            _this.oldLocations.push(oldItem);
        });
        this.myCloner = new Cloner_1.Cloner(myDataContext);
        this.myCloner.AddField('PotentialNumberOfShipments');
        this.myCloner.AddField('PotentialChargeableWeight');
        this.myCloner.AddField('PotentialRevenue');
        this.myCloner.AddField('PotentialTEU');
        this.myCloner.AddField('CommitmentNumberOfShipments');
        this.myCloner.AddField('CommitmentChargeableWeight');
        this.myCloner.AddField('CommitmentRevenue');
        this.myCloner.AddField('CommitmentTEU');
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(myDataContext.entityPM);
        this.myCloner.AddEntity(myDataContext.DataContext.customerPM);
    };
    EditProductPotentialComponent.prototype.RejectChanges = function () {
        var _this = this;
        var addedItems = [];
        var removedItems = [];
        this.oldLocations.forEach(function (item) {
            var existingItem = _this.EntityPM.entityPM.ProductLocations.filter(function (f) { return f.ProductTypeCode == item.ProductTypeCode && f.CountryId == item.CountryId; })[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });
        this.EntityPM.entityPM.ProductLocations.forEach(function (item) {
            var oldItem = _this.oldLocations.filter(function (f) { return f.ProductTypeCode == item.ProductTypeCode && f.CountryId == item.CountryId; })[0];
            if (oldItem) {
                if (item.CommitmentTEU != oldItem.CommitmentTEU) {
                    item.CommitmentTEU = oldItem.CommitmentTEU;
                }
                if (item.CommitmentRevenue != oldItem.CommitmentRevenue) {
                    item.CommitmentRevenue = oldItem.CommitmentRevenue;
                }
                if (item.CommitmentChargeableWeight != oldItem.CommitmentChargeableWeight) {
                    item.CommitmentChargeableWeight = oldItem.CommitmentChargeableWeight;
                }
                if (item.CommitmentNumberOfShipments != oldItem.CommitmentNumberOfShipments) {
                    item.CommitmentNumberOfShipments = oldItem.CommitmentNumberOfShipments;
                }
                if (item.PotentialTEU != oldItem.PotentialTEU) {
                    item.PotentialTEU = oldItem.PotentialTEU;
                }
                if (item.PotentialRevenue != oldItem.PotentialRevenue) {
                    item.PotentialRevenue = oldItem.PotentialRevenue;
                }
                if (item.PotentialChargeableWeight != oldItem.PotentialChargeableWeight) {
                    item.PotentialChargeableWeight = oldItem.PotentialChargeableWeight;
                }
                if (item.PotentialNumberOfShipments != oldItem.PotentialNumberOfShipments) {
                    item.PotentialNumberOfShipments = oldItem.PotentialNumberOfShipments;
                }
            }
            else {
                addedItems.push(item);
            }
        });
        addedItems.forEach(function (item) {
            _this.EntityPM.entityPM.RemoveCustomerProductLocationPM(item);
        });
        removedItems.forEach(function (item) {
            _this.EntityPM.entityPM.AddCustomerProductLocationPM(item);
        });
        this.EntityPM.BuildProductLocations();
        this.myCloner.RejectChanges();
    };
    EditProductPotentialComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditProductPotentialComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditProductPotentialComponent);
    return EditProductPotentialComponent;
}(BaseComponent_1.BaseComponent));
exports.EditProductPotentialComponent = EditProductPotentialComponent;
//# sourceMappingURL=EditProductPotentialComponent.js.map