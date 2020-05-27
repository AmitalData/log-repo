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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var TarrifChargePM_1 = require("../../../../Common/EntityPMs/TarrifChargePM");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ChargesTypeListService_1 = require("../../../../Common/Services/StandardLists/ChargesTypeListService");
var MeasurementListService_1 = require("../../../../Common/Services/StandardLists/MeasurementListService");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var TarrifHeaderPMService_1 = require("../../../../Common/Services/StandardPMs/TarrifHeaderPMService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var AddEditTarrifHeaderComponent = /** @class */ (function (_super) {
    __extends(AddEditTarrifHeaderComponent, _super);
    function AddEditTarrifHeaderComponent(CD) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.ObjectTableName = "TarrifHeader";
        _this.TarrifChargesObsList = [];
        _this.FromToTypeList = [];
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.NewDatesIsChecked = false;
        _this.DeleteTarrifChargeIsEnabled = false;
        _this.AddTarrifChargeString = "";
        _this.DeleteTarrifChargeString = "";
        _this.TariffRadioTo = "";
        _this.TariffRadio = "";
        _this.InActiveCheck = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ChargesTypeIdString = "";
        _this.MeasurementIdString = "";
        _this.CurrencyIdString = "";
        _this.UnitPriceString = "";
        _this.MinPriceString = "";
        _this.MaxPriceString = "";
        _this.OldFromDate = "";
        _this.OldToDate = "";
        _this.isNew = false;
        //Commands 
        _this.ValidationErrorsList = [];
        _this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        if (_this.CurrentSession == null) {
            _this.TariffRadioTo = "RadioTo_-1_-1";
            _this.TariffRadio = "RadioFrom_-1_-1";
            _this.InActiveCheck = "Check_-1";
        }
        else {
            var idIndex = _this.CurrentSession.GetNewId("RadioButton");
            _this.TariffRadioTo = "RadioTo_" + idIndex;
            _this.TariffRadio = "RadioFrom_" + idIndex;
            _this.InActiveCheck = "Check_" + idIndex;
        }
        return _this;
    }
    AddEditTarrifHeaderComponent.prototype.setLabels = function () {
        this.AddTarrifChargeString = TextCodeTranslator_1.TextCodeTranslator.Translate('TarrifCharge.B.AddTarrifCharge');
        this.DeleteTarrifChargeString = TextCodeTranslator_1.TextCodeTranslator.Translate('TarrifCharge.B.DeleteTarrifCharge');
        this.ChargesTypeIdString = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural('TarrifCharge.F.ChargesTypeId'); //ChargeType
        this.MeasurementIdString = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural('TarrifHeader.F.MeasurementId'); //MeasurementCode
        this.CurrencyIdString = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural('TarrifHeader.F.CurrencyId'); //CurrencyCode
        this.UnitPriceString = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural('TarrifHeader.F.UnitPrice'); //Unit Price
        this.MinPriceString = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural('TarrifHeader.F.MinPrice'); //Min
        this.MaxPriceString = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural('TarrifHeader.F.MaxPrice'); //Max
    };
    AddEditTarrifHeaderComponent.prototype.SetDataContext = function (dataContext) {
        var _this = this;
        this.EntityPM = dataContext.EntityPM;
        this.DataContext = dataContext;
        //this.SetLabels();
        this.setLabels();
        this.FillFromToTypeList();
        this.IsNew = dataContext.IsNewEntity;
        if (!this.IsNew) {
            this.DataContext.savedFromDate = this.DataContext.FromDate;
            this.DataContext.savedToDate = this.DataContext.ToDate;
            this.EntityPM.TarrifCharges.forEach(function (p) {
                var item = new TariffChargeItem(p, false, _this);
                _this.TarrifChargesObsList.push(item);
            });
        }
        this.DataContext.SetUIProperties();
    };
    AddEditTarrifHeaderComponent.prototype.NewDateIsClicked = function () {
        this.DataContext.NewDatesIsChecked = !this.NewDatesIsChecked;
        this.DataContext.SetUIProperties();
        this.OldFromDate = this.DataContext.FromOldDateText;
        this.OldToDate = this.DataContext.ToOldDateText;
        this.CD.detectChanges();
    };
    AddEditTarrifHeaderComponent.prototype.FillFromToTypeList = function () {
        if (this.FromToTypeList == null) {
            this.FromToTypeList = [];
        }
        var item1 = new FromToType();
        item1.Code = "A";
        item1.Name = TextCodeTranslator_1.TextCodeTranslator.Translate("TarrifHeader.O.Anyware");
        item1.TypeIsEnabled = true;
        this.FromToTypeList.push(item1);
        var item2 = new FromToType();
        item2.Code = "P";
        item2.Name = TextCodeTranslator_1.TextCodeTranslator.Translate("TarrifHeader.O.PortsList");
        item2.TypeIsEnabled = false;
        this.FromToTypeList.push(item2);
        var item3 = new FromToType();
        item3.Code = "C";
        item3.Name = TextCodeTranslator_1.TextCodeTranslator.Translate("TarrifHeader.O.CountriesList");
        item3.TypeIsEnabled = false;
        this.FromToTypeList.push(item3);
    };
    Object.defineProperty(AddEditTarrifHeaderComponent.prototype, "NewDatesIsCheckedIsEnabled", {
        get: function () { return !this.isNew; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTarrifHeaderComponent.prototype, "IsNew", {
        get: function () { return this.isNew; },
        set: function (value) { this.isNew = value; },
        enumerable: true,
        configurable: true
    });
    AddEditTarrifHeaderComponent.prototype.BuildData = function () {
        var _this = this;
        this.TarrifChargesObsList = [];
        this.EntityPM.TarrifCharges.forEach(function (item) {
            var model = new TariffChargeItem(item, false, _this);
            _this.TarrifChargesObsList.push(model);
        });
    };
    AddEditTarrifHeaderComponent.prototype.InActiveClick = function () {
        this.InActive = !this.InActive;
    };
    Object.defineProperty(AddEditTarrifHeaderComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) { this.inActive = value; },
        enumerable: true,
        configurable: true
    });
    //Tarrif Charges
    AddEditTarrifHeaderComponent.prototype.AddTarrifCharge = function () {
        var itemPM = new TarrifChargePM_1.TarrifChargePM(this.EntityPM);
        itemPM.Tenant = this.TenantPM.Id;
        itemPM.TarrifHeaderId = this.EntityPM.Id;
        var itemViewModel = new TariffChargeItem(itemPM, true, this);
        this.RunChargeWindow(itemViewModel, TextCodeTranslator_1.TextCodeTranslator.Translate("TarrifCharge.B.AddTarrifCharge"));
    };
    AddEditTarrifHeaderComponent.prototype.EditCharge = function (itemViewModel) {
        this.RunChargeWindow(itemViewModel, TextCodeTranslator_1.TextCodeTranslator.Translate("TarrifCharge.O.EditTarrifCharge"));
    };
    AddEditTarrifHeaderComponent.prototype.RunChargeWindow = function (itemComponent, windowTitle) {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./CommonModules/CommonAirline/Components/AddEdit/AddEditTariffChargeComponent');
    };
    AddEditTarrifHeaderComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    AddEditTarrifHeaderComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.DataContext.EntityPM, this.DataContext.ObjectTableName, errors);
        if (this.DataContext.FromDate > this.DataContext.ToDate) {
            errors.push("From Date should be less than To Date");
        }
        if (this.TarrifChargesObsList.length == 0) {
            errors.push("You must add at least one Charge");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity) {
                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Airline", "Tariff Added");
                this.DataContext.fatherComponent.ObsList.push(this.DataContext);
            }
            else
                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Airline", "Tariff Edited");
            this.SubmitChanges();
        }
    };
    AddEditTarrifHeaderComponent.prototype.SubmitChanges = function () {
        var _this = this;
        //this.EntityPM.TarrifCharges = [];
        //this.TarrifChargesObsList.forEach(p => {
        //    this.EntityPM.AddTarrifChargePM(this.MapFromTarrifChargeItemToPM(p));
        //});
        //console.log(this.EntityPM);
        if (this.inActive != null)
            this.EntityPM.InActive = this.inActive;
        else
            this.EntityPM.InActive = this.InActive;
        var myService = new TarrifHeaderPMService_1.TarrifHeaderPMService();
        if (this.DataContext.NewDatesIsChecked)
            this.IsNew = true;
        if (!this.IsNew) {
            myService.update(this.EntityPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }, function (error) {
                _this.CurrentSession.StopBusyIndicator();
                //var dd: Response = error;
                //console.log(dd.text);
            });
        }
        else {
            this.EntityPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            myService.insert(this.EntityPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }, function (error) {
                _this.CurrentSession.StopBusyIndicator();
                //var dd: Response = error;
                //console.log(dd.text);
            });
        }
    };
    AddEditTarrifHeaderComponent.prototype.DeleteTarrifCharge = function () {
        var _this = this;
        var tempArr = [];
        if (this.SelectedItem != null) {
            // this.EntityPM.RemoveTarrifChargePM();
            this.TarrifChargesObsList.forEach(function (p) {
                if (p != _this.SelectedItem)
                    tempArr.push(p);
                else {
                    _this.EntityPM.RemoveTarrifChargePM(_this.MapFromTarrifChargeItemToPM(p));
                }
            });
            this.TarrifChargesObsList = tempArr;
        }
        this.DeleteTarrifChargeIsEnabled = false;
    };
    AddEditTarrifHeaderComponent.prototype.MapFromTarrifChargeItemToPM = function (item) {
        return item.EntityPM;
    };
    AddEditTarrifHeaderComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditTarrifHeaderComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], AddEditTarrifHeaderComponent);
    return AddEditTarrifHeaderComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditTarrifHeaderComponent = AddEditTarrifHeaderComponent;
var FromToType = /** @class */ (function () {
    function FromToType() {
    }
    return FromToType;
}());
exports.FromToType = FromToType;
var QueryFilterItem = /** @class */ (function () {
    function QueryFilterItem() {
    }
    return QueryFilterItem;
}());
exports.QueryFilterItem = QueryFilterItem;
var TariffChargeItem = /** @class */ (function (_super) {
    __extends(TariffChargeItem, _super);
    function TariffChargeItem(entityPM, isNew, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.isNew = isNew;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "TarrifCharge";
        _this.ChargeTypesQueryFilters = [];
        //Cach Lists
        _this.ChargesTypeList = [];
        _this.MeasurementList = [];
        _this.CurrencyList = [];
        _this.EntityPM = entityPM;
        _this.TarrifHeaderPM = fatherComponent.EntityPM;
        _this.IsNewEntity = isNew;
        _this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        _this.LoadCachLists();
        _this.InserQueryFilterItem();
        return _this;
    }
    TariffChargeItem.prototype.LoadCachLists = function () {
        this.LoadChargesTypeListMethod();
        this.LoadMeasurementListMethod();
        this.LoadCurrencyListMethod();
    };
    TariffChargeItem.prototype.InserQueryFilterItem = function () {
        if (this.TarrifHeaderPM.TarrifTypeCode == "S") {
            var query = new QueryFilterItem();
            query.FieldName = "ChargesGroupCode";
            query.FieldValue = "SCH";
            query.Operator = "Equals";
            if (this.ChargeTypesQueryFilters == null) {
                this.ChargeTypesQueryFilters = [];
            }
            this.ChargeTypesQueryFilters.push(query);
        }
    };
    TariffChargeItem.prototype.LoadChargesTypeListMethod = function () {
        var _this = this;
        var myService = new ChargesTypeListService_1.ChargesTypeListService();
        myService.getAll().subscribe(function (myResult) {
            _this.ChargesTypeList = myResult.Result;
        });
    };
    TariffChargeItem.prototype.LoadMeasurementListMethod = function () {
        var _this = this;
        var myService = new MeasurementListService_1.MeasurementListService();
        myService.getAll().subscribe(function (myResult) {
            _this.MeasurementList = myResult.Result;
        });
    };
    TariffChargeItem.prototype.LoadCurrencyListMethod = function () {
        var _this = this;
        var myService = new CurrencyListService_1.CurrencyListService();
        myService.getAll().subscribe(function (myResult) {
            _this.CurrencyList = myResult.Result;
        });
    };
    Object.defineProperty(TariffChargeItem.prototype, "ChargesTypeId", {
        //Props 
        get: function () { return this.EntityPM.ChargesTypeId; },
        set: function (value) {
            if (this.EntityPM.ChargesTypeId != value) {
                this.EntityPM.ChargesTypeId = value;
                this.GetChargesTypeData();
            }
        },
        enumerable: true,
        configurable: true
    });
    TariffChargeItem.prototype.GetChargesTypeData = function () {
        var _this = this;
        if (this.EntityPM.ChargesTypeId == null) {
            this.ChargesGroupCode = null;
            this.ChargesTypeCode = null;
            this.ChargesTypeName = null;
            this.MeasurementId = null;
            this.CurrencyId = null;
        }
        else {
            var list = this.ChargesTypeList.filter(function (d) { return d.Id == _this.EntityPM.ChargesTypeId; })[0];
            if (list != null) {
                this.ChargesGroupCode = list.ChargesGroupCode;
                this.ChargesTypeCode = list.Code;
                this.ChargesTypeName = list.EnglishName;
                this.MeasurementId = list.MeasurementId;
                if (list.ChargesGroupCode == "FRT" || list.ChargesGroupCode == "SCH") {
                    this.CurrencyId = this.TenantPM.FreightCurrencyId;
                }
                else {
                    this.CurrencyId = this.TenantPM.OtherChargesCurrencyId;
                }
            }
        }
    };
    Object.defineProperty(TariffChargeItem.prototype, "ChargesGroupCode", {
        get: function () { return this.chargesGroupCode; },
        set: function (value) {
            if (this.chargesGroupCode != value) {
                this.chargesGroupCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffChargeItem.prototype, "ChargesTypeCode", {
        get: function () { return this.EntityPM.ChargesTypeCode; },
        set: function (value) {
            if (this.EntityPM.ChargesTypeCode != value) {
                this.EntityPM.ChargesTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffChargeItem.prototype, "ChargesTypeName", {
        get: function () { return this.EntityPM.ChargesTypeName; },
        set: function (value) {
            if (this.EntityPM.ChargesTypeName != value) {
                this.EntityPM.ChargesTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffChargeItem.prototype, "ChargeType", {
        get: function () {
            var str = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ChargesTypeId)) {
                str = "(" + this.ChargesTypeCode + ") " + this.ChargesTypeName;
            }
            return str;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffChargeItem.prototype, "MeasurementId", {
        get: function () { return this.EntityPM.MeasurementId; },
        set: function (value) {
            if (this.EntityPM.MeasurementId != value) {
                this.EntityPM.MeasurementId = value;
                this.GetMeasurementData();
            }
        },
        enumerable: true,
        configurable: true
    });
    TariffChargeItem.prototype.GetMeasurementData = function () {
        var _this = this;
        if (this.EntityPM.MeasurementId == null) {
            this.MeasurementCode = null;
        }
        else {
            var list = this.MeasurementList.filter(function (d) { return d.Id == _this.EntityPM.MeasurementId; })[0];
            if (list != null) {
                this.MeasurementCode = list.Code;
            }
        }
    };
    Object.defineProperty(TariffChargeItem.prototype, "CurrencyId", {
        get: function () { return this.EntityPM.CurrencyId; },
        set: function (value) {
            if (this.EntityPM.CurrencyId != value) {
                this.EntityPM.CurrencyId = value;
                this.GetCurrencyData();
            }
        },
        enumerable: true,
        configurable: true
    });
    TariffChargeItem.prototype.GetCurrencyData = function () {
        var _this = this;
        if (this.EntityPM.CurrencyId == null) {
            this.CurrencyCode = null;
        }
        else {
            var list = this.CurrencyList.filter(function (d) { return d.Id == _this.EntityPM.CurrencyId; })[0];
            if (list != null) {
                this.CurrencyCode = list.Code;
            }
        }
    };
    Object.defineProperty(TariffChargeItem.prototype, "CurrencyCode", {
        get: function () { return this.EntityPM.CurrencyCode; },
        set: function (value) {
            if (this.EntityPM.CurrencyCode != value) {
                this.EntityPM.CurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffChargeItem.prototype, "MeasurementCode", {
        get: function () { return this.EntityPM.MeasurementCode; },
        set: function (value) {
            if (this.EntityPM.MeasurementCode != value) {
                this.EntityPM.MeasurementCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffChargeItem.prototype, "UnitPrice", {
        get: function () { return this.EntityPM.UnitPrice; },
        set: function (value) {
            if (this.EntityPM.UnitPrice != value) {
                if (value != null)
                    this.EntityPM.UnitPrice = +value.toFixed(2);
                else
                    this.EntityPM.UnitPrice = +value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffChargeItem.prototype, "MaxPrice", {
        get: function () { return this.EntityPM.MaxPrice; },
        set: function (value) {
            if (this.EntityPM.MaxPrice != value) {
                if (value != null) {
                    this.EntityPM.MaxPrice = +value.toFixed(2);
                }
                else
                    this.EntityPM.MaxPrice = +value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffChargeItem.prototype, "MinPrice", {
        get: function () { return this.EntityPM.MinPrice; },
        set: function (value) {
            if (this.EntityPM.MinPrice != value) {
                if (value != null)
                    this.EntityPM.MinPrice = +value.toFixed(2);
                else
                    this.EntityPM.MinPrice = +value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return TariffChargeItem;
}(BaseComponent_1.BaseComponent));
exports.TariffChargeItem = TariffChargeItem;
//# sourceMappingURL=AddEditTarrifHeaderComponent.js.map