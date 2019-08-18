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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BusinessHourPM_1 = require("../../../../Infrastructure/EntityPMs/BusinessHourPM");
var BusinessHourPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/BusinessHourPMService");
var BusinessHoursHolidayPM_1 = require("../../../../Infrastructure/EntityPMs/BusinessHoursHolidayPM");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var InfrastructureDomainService_1 = require("../../../../Infrastructure/Services/InfrastructureDomainService");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var NewBusinessHourAndHolidaysComponent = /** @class */ (function (_super) {
    __extends(NewBusinessHourAndHolidaysComponent, _super);
    function NewBusinessHourAndHolidaysComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "BusinessHour";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.IsVisible = false;
        _this.IsNew = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DefinedHours = true;
        _this.mondayTotalWorkHours = "0";
        _this.tuesdayTotalWorkHours = "0";
        _this.wednesdayTotalWorkHours = "0";
        _this.thursdayTotalWorkHours = "0";
        _this.fridayTotalWorkHours = "0";
        _this.saturdayTotalWorkHours = "0";
        _this.sundayTotalWorkHours = "0";
        _this.entityPM = new BusinessHourPM_1.BusinessHourPM();
        _this.HolidaysDataList = [];
        _this.getBusinssHourEntityMethod();
        return _this;
    }
    //Load Business Hour Entity
    NewBusinessHourAndHolidaysComponent.prototype.getBusinssHourEntityMethod = function () {
        var _this = this;
        var service = new InfrastructureDomainService_1.InfrastructureDomainService();
        service.GetBusinessHourBM().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.entityPM = myResponse.Result;
                if (_this.entityPM != null) {
                    _this.IsNew = false;
                    _this.RefreshData();
                }
                else {
                    _this.IsNew = true;
                    _this.entityPM = new BusinessHourPM_1.BusinessHourPM();
                    var todayDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                    _this.entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    _this.entityPM.CreateDate = todayDateTime;
                    _this.entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                    _this.entityPM.UpdateDate = todayDateTime;
                    _this.entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                }
                _this.SetEnabled();
                _this.IsVisible = true;
            }
        });
    };
    NewBusinessHourAndHolidaysComponent.prototype.RefreshData = function () {
        this.MondayTotalWorkHours = this.timeDifferencecalCulationMethod(this.MondayToHourDate, this.MondayFromHourDate, this.IsMondayEnabeled);
        this.TuesdayTotalWorkHours = this.timeDifferencecalCulationMethod(this.TuesdayToHourDate, this.TuesdayFromHourDate, this.IsTuesdayEnabeled);
        this.WednesdayTotalWorkHours = this.timeDifferencecalCulationMethod(this.WednesdayToHourDate, this.WednesdayFromHourDate, this.IsWednesdayEnabeled);
        this.ThursdayTotalWorkHours = this.timeDifferencecalCulationMethod(this.ThursdayToHourDate, this.ThursdayFromHourDate, this.IsThursdayEnabeled);
        this.FridayTotalWorkHours = this.timeDifferencecalCulationMethod(this.FridayToHourDate, this.FridayFromHourDate, this.IsFridayEnabeled);
        this.SaturdayTotalWorkHours = this.timeDifferencecalCulationMethod(this.SaturdayToHourDate, this.SaturdayFromHourDate, this.IsSaturdayEnabeled);
        this.SundayTotalWorkHours = this.timeDifferencecalCulationMethod(this.SundayToHourDate, this.SundayFromHourDate, this.IsSundayEnabeled);
        this.getTotalWorkHours();
        this.fillHolidays();
        if (this.entityPM.Is247) {
            this.DefinedHours = false;
        }
    };
    NewBusinessHourAndHolidaysComponent.prototype.SetEnabled = function () {
        this.UIProperties.SetEnabled("IsMondayEnabeled", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("IsTuesdayEnabeled", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("IsWednesdayEnabeled", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("IsThursdayEnabeled", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("IsFridayEnabeled", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("IsSaturdayEnabeled", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("IsSundayEnabeled", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("MondayFromHour", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("TuesdayFromHour", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("WednesdayFromHour", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("ThursdayFromHour", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("FridayFromHour", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("SaturdayFromHour", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("SundayFromHour", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("MondayToHour", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("TuesdayToHour", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("WednesdayToHour", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("ThursdayToHour", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("FridayToHour", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("SaturdayToHour", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("SundayToHour", this.ObjectTableName, !this.Is247);
    };
    //Fill Holidays List
    NewBusinessHourAndHolidaysComponent.prototype.fillHolidays = function () {
        var _this = this;
        if (this.entityPM != null && this.entityPM.BusinessHoursHolidays.length > 0) {
            this.HolidaysDataList = [];
            this.entityPM.BusinessHoursHolidays.forEach(function (item) {
                _this.HolidaysDataList.push(new BusinessHourHolidayArgs(_this.entityPM, item, _this, false, false));
            });
        }
    };
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "MondayFromHourDate", {
        // Properties
        get: function () { return this.entityPM.MondayFromHourDate; },
        set: function (value) {
            if (this.entityPM.MondayFromHourDate != value) {
                this.entityPM.MondayFromHourDate = value;
                this.getTotalWorkHours();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "TuesdayFromHourDate", {
        get: function () { return this.entityPM.TuesdayFromHourDate; },
        set: function (value) {
            if (this.entityPM.TuesdayFromHourDate != value) {
                this.entityPM.TuesdayFromHourDate = value;
                this.getTotalWorkHours();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "WednesdayFromHourDate", {
        get: function () { return this.entityPM.WednesdayFromHourDate; },
        set: function (value) {
            if (this.entityPM.WednesdayFromHourDate != value) {
                this.entityPM.WednesdayFromHourDate = value;
                this.getTotalWorkHours();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "ThursdayFromHourDate", {
        get: function () { return this.entityPM.ThursdayFromHourDate; },
        set: function (value) {
            if (this.entityPM.ThursdayFromHourDate != value) {
                this.entityPM.ThursdayFromHourDate = value;
                this.getTotalWorkHours();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "FridayFromHourDate", {
        get: function () { return this.entityPM.FridayFromHourDate; },
        set: function (value) {
            if (this.entityPM.FridayFromHourDate != value) {
                this.entityPM.FridayFromHourDate = value;
                this.getTotalWorkHours();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "SaturdayFromHourDate", {
        get: function () { return this.entityPM.SaturdayFromHourDate; },
        set: function (value) {
            if (this.entityPM.SaturdayFromHourDate != value) {
                this.entityPM.SaturdayFromHourDate = value;
                this.getTotalWorkHours();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "SundayFromHourDate", {
        get: function () { return this.entityPM.SundayFromHourDate; },
        set: function (value) {
            if (this.entityPM.SundayFromHourDate != value) {
                this.entityPM.SundayFromHourDate = value;
                this.getTotalWorkHours();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "MondayToHourDate", {
        get: function () { return this.entityPM.MondayToHourDate; },
        set: function (value) {
            if (this.entityPM.MondayToHourDate != value) {
                this.entityPM.MondayToHourDate = value;
                this.getTotalWorkHours();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "TuesdayToHourDate", {
        get: function () { return this.entityPM.TuesdayToHourDate; },
        set: function (value) {
            if (this.entityPM.TuesdayToHourDate != value) {
                this.entityPM.TuesdayToHourDate = value;
                this.getTotalWorkHours();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "WednesdayToHourDate", {
        get: function () { return this.entityPM.WednesdayToHourDate; },
        set: function (value) {
            if (this.entityPM.WednesdayToHourDate != value) {
                this.entityPM.WednesdayToHourDate = value;
                this.getTotalWorkHours();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "ThursdayToHourDate", {
        get: function () { return this.entityPM.ThursdayToHourDate; },
        set: function (value) {
            if (this.entityPM.ThursdayToHourDate != value) {
                this.entityPM.ThursdayToHourDate = value;
                this.getTotalWorkHours();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "FridayToHourDate", {
        get: function () { return this.entityPM.FridayToHourDate; },
        set: function (value) {
            if (this.entityPM.FridayToHourDate != value) {
                this.entityPM.FridayToHourDate = value;
                this.getTotalWorkHours();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "SaturdayToHourDate", {
        get: function () { return this.entityPM.SaturdayToHourDate; },
        set: function (value) {
            if (this.entityPM.SaturdayToHourDate != value) {
                this.entityPM.SaturdayToHourDate = value;
                this.getTotalWorkHours();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "SundayToHourDate", {
        get: function () { return this.entityPM.SundayToHourDate; },
        set: function (value) {
            if (this.entityPM.SundayToHourDate != value) {
                this.entityPM.SundayToHourDate = value;
                this.getTotalWorkHours();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "Name", {
        get: function () { return this.entityPM.Name; },
        set: function (value) {
            if (this.entityPM.Name != value) {
                this.entityPM.Name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "Description", {
        get: function () { return this.entityPM.Description; },
        set: function (value) {
            if (this.entityPM.Description != value) {
                this.entityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "IsMondayEnabeled", {
        get: function () { return this.entityPM.IsMondayEnabeled; },
        set: function (value) {
            if (this.entityPM.IsMondayEnabeled != value) {
                this.entityPM.IsMondayEnabeled = value;
                this.CheckBoxProcessing();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "IsTuesdayEnabeled", {
        get: function () { return this.entityPM.IsTuesdayEnabeled; },
        set: function (value) {
            if (this.entityPM.IsTuesdayEnabeled != value) {
                this.entityPM.IsTuesdayEnabeled = value;
                this.CheckBoxProcessing();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "IsWednesdayEnabeled", {
        get: function () { return this.entityPM.IsWednesdayEnabeled; },
        set: function (value) {
            if (this.entityPM.IsWednesdayEnabeled != value) {
                this.entityPM.IsWednesdayEnabeled = value;
                this.CheckBoxProcessing();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "IsThursdayEnabeled", {
        get: function () { return this.entityPM.IsThursdayEnabeled; },
        set: function (value) {
            if (this.entityPM.IsThursdayEnabeled != value) {
                this.entityPM.IsThursdayEnabeled = value;
                this.CheckBoxProcessing();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "IsFridayEnabeled", {
        get: function () { return this.entityPM.IsFridayEnabeled; },
        set: function (value) {
            if (this.entityPM.IsFridayEnabeled != value) {
                this.entityPM.IsFridayEnabeled = value;
                this.CheckBoxProcessing();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "IsSaturdayEnabeled", {
        get: function () { return this.entityPM.IsSaturdayEnabeled; },
        set: function (value) {
            if (this.entityPM.IsSaturdayEnabeled != value) {
                this.entityPM.IsSaturdayEnabeled = value;
                this.CheckBoxProcessing();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "IsSundayEnabeled", {
        get: function () { return this.entityPM.IsSundayEnabeled; },
        set: function (value) {
            if (this.entityPM.IsSundayEnabeled != value) {
                this.entityPM.IsSundayEnabeled = value;
                this.CheckBoxProcessing();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "MondayHourEnabled", {
        get: function () {
            return this.IsMondayEnabeled;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "TuesdayHourEnabled", {
        get: function () {
            return this.IsTuesdayEnabeled;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "WednesdayHourEnabled", {
        get: function () {
            return this.IsWednesdayEnabeled;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "ThursdayHourEnabled", {
        get: function () {
            return this.IsThursdayEnabeled;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "FridayHourEnabled", {
        get: function () {
            return this.IsFridayEnabeled;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "SaturdayHourEnabled", {
        get: function () {
            return this.IsSaturdayEnabeled;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "SundayHourEnabled", {
        get: function () {
            return this.IsSundayEnabeled;
        },
        enumerable: true,
        configurable: true
    });
    NewBusinessHourAndHolidaysComponent.prototype.getDate = function (date) {
        if (date) {
            var h = date.split(':')[0];
            var m = date.split(':')[1];
            var s = date.split(':')[2];
            var myDate = Tools_1.DateTool.GetDateFromDate(new Date());
            myDate.setUTCHours(0);
            myDate.setUTCMinutes(0);
            myDate.setUTCSeconds(0);
            myDate.setUTCMilliseconds(0);
            myDate.setUTCHours(h);
            myDate.setUTCMinutes(m);
            myDate.setUTCSeconds(s);
            return Tools_1.DateTool.GetDateFromDate(myDate);
        }
    };
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "Is247", {
        get: function () { return this.entityPM.Is247; },
        set: function (value) {
            if (this.entityPM.Is247 != value) {
                this.entityPM.Is247 = value;
                this.DefinedHours = !value;
                this.SetEnabled();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "MondayTotalWorkHours", {
        get: function () {
            return this.mondayTotalWorkHours;
        },
        set: function (value) {
            this.mondayTotalWorkHours = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "TuesdayTotalWorkHours", {
        get: function () {
            return this.tuesdayTotalWorkHours;
        },
        set: function (value) {
            this.tuesdayTotalWorkHours = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "WednesdayTotalWorkHours", {
        get: function () {
            return this.wednesdayTotalWorkHours;
        },
        set: function (value) {
            this.wednesdayTotalWorkHours = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "ThursdayTotalWorkHours", {
        get: function () {
            return this.thursdayTotalWorkHours;
        },
        set: function (value) {
            this.thursdayTotalWorkHours = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "FridayTotalWorkHours", {
        get: function () {
            return this.fridayTotalWorkHours;
        },
        set: function (value) {
            this.fridayTotalWorkHours = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "SaturdayTotalWorkHours", {
        get: function () {
            return this.saturdayTotalWorkHours;
        },
        set: function (value) {
            this.saturdayTotalWorkHours = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBusinessHourAndHolidaysComponent.prototype, "SundayTotalWorkHours", {
        get: function () {
            return this.sundayTotalWorkHours;
        },
        set: function (value) {
            this.sundayTotalWorkHours = value;
        },
        enumerable: true,
        configurable: true
    });
    NewBusinessHourAndHolidaysComponent.prototype.getTotalWorkHours = function () {
        this.MondayTotalWorkHours = this.timeDifferencecalCulationMethod(this.MondayToHourDate, this.MondayFromHourDate, this.IsMondayEnabeled);
        this.TuesdayTotalWorkHours = this.timeDifferencecalCulationMethod(this.TuesdayToHourDate, this.TuesdayFromHourDate, this.IsTuesdayEnabeled);
        this.WednesdayTotalWorkHours = this.timeDifferencecalCulationMethod(this.WednesdayToHourDate, this.WednesdayFromHourDate, this.IsWednesdayEnabeled);
        this.ThursdayTotalWorkHours = this.timeDifferencecalCulationMethod(this.ThursdayToHourDate, this.ThursdayFromHourDate, this.IsThursdayEnabeled);
        this.FridayTotalWorkHours = this.timeDifferencecalCulationMethod(this.FridayToHourDate, this.FridayFromHourDate, this.IsFridayEnabeled);
        this.SaturdayTotalWorkHours = this.timeDifferencecalCulationMethod(this.SaturdayToHourDate, this.SaturdayFromHourDate, this.IsSaturdayEnabeled);
        this.SundayTotalWorkHours = this.timeDifferencecalCulationMethod(this.SundayToHourDate, this.SundayFromHourDate, this.IsSundayEnabeled);
        var saturdayTotalValue = parseFloat(this.SaturdayTotalWorkHours.split(':')[0]);
        var mondayTotalValue = parseFloat(this.MondayTotalWorkHours.split(':')[0]);
        var tuesdayTotalValue = parseFloat(this.TuesdayTotalWorkHours.split(':')[0]);
        var wednesdayTotalValue = parseFloat(this.WednesdayTotalWorkHours.split(':')[0]);
        var thursdayTotalValue = parseFloat(this.ThursdayTotalWorkHours.split(':')[0]);
        var fridayTotalValue = parseFloat(this.FridayTotalWorkHours.split(':')[0]);
        var sundayTotalValue = parseFloat(this.SundayTotalWorkHours.split(':')[0]);
        var tempValue = 0;
        tempValue =
            (mondayTotalValue != null ? mondayTotalValue : 0) +
                (tuesdayTotalValue != null ? tuesdayTotalValue : 0) +
                (wednesdayTotalValue != null ? wednesdayTotalValue : 0) +
                (thursdayTotalValue != null ? thursdayTotalValue : 0) +
                (fridayTotalValue != null ? fridayTotalValue : 0) +
                (saturdayTotalValue != null ? saturdayTotalValue : 0) +
                (sundayTotalValue != null ? sundayTotalValue : 0);
        this.AllTotalWorkHours = tempValue.toString();
    };
    NewBusinessHourAndHolidaysComponent.prototype.CheckBoxProcessing = function () {
        if (!this.IsMondayEnabeled) {
            this.MondayToHourDate = Tools_1.DateTool.TruncateTime(this.MondayToHourDate);
            this.MondayFromHourDate = Tools_1.DateTool.TruncateTime(this.MondayFromHourDate);
        }
        if (!this.IsSaturdayEnabeled) {
            this.SaturdayToHourDate = Tools_1.DateTool.TruncateTime(this.SaturdayToHourDate);
            this.SaturdayFromHourDate = Tools_1.DateTool.TruncateTime(this.SaturdayFromHourDate);
        }
        if (!this.IsTuesdayEnabeled) {
            this.TuesdayToHourDate = Tools_1.DateTool.TruncateTime(this.TuesdayToHourDate);
            this.TuesdayFromHourDate = Tools_1.DateTool.TruncateTime(this.TuesdayFromHourDate);
        }
        if (!this.IsWednesdayEnabeled) {
            this.WednesdayToHourDate = Tools_1.DateTool.TruncateTime(this.WednesdayToHourDate);
            this.WednesdayFromHourDate = Tools_1.DateTool.TruncateTime(this.WednesdayFromHourDate);
        }
        if (!this.IsThursdayEnabeled) {
            this.ThursdayToHourDate = Tools_1.DateTool.TruncateTime(this.ThursdayToHourDate);
            this.ThursdayFromHourDate = Tools_1.DateTool.TruncateTime(this.ThursdayFromHourDate);
        }
        if (!this.IsFridayEnabeled) {
            this.FridayToHourDate = Tools_1.DateTool.TruncateTime(this.FridayToHourDate);
            this.FridayFromHourDate = Tools_1.DateTool.TruncateTime(this.FridayFromHourDate);
        }
        if (!this.IsSundayEnabeled) {
            this.SundayToHourDate = Tools_1.DateTool.TruncateTime(this.SundayToHourDate);
            this.SundayFromHourDate = Tools_1.DateTool.TruncateTime(this.SundayFromHourDate);
        }
        this.getTotalWorkHours();
    };
    NewBusinessHourAndHolidaysComponent.prototype.timeDifferencecalCulationMethod = function (toHour, fromHour, isEnable) {
        var time = "0";
        var dateDiff;
        if (isEnable) {
            var d1 = Tools_1.DateTool.GetDateFormats(new Date(toHour.toString())).DateParts.DateObject;
            var d2 = Tools_1.DateTool.GetDateFormats(new Date(fromHour.toString())).DateParts.DateObject;
            var timeDiff = Math.abs(Tools_1.DateTool.GetDateFromDate(toHour).getTime() - Tools_1.DateTool.GetDateFromDate(fromHour).getTime());
            var hours = Math.floor(timeDiff / (1000 * 3600));
            timeDiff -= hours * 1000 * 60 * 60;
            var minutes = Math.floor(timeDiff / 1000 / 60);
            time = Tools_1.AppTool.PadLeft("" + hours, 2, '0') + ":" + Tools_1.AppTool.PadLeft("" + minutes, 2, '0');
        }
        return time;
    };
    // Commands
    NewBusinessHourAndHolidaysComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewBusinessHourAndHolidaysComponent.prototype.OkButtonClicked = function () {
        this.UpadteDates();
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.Name)) {
            errors.push("Name field is required");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsNew) {
                this.InsertBusinesHour();
            }
            else {
                this.UpdateBusinesHour();
            }
        }
    };
    NewBusinessHourAndHolidaysComponent.prototype.InsertBusinesHour = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new BusinessHourPMService_1.BusinessHourPMService();
        service.insert(this.entityPM).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit('ok');
            }
            else {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    };
    NewBusinessHourAndHolidaysComponent.prototype.UpdateBusinesHour = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new BusinessHourPMService_1.BusinessHourPMService();
        service.update(this.entityPM).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit('ok');
            }
            else {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    };
    NewBusinessHourAndHolidaysComponent.prototype.UpadteDates = function () {
        this.entityPM.MondayFromHour = Tools_1.DateTool.GetDateFromDate(this.MondayFromHourDate).getUTCHours() + ":" + Tools_1.DateTool.GetDateFromDate(this.MondayFromHourDate).getUTCMinutes() + ":" + Tools_1.DateTool.GetDateFromDate(this.MondayFromHourDate).getUTCSeconds();
        this.entityPM.MondayToHour = Tools_1.DateTool.GetDateFromDate(this.MondayToHourDate).getUTCHours() + ":" + Tools_1.DateTool.GetDateFromDate(this.MondayToHourDate).getUTCMinutes() + ":" + Tools_1.DateTool.GetDateFromDate(this.MondayToHourDate).getUTCSeconds();
        this.entityPM.TuesdayFromHour = Tools_1.DateTool.GetDateFromDate(this.TuesdayFromHourDate).getUTCHours() + ":" + Tools_1.DateTool.GetDateFromDate(this.TuesdayFromHourDate).getUTCMinutes() + ":" + Tools_1.DateTool.GetDateFromDate(this.TuesdayFromHourDate).getUTCSeconds();
        this.entityPM.TuesdayToHour = Tools_1.DateTool.GetDateFromDate(this.TuesdayToHourDate).getUTCHours() + ":" + Tools_1.DateTool.GetDateFromDate(this.TuesdayToHourDate).getUTCMinutes() + ":" + Tools_1.DateTool.GetDateFromDate(this.TuesdayToHourDate).getUTCSeconds();
        this.entityPM.WednesdayFromHour = Tools_1.DateTool.GetDateFromDate(this.WednesdayFromHourDate).getUTCHours() + ":" + Tools_1.DateTool.GetDateFromDate(this.WednesdayFromHourDate).getUTCMinutes() + ":" + Tools_1.DateTool.GetDateFromDate(this.WednesdayFromHourDate).getUTCSeconds();
        this.entityPM.WednesdayToHour = Tools_1.DateTool.GetDateFromDate(this.WednesdayToHourDate).getUTCHours() + ":" + Tools_1.DateTool.GetDateFromDate(this.WednesdayToHourDate).getUTCMinutes() + ":" + Tools_1.DateTool.GetDateFromDate(this.WednesdayToHourDate).getUTCSeconds();
        this.entityPM.ThursdayFromHour = Tools_1.DateTool.GetDateFromDate(this.ThursdayFromHourDate).getUTCHours() + ":" + Tools_1.DateTool.GetDateFromDate(this.ThursdayFromHourDate).getUTCMinutes() + ":" + Tools_1.DateTool.GetDateFromDate(this.ThursdayFromHourDate).getUTCSeconds();
        this.entityPM.ThursdayToHour = Tools_1.DateTool.GetDateFromDate(this.ThursdayToHourDate).getUTCHours() + ":" + Tools_1.DateTool.GetDateFromDate(this.ThursdayToHourDate).getUTCMinutes() + ":" + Tools_1.DateTool.GetDateFromDate(this.ThursdayToHourDate).getUTCSeconds();
        this.entityPM.FridayFromHour = Tools_1.DateTool.GetDateFromDate(this.FridayFromHourDate).getUTCHours() + ":" + Tools_1.DateTool.GetDateFromDate(this.FridayFromHourDate).getUTCMinutes() + ":" + Tools_1.DateTool.GetDateFromDate(this.FridayFromHourDate).getUTCSeconds();
        this.entityPM.FridayToHour = Tools_1.DateTool.GetDateFromDate(this.FridayToHourDate).getUTCHours() + ":" + Tools_1.DateTool.GetDateFromDate(this.FridayToHourDate).getUTCMinutes() + ":" + Tools_1.DateTool.GetDateFromDate(this.FridayToHourDate).getUTCSeconds();
        this.entityPM.SaturdayFromHour = Tools_1.DateTool.GetDateFromDate(this.SaturdayFromHourDate).getUTCHours() + ":" + Tools_1.DateTool.GetDateFromDate(this.SaturdayFromHourDate).getUTCMinutes() + ":" + Tools_1.DateTool.GetDateFromDate(this.SaturdayFromHourDate).getUTCSeconds();
        this.entityPM.SaturdayToHour = Tools_1.DateTool.GetDateFromDate(this.SaturdayToHourDate).getUTCHours() + ":" + Tools_1.DateTool.GetDateFromDate(this.SaturdayToHourDate).getUTCMinutes() + ":" + Tools_1.DateTool.GetDateFromDate(this.SaturdayToHourDate).getUTCSeconds();
        this.entityPM.SundayFromHour = Tools_1.DateTool.GetDateFromDate(this.SundayFromHourDate).getUTCHours() + ":" + Tools_1.DateTool.GetDateFromDate(this.SundayFromHourDate).getUTCMinutes() + ":" + Tools_1.DateTool.GetDateFromDate(this.SundayFromHourDate).getUTCSeconds();
        this.entityPM.SundayToHour = Tools_1.DateTool.GetDateFromDate(this.SundayToHourDate).getUTCHours() + ":" + Tools_1.DateTool.GetDateFromDate(this.SundayToHourDate).getUTCMinutes() + ":" + Tools_1.DateTool.GetDateFromDate(this.SundayToHourDate).getUTCSeconds();
    };
    NewBusinessHourAndHolidaysComponent.prototype.AddHoliday = function () {
        var todayDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        var holidayPM = new BusinessHoursHolidayPM_1.BusinessHoursHolidayPM(null);
        holidayPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        holidayPM.BusinessHourId = this.entityPM.Id;
        holidayPM.CreateDate = todayDateTime;
        holidayPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        holidayPM.UpdateDate = todayDateTime;
        holidayPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        var viewModel = new BusinessHourHolidayArgs(this.entityPM, holidayPM, this, true, false);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "New Holiday";
        logWindow.Width = 800;
        logWindow.Height = 450;
        logWindow.DataContext = viewModel;
        logWindow.Show('./CRMModules/CRMOthers/Components/BusinessHour/AddEditBusinessHourHolidayComponent');
    };
    NewBusinessHourAndHolidaysComponent.prototype.EditHoliday = function (holiday) {
        var viewModel = new BusinessHourHolidayArgs(this.entityPM, holiday, this, false, true);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Edit Holiday";
        logWindow.Width = 800;
        logWindow.Height = 450;
        logWindow.DataContext = viewModel;
        logWindow.Show('./CRMModules/CRMOthers/Components/BusinessHour/AddEditBusinessHourHolidayComponent');
    };
    NewBusinessHourAndHolidaysComponent.prototype.SetIs247Radio = function (arg) {
        this.Is247 = arg;
    };
    NewBusinessHourAndHolidaysComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewBusinessHourAndHolidaysComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewBusinessHourAndHolidaysComponent);
    return NewBusinessHourAndHolidaysComponent;
}(BaseComponent_1.BaseComponent));
exports.NewBusinessHourAndHolidaysComponent = NewBusinessHourAndHolidaysComponent;
var BusinessHourHolidayArgs = /** @class */ (function (_super) {
    __extends(BusinessHourHolidayArgs, _super);
    function BusinessHourHolidayArgs(businssHourPm, entityPM, trigger, isNew, IsInActiveVisibility) {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "BusinessHoursHoliday";
        _this.isNew = false;
        _this.IsInActiveVisibility = false;
        _this.DataContext = _this;
        _this.createDatePicker = null;
        _this.HolidayDate = null;
        _this.dayMonthVisibility = false;
        _this.datePickerVisibility = false;
        _this.trigger = trigger;
        _this.isNew = isNew;
        _this.IsInActiveVisibility = IsInActiveVisibility;
        _this.entityPM = entityPM;
        _this.businssHourPm = businssHourPm;
        _this.ChangeInActiveVisibility();
        _this.ChangeDateVisibility();
        _this.CreateHolidayDate();
        return _this;
    }
    Object.defineProperty(BusinessHourHolidayArgs.prototype, "HolidayName", {
        // Properties
        get: function () { return this.entityPM.HolidayName; },
        set: function (value) {
            if (this.entityPM.HolidayName != value) {
                this.entityPM.HolidayName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusinessHourHolidayArgs.prototype, "CreateDatePicker", {
        get: function () {
            return this.createDatePicker;
        },
        set: function (value) {
            if (this.createDatePicker != value) {
                this.createDatePicker = value;
                if (this.createDatePicker != null) {
                    this.Day = this.createDatePicker.getUTCDate();
                    this.Month = this.createDatePicker.getUTCMonth() + 1;
                    this.Year = this.createDatePicker.getUTCFullYear();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusinessHourHolidayArgs.prototype, "CreateDate", {
        get: function () { return this.entityPM.CreateDate; },
        set: function (value) {
            if (this.entityPM.CreateDate != value) {
                this.entityPM.CreateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusinessHourHolidayArgs.prototype, "IsRecurring", {
        get: function () { return this.entityPM.IsRecurring; },
        set: function (value) {
            if (this.entityPM.IsRecurring != value) {
                this.entityPM.IsRecurring = value;
                this.ChangeDateVisibility();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusinessHourHolidayArgs.prototype, "Inactive", {
        get: function () { return this.entityPM.Inactive; },
        set: function (value) {
            if (this.entityPM.Inactive != value) {
                this.entityPM.Inactive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    BusinessHourHolidayArgs.prototype.ChangeDateVisibility = function () {
        if (this.IsRecurring) {
            this.DatePickerVisibility = false;
            this.DayMonthVisibility = true;
        }
        else {
            this.DayMonthVisibility = false;
            this.DatePickerVisibility = true;
        }
    };
    BusinessHourHolidayArgs.prototype.ChangeInActiveVisibility = function () {
        if (this.IsInActiveVisibility) {
            this.UIProperties.SetVisibility("Inactive", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetVisibility("Inactive", this.ObjectTableName, false);
        }
    };
    Object.defineProperty(BusinessHourHolidayArgs.prototype, "Day", {
        get: function () {
            return this.entityPM.Day;
        },
        set: function (value) {
            if (this.entityPM.Day != value) {
                this.entityPM.Day = value;
                this.CreateHolidayDate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusinessHourHolidayArgs.prototype, "Month", {
        get: function () {
            return this.entityPM.Month;
        },
        set: function (value) {
            if (this.entityPM.Month != value) {
                this.entityPM.Month = value;
                this.CreateHolidayDate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusinessHourHolidayArgs.prototype, "Year", {
        get: function () {
            return this.entityPM.Year;
        },
        set: function (value) {
            if (this.entityPM.Year != value) {
                this.entityPM.Year = value;
                this.CreateHolidayDate();
            }
        },
        enumerable: true,
        configurable: true
    });
    BusinessHourHolidayArgs.prototype.CreateHolidayDate = function () {
        if (this.entityPM != null && this.entityPM.Day != null && this.entityPM.Year != null && this.entityPM.Month != null) {
            if (this.IsRecurring) {
                if (this.Day != 0 && this.Month != 0) {
                    this.HolidayDate = Tools_1.AppTool.PadLeft("" + this.Day, 2, '0') + "/" + Tools_1.AppTool.PadLeft("" + this.Month, 2, '0');
                }
            }
            else {
                if (this.Day != 0 && this.Month != 0 && this.Year != 0) {
                    var holiday = new Date();
                    holiday.setUTCFullYear(this.Year);
                    holiday.setUTCMonth(this.Month - 1);
                    holiday.setUTCDate(this.Day);
                    holiday.setUTCHours(0);
                    holiday.setUTCMinutes(0);
                    holiday.setUTCSeconds(0);
                    holiday.setUTCMilliseconds(0);
                    this.createDatePicker = holiday;
                    this.HolidayDate = Tools_1.AppTool.PadLeft("" + this.Day, 2, '0') + "/" + Tools_1.AppTool.PadLeft("" + this.Month, 2, '0') + "/" + Tools_1.AppTool.PadLeft("" + this.Year, 2, '0');
                }
            }
        }
    };
    Object.defineProperty(BusinessHourHolidayArgs.prototype, "DayMonthVisibility", {
        get: function () { return this.dayMonthVisibility; },
        set: function (value) {
            this.dayMonthVisibility = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusinessHourHolidayArgs.prototype, "DatePickerVisibility", {
        get: function () { return this.datePickerVisibility; },
        set: function (value) {
            this.datePickerVisibility = value;
        },
        enumerable: true,
        configurable: true
    });
    return BusinessHourHolidayArgs;
}(BaseComponent_1.BaseComponent));
exports.BusinessHourHolidayArgs = BusinessHourHolidayArgs;
//# sourceMappingURL=NewBusinessHourAndHolidaysComponent.js.map