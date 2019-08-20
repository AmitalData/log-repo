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
var QuoteTemplateDetailsFieldPM_1 = require("../../../Quote/EntityPMs/QuoteTemplateDetailsFieldPM");
var QuoteTemplateHeaderFieldPM_1 = require("../../../Quote/EntityPMs/QuoteTemplateHeaderFieldPM");
var TextDesignComponent_1 = require("../../../Infrastructure/Components/LogitudeCustomComponents/TextDesignComponent");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var QuoteTemplateSettingPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService");
var QuoteTemplateTableDesignPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateTableDesignPMService");
var QuoteTemplateTextDesignPMService_1 = require("../../../Quote/Services/StandardPMs/QuoteTemplateTextDesignPMService");
var QuoteTemplateTextDesignExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateTextDesignExtendedPMService");
var QuoteTemplateTextCodeExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateTextCodeExtendedPMService");
var QuoteTemplateDetailsFieldExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateDetailsFieldExtendedPMService");
var QuoteTemplateHeaderFieldExtendedPMService_1 = require("../../../Quote/Services/ExtendedPMs/QuoteTemplateHeaderFieldExtendedPMService");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var QuoteTemplatePricingSettingComponent_1 = require("./QuoteTemplatePricingSettingComponent");
var Tools_1 = require("../../../Infrastructure/Tools");
var QuoteTemplateHeaderDetailsSettingComponent = /** @class */ (function (_super) {
    __extends(QuoteTemplateHeaderDetailsSettingComponent, _super);
    function QuoteTemplateHeaderDetailsSettingComponent() {
        var _this = _super.call(this) || this;
        _this.Alignment = [];
        _this.IsLoadingTextDesign = true;
        _this.IsLoadingQuoteField = true;
        _this.QuoteTemplateDetailsFieldPMList = [];
        _this.QuoteTemplateHeaderFieldPMList = [];
        _this.QuoteTemplateTextDesignPMLists = [];
        _this.QuoteTemplateTextCodePMList = [];
        _this.ObjectFieldTextList = [];
        _this.ObjectFieldTextListColum1 = [];
        _this.ObjectFieldTextListColum2 = [];
        _this.AllObjectFieldTextList = [];
        _this.ObjectFieldPMList = [];
        _this.IsSaveQuoteTemplateTextDesignRuning = false;
        _this.IsSaveQuoteTemplateTableDesignRuning = false;
        _this.IsSaveQuoteTemplateTextCodeRuning = false;
        _this.IsSaveQuoteTemplateObjectField = false;
        _this.BorderTypes = [];
        _this.QuoteTemplateSectionTypeName = "QuoteHeader";
        _this.QuoteTemplateSectionTypeCode = "QH";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //Prop setting 
        _this.ShowTitleQuoteDetailsKey = Guid_1.Guid.newGuid();
        _this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService_1.QuoteTemplateSettingPMService();
        _this.quoteTemplateTableDesignPMService = new QuoteTemplateTableDesignPMService_1.QuoteTemplateTableDesignPMService();
        _this.quoteTemplateTextDesignPMService = new QuoteTemplateTextDesignPMService_1.QuoteTemplateTextDesignPMService();
        _this.quoteTemplateTextDesignExtendedPMService = new QuoteTemplateTextDesignExtendedPMService_1.QuoteTemplateTextDesignExtendedPMService();
        _this.quoteTemplateTextCodeExtendedPMService = new QuoteTemplateTextCodeExtendedPMService_1.QuoteTemplateTextCodeExtendedPMService();
        _this.quoteTemplateDetailsFieldExtendedPMService = new QuoteTemplateDetailsFieldExtendedPMService_1.QuoteTemplateDetailsFieldExtendedPMService();
        _this.quoteTemplateHeaderFieldExtendedPMService = new QuoteTemplateHeaderFieldExtendedPMService_1.QuoteTemplateHeaderFieldExtendedPMService();
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    QuoteTemplateHeaderDetailsSettingComponent.prototype.ngOnInit = function () {
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.SelectedTabCode = "TAC";
        this.QuoteTemplatePM = args.QuoteTemplatePM;
        this.QuoteTemplateSectionTypeName = args.QuoteTemplateSectionTypeName;
        this.QuoteTemplateSectionTypeCode = args.QuoteTemplateSectionTypeCode;
        this.QuoteTemplateSettingPM = args.QuoteTemplateSettingPM;
        this.QuotePM = args.QuotePM;
        this.QuoteTemplateDetailsFieldPMList = [];
        this.QuoteTemplateHeaderFieldPMList = [];
        this.QuoteTemplateTextDesignPMLists = [];
        this.QuoteTemplateTextCodePMList = [];
        this.ObjectFieldTextList = [];
        this.ObjectFieldTextListColum1 = [];
        this.ObjectFieldTextListColum2 = [];
        this.BorderTypes = [];
        this.BorderTypes.push(new TextDesignComponent_1.BorderType("Auto", "AUTO"));
        this.BorderTypes.push(new TextDesignComponent_1.BorderType("Fixed", "FIXED"));
        var selectedBorderCode = this.QuoteTemplateSectionTypeCode == "QH" ? this.QuoteTemplateSettingPM.HeaderTableColumWidthType : this.QuoteTemplateSettingPM.DetailsTableColumWidthType;
        this.BorderTypesSelected = this.BorderTypes.filter(function (d) { return d.Code == selectedBorderCode; })[0];
        this.Alignment.push("Left");
        this.Alignment.push("Center");
        this.Alignment.push("Right");
        if (args.QuoteTemplateTextCodePMList) {
            this.QuoteTemplateTextCodePMList = args.QuoteTemplateTextCodePMList.filter(function (d) { return d.Area == _this.QuoteTemplateSectionTypeName; });
            this.CustomQuoteTemplateTextCodePMLists();
            this.BuildItemsSource();
        }
        this.LoadData();
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.BorderTypesSelectedChanged = function (border) {
        if (this.QuoteTemplateSettingPM) {
            if (this.QuoteTemplateSectionTypeCode == "QH") {
                if (this.QuoteTemplateSettingPM.HeaderTableColumWidthType != border.Code) {
                    this.QuoteTemplateSettingPM.HeaderTableColumWidthType = border.Code;
                }
            }
            else {
                if (this.QuoteTemplateSettingPM.DetailsTableColumWidthType != border.Code) {
                    this.QuoteTemplateSettingPM.DetailsTableColumWidthType = border.Code;
                }
            }
        }
    };
    Object.defineProperty(QuoteTemplateHeaderDetailsSettingComponent.prototype, "ShowTitleQuoteDetails", {
        get: function () {
            var showTitleQuoteDetails = false;
            if (this.QuoteTemplateSettingPM)
                showTitleQuoteDetails = this.QuoteTemplateSettingPM.ShowTitleQuoteDetails;
            return showTitleQuoteDetails;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                this.QuoteTemplateSettingPM.ShowTitleQuoteDetails = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderDetailsSettingComponent.prototype, "TableColumn1LabelWidth", {
        get: function () {
            var tableColumn1LabelWidth = 0;
            if (this.QuoteTemplateSettingPM)
                tableColumn1LabelWidth = this.QuoteTemplateSectionTypeCode == "QH" ? this.QuoteTemplateSettingPM.HeaderTableColumn1LabelWidth : this.QuoteTemplateSettingPM.DetailsTableColumn1LabelWidth;
            return tableColumn1LabelWidth;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeCode == "QH")
                    this.QuoteTemplateSettingPM.HeaderTableColumn1LabelWidth = value;
                else
                    this.QuoteTemplateSettingPM.DetailsTableColumn1LabelWidth = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderDetailsSettingComponent.prototype, "TableColumn1ValueWidth", {
        get: function () {
            var tableColumn1ValueWidth = 0;
            if (this.QuoteTemplateSettingPM)
                tableColumn1ValueWidth = this.QuoteTemplateSectionTypeCode == "QH" ? this.QuoteTemplateSettingPM.HeaderTableColumn1ValueWidth : this.QuoteTemplateSettingPM.DetailsTableColumn1ValueWidth;
            return tableColumn1ValueWidth;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeCode == "QH")
                    this.QuoteTemplateSettingPM.HeaderTableColumn1ValueWidth = value;
                else
                    this.QuoteTemplateSettingPM.DetailsTableColumn1ValueWidth = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderDetailsSettingComponent.prototype, "TableColumn2LabelWidth", {
        get: function () {
            var tableColumn2LabelWidth = 0;
            if (this.QuoteTemplateSettingPM)
                tableColumn2LabelWidth = this.QuoteTemplateSectionTypeCode == "QH" ? this.QuoteTemplateSettingPM.HeaderTableColumn2LabelWidth : this.QuoteTemplateSettingPM.DetailsTableColumn2LabelWidth;
            return tableColumn2LabelWidth;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeCode == "QH")
                    this.QuoteTemplateSettingPM.HeaderTableColumn2LabelWidth = value;
                else
                    this.QuoteTemplateSettingPM.DetailsTableColumn2LabelWidth = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteTemplateHeaderDetailsSettingComponent.prototype, "TableColumn2ValueWidth", {
        get: function () {
            var tableColumn2ValueWidth = 0;
            if (this.QuoteTemplateSettingPM)
                tableColumn2ValueWidth = this.QuoteTemplateSectionTypeCode == "QH" ? this.QuoteTemplateSettingPM.HeaderTableColumn2ValueWidth : this.QuoteTemplateSettingPM.DetailsTableColumn2ValueWidth;
            return tableColumn2ValueWidth;
        },
        set: function (value) {
            if (this.QuoteTemplateSettingPM != null) {
                if (this.QuoteTemplateSectionTypeCode == "QH")
                    this.QuoteTemplateSettingPM.HeaderTableColumn2ValueWidth = value;
                else
                    this.QuoteTemplateSettingPM.DetailsTableColumn2ValueWidth = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteTemplateHeaderDetailsSettingComponent.prototype.CustomQuoteTemplateTextCodePMLists = function () {
        if (this.QuoteTemplateSectionTypeCode == "QD" && this.QuoteTemplateTextCodePMList) {
            var itemShipingLine = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode == "SHIPINFLINE"; })[0];
            var itemTruker = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode == "TRUCKER"; })[0];
            var itemAirLine = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode == "AIRLINE"; })[0];
            var fromport = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode == "FROMPORT"; })[0];
            var toport = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode == "TOPORT"; })[0];
            var fromLocation = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode == "FROMLOCATION"; })[0];
            var toLocation = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode == "TOLOCATION"; })[0];
            var NumberOfPackages = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode == "NUMBEROFPACKAGES"; })[0];
            var NumberOfContainers = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode == "NUMBEROFCONTAINERS"; })[0];
            if (this.QuotePM != null) {
                if (this.QuotePM.TransportModeName == "Air") {
                    this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode != itemShipingLine.TextCode && d.TextCode != itemTruker.TextCode; });
                }
                else if (this.QuotePM.TransportModeName == "Ocean") {
                    this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode != itemAirLine.TextCode && d.TextCode != itemTruker.TextCode; });
                }
                else if (this.QuotePM.TransportModeName == "Inland") {
                    this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode != itemAirLine.TextCode && d.TextCode != itemShipingLine.TextCode && d.TextCode != NumberOfPackages.TextCode; });
                }
                if (this.QuotePM.TransportModeId == "A" || this.QuotePM.ShipmentTypeId == "LCL" || this.QuotePM.ShipmentTypeId == "LCLD" || this.QuotePM.ShipmentTypeId == "LTL") {
                    this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode != NumberOfContainers.TextCode; });
                }
                else if (this.QuotePM.ShipmentTypeId == "FCL" || this.QuotePM.ShipmentTypeId == "FTL" || this.QuotePM.ShipmentTypeId == "FCLD") {
                    this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode != NumberOfPackages.TextCode; });
                }
                if (this.QuotePM.DirectionId == "D") {
                    this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode != toport.TextCode && d.TextCode != fromport.TextCode; });
                }
                else {
                    this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode != toLocation.TextCode && d.TextCode != fromLocation.TextCode; });
                }
            }
            else {
                this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(function (d) { return d.TextCode != itemShipingLine.TextCode && d.TextCode != itemTruker.TextCode && d.TextCode != NumberOfContainers.TextCode && d.TextCode != toport.TextCode && d.TextCode != fromport.TextCode; });
            }
        }
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.CustomQuoteFieldList = function () {
        var _this = this;
        if (this.QuoteTemplateSectionTypeCode == "QD") {
            var QuoteFieldNameString = "Expiration Date, Expiration Days, Shipper Name, Shipper Address, Quote Number, Shipper Contact, Shipper References , Consignee Name, Consignee Address, Consignee Contact, Consignee References, Customer Name, Customer Address, Customer Contact, Customer References, Pickup From, Delivery To, Incoterms, Service, Salesman, Description of goods , Dangerous goods, Chargeable Weight, Gross Weight, Volume, Volumetric Weight, Transit Time, Notify Name, Notify Address, Notify Contact ,Move Type";
            var quoteFieldList = QuoteFieldNameString.split(',');
            if (this.QuotePM != null) {
                if (this.QuotePM.TransportModeName == "Air") {
                    quoteFieldList.push("AirLine");
                }
                else if (this.QuotePM.TransportModeName == "Inland") {
                    quoteFieldList.push("ShipingLine");
                }
                else if (this.QuotePM.TransportModeName == "Ocean") {
                    quoteFieldList.push("Trucker");
                }
                if (this.QuotePM.TransportModeId == "A" || this.QuotePM.ShipmentTypeId == "LCL" || this.QuotePM.ShipmentTypeId == "LCLD" || this.QuotePM.ShipmentTypeId == "LTL") {
                    quoteFieldList.push("Number Of Packages");
                }
                else if (this.QuotePM.ShipmentTypeId == "FCL" || this.QuotePM.ShipmentTypeId == "FTL" || this.QuotePM.ShipmentTypeId == "FCLD") {
                    quoteFieldList.push("Number Of Containers");
                }
                if (this.QuotePM.DirectionId == "D") {
                    quoteFieldList.push("From Location");
                    quoteFieldList.push("To Location");
                }
                else {
                    quoteFieldList.push("From Port");
                    quoteFieldList.push("To Port");
                }
            }
            else {
                quoteFieldList.push("AirLine");
                quoteFieldList.push("From Location");
                quoteFieldList.push("To Location");
                quoteFieldList.push("Number Of Packages");
            }
        }
        else {
            var QuoteFieldNameString = "Quote Date, Expiration Date, Quote Number, Customer, ATTN";
            quoteFieldList = QuoteFieldNameString.split(',');
        }
        this.ObjectFieldTextList = [];
        quoteFieldList.forEach(function (qouteField) {
            _this.ObjectFieldTextList.push(new ObjectFieldText(qouteField, qouteField.trim().replace(" ", "").replace(/\s+/g, '').toUpperCase()));
        });
        var table = window.ObjectTables.filter(function (d) { return d.Name == "Quote"; })[0];
        this.ObjectFieldPMList = window.ObjectFields.filter(function (d) { return d.ObjectTableId == table.Id && d.IsCustom; });
        if (this.ObjectFieldPMList) {
            this.ObjectFieldPMList.forEach(function (objectFieldPM) {
                var defaultText = TextCodeTranslator_1.TextCodeTranslator.Translate(objectFieldPM.FullNameTextCodeCode);
                _this.ObjectFieldTextList.push(new ObjectFieldText(defaultText, objectFieldPM.FullNameTextCodeCode));
            });
        }
        if (this.QuoteTemplateSectionTypeCode == "QD")
            this.LoadQuoteTemplateDetailsFields();
        else
            this.LoadQuoteTemplateHeaderFields();
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.BuildItemsSource = function () {
        var itemsCollection = [];
        this.QuoteTemplateTextCodePMList.forEach(function (item) {
            itemsCollection.push(new QuoteTemplatePricingSettingComponent_1.TextCodeData(item));
        });
        this.ItemsSource.AppendCollection(itemsCollection);
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.LoadData = function () {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        this.QuoteTemplateTextDesignPMLists = [];
        this.LoadTableDesign();
        this.CustomQuoteFieldList();
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.LoadTextDesign = function () {
        var _this = this;
        var ids = "";
        if (this.QuoteTemplateSectionTypeCode == "QD") {
            ids = this.QuoteTemplateSettingPM.DetailsTitleDesignId;
        }
        if (this.TableDesignPM) {
            if (!Tools_1.AppTool.IsNullOrEmpty(ids))
                ids += ",";
            ids += this.TableDesignPM.HeaderDesignId;
            ids += ("," + this.TableDesignPM.LinesDesignId);
        }
        this.quoteTemplateTextDesignExtendedPMService.GetQuoteTemplateTextDesignPMListByIds(ids, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.IsLoadingTextDesign = false;
            _this.LoadCompleted();
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.QuoteTemplateTextDesignPMLists = pmResponse.Result;
                if (_this.TableDesignPM) {
                    _this.HeaderTextDesignPM = _this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.Id == _this.TableDesignPM.HeaderDesignId; })[0];
                    if (_this.HeaderTextDesignPM) {
                        _this.HeaderTextDesignPM.Title = "Label";
                    }
                    _this.RowTextDesignPM = _this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.Id == _this.TableDesignPM.LinesDesignId; })[0];
                    if (_this.RowTextDesignPM) {
                        _this.RowTextDesignPM.Title = "Value";
                        _this.RowTextDesignPM.HideAlignment = true;
                    }
                }
                if (_this.QuoteTemplateSectionTypeCode == "QD") {
                    _this.TitleTextDesignPM = _this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.Id == _this.QuoteTemplateSettingPM.DetailsTitleDesignId; })[0];
                    if (_this.TitleTextDesignPM) {
                        _this.TitleTextDesignPM.Title = "Title Design";
                    }
                }
            }
        });
    };
    //Load Table Design
    QuoteTemplateHeaderDetailsSettingComponent.prototype.LoadTableDesign = function () {
        var _this = this;
        var tableDesignId = this.QuoteTemplateSectionTypeCode == "QD" ? this.QuoteTemplateSettingPM.DetailsTableDesignId : this.QuoteTemplateSettingPM.HeaderTableDesignId;
        this.quoteTemplateTableDesignPMService.get(tableDesignId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.TableDesignPM = pmResponse.Result;
            }
            _this.LoadTextDesign();
        });
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.LoadQuoteTemplateDetailsFields = function () {
        var _this = this;
        this.quoteTemplateDetailsFieldExtendedPMService.GetQuoteTemplateDetailsFieldByQuoteTemplateId(this.QuoteTemplatePM.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.IsLoadingQuoteField = false;
            _this.LoadCompleted();
            _this.ObjectFieldTextListColum2 = [];
            _this.ObjectFieldTextListColum1 = [];
            _this.AllObjectFieldTextList = [];
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.QuoteTemplateDetailsFieldPMList = pmResponse.Result;
                _this.FillObjectFieldTextColumLists(0, _this.QuoteTemplateDetailsFieldPMList, "Details");
                _this.FillObjectFieldTextColumLists(1, _this.QuoteTemplateDetailsFieldPMList, "Details");
            }
            _this.FillAllObjectFieldLists();
        });
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.FillAllObjectFieldLists = function () {
        var _this = this;
        if (this.ObjectFieldTextList) {
            this.ObjectFieldTextList.forEach(function (item) {
                _this.AllObjectFieldTextList.push(item);
            });
        }
        this.ObjectFieldTextListColum1.forEach(function (item) {
            _this.AllObjectFieldTextList.push(item);
        });
        this.ObjectFieldTextListColum2.forEach(function (item) {
            _this.AllObjectFieldTextList.push(item);
        });
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.FillObjectFieldTextColumLists = function (column, quoteTemplateFieldPMList, type) {
        var _this = this;
        quoteTemplateFieldPMList.filter(function (d) { return d.Column == column; }).sort(function (a, b) { return a.Row - b.Row; }).forEach(function (item) {
            var fieldCode = item.FieldCode;
            var field = type == "Header" ? _this.GetFieldNameQuoteHeader(fieldCode) : _this.GetFieldNameQuoteDetails(fieldCode);
            if (Tools_1.AppTool.IsNullOrEmpty(field))
                field = TextCodeTranslator_1.TextCodeTranslator.Translate(item.FieldCode);
            _this.ObjectFieldTextList = _this.ObjectFieldTextList.filter(function (d) { return d.Code != fieldCode; });
            if (item.Column == 0)
                _this.ObjectFieldTextListColum1.push(new ObjectFieldText(field, fieldCode));
            else
                _this.ObjectFieldTextListColum2.push(new ObjectFieldText(field, fieldCode));
        });
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.LoadCompleted = function () {
        if (!this.IsLoadingTextDesign && !this.IsLoadingQuoteField) {
            this.CurrentSession.StopBusyIndicator();
            this.IsLoadPage = true;
        }
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.LoadQuoteTemplateHeaderFields = function () {
        var _this = this;
        this.quoteTemplateHeaderFieldExtendedPMService.GetQuoteTemplateHeaderFieldByQuoteTemplateId(this.QuoteTemplatePM.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.IsLoadingQuoteField = false;
            _this.LoadCompleted();
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.QuoteTemplateHeaderFieldPMList = pmResponse.Result;
                _this.FillObjectFieldTextColumLists(0, _this.QuoteTemplateHeaderFieldPMList, "Header");
                _this.FillObjectFieldTextColumLists(1, _this.QuoteTemplateHeaderFieldPMList, "Header");
            }
            _this.FillAllObjectFieldLists();
        });
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.GetFieldNameQuoteDetails = function (fieldname) {
        var Field = "";
        if (fieldname == "EXPIRATIONDAYS") {
            Field = "Expiration Days";
        }
        else if (fieldname == "EXPIRATIONDATE") {
            Field = "Expiration Date";
        }
        else if (fieldname == "QUOTENUMBER") {
            Field = "Quote Number";
        }
        if (fieldname == "SHIPPERNAME") {
            Field = "Shipper Name";
        }
        else if (fieldname == "SHIPPERADDRESS") {
            Field = "Shipper Address";
        }
        else if (fieldname == "SHIPPERCONTACT") {
            Field = "Shipper Contact";
        }
        else if (fieldname == "SHIPPERREFERENCES") {
            Field = "Shipper References";
        }
        else if (fieldname == "NOTIFYNAME") {
            Field = "Notify Name";
        }
        else if (fieldname == "NOTIFYADDRESS") {
            Field = "Notify Address";
        }
        else if (fieldname == "NOTIFYCONTACT") {
            Field = "Notify Contact";
        }
        if (fieldname == "CONSIGNEENAME") {
            Field = "Consignee Name";
        }
        else if (fieldname == "CONSIGNEEADDRESS") {
            Field = "Consignee Address";
        }
        else if (fieldname == "CONSIGNEECONTACT") {
            Field = "Consignee Contact";
        }
        else if (fieldname == "CONSIGNEEREFERENCES") {
            Field = "Consignee References";
        }
        if (fieldname == "CUSTOMERNAME") {
            Field = "Customer Name";
        }
        else if (fieldname == "CUSTOMERADDRESS") {
            Field = "Customer Address";
        }
        else if (fieldname == "CUSTOMERCONTACT") {
            Field = "Customer Contact";
        }
        else if (fieldname == "TRANSITTIME") {
            Field = "Transit Time";
        }
        else if (fieldname == "MOVETYPE") {
            Field = "Move Type";
        }
        else if (fieldname == "CUSTOMERREFERENCES") {
            Field = "Customer References";
        }
        else if (fieldname == "PICKUPFROM") {
            Field = "Pickup From";
        }
        else if (fieldname == "DELIVERYTO") {
            Field = "Delivery To";
        }
        else if (fieldname == "FROMPORT") {
            Field = "From Port";
        }
        else if (fieldname == "FROMLOCATION") {
            Field = "From Location";
        }
        else if (fieldname == "TOLOCATION") {
            Field = "To Location";
        }
        else if (fieldname == "TOPORT") {
            Field = "To Port";
        }
        else if (fieldname == "INCOTERMS") {
            Field = "Incoterms";
        }
        else if (fieldname == "SERVICE") {
            Field = "Service";
        }
        else if (fieldname == "SALESMAN") {
            Field = "SALESMAN";
        }
        if (fieldname == "DESCRIPTIONOFGOODS") {
            Field = "Description of goods";
        }
        else if (fieldname == "DANGEROUSGOODS") {
            Field = "Dangerous goods";
        }
        else if (fieldname == "AIRLINE") {
            Field = "Airline";
        }
        else if (fieldname == "SHIPINGLINE") {
            Field = "Shipingline";
        }
        else if (fieldname == "TRUCKER") {
            Field = "Trucker";
        }
        else if (fieldname == "CHARGEABLEWEIGHT") {
            Field = "Chargeable Weight";
        }
        else if (fieldname == "GROSSWEIGHT") {
            Field = "Gross Weight";
        }
        else if (fieldname == "VOLUME") {
            Field = "Volume";
        }
        else if (fieldname == "VOLUMETRICWEIGHT") {
            Field = "Volumetric Weight";
        }
        else if (fieldname == "NUMBEROFPACKAGES") {
            Field = "Number Of Packages";
        }
        else if (fieldname == "NUMBEROFCONTAINERS") {
            Field = "Number Of Containers";
        }
        return Field;
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.GetFieldNameQuoteHeader = function (fieldname) {
        var Field = "";
        if (fieldname == "QUOTENUMBER") {
            Field = "Quote Number";
        }
        else if (fieldname == "QUOTEDATE") {
            Field = "Quote Date";
        }
        else if (fieldname == "EXPIRATIONDATE") {
            Field = "Expiration Date";
        }
        else if (fieldname == "CUSTOMER") {
            Field = "Customer";
        }
        else if (fieldname == "ATTN") {
            Field = "ATTN";
        }
        return Field;
    };
    // End Prop setting 
    QuoteTemplateHeaderDetailsSettingComponent.prototype.SaveObjectFieldTextList = function () {
        this.SaveObjectFieldTextListColum(this.QuoteTemplateSectionTypeCode, 0);
        this.SaveObjectFieldTextListColum(this.QuoteTemplateSectionTypeCode, 1);
        if (this.QuoteTemplateSectionTypeCode == "QH") {
            if (this.QuoteTemplateHeaderFieldPMList) {
                var headerField = this.QuoteTemplateHeaderFieldPMList.filter(function (d) { return d.IsDirty; })[0];
                if (headerField)
                    this.IsSaveQuoteTemplateObjectField = true;
            }
        }
        else if (this.QuoteTemplateSectionTypeCode == "QD") {
            if (this.QuoteTemplateDetailsFieldPMList) {
                var detailsField = this.QuoteTemplateDetailsFieldPMList.filter(function (d) { return d.IsDirty; })[0];
                if (detailsField)
                    this.IsSaveQuoteTemplateObjectField = true;
            }
        }
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.SaveObjectFieldTextList();
        var quoteTemplateObjectFieldLists = null;
        this.ValidationErrorsList = [];
        var textDesignPmLists = null;
        var textCodeDataLists = null;
        if (this.QuoteTemplateTextDesignPMLists) {
            textDesignPmLists = this.QuoteTemplateTextDesignPMLists.filter(function (d) { return d.IsDirty == true; });
            if (textDesignPmLists.length > 0)
                this.IsSaveQuoteTemplateTextDesignRuning = true;
            if (this.TableDesignPM.IsDirty)
                this.IsSaveQuoteTemplateTableDesignRuning = true;
        }
        if (this.ItemsSource) {
            textCodeDataLists = this.ItemsSource.Collection.filter(function (d) { return d.EntityPM.IsDirty == true; });
            if (textCodeDataLists.length > 0)
                this.IsSaveQuoteTemplateTextCodeRuning = true;
        }
        if (this.IsSaveQuoteTemplateObjectField) {
            if (this.QuoteTemplateSectionTypeCode == "QH") {
                if (this.QuoteTemplateHeaderFieldPMList) {
                    quoteTemplateObjectFieldLists = this.QuoteTemplateHeaderFieldPMList.filter(function (d) { return d.IsDirty; });
                }
            }
            else if (this.QuoteTemplateSectionTypeCode == "QD") {
                if (this.QuoteTemplateDetailsFieldPMList) {
                    quoteTemplateObjectFieldLists = this.QuoteTemplateDetailsFieldPMList.filter(function (d) { return d.IsDirty; });
                }
            }
        }
        if (this.ObjectFieldTextListColum2 && this.ObjectFieldTextListColum2.length > 0) {
            if (this.QuoteTemplateSectionTypeCode == "QH") {
                this.QuoteTemplateSettingPM.HeaderSectionHasTwoColumns = true;
            }
            else {
                this.QuoteTemplateSettingPM.DetailsSectionHasTwoColumns = true;
            }
        }
        else {
            if (this.QuoteTemplateSectionTypeCode == "QH") {
                this.QuoteTemplateSettingPM.HeaderSectionHasTwoColumns = false;
            }
            else {
                this.QuoteTemplateSettingPM.DetailsSectionHasTwoColumns = false;
            }
        }
        if (this.IsSaveQuoteTemplateTextDesignRuning || this.IsSaveQuoteTemplateTableDesignRuning || this.IsSaveQuoteTemplateTextCodeRuning || this.IsSaveQuoteTemplateObjectField) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
            if (this.QuoteTemplateSettingPM.IsDirty) {
                this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(function (res) {
                    _this.QuoteTemplateSettingPM.IsDirty = false;
                    _this.SaveOthers(textDesignPmLists, textCodeDataLists, quoteTemplateObjectFieldLists);
                });
            }
            else
                this.SaveOthers(textDesignPmLists, textCodeDataLists, quoteTemplateObjectFieldLists);
        }
        else {
            if (this.QuoteTemplateSettingPM.IsDirty) {
                this.SaveQuoteTemplateSetting();
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CloseCurrentWindow();
            }
        }
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.SaveObjectFieldTextListColum = function (type, column) {
        var _this = this;
        var objectFieldTextListColum = column == 0 ? this.ObjectFieldTextListColum1 : this.ObjectFieldTextListColum2;
        var quoteTemplateFieldPMList = type == "QH" ? this.QuoteTemplateHeaderFieldPMList : this.QuoteTemplateDetailsFieldPMList;
        if (objectFieldTextListColum && objectFieldTextListColum.length > 0) {
            quoteTemplateFieldPMList.forEach(function (item) {
                var objectFieldText = objectFieldTextListColum.filter(function (d) { return d.Code == item.FieldCode; })[0];
                if (objectFieldText == null) {
                    if (item != null) {
                        if (item.Column == column && !item.IsEdit)
                            item.IsDelete = true;
                    }
                }
            });
            var i = -1;
            objectFieldTextListColum.forEach(function (newItem) {
                ++i;
                var oldEntity = quoteTemplateFieldPMList.filter(function (d) { return d.FieldCode == newItem.Code; })[0];
                if (oldEntity != null) {
                    oldEntity.Row = i;
                    oldEntity.Column = column;
                    if (oldEntity.IsDirty) {
                        oldEntity.IsEdit = true;
                        oldEntity.IsDelete = false;
                    }
                    ;
                }
                else {
                    var newQuoteTemplateFieldPM = type == "QH" ? new QuoteTemplateHeaderFieldPM_1.QuoteTemplateHeaderFieldPM() : new QuoteTemplateDetailsFieldPM_1.QuoteTemplateDetailsFieldPM();
                    newQuoteTemplateFieldPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    newQuoteTemplateFieldPM.Id = 1 + i.toString();
                    newQuoteTemplateFieldPM.Row = i;
                    newQuoteTemplateFieldPM.Column = column;
                    newQuoteTemplateFieldPM.FieldCode = newItem.Code;
                    newQuoteTemplateFieldPM.QuoteTemplateId = _this.QuoteTemplatePM.Id,
                        newQuoteTemplateFieldPM.IsAdd = true;
                    quoteTemplateFieldPMList.push(newQuoteTemplateFieldPM);
                }
            });
        }
        else {
            quoteTemplateFieldPMList.forEach(function (item) {
                if (item.Column == column && !item.IsEdit)
                    item.IsDelete = true;
            });
        }
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.SaveOthers = function (textDesignPmLists, textCodeDataLists, quoteTemplateObjectFieldLists) {
        if (this.IsSaveQuoteTemplateTextDesignRuning)
            this.SaveQuoteTemplateTextDesign(textDesignPmLists);
        if (this.IsSaveQuoteTemplateTableDesignRuning)
            this.SaveQuoteTemplateTableDesign();
        if (this.IsSaveQuoteTemplateTextCodeRuning)
            this.SaveQuoteTemplateTextCode(textCodeDataLists);
        if (this.IsSaveQuoteTemplateObjectField)
            this.SaveQuoteTemplateObjectFieldDB(quoteTemplateObjectFieldLists);
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.SaveQuoteTemplateObjectFieldDB = function (items) {
        var _this = this;
        items.forEach(function (item) { item.IsDirty = false; });
        if (this.QuoteTemplateSectionTypeCode == "QH") {
            this.quoteTemplateHeaderFieldExtendedPMService.updateHeaderFields(items).subscribe(function (res) {
                _this.IsSaveQuoteTemplateObjectField = false;
                _this.SaveCompleted();
            });
        }
        else {
            this.quoteTemplateDetailsFieldExtendedPMService.updateDetailsFields(items).subscribe(function (res) {
                _this.IsSaveQuoteTemplateObjectField = false;
                _this.SaveCompleted();
            });
        }
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.SaveQuoteTemplateTextDesign = function (items) {
        var _this = this;
        items.forEach(function (item) { item.IsDirty = false; });
        this.quoteTemplateTextDesignExtendedPMService.updateQuoteTemplateTextDesignPMs(items).subscribe(function (res) {
            _this.IsSaveQuoteTemplateTextDesignRuning = false;
            _this.SaveCompleted();
        });
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.SaveQuoteTemplateTableDesign = function () {
        var _this = this;
        this.quoteTemplateTableDesignPMService.update(this.TableDesignPM).subscribe(function (res) {
            _this.IsSaveQuoteTemplateTableDesignRuning = false;
            _this.SaveCompleted();
        });
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.SaveQuoteTemplateSetting = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
        this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(function (res) {
            _this.QuoteTemplateSettingPM.IsDirty = false;
            _this.SaveCompleted();
        });
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.SaveQuoteTemplateTextCode = function (items) {
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
    //Drag Drop ObjectFieldTextLis
    QuoteTemplateHeaderDetailsSettingComponent.prototype.OnObjectFieldTextListDragStart = function (event, item) {
        if (item) {
            event.dataTransfer.setData("Code", item.Code);
        }
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.allowDropObjectFieldTextList = function (event) {
        event.preventDefault();
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.DropItemToObjectFieldTextList = function (event) {
        var code = event.dataTransfer.getData("Code");
        var item = this.AllObjectFieldTextList.filter(function (d) { return d.Code == code; })[0];
        if (item) {
            this.DeleteItemFromLists(item);
            if (this.ObjectFieldTextList) {
                this.ObjectFieldTextList.push(item);
            }
        }
    };
    //Drag Drop ObjectFieldTextColumn1List
    QuoteTemplateHeaderDetailsSettingComponent.prototype.OnObjectFieldTextColumn1ListDragStart = function (event, item) {
        if (item) {
            event.dataTransfer.setData("Code", item.Code);
        }
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.allowDropObjectFieldTextColumn1List = function (event) {
        event.preventDefault();
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.DropItemToObjectFieldTextColumn1List = function (event) {
        var code = event.dataTransfer.getData("Code");
        var item = this.AllObjectFieldTextList.filter(function (d) { return d.Code == code; })[0];
        if (item) {
            this.DeleteItemFromLists(item);
            if (this.ObjectFieldTextListColum1) {
                this.ObjectFieldTextListColum1.push(item);
            }
        }
    };
    //Drag Drop ObjectFieldTextColumn2List
    QuoteTemplateHeaderDetailsSettingComponent.prototype.OnObjectFieldTextColumn2ListDragStart = function (event, item) {
        if (item) {
            event.dataTransfer.setData("Code", item.Code);
        }
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.allowDropObjectFieldTextColumn2List = function (event) {
        event.preventDefault();
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.DropItemToObjectFieldTextColumn2List = function (event) {
        var code = event.dataTransfer.getData("Code");
        var item = this.AllObjectFieldTextList.filter(function (d) { return d.Code == code; })[0];
        if (item) {
            this.DeleteItemFromLists(item);
            if (this.ObjectFieldTextListColum2) {
                this.ObjectFieldTextListColum2.push(item);
            }
        }
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.DeleteItemFromLists = function (item) {
        if (this.ObjectFieldTextList && this.ObjectFieldTextList.length > 0) {
            var item1 = this.ObjectFieldTextList.filter(function (d) { return d.Code == item.Code; })[0];
            if (item1) {
                var index = this.ObjectFieldTextList.indexOf(item1);
                if (index != -1)
                    this.ObjectFieldTextList.splice(index, 1);
            }
        }
        if (this.ObjectFieldTextListColum1 && this.ObjectFieldTextListColum1.length > 0) {
            var item2 = this.ObjectFieldTextListColum1.filter(function (d) { return d.Code == item.Code; })[0];
            if (item2) {
                var index = this.ObjectFieldTextListColum1.indexOf(item2);
                if (index != -1)
                    this.ObjectFieldTextListColum1.splice(index, 1);
            }
        }
        if (this.ObjectFieldTextListColum2 && this.ObjectFieldTextListColum2.length > 0) {
            var item3 = this.ObjectFieldTextListColum2.filter(function (d) { return d.Code == item.Code; })[0];
            if (item3) {
                var index = this.ObjectFieldTextListColum2.indexOf(item3);
                if (index != -1)
                    this.ObjectFieldTextListColum2.splice(index, 1);
            }
        }
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.SaveCompleted = function () {
        if (!this.IsSaveQuoteTemplateTextDesignRuning && !this.IsSaveQuoteTemplateTableDesignRuning && !this.IsSaveQuoteTemplateTextCodeRuning) {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CurrentWindow.Close("Refresh");
        }
    };
    QuoteTemplateHeaderDetailsSettingComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], QuoteTemplateHeaderDetailsSettingComponent.prototype, "viewContainerRef", void 0);
    QuoteTemplateHeaderDetailsSettingComponent = __decorate([
        core_1.Component({
            selector: 'QuoteTemplateHeaderDetailsSettingComponent',
            moduleId: module.id,
            templateUrl: './QuoteTemplateHeaderDetailsSettingComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], QuoteTemplateHeaderDetailsSettingComponent);
    return QuoteTemplateHeaderDetailsSettingComponent;
}(BaseComponent_1.BaseComponent));
exports.QuoteTemplateHeaderDetailsSettingComponent = QuoteTemplateHeaderDetailsSettingComponent;
var ObjectFieldText = /** @class */ (function () {
    function ObjectFieldText(name, code) {
        this.Name = name;
        this.Code = code;
    }
    return ObjectFieldText;
}());
//# sourceMappingURL=QuoteTemplateHeaderDetailsSettingComponent.js.map