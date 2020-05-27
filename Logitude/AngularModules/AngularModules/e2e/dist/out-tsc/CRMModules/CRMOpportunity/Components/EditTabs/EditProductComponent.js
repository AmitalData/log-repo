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
var OpportunityProductLocationPM_1 = require("../../../../CRM/EntityPMs/OpportunityProductLocationPM");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var OpportunityProductsTabComponent_1 = require("./OpportunityProductsTabComponent");
var CountryListService_1 = require("../../../../Common/Services/StandardLists/CountryListService");
var ClassLevelValidator_1 = require("../../../../Infrastructure/Validators/ClassLevelValidator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var EditProductComponent = /** @class */ (function (_super) {
    __extends(EditProductComponent, _super);
    function EditProductComponent() {
        var _this = _super.call(this) || this;
        _this.myCurrencyCode = "";
        _this.ObjectTableName = "OpportunityProduct";
        _this.EntityPM = null;
        _this.DataContext = _this;
        _this.TEUVisibility = true;
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
            _this._currencyListService.getAllFromCache().subscribe(function (result) {
                var list = result.Result.filter(function (d) { return d.Id == (SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId); })[0];
                if (list != null) {
                    _this.myCurrencyCode = list.Code;
                }
                _this.CustomerProductionRevenueHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("OpportunityProductLocation.F.Revenue") + " (" + _this.myCurrencyCode + ")";
            });
        }
        else {
            _this.CustomerProductionRevenueHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("OpportunityProductLocation.F.Revenue");
        }
        return _this;
    }
    Object.defineProperty(EditProductComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            this.searchText = newValue;
            this.SearchCountries();
        },
        enumerable: true,
        configurable: true
    });
    EditProductComponent.prototype.OnDeleteValue = function () {
        var temp = document.getElementById(this.SearchTextId);
        temp.value = null;
        this.SearchText = null;
        temp.focus();
    };
    EditProductComponent.prototype.ClearPlaceHolder = function () {
        var temp = document.getElementById(this.SearchTextId);
        temp.placeholder = "";
        this.SearchText = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchDropButtonId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
    };
    EditProductComponent.prototype.FillPlaceHolder = function () {
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
    EditProductComponent.prototype.SearchCountries = function () {
        var _this = this;
        if (this.EntityPM != null) {
            this.EntityPM.CountriesToggleObsList = [];
            var data = [];
            var _countryListService = new CountryListService_1.CountryListService();
            if (this.CountriesToggleObsListTemp.length == 0) {
                _countryListService.getAllFromCache().subscribe(function (result) {
                    data = result.Result.filter(function (d) { return d.Tenant == SessionLocator_1.SessionLocator.Tenant; });
                    data.sort(function (a, b) { return (a.EnglishName === b.EnglishName) ? 0 : (a.EnglishName < b.EnglishName) ? -1 : 1; }).forEach(function (item) {
                        _this.CountriesToggleObsListTemp.push(new OpportunityProductsTabComponent_1.CountryListViewModel(item, _this.EntityPM.entityPM, _this.EntityPM.DataContext));
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
    EditProductComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args;
        this.SetColumnVisibility();
        this.SearchCountries();
        this.Clone(args);
        args.SetEnabledFields();
    };
    EditProductComponent.prototype.CountyClicked = function (item, i) {
    };
    EditProductComponent.prototype.SetColumnVisibility = function () {
        if (this.EntityPM != null) {
            if (this.EntityPM.TransportModeId == "A") {
                this.TEUVisibility = false;
            }
        }
    };
    //Commands
    EditProductComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    EditProductComponent.prototype.OkButtonClicked = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var validator;
        validator = new ClassLevelValidator_1.ClassLevelValidator();
        var errorsArray = validator.Validate("OpportunityProduct", this.EntityPM.entityPM);
        var anyMinusAmount = false;
        if (this.EntityPM.TEU < 0 || this.EntityPM.Revenue < 0 || this.EntityPM.ChargeableWeight < 0 || this.EntityPM.NumberOfShipments < 0) {
            anyMinusAmount = true;
        }
        else {
            if (this.EntityPM.ProductLocations.filter(function (d) { return d.TEU < 0; })[0]) {
                anyMinusAmount = true;
            }
            else if (this.EntityPM.ProductLocations.filter(function (d) { return d.Revenue < 0; })[0]) {
                anyMinusAmount = true;
            }
            else if (this.EntityPM.ProductLocations.filter(function (d) { return d.ChargeableWeight < 0; })[0]) {
                anyMinusAmount = true;
            }
            else if (this.EntityPM.ProductLocations.filter(function (d) { return d.NumberOfShipments < 0; })[0]) {
                anyMinusAmount = true;
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
    EditProductComponent.prototype.Clone = function (myDataContext) {
        var _this = this;
        myDataContext.entityPM.OpportunityProductLocations.forEach(function (item) {
            var oldItem = new OpportunityProductLocationPM_1.OpportunityProductLocationPM(null);
            oldItem.TEU = item.TEU;
            oldItem.Revenue = item.Revenue;
            oldItem.ChargeableWeight = item.ChargeableWeight;
            oldItem.NumberOfShipments = item.NumberOfShipments;
            oldItem.CountryId = item.CountryId;
            oldItem.LocationCode = item.LocationCode;
            oldItem.LocationName = item.LocationName;
            oldItem.OpportunityId = item.OpportunityId;
            oldItem.OpportunityProductTypeCode = item.OpportunityProductTypeCode;
            oldItem.Tenant = item.Tenant;
            oldItem.LineNumber = item.LineNumber;
            oldItem.ChangeSetOp = item.ChangeSetOp;
            oldItem.OldEntityPM = item.OldEntityPM;
            oldItem.UIProperties = item.UIProperties;
            oldItem.UniqueKey = item.UniqueKey;
            oldItem.IsDirty = item.IsDirty;
            oldItem.EntityParentPM = item.EntityParentPM;
            _this.oldLocations.push(oldItem);
        });
        this.myCloner = new Cloner_1.Cloner(myDataContext);
        this.myCloner.AddField('NumberOfShipments');
        this.myCloner.AddField('ChargeableWeight');
        this.myCloner.AddField('Revenue');
        this.myCloner.AddField('TEU');
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('PrepaidCollectId');
        this.myCloner.AddEntity(myDataContext.entityPM);
        this.myCloner.AddEntity(myDataContext.trigger.EntityPM);
    };
    EditProductComponent.prototype.RejectChanges = function () {
        var _this = this;
        var addedItems = [];
        var removedItems = [];
        this.oldLocations.forEach(function (item) {
            var existingItem = _this.EntityPM.entityPM.OpportunityProductLocations.filter(function (f) { return f.OpportunityProductTypeCode == item.OpportunityProductTypeCode && f.CountryId == item.CountryId; })[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });
        this.EntityPM.entityPM.OpportunityProductLocations.forEach(function (item) {
            var oldItem = _this.oldLocations.filter(function (f) { return f.OpportunityProductTypeCode == item.OpportunityProductTypeCode && f.CountryId == item.CountryId; })[0];
            if (oldItem) {
                if (item.TEU != oldItem.TEU) {
                    item.TEU = oldItem.TEU;
                }
                if (item.ChargeableWeight != oldItem.ChargeableWeight) {
                    item.ChargeableWeight = oldItem.ChargeableWeight;
                }
                if (item.Revenue != oldItem.Revenue) {
                    item.Revenue = oldItem.Revenue;
                }
                if (item.NumberOfShipments != oldItem.NumberOfShipments) {
                    item.NumberOfShipments = oldItem.NumberOfShipments;
                }
            }
            else {
                addedItems.push(item);
            }
        });
        addedItems.forEach(function (item) {
            _this.EntityPM.entityPM.RemoveOpportunityProductLocation(item);
        });
        removedItems.forEach(function (item) {
            _this.EntityPM.entityPM.AddOpportunityProductLocation(item);
        });
        this.EntityPM.BuildProductLocations();
        this.myCloner.RejectChanges();
    };
    EditProductComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditProductComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditProductComponent);
    return EditProductComponent;
}(BaseComponent_1.BaseComponent));
exports.EditProductComponent = EditProductComponent;
//# sourceMappingURL=EditProductComponent.js.map