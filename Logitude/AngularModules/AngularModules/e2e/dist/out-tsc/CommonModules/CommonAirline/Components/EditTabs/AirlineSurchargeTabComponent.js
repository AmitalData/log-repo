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
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var TarrifHeaderPM_1 = require("../../../../Common/EntityPMs/TarrifHeaderPM");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var DateTimeToDatePipe_1 = require("../../../../Controls/Pipes/DateTimeToDatePipe");
var Tools_1 = require("../../../../Infrastructure/Tools");
var AirlineSurchargeTabComponent = /** @class */ (function (_super) {
    __extends(AirlineSurchargeTabComponent, _super);
    function AirlineSurchargeTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Airline";
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.ShowActiveTarrifsString = "";
        _this.ShowAllTarrifsString = "";
        _this.AddSurchargeTarrif = "";
        _this.TarrifHeaderDate = "";
        _this.TarrifHeaderCreateDate = "";
        _this.TarrifHeaderFromLocation = "";
        _this.TarrifHeaderToLocation = "";
        _this.TarrifHeaderNotes = "";
        _this.ActiveTariff = "";
        _this.AllTariff = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Props 
        _this.showAllTarrifs = false;
        _this.showActiveTarrifs = true;
        _this._entityResourceService.getEntityResourceByTableName("TarrifHeader", 0).subscribe(function (response) {
            _this.EntityPM = entityArgs.EntityPM;
            _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
            _this.setLabels();
            _this.ObsList = [];
            _this.FillData();
            if (_this.CurrentSession == null) {
                _this.ActiveTariff = "Active_-1_-1";
                _this.AllTariff = "All_-1_-1";
            }
            else {
                var idIndex = _this.CurrentSession.GetNewId("RadioButton");
                _this.ActiveTariff = "Active_" + idIndex;
                _this.AllTariff = "All_" + idIndex;
            }
        });
        return _this;
    }
    AirlineSurchargeTabComponent.prototype.setLabels = function () {
        this.ShowActiveTarrifsString = TextCodeTranslator_1.TextCodeTranslator.Translate('TarrifHeader.O.ShowActiveTarrifs');
        this.ShowAllTarrifsString = TextCodeTranslator_1.TextCodeTranslator.Translate('TarrifHeader.O.ShowAllTarrifs');
        this.AddSurchargeTarrif = TextCodeTranslator_1.TextCodeTranslator.Translate('TarrifHeader.O.AddSurchargeTarrif');
        this.TarrifHeaderDate = TextCodeTranslator_1.TextCodeTranslator.Translate('TarrifHeader.O.Date');
        this.TarrifHeaderCreateDate = TextCodeTranslator_1.TextCodeTranslator.Translate('TarrifHeader.O.CreateDate');
        this.TarrifHeaderFromLocation = TextCodeTranslator_1.TextCodeTranslator.Translate('TarrifHeader.O.FromLocation');
        this.TarrifHeaderToLocation = TextCodeTranslator_1.TextCodeTranslator.Translate('TarrifHeader.O.ToLocation');
        this.TarrifHeaderNotes = TextCodeTranslator_1.TextCodeTranslator.Translate('TarrifHeader.O.Notes');
        var test = TextCodeTranslator_1.TextCodeTranslator.Translate('TarrifHeader.F.MeasurementId');
    };
    AirlineSurchargeTabComponent.prototype.OpenEdit = function (item) {
        var itemPM = item.EntityPM;
        var Detector;
        var itemViewModel = new TariffHeaderItem(itemPM, false, this, Detector);
        //  var itemviewmodel2: TariffHeaderItem = Object.assign({}, itemViewModel);
        //  itemviewmodel2.InActive = true;
        //  console.log(itemViewModel.InActive);
        //  console.log(itemviewmodel2.InActive);
        this.RunNewWindow(itemViewModel, TextCodeTranslator_1.TextCodeTranslator.Translate("TarrifHeader.O.EditSurchargeTarrif"));
    };
    AirlineSurchargeTabComponent.prototype.ngOnInit = function () {
    };
    AirlineSurchargeTabComponent.prototype.FillData = function () {
        this.LoadTarrifHeaders();
    };
    AirlineSurchargeTabComponent.prototype.LoadTarrifHeaders = function () {
        var _this = this;
        if (this.ObsList == null) {
            this.ObsList = new Array();
        }
        else {
            this.ObsList = [];
        }
        var list = new Array();
        var myService = new PartnersDomainService_1.PartnersDomainService();
        myService.GetTarrifHeadersByCardIdAndTypeCode(this.EntityPM.Id, "S", this.ShowAllTarrifs).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                list = myResponse.Result;
                list.forEach(function (item) {
                    // item.OldEntityPM = item;
                    var Detector;
                    var itemViewModel = new TariffHeaderItem(item, false, _this, Detector);
                    _this.ObsList.push(itemViewModel);
                });
            }
        });
    };
    AirlineSurchargeTabComponent.prototype.ClickAllTariff = function () {
        this.ShowAllTarrifs = true;
        this.ShowActiveTarrifs = false;
    };
    AirlineSurchargeTabComponent.prototype.ClickShowActiveTarrifs = function () {
        this.ShowAllTarrifs = false;
        this.ShowActiveTarrifs = true;
    };
    AirlineSurchargeTabComponent.prototype.CheckColor = function (item) {
        if (item.InActive)
            return '#FCDCDC';
        return "";
    };
    Object.defineProperty(AirlineSurchargeTabComponent.prototype, "ShowAllTarrifs", {
        get: function () { return this.showAllTarrifs; },
        set: function (newValue) {
            if (this.showAllTarrifs != newValue) {
                this.showAllTarrifs = newValue;
                this.FillData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineSurchargeTabComponent.prototype, "ShowActiveTarrifs", {
        get: function () { return this.showActiveTarrifs; },
        set: function (newValue) {
            if (this.showActiveTarrifs != newValue) {
                this.showActiveTarrifs = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AirlineSurchargeTabComponent.prototype.Clone = function (item) {
        //this.myCloner = new Cloner(item);
        //this.myCloner.AddField("FromDate");
        //this.myCloner.AddField("ToDate");
        //   this.myCloner.AddField("TarrifCharges");
        //this.myCloner.AddEntity(item.fatherComponent.EntityPM);
        // this.myCloner.AddEntity(item.EntityPM);
        //  this.myCloner.AddEntity(this.EntityPM);
        //  this.myCloner.AddEntity(item);
        //  this.myCloner.AddField("TarrifCharges");
    };
    AirlineSurchargeTabComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    //Commands
    AirlineSurchargeTabComponent.prototype.AddSurchargeTariff = function () {
        var todayDate = new Date();
        var itemPM = new TarrifHeaderPM_1.TarrifHeaderPM();
        itemPM.Tenant = this.TenantPM.Id;
        itemPM.TarrifTypeCode = "S";
        itemPM.CardId = this.EntityPM.Id;
        itemPM.CreateDate = todayDate;
        itemPM.InActive = false;
        itemPM.FromLocationCode = "A";
        itemPM.ToLocationCode = "A";
        itemPM.FromLocationString = "Anywhere";
        itemPM.ToLocationString = "Anywhere";
        var Detector;
        var itemViewModel = new TariffHeaderItem(itemPM, true, this, Detector);
        this.RunNewWindow(itemViewModel, TextCodeTranslator_1.TextCodeTranslator.Translate("TarrifHeader.O.AddSurchargeTarrif"));
    };
    //TarrifHeader.O.EditSurchargeTarrif
    AirlineSurchargeTabComponent.prototype.RunNewWindow = function (itemComponent, windowTitle) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("TarrifCharge", 0).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("TarrifHeader", 0).subscribe(function (response) {
                _this.Clone(itemComponent);
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                logitudeWindow.Title = windowTitle;
                logitudeWindow.IsFillScreen = true;
                logitudeWindow.DataContext = itemComponent;
                logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewTariffWindowClosed($event); });
                logitudeWindow.Show("./CommonModules/CommonAirline/Components/AddEdit/AddEditTarrifHeaderComponent");
            });
        });
    };
    AirlineSurchargeTabComponent.prototype.OnNewTariffWindowClosed = function (arg) {
        this.LoadTarrifHeaders();
    };
    AirlineSurchargeTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AirlineSurchargeTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AirlineSurchargeTabComponent);
    return AirlineSurchargeTabComponent;
}(BaseComponent_1.BaseComponent));
exports.AirlineSurchargeTabComponent = AirlineSurchargeTabComponent;
var TariffHeaderItem = /** @class */ (function (_super) {
    __extends(TariffHeaderItem, _super);
    function TariffHeaderItem(entityPM, isNew, fatherComponent, CD) {
        var _this = _super.call(this) || this;
        _this.isNew = isNew;
        _this.fatherComponent = fatherComponent;
        _this.CD = CD;
        _this.ObjectTableName = "TarrifHeader";
        _this.makeInActiveIsChecked = false;
        _this.savedFromDate = null;
        _this.savedToDate = null;
        _this.IsNew = false;
        _this.newDatesIsChecked = false;
        _this.FromToTypeList = new Array();
        var Tarrifi = new FromToType();
        Tarrifi.Code = "A";
        Tarrifi.Name = TextCodeTranslator_1.TextCodeTranslator.Translate('TarrifHeader.O.Anyware');
        Tarrifi.TypeIsEnabled = true;
        _this.FromToTypeList.push(Tarrifi);
        Tarrifi = new FromToType();
        Tarrifi.Code = "P";
        Tarrifi.Name = TextCodeTranslator_1.TextCodeTranslator.Translate('TarrifHeader.O.PortsList');
        _this.FromToTypeList.push(Tarrifi);
        Tarrifi = new FromToType();
        Tarrifi.Code = "C";
        Tarrifi.Name = TextCodeTranslator_1.TextCodeTranslator.Translate('TarrifHeader.O.PortsList');
        _this.FromToTypeList.push(Tarrifi);
        _this.EntityPM = entityPM;
        _this.IsNewEntity = isNew;
        _this.GetDatesText();
        _this.GetDateStatus();
        return _this;
    }
    Object.defineProperty(TariffHeaderItem.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) { this.EntityPM.InActive = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "TarrifCharges", {
        get: function () { return this.EntityPM.TarrifCharges; },
        set: function (value) { if (value != null)
            this.EntityPM.TarrifCharges = value; },
        enumerable: true,
        configurable: true
    });
    TariffHeaderItem.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, this.IsDatePickerEnabled);
        this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, this.IsDatePickerEnabled);
    };
    TariffHeaderItem.prototype.GetDatesText = function () {
        this.FromOldDateText = null;
        this.ToOldDateText = null;
        if (this.NewDatesIsChecked) {
            this.FromOldDateText = "Old Date: " + DateTimeToDatePipe_1.DateTimeToDatePipe.Pipe(this.savedFromDate);
            this.ToOldDateText = "Old Date: " + DateTimeToDatePipe_1.DateTimeToDatePipe.Pipe(this.savedToDate);
        }
    };
    Object.defineProperty(TariffHeaderItem.prototype, "MakeInActiveIsChecked", {
        get: function () {
            if (this.EntityPM.InActive)
                this.makeInActiveIsChecked = this.EntityPM.InActive;
            return this.makeInActiveIsChecked;
        },
        set: function (value) {
            this.makeInActiveIsChecked = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "MakeInActiveVisibility", {
        get: function () { return this.EntityPM.Id == null ? false : true; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "MakeInActiveIsEnabled", {
        get: function () { return !this.EntityPM.InActive; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "FromRedDotVisibility", {
        get: function () { return this.FromDate == null ? true : false; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "FromLocationCode", {
        get: function () { return this.EntityPM.FromLocationCode; },
        set: function (value) {
            if (this.EntityPM.FromLocationCode != value)
                this.EntityPM.FromLocationCode = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "FromLocationString", {
        get: function () { return this.EntityPM.FromLocationString; },
        set: function (value) { this.EntityPM.FromLocationString = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "ToLocationCode", {
        get: function () { return this.EntityPM.ToLocationCode; },
        set: function (value) { this.EntityPM.ToLocationCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "SelectedFromType", {
        get: function () {
            var _this = this;
            this.selectedFromType = this.FromToTypeList.filter(function (p) { return p.Code == _this.EntityPM.FromLocationCode; })[0];
            return this.selectedFromType;
        },
        set: function (value) { value != null ? this.FromLocationCode = value.Code : -1; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "SelectedToType", {
        get: function () {
            var _this = this;
            this.selectedToType = this.FromToTypeList.filter(function (p) { return p.Code == _this.EntityPM.ToLocationCode; })[0];
            return this.selectedToType;
        },
        set: function (value) { },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "AddFromPortIsEnabled", {
        get: function () { return !Tools_1.AppTool.IsNullOrEmpty(this.FromPortId); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "FromPortsAreaVisibility", {
        get: function () { return this.FromLocationCode == "P" ? true : false; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "ToPortsAreaVisibility", {
        get: function () { return this.ToPortsAreaVisibility == "P" ? true : false; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "FromPortId", {
        get: function () { return this.fromPortId; },
        set: function (value) { this.fromPortId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "ToPortId", {
        get: function () { return this.toPortId; },
        set: function (value) { this.toPortId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "IsDatePickerEnabled", {
        get: function () {
            var result = this.IsNewEntity || this.NewDatesIsChecked;
            return result;
        },
        enumerable: true,
        configurable: true
    });
    TariffHeaderItem.prototype.GetDateStatus = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FromDate) && !Tools_1.AppTool.IsNullOrEmpty(this.ToDate)) {
            var todayDateTicks = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateTicks;
            var fromDateTicks = Tools_1.DateTool.GetDateParts(this.FromDate).DateTicks;
            var toDateTicks = Tools_1.DateTool.GetDateParts(this.ToDate).DateTicks;
            if (todayDateTicks >= fromDateTicks && todayDateTicks <= toDateTicks) {
                this.DateStatus = "(" + TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Present") + ")";
                this.DateStatusColor = Tools_1.FontTool.Green;
            }
            else if (todayDateTicks < fromDateTicks && todayDateTicks < toDateTicks) {
                this.DateStatus = "(" + TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Future") + ")";
                this.DateStatusColor = Tools_1.FontTool.Magenta;
            }
            else if (todayDateTicks > fromDateTicks && todayDateTicks > toDateTicks) {
                this.DateStatus = "(" + TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Past") + ")";
                this.DateStatusColor = Tools_1.FontTool.Red;
            }
        }
    };
    Object.defineProperty(TariffHeaderItem.prototype, "NewDatesIsChecked", {
        get: function () { return this.newDatesIsChecked; },
        set: function (newValue) {
            if (this.newDatesIsChecked != newValue) {
                this.newDatesIsChecked = newValue;
                if (newValue) {
                    this.FromDate = null;
                    this.ToDate = null;
                }
                else {
                    this.FromDate = this.savedFromDate;
                    this.ToDate = this.savedToDate;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "ToLocationString", {
        get: function () { return this.EntityPM.ToLocationString; },
        set: function (value) { this.EntityPM.ToLocationString = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "FromDate", {
        get: function () {
            return this.EntityPM.FromDate;
        },
        set: function (value) {
            if (this.EntityPM.FromDate != value) {
                this.EntityPM.FromDate = value;
                //  this.CD.detectChanges();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) { this.EntityPM.Notes = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "TransitTimeNotes", {
        get: function () { return this.EntityPM.TransitTimeNotes; },
        set: function (value) { this.EntityPM.TransitTimeNotes = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "FromDateInDate", {
        get: function () {
            if (this.EntityPM.FromDate != null) {
                if (this.EntityPM.FromDate instanceof Date)
                    return this.EntityPM.FromDate;
                return Tools_1.DateTool.GetDateParts(this.EntityPM.FromDate).DateObject;
            }
            else
                return null;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "ToDateInDate", {
        get: function () {
            if (this.EntityPM.ToDate != null) {
                if (this.EntityPM.ToDate instanceof Date)
                    return this.EntityPM.ToDate;
                return Tools_1.DateTool.GetDateParts(this.EntityPM.FromDate).DateObject;
            }
            else
                return null;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "ToDate", {
        get: function () { return this.EntityPM.ToDate; },
        set: function (value) {
            if (this.EntityPM.ToDate != value) {
                this.EntityPM.ToDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffHeaderItem.prototype, "CreateDate", {
        get: function () { return this.EntityPM.CreateDate; },
        set: function (value) { this.EntityPM.CreateDate = value; },
        enumerable: true,
        configurable: true
    });
    return TariffHeaderItem;
}(BaseComponent_1.BaseComponent));
exports.TariffHeaderItem = TariffHeaderItem;
var FromToType = /** @class */ (function () {
    function FromToType() {
    }
    Object.defineProperty(FromToType.prototype, "Code", {
        get: function () { return this.code; },
        set: function (newValue) { this.code = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FromToType.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FromToType.prototype, "TypeIsEnabled", {
        get: function () { return this.typeIsEnabled; },
        set: function (newValue) { this.typeIsEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    return FromToType;
}());
exports.FromToType = FromToType;
//# sourceMappingURL=AirlineSurchargeTabComponent.js.map