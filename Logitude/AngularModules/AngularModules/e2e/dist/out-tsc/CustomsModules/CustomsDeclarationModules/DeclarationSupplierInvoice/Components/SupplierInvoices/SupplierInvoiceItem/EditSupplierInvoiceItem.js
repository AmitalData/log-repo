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
var Tools_1 = require("../../../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ObservableCollection_1 = require("../../../../../../Infrastructure/Utilities/ObservableCollection");
var EntityResourceService_1 = require("../../../../../../Infrastructure/Services/EntityResourceService");
var ApiQueryFilters_1 = require("../../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var DeclarationValidator_1 = require("../../../../../../Customs/Validators/DeclarationValidator");
var LogitudeWindow_1 = require("../../../../../../Controls/Windows/LogitudeWindow");
var SupplierInvoiceItemPM_1 = require("../../../../../../Customs/EntityPMs/SupplierInvoiceItemPM");
var SupplierInvoiceItemsModPM_1 = require("../../../../../../Customs/EntityPMs/SupplierInvoiceItemsModPM");
var SupplierInvoiceItemProcesTypePM_1 = require("../../../../../../Customs/EntityPMs/SupplierInvoiceItemProcesTypePM");
var SupplierInvoiceItemsConDeclarPM_1 = require("../../../../../../Customs/EntityPMs/SupplierInvoiceItemsConDeclarPM");
var SupplierInvoiceItemsSerialNumPM_1 = require("../../../../../../Customs/EntityPMs/SupplierInvoiceItemsSerialNumPM");
var SupplierInvoiceItemsDescriptPM_1 = require("../../../../../../Customs/EntityPMs/SupplierInvoiceItemsDescriptPM");
var SupplierInvoiceItemsProdIdentPM_1 = require("../../../../../../Customs/EntityPMs/SupplierInvoiceItemsProdIdentPM");
var SupplierInvoiceItemsLevyPM_1 = require("../../../../../../Customs/EntityPMs/SupplierInvoiceItemsLevyPM");
var CustomsRequiredFieldListService_1 = require("../../../../../../Customs/Services/StandardLists/CustomsRequiredFieldListService");
var LuhnAlgorithm_1 = require("../../../../../../Customs/Utilities/LuhnAlgorithm");
var ObjectsLocator_1 = require("../../../../../../Infrastructure/Locators/ObjectsLocator");
var EditSupplierInvoiceItem = /** @class */ (function (_super) {
    __extends(EditSupplierInvoiceItem, _super);
    function EditSupplierInvoiceItem(cd) {
        var _this = _super.call(this) || this;
        _this.cd = cd;
        _this.DataContext = _this;
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.declarationValidator = new DeclarationValidator_1.DeclarationValidator();
        _this.ValidationErrorsList = [];
        _this.ObjectTableName = "Customs.SupplierInvoiceItem";
        _this.LayoutDirection = 'ltr';
        _this.IsDisplayOnly = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#region Tabs Code
        _this.TabsSource = [];
        _this.SelectedTab = "";
        _this.modificationCounter = 0;
        _this.ptNumber = 0;
        _this.myNumber = 0;
        _this.snNumber = 0;
        _this.descNumber = 0;
        _this.prodIdNumber = 0;
        _this.LevyNumber = 0;
        //#endregion
        //#region Ok, Cancel Buttons Handler
        _this.hasDash = false;
        _this.digit = null;
        _this.checkDigit = 0;
        _this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        _this.CustomsBookTypeFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.CustomsBookTypeFilterItems.addAdditionalFilter("Code", "2", null, null, "Exclude", false, false, false, "string", false, true);
        _this.BuildTabs();
        // Initilize lists
        _this.ModificationsList = new ObservableCollection_1.ObservableCollection([]);
        _this.ProcessTypesList = new ObservableCollection_1.ObservableCollection([]);
        _this.ConDeclarList = new ObservableCollection_1.ObservableCollection([]);
        _this.SerialNumbersList = new ObservableCollection_1.ObservableCollection([]);
        _this.DescriptionsList = new ObservableCollection_1.ObservableCollection([]);
        _this.IdentificationsList = new ObservableCollection_1.ObservableCollection([]);
        _this.LevyList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    EditSupplierInvoiceItem.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.IsDisplayOnly = args.IsDisplayOnly;
            this.OriginalItemPM = args.SupplierInvoiceItemPM;
            this.ClonedItemPM = this.CloneEntity(args.SupplierInvoiceItemPM);
            this.CustomsItem = this.OriginalItemPM.TaxExemptCode;
            this.FillGridsData(); // copy  grids data from entity PM to ItemSource arrays
            if (this.IsDisplayOnly) {
                this.SetScreenFieldsEditability();
            }
        }
        console.log("--> EditSupplierInvoiceItem window argument passed: ", args);
        this.CheckRequrierdFieldsForSend();
    };
    EditSupplierInvoiceItem.prototype.FillGridsData = function () {
        // Modification List
        this.ModificationsList = new ObservableCollection_1.ObservableCollection([]);
        for (var _i = 0, _a = this.OriginalItemPM.SupplierInvoiceItemsMods; _i < _a.length; _i++) {
            var item = _a[_i];
            this.ModificationsList.Insert(new ModificationItemModel(item));
        }
        // ProcessTypes List
        this.ProcessTypesList = new ObservableCollection_1.ObservableCollection([]);
        for (var _b = 0, _c = this.OriginalItemPM.SupplierInvoiceItemProcesTypes; _b < _c.length; _b++) {
            var item = _c[_b];
            this.ProcessTypesList.Insert(new ProcessTypeItemModel(item));
        }
        // ConDeclar List 
        this.ConDeclarList = new ObservableCollection_1.ObservableCollection([]);
        for (var _d = 0, _e = this.OriginalItemPM.SupplierInvoiceItemsConDeclars; _d < _e.length; _d++) {
            var item = _e[_d];
            this.ConDeclarList.Insert(new ConDeclarItemModel(item));
        }
        // SerialNumbers List 
        this.SerialNumbersList = new ObservableCollection_1.ObservableCollection([]);
        for (var _f = 0, _g = this.OriginalItemPM.SupplierInvoiceItemsSerialNums; _f < _g.length; _f++) {
            var item = _g[_f];
            this.SerialNumbersList.Insert(new SerialNoItemModel(item));
        }
        // Descriptions List 
        this.DescriptionsList = new ObservableCollection_1.ObservableCollection([]);
        for (var _h = 0, _j = this.OriginalItemPM.SupplierInvoiceItemsDescripts; _h < _j.length; _h++) {
            var item = _j[_h];
            this.DescriptionsList.Insert(new DescribtionItemModel(item));
        }
        // Identifications List 
        this.IdentificationsList = new ObservableCollection_1.ObservableCollection([]);
        for (var _k = 0, _l = this.OriginalItemPM.SupplierInvoiceItemsProdIdents; _k < _l.length; _k++) {
            var item = _l[_k];
            this.IdentificationsList.Insert(new ProdIdentItemModel(item));
        }
        // Levies List 
        this.LevyList = new ObservableCollection_1.ObservableCollection([]);
        for (var _m = 0, _o = this.OriginalItemPM.SupplierInvoiceItemLevies; _m < _o.length; _m++) {
            var item = _o[_m];
            this.LevyList.Insert(new LevyItemModel(item));
        }
    };
    EditSupplierInvoiceItem.prototype.SetScreenFieldsEditability = function () {
        this.UIProperties.SetEnabled("StatisticQuantity", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("StatisticQuantityType", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("AdditionalQuantity", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("AdditionalQuantityType", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CustomsBookTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("PreferenceDocumentNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ActualInvoiceLines", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeferredCustomsTax", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeferredPurchaseTax", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("SalesTaxExemptionTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TaxExemptCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("OptionalTamaPercentage", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("NonCustomsItemPrice", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("NonCustomsItemPriceCurCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("WholeSaleItemPrice", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("WholeSaleItemPriceCurrencyCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IsUsed", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ManufactureIdentifier", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DangerousClassificationCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DangerousPackingGroupTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
    };
    EditSupplierInvoiceItem.prototype.CheckRequrierdFieldsForSend = function () {
        var _this = this;
        var customsRequiredFieldListService = new CustomsRequiredFieldListService_1.CustomsRequiredFieldListService();
        var table = window.ObjectTables.filter(function (d) { return d.Name == 'Customs.SupplierInvoiceItem'; })[0];
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.addAdditionalFilter("ObjectTableId", table.Id, null, null, "Equals", false, false, false, "string");
        customsRequiredFieldListService.getAllFromCache(filters).subscribe(function (response) {
            var requiredFields = response.Result;
            requiredFields.forEach(function (field) {
                var objectField = window.ObjectFields.filter(function (d) { return d.Id == field.ObjectfieldId; })[0];
                _this.UIProperties.SetWarning(objectField.FieldName, 'Customs.SupplierInvoiceItem', true);
            });
        });
    };
    EditSupplierInvoiceItem.prototype.BuildTabs = function () {
        this.SelectedTab = "Details";
        this.TabsSource.push({ Name: "Details", isSelected: true, Header: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Details") });
        this.TabsSource.push({ Name: "Declarations", isSelected: false, Header: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Declarations") });
        this.TabsSource.push({ Name: "SerialNumbers", isSelected: false, Header: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.SerialNumbers") });
        this.TabsSource.push({ Name: "Levies", isSelected: false, Header: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Levies") });
    };
    EditSupplierInvoiceItem.prototype.SelectionChanged = function (tab) {
        this.TabsSource.forEach(function (item) {
            item.isSelected = false;
        });
        var index = this.TabsSource.indexOf(tab);
        if (index < 0) {
            console.log("The tab was not found, cant not delete it :( ", tab);
            return;
        }
        var item = this.TabsSource[index];
        item.isSelected = true;
        this.SelectedTab = item.Name;
    };
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "StatisticQuantity", {
        //#endregion
        //#region Tab: Details
        //#region Properties
        get: function () { return this.OriginalItemPM.StatisticQuantity; },
        set: function (value) {
            if (this.OriginalItemPM.StatisticQuantity != value) {
                this.OriginalItemPM.StatisticQuantity = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "StatisticQuantityType", {
        get: function () { return this.OriginalItemPM.StatisticQuantityType; },
        set: function (value) {
            if (this.OriginalItemPM.StatisticQuantityType != value) {
                this.OriginalItemPM.StatisticQuantityType = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "AdditionalQuantity", {
        get: function () { return this.OriginalItemPM.AdditionalQuantity; },
        set: function (value) {
            if (this.OriginalItemPM.AdditionalQuantity != value) {
                this.OriginalItemPM.AdditionalQuantity = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "AdditionalQuantityType", {
        get: function () { return this.OriginalItemPM.AdditionalQuantityType; },
        set: function (value) {
            if (this.OriginalItemPM.AdditionalQuantityType != value) {
                this.OriginalItemPM.AdditionalQuantityType = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "CustomsBookTypeCode", {
        get: function () { return this.OriginalItemPM.CustomsBookTypeCode; },
        set: function (value) {
            if (this.OriginalItemPM.CustomsBookTypeCode != value) {
                this.OriginalItemPM.CustomsBookTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "PreferenceDocumentNumber", {
        get: function () { return this.OriginalItemPM.PreferenceDocumentNumber; },
        set: function (value) {
            if (this.OriginalItemPM.PreferenceDocumentNumber != value) {
                this.OriginalItemPM.PreferenceDocumentNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "ActualInvoiceLines", {
        get: function () { return this.OriginalItemPM.ActualInvoiceLines; },
        set: function (value) {
            if (this.OriginalItemPM.ActualInvoiceLines != value) {
                this.OriginalItemPM.ActualInvoiceLines = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "DeferredCustomsTax", {
        get: function () { return this.OriginalItemPM.DeferredCustomsTax; },
        set: function (value) {
            if (this.OriginalItemPM.DeferredCustomsTax != value) {
                this.OriginalItemPM.DeferredCustomsTax = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "DeferredPurchaseTax", {
        get: function () { return this.OriginalItemPM.DeferredPurchaseTax; },
        set: function (value) {
            if (this.OriginalItemPM.DeferredPurchaseTax != value) {
                this.OriginalItemPM.DeferredPurchaseTax = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "SalesTaxExemptionTypeCode", {
        get: function () { return this.OriginalItemPM.SalesTaxExemptionTypeCode; },
        set: function (value) {
            if (this.OriginalItemPM.SalesTaxExemptionTypeCode != value) {
                this.OriginalItemPM.SalesTaxExemptionTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "TaxExemptCode", {
        get: function () { return this.OriginalItemPM.TaxExemptCode; },
        set: function (value) {
            if (this.OriginalItemPM.TaxExemptCode != value) {
                this.OriginalItemPM.TaxExemptCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "OptionalTamaPercentage", {
        get: function () { return this.OriginalItemPM.OptionalTamaPercentage; },
        set: function (value) {
            if (this.OriginalItemPM.OptionalTamaPercentage != value) {
                this.OriginalItemPM.OptionalTamaPercentage = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "NonCustomsItemPrice", {
        get: function () { return this.OriginalItemPM.NonCustomsItemPrice; },
        set: function (value) {
            if (this.OriginalItemPM.NonCustomsItemPrice != value) {
                this.OriginalItemPM.NonCustomsItemPrice = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "NonCustomsItemPriceCurCode", {
        get: function () { return this.OriginalItemPM.NonCustomsItemPriceCurCode; },
        set: function (value) {
            if (this.OriginalItemPM.NonCustomsItemPriceCurCode != value) {
                this.OriginalItemPM.NonCustomsItemPriceCurCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "WholeSaleItemPrice", {
        get: function () { return this.OriginalItemPM.WholeSaleItemPrice; },
        set: function (value) {
            if (this.OriginalItemPM.WholeSaleItemPrice != value) {
                this.OriginalItemPM.WholeSaleItemPrice = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "WholeSaleItemPriceCurrencyCode", {
        get: function () { return this.OriginalItemPM.WholeSaleItemPriceCurrencyCode; },
        set: function (value) {
            if (this.OriginalItemPM.WholeSaleItemPriceCurrencyCode != value) {
                this.OriginalItemPM.WholeSaleItemPriceCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "IsUsed", {
        get: function () { return this.OriginalItemPM.IsUsed; },
        set: function (value) {
            if (this.OriginalItemPM.IsUsed != value) {
                this.OriginalItemPM.IsUsed = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "ManufactureIdentifier", {
        get: function () { return this.OriginalItemPM.ManufactureIdentifier; },
        set: function (value) {
            if (this.OriginalItemPM.ManufactureIdentifier != value) {
                this.OriginalItemPM.ManufactureIdentifier = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "DangerousClassificationCode", {
        get: function () { return this.OriginalItemPM.DangerousClassificationCode; },
        set: function (value) {
            if (this.OriginalItemPM.DangerousClassificationCode != value) {
                if (value) {
                    this.OriginalItemPM.DangerousClassificationCode = value + "";
                }
                else {
                    this.OriginalItemPM.DangerousClassificationCode = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "DangerousPackingGroupTypeCode", {
        get: function () { return this.OriginalItemPM.DangerousPackingGroupTypeCode; },
        set: function (value) {
            if (this.OriginalItemPM.DangerousPackingGroupTypeCode != value) {
                this.OriginalItemPM.DangerousPackingGroupTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    EditSupplierInvoiceItem.prototype.AddModificationButton = function () {
        if (this.IsModificationValid(this.ModificationsList.Collection[this.ModificationsList.Collection.length - 1])) {
            var item = new SupplierInvoiceItemsModPM_1.SupplierInvoiceItemsModPM(this.OriginalItemPM);
            if (this.ModificationsList.Length > 0) {
                this.modificationCounter = this.getMax(this.ModificationsList.Collection, "ModificationCounterKey");
            }
            item.DeclarationId = this.OriginalItemPM.DeclarationId,
                item.Tenant = this.OriginalItemPM.Tenant,
                item.InvoiceCounterKey = this.OriginalItemPM.CounterKey,
                item.LineNumber = this.OriginalItemPM.LineNumber,
                item.ModificationCounterKey = this.modificationCounter,
                item.ChangeSetOp = "Insert",
                this.OriginalItemPM.AddSupplierInvoiceItemsMod(item);
            this.ModificationsList.Insert(new ModificationItemModel(item));
            //this.CurrentSession.ResetRowIndex();
        }
    };
    EditSupplierInvoiceItem.prototype.RemoveModification = function (item) {
        console.log("... Removing ", item);
        this.ModificationsList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemsMod(item.ModificationPM); // remove from entity
    };
    EditSupplierInvoiceItem.prototype.IsModificationValid = function (modificationM) {
        if (!Tools_1.AppTool.IsNullOrEmpty(modificationM)) {
            // requierd fields for last row
            if (!Tools_1.AppTool.IsNullOrEmpty(modificationM.TypeCode) && !Tools_1.AppTool.IsNullOrEmpty(modificationM.CurrencyTypeCode)) {
                return true;
            }
            return false;
        }
        return true;
    };
    EditSupplierInvoiceItem.prototype.OnRowEnded = function ($event) {
        console.log("Length : " + this.ModificationsList.Length);
        if (($event) == this.ModificationsList.Length) {
            this.AddModificationButton();
        }
    };
    EditSupplierInvoiceItem.prototype.OnFocus = function () {
        //if (this.ModificationsList.Length == 0) {
        //    this.AddModificationButton();
        //}
    };
    EditSupplierInvoiceItem.prototype.AddProcessTypeButton = function () {
        var pm = new SupplierInvoiceItemProcesTypePM_1.SupplierInvoiceItemProcesTypePM(this.OriginalItemPM);
        //max
        if (this.ProcessTypesList.Length > 0) {
            this.ptNumber = this.getMax(this.ProcessTypesList.Collection, "LineNumber");
        }
        pm.LineNumber = ++this.ptNumber;
        pm.DeclarationId = this.OriginalItemPM.DeclarationId;
        pm.Tenant = this.OriginalItemPM.Tenant;
        pm.InvoiceCounterKey = this.OriginalItemPM.CounterKey;
        pm.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber;
        this.ProcessTypesList.Insert(new ProcessTypeItemModel(pm));
        this.OriginalItemPM.AddSupplierInvoiceItemProcesType(pm);
    };
    EditSupplierInvoiceItem.prototype.RemoveProcessType = function (item) {
        this.ProcessTypesList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemProcesType(item.ProcessTypePM);
    };
    //#endregion
    EditSupplierInvoiceItem.prototype.OnDangClasCodeBlur = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DangerousClassificationCode)) { // because we convert the number value to text (+"")
            if (this.DangerousClassificationCode.length < 8) {
                this.UIProperties.SetValidity("DangerousClassificationCode", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort"));
            }
            else if (this.DangerousClassificationCode.length > 8) {
                this.UIProperties.SetValidity("DangerousClassificationCode", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CodeLong"));
            }
            else { //valid
                this.UIProperties.SetValidity("DangerousClassificationCode", this.ObjectTableName, true, "");
            }
        }
        else {
            this.UIProperties.SetValidity("DangerousClassificationCode", this.ObjectTableName, true, "");
        }
    };
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "CustomsItem", {
        get: function () { return this.customsItem; },
        set: function (value) {
            this.customsItem = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditSupplierInvoiceItem.prototype, "CustomsItemTextValue", {
        get: function () { return this.customsItemTextValue; },
        set: function (value) {
            this.customsItemTextValue = value;
        },
        enumerable: true,
        configurable: true
    });
    EditSupplierInvoiceItem.prototype.CustomsItemClicked = function (item) {
        if (item) {
            this.CustomsItem = null;
            this.cd.detectChanges();
            this.CustomsItem = item.FullClassification;
            //this.CustomsItem = item.FullClassification + (item.ComputedCheckDigit ? item.ComputedCheckDigit : "");
        }
    };
    EditSupplierInvoiceItem.prototype.CustomsItemTextChanged = function (text) {
        this.CustomsItemTextValue = text;
    };
    EditSupplierInvoiceItem.prototype.CustomsItemLostFocus = function (text) {
        if (!Tools_1.AppTool.IsNullOrEmpty(text))
            this.ValidateCustomsItemField(text);
        this.TaxExemptCode = this.CustomsItemTextValue;
        this.CustomsItem = this.CustomsItemTextValue;
    };
    EditSupplierInvoiceItem.prototype.AddConnDeclarButton = function () {
        var item = new SupplierInvoiceItemsConDeclarPM_1.SupplierInvoiceItemsConDeclarPM(this.OriginalItemPM);
        //max
        if (this.ConDeclarList.Length > 0) {
            this.myNumber = this.getMax(this.ConDeclarList.Collection, "LineNumber");
        }
        item.LineNumber = ++this.myNumber;
        item.DeclarationId = this.OriginalItemPM.DeclarationId;
        item.Tenant = this.OriginalItemPM.Tenant;
        item.InvoiceCounterKey = this.OriginalItemPM.CounterKey;
        item.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber;
        this.OriginalItemPM.AddSupplierInvoiceItemsConDeclar(item);
        this.ConDeclarList.Insert(new ConDeclarItemModel(item));
    };
    EditSupplierInvoiceItem.prototype.RemoveConnDeclar = function (item) {
        this.ConDeclarList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemsConDeclar(item.ConnDeclarPM); // remove from entity
    };
    EditSupplierInvoiceItem.prototype.AddSerialNumberButton = function () {
        var item = new SupplierInvoiceItemsSerialNumPM_1.SupplierInvoiceItemsSerialNumPM(this.OriginalItemPM);
        //max
        if (this.SerialNumbersList.Length > 0)
            this.snNumber = this.getMax(this.SerialNumbersList.Collection, "LineNumber");
        item.LineNumber = ++this.snNumber;
        item.DeclarationId = this.OriginalItemPM.DeclarationId;
        item.Tenant = this.OriginalItemPM.Tenant;
        item.InvoiceCounterKey = this.OriginalItemPM.CounterKey;
        item.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber;
        this.OriginalItemPM.AddSupplierInvoiceItemsSerialNum(item);
        this.SerialNumbersList.Insert(new SerialNoItemModel(item));
        //this.CurrentSession.ResetRowIndex();
    };
    EditSupplierInvoiceItem.prototype.RemoveSerialNumber = function (item) {
        this.SerialNumbersList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemsSerialNum(item.SerialNoPM); // remove from entity
    };
    EditSupplierInvoiceItem.prototype.AddDescriptionButton = function () {
        var item = new SupplierInvoiceItemsDescriptPM_1.SupplierInvoiceItemsDescriptPM(this.OriginalItemPM);
        //max
        if (this.DescriptionsList.Length > 0) {
            this.descNumber = this.getMax(this.DescriptionsList.Collection, "LineNumber");
        }
        item.LineNumber = ++this.descNumber;
        item.DeclarationId = this.OriginalItemPM.DeclarationId;
        item.Tenant = this.OriginalItemPM.Tenant;
        item.InvoiceCounterKey = this.OriginalItemPM.CounterKey;
        item.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber;
        this.OriginalItemPM.AddSupplierInvoiceItemsDescript(item);
        this.DescriptionsList.Insert(new DescribtionItemModel(item));
        //this.CurrentSession.ResetRowIndex();
    };
    EditSupplierInvoiceItem.prototype.RemoveDescription = function (item) {
        this.DescriptionsList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemsDescript(item.DescribtionPM); // remove from entity
    };
    EditSupplierInvoiceItem.prototype.AddIdentificationButton = function () {
        var item = new SupplierInvoiceItemsProdIdentPM_1.SupplierInvoiceItemsProdIdentPM(this.OriginalItemPM);
        //max
        if (this.IdentificationsList.Length > 0) {
            this.prodIdNumber = this.getMax(this.IdentificationsList.Collection, "LineNumber");
        }
        item.LineNumber = ++this.prodIdNumber;
        item.DeclarationId = this.OriginalItemPM.DeclarationId;
        item.Tenant = this.OriginalItemPM.Tenant;
        item.InvoiceCounterKey = this.OriginalItemPM.CounterKey;
        item.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber;
        this.OriginalItemPM.AddSupplierInvoiceItemsProdIdent(item);
        this.IdentificationsList.Insert(new ProdIdentItemModel(item));
        //this.CurrentSession.ResetRowIndex();
    };
    EditSupplierInvoiceItem.prototype.RemoveIdentification = function (item) {
        this.IdentificationsList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemsProdIdent(item.ProdIdentPM); // remove from entity
    };
    EditSupplierInvoiceItem.prototype.AddLevyButton = function () {
        var item = new SupplierInvoiceItemsLevyPM_1.SupplierInvoiceItemsLevyPM(this.OriginalItemPM);
        //max
        if (this.LevyList.Length > 0) {
            this.LevyNumber = this.getMax(this.LevyList.Collection, "LineNumber");
        }
        item.LineNumber = ++this.LevyNumber;
        item.DeclarationId = this.OriginalItemPM.DeclarationId;
        item.Tenant = this.OriginalItemPM.Tenant;
        item.InvoiceCounterKey = this.OriginalItemPM.CounterKey;
        item.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber;
        this.OriginalItemPM.AddSupplierInvoiceItemsLevy(item);
        this.LevyList.Insert(new LevyItemModel(item));
        //this.CurrentSession.ResetRowIndex();
    };
    EditSupplierInvoiceItem.prototype.Removelevy = function (item) {
        this.LevyList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemsLevy(item.LevyPM); // remove from entity
    };
    EditSupplierInvoiceItem.prototype.OkButtonClicked = function () {
        var isValid = this.ValidateCustomsItemField();
        if (!isValid)
            return;
        this.TaxExemptCode = this.CustomsItemTextValue;
        var errors = [];
        this.ValidationErrorsList = errors;
        errors = this.declarationValidator.ValidateSupplierInvoiceItem(this.OriginalItemPM);
        if (this.StatisticQuantity != null || this.AdditionalQuantity != null || this.CustomsBookTypeCode != null || this.PreferenceDocumentNumber != null || this.ActualInvoiceLines != null
            || this.DeferredCustomsTax != null || this.DeferredPurchaseTax != null || this.SalesTaxExemptionTypeCode != null || this.TaxExemptCode != null || this.OptionalTamaPercentage != null
            || this.NonCustomsItemPrice != null || this.WholeSaleItemPrice != null || this.IsUsed || this.ManufactureIdentifier != null || this.DangerousClassificationCode != null || this.DangerousPackingGroupTypeCode != null
            || this.OriginalItemPM.SupplierInvoiceItemsMods.length > 0 || this.OriginalItemPM.SupplierInvoiceItemProcesTypes.length > 0 || this.OriginalItemPM.SupplierInvoiceItemsConDeclars.length > 0 || this.OriginalItemPM.SupplierInvoiceItemLevies.length > 0
            || this.OriginalItemPM.SupplierInvoiceItemsDescripts.length > 0 || this.OriginalItemPM.SupplierInvoiceItemsSerialNums.length > 0 || this.OriginalItemPM.SupplierInvoiceItemsProdIdents.length > 0) {
            this.OriginalItemPM.ItemAdditionalStatus = true;
        }
        else {
            this.OriginalItemPM.ItemAdditionalStatus = false;
        }
        //console.log("Ok, New -> ", this.ClonedItemPM)
        //console.log("    Old -> ", this.OldItemPM)
        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    EditSupplierInvoiceItem.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    EditSupplierInvoiceItem.prototype.ValidateCustomsItemField = function (text) {
        if (text === void 0) { text = null; }
        this.CustomItemErrorMessage = null;
        var valid = true;
        if (!this.CustomsItemTextValue && this.CustomsItem) // set the value when entering the window
            this.CustomsItemTextValue = this.CustomsItem;
        if (text) {
            this.CustomsItemTextValue = text;
            this.CustomsItem = text;
        }
        var value = this.CustomsItemTextValue;
        if (value) {
            this.hasDash = value.includes("-");
            value = value.replace("-", "");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(value)) {
            this.CustomItemErrorMessage = null;
        }
        else if (value.toString().length > 12) {
            this.CustomItemErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CodeLong");
            valid = false;
        }
        else if (value.toString().length < 8) {
            this.CustomItemErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort");
            valid = false;
        }
        else if (value.toString().length == 8) {
            value = value + "00";
            this.checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(value);
            if (this.checkDigit)
                value = value + this.checkDigit;
        }
        else if (value.toString().length == 9) {
            this.digit = value.toString().substring(8);
            value = value.toString().substring(0, 8) + "00" + value.toString().substring(8);
            this.checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(value.substring(0, 10));
            if (this.digit != this.checkDigit.toString()) {
                this.CustomItemErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + this.checkDigit.toString();
                valid = false;
            }
            else {
            }
        }
        else if (value.toString().length == 10) {
            this.checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(value);
            if (this.checkDigit)
                value = value + "" + this.checkDigit;
        }
        else if (value.toString().length == 11) {
            this.digit = value.toString().substring(10);
            this.checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(value.toString().substring(0, 10));
            if (this.digit != this.checkDigit.toString()) {
                this.CustomItemErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + this.checkDigit.toString();
                valid = false;
            }
            else {
            }
        }
        else {
            this.CustomItemErrorMessage = null;
        }
        this.CustomsItemTextValue = value;
        if (this.hasDash)
            this.CustomsItemTextValue = "-" + this.CustomsItemTextValue;
        return valid;
    };
    //#endregion
    //#region (Clone) Code
    EditSupplierInvoiceItem.prototype.CloneEntity = function (entityToClone) {
        var _this = this;
        var clonedEntity;
        clonedEntity = new SupplierInvoiceItemPM_1.SupplierInvoiceItemPM(entityToClone.EntityParentPM); // check it !!
        this.MapEntitytoEntity(entityToClone, clonedEntity);
        // --------------------------[ Arrays ]------------------------------
        // Modification
        clonedEntity.SupplierInvoiceItemsMods = [];
        entityToClone.SupplierInvoiceItemsMods.forEach(function (itemMod) {
            var clonedItemMod = new SupplierInvoiceItemsModPM_1.SupplierInvoiceItemsModPM(itemMod.EntityParentPM);
            _this.MapEntitytoEntity(itemMod, clonedItemMod);
            clonedEntity.SupplierInvoiceItemsMods.push(clonedItemMod);
        });
        //Process Types
        clonedEntity.SupplierInvoiceItemProcesTypes = [];
        entityToClone.SupplierInvoiceItemProcesTypes.forEach(function (item) {
            var clonedItem = new SupplierInvoiceItemProcesTypePM_1.SupplierInvoiceItemProcesTypePM(item.EntityParentPM);
            _this.MapEntitytoEntity(item, clonedItem);
            clonedEntity.SupplierInvoiceItemProcesTypes.push(clonedItem);
        });
        // ConDeclar List 
        clonedEntity.SupplierInvoiceItemsConDeclars = [];
        entityToClone.SupplierInvoiceItemsConDeclars.forEach(function (item) {
            var clonedItem = new SupplierInvoiceItemsConDeclarPM_1.SupplierInvoiceItemsConDeclarPM(item.EntityParentPM);
            _this.MapEntitytoEntity(item, clonedItem);
            clonedEntity.SupplierInvoiceItemsConDeclars.push(clonedItem);
        });
        // SerialNumbers List 
        clonedEntity.SupplierInvoiceItemsSerialNums = [];
        entityToClone.SupplierInvoiceItemsSerialNums.forEach(function (item) {
            var clonedItem = new SupplierInvoiceItemsSerialNumPM_1.SupplierInvoiceItemsSerialNumPM(item.EntityParentPM);
            _this.MapEntitytoEntity(item, clonedItem);
            clonedEntity.SupplierInvoiceItemsSerialNums.push(clonedItem);
        });
        // Descriptions List 
        clonedEntity.SupplierInvoiceItemsDescripts = [];
        entityToClone.SupplierInvoiceItemsDescripts.forEach(function (item) {
            var clonedItem = new SupplierInvoiceItemsDescriptPM_1.SupplierInvoiceItemsDescriptPM(item.EntityParentPM);
            _this.MapEntitytoEntity(item, clonedItem);
            clonedEntity.SupplierInvoiceItemsDescripts.push(clonedItem);
        });
        // Identifications List 
        clonedEntity.SupplierInvoiceItemsProdIdents = [];
        entityToClone.SupplierInvoiceItemsProdIdents.forEach(function (item) {
            var clonedItem = new SupplierInvoiceItemsProdIdentPM_1.SupplierInvoiceItemsProdIdentPM(item.EntityParentPM);
            _this.MapEntitytoEntity(item, clonedItem);
            clonedEntity.SupplierInvoiceItemsProdIdents.push(clonedItem);
        });
        // Levies List 
        clonedEntity.SupplierInvoiceItemLevies = [];
        entityToClone.SupplierInvoiceItemLevies.forEach(function (item) {
            var clonedItem = new SupplierInvoiceItemsLevyPM_1.SupplierInvoiceItemsLevyPM(item.EntityParentPM);
            _this.MapEntitytoEntity(item, clonedItem);
            clonedEntity.SupplierInvoiceItemLevies.push(clonedItem);
        });
        return clonedEntity;
    };
    EditSupplierInvoiceItem.prototype.RejectChanges = function () {
        this.MapEntitytoEntity(this.ClonedItemPM, this.OriginalItemPM, true);
    };
    EditSupplierInvoiceItem.prototype.MapEntitytoEntity = function (srcEntity, targetEntity, takeKeysFromTarget) {
        if (takeKeysFromTarget === void 0) { takeKeysFromTarget = false; }
        var keys;
        keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    };
    //#endregion
    EditSupplierInvoiceItem.prototype.getMax = function (list, propertyName) {
        var max = -99999;
        var maxObj = list ? list.reduce(function (prev, current) { return (prev[propertyName] > current[propertyName]) ? prev : current; }) : null;
        if (maxObj != null)
            if (max <= maxObj[propertyName])
                max = maxObj[propertyName];
        return max;
    };
    EditSupplierInvoiceItem.prototype.EditActualLines = function () {
        var _this = this;
        if (!this.IsDisplayOnly) {
            var windowArgs = {};
            windowArgs.SupplierInvoiceItemPM = this.OriginalItemPM;
            windowArgs.IsDisplayOnly = this.IsDisplayOnly;
            windowArgs.ActualInvoiceLines = this.ActualInvoiceLines;
            var windowTitle = "שורות חשבון בפועל";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 700;
            logWindow.Height = 500;
            logWindow.Title = windowTitle;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.UpdateActualInvoiceLines($event); });
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/AddEditActualLinesComponent');
        }
    };
    EditSupplierInvoiceItem.prototype.UpdateActualInvoiceLines = function (data) {
        if (data != "cancel") {
            this.ActualInvoiceLines = data;
        }
    };
    EditSupplierInvoiceItem = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditSupplierInvoiceItem.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], EditSupplierInvoiceItem);
    return EditSupplierInvoiceItem;
}(BaseComponent_1.BaseComponent));
exports.EditSupplierInvoiceItem = EditSupplierInvoiceItem;
// Details Tab
var ModificationItemModel = /** @class */ (function (_super) {
    __extends(ModificationItemModel, _super);
    function ModificationItemModel(modificationPM) {
        var _this = _super.call(this) || this;
        _this.modificationPM = modificationPM;
        _this.ModificationPM = null;
        _this.ObjectTableName = "Customs.SupplierInvoiceItemsMod";
        _this.DataContext = _this;
        _this.ModificationPM = modificationPM;
        return _this;
    }
    Object.defineProperty(ModificationItemModel.prototype, "TypeCode", {
        //#region Properties
        get: function () { return this.ModificationPM.TypeCode; },
        set: function (value) {
            if (this.ModificationPM.TypeCode != value) {
                this.ModificationPM.TypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ModificationItemModel.prototype, "TypeName", {
        get: function () { return this.ModificationPM.TypeName; },
        set: function (value) {
            if (this.ModificationPM.TypeName != value) {
                this.ModificationPM.TypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ModificationItemModel.prototype, "CurrencyTypeCode", {
        get: function () { return this.ModificationPM.CurrencyTypeCode; },
        set: function (value) {
            if (this.ModificationPM.CurrencyTypeCode != value) {
                this.ModificationPM.CurrencyTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ModificationItemModel.prototype, "CurrencyTypeName", {
        get: function () { return this.ModificationPM.CurrencyTypeName; },
        set: function (value) {
            if (this.ModificationPM.CurrencyTypeName != value) {
                this.ModificationPM.CurrencyTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ModificationItemModel.prototype, "Amount", {
        get: function () { return this.ModificationPM.Amount; },
        set: function (value) {
            if (this.ModificationPM.Amount != value) {
                this.ModificationPM.Amount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    ModificationItemModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return ModificationItemModel;
}(BaseComponent_1.BaseComponent));
exports.ModificationItemModel = ModificationItemModel;
var ProcessTypeItemModel = /** @class */ (function (_super) {
    __extends(ProcessTypeItemModel, _super);
    function ProcessTypeItemModel(processTypePM) {
        var _this = _super.call(this) || this;
        _this.processTypePM = processTypePM;
        _this.ProcessTypePM = null;
        _this.ObjectTableName = "Customs.SupplierInvoiceItemProcesType";
        _this.DataContext = _this;
        _this.ProcessTypePM = processTypePM;
        return _this;
    }
    Object.defineProperty(ProcessTypeItemModel.prototype, "ProcessTypeCode", {
        //#region Properties
        get: function () { return this.ProcessTypePM.ProcessTypeCode; },
        set: function (value) {
            if (this.ProcessTypePM.ProcessTypeCode != value) {
                this.ProcessTypePM.ProcessTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProcessTypeItemModel.prototype, "ProcessTypeName", {
        get: function () { return this.ProcessTypePM.ProcessTypeName; },
        set: function (value) {
            if (this.ProcessTypePM.ProcessTypeName != value) {
                this.ProcessTypePM.ProcessTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    ProcessTypeItemModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return ProcessTypeItemModel;
}(BaseComponent_1.BaseComponent));
exports.ProcessTypeItemModel = ProcessTypeItemModel;
// Declaration Tab
var ConDeclarItemModel = /** @class */ (function (_super) {
    __extends(ConDeclarItemModel, _super);
    function ConDeclarItemModel(connDeclar) {
        var _this = _super.call(this) || this;
        _this.connDeclar = connDeclar;
        _this.ConnDeclarPM = null;
        _this.ObjectTableName = "Customs.SupplierInvoiceItemsConDeclar";
        _this.DataContext = _this;
        _this.ConnDeclarPM = connDeclar;
        return _this;
    }
    Object.defineProperty(ConDeclarItemModel.prototype, "DeclarationTypeCode", {
        //#region Properties
        get: function () { return this.ConnDeclarPM.DeclarationTypeCode; },
        set: function (value) {
            if (this.ConnDeclarPM.DeclarationTypeCode != value) {
                this.ConnDeclarPM.DeclarationTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConDeclarItemModel.prototype, "DeclarationTypeName", {
        get: function () { return this.ConnDeclarPM.DeclarationTypeName; },
        set: function (value) {
            if (this.ConnDeclarPM.DeclarationTypeName != value) {
                this.ConnDeclarPM.DeclarationTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConDeclarItemModel.prototype, "DeclarationNumber", {
        get: function () { return this.ConnDeclarPM.DeclarationNumber; },
        set: function (value) {
            if (this.ConnDeclarPM.DeclarationNumber != value) {
                this.ConnDeclarPM.DeclarationNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConDeclarItemModel.prototype, "InvoiceNumber", {
        get: function () { return this.ConnDeclarPM.InvoiceNumber; },
        set: function (value) {
            if (this.ConnDeclarPM.InvoiceNumber != value) {
                this.ConnDeclarPM.InvoiceNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConDeclarItemModel.prototype, "ItemSequence", {
        get: function () { return this.ConnDeclarPM.ItemSequence; },
        set: function (value) {
            if (this.ConnDeclarPM.ItemSequence != value) {
                this.ConnDeclarPM.ItemSequence = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConDeclarItemModel.prototype, "Quantity", {
        get: function () { return this.ConnDeclarPM.Quantity; },
        set: function (value) {
            if (this.ConnDeclarPM.Quantity != value) {
                this.ConnDeclarPM.Quantity = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConDeclarItemModel.prototype, "QuantityTypeCode", {
        get: function () { return this.ConnDeclarPM.QuantityTypeCode; },
        set: function (value) {
            if (this.ConnDeclarPM.QuantityTypeCode != value) {
                this.ConnDeclarPM.QuantityTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConDeclarItemModel.prototype, "QuantityTypeName", {
        get: function () { return this.ConnDeclarPM.QuantityTypeName; },
        set: function (value) {
            if (this.ConnDeclarPM.QuantityTypeName != value) {
                this.ConnDeclarPM.QuantityTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    ConDeclarItemModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return ConDeclarItemModel;
}(BaseComponent_1.BaseComponent));
exports.ConDeclarItemModel = ConDeclarItemModel;
// Serial Numbers Tab
var SerialNoItemModel = /** @class */ (function (_super) {
    __extends(SerialNoItemModel, _super);
    function SerialNoItemModel(entity) {
        var _this = _super.call(this) || this;
        _this.entity = entity;
        _this.SerialNoPM = null;
        _this.ObjectTableName = "Customs.SupplierInvoiceItemsSerialNum";
        _this.DataContext = _this;
        _this.SerialNoPM = entity;
        return _this;
    }
    Object.defineProperty(SerialNoItemModel.prototype, "TypeCode", {
        //#region Properties
        get: function () { return this.SerialNoPM.TypeCode; },
        set: function (value) {
            if (this.SerialNoPM.TypeCode != value) {
                this.SerialNoPM.TypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SerialNoItemModel.prototype, "TypeName", {
        get: function () { return this.SerialNoPM.TypeName; },
        set: function (value) {
            if (this.SerialNoPM.TypeName != value) {
                this.SerialNoPM.TypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SerialNoItemModel.prototype, "SerialNumber", {
        get: function () { return this.SerialNoPM.SerialNumber; },
        set: function (value) {
            if (this.SerialNoPM.SerialNumber != value) {
                this.SerialNoPM.SerialNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    SerialNoItemModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return SerialNoItemModel;
}(BaseComponent_1.BaseComponent));
exports.SerialNoItemModel = SerialNoItemModel;
var DescribtionItemModel = /** @class */ (function (_super) {
    __extends(DescribtionItemModel, _super);
    function DescribtionItemModel(entity) {
        var _this = _super.call(this) || this;
        _this.entity = entity;
        _this.DescribtionPM = null;
        _this.ObjectTableName = "Customs.SupplierInvoiceItemsDescript";
        _this.DataContext = _this;
        _this.DescribtionPM = entity;
        return _this;
    }
    Object.defineProperty(DescribtionItemModel.prototype, "TypeCode", {
        //#region Properties
        get: function () { return this.DescribtionPM.TypeCode; },
        set: function (value) {
            if (this.DescribtionPM.TypeCode != value) {
                this.DescribtionPM.TypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DescribtionItemModel.prototype, "TypeName", {
        get: function () { return this.DescribtionPM.TypeName; },
        set: function (value) {
            if (this.DescribtionPM.TypeName != value) {
                this.DescribtionPM.TypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DescribtionItemModel.prototype, "Description", {
        get: function () { return this.DescribtionPM.Description; },
        set: function (value) {
            if (this.DescribtionPM.Description != value) {
                this.DescribtionPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    DescribtionItemModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return DescribtionItemModel;
}(BaseComponent_1.BaseComponent));
exports.DescribtionItemModel = DescribtionItemModel;
var ProdIdentItemModel = /** @class */ (function (_super) {
    __extends(ProdIdentItemModel, _super);
    function ProdIdentItemModel(entity) {
        var _this = _super.call(this) || this;
        _this.entity = entity;
        _this.ProdIdentPM = null;
        _this.ObjectTableName = "Customs.SupplierInvoiceItemsProdIdent";
        _this.DataContext = _this;
        _this.ProdIdentPM = entity;
        return _this;
    }
    Object.defineProperty(ProdIdentItemModel.prototype, "TypeCode", {
        //#region Properties
        get: function () { return this.ProdIdentPM.TypeCode; },
        set: function (value) {
            if (this.ProdIdentPM.TypeCode != value) {
                this.ProdIdentPM.TypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProdIdentItemModel.prototype, "TypeName", {
        get: function () { return this.ProdIdentPM.TypeName; },
        set: function (value) {
            if (this.ProdIdentPM.TypeName != value) {
                this.ProdIdentPM.TypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProdIdentItemModel.prototype, "Identification", {
        get: function () { return this.ProdIdentPM.Identification; },
        set: function (value) {
            if (this.ProdIdentPM.Identification != value) {
                this.ProdIdentPM.Identification = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    ProdIdentItemModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return ProdIdentItemModel;
}(BaseComponent_1.BaseComponent));
exports.ProdIdentItemModel = ProdIdentItemModel;
var LevyItemModel = /** @class */ (function (_super) {
    __extends(LevyItemModel, _super);
    function LevyItemModel(entity) {
        var _this = _super.call(this) || this;
        _this.entity = entity;
        _this.LevyPM = null;
        _this.ObjectTableName = "Customs.SupplierInvoiceItemsLevy";
        _this.DataContext = _this;
        _this.LevyPM = entity;
        return _this;
    }
    Object.defineProperty(LevyItemModel.prototype, "TradeLevyExamptCode", {
        //#region Properties
        get: function () { return this.LevyPM.TradeLevyExamptCode; },
        set: function (value) {
            if (this.LevyPM.TradeLevyExamptCode != value) {
                this.LevyPM.TradeLevyExamptCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LevyItemModel.prototype, "TradeLevyExamptName", {
        get: function () { return this.LevyPM.TradeLevyExamptName; },
        set: function (value) {
            if (this.LevyPM.TradeLevyExamptName != value) {
                this.LevyPM.TradeLevyExamptName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LevyItemModel.prototype, "TradeLevyNumber", {
        get: function () { return this.LevyPM.TradeLevyNumber; },
        set: function (value) {
            if (this.LevyPM.TradeLevyNumber != value) {
                this.LevyPM.TradeLevyNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    LevyItemModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return LevyItemModel;
}(BaseComponent_1.BaseComponent));
exports.LevyItemModel = LevyItemModel;
//# sourceMappingURL=EditSupplierInvoiceItem.js.map