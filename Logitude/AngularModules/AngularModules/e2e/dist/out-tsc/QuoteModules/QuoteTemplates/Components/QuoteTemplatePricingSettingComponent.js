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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var QuoteTemplateSettingPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService");
var QuoteTemplateTableDesignPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateTableDesignPMService");
var QuoteTemplateTextDesignPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateTextDesignPMService");
var QuoteTemplateTextDesignExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateTextDesignExtendedPMService");
var QuoteTemplateTextCodeExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateTextCodeExtendedPMService");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var Tools_1 = require("../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var QuoteTemplatePricingSettingComponent = /** @class */ (function (_super) {
    __extends(QuoteTemplatePricingSettingComponent, _super);
    function QuoteTemplatePricingSettingComponent() {
        var _this = _super.call(this) || this;
        _this.Alignment = [];
        _this.QuoteTemplateTextDesignPMLists = [];
        _this.QuoteTemplateTextCodePMList = [];
        _this.AllQuoteTemplateTextCodePMList = [];
        _this.IsSaveQuoteTemplateTextDesignRuning = false;
        _this.IsSaveQuoteTemplateTableDesignRuning = false;
        _this.IsSaveQuoteTemplateTextCodeRuning = false;
        _this.QuoteTemplateSectionTypeName = "Packages";
        _this.IsPerContainerChange = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //  LoadHeaderTextDesign(headerDesignId:string) {
        //      this.quoteTemplateTextDesignPMService.get(headerDesignId).subscribe(res => {
        //          var pmResponse: ServiceResponse = res;
        //          this.IsLoadHeaderTextDesignRuning = false;
        //          this.LoadCompleted();
        //          if (!pmResponse.HasError && pmResponse.Result) {
        //              this.HeaderTextDesignPM = pmResponse.Result;
        //              this.HeaderTextDesignPM.Title = "Header";
        //              this.QuoteTemplateTextDesignPMLists.push(this.HeaderTextDesignPM);
        //          }
        //      });
        //  }
        //  LoadRowTextDesign(linesDesignId: string) {
        //      this.quoteTemplateTextDesignPMService.get(linesDesignId).subscribe(res => {
        //          var pmResponse: ServiceResponse = res;
        //          this.IsLoadRowTextDesignRuning = false;
        //          this.LoadCompleted();
        //          if (!pmResponse.HasError && pmResponse.Result) {
        //              this.RowTextDesignPM = pmResponse.Result;
        //              this.RowTextDesignPM.Title = "Rows";
        //              this.QuoteTemplateTextDesignPMLists.push(this.RowTextDesignPM);
        //          }
        //      });
        //  }
        // //Load Totals Text Design
        //    LoadTotalsLabelTextDesign() {
        //      var totalsLabelTextDesignId: string = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.TotalsPackagesLabelDesignId : this.QuoteTemplateSettingPM.TotalsContainsersLabelDesignId;
        //      this.quoteTemplateTextDesignPMService.get(totalsLabelTextDesignId).subscribe(res => {
        //          var pmResponse: ServiceResponse = res;
        //          this.IsLoadTotalLabelTextDesignRuning = false;
        //          this.LoadCompleted();
        //          if (!pmResponse.HasError && pmResponse.Result) {
        //              this.TotalLabelTextDesignPM = pmResponse.Result;
        //              this.TotalLabelTextDesignPM.Title = "Label";
        //              this.QuoteTemplateTextDesignPMLists.push(this.TotalLabelTextDesignPM);
        //          }
        //      });
        //  }
        //    LoadTotalsValueTextDesign() {
        //       var totalsValueTextDesignId: string = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.TotalsPackagesValueDesignId : this.QuoteTemplateSettingPM.TotalsContainsersValueDesignId;
        //        this.quoteTemplateTextDesignPMService.get(totalsValueTextDesignId).subscribe(res => {
        //            var pmResponse: ServiceResponse = res;
        //            this.IsLoadTotalValueTextDesignRuning = false;
        //            this.LoadCompleted();
        //            if (!pmResponse.HasError && pmResponse.Result) {
        //                this.TotalValueTextDesignPM = pmResponse.Result;
        //                this.TotalValueTextDesignPM.Title = "Value";
        //                this.QuoteTemplateTextDesignPMLists.push(this.TotalValueTextDesignPM);
        //            }
        //        });
        //    }
        // //GroupBy Text Design
        //    LoadGroupByTextDesign() {
        //        var groupByTextDesignId: string = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.GroupByPackagesValueDesignId : this.QuoteTemplateSettingPM.GroupByContainsersValueDesignId;
        //        this.quoteTemplateTextDesignPMService.get(groupByTextDesignId).subscribe(res => {
        //            var pmResponse: ServiceResponse = res;
        //            this.IsLoadGroupByTextDesignRuning = false;
        //            this.LoadCompleted();
        //            if (!pmResponse.HasError && pmResponse.Result) {
        //                this.GroupByTextDesignPM = pmResponse.Result;
        //                this.GroupByTextDesignPM.Title = "Group By Design";
        //                this.QuoteTemplateTextDesignPMLists.push(this.GroupByTextDesignPM);
        //            }
        //        });
        //    }
        ////Title Text Design
        //    LoadTitleTextDesign() {
        //        var titleTextDesignId: string = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.PricingPackagesTitleDesignId : this.QuoteTemplateSettingPM.PricingContainsersTitleDesignId;
        //        this.quoteTemplateTextDesignPMService.get(titleTextDesignId).subscribe(res => {
        //            var pmResponse: ServiceResponse = res;
        //            this.IsLoadTitleTextDesignRuning = false;
        //            this.LoadCompleted();
        //            if (!pmResponse.HasError && pmResponse.Result) {
        //                this.TitleTextDesignPM = pmResponse.Result;
        //                this.TitleTextDesignPM.Title = "Title";
        //                this.QuoteTemplateTextDesignPMLists.push(this.TitleTextDesignPM);
        //            }
        //        });
        //    }
        //Prop setting 
        _this.ShowTotalInLocalCurrencyKey = Guid_1.Guid.newGuid();
        _this.ShowTotalInSaleCurrencyKey = Guid_1.Guid.newGuid();
        _this.ShowTitlePricingKey = Guid_1.Guid.newGuid();
        _this.ShowPricesTableKey = Guid_1.Guid.newGuid();
        _this.SplitChargesbyGroupsKey = Guid_1.Guid.newGuid();
        _this.ShowChargeCodeKey = Guid_1.Guid.newGuid();
        _this.ShowChargeNameKey = Guid_1.Guid.newGuid();
        _this.ShowMeasurementKey = Guid_1.Guid.newGuid();
        _this.ShowPrice1Key = Guid_1.Guid.newGuid();
        _this.ShowPrice2Key = Guid_1.Guid.newGuid();
        _this.ShowSaleCurrencyColumnKey = Guid_1.Guid.newGuid();
        _this.ShowLocalCurrencyColumnKey = Guid_1.Guid.newGuid();
        _this.ShowChargeDescriptionKey = Guid_1.Guid.newGuid();
        _this.ShowTotalPerChargeGroupKey = Guid_1.Guid.newGuid();
        _this.ShowChargeNoteColumnKey = Guid_1.Guid.newGuid();
        _this.ShowSaleMaxMinAmountColumnKey = Guid_1.Guid.newGuid();
        _this.ShowHeaderLabelsKey = Guid_1.Guid.newGuid();
        _this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService_1.QuoteTemplateSettingPMService();
        _this.quoteTemplateTableDesignPMService = new QuoteTemplateTableDesignPMService_1.QuoteTemplateTableDesignPMService();
        _this.quoteTemplateTextDesignPMService = new QuoteTemplateTextDesignPMService_1.QuoteTemplateTextDesignPMService();
        _this.quoteTemplateTextDesignExtendedPMService = new QuoteTemplateTextDesignExtendedPMService_1.QuoteTemplateTextDesignExtendedPMService();
        _this.quoteTemplateTextCodeExtendedPMService = new QuoteTemplateTextCodeExtendedPMService_1.QuoteTemplateTextCodeExtendedPMService();
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    QuoteTemplatePricingSettingComponent.prototype.ngOnInit = function () {
    };
    QuoteTemplatePricingSettingComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.SelectedTabCode = "PRT";
        this.QuoteTemplatePM = args.QuoteTemplatePM;
        this.QuoteTemplateSectionTypeName = args.QuoteTemplateSectionTypeName;
        this.QuoteTemplateSettingPM = args.QuoteTemplateSettingPM;
        this.Alignment.push("Left");
        this.Alignment.push("Center");
        this.Alignment.push("Right");
        if (args.QuoteTemplateTextCodePMList) {
            this.QuoteTemplateTextCodePMList = args.QuoteTemplateTextCodePMList.filter(function (d) { return d.Area == _this.QuoteTemplateSectionTypeName; });
            this.AllQuoteTemplateTextCodePMList = args.QuoteTemplateTextCodePMList;
            this.BuildItemsSource();
        }
        this.LoadData();
    };
    QuoteTemplatePricingSettingComponent.prototype.BuildItemsSource = function () {
        var itemsCollection = [];
        this.QuoteTemplateTextCodePMList.forEach(function (item) {
            itemsCollection.push(new TextCodeData(item));
        });
        this.ItemsSource.AppendCollection(itemsCollection);
    };
    QuoteTemplatePricingSettingComponent.prototype.LoadData = function () {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        this.QuoteTemplateTextDesignPMLists = [];
        this.LoadTableDesign();
    };
    QuoteTemplatePricingSettingComponent.prototype.LoadTextDesign = function () {
        var _this = this;
        var totalsLabelTextDesignId = (this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.TotalsPackagesLabelDesignId : this.QuoteTemplateSettingPM.TotalsContainsersLabelDesignId);
        var totalValueTextDesignId = (this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.TotalsPackagesValueDesignId : this.QuoteTemplateSettingPM.TotalsContainsersValueDesignId);
        var titleTextDesignId = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.PricingPackagesTitleDesignId : this.QuoteTemplateSettingPM.PricingContainsersTitleDesignId;
        var groupByHeaderTextDesignId = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.GroupByPackagesValueDesignId : this.QuoteTemplateSettingPM.GroupByContainsersValueDesignId;
        var groupByTotailTextDesignId = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.GroupByPackagesLabelDesignId : this.QuoteTemplateSettingPM.GroupByContainsersLabelDesignId;
        var ids = totalsLabelTextDesignId;
        ids += ("," + totalValueTextDesignId);
        ids += ("," + groupByHeaderTextDesignId);
        ids += ("," + titleTextDesignId);
        ids += ("," + groupByTotailTextDesignId);
        if (this.TableDesignPM) {
            ids += ("," + this.TableDesignPM.HeaderDesignId);
            ids += ("," + this.TableDesignPM.LinesDesignId);
        }
        this.quoteTemplateTextDesignExtendedPMService.GetQuoteTemplateTextDesignPMListByIds(ids, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.QuoteTemplateTextDesignPMLists = pmResponse.Result;
                if (_this.TableDesignPM) {
                    _this.HeaderTextDesignPM = _this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.Id == _this.TableDesignPM.HeaderDesignId; })[0];
                    if (_this.HeaderTextDesignPM) {
                        _this.HeaderTextDesignPM.Title = "Header";
                    }
                    _this.RowTextDesignPM = _this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.Id == _this.TableDesignPM.LinesDesignId; })[0];
                    if (_this.RowTextDesignPM) {
                        _this.RowTextDesignPM.Title = "Rows";
                        //this.RowTextDesignPM.HideAlignment = true;
                    }
                }
                _this.TotalLabelTextDesignPM = _this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.Id == totalsLabelTextDesignId; })[0];
                if (_this.TotalLabelTextDesignPM) {
                    _this.TotalLabelTextDesignPM.Title = "Label";
                    //this.TotalLabelTextDesignPM.HideAlignment = true;
                }
                _this.TotalValueTextDesignPM = _this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.Id == totalValueTextDesignId; })[0];
                if (_this.TotalValueTextDesignPM) {
                    // this.TotalValueTextDesignPM.HideAlignment = true;
                    _this.TotalValueTextDesignPM.Title = "Value";
                }
                _this.GroupByHeaderTextDesignPM = _this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.Id == groupByHeaderTextDesignId; })[0];
                if (_this.GroupByHeaderTextDesignPM) {
                    _this.GroupByHeaderTextDesignPM.Title = "Header";
                }
                _this.GroupByTotalTextDesignPM = _this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.Id == groupByTotailTextDesignId; })[0];
                if (_this.GroupByTotalTextDesignPM) {
                    // this.GroupByTotalTextDesignPM.HideAlignment = true;
                    _this.GroupByTotalTextDesignPM.Title = "Total";
                }
                _this.TitleTextDesignPM = _this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.Id == titleTextDesignId; })[0];
                if (_this.TitleTextDesignPM) {
                    _this.TitleTextDesignPM.Title = "Title";
                }
            }
            _this.IsLoadPage = true;
        });
    };
    //Load Table Design
    QuoteTemplatePricingSettingComponent.prototype.LoadTableDesign = function () {
        var _this = this;
        var tableDesignId = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.PackagesTableDesignId : this.QuoteTemplateSettingPM.ContainserTableDesignId;
        this.quoteTemplateTableDesignPMService.get(tableDesignId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.TableDesignPM = pmResponse.Result;
            }
            _this.LoadTextDesign();
        });
    };
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowTotalInLocalCurrency", {
        get: function () {
            var showTotalInLocalCurrency = false;
            if (this.QuoteTemplateSettingPM)
                showTotalInLocalCurrency = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowTotalInLocalCurrencyPackages : this.QuoteTemplateSettingPM.ShowTotalInLocalCurrencyContainers;
            return showTotalInLocalCurrency;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages") {
                    this.QuoteTemplateSettingPM.ShowTotalInLocalCurrencyPackages = value;
                }
                else
                    this.QuoteTemplateSettingPM.ShowTotalInLocalCurrencyContainers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowTotalInSaleCurrency", {
        get: function () {
            var showTotalInSaleCurrency = false;
            if (this.QuoteTemplateSettingPM)
                showTotalInSaleCurrency = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowTotalInSaleCurrencyPackages : this.QuoteTemplateSettingPM.ShowTotalInSaleCurrencyContainers;
            return showTotalInSaleCurrency;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages") {
                    this.QuoteTemplateSettingPM.ShowTotalInSaleCurrencyPackages = value;
                }
                else
                    this.QuoteTemplateSettingPM.ShowTotalInSaleCurrencyContainers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowTitlePricing", {
        get: function () {
            var showTitlePricing = false;
            if (this.QuoteTemplateSettingPM)
                showTitlePricing = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowTitlePricingPackages : this.QuoteTemplateSettingPM.ShowTitlePricingContainsers;
            return showTitlePricing;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages") {
                    this.QuoteTemplateSettingPM.ShowTitlePricingPackages = value;
                }
                else
                    this.QuoteTemplateSettingPM.ShowTitlePricingContainsers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowPricesTable", {
        get: function () {
            var showPricesTable = false;
            if (this.QuoteTemplateSettingPM)
                showPricesTable = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowPricesTablePackages : this.QuoteTemplateSettingPM.ShowPricesTableContainers;
            if (!showPricesTable)
                this.DisablePricingSetting();
            return showPricesTable;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages") {
                    this.QuoteTemplateSettingPM.ShowPricesTablePackages = value;
                }
                else
                    this.QuoteTemplateSettingPM.ShowPricesTableContainers = value;
                if (!value)
                    this.DisablePricingSetting();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "SplitChargesbyGroups", {
        get: function () {
            var splitChargesbyGroups = false;
            if (this.QuoteTemplateSettingPM)
                splitChargesbyGroups = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.SplitChargesbyGroupsPackages : this.QuoteTemplateSettingPM.SplitChargesbyGroupsContainers;
            return splitChargesbyGroups;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages") {
                    this.QuoteTemplateSettingPM.SplitChargesbyGroupsPackages = value;
                }
                else
                    this.QuoteTemplateSettingPM.SplitChargesbyGroupsContainers = value;
                if (!value) {
                    this.ShowTotalPerChargeGroup = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowChargeCode", {
        get: function () {
            var showChargeCode = false;
            if (this.QuoteTemplateSettingPM)
                showChargeCode = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowChargeCodePackages : this.QuoteTemplateSettingPM.ShowChargeCodeContainers;
            return showChargeCode;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages") {
                    this.QuoteTemplateSettingPM.ShowChargeCodePackages = value;
                }
                else
                    this.QuoteTemplateSettingPM.ShowChargeCodeContainers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowChargeName", {
        get: function () {
            var showChargeName = false;
            if (this.QuoteTemplateSettingPM)
                showChargeName = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowChargeNamePackages : this.QuoteTemplateSettingPM.ShowChargeNameContainers;
            return showChargeName;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages") {
                    this.QuoteTemplateSettingPM.ShowChargeNamePackages = value;
                }
                else
                    this.QuoteTemplateSettingPM.ShowChargeNameContainers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowMeasurement", {
        get: function () {
            var showMeasurement = false;
            if (this.QuoteTemplateSettingPM)
                showMeasurement = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowMeasurementPackages : this.QuoteTemplateSettingPM.ShowMeasurementContainers;
            return showMeasurement;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages") {
                    this.QuoteTemplateSettingPM.ShowMeasurementPackages = value;
                }
                else
                    this.QuoteTemplateSettingPM.ShowMeasurementContainers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowPrice1Label", {
        get: function () {
            var showPrice1Label = "";
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                showPrice1Label = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.ShowUnits");
            }
            else
                showPrice1Label = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.ShowFixedPrice");
            return showPrice1Label;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowPrice1", {
        get: function () {
            var showPrice1 = false;
            if (this.QuoteTemplateSettingPM)
                showPrice1 = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowUnitsPackages : this.QuoteTemplateSettingPM.ShowFixedPriceContainers;
            return showPrice1;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages") {
                    this.QuoteTemplateSettingPM.ShowUnitsPackages = value;
                }
                else
                    this.QuoteTemplateSettingPM.ShowFixedPriceContainers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowPrice2Label", {
        get: function () {
            var showPrice2Label = "";
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                showPrice2Label = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.ShowUnitPrice");
            }
            else
                showPrice2Label = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.ShowPriceByContainer");
            return showPrice2Label;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowPrice2", {
        get: function () {
            var showPrice2 = false;
            if (this.QuoteTemplateSettingPM)
                showPrice2 = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowUnitPricePackages : this.QuoteTemplateSettingPM.ShowPriceByContainerColumn;
            return showPrice2;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages") {
                    this.QuoteTemplateSettingPM.ShowUnitPricePackages = value;
                }
                else
                    this.QuoteTemplateSettingPM.ShowPriceByContainerColumn = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowSaleCurrencyColumn", {
        get: function () {
            var showSaleCurrencyColumn = false;
            if (this.QuoteTemplateSettingPM)
                showSaleCurrencyColumn = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowSaleCurrencyColumnPackages : this.QuoteTemplateSettingPM.ShowSaleCurrencyColumnContainers;
            return showSaleCurrencyColumn;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (!value)
                    this.ShowTotalPerChargeGroup = false;
                if (this.QuoteTemplateSectionTypeName == "Packages") {
                    this.QuoteTemplateSettingPM.ShowSaleCurrencyColumnPackages = value;
                }
                else
                    this.QuoteTemplateSettingPM.ShowSaleCurrencyColumnContainers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowLocalCurrencyColumn", {
        get: function () {
            var showLocalCurrencyColumn = false;
            if (this.QuoteTemplateSettingPM)
                showLocalCurrencyColumn = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowLocalCurrencyColumnPackages : this.QuoteTemplateSettingPM.ShowLocalCurrencyColumnContainers;
            return showLocalCurrencyColumn;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages") {
                    this.QuoteTemplateSettingPM.ShowLocalCurrencyColumnPackages = value;
                }
                else
                    this.QuoteTemplateSettingPM.ShowLocalCurrencyColumnContainers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowChargeDescription", {
        get: function () {
            var showChargeDescription = false;
            if (this.QuoteTemplateSettingPM)
                showChargeDescription = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowChargeDescriptionPackages : this.QuoteTemplateSettingPM.ShowChargeDescriptionContainers;
            return showChargeDescription;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages") {
                    this.QuoteTemplateSettingPM.ShowChargeDescriptionPackages = value;
                }
                else
                    this.QuoteTemplateSettingPM.ShowChargeDescriptionContainers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowTotalPerChargeGroup", {
        get: function () {
            var showTotalPerChargeGroup = false;
            if (this.QuoteTemplateSettingPM)
                showTotalPerChargeGroup = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowTotalPerChargeGroupPackages : this.QuoteTemplateSettingPM.ShowTotalPerChargeGroupContainers;
            return showTotalPerChargeGroup;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages")
                    this.QuoteTemplateSettingPM.ShowTotalPerChargeGroupPackages = value;
                else
                    this.QuoteTemplateSettingPM.ShowTotalPerChargeGroupContainers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowChargeNoteColumn", {
        get: function () {
            var showChargeNoteColumn = false;
            if (this.QuoteTemplateSettingPM)
                showChargeNoteColumn = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowChargeNotePackages : this.QuoteTemplateSettingPM.ShowChargeNoteContainers;
            return showChargeNoteColumn;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages")
                    this.QuoteTemplateSettingPM.ShowChargeNotePackages = value;
                else
                    this.QuoteTemplateSettingPM.ShowChargeNoteContainers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowSaleMaxMinAmountColumn", {
        get: function () {
            var showSaleMaxMinAmountColumn = false;
            if (this.QuoteTemplateSettingPM)
                showSaleMaxMinAmountColumn = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowSaleMaxMinAmountPackages : this.QuoteTemplateSettingPM.ShowSaleMaxMinAmountContainers;
            return showSaleMaxMinAmountColumn;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages")
                    this.QuoteTemplateSettingPM.ShowSaleMaxMinAmountPackages = value;
                else
                    this.QuoteTemplateSettingPM.ShowSaleMaxMinAmountContainers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplatePricingSettingComponent.prototype, "ShowHeaderLabels", {
        get: function () {
            var showHeaderLabels = false;
            if (this.QuoteTemplateSettingPM)
                showHeaderLabels = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowHeaderLabelsPackages : this.QuoteTemplateSettingPM.ShowHeaderLabelsContainers;
            return showHeaderLabels;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeName == "Packages")
                    this.QuoteTemplateSettingPM.ShowHeaderLabelsPackages = value;
                else
                    this.QuoteTemplateSettingPM.ShowHeaderLabelsContainers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteTemplatePricingSettingComponent.prototype.DisablePricingSetting = function () {
        this.ShowChargeCode = false;
        this.ShowChargeName = false;
        this.ShowMeasurement = false;
        this.ShowPrice1 = false;
        this.ShowPrice2 = false;
        this.ShowSaleCurrencyColumn = false;
        this.ShowLocalCurrencyColumn = false;
        this.ShowChargeDescription = false;
        this.ShowSaleMaxMinAmountColumn = false;
        this.ShowHeaderLabels = false;
    };
    // End Prop setting 
    QuoteTemplatePricingSettingComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var textDesignPmLists = this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.IsDirty == true; });
        if (textDesignPmLists.length > 0)
            this.IsSaveQuoteTemplateTextDesignRuning = true;
        if (this.TableDesignPM.IsDirty)
            this.IsSaveQuoteTemplateTableDesignRuning = true;
        var textCodeDataLists = this.ItemsSource.Collection.filter(function (d) { return d.EntityPM.IsDirty == true; });
        if (textCodeDataLists.length > 0)
            this.IsSaveQuoteTemplateTextCodeRuning = true;
        if (this.IsSaveQuoteTemplateTextDesignRuning || this.IsSaveQuoteTemplateTableDesignRuning || this.IsSaveQuoteTemplateTextCodeRuning) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
            if (this.QuoteTemplateSettingPM.IsDirty) {
                this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(function (res) {
                    _this.QuoteTemplateSettingPM.IsDirty = false;
                    _this.SaveOthers(textDesignPmLists, textCodeDataLists);
                });
            }
            else
                this.SaveOthers(textDesignPmLists, textCodeDataLists);
        }
        else {
            if (this.QuoteTemplateSettingPM.IsDirty) {
                this.SaveQuoteTemplateSetting();
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                if (this.IsPerContainerChange == true) {
                    this.CurrentSession.CurrentWindow.Close("Refresh");
                }
                else
                    this.CurrentSession.CloseCurrentWindow();
            }
        }
    };
    QuoteTemplatePricingSettingComponent.prototype.SaveOthers = function (textDesignPmLists, textCodeDataLists) {
        if (this.IsSaveQuoteTemplateTextDesignRuning)
            this.SaveQuoteTemplateTextDesign(textDesignPmLists);
        if (this.IsSaveQuoteTemplateTableDesignRuning)
            this.SaveQuoteTemplateTableDesign();
        if (this.IsSaveQuoteTemplateTextCodeRuning)
            this.SaveQuoteTemplateTextCode(textCodeDataLists);
    };
    QuoteTemplatePricingSettingComponent.prototype.SaveQuoteTemplateTextDesign = function (items) {
        var _this = this;
        items.forEach(function (item) { item.IsDirty = false; });
        this.quoteTemplateTextDesignExtendedPMService.updateQuoteTemplateTextDesignPMs(items).subscribe(function (res) {
            _this.IsSaveQuoteTemplateTextDesignRuning = false;
            _this.SaveCompleted();
        });
    };
    QuoteTemplatePricingSettingComponent.prototype.SaveQuoteTemplateTableDesign = function () {
        var _this = this;
        this.quoteTemplateTableDesignPMService.update(this.TableDesignPM).subscribe(function (res) {
            _this.IsSaveQuoteTemplateTableDesignRuning = false;
            _this.SaveCompleted();
        });
    };
    QuoteTemplatePricingSettingComponent.prototype.SaveQuoteTemplateSetting = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
        this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(function (res) {
            _this.QuoteTemplateSettingPM.IsDirty = false;
            _this.SaveCompleted();
        });
    };
    QuoteTemplatePricingSettingComponent.prototype.SaveQuoteTemplateTextCode = function (items) {
        var _this = this;
        var quoteTemplateTextCodePMLists = [];
        items.forEach(function (item) {
            if (item.EntityPM) {
                item.EntityPM.IsDirty = false;
                quoteTemplateTextCodePMLists.push(item.EntityPM);
            }
        });
        this.quoteTemplateTextCodeExtendedPMService.updateTextCodes(quoteTemplateTextCodePMLists).subscribe(function (res) {
            _this.IsSaveQuoteTemplateTextCodeRuning = false;
            _this.SaveCompleted();
        });
    };
    QuoteTemplatePricingSettingComponent.prototype.ShowTotalPerContainer = function () {
        var _this = this;
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.QuoteTemplatePM = this.EntityPM;
        windowArgs.QuoteTemplateSettingPM = this.QuoteTemplateSettingPM;
        windowArgs.QuoteTemplateTextCodePMList = this.AllQuoteTemplateTextCodePMList;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 930;
        logWindow.Height = 580;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.S.TotalPerContainerSettings");
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/QuoteTemplateTotalPerContainerSetting");
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event == "Refresh") {
                _this.IsPerContainerChange = true;
            }
        });
    };
    QuoteTemplatePricingSettingComponent.prototype.SaveCompleted = function () {
        if (!this.IsSaveQuoteTemplateTextDesignRuning && !this.IsSaveQuoteTemplateTableDesignRuning && !this.IsSaveQuoteTemplateTextCodeRuning) {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CurrentWindow.Close("Refresh");
        }
    };
    QuoteTemplatePricingSettingComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], QuoteTemplatePricingSettingComponent.prototype, "viewContainerRef", void 0);
    QuoteTemplatePricingSettingComponent = __decorate([
        core_1.Component({
            selector: 'QuoteTemplatePricingSettingComponent',
            moduleId: module.id,
            templateUrl: './QuoteTemplatePricingSettingComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], QuoteTemplatePricingSettingComponent);
    return QuoteTemplatePricingSettingComponent;
}(BaseComponent_1.BaseComponent));
exports.QuoteTemplatePricingSettingComponent = QuoteTemplatePricingSettingComponent;
var TextCodeData = /** @class */ (function (_super) {
    __extends(TextCodeData, _super);
    function TextCodeData(entity) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ToolTipEnglishNameMessage = "";
        _this.ToolTipLocalNameMessage = "";
        _this.EntityPM = entity;
        return _this;
    }
    Object.defineProperty(TextCodeData.prototype, "EnglishName", {
        get: function () {
            var englishName = "";
            if (this.EntityPM)
                englishName = this.EntityPM.EnglishName;
            return englishName;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                this.EntityPM.EnglishName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextCodeData.prototype, "LocalName", {
        get: function () {
            var localName = "";
            if (this.EntityPM)
                localName = this.EntityPM.LocalName;
            return localName;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                this.EntityPM.LocalName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextCodeData.prototype, "IsShowRestoreOriginalEnglishName", {
        get: function () {
            var isShowRestoreOriginalEnglishName = false;
            this.ToolTipEnglishNameMessage = "";
            if (this.EntityPM) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginalEnglishName)) {
                    if (this.EntityPM.OriginalEnglishName != this.EntityPM.EnglishName) {
                        isShowRestoreOriginalEnglishName = true;
                        this.ToolTipEnglishNameMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.ValueEditedByUserMessage") + " {" + this.EntityPM.OriginalEnglishName + "}";
                    }
                }
            }
            return isShowRestoreOriginalEnglishName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextCodeData.prototype, "IsShowRestoreOriginalLocalName", {
        get: function () {
            var isShowRestoreOriginalLocalName = false;
            this.ToolTipLocalNameMessage = "";
            if (this.EntityPM) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginalLocalName)) {
                    if (this.EntityPM.OriginalLocalName != this.EntityPM.LocalName) {
                        isShowRestoreOriginalLocalName = true;
                        this.ToolTipLocalNameMessage = "Value edited by user, double click to reset to {" + this.EntityPM.OriginalLocalName + "}";
                    }
                }
            }
            return isShowRestoreOriginalLocalName;
        },
        enumerable: true,
        configurable: true
    });
    TextCodeData.prototype.RestoreEnglishName = function (item) {
        item.EnglishName = item.EntityPM.OriginalEnglishName;
    };
    TextCodeData.prototype.RestoreLocalName = function (item) {
        item.LocalName = item.EntityPM.OriginalLocalName;
    };
    return TextCodeData;
}(BaseComponent_1.BaseComponent));
exports.TextCodeData = TextCodeData;
//# sourceMappingURL=QuoteTemplatePricingSettingComponent.js.map