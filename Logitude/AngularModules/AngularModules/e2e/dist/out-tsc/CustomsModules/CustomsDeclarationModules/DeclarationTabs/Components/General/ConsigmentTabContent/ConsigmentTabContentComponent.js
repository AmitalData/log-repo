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
var EntityArgs_1 = require("../../../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../../Infrastructure/Utilities/ObservableCollection");
var ConfirmWindow_1 = require("../../../../../../Controls/Windows/ConfirmWindow");
var DeclarationPMService_1 = require("../../../../../../Customs/Services/StandardPMs/DeclarationPMService");
var ConsignmentInternalTransitionPM_1 = require("../../../../../../Customs/EntityPMs/ConsignmentInternalTransitionPM");
var ConsignmentPackagePM_1 = require("../../../../../../Customs/EntityPMs/ConsignmentPackagePM");
var CustomsRequiredFieldListService_1 = require("../../../../../../Customs/Services/StandardLists/CustomsRequiredFieldListService");
var CustomsRequestMenuService_1 = require("../../../../../../Customs/Services/Others/CustomsRequestMenuService");
var DeliverySiteTypeListService_1 = require("../../../../../../Customs/Services/StandardLists/DeliverySiteTypeListService");
var DeclarationEventManager_1 = require("../../../../../../Customs/Utilities/DeclarationEventManager");
var ApiQueryFilters_1 = require("../../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var CouriersVatPMService_1 = require("../../../../../../Customs/Services/StandardPMs/CouriersVatPMService");
var CouriersVatExtendedPMService_1 = require("../../../../../../Customs/Services/ExtendedPMs/CouriersVatExtendedPMService");
var MessageWindow_1 = require("../../../../../../Controls/Windows/MessageWindow");
var ConsigmentTabContentComponent = /** @class */ (function (_super) {
    __extends(ConsigmentTabContentComponent, _super);
    function ConsigmentTabContentComponent(entityArgs, cd) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.cd = cd;
        _this.ObjectTableName = "Customs.Consignment";
        _this.DataContext = _this;
        _this.IsDisplayOnly = false;
        _this.ParentIsDisplayOnly = false;
        _this.ShowExcludeConsignmentBoolean = false;
        _this.IsCourierDeclaration = false;
        _this.SiteList = [];
        _this._DeliverySiteTypeListService = new DeliverySiteTypeListService_1.DeliverySiteTypeListService();
        _this._CouriersVatPMService = new CouriersVatPMService_1.CouriersVatPMService();
        _this._CouriersVatExtendedPMService = new CouriersVatExtendedPMService_1.CouriersVatExtendedPMService();
        _this._DeclarationPMService = new DeclarationPMService_1.DeclarationPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SecondCargoIDPlaceholder = " ";
        _this.ManifestNumberPlaceholder = " ";
        _this.ThirdCargoIdPlaceholder = " ";
        _this.SelectedRow = null;
        _this.ConsimentPackages = new ObservableCollection_1.ObservableCollection([]);
        // this.declarationPM = entityArgs.EntityPM;
        _this.WeightValueFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.WeightValueFilterItems.addAdditionalFilter("Code", "CC,CA,NC,PO,PP", null, null, "InListExact", false, false, false, "string", false, true);
        _this.SiteList = [];
        _this.LoadingPortFilterItems = new ApiQueryFilters_1.ApiQueryFilters(); //38388
        _this.Listen();
        return _this;
    }
    ConsigmentTabContentComponent.prototype.ngOnDestroy = function () {
        console.log("ConsigmentTabContentComponent:ngOnDestroy");
        //if (this.Tab.ComponentReference && this.Tab.ComponentReference.ngOnDestroy) {
        //    this.Tab.ComponentReference.ngOnDestroy();
        //}
        if (this.Tab) {
            this.Tab.ComponentReference = null;
            this.Tab = null;
        }
        if (this._SubDisplayModeChanged) {
            this._SubDisplayModeChanged.unsubscribe();
            this._SubDisplayModeChanged = null;
        }
        if (this._SubConsignmentsChanged) {
            this._SubConsignmentsChanged.unsubscribe();
            this._SubConsignmentsChanged = null;
        }
    };
    ConsigmentTabContentComponent.prototype.Listen = function () {
        var _this = this;
        this._SubDisplayModeChanged =
            DeclarationEventManager_1.DeclarationEventManager.DisplayModeChanged.subscribe(function (IsDisplayOnly) {
                if (_this.ShowExcludeConsignmentBoolean && _this.ExcludeConsignment)
                    _this.IsDisplayOnly = true;
                else
                    _this.IsDisplayOnly = IsDisplayOnly;
                _this.ParentIsDisplayOnly = IsDisplayOnly;
                _this.SetScreenFieldsEditability();
                _this.BuildSitesList();
                _this.SetTipsInsideCargoIdentifires(_this.EntityPM.CargoTypeCode);
            });
        this._SubConsignmentsChanged =
            DeclarationEventManager_1.DeclarationEventManager.ConsignmentsChanged.subscribe(function (e) {
                console.log("ConsignmentsChanged", _this.declarationPM, e);
                _this.SetExcludeConsignmentVisibility();
            });
    };
    ConsigmentTabContentComponent.prototype.SetExcludeConsignmentVisibility = function () {
        this.ShowExcludeConsignmentBoolean = this.declarationPM.Consignments.length == 1;
        if (this.ShowExcludeConsignmentBoolean && this.ExcludeConsignment) {
            this.IsDisplayOnly = true;
        }
    };
    ConsigmentTabContentComponent.prototype.SetTabArgs = function (args) {
        this.EntityPM = args.EntityPM;
        this.Tab = args.Tab;
        this.IsDisplayOnly = args.Disabled;
        this.ParentIsDisplayOnly = args.Disabled;
        if (this.EntityPM.CargoTypeCode == "17")
            this.LoadCouriersVat();
        if (!this.declarationPM)
            this.declarationPM = args.Parent;
        this.IsCourierDeclaration = this.declarationPM.IsCourierDeclaration;
        this.ShowExcludeConsignmentBoolean = this.declarationPM.Consignments.length == 1;
        this.SetExcludeConsignmentVisibility();
        this.SetTipsInsideCargoIdentifires(this.EntityPM.CargoTypeCode);
        //this.EntityPM.PropertyChanged.subscribe((event) => { console.log("PropertyChanged: ", event); });
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsignmentPackages)) {
            for (var _i = 0, _a = this.EntityPM.ConsignmentPackages; _i < _a.length; _i++) {
                var pkg = _a[_i];
                var item = new ConsigmentPackageModel(pkg);
                this.ConsimentPackages.Insert(item);
            }
        }
        if (this.IsDisplayOnly) {
            this.SetScreenFieldsEditability();
        }
        //**
        //this.SetDateVisibilty(); // this make entity dirty on tab loaded, the following should solve it
        if (this.CargoTypeCode == "17") {
            this.UIProperties.SetVisibility("ThirdCargoID", this.ObjectTableName, false);
            this.UIProperties.SetVisibility("CargoDate", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetVisibility("ThirdCargoID", this.ObjectTableName, true);
            this.UIProperties.SetVisibility("CargoDate", this.ObjectTableName, false);
        }
        //**
        // show xml errors
        if (!Tools_1.AppTool.IsNullOrEmpty(args.DecErrors)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(args.DecErrors.Field)) {
                this.UIProperties.SetValidity(args.DecErrors.Field, "Customs.Consignment", false, args.DecErrors.Description);
            }
        }
        this.BuildSitesList();
        this.InitLOVFilters(); //38388
        this.CheckRequrierdFieldsForSend();
        console.log("Tabs Args: ", args);
    };
    ConsigmentTabContentComponent.prototype.SetDateVisibilty = function () {
        if (this.CargoTypeCode == "17") {
            this.ThirdCargoID = null;
            this.UIProperties.SetVisibility("ThirdCargoID", this.ObjectTableName, false);
            this.UIProperties.SetVisibility("CargoDate", this.ObjectTableName, true);
        }
        else {
            this.CargoDate = null;
            this.UIProperties.SetVisibility("ThirdCargoID", this.ObjectTableName, true);
            this.UIProperties.SetVisibility("CargoDate", this.ObjectTableName, false);
        }
    };
    ConsigmentTabContentComponent.prototype.SetScreenFieldsEditability = function () {
        console.log("SetScreenFieldsEditability: " + this.EntityPM);
        this.UIProperties.SetEnabled("CargoDescription", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("UnloadDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("OriginCountryCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ReceiverWarehouseCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("UnloadPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IsLastReleaseFromWarehous", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("StorageSiteCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("LoadingPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CargoDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ManifestDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeliveryPlaceName", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("WeightValue", this.ObjectTableName, !this.IsDisplayOnly);
    };
    ConsigmentTabContentComponent.prototype.LoadCouriersVat = function () {
        var _this = this;
        var vatNumber = this.SecondCargoID;
        this._CouriersVatExtendedPMService.GetSingleCouriersVatByCode(vatNumber).subscribe(function (response) {
            console.log("[response] GetSingleCouriersVatByCode: ", response);
            var result = response.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                _this.CouriersVatId = result.Id;
                _this.CouriersVat = result;
            }
        });
    };
    ConsigmentTabContentComponent.prototype.InitLOVFilters = function () {
        // initialize query filters for LoadingPort according to CountryCode
        if (!Tools_1.AppTool.IsNullOrEmpty(this.OriginCountryCode)) {
            this.LoadingPortFilterItems.removeAdditionalFilter("CountryTypeCode");
            this.LoadingPortFilterItems.addAdditionalFilter("CountryTypeCode", this.OriginCountryCode, null, null, "Equals", false, false, false, "string", false, true);
            //this.LoadingPortFilterItems.ForceCacheRefresh = true;
        }
        else {
            this.LoadingPortFilterItems.removeAdditionalFilter("CountryTypeCode");
        }
    };
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "CouriersVatId", {
        get: function () { return this.couriersVatId; },
        set: function (newValue) { this.couriersVatId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "CouriersVat", {
        get: function () { return this.couriersVat; },
        set: function (newValue) {
            this.couriersVat = newValue;
            if (this.couriersVat) {
                this.SecondCargoID = this.couriersVat.VatNumber;
            }
        },
        enumerable: true,
        configurable: true
    });
    ConsigmentTabContentComponent.prototype.CouriersVatTextChanged = function (text) {
        this.SecondCargoID = text;
    };
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "CargoTypeCode", {
        get: function () { return this.EntityPM ? this.EntityPM.CargoTypeCode : null; },
        set: function (newValue) {
            this.EntityPM.CargoTypeCode = newValue;
            this.SetDateVisibilty();
            this.SetTipsInsideCargoIdentifires(newValue);
            if (newValue == "17")
                this.LoadCouriersVat();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "CargoDescription", {
        get: function () { return this.EntityPM ? this.EntityPM.CargoDescription : null; },
        set: function (newValue) { this.EntityPM.CargoDescription = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "ThirdCargoID", {
        get: function () { return this.EntityPM ? this.EntityPM.ThirdCargoID : null; },
        set: function (newValue) { this.EntityPM.ThirdCargoID = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "UnloadDate", {
        get: function () { return this.EntityPM ? this.EntityPM.UnloadDate : null; },
        set: function (newValue) { this.EntityPM.UnloadDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "ManifestDate", {
        get: function () { return this.EntityPM ? this.EntityPM.ManifestDate : null; },
        set: function (newValue) { this.EntityPM.ManifestDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "OriginCountryCode", {
        get: function () { return this.EntityPM ? this.EntityPM.OriginCountryCode : null; },
        set: function (newValue) {
            this.EntityPM.OriginCountryCode = newValue;
            this.InitLOVFilters();
        } //38388
        ,
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "SecondCargoID", {
        get: function () { return this.EntityPM ? this.EntityPM.SecondCargoID : null; },
        set: function (newValue) {
            this.EntityPM.SecondCargoID = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "ReceiverWarehouseCode", {
        get: function () { return this.EntityPM ? this.EntityPM.ReceiverWarehouseCode : null; },
        set: function (newValue) { this.EntityPM.ReceiverWarehouseCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "DeliveryPlaceName", {
        get: function () { return this.EntityPM ? this.EntityPM.DeliveryPlaceName : null; },
        set: function (newValue) { this.EntityPM.DeliveryPlaceName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "UnloadPortCode", {
        get: function () { return this.EntityPM ? this.EntityPM.UnloadPortCode : null; },
        set: function (newValue) {
            var _this = this;
            this.EntityPM.UnloadPortCode = newValue;
            if (this.declarationPM.TransportModeId == 'A') {
                this._DeliverySiteTypeListService.getSingle(newValue).subscribe(function (response) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(response)) {
                        var entity = response.Result;
                        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) { //Exisit
                            _this.EntityPM.StorageSiteCode = newValue;
                            console.log("The StorageSiteCode is set to ", newValue);
                        }
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "ManifestNumber", {
        get: function () { return this.EntityPM ? this.EntityPM.ManifestNumber : null; },
        set: function (newValue) {
            this.EntityPM.ManifestNumber = newValue;
            this.Tab.Header = (newValue ? (newValue + '-') : '') + this.EntityPM.SequenceNumeric;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "IsLastReleaseFromWarehous", {
        get: function () { return this.EntityPM ? this.EntityPM.IsLastReleaseFromWarehous : null; },
        set: function (newValue) { this.EntityPM.IsLastReleaseFromWarehous = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "StorageSiteCode", {
        get: function () { return this.EntityPM ? this.EntityPM.StorageSiteCode : null; },
        set: function (newValue) { this.EntityPM.StorageSiteCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "LoadingPortCode", {
        get: function () { return this.EntityPM ? this.EntityPM.LoadingPortCode : null; },
        set: function (newValue) { this.EntityPM.LoadingPortCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "CargoDate", {
        get: function () { return this.EntityPM ? this.EntityPM.CargoDate : null; },
        set: function (newValue) {
            this.EntityPM.CargoDate = newValue;
            if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                var day = newValue.getDate() + "";
                var month = (newValue.getMonth() + 1) + "";
                //var year = (newValue.getFullYear()) + ""; 
                var year = newValue.getFullYear().toString().substring(2, 4);
                var id = this.ApplyPadding(day) + this.ApplyPadding(month) + year;
                this.ThirdCargoID = id;
                console.log("Date::: ", id);
            }
            else {
                this.ThirdCargoID = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "ExcludeConsignment", {
        get: function () { return this.declarationPM.ExcludeConsignment; },
        set: function (newValue) {
            this.declarationPM.ExcludeConsignment = newValue;
            if (this.ParentIsDisplayOnly) {
                this.IsDisplayOnly = true;
            }
            else {
                this.IsDisplayOnly = newValue;
            }
            this.SetScreenFieldsEditability();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "WeightValue", {
        get: function () { return this.declarationPM.WeightValue; },
        set: function (newValue) { this.declarationPM.WeightValue = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentTabContentComponent.prototype, "AddSiteEnabled", {
        get: function () { return this.EntityPM ? this.addSiteEnabled : null; },
        set: function (newValue) {
            this.addSiteEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    ConsigmentTabContentComponent.prototype.SetTipsInsideCargoIdentifires = function (value) {
        switch (value) {
            case '1':
                {
                    this.ManifestNumberPlaceholder = "הזן שנת טיסה";
                    this.SecondCargoIDPlaceholder = "הזן שט”מ ראשי";
                    this.ThirdCargoIdPlaceholder = "הזן שט”מ פנימי";
                    break;
                }
            case '2':
                {
                    this.ManifestNumberPlaceholder = "הזן מספר חבילה";
                    this.SecondCargoIDPlaceholder = "הזן שנת יצירת מטען";
                    this.ThirdCargoIdPlaceholder = " ";
                    break;
                }
            case '8':
                {
                    this.ManifestNumberPlaceholder = "הזן הצהרת אחסנה";
                    this.SecondCargoIDPlaceholder = " ";
                    this.ThirdCargoIdPlaceholder = " ";
                    break;
                }
            case '11':
                {
                    this.ManifestNumberPlaceholder = "הזן מצהר";
                    this.SecondCargoIDPlaceholder = " הזן מזהה עסקה";
                    this.ThirdCargoIdPlaceholder = " ";
                    break;
                }
            case '17':
                {
                    this.ManifestNumberPlaceholder = "הזן ש.מ בלדר";
                    this.SecondCargoIDPlaceholder = "הזן ח.פ בלדר";
                    this.ThirdCargoIdPlaceholder = "הזן תאריך הקמה";
                    break;
                }
            case '20':
                {
                    this.ManifestNumberPlaceholder = "הזן מזהה עסקה מלא";
                    this.SecondCargoIDPlaceholder = " ";
                    this.ThirdCargoIdPlaceholder = " ";
                    break;
                }
            default:
                {
                    this.ManifestNumberPlaceholder = " ";
                    this.SecondCargoIDPlaceholder = " ";
                    this.ThirdCargoIdPlaceholder = " ";
                    break;
                }
        }
    };
    ConsigmentTabContentComponent.prototype.AddPackageButtonClicked = function () {
        var line = new ConsignmentPackagePM_1.ConsignmentPackagePM(this.EntityPM);
        this.EntityPM.AddConsignmentPackage(line);
        var item = new ConsigmentPackageModel(line);
        this.ConsimentPackages.Insert(item);
        //this.CurrentSession.ResetRowIndex();
    };
    ConsigmentTabContentComponent.prototype.RemovePackageButton = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeletePackage");
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Height = 150;
            confirmWindow.Show(msg);
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) { // YES
                    _this.ConsimentPackages.Remove(item);
                    _this.EntityPM.RemoveConsignmentPackage(item.EntityPM);
                }
            });
        }
    };
    ConsigmentTabContentComponent.prototype.OnSecondCargoLostFocus = function () {
        if (this.CargoTypeCode == "11") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SecondCargoID)) {
                if (this.SecondCargoID.length == 9) {
                    if (this.SecondCargoID.charAt(0) != 'I') {
                        this.SecondCargoID = 'I' + this.SecondCargoID;
                    }
                }
            }
        }
    };
    ConsigmentTabContentComponent.prototype.OnRowEnded = function ($event) {
        console.log("this.ConsimentPackages.Length : " + this.ConsimentPackages.Length);
        if (($event) == this.ConsimentPackages.Length) {
            this.AddPackageButtonClicked();
        }
    };
    ConsigmentTabContentComponent.prototype.MasterBOLRequestMethod = function () {
        var _this = this;
        if (this.IsDisplayOnly) {
            return;
        }
        var customsRequestMenuService = new CustomsRequestMenuService_1.CustomsRequestMenuService();
        var my = {
            "CustomFileNo": this.declarationPM.CustomFileNo,
            "Date": this.EntityPM.ManifestNumber,
            "MasterBillOfLading": this.EntityPM.SecondCargoID,
            "InternalIdentifier": this.EntityPM.ThirdCargoID,
            //"ReturnAllInernalCargos": this.EntityPM.ThirdCargoID ? false : true,//task 44705 21.11.18
            "ReturnAllInernalCargos": true,
            "DeclarationId": this.EntityPM.DeclarationId,
        };
        customsRequestMenuService.WindowClosed.subscribe(function ($event) { return _this.OnMasterBOLRequestWindowClosed($event); });
        customsRequestMenuService.ShowModalAsEditMenuAction("9020", my);
    };
    ConsigmentTabContentComponent.prototype.OnMasterBOLRequestWindowClosed = function (arg) {
        if (!Tools_1.AppTool.IsNullOrEmpty(arg)) {
            this.ThirdCargoID = arg.CargoIdentifierKey3;
            if (this.ConsimentPackages != null && this.ConsimentPackages.Length > 0) {
                this.ConsimentPackages.Collection.forEach(function (item) {
                    if (item.PackageMeasureQualifierCode == "2") {
                        item.PackageQuantity = arg.PacakgesQuantity;
                        item.GrossMassMeasure = arg.TotalWheight;
                        return;
                    }
                });
            }
            else {
                var line = new ConsignmentPackagePM_1.ConsignmentPackagePM(this.EntityPM);
                this.EntityPM.AddConsignmentPackage(line);
                var item = new ConsigmentPackageModel(line);
                item.PackageMeasureQualifierCode = "2";
                item.PackageQuantity = arg.PacakgesQuantity;
                item.GrossMassMeasure = arg.TotalWheight;
                this.ConsimentPackages.Insert(item);
            }
        }
    };
    ConsigmentTabContentComponent.prototype.CourierBOLRequestMethod = function () {
        var _this = this;
        if (this.IsDisplayOnly) {
            return;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SecondCargoID)) {
            var secondCargoID = this.SecondCargoID;
            if (this.SecondCargoID.length != 9 || isNaN(secondCargoID) || this.SecondCargoID.indexOf('e') >= 0) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
                messageWindow.Width = 250;
                messageWindow.Height = 150;
                messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                messageWindow.Show("יש להזין מספר בעל 9 ספרות בלבד בשדה מזהה מטען שני");
                return;
            }
        }
        var customsRequestMenuService = new CustomsRequestMenuService_1.CustomsRequestMenuService();
        var my = {
            "CustomFileNo": this.declarationPM.CustomFileNo,
            "CourierBOL": this.EntityPM.ManifestNumber,
            "CourierVAT": this.EntityPM.SecondCargoID,
            "DeclarationId": this.EntityPM.DeclarationId,
        };
        customsRequestMenuService.WindowClosed.subscribe(function (arg) {
            if (!Tools_1.AppTool.IsNullOrEmpty(arg)) {
                _this.GetDateFromString(arg);
            }
        });
        customsRequestMenuService.ShowModalAsEditMenuAction("9022", my);
    };
    ConsigmentTabContentComponent.prototype.GetDateFromString = function (cargoDate) {
        if (Tools_1.AppTool.IsNullOrEmpty(cargoDate)) {
            return "";
        }
        var day = Number(cargoDate.substring(0, 2));
        var month = Number(cargoDate.substring(2, 4));
        var year = Number("20" + cargoDate.substring(4, 6));
        this.CargoDate = Tools_1.DateTool.GetDate(year, month - 1, day, 0, 0, 0);
    };
    ConsigmentTabContentComponent.prototype.CargoQueryRequestMethod = function () {
        if (this.IsDisplayOnly) {
            return;
        }
        if (this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty) {
            //Save changes
            this.SaveChangesAndSendRequest();
        }
        else {
            this.SendCargoQueryRequestMethod();
        }
    };
    ConsigmentTabContentComponent.prototype.SendCargoQueryRequestMethod = function () {
        var _this = this;
        var customsRequestMenuService = new CustomsRequestMenuService_1.CustomsRequestMenuService();
        var my = {
            "Mode": "SendCargoQueryRequestFromDeclaration",
            "CargoTypeCode": this.EntityPM.CargoTypeCode,
            "ManifestNumber": this.EntityPM.ManifestNumber,
            "SecondCargoID": this.EntityPM.SecondCargoID,
            "DeclarationId": this.EntityPM.DeclarationId,
        };
        customsRequestMenuService.WindowClosed.subscribe(function (arg) {
            if (!Tools_1.AppTool.IsNullOrEmpty(arg) && arg == "ReloadEntity") {
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
        });
        customsRequestMenuService.ShowModalAsEditMenuAction("8240", my);
    };
    ConsigmentTabContentComponent.prototype.SaveChangesAndSendRequest = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var sub = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (myResult) {
            sub.unsubscribe();
            var res = myResult;
            if (!res.HasError) {
                var entity = res.Result;
                console.log("..Saved Successfully ", entity);
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                _this.SendCargoQueryRequestMethod();
            }
            else {
                //this.ValidationErrorsList = res.ErrorsArray;
            }
            _this.CurrentSession.StopBusyIndicator();
        });
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    };
    ConsigmentTabContentComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    ConsigmentTabContentComponent.prototype.OnFocus = function () {
        if (this.ConsimentPackages.Length == 0) {
            this.AddPackageButtonClicked();
        }
    };
    ConsigmentTabContentComponent.prototype.AddSiteButtonClicked = function () {
        if (this.AddSiteEnabled && !this.IsDisplayOnly) {
            var consignmentNumber = 0;
            consignmentNumber = this.EntityPM.ConsignmentNumber;
            var lineNumber = 1;
            if (this.EntityPM.ConsignmentInternalTransitions.length != 0) {
                var maxObj = this.EntityPM.ConsignmentInternalTransitions.reduce(function (prev, current) { return (prev.LineNumber > current.LineNumber) ? prev : current; });
                if (maxObj != null) {
                    if (lineNumber <= maxObj.LineNumber)
                        lineNumber = maxObj.LineNumber;
                }
                lineNumber = lineNumber + 1;
            }
            var item = new ConsignmentInternalTransitionPM_1.ConsignmentInternalTransitionPM(this.EntityPM);
            item.ConsignmentNumber = consignmentNumber;
            item.DeclarationId = this.EntityPM.DeclarationId;
            item.Tenant = this.EntityPM.Tenant;
            item.LineNumber = lineNumber;
            if (!this.EntityPM.ConsignmentInternalTransitions.includes(item)) {
                //this.EntityPM.ConsignmentInternalTransitions.push(item);
                this.EntityPM.AddConsignmentInternalTransition(item);
            }
            this.AddSiteEnabled = false;
            this.BuildSitesList();
        }
    };
    ConsigmentTabContentComponent.prototype.BuildSitesList = function () {
        var count = 1;
        this.SiteList = [];
        this.AddSiteEnabled = true;
        for (var i = 0; i < this.EntityPM.ConsignmentInternalTransitions.length; i++) {
            var viewModel = new ConsignmentInternalTransitionModel(this.EntityPM.ConsignmentInternalTransitions[i], this);
            viewModel.TransitionNumber = i + 1;
            if (viewModel.SiteCode == null) {
                this.AddSiteEnabled = false;
            }
            this.SiteList.push(viewModel);
        }
        if (this.SiteList.length == 0) {
            var item = new ConsignmentInternalTransitionPM_1.ConsignmentInternalTransitionPM(this.EntityPM);
            var viewModel = new ConsignmentInternalTransitionModel(item, this);
            viewModel.TransitionNumber;
            this.SiteList.push(viewModel);
            //  this.EntityPM.AddConsignmentInternalTransition(item);
            this.AddSiteEnabled = false;
        }
    };
    ConsigmentTabContentComponent.prototype.ApplyPadding = function (str) {
        var pad = "00";
        var ans = pad.substring(0, pad.length - str.length) + str;
        return ans;
    };
    ConsigmentTabContentComponent.prototype.CheckRequrierdFieldsForSend = function () {
        var _this = this;
        var customsRequiredFieldListService = new CustomsRequiredFieldListService_1.CustomsRequiredFieldListService();
        var table = window.ObjectTables.filter(function (d) { return d.Name == 'Customs.Consignment'; })[0];
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.addAdditionalFilter("ObjectTableId", table.Id, null, null, "Equals", false, false, false, "string");
        customsRequiredFieldListService.getAllFromCache(filters).subscribe(function (response) {
            var requiredFields = response.Result;
            requiredFields.forEach(function (field) {
                var objectField = window.ObjectFields.filter(function (d) { return d.Id == field.ObjectfieldId; })[0];
                _this.UIProperties.SetWarning(objectField.FieldName, 'Customs.Consignment', true);
            });
        });
    };
    ConsigmentTabContentComponent = __decorate([
        core_1.Component({
            selector: 'ConsigmentTabContent',
            moduleId: module.id,
            templateUrl: './ConsigmentTabContentComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], ConsigmentTabContentComponent);
    return ConsigmentTabContentComponent;
}(BaseComponent_1.BaseComponent));
exports.ConsigmentTabContentComponent = ConsigmentTabContentComponent;
var ConsigmentPackageModel = /** @class */ (function (_super) {
    __extends(ConsigmentPackageModel, _super);
    function ConsigmentPackageModel(line) {
        var _this = _super.call(this) || this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = line;
        return _this;
    }
    Object.defineProperty(ConsigmentPackageModel.prototype, "PackageMeasureQualifierCode", {
        //#region Properties
        get: function () { return this.EntityPM.PackageMeasureQualifierCode; },
        set: function (newValue) { this.EntityPM.PackageMeasureQualifierCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentPackageModel.prototype, "PackageMeasureQualifierName", {
        get: function () { return this.EntityPM.PackageMeasureQualifierName; },
        set: function (newValue) { this.EntityPM.PackageMeasureQualifierName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentPackageModel.prototype, "PackageTypeName", {
        get: function () { return this.EntityPM.PackageTypeName; },
        set: function (newValue) { this.EntityPM.PackageTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentPackageModel.prototype, "PackageTypeCode", {
        get: function () { return this.EntityPM.PackageTypeCode; },
        set: function (newValue) { this.EntityPM.PackageTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentPackageModel.prototype, "MarksNumbers", {
        get: function () { return this.EntityPM.MarksNumbers; },
        set: function (newValue) { this.EntityPM.MarksNumbers = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentPackageModel.prototype, "GrossMassMeasure", {
        get: function () { return this.EntityPM.GrossMassMeasure; },
        set: function (newValue) { this.EntityPM.GrossMassMeasure = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsigmentPackageModel.prototype, "PackageQuantity", {
        get: function () { return this.EntityPM.PackageQuantity; },
        set: function (newValue) { this.EntityPM.PackageQuantity = newValue; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    ConsigmentPackageModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return ConsigmentPackageModel;
}(BaseComponent_1.BaseComponent));
exports.ConsigmentPackageModel = ConsigmentPackageModel;
var ConsignmentInternalTransitionModel = /** @class */ (function (_super) {
    __extends(ConsignmentInternalTransitionModel, _super);
    function ConsignmentInternalTransitionModel(item, parent) {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customs.ConsignmentInternalTransition";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#region Properties
        _this.deleteSiteVisible = false;
        _this.transitionNumber = 1;
        // close button
        _this.overCloseButton = false;
        _this.EntityPM = item;
        _this.Parent = parent;
        _this.UIProperties.SetEnabled("SiteCode", _this.ObjectTableName, !_this.Parent.IsDisplayOnly);
        return _this;
    }
    Object.defineProperty(ConsignmentInternalTransitionModel.prototype, "DeleteSiteVisible", {
        get: function () { return this.deleteSiteVisible; },
        set: function (newValue) { this.deleteSiteVisible = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsignmentInternalTransitionModel.prototype, "TransitionNumber", {
        get: function () { return this.transitionNumber; },
        set: function (newValue) { this.transitionNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsignmentInternalTransitionModel.prototype, "LineNumber", {
        get: function () { return this.EntityPM.LineNumber; },
        set: function (newValue) { this.EntityPM.LineNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConsignmentInternalTransitionModel.prototype, "SiteCode", {
        get: function () { return this.EntityPM.SiteCode; },
        set: function (newValue) {
            this.EntityPM.SiteCode = newValue;
            if (newValue != null) {
                if (this.Parent.SiteList.length == 1) {
                    this.Parent.EntityPM.AddConsignmentInternalTransition(this.EntityPM);
                }
                this.Parent.AddSiteEnabled = true;
            }
            else {
                this.Parent.AddSiteEnabled = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    ConsignmentInternalTransitionModel.prototype.OnMouseOver = function () {
        if (this.EntityPM.LineNumber > 1) {
            this.DeleteSiteVisible = true;
        }
    };
    ConsignmentInternalTransitionModel.prototype.OnMouseLeave = function () {
        if (!this.overCloseButton) {
            this.DeleteSiteVisible = false;
        }
    };
    ConsignmentInternalTransitionModel.prototype.OnIconButtonMouseOver = function () {
        this.overCloseButton = true;
    };
    ConsignmentInternalTransitionModel.prototype.OnIconButtonMouseLeave = function () {
        this.overCloseButton = false;
    };
    ConsignmentInternalTransitionModel.prototype.DeleteSiteButtonClicked = function () {
        var _this = this;
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeleteSite");
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Height = 150;
        confirmWindow.Show(msg);
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) { // YES
                _this.Parent.EntityPM.RemoveConsignmentInternalTransition(_this.EntityPM);
                _this.Parent.BuildSitesList();
            }
        });
    };
    return ConsignmentInternalTransitionModel;
}(BaseComponent_1.BaseComponent));
exports.ConsignmentInternalTransitionModel = ConsignmentInternalTransitionModel;
//# sourceMappingURL=ConsigmentTabContentComponent.js.map