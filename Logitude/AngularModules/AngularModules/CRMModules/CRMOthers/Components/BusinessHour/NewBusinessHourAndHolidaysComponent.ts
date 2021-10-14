import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CRMDomainService} from '../../../../CRM/Services/CRMDomainService';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {BusinessHourPM} from '../../../../Infrastructure/EntityPMs/BusinessHourPM';
import {BusinessHourPMService} from '../../../../Infrastructure/Services/StandardPMs/BusinessHourPMService';
import {BusinessHoursHolidayPM} from '../../../../Infrastructure/EntityPMs/BusinessHoursHolidayPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {InfrastructureDomainService} from '../../../../Infrastructure/Services/InfrastructureDomainService';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from  '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({

    templateUrl: './NewBusinessHourAndHolidaysComponent.html',
})

export class NewBusinessHourAndHolidaysComponent extends BaseComponent {
    public ObjectTableName: string = "BusinessHour";
    public DataContext: NewBusinessHourAndHolidaysComponent = this;
    public entityPM: BusinessHourPM;
    public ObjectTable: any;
    public ValidationErrorsList: string[] = [];
    public HolidaysDataList: BusinessHourHolidayArgs[];
    public IsVisible = false;
    private IsNew = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.entityPM = new BusinessHourPM();
        this.HolidaysDataList = [];
        this.getBusinssHourEntityMethod();

    }

    //Load Business Hour Entity
    private getBusinssHourEntityMethod() {
        var service = new InfrastructureDomainService();
        service.GetBusinessHourBM().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.entityPM = myResponse.Result;
                if (this.entityPM != null) {
                    this.IsNew = false;
                    this.RefreshData();
                }
                else {
                    this.IsNew = true;
                    this.entityPM = new BusinessHourPM();
                    var todayDateTime = DateTool.GetCurrentDateTimeAsUtc();
                    this.entityPM.Tenant = SessionLocator.Tenant;
                    this.entityPM.CreateDate = todayDateTime;
                    this.entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
                    this.entityPM.UpdateDate = todayDateTime;
                    this.entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
                }

                this.IsVisible = true;
                this.SetEnabled();
            }
        });
    }
    private RefreshData() {
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
        this.SetDateEnabled();
    }

    SetEnabled() {
        this.UIProperties.SetEnabled("IsMondayEnabeled", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("IsTuesdayEnabeled", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("IsWednesdayEnabeled", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("IsThursdayEnabeled", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("IsFridayEnabeled", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("IsSaturdayEnabeled", this.ObjectTableName, !this.Is247);
        this.UIProperties.SetEnabled("IsSundayEnabeled", this.ObjectTableName, !this.Is247);
        this.CheckBoxProcessing();

    }

    SetDateEnabled() {

        this.UIProperties.SetEnabled("MondayToHourDate_timepicker", this.ObjectTableName, this.IsMondayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("MondayFromHourDate_timepicker", this.ObjectTableName, this.IsMondayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("SaturdayToHourDate_timepicker", this.ObjectTableName, this.IsSaturdayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("SaturdayFromHourDate_timepicker", this.ObjectTableName, this.IsSaturdayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("SundayToHourDate_timepicker", this.ObjectTableName, this.IsSundayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("SundayFromHourDate_timepicker", this.ObjectTableName, this.IsSundayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("FridayToHourDate_timepicker", this.ObjectTableName, this.IsFridayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("FridayFromHourDate_timepicker", this.ObjectTableName, this.IsFridayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("ThursdayToHourDate_timepicker", this.ObjectTableName, this.IsThursdayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("ThursdayFromHourDate_timepicker", this.ObjectTableName, this.IsThursdayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("WednesdayToHourDate_timepicker", this.ObjectTableName, this.IsWednesdayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("WednesdayFromHourDate_timepicker", this.ObjectTableName, this.IsWednesdayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("TuesdayToHourDate_timepicker", this.ObjectTableName, this.IsTuesdayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("TuesdayFromHourDate_timepicker", this.ObjectTableName, this.IsTuesdayEnabeled && !this.Is247);

        this.UIProperties.SetEnabled("MondayToHourDate", this.ObjectTableName, this.IsMondayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("MondayFromHourDate", this.ObjectTableName, this.IsMondayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("SaturdayToHourDate", this.ObjectTableName, this.IsSaturdayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("SaturdayFromHourDate", this.ObjectTableName, this.IsSaturdayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("SundayToHourDate", this.ObjectTableName, this.IsSundayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("SundayFromHourDate", this.ObjectTableName, this.IsSundayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("FridayToHourDate", this.ObjectTableName, this.IsFridayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("FridayFromHourDate", this.ObjectTableName, this.IsFridayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("ThursdayToHourDate", this.ObjectTableName, this.IsThursdayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("ThursdayFromHourDate", this.ObjectTableName, this.IsThursdayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("WednesdayToHourDate", this.ObjectTableName, this.IsWednesdayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("WednesdayFromHourDate", this.ObjectTableName, this.IsWednesdayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("TuesdayToHourDate", this.ObjectTableName, this.IsTuesdayEnabeled && !this.Is247);
        this.UIProperties.SetEnabled("TuesdayFromHourDate", this.ObjectTableName, this.IsTuesdayEnabeled && !this.Is247);

    }


    //Fill Holidays List
    public fillHolidays() {
        if (this.entityPM != null && this.entityPM.BusinessHoursHolidays.length > 0) {
            this.HolidaysDataList = [];
            this.entityPM.BusinessHoursHolidays.forEach(item => {
                this.HolidaysDataList.push(new BusinessHourHolidayArgs(this.entityPM, item, this, false, false));
            });
        }
    }

    // Properties
    get MondayFromHourDate() { return this.entityPM.MondayFromHourDate; }
    set MondayFromHourDate(value: Date) {
        if (this.entityPM.MondayFromHourDate != value) {
            this.entityPM.MondayFromHourDate = value;
            this.getTotalWorkHours();
        }
    }
    get TuesdayFromHourDate() { return this.entityPM.TuesdayFromHourDate; }
    set TuesdayFromHourDate(value: Date) {
        if (this.entityPM.TuesdayFromHourDate != value) {
            this.entityPM.TuesdayFromHourDate = value;
            this.getTotalWorkHours();
        }
    }
    get WednesdayFromHourDate() { return this.entityPM.WednesdayFromHourDate; }
    set WednesdayFromHourDate(value: Date) {
        if (this.entityPM.WednesdayFromHourDate != value) {
            this.entityPM.WednesdayFromHourDate = value;
            this.getTotalWorkHours();
        }
    }
    get ThursdayFromHourDate() { return this.entityPM.ThursdayFromHourDate; }
    set ThursdayFromHourDate(value: Date) {
        if (this.entityPM.ThursdayFromHourDate != value) {
            this.entityPM.ThursdayFromHourDate = value;
            this.getTotalWorkHours();
        }
    }
    get FridayFromHourDate() { return this.entityPM.FridayFromHourDate; }
    set FridayFromHourDate(value: Date) {
        if (this.entityPM.FridayFromHourDate != value) {
            this.entityPM.FridayFromHourDate = value;
            this.getTotalWorkHours();
        }
    }
    get SaturdayFromHourDate() { return this.entityPM.SaturdayFromHourDate; }
    set SaturdayFromHourDate(value: Date) {
        if (this.entityPM.SaturdayFromHourDate != value) {
            this.entityPM.SaturdayFromHourDate = value;
            this.getTotalWorkHours();
        }
    }
    get SundayFromHourDate() { return this.entityPM.SundayFromHourDate; }
    set SundayFromHourDate(value: Date) {
        if (this.entityPM.SundayFromHourDate != value) {
            this.entityPM.SundayFromHourDate = value;
            this.getTotalWorkHours();
        }
    }

    get MondayToHourDate() { return this.entityPM.MondayToHourDate; }
    set MondayToHourDate(value: Date) {
        if (this.entityPM.MondayToHourDate != value) {
            this.entityPM.MondayToHourDate = value;
            this.getTotalWorkHours();
        }
    }
    get TuesdayToHourDate() { return this.entityPM.TuesdayToHourDate; }
    set TuesdayToHourDate(value: Date) {
        if (this.entityPM.TuesdayToHourDate != value) {
            this.entityPM.TuesdayToHourDate = value;
            this.getTotalWorkHours();
        }
    }
    get WednesdayToHourDate() { return this.entityPM.WednesdayToHourDate; }
    set WednesdayToHourDate(value: Date) {
        if (this.entityPM.WednesdayToHourDate != value) {
            this.entityPM.WednesdayToHourDate = value;
            this.getTotalWorkHours();
        }
    }
    get ThursdayToHourDate() { return this.entityPM.ThursdayToHourDate; }
    set ThursdayToHourDate(value: Date) {
        if (this.entityPM.ThursdayToHourDate != value) {
            this.entityPM.ThursdayToHourDate = value;
            this.getTotalWorkHours();
        }
    }
    get FridayToHourDate() { return this.entityPM.FridayToHourDate; }
    set FridayToHourDate(value: Date) {
        if (this.entityPM.FridayToHourDate != value) {
            this.entityPM.FridayToHourDate = value;
            this.getTotalWorkHours();
        }
    }
    get SaturdayToHourDate() { return this.entityPM.SaturdayToHourDate; }
    set SaturdayToHourDate(value: Date) {
        if (this.entityPM.SaturdayToHourDate != value) {
            this.entityPM.SaturdayToHourDate = value;
            this.getTotalWorkHours();
        }
    }
    get SundayToHourDate() { return this.entityPM.SundayToHourDate; }
    set SundayToHourDate(value: Date) {
        if (this.entityPM.SundayToHourDate != value) {
            this.entityPM.SundayToHourDate = value;
            this.getTotalWorkHours();
        }
    }


    get Name() { return this.entityPM.Name; }
    set Name(value: string) {
        if (this.entityPM.Name != value) {
            this.entityPM.Name = value;
        }
    }

    get Description() { return this.entityPM.Description; }
    set Description(value: string) {
        if (this.entityPM.Description != value) {
            this.entityPM.Description = value;
        }
    }

    get IsMondayEnabeled() { return this.entityPM.IsMondayEnabeled; }
    set IsMondayEnabeled(value: boolean) {
        if (this.entityPM.IsMondayEnabeled != value) {
            this.entityPM.IsMondayEnabeled = value;
            this.CheckBoxProcessing();

        }
    }

    get IsTuesdayEnabeled() { return this.entityPM.IsTuesdayEnabeled; }
    set IsTuesdayEnabeled(value: boolean) {
        if (this.entityPM.IsTuesdayEnabeled != value) {
            this.entityPM.IsTuesdayEnabeled = value;
            this.CheckBoxProcessing();

        }
    }

    get IsWednesdayEnabeled() { return this.entityPM.IsWednesdayEnabeled; }
    set IsWednesdayEnabeled(value: boolean) {
        if (this.entityPM.IsWednesdayEnabeled != value) {
            this.entityPM.IsWednesdayEnabeled = value;
            this.CheckBoxProcessing();

        }
    }

    get IsThursdayEnabeled() { return this.entityPM.IsThursdayEnabeled; }
    set IsThursdayEnabeled(value: boolean) {
        if (this.entityPM.IsThursdayEnabeled != value) {
            this.entityPM.IsThursdayEnabeled = value;
            this.CheckBoxProcessing();
        }
    }

    get IsFridayEnabeled() { return this.entityPM.IsFridayEnabeled; }
    set IsFridayEnabeled(value: boolean) {
        if (this.entityPM.IsFridayEnabeled != value) {
            this.entityPM.IsFridayEnabeled = value;
            this.CheckBoxProcessing();
        }
    }

    get IsSaturdayEnabeled() { return this.entityPM.IsSaturdayEnabeled; }
    set IsSaturdayEnabeled(value: boolean) {
        if (this.entityPM.IsSaturdayEnabeled != value) {
            this.entityPM.IsSaturdayEnabeled = value;
            this.CheckBoxProcessing();
        }
    }

    get IsSundayEnabeled() { return this.entityPM.IsSundayEnabeled; }
    set IsSundayEnabeled(value: boolean) {
        if (this.entityPM.IsSundayEnabeled != value) {
            this.entityPM.IsSundayEnabeled = value;
            this.CheckBoxProcessing();
        }
    }

    get MondayHourEnabled() {
        return this.IsMondayEnabeled;
    }
    get TuesdayHourEnabled() {
        return this.IsTuesdayEnabeled;
    }
    get WednesdayHourEnabled() {
        return this.IsWednesdayEnabeled;
    }
    get ThursdayHourEnabled() {
        return this.IsThursdayEnabeled;
    }
    get FridayHourEnabled() {
        return this.IsFridayEnabeled;
    }
    get SaturdayHourEnabled() {
        return this.IsSaturdayEnabeled;
    }
    get SundayHourEnabled() {
        return this.IsSundayEnabeled;
    }

    getDate(date) {
        if (date) {
            var h = date.split(':')[0];
            var m = date.split(':')[1];
            var s = date.split(':')[2];
            var myDate = DateTool.GetDateFromDate(new Date());
            myDate.setUTCHours(0);
            myDate.setUTCMinutes(0);
            myDate.setUTCSeconds(0);
            myDate.setUTCMilliseconds(0);
            myDate.setUTCHours(h);
            myDate.setUTCMinutes(m);
            myDate.setUTCSeconds(s);
            return DateTool.GetDateFromDate(myDate);
        }
    }

    public DefinedHours = true;
    get Is247() { return this.entityPM.Is247; }
    set Is247(value: boolean) {
        if (this.entityPM.Is247 != value) {
            this.entityPM.Is247 = value;
            this.DefinedHours = !value;
            this.SetEnabled();
        }
    }

    private mondayTotalWorkHours = "0";
    get MondayTotalWorkHours() {
        return this.mondayTotalWorkHours;
    }
    set MondayTotalWorkHours(value: string) {
        this.mondayTotalWorkHours = value;
    }

    private tuesdayTotalWorkHours = "0";
    get TuesdayTotalWorkHours() {
        return this.tuesdayTotalWorkHours;
    }
    set TuesdayTotalWorkHours(value: string) {
        this.tuesdayTotalWorkHours = value;
    }

    private wednesdayTotalWorkHours = "0";
    get WednesdayTotalWorkHours() {
        return this.wednesdayTotalWorkHours;
    }
    set WednesdayTotalWorkHours(value: string) {
        this.wednesdayTotalWorkHours = value;
    }

    private thursdayTotalWorkHours = "0";
    get ThursdayTotalWorkHours() {
        return this.thursdayTotalWorkHours;
    }
    set ThursdayTotalWorkHours(value: string) {
        this.thursdayTotalWorkHours = value;
    }

    private fridayTotalWorkHours = "0";
    get FridayTotalWorkHours() {
        return this.fridayTotalWorkHours;
    }
    set FridayTotalWorkHours(value: string) {
        this.fridayTotalWorkHours = value;
    }

    private saturdayTotalWorkHours = "0";
    get SaturdayTotalWorkHours() {
        return this.saturdayTotalWorkHours;
    }
    set SaturdayTotalWorkHours(value: string) {
        this.saturdayTotalWorkHours = value;
    }

    private sundayTotalWorkHours = "0";
    get SundayTotalWorkHours() {
        return this.sundayTotalWorkHours;
    }
    set SundayTotalWorkHours(value: string) {
        this.sundayTotalWorkHours = value;
    }

    public AllTotalWorkHours: string;
    private getTotalWorkHours() {
        this.MondayTotalWorkHours = this.timeDifferencecalCulationMethod(this.MondayToHourDate, this.MondayFromHourDate, this.IsMondayEnabeled);
        this.TuesdayTotalWorkHours = this.timeDifferencecalCulationMethod(this.TuesdayToHourDate, this.TuesdayFromHourDate, this.IsTuesdayEnabeled);
        this.WednesdayTotalWorkHours = this.timeDifferencecalCulationMethod(this.WednesdayToHourDate, this.WednesdayFromHourDate, this.IsWednesdayEnabeled);
        this.ThursdayTotalWorkHours = this.timeDifferencecalCulationMethod(this.ThursdayToHourDate, this.ThursdayFromHourDate, this.IsThursdayEnabeled);
        this.FridayTotalWorkHours = this.timeDifferencecalCulationMethod(this.FridayToHourDate, this.FridayFromHourDate, this.IsFridayEnabeled);
        this.SaturdayTotalWorkHours = this.timeDifferencecalCulationMethod(this.SaturdayToHourDate, this.SaturdayFromHourDate, this.IsSaturdayEnabeled);
        this.SundayTotalWorkHours = this.timeDifferencecalCulationMethod(this.SundayToHourDate, this.SundayFromHourDate, this.IsSundayEnabeled);

        var saturdayTotalValue: number = parseFloat(this.SaturdayTotalWorkHours.split(':')[0]);
        var mondayTotalValue: number = parseFloat(this.MondayTotalWorkHours.split(':')[0]);
        var tuesdayTotalValue: number = parseFloat(this.TuesdayTotalWorkHours.split(':')[0]);
        var wednesdayTotalValue: number = parseFloat(this.WednesdayTotalWorkHours.split(':')[0]);
        var thursdayTotalValue: number = parseFloat(this.ThursdayTotalWorkHours.split(':')[0]);
        var fridayTotalValue: number = parseFloat(this.FridayTotalWorkHours.split(':')[0]);
        var sundayTotalValue: number = parseFloat(this.SundayTotalWorkHours.split(':')[0]);

        var tempValue: number = 0;
        tempValue =
            (mondayTotalValue != null ? mondayTotalValue : 0) +
            (tuesdayTotalValue != null ? tuesdayTotalValue : 0) +
            (wednesdayTotalValue != null ? wednesdayTotalValue : 0) +
            (thursdayTotalValue != null ? thursdayTotalValue : 0) +
            (fridayTotalValue != null ? fridayTotalValue : 0) +
            (saturdayTotalValue != null ? saturdayTotalValue : 0) +
            (sundayTotalValue != null ? sundayTotalValue : 0);

        this.AllTotalWorkHours = tempValue.toString();
    }
    private CheckBoxProcessing() {
        if (!this.IsMondayEnabeled) {
            this.MondayToHourDate = null;
            this.MondayFromHourDate = null;
        }

        if (!this.IsSaturdayEnabeled) {
            this.SaturdayToHourDate = null;
            this.SaturdayFromHourDate = null;
        }

        if (!this.IsTuesdayEnabeled) {
            this.TuesdayToHourDate = null;
            this.TuesdayFromHourDate = null;
        }

        if (!this.IsWednesdayEnabeled) {
            this.WednesdayToHourDate = null;
            this.WednesdayFromHourDate = null;
        }

        if (!this.IsThursdayEnabeled) {
            this.ThursdayToHourDate = null;
            this.ThursdayFromHourDate = null;
        }

        if (!this.IsFridayEnabeled) {
            this.FridayToHourDate = null;
            this.FridayFromHourDate = null;
        }

        if (!this.IsSundayEnabeled) {
            this.SundayToHourDate = null;
            this.SundayFromHourDate = null;
        }

        this.SetDateEnabled();
        this.getTotalWorkHours();
    }


    private timeDifferencecalCulationMethod(toHour: Date, fromHour: Date, isEnable: boolean) {
        var time = "0";
        var dateDiff;
        if (isEnable && toHour && fromHour) {
            var d1 = DateTool.GetDateFormats(new Date(toHour.toString())).DateParts.DateObject;
            var d2 = DateTool.GetDateFormats(new Date(fromHour.toString())).DateParts.DateObject;
            var timeDiff = Math.abs(DateTool.GetDateFromDate(toHour).getTime() - DateTool.GetDateFromDate(fromHour).getTime());

            var hours = Math.floor(timeDiff / (1000 * 3600));
            timeDiff -= hours * 1000 * 60 * 60
            var minutes = Math.floor(timeDiff / 1000 / 60);
            time = AppTool.PadLeft("" + hours, 2, '0') + ":" + AppTool.PadLeft("" + minutes, 2, '0');
        }
        return time;
    }

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.UpadteDates();
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors: string[] = [];
        Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
        if (AppTool.IsNullOrEmpty(this.entityPM.Name)) {
            errors.push("Name field is required");
        }

        this.ValidationErrorsList = errors;
        this.ValidateBusinessHoursTime();
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsNew) {
                this.InsertBusinesHour();
            }
            else {
                this.UpdateBusinesHour();
            }
        }
    }
    InsertBusinesHour() {
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new BusinessHourPMService();
        service.insert(this.entityPM).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit('ok');
            }
            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }
    UpdateBusinesHour() {
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new BusinessHourPMService();
        service.update(this.entityPM).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit('ok');
            }
            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }

    ValidateBusinessHoursTime() {
        var isValid = true;

        if (this.SundayFromHourDate != null && this.SundayToHourDate != null && DateTool.GetDateFromDate(this.SundayToHourDate) <= DateTool.GetDateFromDate(this.SundayFromHourDate)) {
            isValid = false;
        }
        if (this.MondayFromHourDate != null && this.MondayToHourDate != null && DateTool.GetDateFromDate(this.MondayToHourDate) <= DateTool.GetDateFromDate(this.MondayFromHourDate)) {
            isValid = false;
        }
        if (this.TuesdayFromHourDate != null && this.TuesdayToHourDate != null && DateTool.GetDateFromDate(this.TuesdayToHourDate) <= DateTool.GetDateFromDate(this.TuesdayFromHourDate)) {
            isValid = false;
        }
        if (this.WednesdayFromHourDate != null && this.WednesdayToHourDate != null && DateTool.GetDateFromDate(this.WednesdayToHourDate) <= DateTool.GetDateFromDate(this.WednesdayFromHourDate)) {
            isValid = false;
        }
        if (this.ThursdayFromHourDate != null && this.ThursdayToHourDate != null && DateTool.GetDateFromDate(this.ThursdayToHourDate) <= DateTool.GetDateFromDate(this.ThursdayFromHourDate)) {
            isValid = false;
        }
        if (this.FridayFromHourDate != null && this.FridayToHourDate != null && DateTool.GetDateFromDate(this.FridayToHourDate) <= DateTool.GetDateFromDate(this.FridayFromHourDate)){
            isValid = false;
        }
        if (this.SaturdayFromHourDate != null && this.SaturdayToHourDate != null && DateTool.GetDateFromDate(this.SaturdayToHourDate) <= DateTool.GetDateFromDate(this.SaturdayFromHourDate)) {
            isValid = false;
        }
        if (!isValid) this.ValidationErrorsList.push("Exist time Field must be greater than Entry time Field");
    }

    UpadteDates() {
        if (this.MondayFromHourDate) {
            this.entityPM.MondayFromHour = DateTool.GetDateFromDate(this.MondayFromHourDate).getUTCHours() + ":" + DateTool.GetDateFromDate(this.MondayFromHourDate).getUTCMinutes() + ":" + DateTool.GetDateFromDate(this.MondayFromHourDate).getUTCSeconds();
        }
        else {
            this.entityPM.MondayFromHour = null;
        }

        if (this.MondayToHourDate) {
            this.entityPM.MondayToHour = DateTool.GetDateFromDate(this.MondayToHourDate).getUTCHours() + ":" + DateTool.GetDateFromDate(this.MondayToHourDate).getUTCMinutes() + ":" + DateTool.GetDateFromDate(this.MondayToHourDate).getUTCSeconds();
        }
        else {
            this.entityPM.MondayToHour = null;
        }

        if (this.TuesdayFromHourDate) {
            this.entityPM.TuesdayFromHour = DateTool.GetDateFromDate(this.TuesdayFromHourDate).getUTCHours() + ":" + DateTool.GetDateFromDate(this.TuesdayFromHourDate).getUTCMinutes() + ":" + DateTool.GetDateFromDate(this.TuesdayFromHourDate).getUTCSeconds();

        }
        else {
            this.entityPM.TuesdayFromHour = null;
        }

        if (this.TuesdayToHourDate) {
            this.entityPM.TuesdayToHour = DateTool.GetDateFromDate(this.TuesdayToHourDate).getUTCHours() + ":" + DateTool.GetDateFromDate(this.TuesdayToHourDate).getUTCMinutes() + ":" + DateTool.GetDateFromDate(this.TuesdayToHourDate).getUTCSeconds();
        }
        else {
            this.entityPM.TuesdayToHour = null;
        }

        if (this.WednesdayFromHourDate) {
            this.entityPM.WednesdayFromHour = DateTool.GetDateFromDate(this.WednesdayFromHourDate).getUTCHours() + ":" + DateTool.GetDateFromDate(this.WednesdayFromHourDate).getUTCMinutes() + ":" + DateTool.GetDateFromDate(this.WednesdayFromHourDate).getUTCSeconds();

        }
        else {
            this.entityPM.WednesdayFromHour = null;
        }
        if (this.WednesdayToHourDate) {
            this.entityPM.WednesdayToHour = DateTool.GetDateFromDate(this.WednesdayToHourDate).getUTCHours() + ":" + DateTool.GetDateFromDate(this.WednesdayToHourDate).getUTCMinutes() + ":" + DateTool.GetDateFromDate(this.WednesdayToHourDate).getUTCSeconds();
        }
        else {
            this.entityPM.WednesdayToHour = null;
        }
        if (this.ThursdayFromHourDate) {
            this.entityPM.ThursdayFromHour = DateTool.GetDateFromDate(this.ThursdayFromHourDate).getUTCHours() + ":" + DateTool.GetDateFromDate(this.ThursdayFromHourDate).getUTCMinutes() + ":" + DateTool.GetDateFromDate(this.ThursdayFromHourDate).getUTCSeconds();
        }
        else {
            this.entityPM.ThursdayFromHour = null;
        }
        if (this.ThursdayToHourDate) {
            this.entityPM.ThursdayToHour = DateTool.GetDateFromDate(this.ThursdayToHourDate).getUTCHours() + ":" + DateTool.GetDateFromDate(this.ThursdayToHourDate).getUTCMinutes() + ":" + DateTool.GetDateFromDate(this.ThursdayToHourDate).getUTCSeconds();
        }
        else {
            this.entityPM.ThursdayToHour = null;
        }
        if (this.FridayFromHourDate) {
            this.entityPM.FridayFromHour = DateTool.GetDateFromDate(this.FridayFromHourDate).getUTCHours() + ":" + DateTool.GetDateFromDate(this.FridayFromHourDate).getUTCMinutes() + ":" + DateTool.GetDateFromDate(this.FridayFromHourDate).getUTCSeconds();

        }
        else {
            this.entityPM.FridayFromHour = null;
        }
        if (this.FridayToHourDate) {
            this.entityPM.FridayToHour = DateTool.GetDateFromDate(this.FridayToHourDate).getUTCHours() + ":" + DateTool.GetDateFromDate(this.FridayToHourDate).getUTCMinutes() + ":" + DateTool.GetDateFromDate(this.FridayToHourDate).getUTCSeconds();
        }
        else {
            this.entityPM.FridayToHour = null;
        }
        if (this.SaturdayFromHourDate) {
            this.entityPM.SaturdayFromHour = DateTool.GetDateFromDate(this.SaturdayFromHourDate).getUTCHours() + ":" + DateTool.GetDateFromDate(this.SaturdayFromHourDate).getUTCMinutes() + ":" + DateTool.GetDateFromDate(this.SaturdayFromHourDate).getUTCSeconds();

        }
        else {
            this.entityPM.SaturdayFromHour = null;
        }
        if (this.SaturdayToHourDate) {
            this.entityPM.SaturdayToHour = DateTool.GetDateFromDate(this.SaturdayToHourDate).getUTCHours() + ":" + DateTool.GetDateFromDate(this.SaturdayToHourDate).getUTCMinutes() + ":" + DateTool.GetDateFromDate(this.SaturdayToHourDate).getUTCSeconds();
        }
        else {
            this.entityPM.SaturdayToHour = null;
        }
        if (this.SundayFromHourDate) {
            this.entityPM.SundayFromHour = DateTool.GetDateFromDate(this.SundayFromHourDate).getUTCHours() + ":" + DateTool.GetDateFromDate(this.SundayFromHourDate).getUTCMinutes() + ":" + DateTool.GetDateFromDate(this.SundayFromHourDate).getUTCSeconds();

        }
        else {
            this.entityPM.SundayFromHour = null;
        }
        if (this.SundayToHourDate) {
            this.entityPM.SundayToHour = DateTool.GetDateFromDate(this.SundayToHourDate).getUTCHours() + ":" + DateTool.GetDateFromDate(this.SundayToHourDate).getUTCMinutes() + ":" + DateTool.GetDateFromDate(this.SundayToHourDate).getUTCSeconds();
        }
        else {
            this.entityPM.SundayToHour = null;
        }
    }

    AddHoliday() {
        var todayDateTime = DateTool.GetCurrentDateTimeAsUtc();
        var holidayPM: BusinessHoursHolidayPM = new BusinessHoursHolidayPM(null);
        holidayPM.Tenant = SessionLocator.Tenant;
        holidayPM.BusinessHourId = this.entityPM.Id;
        holidayPM.CreateDate = todayDateTime;
        holidayPM.CreatedByUserId = SessionLocator.LoggedUserId;
        holidayPM.UpdateDate = todayDateTime;
        holidayPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        var viewModel = new BusinessHourHolidayArgs(this.entityPM, holidayPM, this, true, false);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "New Holiday";
        logWindow.Width = 800;
        logWindow.Height = 450;
        logWindow.DataContext = viewModel;
        logWindow.Show('./CRMModules/CRMOthers/Components/BusinessHour/AddEditBusinessHourHolidayComponent');
    }
    EditHoliday(holiday: BusinessHoursHolidayPM) {
        var viewModel = new BusinessHourHolidayArgs(this.entityPM, holiday, this, false, true);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit Holiday";
        logWindow.Width = 800;
        logWindow.Height = 450;
        logWindow.DataContext = viewModel;
        logWindow.Show('./CRMModules/CRMOthers/Components/BusinessHour/AddEditBusinessHourHolidayComponent');
    }
    SetIs247Radio(arg) {
        this.Is247 = arg;
    }
}

export class BusinessHourHolidayArgs extends BaseComponent{
    public entityPM: BusinessHoursHolidayPM;
    public businssHourPm: BusinessHourPM;
    public ObjectTableName = "BusinessHoursHoliday";
    public trigger: NewBusinessHourAndHolidaysComponent;
    public isNew = false;
    public IsInActiveVisibility = false;
    public DataContext: BusinessHourHolidayArgs = this;

    constructor(businssHourPm: BusinessHourPM, entityPM: BusinessHoursHolidayPM, trigger: NewBusinessHourAndHolidaysComponent, isNew: boolean, IsInActiveVisibility: boolean) {
        super();
        this.trigger = trigger;
        this.isNew = isNew;
        this.IsInActiveVisibility = IsInActiveVisibility;
        this.entityPM = entityPM;
        this.businssHourPm = businssHourPm;
        this.ChangeInActiveVisibility();
        this.ChangeDateVisibility();
        this.CreateHolidayDate();
    }

    // Properties
    get HolidayName() { return this.entityPM.HolidayName; }
    set HolidayName(value:string) {
        if (this.entityPM.HolidayName != value) {
            this.entityPM.HolidayName = value;
        }
    }

    private createDatePicker: Date = null;
    get CreateDatePicker() {
        return this.createDatePicker;
    }
    set CreateDatePicker(value: Date) {
        if (this.createDatePicker != value) {
            this.createDatePicker = value;
            if (this.createDatePicker != null) {
                this.Day = this.createDatePicker.getUTCDate();
                this.Month = this.createDatePicker.getUTCMonth() + 1;
                this.Year = this.createDatePicker.getUTCFullYear();
            }
        }
    }

    get CreateDate() { return this.entityPM.CreateDate; }
    set CreateDate(value: Date) {
        if (this.entityPM.CreateDate != value) {
            this.entityPM.CreateDate = value;
        }
    }

    get IsRecurring() { return this.entityPM.IsRecurring; }
    set IsRecurring(value: boolean) {
        if (this.entityPM.IsRecurring != value) {
            this.entityPM.IsRecurring = value;
            this.ChangeDateVisibility();
        }
    }


    get Inactive() { return this.entityPM.Inactive; }
    set Inactive(value: boolean) {
        if (this.entityPM.Inactive != value) {
            this.entityPM.Inactive = value;
        }
    }

    private ChangeDateVisibility() {
        if (this.IsRecurring) {
            this.DatePickerVisibility = false;
            this. DayMonthVisibility = true;

        }
        else {
            this. DayMonthVisibility = false;
            this.DatePickerVisibility = true;
        }
    }

    private ChangeInActiveVisibility() {
        if (this.IsInActiveVisibility) {
            this.UIProperties.SetVisibility("Inactive", this.ObjectTableName, true);

        }
        else {
            this.UIProperties.SetVisibility("Inactive", this.ObjectTableName, false);
        }
    }

    get Day() {
        return this.entityPM.Day;
    }
    set Day(value: number) {
        if (this.entityPM.Day != value) {
            this.entityPM.Day = value;
            this.CreateHolidayDate();
        }
    }

    get Month() {
        return this.entityPM.Month;
    }
    set Month(value: number) {
        if (this.entityPM.Month != value) {
            this.entityPM.Month = value;
            this.CreateHolidayDate();
        }
    }


    get Year() {
        return this.entityPM.Year;
    }
    set Year(value: number) {
        if (this.entityPM.Year != value) {
            this.entityPM.Year = value;
            this.CreateHolidayDate();
        }
    }

    private CreateHolidayDate() {
        if (this.entityPM != null && this.entityPM.Day != null && this.entityPM.Year != null && this.entityPM.Month != null) {
            if (this.IsRecurring) {
                if (this.Day != 0 && this.Month != 0) {
                    this.HolidayDate = AppTool.PadLeft("" + this.Day, 2, '0') + "/" + AppTool.PadLeft("" + this.Month, 2, '0');
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
                    this.HolidayDate = AppTool.PadLeft("" + this.Day, 2, '0') + "/" + AppTool.PadLeft("" + this.Month, 2, '0') + "/" + AppTool.PadLeft("" + this.Year, 2, '0');
                }
            }
        }
    }

    public HolidayDate = null;

    private dayMonthVisibility = false;
    get DayMonthVisibility() { return this.dayMonthVisibility; }
    set DayMonthVisibility(value: boolean) {
        this.dayMonthVisibility = value;
    }

    private datePickerVisibility = false;
    get DatePickerVisibility() { return this.datePickerVisibility; }
    set DatePickerVisibility(value: boolean) {
        this.datePickerVisibility = value;
    }
}
